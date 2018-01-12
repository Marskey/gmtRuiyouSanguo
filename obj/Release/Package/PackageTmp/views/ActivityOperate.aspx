<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityOperate.aspx.cs" Inherits="gmt.ActivityOperate" %>

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
                    <table style="width: 1000px">
                        <tr>
                            <td>
                                <asp:Label ID="serverTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="serverList" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="serverList_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="versionTextBox" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="sendButton" runat="server" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" Text="" OnClick="sendButton_Click1" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                <asp:Button ID="downloadButton" runat="server" Text="" OnClientClick="window.open('DownloadCDNTableZip.aspx?version='+document.getElementById('versionTextBox').value, 'newwindow');" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="activityTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="ActivityTitle" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="ErrorLable" runat="server" visable="true" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="activityContentTipLabel" runat="server"></asp:Label></td>
                            <td colspan="2">
                                <asp:TextBox ID="TitleContentText" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top">
                                <asp:Label ID="contentTipLabel" runat="server"></asp:Label></td>
                            <td colspan="4">
                                <asp:ListBox ID="AllContentBox" runat="server" Width="500px" Height="100px" SelectionMode="Multiple"></asp:ListBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="contentDescTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="ContentNumText" runat="server" Width="150px"></asp:TextBox>
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="overlappingDisplayCheckBox" runat="server" TextAlign="Left" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="activityTypeTipLabel" runat="server"></asp:Label></td>
                            <td colspan="7">
                                <asp:DropDownList ID="TypeList" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;<asp:Label ID="activityTypeTipLabel1" runat="server"></asp:Label>
                                <asp:DropDownList ID="Opt_type_1_list" runat="server" AutoPostBack="True" Width="150px"></asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;<asp:Label ID="activityOptionTipLabel" runat="server"></asp:Label>
                                <asp:TextBox ID="Opt_value_1_Text" runat="server" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="showOrderTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="SortValue_Text" runat="server" Width="150px"></asp:TextBox>
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="ShowCheckBox" runat="server" OnCheckedChanged="UseCheckBox_CheckedChanged" TextAlign="Left" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td><b>
                                <asp:Label ID="rewardTipLabel" runat="server"></asp:Label></b></td>
                            <td colspan="3" style="color: red;">
                                <asp:Label ID="rewardTipTipLabel" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="goodsTypeTipLabel" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="goodsTypeList" runat="server" OnSelectedIndexChanged="goodsTypeList_SelectedIndexChanged" AutoPostBack="True" Width="150px"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="goodsIdTipLabel" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="goodsIDList" runat="server" AutoPostBack="True" Width="150px"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="goodsNumTipLabel" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="goodsNumText" runat="server" MaxLength="9999" Width="150px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="AddButton" runat="server" Text="" OnClick="AddButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top">
                                <asp:Label ID="rewardPreTipLabel" runat="server"></asp:Label></td>
                            <td colspan="2">
                                <asp:ListBox ID="AllRewardBox" runat="server" Width="400px" Height="100px" SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td colspan="6">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="DeleteoneButton" runat="server" Text="" OnClick="DeleteoneButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="DeleteButton" runat="server" Text="" OnClick="DeleteButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                        </td>
                                        <td style="color: red; vertical-align: bottom; text-align: center">
                                            <pre><asp:Label ID="editorTiplabel" runat="server"></asp:Label></pre>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="CommietButton" runat="server" Text="" OnClick="CommietButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                        </td>
                                        <td>
                                            <asp:Button ID="SaveButton" runat="server" Text="" OnClick="SaveButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="150px" Height="30" Font-Bold="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <hr style="width: 99%; border: 2px dashed #000" />
                    <table>
                        <tr>
                            <td style="vertical-align: top">
                                <asp:Label ID="alreadyTipLabel" runat="server"></asp:Label></td>
                            <td colspan="2">
                                <asp:ListBox ID="ActivityListBox" runat="server" Width="300px" Height="100px"></asp:ListBox>
                            </td>
                            <td style="vertical-align: top">
                                <asp:Label ID="detailTipLabel" runat="server"></asp:Label></td>
                            <td colspan="2">
                                <asp:ListBox ID="DetailmessageListBox" runat="server" Width="300px" Height="100px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Button ID="DeleteactivityButton" runat="server" Text="" OnClick="DeleteactivityButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="150px" Height="30" Font-Bold="true" />
                                <asp:Button ID="DetailamessageButton" runat="server" Text="" OnClick="DetailamessageButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="150px" Height="30" Font-Bold="true" />
                                <asp:Button ID="CleanButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_clean')))  {return;} " OnClick="CleanButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="150px" Height="30" Font-Bold="true" />
                                <asp:Button ID="SinggleSendButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" OnClick="SinggleSendButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="150px" Height="30" Font-Bold="true" />
                                <asp:Button ID="downloadActivityButton" runat="server" Text="" OnClientClick="window.open('DownloadActivity.aspx', 'newwindow');" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="150px" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="ActivityDeleteLable" runat="server" visable="true" ForeColor="Red"></asp:Label></td>
                        </tr>
                    </table>
                </form>
            </div>
    </div>
</body>
</html>
