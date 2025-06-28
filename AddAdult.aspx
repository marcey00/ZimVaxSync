<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAdult.aspx.cs" Inherits="ZimVaxSync.AddAdult" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Adult Profile - ZimVaxSync</title>
    <style>
        body, html {
            height: 100%;
            margin: 0;
            font-family: Arial, sans-serif;
            background-image: url('background.png');
            background-size: cover;
            background-position: center;
        }

        .form-wrapper {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }

        .signup-box {
            background-color: #fff;
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
            width: 500px;
        }

        .signup-box h2 {
            text-align: center;
            margin-bottom: 20px;
            color: #333;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-group label {
            font-weight: bold;
            display: block;
            margin-bottom: 5px;
        }

        .form-control, input[type="date"] {
            width: 100%;
            padding: 10px;
            border-radius: 8px;
            border: 1px solid #ccc;
            box-sizing: border-box;
        }

        .gender-options {
            display: flex;
            gap: 15px;
        }

        .button-row {
            display: flex;
            justify-content: space-between;
            margin-top: 20px;
        }

        .nav-button {
            background-color: #007bff;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 8px;
            cursor: pointer;
            font-size: 14px;
        }

        .nav-button:hover {
            background-color: #0056b3;
        }

        .message-label {
            color: red;
            margin-top: 10px;
        }
    </style>
</head>
<body>
    <form id="formAddAdult" runat="server">
        <div class="form-wrapper">
            <div class="signup-box">
                <h2>Add Adult Profile</h2>

                <div class="form-group">
                    <label for="txtFullName">Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label for="txtDOB">Date of Birth</label>
                    <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="form-group">
                    <label for="txtNationalID">National ID Number</label>
                    <asp:TextBox ID="txtNationalID" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label>Gender</label>
                    <div class="gender-options">
                        <asp:RadioButton ID="rdoMale" runat="server" GroupName="Gender" Text="Male" />
                        <asp:RadioButton ID="rdoFemale" runat="server" GroupName="Gender" Text="Female" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="txtAddress">Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label for="txtPhone">Phone Number</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label for="txtEmail">Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                </div>

                <div class="form-group">
                    <label for="ddlSecurityQuestion">Security Question</label>
                    <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="form-control">
                        <asp:ListItem Text="-- Select a question --" Value="" />
                        <asp:ListItem Text="What is your mother’s maiden name?" Value="MotherMaidenName" />
                        <asp:ListItem Text="What was the name of your first pet?" Value="FirstPet" />
                        <asp:ListItem Text="What was your first school?" Value="FirstSchool" />
                        <asp:ListItem Text="What is your favorite food?" Value="FavoriteFood" />
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label for="txtSecurityAnswer">Your Answer</label>
                    <asp:TextBox ID="txtSecurityAnswer" runat="server" CssClass="form-control" />
                </div>

                <asp:Label ID="lblMessage" runat="server" CssClass="message-label" />

                <div class="button-row">
                    <asp:Button ID="btnBack" runat="server" Text="← Back" CssClass="nav-button" OnClick="btnBack_Click" />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="nav-button" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
