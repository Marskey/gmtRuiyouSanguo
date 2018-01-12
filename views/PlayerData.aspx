<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlayerData.aspx.cs" Inherits="gmt.views.PlayerData" %>

<!DOCTYPE html>

<html >
<head>
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
            <h1 data-lan-id="Player_Data"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <h1 class="page-header" id="wanjiashuju" runat="server"></h1>
                <table>
                    <tr>
                        <td>
                            <div class="label label-primary" style="font-size: 20px">
                                <label data-lan-id="PlayerHistory_server_list"></label></div>
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <select id="server_list" style="width: 200px"></select>
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td style="text-align: center; vertical-align: central; width: 100px;">
                            <button class="btn btn-danger" type="button" onclick="queryPlayerData()">
                                <i class="glyphicon glyphicon-cloud-download"></i>
                            </button>
                        </td>
                    </tr>
                </table>

                <div class="col-md-12">
                    <table id="playerdata_table"></table>
                </div>
            </div>


    </div>
</body>
</html>

<script type="text/javascript" src="../bootstrap/js/bootstrap-table.min.js"></script>
<script type="text/javascript" src="../bootstrap/js/locales/bootstrap-table-locale-all.js"></script>
<script type="text/javascript" src="../bootstrap/js/extensions/toolbar/bootstrap-table-toolbar.min.js"></script>
<script>
    $(document).ready(function () {
        queryServerList();
    })


    function queryPlayerData() {
        var param_json = {
            "server_id": $('#server_list').val(),
        }

        $.ajax({
            type: "POST",
            url: "PlayerData.aspx/QueryPlayerData",
            data: JSON.stringify(param_json),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                data_json = JSON.parse(ack_data.d);
                if (data_json[0].error == 0) {
                    alert(GetContentMsg("PlayerData_success"));
                }
                else if (data_json[0].error == 1) {
                    alert(GetContentMsg("PlayerData_failed"));
                }
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        });

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
                    alert(GetContentMsg("PlayerData_server_failed"));
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
