<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    AutoEventWireup="true" CodeFile="GRNReport.aspx.cs" Inherits="Reports_SalesChannel_GRNReport" %>

<%@ Register Src="~/UserControls/Footer.ascx" TagName="ucFooter" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Header.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                                                            GRN Report Search</div>
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
                                                <td valign="top" align="left" class="tableposition">
                                                    <div class="contentbox">
                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                            <!-- <tr>
                                                                            <td colspan="7" height="20" class="mandatory" valign="top">
                                                                                (<font class="error">*</font>) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>-->
                                                            <tr>
                                                                <td valign="top" width="10%" align="right" class="formtext" style="padding-top: 7px;">
                                                                    Date From:
                                                                </td>
                                                                <td align="left" valign="top" width="13%">
                                                                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Please enter a from date."
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                                                </td>
                                                                <td valign="top" align="right" class="formtext" width="10%" style="padding-top: 7px;">
                                                                    Date To:
                                                                </td>
                                                                <td valign="top" align="left" width="20%">
                                                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Please enter a to date."
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                                                </td>
                                                                <td valign="top" align="left" width="18%">
                                                                    <asp:CheckBox ID="ChkSB" runat="server" Text="With Serial/Batch" TextAlign="Right" />
                                                                </td>
                                                                <td valign="top" align="left">
                                                                    <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                                                                    <asp:Button ID="btnSearch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                                                        OnClick="btnSearch_Click" ValidationGroup="insert" CausesValidation="False" />
                                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                                        OnClick="btnCancel_Click" CausesValidation="False" />
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
                        <td height="10">
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
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading_rpt">
                                                <div class="mainheading_rpt_left">
                                                </div>
                                                <div class="mainheading_rpt_mid">
                                                    Report</div>
                                                <div class="mainheading_rpt_right">
                                                </div>
                                            </div>
                                        </td>
                                        <%--<td width="10%" align="right">
                                                        <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
                                                            OnClick="btnExprtToExcel_Click"  />
                                                    </td>--%>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2" class="tableposition2">
                                            <div class="contentbox">
                                                <div class="grid2" style="padding-bottom: 15px; width: 955px;">
                                                    <dxwpg:ASPxPivotGrid Width="100%" ID="grdGRN" runat="server" EnableRowsCache="false">
                                                        <fields>
                                                            <dxwpg:PivotGridField ID="PivotGridField1" Area="FilterArea" AreaIndex="0" FieldName="HOName"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName1 %>" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField9" Area="FilterArea" AreaIndex="1" FieldName="RBHName"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName2 %>" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField2" Area="FilterArea" AreaIndex="2" FieldName="ZSMName"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName3 %>" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField3" Area="FilterArea" AreaIndex="3" FieldName="SBHName"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName4 %>" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField5" Area="FilterArea" AreaIndex="4" FieldName="ASOName"
                                                                Caption="<%$ Resources:SalesHierarchy, HierarchyName5 %>" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                             <dxwpg:PivotGridField ID="PivotGridField11" Area="FilterArea" AreaIndex="5" FieldName="CountryName"
                                                                Caption="Country" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                             <dxwpg:PivotGridField ID="PivotGridField12" Area="FilterArea" AreaIndex="6" FieldName="StateName"
                                                                Caption="State" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField4" Area="RowArea" AreaIndex="5" Caption="SalesChannel Code"
                                                                FieldName="SalesChannelCode" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>

                                                            <%--#CC02 START ADDED--%>
                                                             <dxwpg:PivotGridField ID="PivotGridField17" Area="RowArea" AreaIndex="8" FieldName="StockBinTypeDesc"
                                                                Caption="Stock Bin Type">
                                                            </dxwpg:PivotGridField>
                                                              <%--#CC02 END ADDED--%>
                                                                 <dxwpg:PivotGridField ID="PivotGridField13" Area="RowArea" AreaIndex="4" Caption="SalesChannel Name"
                                                                FieldName="SalesChannelName" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField6" Area="ColumnArea" AreaIndex="0" FieldName="SKUName"
                                                                Caption="SKU" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField8" Area="FilterArea" AreaIndex="7" Caption="Category"
                                                                FieldName="ProductCategory" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField7" Area="FilterArea" AreaIndex="8" Caption="Product"
                                                                FieldName="Product" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridFilter2" Area="FilterArea" AreaIndex="9" Caption="Brand"
                                                                FieldName="Brand" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridFilter3" Area="FilterArea" AreaIndex="10" Caption="Model"
                                                                FieldName="Model" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridFilter4" Area="FilterArea" AreaIndex="11" Caption="Color"
                                                                FieldName="Color" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField10" Area="FilterArea" AreaIndex="12" Caption="SKU Code"
                                                                FieldName="SKUCode" EmptyValueText="N/A">
                                                            </dxwpg:PivotGridField>
                                                            <%-- <dxwpg:PivotGridField ID="PivotGridField8" Area="FilterArea" AreaIndex="4" Caption="ProductCategory"
                                                                            FieldName="ProductCategory">
                                                                        </dxwpg:PivotGridField>--%>
                                                            <dxwpg:PivotGridField ID="PivotGridData1" Area="DataArea" CellFormat-FormatType="Numeric"
                                                                AreaIndex="0" Caption="Quantity" FieldName="Quantity"> <%--#CC01 fieldName changed from ReportQuantity to Quantity--%>
                                                            </dxwpg:PivotGridField>
                                                        </fields>
                                                        <styles cssfilepath="~/App_Themes/Aqua/{0}/styles.css" csspostfix="Aqua">
                                                            <MenuStyle GutterWidth="0px" />
                                                        </styles>
                                                        <images spritecssfilepath="~/App_Themes/Aqua/{0}/sprite.css">
                                                            <CustomizationFieldsBackground Url="~/App_Themes/Aqua/PivotGrid/pcHeaderBack.gif">
                                                            </CustomizationFieldsBackground>
                                                            <LoadingPanel Url="~/App_Themes/Aqua/PivotGrid/Loading.gif">
                                                            </LoadingPanel>
                                                        </images>
                                                        <optionsloadingpanel>
                                                            <Image Url="~/App_Themes/Aqua/PivotGrid/Loading.gif">
                                                            </Image>
                                                        </optionsloadingpanel>
                                                        <imageseditors>
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
                                                        </imageseditors>
                                                    </dxwpg:ASPxPivotGrid>
                                                    <dxpgex:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server" ASPxPivotGridID="grdGRN"
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
