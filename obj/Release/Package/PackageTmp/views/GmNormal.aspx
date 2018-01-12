<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GmNormal.aspx.cs" Inherits="gmt.GmNormal" %>
<!DOCTYPE html>

<html >
<head id="Head1" runat="server">
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
                <form class="form-horizontal" runat="server">
                    <table>
                            <tr>
                                <td><b>
                                    <asp:Label ID="versionTipLabel" runat="server"></asp:Label></b>
                                    <asp:Label ID="versionLabel" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <table>
                                        <tr>
                                            <td><b>
                                                <asp:Label ID="serverTipLabel" runat="server"></asp:Label></b></td>
                                            <td colspan="3">
                                                <asp:Label ID="errorLabel" runat="server" ForeColor="Red"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td rowspan="4">
                                                <asp:ListBox ID="serverListBox" runat="server" SelectionMode="Multiple" Height="200px" Width="150px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="addAllButton" runat="server" Text="" OnClick="addAllButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="addServerButton" runat="server" Text="" OnClick="addServerButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="removeServerButton" runat="server" Text="" OnClick="removeServerButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="removeAllButton" runat="server" Text="" OnClick="removeAllButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td rowspan="4">
                                                <asp:ListBox ID="selectListBox" runat="server" SelectionMode="Multiple" Height="200px" Width="150px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1"><b>
                                    <asp:Label ID="searchPlayerTipLabel" runat="server"></asp:Label></b></td>
                                <td style="color: red" colspan="7">
                                    <asp:Label ID="searchTipLabel" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="playerNameTipLabel" runat="server"></asp:Label>
                                    <asp:TextBox ID="playerNameTextBox" runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="uidTipLabel" runat="server"></asp:Label>
                                    <asp:TextBox ID="uidTextBox"
                                        runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="cyidTipLabel" runat="server"></asp:Label>
                                    <asp:TextBox ID="cyidTextBox" runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="findIdButton" runat="server" OnClick="findIdButton_Click" Text="" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                </td>
                                <td>
                                    <asp:Button ID="informationButton" runat="server" Text="" OnClick="informationButton_Click" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Label ID="idLabel" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Button ID="shutupButton" runat="server" OnClick="shutupButton_Click" Text="" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                    &nbsp;<asp:Button ID="notShutupButton" runat="server" OnClick="notShutupButton_Click" Text="" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                    &nbsp;<asp:Button ID="banButton" runat="server" OnClientClick="if (!confirm(GetContentMsg('GmNormal_ban')))  return;" OnClick="banButton_Click" Text="" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                    &nbsp;<asp:Button ID="unbanButton" runat="server" OnClientClick="if (!confirm(GetContentMsg('GmNormal_unBan'))) return;" OnClick="unbanButton_Click" Text="" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                    &nbsp;<asp:Button ID="kickButton" runat="server" OnClick="kickButton_Click" Text="" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                    &nbsp;<asp:Button ID="rechargeButton" runat="server" OnClick="rechargeButton_Click" Text="" Visible="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                    &nbsp;<asp:Button ID="downloadLogButton" runat="server" Text="" OnClientClick="window.open('DownloadLog.aspx', 'newwindow');" ForeColor="White" BackColor="Blue" Width="150" Height="30" Font-Bold="true" />
                                </td>
                            </tr>
                        </table>
                </form>
            </div>
    </div>
</body>
</html>
