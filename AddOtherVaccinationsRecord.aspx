<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOtherVaccinationsRecord.aspx.cs" Inherits="ZimVaxSync.AddOtherVaccinationsRecord" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Other Vaccination Record</title>
    <style>
        body {
            font-family: Arial;
            background: url('background.png') no-repeat center center fixed;
            background-size: cover;
            margin: 0;
            padding: 20px;
        }

        .form-container {
            background-color: #ffffff;
            border-radius: 12px;
            padding: 25px;
            max-width: 700px;
            margin: auto;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        }

        .form-container h2 {
            margin-bottom: 20px;
            color: #004080;
        }

        .field-label {
            margin-top: 12px;
            font-weight: bold;
        }

        .form-control {
            width: 100%;
            padding: 8px;
            margin-top: 5px;
            border-radius: 6px;
            border: 1px solid #ccc;
        }

        .readonly-text {
            background-color: #f3f3f3;
        }

        .btn-submit {
            margin-top: 20px;
            background-color: #007bff;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 8px;
            cursor: pointer;
        }

        .btn-submit:hover {
            background-color: #0056b3;
        }

        .patient-info {
            margin-bottom: 20px;
            padding: 15px;
            background-color: #e9f5ff;
            border-radius: 8px;
        }

        .info-item {
            margin-bottom: 6px;
        }

        .status-message {
            margin-top: 10px;
            color: red;
            font-weight: bold;
        }

        /* Modal styles */
        .modal {
            display: none; 
            position: fixed; 
            z-index: 1000; 
            left: 0;
            top: 0;
            width: 100%; 
            height: 100%; 
            overflow: auto; 
            background-color: rgba(0,0,0,0.5); 
        }

        .modal-content {
            background-color: #fff;
            margin: 15% auto; 
            padding: 20px;
            border-radius: 10px;
            max-width: 400px;
            text-align: center;
            box-shadow: 0 5px 15px rgba(0,0,0,0.3);
        }

        .modal-header {
            font-weight: bold;
            font-size: 1.2em;
            margin-bottom: 10px;
        }

        .modal-body {
            margin-bottom: 20px;
        }

        .modal-footer button {
            padding: 8px 16px;
            border: none;
            border-radius: 6px;
            background-color: #007bff;
            color: white;
            cursor: pointer;
        }

        .modal-footer button:hover {
            background-color: #0056b3;
        }
    </style>
    <script type="text/javascript">
        // Show modal function
        function showModal(id) {
            var modal = document.getElementById(id);
            modal.style.display = "block";
        }

        // Hide modal function
        function hideModal(id) {
            var modal = document.getElementById(id);
            modal.style.display = "none";
        }

        // Close modal on clicking outside the modal content
        window.onclick = function (event) {
            var successModal = document.getElementById("successModal");
            var errorModal = document.getElementById("errorModal");
            if (event.target == successModal) {
                successModal.style.display = "none";
            }
            if (event.target == errorModal) {
                errorModal.style.display = "none";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>Add Other Vaccination Record</h2>

            <!-- Patient  -->
            <div class="patient-info">
                <div class="info-item"><strong>Full Name:</strong> <asp:Label ID="lblFullName" runat="server" /></div>
                <div class="info-item"><strong>Gender:</strong> <asp:Label ID="lblGender" runat="server" /></div>
                <div class="info-item"><strong>Date of Birth:</strong> <asp:Label ID="lblDOB" runat="server" /></div>
                <div class="info-item"><strong>Address:</strong> <asp:Label ID="lblAddress" runat="server" /></div>
                <div class="info-item"><strong>ID Number:</strong> <asp:Label ID="lblIDNumber" runat="server" /></div>
                <div class="info-item"><strong>Phone Number:</strong> <asp:Label ID="lblPhoneNumber" runat="server" /></div>

                <asp:Panel ID="caregiverSection" runat="server" Visible="false">
                    <p><strong>Caregiver Name:</strong> <asp:Label ID="lblCaregiverName" runat="server" /></p>
                    <p><strong>Relationship:</strong> <asp:Label ID="lblRelationship" runat="server" /></p>
                </asp:Panel>

                <asp:Label ID="lblMessage" runat="server" CssClass="status-message" />
            </div>

            <!-- Vaccination Fields -->
            <div class="field-label">Vaccine Name</div>
            <asp:DropDownList ID="ddlVaccineName" runat="server" CssClass="form-control" AutoPostBack="false">
                <asp:ListItem Text="-- Select Vaccine --" Value="" />
                <asp:ListItem Text="COVID-19" Value="COVID-19" />
                <asp:ListItem Text="HEPATITIS A" Value="HEPATITIS A" />
                <asp:ListItem Text="HEPATITIS B" Value="HEPATITIS B" />
                <asp:ListItem Text="TYPHOID" Value="TYPHOID" />
                <asp:ListItem Text="RABIES" Value="RABIES" />
                <asp:ListItem Text="CHIKUNGUNYA" Value="CHIKUNGUNYA" />
                <asp:ListItem Text="YELLOW FEVER" Value="YELLOW FEVER" />
                <asp:ListItem Text="CHOLERA" Value="CHOLERA" />
                <asp:ListItem Text="MENINGITIS ACWY" Value="MENINGITIS ACWY" />
                <asp:ListItem Text="MENINGITIS B" Value="MENINGITIS B" />
            </asp:DropDownList>

            <div class="field-label">Dose Number</div>
            <asp:TextBox ID="txtDoseNumber" runat="server" CssClass="form-control"></asp:TextBox>

            <div class="field-label">Batch Number</div>
            <asp:TextBox ID="txtBatchNumber" runat="server" CssClass="form-control"></asp:TextBox>

            <div class="field-label">Manufacturer</div>
            <asp:TextBox ID="txtManufacturer" runat="server" CssClass="form-control"></asp:TextBox>

            <div class="field-label">Date Given</div>
            <asp:TextBox ID="txtDateGiven" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>

            <div class="field-label">Next Dose Due</div>
            <asp:TextBox ID="txtNextDateDue" runat="server" CssClass="form-control" TextMode="Date" ReadOnly="true"></asp:TextBox>

            <div class="field-label">Administered By</div>
            <asp:TextBox ID="txtAdministeredBy" runat="server" CssClass="form-control readonly-text" ReadOnly="true"></asp:TextBox>

            <div class="field-label">Facility Name</div>
            <asp:TextBox ID="txtFacilityName" runat="server" CssClass="form-control readonly-text" ReadOnly="true"></asp:TextBox>

            <div class="field-label">Comments</div>
            <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>

            <asp:Button ID="btnSave" runat="server" Text="Save Record" CssClass="btn-submit" OnClick="btnSave_Click" />
            <asp:Label ID="Label1" runat="server" CssClass="text-success" />

        </div>
    </form>

    <!-- Success Modal -->
    <div id="successModal" class="modal">
        <div class="modal-content">
            <div class="modal-header">Success</div>
            <div class="modal-body">
                Vaccination record saved and SMS sent successfully.
            </div>
            <div class="modal-footer">
                <button type="button" onclick="hideModal('successModal')">OK</button>
            </div>
        </div>
    </div>

    <!-- Error Modal -->
    <div id="errorModal" class="modal">
        <div class="modal-content">
            <div class="modal-header">Error</div>
            <div class="modal-body">
                An error occurred while sending SMS. Please try again later.
            </div>
            <div class="modal-footer">
                <button type="button" onclick="hideModal('errorModal')">Close</button>
            </div>
        </div>
    </div>

</body>
</html>
