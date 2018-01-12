<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FindNewbieError.aspx.cs" Inherits="gm.FindNewbieError" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<style type="text/css">
		.auto-style1 {
			width: 300px;
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
				<td>
					<asp:DropDownList ID="serverList" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="serverList_SelectedIndexChanged">
					</asp:DropDownList>
				</td>
			</tr>
		</table>
		<br />
		<table>
			<tr><td class="auto-style1">
				<asp:Button ID="findButton" runat="server" Text="查找" OnClick="findButton_Click" />
				</td></tr>
			<tr><td class="auto-style1">
				<asp:Label ID="resultLabel" runat="server"></asp:Label>
				</td></tr>
		</table>
    </div>
    </form>
</body>
</html>
