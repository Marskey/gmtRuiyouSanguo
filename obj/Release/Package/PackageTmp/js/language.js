var language_json=null;

loadDict();

function loadDict() {
    $.ajax({
        async: false,
        type: "GET",
        url: "../Json/LanguageData.min.json",
        dataType: "json",
        success: function (msg) {
            language_json = msg;
        },
        error: function (msg) {
        }
    });
}

var language_default = "zh-CN";
var language_type = getCookie('lan');
if (!language_type) language_type = language_default;

function GetContentMsg(msgId) {
    if (language_json[msgId])
        return language_json[msgId][language_type];
    else
        return undefined;
}

$(document).ready(function () {
    SetContentMsg();
});

function SetContentMsg() {
    /*
    * data-lan-id：文本的在language_json中的ID
    * data-lan-type：文本放置位置类型，默认html
    */
    var lanList = $("[id][data-lan-id]");
    for (var i = 0; i < lanList.length; i++) {
        var msgId = $('#' + lanList[i].id).attr('data-lan-id');
        var msgType = $('#' + lanList[i].id).attr('data-lan-type');
        if (!msgType) {
            msgType = "html";
        }
        var msg = GetContentMsg(msgId);
        if (msg) {
            switch (msgType) {
                case "html": $('#' + lanList[i].id).html(msg); break;
                case "text": $('#' + lanList[i].id).val(msg); break;
                case "placeholder": $('#' + lanList[i].id).attr('placeholder', msg); break;
            }
        }
    }

}