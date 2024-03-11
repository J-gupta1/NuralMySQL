<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesChannelLedger.aspx.cs"
    Inherits="Reports_POC_SalesChannelLedger" MasterPageFile="~/CommonMasterPages/MasterPage.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div>
                <div>
                    <asp:UpdatePanel ID="updpnlMsg" runat="server">
                        <ContentTemplate>
                            <uc1:ucMessage ID="ucMessage1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="mainheading">
                    Search
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">Sales Channel Type: </li>
                            <li class="field">
                                <asp:DropDownList ID="cmbsaleschanneltype" runat="server" CssClass="formselect"
                                    OnSelectedIndexChanged="cmbsaleschanneltype_SelectedIndexChanged1" AutoPostBack="True">
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="cmbsaleschanneltype"
                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                    SetFocusOnError="true" ValidationGroup="report"></asp:RequiredFieldValidator>--%>
                            </li>
                            <li class="text">SalesChannel Name:  </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSaleschannel" runat="server" CssClass="formselect" ValidationGroup="Add"
                                    AutoPostBack="false">
                                    <asp:ListItem Selected="True" Value="0" Text="Select"></asp:ListItem>
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSaleschannel"
                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel."
                                    SetFocusOnError="true" ValidationGroup="report"></asp:RequiredFieldValidator>--%>
                            </li>
                            <li class="text">Date From : <span class="error">*</span>
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Please Enter Date."
                                    RangeErrorMessage="Invalid date!" IsRequired="True" ValidationGroup="report" />
                            </li>
                        </ul>
                        <ul>
                            <li class="text">Date To: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Please Enter Date."
                                    RangeErrorMessage="Invalid date!" IsRequired="True" ValidationGroup="report" />
                            </li>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" ActionTag="View"
                                        OnClick="btnSearch_Click" ValidationGroup="report" />
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnCancel" runat="server" Text="Reset" CssClass="buttonbg" OnClick="btnCancel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>           
            <div id="dvReport" runat="server" visible="false">
                <div id="dvExport" runat="server">
                    <div>
                        <div class="mainheading">
                            Export
                        </div>
                        <div class="contentbox">
                            <div class="print-col">
                                <ul>
                                     <li class="frmtxt1">
                                        <strong>Export Options: </strong>
                                    </li>
                                    <li>
                                        <asp:CheckBox ID="checkPrintHeadersOnEveryPage" runat="server" Text="Print headers on every page" />
                                    </li>
                                    <li>
                                        <asp:CheckBox ID="checkPrintFilterHeaders" runat="server" Text="Print filter headers"
                                            Checked="false" />
                                    </li>
                                    <li>
                                        <asp:CheckBox ID="checkPrintColumnHeaders" runat="server" Text="Print column headers"
                                            Checked="false" />
                                    </li>
                                    <li>
                                        <asp:CheckBox ID="checkPrintRowHeaders" runat="server" Text="Print row headers" Checked="True" />
                                    </li>
                                    <li>
                                        <asp:CheckBox ID="checkPrintDataHeaders" runat="server" Text="Print data headers"
                                            Checked="false" />
                                    </li>
                                </ul>
                                <ul>
                                    <li class="frmtxt1">Export to:
                                    </li>
                                    <li>
                                        <dx:ASPxComboBox ID="listExportFormat" runat="server"
                                            SelectedIndex="1" ValueType="System.String" Width="40px" CssClass="formselect" >
                                            <Items>
                                                <dx:ListEditItem Text="Excel" Value="1" />
                                                <dx:ListEditItem Text="Excel (.xlsx)" Value="2" />
                                                <dx:ListEditItem Text="PDF" Value="3" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </li>
                                    <li>
                                        <asp:Button ID="buttonSaveAs" runat="server" Text="Export" ToolTip="Export and save"
                                            CssClass="buttonbg" OnClick="buttonSaveAs_Click"  />
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mainheading">
                    Sales Infomation
                </div>
                <div class="contentbox">
                    <div class="PivotGrid">
                        <dx:ASPxGridViewExporter ID="ASPxGridExporter1" runat="server" GridViewID="gvVoucher">
                        </dx:ASPxGridViewExporter>
                        <dx:ASPxGridView ID="gvVoucher" runat="server" AutoGenerateColumns="False" ClientInstanceName="gvVoucher"
                            EnableViewState="True" Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                            CssPostfix="Aqua" EnableRowsCache="False">
                            <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                                <LoadingPanel ImageSpacing="8px">
                                </LoadingPanel>
                            </Styles>
                            <Settings ShowColumnHeaders="true" ShowGroupPanel="false" ShowGroupedColumns="false"
                                VerticalScrollableHeight="0" ShowFilterRow="false" />
                            <SettingsLoadingPanel ImagePosition="Top" />
                            <SettingsPager Mode="ShowPager" NextPageButton-Text="Next &gt;" PrevPageButton-Text="&lt; Prev">
                                <NextPageButton Text="Next &gt;">
                                </NextPageButton>
                                <PrevPageButton Text="&lt; Prev">
                                </PrevPageButton>
                            </SettingsPager>
                            <ImagesFilterControl>
                                <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                </LoadingPanel>
                            </ImagesFilterControl>
                            <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                                </LoadingPanelOnStatusBar>
                                <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                                </LoadingPanel>
                            </Images>
                            <Columns>
                                <dx:GridViewDataTextColumn Width="200px" FieldName="DocNo" Caption="DOC NO." VisibleIndex="0">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Width="200px" FieldName="Trans" Caption="DOC TYPE" VisibleIndex="1">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Width="200px" FieldName="DocDate" Caption="REF. DATE"
                                    VisibleIndex="2">
                                    <PropertiesTextEdit DisplayFormatString="d">
                                    </PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="DRAmt" Caption="DR. BALANCE" VisibleIndex="3">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="CRAmt" Caption="CR. BALANCE" VisibleIndex="4">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="balance" Caption="Closing BALANCE" VisibleIndex="5">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <StylesEditors>
                                <CalendarHeader Spacing="1px">
                                </CalendarHeader>
                                <ProgressBar Height="25px">
                                </ProgressBar>
                            </StylesEditors>
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
                            </ImagesEditors>
                        </dx:ASPxGridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="buttonSaveAs" />
            <asp:PostBackTrigger ControlID="btnCancel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
