var language_json=null;

function loadDict() {
    $.ajax({
        async: false,
        type: "GET",
        url: "../Json/locale." + language_type +".json",
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
        return language_json[msgId];
    else
        return undefined;
}

$(document).ready(function () {
    loadDict();
});

function SetContentMsg() {
    /*
    * data-lan-id：文本的在language_json中的ID
    * data-lan-type：文本放置位置类型，默认html
    */
    $("[data-lan-id]").each(function () {
        var lanId= $(this).attr('data-lan-id');
        var lanType = $(this).attr('data-lan-type');
        if (!lanType) {
            lanType = "html";
        }
        var text = GetContentMsg(lanId);
        if (text) {
            switch (lanType) {
                case "html": $(this).prepend(text); break;
                case "text": $(this).val(text); break;
                case "placeholder": $(this).attr('placeholder', text); break;
            }
        }
    })
}