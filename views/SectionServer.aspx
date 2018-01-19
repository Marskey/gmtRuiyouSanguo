<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SectionServer.aspx.cs" Inherits="gmt.SectionServer" %>

<!DOCTYPE html>

<html >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link href="../mycss/docs.min.css" rel="stylesheet" media="screen" />
    <link rel="stylesheet" href="../mycss/style.css" />
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <header class="navbar navbar-static-top bs-docs-nav" id="header" ></header>
    <div class="bs-docs-header" id="content">
        <div class="container">
            <h1 data-lan-id="SectionServer"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <form id="form1" runat="server">
                    <div>
                        <table>
                            <tr>
                                <td colspan="2"><a href="GmModify.aspx" data-lan-id="Return_GMT"></a></td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td colspan="2"><a href="SectionServerList.aspx" data-lan-id="Editor_Channel"></a></td>
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
                                                <label data-lan-id="Number"></label></td>
                                            <td>
                                                <label data-lan-id="ServerData_serverName"></label></td>
                                            <td>
                                                <label data-lan-id="IP"></label></td>
                                            <td>
                                                <label data-lan-id="Port"></label></td>
                                            <td>
                                                <label data-lan-id="PlayerHistory_action"></label></td>
                                            <td>
                                                <label data-lan-id="Recommend"></label></td>
                                            <td>
                                                <label data-lan-id="Area"></label></td>
                                            <td>
                                                <label data-lan-id="PlayerRechargeLook_server_id"></label></td>
                                            <td>
                                                <label data-lan-id="Optional_Param"></label></td>
                                            <td>
                                                <label data-lan-id="List_Can_See"></label></td>
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
                                    <asp:Button ID="addButton" data-lan-id="Add" data-lan-type="text" runat="server" Text="" OnClick="addButton_Click" /><br />
                                    <asp:Button ID="modifyButton" data-lan-id="Modify" data-lan-type="text" runat="server" Text="" OnClick="modifyButton_Click" /><br />
                                    <asp:Button ID="deleteButton" data-lan-id="Delete" data-lan-type="text" runat="server" Text="" OnClick="deleteButton_Click" />
                                    <br />
                                    <asp:Button ID="upButton" data-lan-id="Move_Up" data-lan-type="text" runat="server" Text="" OnClick="upButton_Click" />
                                    <br />
                                    <asp:Button ID="downButton" data-lan-id="Move_Down" data-lan-type="text" runat="server" Text="" OnClick="downButton_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right">
                                    <label data-lan-id="Version"></label><asp:TextBox ID="versionTextBox" runat="server"></asp:TextBox>
                                    <asp:Button ID="sendFileButton" data-lan-id="Send_Server_List" data-lan-type="text" runat="server" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" Text="" OnClick="sendFileButton_Click" UseSubmitBehavior="False" />
                                    <asp:Button ID="sendButton" data-lan-id="Send_ZIP" data-lan-type="text" runat="server" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" OnClick="sendButton_Click" Text="" UseSubmitBehavior="False" />
                                    <asp:Button ID="downloadButton" data-lan-id="Download_ZIP" data-lan-type="text" runat="server" Text="" OnClientClick="window.open('DownloadCDNZip.aspx?version='+document.getElementById('versionTextBox').value, 'newwindow');" />
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
