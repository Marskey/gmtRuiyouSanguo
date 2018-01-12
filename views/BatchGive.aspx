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
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <header class="navbar navbar-static-top bs-docs-nav" id="header" ></header>
    <div class="bs-docs-header" id="content">
        <div class="container">
            <h1 data-lan-id="BatchGive"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <form id="form1" runat="server">
                    <table>
                        <tr>
                            <td>
                                <label data-lan-id="Select_Server"></label></td>
                            <td colspan="2">
                                <asp:DropDownList ID="serverList" runat="server" Width="150px" OnSelectedIndexChanged="serverList_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
                        </tr>
                        <tr style="height: 150px">
                            <td style="vertical-align: top">
                                <label data-lan-id="PlayerHistory_palyer_id"></label></td>
                            <td colspan="2">
                                <asp:TextBox ID="playerTextBox" runat="server" Width="200px" Height="150px" TextMode="MultiLine"></asp:TextBox></td>
                            <td></td>
                            <td colspan="2">
                                <asp:ListBox ID="playerListBox" runat="server" Width="200px" Height="150px"></asp:ListBox></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:Button runat="server" OnClick="playerAddButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" data-lan-id="Add_Player_ID" data-lan-type="text" /></td>
                            <td></td>
                            <td colspan="2">
                                <asp:Button data-lan-id="Delete" data-lan-type="text" runat="server" Text="" OnClick="playerRemoveButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                &nbsp;
                                <asp:Button data-lan-id="Clean" data-lan-type="text" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_clear'))) return;" OnClick="playerClearButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label data-lan-id="Select_Gift"></label></td>
                            <td colspan="2">
                                <asp:DropDownList ID="giftDropDownList" runat="server" Width="150px" OnSelectedIndexChanged="giftDropDownList_SelectedIndexChanged"></asp:DropDownList></td>
                            <td colspan="2">
                                <asp:Label ID="giftLabel" runat="server" ForeColor="Red"></asp:Label></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:Button data-lan-id="Add" data-lan-type="text" runat="server" Text="" OnClick="giftAddButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr style="height: 150px">
                            <td></td>
                            <td colspan="2">
                                <asp:ListBox ID="giftListBox" runat="server" Width="185px" Height="150px" SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td></td>
                            <td style="vertical-align: top">
                                <asp:Label ID="reportLabel" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:Button data-lan-id="Delete" data-lan-type="text" runat="server" Text="" OnClick="giftRemoveButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                &nbsp;
					            <asp:Button data-lan-id="Clean" data-lan-type="text" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_clear'))) return;" OnClick="giftClearButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button data-lan-id="Add_Give" data-lan-type="text" runat="server" Text="" OnClick="giveButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="sourceList" runat="server" Width="150px" Visible="false"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td><b>
                                <label data-lan-id="Senior_Code"></label></b></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label data-lan-id="Input"></label>
                                <asp:TextBox ID="gmTextBox" runat="server"
                                    Width="200"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button data-lan-id="Confirm_Code" data-lan-type="text" runat="server" OnClick="gmButton_Click" UseSubmitBehavior="false" Text="" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>

                    </table>
                </form>
            </div>
    </div>
</body>
</html>
