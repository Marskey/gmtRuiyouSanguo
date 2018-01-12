<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchGive.aspx.cs" Inherits="gmt.BatchGive" %>

<!DOCTYPE html>

<html lang="zh-CN">
<head >
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
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="serverTipLabel" runat="server"></asp:Label></td>
                            <td colspan="2">
                                <asp:DropDownList ID="serverList" runat="server" Width="150px" OnSelectedIndexChanged="serverList_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
                        </tr>
                        <tr style="height: 150px">
                            <td style="vertical-align: top">
                                <asp:Label ID="playerIdTipLabel" runat="server"></asp:Label></td>
                            <td colspan="2">
                                <asp:TextBox ID="playerTextBox" runat="server" Width="200px" Height="150px" TextMode="MultiLine"></asp:TextBox></td>
                            <td></td>
                            <td colspan="2">
                                <asp:ListBox ID="playerListBox" runat="server" Width="200px" Height="150px"></asp:ListBox></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:Button ID="playerAddButton" runat="server" Text="" OnClick="playerAddButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" /></td>
                            <td></td>
                            <td colspan="2">
                                <asp:Button ID="playerRemoveButton" runat="server" Text="" OnClick="playerRemoveButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                &nbsp;
                                <asp:Button ID="playerClearButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_clear'))) return;" OnClick="playerClearButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="giftTipLabel" runat="server"></asp:Label></td>
                            <td colspan="2">
                                <asp:DropDownList ID="giftDropDownList" runat="server" Width="150px" OnSelectedIndexChanged="giftDropDownList_SelectedIndexChanged"></asp:DropDownList></td>
                            <td colspan="2">
                                <asp:Label ID="giftLabel" runat="server" ForeColor="Red"></asp:Label></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:Button ID="giftAddButton" runat="server" Text="" OnClick="giftAddButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr style="height: 150px">
                            <td></td>
                            <td colspan="2">
                                <asp:ListBox ID="giftListBox" runat="server" Width="185px" Height="150px"></asp:ListBox>
                            </td>
                            <td></td>
                            <td style="vertical-align: top">
                                <asp:Label ID="reportLabel" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:Button ID="giftRemoveButton" runat="server" Text="" OnClick="giftRemoveButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                &nbsp;
					            <asp:Button ID="giftClearButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_clear'))) return;" OnClick="giftClearButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="giveButton" runat="server" Text="" OnClick="giveButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="compensateButton" runat="server" OnClick="compensateButton_Click" Text="" Visible="false" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                            <td>
                                <asp:DropDownList ID="sourceList" runat="server" Width="150px" Visible="false"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td><b>
                                <asp:Label ID="seniorCodeTipLabel" runat="server"></asp:Label></b></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="inputTipLabel" runat="server"></asp:Label>
                                <asp:TextBox ID="gmTextBox" runat="server"
                                    Width="200"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="gmButton" runat="server" OnClick="gmButton_Click" UseSubmitBehavior="false" Text="" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>

                    </table>
                </form>
            </div>
    </div>
</body>
</html>
