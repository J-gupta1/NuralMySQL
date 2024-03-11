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
    <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
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
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="mainheading_rpt">
                                                        <div class="mainheading_rpt_left">
                                                        </div>
                                                        <div class="mainheading_rpt_mid">
                                                            Search
                                                        </div>
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
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top"></td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" class="tableposition">
                                                    <div class="contentbox">
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="2" class="mandatory" valign="top" height="35">(<font class="error">*</font>) marked fields are mandatory.
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" valign="top" align="left">
                                                                    <div runat="server" id="dvSalesChannel" style="display: none">
                                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                            <tr>
                                                                                <td width="15%" align="right" valign="top" height="35">Sales Channel Type:<font class="error">*</font>
                                                                                </td>
                                                                                <td width="20%" align="left" valign="top">
                                                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form_select4" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                                                                        AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                    <div style="width: 180px;">
                                                                                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                                                                                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                                                                            SetFocusOnError="true" ValidationGroup="SalesReport"></asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                </td>
                                                                                <td width="15%" align="right" valign="top">Report for:<font class="error">&nbsp;</font>
                                                                                </td>
                                                                                <td align="left" valign="top">
                                                                                    <asp:TextBox ID="txtReportFor" runat="server" MaxLength="30" 
                                                                                        CssClass="form_input9" Width="150px"></asp:TextBox>
                                                                                    <%--<cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                                                                                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                                                                                        MinimumPrefixLength="3" ServiceMethod="GetSalesChannelCodeList" ServicePath="../../CommonService.asmx"
                                                                                        TargetControlID="txtReportFor" UseContextKey="true">
                                                                                    </cc1:AutoCompleteExtender>--%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                        <tr>
                                                                            <%-- <td valign="top" class="formtext" align="right" width="10%">
                                                                                Sales Type:<font class="error">*</font>
                                                                            </td>
                                                                            <td align="left" valign="top" width="20%">
                                                                              <asp:DropDownList ID="ddlSalesType" runat="server" CssClass="form_select4">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                              <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlSalesType"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales type."
                                                                                    SetFocusOnError="true" ValidationGroup="SalesReport"></asp:RequiredFieldValidator>
                                                                            </td>--%>
                                                                            <td width="15%" valign="top" align="right" class="formtext">Date From:<font class="error">*</font>
                                                                            </td>
                                                                            <td width="20%" align="left" valign="top">
                                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                                                                                    ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                                <br />
                                                                            </td>
                                                                            <td width="15%" valign="top" align="right" class="formtext">Date To:<font class="error">*</font>
                                                                            </td>
                                                                            <td width="25%" valign="top" align="left">
                                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="SalesReport"
                                                                                    defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                            </td>
                                                                            <td valign="top" align="left">
                                                                                <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                                                                                <asp:TextBox ID="hdnReportSalesChannelCode" runat="server" Value="0" Style="display: none" />
                                                                                <asp:Button ID="btnSearch" Text="Show" runat="server" ValidationGroup="SalesReport"
                                                                                    ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                                                    OnClick="btnCancel_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                        <td height="10"></td>
                    </tr>
                    <asp:Panel ID="pnlSearch" runat="server" Visible="false">
                        <tr>
                            <td align="left" class="tableposition">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        Export
                                    </div>
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
                                                <table border="0" cellpadding="3" cellspacing="0" style="float: left">
                                                    <tr>
                                                        <td valign="top" width="10%" class="formtext">
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
                                                            <div style="float: left; width: 135px;">
                                                                <asp:DropDownList ID="ddlExportFormat" runat="server" Style="vertical-align: middle"
                                                                    ValueType="System.String" CssClass="form_select4">
                                                                    <asp:ListItem Text="Excel" Value="1" />
                                                                    <asp:ListItem Text="Excel (.xlsx)" Value="2" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td style="padding-top: 10px;">
                                                            <asp:Button ID="buttonSaveAs" runat="server" ToolTip="Export and save" Style="vertical-align: middle;"
                                                                OnClick="buttonSaveAs_Click" Text="Save" CssClass="buttonbg" />
                                                            <asp:Button ID="buttonOpen" Visible="false" runat="server" ToolTip="Export and open"
                                                                Style="vertical-align: middle" OnClick="buttonOpen_Click" Text="Open" CssClass="buttonbg" />
                                                        </td>
                                                        <td style="padding-top: 10px;"></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px"></td>
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
                                                                OPSI Infomation
                                                            </div>
                                                            <div class="mainheading_rpt_right">
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td width="10%">
                                                        <%--      <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExprtToExcel_Click"
                                                                        CausesValidation="False" />--%>
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
                                                        <OptionsView ShowHorizontalScrollBar="false" ShowColumnGrandTotals="False" ShowColumnTotals="False" />
                                                        <Fields>
                                                            <dxwpg:PivotGridField ID="PivotGridField1" Area="FilterArea" AreaIndex="0" EmptyValueText="N/A"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName1 %>" FieldName="HL1Name">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField2" Area="FilterArea" AreaIndex="1" EmptyValueText="N/A"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName2 %>" FieldName="HL2Name">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField3" Area="FilterArea" AreaIndex="2" EmptyValueText="N/A"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName3 %>" FieldName="HL3Name">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField4" Area="FilterArea" AreaIndex="3" EmptyValueText="N/A"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName4 %>" FieldName="HL4Name">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField5" Area="FilterArea" AreaIndex="4" EmptyValueText="N/A"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName5 %>" FieldName="HL5Name">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField17" Area="FilterArea" AreaIndex="5" Caption="Country"
                                                                FieldName="CountryName">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField18" Area="FilterArea" AreaIndex="6" Caption="State"
                                                                FieldName="StateName">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField6" Area="RowArea" AreaIndex="5" Caption="Sales Channel Type"
                                                                FieldName="SalesChannelTypeName">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField7" Area="RowArea" AreaIndex="6" Caption="Sales Channel Name"
                                                                FieldName="SalesChannelName">
                                                            </dxwpg:PivotGridField>
                                                             <%--#CC02 START ADDED--%>
                                                             <dxwpg:PivotGridField ID="PivotDistributorCode" Area="RowArea" AreaIndex="6" Caption="Sales Channel Code"
                                                                FieldName="SalesChannelCode">
                                                            </dxwpg:PivotGridField>
                                                             <%--#CC02 START End--%>
                                                            <%--#CC01 START ADDED--%>
                                                            <dxwpg:PivotGridField ID="PivotGridField70" Area="RowArea" AreaIndex="8" FieldName="StockBinTypeDesc"
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
                                                            <dxwpg:PivotGridField ID="PivotGridField11" Area="FilterArea" AreaIndex="7" Caption="Category"
                                                                FieldName="ProductCategoryName">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField16" Area="FilterArea" AreaIndex="8" Caption="Product"
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
                                                            <dxwpg:PivotGridField ID="PivotGridField9" Area="FilterArea" AreaIndex="12" FieldName="SKUCode"
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
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td height="10"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
