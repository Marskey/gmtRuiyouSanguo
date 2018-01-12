<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="gmt.User" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link href="../bootstrap/css/bootstrap-table.min.css" rel="stylesheet" media="screen" />
    <link href="../mycss/docs.min.css" rel="stylesheet" media="screen" />
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <header class="navbar navbar-static-top bs-docs-nav" id="header"></header>
    <div class="bs-docs-header" id="content">
        <div class="container">
            <h1 data-lan-id="User_Manager"></h1>
        </div>
    </div>
    <br />
    <div class="container">
        <div class="row">
            <div id="toolbar">
                <button class="btn btn-primary" type="button" onclick="userPlus();">
                    <i class="glyphicon glyphicon-plus"></i>
                </button>
            </div>
            <table id="user_info_table"></table>
        </div>
    </div>

    <!--Dialog Start-->
    <div class="modal fade" style="top: 15%" id="user_operate_dialog" tabindex="-1" role="dialog" aria-labelledby="user_operate_dialog_title_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue" id="user_operate_dialog_title_label">
                        <label id="uod_title_tip_label"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label" data-lan-id="User_Name"></label>
                            <div class="col-sm-10">
                                <input class="form-control" id="nameInput" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label" data-lan-id="Password"></label>
                            <div class="col-sm-10">
                                <input class="form-control" type="text" id="pwdInput" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label" data-lan-id="Privilege_Setting" style="text-align: center"></label>
                            <div class="col-sm-10">
                                <table id="user_privilege_table"></table>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" onclick="onUserOperateConfirm();" id="operateBtn"></button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="$('#user_operate_dialog').modal({keyboard:false})" data-lan-id="GmModify_close"></button>
                </div>
            </div>
        </div>
    </div>
    <!--Dialog End-->

</body>
</html>

<script type="text/javascript" src="../bootstrap/js/bootstrap-table.min.js"></script>
<script type="text/javascript" src="../bootstrap/js/locales/bootstrap-table-locale-all.js"></script>

<script>

    //所有权限
    var PrivilegeJson;

    function SetPrivilegeJson() {
        PrivilegeJson = [
        {
            privilege_name: GetContentMsg("Modify"),
            privilege_idx: "Modify",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("Download"),
            privilege_idx: "Download",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("HomePage"),
            privilege_idx: "GmModify",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("User_Manager"),
            privilege_idx: "User",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("Notice"),
            privilege_idx: "Notice",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("Activity"),
            privilege_idx: "Activity",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("ActivityOperate"),
            privilege_idx: "ActivityOperate",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("CreateKey"),
            privilege_idx: "CreateKey",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("CreateGift"),
            privilege_idx: "CreateGift",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("Gift"),
            privilege_idx: "Gift",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("BatchGive"),
            privilege_idx: "BatchGive",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("ActivityOpenTime"),
            privilege_idx: "ActivityOpenTime",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("GmNormal"),
            privilege_idx: "GmNormal",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("PlayerRechargeLook"),
            privilege_idx: "PlayerRechargeLook",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("PlayerHistory"),
            privilege_idx: "PlayerHistory",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("ServerData"),
            privilege_idx: "ServerData",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("FTPEdit"),
            privilege_idx: "FTPEdit",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("PayType"),
            privilege_idx: "PayType",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("MergeServer"),
            privilege_idx: "MergeServer",
            privilege_allow: false,
        },
        {
            privilege_name: GetContentMsg("SectionServer"),
            privilege_idx: "SectionServer",
            privilege_allow: false,
        },
        ];
    }

</script>

<script>
    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales[language_type]);

    $(document).ready(function () {
        var data = { "none": "-" };
        initUserInfoTable(data);
        queryUserData();
        SetPrivilegeJson();
    })

    function userPlus() {
        initUserPrivilegeTable(PrivilegeJson);
        $('#nameInput').val("");
        $('#nameInput').attr('readonly', false);
        $('#pwdInput').val("");
        $('#pwdInput').attr("type", "text");
        $('#pwdInput').attr('readonly', false);
        $('#operateBtn').html(GetContentMsg("Add"));
        $('#uod_title_tip_label').html(GetContentMsg("Add_User"));
        optType = "Add";
        $('#user_operate_dialog').modal({ keyboard: true });
    }

    $('#user_operate_dialog').on('shown.bs.modal', function () {
        $('#user_privilege_table').bootstrapTable('resetView');
    });

    var optType = "";
    function onUserOperateConfirm() {
        if (null == $('#nameInput').val() || "" == $('#nameInput').val()) {
            alert(GetContentMsg("Error_User_Name_Null"));
            return;
        }
        if (null == $('#pwdInput').val() || "" == $('#pwdInput').val()) {
            alert(GetContentMsg("Error_User_Pwd_Null"));
            return;
        }

        var privilegeList = {};
        $('input[id^=ck_]').each(function () {
            var idx = $(this).attr("id").slice("ck_".length);
            privilegeList[idx] = $(this).attr("checked") ? true : false;
        });

        updateUserInfo($('#nameInput').val(), $('#pwdInput').val(), JSON.stringify(privilegeList));

        $('#user_operate_dialog').modal({ keyboard: false });
    }

    function initUserPrivilegeTable(dt) {
        $('#user_privilege_table').bootstrapTable('destroy');
        $('#user_privilege_table').bootstrapTable({
            data: dt,
            height: 200,
            striped: true,
            columns: [
                {
                    title: GetContentMsg("Privilege_Name"),
                    field: 'privilege_name',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("Privilege_Allow"),
                    field: 'privilege_allow',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        var idx = "ck_" + row.privilege_idx;
                        if (row.privilege_allow)
                            return ['<input type="checkbox" class="ck" id=' + idx + ' checked="true" />'].join('');
                        else
                            return ['<input type="checkbox" class="ck" id=' + idx + ' />'].join('');
                    },
                    events: {
                        'click .ck': function (e, value, row, index) {
                            var idx = "ck_" + row.privilege_idx;
                            if ($("#" + idx).attr("checked"))
                                $("#" + idx).attr("checked", false);
                            else
                                $("#" + idx).attr("checked", true);
                        }
                    },
                },
            ],
        });
    }

    function initUserInfoTable(dt) {
        $('#user_info_table').bootstrapTable('destroy');
        $('#user_info_table').bootstrapTable({
            data: dt,
            pagination: true,
            height: $(window).height(),
            sidePagination: 'client',
            pageSize: 10,
            pageList: [10, 20, 50, 100, GetContentMsg("ServerData_all")],
            striped: true, // 隔行变色
            search: true,
            toolbar: '#toolbar',
            showColumns: true,
            columns: [
                {
                    title: GetContentMsg("User"),
                    field: 'account',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("Modify"),
                    field: 'Modify',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.Modify)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("Download"),
                    field: 'Download',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.Download)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("HomePage"),
                    field: 'GmModify',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.GmModify)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("User_Manager"),
                    field: 'User_Manager',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.User)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("Notice"),
                    field: 'Notice',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.Notice)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("Activity"),
                    field: 'Activity',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.Activity)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("ActivityOperate"),
                    field: 'ActivityOperate',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.ActivityOperate)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("CreateKey"),
                    field: 'CreateKey',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.CreateKey)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("CreateGift"),
                    field: 'CreateGift',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.CreateGift)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("Gift"),
                    field: 'Gift',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.Gift)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("BatchGive"),
                    field: 'BatchGive',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.BatchGive)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("ActivityOpenTime"),
                    field: 'ActivityOpenTime',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.ActivityOpenTime)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("GmNormal"),
                    field: 'GmNormal',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.GmNormal)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("PlayerRechargeLook"),
                    field: 'PlayerRechargeLook',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.PlayerRechargeLook)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("PlayerHistory"),
                    field: 'PlayerHistory',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.PlayerHistory)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("ServerData"),
                    field: 'ServerData',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.ServerData)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("FTPEdit"),
                    field: 'FTPEdit',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.FTPEdit)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                //{
                //    title: GetContentMsg("PayType"),
                //    field: 'PayType',
                //    align: 'center',
                //    valign: 'middle',
                //    visible: false,
                //    formatter: function (value, row, index) {
                //        if (row.privilegeDic.PayType)
                //            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                //        else
                //            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                //    }
                //},
                {
                    title: GetContentMsg("MergeServer"),
                    field: 'MergeServer',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.MergeServer)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("SectionServer"),
                    field: 'SectionServer',
                    align: 'center',
                    valign: 'middle',
                    visible: false,
                    formatter: function (value, row, index) {
                        if (row.privilegeDic.SectionServer)
                            return ['<i class="glyphicon glyphicon-ok" style="color:green"></i>'].join('');
                        else
                            return ['<i class="glyphicon glyphicon-remove" style="color:red"></i>'].join('');
                    }
                },
                {
                    title: GetContentMsg("Table_th_opt"),
                    field: 'operate',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return ['<button class="modify btn btn-primary" type="button">',
                                        '<i class="glyphicon glyphicon-wrench"></i>',
                                    '</button>',
                                    '&nbsp;&nbsp;&nbsp;',
                                    '<button class="delete btn btn-danger" type="button">',
                                        '<i class="glyphicon glyphicon-trash"></i>',
                                    '</button>'].join('');
                    },
                    events: {
                        'click .modify': function (e, value, row, index) {
                            var tempJson = [];
                            for (var i = 0; i < PrivilegeJson.length; i++) {
                                var temp = {
                                    privilege_name: "",
                                    privilege_idx: "",
                                    privilege_allow: "",
                                };
                                temp.privilege_name = PrivilegeJson[i].privilege_name;
                                temp.privilege_idx = PrivilegeJson[i].privilege_idx;
                                temp.privilege_allow = row.privilegeDic[PrivilegeJson[i].privilege_idx];
                                tempJson[i] = temp;
                            }
                            initUserPrivilegeTable(tempJson);
                            $('#nameInput').val(row.account);
                            $('#nameInput').attr('readonly', true);
                            $('#pwdInput').val(row.password);
                            //$('#pwdInput').attr("type", "password");
                            $('#pwdInput').attr('readonly', true);
                            $('#operateBtn').html(GetContentMsg("Modify"));
                            $('#uod_title_tip_label').html(GetContentMsg("Modify_User"));
                            $('#user_operate_dialog').modal({ keyboard: true });
                            optType = "Modify";
                        },

                        'click .delete': function (e, value, row, index) {
                            optType = "Delete";
                            updateUserInfo(row.account, "", "{}");
                        }
                    },
                },
            ],
        });
    }

</script>

<script>

    function queryUserData() {
        $.ajax({
            type: "post",
            url: "User.aspx/GetUserData",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                initUserInfoTable();
            },
            success: function (result) {
                var data_json = JSON.parse(result.d);
                if (data_json.error == 1) {
                    alert(GetContentMsg("User_Info_Is_Null"));
                }
                else
                    initUserInfoTable(data_json);
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        });
    }

    function updateUserInfo(account, pwd, privileges) {
        var data_json = { "account": account, "pwd": pwd, "privileges": privileges, "opt": optType };
        $.ajax({
            type: "post",
            url: "User.aspx/UpdateUserInfo",
            async: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data_json),
            dataType: "json",
            beforeSend: function () {
            },
            success: function (result) {
                var data_json = JSON.parse(result.d);
                alert(GetContentMsg(data_json.error));
                queryUserData();
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        });
    }

</script>
