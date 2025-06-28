<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherDashboard.aspx.cs" Inherits="ZimVaxSync.OtherDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Other User Dashboard - ZimVaxSync</title>
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
    </style>

    <script type="text/javascript">
        function toggleSidebar() {
            var sidebar = document.getElementById("mySidebar");
            if (sidebar.style.width === "250px") {
                sidebar.style.width = "0";
            } else {
                sidebar.style.width = "250px";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Sidebar -->
        <div id="mySidebar" class="sidebar">
            <a href="javascript:void(0);" class="closebtn" onclick="toggleSidebar()">×</a>
            <div class="profile-section">
                <div class="profile-icon"></div>
                <h3><asp:Label ID="lblFullName" runat="server" Text="Full Name" /></h3>
            </div>

            <a href="OtherProfile.aspx">Profile</a>
            <a href="ViewChildhoodRecord.aspx">Childhood Vaccinations</a>
            <a href="ViewOtherRecord.aspx">Other Vaccinations</a>
            <a href="UploadVaccinationRecord.aspx">Upload Vaccination Record</a>
            <a href="VaccinationInfo.aspx">Vaccination Information</a>
            <a href="Settings.aspx">Settings</a>
            <a href="Support.aspx">Support</a>
            <a href="Logout.aspx">Logout</a>
        </div>

        <!-- Topbar -->
        <div class="topbar">
            <button class="openbtn" type="button" onclick="toggleSidebar()">☰</button>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-bar" placeholder="Search..." />
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

            <div class="dashboard-card">
                <h2>Vaccination Records</h2>
                <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="false" CssClass="table">
                    <Columns>
                        <asp:BoundField DataField="VaccineName" HeaderText="Vaccine" />
                        <asp:BoundField DataField="DateGiven" HeaderText="Date Given" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="HealthFacility" HeaderText="Health Facility" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
