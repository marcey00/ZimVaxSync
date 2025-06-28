using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace ZimVaxSync
{
    public partial class AddAdult : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string gender = rdoMale.Checked ? "Male" : "Female";
            string email = txtEmail.Text.Trim();

            if (!IsValidEmail(email))
            {
                lblMessage.Text = "Invalid email format.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string insertQuery = @"
                        INSERT INTO OtherGeneralUser 
                        (FullName, DOB, Gender, NationalIDNumber, PhoneNumber, Email, Address, SecurityQuestion, SecurityAnswer)
                        VALUES 
                        (@FullName, @DOB, @Gender, @NationalIDNumber, @PhoneNumber, @Email, @Address, @SecurityQuestion, @SecurityAnswer);";

                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@DOB", txtDOB.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@NationalIDNumber", txtNationalID.Text.Trim());
                    cmd.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@SecurityQuestion", ddlSecurityQuestion.SelectedValue);
                    cmd.Parameters.AddWithValue("@SecurityAnswer", txtSecurityAnswer.Text.Trim());

                    cmd.ExecuteNonQuery();

                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Adult profile added successfully.";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("HealthcareDashboard.aspx"); // or whichever page is appropriate
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
