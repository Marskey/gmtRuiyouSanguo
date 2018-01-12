<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityOpenTime.aspx.cs" Inherits="gmt.ActivityOpenTime" %>

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
            <h1 id="titleTipLabel" runat="server" data-lan-id="ActivityOpenTime"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <form id="form1" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="serverTipLabel" runat="server" data-lan-id="Select_Server"></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="OpentimeserverList" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="expiryDateTipLabel" runat="server" data-lan-id="SetDateTime"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="expiryDateTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                                <asp:Button ID="hideButton" runat="server" OnClick="hideButton_Click" Style="display: none" Width="40px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="ActivityOpenButton" runat="server" Text="" UseSubmitBehavior="False" OnClick="ActivityOpenButton_Click" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" data-lan-id="ConfirmSetting" data-lan-type="text" />
                            </td>
                            <td>
                                <asp:Label ID="reportLabel" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="activityOpenListTipLabel" runat="server" data-lan-id="ServerActivityOpenTimeList"></asp:Label></td>
                        </tr>
                        <tr style="height: 500px">
                            <td colspan="8">
                                <asp:ListBox ID="activityOpenTimeListBox" runat="server" Font-Size="Medium" Width="100%" Height="500px"></asp:ListBox>
                                <%--<asp:Label ID="acyivityOpenTimeLabel" runat="server" Width="500px" Height="500px"></asp:Label>--%>
                            </td>
                        </tr>
                    </table>
                    <div class="mr50">
                        <asp:Calendar ID="expirySelectCalendar" runat="server" OnSelectionChanged="expirySelectCalendar_SelectionChanged" Visible="False" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="150px" NextPrevFormat="FullMonth" Width="250px">
                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                            <OtherMonthDayStyle ForeColor="#999999" />
                            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                            <TitleStyle BackColor="White" BorderColor="Black" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                            <TodayDayStyle BackColor="#CCCCCC" />
                        </asp:Calendar>
                    </div>
                </form>
            </div>
    </div>
</body>
</html>
