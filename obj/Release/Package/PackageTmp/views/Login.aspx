<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="gmt.Login" %>

<!DOCTYPE html>

<html >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        body {
            margin: 0 auto;
            color: #586e75;
            display: block;
            background: #0094ff;
        }

        .wrap {
            width: 90%;
            margin: 0 auto;
        }

        .Context {
            position: relative;
            top: 200px;
            left: 400px;
            width: 400px;
            height: 300px;
            background: #fff;
            border: 1px solid red;
        }

        table {
            margin-left: 10px;
            margin-top: 80px;
        }

        tr {
            height: 50px;
        }

        p {
            position: absolute;
            top: 100px;
            left: 300px;
            font-size: 30px;
            color: #000;
        }
    </style>
</head>
<body>
    <div class="wrap">
        <p>
            <asp:Label ID="titleTipLabel" runat="server"></asp:Label></p>
        <form id="form1" runat="server">
            <div class="Context">
                <table>
                    <tr>
                        <th>
                            <asp:Label ID="yonghu" runat="server"></asp:Label></th>
                        <td>
                            <asp:TextBox ID="userTextBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="mima" runat="server"></asp:Label></th>
                        <td>
                            <asp:TextBox ID="passwordTextBox" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="1" style="text-align: center">&nbsp&nbsp&nbsp
                            
				    <asp:Button ID="LoginButton" runat="server" Width="100" Text="" OnClick="LoginButton_Click" />
                        </th>
                        <th>
                            <asp:Button ID="changepasswordButton" runat="server" Width="100" Text="" OnClick="changepasswordButton_Click" />
                        </th>
                    </tr>
                </table>
                <br />
                <asp:Label ID="outputLabel" runat="server"></asp:Label>
            </div>
        </form>
    </div>
</body>
</html>
