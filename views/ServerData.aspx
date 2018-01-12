<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServerData.aspx.cs" Inherits="gmt.views.ServerData" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<!DOCTYPE html>

<html >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
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
             <h1 data-lan-id="ServerData"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <h1 class="page-header" data-lan-id="ServerData"></h1>
                <form class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-4" for="select_channel" id="lan_ServerData_channel" data-lan-id="ServerData_channel"></label>
                        <div class="col-sm-8">
                            <select class="form-control" id="select_channel">
                                <option value="">All</option>
                                <option value="lzsgkr_google">Google</option>
                                <option value="lzsgkr_onestore">OneStore</option>
                                <option value="lzsgkr_ios">IOS</option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-4" for="datetimepicker_1" data-lan-id="PlayerHistory_start_time"></label>
                        <div class="col-sm-8">
                            <input class="form-control" size="25" type="text" placeholder="" value="" id="datetimepicker_1" runat="server" name="datetimepicker_1" data-lan-id="ServerData_start_time_placeholder" data-lan-type="placeholder" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-sm-4" for="datetimepicker_2" data-lan-id="PlayerHistory_end_time"></label>
                        <div class="col-sm-8">
                            <input class="form-control" size="25" type="text" placeholder="" value="" id="datetimepicker_2" runat="server" name="datetimepicker_2" data-lan-id="ServerData_end_time_placeholder" data-lan-type="placeholder" />
                        </div>
                    </div>
                    <hr />

                    <div class="form-group">
                        <div class="col-sm-3">
                            <button class="btn btn-danger form-control" type="button" onclick="queryServerData()">
                                <i class="glyphicon glyphicon-search"></i>
                            </button>
                        </div>
                    </div>
                </form>
                <div>
                    <table id="table_server_data"></table>
                </div>
            </div>
    </div>
</body>
<script src="http://cdn.static.runoob.com/libs/jquery/2.1.1/jquery.min.js"></script>
<script src="../bootstrap/js/bootstrap-datetimepicker.js" charset="utf-8"></script>
<script src="../bootstrap/js/locales/bootstrap-datetimepicker.ko.js" charset="utf-8"></script>
<script>
    $('#datetimepicker_1').datetimepicker({
        language: language_type,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 3,
        format: 'yyyy-mm-dd 00:00:00',
        forceParse: 1
    });
    $('#datetimepicker_1').datetimepicker('setValue');

    $('#datetimepicker_2').datetimepicker({
        language: language_type,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 3,
        format: 'yyyy-mm-dd 23:59:59',
        forceParse: 1
    });
    $('#datetimepicker_2').datetimepicker('setValue');
</script>

<script type="text/javascript" src="../bootstrap/js/bootstrap-table.min.js"></script>
<script type="text/javascript" src="../bootstrap/js/locales/bootstrap-table-locale-all.js"></script>
<script>
    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales[language_type]);
    $(document).ready(function () {
        initServerTable();
        initSevenDataTable();
    })

    function sumFormatter(data, field) {
        if (data.length == 0)
            return "-";
        if (field == null) {
            field = this.field;
        }
        return data.reduce(function (sum, row) {
            return sum + (+row[field]);
        }, 0);
    }

    function initServerTable(dt) {
        $("#table_server_data").bootstrapTable("destroy");
        $("#table_server_data").bootstrapTable({
            data: dt,
            height: $(window).height(),
            pagination: true,
            sidePagination: 'client',
            pageSize: 10,
            pageList: [10, 20, 50, 100, GetContentMsg("ServerData_all")],
            striped: true, // 隔行变色
            showFooter: true,
            search: true,
            columns: [
                {
                    title: GetContentMsg("PlayerHistory_server_list"),
                    field: 'section',
                    align: 'center',
                    valign: 'middle',
                    footerFormatter: function () { return GetContentMsg("PlayerHistory_total_count"); }
                },
                {
                    title: GetContentMsg("ServerData_serverName"),
                    field: 'serverName',
                    align: 'center',
                    valign: 'middle',
                    footerFormatter: function (data) { return data.length + GetContentMsg("PlayerHistory_count"); }
                },
                {
                    title: GetContentMsg("ServerData_channel"),
                    field: 'channelId',
                    align: 'center',
                    valign: 'middle',
                    footerFormatter: function () { return '-' }
                },
                {
                    title: GetContentMsg("ServerData_newUserCnt"),
                    field: 'newUserCnt',
                    align: 'center',
                    valign: 'middle',
                    footerFormatter: sumFormatter,
                },
                {
                    title: GetContentMsg("ServerData_newDeviceCnt"),
                    field: 'newDeviceCnt',
                    align: 'center',
                    valign: 'middle',
                    footerFormatter: sumFormatter,
                },
                {
                    title: GetContentMsg("ServerData_activeUserCnt"),
                    field: 'activeUserCnt',
                    align: 'center',
                    valign: 'middle',
                    footerFormatter: sumFormatter,
                },
                {
                    title: GetContentMsg("ServerData_newUserPayCnt"),
                    field: 'newUserPayCnt',
                    align: 'center',
                    valign: 'middle',
                    footerFormatter: sumFormatter,
                },
                {
                    title: GetContentMsg("ServerData_totalPayCnt"),
                    field: 'totalPayCnt',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    footerFormatter: sumFormatter,
                },
                {
                    title: GetContentMsg("ServerData_newUserPayVal"),
                    field: 'newUserPayVal',
                    align: 'center',
                    valign: 'middle',
                    footerFormatter: sumFormatter,
                },
               {
                   title: GetContentMsg("ServerData_totalPayVal"),
                   field: 'totalPayVal',
                   align: 'center',
                   valign: 'middle',
                   footerFormatter: sumFormatter,
               },
                {
                    title: GetContentMsg("ServerData_newUserPayRate"),
                    field: 'newUserPayRate',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return value.toFixed(2);
                    },
                    footerFormatter: function (data) {
                        if (data.length == 0)
                            return "-";
                        return (sumFormatter.call(this, data, "newUserPayCnt") / sumFormatter.call(this, data, "newUserCnt") * 100).toFixed(2);
                    }
                },
                {
                    title: GetContentMsg("ServerData_newPayUserCnt"),
                    field: 'newPayUserCnt',
                    align: 'center',
                    valign: 'middle',
                    footerFormatter: function () { return '-' }
                },
                {
                    title: GetContentMsg("ServerData_activePayRate"),
                    field: 'activePayRate',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return value.toFixed(2);
                    },
                    footerFormatter: function (data) {
                        if (data.length == 0)
                            return "-";
                        return (sumFormatter.call(this, data, "totalPayCnt") / sumFormatter.call(this, data, "activeUserCnt") * 100).toFixed(2);
                    }
                },
                {
                    title: 'ARPPU',
                    field: 'arppu',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return value.toFixed(2);
                    },
                    footerFormatter: function (data) {
                        if (data.length == 0)
                            return "-";
                        return (sumFormatter.call(this, data, "totalPayVal") / sumFormatter.call(this, data, "totalPayCnt")).toFixed(2);
                    }
                },
                {
                    title: 'ARPU',
                    field: 'arpu',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return value.toFixed(2);
                    },
                    footerFormatter: function (data) {
                        if (data.length == 0)
                            return "-";
                        return (sumFormatter.call(this, data, "totalPayVal") / sumFormatter.call(this, data, "activeUserCnt")).toFixed(2);
                    }
                },
            ]
        }
    );
    };

    function queryServerData() {
        var jsonData = { "channel_id": $('#select_channel').val(), "date_start": $('#datetimepicker_1').val(), "date_end": $('#datetimepicker_2').val() };
        $.ajax(
        {
            type: "POST",
            url: "ServerData.aspx/QueryServerData",
            data: JSON.stringify(jsonData),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var data_json = JSON.parse(ack_data.d);
                initServerTable(data_json);
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        }
        );
    }

    function initSevenDataTable(dt) {
        $("#seven_data_table").bootstrapTable("destroy");
        $("#seven_data_table").bootstrapTable({
            data: dt,
            pagination: true,
            sidePagination: 'client',
            pageSize: 10,
            pageList: [10, 20, 50, 100, GetContentMsg("ServerData_all")],
            striped: true, // 隔行变色
            search: true,
            columns: [
                {
                    title: GetContentMsg("ServerData_serverName"),
                    field: 'serverName',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_activeUserCnt0"),
                    field: 'activeUserCnt.0',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayVal0"),
                    field: 'totalPayVal.0',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayCnt0"),
                    field: 'totalPayCnt.0',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_activeUserCnt1"),
                    field: 'activeUserCnt.1',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayVal1"),
                    field: 'totalPayVal.1',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayCnt1"),
                    field: 'totalPayCnt.1',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_activeUserCnt2"),
                    field: 'activeUserCnt.2',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayVal2"),
                    field: 'totalPayVal.2',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayCnt2"),
                    field: 'totalPayCnt.2',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_activeUserCnt3"),
                    field: 'activeUserCnt.3',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayVal3"),
                    field: 'totalPayVal.3',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayCnt3"),
                    field: 'totalPayCnt.3',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_activeUserCnt4"),
                    field: 'activeUserCnt.4',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayVal4"),
                    field: 'totalPayVal.4',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayCnt4"),
                    field: 'totalPayCnt.4',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_activeUserCnt5"),
                    field: 'activeUserCnt.5',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayVal5"),
                    field: 'totalPayVal.5',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayCnt5"),
                    field: 'totalPayCnt.5',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_activeUserCnt6"),
                    field: 'activeUserCnt.6',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayVal6"),
                    field: 'totalPayVal.6',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("ServerData_totalPayCnt6"),
                    field: 'totalPayCnt.6',
                    align: 'center',
                    valign: 'middle',
                },
            ]
        }
    );
    };

    function querySevenData() {
        if ($('#week_num').val() < 1) {
            alert(GetContentMsg("ServerData_error_week"));
            return;
        }
        var jsonData = { "channel_num": $('#channel_num').val(), "week_num": $('#week_num').val() };
        $.ajax(
        {
            type: "POST",
            url: "ServerData.aspx/QuerySevenData",
            data: JSON.stringify(jsonData),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var data_json = JSON.parse(ack_data.d);
                initSevenDataTable(data_json);
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
</html>
