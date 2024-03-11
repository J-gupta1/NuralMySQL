<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucDisplayBoard.ascx.cs"
    Inherits="BulletinBoard_ucDisplayBoard" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Import Namespace="BussinessLogic" %>

<script language="javascript" type="text/javascript">
    function BulletinDetail(BulletinId) {
        window.location = "Masters/Common/BulletinDetail.aspx?BulletinId=" + BulletinId;
        return false;
    }
       </script>
    

<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <%--<tr>
        <td align="center" style="text-align: justify;">
            <div id="divBulletin" runat="server">
            </div>
        </td>
    </tr>
    <tr>
        <td align="right" style="height: 25px">
            <a class="news" style="font-weight: bold; color: white">read more...</a>
        </td>
    </tr>--%>
    <marquee id="ml" style="text-align: left" direction="up" width="185" height="200"
        scrolldelay="10" scrollamount="1" onmouseover='this.stop();'
        onmouseout='this.start();'>
<asp:Repeater ID="RptMarquee" runat="server" OnItemDataBound="RptMarquee_ItemDataBound" 
  >
    <ItemTemplate><asp:Label ID="lblBulletinID" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"BulletinID"))%>'
                                                                    Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblExpiryDate" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpiryDate"))%>'
                                                                    Visible="false"></asp:Label>
        
      
        <asp:HyperLink CssClass="bulletinlnk" id="hlEdit"  Text='<%# (DataBinder.Eval(Container.DataItem,"BulletinSubject"))%>' runat="server">
        </asp:HyperLink><br /><br />
    </ItemTemplate>
</asp:Repeater>
         </marquee>
</table>
