using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;

namespace ZimVaxSync
{
    public partial class HealthcareDashboard : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadHealthcareProviderInfo();

                // Show modal if redirected back from search
                if (Request.QueryString["showModal"] == "1")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "showAddRecordModal();", true);
                }
            }
        }

        private void LoadHealthcareProviderInfo()
        {
            if (Session["HealthcareProviderId"] != null)
            {
                string healthcareId = Session["HealthcareProviderId"].ToString();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT FullName, Profession, Institution FROM HealthcareProvider WHERE HealthcareProviderId = @HealthcareProviderId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@HealthcareProviderId", healthcareId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblHealthcareName.Text = reader["FullName"].ToString();
                        lblFullName.Text = reader["FullName"].ToString();
                        lblProfession.Text = reader["Profession"].ToString();
                        lblInstitution.Text = reader["Institution"].ToString();
                    }

                    reader.Close();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // 1. Search ChildProfile (by IDNumber or FullName)
                    string childQuery = "SELECT ChildId, FullName, DateOfBirth, IDNumber FROM ChildProfile WHERE IDNumber = @Search OR FullName LIKE '%' + @Search + '%'";
                    SqlCommand childCmd = new SqlCommand(childQuery, conn);
                    childCmd.Parameters.AddWithValue("@Search", searchQuery);

                    using (SqlDataReader reader = childCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblName.Text = reader["FullName"].ToString();
                            lblDOB.Text = Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd MMM yyyy");
                            lblIDNumber.Text = reader["IDNumber"].ToString();

                            hfUserType.Value = "Child";
                            hfUserId.Value = reader["ChildId"].ToString();

                            pnlResult.Visible = true;
                            lblMessage.Text = "";
                            return;
                        }
                    }

                    // 2. Search OtherGeneralUser (by NationalIDNumber or FullName)
                    string userQuery = "SELECT FullName, DOB, NationalIDNumber FROM OtherGeneralUser WHERE NationalIDNumber = @Search OR FullName LIKE '%' + @Search + '%'";
                    SqlCommand userCmd = new SqlCommand(userQuery, conn);
                    userCmd.Parameters.AddWithValue("@Search", searchQuery);

                    using (SqlDataReader reader = userCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblName.Text = reader["FullName"].ToString();
                            lblDOB.Text = Convert.ToDateTime(reader["DOB"]).ToString("dd MMM yyyy");
                            lblIDNumber.Text = reader["NationalIDNumber"].ToString();

                            hfUserType.Value = "GeneralUser";
                            hfUserId.Value = reader["NationalIDNumber"].ToString();

                            pnlResult.Visible = true;
                            lblMessage.Text = "";
                            return;
                        }
                    }

                    // No match found
                    pnlResult.Visible = false;
                    lblMessage.Text = "No record found for the given search term.";
                }
            }
            else
            {
                pnlResult.Visible = false;
                lblMessage.Text = "Please enter a valid search term.";
            }
        }

        protected void btnAddChildVaccination_Click(object sender, EventArgs e)
        {
            if (hfUserType.Value == "Child" && int.TryParse(hfUserId.Value, out int childId))
            {
                Response.Redirect($"AddChildhoodRecord.aspx?childId={childId}");
            }
            else
            {
                lblMessage.Text = "Child ID not found. Please search and select a child record.";
            }
        }

        protected void btnAddOtherVaccinationsRecord_Click(object sender, EventArgs e)
        {
            string userType = hfUserType.Value;
            string userId = hfUserId.Value;

            if (userType == "Child" && int.TryParse(userId, out int childId))
            {
                Response.Redirect($"AddOtherVaccinationsRecord.aspx?childId={childId}");
            }
            else if (userType == "GeneralUser")
            {
                Response.Redirect($"AddOtherVaccinationsRecord.aspx?userId={userId}");
            }
            else
            {
                lblMessage.Text = "Invalid user selection. Please search and select a valid user.";
            }
        }

        protected void btnAddChild_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddNewChild.aspx");
        }

        protected void btnViewChildhoodRecord_Click(object sender, EventArgs e)
        {
            string userType = hfUserType.Value;
            string userId = hfUserId.Value;

            if (string.IsNullOrEmpty(userType) || string.IsNullOrEmpty(userId))
            {
                lblMessage.Text = "Please search and select a valid user before viewing records.";
                return;
            }

            if (userType == "Child" && int.TryParse(userId, out int childId))
            {
                Session["ChildId"] = childId;
                Session["OtherUserId"] = null;
                Response.Redirect("ViewChildhoodRecord.aspx?childId=" + childId);
            }
            else
            {
                lblMessage.Text = "Childhood records can only be viewed for child users.";
            }
        }

        protected void btnViewOtherRecord_Click(object sender, EventArgs e)
        {
            string userType = hfUserType.Value;
            string userId = hfUserId.Value;

            if (string.IsNullOrEmpty(userType) || string.IsNullOrEmpty(userId))
            {
                lblMessage.Text = "Please search and select a valid user before viewing records.";
                return;
            }

            if (userType == "Child" && int.TryParse(userId, out int childId))
            {
                Session["ChildId"] = childId;
                Session["OtherUserId"] = null;
                Response.Redirect("ViewOtherRecord.aspx?childId=" + childId);
            }
            else if (userType == "GeneralUser")
            {
                Session["OtherUserId"] = userId;
                Session["ChildId"] = null;
                Response.Redirect("ViewOtherRecord.aspx?userId=" + userId);
            }
            else
            {
                lblMessage.Text = "Invalid user type selected.";
            }
        }
    }
}
