<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucCheckSessionTimeout.ascx.cs" Inherits="UserControls_ucCheckSessionTimeout" %>

<script type="text/javascript">
    function checkinterval() {
        setInterval(function () { startTimer() }, 1000);
    }
    function startTimer() {
        var vrsesiontimehdn = document.getElementById('<%=hdnSessionTimeout.ClientID%>');
        var vrRedirectTo = document.getElementById('<%=hdnRedirectToPage.ClientID%>');

        vrsesiontimehdn.value = parseInt(vrsesiontimehdn.value) - 1;
        if (vrsesiontimehdn.value < 1) {
            window.location = vrRedirectTo.value;
        }
    }

    /*
    function Redirect(path) {
        alert(path);
        window.location = path;
    }
    // document.write("You will be redirected to a new page in 5 seconds");
    //setTimeout('Redirect()', 5000);
    */

</script>

<div id="dvCheckSession" style="float: left; width: 100%;">
    <asp:UpdatePanel ID="upSession" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="hdnSessionTimeout" runat="server" />
            <asp:HiddenField ID="hdnRedirectToPage" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
