<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerificationPage.aspx.cs" Inherits="ZimVaxSync.VerificationPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Verification Page</title>
    <style>
        body {
            font-family: Arial;
            margin: 50px;
            background-color: #f5f5f5;
             background-image: url('background.png');
        }
        .container {
            width: 700px;
            margin: auto;
            background: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 0 15px rgba(0,0,0,0.2);
        }
        .title {
            font-size: 24px;
            margin-bottom: 15px;
        }
        .btn {
            padding: 10px 15px;
            background-color: #0078d4;
            color: white;
            border: none;
            border-radius: 5px;
            margin-top: 10px;
            cursor: pointer;
        }
        .btn:hover {
            background-color: #005ea3;
        }
        .error {
            color: red;
            margin-top: 10px;
        }
        .success {
            color: green;
            margin-top: 10px;
        }
    </style>
</head>
    <script type="text/javascript">
    let countdownSeconds = 0;

    function startCountdown(seconds) {
        countdownSeconds = seconds;
        const btn = document.getElementById('<%= btnResendCode.ClientID %>');
        const lbl = document.getElementById('countdownLabel');

        btn.disabled = true;

        const timer = setInterval(() => {
            countdownSeconds--;
            lbl.innerText = "You can resend in " + countdownSeconds + "s";

            if (countdownSeconds <= 0) {
                clearInterval(timer);
                btn.disabled = false;
                lbl.innerText = "";
            }
        }, 1000);
    }

    // Auto start if the page is loaded with a timer
    window.onload = function () {
        const remaining = parseInt('<%= Session["CooldownRemaining"] ?? "0" %>');
        if (remaining > 0) {
            startCountdown(remaining);
        }
    }
    </script>

<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="title">Enter Verification Code</div>
           <asp:Label ID="lblInstruction" runat="server" />
            <asp:TextBox ID="txtVerificationCode" runat="server" CssClass="form-control" MaxLength="6" placeholder="Enter code here" /><br />
            <asp:Button ID="btnVerify" runat="server" CssClass="btn" Text="Verify" OnClick="btnVerify_Click" />
            <asp:Button ID="btnResendCode" runat="server" CssClass="btn" Text="Resend Code" OnClick="btnResendCode_Click" /><br />
            <asp:Label ID="lblResult" runat="server" CssClass="error" />
        </div>
    </form>
</body>
</html>
