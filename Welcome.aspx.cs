// Welcome.aspx.cs
using System;

namespace ZimVaxSync
{
    public partial class Welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnCaregiver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Loginpage.aspx?role=Caregiver / Parent");
        }

        protected void btnHealthcare_Click(object sender, EventArgs e)
        {
            Response.Redirect("Loginpage.aspx?role=Healthcare Provider");
        }

        protected void btnOther_Click(object sender, EventArgs e)
        {
            Response.Redirect("Loginpage.aspx?role=Other General Users");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect to a landing or home page if available
            Response.Redirect("Home.aspx");
        }
    }
}
