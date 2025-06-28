using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace ZimVaxSync
{
    public partial class CaregiverSettings : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
                Response.Redirect("Loginpage.aspx");

            if (!IsPostBack)
            {
                LoadNotificationPreferences();
                LoadAdultChildren();
            }
        }

        protected void LoadNotificationPreferences()
        {
            string username = Session["Username"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT NotifyEmail, NotifySMS, PreferredLanguage FROM Caregiver WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    chkEmail.Checked = Convert.ToBoolean(reader["NotifyEmail"]);
                    chkSMS.Checked = Convert.ToBoolean(reader["NotifySMS"]);
                    ddlLanguage.SelectedValue = reader["PreferredLanguage"].ToString();
                }
            }
        }

        protected void LoadAdultChildren()
        {
            string username = Session["Username"].ToString();
            ddlAdultChildren.Items.Clear();
            ddlAdultChildren.Items.Add("Select...");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT c.FullName, c.IDNumber 
                    FROM ChildProfile c 
                    JOIN Caregiver cg ON c.CaregiverID = cg.CaregiverID 
                    WHERE cg.Username = @Username AND DATEDIFF(YEAR, c.DateOfBirth, GETDATE()) >= 18";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ddlAdultChildren.Items.Add(new System.Web.UI.WebControls.ListItem(
                        reader["FullName"].ToString(), reader["IDNumber"].ToString()));
                }
            }
        }

        protected void btnSavePreferences_Click(object sender, EventArgs e)
        {
            string username = Session["Username"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Caregiver SET NotifyEmail = @Email, NotifySMS = @SMS, PreferredLanguage = @Lang WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", chkEmail.Checked);
                cmd.Parameters.AddWithValue("@SMS", chkSMS.Checked);
                cmd.Parameters.AddWithValue("@Lang", ddlLanguage.SelectedValue);
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Preferences saved.');", true);
        }

        protected void ddlAdultChildren_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAdultChildren.SelectedIndex <= 0)
                return;

            string selectedChildId = ddlAdultChildren.SelectedValue;
            string newUsername = selectedChildId + "_user"; // For demo purposes
            string defaultPassword = "Temp123"; // Use hashing in production

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Get child data
                string getChildQuery = "SELECT FullName, IDNumber FROM ChildProfile WHERE IDNumber = @ID";
                SqlCommand getCmd = new SqlCommand(getChildQuery, conn);
                getCmd.Parameters.AddWithValue("@ID", selectedChildId);
                SqlDataReader reader = getCmd.ExecuteReader();

                if (reader.Read())
                {
                    string fullName = reader["FullName"].ToString();
                    string idNumber = reader["IDNumber"].ToString();
                    reader.Close();

                    // Insert into Caregiver as new user
                    string insertQuery = @"INSERT INTO Caregiver (FullName, NationalID, Username, Password)
                                           VALUES (@FullName, @NationalID, @Username, @Password)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@FullName", fullName);
                    insertCmd.Parameters.AddWithValue("@NationalID", idNumber);
                    insertCmd.Parameters.AddWithValue("@Username", newUsername);
                    insertCmd.Parameters.AddWithValue("@Password", defaultPassword);
                    insertCmd.ExecuteNonQuery();

                    ClientScript.RegisterStartupScript(GetType(), "alert", $"alert('New caregiver account created for {fullName}. Username: {newUsername}');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Child not found.');", true);
                }
            }
        }

        protected void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string username = Session["Username"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Caregiver WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Session.Clear();
            Response.Redirect("Loginpage.aspx");
        }
    }
}
