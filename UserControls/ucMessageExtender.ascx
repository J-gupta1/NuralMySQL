<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucMessageExtender.ascx.cs" Inherits="LuminousSMS.Controls.ucMessageExtender" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!--<script language="javascript" type="text/javascript">

    function ShowPopupMessage() {
		
		alert('sdf');
 var pnlGrid = document.getElementById('<%=UCpnlMsggrid.ClientID%>');
      pnlGrid.style.display = "block";
		//pnlgrid.style.visibility = "visible";
      //  document.getElementById("somediv").innerHTML = decodeURIComponent(Str).replace("+", "  ");

//        dhtmlwindow.open('divbox', 'div', 'somediv', "Error Detail", 'width=450px,height=300px,left=200px,top=150px,resize=1,scrolling=1');
//        return false;

    }

</script>-->

<style type="text/css">

/*.black_overlay{
			display: none;
			position: absolute;
			float:left;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
		
			background-color: black;
			z-index:100;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}*/



.pop {
position:fixed;
top:30%;
left:30%;
width:440px;
height:350px;
display:none;
z-index:1001;





}
</style>

<script language="JavaScript">

function setVisibility(visibility) {

document.getElementById('<%=dvShowError.ClientID%>').style.display = visibility;
}


</script>


<!--<script language="javascript" type="text/javascript">
function validcheck(){ document.getElementById('btnCancelList').style.visibility='hidden';} 
</script>-->



<asp:Panel ID="pnlUcMessageBox" runat="server" CssClass="error">

    <%--#CC07: commented START--%>
    <%--<table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left;">
        <tr>
            <td>
                <p style="float: left;">--%>
     <%--#CC07: commented END--%>

                    <div class="float-left" style="margin-left: 40px; width: 75%;"><%--#CC07--%>
                        <div style="visibility: hidden;">
                            <asp:TextBox ID="txtfocus" BorderWidth="0px" BackColor="Transparent" runat="server" CssClass="display-none"
                                BorderColor="Transparent" ReadOnly="True" Height="0px" Width="0px"></asp:TextBox><%--#CC07: CssClass added--%>
                        </div>
                        <%--#cc05 added--%>
                        <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                    </div>
                    <div class="float-right" style="width: 10%;"><%--#CC07--%>
                        <a id="billu" runat="server" visible="false" onclick="setVisibility('inline');" class="viewerror">View Error </a>
                    </div>
                    <%-- #CC06 Added Start --%>
                    <div class="float-right" style="width: 10%;"><%--#CC07 style replaced with class--%>
                        <a id="lnkTicketGeneration" runat="server" visible="false" class="viewerror">Generate Ticket </a>
                    </div>
                    <%-- #CC06 Added End --%>
                <div class="clear"></div><%--#CC07: div tag addded--%>
                <%--</p>--%><%--#CC07: commented--%>
                <div id="dvShowError" runat="server" class="pop" style="display: none; position: fixed; top: 30%; left: 30%; width: 440px; height: 350px; z-index: 1001;">
                    <div style="position: relative;">
                        <div style="width: 450px; float: right; position: absolute; z-index: 1004; text-align: right; margin: :0px;">
                            <input name="liqt" type="button" class="buttonpop" onclick="setVisibility('none');"
                                value="" />
                        </div>
                        <div style="position: relative; z-index: 1003;">
                            <asp:Panel ID="UCpnlMsggrid" runat="server" BackColor="AliceBlue" CssClass="modalPopup">
                                <!-- <asp:Button ID="btnCancelList"  runat="server" CssClass="button2" Text="X" />-->
                                <!--<div ID="pnlGrid_ModalPopupExtender"    runat="server"  >
            </div>-->
        </asp:Panel>
        </div>
        </div>
           </div>
        <input type="hidden" id="hidn" runat="server" />

<%--#CC07: commented START--%>
 <%--#CC07--%>      
    <%--</td>
  </tr>
</table>--%>
<%--#CC07--%>
<%--#CC07: commented END--%>

</asp:Panel>
<!--<div id="fade" class="black_overlay"></div>-->
