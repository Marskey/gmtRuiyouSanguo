<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoticeEdit.aspx.cs" Inherits="gmt.NoticeEdit" %>

<!DOCTYPE html>

<html >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <header id="header" class="navbar navbar-static-top" style="margin-top: 0; margin-bottom: 0; border-bottom: 0; background: #337ab7"> </header>
    <div class="father">
        <fieldset>
            <legend>
                <asp:Label ID="titleTipLabel" runat="server"></asp:Label></legend>
            <form id="form1" runat="server">
                <div>
                    <table>
                        <tr>
                            <td colspan="2"><a href="GmModify.aspx">
                                <asp:Label ID="returnTipLabel" runat="server"></asp:Label></a></td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="errorLabel" runat="server"></asp:Label>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="noticeTitleTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="titleTextBox" runat="server" Width="350px"></asp:TextBox>
                            </td>
                            <td rowspan="5">
                                <table>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="addButton" runat="server" Text="" OnClick="addButton_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="modifyButton" runat="server" Text="" OnClick="modifyButton_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="deleteButton" runat="server" Text="" OnClick="deleteButton_Click" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Button ID="sendButton" runat="server" Text="" OnClick="sendButton_Click" Visible="False" /></td>
                                    </tr>
                                </table>
                            </td>
                            <td rowspan="3">
                                <table>
                                    <tr>
                                        <td style="color: #FF0000">[ff0000][-]</td>
                                    </tr>
                                    <tr>
                                        <td style="color: #00FF00">[00ff00][-]</td>
                                    </tr>
                                    <tr>
                                        <td style="color: #0000FF">[0000ff][-]</td>
                                    </tr>
                                    <tr>
                                        <td style="color: #FFFF00">[ffff00][-]</td>
                                    </tr>
                                    <tr>
                                        <td style="color: #00FFFF">[00ffff][-]</td>
                                    </tr>
                                    <tr>
                                        <td style="color: #FF00FF">[ff00ff][-]</td>
                                    </tr>

                                </table>
                            </td>
                            <td rowspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="jieriTipLabel" runat="server"></asp:Label></td>
                                        <td>8201</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="cuxiaoTipLabel" runat="server"></asp:Label></td>
                                        <td>8202</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="huodongTipLabel" runat="server"></asp:Label></td>
                                        <td>8203</td>
                                    </tr>
                                    <tr>
                                        <td>NEW</td>
                                        <td>8204</td>
                                    </tr>
                                    <tr>
                                        <td>HOT</td>
                                        <td>8205</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="gonggaoTipLabel" runat="server"></asp:Label></td>
                                        <td>8207</td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="contentTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="contentTextBox" runat="server" Width="350px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="bindIdTipLabel" runat="server"></asp:Label><asp:TextBox ID="activityTextBox" runat="server" Width="45px"></asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="icon1TextBox" runat="server" Width="50px" Visible="false"></asp:TextBox>
                                &nbsp;
                                <asp:TextBox ID="icon2TextBox" runat="server" Width="50px" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:ListBox ID="noticeListBox" runat="server" Height="150px" Width="400px" AutoPostBack="True" OnSelectedIndexChanged="noticeListBox_SelectedIndexChanged"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="versionTextBox" runat="server" Width="77px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="sendFileButton" runat="server" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" Text="" OnClick="sendFileButton_Click" UseSubmitBehavior="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="sendButton0" runat="server" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" Text="" OnClick="sendButton0_Click" UseSubmitBehavior="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="downloadButton" runat="server" Text="" OnClientClick="window.open('DownloadCDNTableZip.aspx?version='+document.getElementById('versionTextBox').value, 'newwindow');" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="downloadNoticeButton" runat="server" Text="" OnClientClick="window.open('DownloadNotice.aspx', 'newwindow');" />
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label ID="upLoadTipLabel" runat="server"></asp:Label><asp:FileUpload ID="noticeFileUpload" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="uploadButton" runat="server" Text="" OnClick="uploadButton_Click" />
                            </td>
                        </tr>
                    </table>


                </div>
            </form>
        </fieldset>
    </div>
</body>
</html>
