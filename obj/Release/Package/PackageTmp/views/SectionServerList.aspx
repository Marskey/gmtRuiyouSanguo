<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SectionServerList.aspx.cs" Inherits="gmt.SectionServerList" %>

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
                        <table>
                            <tr>
                                <td colspan="2"><a href="GmModify.aspx">
                                    <asp:Label ID="returnGmModifyTipLabel" runat="server"></asp:Label></a></td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td colspan="2"><a href="SectionServer.aspx">
                                    <asp:Label ID="bianjifuwuqi" runat="server"></asp:Label></a></td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:ListBox ID="channelListBox" runat="server" Height="205px" Width="155px"
                                        AutoPostBack="True" OnSelectedIndexChanged="channelListBox_SelectedIndexChanged"></asp:ListBox>
                                </td>
                                <td style="vertical-align: top">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="daqu" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="nameTextBox" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="qudao" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="channelTextBox" runat="server" Height="150px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: right">
                                                <asp:Button ID="addButton" runat="server" Text="" OnClick="addButton_Click" />
                                                <asp:Button ID="modifyButton" runat="server" Text="" OnClick="modifyButton_Click" />
                                                <asp:Button ID="deleteButton" runat="server" Text="" OnClick="deleteButton_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </form>
            </div>
    </div>
</body>
</html>
