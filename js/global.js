$(function () {
    $("#header").load('Navbar.html');
});

function ChangeLan(lan) {
    setCookie('lan', lan);
    var lan_name = $(".nav.navbar-nav.navbar-right [data-language='" + lan + "']").html();
    $(".nav.navbar-nav.navbar-right span.language").html(lan_name);
    language_type = lan;
    window.location.reload()
}

function setCookie(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days*24*60*60*1000);
    document.cookie = name + "="+ escape (value) + ";expires=" + exp.toGMTString();
}

//获取cookie
function getCookie(name)
{
    var arr,reg=new RegExp("(^| )"+name+"=([^;]*)(;|$)");
    if(arr=document.cookie.match(reg))
    return unescape(arr[2]);
    else
    return null;
}

function getQuestTypes(selectEle) {
    $.ajax({
        type: "get",
        url: "ActivityOperate.aspx/GetQuestTypes",
        contentType: "application/json",
        dataType: "json",
        beforeSend: function () {
        },
        success: function (ack_data) {
            selectEle.html('');
            if (ack_data.d) {
                var data = JSON.parse(ack_data.d);
                for (var i = 0; i < data.length; ++i) {
                    var option = document.createElement("OPTION");
                    option.value = data[i].value;
                    option.innerHTML = data[i].text;
                    selectEle.append(option);
                }
            }
        },
        error: function (msg) {
            alert(msg.responseText);
        },
        complete: function () {
        }
    });
}

function getRequestInfos(selectEle) {
    $.ajax({
        type: "get",
        url: "ActivityOperate.aspx/GetRequestInfos",
        contentType: "application/json",
        dataType: "json",
        beforeSend: function () {
        },
        success: function (ack_data) {
            selectEle.html('');
            if (ack_data.d) {
                var data = JSON.parse(ack_data.d);
                for (var i = 0; i < data.length; ++i) {
                    var option = document.createElement("OPTION");
                    option.value = data[i].value;
                    option.innerHTML = data[i].text;
                    selectEle.append(option);
                }
            }
        },
        error: function (msg) {
            alert(msg.responseText);
        },
        complete: function () {
        }
    });
}

function getRewardTypes(selectEle) {
    $.ajax({
        type: "get",
        url: "ActivityOperate.aspx/GetRewardTypes",
        contentType: "application/json",
        dataType: "json",
        beforeSend: function () {
        },
        success: function (ack_data) {
            selectEle.html('');
            if (ack_data.d) {
                var data = JSON.parse(ack_data.d);
                for (var i = 0; i < data.length; ++i) {
                    var option = document.createElement("OPTION");
                    option.value = data[i].value;
                    option.innerHTML = data[i].text;
                    selectEle.append(option);
                }
            }
        },
        error: function (msg) {
            alert(msg.responseText);
        },
        complete: function () {
        }
    });
}

function getItemNames4Select(rwd_type, selectEle) {
    $.ajax({
        type: "get",
        url: "ActivityOperate.aspx/GetItemName",
        contentType: "application/json",
        data: { "rwdType": rwd_type },
        dataType: "json",
        beforeSend: function () {
            selectEle.html("<option>Loading data...</option>");
        },
        success: function (ack_data) {
            selectEle.html('');
            if (ack_data.d) {
                var data = JSON.parse(ack_data.d);
                for (var i = 0; i < data.length; ++i) {
                    var option = document.createElement("OPTION");
                    option.value = data[i].value;
                    option.innerHTML = data[i].text;
                    selectEle.append(option);
                }
            }
        },
        error: function (msg) {
            alert(msg.responseText);
        },
        complete: function () {
        }
    });

}

String.prototype.truncString = function(max, add){
   add = add || '...';
   return (this.length > max ? this.substring(0,max)+add : this);
};

//function showLoadingToast() {
//    var $loadingToast = $('#loadingToast');
//    if ($loadingToast.css('display') != 'none') return;
//    $loadingToast.fadeIn(100);
//}

//function hideLoadingToast() {
//    var $loadingToast = $('#loadingToast');
//    if ($loadingToast.css('display') == 'none') return;
//    $loadingToast.fadeOut(100);
//}

//function showSucToast() {
//    var $sucToast = $('#sucToast');
//    if ($sucToast.css('display') != 'none') return;
//    $sucToast.fadeIn(100);
//    setTimeout(function () {
//        $sucToast.fadeOut(100);
//    }, 1000);
//}

//    <!--BEGIN toast-->
//    <div id="sucToast" style="display: none;">
//        <div class="weui-mask_transparent" style="z-index: 5500"></div>
//        <div class="weui-toast" style="z-index: 6000">
//            <i class="weui-icon-success-no-circle weui-icon_toast"></i>
//            <p class="weui-toast__content">已完成</p>
//        </div>
//    </div>
//    <!--end toast-->

//    <!-- loading toast -->
//    <div id="loadingToast" style="display: none;">
//        <div class="weui-mask_transparent" style="z-index: 5500"></div>
//        <div class="weui-toast" style="z-index: 6000">
//            <i class="weui-loading weui-icon_toast"></i>
//            <p class="weui-toast__content">数据加载中</p>
//        </div>
//    </div>
//    <!--END toast-->