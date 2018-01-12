<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="gmt.Login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <link type="text/css" rel="stylesheet" href="../mycss/bootstrap-material-design.min.css" />
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


<script src="../js/angular.min.js"></script>
<script src="../js/angular-route.js"></script>
<script src="../js/material.min.js"></script>
<script src="../js/mainApp.js"></script>
<script src="../js/indexCtrl.js"></script>
<script src="../js/loginCtrl.js"></script>
<script>
    $(document).ready(function () {
        SetContentMsg();
        $('#loginModal').modal({ keyboard: false, backdrop: false })
    })
</script>

<!--Import Login Modal -->
<!--Login Modal -->
<div id="loginModal" class="modal" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content"> 
            <button type="button" class="close" data-dismiss="modal">×</button>
            <div class="modal-body">
                <div ng-controller="loginCtrl">
                    <div ng-show="showLoginForm">
                        <div class="welcome-text">
                            <h3>Let's get started!</h3>
                        </div>
                        <div class="welcome-desc">
                            <p>Login/Signup to create, validate & hand-off prototypes</p>
                        </div>
                        <form role="form" ng-model="signInForm" novalidate="" id="signInForm" name="signInForm" ng-submit="signInForm.$valid && submitSignIn()">
                            <div class="form-group form-group-lg label-floating is-empty">
                                <label for="i6" class="control-label">Email Address</label>
                                <input class="form-control" ng-required="true" ng-model="signInLoginId" name="loginId" type="email" id="loginId" required ng-change="hideError()"/>                                       
                                <p ng-cloak class="help-text text-danger" ng-show="signInForm.$submitted && !signInForm.loginId.$error.required && signInForm.loginId.$invalid"><span class="text-red">* </span>Enter a valid email.</p>
                                <p ng-cloak class="help-text text-danger" ng-show="signInForm.$submitted && signInForm.loginId.$error.required"> <span class="text-red">* </span>Email is required.</p>
                                <p ng-cloak class="help-text text-danger" id="signInIncorrectEmail"><span class="text-red">* </span>The E-mail you entered is incorrect.</p>
                            </div>
                            <div class="form-group form-group-lg label-floating is-empty">
                                <label for="signInPassword" class="control-label">Password</label>
                                <input ng-minlength="6" ng-maxlength="20" ng-required="true" ng-model="signInPassword" name="password" type="password" id="signInPassword" ng-change="hideError()" required class="form-control"/>
                                <p ng-cloak class="help-text text-danger" ng-show="signInForm.$submitted && signInForm.password.$error.required"> <span class="text-red">* </span>Password is required</p>
                                <p ng-cloak class="help-text text-danger" ng-show="signInForm.$submitted && signInForm.password.$error.minlength"> <span class="text-red">* </span>Password is too short</p>
                                <p ng-cloak class="help-text text-danger" ng-show="signInForm.$submitted && signInForm.password.$error.maxlength"> <span class="text-red">* </span>Password is too long</p>
                                <p ng-cloak class="help-text text-danger" id="signInIncorrectPassword" ng-cloak><span class="text-red">* </span>Your password is incorrect.</p>                               
                            </div>
                            <!-- send ref code with formdata -->
                            <input name="refCode" type="hidden" ng-bind="refCode" value="{{refCode}}"/>
                            <input name="refCodeBy" type="hidden" ng-bind="refCodeBy" value="{{refCodeBy}}"/>
                            <div class="forgotten-text">
                                <p class="text-right" data-ga data-ga-action="click" data-ga-category="Login Form " data-ga-title="Toggle Login Form FP Btn"><a href="#"  ng-click="toggleForgotPasswordForm()">Forgotten Password?</a></p>
                            </div>
                            <button data-ga data-ga-action="click" data-ga-category="Login Form " data-ga-title="Submit Login Btn" type="submit" data-loading-text="Login<div class='btn-loader btn-loader-blue'></div>" class="btn btn-block">Login</button>
                        </form>
                    </div>
                </div>
            </div>                  
        </div>
    </div>
</div>
