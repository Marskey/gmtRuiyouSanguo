<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SectionServer.aspx.cs" Inherits="gmt.SectionServer" %>

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
                                    <asp:Label ID="returnTipLabel" runat="server"></asp:Label></a></td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td colspan="2"><a href="SectionServerList.aspx">
                                    <asp:Label ID="editorChannelTipLabel" runat="server"></asp:Label></a></td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td rowspan="3" style="vertical-align: top">
                                    <asp:ListBox ID="channelListBox" runat="server" Height="455px" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="channelListBox_SelectedIndexChanged"></asp:ListBox>
                                </td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="idTipLabel" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="nameTipLabel" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="ipTipLabel" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="portTipLabel" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="stateTipLabel" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="tuijianTipLabel" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="daquTipLabel" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="serverIdTipLabel" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="paramdTipLabel" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="listTipLabel" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="indexTextBox" runat="server" Width="35px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="nameTextBox" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ipTextBox" runat="server" Width="150px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="portTextBox" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="statusDropDownList" runat="server"></asp:DropDownList></td>
                                            <td>
                                                <asp:CheckBox ID="recommendCheckBox" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="regionTextBox" runat="server" Width="25px"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="idTextBox" runat="server" Width="60px"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="paramTextBox" runat="server" Width="60px"></asp:TextBox></td>
                                            <td>
                                                <asp:CheckBox ID="visibleCheckBox" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="serverListBox" runat="server" Height="400px" Width="600px" AutoPostBack="True" OnSelectedIndexChanged="serverListBox_SelectedIndexChanged"></asp:ListBox>
                                </td>
                                <td style="vertical-align: top">
                                    <asp:Button ID="addButton" runat="server" Text="" OnClick="addButton_Click" /><br />
                                    <asp:Button ID="modifyButton" runat="server" Text="" OnClick="modifyButton_Click" /><br />
                                    <asp:Button ID="deleteButton" runat="server" Text="" OnClick="deleteButton_Click" />
                                    <br />
                                    <asp:Button ID="upButton" runat="server" Text="" OnClick="upButton_Click" />
                                    <br />
                                    <asp:Button ID="downButton" runat="server" Text="" OnClick="downButton_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right">
                                    <asp:Label ID="versionTipLabel" runat="server"></asp:Label><asp:TextBox ID="versionTextBox" runat="server"></asp:TextBox>
                                    <asp:Button ID="sendFileButton" runat="server" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" Text="" OnClick="sendFileButton_Click" UseSubmitBehavior="False" />
                                    <asp:Button ID="sendButton" runat="server" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" OnClick="sendButton_Click" Text="" UseSubmitBehavior="False" />
                                    <asp:Button ID="downloadButton" runat="server" Text="" OnClientClick="window.open('DownloadCDNZip.aspx?version='+document.getElementById('versionTextBox').value, 'newwindow');" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="outputLabel" runat="server" Text=""></asp:Label>
                    </div>
                </form>
            </div>
    </div>
</body>
</html>
