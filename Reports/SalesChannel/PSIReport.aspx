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
    <asp:UpdatePanel runat="server" ID="updYear" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="contentbox">
                <div class="searchbg">
                    <div class="H25-C4-S">
                        <ul>
                            <li class="text">Year
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="cmbYear"
                                        runat="server" CssClass="formselect" AutoPostBack="True" OnSelectedIndexChanged="cmbYear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </li>
                            <li class="text">Month
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="cmbMonth" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="field3">
                                <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                                <asp:Button ID="btnExport" runat="server" CssClass="buttonbg"
                                    Text="Export in Excel" OnClick="btnExport_Click" />
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <div style="display: none">
        <div class="mainheading">
            PSI Information
        </div>
    </div>
    <div style="display: none">
        <div class="contentbox">
            <div class="PivotGrid">
                <dxwpg:ASPxPivotGrid Width="100%" ID="ASPxPvtGrd" runat="server" EnableRowsCache="false">
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
    </div>
</asp:Content>

