<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GmModify.aspx.cs" Inherits="gmt.GmModify" %>

<!DOCTYPE html>

<html lang="zh-CN">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link href="../bootstrap/css/bootstrap-datetimepicker.min.css" rel="stylesheet" media="screen" />
    <link href="../bootstrap/css/bootstrap-table.min.css" rel="stylesheet" media="screen" />
    <link href="../mycss/docs.min.css" rel="stylesheet" media="screen" />
    <link rel="stylesheet" href="../mycss/style.css" />
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <header class="navbar navbar-static-top bs-docs-nav" id="header" ></header>
    <div class="bs-docs-header" id="content">
        <div class="container">
            <h1 data-lan-id="HomePage"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <form id="form1" runat="server">
                    <table>
                        <tr>
                            <td colspan="4">
                                <label data-lan-id="Version"></label><asp:Label ID="versionLabel" runat="server"
                                    Width="200"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>
                                <label data-lan-id="Select_Server"></label></b></td>
                        </tr>
                        <tr style="height: 200px">
                            <td colspan="2">
                                <asp:ListBox ID="serverListBox" runat="server" Height="200px" Width="300px" SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td style="text-align: center">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button data-lan-id="AddAll" data-lan-type="text" runat="server" Text="" OnClick="addAllButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="125px" Height="30px" Font-Bold="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button data-lan-id="AddSelect" data-lan-type="text" runat="server" Text="" OnClick="addSelectButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="125px" Height="30px" Font-Bold="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button data-lan-id="RemoveSelect" data-lan-type="text" runat="server" Text="" OnClick="removeSelectButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="125px" Height="30px" Font-Bold="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button data-lan-id="RemoveAll" data-lan-type="text" runat="server" Text="" OnClick="removeAllButton_Click" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="125px" Height="30px" Font-Bold="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <asp:ListBox ID="selectListBox" runat="server" Height="200px" Width="300px" SelectionMode="Multiple"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label data-lan-id="PlayerHistory_palyer_id"></label>
                                <asp:TextBox ID="uidTextBox" runat="server"
                                    Width="150"></asp:TextBox></td>
                            <td colspan="2" style="color: red">
                                <label data-lan-id="Player_Name"></label>
                                <asp:Label ID="playerNameLabel" runat="server" Width="160"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <label data-lan-id="Server_Status"></label><asp:Label ID="serverStatusLabel"
                                    runat="server" Width="200" Font-Bold="true" ForeColor="Orange"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button data-lan-id="Confirm_Player" data-lan-type="text" runat="server"
                                    OnClick="ensureButton_Click" UseSubmitBehavior="false" Text="" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>


                        <tr>
                            <td><b>
                                <label data-lan-id="Give_Item"></label></b></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <label data-lan-id="PlayerHistory_type"></label><asp:DropDownList ID="itemGiveTypeDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="itemGiveTypeDropDownList_SelectedIndexChanged" Width="200"></asp:DropDownList>
                                <label data-lan-id="Select"></label><asp:DropDownList ID="itemGiveOptionDropDownList" runat="server" Width="200"></asp:DropDownList>
                                <label data-lan-id="PlayerHistory_quantity"></label><asp:TextBox ID="itemGiveCountTextBox" runat="server" Width="100"></asp:TextBox>
                                <asp:Button data-lan-id="Confirm_Give" data-lan-type="text" runat="server" OnClick="givePropButton_Click" UseSubmitBehavior="false" Text="" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td><b>
                                <label data-lan-id="Delete_Item"></label></b></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <label data-lan-id="PlayerHistory_type"></label><asp:DropDownList ID="itemDelTypeDropDownList" runat="server" AutoPostBack="true" Width="200" OnSelectedIndexChanged="itemDelTypeDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                <label data-lan-id="Select"></label><asp:DropDownList ID="itemDelOptionDropDownList" runat="server" Width="200"></asp:DropDownList>
                                <label data-lan-id="PlayerHistory_quantity"></label><asp:TextBox ID="itemDelCountTextBox" runat="server" Width="100"></asp:TextBox>
                                <asp:Button data-lan-id="Delete" data-lan-type="text" Text="" runat="server" UseSubmitBehavior="false" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" OnClick="delItemButton_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Button data-lan-id="Clear_Player" data-lan-type="text" runat="server" OnClick="clearButton_Click" UseSubmitBehavior="false" Text="" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                <asp:Button data-lan-id="Skip_Newbie" data-lan-type="text" runat="server" OnClick="skipNewbieButton_Click" UseSubmitBehavior="false" Text="" ForeColor="White" BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td><b>
                                <label data-lan-id="Gift_Give"></label></b></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <label data-lan-id="Select"></label>
                                <asp:DropDownList ID="giftOptionDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="giftOptionDropDownList_SelectedIndexChanged" Width="250px"></asp:DropDownList>
                                <asp:Label ID="giftLabel" runat="server" Width="400"
                                    ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="singleGiftButton" data-lan-id="Gift_Single_Give" data-lan-type="text" runat="server"
                                    OnClick="singleGiftButton_Click" UseSubmitBehavior="false" Text="" ForeColor="White"
                                    BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                                <asp:Button ID="wholeGiftButton" data-lan-id="Gift_All_Give" data-lan-type="text" runat="server"
                                    OnClick="wholeGiftButton_Click" UseSubmitBehavior="false" Text="" ForeColor="White"
                                    BackColor="Blue" Width="100" Height="30" Font-Bold="true" />
                            </td>
                            <td>
                                <button type="button" onclick="showTimedGiftList()">List</button>
                            </td>
                            <td>
                                <label data-lan-id="AutoSend"></label>
                                <input type="checkbox" aria-label="" name="gift_timer_checker" onchange="OnGiftTimerCheck()">
                            </td>
                            <td>
                                <input class="form-control" style="display: none;" type="text" placeholder="选择起始时间..." value="" id="datetimepicker_1" runat="server" />
                                <!-- /input-group -->
                            </td>
                            <td rowspan="7" class="errorLabel">
                                <asp:Label ID="errorLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><b>
                                <label data-lan-id="Senior_Code"></label></b></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label data-lan-id="Input"></label>
                                <asp:TextBox ID="gmTextBox" runat="server"
                                    Width="200"></asp:TextBox></td>
                        </tr>
<%--                        <tr>
                            <td colspan="7">
                                <asp:TextBox ID="jsondataTextBox" runat="server" Width="200px" Height="150px" TextMode="MultiLine"></asp:TextBox>

                            </td>
                            <td colspan="3">
                                <asp:Button ID="jsonDataButton" runat="server" Text="JsonData"
                                    OnClientClick="if (!confirm()) return;" UseSubmitBehavior="False"
                                    ForeColor="White" BackColor="Blue" Width="200" Height="30" Font-Bold="true" OnClick="jsonDataButton_Click" />
                            </td>
                        </tr>--%>
                    </table>
                        <div class="form-group">
                            <label class="col-sm-4" for ="input_swap_uid_1">UID 1</label>
                            <div class="col-sm-8">
                                <input id="input_swap_uid_1" name="input_swap_uid_1" class="form-control" type="number"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for ="input_swap_uid_2">UID 2</label>
                            <div class="col-sm-8">
                                <input id="input_swap_uid_2" name="input_swap_uid_2" class="form-control" type="number"/>
                            </div>
                        </div>
                        <div class="form-group" runat ="server">
                            <div class="col-sm-offset-1 col-sm-10">
                                <asp:button id="btn_swap_openid" runat="server" Text="SWAP" class="btn btn-danger form-control" OnClick="btn_swap_openid_Click"></asp:button>
                            </div>
                        </div>
                </form>
            </div>


    </div>

    <div class="modal fade" style="top: 30%" id="modal_timed_mail" tabindex="-1" role="dialog" aria-labelledby="modal_timed_mailLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue" id="modal_timed_mailLabel">
                        <label data-lan-id="GmModify_mail"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <table id="timedgift_table"></table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="$('#modal_edit_mail').modal({keyboard:false})" data-lan-id="GmModify_close"></button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /.modal -->

</body>
</html>
<script type="text/javascript" src="../bootstrap/js/bootstrap-table.min.js"></script>
<script src="../bootstrap/js/bootstrap-datetimepicker.min.js" charset="utf-8"></script>
<script src="../bootstrap/js/locales/bootstrap-datetimepicker.ko.js" charset="utf-8"></script>
<script>
    $('#datetimepicker_1').datetimepicker({
        language: 'zh-CN',
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 0,
        format: 'yyyy-mm-dd  hh:ii',
        forceParse: 1
    });

    function OnGiftTimerCheck() {
        $('#datetimepicker_1').toggle('disabled');
    }

    function showTimedGiftList() {
        var data = GetTimedMailListData();
        UpdateTimedGiftTable(data);
        $('#modal_timed_mail').modal({ keyboard: false })
    }

    function UpdateTimedGiftTable(dt) {
        var data = JSON.parse(dt);
        $("#timedgift_table").bootstrapTable("destroy");
        $("#timedgift_table").bootstrapTable({
            data: data,
            //height: 200,
            striped: true, // 隔行变色
            search: true,
            sidePagination: 'client',
            pagination: true,
            pageSize: 5,
            //showFooter: true,
            columns: [
                {
                    title: 'ID',
                    field: 'id',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: 'UID',
                    field: 'uid',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == 0)
                            return "All";
                        return value;
                    }
                },
                {
                    title: 'Server',
                    field: 'serverName',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: 'Mail ID',
                    field: 'mailId',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: 'Send Time',
                    field: 'sendTime',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        var dt = new Date(value * 1000);
                        return dt.toLocaleString('en-US', { hour12: false });
                    }
                },
                {
                    title: 'OPT',
                    field: 'operate',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return [
                            '<a class="operate_delete" href="javascript:void(0)" title="Delete">',
                            '<i class="glyphicon glyphicon-trash"></i>',
                            '</a>',
                        ].join('');
                    },
                    events: {
                        'click .operate_delete': function (e, value, row, index) {
                            if (row.id == null) {
                                alert(GetContentMsg("GmModify_mail_not_found"));
                            }
                            else if (confirm(GetContentMsg("GmModify_mail_delete") + "(ID：" + row.id + ")")) {
                                DelTimedMail(row.id);
                            }
                        },

                    }
                }
            ]
        });
    }

    function GetTimedMailListData() {
        var i = 0;
        var ret = "";
        $.ajax(
            {
                type: "POST",
                url: "GmModify.aspx/GetTimedMailData",
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    i++;
                },
                success: function (msg) {
                    ret = msg.d;
                },
                error: function (msg) {
                    alert("get data fail");
                },
                complete: function () {
                    i--;
                    if (i <= 0) {
                    }
                }
            }
            )
        return ret;
    }

    function DelTimedMail(Id) {
        var i = 0;
        var data_json = { "Id": Id };
        $.ajax(
            {
                type: "POST",
                url: "GmModify.aspx/DelTimedMail",
                async: false,
                data: JSON.stringify(data_json),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    i++;
                },
                success: function (msg) {
                    var data_json = JSON.parse(msg.d);
                    if (data_json.error == 0) {
                        var data = GetTimedMailListData();
                        UpdateTimedGiftTable(data);
                    }
                },
                error: function (msg) {
                    alert("error！");
                },
                complete: function () {
                    i--;
                    if (i <= 0) {
                    }
                }
            }
            )
    }

</script>

