<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaregiverProfile.aspx.cs" Inherits="ZimVaxSync.CaregiverProfile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Caregiver Profile - ZimVaxSync</title>
    <style>
        body { font-family: Arial; padding: 20px;background-image: url('background.png'); }
        .container { max-width: 700px; margin: auto; border: 1px solid #ccc; background-color: #ffffff; padding: 20px; border-radius: 10px; }
        .section-title { font-size: 20px; margin-top: 30px; font-weight: bold; }
        .form-group { margin-bottom: 15px; }
        label { display: block; margin-bottom: 5px; font-weight: bold; }
        input, select { width: 100%; padding: 8px; }
        .btn { padding: 10px 20px; margin-top: 10px; cursor: pointer; border: none; border-radius: 5px; }
        .btn-primary { background-color: #007bff; color: white; }
        .btn-secondary { background-color: #6c757d; color: white; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Caregiver Profile</h2>

            <div class="section-title">Personal Information</div>
            <div class="form-group">
                <label for="txtFullName">Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" />
            </div>
            <div class="form-group">
                <label for="txtEmail">Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" />
            </div>
            <div class="form-group">
                <label for="txtPhone">Phone Number</label>
                <asp:TextBox ID="txtPhone" runat="server" />
            </div>
            <div class="form-group">
                <label for="txtAddress">Address</label>
                <asp:TextBox ID="txtAddress" runat="server" />
            </div>
            <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" CssClass="btn btn-primary" OnClick="btnUpdateProfile_Click" />

            <div class="section-title">Change Password</div>
            <div class="form-group">
                <label for="txtOldPassword">Old Password</label>
                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" />
            </div>
            <div class="form-group">
                <label for="txtNewPassword">New Password</label>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" />
            </div>
            <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-secondary" OnClick="btnChangePassword_Click" />

            <div class="section-title">Update Security Question</div>
            <div class="form-group">
                <label for="ddlSecurityQuestion">Security Question</label>
                <asp:DropDownList ID="ddlSecurityQuestion" runat="server">
                    <asp:ListItem Text="Select a question" Value="" />
                    <asp:ListItem Text="What is your favorite color?" Value="What is your favorite color?" />
                    <asp:ListItem Text="What is your mother's maiden name?" Value="What is your mother's maiden name?" />
                    <asp:ListItem Text="What is the name of your first pet?" Value="What is the name of your first pet?" />
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label for="txtSecurityAnswer">Answer</label>
                <asp:TextBox ID="txtSecurityAnswer" runat="server" />
            </div>
            <asp:Button ID="btnUpdateSecurity" runat="server" Text="Update Security Question" CssClass="btn btn-secondary" OnClick="btnUpdateSecurity_Click" />
        </div>
    </form>
</body>
</html>

