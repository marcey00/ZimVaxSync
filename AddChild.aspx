<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddChild.aspx.cs" Inherits="ZimVaxSync.AddChild" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Child Profile</title>
        <style>
    body, html {
        background-image: url('background.png');
        background-size: cover;
        background-repeat: no-repeat;
        background-position: center;
        margin: 0;
        padding: 0;
        font-family: Arial, sans-serif;
    }

    .form-container {
        width: 75%;
        max-width: 1000px;
        min-width: 300px;
        margin: 30px auto;
        padding: 20px;
        border: 1px solid #ccc;
        border-radius: 12px;
        background-color: #f9f9f9;
        box-sizing: border-box;
    }

    /* Improve mobile layout */
    @media (max-width: 768px) {
        .form-container {
            width: 90%;
            padding: 15px;
        }

        .form-container .buttons {
            flex-direction: column;
            gap: 10px;
        }

        .form-container .buttons input {
            width: 100%;
        }

        .form-container .gender {
            flex-direction: column;
        }
    }

    .form-container h2 {
        text-align: center;
    }

    .form-container label {
        display: block;
        margin-top: 10px;
        font-weight: bold;
    }

    .form-container input[type="text"],
    .form-container input[type="date"] {
        width: 100%;
        padding: 6px;
        border: 1px solid #aaa;
        border-radius: 6px;
    }

    .form-container .gender {
        display: flex;
        gap: 10px;
        margin-top: 5px;
    }

    .form-container .buttons {
        margin-top: 20px;
        display: flex;
        justify-content: space-between;
    }

    .form-container .buttons input {
        width: 48%;
        padding: 8px;
        border: none;
        border-radius: 8px;
        background-color: #007bff;
        color: white;
        font-weight: bold;
        cursor: pointer;
    }

    .form-container .buttons input:hover {
        background-color: #0056b3;
    }
</style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>Add New Child Profile</h2>

            <label for="txtChildName">Child's Full Name:</label>
            <asp:TextBox ID="txtChildName" runat="server" />

            <label for="txtDOB">Date of Birth:</label>
            <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" />

            <label>Gender:</label>
            <div class="gender">
                <asp:RadioButton ID="rdoMale" runat="server" GroupName="Gender" Text="Male" />
                <asp:RadioButton ID="rdoFemale" runat="server" GroupName="Gender" Text="Female" />
            </div>

            <label for="txtIDNumber">National ID Number:</label>
            <asp:TextBox ID="txtIDNumber" runat="server" />

            <label for="txtRelationship">Relationship to Child:</label>
            <asp:TextBox ID="txtRelationship" runat="server" />

            <div class="buttons">
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                <asp:Button ID="btnOK" runat="server" Text="Add Child" OnClick="btnOK_Click" />
            </div>
        </div>
    </form>
</body>
</html>

