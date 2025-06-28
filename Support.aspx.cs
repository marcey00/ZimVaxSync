using System;
using System.Web.UI;

namespace ZimVaxSync
{
    public partial class Support : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ChatHistory"] = "";
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string userMessage = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(userMessage)) return;

            string previousChat = Session["ChatHistory"]?.ToString() ?? "";
            string botReply = GetBotReply(userMessage);

            string newChat = $"{previousChat}<div class='user-msg'><strong>You:</strong> {userMessage}</div>" +
                                            $"<div class='bot-msg'><strong>ZimBot:</strong> {botReply}</div>";

            Session["ChatHistory"] = newChat;
            litChatBox.Text = $"<div id='chatBox'>{newChat}</div>";

            txtMessage.Text = "";
        }

        private string GetBotReply(string message)
        {
            // Basic mock reply logic
            if (message.ToLower().Contains("add child"))
                return "To add a child, log in as a caregiver and use the 'Add Child' button.";
            else if (message.ToLower().Contains("vaccine") || message.ToLower().Contains("immunisation"))
                return "You can view the full vaccination schedule under 'Vaccination Info' in your dashboard.";
            else if (message.ToLower().Contains("login"))
                return "Make sure you are using the correct email and password. Try resetting your password if needed.";
            else
                return "I'm here to help! Please ask about using the platform, login issues, or vaccination info.";
        }
    }
}
