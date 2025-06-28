<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VaccineRecord.aspx.cs" Inherits="ZimVaxSync.VaccineRecord" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vaccine Record</title>
    <style>
        .form-section {
            width: 500px;
            margin: auto;
            font-family: Arial;
        }

        .form-section h2 {
            text-align: center;
        }

        .form-section label {
            display: block;
            margin-top: 10px;
        }

        .form-section input, .form-section textarea {
            width: 100%;
            padding: 8px;
            margin-top: 5px;
        }

        .form-section .btn {
            margin-top: 15px;
            padding: 10px;
            width: 100%;
            background-color: #007bff;
            color: white;
            border: none;
            font-size: 16px;
            cursor: pointer;
        }

        .info-label {
            font-weight: bold;
            margin-top: 10px;
            display: block;
        }

        .message {
            color: green;
            text-align: center;
            margin-top: 10px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-section">
            <h2>Vaccination Record</h2>

            <asp:Label ID="lblFullName" runat="server" CssClass="info-label" Text="Full Name: " />
            <asp:Label ID="lblGender" runat="server" CssClass="info-label" Text="Gender: " />
            <asp:Label ID="lblDOB" runat="server" CssClass="info-label" Text="Date of Birth: " />
            <asp:Label ID="lblNationalID" runat="server" CssClass="info-label" Text="National ID: " />
            <asp:Label ID="lblPhone" runat="server" CssClass="info-label" Text="Phone Number: " />
            <asp:Label ID="lblAddress" runat="server" CssClass="info-label" Text="Address: " />

            <label for="txtVaccineName">Vaccine Name</label>
            <asp:TextBox ID="txtVaccineName" runat="server" />

            <label for="txtDoseNumber">Dose Number</label>
            <asp:TextBox ID="txtDoseNumber" runat="server" />

            <label for="txtDateGiven">Date Given</label>
            <asp:TextBox ID="txtDateGiven" runat="server" TextMode="Date" />

            <label for="txtBatchNumber">Batch Number</label>
            <asp:TextBox ID="txtBatchNumber" runat="server" />

            <label for="txtManufacturer">Manufacturer</label>
            <asp:TextBox ID="txtManufacturer" runat="server" />

            <label for="txtAdministeredBy">Administered By</label>
            <asp:TextBox ID="txtAdministeredBy" runat="server" />

            <label for="txtNextDose">Next Dose Due</label>
            <asp:TextBox ID="txtNextDose" runat="server" TextMode="Date" />

            <label for="txtFacilityName">Facility Name</label>
            <asp:TextBox ID="txtFacilityName" runat="server" />

            <label for="txtComments">Comments</label>
            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="3" />

            <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save Record" OnClick="btnSave_Click" />

            <asp:Label ID="lblMessage" runat="server" CssClass="message" />
        </div>
    </form>
</body>
</html>

