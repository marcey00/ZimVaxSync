<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="AddChildhoodRecord.aspx.cs" Inherits="ZimVaxSync.AddChildhoodRecord" %> 
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Child Vaccination Record</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #fff;
            padding: 20px;
        }

        .vaccination-table {
            border-collapse: collapse;
            width: 100%;
            border: 2px solid #000;
            table-layout: fixed;
        }
        .back-button {
    display: inline-block;
    padding: 12px 24px;
    background-color: #337ab7;
    color: white;
    text-decoration: none;
    border-radius: 5px;
    margin-top: 30px;
}

.back-button:hover {
    background-color: #286090;
}

        .vaccination-table th,
        .vaccination-table td {
            border: 1px solid #000;
            padding: 4px;
            font-size: 13px;
            text-align: center;
            vertical-align: middle;
        }

        .vaccination-table th {
            background-color: #f0f0f0;
            font-weight: bold;
        }

        .input-field {
            width: 95%;
            padding: 4px;
            border-radius: 6px;
            border: 1px solid #ccc;
            font-size: 12px;
        }

        .section-title {
            writing-mode: vertical-lr;
            text-orientation: upright;
            font-weight: bold;
            font-size: 13px;
            background-color: #fff;
            padding: 10px 5px;
            white-space: nowrap;
        }

        .girls-row {
            background-color: #fddde6;
        }

        .age-column { width: 8%; }
        .vaccine-column { width: 18%; }
        .status-column { width: 5%; }
        .batch-column { width: 10%; }
        .date-column { width: 10%; }
        .next-column { width: 10%; }
    </style>
    <style>
    .section-heading {
        background-color: #e6ccff; /* light purple */
        text-align: center;
        font-weight: bold;
        padding: 10px;
        border-radius: 6px 6px 0 0;
        border: 1px solid #ccc;
        width: 100%;
    }

    .dose-header {
        display: flex;
        width: 100%;
        border: 1px solid #ccc;
        border-top: none;
    }

    .dose-left {
        width: 20%;
        background-color: #f3f3f3;
        font-weight: bold;
        text-align: center;
        padding: 10px;
        border-right: 1px solid #ccc;
    }

    .dose-right {
        width: 80%;
        background-color: #f3f3f3;
        font-weight: bold;
        text-align: center;
        padding: 10px;
    }

    .vitamin-table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 0;
    }

    .vitamin-table th, .vitamin-table td {
        border: 1px solid #ccc;
        text-align: center;
        padding: 6px;
    }

    .vitamin-textbox {
        width: 100%;
        height: 50px;
        padding: 0;
        margin: 0;
        border: none;
        border-top: 1px solid #ccc;
        border-bottom: 1px solid #ccc;
        position: relative;
        font-size: 12px;
        text-align: center;
        box-sizing: border-box;
    }

    .vitamin-textbox::placeholder {
        color: #aaa;
        font-style: italic;
    }
.textbox-container {
    display: flex;
    flex-direction: column;
    gap: 4px;
}

.textbox-container input[type="text"],
.textbox-container input[type="date"] {
    width: 100%;
    box-sizing: border-box;
}


    .textbox-container::before {
        content: "";
        position: absolute;
        top: 50%;
        left: 0;
        width: 100%;
        height: 1px;
        background-color: #ccc;
    }

    .textbox-top, .textbox-bottom {
        position: absolute;
        width: 100%;
        height: 50%;
        border: none;
        text-align: center;
        font-size: 11px;
        box-sizing: border-box;
    }

    .textbox-top {
        top: 0;
        border-bottom: none;
        padding-top: 4px;
    }

    .textbox-bottom {
        bottom: 0;
        border-top: none;
        padding-bottom: 4px;
    }
</style>
    <!-- 4. Deworming Section -->
<style>
    .section-heading {
        background-color: #e6ccff; /* Light purple */
        text-align: center;
        font-weight: bold;
        padding: 10px;
        font-size: 18px;
        border: 1px solid #ccc;
    }

    .deworming-table, .deworming-table th, .deworming-table td {
        border: 1px solid #ccc;
        border-collapse: collapse;
        text-align: center;
        vertical-align: middle;
    }

    .deworming-table th, .deworming-table td {
        padding: 10px;
    }

    .deworming-table {
        width: 100%;
        margin-top: 10px;
    }
</style>
</head>
<body>
     <form id="form" runat="server">
        <a href="HealthcareDashboard.aspx" class="back-button">&larr; Back to Dashboard</a>
      <!-- Added Child and Caregiver Information (do not alter below) -->
        <div style="margin-bottom:20px; text-align: center;">
            <asp:Label ID="lblChildInfo" runat="server" Font-Bold="True" Font-Size="Large" /><br />
           <asp:Label ID="lblGender" runat="server" Font-Bold="True" Font-Size="Large" /><br />
            <asp:Label ID="lblDOB" runat="server" Font-Bold="True" Font-Size="Medium" Text="Date of Birth:" />
            <br />
            <asp:Label ID="lblIDNumber" runat="server" Font-Bold="True" Font-Size="Medium" Text="ID Number:" />
            <br />
            
           <asp:Label ID="lblCaregiverName" runat="server" CssClass="info-label" /><br />
            <asp:Label ID="lblRelationship" runat="server" CssClass="info-label" /><br />
<asp:Label ID="lblCaregiverInfo" runat="server" CssClass="info-label" /><br />
            <asp:Label ID="lblCaregiverAddress" runat="server" />
<asp:Label ID="lblCaregiverPhone" runat="server" />
<asp:Label ID="Label1" runat="server" ForeColor="Red" />

        </div>
        <!-- End Added Child Information -->
        
        <table class="vaccination-table">
            <tr>
                <td rowspan="17" class="section-title">PRIMARY<br/>COURSE</td>
                <th class="age-column">AGE</th>
                <th class="vaccine-column">VACCINE</th>
                <th class="vaccine-column">BATCH NUMBER</th>
                <th class="status-column">STATUS</th>
                <th class="date-column">DATE OF ADMINISTER</th>
                <th class="next-column">Next dose date</th>
            </tr>

            <!-- PRIMARY COURSE -->
            <!-- PRIMARY COURSE -->
<tr>
    <td>Birth</td>
    <td>BCG</td>
    <td><asp:TextBox ID="txtBatchBCG" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusBCG" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateBCG" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextBCG" runat="server" TextMode="Date" CssClass="input-field" ReadOnly="true" Enabled="false" /></td>
</tr>
<tr>
    <td></td>
    <td>HEPB</td>
    <td><asp:TextBox ID="txtBatchHEPB" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusHEPB" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateHEPB" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextHEPB" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td>6 Weeks</td>
    <td>DTP-Hib-HEPB1</td>
    <td><asp:TextBox ID="txtBatchDTP1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusDTP1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateDTP1" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextDTP1" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>OPV1</td>
    <td><asp:TextBox ID="txtBatchOPV1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusOPV1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateOPV1" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextOPV1" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>ROTAVIRUS 1</td>
    <td><asp:TextBox ID="txtBatchROTA1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusROTA1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateROTA1" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextROTA1" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>NEUMOCOCCAL1</td>
    <td><asp:TextBox ID="txtBatchNEU1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusNEU1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateNEU1" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextNEU1" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td>10 Weeks</td>
    <td>DTP-Hib-HEPB 1.2</td>
    <td><asp:TextBox ID="txtBatchDTP2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusDTP2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateDTP2" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextDTP2" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>OPV2</td>
    <td><asp:TextBox ID="txtBatchOPV2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusOPV2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateOPV2" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextOPV2" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>ROTAVIRUS 2</td>
    <td><asp:TextBox ID="txtBatchROTA2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusROTA2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateROTA2" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextROTA2" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>NEUMOCOCCAL2</td>
    <td><asp:TextBox ID="txtBatchNEU2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusNEU2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateNEU2" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextNEU2" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td>14 Weeks</td>
    <td>DTP-Hib-HEPB 1.3</td>
    <td><asp:TextBox ID="txtBatchDTP3" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusDTP3" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateDTP3" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextDTP3" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>NEUMOCOCCAL3</td>
    <td><asp:TextBox ID="txtBatchNEU3" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusNEU3" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateNEU3" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextNEU3" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>OPV3</td>
    <td><asp:TextBox ID="txtBatchOPV3" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusOPV3" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateOPV3" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextOPV3" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>IPV</td>
    <td><asp:TextBox ID="txtBatchIPV" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusIPV" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateIPV" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextIPV" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td>9 Months</td>
    <td>MEASLES RUBELLA 1</td>
    <td><asp:TextBox ID="txtBatchMR1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusMR1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateMR1" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextMR1" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>TYPHOID VACCINE</td>
    <td><asp:TextBox ID="txtBatchTyphoid" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusTyphoid" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateTyphoid" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextTyphoid" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>


          <!-- BOOSTERS section --> 
<tr>
    <td rowspan="5" class="section-title">BOOSTERS</td>
    <td>18 Months</td>
    <td>OPV</td>
    <td><asp:TextBox ID="txtBatchOPVBooster" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusOPVBooster" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateOPVBooster" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextOPVBooster" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>DTP</td>
    <td><asp:TextBox ID="txtBatchDTPBooster" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusDTPBooster" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateDTPBooster" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextDTPBooster" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td></td>
    <td>MEASLES RUBELLA 2</td>
    <td><asp:TextBox ID="txtBatchMR2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusMR2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateMR2" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextMR2" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td>5 Years</td>
    <td>Td1</td>
    <td><asp:TextBox ID="txtBatchTd1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusTd1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateTd1" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextTd1" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr>
    <td>10 Years</td>
    <td>Td2</td>
    <td><asp:TextBox ID="txtBatchTd2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusTd2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateTd2" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextTd2" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<!-- GIRLS -->
    <tr id="Tr1" runat="server" class="girls-row">
    <td rowspan="2" class="section-title">GIRLS</td>
    <td>Grade 5 (10 Years)</td>
    <td>HPV 1</td>
    <td><asp:TextBox ID="txtBatchHPV1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusHPV1" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateHPV1" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextHPV1" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
<tr id="Tr2" runat="server" class="girls-row">
    <td>Grade 6 (11 Years)</td>
    <td>HPV 2</td>
    <td><asp:TextBox ID="txtBatchHPV2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtStatusHPV2" runat="server" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtDateHPV2" runat="server" TextMode="Date" CssClass="input-field" /></td>
    <td><asp:TextBox ID="txtNextHPV2" runat="server" TextMode="Date" CssClass="input-field" /></td>
</tr>
</table>


        <div style="text-align: center; margin-top: 25px;">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Record" CssClass="input-field" OnClick="btnSubmit_Click" />
        </div>
          
    

<hr />

<!-- 1. Growth Monitoring Section -->
<h3>Growth Monitoring</h3>
<asp:GridView ID="gvGrowthMonitoring" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
    <Columns>

        <asp:BoundField DataField="Age" HeaderText="Age (Months)" />
        <asp:BoundField DataField="Weight" HeaderText="Weight (kg)" />
        <asp:BoundField DataField="Length" HeaderText="Length (cm)" />
        <asp:BoundField DataField="HeadCircumference" HeaderText="Head Circumference (cm)" />
        <asp:BoundField DataField="Date" HeaderText="Date" />
    </Columns>
</asp:GridView>

<!-- 2. Developmental Milestones Section -->
<h4>Developmental Milestones</h4>
<table class="table table-bordered">
    <tr>
        <th>Milestone</th>
        <th>Achieved</th>
        <th>Date Recorded</th>
    </tr>
    <tr>
        <td>Smiles / Responds to Sound (1.5–2 months)</td>
        <td><asp:CheckBox ID="chkSmiles" runat="server" /></td>
        <td><asp:TextBox ID="txtDateSmiles" runat="server" TextMode="Date" CssClass="form-control" /></td>
    </tr>
    <tr>
        <td>Holds head up (2–3 months)</td>
        <td><asp:CheckBox ID="chkHoldsHead" runat="server" /></td>
        <td><asp:TextBox ID="txtDateHoldsHead" runat="server" TextMode="Date" CssClass="form-control" /></td>
    </tr>
    <tr>
        <td>Sits with support (4–6 months)</td>
        <td><asp:CheckBox ID="chkSits" runat="server" /></td>
        <td><asp:TextBox ID="txtDateSits" runat="server" TextMode="Date" CssClass="form-control" /></td>
    </tr>
    <tr>
        <td>Crawls / Walks (6–18 months)</td>
        <td><asp:CheckBox ID="chkCrawls" runat="server" /></td>
        <td><asp:TextBox ID="txtDateCrawls" runat="server" TextMode="Date" CssClass="form-control" /></td>
    </tr>
    <tr>
        <td>Says first words (10–15 months)</td>
        <td><asp:CheckBox ID="chkFirstWords" runat="server" /></td>
        <td><asp:TextBox ID="txtDateFirstWords" runat="server" TextMode="Date" CssClass="form-control" /></td>
    </tr>
    <tr>
        <td>Social interaction (6–12 months)</td>
        <td><asp:CheckBox ID="chkSocial" runat="server" /></td>
        <td><asp:TextBox ID="txtDateSocial" runat="server" TextMode="Date" CssClass="form-control" /></td>
    </tr>
</table>





<div class="section-heading">VITAMIN A SUPPLEMENTATION SCHEDULE</div>

<div class="dose-header">
    <div class="dose-left">DOSE</div>
    <div class="dose-right">ENTER DATE GIVEN AND BATCH NUMBER</div>
</div>

<table class="vitamin-table">
    <tr>
        <th>Age (Months)</th>
        <th>6–11</th>
        <th>12–23</th>
        <th>24–35</th>
        <th>36–47</th>
        <th>48–59</th>
    </tr>
    <!-- First Dose Row -->
    <tr>
        <td><strong>First Dose</strong></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtFirstDose_6_11_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtFirstDose_6_11_Date" runat="server" TextMode="Date" />
        </div></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtFirstDose_12_23_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtFirstDose_12_23_Date" runat="server" TextMode="Date" />
        </div></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtFirstDose_24_35_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtFirstDose_24_35_Date" runat="server" TextMode="Date" />
        </div></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtFirstDose_36_47_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtFirstDose_36_47_Date" runat="server" TextMode="Date" />
        </div></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtFirstDose_48_59_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtFirstDose_48_59_Date" runat="server" TextMode="Date" />
        </div></td>
    </tr>

    <!-- Second Dose Row -->
    <tr>
        <td><strong>Second Dose</strong></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtSecondDose_6_11_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtSecondDose_6_11_Date" runat="server" TextMode="Date" />
        </div></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtSecondDose_12_23_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtSecondDose_12_23_Date" runat="server" TextMode="Date" />
        </div></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtSecondDose_24_35_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtSecondDose_24_35_Date" runat="server" TextMode="Date" />
        </div></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtSecondDose_36_47_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtSecondDose_36_47_Date" runat="server" TextMode="Date" />
        </div></td>
        <td><div class="textbox-container">
            <asp:TextBox ID="txtSecondDose_48_59_Batch" runat="server" Placeholder="Batch No." />
            <asp:TextBox ID="txtSecondDose_48_59_Date" runat="server" TextMode="Date" />
        </div></td>
    </tr>
</table>


<!-- 4. Clinical Notes / Doctor Remarks Section -->
<h3>Clinical Notes / Doctor Remarks</h3>
<asp:TextBox ID="txtDoctorRemarks" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" placeholder="Enter any additional health notes or observations here..." />

<br />
<asp:Button ID="btnSaveAll" runat="server" Text="Save Record" CssClass="btn btn-success" OnClick="btnSaveAll_Click" />
<asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
                 <!-- Child Growth Monitoring Section -->
        <hr />
       
         
         
         
         <h3>Child Growth Monitoring</h3>

        <div style="margin-bottom: 10px;">
            <table style="width: 100%; border-collapse: collapse;">
                <tr>

                    <td style="padding: 5px;">Age (months):</td>
                    <td style="padding: 5px;"><asp:TextBox ID="txtAgeInMonths" runat="server" CssClass="input-field" /></td>
                    <td style="padding: 5px;">Height (cm):</td>
                    <td style="padding: 5px;"><asp:TextBox ID="txtHeight" runat="server" CssClass="input-field" /></td>
                    <td style="padding: 5px;">Weight (kg):</td>
                    <td style="padding: 5px;"><asp:TextBox ID="txtWeight" runat="server" CssClass="input-field" /></td>
                    <td style="padding: 5px;">
                        <asp:Button ID="btnSaveGrowth" runat="server" Text="Save Entry" OnClick="btnSaveGrowth_Click" />
                        <asp:Button ID="btnClearGrowth" runat="server" Text="Clear" OnClick="btnClearGrowth_Click" />
                    </td>
                </tr>
            </table>          
        </div>
<asp:Chart ID="chartHeightForAge" runat="server" Width="1600px" Height="1000px" ImageStorageMode="UseImageLocation">
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1" BackColor="#fffbe6">
            <AxisX Title="Age (Months)" Minimum="0" Maximum="60" Interval="2" LineColor="Black">
                <MajorGrid LineColor="#dddddd" />
            </AxisX>
            <AxisY Title="Height (cm)" Minimum="40" Maximum="125" Interval="5" LineColor="Black">
                <MajorGrid LineColor="#dddddd" />
            </AxisY>
            <AxisY2 Title=" " Enabled="True" LineColor="Black" />
        </asp:ChartArea>
    </ChartAreas>
    <Legends>
        <asp:Legend Enabled="true" Docking="Bottom" Alignment="Center" />
    </Legends>
</asp:Chart>


<asp:Chart ID="chartWeightForAge" runat="server" Width="1600px" Height="1000px" ImageStorageMode="UseImageLocation">
    <Series>
        <asp:Series Name="WHO Standard" ChartType="Line" BorderWidth="2" Color="Blue" ChartArea="ChartArea2" />
        <asp:Series Name="Child's Data" ChartType="Line" BorderWidth="2" Color="Red" ChartArea="ChartArea2" />
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea2" BackColor="#f3f9ff">
            <AxisX Title="Age (Months)" Minimum="0" Maximum="60" Interval="2" LineColor="Black">
                <MajorGrid LineColor="#dddddd" />
            </AxisX>
            <AxisY Title="Weight (kg)" Minimum="1" Maximum="30" Interval="1" LineColor="Black">
                <MajorGrid LineColor="#dddddd" />
            </AxisY>
        </asp:ChartArea>
    </ChartAreas>
    <Legends>
        <asp:Legend Enabled="true" Docking="Bottom" Alignment="Center" />
    </Legends>
</asp:Chart>

 
         <div style="clear: both;"></div>

         <!-- INFANT FEEDING Section -->
<hr />
<h3 style="background-color: lightpink; padding: 10px; margin-top: 0;">INFANT FEEDING</h3>
         <table border="1" style="border-collapse: collapse; text-align: center;">
    <tr>
        <th>Follow up time</th>
        <th>Birth</th>
        <th>6W</th>
        <th>10W</th>
        <th>14W</th>
        <th>5M</th>
    </tr>
    <tr>
        <td>Breast milk only</td>
        <td><asp:TextBox ID="txtBirth1" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt6W1" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt10W1" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt14W1" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt5M1" runat="server" CssClass="input-field" /></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="txtBirth2" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt6W2" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt10W2" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt14W2" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt5M2" runat="server" CssClass="input-field" /></td>
    </tr>
</table>
<table border="1" style="border-collapse: collapse; text-align: center; margin-top: 5px;">
    <tr>
        <th>Follow up time</th>
        <th>6M</th><th>7M</th><th>8M</th><th>9M</th><th>10M</th>
        <th>11M</th><th>12M</th><th>13M</th><th>14M</th><th>15M</th>
        <th>16M</th><th>17M</th><th>18M</th><th>19M</th><th>20M</th>
        <th>21M</th><th>22M</th><th>23M</th><th>24M</th>
    </tr>
    <tr>
        <td>Complementary foods</td>
        <td><asp:TextBox ID="txt6M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt7M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt8M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt9M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt10M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt11M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt12M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt13M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt14M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt15M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt16M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt17M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt18M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt19M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt20M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt21M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt22M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt23M_CF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt24M_CF" runat="server" CssClass="input-field" /></td>
    </tr>
    <tr>
        <td>Breastfeeding continues</td>
        <td><asp:TextBox ID="txt6M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt7M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt8M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt9M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt10M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt11M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt12M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt13M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt14M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt15M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt16M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt17M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt18M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt19M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt20M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt21M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt22M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt23M_BF" runat="server" CssClass="input-field" /></td>
        <td><asp:TextBox ID="txt24M_BF" runat="server" CssClass="input-field" /></td>
    </tr>
    <tr>
        <td>Food Groups given/day</td>
        <td colspan="19"><asp:TextBox ID="txtFoodGroups" runat="server" Width="100%" CssClass="input-field" /></td>
    </tr>
    <tr>
        <td>No. of meals/ day</td>
        <td colspan="19"><asp:TextBox ID="txtMealsPerDay" runat="server" Width="100%" CssClass="input-field" /></td>
    </tr>
</table>
<!-- Feeding Code Reference -->
<div style="margin-top: 20px;">
    <strong>Feeding Code:</strong><br />
    1. Breast milk only<br />
    2. Breast milk and Water only<br />
    3. Breast milk and other liquids<br />
    4. Breast milk and solids<br />
    5. Formula only<br />
    6. Other milks
</div>
         <asp:Button ID="btnSaveFeeding" runat="server" Text="Save Infant Feeding" OnClick="btnSaveFeeding_Click" CssClass="btn btn-primary" />

<!-- Care Section -->
<div style="background-color: #ffe6f0; padding: 20px; border-radius: 8px; margin-top: 20px;">
    <h3 style="color: #cc3366;">Care</h3>

    <p>Is the mother on ART? (Tick appropriate box)</p>
    <asp:CheckBox ID="chkMotherOnARTYes" runat="server" Text="Yes" />
    <asp:CheckBox ID="chkMotherOnARTNo" runat="server" Text="No" />

    <p>ARV prophylaxis given at birth? (Tick appropriate box)</p>
    <asp:CheckBox ID="chkARVYes" runat="server" Text="Yes" />
    <asp:CheckBox ID="chkARVNo" runat="server" Text="No" />
    <asp:CheckBox ID="chkARVNA" runat="server" Text="N/A" />

    <p>If yes, specify the ARV Prophylaxis:</p>
    <asp:TextBox ID="txtARVProphylaxis" runat="server" Width="300px" TextMode="MultiLine" Rows="2" />

    <p style="color: red;">Enter initials of the ARV Prophylaxis supplied e.g NVP</p>

    <!-- HIV Test Table -->
    <table border="1" cellpadding="5" cellspacing="0" style="width: 100%; margin-top: 20px;">
        <tr style="background-color: #f2f2f2; font-weight: bold;">
            <th>HIV Test</th>
            <th>Type of Test Used</th>
            <th>Date Test Done</th>
            <th>Test Number</th>
            <th>Test Result (code 0/1)</th>
            <th>Date Referred for ART</th>
            <th>Date Initiated on ART</th>
        </tr>
        <%-- Row: Test 1 --%>
        <tr>
            <td>Test 1</td>
            <td><asp:TextBox ID="txtTest1Type" runat="server" /></td>
            <td><asp:TextBox ID="txtTest1Date" runat="server" /></td>
            <td><asp:TextBox ID="txtTest1Number" runat="server" /></td>
            <td><asp:TextBox ID="txtTest1Result" runat="server" /></td>
            <td><asp:TextBox ID="txtTest1Referred" runat="server" /></td>
            <td><asp:TextBox ID="txtTest1Initiated" runat="server" /></td>
        </tr>
        <%-- Row: Test 2 --%>
        <tr>
            <td>Test 2</td>
            <td><asp:TextBox ID="txtTest2Type" runat="server" /></td>
            <td><asp:TextBox ID="txtTest2Date" runat="server" /></td>
            <td><asp:TextBox ID="txtTest2Number" runat="server" /></td>
            <td><asp:TextBox ID="txtTest2Result" runat="server" /></td>
            <td><asp:TextBox ID="txtTest2Referred" runat="server" /></td>
            <td><asp:TextBox ID="txtTest2Initiated" runat="server" /></td>
        </tr>
        <%-- Row: Test 3 --%>
        <tr>
            <td>Test 3</td>
            <td><asp:TextBox ID="txtTest3Type" runat="server" /></td>
            <td><asp:TextBox ID="txtTest3Date" runat="server" /></td>
            <td><asp:TextBox ID="txtTest3Number" runat="server" /></td>
            <td><asp:TextBox ID="txtTest3Result" runat="server" /></td>
            <td><asp:TextBox ID="txtTest3Referred" runat="server" /></td>
            <td><asp:TextBox ID="txtTest3Initiated" runat="server" /></td>
        </tr>
    </table>
    </div>

   <!-- Follow-up Table -->
<div style="overflow-x: auto; margin-top: 30px;">
    <table class="table table-bordered" border="1" cellpadding="5" cellspacing="0" style="width: max-content; min-width: 100%;">
        <thead>
            <tr style="background-color: #f2f2f2; font-weight: bold;">
                <th>Follow up time</th>
                <th>6W</th><th>10W</th><th>14W</th><th>5M</th><th>7M</th><th>8M</th><th>10M</th><th>11M</th><th>13M</th><th>14M</th>
                <th>15M</th><th>16M</th><th>17M</th><th>18M</th><th>19M</th><th>20M</th><th>21M</th><th>22M</th><th>23M</th><th>24M</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td><b>ARV Supplied</b></td>
                <td><asp:TextBox ID="txtARV_6W" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_10W" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_14W" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_5M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_7M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_8M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_10M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_11M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_13M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_14M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_15M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_16M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_17M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_18M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_19M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_20M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_21M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_22M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_23M" runat="server" /></td>
                <td><asp:TextBox ID="txtARV_24M" runat="server" /></td>
            </tr>
            <tr>
                <td><b>CTX Supplied</b></td>
                <td><asp:TextBox ID="txtCTX_6W" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_10W" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_14W" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_5M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_7M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_8M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_10M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_11M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_13M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_14M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_15M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_16M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_17M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_18M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_19M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_20M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_21M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_22M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_23M" runat="server" /></td>
                <td><asp:TextBox ID="txtCTX_24M" runat="server" /></td>
            </tr>
        </tbody>
    </table>
</div>
         <asp:Button ID="btnSaveCare" runat="server" Text="Save Care Data" OnClick="btnSaveCare_Click" />
    </form>
</body>
</html>