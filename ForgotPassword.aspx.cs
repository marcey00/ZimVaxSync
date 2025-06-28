using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ZimVaxSync
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "";
        }

        protected void txtUsername_TextChanged(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            lblSecurityQuestion.Text = "";

            if (string.IsNullOrEmpty(username))
            {
                lblStatus.Text = "Please enter a username.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string[] tables = { "Caregiver", "HealthcareProvider", "OtherGeneralUser" };
                foreach (string table in tables)
                {
                    string query = $"SELECT SecurityQuestion FROM {table} WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            lblSecurityQuestion.Text = result.ToString();
                            return;
                        }
                    }
                }

                lblStatus.Text = "Username not found.";
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string securityAnswer = txtSecurityAnswer.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(securityAnswer))
            {
                lblStatus.Text = "All fields are required.";
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblStatus.Text = "Passwords do not match.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;
            string hashedPassword = HashPassword(newPassword);
            bool updated = false;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string[] tables = { "Caregiver", "HealthcareProvider", "OtherGeneralUser" };
                foreach (string table in tables)
                {
                    string checkAnswerQuery = $"SELECT SecurityAnswer FROM {table} WHERE Username = @Username";
                    using (SqlCommand checkCmd = new SqlCommand(checkAnswerQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", username);
                        object storedAnswer = checkCmd.ExecuteScalar();

                        if (storedAnswer != null && storedAnswer.ToString().Equals(securityAnswer, StringComparison.OrdinalIgnoreCase))
                        {
                            string updateQuery = $"UPDATE {table} SET PasswordHash = @PasswordHash WHERE Username = @Username";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                                updateCmd.Parameters.AddWithValue("@Username", username);
                                int rowsAffected = updateCmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    updated = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (updated)
            {
                Response.Redirect("Loginpage.aspx?reset=success");
            }
            else
            {
                lblStatus.Text = "Invalid security answer or username.";
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}

