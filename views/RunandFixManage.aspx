G<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RunandFixManage.aspx.cs" Inherits="gmt.RunandFixManage" %>

<!DOCTYPE html>

<html >
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 29px;
        }
        .auto-style2 {
            height: 25px;
        }
    </style>
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
        <tr><td colspan="2"><a href="GmModify.aspx" data-lan-id="Return_GMT"></a></td></tr>
          <tr><td class="auto-style2"><label data-lan-id="Look_Server"></label></td><td class="auto-style2">
              <asp:DropDownList ID="Serverlist"   runat="server"   Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ServerList_SelectedIndexChanged"></asp:DropDownList>
          </td></tr>
        </table>
         <table>
            <tr><td><label data-lan-id="Please_Select_Channel"></label></td><td>
                <asp:DropDownList ID="ChannelList"  runat="server"  OnSelectedIndexChanged="ChannelList_SelectedIndexChanged"  Width="100px" AutoPostBack="True"></asp:DropDownList>
            </td></tr>


        </table>

        <table>
            <tr><td><label data-lan-id="Start_With_Server"></label></td><td>
                <asp:DropDownList ID="Checkservers"       runat="server"  Width="100px" AutoPostBack="True"  OnSelectedIndexChanged="Checkservers_SelectedIndexChanged"></asp:DropDownList></td>
                <td></td><td></td><td><label data-lan-id="PlayerHistory_action"></label></td><td><asp:TextBox   ID="ServerStateTextBox"   runat="server"  Width="100px"    style="height: 19px"></asp:TextBox></td> 
                <td></td><td><asp:Button ID="SeeServerState" data-lan-id="Look" data-lan-type="text" runat="server"  Text=""  OnClick="SeeServerState_Click" /></td>         
            </tr>
        </table>

        <table>
            <tr><td><label data-lan-id="Imitate_Server"></label></td><td>
            <asp:DropDownList ID="ImitateloginServer" runat="server"   Width="100px"  OnSelectedIndexChanged="ImitateloginServer_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
            <td></td><td></td><td><label data-lan-id="Return_Result"></label></td><td><asp:TextBox ID="CheckCannotlogin" runat="server" ></asp:TextBox></td>
            </tr>
        </table>

		<table>
        <tr><td></td><td></td></tr>
		<tr><td><label data-lan-id="White_List_UID"></label></td>
            <td><asp:CheckBox ID="AllServerCheckBox" data-lan-id="All_Server" data-lan-type="text" runat="server" Text=""  OnCheckedChanged="AllServerCheckBox_CheckedChanged1" AutoPostBack="True" />
		</td></tr>
            
			<tr><td colspan="2">
				<asp:TextBox ID="WhiteNameTextBox" runat="server" Width="180px" Height="150px" TextMode="MultiLine" OnTextChanged="WhiteNameTextBox_TextChanged"></asp:TextBox>
				</td><td>
					<asp:Button ID="WhiteNameAddButton" data-lan-id="Add" data-lan-type="text" runat="server" Text="" OnClick="WhiteNameAddButton_Click" />
		    </td></tr>
            <tr><td></td><td></td></tr>


            <tr><td colspan="2">
				<asp:ListBox ID="WhiteNameListBox" runat="server" Width="180px" Height="150px" OnSelectedIndexChanged="WhiteNameListBox_SelectedIndexChanged"></asp:ListBox>
				</td><td>
					<asp:Button ID="WhiteNameRemoveButton" data-lan-id="Delete" data-lan-type="text" runat="server" Text="" OnClick="WhiteNameRemoveButton_Click" />
					<br />
					<asp:Button ID="WhiteNameClearButton" data-lan-id="Clean" data-lan-type="text" runat="server" Text="" OnClientClick="if (!confirm(GetContentMsg('Tip_clear')))  {return;} " OnClick="WhiteNameClearButton_Click" UseSubmitBehavior="False" />
				</td></tr>
                <tr><td></td><td></td></tr>
		</table>
        <table>
			<tr><td><label data-lan-id="Max_Online_Count"></label></td><td>
				<asp:TextBox ID="MaxOnlineTextBox" runat="server" OnTextChanged="MaxOnlineTextBox_TextChanged" Width="120px" AutoPostBack="True"></asp:TextBox>
		     </td></tr>	
		</table>
         <table>
            <tr><td><label data-lan-id="Client_Resource_Download_Url"></label></td><td>
                <asp:TextBox  ID="URLOfClientResourceTextBox"  runat="server"  OnTextChanged="URLOfClientResourceTextBox_TextChanged" Width="250px" AutoPostBack="True"></asp:TextBox>
               
            </td></tr>
        </table>
       
         
        <table>
            <tr><td><label data-lan-id="Min_Version"></label></td><td>
                <asp:TextBox ID="MinVersionTextBox"   runat="server"  OnTextChanged="MinVersionTextBox_TextChanged"  Width="120px" AutoPostBack="True"></asp:TextBox> 
            </td></tr>
        </table>
         
        <table>
            <tr><td class="auto-style1"><label data-lan-id="New_Package_Download_Url"></label></td><td class="auto-style1">
                <asp:TextBox ID="URLOfNewPackageTextBox"  runat="server"  OnTextChanged="URLOfNewPackageTextBox_TextChanged"  Width="250px" AutoPostBack="True"></asp:TextBox>
                <asp:CheckBox  ID="CheckBox1" data-lan-id="Force_Update" data-lan-type="text" runat="server"  Text=""    OnCheckedChanged="ForceUpdateCheckBox_CheckedChanged" AutoPostBack="True" />
            </td></tr>
        </table>
		<br />
        <asp:Label ID="reportLabel" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
