<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaregiverSignup.aspx.cs" Inherits="ZimVaxSync.CaregiverSignup" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Caregiver Signup</title>
    <style>
        body {
            font-family: Arial;
            margin: 0;
            padding: 0;
            background-image: url('background.png');
        }

        .container {
            width: 700px;
            margin: 30px auto;
            padding: 25px;
            background-color: #ffffff;
            box-shadow: 0px 0px 15px rgba(0, 0, 0, 0.1);
            border-radius: 10px;
        }

        h2 {
            text-align: center;
            margin-bottom: 30px;
            color: #333333;
        }

        .section-heading {
            font-weight: bold;
            color: #007BFF;
            margin-top: 25px;
            margin-bottom: 10px;
            font-size: 1.1em;
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            display: block;
            margin-bottom: 6px;
            color: #333;
        }

        input[type="text"], input[type="password"], input[type="email"], select {
            width: 100%;
            padding: 10px;
            border-radius: 6px;
            border: 1px solid #ccc;
        }

        .dob-input {
            font-size: 1rem;
            padding: 12px;
        }

        .gender-options {
            display: flex;
            gap: 20px;
            margin-top: 5px;
        }

        .button-group {
            display: flex;
            justify-content: space-between;
            margin-top: 30px;
        }

        .button-group input {
            padding: 10px 25px;
            border: none;
            border-radius: 6px;
            background-color: #007BFF;
            color: #ffffff;
            cursor: pointer;
        }

        .button-group input:hover {
            background-color: #0056b3;
        }

        .error-message {
            color: red;
            text-align: center;
            margin-bottom: 10px;
        }
    </style>

    <script type="text/javascript">
        function togglePassword() {
            var passwordBox = document.getElementById('<%= txtPassword.ClientID %>');
            var confirmPwd = document.getElementById('<%= txtConfirmPassword.ClientID %>');
            var checkbox = document.getElementById('<%= chkShowPassword.ClientID %>');

            if (checkbox.checked) {
                passwordBox.type = 'text';
                confirmPwd.type = 'text';
            } else {
                passwordBox.type = 'password';
                confirmPwd.type = 'password';
            }
        }

        function validateCheckbox(source, args) {
            var chk = document.getElementById('<%= chkAgreeTerms.ClientID %>');
            args.IsValid = chk.checked;
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Caregiver / Parent Signup</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />

            <div class="section-heading">Child's Details</div>

            <div class="form-group">
                <label>Child’s Full Name</label>
                <asp:TextBox ID="txtChildName" runat="server" />
            </div>

            <div class="form-group">
                <label>Child’s Date of Birth</label>
                <asp:TextBox ID="txtChildDOB" runat="server" TextMode="Date" CssClass="dob-input" />
            </div>

            <div class="form-group">
                <label>Gender</label>
                <div class="gender-options">
                    <asp:RadioButton ID="rdoMale" runat="server" GroupName="Gender" Text="Male" />
                    <asp:RadioButton ID="rdoFemale" runat="server" GroupName="Gender" Text="Female" />
                </div>
            </div>

            <div class="form-group">
                <label>Child ID Number</label>
                <asp:TextBox ID="txtChildID" runat="server" />
            </div>

            <div class="section-heading">Relationship to Child</div>

            <div class="form-group">
                <label>Your Relationship to the Child</label>
                <asp:TextBox ID="txtRelationship" runat="server" />
            </div>

            <div class="form-group">
                <label>Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" />
            </div>

            <div class="form-group">
                <label>Phone Number</label>
                <asp:TextBox ID="txtPhoneNumber" runat="server" TextMode="Phone" />
            </div>

            <div class="form-group">
                <label>Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" />
            </div>

            <div class="form-group">
                <label>Address</label>
                <asp:TextBox ID="txtAddress" runat="server" />
            </div>

            <div class="section-heading">Login Credentials</div>

            <div class="form-group">
                <label>Username</label>
                <asp:TextBox ID="txtUsername" runat="server" />
            </div>

            <div class="form-group">
                <label>Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label>Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" />
            </div>

            <div class="form-group checkbox">
                <asp:CheckBox ID="chkShowPassword" runat="server" Text=" Show Password" onclick="togglePassword()" />
            </div>
            Password should be at least 8 characters long, contain uppercase and lowercase letters, a number, and a special character.

            <div class="form-group">
                <label>Security Question</label>
                <asp:DropDownList ID="ddlSecurityQuestion" runat="server">
                    <asp:ListItem Text="What is your mother's maiden name?" Value="maiden" />
                    <asp:ListItem Text="What is your favorite childhood pet's name?" Value="pet" />
                    <asp:ListItem Text="What was your first school?" Value="school" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label>Security Answer</label>
                <asp:TextBox ID="txtSecurityAnswer" runat="server" />
            </div>

            <div class="form-group">
                <label>Preferred Verification Method</label>
                <div class="gender-options">
                    <asp:RadioButton ID="rdoEmail" runat="server" GroupName="Verification" Text="Email" Checked="true" />
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

            <div class="button-group">
              <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-back" OnClick="btnBack_Click" CausesValidation="false" />
                <asp:Button ID="btnOk" runat="server" Text="OK" OnClick="btnOk_Click" />
            </div>
        </div>
    </form>
</body>
</html>
