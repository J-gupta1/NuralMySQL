<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    AutoEventWireup="true" CodeFile="BTMReport.aspx.cs" Inherits="Reports_SalesChannel_BTMReport" %>

<%@ Register Src="~/UserControls/Footer.ascx" TagName="ucFooter" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Header.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
                                                                </asp:ScriptManager>--%>
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Stock Transfer Report Search
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <!-- <tr>
                                                                            <td colspan="7" height="20" class="mandatory" valign="top">
                                                                                (<font class="error">*</font>) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>-->
            <ul>
                <li class="text">From Sales Channel Type:
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbFrom" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbFrom_SelectedIndexChanged">
                    </asp:DropDownList>
                </li>
                <li class="text">Date From:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Please enter a from date."
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date."
                        ValidationGroup="insert" />
                </li>
                <li class="text">Date To:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Please enter a to date."
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date."
                        ValidationGroup="insert" />
                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                    <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                    <asp:Button ID="btnSearch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                        OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                        OnClick="btnCancel_Click" CausesValidation="False" />
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
                     <li class="frmtxt1">
                        Export to:
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
            Report
        </div>
        <%-- <td width="10%" align="right">
                                                        <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
                                                            OnClick="btnExprtToExcel_Click"   />
                                                    </td>--%>
        <div class="contentbox">
            <div class="PivotGrid">
                <dxwpg:ASPxPivotGrid Width="100%" ID="grdBTM" runat="server" EnableRowsCache="false" OptionsView-ShowHorizontalScrollBar="false">
                    <Fields>
                        <dxwpg:PivotGridField ID="PivotGridField1" Area="FilterArea" AreaIndex="0" Caption="<%$ Resources:SalesHierarchy, HierarchyName1 %>"
                            FieldName="HOName" EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField8" Area="FilterArea" AreaIndex="1" Caption="<%$ Resources:SalesHierarchy, HierarchyName2 %>"
                            FieldName="RBHName" EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField4" Area="FilterArea" AreaIndex="2" Caption="<%$ Resources:SalesHierarchy, HierarchyName3 %>"
                            FieldName="ZSMName" EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField5" Area="FilterArea" AreaIndex="3" Caption="<%$ Resources:SalesHierarchy, HierarchyName4 %>"
                            FieldName="SBHName" EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField7" Area="FilterArea" AreaIndex="4" Caption="<%$ Resources:SalesHierarchy, HierarchyName5 %>"
                            FieldName="ASOName" EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField12" Area="FilterArea" AreaIndex="5" Caption="Country"
                            FieldName="CountryName" EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField13" Area="FilterArea" AreaIndex="6" Caption="State"
                            FieldName="StateName" EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField2" Area="RowArea" AreaIndex="5" Caption="From"
                            FieldName="FromSalesChannel" EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField3" Area="RowArea" AreaIndex="6" Caption="To"
                            FieldName="ToSalesChannel" EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField6" Area="ColumnArea" AreaIndex="0" FieldName="SKUName"
                            Caption="SKU Name " EmptyValueText="N/A">
                        </dxwpg:PivotGridField>
                        <%-- <dxwpg:PivotGridField ID="PivotGridFilter1" Area="FilterArea" AreaIndex="3" Caption="Quantity"
                                                                            FieldName="ReportQuantity">
                                                                        </dxwpg:PivotGridField>--%>
                        <dxwpg:PivotGridField ID="PivotGridField10" Area="FilterArea" AreaIndex="7" Caption="Category"
                            FieldName="ProductCategoryName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField9" Area="FilterArea" AreaIndex="8" Caption="Product"
                            FieldName="ProductName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridFilter2" Area="FilterArea" AreaIndex="9" Caption="Brand"
                            FieldName="BrandName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridFilter3" Area="FilterArea" AreaIndex="10" Caption="Model"
                            FieldName="ModelName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridFilter4" Area="FilterArea" AreaIndex="11" Caption="Color"
                            FieldName="ColorName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField11" Area="FilterArea" AreaIndex="12" Caption="SKU Code"
                            FieldName="SKUCode">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridData1" Area="DataArea" CellFormat-FormatType="Numeric"
                            AreaIndex="0" Caption="Quantity" FieldName="ReportQuantity">
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
                <dxpgex:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server" ASPxPivotGridID="grdBTM"
                    Visible="False" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
