<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Passwordchange.aspx.cs" Inherits="gmt.Passwordchange" %>

<!DOCTYPE html>

<html >
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
             <tr><td colspan="6" class="auto-style31"><a href="Login.aspx"><asp:Label ID="returnLoginTipLabel" runat="server"></asp:Label> </a></td></tr>
            <tr><td><asp:Label ID="userTipLabel" runat="server"></asp:Label></td><td>
				<asp:TextBox ID="userTextBox" runat="server" ></asp:TextBox>
				</td></tr>
			<tr><td><asp:Label ID="oldPwdTipLabel" runat="server"></asp:Label></td><td>
				<asp:TextBox ID="oldpassword" runat="server" TextMode="Password"></asp:TextBox>
				</td></tr>
            <tr><td><asp:Label ID="newPwdTipLabel" runat="server"></asp:Label></td><td>
				<asp:TextBox ID="newpassword" runat="server" TextMode="Password"></asp:TextBox>
				</td></tr>

            <tr><td>
                <asp:Button ID="LoginButton" runat="server" Text="" OnClick="LoginButton_Click" />
            </td></tr>
            </table>
            <asp:Label ID="outputLabel" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
