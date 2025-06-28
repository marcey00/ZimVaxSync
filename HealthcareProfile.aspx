<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HealthcareProfile.aspx.cs" Inherits="ZimVaxSync.HealthcareProfile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Healthcare Provider Profile</title>
    <style>
        body, html {
            height: 100%;
            margin: 0;
            font-family: Arial, sans-serif;
            background-image: url('background.png');
            background-size: cover;
            background-position: center;
        }
        .container {
            max-width: 600px;
            margin: auto;
            padding: 30px;
            font-family: Arial;
            border: 1px solid #ccc;
            border-radius: 10px;
        }
        h2 {
            text-align: center;
        }
        .form-group {
            margin-bottom: 15px;
        }
        label, input, select {
            width: 100%;
            padding: 8px;
        }
        .btn-group {
            text-align: center;
            margin-top: 20px;
        }
        .btn {
            padding: 10px 20px;
            margin: 0 10px;
            border: none;
            border-radius: 5px;
            background-color: #2b7a78;
            color: white;
            cursor: pointer;
        }
        .btn:hover {
            background-color: #205e5c;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Healthcare Provider Profile</h2>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />

            <div class="form-group">
                <label>Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" />
            </div>
            <div class="form-group">
                <label>Profession</label>
                <asp:TextBox ID="txtProfession" runat="server" />
            </div>
            <div class="form-group">
                <label>Institution</label>
                <asp:TextBox ID="txtInstitution" runat="server" />
            </div>
            <div class="form-group">
                <label>Institution Address</label>
                <asp:TextBox ID="txtAddress" runat="server" />
            </div>
            <div class="form-group">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" />
            </div>


            <h2>Change Password</h2>
            <div class="form-group">
                <label>Old Password</label>
                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" />
            </div>
            <div class="form-group">
                <label>New Password</label>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" />
            </div>

            <div class="btn-group">
                <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn" OnClick="btnChangePassword_Click" />
            </div>

            <h2>Update Security Question</h2>
            <div class="form-group">
                <label>Security Question</label>
                <asp:DropDownList ID="ddlSecurityQuestion" runat="server">
                    <asp:ListItem Text="What is your mother's maiden name?" />
                    <asp:ListItem Text="What is your favorite book?" />
                    <asp:ListItem Text="What is the name of your first pet?" />
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Answer</label>
                <asp:TextBox ID="txtSecurityAnswer" runat="server" />
            </div>

            <div class="btn-group">
                <asp:Button ID="btnUpdateSecurity" runat="server" Text="Update Security Info" CssClass="btn" OnClick="btnUpdateSecurity_Click" />
            </div>
        </div>
    </form>
</body>
</html>

