using System;
using System.Data.SqlClient;
using System.Configuration;

namespace ZimVaxSync
{
    public partial class CaregiverDashboard : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadChildProfiles();

                // Load default child from session if available
                if (Session["ChildID"] != null && int.TryParse(Session["ChildID"].ToString(), out int sessionChildId))
                {
                    ddlAccounts.SelectedValue = sessionChildId.ToString();
                    LoadChildDetails(sessionChildId);
                    LoadVaccinationSchedule(sessionChildId);
                }
                else if (ddlAccounts.Items.Count > 0)
                {
                    int selectedId = int.Parse(ddlAccounts.SelectedValue);
                    Session["ChildID"] = selectedId;
                    LoadChildDetails(selectedId);
                    LoadVaccinationSchedule(selectedId);
                }
            }
        }

        protected void ddlAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlAccounts.SelectedValue, out int selectedId))
            {
                Session["ChildID"] = selectedId;
                LoadChildDetails(selectedId);
                LoadVaccinationSchedule(selectedId);
            }
        }

        private void LoadChildProfiles()
        {
            if (Session["CaregiverId"] == null)
                return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT ChildId, FullName FROM ChildProfile WHERE CaregiverId = @CaregiverId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CaregiverId", Session["CaregiverId"]);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlAccounts.DataSource = reader;
                ddlAccounts.DataTextField = "FullName";
                ddlAccounts.DataValueField = "ChildId";
                ddlAccounts.DataBind();
            }
        }

        private void LoadChildDetails(int childId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FullName, DateOfBirth, Gender FROM ChildProfile WHERE ChildId = @ChildId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ChildId", childId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblFullName.Text = reader["FullName"].ToString();

                    DateTime dob = Convert.ToDateTime(reader["DateOfBirth"]);
                    lblAge.Text = CalculateAge(dob).ToString();
                    lblGender.Text = reader["Gender"].ToString();
                }
                else
                {
                    lblFullName.Text = "N/A";
                    lblAge.Text = "-";
                    lblGender.Text = "-";
                }
            }
        }

        private void LoadVaccinationSchedule(int childId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT VaccineName, DueDate 
                    FROM VaccinationSchedule 
                    WHERE ChildId = @ChildId 
                    ORDER BY DueDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ChildId", childId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                rptSchedule.DataSource = reader;
                rptSchedule.DataBind();
            }
        }

        private int CalculateAge(DateTime dob)
        {
            int age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now.DayOfYear < dob.DayOfYear)
                age--;
            return age;
        }
    }
}
