<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Support.aspx.cs" Inherits="ZimVaxSync.Support" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ZimVaxSync Support</title>
    <style>
        body { font-family: Arial; padding: 20px;  background-image: url('background.png'); }
        .container { max-width: 900px; margin: auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px #ccc; }
        h2 { color: #007bff; margin-top: 0; }

        .faq-section { margin-bottom: 40px; }
        .faq-item { margin-bottom: 15px; }
        .faq-question { font-weight: bold; }

        .chat-section { border-top: 1px solid #ccc; padding-top: 20px; }
        #chatBox { height: 250px; overflow-y: scroll; border: 1px solid #ddd; padding: 10px; background: #f7f7f7; margin-bottom: 10px; }
        .user-msg { color: #333; margin-bottom: 5px; }
        .bot-msg { color: #007bff; margin-bottom: 10px; }

        #txtMessage { width: 80%; padding: 8px; }
        #btnSend { padding: 8px 16px; background-color: #007bff; color: white; border: none; cursor: pointer; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Support Center</h2>

            <div class="faq-section">
                <h3>Frequently Asked Questions</h3>

                <div class="faq-item">
                    <div class="faq-question">Q: How do I add my child’s vaccination record?</div>
                    <div class="faq-answer">A: Login as a caregiver, go to the dashboard, then click "Add Child".</div>
                </div>

                <div class="faq-item">
                    <div class="faq-question">Q: Can I use the platform without mobile data?</div>
                    <div class="faq-answer">A: No, an internet connection is required for login and syncing vaccination data.</div>
                </div>

                <div class="faq-item">
                    <div class="faq-question">Q: I forgot my password. What do I do?</div>
                    <div class="faq-answer">A: Use the “Forgot Password” link on the login page to reset it using your email or security question.</div>
                </div>
            </div>

            <div class="chat-section">
                <h3>Live Chat with Support A.I.</h3>
                <asp:Literal ID="litChatBox" runat="server" />

                <asp:TextBox ID="txtMessage" runat="server" placeholder="Type your question..." />
                <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" />
            </div>
        </div>
    </form>
</body>
</html>
