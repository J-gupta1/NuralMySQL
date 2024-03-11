﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModelRegionMonthWiseReport.aspx.cs"
    MasterPageFile="~/CommonMasterPages/ReportPage.master" Inherits="Reports_SalesChannel_ModelRegionMonthWiseReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>
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
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" class="tableposition">
                                                    <div class="contentbox">
                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="7" class="error" valign="top">
                                                                    (<font class="error">*</font>) marked fields are mandatory.
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right" class="formtext" width="10%">
                                                                    Date From:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="13%">
                                                                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                                                                        ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                    <br />
                                                                </td>
                                                                <td valign="top" align="right" class="formtext" width="10%">
                                                                    Date To:<font class="error">*</font>
                                                                </td>
                                                                <td valign="top" align="left" width="15%">
                                                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="SalesReport"
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                </td>
                                                                <td valign="top" align="left" width="22%">
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
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                            &nbsp;
                        </td>
                    </tr>
                    <asp:Panel ID="pnlSearch" runat="server" Visible="true">
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
                                                    <asp:Button ID="exportToExel" Text="" runat="server" OnClick="exportToExel_Click"
                                            CssClass="excel" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="tableposition2" style="float: left;">
                                            <div class="contentbox">
                                                <div class="grid1">
                                                   <dxwpg:ASPxPivotGrid Width="958"  ID="ASPxPvtGrd" runat="server"  EnableRowsCache="false"  
                                                    OptionsView-ColumnTotalsLocation="Far" OptionsView-ShowColumnGrandTotalHeader="True" 
                                                    OptionsView-ShowColumnGrandTotals="True" OptionsView-ShowColumnTotals="False" OptionsView-ShowRowTotals="False" OptionsView-ShowRowGrandTotals="True"
                                                    OptionsView-ShowGrandTotalsForSingleValues="True" 
                                                   >
                                                        <OptionsView ShowHorizontalScrollBar="false" />
                                                        <Fields>
                                                      
                                                            
                                                          
                                                             <dxwpg:PivotGridField ID="PvtRetailerType" Area="RowArea" AreaIndex="0"
                                                                Caption="SalesChannel Name" FieldName="SalesChannelName">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField6" Area="RowArea" AreaIndex="1" Caption="Model"
                                                                FieldName="ModelCode">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField7" Area="RowArea" AreaIndex="2" Caption="Region"
                                                                FieldName="Region">
                                                            </dxwpg:PivotGridField>
                                                            <dxwpg:PivotGridField ID="PivotGridField8" Area="ColumnArea" AreaIndex="0" Caption="Month"
                                                                FieldName="MonthName">
                                                            </dxwpg:PivotGridField>
                                                           <dxwpg:PivotGridField ID="PivotGridField1" Area="ColumnArea" AreaIndex="1" Caption="Year"
                                                                FieldName="Year">
                                                            </dxwpg:PivotGridField>
                                                         
                                                               <dxwpg:PivotGridField ID="PivotGridField15" Area="DataArea" CellFormat-FormatType="Numeric"
                                                                AreaIndex="0" Caption="Quantity" FieldName="Quantity">
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