<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="ZimVaxSync.Logout" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Confirm Logout</title>
    <style>
        body {
            font-family: Arial;
            background-color: #f0f0f0;
        }

        .modal {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .modal-content {
            background: white;
            padding: 30px;
            width: 350px;
            text-align: center;
            box-shadow: 0 0 10px rgba(0,0,0,0.3);
            border-radius: 10px;
            position: relative;
        }

        .modal-content h2 {
            font-size: 20px;
            margin-bottom: 20px;
        }

        .btn {
            padding: 10px 20px;
            border: none;
            font-weight: bold;
            cursor: pointer;
            border-radius: 5px;
        }

        .btn-no {
            background-color: #ccc;
            position: absolute;
            bottom: 20px;
            left: 20px;
        }

        .btn-yes {
            background-color: #d9534f;
            color: white;
            position: absolute;
            bottom: 20px;
            right: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="modal">
            <div class="modal-content">
                <h2>You are about to logout.<br />Are you sure you want to log out?</h2>
                <asp:HiddenField ID="hfUserRole" runat="server" />
                <button type="button" class="btn btn-no" onclick="cancelLogout()">No</button>
                <button type="button" class="btn btn-yes" onclick="confirmLogout()">Yes</button>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function confirmLogout() {
            window.location.href = "LogoutNow.aspx";
        }

        function cancelLogout() {
            var role = document.getElementById('<%= hfUserRole.ClientID %>').value;
            if (role === "Caregiver") {
                window.location.href = "CaregiverDashboard.aspx";
            } else if (role === "HealthcareProvider") {
                window.location.href = "HealthcareDashboard.aspx";
            } else if (role === "Other") {
                window.location.href = "OtherDashboard.aspx";
            } else {
                window.location.href = "Welcome.aspx";
            }
        }
    </script>
</body>
</html>
