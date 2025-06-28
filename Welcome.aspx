a// Welcome.aspx
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="ZimVaxSync.Welcome" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ZIM VAXSYNC - Welcome</title>
    <style>
        body, html {
            margin: 0;
            padding: 0;
            height: 100%;
            font-family: Arial, sans-serif;
        }
        .container {
            display: flex;
            height: 100vh;
            width: 100%;
        }
        .left-panel {
            width: 50%;
            background-color: lightsteelblue ;
            text-align: center;
            padding-top: 50px;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            padding-bottom: 30px;
        }
        .left-panel img {
            width: 400px;
            margin: 0 auto;
        }
        .back-button {
            width: 80px;
            margin: 0 auto;
            padding: 10px;
            background-color: white;
            border: 2px solid black;
            font-weight: bold;
            cursor: pointer;
        }
        .back-button:hover {
            background-color: #e0e0e0;
        }
        .right-panel {
            flex: 1;
            background-image: url('background.png');
            background-size: cover;
            background-position: center;
            position: relative;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            color: black;
        }
        .heading {
            position: absolute;
            top: 30px;
            right: 30px;
            font-size: 20px;
            font-weight: bold;
            text-align: right;
        }
        .category-button {
            width: 250px;
            padding: 15px;
            margin: 10px 0;
            font-size: 16px;
            font-weight: bold;
            background-color: white;
            border: 2px solid black;
            cursor: pointer;
            transition: background 0.3s;
        }
        .category-button:hover {
            background-color: #e0e0e0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="left-panel">
                <img src="logo recent.png" alt="ZIM VAXSYNC Logo" />
                <asp:Button ID="btnBack" runat="server" CssClass="back-button" Text="BACK" OnClick="btnBack_Click" />
            </div>
            <div class="right-panel">
                <div class="heading">
                    SELECT YOUR CATEGORY TO LOGIN OR CREATE ACCOUNT
                </div>
                <asp:Button ID="btnCaregiver" runat="server" CssClass="category-button" Text="CAREGIVER / PARENT" OnClick="btnCaregiver_Click" />
                <asp:Button ID="btnHealthcare" runat="server" CssClass="category-button" Text="HEALTHCARE PROVIDER" OnClick="btnHealthcare_Click" />
                <asp:Button ID="btnOther" runat="server" CssClass="category-button" Text="OTHER GENERAL USERS" OnClick="btnOther_Click" />

                 <a href="TermsAndConditions.aspx" target="_blank">Terms and Conditions</a>

            </div>
          
        </div>
    </form>
</body>
</html>
