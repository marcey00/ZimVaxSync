<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HealthcareSignup.aspx.cs" Inherits="ZimVaxSync.HealthcareSignup"Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Healthcare Provider Signup</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            padding: 40px;
            background: url('background.png');
        }

        .form-container {
            background-color: #ffffff;
            max-width: 600px;
            margin: auto;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 0 15px rgba(0,0,0,0.1);
        }

        h2 {
            text-align: center;
            margin-bottom: 25px;
            color: #333;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-label {
            display: block;
            margin-bottom: 6px;
            font-weight: bold;
        }

        .form-control {
            width: 100%;
            padding: 8px 10px;
            border: 1px solid #ccc;
            border-radius: 6px;
        }

        .btn-group {
            display: flex;
            justify-content: space-between;
            margin-top: 30px;
        }

        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            font-weight: bold;
            cursor: pointer;
        }

        .btn-ok, .btn-back {
            background-color: #007bff;
            color: white;
        }

        .gender-options, .verification-options {
            display: flex;
            gap: 15px;
        }

        .checkbox {
            margin-top: 10px;
        }

        .message-label {
            color: red;
            font-weight: bold;
            text-align: center;
            margin-bottom: 10px;
        }
    </style>

    <script type="text/javascript">
        function togglePassword() {
            var pwd = document.getElementById('<%= txtPassword.ClientID %>');
            var confirmPwd = document.getElementById('<%= txtConfirmPassword.ClientID %>');
            var checkbox = document.getElementById('<%= chkShowPassword.ClientID %>');

            pwd.type = checkbox.checked ? 'text' : 'password';
            confirmPwd.type = checkbox.checked ? 'text' : 'password';
        }

        function validateCheckbox(source, args) {
            var chk = document.getElementById('<%= chkAgreeTerms.ClientID %>');
            args.IsValid = chk.checked;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>Healthcare Provider Signup</h2>

            <asp:Label ID="lblMessage" runat="server" CssClass="message-label" />

            <div class="form-group">
                <label class="form-label">Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Date of Birth</label>
                <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Gender</label>
                <div class="gender-options">
                    <asp:RadioButton ID="rdoMale" runat="server" GroupName="Gender" Text="Male" />
                    <asp:RadioButton ID="rdoFemale" runat="server" GroupName="Gender" Text="Female" />
                </div>
            </div>

            <div class="form-group">
                <label class="form-label">National ID Number</label>
                <asp:TextBox ID="txtNationalID" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <asp:Label CssClass="form-label" ID="lblProfession" runat="server" Text="Profession:" />
                <asp:TextBox ID="txtProfession" CssClass="form-control" runat="server" />
            </div>

            <div class="form-group">
                <label class="form-label">Practice Certificate Number</label>
                <asp:TextBox ID="txtPracticeCertificateNumber" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Institution</label>
                <asp:TextBox ID="txtInstitution" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Phone</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" />
            </div>

            <div class="form-group checkbox">
                <asp:CheckBox ID="chkShowPassword" runat="server" Text=" Show Password" onclick="togglePassword()" />
            </div>

            <div class="form-group">
                <label class="form-label">Security Question</label>
                <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-- Select a Question --" Value="" />
                    <asp:ListItem Text="What is your mother's maiden name?" Value="maiden" />
                    <asp:ListItem Text="What is the name of your first pet?" Value="pet" />
                    <asp:ListItem Text="What was the name of your primary school?" Value="school" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label class="form-label">Security Answer</label>
                <asp:TextBox ID="txtSecurityAnswer" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Verification Method</label>
                <div class="verification-options">
                    <asp:RadioButton ID="rdoEmail" runat="server" GroupName="Verification" Text="Email" Checked="True" />
                    <asp:RadioButton ID="rdoSMS" runat="server" GroupName="Verification" Text="SMS" />
                </div>
            </div>

            <div class="form-group">
                <asp:CheckBox ID="chkAgreeTerms" runat="server" />
                <span>
                    I agree to the 
                    <a href="TermsAndConditions.aspx" target="_blank">terms and conditions</a>.
                </span>
                <br />
                <asp:CustomValidator ID="cvAgreeTerms" runat="server"
                    OnServerValidate="cvAgreeTerms_ServerValidate"
                    ClientValidationFunction="validateCheckbox"
                    ErrorMessage="You must agree to the terms and conditions."
                    Display="Dynamic"
                    ForeColor="Red" />
            </div>

            <div class="btn-group">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-back" OnClick="btnBack_Click" CausesValidation="false" />
                <asp:Button ID="btnOk" runat="server" Text="OK" CssClass="btn btn-ok" OnClick="btnOk_Click" />
            </div>
        </div>
    </form>
</body>
</html>
