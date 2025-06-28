using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace ZimVaxSync
{
    public partial class CaregiverProfile : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;
        private string ComputeSHA256Hash(string rawData)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower(); // e.g., "a1b2c3..."
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CaregiverId"] != null)
                {
                    LoadCaregiverProfile();
                }
                else
                {
                    // Redirect to login if session expired or missing
                    Response.Redirect("Loginpage.aspx");
                }
            }
        }

        private void LoadCaregiverProfile()
        {
            string caregiverId = Session["CaregiverId"]?.ToString();
            if (string.IsNullOrEmpty(caregiverId)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FullName, Email, PhoneNumber, Address FROM Caregiver WHERE CaregiverID = @CaregiverId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CaregiverId", caregiverId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtFullName.Text = reader["FullName"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtPhone.Text = reader["PhoneNumber"].ToString();
                    txtAddress.Text = reader["Address"].ToString();
                }
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            string caregiverId = Session["CaregiverId"]?.ToString();
            if (string.IsNullOrEmpty(caregiverId)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE Caregiver SET FullName=@FullName, Email=@Email, PhoneNumber=@PhoneNumber, Address=@Address WHERE CaregiverID=@CaregiverId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@CaregiverId", caregiverId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string caregiverId = Session["CaregiverId"]?.ToString();
            if (string.IsNullOrEmpty(caregiverId)) return;

            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                // Optionally notify user that password fields are required
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand checkCmd = new SqlCommand("SELECT Password FROM Caregiver WHERE CaregiverID = @CaregiverId", conn);
                checkCmd.Parameters.AddWithValue("@CaregiverId", caregiverId);
                object result = checkCmd.ExecuteScalar();

                if (result != null)
                {
                    string storedPassword = result.ToString();
                    string hashedOld = ComputeSHA256Hash(oldPassword);

                    if (storedPassword == hashedOld)
                    {
                        string hashedNew = ComputeSHA256Hash(newPassword);
                        SqlCommand updateCmd = new SqlCommand("UPDATE Caregiver SET Password = @NewPassword WHERE CaregiverID = @CaregiverId", conn);
                        updateCmd.Parameters.AddWithValue("@NewPassword", hashedNew);
                        updateCmd.Parameters.AddWithValue("@CaregiverId", caregiverId);
                        updateCmd.ExecuteNonQuery();

                        // Optionally notify user that password was changed successfully
                    }
                    else
                    {
                        // Optionally notify user: "Old password is incorrect"
                    }
                }
                else
                {
                    // Optionally notify user: "Caregiver record not found"
                }
            }
        }


        protected void btnUpdateSecurity_Click(object sender, EventArgs e)
        {
            string caregiverId = Session["CaregiverId"]?.ToString();
            if (string.IsNullOrEmpty(caregiverId)) return;

            string question = ddlSecurityQuestion.SelectedValue;
            string answer = txtSecurityAnswer.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE Caregiver SET SecurityQuestion=@SecurityQuestion, SecurityAnswer=@SecurityAnswer WHERE CaregiverID=@CaregiverId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SecurityQuestion", question);
                cmd.Parameters.AddWithValue("@SecurityAnswer", answer);
                cmd.Parameters.AddWithValue("@CaregiverId", caregiverId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

