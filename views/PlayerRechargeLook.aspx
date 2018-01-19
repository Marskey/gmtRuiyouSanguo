<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlayerRechargeLook.aspx.cs" Inherits="gmt.PlayerRechargeLook" %>

<!DOCTYPE html>

<html >
<head runat="server">
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
            <h1 data-lan-id="PlayerRechargeLook"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <form class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-4" data-lan-id="PlayerHistory_palyer_id"></label>
                        <div class="col-sm-8">
                            <input class="form-control" size="25" type="text" placeholder="" id="playerId" style="width: 300px" data-lan-id="PlayerHistory_player_id_input_tip" data-lan-type="placeholder" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-sm-4" data-lan-id="PlayerHistory_start_time"></label>
                        <div class="col-sm-8">
                            <input class="form-control" size="25" type="text" placeholder="" value="" id="datetimepicker_1" runat="server" name="datetimepicker_1" data-lan-id="PlayerHistory_start_time_input_tip" data-lan-type="placeholder" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-sm-4" data-lan-id="PlayerHistory_end_time"></label>
                        <div class="col-sm-8">
                            <input class="form-control" size="25" type="text" placeholder="" value="" id="datetimepicker_2" runat="server" name="datetimepicker_2" data-lan-id="PlayerHistory_end_time_input_tip" data-lan-type="placeholder" />
                        </div>
                    </div>
                    <hr />

                    <div class="form-group">
                        <div class="col-sm-3">
                            <button class="btn btn-danger form-control" type="button" onclick="btnQueryRecharge()">
                                <i class="glyphicon glyphicon-search"></i>
                            </button>
                        </div>
                    </div>
                    <hr />


                    <div class="form-group">
                        <label class="col-sm-4" data-lan-id="Current_Order_Total_Cost"></label>
                        <div class="col-sm-8">
                            <input class="form-control" size="25" type="text" placeholder="0" id="totalCost" readonly="true" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-sm-4" data-lan-id="Current_Recharge_Player_Count"></label>

                        <div class="col-sm-8">
                            <input class="form-control" size="25" type="text" placeholder="0" id="playerCnt" readonly="true" />
                        </div>
                    </div>
                </form>

                <div class="col-md-12">
                    <table id="table"></table>
                </div>
            </div>
    </div>
</body>
</html>

<script src="../bootstrap/js/bootstrap-datetimepicker.js" charset="utf-8"></script>
<script src="../bootstrap/js/locales/bootstrap-datetimepicker.ko.js" charset="utf-8"></script>
<script>
    $('#datetimepicker_1').datetimepicker({
        language: language_type,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 0,
        format: 'yyyy-mm-dd  hh:ii',
        forceParse: 1
    });

    $('#datetimepicker_2').datetimepicker({
        language: language_type,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 0,
        format: 'yyyy-mm-dd  hh:ii',
        forceParse: 1
    });

</script>

<script type="text/javascript" src="../bootstrap/js/bootstrap-table.min.js"></script>
<script type="text/javascript" src="../bootstrap/js/locales/bootstrap-table-locale-all.min.js"></script>
<script type="text/javascript" src="../bootstrap/js/extensions/toolbar/bootstrap-table-toolbar.min.js"></script>
<script>
    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales[language_type]);

    $(document).ready(function () {
        InitTable();
        queryPlayerCnt();
        queryTotalCost();
    })

    function InitTable() {
        $("#table").bootstrapTable("destroy");
        $("#table").bootstrapTable({
            method: 'post',
            url: "PlayerRechargeLook.aspx/UpdatePlayerRechargeTableData",
            contentType: "application/json",
            height: $(window).height(),
            striped: true, // 隔行变色
            advancedSearch: true,
            idTable: "table",
            pagination: true,
            pageSize: 50,
            pageList: [50, 100, 150, 200, 250, 300],
            pageNumber: 1,
            sidePagination: 'server',
            queryParamsType: 'limit',
            sortName: 'last_update_time',
            sortOrder: 'desc',
            queryParams: queryParams,
            responseHandler: responseHandler,
            onBtnColumnAdvancedSearch: onBtnColumnAdvancedSearch,
            showRefresh: true,
            columns: [
                {
                    title: 'OrderId',
                    field: 'plat_order_id',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: GetContentMsg("PlayerHistory_palyer_id"),
                    field: 'userID',
                    align: 'center',
                    valign: 'middle',
                    searchable: false,
                    sortable: true,
                },
                {
                    title: 'type',
                    field: 'raw_type',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    title: 'CYUID',
                    field: 'cyUid',
                    align: 'center',
                    valign: 'middle',
                    //sortable: true,
                },
                {
                    title: GetContentMsg("PlayerRechargeLook_server_id"),
                    field: 'serverIdStr',
                    align: 'center',
                    valign: 'middle',
                    //sortable: true,
                },
                {
                    title: GetContentMsg("PlayerRechargeLook_goodsName"),
                    field: 'goodsName',
                    align: 'center',
                    valign: 'middle',
                    searchable: false,
                    //sortable: true,
                },
                {
                    title: GetContentMsg("PlayerRechargeLook_goodsCost"),
                    field: 'goodsCost',
                    align: 'center',
                    valign: 'middle',
                    searchable: false,
                    //sortable: true,
                },
                {
                    title: GetContentMsg("PlayerHistory_time"),
                    field: 'last_update_time',
                    align: 'center',
                    valign: 'middle',
                    searchable: false,
                    sortable: true,
                },
                {
                    title: GetContentMsg("PlayerRechargeLook_state"),
                    field: 'state',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
            ],
        });

    }

    function queryParams(params) {
        var param = {
            filter: (params.filter) ? params.filter : "",
            limit: params.limit,
            offset: params.offset,
            sort: params.sort,
            order: params.order,
        };
        return param;
    }

    function responseHandler(res) {
        data_json = JSON.parse(res.d);
        if (data_json.error != 0) {
            alert(GetContentMsg("PlayerHistory_query_failed"));
            return ""
        }
        return data_json.data;
    }

    function btnQueryRecharge() {
        if ($('#playerId').val().length == 0) {
            $('#playerId').val(0)
        }

        var param = {
            playerId: $('#playerId').val(),
            start_time: $('#datetimepicker_1').val(),
            end_time: $('#datetimepicker_2').val()
        }

        $.ajax(
        {
            type: "POST",
            url: "PlayerRechargeLook.aspx/BtnQuery",
            data: JSON.stringify(param),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var data_json = JSON.parse(ack_data.d);
                if (data_json.error == 0) {
                    $('#table').bootstrapTable('refresh');
                    queryPlayerCnt();
                    queryTotalCost();
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

    function onBtnColumnAdvancedSearch(filter) {
        queryPlayerCnt(filter);
        queryTotalCost(filter);
    }
</script>

<script>
    function queryTotalCost(filter) {
        if (!filter) filter = "";
        $('#totalCost').val("searching...");
        $.ajax(
        {
            type: "POST",
            url: "PlayerRechargeLook.aspx/QueryTotalCost",
            async: true,
            data: JSON.stringify({"filter":filter}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                data_json = JSON.parse(ack_data.d);
                if (data_json.error == 1) {
                    alert(GetContentMsg("PlayerHistory_query_failed"));
                }
                else {
                    $('#totalCost').val(data_json.totalCost);
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        });
    }

    function queryPlayerCnt(filter) {
        if (null != $('#playerId').val() && "" != $('#playerId').val()) {
            $('#playerCnt').val(1);
            return;
        }

        if (!filter) filter = "";

        $('#playerCnt').val("searching...");
        $.ajax(
        {
            type: "POST",
            url: "PlayerRechargeLook.aspx/QueryPlayerCnt",
            async: true,
            data: JSON.stringify({"filter":filter}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                data_json = JSON.parse(ack_data.d);
                if (data_json.error == 1) {
                    alert(GetContentMsg("PlayerHistory_query_failed"));
                }
                else {
                    $('#playerCnt').val(data_json.playerCnt);
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        });
    }

</script>
