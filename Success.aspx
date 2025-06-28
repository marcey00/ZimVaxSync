<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="ZimVaxSync.Success" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Verification Successful - ZimVaxSync</title>
    <style>
        body, html {
            height: 100%;
            margin: 0;
            font-family: Arial, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            background-color: #e8f5e9;
        }

        .success-box {
            text-align: center;
            background-color: white;
            padding: 50px;
            border-radius: 15px;
            box-shadow: 0 0 20px rgba(0, 128, 0, 0.2);
        }

        .success-box h2 {
            color: #28a745;
            margin-bottom: 20px;
        }

        .success-box p {
            font-size: 16px;
            margin-bottom: 30px;
        }

        .success-box .btn {
            padding: 10px 20px;
            font-size: 14px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            background-color: #28a745;
            color: white;
        }

        .success-box .btn:hover {
            background-color: #218838;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="success-box">
            <h2>Verification Successful</h2>
            <p>Your account has been successfully verified.</p>
            <asp:Button ID="btnProceed" runat="server" Text="Go to Login Page" CssClass="btn" OnClick="btnProceed_Click" />
        </div>
    </form>
</body>
</html>

