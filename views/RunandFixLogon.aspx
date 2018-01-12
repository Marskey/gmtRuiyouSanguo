<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RunandFixLogon.aspx.cs" Inherits="gm.RunandFixLogon" %>

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
        <tr><td></td><td></td><td></td><td></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;运维管理登陆界面</td></tr>
    </table>  
    <table> 
        <tr><td>用户名</td><td>
            <asp:TextBox ID="userTextBox" runat="server"  Width="140px"  ></asp:TextBox>
        </td></tr>
        <tr><td>密码</td><td>
		    <asp:TextBox ID="passwordTextBox" runat="server" TextMode="Password" Width="140px"></asp:TextBox>
	    </td></tr>
		<tr><td colspan="2" style="text-align: center">
		    <asp:Button ID="logonButton" runat="server" Text="登录" OnClick="logonButton_Click" />
		</td></tr>

    </table>
    </div>
    <asp:Label ID="outputLabel" runat="server"></asp:Label>
    </form>
</body>
</html>
