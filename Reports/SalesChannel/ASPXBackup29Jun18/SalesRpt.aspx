<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesRpt.aspx.cs" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    Inherits="Reports_SalesChannel_SalesRpt" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0">
        <tr>
            <td align="left" valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="mainheading_rpt">
                                                        <div class="mainheading_rpt_left">
                                                        </div>
                                                        <div class="mainheading_rpt_mid">
                                                            Search</div>
                                                        <div class="mainheading_rpt_right">
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <div class="contentbox">
                                            <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                <tr>
                                                    <td colspan="6" class="error" valign="top">
                                                        (*) marked fields are mandatory.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="right" width="12%" height="35">
                                                        Sales Type:<font class="error">*</font>
                                                    </td>
                                                    <td align="left" valign="top" width="20%">
                                                        <div>
                                                            <asp:DropDownList ID="ddlSalesType" runat="server" CssClass="form_select4">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div style="width: 180px;">
                                                            <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlSalesType"
                                                                CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales type."
                                                                SetFocusOnError="true" ValidationGroup="SalesReport"></asp:RequiredFieldValidator></div>
                                                    </td>
                                                    <td valign="top" align="right" width="10%">
                                                        Date From:<font class="error">*</font>
                                                    </td>
                                                    <td align="left" valign="top" width="20%">
                                                        <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                                                            ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                    </td>
                                                    <td valign="top" align="right" width="17%">
                                                        Date To:<font class="error">*</font>
                                                    </td>
                                                    <td valign="top" align="left">
                                                        <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="SalesReport"
                                                            defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="right">
                                                        <asp:Label ID="lbllocation" runat="server" Text="">Region:  <font class="error">&nbsp;</font></asp:Label>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:DropDownList ID="ddllocation" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="top" align="right" height="35">
                                                        <asp:Label ID="lblState" runat="server" Text="">State:  <font class="error">&nbsp;</font></asp:Label>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="top" align="right">
                                                        <asp:Label ID="lblProductCategory" runat="server" Text="">Product Category: <span class="error">&nbsp;</span></asp:Label>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="form_select4"
                                                            AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="right" height="35">
                                                        <asp:Label ID="lblsku" runat="server" Text="">Model Name: <font class="error">&nbsp;</font></asp:Label>
                                                    </td>
                                                    <td width="20%" align="left" valign="top">
                                                        <asp:DropDownList ID="ddlModelName" CssClass="form_select4" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlModelName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="top" align="right">
                                                        <asp:Label ID="lblSkuName" runat="server" Text="">Sku Name:  <font class="error">&nbsp;</font></asp:Label>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:DropDownList ID="ddlSku" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="top" align="right">
                                                        <div class="float-left">
                                                            <asp:CheckBox ID="chkZeroQuantity" runat="server" />
                                                        </div>
                                                        <div class="float-left" style="margin-top: 3px;">
                                                            <asp:Label ID="lblZeroQuantity" runat="server" Text="">Show Zero Qty Records</asp:Label>
                                                        </div>
                                                    </td>
                                                    <td valign="top" align="left">
                                                        <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                                                        <asp:Button ID="btnSearch" Text="Show" runat="server" ValidationGroup="SalesReport"
                                                            ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                            OnClick="btnCancel_Click" />
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
                        <td height="10">
                            &nbsp;
                        </td>
                    </tr>
                    <asp:Panel ID="pnlSearch" runat="server" Visible="false">
                        <tr>
                            <td align="left" class="tableposition">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        Export</div>
                                    <div class="mainheading_rpt_right">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td valign="top" align="left" class="tableposition">
                                            <div class="contentbox">
                                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td valign="top" width="10%">
                                                            <strong>Print options: </strong>
                                                        </td>
                                                        <td width="18%">
                                                            <asp:CheckBox ID="checkPrintHeadersOnEveryPage" runat="server" Text="Header on every page" />
                                                        </td>
                                                        <td width="18%">
                                                            <asp:CheckBox ID="checkPrintFilterHeaders" runat="server" Text="Filter header" Checked="True" />
                                                        </td>
                                                        <td width="18%">
                                                            <asp:CheckBox ID="checkPrintColumnHeaders" runat="server" Text="Column header" Checked="True" />
                                                        </td>
                                                        <td width="18%">
                                                            <asp:CheckBox ID="checkPrintRowHeaders" runat="server" Text="Row header" Checked="True" />
                                                        </td>
                                                        <td width="18%">
                                                            <asp:CheckBox ID="checkPrintDataHeaders" runat="server" Text="Data header" Checked="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-top: 10px;" class="formtext">
                                                            <strong>Export to:</strong>
                                                        </td>
                                                        <td style="padding-top: 10px;">
                                                            <asp:DropDownList ID="ddlExportFormat" runat="server" Style="vertical-align: middle"
                                                                ValueType="System.String" CssClass="form_select4">
                                                                <asp:ListItem Text="Excel" Value="1" />
                                                                <asp:ListItem Text="Excel (.xlsx)" Value="2" />
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="padding-top: 10px;">
                                                            <asp:Button ID="buttonSaveAs" runat="server" ToolTip="Export and save" Style="vertical-align: middle;"
                                                                OnClick="buttonSaveAs_Click" Text="Save" CssClass="buttonbg" />
                                                            <asp:Button ID="buttonOpen" Visible="false" runat="server" ToolTip="Export and open"
                                                                Style="vertical-align: middle" OnClick="buttonOpen_Click" Text="Open" CssClass="buttonbg" />
                                                        </td>
                                                        <td style="padding-top: 10px;">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="90%">
                                                        <div class="mainheading_rpt">
                                                            <div class="mainheading_rpt_left">
                                                            </div>
                                                            <div class="mainheading_rpt_mid">
                                                                Sales Infomation</div>
                                                            <div class="mainheading_rpt_right">
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td width="10%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="tableposition2" style="float: left;">
                                             <div class="contentbox">
                                                <div class="grid1">
                                                    <dxwpg:ASPxPivotGrid Width="958" ID="ASPxPvtGrd" runat="server" EnableRowsCache="false">
                                                        <OptionsView ShowHorizontalScrollBar="false" />
                                                        <Fields>
                                                            <dxwpg:PivotGridField ID="PivotGridField1" Area="FilterArea" AreaIndex="0" EmptyValueText="N/A"
                                                                FieldName="HL1Name" Caption="<%$ Resources:SalesHierarchy, HierarchyName1 %>">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField2" Area="FilterArea" AreaIndex="1" EmptyValueText="N/A"
                                                                FieldName="HL2Name" Caption="<%$ Resources:SalesHierarchy, HierarchyName2 %>">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField3" Area="FilterArea" AreaIndex="2" EmptyValueText="N/A"
                                                                FieldName="HL3Name" Caption="<%$ Resources:SalesHierarchy, HierarchyName3 %>">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField4" Area="FilterArea" AreaIndex="3" EmptyValueText="N/A"
                                                                FieldName="HL4Name" Caption="<%$ Resources:SalesHierarchy, HierarchyName4 %>">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField5" Area="FilterArea" AreaIndex="4" EmptyValueText="N/A"
                                                                FieldName="HL5Name" Caption="<%$ Resources:SalesHierarchy, HierarchyName5 %>">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField18" Area="FilterArea" AreaIndex="5" EmptyValueText="N/A"
                                                                FieldName="Country" Caption="Country">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField19" Area="FilterArea" AreaIndex="6" EmptyValueText="N/A"
                                                                FieldName="State" Caption="State">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField6" Area="RowArea" AreaIndex="0" Caption="Sales From Name"
                                                                FieldName="SalesFromName">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField7" Area="RowArea" AreaIndex="1" Caption="Sales To Name"
                                                                FieldName="SalesToName">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField8" Area="RowArea" AreaIndex="2" Caption="Type"
                                                                FieldName="TYPE">
                                                            </dxwpg:PivotGridField>
                                                            <%--#CC01 START ADDED--%>
                                                            <dxwpg:PivotGridField ID="PivotGridFieldStockBin" Area="RowArea" AreaIndex="2" Caption="Stock Type"
                                                                FieldName="StockBin">
                                                            </dxwpg:PivotGridField>
                                                            <%--#CC01 END ADDED--%>
                                                            <dxwpg:PivotGridField ID="PvtRetailerType" Area="RowArea" Visible="False" AreaIndex="3"
                                                                Caption="Retailer Type" FieldName="RetailerTypeName">
                                                            </dxwpg:PivotGridField>
                                                            <%--#CC02 START ADDED--%>
                                                            <dxwpg:PivotGridField ID="PvtFromcode" Area="RowArea"  AreaIndex="2"
                                                                Caption="Sales From Code" FieldName="SalesFromCode">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PvtTocode" Area="RowArea"  AreaIndex="2"
                                                                Caption="Sales To Code" FieldName="SalesToCode">
                                                            </dxwpg:PivotGridField>
                                                             <%--#CC02 END ADDED--%>
                                                            <dxwpg:PivotGridField ID="PivotGridField9" Area="ColumnArea" AreaIndex="0" FieldName="SKUName"
                                                                Caption="SKU Name">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField11" Area="FilterArea" AreaIndex="7" Caption="Category"
                                                                FieldName="ProductCategoryName">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField17" Area="FilterArea" AreaIndex="8" Caption="Product"
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
                                                            <dxwpg:PivotGridField ID="PivotGridField16" Area="DataArea" CellFormat-FormatType="Numeric"
                                                                AreaIndex="1" Caption="Amount" FieldName="Amount">
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
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
