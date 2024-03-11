<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PriceProtectionReport.aspx.cs" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    Inherits="Reports_Common_PriceProtectionReport" %>

<%@ Register Src="~/UserControls/Footer.ascx" TagName="ucFooter" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Header.ascx" TagName="ucHeader" TagPrefix="uc1" %>
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
        Price Protection Report Search
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <%-- <td valign="top" class="formtext" align="right" width="10%">
                                                                                Sales Type:<span class="error">*</span>
                                                                            </td>
                                                                            <td align="left" valign="top" width="20%">
                                                                              <asp:DropDownList ID="ddlSalesType" runat="server" CssClass="formselect">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                              <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlSalesType"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales type."
                                                                                    SetFocusOnError="true" ValidationGroup="SalesReport"></asp:RequiredFieldValidator>
                                                                            </td>--%>
                <li class="text">Date From:<span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlDate" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="req1" CssClass="error" runat="server" InitialValue="0"
                            ControlToValidate="ddlDate" ErrorMessage="Please select price dropped date."
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <%--  <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                                                                                    ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                                <br />--%>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                        <asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="SalesReport"
                            ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CausesValidation="false"
                            CssClass="buttonbg" OnClick="btnCancel_Click" />
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
                    <li class="frmtxt1">Print options:
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
                    <li class="frmtxt1">Export to:
                    </li>
                    <li>
                        <asp:DropDownList ID="ddlExportFormat" runat="server" Width="40px"
                            ValueType="System.String" CssClass="formselect">
                            <asp:ListItem Text="Excel" Value="1" />
                            <asp:ListItem Text="Excel (.xlsx)" Value="2" />
                        </asp:DropDownList>
                    </li>
                    <li>
                        <div class="float-margin">
                            <asp:Button ID="buttonSaveAs" runat="server" ToolTip="Export and save" Style="vertical-align: middle;"
                                OnClick="buttonSaveAs_Click" Text="Save" CssClass="buttonbg" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="buttonOpen" Visible="false" runat="server" ToolTip="Export and open"
                                Style="vertical-align: middle" OnClick="buttonOpen_Click" Text="Open" CssClass="buttonbg" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="mainheading">
            Price Protection Infomation
        </div>
        <%--      <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExprtToExcel_Click"
                                                                        CausesValidation="False" />--%>
        <div class="contentbox">
            <div class="PivotGrid">
                <dxwpg:ASPxPivotGrid Width="100%" ID="ASPxPvtGrd" runat="server" EnableViewState="false" EnableRowsCache="false">
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
                        <dxwpg:PivotGridField ID="PivotGridField14" Area="FilterArea" AreaIndex="4" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName5 %>" FieldName="ASO">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField16" Area="RowArea" AreaIndex="5" EmptyValueText="N/A"
                            Caption="State" FieldName="State">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField5" Area="RowArea" AreaIndex="6" EmptyValueText="N/A"
                            Caption="Sales Channel" FieldName="SalesChannel">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField8" Area="ColumnArea" AreaIndex="0" FieldName="SKUName"
                            Caption="SKU Name">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField18" Area="FilterArea" AreaIndex="5" Caption="Category"
                            FieldName="ProductCategoryName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField9" Area="FilterArea" AreaIndex="6" Caption="Product"
                            FieldName="ProductName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField10" Area="FilterArea" AreaIndex="7" Caption="Brand"
                            FieldName="BrandName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField11" Area="FilterArea" AreaIndex="8" Caption="Model"
                            FieldName="ModelName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField12" Area="FilterArea" AreaIndex="9" Caption="Color"
                            FieldName="ColorName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField17" Area="FilterArea" AreaIndex="10" Caption="SKU Code"
                            FieldName="SKUCode">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField13" Area="DataArea" CellFormat-FormatType="Numeric"
                            AreaIndex="1" Caption="Quantity" FieldName="Quantity">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField7" Area="DataArea" CellFormat-FormatType="Numeric"
                            AreaIndex="2" Caption="Coverage value" FieldName="Coveragevalue">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField6" Area="DataArea" CellFormat-FormatType="Numeric"
                            AreaIndex="3" Caption="Old Price" FieldName="OldPrice">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField15" Area="DataArea" CellFormat-FormatType="Numeric"
                            AreaIndex="4" Caption="New Price" FieldName="NewPrice">
                        </dxwpg:PivotGridField>
                    </Fields>
                    <OptionsView DataHeadersDisplayMode="Popup" ShowDataHeaders="False" ShowHorizontalScrollBar="false" />
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
