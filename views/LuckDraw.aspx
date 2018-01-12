<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LuckDraw.aspx.cs" Inherits="gm.LuckDraw" %>

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
		<tr><td><asp:DropDownList ID="randTypeDropDownList" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="randTypeDropDownList_SelectedIndexChanged"></asp:DropDownList></td>
			<td><asp:Label ID="errorLabel" runat="server"></asp:Label></td>
		</tr>
	</table>
    	<br />
    <table>
		<tr><td>商品类型</td><td>物品ID</td><td>数量</td><td>
			<asp:Label ID="numberLabel" runat="server" Text="Label"></asp:Label>
			</td><td>
				<asp:Label ID="certainlyLabel" runat="server" Text="必抽"></asp:Label>
			</td></tr>
		<tr><td>
			<asp:DropDownList ID="rewardTypeDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rewardTypeDropDownList_SelectedIndexChanged">
			</asp:DropDownList>
			</td><td>
				<asp:DropDownList ID="idDropDownList" runat="server">
				</asp:DropDownList>
			</td><td>
				<asp:TextBox ID="minCountTextBox" runat="server" Width="50px"></asp:TextBox>
				<asp:TextBox ID="countTextBox" runat="server" Width="50px"></asp:TextBox>
			</td><td>
				<asp:TextBox ID="limitTextBox" runat="server" Width="80px"></asp:TextBox>
				<asp:TextBox ID="minTextBox" runat="server" Width="40px"></asp:TextBox>
				<asp:TextBox ID="maxTextBox" runat="server" Width="40px"></asp:TextBox>
			</td><td>
				<asp:TextBox ID="counterIndexTextBox" runat="server" Width="25px"></asp:TextBox>
				<asp:TextBox ID="counterValueTextBox" runat="server" Width="50px"></asp:TextBox>
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
				<asp:Button ID="sendButton" runat="server" OnClientClick="if (!confirm('确定要发送吗?')) return;" OnClick="sendButton_Click" Text="发送服务器" UseSubmitBehavior="False" />
			</td></tr>
    </table>

    	<asp:TextBox ID="versionTextBox" runat="server" Width="77px"></asp:TextBox>
			<asp:Button ID="sendFileButton" runat="server" OnClientClick="if (!confirm('确定要发送吗?')) return;" Text="直接发送所有表格" OnClick="sendFileButton_Click" UseSubmitBehavior="False" />
			<asp:Button ID="sendButton0" runat="server" OnClientClick="if (!confirm('确定要发送吗?')) return;" Text="发送ZIP包" OnClick="sendButton0_Click" UseSubmitBehavior="False" />
			<asp:Button ID="downloadButton" runat="server" Text="下载ZIP包" OnClientClick="window.open('DownloadCDNTableZip.aspx?version='+document.getElementById('versionTextBox').value, 'newwindow');" />

    	<br />
				<asp:Button ID="downloadRewardButton" runat="server" Text="保存抽奖和商城到本机" OnClientClick="window.open('DownloadReward.aspx', 'newwindow');" />

    	<br />
				上传抽奖和商城<asp:FileUpload ID="rewardFileUpload" runat="server" />

    	<br />
				<asp:Button ID="uploadButton" runat="server" Text="上传" OnClick="uploadButton_Click" />

    </div>
    </form>
</body>
</html>
