<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ParallelOrgnHierarchyBrandMapping.aspx.cs" Inherits="Masters_HO_Admin_ParallelOrgnHierarchyBrandMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShow" />
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="mainheading">
        Upload
    </div>

    <div class="contentbox">
        <div class="mandatory">(*) Marked fields are mandatory (++) Marked fields are optional.</div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>

                <%--    <td valign="top" width="10%">
                                            <asp:Label ID="lblActivity" runat="server" Text="Activity"></asp:Label><font class="error">*</font>
                                        </td>
                                        <td valign="top" width="10%">

                                            <asp:DropDownList ID="ddlUploadType" runat="server" CssClass="form_select">
                                                <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Add" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Update" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>--%>

                <li class="field3">
                    <div>
                        <div class="float-margin">
                            <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                OnClick="btnUpload_Click" />
                        </div>
                        <div class="float-left">
                            <asp:Button ID="BtnReset" CssClass="buttonbg" runat="server" Text="Reset"
                                OnClick="BtnReset_Click" />
                        </div>
                    </div>
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
            </ul>
            <ul>
                <li class="link">
                    <div class="float-margin">
                        <asp:LinkButton ID="DwnldReferenceCode" runat="server" Text="Download Referance Code" CssClass="elink2" OnClick="DwnldReferenceCode_Click"></asp:LinkButton>
                    </div>
                </li>
                <li class="link">
                    <div class="float-margin">
                        <a class="elink2" href="../../../Excel/Templates/ParallelOrgnHierarchyBrandMappingUploadTemplate.xlsx">Download Template</a>
                    </div>
                </li>
            </ul>
        </div>
    </div>

    <div class="mainheading">
        Search
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">Hierarchy Level Type:<span class="error">++</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList CausesValidation="false" ID="ddlSerHierarchyLevel" runat="server"
                                CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="ddlSerHierarchyLevel_SelectedIndexChanged">
                            </asp:DropDownList>
                        </li>
                        <li class="text">Location:<span class="error">++</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList CausesValidation="false" ID="ddlLocationSearch" runat="server"
                                CssClass="formselect" AutoPostBack="false">
                            </asp:DropDownList>
                        </li>
                        <li class="text">SalesChannel Code:<span class="error">++</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSalesChannelCodeSearch" runat="server" CssClass="formfields" MaxLength="100">
                            </asp:TextBox>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">Brand Code:<span class="error">++</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList CausesValidation="false" ID="ddlBrand" runat="server"
                                CssClass="formselect" AutoPostBack="false">
                            </asp:DropDownList>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                    OnClick="btnSearch_Click"></asp:Button>
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" OnClick="btnShow_Click"
                                    Text="Show All" ToolTip="Search" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        Mapping Search List
    </div>
    <div class="export">
        <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
            OnClick="btnExprtToExcel_Click" />
    </div>
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Always">
        <ContentTemplate>
            <div class="contentbox">
                <div class="grid1">
                    <asp:GridView ID="gvParallelOrgnHierarchyUser" runat="server" FooterStyle-VerticalAlign="Top"
                        FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                        HeaderStyle-HorizontalAlign="left" EmptyDataText="No Record found" HeaderStyle-VerticalAlign="top"
                        GridLines="none" AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow"
                        FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" CellSpacing="1"
                        PageSize='<%$ AppSettings:GridViewPageSize %>' CellPadding="4" bgcolor="" BorderWidth="0px"
                        Width="100%" AutoGenerateColumns="false" AllowPaging="True" SelectedStyle-CssClass="gridselected" OnRowCommand="gvParallelOrgnHierarchyUser_RowCommand">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                        <Columns>

                            <asp:TemplateField HeaderText="Hierarchy Level Type" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblHierarchyLevelName" runat="server" Text='<%# Eval("HierarchyLevelName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Location Code" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblLocationCode" runat="server" Text='<%# Eval("LocationCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location Name" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("LocationName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="SalesChannel Code" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalesChannelCode" runat="server" Text='<%# Eval("SalesChannelCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="SalesChannel Name" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalesChannelName" runat="server" Text='<%# Eval("SalesChannelName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Brand Code" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBrandCode" runat="server" Text='<%# Eval("BrandCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Brand Name" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBrandName" runat="server" Text='<%# Eval("BrandName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Valid From" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblValidFrom" runat="server" Text='<%# Eval("ValidFrom") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False"></ItemStyle>
                                <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnActiveDeactive"
                                        runat="server" CommandArgument='<%# Container.DataItemIndex+1 %>' CommandName="Active"
                                        ImageUrl='<%#BussinessLogic.PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#BussinessLogic.PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />

                                    <asp:HiddenField ID="hdnParallelOrgnSalesChannelBrandMappingID" runat="server" Value='<%#Eval("ParallelOrgnSalesChannelBrandMappingID") %>' />

                                    <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Eval("Status") %>' />

                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <%-- <PagerStyle CssClass="PagerStyle" />--%>
                    </asp:GridView>
                    <div class="clear">
                    </div>
                </div>
                <div id="dvFooter" runat="server" class="pagination">
                    <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShow" />
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:AsyncPostBackTrigger ControlID="gvParallelOrgnHierarchyUser" EventName="RowEditing" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
