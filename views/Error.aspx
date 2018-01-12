<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="gmt.Error" %>


<script type="text/javascript" src="../bootstrap/js/jquery-2.0.2.min.js"></script>
<script type="text/javascript" src="../js/global.js"></script>
<script type="text/javascript" src="../js/language.js"></script>
<script>
    $(document).ready(function () {
        alert(GetContentMsg("Error_Tip"));
        history.back(-1);
    });
</script>
