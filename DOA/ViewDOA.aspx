<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ViewDOA.aspx.cs" Inherits="DOA_ViewDOA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function SelectCheckboxes(headerChk, grdId, columnIndex) {
            var IsChecked = headerChk.checked;
            var tbl = document.getElementById('<%= GridDOA.ClientID %>');
            for (i = 1; i < tbl.rows.length; i++) {
                var curTd = tbl.rows[i].cells[columnIndex];
                var item = curTd.getElementsByTagName('input');
                for (j = 0; j < item.length; j++) {
                    if (item[j].type == "checkbox") {
                        if (item[j].checked != IsChecked) {
                            item[j].click();
                        }
                    }
                }
            }
        }    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td align="left" valign="top">
                                <uc4:ucMessage ID="ucMessage1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                          
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        View DOA
                                    </div>
                                    <div class="mainheading_rpt_right">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <div class="contentbox">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td class="mandatory" colspan="5" height="20" valign="top" align="left">(<font class="error">*</font>) marked fields are mandatory.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="15%" align="right" valign="top" class="formtext" height="35">DOA Certificate No:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtDoaCertificateno" CssClass="form_input2" runat="server" MaxLength="17"></asp:TextBox>

                                                        </td>
                                                        <td width="10%" align="right" valign="top" class="formtext">IMEI:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtIMEINo" runat="server" CssClass="form_input2" MaxLength="15"></asp:TextBox>
                                                        </td>
                                                        <td width="10%" align="right" valign="top" class="formtext">Status:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:DropDownList ID="ddldoastatus" Width="120px" runat="server" CssClass="form_select">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="right" class="formtext">From Date:<font class="error">+</font>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                        <td align="right" valign="top" class="formtext">To Date:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                        <td align="left" valign="top"></td>
                                                        <td align="left" valign="top">
                                                            <asp:Button ID="Search" runat="server" Text="Search" CssClass="buttonbg"
                                                                CausesValidation="true" ValidationGroup="Search" OnClick="Search_Click" />
                                                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                                                CausesValidation="false" OnClick="Cancel_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="5"></td>
                        </tr>     
                    </table>
                </div>
                <div>
                    
                </div>
                <div>
                    <asp:Panel ID="dvhide" runat="server" Visible="true">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td height="5"></td>
                                        </tr>
                                        <tr>
                                            <td width="60%" align="left" class="tableposition">
                                                <div class="mainheading">
                                                    &nbsp;List
                                                </div>
                                            </td>
                                            <td width="20%" align="right">
                                                <asp:Button ID="btnAcknowledgement" runat="server" CssClass="buttonbg" Text="Acknowledgement" OnClick="btnAcknowledgement_Click" />
                                            </td>

                                            <td width="13%" align="right">
                                                <asp:Button ID="btnExport" runat="server" CssClass="excel" Text="" OnClick="btnExport_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="contentbox">
                                                    <div class="grid1">
                                                        <asp:GridView ID="GridDOA" runat="server" AlternatingRowStyle-CssClass="gridrow1"
                                                            bgcolor="" BorderWidth="0px" EmptyDataText="No record found" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                                            DataKeyNames="DOAID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="1250px"
                                                            AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                            <PagerStyle CssClass="gridfooter" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" Visible="false">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="checkAll" Text="Select" AutoPostBack="True" OnCheckedChanged="checkAll_CheckedChanged" ToolTip="Select Acknowlegement" runat="server" onclick="SelectCheckboxes(this,'<%=GridDOA.ClientID %>', 0);" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="True" OnCheckedChanged="chkRow_CheckedChanged" Visible='<%# Convert.ToBoolean(Eval("receiveAllowed")) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DOACertificateNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="DOA Certificate No."
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="DOA State"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SvcCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SVC Code"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ModelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Model Name"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU CODE"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IMEINumber" HeaderStyle-HorizontalAlign="Left" HeaderText="IMEI Number"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Name"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                 <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannel Name"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DOACertificateGenerateDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Doa Date"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoaReceiveDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Doa ReceiveDate"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                               <%-- <asp:BoundField DataField="Price" HeaderStyle-HorizontalAlign="Left" HeaderText="Price"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Invoice Number"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                <asp:BoundField DataField="DOAStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="DOA Status"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <div id="dvFooter" runat="server" class="pagination">
                                                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" height="5">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" height="5"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
                <asp:PostBackTrigger ControlID="Search" />
                <asp:PostBackTrigger ControlID="Cancel" />           
                <asp:PostBackTrigger ControlID="btnAcknowledgement" />        
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
