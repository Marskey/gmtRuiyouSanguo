<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="gmt.Login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link href="../mycss/docs.min.css" rel="stylesheet" media="screen" />
    <link href="../mycss/index-style.css" rel="stylesheet" media="screen" />
    <link href="../mycss/inspire.css" rel="stylesheet" media="screen" />
    <link href="../mycss/ripples.min.css" rel="stylesheet" media="screen" />
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body style="background:#0094ff">

    <!-- Modal Start -->
    <div id="modal_login" class="modal" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div>
                        <div>
                            <div class="welcome-text">
                                <h1 data-lan-id="GMT"></h1>
                            </div>
                            <form id="form1" runat="server">
                                <div class="form-group">
                                    <label data-lan-id="User" class="col-sm-2 control-label"></label>
                                    <asp:TextBox ID="userTextBox" class="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label data-lan-id="Password" class="col-sm-2 control-label"></label>
                                    <asp:TextBox ID="passwordTextBox" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
                                </div>
                                <asp:Button ID="LoginButton" CssClass="btn btn-block" data-lan-id="Login" data-lan-type="text" runat="server" Width="100" Text="" OnClick="LoginButton_Click" />
                                <br />
                                <asp:Label ID="outputLabel" runat="server"></asp:Label>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal End -->

</body>
</html>

<script>
    $(document).ready(function () {
        SetContentMsg();
        $('#modal_login').modal({ keyboard: false, backdrop: false })
    })
</script>
