<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherSignup.aspx.cs" Inherits="ZimVaxSync.OtherSignup" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Other General User Signup - ZimVaxSync</title>
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

        .section-heading {
            font-size: 16px;
            font-weight: bold;
            margin-top: 20px;
            margin-bottom: 10px;
            color: #007bff;
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
    </style>

    <script type="text/javascript">
        function togglePasswordVisibility() {
            var pwd = document.getElementById('<%= txtPassword.ClientID %>');
            if (pwd.type === "password") {
                pwd.type = "text";
            } else {
                pwd.type = "password";
            }
        }
    </script>
 <script type="text/javascript">
     function validateCheckbox(source, args) {
         var chk = document.getElementById('<%= chkAgreeTerms.ClientID %>');
         args.IsValid = chk.checked;
     }
 </script>



</head>

<body>
    <form id="formOther" runat="server">
        <div class="form-wrapper">
            <div class="signup-box">
                <h2>Other General User Signup</h2>

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

                <div class="section-heading">Login Credentials</div>

                <div class="form-group">
                    <label for="txtUsername">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                    <br />
                    <input type="checkbox" onclick="togglePasswordVisibility()" /> Show Password
                    <div style="font-size: 0.9em; color: #555; margin-top: 5px;">
                        Password should be at least 8 characters long, contain uppercase and lowercase letters, a number, and a special character.
                    </div>
                </div>

                <div class="form-group">
                    <label for="txtConfirmPassword">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
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

                <div class="section-heading">Verification Method</div>

                <div class="form-group">
                    <label>How would you like to receive the verification code?</label>
                    <div class="gender-options">
                        <asp:RadioButton ID="rdoSMS" runat="server" GroupName="VerificationMethod" Text="SMS" />
                        <asp:RadioButton ID="rdoEmail" runat="server" GroupName="VerificationMethod" Text="Email" />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" CssClass="message-label" />
                </div>

                <div class="button-row">
                   <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-back" OnClick="btnBack_Click" CausesValidation="false" />

                    <asp:Button ID="btnOk" runat="server" Text="OK →" CssClass="nav-button" OnClick="btnOk_Click" />
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





            </div>
        </div>
    </form>
</body>
</html>
