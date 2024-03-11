<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadBrandSalesChannelMapping.aspx.cs" Inherits="Masters_SalesChannel_UploadBrandSalesChannelMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0">
        <tr>
            <td align="left" valign="top">
                <table cellspacing="0" cellpadding="0" width="965" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                                </td>
                                            </tr>
                                        </table>



                                        <tr>
                                            <td align="left" valign="top">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td align="left" valign="top" class="tableposition">
                                                                        <div class="mainheading">
                                                                            Upload Sales Channel Brand Mapping
                                                                        </div>
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
                                                                        <td colspan="6" height="20" class="mandatory" valign="top">(*) marked fields are mandatory.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="35" align="left" width="10%" class="formtext" valign="top">Upload File:<font class="error">*</font>
                                                                        </td>
                                                                        <td width="20%" align="left" class="formtext" valign="top">
                                                                            <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                                                                            <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                                                                        </td>
                                                                        <td align="left" width="10%" class="formtext" valign="top">
                                                                            <asp:Button ID="Button1" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                                                OnClick="btnUpload_Click" ValidationGroup="Upload" />
                                                                        </td>
                                                                        <td align="left" width="10%" class="formtext" valign="top">
                                                                            <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                                                CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton></td>
                                                                        <td align="left" width="10%" class="formtext" valign="top">
                                                                            <a class="elink2" href="../../Excel/Templates/BrandSalesChannelMapping.xlsx">Download Template</a>
                                                                            <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
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
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td align="left" valign="top" class="tableposition">
                                                                <div class="mainheading">
                                                                    Search Sales Channel
                                                                </div>
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
                                                                <td colspan="6" height="20" class="mandatory" valign="top">(*) marked fields are mandatory.
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right" width="15%"> Type:<font class="error">*</font>
                                                                </td>
                                                                <td valign="top" align="left" width="19%">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlSalesChannelType" runat="server" CausesValidation="true"
                                                                            CssClass="form_select">
                                                                        </asp:DropDownList>

                                                                    </div>
                                                                    <div style="width: 160px;">
                                                                        <asp:RequiredFieldValidator ID="ReqUserGroup0" runat="server" ControlToValidate="ddlSalesChannelType"
                                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please select Sales Channel type."
                                                                            InitialValue="0" SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </td>

                                                                <td valign="top" align="right" width="17%" height="35">Brand Name:<font class="error">++</font>
                                                                </td>
                                                                <td width="19%" align="left" valign="top">
                                                                    <div style="width: 160px;">
                                                                        <asp:DropDownList CausesValidation="true" ID="ddlBrandName" runat="server"
                                                                            CssClass="form_select">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                   
                                                                </td>
                                                                <td valign="top" align="right" width="17%" height="35">Product Category Name:<font class="error">++</font>
                                                                </td>
                                                                <td width="19%" align="left" valign="top">
                                                                    <div style="width: 160px;">
                                                                        <asp:DropDownList CausesValidation="true" ID="ddlProductCategory" runat="server"
                                                                            CssClass="form_select">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    
                                                                </td>
                                                                

                                                            </tr>

                                                            <tr>
                                                                <td align="left" valign="top" height="35"></td>
                                                                <td align="left" valign="top">
                                                                    <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                                                                        Text="Search" ToolTip="Search" OnClick="btnSearch_Click" />
                                                                    <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                                                                        OnClick="btnCancel_Click1" />
                                                                </td>
                                                                <td width="10%" align="right">
                                                                    <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False" OnClick="btnExprtToExcel_Click" Visible="false" />
                                                                </td>
                                                                <td align="left" valign="top" colspan="3">&nbsp;
                                                                </td>
                                                                <td align="left" valign="top">&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="10"></td>
                    </tr>
                    <tr>
                        <td align="left" height="10"></td>
                    </tr>
                    <asp:Panel ID="pnlHide" runat="server" Visible="false">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading">
                                                Sales Channel List
                                            </div>
                                        </td>
                                        <td width="10%" align="right"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="contentbox">
                                    <div class="grid2">
                                        <asp:GridView ID="grdSalesChannelList" runat="server" FooterStyle-VerticalAlign="Top"
                                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" EmptyDataText="No Record found"
                                            RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                            GridLines="none" AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow"
                                            FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" CellSpacing="1"
                                            CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                                            AllowPaging="True" PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                                            OnPageIndexChanging="grdSalesChannelList_PageIndexChanging" DataKeyNames="SalesChannelID"
                                            OnRowCommand="grdSalesChannelList_RowCommand">
                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                            <Columns>

                                                <asp:BoundField DataField="SalesChannelTypename" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderText="Sales Channel Type" HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Code"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ParentName" HeaderStyle-HorizontalAlign="Left" HeaderText="Parent Name"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LocationName" HeaderStyle-HorizontalAlign="Left" HeaderText="Repo. Hierarchy Name"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Email" HeaderStyle-HorizontalAlign="Left" HeaderText="Email"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MobileNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProductCategory" HeaderStyle-HorizontalAlign="Left" HeaderText="Product Category"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Brand" HeaderStyle-HorizontalAlign="Left" HeaderText="Brand Name"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>

                                            </Columns>
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                        <tr>
                                            <td align="right" valign="top" height="5" class="formtext"></td>
                                        </tr>

                                    </div>
                                </div>
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td align="left" height="10"></td>
                    </tr>
                </table>
</asp:Content>

