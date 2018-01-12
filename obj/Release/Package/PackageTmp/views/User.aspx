<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="gmt.User" %>

<!DOCTYPE html>

<html >
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
        .auto-style2 {
            text-align: center;
            width: 436px;
        }
        .auto-style3 {
            width: 436px;
        }
    </style>
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="auto-style1">
		<table>
			<tr><td class="auto-style1"><a href="Login.aspx"><asp:Label ID="returnLoginTipLabel" runat="server"></asp:Label></a></td></tr>
		</table>
		<table><tr><td style="vertical-align: top" class="auto-style1"><table>
			<tr><td class="auto-style1"><asp:Label ID="userNameTipLabel" runat="server"></asp:Label></td><td>
				<asp:TextBox ID="userTextBox" runat="server" Width="80px"></asp:TextBox>
				</td><td class="auto-style1"><asp:Label ID="pwdTipLabel" runat="server"></asp:Label></td><td>
				<asp:TextBox ID="passwordTextBox" runat="server" TextMode="Password" Width="80px"></asp:TextBox>
				</td><td>
					<asp:Button ID="modifyButton" runat="server" Text="" OnClick="modifyButton_Click" />
					<asp:Button ID="addButton" runat="server" Text="" OnClick="addButton_Click" />
				</td></tr>
			<tr>
				<td colspan="5" class="auto-style1">
					<asp:CheckBox ID="modifyCheckBox" runat="server" Text="" />
				&nbsp;&nbsp;
					<asp:CheckBox ID="downloadCheckBox" runat="server" Text="" />
					<asp:CheckBox ID="queryDataCheckBox" runat="server" Text="" />
				</td>
			</tr>
			<tr><td colspan="5">
				<asp:ListBox ID="userListBox" runat="server" Height="150px" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="userListBox_SelectedIndexChanged"></asp:ListBox>
				</td></tr>
			<tr><td colspan="4"></td><td>
				<asp:Button ID="removeButton" runat="server" Text="" OnClick="removeButton_Click" />
				</td></tr>
		</table>
		    <div class="auto-style1">
		<br />
		<asp:Label ID="outputLabel" runat="server"></asp:Label>
            </div>
            </td><td class="auto-style2"><table">
			<tr><td colspan="2">
				<asp:ListBox ID="serverListBox" runat="server" Width="500px" Height="200px" AutoPostBack="True" OnSelectedIndexChanged="serverListBox_SelectedIndexChanged"></asp:ListBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="serverIdTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="idTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="serverNameTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="nameTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="serverGmTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="gmTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="serverGameAddressTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="gameAddressTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="serverGamePortTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="gamePortTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="gameCharsetTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="gameCharsetTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="gameUserTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="gameUserTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="gamePwdTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="gamePasswordTextBox" runat="server" Width="250px" TextMode="Password"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="gameDatabaseTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="gameDatabaseTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="codeAddressTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="codeAddressTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="codePortTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="codePortTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="codeCharsetTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="codeCharsetTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="codeUserTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="codeUserTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="codePwdTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="codePasswordTextBox" runat="server" Width="250px" TextMode="Password"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="codeDatabaseTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="codeDatabaseTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>

			<tr><td class="auto-style1"><asp:Label ID="logAddressTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="logAddressTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="logPortTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="logPortTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="logCharsetTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="logCharsetTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="logUserTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="logUserTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="logPwdTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="logPasswordTextBox" runat="server" Width="250px" TextMode="Password"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="logDatabaseTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="logDatabaseTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>

                			<tr><td class="auto-style1"><asp:Label ID="billAddressTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="billAddressTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="billPortTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="billPortTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="billCharsetTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="billCharsetTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="billUserTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="billUserTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="billPwdTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="billPasswordTextBox" runat="server" Width="250px" TextMode="Password"></asp:TextBox>
				</td></tr>
			<tr><td class="auto-style1"><asp:Label ID="billDatabaseTipLabel" runat="server"></asp:Label></td><td class="auto-style3">
				<asp:TextBox ID="billDatabaseTextBox" runat="server" Width="250px"></asp:TextBox>
				</td></tr>

			<tr><td></td><td class="auto-style3">
				<asp:Button ID="modifyServerButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_modify'))) return;" OnClick="modifyServerButton_Click" UseSubmitBehavior="False" />
				<asp:Button ID="addServerButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_add'))) return;" OnClick="addServerButton_Click" UseSubmitBehavior="False" />
				<asp:Button ID="removeServerButton" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_clear'))) return;" OnClick="removeServerButton_Click" UseSubmitBehavior="False" />
				</td></tr>
		</table></td></tr></table>
    </div>
    </form>
</body>
</html>
