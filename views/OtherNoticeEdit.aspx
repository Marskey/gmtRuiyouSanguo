<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherNoticeEdit.aspx.cs" Inherits="gmt.OtherNoticeEdit" %>

<!DOCTYPE html>

<html >
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <script type="text/javascript" src="../js/global.js"></script>
    <script type="text/javascript" src="../js/language.js"></script>
</head>
<body>
    <header id="header" class="navbar navbar-static-top" style="margin-top: 0; margin-bottom: 0; border-bottom: 0; background: #337ab7"> </header>
    <div class="father">
        <fieldset>
            <legend></legend>

            <form id="form1" runat="server">
                <h1 style="color: blue"><b><label data-lan-id="Before_Game_Notice"></label></b></h1>
                <div>
                    <table>
                        <tr>
                            <td colspan="2"><a href="GmModify.aspx" data-lan-id="Return_GMT"></a></td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td><label data-lan-id="Before_Game_Notice_Editor"></label></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="Noticetxt" runat="server" Width="250px" Height="200" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="color: #00FF00">[00ff00][-]</td>
                                    </tr>
                                    <tr>
                                        <td style="color: #0000FF">[0000ff][-]</td>
                                    </tr>
                                    <tr>
                                        <td style="color: #FFFF00">[ffff00][-]</td>
                                    </tr>
                                    <tr>
                                        <td style="color: #00FFFF">[00ffff][-]</td>
                                    </tr>
                                    <tr>
                                        <td style="color: #FF00FF">[ff00ff][-]</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="saveButton" data-lan-id="Save_Notice" data-lan-type="text" runat="server" Text="" OnClick="saveButton_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="uploadButton" data-lan-id="Upload" data-lan-type="text" runat="server" Text="" OnClick="uploadButton_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style9">
                                <asp:Label ID="OutputLable" runat="server" visable="true" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>

                </div>
            </form>
        </fieldset>
    </div>
</body>
</html>
