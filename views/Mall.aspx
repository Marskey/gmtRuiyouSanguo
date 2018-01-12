<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mall.aspx.cs" Inherits="gm.Mall" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	<table>
		<tr><td colspan="2"><a href="GmModify.aspx">返回GM工具</a></td></tr>
	</table>
		<br />
	<table>
		<tr><td><asp:DropDownList ID="randTypeDropDownList" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="randTypeDropDownList_SelectedIndexChanged" Visible="False"></asp:DropDownList></td>
			<td><asp:Label ID="errorLabel" runat="server"></asp:Label></td>
		</tr>
	</table>
    	<br />
    <table>
		<tr><td>位置索引</td><td>商品类型</td><td>物品ID</td><td>数量</td><td>
			<asp:Label ID="costLabel" runat="server" Text="费用"></asp:Label>
			</td><td>
			<asp:Label ID="numberLabel" runat="server" Text="Label"></asp:Label>
			</td><td>限制分组</td></tr>
		<tr><td>
			<asp:TextBox ID="indexTextBox" runat="server" Width="50px"></asp:TextBox>
			</td><td>
			<asp:DropDownList ID="rewardTypeDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rewardTypeDropDownList_SelectedIndexChanged">
			</asp:DropDownList>
			</td><td>
				<asp:DropDownList ID="idDropDownList" runat="server">
				</asp:DropDownList>
			</td><td>
				<asp:TextBox ID="countTextBox" runat="server" Width="50px"></asp:TextBox>
			</td><td>
				<asp:TextBox ID="costTextBox" runat="server" Width="50px"></asp:TextBox><asp:DropDownList ID="costDropDownList" runat="server"></asp:DropDownList>
			</td><td>
				<asp:TextBox ID="limitTextBox" runat="server" Width="80px"></asp:TextBox>
				<asp:TextBox ID="minTextBox" runat="server" Width="40px"></asp:TextBox>
				<asp:TextBox ID="maxTextBox" runat="server" Width="40px"></asp:TextBox>
			</td><td>
				<asp:TextBox ID="groupTextBox" runat="server" Width="35px"></asp:TextBox>
			</td></tr>
	</table>
	<table>
		<tr><td>
			<asp:ListBox ID="configListBox" runat="server" Height="150px" Width="500px" AutoPostBack="True" OnSelectedIndexChanged="configListBox_SelectedIndexChanged"></asp:ListBox>
			</td><td>
				<asp:Button ID="addButton" runat="server" OnClick="addButton_Click" Text="添加" />
				<br />
				<asp:Button ID="modifyButton" runat="server" OnClick="modifyButton_Click" Text="修改" />
				<br />
				<asp:Button ID="deleteButton" runat="server" OnClick="deleteButton_Click" Text="删除" />
				<br />
				<asp:Button ID="sendButton" runat="server" OnClientClick="if (!confirm('确定要发送吗?')) return;" OnClick="sendButton_Click" Text="发送服务器" UseSubmitBehavior="False" />
			</td></tr>
    </table>
    </div>
    </form>
</body>
</html>
