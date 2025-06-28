<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewChildhoodRecord.aspx.cs" Inherits="ZimVaxSync.ViewChildhoodRecord" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Child Vaccination Record - View</title>
    <style>
        /* General Styles */
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #ffffff;
            color: #222;
            padding: 25px 40px;
            margin: 0;
            -webkit-print-color-adjust: exact;
            print-color-adjust: exact;
        }

        h2 {
            text-align: center;
            font-weight: 700;
            font-size: 28px;
            color: #3b3b98;
            margin-bottom: 30px;
        }
        h3 {
            font-weight: 600;
            font-size: 20px;
            margin-bottom: 12px;
            color: #555;
        }

        /* Section Heading */
        .section-heading {
            background-color: #d6d9f7;
            color: #2e2e8e;
            font-weight: 700;
            font-size: 18px;
            padding: 12px 15px;
            margin-bottom: 15px;
            border-radius: 8px;
            border: 1px solid #b6b9e8;
            box-shadow: inset 0 1px 0 #e2e5ff;
        }

        /* Layout container */
        .flex-container {
            display: flex;
            gap: 30px;
            flex-wrap: wrap;
            margin-bottom: 30px;
        }

        .flex-item {
            flex: 1 1 48%;
            min-width: 300px;
        }

        /* Labels that are data containers */
        asp\:Label.data-table {
            display: block;
            padding: 15px;
            background-color: #f7f8fc;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 14px;
            color: #444;
            min-height: 80px;
            white-space: pre-wrap;
            box-sizing: border-box;
        }

        /* GridView table styling */
        .data-table {
            width: 100%;
            border-collapse: collapse;
            font-size: 14px;
            color: #333;
            margin-bottom: 30px;
            box-shadow: 0 0 8px rgba(0,0,0,0.05);
            border-radius: 8px;
            overflow: hidden;
        }
        .data-table th, .data-table td {
            border: 1px solid #ccc;
            padding: 10px 12px;
            text-align: center;
            vertical-align: middle;
        }
        .data-table th {
            background: linear-gradient(90deg, #6a6aff, #3b3b98);
            color: #fff;
            font-weight: 600;
            letter-spacing: 0.03em;
        }
        .data-table tbody tr:nth-child(odd) {
            background-color: #f9f9ff;
        }
        .data-table tbody tr:hover {
            background-color: #e6e6ff;
        }

        /* Chart container */
        .chart-row {
            display: flex;
            gap: 30px;
            justify-content: space-between;
            flex-wrap: wrap;
            margin-bottom: 35px;
        }
        .chart-box {
            flex: 1 1 48%;
            min-width: 300px;
            background: #f7f8fc;
            padding: 15px;
            border-radius: 8px;
            box-shadow: inset 0 0 8px rgba(0,0,0,0.03);
        }

        /* Notes box */
        .notes-box textarea {
            width: 100%;
            font-size: 14px;
            padding: 12px;
            border-radius: 6px;
            border: 1px solid #ccc;
            resize: vertical;
            box-sizing: border-box;
            min-height: 100px;
            color: #333;
        }

        /* QR code */
        .qr-box {
            text-align: center;
            margin-top: 30px;
            margin-bottom: 40px;
        }
        .qr-box img {
            border: 2px solid #3b3b98;
            padding: 10px;
            border-radius: 12px;
            background-color: #fff;
            max-width: 180px;
            max-height: 180px;
        }

        /* Button row */
        .button-row {
            text-align: center;
            margin-top: 40px;
            margin-bottom: 20px;
        }
        .button-row input[type="submit"],
        .button-row input[type="button"],
        .button-row .aspNetDisabled {
            background-color: #3b3b98;
            color: white;
            font-weight: 600;
            padding: 10px 22px;
            margin: 0 10px 10px 10px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 15px;
            transition: background-color 0.3s ease;
        }
        .button-row input[type="submit"]:hover,
        .button-row input[type="button"]:hover {
            background-color: #252575;
        }

        /* Responsive */
        @media (max-width: 900px) {
            .flex-container {
                flex-direction: column;
            }
            .flex-item {
                flex-basis: 100%;
            }
        }

        /* Print Styles */
        @media print {
            body {
                padding: 10mm 15mm;
                color-adjust: exact;
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
            }
            .button-row {
                display: none !important;
            }
            .section-heading {
                background-color: #d6d9f7 !important;
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
                page-break-inside: avoid;
                border: 1px solid #b6b9e8 !important;
            }
            .data-table th {
                background: #6a6aff !important;
                color: white !important;
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
            }
            .qr-box img {
                max-width: 150px !important;
                max-height: 150px !important;
                border: 2px solid #3b3b98 !important;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Childhood Vaccination Record</h2>

        <div class="section-heading">Child Information</div>
        <asp:Label ID="lblChildInfo" runat="server" Font-Bold="True" CssClass="data-table" />

            <div class="flex-item">
                <div class="section-heading">Vaccination History</div>
                <asp:GridView ID="gvVaccinations" runat="server" AutoGenerateColumns="False" CssClass="data-table">
                    <Columns>
                        <asp:BoundField HeaderText="Age" DataField="AgeGroup" />
                        <asp:BoundField HeaderText="Vaccine" DataField="VaccineName" />
                        <asp:BoundField HeaderText="Batch No." DataField="BatchNumber" />
                        <asp:BoundField HeaderText="Status" DataField="Status" />
                        <asp:BoundField HeaderText="Date Administered" DataField="DateAdministered" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField HeaderText="Next Dose Date" DataField="NextDoseDate" DataFormatString="{0:dd MMM yyyy}" />
                    </Columns>
                </asp:GridView>
                 <div class="flex-container">
     <div class="flex-item">
         <div class="section-heading">Feeding Practices (6–15 Months)</div>
         <asp:Label ID="lblFeeding615" runat="server" Text="Loading..." CssClass="data-table" />

         <div class="section-heading">Feeding Practices (16–24 Months)</div>
         <asp:Label ID="lblFeeding" runat="server" Text="Loading..." CssClass="data-table" />

         <div class="section-heading">Infant Care Practices</div>
         <asp:Label ID="lblInfantCare" runat="server" Text="Loading..." CssClass="data-table" />
     </div>

                <div class="section-heading">Vitamin A Supplement Schedule</div>
                <asp:Label ID="lblVitaminA" runat="server" Text="Loading..." CssClass="data-table" />

                <div class="section-heading">Deworming Schedule</div>
                <asp:Label ID="lblDeworming" runat="server" Text="Loading..." CssClass="data-table" />
            </div>
        </div>

        <div class="section-heading">Developmental Milestones</div>
        <asp:GridView ID="gvMilestones" runat="server" AutoGenerateColumns="False" CssClass="data-table">
            <Columns>
                <asp:BoundField HeaderText="Milestone" DataField="Milestone" />
                <asp:BoundField HeaderText="Achieved" DataField="Achieved" />
                <asp:BoundField HeaderText="Notes" DataField="Notes" />
                <asp:BoundField HeaderText="Date Recorded" DataField="DateRecorded" DataFormatString="{0:dd MMM yyyy}" />
            </Columns>
        </asp:GridView>

        <div class="section-heading">Growth Charts (WHO Reference Lines Included)</div>
        <div class="chart-row">
            <div class="chart-box">
                <asp:Chart ID="ChartWeight" runat="server" Width="400" Height="300">
                    <ChartAreas>
                        <asp:ChartArea Name="WeightArea">
                            <AxisX Title="Age (Months)" />
                            <AxisY Title="Weight (kg)" />
                        </asp:ChartArea>
                    </ChartAreas>
                    <Legends>
                        <asp:Legend Docking="Bottom" />
                    </Legends>
                </asp:Chart>
            </div>
            <div class="chart-box">
                <asp:Chart ID="ChartHeight" runat="server" Width="400" Height="300">
                    <ChartAreas>
                        <asp:ChartArea Name="HeightArea">
                            <AxisX Title="Age (Months)" />
                            <AxisY Title="Height (cm)" />
                        </asp:ChartArea>
                    </ChartAreas>
                    <Legends>
                        <asp:Legend Docking="Bottom" />
                    </Legends>
                </asp:Chart>
            </div>
        </div>

        <div class="section-heading">Additional Comments</div>
        <div class="notes-box">
            <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="5" Width="100%" CssClass="input-field" />
        </div>

        <div class="section-heading">QR Code (Scan to Access Record)</div>
        <div class="qr-box">
            <asp:Image ID="imgQrCode" runat="server" Width="150" Height="150" />
        </div>

        <div class="button-row">
            <asp:Button ID="btnEdit" runat="server" Text="Edit Record" OnClick="btnEdit_Click" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="window.print(); return false;" />
            <asp:Button ID="btnPreview" runat="server" Text="Preview Print" OnClientClick="window.print(); return false;" />
        </div>
    </form>
</body>
</html>
