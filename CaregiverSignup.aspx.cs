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
    public partial class CaregiverSignup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void btnOk_Click(object sender, EventArgs e)
        {
            string verificationMethod = rdoEmail.Checked ? "Email" : "SMS";
            string phone = txtPhoneNumber.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (verificationMethod == "Email" && !IsValidEmail(email))
            {
                lblMessage.Text = "Invalid email address format. Please enter a valid email.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Hash password
                    string hashedPassword = HashPassword(txtPassword.Text.Trim());

                    string insertCaregiverQuery = @"
                        INSERT INTO Caregiver 
                        (FullName, RelationshipToChild, PhoneNumber, Email, Address, Username, PasswordHash, SecurityQuestion, SecurityAnswer, PreferredVerification)
                        VALUES 
                        (@FullName, @RelationshipToChild, @PhoneNumber, @Email, @Address, @Username, @PasswordHash, @SecurityQuestion, @SecurityAnswer, @PreferredVerification);
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(insertCaregiverQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@RelationshipToChild", txtRelationship.Text.Trim());
                    cmd.Parameters.AddWithValue("@PhoneNumber", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    cmd.Parameters.AddWithValue("@SecurityQuestion", ddlSecurityQuestion.SelectedValue);
                    cmd.Parameters.AddWithValue("@SecurityAnswer", txtSecurityAnswer.Text.Trim());
                    cmd.Parameters.AddWithValue("@PreferredVerification", verificationMethod);

                    int caregiverId = Convert.ToInt32(cmd.ExecuteScalar());

                    string gender = rdoMale.Checked ? "Male" : "Female";
                    SqlCommand cmdChild = new SqlCommand(@"
                        INSERT INTO ChildProfile 
                        (CaregiverId, FullName, DateOfBirth, Gender, IDNumber)
                        VALUES 
                        (@CaregiverId, @FullName, @DateOfBirth, @Gender, @IDNumber);", conn, transaction);

                    cmdChild.Parameters.AddWithValue("@CaregiverId", caregiverId);
                    cmdChild.Parameters.AddWithValue("@FullName", txtChildName.Text.Trim());
                    cmdChild.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(txtChildDOB.Text));
                    cmdChild.Parameters.AddWithValue("@Gender", gender);
                    cmdChild.Parameters.AddWithValue("@IDNumber", txtChildID.Text.Trim());

                    cmdChild.ExecuteNonQuery();
                    transaction.Commit();

                    // Generate code and set sessions
                    string code = GenerateVerificationCode();
                    Session["VerificationCode"] = code;
                    Session["VerificationEmail"] = email;
                    Session["UserEmail"] = email;
                    Session["UserCategory"] = "Caregiver";

                    if (verificationMethod == "Email")
                    {
                        SendVerificationEmail(email, code);
                    }
                    else if (verificationMethod == "SMS")
                    {
                        var smsSender = new TelerivetSmsSender();
                        string message = $"Dear user, your ZimVaxSync verification code is: {code}";
                        await smsSender.SendSmsAsync(phone, message);
                    }

                    Response.Redirect("VerificationPage.aspx?email=" + Server.UrlEncode(email), false);
                    Context.ApplicationInstance.CompleteRequest();

                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction?.Rollback();
                    }
                    catch (Exception rollbackEx)
                    {
                        lblMessage.Text = "Rollback Error: " + rollbackEx.Message + " Original Error: " + ex.Message;
                        return;
                    }

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

        private void SendVerificationEmail(string toEmail, string code)
        {
            string fromEmail = ConfigurationManager.AppSettings["SMTPUser"];
            string subject = "ZimVaxSync Verification Code";
            string body = $"Dear user,\n\nYour verification code is: {code}\n\nRegards,\nZimVaxSync Team";

            MailMessage mail = new MailMessage(fromEmail, toEmail, subject, body);
            SmtpClient client = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["SMTPHost"],
                Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["SMTPUser"],
                    ConfigurationManager.AppSettings["SMTPPass"]
                )
            };

            client.Send(mail);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
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
