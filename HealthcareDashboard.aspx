<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HealthcareDashboard.aspx.cs" Inherits="ZimVaxSync.HealthcareDashboard" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Healthcare Provider Dashboard</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            font-family: 'Segoe UI', sans-serif;
            margin: 0;
            background-color: #f8f9fa;
        }
        .topbar {
            background-color: #007bff;
            color: white;
            padding: 10px 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .menu-icon {
            font-size: 24px;
            cursor: pointer;
            margin-right: 15px;
        }
        .topbar-right input[type="text"] {
            padding: 6px;
            width: 200px;
        }
        .asp-button {
            padding: 6px 12px;
            margin-left: 5px;
            background-color: white;
            border: none;
            color: #007bff;
            cursor: pointer;
        }
        .sidebar {
            height: 100%;
            width: 0;
            position: fixed;
            top: 0;
            left: 0;
            background-color: #343a40;
            overflow-x: hidden;
            transition: 0.3s;
            padding-top: 60px;
            color: white;
        }
        .sidebar.show {
            width: 250px;
        }
        .sidebar .profile {
            text-align: center;
            padding: 10px;
        }
        .sidebar .profile img {
            width: 60px;
            border-radius: 50%;
        }
        .sidebar a {
            display: block;
            padding: 12px 16px;
            color: white;
            text-decoration: none;
        }
        .sidebar a:hover {
            background-color: #495057;
        }
        .main-content {
            padding: 20px;
            transition: margin-left 0.3s;
        }
        .main-content.shifted {
            margin-left: 250px;
        }
        .dashboard-heading {
            font-size: 24px;
            margin-bottom: 20px;
        }
        .info-card {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            padding: 20px;
            margin-bottom: 20px;
        }
        .modal-centered {
            display: none;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            justify-content: center;
            align-items: center;
            background-color: rgba(0,0,0,0.6);
        }
        .modal-centered-content {
            background-color: white;
            padding: 30px;
            border-radius: 12px;
            width: 90%;
            max-width: 400px;
            box-shadow: 0 5px 20px rgba(0,0,0,0.3);
            text-align: center;
        }
        .modal-centered-content .btn {
            display: block;
            width: 100%;
            margin: 10px 0;
            padding: 12px;
            font-size: 16px;
            border-radius: 6px;
            border: none;
            cursor: pointer;
        }
        .modal-centered-content .btn-primary {
            background-color: #007bff;
            color: white;
        }
        .modal-centered-content .btn-success {
            background-color: #28a745;
            color: white;
        }
        .close-btn {
            margin-top: 15px;
            color: #888;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="topbar">
            <div class="topbar-left">
                <span class="menu-icon" onclick="toggleSidebar()"><i class="fas fa-bars"></i></span>
                <h2 style="display:inline;">Healthcare Dashboard</h2>
            </div>
            <div class="topbar-right">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search users..." />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="asp-button" OnClick="btnSearch_Click" />
            </div>
        </div>

        <div id="sidebar" class="sidebar">
            <div class="profile">
                <img src="https://via.placeholder.com/60" alt="Profile" />
                <asp:Label ID="lblFullName" runat="server" Font-Bold="true" /><br />
                <asp:Label ID="lblProfession" runat="server" /><br />
                <asp:Label ID="lblInstitution" runat="server" />
            </div>
            <a href="HealthcareProfile.aspx">Profile</a>
            <a href="javascript:void(0);" onclick="showAddPatientModal()">Add New Patient</a>
            <a href="Support.aspx">Support</a>
            <a href="Logout.aspx">Logout</a>
        </div>

        <div id="mainContent" class="main-content">
            <h2 class="dashboard-heading">Welcome, <asp:Label ID="lblHealthcareName" runat="server" /></h2>

            <asp:Panel ID="pnlResult" runat="server" Visible="false" CssClass="info-card">
                <h4>Search Results</h4>
                <p><strong>Name:</strong> <asp:Label ID="lblName" runat="server" /></p>
                <p><strong>Date of Birth:</strong> <asp:Label ID="lblDOB" runat="server" /></p>
                <p><strong>ID Number:</strong> <asp:Label ID="lblIDNumber" runat="server" /></p>
                <asp:HiddenField ID="hfUserType" runat="server" />
                <asp:HiddenField ID="hfUserId" runat="server" />
                <div class="d-flex gap-2">
                    <button type="button" class="btn btn-primary" onclick="showAddRecordModal()">Add Vaccination Record</button>
                    <asp:Button ID="btnViewRecord" runat="server" Text="View Vaccination Record" CssClass="btn btn-success" OnClientClick="showViewRecordModal(); return false;" />
                </div>
            </asp:Panel>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
        </div>

        <!-- Add Patient Modal -->
        <div id="addPatientModal" class="modal-centered">
            <div class="modal-centered-content">
                <h3>Select Patient Type</h3>
                <button class="btn btn-primary" type="button" onclick="location.href='AddNewChild.aspx'">Child</button>
                <button class="btn btn-success" type="button" onclick="location.href='AddAdult.aspx'">Adult</button>
                <div class="close-btn" onclick="closeAddPatientModal()">Cancel</div>
            </div>
        </div>

        <!-- Add Record Modal -->
        <div id="addRecordModal" class="modal-centered">
            <div class="modal-centered-content">
                <h3>Select Vaccination Type</h3>
                <asp:Button ID="btnAddChildVaccination" runat="server" Text="Childhood" CssClass="btn btn-primary" OnClick="btnAddChildVaccination_Click" />
                <asp:Button ID="btnAddOtherVaccination" runat="server" Text="Other Vaccinations" CssClass="btn btn-success" OnClick="btnAddOtherVaccinationsRecord_Click" />
                <div class="close-btn" onclick="closeAddRecordModal()">Cancel</div>
            </div>
        </div>

        <!-- View Record Modal -->
        <div id="viewRecordModal" class="modal-centered">
            <div class="modal-centered-content">
                <h3>Select Record Type to View</h3>
                <asp:Button ID="btnViewChildhoodRecord" runat="server" Text="Childhood" CssClass="btn btn-primary" OnClick="btnViewChildhoodRecord_Click" />
                <asp:Button ID="btnViewOtherRecord" runat="server" Text="Other Vaccinations" CssClass="btn btn-success" OnClick="btnViewOtherRecord_Click" />
                <div class="close-btn" onclick="closeViewRecordModal()">Cancel</div>
            </div>
        </div>

        <script>
            function toggleSidebar() {
                document.getElementById("sidebar").classList.toggle("show");
                document.getElementById("mainContent").classList.toggle("shifted");
            }
            function showAddPatientModal() {
                document.getElementById("addPatientModal").style.display = "flex";
            }
            function closeAddPatientModal() {
                document.getElementById("addPatientModal").style.display = "none";
            }
            function showAddRecordModal() {
                document.getElementById("addRecordModal").style.display = "flex";
            }
            function closeAddRecordModal() {
                document.getElementById("addRecordModal").style.display = "none";
            }
            function showViewRecordModal() {
                document.getElementById("viewRecordModal").style.display = "flex";
            }
            function closeViewRecordModal() {
                document.getElementById("viewRecordModal").style.display = "none";
            }
            window.onclick = function (event) {
                ['addPatientModal', 'addRecordModal', 'viewRecordModal'].forEach(id => {
                    const modal = document.getElementById(id);
                    if (event.target === modal) modal.style.display = "none";
                });
            }
        </script>
    </form>
</body>
</html>
