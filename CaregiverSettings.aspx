<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaregiverSettings.aspx.cs" Inherits="ZimVaxSync.CaregiverSettings" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Caregiver Settings</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 30px;
            background-color: #f9f9f9;
             background-image: url('background.png');
        }
        .container {
            width: 90%;
            max-width: 650px;
            margin: 0 auto;
            background: #fff;
            padding: 25px;
            box-shadow: 0 0 8px rgba(0,0,0,0.1);
            border-radius: 8px;
        }
        h2 {
            text-align: center;
        }
        .nav-links {
            margin-bottom: 20px;
            text-align: center;
        }
        .nav-links a {
            margin: 0 10px;
            text-decoration: none;
            color: #007bff;
        }
        .form-section {
            margin-top: 20px;
        }
        .form-section h4 {
            color: #007bff;
            margin-bottom: 10px;
        }
        .form-section label {
            font-weight: bold;
            display: block;
            margin-top: 12px;
        }
        .form-section input[type="password"],
        .form-section select {
            width: 100%;
            padding: 8px;
            margin-top: 5px;
            border-radius: 4px;
            border: 1px solid #ccc;
        }
        .form-section input[type="checkbox"] {
            margin-right: 10px;
        }
        .form-section .btn {
            margin-top: 15px;
            padding: 10px 20px;
            border: none;
            color: white;
            background: #28a745;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn-danger {
            background-color: #dc3545;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Caregiver Settings</h2>
            <div class="nav-links">
                <a href="CaregiverDashboard.aspx">Dashboard</a> |
                <a href="CaregiverProfile.aspx">Profile</a> |
                <a href="Support.aspx">Support</a> |
                <a href="Logout.aspx">Logout</a>
            </div>

            <div class="form-section">
                <h4>Notification Preferences</h4>
                <asp:CheckBox ID="chkEmail" runat="server" Text=" Email" />
                <asp:CheckBox ID="chkSMS" runat="server" Text=" SMS" />

                <h4>Preferred Notification Language</h4>
                <asp:DropDownList ID="ddlLanguage" runat="server">
                    <asp:ListItem Text="English" Value="English" />
                    <asp:ListItem Text="Shona" Value="Shona" />
                    <asp:ListItem Text="Ndebele" Value="Ndebele" />
                </asp:DropDownList>

                <h4>Extract Adult Child Into Own Account</h4>
                <asp:DropDownList ID="ddlAdultChildren" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAdultChildren_SelectedIndexChanged" />

                <asp:Button ID="btnSavePreferences" runat="server" Text="Save Preferences" CssClass="btn" OnClick="btnSavePreferences_Click" />
                <asp:Button ID="btnDeleteAccount" runat="server" Text="Deactivate & Delete Account" CssClass="btn btn-danger"
                    OnClientClick="return confirm('Are you sure you want to delete your account? This action cannot be undone.');"
                    OnClick="btnDeleteAccount_Click" />
            </div>
        </div>
    </form>
</body>
</html>
