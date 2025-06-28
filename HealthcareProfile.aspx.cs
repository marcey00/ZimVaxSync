using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Twilio.TwiML.Voice;

namespace ZimVaxSync
{
    public partial class HealthcareProfile : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private void LoadProfile()
        {
            string providerId = Session["HealthcareProviderId"]?.ToString();
            if (string.IsNullOrEmpty(providerId)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT FullName, Profession, Institution, Email FROM HealthcareProvider WHERE HealthcareProviderID = @HealthcareProviderID", conn);
                cmd.Parameters.AddWithValue("@HealthcareProviderID", providerId); // ✅ Correct parameter name
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtFullName.Text = reader["FullName"].ToString();
                    txtProfession.Text = reader["Profession"].ToString();
                    txtInstitution.Text = reader["Institution"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                }
            }
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string providerId = Session["HealthcareProviderId"]?.ToString();
            if (string.IsNullOrEmpty(providerId)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE HealthcareProvider SET FullName = @FullName, Profession = @Profession, Institution = @Institution, Address = @Address, Email = @Email WHERE ProviderID = @ProviderID", conn);
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@Profession", txtProfession.Text.Trim());
                cmd.Parameters.AddWithValue("@Institution", txtInstitution.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@ProviderID", providerId);
                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Profile updated successfully.";
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string providerId = Session["HealthcareProviderId"]?.ToString();
            if (string.IsNullOrEmpty(providerId)) return;

            string oldPwd = txtOldPassword.Text.Trim();
            string newPwd = txtNewPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {

                conn.Open();
                SqlCommand getPwd = new SqlCommand("SELECT Password FROM HealthcareProvider WHERE ProviderID = @ProviderID", conn);
                getPwd.Parameters.AddWithValue("@ProviderID", providerId);
                object result = getPwd.ExecuteScalar();

                if (result != null && result.ToString() == HashPassword(oldPwd))
                {
                    SqlCommand updatePwd = new SqlCommand("UPDATE HealthcareProvider SET Password = @Password WHERE ProviderID = @ProviderID", conn);
                    updatePwd.Parameters.AddWithValue("@Password", HashPassword(newPwd));
                    updatePwd.Parameters.AddWithValue("@ProviderID", providerId);
                    updatePwd.ExecuteNonQuery();
                    lblMessage.Text = "Password changed successfully.";
                }
                else
                {
                    lblMessage.Text = "Incorrect old password.";
                }
            }
        }

        protected void btnUpdateSecurity_Click(object sender, EventArgs e)
        {
            string providerId = Session["HealthcareProviderId"]?.ToString();
            if (string.IsNullOrEmpty(providerId)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE HealthcareProvider SET SecurityQuestion = @Question, SecurityAnswer = @Answer WHERE ProviderID = @ProviderID", conn);
                cmd.Parameters.AddWithValue("@Question", ddlSecurityQuestion.SelectedValue);
                cmd.Parameters.AddWithValue("@Answer", txtSecurityAnswer.Text.Trim());
                cmd.Parameters.AddWithValue("@ProviderID", providerId);
                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Security question updated.";
            }
        }
    }
}

