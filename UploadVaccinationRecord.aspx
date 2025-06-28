<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadVaccinationRecord.aspx.cs" Inherits="ZimVaxSync.UploadVaccinationRecord" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Upload Vaccination Record</title>
    <style>
         body {
     font-family: Arial;
     margin: 0;
     padding: 0;
     background-image: url('background.png');
 }
     
        .container {
            max-width: 900px;
            margin: 30px auto;
            padding: 20px;
            background-color: #f4f4f4;
            border-radius: 15px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        h2 {
            text-align: center;
            color: #333;
        }
        .form-field {
            margin-bottom: 15px;
        }
        label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }
        input[type="text"], textarea, input[type="file"] {
            width: 100%;
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #ccc;
        }
        .btn-upload {
            background-color: #007bff;
            color: white;
            padding: 10px 25px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }
        .btn-upload:hover {
            background-color: #0056b3;
        }
        .message {
            font-weight: bold;
            margin-top: 10px;
        }
        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        .gridview th, .gridview td {
            border: 1px solid #ddd;
            padding: 10px;
        }
        .gridview th {
            background-color: #0275d8;
            color: white;
        }
        .btn-delete {
            color: red;
            background: none;
            border: none;
            cursor: pointer;
        }
        .search-bar {
            margin: 20px 0;
            display: flex;
            gap: 10px;
        }
        .search-bar input[type="text"] {
            flex: 1;
        }
        a.view-link {
            color: green;
        }
    </style>
</head>
<body>
    <form id="formUpload" runat="server">
        <div class="container">
            <h2>Upload Vaccination Record</h2>

            <div class="form-field">
                <label for="txtTitle">Title</label>
                <asp:TextBox ID="txtTitle" runat="server" />
            </div>
            <div class="form-field">
                <label for="txtDescription">Description</label>
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="3" runat="server" />
            </div>
            <div class="form-field">
                <label for="fileUpload">Select File</label>
                <asp:FileUpload ID="fileUpload" runat="server" />
            </div>
            <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn-upload" OnClick="btnUpload_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="message" />

            <hr />

            <h2>My Uploaded Records</h2>
            <div class="search-bar">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by title or description..." />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-upload" OnClick="btnSearch_Click" />
            </div>

            <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                AllowPaging="True" PageSize="5" OnPageIndexChanging="gvRecords_PageIndexChanging"
                OnRowCommand="gvRecords_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="UploadDate" HeaderText="Uploaded On" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:TemplateField HeaderText="File">
                        <ItemTemplate>
                            <a class="view-link" href='<%# ResolveUrl(Eval("FilePath").ToString()) %>' target="_blank">View</a>
                            <br />
                            <%# (Eval("FilePath").ToString().EndsWith(".jpg") || Eval("FilePath").ToString().EndsWith(".jpeg") || Eval("FilePath").ToString().EndsWith(".png")) 
                                ? $"<img src='{ResolveUrl(Eval("FilePath").ToString())}' width='100' />" : "" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn-delete"
                                CommandName="DeleteRecord" CommandArgument='<%# Eval("RecordID") %>'
                                OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>

