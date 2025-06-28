<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOtherRecord.aspx.cs" Inherits="ZimVaxSync.ViewOtherRecord" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Other Record</title>
    <style>
        body {
            font-family: Arial, sans-serif;
              background-image: url('background.png');
            margin: 0;
            padding: 20px;
        }

        .a4-container {
            width: 794px; /* A4 width at 96dpi */
            min-height: 1123px; /* A4 height */
            margin: auto;
            background-color: #fff;
            padding: 30px;
            box-shadow: 0 0 8px rgba(0, 0, 0, 0.2);
            box-sizing: border-box;
        }

        .top-section {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            margin-bottom: 20px;
        }

        .header-info {
            font-size: 1.1em;
            font-weight: bold;
            line-height: 1.6;
            background-color: #e8f4fd;
            border-left: 5px solid #007acc;
            padding: 12px;
            white-space: pre-line;
            flex: 1;
        }

        .qr-code {
            width: 150px;
            height: 150px;
            margin-left: 20px;
            border: 1px solid #ccc;
        }

        .filter-section {
            margin-bottom: 20px;
        }

        .form-control {
            width: 300px;
            padding: 6px;
            font-size: 1em;
            margin-left: 10px;
        }

        .record-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }

        .record-table th,
        .record-table td {
            border: 1px solid #999;
            padding: 8px;
            text-align: left;
            font-size: 0.95em;
        }

        .record-table th {
            background-color: #f0f8ff;
        }

        .action-buttons {
            margin-top: 25px;
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
        }

        .btn {
            background-color: #007acc;
            color: white;
            padding: 10px 18px;
            border: none;
            border-radius: 5px;
            font-weight: bold;
            cursor: pointer;
            text-decoration: none;
            display: inline-block;
        }

        .btn:hover {
            background-color: #005f99;
        }
        .logo-img {
    width: 60mm; 
    height: 60mm; 
    object-fit: cover;
    border: 1px solid #ccc;
    margin-left: 20px;
}


        @media print {
            body {
                background: none;
                padding: 0;
            }

            .a4-container {
                box-shadow: none;
                margin: 0;
                width: 100%;
                min-height: auto;
                padding: 0;
            }

            .btn, .form-control {
                display: none;
            }

            
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="a4-container">
            <div class="nav-links">

</div>


            <div class="top-section">
                <asp:Label ID="lblHeader" runat="server" CssClass="header-info" />
                <img src="logo recent.png" alt="Logo" class="logo-img" />
               
            </div>

            <div class="filter-section">
                <asp:Label ID="lblFilter" runat="server" Text="Filter by Vaccine Name:" />
                <asp:TextBox ID="txtFilter" runat="server" AutoPostBack="True" OnTextChanged="txtFilter_TextChanged" CssClass="form-control" />
            </div>

            <asp:GridView ID="gvVaccinationRecords" runat="server" AutoGenerateColumns="False" CssClass="record-table" AllowSorting="True" OnSorting="gvVaccinationRecords_Sorting">
                <Columns>
                    <asp:BoundField DataField="VaccineName" HeaderText="Vaccine Name" SortExpression="VaccineName" />
                    <asp:BoundField DataField="DoseNumber" HeaderText="Dose" SortExpression="DoseNumber" />
                    <asp:BoundField DataField="Manufacturer" HeaderText="Manufacturer" SortExpression="Manufacturer" />
                    <asp:BoundField DataField="DateGiven" HeaderText="Date Given" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DateGiven" />
                    <asp:BoundField DataField="NextDoseDue" HeaderText="Next Due" DataFormatString="{0:dd/MM/yyyy}" SortExpression="NextDoseDue" />
                    <asp:BoundField DataField="FacilityName" HeaderText="Facility" SortExpression="FacilityName" />
                    <asp:BoundField DataField="AdministeredBy" HeaderText="Health Worker" SortExpression="AdministeredBy" />
                </Columns>
            </asp:GridView>

             <asp:Image ID="imgQRCode" runat="server" CssClass="qr-code" />

            <div class="action-buttons">
                <asp:Button ID="btnExportPdf" runat="server" Text="Export to PDF" OnClick="btnExportPdf_Click" />

                <asp:HyperLink ID="lnkDownloadQRCode" runat="server" Visible="false" Text="Download QR Code" CssClass="btn" Target="_blank" />
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn" OnClick="btnBack_Click" />
            </div>

        </div>
    </form>
</body>
</html>
