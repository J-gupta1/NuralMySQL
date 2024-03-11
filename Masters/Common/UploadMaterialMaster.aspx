<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadMaterialMaster.aspx.cs" Inherits="Masters_Common_UploadMaterialMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
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

                <li class="link">
                    Note: In IsSerialBatch Column in Excel if you want to fill then should be  Serialized/Non-Serialized/Batchwise.
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

                 <li class="link">
                    Note: In ActionType Column in Excel if you want to inactive sku then enter Inactive in ActionType column.
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
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
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
                Search SKU
            </div>
            <div class="contentbox">
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblserprodcat" runat="server" Text="Product Category:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerProdCat" runat="server" OnSelectedIndexChanged="cmbSerProdCat_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="true">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsermodel" runat="server" Text="Model:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerModel" runat="server" CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsercolor" runat="server" Text="Color :"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSercolor" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblsername" runat="server" Text="SKU Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerName" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserCode" runat="server" Text="SKU Code:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerCode" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text"></li>
                        </ul>
                         <ul class="fullb">
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
        </Triggers>
        
    </asp:UpdatePanel>
    <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="exportToExel" Text="" runat="server" OnClick="exportToExel_Click"
            CssClass="excel" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdSKU" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="SKUID" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                        RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                        RowStyle-VerticalAlign="top" Width="100%"  EmptyDataText="No record found"
                        OnPageIndexChanging="grdSKUpage_indexchanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                        <Columns>
                            <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code "
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SKUName" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SKUDesc" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Description"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductCategoryName" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="Product Category" HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductName" HeaderStyle-HorizontalAlign="Left" HeaderText="Product"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="BrandName" HeaderStyle-HorizontalAlign="Left" HeaderText="Brand"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ModelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Model"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ColorName" HeaderStyle-HorizontalAlign="Left" HeaderText="Color"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>

                             <asp:BoundField DataField="CurrentStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                               >
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Attribute1" HeaderStyle-HorizontalAlign="Left" HeaderText="Attribute1"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Attribute2" HeaderStyle-HorizontalAlign="Left" HeaderText="Attribute2"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Attribute2" HeaderStyle-HorizontalAlign="Left" HeaderText="Attribute2"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                
               
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
