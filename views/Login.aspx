<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="gmt.Login" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="../mdbootstrap/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="../mdbootstrap/css/mdb.min.css"/>
    <link rel="stylesheet" href="../mycss/style.css" />
    <link href="http://cdn.bootcss.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet"/>
    <script src="../mdbootstrap/js/jquery-3.2.1.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <!--Modal: Login Form-->
    <div class="modal fade" id="modalLogin" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog cascading-modal" role="document">
            <div class="modal-content">
                <div class="modal-header light-blue darken-3 white-text">
                    <h4 class="title" data-lan-id="GMT"></h4>
                </div>
                <div class="modal-body">
                    <form action="Login.aspx" method="post" onsubmit="return check();">
                        <div class="md-form form-sm">
                            <i class="fa fa-user prefix"></i>
                            <input type="text" id="user" name="user" pattern="^[a-zA-Z0-9_+`!@#$%^&*;./:<>?]{3,18}$" title="" class="form-control">
                            <label for="user" data-lan-id="User"></label>
                        </div>

                        <div class="md-form form-sm">
                            <i class="fa fa-lock prefix"></i>
                            <input type="password" id="pwd" name="pwd" pattern="^[a-zA-Z0-9_+`!@#$%^&*;./:<>?]{3,18}$" title="" class="form-control">
                            <label for="pwd" data-lan-id="Password"></label>
                        </div>
                        <div class="text-center mt-2">
                            <button class="btn btn-info" type="submit" data-lan-id="Login"><i class="fa fa-sign-in ml-1"></i></button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <!--Modal: Login Form-->

</body>
</html>
<script src="../mdbootstrap/js/popper.min.js"></script>
<script src="../mdbootstrap/js/bootstrap.min.js"></script>
<script src="../mdbootstrap/js/mdb.js"></script>
<script>
    $(document).ready(function () {
        SetContentMsg();
        $('#modalLogin').modal({ keyboard: false, backdrop: false })
    })

    function check() {
        if ($('#user').val() == "") {
            alert(GetContentMsg('Error_User_Name_Null'));
            return false;
        }

        if ($('#pwd').val() == "") {
            alert(GetContentMsg('Error_User_Pwd_Null'));
            return false;
        }

        return true;
    }
</script>

