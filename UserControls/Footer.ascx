<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="Web.Controls.Footer" %>
<footer>
    <div class="container">

        <div class="copyrightnew">
            Power by <span class="ftrlogo">
                <img src='<%=strSiteUrl%><%= strAssets %>/CSS/Images/footerlogo.png'></span>
        </div>
    </div>

</footer>
<div id="updateProgressDiv" class="updateProgress" style="display: none;">
    <div align="center" class="Progress">
        Loading...<br />
        <img align="absmiddle" alt="" src='<%=siteURL%>Images/loading.gif' />
    </div>
</div>
