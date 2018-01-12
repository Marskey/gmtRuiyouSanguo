<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlayerHistory.aspx.cs" Inherits="gmt.playerHistroy" %>

<!DOCTYPE html>

<html >
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link href="../bootstrap/css/bootstrap-datetimepicker.min.css" rel="stylesheet" media="screen" />
    <link href="../bootstrap/css/bootstrap-table.min.css" rel="stylesheet" media="screen" />
    <link href="../mycss/docs.min.css" rel="stylesheet" media="screen" />
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <header class="navbar navbar-static-top bs-docs-nav" id="header" ></header>
    <div class="bs-docs-header" id="content">
        <div class="container">
            <h1 data-lan-id="PlayerHistory_title"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <form class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-4" for="server_list" id="lan_PlayerHistory_server_list" data-lan-id="PlayerHistory_server_list"></label>
                        <div class="col-sm-8">
                            <select id="server_list" class="form-control"></select>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-sm-4" for="type_select" data-lan-id="PlayerHistory_type"></label>
                        <div class="col-sm-8">
                            <select id="type_select" class="col-sm-6 form-control">
                                <option value="0" data-lan-id="PlayerHistory_type0"></option>
                                <option value="1" data-lan-id="PlayerHistory_type1"></option>
                                <option value="2" data-lan-id="PlayerHistory_type2"></option>
                            </select>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-sm-4" for="player_id" data-lan-id="PlayerHistory_palyer_id"></label>
                        <div class="col-sm-8">
                            <input id="player_id" class="col-sm-6 form-control" size="25" type="text" placeholder="" data-lan-id="PlayerHistory_player_id_input_tip" data-lan-type="placeholder" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-sm-4" for="datetimepicker_1" data-lan-id="PlayerHistory_start_time"></label>
                        <div class="col-sm-8">
                            <input class="form-control" size="25" type="text" placeholder="" value="" id="datetimepicker_1" runat="server" name="datetimepicker_1" data-lan-id="PlayerHistory_start_time_input_tip" data-lan-type="placeholder" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-4" for="datetimepicker_2" data-lan-id="PlayerHistory_end_time"></label>
                        <div class="col-sm-8">
                            <input class="form-control" size="25" type="text" placeholder="" value="" id="datetimepicker_2" runat="server" name="datetimepicker_2" data-lan-id="PlayerHistory_end_time_input_tip" data-lan-type="placeholder" />
                        </div>
                    </div>

                    <hr />

                    <div class="form-group">
                        <div class="col-sm-3">
                            <button class="btn btn-danger form-control" type="button" onclick="btnQueryData()">
                                <i class="glyphicon glyphicon-search"></i>
                            </button>
                        </div>
                    </div>
                </form>

                <div>
                    <table id="table"></table>
                </div>
            </div>
    </div>
</body>
</html>

<script src="../bootstrap/js/bootstrap-datetimepicker.js" charset="utf-8"></script>
<script src="../bootstrap/js/locales/bootstrap-datetimepicker.ko.js" charset="utf-8"></script>
<script src="../bootstrap/js/locales/bootstrap-datetimepicker.zh-CN.js" charset="utf-8"></script>
<script>
    $('#datetimepicker_1').datetimepicker({
        language: language_type,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 0,
        format: 'yyyy-mm-dd hh:ii',
        forceParse: 1
    });

    $('#datetimepicker_2').datetimepicker({
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

<script type="text/javascript" src="../bootstrap/js/bootstrap-table.min.js"></script>
<script type="text/javascript" src="../bootstrap/js/locales/bootstrap-table-locale-all.js"></script>
<script type="text/javascript" src="../bootstrap/js/extensions/toolbar/bootstrap-table-toolbar.js"></script>
<script>
    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales[language_type]);

    $(document).ready(function () {
        queryServerList();
        initTable();
    })

    function initTable() {
        $("#table").bootstrapTable("destroy");
        $("#table").bootstrapTable({
            method: 'post',
            height: $(window).height(),
            contentType: "application/json",
            striped: true, // 隔行变色
            //search: true,
            advancedSearch: true,
            idTable: "table",
            pagination: true,
            sidePagination: 'server',
            queryParamsType: 'limit',//查询参数组织方式
            queryParams: queryParams,//请求服务器时所传的参数
            responseHandler: responseHandler,
            sortable: true,
            sortName: "date",
            sortOrder: "desc",
            //showRefresh: true,
            //clickToSelect: true,
            pageSize: 50,
            pageList: [50, 100, 150, 200, 250, 300],
            columns: [
                {
                    title: GetContentMsg("PlayerHistory_time"),
                    field: 'date',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    searchable: false,
                },
                {
                    title: GetContentMsg("PlayerHistory_palyer_id"),
                    field: 'uid',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    searchable: false,
                },
                {
                    title: GetContentMsg("PlayerHistory_action"),
                    field: 'action',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == 1) {
                            return "<div class=\"label label-success\" style=\"font-size: 12px\">" + GetContentMsg("PlayerHistory_get") + "</div>";
                        }
                        else {
                            return "<div class=\"label label-danger\" style=\"font-size: 12px\">" + GetContentMsg("PlayerHistory_expand") + "</div>";
                        }
                    },
                },
                {
                    title: GetContentMsg("PlayerHistory_channel_id"),
                    field: 'channel_id',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: GetContentMsg("PlayerHistory_team_level"),
                    field: 'team_level',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    searchable: false,
                },
                {
                    title: GetContentMsg("PlayerHistory_cause_name"),
                    field: 'cause_name',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: GetContentMsg("PlayerHistory_item_type_name"),
                    field: 'item_type_name',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: GetContentMsg("PlayerHistory_item_name"),
                    field: 'item_name',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: GetContentMsg("PlayerHistory_quantity"),
                    field: 'quantity',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: GetContentMsg("PlayerHistory_total"),
                    field: 'total',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == -1) {
                            return "-";
                        }
                        return value;
                    },
                },
                {
                    title: GetContentMsg("PlayerHistory_vip"),
                    field: 'vip',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
            ],
        });
    }

    function initHeroTable() {
        $("#table").bootstrapTable("destroy");
        $("#table").bootstrapTable({
            method: 'post',
            height: $(window).height(),
            contentType: "application/json",
            striped: true, // 隔行变色
            advancedSearch: true,
            idTable: "table",
            pagination: true,
            sidePagination: 'server',
            queryParamsType: 'limit',//查询参数组织方式
            queryParams: queryParams,//请求服务器时所传的参数
            responseHandler: responseHandler,
            sortable: true,
            sortName: "date",
            sortOrder: "desc",
            //showRefresh: true,
            //clickToSelect: true,
            pageSize: 50,
            pageList: [50, 100, 150, 200, 250, 300],
            columns: [
                {
                    title: '시간',
                    field: 'date',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    searchable: false,
                },
                {
                    title: '유저ID',
                    field: 'uid',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    searchable: false,
                },
                {
                    title: '마켓번호',
                    field: 'channel_id',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: '레벨',
                    field: 'team_level',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    searchable: false,
                },
                {
                    title: 'TYPE',
                    field: 'type_name',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: 'Hero',
                    field: 'hero_name',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == -1) {
                            return "-";
                        }
                        return value;
                    },
                },
                {
                    title: 'Target',
                    field: 'target_id',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == -1) {
                            return "-";
                        }
                        return value;
                    },
                },
                {
                    title: 'Value Before',
                    field: 'value_bf',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == -1) {
                            return "-";
                        }
                        return value;
                    },
                },
                {
                    title: 'Value After',
                    field: 'value_af',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == -1) {
                            return "-";
                        }
                        return value;
                    },
                },
                {
                    title: 'Hero ZDL Before',
                    field: 'zdl_bf',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == -1) {
                            return "-";
                        }
                        return value;
                    },
                },
                {
                    title: 'Hero ZDL After',
                    field: 'zdl_af',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == -1) {
                            return "-";
                        }
                        return value;
                    },
                },
                {
                    title: 'Team ZDL Before',
                    field: 'team_zdl_bf',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == -1) {
                            return "-";
                        }
                        return value;
                    },
                },
                {
                    title: 'Team ZDL After',
                    field: 'team_zdl_af',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == -1) {
                            return "-";
                        }
                        return value;
                    },
                },
                {
                    title: 'VIP레벨',
                    field: 'vip',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
            ],
        });

    }
    //请求服务数据时所传参数
    function queryParams(params) {
        var p = {
            search: (params.filter) ? params.filter : "",
            limit: params.limit,
            offset: params.offset,
            sort: (params.sort) ? params.sort : "",
            order: params.order
        }
        return p;
    }

    function responseHandler(res) {
        data_json = JSON.parse(res.d);
        if (data_json.error != 0) {
            alert(GetContentMsg("PlayerHistory_query_failed"));
            return ""
        }
        return data_json.data;
    }

    function btnQueryData() {
        if ($('#player_id').val().length == 0) {
            $('#player_id').val(0)
        }

        if ($(server_list).val().length == 0) {
            alert(GetContentMsg("PlayerHistory_server_0"));
            return;
        }

        if ($(type_select).val() == 2) {
            initHeroTable();
        }
        else {
            initTable();
        }

        var param = {
            server_id: $('#server_list').val(),
            query_type: $('#type_select').val(),
            playerId: $('#player_id').val(),
            start_time: $('#datetimepicker_1').val(),
            end_time: $('#datetimepicker_2').val(),
        }

        $.ajax(
        {
            type: "POST",
            url: "PlayerHistory.aspx/BtnQuery",
            data: JSON.stringify(param),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var data_json = JSON.parse(ack_data.d);
                if (data_json.error == 0) {
                    $('#table').bootstrapTable('refresh', {url: 'PlayerHistory.aspx/QueryPlayerHistoryData'});
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        }
        );
    }

    function queryServerList() {
        $.ajax(
        {
            type: "POST",
            url: "PlayerHistory.aspx/GetServerList",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var data_json = JSON.parse(ack_data.d);
                if (data_json[0].error == 1) {
                    alert(GetContentMsg("PlayerHistory_get_server_failed"));
                }
                else {
                    for (var i = 0; i < data_json.length; i++) {
                        $("#server_list").append('<option value="' + data_json[i].id + '">' + data_json[i].name + '</option>');
                    }
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        }
        );
    }

</script>

