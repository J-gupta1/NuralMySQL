<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucMessage.ascx.cs" Inherits="Controls_ucMessage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">

    function ShowPopupMessage() {
        debugger;
         var pnlgrid = document.getElementById('<%=pnlUcMessagegrid.ClientID%>');
       
        pnlgrid.style.display = "block";
      //  document.getElementById("somediv").innerHTML = decodeURIComponent(Str).replace("+", "  ");

//        dhtmlwindow.open('divbox', 'div', 'somediv', "Error Detail", 'width=450px,height=300px,left=200px,top=150px,resize=1,scrolling=1');
//        return false;

    }

</script>
<div class="message-area">
<div id="message">
    <asp:Panel ID="pnlUcMessageBox" runat="server" CssClass="error">
        <div style="float: left; margin-left: 40px; width: 80%;">
            <asp:Literal ID="litMessage" runat="server"></asp:Literal></div>
        <div style="float: right; width: 10%;">
            <a id="Atag" runat="server" visible="false" onclick="ShowPopupMessage();" class="viewerror">View Error </a>
        </div>

        <%--  <div id="somediv" style="display: none">
        </div>--%>
        <asp:Panel ID="pnlUcMessagegrid" runat="server" style="display: none" BackColor="AliceBlue" CssClass="modalPopup" >
       <!--  <a id="Aclose" runat="server" class="viewerror">Close</a>-->
        <div style="text-align:right;  padding-bottom:5px; margin::0px; width:420px;  "><asp:Button ID="btnCancelList" runat="server" CssClass="popbutton" Text=""  /></div>
       <div Class="modalPopupinner" id="test" runat="server">
        <cc1:ModalPopupExtender ID="pnlGrid_ModalPopupExtender" DropShadow="true"  BackgroundCssClass="modalBackground"  CancelControlID="btnCancelList"
            runat="server" TargetControlID="Atag" PopupControlID="pnlUcMessagegrid" >
        </cc1:ModalPopupExtender> 
       </div>
       
         
        
        </asp:Panel>
        <input type="hidden" id="hidn" runat="server" />
    </asp:Panel>
</div>
</div>
