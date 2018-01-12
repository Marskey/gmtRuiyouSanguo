<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MergeServer.aspx.cs" Inherits="gmt.MergeServer" %>

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
    <script type="text/javascript" src="../js/language.js"></script>
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
                        <table>
                            <tr>
                                <td colspan="2"><a href="GmModify.aspx">
                                    <asp:Label ID="fanhuiGM" runat="server"></asp:Label></a></td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="cong" runat="server"></asp:Label>
                                    <asp:DropDownList ID="fromDropDownList" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="dao" runat="server"></asp:Label>
                                    <asp:DropDownList ID="toDropDownList" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="mergeButton" runat="server" OnClientClick="if (!confirm(GetContentMsg('MergeServer_merge'))) return;" OnClick="mergeButton_Click" Text="" UseSubmitBehavior="False" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="outputLabel" runat="server"></asp:Label>
                    </div>
                </form>
            </div>
    </div>
</body>
</html>
