$(function () {
    $("#header").load('Navbar.html');

    //var href = window.location.pathname.split("/").pop();
    //$(".nav.navbar-nav a[href='" + href + "']").parent().addClass('active');
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