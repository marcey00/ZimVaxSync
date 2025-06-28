<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="ZimVaxSync.ForgotPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password - ZimVaxSync</title>
    <style>
        body, html {
             background-image: url('background.png');
            font-family: Arial, sans-serif;
            height: 100%;
            margin: 0;
            padding: 0;
        }

        .container {
            width: 800px;
            margin: 60px auto;
            padding: 30px;
            background: white;
            border-radius: 10px;
            box-shadow: 0 0 15px rgba(0,0,0,0.2);
        }

        .container h2 {
            text-align: center;
            margin-bottom: 25px;
        }

        .form-control {
            width: 100%;
            padding: 12px;
            margin-bottom: 15px;
            font-size: 14px;
        }

        .btn {
            width: 100%;
            padding: 12px;
            font-size: 15px;
            background-color: #007bff;
            color: white;
            border: none;
            cursor: pointer;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        .message-label {
            color: red;
            text-align: center;
            margin-top: 10px;
        }

        .status-label {
            color: green;
            text-align: center;
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Forgot Password</h2>

            <asp:Label ID="lblStatus" runat="server" CssClass="status-label" />

            <asp:TextBox ID="txtUsername" runat="server" Placeholder="Enter your username" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtUsername_TextChanged" />

            <asp:Label ID="lblSecurityQuestion" runat="server" CssClass="form-control" Text="" AssociatedControlID="txtSecurityAnswer" />

            <asp:TextBox ID="txtSecurityAnswer" runat="server" Placeholder="Your answer" CssClass="form-control" />

            <asp:TextBox ID="txtNewPassword" runat="server" Placeholder="New password" TextMode="Password" CssClass="form-control" />

            <asp:TextBox ID="txtConfirmPassword" runat="server" Placeholder="Confirm new password" TextMode="Password" CssClass="form-control" />

            <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" OnClick="btnResetPassword_Click" CssClass="btn" />

            <asp:Label ID="lblMessage" runat="server" CssClass="message-label" />
        </div>
    </form>
</body>
</html>

