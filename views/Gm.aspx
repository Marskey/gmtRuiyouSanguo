<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gm.aspx.cs" Inherits="gm.Gm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<style type="text/css">
		.auto-style1 {
			width: 228px;
		}
	    .auto-style3 {
            width: 187px;
        }
        .auto-style18 {
            width: 372px;
        }
        .auto-style22 {
            width: 565px;
        }
	    .auto-style25 {
            width: 648px;
        }
	    .auto-style27 {
            width: 585px;
        }
        .auto-style28 {
            width: 490px;
        }
        .auto-style30 {
            width: 501px;
        }
	</style>
	</head>
<body>
    <form id="form1" runat="server">
    <div>
		<table>
			<tr><td colspan="6"><a href="CreateKey.aspx">激活码</a></td></tr>
			<tr><td colspan="6"><a href="CreateGift.aspx">礼包码</a></td></tr>
			<tr><td colspan="6"><a href="Notice.aspx">公告</a></td></tr>
			<tr><td colspan="6"><a href="BatchGive.aspx">批量给予物品</a></td></tr>
			<tr><td colspan="6"><a href="Activity.aspx">活动</a></td></tr>
			<tr><td colspan="6"><a href="PVPReward.aspx">PVP奖励</a></td></tr>
			<tr><td colspan="6"><a href="Mall.aspx">商城</a></td></tr>
			<tr><td colspan="6"><a href="LuckDraw.aspx">抽奖</a></td></tr>
            <tr><td colspan="6"><a href="ActivityOperate.aspx">活动表操作</a></td></tr>
            <tr><td colspan="6"><a href="Gift.aspx">礼包编辑</a></td></tr>
            <tr><td colspan="6"><a href="NoticeEdit.aspx">公告编辑</a></td></tr>
			<%--<tr><td colspan="6"><a href="FileSaveLoad.aspx">活动表操作</a></td></tr>
            <tr><td colspan="6"><a href="SysNtfConfig.aspx">SysNtfConfig表操作</a></td></tr>
            <tr><td colspan="6"><a href="ActivityOperate.aspx">活动表操作</a></td></tr>
			<tr><td colspan="6"><a href="RunandFixManage.aspx">运维管理</a></td></tr>--%>
            <tr><td colspan="6"><a href="SectionServer.aspx">服务器列表编辑</a></td></tr>
            <tr><td colspan="6"><a href="MergeServer.aspx">合并服务器</a></td></tr>
            <%--<tr><td colspan="6"><a href="GmAnalysis.aspx">统计与分析</a></td></tr>--%>
			<tr><td>&nbsp;</td></tr>
			<tr><td colspan="2">
				对应游戏版本：<asp:Label ID="versionLabel" runat="server" Text="Label"></asp:Label>
				</td></tr>
			<tr><td>&nbsp;</td></tr>
			<tr>
				<td>玩家名称</td>
				<td>
				<asp:TextBox ID="playerNameTextBox" runat="server" Width="80px"></asp:TextBox>
				</td>
				<td>&nbsp;</td>
				<td>
					&nbsp;</td>
				<td>&nbsp;</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr><td>玩家编号UID</td><td>
				<asp:TextBox ID="uidTextBox" runat="server" Width="80px"></asp:TextBox>
				</td></tr>
			<tr><td>平台编号CYID</td><td>
				<asp:TextBox ID="cyidTextBox" runat="server" Width="80px"></asp:TextBox>
				</td></tr>
			<tr><td>
				<asp:Button ID="findIdButton" runat="server" OnClick="findIdButton_Click" Text="查询玩家" />
				</td><td colspan="5">
					<asp:Label ID="idLabel" runat="server"></asp:Label>
				</td></tr>
			<tr><td></td></tr>
			<tr><td>
				<asp:Button ID="initializeButton" runat="server" Text="新服一键初始化" OnClick="initializeButton_Click" Visible="False" />
				</td></tr>
			<tr><td>
				<asp:Button ID="dbUpdateButton" runat="server" OnClientClick="if (!confirm('确定要数据库设置ServerId吗?')) return;" OnClick="dbUpdateButton_Click" Text="数据库设置ServerId" CausesValidation="False" Visible="False" />
				</td></tr>
		</table>
		<br />
		<table><tr><td>&nbsp;</td>
				<td colspan="2">
					<table>
						<tr><td rowspan="4">
							<asp:ListBox ID="serverListBox" runat="server" SelectionMode="Multiple" Height="200px" Width="150px"></asp:ListBox>
							</td><td>
							<asp:Button ID="addAllButton" runat="server" Text="全部-&gt;" OnClick="addAllButton_Click" />
							</td><td rowspan="4">
								<asp:ListBox ID="selectListBox" runat="server" SelectionMode="Multiple" Height="200px" Width="150px"></asp:ListBox>
							</td></tr>
						<tr><td>
								<asp:Button ID="addServerButton" runat="server" Text="添加-&gt;" OnClick="addServerButton_Click" />
							</td></tr><tr><td>
							<asp:Button ID="removeServerButton" runat="server" Text="&lt;-移除" OnClick="removeServerButton_Click" />
							</td></tr><tr><td>
							<asp:Button ID="removeAllButton" runat="server" Text="&lt;-全部" OnClick="removeAllButton_Click" />
							</td></tr>
					</table>
				</td></tr>
			<tr>
				<td>执行</td><td>ID</td><td>数量</td><td></td><td rowspan="9" class="auto-style1">
				<asp:Label ID="errorLabel" runat="server"></asp:Label>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="cardButton" runat="server" OnClick="cardButton_Click" Text="卡牌" UseSubmitBehavior="False" />
				</td><td>
					<asp:TextBox ID="cardIdTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td><td>
					&nbsp;</td><td>
					<asp:CheckBox ID="cardAllCheckBox" runat="server" Text="全员" Visible="False" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="expButton" runat="server" OnClick="expButton_Click" Text="经验" UseSubmitBehavior="False" />
				</td><td>
					<asp:TextBox ID="teamExpTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')">0</asp:TextBox>
				</td><td>
					<asp:TextBox ID="heroExpTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')">0</asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="itemButton" runat="server" OnClick="itemButton_Click" Text="物品" UseSubmitBehavior="False" />
				</td><td>
					<asp:TextBox ID="itemIdTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td><td>
					<asp:TextBox ID="itemCountTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td>
				<td>
					<asp:CheckBox ID="itemAllCheckBox" runat="server" Text="全员" Visible="False" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="peaceButton" runat="server" OnClick="peaceButton_Click" Text="碎片" UseSubmitBehavior="False" />
				</td><td>
					<asp:TextBox ID="peaceIdTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td><td>
					<asp:TextBox ID="peaceCountTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="moneyButton" runat="server" OnClick="moneyButton_Click" Text="金钱" UseSubmitBehavior="False" />
				</td><td>
					&nbsp;</td><td>
					<asp:TextBox ID="moneyTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="tokenButton" runat="server" OnClick="tokenButton_Click" Text="元宝" UseSubmitBehavior="False" />
				</td><td>
					&nbsp;</td><td>
					<asp:TextBox ID="tokenTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="skillPointButton" runat="server" OnClick="skillPointButton_Click" Text="技能点" UseSubmitBehavior="False" />
				</td><td>
					&nbsp;</td><td>
					<asp:TextBox ID="skillPointTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="friendPointButton" runat="server" OnClick="friendPointButton_Click" Text="友情点" UseSubmitBehavior="False" />
				</td><td>
					&nbsp;</td><td>
					<asp:TextBox ID="friendPointTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td>
			</tr>
			<%--<tr>
				<td>
					<asp:Button ID="BMonCardButton" runat="server"  Text="大月卡天数" UseSubmitBehavior="False" OnClick="BMonCardButton_Click" />
                    </td><td>
					<asp:TextBox ID="BMonCardTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" ></asp:TextBox>
				    </td>
                    <td>
                    <asp:Button ID="SMonCardButton" runat="server"  Text="小月卡天数" UseSubmitBehavior="False" OnClick="SMonCardButton_Click" />
				</td><td>
					<asp:TextBox ID="SMonCardTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				    </td>
			</tr>--%>
			<tr><td>
					<asp:Button ID="rankRewardButton" runat="server" Text="排行榜奖励" UseSubmitBehavior="False" OnClick="rankRewardButton_Click" />
				</td><td>
					<asp:DropDownList ID="rankTypeDropDownList" runat="server" Width="100px">
					</asp:DropDownList>
					</td><td><asp:TextBox ID="rankGiftTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')">0</asp:TextBox>
					</td></tr>
			<tr>
				<td>
					&nbsp;</td><td>
					<asp:Button ID="shutupButton" runat="server" OnClick="shutupButton_Click" Text="禁言" UseSubmitBehavior="False" />
				</td><td>
					<asp:Button ID="notShutupButton" runat="server" OnClick="notShutupButton_Click" Text="解禁" UseSubmitBehavior="False" />
				</td>
			</tr>
            <tr>
                <td>
                    &nbsp;</td><td>
                     <asp:Button ID="InputNotActivityButton"   runat="server"    Text="载入(不包括活动)" OnClientClick="if (!confirm('确定载入配置(不包括活动)吗?')) return;"  UseSubmitBehavior="False" OnClick="InputNotActivityButton_Click" />
                </td><td>
                     <asp:Button ID="InputAllButton"     runat="server"      Text="重新载入所有配置"  OnClientClick="if (!confirm('确定重新载入所有配置吗?')) return;"  UseSubmitBehavior ="False" OnClick="InputAllButton_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td><td>
                </td><td>
                </td>
            </tr>
			<tr>
				<td>
					<asp:Button ID="clearButton" runat="server" OnClick="clearButton_Click" Text="清理角色" UseSubmitBehavior="False" />
				</td>
				<td>
					<asp:Button ID="skipNewbieButton" runat="server" Text="跳过新手" OnClick="skipNewbieButton_Click" />
				</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="kickButton" runat="server" OnClick="kickButton_Click" Text="踢人" />
				</td>
				<td>
					<asp:Button ID="banButton" runat="server" OnClientClick="if (!confirm('确定要封号吗?')) return;" OnClick="banButton_Click" Text="封号" UseSubmitBehavior="False" />
				</td>
				<td>
					<asp:Button ID="unbanButton" runat="server" OnClientClick="if (!confirm('确定要解除封号吗?')) return;" OnClick="unbanButton_Click" Text="解封" UseSubmitBehavior="False" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="vipButton" runat="server" Text="VIP" OnClick="vipButton_Click" Visible="False" />
				</td>
				<td>
					<asp:TextBox ID="vipTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" Visible="False">0</asp:TextBox>
				</td>
			</tr>
			<tr><td>
					<asp:Button ID="onlineButton" runat="server" Text="在线" OnClick="onlineButton_Click" />
				</td><td>
					<asp:TextBox ID="onlineTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')">0</asp:TextBox>
				</td></tr>
			<tr><td>
					<asp:Button ID="preButton" runat="server" Text="预创建" OnClick="preButton_Click" />
				</td><td>
					<asp:TextBox ID="preTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')">0</asp:TextBox>
				</td></tr>
			<tr>
				<td>
					<asp:Button ID="noticeButton" runat="server" Text="公告" UseSubmitBehavior="False" OnClick="noticeButton_Click" />
				</td><td colspan="4">
					<asp:TextBox ID="noticeTextBox" runat="server" Width="400px" MaxLength="512"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="giftButton" runat="server" OnClick="giftButton_Click" Text="发礼包" UseSubmitBehavior="False" />
				</td><td>
					<asp:TextBox ID="giftTextBox" runat="server" Width="80px" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
				</td><td>
					<asp:CheckBox ID="allCheckBox" runat="server" Text="全员" />
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;</td><td>
					<asp:Button ID="stopButton" runat="server" OnClick="stopButton_Click" OnClientClick="if (!confirm('确定要开启IP过滤吗?')) return;" Text="阻止" UseSubmitBehavior="False" />
				</td><td>
					<asp:Button ID="notStopButton" runat="server" OnClick="notStopButton_Click" OnClientClick="if (!confirm('确定要关闭IP过滤吗?')) return;" Text="不阻止" UseSubmitBehavior="False" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="whiteListButton" runat="server" Text="白名单" UseSubmitBehavior="False" OnClick="whiteListButton_Click" />
				</td><td colspan="2">
					<asp:TextBox ID="whileListTextBox" runat="server" Width="170px"></asp:TextBox>
				</td>
			</tr>
			
			<tr>
				<td>
					<asp:Button ID="gmButton" runat="server" Text="GM命令" OnClick="gmButton_Click" UseSubmitBehavior="False" />
				</td><td colspan="2">
					<asp:TextBox ID="gmTextBox" runat="server" Width="170px"></asp:TextBox>
					<asp:CheckBox ID="allCommandCheckBox" runat="server" Text="所有服务器" Visible="False" />
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;</td><td colspan="4" style="text-align: right">
					<asp:Button ID="shutdownButton" runat="server" Text="关服" OnClientClick="if (!confirm('确定要关服吗?') || !confirm('关闭服务器可能会给玩家带来不好的体验，真的确定要关服吗?')) return;" UseSubmitBehavior="False" OnClick="shutdownButton_Click" />
				</td>
			</tr>
		</table>
		<br />
		<asp:Label ID="reportLabel" runat="server"></asp:Label>
		<br />
		
		<br />
		<asp:Button ID="rechargeButton" runat="server" OnClick="rechargeButton_Click" Text="充值测试" Visible="False" />
    	<br />
		<br />
		<asp:Button ID="downloadLogButton" runat="server" Text="下载日志文件" OnClientClick="window.open('DownloadLog.aspx', 'newwindow');" />
    	<br />
        <br />
		<asp:Button ID="informationButton" runat="server" Text="显示信息" OnClick="informationButton_Click" />
    	<br />
        <br />
        <br />
		<%--<br /> 
		<asp:Label ID="otherLabel" runat="server"></asp:Label>
        <br />--%>
        <td>*帐号信息</td>
    	<br />
        <table style="border-style: solid; border-width: thin; width:1300px;  height:50px;"> 
             
             <td class="auto-style3">&nbsp &nbsp &nbsp Vip等级:
                    <asp:TextBox ID="VipLevelText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>    
             <td class="auto-style3">&nbsp &nbsp &nbsp 在线时间:
                    <asp:TextBox ID="OnlineTimeText" runat="server" Width="150px" ReadOnly="True" ></asp:TextBox>
             </td>
            <td class="auto-style3">&nbsp &nbsp &nbsp 元宝充值数:
                    <asp:TextBox ID="yuanbao_all" runat="server" Width="150px" ReadOnly="True" ></asp:TextBox>
             </td>
            <td class="auto-style3">&nbsp &nbsp &nbsp 创建时间:
                    <asp:TextBox ID="RolecreateTimeText" runat="server" Width="150px" ReadOnly="True" ></asp:TextBox>
             </td>
           
        </table>
        <td>*基础信息</td>
        <table style="border-style: solid; border-width: thin; width:1300px;  height:70px;">
             <tr>
             <td class="auto-style28">玩家uid:
                    <asp:TextBox ID="DescPlayeruidText" runat="server" Width="100px" ReadOnly="True" ></asp:TextBox>
             </td>      
             <td class="auto-style22">金钱:
                    <asp:TextBox ID="MoneyText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style27">元宝:
                    <asp:TextBox ID="YubaoText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style27">精力:
                    <asp:TextBox ID="EnergeText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style30">技能点:
                    <asp:TextBox ID="SkillpointText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style25">分解点:
                    <asp:TextBox ID="ResorvepointText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style18">活跃点:
                    <asp:TextBox ID="ActivepointText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
            
             </tr><tr>
              <td class="auto-style28">荣誉点:
                    <asp:TextBox ID="HonorText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style22">技能点时间:<asp:TextBox ID="SkillpointTimeText" runat="server" Width="80px" ReadOnly="True" ></asp:TextBox>
             &nbsp;</td>  
             <td class="auto-style27">经验:
                    <asp:TextBox ID="ExpText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style27">团队等级:
                    <asp:TextBox ID="LevelText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style30">通天塔层数:
                    <asp:TextBox ID="TTTCurCengText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style25">通天塔最高层数:<asp:TextBox ID="TTTMaxCengText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             &nbsp;</td>    
            
             </tr>
             <tr> 
                  <td class="auto-style28">通天塔重置数:
                    <asp:TextBox ID="TTTResetTimeText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
                  </td>  
                  <td class="auto-style22">副本进度:
                    <asp:TextBox ID="FubenprogressText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
                  </td>  
                  <td class="auto-style27">天赋信息:
                    <asp:TextBox ID="TalentlevelText" runat="server" Width="100px" ReadOnly="True" ></asp:TextBox>
                  </td>  
                  <td class="auto-style27">PVP最高排名:<asp:TextBox ID="PVPText" runat="server" Width="100px" ReadOnly="True" ></asp:TextBox>
                  </td>  
                 <td class="auto-style30">精力时间:
                    <asp:TextBox ID="EnergeTimeText" runat="server" Width="100px" ReadOnly="True" ></asp:TextBox>
                 </td>
                 
             </tr>
             <tr>
                 <td class="auto-style28">魂阵数:
                    <asp:TextBox ID="CardSlotNumText" runat="server" Width="100px" ReadOnly="True" ></asp:TextBox>
                  </td>
                 
                 
             </tr>
             <tr>
                 <td class="auto-style28">大月卡时间:
                    <asp:TextBox ID="BigMonthText" runat="server" Width="80px" ReadOnly="True" ></asp:TextBox>
                  </td> 
                 <td class="auto-style22">小月卡时间:
                    <asp:TextBox ID="SmallMonthText" runat="server" Width="80px" ReadOnly="True" ></asp:TextBox>
                  </td>   
                  <td class="auto-style27">情缘领取ID:
                    <asp:TextBox ID="QingyuanText" runat="server" Width="100px" ReadOnly="True" ></asp:TextBox>
                  </td>   
              </tr>
        </table>
        <%--<td>*角色额外数据</td>
    	<br />
        <table style="border-style: solid; border-width: thin; width:1300px;  height:50px;">    
             <td class="auto-style3">&nbsp &nbsp &nbsp flag:
                    <asp:TextBox ID="RoleflagText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
            <td class="auto-style3">&nbsp &nbsp &nbsp 免费抽奖:
                    <asp:TextBox ID="FreechoujiangText" runat="server" Width="100px" ReadOnly="True" ></asp:TextBox>
             </td>
            <td class="auto-style3">&nbsp &nbsp &nbsp 最后日期:
                    <asp:TextBox ID="LastDateText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
            <td class="auto-style3">&nbsp &nbsp &nbsp 位置:
                    <asp:TextBox ID="IndexText" runat="server" Width="100px" ReadOnly="True" ></asp:TextBox>
             </td>
            
        </table>--%>
        <td>*Vip购买次数</td>
    	<br />
        <table style="border-style: solid; border-width: thin; width:1300px;  height:50px;">    
             <td class="auto-style3">&nbsp &nbsp &nbsp 挑战:
                    <asp:TextBox ID="VipChalengeText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style3">&nbsp &nbsp &nbsp 精力 :
                    <asp:TextBox ID="VipEnergeText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style3">&nbsp &nbsp &nbsp 金钱 :
                    <asp:TextBox ID="VipMoneyText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
            <td class="auto-style3">&nbsp &nbsp &nbsp  技能点:
                    <asp:TextBox ID="VipSkillpointText" runat="server" Width="50px" ReadOnly="True" ></asp:TextBox>
             </td>
             <td class="auto-style3">&nbsp &nbsp &nbsp 元宝:
                    <asp:TextBox ID="VipYuanbaoText" runat="server" Width="50px" ></asp:TextBox>
             </td>
           
        </table>
        <td>*卡牌数据</td>
    	<br />
        <table style="border-style: solid; border-width: thin; width:1300px;  height:auto;">    
             <td class="auto-style3">&nbsp &nbsp &nbsp 
                    <asp:Label ID="Cardinfolabel" runat="server" Width="200px" ></asp:Label>
             </td>   
        </table>
        <td>*碎片数据</td>
    	<br />
        <table style="border-style: solid; border-width: thin; width:1300px;  height:auto;">    
             <td class="auto-style3">&nbsp &nbsp &nbsp 
                    <asp:Label ID="Chipinfolabel" runat="server" Width="200px" ></asp:Label>
             </td>   
        </table>
        <td>*好友数据</td>
    	<br />
        <table style="border-style: solid; border-width: thin; width:1300px;  height:auto;">    
             <td class="auto-style3">&nbsp &nbsp &nbsp 
                    <asp:Label ID="Friendinfolabel" runat="server" Width="200px" ></asp:Label>
             </td>   
        </table>
         <td>*宠物数据</td>
    	<br />
        <table style="border-style: solid; border-width: thin; width:1300px;  height:auto;">    
             <td class="auto-style3">&nbsp &nbsp &nbsp 
                    <asp:Label ID="Petinfolabel" runat="server" Width="200px" ></asp:Label>
             </td>   
        </table>
       <%-- <td>*gift_flag</td>
    	<br />
        <table style="border-style: solid; border-width: thin; width:1300px;  height:auto;">    
             <td class="auto-style3">&nbsp &nbsp &nbsp 
                    <asp:Label ID="gift_flaglabel" runat="server" Width="200px" ></asp:Label>
             </td>   
        </table>
        <td>*giftinfos</td>
    	<br />
        <table style="border-style: solid; border-width: thin; width:1300px;  height:auto;">    
             <td class="auto-style3">&nbsp &nbsp &nbsp 
                    <asp:Label ID="giftinfoslabel" runat="server" Width="200px" ></asp:Label>
             </td>   
        </table>--%>
        <br /> 
		<asp:Label ID="otherLabel" runat="server"></asp:Label>
        <br />
    	
    </div>
    </form>
</body>
</html>