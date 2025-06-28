<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchResults.aspx.cs" Inherits="ZimVaxSync.SearchResults" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Results</title>
    <style>
        body {
            font-family: Arial;
            margin: 20px;
        }

        h2 {
            color: #007bff;
        }

        .result-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .result-table th, .result-table td {
            border: 1px solid #ddd;
            padding: 8px;
        }

        .result-table th {
            background-color: #f2f2f2;
        }

        .no-results {
            color: #888;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Search Results</h2>
        <asp:Label ID="lblKeyword" runat="server" Font-Bold="true"></asp:Label><br />

        <asp:GridView ID="gvResults" runat="server" CssClass="result-table" AutoGenerateColumns="true" />
        <asp:GridView ID="GridView1" runat="server" CssClass="result-table" AutoGenerateColumns="False" OnRowCommand="gvResults_RowCommand">
    <Columns>
        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
        <asp:BoundField DataField="DOB" HeaderText="Date of Birth" />
        <asp:BoundField DataField="Identifier" HeaderText="ID Number" />
        <asp:BoundField DataField="UserType" HeaderText="Type" />

        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="btnAdd" runat="server" Text="Add Vaccination"
                            CommandName="AddRecord"
                            CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-sm btn-success" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>


        <asp:Label ID="lblNoResults" runat="server" CssClass="no-results" Visible="false" />
    </form>
</body>
</html>

