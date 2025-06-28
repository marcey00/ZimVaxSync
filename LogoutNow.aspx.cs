using System;

namespace ZimVaxSync
{
    public partial class LogoutNow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Capture role before clearing session
            string role = Session["Role"] as string;

            // Clear session and prevent caching
            Session.Clear();
            Session.Abandon();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            // Redirect with role to login page
            string redirectUrl = "Loginpage.aspx?msg=logout";
            if (!string.IsNullOrEmpty(role))
            {
                redirectUrl += $"&role={role}";
            }

            Response.Redirect(redirectUrl);
        }
    }
}
