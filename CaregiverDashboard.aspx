<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaregiverDashboard.aspx.cs" Inherits="ZimVaxSync.CaregiverDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Caregiver Dashboard - ZimVaxSync</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            font-family: 'Segoe UI', sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f6f8;
        }

        .topbar {
            background-color: #007bff;
            color: white;
            padding: 14px 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .topbar .openbtn {
            font-size: 22px;
            background: none;
            border: none;
            color: white;
            cursor: pointer;
        }

        .sidebar {
            height: 100%;
            width: 0;
            position: fixed;
            top: 0;
            left: 0;
            background-color: #2c3e50;
            overflow-x: hidden;
            overflow-y: auto;
            transition: 0.3s;
            padding-top: 60px;
            z-index: 1000;
            color: white;
        }

        .sidebar a {
            padding: 12px 24px;
            text-decoration: none;
            color: white;
            display: block;
            transition: 0.3s;
            cursor: pointer;
        }

        .sidebar a:hover {
            background-color: #34495e;
        }

        .profile-section {
            text-align: center;
            padding: 20px;
            border-bottom: 1px solid #34495e;
        }

        .profile-icon {
            width: 80px;
            height: 80px;
            border-radius: 50%;
            background-color: #ecf0f1;
            margin: auto;
        }

        .child-profile-section {
            padding: 20px;
            border-bottom: 1px solid #34495e;
        }

        .main-content {
            margin-left: 0;
            padding: 30px;
        }

        .dashboard-card {
            background-color: white;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            max-width: 800px;
            margin: auto;
            margin-bottom: 30px;
        }

        .dashboard-card h2 {
            margin-top: 0;
            margin-bottom: 20px;
            color: #333;
        }

        .search-bar {
            padding: 8px 12px;
            font-size: 16px;
            border-radius: 4px;
            border: 1px solid #ccc;
            width: 250px;
        }

        .vaccination-schedule table {
            width: 100%;
            border-collapse: collapse;
        }

        .vaccination-schedule th, .vaccination-schedule td {
            border: 1px solid #ddd;
            padding: 10px;
            text-align: left;
        }

        .vaccination-schedule th {
            background-color: #007bff;
            color: white;
        }

        .table {
            width: 100%;
            border-collapse: collapse;
        }

        .table th, .table td {
            padding: 12px 15px;
            border: 1px solid #ddd;
        }

        .table th {
            background-color: #007bff;
            color: white;
        }

        .table tr:hover {
            background-color: #f1f1f1;
        }

        .closebtn {
            position: absolute;
            top: 15px;
            right: 25px;
            font-size: 24px;
            cursor: pointer;
        }

        .modal-overlay {
            display: none;
            position: fixed;
            z-index: 1500;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0,0,0,0.4);
        }

        .modal-content {
            background-color: #fff;
            margin: 10% auto;
            padding: 30px;
            border: 1px solid #888;
            width: 300px;
            border-radius: 10px;
            text-align: center;
        }

        .modal-content button {
            display: block;
            width: 100%;
            padding: 12px;
            margin-bottom: 15px;
            font-size: 16px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }

        .modal-content button:hover {
            background-color: #0056b3;
        }

        .modal-content a {
            display: block;
            margin-top: 10px;
            color: #007bff;
            text-decoration: none;
            font-size: 14px;
        }

        .modal-content a:hover {
            text-decoration: underline;
        }
    </style>

    <script type="text/javascript">
        function openSidebar() {
            document.getElementById("mySidebar").style.width = "250px";
        }

        function closeSidebar() {
            document.getElementById("mySidebar").style.width = "0";
        }

        function openModal() {
            document.getElementById("recordModal").style.display = "block";
        }

        function closeModal() {
            document.getElementById("recordModal").style.display = "none";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfSelectedChildId" runat="server" />
        <!-- Sidebar -->
        <div id="mySidebar" class="sidebar">
            <a href="javascript:void(0);" class="closebtn" onclick="closeSidebar()">×</a>
            <div class="profile-section">
                <div class="profile-icon"></div>
                <h3><asp:Label ID="lblFullName" runat="server" Text="Name Here" /></h3>
                <p>Age: <asp:Label ID="lblAge" runat="server" /></p>
                <p>Gender: <asp:Label ID="lblGender" runat="server" /></p>
            </div>

            <div class="child-profile-section">
                <h4>Select Child Profile</h4>
                <asp:DropDownList ID="ddlAccounts" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAccounts_SelectedIndexChanged" />
            </div>

            <a href="CaregiverProfile.aspx">Profile</a>
            <a href="ViewChildhoodRecord.aspx">Childhood Vaccinations</a>
            <a href="ViewOtherRecord.aspx">Other Vaccinations</a>
             <a href="UploadVaccinationRecord.aspx">Upload Vaccination Record</a>
            <a href="VaccinationInfo.aspx">Vaccination Information</a>
            <a href="AddChild.aspx">Add Child</a>
            <a href="CaregiverSettings.aspx">Settings</a>
            <a href="Support.aspx">Support</a>
            <a href="Logout.aspx">Logout</a>
        </div>

        <!-- Topbar -->
        <div class="topbar">
            <button class="openbtn" type="button" onclick="openSidebar()">☰</button>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-bar" Placeholder="Search..." />
        </div>

        <!-- Main Content -->
        <div class="main-content">
            <div class="dashboard-card">
                <h2>Upcoming Vaccinations</h2>
                <div class="vaccination-schedule">
                    <asp:Repeater ID="rptSchedule" runat="server">
                        <HeaderTemplate>
                            <table>
                                <tr>
                                    <th>Vaccine</th>
                                    <th>Due Date</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("VaccineName") %></td>
                                <td><%# Eval("DueDate", "{0:dd MMM yyyy}") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
