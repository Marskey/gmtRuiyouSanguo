<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Passwordchange.aspx.cs" Inherits="gmt.Passwordchange" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link href="../mycss/docs.min.css" rel="stylesheet" media="screen" />
    <link rel="stylesheet" href="../mycss/style.css" />
    <script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" src="../bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <header class="navbar navbar-static-top bs-docs-nav" id="header"></header>
    <div class="bs-docs-header" id="content">
        <div class="container">
            <h1 data-lan-id="Change_Password"></h1>
        </div>
    </div>
    <form id="form1" runat="server" class="form-horizontal">
        <div class="form-group">
            <label data-lan-id="User_Name" class="col-sm-2 control-label"></label>
            <div class="col-sm-5">
                <asp:TextBox ID="userTextBox" class="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label data-lan-id="Old_Password" class="col-sm-2 control-label"></label>
            <div class="col-sm-5">
                <asp:TextBox ID="oldpassword" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label data-lan-id="New_Password" class="col-sm-2 control-label"></label>
            <div class="col-sm-5">
                <asp:TextBox ID="newpassword" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-5">
                <asp:Button ID="LoginButton" class="btn btn-primary" data-lan-id="Modify" data-lan-type="text" runat="server" Text="" OnClick="LoginButton_Click" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Label ID="outputLabel" runat="server" class="control-label"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
