﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesContributionRpt.aspx.cs"
    MasterPageFile="~/CommonMasterPages/ReportPage.master" Inherits="Reports_SalesChannel_SalesContributionRpt" %>

<%-----------------------#CC02,Vijay Kumar Prajapati,15/11/2017,Add Distributor code in List----------------------------------------------------%>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Search
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Type:<span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlSalesType" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlSalesType"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales type."
                            SetFocusOnError="true" ValidationGroup="SalesReport"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Date From:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                        ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />

                </li>
                <li class="text">Date To:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="SalesReport"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                        <asp:Button ID="btnSearch" Text="Show" runat="server" ValidationGroup="SalesReport"
                            ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                            OnClick="btnCancel_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlSearch" runat="server" Visible="false">
        <div class="mainheading">
            Export
        </div>
        <div class="contentbox">
            <div class="print-col">
                <ul>
                    <li class="frmtxt1">
                        Print options:
                    </li>
                    <li>
                        <asp:CheckBox ID="checkPrintHeadersOnEveryPage" runat="server" Text="Header on every page" />
                    </li>
                    <li>
                        <asp:CheckBox ID="checkPrintFilterHeaders" runat="server" Text="Filter header" Checked="True" />
                    </li>
                    <li>
                        <asp:CheckBox ID="checkPrintColumnHeaders" runat="server" Text="Column header" Checked="True" />
                    </li>
                    <li>
                        <asp:CheckBox ID="checkPrintRowHeaders" runat="server" Text="Row header" Checked="True" />
                    </li>
                    <li>
                        <asp:CheckBox ID="checkPrintDataHeaders" runat="server" Text="Data header" Checked="True" />
                    </li>
                </ul>
                <ul>
                    <li>
                        <strong>Export to:</strong>
                    </li>
                    <li>
                        <asp:DropDownList ID="ddlExportFormat" runat="server" Width="40px"
                            ValueType="System.String" CssClass="formselect">
                            <asp:ListItem Text="Excel" Value="1" />
                            <asp:ListItem Text="Excel (.xlsx)" Value="2" />
                        </asp:DropDownList>
                    </li>
                    <li>
                        <asp:Button ID="buttonSaveAs" runat="server" ToolTip="Export and save"
                            OnClick="buttonSaveAs_Click" Text="Save" CssClass="buttonbg" />
                        <asp:Button ID="buttonOpen" Visible="false" runat="server" ToolTip="Export and open"
                            Style="vertical-align: middle" OnClick="buttonOpen_Click" Text="Open" CssClass="buttonbg" />
                    </li>
                </ul>
            </div>
        </div>
        <div class="mainheading">
            Sales Contribution Infomation
        </div>
        <div class="contentbox">
            <div class="PivotGrid">
                <dxwpg:ASPxPivotGrid Width="100%" ID="ASPxPvtGrd" runat="server" EnableRowsCache="false">
                    <OptionsView ShowHorizontalScrollBar="false" />
                    <Fields>
                        <dxwpg:PivotGridField ID="PivotGridField1" Area="FilterArea" AreaIndex="0" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName1 %>" FieldName="HO">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField2" Area="FilterArea" AreaIndex="1" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName2 %>" FieldName="RBH">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField3" Area="FilterArea" AreaIndex="2" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName3 %>" FieldName="ZBH">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField4" Area="FilterArea" AreaIndex="3" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName4 %>" FieldName="SBH">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField5" Area="FilterArea" AreaIndex="4" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName5 %>" FieldName="ASO">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField17" Area="FilterArea" AreaIndex="5" EmptyValueText="N/A"
                            Caption="Country" FieldName="CountryName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField18" Area="FilterArea" AreaIndex="6" EmptyValueText="N/A"
                            Caption="State" FieldName="StateName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField6" Area="RowArea" AreaIndex="5" Caption="Sales From Name"
                            FieldName="SalesFromName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField7" Area="RowArea" AreaIndex="6" Caption="Sales To Name"
                            FieldName="SalesToName">
                        </dxwpg:PivotGridField>

                        <%--#CC01 START ADDED--%>
                        <dxwpg:PivotGridField ID="PivotGridField70" Area="RowArea" AreaIndex="8" FieldName="StockBinTypeDesc"
                            Caption="Stock Type">
                        </dxwpg:PivotGridField>
                        <%--#CC01 END ADDED--%>
                        <dxwpg:PivotGridField ID="PvtRetailerType" Area="RowArea" Visible="False" AreaIndex="3"
                            Caption="Retailer Type" FieldName="RetailerTypeName">
                        </dxwpg:PivotGridField>

                        <%--#CC02 START ADDED--%>
                        <dxwpg:PivotGridField ID="Pivotdistributorfromcode" Area="RowArea" AreaIndex="7" Caption="Sales From Code"
                            FieldName="SalesFromCode">
                        </dxwpg:PivotGridField>

                        <dxwpg:PivotGridField ID="Pivotdistributortocode" Area="RowArea" AreaIndex="8" Caption="Sales To Code"
                            FieldName="SalesToCode">
                        </dxwpg:PivotGridField>
                        <%--#CC02 START End--%>
                        <%--<dxwpg:PivotGridField ID="PivotGridField8" Area="RowArea" AreaIndex="7" Caption="Type"
                                                                FieldName="TYPE">
                                                            </dxwpg:PivotGridField>--%>
                        <dxwpg:PivotGridField ID="PivotGridField9" Area="ColumnArea" AreaIndex="0" FieldName="SKUName"
                            Caption="SKU Name">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField11" Area="FilterArea" AreaIndex="7" Caption="Category"
                            FieldName="ProductCategoryName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField8" Area="FilterArea" AreaIndex="8" Caption="Product"
                            FieldName="ProductName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField12" Area="FilterArea" AreaIndex="9" Caption="Brand"
                            FieldName="BrandName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField13" Area="FilterArea" AreaIndex="10" Caption="Model"
                            FieldName="ModelName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField14" Area="FilterArea" AreaIndex="11" Caption="Color"
                            FieldName="ColorName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField10" Area="FilterArea" AreaIndex="12" FieldName="SKUCode"
                            Caption="SKU Code ">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField15" Area="DataArea" CellFormat-FormatType="Numeric"
                            AreaIndex="0" Caption="Quantity" FieldName="Quantity">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField16" Area="DataArea" AreaIndex="1" CellFormat-FormatType="Numeric"
                            Caption="Sales Contribution(%)" FieldName="SalesContribution">
                        </dxwpg:PivotGridField>
                    </Fields>
                    <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                        <MenuStyle GutterWidth="0px" />
                    </Styles>
                    <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                        <CustomizationFieldsBackground Url="~/App_Themes/Aqua/PivotGrid/pcHeaderBack.gif">
                        </CustomizationFieldsBackground>
                        <LoadingPanel Url="~/App_Themes/Aqua/PivotGrid/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <OptionsLoadingPanel>
                        <Image Url="~/App_Themes/Aqua/PivotGrid/Loading.gif">
                        </Image>
                    </OptionsLoadingPanel>
                    <ImagesEditors>
                        <DropDownEditDropDown>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                        </DropDownEditDropDown>
                        <SpinEditIncrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                        </SpinEditIncrement>
                        <SpinEditDecrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                        </SpinEditDecrement>
                        <SpinEditLargeIncrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                        </SpinEditLargeIncrement>
                        <SpinEditLargeDecrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                        </SpinEditLargeDecrement>
                        <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                        </LoadingPanel>
                    </ImagesEditors>
                </dxwpg:ASPxPivotGrid>
                <dxpgex:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server" ASPxPivotGridID="ASPxPvtGrd"
                    Visible="False" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>