<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gift.aspx.cs" Inherits="gmt.Gift" %>

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
                                <td>
                                    <asp:Label ID="giftTitleTipLabel" runat="server"></asp:Label></td>
                                <td colspan="2">
                                    <asp:TextBox ID="titleTextBox" runat="server" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="descTipLabel" runat="server"></asp:Label></td>
                                <td colspan="3">
                                    <asp:TextBox ID="descriptionTextBox" runat="server" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">
                                    <asp:Label ID="curGiftTipLabel" runat="server"></asp:Label></td>
                                <td colspan="4">
                                    <asp:ListBox ID="giftListBox" runat="server" Height="150px" Width="400px" AutoPostBack="True" OnSelectedIndexChanged="giftListBox_SelectedIndexChanged"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Button ID="addButton" runat="server" Text="" OnClick="addButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100px" Height="30" Font-Bold="true" />
                                    &nbsp;
                                <asp:Button ID="modifyButton" runat="server" Text="" OnClick="modifyButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100px" Height="30" Font-Bold="true" />
                                    &nbsp;
                                <asp:Button ID="deleteButton" runat="server" Text="" OnClick="deleteButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100px" Height="30" Font-Bold="true" />
                                    &nbsp;
                                <asp:Button ID="sendButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" OnClick="sendButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100px" Height="30" Font-Bold="true" />
                                    &nbsp;
                                <asp:Button ID="sendtableButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_send'))) return;" UseSubmitBehavior="False" OnClick="sendtableButton_Click" ForeColor="White" BackColor="Blue" Width="100px" Height="30" Font-Bold="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="versionTipLabel" runat="server"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="VersionIDText" runat="server" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="downloadGiftButton" runat="server" Text="" OnClientClick="window.open('DownloadGift.aspx', 'newwindow');" UseSubmitBehavior="False" OnClick="sendtableButton_Click" ForeColor="White" BackColor="Blue" Width="150px" Height="30" Font-Bold="true" />
                                </td>
                                <td>
                                    <asp:Label ID="outputLabel" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <asp:Label ID="rewardSetTipLabel" runat="server"></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Label ID="typeTipLabel" runat="server"></asp:Label>
                                    <asp:DropDownList ID="typeDropDownList" runat="server" AutoPostBack="True" Width="150px" OnSelectedIndexChanged="typeDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="goodsIdTipLabel" runat="server"></asp:Label>
                                    <asp:DropDownList ID="idDropDownList" runat="server" Width="150px"></asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="countTipLabel" runat="server"></asp:Label>
                                    <asp:TextBox ID="countTextBox" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ListBox ID="itemListBox" runat="server" Width="300px" Height="150px" AutoPostBack="True" OnSelectedIndexChanged="itemListBox_SelectedIndexChanged"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="modifyItemButton" runat="server" OnClick="modifyItemButton_Click" Text="" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100px" Height="30" Font-Bold="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </form>

            </div>
    </div>
</body>
</html>
