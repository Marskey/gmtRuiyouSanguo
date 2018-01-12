<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysNtfConfig.aspx.cs" Inherits="gm.SysNtfConfig" %>

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
            <tr><td colspan="2"><a href="gm.aspx">返回GM工具</a></td></tr>
             <tr><td>SysNtfConfig表内容</td></tr>
             <tr><td colspan="2">
				<asp:ListBox ID="SysNtfListBox" runat="server" Width="500px" Height="200px" SelectionMode="Multiple" OnSelectedIndexChanged="SysNtfListBox_SelectedIndexChanged"></asp:ListBox>
		     </td></tr>
             <tr><td>
               <asp:Button ID="LoadBtn"    runat="server"   Text="读文件" OnClick="LoadBtn_Click"   />
               <asp:Button ID="RemoveBtn"  runat="server"   Text="移除" OnClick="RemoveBtn_Click" />
               <asp:Button ID="GetBtn"     runat ="server"  Text="取出" OnClick="GetBtn_Click"   />
             </td></tr>
            <tr>
			<td>SysNtf名称</td>
			<td colspan="2">
			<asp:TextBox ID="SysNtfname" runat="server" Width="235px" ></asp:TextBox>
            <asp:Button  ID="AddBtn"  runat="server"   Text="添加" OnClick="AddBtn_Click"  />
            <asp:Button  ID="UpdateBtn"  runat="server"   Text="修改" OnClick="UpdateBtn_Click" />
            <asp:Label   ID="TipLabel"   runat="server" ></asp:Label>
		    </td>
			</tr>
            <tr><td>ID</td>
			<td colspan="2">
			<asp:TextBox ID="SysNtfID" runat="server" Width="100px" ></asp:TextBox>
		    </td></tr>
            <tr><td>
               <asp:Label   ID="ErrorLable"   runat="server" ></asp:Label>
            </td></tr>
        </table>
    
    </div>
    </form>
</body>
</html>
