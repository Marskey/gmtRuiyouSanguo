<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileSaveLoad.aspx.cs" Inherits="gm.FileSaveLoad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 309px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table>
			<tr><td colspan="2"><a href="gm.aspx">返回GM工具</a></td></tr>
            <tr>
				<td>选择服务器</td>
				<td class="auto-style1">
					<asp:DropDownList ID="serverList" runat="server" Width="100px" AutoPostBack="True" >
					</asp:DropDownList>
					</td>
			</tr>
            <tr><td>下次活动</td><td class="auto-style1">
				<asp:TextBox ID="nextDateTextBox" runat="server" ReadOnly="True" ></asp:TextBox>
					</td><td rowspan="2" style="vertical-align: top">
				<%--<asp:Calendar ID="nextSelectCalendar" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth"  Visible="True" Width="350px" OnSelectionChanged="nextSelectCalendar_SelectionChanged">
					<DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
					<NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
					<OtherMonthDayStyle ForeColor="#999999" />
					<SelectedDayStyle BackColor="#333399" ForeColor="White" />
					<TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
					<TodayDayStyle BackColor="#CCCCCC" />
				</asp:Calendar>--%>
		   </td></tr>
           <tr><td colspan="2">
				<asp:ListBox ID="nextListBox" runat="server" Width="500px" Height="200px" SelectionMode="Multiple"></asp:ListBox>
		   </td></tr>
           <tr><td>
               <asp:Button ID="RemoveactivityButton"  runat="server"  Text="移除" OnClick="RemoveactivityButton_Click"/>
               <asp:Button ID="LoadactivityButton"   runat="server"   Text="读文件" OnClick="LoadactivityButton_Click"  />
               <asp:Button ID="GetacticityButton"  runat="server"  Text="取出" OnClick="GetacticityButton_Click"  />
           </td></tr>
		   <tr>
			<td>活动名称</td>
			<td colspan="2">
			<asp:TextBox ID="activityname" runat="server" Width="235px" ></asp:TextBox>
            <asp:Button  ID="addnewactivityButton"  runat="server"   Text="添加" OnClick="addnewactivityButton_Click" />
            <asp:Button  ID="UpdateacticityButton"  runat="server"   Text="修改" OnClick="UpdateacticityButton_Click" />
            <asp:Label   ID="ErrotLable"   runat="server" ></asp:Label>
		    </td>
			</tr>
		    <tr>
			<td colspan="2">活动ID
			<asp:TextBox ID="idTextBox"   runat="server"   Width="60px"   ></asp:TextBox>
			</td></tr>
            <tr><td colspan="2">sign
			<asp:TextBox ID="signTextBox" runat="server" Width="60px" ></asp:TextBox>
			</td>
			</tr>
            <tr><td colspan="2">param
			<asp:TextBox ID="paramTextBox" runat="server" Width="60px" ></asp:TextBox>
			</td>
			</tr>
            <tr><td colspan="2">持续时间
			<asp:TextBox ID="lastTextBox" runat="server" Width="60px" ></asp:TextBox>
			&nbsp;</td>
			</tr>
          <tr><td>&nbsp</td></tr>
      </table>
    </div>
    </form>
</body>
</html>
