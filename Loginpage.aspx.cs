

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ZimVaxSync
{
    public partial class Loginpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUsername.Text = "";
                txtPassword.Attributes["value"] = "";

                string userRole = Request.QueryString["role"] ?? Session["UserRole"] as string;

                if (!string.IsNullOrEmpty(userRole))
                {
                    Session["UserRole"] = userRole;
                    lblSelectedRole.Text = "Logging in as: " + userRole;
                }
                else
                {
                    Response.Redirect("Welcome.aspx");
                    return;
                }

                HttpCookie cookie = Request.Cookies["ZimVaxSyncCreds"];
                if (cookie != null)
                {
                    txtUsername.Text = cookie["Username"];
                    txtPassword.Attributes["value"] = cookie["Password"];
                    chkRememberMe.Checked = true;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string hashedPassword = HashPassword(password);
            string role = "";
            lblStatus.Text = "";

            string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check Caregiver
                string queryCG = "SELECT CaregiverId, FullName, Email FROM Caregiver WHERE Username = @Username AND PasswordHash = @PasswordHash";
                using (SqlCommand cmd = new SqlCommand(queryCG, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            role = "Caregiver";
                            Session["CaregiverId"] = reader["CaregiverId"].ToString();
                            Session["FullName"] = reader["FullName"].ToString();
                            Session["Email"] = reader["Email"].ToString();
                        }
                    }
                }

                // Check Healthcare Provider
                if (role == "")
                {
                    string queryHP = "SELECT HealthcareProviderId, FullName, Email, Profession, Institution FROM HealthcareProvider WHERE Username = @Username AND PasswordHash = @PasswordHash";
                    using (SqlCommand cmd = new SqlCommand(queryHP, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                role = "HealthcareProvider";
                                Session["HealthcareProviderId"] = reader["HealthcareProviderId"].ToString();
                                Session["FullName"] = reader["FullName"].ToString();
                                Session["Email"] = reader["Email"].ToString();
                                Session["Profession"] = reader["Profession"].ToString();
                                Session["Institution"] = reader["Institution"].ToString();
                            }
                        }
                    }
                }

                // Check Other User
                if (role == "")
                {
                    string queryOther = "SELECT OtherUserId, FullName, Email FROM OtherGeneralUser WHERE Username = @Username AND PasswordHash = @PasswordHash";
                    using (SqlCommand cmd = new SqlCommand(queryOther, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                role = "Other";
                                Session["OtherUserId"] = reader["OtherUserId"].ToString();
                                Session["FullName"] = reader["FullName"].ToString();
                                Session["Email"] = reader["Email"].ToString();
                            }
                        }
                    }
                }

                // Validate and redirect
                string selectedRole = Session["UserRole"] as string;

                if (!string.IsNullOrEmpty(role))
                {
                    if (selectedRole == "Caregiver / Parent" && role != "Caregiver" ||
                        selectedRole == "Healthcare Provider" && role != "HealthcareProvider" ||
                        selectedRole == "Other General Users" && role != "Other")
                    {
                        lblStatus.Text = $"This account does not belong to a {selectedRole}.";
                        return;
                    }

                    if (chkRememberMe.Checked)
                    {
                        HttpCookie cookie = new HttpCookie("ZimVaxSyncCreds");
                        cookie["Username"] = username;
                        cookie["Password"] = password;
                        cookie.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        if (Request.Cookies["ZimVaxSyncCreds"] != null)
                        {
                            HttpCookie expired = new HttpCookie("ZimVaxSyncCreds");
                            expired.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(expired);
                        }
                    }

                    Session["Username"] = username;
                    Session["Role"] = role;

                    if (role == "Caregiver")
                        Response.Redirect("CaregiverDashboard.aspx", false);
                    else if (role == "HealthcareProvider")
                        Response.Redirect("HealthcareDashboard.aspx", false);
                    else if (role == "Other")
                        Response.Redirect("OtherDashboard.aspx", false);

                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    lblStatus.Text = "Invalid credentials or account does not exist.";
                }
            }
        }

        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            string selectedRole = Session["UserRole"] as string;

            if (selectedRole == "Caregiver / Parent")
                Response.Redirect("CaregiverSignup.aspx");
            else if (selectedRole == "Healthcare Provider")
                Response.Redirect("HealthcareSignup.aspx");
            else if (selectedRole == "Other General Users")
                Response.Redirect("OtherSignup.aspx");
            else
                Response.Redirect("Welcome.aspx");
        }

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Welcome.aspx");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashed).Replace("-", "").ToLower();
            }
        }
    }
}

