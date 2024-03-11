<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadEntityBrandMapping.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Masters_Common_UploadEntityBrandMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="clear"></div>

    <div class="mainheading">
        Step 1 : Click On Button For Save Or Update Process
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">

            <ul>
                <li class="link">
                    <asp:RadioButtonList ID="Rbtdownloadtemplate" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="radio-rs" CellPadding="2" CellSpacing="0" OnSelectedIndexChanged="Rbtdownloadtemplate_SelectedIndexChanged">
                        <asp:ListItem Value="1" Text="Save" Selected="True">
                            
                        </asp:ListItem>
                        <asp:ListItem Value="2" Text="Update">
                            
                        </asp:ListItem>
                    </asp:RadioButtonList>
                </li>


            </ul>
        </div>
    </div>
    <div class="mainheading" runat="server" id="ForSaveTemplateheading" visible="false">
        Step 2 : Download  Template For Save Record 
    </div>
    <div class="contentbox" runat="server" id="ForSaveTemplatedownload" visible="false">
        <div class="H25-C3-S">

            <ul>
                <li class="link">
                    <asp:LinkButton ID="DwnldTemplate" runat="server" Text="Download Template File"
                        CssClass="elink2" OnClick="DwnldTemplate_Click"></asp:LinkButton>
                </li>
                <li class="link">Note: In Status Column in Excel  should be  Active/Inactive.
                </li>
            </ul>
        </div>
    </div>
    <div class="mainheading" runat="server" id="ForUploadTemplateheading" visible="false">
        Step 2 : Download  Template For  Update Record 
    </div>
    <div class="contentbox" runat="server" id="ForUpdateTemplatedownload" visible="false">
        <div class="H25-C3-S">

            <ul>
                <li class="link">
                    <asp:LinkButton ID="UpdateTemplateFile" runat="server" Text="Download Template File"
                        CssClass="elink2" OnClick="UpdateTemplateFile_Click"></asp:LinkButton>
                </li>
                <li class="link">Note: In Status Column in Excel  should be  Active/Inactive.
                </li>

            </ul>
        </div>
    </div>
    <div class="mainheading" runat="server" id="ReferenceIdForsaveheading" visible="false">
        Step 3 : Download Reference Code For Save Record 
    </div>
    <div class="contentbox" runat="server" id="ReferenceIdForsave" visible="false">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DownloadReferenceCodeForSave" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DownloadReferenceCodeForSave_Click"></asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>

    <div class="mainheading" runat="server" id="ReferenceIdForupdateheading" visible="false">
        Step 3 : Download Reference Code For Update Record 
    </div>
    <div class="contentbox" runat="server" id="ReferenceIdForupdate" visible="false">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DownloadReferenceCodeForUpdate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DownloadReferenceCodeForUpdate_Click"></asp:LinkButton>
                </li>

            </ul>
        </div>
    </div>
    <div class="mainheading">
        Step 4 : Upload Excel File
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">

            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload"
                        OnClick="btnUpload_Click" />
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="mainheading">
                Search Entity Brand Mapping
            </div>
            <div class="contentbox">
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblentitytype" runat="server" Text="Entity Type:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlEntityType" runat="server" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="true">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblentityname" runat="server" Text="Entity Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlEntityName" runat="server" CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblbrand" runat="server" Text="Brand:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlBrand" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblcategory" runat="server" Text="Product Category:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlProductcategory" runat="server" CssClass="formselect"></asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblstatus" runat="server" Text="Status:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formselect">
                                <asp:ListItem Text="Select" Value="255"></asp:ListItem>
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="fillallgrid" Text="Show All Data" runat="server" OnClick="fillallgrid_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="fillallgrid" />
        </Triggers>

    </asp:UpdatePanel>
    <asp:Panel ID="PnlGrid" Visible="false" runat="server">
    <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="exportToExel" Text="" runat="server" OnClick="exportToExel_Click"
            CssClass="excel" />
    </div>
    <div class="contentbox">
        <div class="grid">
            <asp:GridView ID="gvEntityMappingDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                bgcolor="" BorderWidth="0px" DataKeyNames="BrandMappingID" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                <PagerStyle CssClass="gridfooter" />
                <Columns>

                    <asp:BoundField DataField="BrandName" HeaderStyle-HorizontalAlign="Left" HeaderText="Brand Name"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductCategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Product Category Name"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="EntityType" HeaderStyle-HorizontalAlign="Left" HeaderText="Entity Type"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="EntityName" HeaderStyle-HorizontalAlign="Left" HeaderText="Entity Name"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="EntityCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Entity Code"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreationDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Creation Date"
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
        </asp:Panel>
</asp:Content>
