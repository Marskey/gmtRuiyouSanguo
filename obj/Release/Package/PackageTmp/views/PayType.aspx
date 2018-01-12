<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayType.aspx.cs" Inherits="gmt.PayType" %>

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
            <h1 >결제방식</h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
                <h1 class="page-header" id="lan_PayType" data-lan-id="PayType"></h1>
                <div id="toolbar">
                    <button class="btn btn-primary" type="button" onclick="addPayType()">
                        <i class="glyphicon glyphicon-plus"></i>
                    </button>
                    <%--<button class="btn btn-primary" type="button" onclick="testFunc()">
                    </button>--%>
                </div>
                <table id="pay_type_table"></table>

            <div class="modal fade" style="top: 30%" id="modal_modify" tabindex="-1" role="dialog" aria-labelledby="lan_PayType_Modify" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                            <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue" id="lan_PayType_Modify" data-lan-id="PayType_Modify"></h4>
                        </div>
                        <div class="modal-body">
                            <table class="table_td_200">
                                <tr>
                                    <td>
                                        <label id="lan_PayType_input_pkg_name" data-lan-id="PayType_input_pkg_name"></label>
                                    </td>
                                    <td>
                                        <input id="pkg_name" class="form-control" placeholder="" data-lan-id="PayType_input_pkg_name" data-lan-type="placeholder" />
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;</td>
                                    <td>
                                        <label>APPID：</label>
                                    </td>
                                    <td>
                                        <input id="pkg_type" class="form-control" placeholder="" data-lan-id="PayType_input_appId" data-lan-type="placeholder" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label id="lan_PayType_Name" data-lan-id="PayType_Name"></label>
                                    </td>
                                    <td>
                                        <select id="pay_type" class="form-control" style="width: 200px" onchange="payTypeChanged();">
                                        </select>
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;</td>
                                    <td>
                                        <label id="ex_param_label">爱贝 APPID：</label>
                                    </td>
                                    <td>
                                        <input id="ex_param" class="form-control" placeholder="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label id="ex_param_2_label">爱贝商品ID：</label>
                                    </td>
                                    <td>
                                        <input id="ex_param_2" class="form-control" placeholder="" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" onclick="setPayType();" data-dismiss="modal" id="lan_PayType_Confirm" data-lan-id="PayType_Confirm"></button>
                            <button type="button" class="btn btn-default" data-dismiss="modal" id="lan_PayType_Cancel" data-lan-id="PayType_Cancel"></button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>

<script type="text/javascript" src="../bootstrap/js/bootstrap-table.min.js"></script>
<script type="text/javascript" src="../bootstrap/js/locales/bootstrap-table-locale-all.js"></script>
<script>
    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales['zh-CN']);
    $(document).ready(function () {
        getPayType();
        queryPayTypeData();
    })

    var $optType = 0;   //0-add,1-modify,2-delete

    function initPayTypeTable(dt) {
        $('#pay_type_table').bootstrapTable('destroy');
        $('#pay_type_table').bootstrapTable({
            data: dt,
            pagination: true,
            height: $(window).height(),
            sidePagination: 'client',
            pageSize: 10,
            pageList: [10, 20, 50, 100, '전체'],
            striped: true, // 隔行变色
            search: true,
            toolbar: '#toolbar',
            columns: [
                {
                    title: GetContentMsg("PayType_pkg_name"),
                    field: 'packageName',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: 'APPID',
                    field: 'appId',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("PayType_Name"),
                    field: 'payTypeDesc',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    title: GetContentMsg("PayType_opt"),
                    field: 'operate',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return ['<button class="modify btn btn-primary" type="button"><i class="glyphicon glyphicon-wrench"></i></button>&nbsp;&nbsp;&nbsp;<button class="delete btn btn-danger" type="button"><i class="glyphicon glyphicon-trash"></i></button>'].join('');
                    },
                    events: {
                        'click .modify': function (e, value, row, index) {
                            operateEvents(1, row);
                        },

                        'click .delete': function (e, value, row, index) {
                            operateEvents(2, row);
                        }
                    },
                },
            ],
        });
    }

    function operateEvents(opt, row) {
        $('#pkg_name').val(row.packageName);
        $('#pkg_name').attr('disabled', true);
        $('#pkg_type').val(row.appId);
        $('#pkg_type').attr('disabled', true);
        $('#pay_type').val(row.payTypeValue);
        if ($('#pay_type').val() == 1) {
            $('#ex_param').val(row.exParam);
            $('#ex_param').show();
            $('#ex_param_label').show();
            $('#ex_param_2').val(row.exParam2);
            $('#ex_param_2').show();
            $('#ex_param_2_label').show();
        }
        else {
            $('#ex_param').hide();
            $('#ex_param_label').hide();
            $('#ex_param_2').hide();
            $('#ex_param_2_label').hide();
        }

        if (opt == 1) {
            $('#pay_type').attr('disabled', false);
            $('#ex_param').attr('disabled', false);
            $('#lan_PayType_Modify').html(GetContentMsg("PayType_modify"));
        }
        else if (opt == 2) {
            $('#pay_type').attr('disabled', true);
            $('#ex_param').attr('disabled', true);
            $('#lan_PayType_Modify').html(GetContentMsg("PayType_delete"));
        }

        $("#modal_modify").modal({ keyboard: false });
        $optType = opt;
    }

    function queryPayTypeData() {
        $.ajax(
        {
            type: "POST",
            url: "PayType.aspx/QueryPayTypeData",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var data_json = JSON.parse(ack_data.d);
                initPayTypeTable(data_json);
            },
            error: function (msg) {
                alert(msg.responseText);
            },
            complete: function () {
            }
        });
    }

    function addPayType() {
        $optType = 0;
        $('#pkg_name').val('');
        $('#pkg_name').attr('disabled', false);
        $('#pkg_type').val('');
        $('#pkg_type').attr('disabled', false);
        $('#pay_type').val(0);
        $('#pay_type').attr('disabled', false);
        $('#ex_param').hide();
        $('#ex_param_label').hide();
        $('#ex_param_2').hide();
        $('#ex_param_2_label').hide();
        $('#lan_PayType_Modify').html(GetContentMsg("PayType_add"));
        $("#modal_modify").modal({ keyboard: false });
    }

    function setPayType() {
        if ($('#pkg_name').val() && $('#pkg_type').val() && $('#pay_type').val()) {
            if ($('#pay_type').val() == "1") {
                if(!$('#ex_param').val()) {
                    alert("爱贝APPId不能为空！");
                    return;
                }

                if (!$('#ex_param_2').val()) {
                    alert("爱贝商品Id不能为空！");
                    return;
                }
            }

            var jsonData = { "optType": $optType, "packageName": $('#pkg_name').val(), "appId": $('#pkg_type').val(), "payType": $('#pay_type').val(), "exParam": $('#ex_param').val(), "exParam2": $('#ex_param_2').val() };
            $.ajax(
            {
                type: "POST",
                url: "PayType.aspx/SetPayType",
                data: JSON.stringify(jsonData),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                },
                success: function (ack_data) {
                    var data_json = JSON.parse(ack_data.d);
                    if (data_json[0].error == 1) {
                        if ($optType == 0) {
                            alert(GetContentMsg("PayType_add_failed1"))
                        }
                        else if ($optType == 1) {
                            alert(GetContentMsg("PayType_add_failed2"))
                        }
                        else if ($optType == 2) {
                            alert(GetContentMsg("PayType_delete_failed"))
                        }
                    }
                    else {
                        initPayTypeTable(data_json);
                        $("#modal_modify").modal({ keyboard: true });
                    }
                },
                error: function (msg) {
                    alert(msg.responseText);
                },
                complete: function () {
                }
            });
        }
        else {
            alert(GetContentMsg("PayType_null_tip"));
            return;
        }

    }

    function getPayType() {
        $.ajax(
        {
            type: "POST",
            url: "PayType.aspx/GetPayType",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var data_json = JSON.parse(ack_data.d);
                if (data_json[0].error == 1) {
                    alert(GetContentMsg("PayType_get_failed"));
                }
                else {
                    for (var i = 0; i < data_json.length; i++) {
                        $("#pay_type").append('<option value="' + data_json[i].payTypeValue + '">' + data_json[i].payTypeDesc + '</option>');
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

    function payTypeChanged() {
        if ($('#pay_type').val() == 1) {
            $('#ex_param').show();
            $('#ex_param_label').show();
            $('#ex_param_2').show();
            $('#ex_param_2_label').show();
        }
        else {
            $('#ex_param').hide();
            $('#ex_param_label').hide();
            $('#ex_param_2').hide();
            $('#ex_param_2_label').hide();
        }
    }

    //function testFunc() {
    //    $.ajax(
    //        {
    //            url: "PayType.aspx",
    //            data: { "AppId": "789" },
    //            //dataType: "json",
    //            type: "get",
    //            beforeSend: function () {

    //            },
    //            success: function (ack_data) {
    //                alert(ack_data);
    //            },
    //            error: function () {

    //            },
    //            complete: function () {

    //            }
    //        }
    //        );
    //}

</script>

</html>
