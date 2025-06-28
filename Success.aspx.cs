using System;
using System.Web.UI;

namespace ZimVaxSync
{
    public partial class Success : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            Response.Redirect("Loginpage.aspx");
        }
    }
}
