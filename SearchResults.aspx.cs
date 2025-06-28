using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZimVaxSync
{
    public partial class SearchResults : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string keyword = Request.QueryString["query"];

                if (!string.IsNullOrEmpty(keyword))
                {
                    lblKeyword.Text = $"You searched for: <i>{Server.HtmlEncode(keyword)}</i>";
                    SearchDatabase(keyword);
                }
                else
                {
                    lblKeyword.Text = "No search keyword provided.";
                }
            }
        }

        private void SearchDatabase(string keyword)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
    SELECT FullName, IDNumber AS Identifier, DateOfBirth, 'Child' AS UserType, ChildId AS UniqueId      
    FROM ChildProfile
    WHERE FullName LIKE @Keyword OR IDNumber LIKE @Keyword

    UNION

    SELECT FullName, NationalIDNumber AS Identifier, DOB, 'Adult' AS UserType, OtherUserID AS UniqueId
    FROM OtherGeneralUser
    WHERE FullName LIKE @Keyword OR NationalIDNumber LIKE @Keyword
";


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                conn.Open();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    gvResults.DataSource = dt;
                    gvResults.DataBind();
                    lblNoResults.Visible = false;
                }
                else
                {
                    lblNoResults.Text = "No matching records found.";
                    lblNoResults.Visible = true;
                }
            }
        }

        protected void gvResults_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddRecord")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvResults.Rows[index];

                string fullName = row.Cells[0].Text;
                string dob = row.Cells[1].Text;
                string idNumber = row.Cells[2].Text;
                string userType = row.Cells[3].Text;

                Session["SelectedName"] = fullName;
                Session["SelectedDOB"] = dob;
                Session["SelectedID"] = idNumber;
                Session["SelectedUserType"] = userType;

                // Optional: store childId in case of child
                if (userType == "Child")
                {
                    // This assumes you added the childId as a DataKey
                    Session["ChildId"] = gvResults.DataKeys[index]["UniqueId"];
                }

                // Show the modal (you can trigger it using JS or redirect to the modal page)
                Response.Redirect("HealthcareDashboard.aspx?showModal=1");
            }
        }
    }
}

