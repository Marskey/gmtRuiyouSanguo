<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityOperate.aspx.cs" Inherits="gmt.ActivityOperate" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../bootstrap/css/bootstrap-table.min.css" />
    <link href="../mycss/docs.min.css" rel="stylesheet" media="screen" />
    <link rel="stylesheet" href="../mycss/style.css" />
    <script src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script src="../bootstrap/js/bootstrap.min.js"></script>
    <script src="../js/global.js"></script>
    <script src="../js/language.js"></script>
    <script src="../js/language.js"></script>
    <script src="../js/ajaxSubmit.js"></script>
</head>
<body>
    <header class="navbar navbar-static-top bs-docs-nav" id="header"></header>
    <div class="bs-docs-header" id="content">
        <div class="container">
            <h1 data-lan-id="AcitvityOperate_head_title"></h1>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <form class="form-horizontal" data-table-id="table_activity">
                <h2 data-lan-id="ActivityOperate_activity_header_2"></h2>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <button id="btn_activity_add" type="button" class="btn btn-default" data-lan-id="Add">
                            <i class="glyphicon glyphicon-plus"></i>
                        </button>
                        <button id="btn_activity_del" type="button" class="btn btn-default" data-lan-id="Delete" disabled>
                            <i class="glyphicon glyphicon-remove"></i>
                        </button>
                        <button id="btn_activity_download" type="button" class="btn btn-default" data-lan-id="Download">
                            <i class="glyphicon glyphicon-cloud-download"></i>
                        </button>
                        <button id="btn_activity_upload" type="button" class="btn btn-default" data-lan-id="Upload">
                            <i class="glyphicon glyphicon-cloud-upload"></i>
                        </button>
                    </div>
                    <table id="table_activity" class="table table-no-bordered" style="table-layout: fixed;"></table>
                </div>
            </form>
        </div>
    </div>

    <!-- /activity.modal -->
    <div class="modal fade" id="modal_activity" tabindex="-1" role="dialog" aria-labelledby="modal_activity_title" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue" id="modal_activity_title">
                        <label data-lan-id="Modal_activity_title"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal" data-table-id="table_activity">
                        <div class="form-group">
                            <label class="col-sm-4" for="server_list" data-lan-id="ActivityOperate_activity_title"></label>
                            <div class="col-sm-8">
                                <input id="input_modal_activity_title" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="textarea_modal_activty_desc" data-lan-id="ActivityOperate_activity_desc"></label>
                            <div class="col-sm-8">
                                <textarea id="textarea_modal_activity_desc" class="form-control"></textarea>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" data-lan-id="ActivityOperate_activity_firstmark"></label>
                            <div class="col-sm-8">
                                <label for="switch_modal_activity_firstmark" class="ry-switch-cp">
                                    <input id="switch_modal_activity_firstmark" data-table-th="firstmark" class="ry-switch-cp__input" type="checkbox">
                                    <div class="ry-switch-cp__box"></div>
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="input_modal_activity_order" data-lan-id="ActivityOperate_activity_order"></label>
                            <div class="col-sm-8">
                                <input id="input_modal_activity_order" data-table-th="order" type="number" class="form-control" />
                            </div>
                        </div>
                        <hr />
                        <div id="table_quest_toolbar">
                            <button id="btn_quest_add" type="button" class="btn btn-default" data-lan-id="Add">
                                <i class="glyphicon glyphicon-plus"></i>
                            </button>
                            <button id="btn_quest_del" type="button" class="btn btn-danger" data-lan-id="Delete" disabled>
                                <i class="glyphicon glyphicon-remove"></i>
                            </button>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="table_quests" data-lan-id="ActivityOperate_activity_quest_list"></label>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-12">
                                <table id="table_quest" class="table table-no-bordered"></table>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="return confirmAddActivity()" data-lan-id="Modal_btn_ok"></button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_close"></button>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /activity.modal -->

    <!-- /quest.modal -->
    <div class="modal fade" id="modal_quest" style="z-index:1060" tabindex="-1" role="dialog" aria-labelledby="modal_quest" aria-hidden="true">
        <div class="modal-dialog ">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue">
                        <label data-lan-id="Modal_quest_title"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal" data-table-id="table_quest">
                        <div class="form-group">
                            <label class="col-sm-4" for="input_modal_quest_desc" data-lan-id="Modal_quest_desc"></label>
                            <div class="col-sm-8">
                                <input id="input_modal_quest_desc" data-table-th="desc" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="select_modal_quest_type" data-lan-id="Modal_quest_type"></label>
                            <div class="col-sm-8">
                                <select id="select_modal_quest_type" data-table-th="type" class="form-control" >
                                    <option>Loading data...</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="select_modal_quest_request" data-lan-id="Modal_quest_request"></label>
                            <div class="col-sm-8">
                                <select id="select_modal_quest_request" data-table-th="request" class="form-control" >
                                    <option>Loading data...</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="input_modal_quest_count_a" data-lan-id="Modal_quest_count_a"></label>
                            <div class="col-sm-8">
                                <input id="input_modal_quest_count_a" data-table-th="count_a" type="number" placeholder="1~9999" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="input_modal_quest_count_b" data-lan-id="Modal_quest_count_b"></label>
                            <div class="col-sm-8">
                                <input id="input_modal_quest_count_b" data-table-th="count_b" type="number" placeholder="1~9999" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="input_modal_quest_count_c" data-lan-id="Modal_quest_count_c"></label>
                            <div class="col-sm-8">
                                <input id="input_modal_quest_count_c" data-table-th="count_c" type="number" placeholder="1~9999" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" data-lan-id="Modal_quest_overlap"></label>
                            <div class="col-sm-8">
                                <label for="switch_modal_quest_overlap" class="ry-switch-cp">
                                    <input id="switch_modal_quest_overlap" data-table-th="overlap" class="ry-switch-cp__input" type="checkbox">
                                    <div class="ry-switch-cp__box"></div>
                                </label>
                            </div>
                        </div>
                    </form>
                    <form class="form-horizontal" hidden>
                        <div class="form-group">
                        <label class="col-sm-4" for="table_reward" data-lan-id="ActivityOperate_activity_reward_list"></label>
                        </div>
                        <div id="table_reward_toolbar">
                            <button id="btn_reward_add" type="button" class="btn btn-default" data-lan-id="Add">
                                <i class="glyphicon glyphicon-plus"></i>
                            </button>
                            <button id="btn_reward_del" type="button" class="btn btn-danger" data-lan-id="Delete" disabled>
                                <i class="glyphicon glyphicon-remove"></i>
                            </button>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-12">
                                <table id="table_reward" class="table table-no-bordered"></table>
                            </div>
                        </div>
                    </form>
                </div>
                <div>
                    <button id="Modal_btn_quest_switch" class="btn btn-primary form-control" style="border-radius:0" type="button" data-lan-id="Modal_quest_btn_reward" value="0">
                        &nbsp<i class="glyphicon glyphicon-triangle-top"></i>
                    </button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary modal-btn-ok" data-lan-id="Modal_btn_ok"></button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_close"></button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /quest.modal -->

    <!-- /reward.modal -->
    <div class="modal fade" id="modal_reward" style="z-index:1070" tabindex="-1" role="dialog" aria-labelledby="modal_reward_title" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue" id="modal_reward_title">
                        <label data-lan-id="Modal_reward_title"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal" data-table-id="table_reward">
                        <div class="form-group">
                            <label class="col-sm-4" for="select_modal_reward_type" data-lan-id="Modal_reward_type"></label>
                            <div class="col-sm-8">
                                <select id="select_modal_reward_type" data-table-th="type" class="form-control" >
                                    <option>Loading data...</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="select_modal_reward_item" data-lan-id="Modal_reward_item"></label>
                            <div class="col-sm-8">
                                <select id="select_modal_reward_item" data-table-th="item" class="form-control" >
                                    <option>Loading data...</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4" for="input_modal_reward_count" data-lan-id="Modal_reward_count"></label>
                            <div class="col-sm-8">
                                <input id="input_modal_reward_count" data-table-th="count" type="number" placeholder="1~9999" class="form-control" />
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary modal-btn-ok" data-lan-id="Modal_btn_ok"></button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_close"></button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /reward.modal --> 
    <!-- /upload.modal -->
    <div class="modal fade" id="modal_upload" style="z-index:1070" tabindex="-1" role="dialog" aria-labelledby="modal_upload_title" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background: #337ab7; border-bottom: 1px; padding: 15px; border-radius: 4px 4px 0px 0px">
                    <h4 class="modal-title" style="margin: 0; line-height: 1.42857143; color: aliceblue" id="modal_upload_title">
                        <label data-lan-id="Modal_upload_title"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <form class="form-inline" action="ActivityOperate.aspx" enctype="multipart/form-data" method="post" onsubmit="onUploadSubmit(this); return false;">
                        <div class="form-group">
                            <input type="file" id="uploadFile" multiple name="bytes" accept=".protodata.bytes" style="display:none">
                            <label data-lan-id="Choose_file"></label>
                            <div class="input-group">
                                <span class="input-group-btn">
                                    <button id="modal_btn_choose_file" class="btn btn-default" type="button" data-lan-id="Choose_file" onclick="$('#uploadFile').click()"></button>
                                </span>
                                <input id="modal_input_chosen_file" type="text" class="form-control" placeholder="" disabled>
                            </div>
                        </div>
                        <button id="modal_btn_upload" type="submit" class="btn btn-primary form-control" onclick="onBtnUpload();" data-lan-id="Modal_btn_ok"></button>
                    </form>
                </div>
                <div class="progress" style="border-radius: 0; margin-bottom: 0px; height: 3px;" hidden>
                    <div class="progress-bar progress-bar-primary progress-bar-striped active" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%; ">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" data-lan-id="Modal_btn_close"></button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /upload.modal -->
</body>
</html>

<script type="text/javascript" src="../bootstrap/js/bootstrap-table.min.js"></script>
<script type="text/javascript" src="../bootstrap/js/locales/bootstrap-table-locale-all.js"></script>
<script>
    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales[language_type]);
    var sprintf = $.fn.bootstrapTable.utils.sprintf;


    var $addQuset = $('#btn_quest_add'),
        $addReward = $('#btn_reward_add'),
        $addActivity = $('#btn_activity_add');

    var $removeQuest = $('#btn_quest_del'),
        $removeReward = $('#btn_reward_del'),
        $removeActivity = $('#btn_activity_del');

    var $tableQuest = $("#table_quest"),
        $tableReward = $("#table_reward"),
        $tableActivity = $("#table_activity");

    var increasId = 0;

    var $selectItem = $('#select_modal_reward_item');

    var modalOpened = [];

    $(document).ready(function () {
        initTableQuest($tableQuest);
        initTableReward($tableReward);
        initTableActivity($tableActivity);
        getQuestTypes($('#select_modal_quest_type'));
        getRequestInfos($('#select_modal_quest_request'));
        getRewardTypes($('#select_modal_reward_type'));
        getItemNames4Select(0, $selectItem);
        getActivities();
    })

    $(window).resize(function () {
        $tableActivity.bootstrapTable('resetView');
        $tableQuest.bootstrapTable('resetView');
    })

    $('#select_modal_reward_type').on('change', function () {
        var type = $(this).val();
        getItemNames4Select(type, $selectItem);
    })

    function boolFormatter(value) {
        if (value == true) {
            return '<i class="glyphicon glyphicon-ok text-success"></i>'
        } else {
            return '<i class="glyphicon glyphicon-remove text-danger"></i>'
        }
    }

    function initTableQuest(tableEle) {
        tableEle.bootstrapTable("destroy");
        tableEle.bootstrapTable({
            height: 300,
            striped: true,
            classes: "table table-no-bordered",
            toolbar: "#table_quest_toolbar",
            clickToSelect: true,
            columns: [
                { field:'check', checkbox: true, align: 'center', valign: 'middle', },
                { title:'#', field:'id', align: 'center', valign: 'middle', },
                { title: GetContentMsg("table_quests_th_desc"), field: 'desc', align: 'center', valign: 'middle',},
                { field: 'type', align: 'center', valign: 'middle', visible: false, },
                { field: 'typeidx', align: 'center', valign: 'middle', visible: false, },
                { title: GetContentMsg("table_quests_th_type"), field: 'typename', align: 'center', valign: 'middle', },
                { field: 'request', align: 'center', valign: 'middle', visible: false, },
                { field: 'requestidx', align: 'center', valign: 'middle', visible: false, },
                { title: GetContentMsg("table_quests_th_request"), field: 'requestname', align: 'center', valign: 'middle', },
                { title: GetContentMsg("table_quests_th_count_a"), field: 'count_a', align: 'center', valign: 'middle', },
                { title: GetContentMsg("table_quests_th_count_b"), field: 'count_b', align: 'center', valign: 'middle', },
                { title: GetContentMsg("table_quests_th_count_c"), field: 'count_c', align: 'center', valign: 'middle', },
                { title: GetContentMsg("table_quests_th_overlap"), field: 'overlap', align: 'center', valign: 'middle', formatter: boolFormatter},
                { field: 'rewards', align: 'center', valign: 'middle', visible: false},
                {
                    title: GetContentMsg("Table_th_opt"),
                    field: 'opt',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return ['<a class="modify btn btn-default" style="display:block;"><i class="glyphicon glyphicon-wrench"></i></a>'].join('');
                    },
                    events: {
                        'click .modify': function (e, value, row, index) {
                            questOperate(index, row);
                        },
                    },
                },
            ]
        });
    };

    function initTableReward(tableEle) {
        tableEle.bootstrapTable("destroy");
        tableEle.bootstrapTable({
            height: 369,
            striped: true,
            toolbar: "#table_reward_toolbar",
            classes: "table table-no-bordered",
            clickToSelect: true,
            columns: [
                { checkbox: true, align: 'center', valign: 'middle', },
                { title:'#', field:'id', align: 'center', valign: 'middle', },
                { field: 'type', align: 'center', valign: 'middle', visible:false, },
                { title: GetContentMsg("table_reward_th_type"), field: 'typename', align: 'center', valign: 'middle', },
                { field: 'item', align: 'center', valign: 'middle', visible:false, },
                { title: GetContentMsg("table_reward_th_item"), field: 'itemname', align: 'center', valign: 'middle', },
                { title: GetContentMsg("table_reward_th_count"), field: 'count', align: 'center', valign: 'middle', },
            ]
        });
    };

    function initTableActivity(tableEle) {
        tableEle.bootstrapTable("destroy");
        tableEle.bootstrapTable({
            height: $(window).height() - $(header).outerHeight() - $('#content').outerHeight(),
            detailView: true,
            clickToSelect: true,
            classes: "table table-no-bordered",
            detailFormatter: detailFormatter,
            columns: [
                { checkbox: true, align: 'center', valign: 'middle',},
                { title: 'ID', field: 'id', width: '10%',align: 'center', valign: 'middle', },
                { title: GetContentMsg("table_activity_th_title"), field: 'title', width: '20%', align: 'left', valign: 'middle', cellStyle: { classes: 'td-ellipsis'}},
                { title: GetContentMsg("table_activity_th_desc"), field: 'desc', width: '20%', align: 'left', valign: 'middle', cellStyle: { classes: 'td-ellipsis'}},
                { title: GetContentMsg("table_activity_th_firstmark"), field: 'firstmark', align: 'center', valign: 'middle', formatter: boolFormatter },
                { title: GetContentMsg("table_activity_th_order"), field: 'order', align: 'center', valign: 'middle',},
                {
                    field: 'opt',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return ['<a class="delete btn btn-danger" ><i class="glyphicon glyphicon-remove"></i></a>'].join('');
                    },
                    events: {
                        'click .delete': function (e, value, row, index) {
                            if (!confirm(GetContentMsg('Confirm_delete')))
                                return;
                            activityOptEvents(row);
                        }
                    },
                },
            ]
        });
    };

    function detailFormatter(index, row) {
        var html = [];
        html.push('<div class="well well-lg">');
        html.push(GetContentMsg('ActivityOperate_activity_title'));
        html.push(": ");
        html.push(row.title);
        html.push("<br/>");
        html.push(GetContentMsg('ActivityOperate_activity_desc'));
        html.push(": ");
        html.push(row.desc);
        html.push('</div>');

        html.push(sprintf("<p><b>%s</b></p>", GetContentMsg('ActivityOperate_activity_quest_list')));
        html.push('<div class="panel-group" id="accordion_' + row.id + '" role="tablist" aria-multiselectable="true">');
        for (var i = 0; i < row.quests.length; ++i) {
            var hrefId = row.id + "_" + i;
            html.push('<div class="panel panel-default">');
            html.push('<div class="panel-heading" role="tab" id="heading_' + hrefId + '">');
            html.push('<h4 class="panel-title">');
            html.push(sprintf('<a class="listButton" type="button" data-toggle="collapse" data-parent="#accordion_%s" href="#collapse_%s" aria-expanded="false" aria-controls="collapse_%s">', row.id, hrefId, hrefId));
            var questContent = sprintf('%s: %s %s: %s', GetContentMsg('Modal_quest_desc'), row.quests[i].desc, GetContentMsg('Modal_quest_type'), row.quests[i].typename);
            html.push(questContent);
            html.push('</a>');
            html.push('</h4>');
            html.push('</div>');
            html.push(sprintf('<div id="collapse_%s" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading_%s">', hrefId, hrefId));
            html.push('<div class="panel-body">');
            html.push('<div class="col-sm-4">');
            html.push(sprintf('%s: %s', GetContentMsg('Modal_quest_request'), row.quests[i].requestname));
            html.push('</div>');
            html.push('<div class="col-sm-2">');
            html.push(sprintf('%s: %s', GetContentMsg('Modal_quest_count_a'), row.quests[i].count_a));
            html.push('</div>');
            if (row.quests[i].count_b != 0) {
                html.push('<div class="col-sm-2">');
                html.push(sprintf('%s: %s', GetContentMsg('Modal_quest_count_b'), row.quests[i].count_b));
                html.push('</div>');
            }
            if (row.quests[i].count_c != 0) {
                html.push('<div class="col-sm-2">');
                html.push(sprintf('%s: %s', GetContentMsg('Modal_quest_count_c'), row.quests[i].count_c));
                html.push('</div>');
            }
            html.push('<div class="col-sm-2">');
            html.push(GetContentMsg('Modal_quest_overlap') + ": ");
            html.push(boolFormatter(row.quests[i].overlap));
            html.push('</div>');
            html.push('<div class="col-sm-12">');
            html.push('<hr >');
            html.push('</div>');
            if (row.quests[i].rewards) {
                for (var j = 0; j < row.quests[i].rewards.length; ++j) {
                    html.push('<div class="col-sm-4">');
                    html.push(sprintf('%s: %s', GetContentMsg('Modal_reward_type'), row.quests[i].rewards[j].typename));
                    html.push('</div>');
                    html.push('<div class="col-sm-4">');
                    html.push(sprintf('%s: %s', GetContentMsg('Modal_reward_item'), row.quests[i].rewards[j].itemname));
                    html.push('</div>');
                    html.push('<div class="col-sm-4">');
                    html.push(sprintf('%s: %s', GetContentMsg('Modal_reward_count'), row.quests[i].rewards[j].count));
                    html.push('</div>');
                    html.push('<br>');
                }
            }
            html.push('</div>');
            html.push('</div>');
            html.push('</div>');
        }
        return html.join('');
      }

    function activityOptEvents(row) {
        var ids = [];
        ids.push(row.id);
        removeActivities(ids);
    }

    $tableQuest.on('check.bs.table uncheck.bs.table ' +
                'check-all.bs.table uncheck-all.bs.table', function () {
                    $removeQuest.prop('disabled', !$tableQuest.bootstrapTable('getSelections').length);
                });


    $tableReward.on('check.bs.table uncheck.bs.table ' +
                'check-all.bs.table uncheck-all.bs.table', function () {
                    $removeReward.prop('disabled', !$tableReward.bootstrapTable('getSelections').length);
                });

    $tableActivity.on('check.bs.table uncheck.bs.table ' +
                'check-all.bs.table uncheck-all.bs.table', function () {
                        $removeActivity.prop('disabled', !$tableActivity.bootstrapTable('getSelections').length);
                    });

    $addQuset.on('click', function () {
        $('#modal_quest').modal({ keyboard: false });
        resetModal($('#modal_quest'));
    });

    $addReward.on('click', function () {
        $('#modal_reward').modal({ keyboard: false });
        resetModal($('#modal_reward'));
    });

    $addActivity.on('click', function () {
        $('#modal_activity').modal({ keyboard: false });
        resetModal($('#modal_activity'));
    });

    $('#modal_activity').on('shown.bs.modal', function () {
        $tableQuest.bootstrapTable('resetView');
    });

    function confirmAddActivity() {
        var questJsonData = $('#table_quest').bootstrapTable('getData');
        var param = {
            title: $('#input_modal_activity_title').val(),
            desc: $('#textarea_modal_activity_desc').val(),
            firstmark: $('#switch_modal_activity_firstmark').prop('checked'),
            order: $('#input_modal_activity_order').val(),
            quests: questJsonData,
        }

        if (param.title == "") {
            var errmsg = GetContentMsg('ActivityOperate_activity_title') + GetContentMsg('Cannot_be_empty');
            alert(errmsg);
            return;
        }

        if (param.desc == "") {
            var errmsg = GetContentMsg('ActivityOperate_activity_desc') + GetContentMsg('Cannot_be_empty');
            alert(errmsg);
            return;
        }

        if (param.order == "") {
            param.order = 0;
        }

        if (param.quests.length == 0) {
            var errmsg = GetContentMsg('ActivityOperate_activity_quest_list') + GetContentMsg('Cannot_be_empty');
            alert(errmsg);
            return;
        }

        $.ajax(
            {
                type: "post",
                url: "ActivityOperate.aspx/AddNewActivity",
                data: JSON.stringify(param),
                async: false,
                contentType: "application/json",
                dataType: "json",
                beforeSend: function () {
                },
                success: function (ack_data) {
                    var json = JSON.parse(ack_data.d)
                    if (json.error == 0) {
                        $tableActivity.bootstrapTable("append", json.data);
                        $('#modal_activity').modal('hide');
                    }
                    else {
                        alert(json.msg);
                    }
                },
                error: function (msg) {
                    alert(msg.responseText);
                },
                complete: function () {
                }
            }
            );
    };

    function GetModalData(vform) {
        var data = {};
        for (var i = 0; i < vform.length; i++) {
            var idTh = vform[i].dataset['tableTh'];
            var value = vform[i].value;

            if (vform[i].type == 'checkbox')
            {
                value = vform[i].checked;
            }
            else if (vform[i].type == 'select-one')
            {
                var idx = vform[i].selectedIndex;
                if (idx != -1) {
                    data[idTh + "name"] = vform[i][idx].innerHTML;
                }
            }
            else if (vform[i].type == 'number')
            {
                if (value == "") value = 0;
            }

            if (value === "") {
                var errmsg = $(vform[i]).parents('div').first().prev().html() + GetContentMsg('Cannot_be_empty');
                alert(errmsg);
                return;
            }

            data[idTh] = value;
        }

        var tableId = vform.dataset['tableId'];
        if (tableId == 'table_quest') {
            var rewardData = $tableReward.bootstrapTable('getData');
            data['rewards'] = $.extend(true, [], rewardData);
            if (data['rewards'].length == 0) {
                var errmsg = $('[data-lan-id="ActivityOperate_activity_reward_list"]').html() + GetContentMsg('Cannot_be_empty');
                alert(errmsg);
                return;
            }
        }

        return data;
    }

    $(".modal-btn-ok").on('click', function () {
        var vform = $(this).parent().siblings().find('form')[0];
        var data = GetModalData(vform);
        if (data) {
            data['id'] = increasId++;
        }

        var tableId = vform.dataset['tableId'];
        if ($(this).hasClass('modify')) {
            $('#' + tableId).bootstrapTable('updateRow', { index: modifyQuestIndex, row: data })
        } else {
            $('#' + tableId).bootstrapTable('append', data);
        }

        var thisModal = $(this).parents('.modal');
        $(thisModal).modal('hide');
    })

    function getRowSelections(table) {
        return $.map(table.bootstrapTable('getSelections'), function (row) {
            return row.id;
        });
    }

    $removeQuest.click(function () {
        if (!confirm(GetContentMsg('Confirm_delete')))
            return;
        var ids = getRowSelections($tableQuest);
        $tableQuest.bootstrapTable('remove', {
            field: 'id',
            values: ids
        });
        $(this).prop('disabled', true);
    });

    $removeReward.click(function () {
        if (!confirm(GetContentMsg('Confirm_delete')))
            return;
        var ids = getRowSelections($tableReward);
        $tableReward.bootstrapTable('remove', {
            field: 'id',
            values: ids
        });
        $(this).prop('disabled', true);
    });

    function removeActivities(ids) {
        var param = {
            ids: ids
        }
        $.ajax({
            type: "post",
            url: "ActivityOperate.aspx/RemoveActivity",
            data: JSON.stringify(param),
            async: false,
            contentType: "application/json",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (ack_data) {
                var json = JSON.parse(ack_data.d)
                if (json.length != 0)
                {
                    $tableActivity.bootstrapTable('remove', {
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

    $removeActivity.click(function () {
        if (!confirm(GetContentMsg('Confirm_delete')))
            return;
        var ids = getRowSelections($tableActivity);
        removeActivities(ids);
        $(this).prop('disabled', true);
    });

    $('#Modal_btn_quest_switch').on('click', function () {
        var vforms = $(this).parent().parent().find("form");
        $(vforms[0]).slideToggle('fast');
        $(vforms[1]).slideToggle('fast');
        if ($(this).val() == 0) {
            $(this).html(GetContentMsg('Modal_quest_btn_back') + ' <i class="glyphicon glyphicon-triangle-bottom"></i>');
            $(this).val(1);
        }
        else {
            $(this).html(GetContentMsg('Modal_quest_btn_reward') + ' <i class="glyphicon glyphicon-triangle-top"></i>');
            $(this).val(0);
        }
        $tableReward.bootstrapTable('resetView');
    })

    var modifyQuestIndex = -1;
    function questOperate(index, row) {
        $('#select_modal_quest_type').select(row.typeidx);
        $('#select_modal_quest_request').select(row.requestidx);
        $('#input_modal_quest_count_a').val(row.count_a);
        $('#input_modal_quest_count_b').val(row.count_b);
        $('#input_modal_quest_count_c').val(row.count_c);
        $('#switch_modal_quest_overlap').prop('checked', row.overlap);
        $('#input_modal_quest_desc').val(row.desc);
        $tableReward.bootstrapTable('load', row.rewards);

        $('#modal_quest').find('.modal-btn-ok').html(GetContentMsg("Modal_btn_modify"));
        $('#modal_quest').find('.modal-btn-ok').addClass('modify');
        $('#modal_quest').find('.modal-title').children().html(GetContentMsg('Modal_quest_title_modify'));
        modifyQuestIndex = index;

        $('#modal_quest').modal({ keyboard: false });
    }

    function resetModal(modalEle) {
        var vform = modalEle.find('form')[0];
        for (var i = 0; i < vform.length; i++) {
            vform[i].value = '';

            if (vform[i].type == 'checkbox')
            {
                vform[i].checked = false;
            }
            else if (vform[i].type == 'select-one')
            {
                vform[i].selectedIndex = -1;
            }
        }
        var tableId = vform.dataset['tableId'];
        if (tableId == 'table_quest') {
            $tableReward.bootstrapTable('removeAll');
        } else if (tableId == 'table_activity') {
            $tableQuest.bootstrapTable('removeAll');
        }
    }

    function getActivities() {
        $.ajax(
            {
                type: "get",
                url: "ActivityOperate.aspx/GetActivities",
                contentType: "application/json",
                dataType: "json",
                beforeSend: function () {
                    $tableActivity.bootstrapTable("showLoading");
                },
                success: function (ack_data) {
                    var json = JSON.parse(ack_data.d)
                    {
                        $tableActivity.bootstrapTable("load", json);
                    }
                },
                complete: function () {
                    $tableActivity.bootstrapTable("hideLoading");
                }
            }
            );
    }

    var backdropz_index = 1040;
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

        var vform = $(this).find('form')[0];
        var tableId = vform.dataset['tableId'];
        if (tableId == 'table_quest') {
            var val = $('#Modal_btn_quest_switch').val();
            if (val == 1) {
                $($(this).find('form')[0]).show();
                $($(this).find('form')[1]).hide();
                $('#Modal_btn_quest_switch').html(GetContentMsg('Modal_quest_btn_reward') + ' <i class="glyphicon glyphicon-triangle-top"></i>');
                $('#Modal_btn_quest_switch').val(0);
            }

            var btn = $('#modal_quest').find('.modal-btn-ok');
            if (btn.hasClass('modify')) {
                btn.html(GetContentMsg("Modal_btn_ok"));
                modifyQuestIndex = -1;
                btn.removeClass('modify');
            }

            $(this).find('.modal-title').children().html(GetContentMsg('Modal_quest_title'));
        }
    });

    $tableActivity.on('dbl-click-row.bs.table', function (event, row, $element) {
        var index = $element[0].dataset['index'];
        var expand = !($element.next().is('tr.detail-view'));
        if (expand)
            $tableActivity.bootstrapTable('expandRow', index);
        else
            $tableActivity.bootstrapTable('collapseRow', index);
    })

    $('#btn_activity_download').on('click', function () {
        window.location.replace('\DownloadCDNTableZip.aspx');
    });

    $('#btn_activity_upload').on('click', function () {
        $('#modal_upload').modal({keyboard: false});
    });

    $('#uploadFile').on('change', function () {
        var filecnt = this.files.length;
        if (0 == filecnt) {
            $('#modal_input_chosen_file').val('');
        } else {
            $('#modal_input_chosen_file').val(filecnt + GetContentMsg('Amount_files'));
        }
    });

    function onBtnUpload () {
        $('.progress').slideToggle('fast');
        $('.progress-bar').css('width', '0%');
        $('.progress-bar').removeClass('progress-bar-success');
    };

    function onUploadSubmit(formElement) {
        $('#modal_btn_upload').prop('disabled', true);
        $('#modal_btn_choose_file').prop('disabled', true);
        $('#modal_btn_upload').html(GetContentMsg('Uploading'));
        AJAXSubmit(formElement
            , {
                onProgress: function (evt) {
                    if (evt.lengthComputable) {
                        var percentComplete = evt.loaded / evt.total * 100;
                        $('.progress-bar').css('width', percentComplete + "%");
                        if (percentComplete == 100) {
                            $('.progress-bar').addClass('progress-bar-success');
                            $('#modal_btn_upload').html(GetContentMsg('Installing'));
                        }
                    }
                },
                onLoad: function (res) {
                    var json = JSON.parse(res);
                    if (json.error == 0) {
                        alert(GetContentMsg('Upload_success') + ":\n" + json.msg);
                        getActivities();
                    } else {
                        alert(GetContentMsg('Upload_fail'));
                    }
                },
                onLoadEnd: function () {
                        $('.progress').slideToggle('normal');
                        $('#modal_btn_upload').prop('disabled', false);
                        $('#modal_btn_choose_file').prop('disabled', false);
                        $('#modal_btn_upload').html(GetContentMsg('Modal_btn_ok'));
                }
            });
    }

</script>
