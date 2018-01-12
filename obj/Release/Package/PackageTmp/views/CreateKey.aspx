<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateKey.aspx.cs" Inherits="gmt.CreateKey" %>

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
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="serverTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="serverList" runat="server" Width="150px" OnSelectedIndexChanged="serverList_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="channelTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="channelList" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">
                                <asp:Label ID="expiryDateTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="expiryDateTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                                <asp:Button ID="hideButton" runat="server" OnClick="hideButton_Click" Style="display: none" Width="20px" />
                            </td>
                            <td rowspan="10">
                                <asp:Calendar ID="expirySelectCalendar" runat="server" OnSelectionChanged="expirySelectCalendar_SelectionChanged"
                                    Visible="False" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black"
                                    Height="190px" NextPrevFormat="FullMonth" Width="350px">
                                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                    <OtherMonthDayStyle ForeColor="#999999" />
                                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                                    <TodayDayStyle BackColor="#CCCCCC" />
                                </asp:Calendar>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">
                                <asp:Label ID="countTipLabel" runat="server"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="countTextBox" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="createButton" runat="server" Text="" OnClick="createButton_Click" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="resultLabel" runat="server"></asp:Label>
                </form>
            </div>
    </div>
</body>
</html>
