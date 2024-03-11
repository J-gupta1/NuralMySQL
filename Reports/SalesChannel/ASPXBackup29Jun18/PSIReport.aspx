<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master" AutoEventWireup="true" CodeFile="PSIReport.aspx.cs" Inherits="Reports_SalesChannel_PSIReport" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1" Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
        Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
    <%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
        Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <table cellspacing="0" cellpadding="0" width="100%" border="0" style="float: left;">

        <tr>
            <td valign="top" align="left" class="tableposition">
                <asp:UpdatePanel runat="server" ID="updYear" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="contentbox">
                            <table cellspacing="0" cellpadding="4" width="100%" border="0">

                                <tr>

                                    <td class="formtext" valign="top" align="right">
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr class="searchbg">
                                                <td width="5%"></td>
                                                <td align="left" valign="middle" height="35" width="4%">Year
                                                </td>

                                                <td width="15%" align="left" valign="middle">
                                                    <div style="float: left; width: 135px;">
                                                        <asp:DropDownList ID="cmbYear"
                                                            runat="server" CssClass="form_select" AutoPostBack="True" OnSelectedIndexChanged="cmbYear_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>

                                                </td>

                                                <td width="5%" align="left" valign="middle">Month
                                                </td>
                                                <td width="15%" align="left" valign="middle">
                                                    <div style="float: left; width: 135px;">
                                                        <asp:DropDownList ID="cmbMonth" runat="server" CssClass="form_select">
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>


                                                <td width="40%" align="left" valign="middle">
                                                    <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                                                    <asp:Button ID="btnExport" runat="server" CssClass="buttonbg"
                                                        Text="Export in Excel" OnClick="btnExport_Click" />
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExport" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>

        <tr style="display: none">
            <td align="left" valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left" valign="top" class="tableposition">
                            <div class="mainheading">
                                PSI Information
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="display: none">
            <td align="left" valign="top" class="tableposition2" style="float: left;">
                <div class="contentbox">
                    <div class="grid1">
                        <dxwpg:ASPxPivotGrid Width="958" ID="ASPxPvtGrd" runat="server" EnableRowsCache="false">
                            <OptionsView ShowHorizontalScrollBar="false" ShowRowGrandTotals="False" ShowRowTotals="True" ShowColumnGrandTotals="False" />
                            <Fields>
                                <dxwpg:PivotGridField ID="PivotGridField1" Area="RowArea" AreaIndex="0" EmptyValueText="N/A"
                                    FieldName="SalesChannelCode" Caption="RDS Code">
                                </dxwpg:PivotGridField>
                                <dxwpg:PivotGridField ID="PivotGridField2" Area="RowArea" AreaIndex="1" EmptyValueText="N/A"
                                    FieldName="SalesChannelName" Caption="RDS Name">
                                </dxwpg:PivotGridField>
                                <dxwpg:PivotGridField ID="PivotGridField3" Area="RowArea" AreaIndex="2" EmptyValueText="N/A"
                                    FieldName="Model" Caption="Model">
                                </dxwpg:PivotGridField>
                                <dxwpg:PivotGridField ID="PivotGridFieldBin" Area="RowArea" AreaIndex="3" EmptyValueText="N/A"
                                    FieldName="StockBin" Caption="StockBin">
                                </dxwpg:PivotGridField>
                                <dxwpg:PivotGridField ID="PivotGridField4" Area="ColumnArea" AreaIndex="2" EmptyValueText="N/A"
                                    FieldName="TransactionType" Caption="Transaction" TotalsVisibility="AutomaticTotals">
                                </dxwpg:PivotGridField>

                                <dxwpg:PivotGridField ID="PivotGridField16" Area="ColumnArea" ValueFormat-FormatString="dd/MMM/yy" ValueFormat-FormatType="DateTime"
                                    AreaIndex="0" Caption="Date" FieldName="Date" TotalsVisibility="None">
                                </dxwpg:PivotGridField>

                                <dxwpg:PivotGridField ID="PivotGridField15" Area="DataArea" CellFormat-FormatType="Numeric"
                                    AreaIndex="1" Caption="Qty" FieldName="Qty">
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
</asp:Content>

