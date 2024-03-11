<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SalesChannelInfoFilterControl.ascx.cs"
    Inherits="UserControls_SalesChannelInfoFilterControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
 
<script language="javascript" type="text/javascript">
    function funcwindowclose(SalesChannelID, SalesChannelName) { 
    
        window.parent.parent.document.getElementById("ctl00_contentHolderMain_hdnID").value = SalesChannelID;
        window.parent.parent.document.getElementById("ctl00_contentHolderMain_hdnName").value = SalesChannelName;
        window.parent.parent.document.getElementById("ctl00_contentHolderMain_txtSalesChannelName").value = SalesChannelName;
        window.parent.parent.WinSearchChannelCode.hide();
       
        return false;
    }
</script>
<%--<uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />--%>
<table cellspacing="0" cellpadding="0" width="500" border="0" style="height: 142px">
  
    <tr>
        <td>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td align="left" valign="top">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left" valign="top" class="tableposition">
                                    <div class="mainheading">
                                        Search</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" class="tableposition">
                        <div class="contentbox">
                            <table cellspacing="0" cellpadding="4" width="100%" border="0">
                               
                                <tr>
                                    <td class="formtext" valign="top" align="right" width="10%" height="15">
                                        <asp:Label ID="Label1" runat="server" Text="Parent Location"></asp:Label>
                                    </td>
                                    <td width="10%" align="left" valign="top">
                                        <div style="float: left; width: 135px;">
                                            <asp:DropDownList ID="cmbParentLocation" runat="server"  CssClass="form_select">
                                            </asp:DropDownList>
                                        </div>
                                        </td>
                                       
                                     
                                        <td class="formtext" valign="top" align="right" width="10%" height="35">
                                            <asp:Label ID="lblstate" runat="server" Text="Location"></asp:Label>
                                        </td>
                                    <td width="25%" align="left" valign="top">
                                        <div style="float: left; width: 135px;">
                                            <asp:DropDownList ID="cmbLocation" runat="server"  CssClass="form_select">
                                            </asp:DropDownList>
                                        </div>
                                     
                                        </td>
                                        </tr>
                                        <tr>
                                    <td  class="formtext" valign="top" align="right" width="15%">
                                        <asp:Label ID="lbldst" runat="server" Text="Parent Sales Channel"></asp:Label>
                                    </td>
                                    <td colspan="6" width="20%" align="left" valign="top">
                                        <div style="float: left; width: 135px;">
                                            <asp:DropDownList ID="cmbParentSalesChannel" runat="server"  CssClass="form_select">
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <asp:HiddenField ID="hdnID" runat="server" Value="0" />
                                      
                                    </td>
                                     <td>
                                        <asp:Button ID="btnSubmit" Text="Search" runat="server" OnClick="btnSearch_click"
                                            CssClass="buttonbg" />
                                        &nbsp; &nbsp; &nbsp;
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_click"
                                            CssClass="buttonbg" />
                                    </td>
                                </tr>
                                <tr>
                                  
                                   
                                </tr>
                            </table>
                            <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td style="height:10px; background-color:#fff"></td></tr>
    <tr>
        <td>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="top" class="tableposition">
                        <div class="mainheading">
                            List</div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <div class="contentbox11">
                <div class="grid1">
                    <asp:GridView ID="grdSalesChannelDetails" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="GrdRow_DataBound" CellPadding="4" CellSpacing="1" DataKeyNames="SalesChannelId"
                        EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found" GridLines="None"
                        HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="gridrow1"
                        Width="100%">
                        <RowStyle CssClass="gridrow" />
                        <Columns>
                            <asp:BoundField DataField="HOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>"
                                HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>"
                                HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ZSMName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>"
                                HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName4 %>"
                                HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ASOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName5 %>"
                                HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        
                            
                              <asp:TemplateField HeaderText="SalesChannelName" Visible="True">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalesChannelName" runat="server" Text='<%# Bind("SalesChannelName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                HeaderText="SalesChannel Code">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="SalesChannleid" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalesChannelID" runat="server" Text='<%# Bind("SalesChannelID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SalesChannleTypeid" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalesChannelTypeID" runat="server" Text='<%# Bind("SalesChannelTypeID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelTypeName"
                                HeaderText="SalesChannelType">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="buttonbg" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" />
                        <EditRowStyle CssClass="editrow" />
                        <AlternatingRowStyle CssClass="gridrow1"></AlternatingRowStyle>
                    </asp:GridView>
                </div>
            </div>
        </td>
    </tr>
</table>
<%--<table cellspacing="0" cellpadding="0" width="965" border="0" style="height: 142px">
   
           
                <tr>
                    <td align="left" valign="top">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left" valign="top" class="tableposition">
                                    <div class="mainheading">
                                        Search</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                <td>
                 <div class="contentbox">
          
                 <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td class="formtext" valign="top" align="right" width="10%">
                        <asp:Label ID="Label2" runat="server" Text="">Parent Location:<font class="error">*</font></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbParentLocation" runat="server" CssClass="form_select">
                        </asp:DropDownList>
                    </td>
                    <td class="formtext" valign="top" align="right" width="10%">
                        <asp:Label ID="Label4" runat="server" Text="">Location:<font class="error">*</font></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbLocation" runat="server" CssClass="form_select" >
                        </asp:DropDownList>
                    </td>
                    <td class="formtext" valign="top" align="right" width="10%">
                        <asp:Label ID="Label6" runat="server" Text="">Parent SalesChannel:<font class="error">*</font></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbParentSalesChannel" runat="server" CssClass="form_select"
                            >
                        </asp:DropDownList>
                    </td>
                </tr>
                  <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSubmit" Text="Search" runat="server" OnClick="btnSearch_click"
                            CssClass="buttonbg" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_click"
                            CssClass="buttonbg" />
                    </td>
                </tr>
           </table>
        
           </div>
           </td>
     </tr>
     
    <tr>
        <td>
        </td>
    </tr>
    <asp:UpdatePanel id="updGrid" updatemode="conditional" runat="server">
        <contenttemplate>
        
    <tr>
    <td>
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading">
                                                &nbsp;List</div>
                                        </td>
                                        <td width="10%" align="right">
                                            <div style="float: right">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
      </td>
     </tr>
    <tr>
                            <td>
                               <asp:GridView ID="grdSalesChannelDetails" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="GrdRow_DataBound" CellPadding="4" CellSpacing="1" DataKeyNames="SalesChannelId"
                                    EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found" GridLines="None"
                                    HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="gridrow1"
                                    Width="100%">
                                    <RowStyle CssClass="gridrow" />
                                    <Columns>
                                        <asp:BoundField DataField="HOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ZSMName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName4 %>"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ASOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName5 %>"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelName"
                                            HeaderText="SalesChannel">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                            HeaderText="SalesChannel Code">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="SalesChannleid" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesChannelID" runat="server" Text='<%# Bind("SalesChannelID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SalesChannleTypeid" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesChannelTypeID" runat="server" Text='<%# Bind("SalesChannelTypeID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelTypeName"
                                            HeaderText="SalesChannelType">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ParentSalesChannelType"
                                            HeaderText="Parent SalesChannelType">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ParentSalesChannelName"
                                            HeaderText="Parent SalesChannelName">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select" ShowHeader="False">
                                            <ItemTemplate>
                                                <dx:ASPxButton ID="btnSelect" runat="server" Text="Select">
                                                </dx:ASPxButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridheader" />
                                    <EditRowStyle CssClass="editrow" />
                                    <AlternatingRowStyle CssClass="gridrow1"></AlternatingRowStyle>
                                </asp:GridView>
                            </td>
                        </tr>
                       
        
    </ContentTemplate>
    </asp:UpdatePanel>
   
</table>--%>