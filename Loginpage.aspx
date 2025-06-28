<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Loginpage.aspx.cs" Inherits="ZimVaxSync.Loginpage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ZIM VAXSYNC - Login</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        body {
            background-color: #f0f2f5;
            font-family: 'Segoe UI', sans-serif;
            background: url('background.png');
        }

        .login-container {
            max-width: 800px;
            margin: 80px auto;
             background-color: #fff;
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0,0,0,0.1);
        }

        .form-check-label {
            margin-left: 5px;
        }

        .btn-custom {
            width: 100%;
            margin-bottom: 10px;
        }

        #lblStatus {
            color: red;
            margin-top: 10px;
            display: block;
        }

        #lblSelectedRole {
            font-weight: bold;
            color: #2c3e50;
            text-align: center;
            margin-bottom: 20px;
            display: block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <asp:Label ID="lblSelectedRole" runat="server" Text=""></asp:Label>

            <h3 class="text-center mb-4">Login</h3>

            <div class="mb-3">
                <label for="txtUsername" class="form-label">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label for="txtPassword" class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password"></asp:TextBox>
                <div class="form-check mt-2">
                    <input type="checkbox" class="form-check-input" id="showPassword" onclick="togglePassword()" />
                    <label class="form-check-label" for="showPassword">Show Password</label>
                </div>
            </div>

            <div class="form-check mb-3">
                <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input" />
                <label class="form-check-label" for="chkRememberMe">Remember Me</label>
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary btn-custom" OnClick="btnLogin_Click" />
            <asp:Button ID="btnCreateAccount" runat="server" Text="Create Account" CssClass="btn btn-success btn-custom" OnClick="btnCreateAccount_Click" />
            <asp:LinkButton ID="lnkForgotPassword" runat="server" CssClass="btn btn-link btn-custom" OnClick="lnkForgotPassword_Click">Forgot Password?</asp:LinkButton>
            <asp:Button ID="btnBack" runat="server" Text="Back to Welcome" CssClass="btn btn-secondary btn-custom" OnClick="btnBack_Click" />

            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        </div>
    </form>

    <script type="text/javascript">
        function togglePassword() {
            var pwd = document.getElementById('<%= txtPassword.ClientID %>');
            if (pwd.type === "password") {
                pwd.type = "text";
            } else {
                pwd.type = "password";
            }
        }
    </script>
</body>
</html>

