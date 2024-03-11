<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    AutoEventWireup="true" CodeFile="TertiaryReportSummary.aspx.cs" Inherits="Reports_Common_TertiaryReportSummary" %>
<%@ Register Src="~/UserControls/Footer.ascx" TagName="ucFooter" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Header.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="ucMsg" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>


<%--
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <ucMsg:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Tertiary Report Summary
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C4-S">
            <ul>
                <li class="text">Date From:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="To date required." ValidationGroup="rptPurchase"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
                <li class="text">Date To:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="rptPurchase"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
                <li class="text">ND: <span class="error">&nbsp;</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlNDS" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                </li>
            </ul>
            <ul>
                <li class="text">Model: <span class="error">&nbsp;</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlModelName" CssClass="formselect" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlModelName_SelectedIndexChanged">
                    </asp:DropDownList>
                </li>
                <li class="text">SKU: <span class="error">&nbsp;</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlSku" runat="server" CssClass="formselect" AutoPostBack="false">
                    </asp:DropDownList>
                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                        <asp:Button ID="btnSearch" Text="Show" runat="server" ValidationGroup="rptPurchase"
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
        <div>
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
                            <asp:Button ID="buttonSaveAs" runat="server" ToolTip="Export and save" Style="vertical-align: middle;"
                                OnClick="buttonSaveAs_Click" Text="Save" CssClass="buttonbg" />
                            <asp:Button ID="buttonOpen" Visible="false" runat="server" ToolTip="Export and open"
                                Style="vertical-align: middle" OnClick="buttonOpen_Click" Text="Open" CssClass="buttonbg" />
                        </li>
                    </ul>
                </div>
            </div>
            <div class="mainheading">
                Stock Infomation
            </div>
            <div class="contentbox">
               <div class="PivotGrid">
                    <dxwpg:ASPxPivotGrid Width="100%" ID="ASPxPvtGrd" runat="server" EnableViewState="false"
                        EnableRowsCache="false">

                        <OptionsView ShowHorizontalScrollBar="false" />
                        <%--<OptionsView ShowFilterHeaders="False" />
                        <OptionsView ShowColumnHeaders="False" />
                        <OptionsView ShowDataHeaders="False" />
                        <OptionsView ShowColumnGrandTotals="False" />--%>

                        <%-- <OptionsView ShowRowHeaders="False" />--%>
                        <Fields>

                            <dxwpg:PivotGridField ID="PivotGridField17" Area="RowArea" AreaIndex="1" FieldName="DATE"
                                Caption="DATE">
                            </dxwpg:PivotGridField>
                            <dxwpg:PivotGridField ID="PivotGridField18" Area="ColumnArea" AreaIndex="0" FieldName="Activation"
                                Caption="Activation">
                            </dxwpg:PivotGridField>



                            <dxwpg:PivotGridField ID="PivotGridField15" Area="DataArea" CellFormat-FormatType="Numeric"
                                AreaIndex="0" Caption="COUNT" FieldName="COUNT">
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
        </div>
    </asp:Panel>
</asp:Content>
