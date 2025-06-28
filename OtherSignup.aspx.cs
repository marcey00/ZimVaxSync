using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZimVaxSync
{
    public partial class OtherSignup : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void btnOk_Click(object senderObj, EventArgs e)
        {
            string gender = rdoMale.Checked ? "Male" : "Female";
            string verificationMethod = rdoEmail.Checked ? "Email" : "SMS";

            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (verificationMethod == "Email" && !IsValidEmail(email))
            {
                lblMessage.Text = "Invalid email address format. Please enter a valid email.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string hashedPassword = HashPassword(txtPassword.Text.Trim());

                    string insertQuery = @"
                        INSERT INTO OtherGeneralUser 
                        (FullName, DOB, Gender, NationalIDNumber, PhoneNumber, Email, Address, Username, PasswordHash, SecurityQuestion, SecurityAnswer, PreferredVerificationMethod)
                        VALUES 
                        (@FullName, @DOB, @Gender, @NationalIDNumber, @PhoneNumber, @Email, @Address, @Username, @PasswordHash, @SecurityQuestion, @SecurityAnswer, @PreferredVerificationMethod);";

                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@DOB", txtDOB.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@NationalIDNumber", txtNationalID.Text.Trim());
                    cmd.Parameters.AddWithValue("@PhoneNumber", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    cmd.Parameters.AddWithValue("@SecurityQuestion", ddlSecurityQuestion.SelectedValue);
                    cmd.Parameters.AddWithValue("@SecurityAnswer", txtSecurityAnswer.Text.Trim());
                    cmd.Parameters.AddWithValue("@PreferredVerificationMethod", verificationMethod);

                    cmd.ExecuteNonQuery();

                    // Generate verification code
                    string code = GenerateVerificationCode();
                    Session["VerificationCode"] = code;
                    Session["VerificationEmail"] = email;
                    Session["UserEmail"] = email;
                    Session["UserCategory"] = "Other";

                    if (verificationMethod == "Email")
                    {
                        EmailHelper.SendVerificationEmail(email, code);
                    }
                    else if (verificationMethod == "SMS")
                    {
                        var smsSender = new TelerivetSmsSender();
                        string message = $"Dear user, your ZimVaxSync verification code is: {code}";
                        await smsSender.SendSmsAsync(phone, message);
                    }

                    Response.Redirect("VerificationPage.aspx?email=" + Server.UrlEncode(email));
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Welcome.aspx");
        }

        private string GenerateVerificationCode()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }

        protected void cvAgreeTerms_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = chkAgreeTerms.Checked;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return email.Contains(".") && addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
