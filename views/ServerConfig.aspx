<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServerConfig.aspx.cs" Inherits="gmt.views.ServerConfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../bootstrap/css/bootstrap-table.min.css" />
    <link href="../bootstrap/css/bootstrap-datetimepicker.min.css" rel="stylesheet" media="screen" />
    <link href="../mycss/docs.min.css" rel="stylesheet" media="screen" />
    <link rel="stylesheet" href="../mycss/style.css" />
    <link href="http://cdn.bootcss.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" />
    <script src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script src="../bootstrap/js/bootstrap.min.js"></script>
    <script src="../js/global.js"></script>
    <script src="../js/language.js"></script>
    <script src="../js/ajaxSubmit.js"></script>
</head>
<body>
    <header class="navbar navbar-static-top bs-docs-nav" id="header"></header>
    <div class="bs-docs-header" id="content">
        <div class="container">
            <h1 data-lan-id="Page_title_ServerConfig"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <!--Card-->
            <div class="panel panel-default">
                <!--Card header-->
                <div class="panel-heading">
                    <div>
                        <button id="add_server_config" type="button" class="btn btn-default" data-lan-id="Add">
                            <i class="fa fa-pencil mt-0"></i>
                        </button>
                        <button id="del_server_config" type="button" class="btn btn-default" data-lan-id="Delete" disabled>
                            <i class="fa fa-remove mt-0"></i>
                        </button>
                        <button id="modify_server_config" type="button" class="btn btn-default" data-lan-id="Modify" disabled>
                            <i class="fa fa-gear"></i>
                        </button>
                        <button id="merge_server_config" type="button" class="btn btn-default" data-lan-id="MergeServer" disabled>
                            <i class="fa fa-sitemap"></i>
                        </button>
                        <button id="start_server_config" type="button" class="btn btn-default" data-lan-id="Open-door" disabled>
                            <i class="fa fa-play text-success"></i>
                        </button>
                        <button id="stop_server_config" type="button" class="btn btn-default" data-lan-id="Stop" disabled>
                            <i class="fa fa-power-off text-danger"></i>
                        </button>
                        <div class="btn-group">
                            <button id="more_server_config" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" data-lan-id="More" disabled>
                                <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu">
                                <li><a href="javascript:void(0);" data-lan-id="Stop_In" data-fun-name="StopAllUserIn"></a></li>
                                <li><a href="javascript:void(0);" data-lan-id="Allow_In" data-fun-name="AllowAllUserIn"></a></li>
                                <li><a href="javascript:onBtnWhiteList();" data-lan-id="White_List"></a></li>
                                <li role="separator" class="divider"></li>
                                <li><a href="javascript:void(0);" data-lan-id="Stop_Register" data-fun-name="StopAllUserRegister"></a></li>
                                <li><a href="javascript:void(0);" data-lan-id="Allow_Register" data-fun-name="AllowAllUserRegister"></a></li>
                                <li role="separator" class="divider"></li>
                                <li><a href="javascript:void(0);" data-lan-id="Open_Fight_Check" data-fun-name="StopFightCheck"></a></li>
                                <li><a href="javascript:void(0);" data-lan-id="Close_Fight_Check" data-fun-name="StartFightCheck"></a></li>
                                <li role="separator" class="divider"></li>
                                <li><a href="javascript:void(0);" data-lan-id="Import_all" data-fun-name="ReloadAllTableConfig"></a></li>
                                <li><a href="javascript:void(0);" data-lan-id="Import_all_but_activity" data-fun-name="ReloadAllTableConfigButActivity"></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!--/panel header-->

                <!--panel content-->
                <!--/.panel content-->
                <table id="svrTable"></table>
            </div>
            <!--/.panel-->
        </div>
    </div>

    <!-- /server.modal -->
    <div class="modal fade" id="modal_server" style="z-index: 1050" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue">
                        <label data-lan-id="Modal_title_new_server"></label>
                    </h4>
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue" hidden>
                        <label data-lan-id="Modal_title_modify_server"></label>
                    </h4>
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue" hidden>
                        <label data-lan-id="Modal_title_detail_server"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal" data-table-id="table_server" action="ServerConfig.aspx" method="post" onsubmit="AJAXSubmit(this, {onLoad: onSubmitSuccess}); return false;">
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_input_svr_id" data-lan-id="table_svr_th_svr_id"></label>
                            <div class="col-sm-8">
                                <input id="modal_input_svr_id" name="svr_id" class="form-control" type="text" placeholder="1-1-1-1" pattern="(([1-9][0-9]{0,2}-){3}[1-9][0-9]{0,2})" title="" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_input_svr_section" data-lan-id="table_svr_th_section"></label>
                            <div class="col-sm-8">
                                <input id="modal_input_svr_section" name="svr_section" class="form-control" type="number" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_input_svr_section_name" data-lan-id="table_svr_th_section_name"></label>
                            <div class="col-sm-8">
                                <input id="modal_input_svr_section_name" name="svr_section_name" class="form-control" type="text" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_input_svr_name" data-lan-id="table_svr_th_name"></label>
                            <div class="col-sm-8">
                                <input id="modal_input_svr_name" name="svr_name" class="form-control" type="text" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_input_svr_ip" data-lan-id="Modal_server_ip"></label>
                            <div class="col-sm-8">
                                <input id="modal_input_svr_ip" name="svr_ip" class="form-control" type="text" pattern="^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])(\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])){3}:\d+$" title="" placeholder="0.0.0.0:0000" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_input_auth_gm_http" data-lan-id="table_svr_th_auth_gm_http"></label>
                            <div class="col-sm-8">
                                <input id="modal_input_auth_gm_http" name="auth_gm_http" class="form-control" type="url" pattern="http://(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])(\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])){3}:\d+/gm" title="" placeholder="http://0.0.0.0:0000/gm" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" data-lan-id="table_svr_th_recommend"></label>
                            <div class="col-sm-8">
                                <label for="switch_server_recommend" class="ry-switch-cp">
                                    <input id="switch_server_recommend" name="svr_recommend" class="ry-switch-cp__input" type="checkbox">
                                    <div class="ry-switch-cp__box"></div>
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="select_server_status" data-lan-id="table_svr_th_status"></label>
                            <div class="col-sm-8">
                                <select id="select_server_status" name="svr_status" class="form-control">
                                    <option value="0" data-lan-id="svr_status_few"></option>
                                    <option value="1" data-lan-id="svr_status_crowd"></option>
                                    <option value="2" data-lan-id="svr_status_full"></option>
                                    <option value="3" data-lan-id="svr_status_maintaining"></option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_input_param" data-lan-id="table_svr_th_param"></label>
                            <div class="col-sm-8">
                                <input id="modal_input_param" name="svr_param" class="form-control" type="text" data-lan-id="Optional" data-lan-type="placeholder" placeholder="" />
                            </div>
                        </div>
                        <div class="well" style="height: 150px; overflow: hidden; overflow-y: auto;">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <label class="col-xs-4" data-lan-id="Modal_server_db_game"></label>
                                    <button class="btn btn-default col-xs-offset-6 col-xs-2" type="button" data-lan-id="Config" data-db-type="gamedb"></button>
                                    <input type="text" name="gamedb" hidden />
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <label class="col-xs-4" data-lan-id="Modal_server_db_code"></label>
                                    <button class="btn btn-default col-xs-offset-6 col-xs-2" type="button" data-lan-id="Config" data-db-type="codedb"></button>
                                    <input type="text" name="codedb" hidden />
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <label class="col-xs-4" data-lan-id="Modal_server_db_log"></label>
                                    <button class="btn btn-default col-xs-offset-6 col-xs-2" type="button" data-lan-id="Config" data-db-type="logdb"></button>
                                    <input type="text" name="logdb" hidden />
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <label class="col-xs-4" data-lan-id="Modal_server_db_auth"></label>
                                    <button class="btn btn-default col-xs-offset-6 col-xs-2" type="button" data-lan-id="Config" data-db-type="authdb"></button>
                                    <input type="text" name="authdb" hidden />
                                </div>
                            </div>
                        </div>
                        <button type="submit" name="ok" class="btn btn-default" data-lan-id="Modal_btn_ok"></button>
                        <button type="submit" name="modify" class="btn btn-default" data-lan-id="Modal_btn_modify"></button>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_close"></button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /server.modal -->

    <!-- /db.modal -->
    <div class="modal fade" id="modal_db" style="z-index: 1060" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue">
                        <label data-lan-id="Modal_title_db_config"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_db_input_name" data-lan-id="Modal_db_name"></label>
                            <div class="col-sm-8">
                                <input id="modal_db_input_name" class="form-control" type="text" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_db_input_host" data-lan-id="Modal_db_host"></label>
                            <div class="col-sm-8">
                                <input id="modal_db_input_host" class="form-control" type="text" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_db_input_port" data-lan-id="Modal_db_port"></label>
                            <div class="col-sm-8">
                                <input id="modal_db_input_port" class="form-control" type="text" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_db_input_user" data-lan-id="Modal_db_user"></label>
                            <div class="col-sm-8">
                                <input id="modal_db_input_user" class="form-control" type="text" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_db_input_pwd" data-lan-id="Modal_db_pwd"></label>
                            <div class="col-sm-8">
                                <input id="modal_db_input_pwd" class="form-control" type="text" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_db_input_cset" data-lan-id="Modal_db_cset"></label>
                            <div class="col-sm-8">
                                <input id="modal_db_input_cset" class="form-control" type="text" />
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_ok"></button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_modify"></button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_close"></button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /db.modal -->

    <!-- /multi_server.modal -->
    <div class="modal fade" id="modal_mutli_server" style="z-index: 1050" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue">
                        <label data-lan-id="Modal_title_multi_server_config"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_input_svr_id" data-lan-id="table_svr_th_svr_id"></label>
                            <div class="col-sm-8">
                                <input id="modal_input_multi_svr_id" name="svr_id" class="form-control" type="text" placeholder="1-1-1-1" pattern="(([1-9][0-9]{0,2}-){3}[1-9][0-9]{0,2})" title="" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="select_server_status" data-lan-id="table_svr_th_status"></label>
                            <div class="col-sm-8">
                                <select id="select_multi_server_status" name="svr_status" class="form-control">
                                    <option value="0" data-lan-id="svr_status_few"></option>
                                    <option value="1" data-lan-id="svr_status_crowd"></option>
                                    <option value="2" data-lan-id="svr_status_full"></option>
                                    <option value="3" data-lan-id="svr_status_maintaining"></option>
                                    <option value="4" data-lan-id="svr_status_close"></option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" data-lan-id="table_svr_th_recommend"></label>
                            <div class="col-sm-8">
                                <label for="switch_multi_server_recommend" class="ry-switch-cp">
                                    <input id="switch_multi_server_recommend" name="multi_svr_recommend" class="ry-switch-cp__input" type="checkbox">
                                    <div class="ry-switch-cp__box"></div>
                                </label>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="confirmServerStatus();" data-lan-id="Modal_btn_modify"></button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_close"></button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /multi.modal -->

    <!-- /white_list.modal -->
    <div class="modal fade" id="modal_white_list" style="z-index: 1050" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue">
                        <label data-lan-id="White_List"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="modal_white_list_ip" data-lan-id="IP"></label>
                            <div class="col-sm-8">
                                <textarea id="modal_white_list_ip" class="form-control" data-lan-id="sepBycomma-space-return" data-lan-type="placeholder" placeholder=""></textarea>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="onAddWhiteListIp();" data-lan-id="Modal_btn_modify"></button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_close"></button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /white_list.modal -->

    <!-- /open-door server.modal -->
    <div class="modal fade" id="modal_server_open_door" style="z-index: 1060" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue">
                        <label data-lan-id="Open-door"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-4 control-label" data-lan-id="SetTimer"></label>
                            <div class="col-sm-8">
                                <label for="switch_is_set_timer" class="ry-switch-cp">
                                    <input id="switch_is_set_timer" class="ry-switch-cp__input" type="checkbox"/>
                                    <div class="ry-switch-cp__box"></div>
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label" for="datetimepicker" data-lan-id="Open-door_time"></label>
                            <div class="col-sm-8">
                                <input class="form-control" type="text" id="datetimepicker" data-lan-id="Open-door_time" data-lan-type="placeholder" placeholder="" value="" />
                            </div>
                        </div>
                        <div class="well well-lg">

                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="" data-lan-id="Modal_btn_modify"></button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_close"></button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /open-door server.modal -->

</body>
</html>
<script type="text/javascript" src="../bootstrap/js/bootstrap-table.min.js"></script>
<script type="text/javascript" src="../bootstrap/js/locales/bootstrap-table-locale-all.js"></script>
<script>
    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales[language_type]);
    var sprintf = $.fn.bootstrapTable.utils.sprintf;
    var $table = $('#svrTable');
    var $modal_server_config = $('#modal_server');
    var $modal_db_config = $('#modal_db');
    var $modal_multi_server_config = $('#modal_mutli_server');

    $(document).ready(function () {
        initTable($table);
    });

    function initTable(tableEle) {
        tableEle.bootstrapTable("destroy");
        tableEle.bootstrapTable({
            height: $(window).height() - $(header).outerHeight() - $('#content').outerHeight(),
            clickToSelect: true,
            method: 'post',
            url: "ServerConfig.aspx/GetServerInfoList",
            contentType: "application/json",
            classes: "table table-no-bordered",
            responseHandler: function (res) {
                return JSON.parse(res.d);
            },
            columns: [
                { checkbox: true, align: 'center', valign: 'middle', },
                { title: 'ID', field: 'id', width: '10%', align: 'center', valign: 'middle', cellStyle: { classes: 'td-ellipsis' }, },
                { title: GetContentMsg("table_svr_th_svr_id"), field: 'svrID', width: '10%', align: 'center', valign: 'middle', cellStyle: { classes: 'td-ellipsis' } },
                { title: GetContentMsg("table_svr_th_section"), field: 'section', width: '10%', align: 'center', valign: 'middle', },
                { title: GetContentMsg("table_svr_th_section_name"), field: 'sectionName', width: '10%', align: 'center', valign: 'middle', cellStyle: { classes: 'td-ellipsis' }, visible: false },
                { title: GetContentMsg("table_svr_th_name"), field: 'name', width: '10%', align: 'center', valign: 'middle', cellStyle: { classes: 'td-ellipsis' } },
                { title: GetContentMsg("table_svr_th_ip"), field: 'ip', width: '10%', align: 'center', valign: 'middle', cellStyle: { classes: 'td-ellipsis' } },
                { title: GetContentMsg("table_svr_th_recommend"), field: 'recommend', align: 'center', valign: 'middle', formatter: boolFormatter },
                {
                    title: GetContentMsg("table_svr_th_status"), field: 'status', align: 'center', valign: 'middle', formatter: function (value) {
                        return $('#select_server_status')[0][value].innerHTML;
                    }
                },
                { title: GetContentMsg("table_svr_th_param"), field: 'param', align: 'center', valign: 'middle', },
                {
                    field: 'opt',
                    align: 'center',
                    width: '15%',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return ['<a class="modify btn btn-default" ><i class="fa fa-gear"></i></a> <a class="delete btn btn-danger" ><i class="fa fa-remove"></i></a>'].join('');
                    },
                    events: {
                        'click .delete': function (e, value, row, index) {
                            if (!confirm(GetContentMsg('Confirm_delete')))
                                return;
                            var ids = [row.id];
                            removeServers(ids);
                        },
                        'click .modify': function (e, value, row, index) {
                            $modal_server_config.find('[data-lan-id="Modal_btn_ok"]').hide();
                            $modal_server_config.find('[data-lan-id="Modal_btn_modify"]').show();
                            $modal_db_config.find('[data-lan-id="Modal_btn_ok"]').hide();
                            $modal_db_config.find('[data-lan-id="Modal_btn_modify"]').show();
                            $('[data-lan-id="Modal_title_new_server"]').parent().hide();
                            $('[data-lan-id="Modal_title_modify_server"]').parent().hide();
                            $('[data-lan-id="Modal_title_detail_server"]').parent().show();

                            $('#modal_input_svr_id').val(row.svrID);
                            $('#modal_input_svr_id').prop('disabled', true);
                            $('#modal_input_svr_section').val(row.section);
                            $('#modal_input_svr_section_name').val(row.sectionName);
                            $('#modal_input_svr_name').val(row.name);
                            $('#modal_input_svr_ip').val(row.ip);
                            $('#modal_input_auth_gm_http').val(row.authGMHttp);
                            $('#switch_server_recommend')[0].checked = row.recommend;
                            $('#select_server_status')[0].selectedIndex = row.status;
                            $('#modal_input_param').val(row.param);
                            $('[data-db-type]').each(function () {
                                var dbconfig = row[$(this).attr('data-db-type')];
                                $(this).siblings('input').val(JSON.stringify(dbconfig));
                            });
                            $modal_server_config.modal({ keyboard: false });
                        }
                    },
                },
            ]
        });
    };

    $table.on('check.bs.table uncheck.bs.table ' +
                'check-all.bs.table uncheck-all.bs.table', function () {
                    $('#del_server_config').prop('disabled', !$table.bootstrapTable('getSelections').length);
                    $('#modify_server_config').prop('disabled', !$table.bootstrapTable('getSelections').length);
                    $('#detail_server_config').prop('disabled', !$table.bootstrapTable('getSelections').length);
                    $('#merge_server_config').prop('disabled', !($table.bootstrapTable('getSelections').length > 1));
                    $('#start_server_config').prop('disabled', !$table.bootstrapTable('getSelections').length);
                    $('#stop_server_config').prop('disabled', !$table.bootstrapTable('getSelections').length);
                    $('#more_server_config').prop('disabled', !$table.bootstrapTable('getSelections').length);
                });

    var backdropz_index = 1040;
    var modalOpened = [];
    $(".modal").on('show.bs.modal', function () {
        modalOpened.push(this);
        var index = backdropz_index + 10 * ($('.modal-backdrop').length);
        $('.modal-backdrop').first().css('z-index', index);
    });

    $(".modal").on('hide.bs.modal', function () {
        var index = backdropz_index + 10 * ($('.modal-backdrop').length - 2);
        $('.modal-backdrop').first().css('z-index', index);
    });

    $(".modal").on('hidden.bs.modal', function () {
        modalOpened.pop();
        if (modalOpened.length != 0) {
            $("body").addClass('modal-open');
        }
    });

    function resetModal(modalEle) {
        var vform = modalEle.find('form')[0];
        for (var i = 0; i < vform.length; i++) {
            vform[i].value = '';

            if (vform[i].type == 'checkbox') {
                vform[i].checked = false;
            }
            else if (vform[i].type == 'select-one') {
                vform[i].selectedIndex = -1;
            }
        }
    }

    $("#add_server_config").on('click', function () {
        $modal_server_config.find('[data-lan-id="Modal_btn_ok"]').show();
        $modal_server_config.find('[data-lan-id="Modal_btn_modify"]').hide();
        $modal_db_config.find('[data-lan-id="Modal_btn_ok"]').show();
        $modal_db_config.find('[data-lan-id="Modal_btn_modify"]').hide();
        $('[data-lan-id="Modal_title_new_server"]').parent().show();
        $('[data-lan-id="Modal_title_modify_server"]').parent().hide();
        $('[data-lan-id="Modal_title_detail_server"]').parent().hide();
        $('#modal_input_svr_id').prop('disabled', false);
        resetModal($modal_server_config);
        $modal_server_config.modal({ keyboard: false });
    });

    $('[data-lan-id="Config"]').on('click', function () {
        var db_config = $(this).siblings('input').val();
        if (db_config == "null" || !db_config) {
            resetModal($modal_db_config);
        } else {
            db_config = JSON.parse(db_config);
            $('#modal_db_input_name').val(db_config['name']);
            $('#modal_db_input_host').val(db_config['host']);
            $('#modal_db_input_port').val(db_config['port']);
            $('#modal_db_input_user').val(db_config['user']);
            $('#modal_db_input_pwd').val(db_config['pwd']);
            $('#modal_db_input_cset').val(db_config['cset']);
        }

        $modal_db_config.attr('data-db-config', $(this).attr('data-db-type'));
        $modal_db_config.modal({ keyboard: false });
    });

    $('#modal_db button[data-lan-id="Modal_btn_ok"], #modal_db button[data-lan-id="Modal_btn_modify"]').on('click', function () {
        var db_config = {};
        db_config['name'] = $('#modal_db_input_name').val()
        db_config['host'] = $('#modal_db_input_host').val()
        db_config['port'] = $('#modal_db_input_port').val()
        db_config['user'] = $('#modal_db_input_user').val()
        db_config['pwd'] = $('#modal_db_input_pwd').val()
        db_config['cset'] = $('#modal_db_input_cset').val()
        var db_type = $('#modal_db').attr('data-db-config');
        $('button[data-db-type="' + db_type + '"]').siblings('input').val(JSON.stringify(db_config));
    });

    function removeServers(ids) {
        var param = {
            ids: ids
        }
        $.ajax({
            type: "post",
            url: "ServerConfig.aspx/RemoveServerInfo",
            data: JSON.stringify(param),
            async: false,
            contentType: "application/json",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var json = JSON.parse(ack_data.d)
                if (json.length != 0) {
                    $table.bootstrapTable('remove', {
                        field: 'id',
                        values: json
                    });
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        })
    }

    function getRowSelections(table) {
        return $.map(table.bootstrapTable('getSelections'), function (row) {
            return row.id;
        });
    }

    function delServerConfig() {
        if (!confirm(GetContentMsg('Confirm_delete')))
            return;
        var ids = getRowSelections($table);
        removeServers(ids);
        $(this).prop('disabled', true);
    }

    $('#del_server_config').on('click', function () {
        delServerConfig();
    });

    function onSubmitSuccess(res) {
        var json = JSON.parse(res);
        if (json.error == 0) {
            $table.bootstrapTable("refresh");
            alert("SUCCESS");
        }
    }

    function boolFormatter(value) {
        if (value == true) {
            return '<i class="glyphicon glyphicon-ok text-success"></i>'
        } else {
            return '<i class="glyphicon glyphicon-remove text-danger"></i>'
        }
    }

    function modifyServer(rows) {
        $('#modal_input_multi_svr_id').val(rows.map(function (value) {
            return value.svrID;
        }).join(', '));
        $('#modal_input_multi_svr_id').prop('disabled', true);
        $('#switch_multi_server_recommend')[0].checked = rows[0].recommend;
        $('#select_multi_server_status')[0].selectedIndex = rows[0].status;

        $modal_multi_server_config.modal({ keyboard: false });
    }

    $('#modify_server_config').on('click', function () {
        var rows = $table.bootstrapTable('getSelections');
        modifyServer(rows);
    });

    function confirmServerStatus() {
        var param = {
            ids: getRowSelections($table),
            bIsRecommend: $('#switch_multi_server_recommend')[0].checked,
            nListStatus: $('#select_multi_server_status').val()
        };

        $.ajax({
            type: "post",
            url: "ServerConfig.aspx/ModifyServerStatus",
            data: JSON.stringify(param),
            async: false,
            contentType: "application/json",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var json = JSON.parse(ack_data.d)
                if (json.error == 0) {
                    $table.bootstrapTable("refresh");
                    alert('SUCCESS!');
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        })
    };

    $('#stop_server_config').on('click', function () {
        var $btn = $(this);
        var param = {
            ids: getRowSelections($table),
        };

        $.ajax({
            type: "post",
            url: "ServerConfig.aspx/StopServer",
            data: JSON.stringify(param),
            async: false,
            contentType: "application/json",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var json = JSON.parse(ack_data.d.replace(/\n/g, "\\n"))
                if (json.ids.length != 0) {
                    $btn.prop('disabled', true);
                    $btn.html(GetContentMsg('Stopping') + ' <i class="fa fa-spinner btn-loading"></i>');
                    setTimeout(function () {
                        $btn.prop('disabled', false);
                        $btn.html(GetContentMsg('Stop') + ' <i class="fa fa-power-off text-danger"></i>');
                    }, 60000);
                }

                if (json.msg.length != 0) {
                    alert(json.msg);
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        })
    });


    $('[data-fun-name]').on('click', function () {
        var fun = $(this).attr('data-fun-name');
        var param = {
            svrIds: getRowSelections($table),
        };

        $.ajax({
            type: "post",
            url: 'Serverconfig.aspx/' + fun,
            data: JSON.stringify(param),
            async: false,
            contentType: "application/json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var json = JSON.parse(ack_data.d.replace(/\n/g, "\\n"))
                var json = JSON.parse(ack_data.d)
                if (json.msg.length != 0) {
                    alert(json.msg);
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        })
    })

    function onBtnWhiteList() {
        $('#modal_white_list').modal({ keyboard: false });
    }

    function onAddWhiteListIp() {
        var ipTextArea = $('#modal_white_list_ip').val();
        var reg = /^((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])(\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])){3}[ ,\r\n]?)*$/;
        if (!reg.test(ipTextArea)) {
            alert("ERROR INPUT");
            return;
        }

        var param = {
            svrIds: getRowSelections($table),
            ips: ipTextArea.split(/[, \r\n]/)
        };

        $.ajax({
            type: "post",
            url: 'Serverconfig.aspx/AddWhiteList',
            data: JSON.stringify(param),
            async: false,
            contentType: "application/json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var json = JSON.parse(ack_data.d.replace(/\n/g, "\\n"))
                var json = JSON.parse(ack_data.d)
                if (json.msg.length != 0) {
                    alert(json.msg);
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        })
    };

    $('#start_server_config').on('click', function () {
        $('#datetimepicker').prop('disabled', !$('#switch_is_set_timer')[0].checked);
        $('#modal_server_open_door').modal({keyboard:false});
    })

    $('#switch_is_set_timer').on('change', function () {
        $('#datetimepicker').prop('disabled', !$('#switch_is_set_timer')[0].checked);
    });

</script>
<script src="../bootstrap/js/bootstrap-datetimepicker.js" charset="utf-8"></script>
<script src="../bootstrap/js/locales/bootstrap-datetimepicker.ko.js" charset="utf-8"></script>
<script src="../bootstrap/js/locales/bootstrap-datetimepicker.zh-CN.js" charset="utf-8"></script>
<script>
    $('#datetimepicker').datetimepicker({
        language: language_type,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 0,
        format: 'yyyy-mm-dd hh:ii',
        forceParse: 1
    });
</script>
