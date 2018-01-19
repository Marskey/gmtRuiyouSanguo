<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateGift.aspx.cs" Inherits="gmt.CreateGift" %>

<!DOCTYPE html>

<html >
<head id="Head1" runat="server">
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
            <h1 data-lan-id="CreateGift"></h1>
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
                                <asp:DropDownList ID="serverList" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="serverList_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="errorLabel" runat="server" ForeColor="Red"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">
                                <label data-lan-id="Select_Channel"></label></td>
                            <td>
                                <asp:DropDownList ID="channelList" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label data-lan-id="Gift_Type"></label></td>
                            <td>
                                <asp:TextBox ID="typeTextBox" runat="server" AutoPostBack="true" OnTextChanged="typeTextBox_TextChanged"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="Mutitimes" runat="server" Width="100px" AutoPostBack="true" Enabled="false"></asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:Label data-lan-id="Create_Gift_Rule" runat="server" Font-Bold="true" ForeColor="#006600" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label data-lan-id="Select_Coupon"></label></td>
                            <td>
                                <asp:DropDownList ID="giftDropDownList" runat="server" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="giftDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                <%--<asp:TextBox ID="giftTextBox" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>--%>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="giftLabel" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label data-lan-id="Generate_Count"></label></td>
                            <td>
                                <asp:TextBox ID="countTextBox" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label data-lan-id="Expiry_Date"></label></td>
                            <td>
                                <asp:TextBox ID="expiryDateTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <asp:Button ID="hideButton" runat="server" OnClick="hideButton_Click" Style="display: none" Width="20px" />
                            <td rowspan="8">
                                <asp:Calendar ID="expirySelectCalendar" runat="server" OnSelectionChanged="expirySelectCalendar_SelectionChanged" Visible="False" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="150px" NextPrevFormat="FullMonth" Width="250px">
                                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                    <OtherMonthDayStyle ForeColor="#999999" />
                                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                    <TitleStyle BackColor="White" BorderColor="Black" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                                    <TodayDayStyle BackColor="#CCCCCC" />
                                </asp:Calendar>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label data-lan-id="Use_Times"></label></td>
                            <td>
                                <asp:TextBox ID="UseTimes" runat="server" OnTextChanged="UseTimes_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button data-lan-id="Confirm_Create" data-lan-type="text" runat="server" Text="" OnClick="createButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="resultLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </form>
            </div>
    </div>
</body>
</html>
