using System;

namespace ZimVaxSync
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string role = Session["Role"] as string;
                hfUserRole.Value = role ?? "";
            }
        }
    }
}

