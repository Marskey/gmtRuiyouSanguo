<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Activity.aspx.cs" Inherits="gmt.Activity" %>

<!DOCTYPE html>

<html>
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
            <h1 data-lan-id="Activity"></h1>
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
                                <asp:DropDownList ID="serverList" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="serverList_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="refresh" runat="server" Text="" ForeColor="White" BackColor="Blue" Width="81px" Height="30px" Font-Bold="true" OnClientClick="return confirm(GetContentMsg('Activity_Tip_refresh'));" OnClick="refresh_Click" data-lan-id="Refresh" data-lan-type="text" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="curDateTipLabel" runat="server" data-lan-id="Cur_Activity_Start_Time"></asp:Label>
                                <asp:TextBox ID="curDate" runat="server" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td></td>
                            <td colspan="4">
                                <asp:Label ID="nextDateTipLabel" runat="server" data-lan-id="Next_Activity_Start_Time"></asp:Label>
                                <asp:TextBox ID="nextDateTextBox" runat="server" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:ListBox ID="currentListBox" runat="server" Width="480px" Height="200px" CssClass="auto-style4"></asp:ListBox>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:ListBox ID="nextListBox" runat="server" Height="200px" Width="480px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="addCurActivityButton" runat="server" Text="" ForeColor="White" BackColor="Blue" Width="81px" Height="30px" Font-Bold="true" OnClick="addActivityButton_Click" data-lan-id="Add" data-lan-type="text" />
                                <asp:Button ID="delCurActivityButton" runat="server" Text="" ForeColor="White" BackColor="Blue" Width="81px" Height="30px" Font-Bold="true" OnClick="delCurActivityButton_Click" data-lan-id="Delete" data-lan-type="text" />
                                <asp:Button ID="cleanCurActivityButton" runat="server" Text="" ForeColor="White" BackColor="Blue" Width="81px" Height="30px" Font-Bold="true" OnClick="cleanCurActivityButton_Click" data-lan-id="Clean" data-lan-type="text" />
                                <asp:Button ID="cleanPlayerDataButton" runat="server" Text="" OnClientClick="return confirm(GetContentMsg('Activity_Tip_clean'));" ForeColor="White" BackColor="Blue" Width="200" Height="30px" Font-Bold="true" OnClick="cleanPlayerDataButton_Click" data-lan-id="Clean_Player_Data" data-lan-type="text" />
                            </td>
                            <td></td>
                            <td colspan="4">
                                <asp:Button ID="addNextActivityButton" runat="server" Text="" ForeColor="White" BackColor="Blue" Width="81px" Height="30px" Font-Bold="true" OnClick="addActivityButton_Click" data-lan-id="Add" data-lan-type="text" />
                                <asp:Button ID="delNextActivityButton" runat="server" Text="" ForeColor="White" BackColor="Blue" Width="81px" Height="30px" Font-Bold="true" OnClick="delNextActivityButton_Click" data-lan-id="Delete" data-lan-type="text" />
                                <asp:Button ID="cleanNextActivityButton" runat="server" Text="" ForeColor="White" BackColor="Blue" Width="81px" Height="30px" Font-Bold="true" OnClick="cleanNextActivityButton_Click" data-lan-id="Clean" data-lan-type="text" />
                                <asp:Button ID="addOpenActivityButton" runat="server" Text="" ForeColor="White" BackColor="Blue" Width="200" Height="30px" Font-Bold="true" OnClick="addOpenActivityButton_Click" data-lan-id="Add_Open_Activity" data-lan-type="text" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="expiryDateTipLabel" runat="server" data-lan-id="Set_Next_Activity_Time"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="expiryDateTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Calendar ID="nextSelectCalendar" runat="server" OnSelectionChanged="nextSelectCalendar_SelectionChanged" Visible="False" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="150px" NextPrevFormat="FullMonth" Width="250px">
                                    <DayHeaderStyle Font-Size="8pt" />
                                    <NextPrevStyle Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                    <OtherMonthDayStyle ForeColor="#999999" />
                                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                    <TitleStyle BackColor="White" BorderColor="Black" Font-Size="12pt" ForeColor="#333399" />
                                    <TodayDayStyle BackColor="#CCCCCC" />
                                </asp:Calendar>
                            </td>
                            <td>
                                <asp:Button ID="updateTimeButton" runat="server" Text="" OnClick="updateTimeButton_Click" Visible="false" data-lan-id="ConfirmDate" data-lan-type="text" />
                                <asp:Button ID="hideButton" runat="server" OnClick="hideButton_Click" Style="display: none" Width="20px" />
                            </td>
                            <td>
                                <asp:Button ID="closeTimeButton" runat="server" Text="" Visible="false" OnClick="closeTimeButton_Click" data-lan-id="Cancel" data-lan-type="text" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="errorLabel" runat="server" BorderStyle="None" ForeColor="#CC0000"></asp:Label></td>
                        </tr>
                        <tr>
                            <%--                            <td colspan="4">活动内容</td>
                            <td></td>--%>
                            <td colspan="4">
                                <asp:Label ID="curActivityTipLabel" runat="server" data-lan-id="Cur_Activity"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="durationTipLabel" runat="server" data-lan-id="Duration"></asp:Label><asp:TextBox ID="durationTextBox" runat="server" Width="20px">1</asp:TextBox><asp:Label ID="dayTipLabel" runat="server" data-lan-id="Day"></asp:Label>
                                <asp:Label ID="delayTipLabel" runat="server" data-lan-id="Delay"></asp:Label><asp:TextBox ID="delayTextBox" runat="server" Width="20px">0</asp:TextBox><asp:Label ID="dayTipLabel1" runat="server" data-lan-id="Day"></asp:Label>
                                <asp:Label ID="paramTipLabel" runat="server" data-lan-id="Param"></asp:Label><asp:TextBox ID="paramTextBox" runat="server" Width="20px">0</asp:TextBox>
                                <asp:CheckBox ID="forverCheckBox" runat="server" Text="" TextAlign="Left" data-lan-id="Forever" data-lan-type="text" />
                                <%--                                <asp:Button ID="addButton" runat="server" OnClick="addButton_Click" Text="添加" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                <asp:Button ID="removeButton" runat="server" OnClick="removeButton_Click" Text="删除" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />--%>
                            </td>
                        </tr>
                        <tr>
                            <%--                            <td colspan="4">
                                <asp:ListBox ID="addListBox" runat="server" Width="480px" Height="200px"></asp:ListBox>
                            </td>
                            <td></td>--%>
                            <td colspan="7">
                                <asp:ListBox ID="allListBox" runat="server" Width="100%" Height="200px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <%--                            <td colspan="5">
                                <asp:Button ID="immediateButton" runat="server" OnClick="immediateButton_Click" Text="添加" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="80" Height="30" Font-Bold="true" />
                                <asp:Button ID="Button_add_activity" runat="server" OnClick="Button_add_activity_Click" Text="追加" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="80" Height="30" Font-Bold="true" />
                                <asp:Button ID="Button1" runat="server" Text="立即生效" OnClick="Button1_Click" CssClass="auto-style9" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                <asp:Button ID="CleanNext" runat="server" Text="清空下次活动" OnClick="CleanNext_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="90" Height="30" Font-Bold="true" />
                                <asp:Button ID="normalButton" runat="server" Text="添加开服活动" OnClick="normalButton_Click" Visible="true" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="serverTipLabel1" runat="server" data-lan-id="Select_Server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:ListBox ID="serverListBox" runat="server" Height="200px" Width="400px" SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="addAllButton" runat="server" Text="" OnClick="addAllButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" data-lan-id="AddAll" data-lan-type="text" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="addSelectButton" runat="server" Text="" OnClick="addSelectButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" data-lan-id="AddSelect" data-lan-type="text" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="removeSelectButton" runat="server" Text="" OnClick="removeSelectButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" data-lan-id="RemoveSelect" data-lan-type="text" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="removeAllButton" runat="server" Text="" OnClick="removeAllButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" data-lan-id="RemoveAll" data-lan-type="text" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="3">
                                <asp:ListBox ID="selectListBox" runat="server" Height="200px" Width="400px" SelectionMode="Multiple"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="applyButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Activity_Tip_apply'))) return;" OnClick="applyButton_Click" data-lan-id="ApplyActivity" data-lan-type="text" />
                            </td>
                            <%--                            <td colspan="9">
                                <asp:Button ID="copyButton" runat="server" Text="保存活动和时间到以上服务器" OnClientClick="if (!confirm('确定要保存吗?这会覆盖这些服务器原来的设置')) return;" OnClick="copyButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="200" Height="30" Font-Bold="true" />
                                <asp:Button ID="batchUpdateTimeButton" runat="server" Text="立即更新以上服务器时间" OnClientClick="if (!confirm('确定要更新活动时间吗?')) return;" OnClick="batchUpdateTimeButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="200" Height="30" Font-Bold="true" />
                                <asp:Button ID="batchUpdateActivityButton" runat="server" Text="立即更新以上服务器活动" OnClientClick="if (!confirm('确定要更新活动吗?')) return;" OnClick="batchUpdateActivityButton_Click" UseSubmitBehavior="False" ForeColor="White" BackColor="Blue" Width="200" Height="30" Font-Bold="true" />
                                <asp:Button ID="ResetThreeYuanbaoButton" runat="server" Text="重置充值" OnClientClick="if (!confirm('确定重置充值返三倍元宝吗?')) return;" UseSubmitBehavior="False" OnClick="ResetThreeYuanbaoButton_Click" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                <asp:Button ID="ResetVipGiftBuyButton" runat="server" Text="重置礼包" OnClientClick="if (!confirm('确定重置vip礼包购买吗?')) return;" UseSubmitBehavior="False" OnClick="ResetVipGiftBuyButton_Click" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>--%>
                        </tr>
                    </table>
                </form>
            </div>

    </div>
</body>
</html>
