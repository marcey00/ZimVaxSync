using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ZimVaxSync
{
    public partial class OtherDashboard : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["OtherUserID"] == null)
                {
                    Response.Redirect("Loginpage.aspx");
                    return;
                }

                lblFullName.Text = Session["FullName"]?.ToString() ?? "Unknown User";

                VaccinationSchedule();
                
            }
        }

        private void VaccinationSchedule()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT VaccineName, DueDate FROM VaccinationSchedule WHERE OtherUserID = @OtherUserID", conn))
            {
                cmd.Parameters.AddWithValue("@OtherUserID", Session["OtherUserID"]);

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                rptSchedule.DataSource = dt;
                rptSchedule.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                Response.Redirect($"SearchResults.aspx?query={HttpUtility.UrlEncode(keyword)}");
            }
        }
    }
}
