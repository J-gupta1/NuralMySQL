<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OPSIRpt.aspx.cs" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    Inherits="Reports_SalesChannel_OPSIRpt" %>

<%---------------------#CC02 ,15-11-2017,Vijay Kumar Prajapati Add Distributor code.-----------------------------------------------------%>
<%@ Register Src="~/UserControls/Footer.ascx" TagName="ucFooter" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Header.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function txtReportForSalesChannelCodeTextChanged() {
            var v = ValidateAdjustForType();
            if (v == false) {
                return;
            }
            var strUser = "0";
            var v = "";
            var hdnSaleschannelCode = document.getElementById('<%= hdnReportSalesChannelCode.ClientID %>');
            var ChannelsTypeid = document.getElementById('<%= ddlType.ClientID %>');
            var SalesChannelTypeid = ChannelsTypeid.options[ChannelsTypeid.selectedIndex].value;

            var ReportForCode = document.getElementById('<%= txtReportFor.ClientID %>').value;
            if (ReportForCode != '') {
                if (ReportForCode.indexOf('-') <= 0) {
                    alert("Please Enter valid SalesChannel Code.");
                    return;
                }
            }

            if (ReportForCode.indexOf('-') > 0) {
                var SplittedRecords = ReportForCode.split("-");
                hdnSaleschannelCode.value = SplittedRecords[1];
            }

            Typeid = SalesChannelTypeid;
            if (ReportForCode == '') {
                hdnSaleschannelCode.value = '0';
            }

        }

        function ValidateAdjustForType() {

            var e = document.getElementById('<%= ddlType.ClientID %>');
            var strUser = e.value;
            if (strUser == "0") {
                alert('Please select Report for Type.');
                document.getElementById('<%= txtReportFor.ClientID %>').value = '';
                return false
            }
            else
                return true;
        }

    </script>

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
            <ul runat="server" id="dvSalesChannel" style="display: none">
                <li class="text">Sales Channel Type:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                        SetFocusOnError="true" ValidationGroup="SalesReport"></asp:RequiredFieldValidator>

                </li>
                <li class="text">Report for:<span class="error">&nbsp;</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtReportFor" runat="server" MaxLength="30"
                        CssClass="formfields"></asp:TextBox>
                    <%--<cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                                                                                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                                                                                        MinimumPrefixLength="3" ServiceMethod="GetSalesChannelCodeList" ServicePath="../../CommonService.asmx"
                                                                                        TargetControlID="txtReportFor" UseContextKey="true">
                                                                                    </cc1:AutoCompleteExtender>--%>
                </li>
            </ul>
            <div class="clear"></div>
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
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                        ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />

                </li>
                <li class="text">Date To:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="SalesReport"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                        <asp:TextBox ID="hdnReportSalesChannelCode" runat="server" Value="0" Style="display: none" />
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
            OPSI Infomation
        </div>
        <%--      <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExprtToExcel_Click"
                                                                        CausesValidation="False" />--%>


        <div class="contentbox">
            <div class="PivotGrid">
                <dxwpg:ASPxPivotGrid Width="100%" ID="ASPxPvtGrd" runat="server" EnableRowsCache="false">
                    <OptionsView ShowHorizontalScrollBar="false" ShowColumnGrandTotals="False" ShowColumnTotals="False" />
                    <Fields>
                        <dxwpg:PivotGridField ID="PivotGridField1" Area="FilterArea" AreaIndex="0" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName1 %>" FieldName="HO">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField2" Area="FilterArea" AreaIndex="1" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName2 %>" FieldName="Business Head">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField3" Area="FilterArea" AreaIndex="2" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName3 %>" FieldName="Zonal Head">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField4" Area="FilterArea" AreaIndex="3" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName4 %>" FieldName="RSM">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField5" Area="FilterArea" AreaIndex="4" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName5 %>" FieldName="Branch Head">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField25" Area="FilterArea" AreaIndex="5" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName6 %>" FieldName="ASM">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField26" Area="FilterArea" AreaIndex="6" EmptyValueText="N/A"
                            Caption="<%$ Resources:SalesHierarchy, HierarchyName7 %>" FieldName="TSM">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField17" Area="FilterArea" AreaIndex="7" Caption="Country"
                            FieldName="CountryName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField18" Area="FilterArea" AreaIndex="8" Caption="State"
                            FieldName="StateName">
                        </dxwpg:PivotGridField>
                        <%--   ===================#CC03 Added Started========================================--%>
                        <dxwpg:PivotGridField ID="PivotGridFieldRegion" Area="FilterArea" AreaIndex="9" Caption="Region"
                            FieldName="RegionName">
                        </dxwpg:PivotGridField>

                        <dxwpg:PivotGridField ID="PivotGridFieldZone" Area="FilterArea" AreaIndex="10" Caption="Zone"
                            FieldName="ZoneName">
                        </dxwpg:PivotGridField>

                        <%--  =======================#CC03 Added End======================================--%>
                        <dxwpg:PivotGridField ID="PivotGridField6" Area="RowArea" AreaIndex="7" Caption="Sales Channel Type"
                            FieldName="SalesChannelTypeName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField7" Area="RowArea" AreaIndex="8" Caption="Sales Channel Name"
                            FieldName="SalesChannelName">
                        </dxwpg:PivotGridField>
                        <%--#CC02 START ADDED--%>
                        <dxwpg:PivotGridField ID="PivotDistributorCode" Area="RowArea" AreaIndex="8" Caption="Sales Channel Code"
                            FieldName="SalesChannelCode">
                        </dxwpg:PivotGridField>
                        <%--#CC02 START End--%>
                        <%--#CC01 START ADDED--%>
                        <dxwpg:PivotGridField ID="PivotGridField70" Area="RowArea" AreaIndex="9" FieldName="StockBinTypeDesc"
                            Caption="Stock Bin Type">
                        </dxwpg:PivotGridField>
                        <%--#CC01 START ADDED--%>
                        <dxwpg:PivotGridField ID="PivotGridField8" Area="ColumnArea" AreaIndex="0" FieldName="SKUName"
                            Caption="SKU Name">
                        </dxwpg:PivotGridField>
                        <%--  <dxwpg:PivotGridField ID="PivotGridField10" Area="ColumnArea" AreaIndex="2" Caption="Type"
                                                                FieldName="TransType" Options-AllowDrag="False">
                                                            </dxwpg:PivotGridField>--%>
                        <dxwpg:PivotGridField ID="PivotGridField10" Area="ColumnArea" AreaIndex="2" Caption="Type"
                            FieldName="TransType">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField11" Area="FilterArea" AreaIndex="10" Caption="Category"
                            FieldName="ProductCategoryName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField16" Area="FilterArea" AreaIndex="11" Caption="Product"
                            FieldName="ProductName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField12" Area="FilterArea" AreaIndex="12" Caption="Brand"
                            FieldName="BrandName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField13" Area="FilterArea" AreaIndex="13" Caption="Model"
                            FieldName="ModelName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField14" Area="FilterArea" AreaIndex="14" Caption="Color"
                            FieldName="ColorName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField9" Area="FilterArea" AreaIndex="15" FieldName="SKUCode"
                            Caption="SKU Code">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField15" Area="DataArea" CellFormat-FormatType="Numeric"
                            AreaIndex="0" Caption="Quantity" FieldName="Quantity" Options-ShowGrandTotal="False">
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
