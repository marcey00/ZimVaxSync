using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace ZimVaxSync
{
    public partial class VerificationPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["VerificationCode"] == null || Session["VerificationEmail"] == null)
                {
                    lblResult.Text = "Session expired. Please sign up again.";
                    lblResult.CssClass = "error";
                    btnVerify.Enabled = false;
                    btnResendCode.Enabled = false;
                    return;
                }

                string method = Session["VerificationMethod"]?.ToString() ?? "Email";
                lblInstruction.Text = $"Please enter the 6-digit code sent to your {(method == "SMS" ? "SMS" : "email")}.";

                if (IsUserAlreadyVerified(Session["VerificationEmail"].ToString()))
                {
                    lblResult.Text = "You are already verified. Redirecting to dashboard...";
                    lblResult.CssClass = "success";
                    Response.AddHeader("REFRESH", "3;URL=Welcome.aspx");
                }
            }
        }


        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string inputCode = txtVerificationCode.Text.Trim();
            string sessionCode = Session["VerificationCode"]?.ToString();
            string email = Session["VerificationEmail"]?.ToString();
            string category = Session["UserCategory"]?.ToString();

            if (string.IsNullOrEmpty(inputCode))
            {
                lblResult.Text = "Please enter the code.";
                lblResult.CssClass = "error";
                return;
            }

            if (IsUserAlreadyVerified(email))
            {
                lblResult.Text = "You are already verified.";
                lblResult.CssClass = "success";
                return;
            }

            if (inputCode == sessionCode)
            {
                MarkUserAsVerified(email);
                lblResult.Text = "Verification successful!";
                lblResult.CssClass = "success";

                // Redirect
                if (category == "Caregiver")
                    Response.Redirect("CaregiverDashboard.aspx");
                else if (category == "Healthcare")
                    Response.Redirect("HealthcareDashboard.aspx");
                else
                    Response.Redirect("OtherDashboard.aspx");
            }
            else
            {
                lblResult.Text = "Incorrect verification code.";
                lblResult.CssClass = "error";
            }
        }

        protected void btnResendCode_Click(object sender, EventArgs e)
        {
            string email = Session["VerificationEmail"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                lblResult.Text = "Session expired. Cannot resend code.";
                lblResult.CssClass = "error";
                return;
            }

            DateTime lastSentTime = Session["LastResendTime"] != null
                ? (DateTime)Session["LastResendTime"]
                : DateTime.MinValue;

            int cooldownSeconds = 120;
            double secondsSinceLast = (DateTime.Now - lastSentTime).TotalSeconds;

            if (secondsSinceLast < cooldownSeconds)
            {
                int waitSeconds = cooldownSeconds - (int)secondsSinceLast;
                Session["CooldownRemaining"] = waitSeconds;  // passed to JavaScript
                lblResult.Text = $"Please wait {waitSeconds} seconds before resending.";
                lblResult.CssClass = "error";
                return;
            }

            // Send new code
            string newCode = new Random().Next(100000, 999999).ToString();
            Session["VerificationCode"] = newCode;
            Session["LastResendTime"] = DateTime.Now;
            Session["CooldownRemaining"] = cooldownSeconds;

            EmailHelper.SendVerificationEmail(email, newCode);

            lblResult.Text = "A new verification code has been sent.";
            lblResult.CssClass = "success";
        }

        private void MarkUserAsVerified(string email)
        {
            string constr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string[] tables = { "Caregiver", "HealthcareProvider", "OtherGeneralUser" };
                foreach (string table in tables)
                {
                    string query = $"UPDATE {table} SET IsVerified = 1 WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private bool IsUserAlreadyVerified(string email)
        {
            string constr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string[] tables = { "Caregiver", "HealthcareProvider", "OtherGeneralUser" };

                foreach (string table in tables)
                {
                    string query = $"SELECT IsVerified FROM {table} WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value && Convert.ToBoolean(result) == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
