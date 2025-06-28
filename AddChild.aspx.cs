using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ZimVaxSync
{
    public partial class AddChild : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("CaregiverDashboard.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            string childName = txtChildName.Text.Trim();
            string dob = txtDOB.Text;
            string gender = rdoMale.Checked ? "Male" : rdoFemale.Checked ? "Female" : "";
            string idNumber = txtIDNumber.Text.Trim();
            string relationship = txtRelationship.Text.Trim();

            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username))
            {
                // Handle missing session
                Response.Redirect("Loginpage.aspx");
                return;
            }

            string guardianName = "";
            string address = "";
            int caregiverId = 0;

            string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            // Step 1: Retrieve GuardianName, Address, and CaregiverId from Caregiver table
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string caregiverQuery = "SELECT CaregiverId, FullName, Address FROM Caregiver WHERE Username = @CaregiverUsername";
                using (SqlCommand cmd = new SqlCommand(caregiverQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CaregiverUsername", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            caregiverId = Convert.ToInt32(reader["CaregiverId"]);
                            guardianName = reader["FullName"].ToString();
                            address = reader["Address"].ToString();
                        }
                        else
                        {
                            // Caregiver not found
                            Response.Write("Caregiver information not found.");
                            return;
                        }
                    }
                }

                // Step 2: Insert new child into ChildProfile table
                string insertQuery = @"INSERT INTO ChildProfile 
                    (FullName, DateOfBirth, Gender, IDNumber, RelationshipToChild, CaregiverId)
                    VALUES 
                    (@FullName, @DOB, @Gender, @IDNumber, @Relationship, @CaregiverId)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@FullName", childName);
                    insertCmd.Parameters.AddWithValue("@DOB", dob);
                    insertCmd.Parameters.AddWithValue("@Gender", gender);
                    insertCmd.Parameters.AddWithValue("@IDNumber", idNumber);
                    insertCmd.Parameters.AddWithValue("@Relationship", relationship);
                    insertCmd.Parameters.AddWithValue("@CaregiverId", caregiverId);

                    insertCmd.ExecuteNonQuery();
                }
            }

            // Redirect to dashboard after successful insert
            Response.Redirect("CaregiverDashboard.aspx");
        }
    }
}

