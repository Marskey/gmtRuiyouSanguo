<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FTPEdit.aspx.cs" Inherits="gmt.FTPEdit" %>

<!DOCTYPE html>

<html >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link href="../mycss/docs.min.css" rel="stylesheet" media="screen" />
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
</head>
<body>
    <header class="navbar navbar-static-top bs-docs-nav" id="header" ></header>
    <div class="bs-docs-header" id="content">
        <div class="container">
            <h1 id="titleTipLabel" runat="server"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <form id="form1" runat="server">
                    <div>
                        <br />
                        <br />
                        <asp:Table ID="ftpTable" runat="server">
                        </asp:Table>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="addButton" runat="server" Text="+" OnClick="addButton_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="saveButton" runat="server" Text="" OnClick="saveButton_Click" ForeColor="White" BackColor="Blue" Width="81px" Height="30px" Font-Bold="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </form>
            </div>
    </div>
</body>
</html>
