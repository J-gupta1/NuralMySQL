<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    CodeFile="TargetVsAchievementRpt.aspx.cs" Inherits="Reports_Common_TargetVsAchievementRpt" %>

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
        Target Vs. Achievement Report
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Target Based On:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlTagetBased" CssClass="formselect" runat="server">
                        <asp:ListItem Selected="True" Value="0" Text="Select">
                        </asp:ListItem>
                        <asp:ListItem Value="1" Text="Sales">
                        </asp:ListItem>
                        <asp:ListItem Value="2" Text="Purchase">
                        </asp:ListItem>
                        <asp:ListItem Value="3" Text="WoD">
                        </asp:ListItem>
                    </asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTagetBased"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select target based on."
                            SetFocusOnError="true" ValidationGroup="rpt"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <%-- #CC02 Add Start 
                <li class="text">Select Type:<span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlType" CssClass="formselect" ValidationGroup="rpt" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                            runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="rqDDLType" runat="server" ValidationGroup="rpt"
                            InitialValue="0" ControlToValidate="ddlType" CssClass="error" Display="Dynamic"
                            ErrorMessage="Please Select Type."></asp:RequiredFieldValidator>
                    </div>
                </li>
                #CC02 Add End --%>

                <li class="text">
                    <%-- SalesChannel #CC02 Commented --%> Select Channel Type:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlHierarchy" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlHierarchy"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales hannel type."
                            SetFocusOnError="true" ValidationGroup="rpt"></asp:RequiredFieldValidator>
                    </div>
                </li>

                <li class="text">Date From:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                        ValidationGroup="rpt" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
                <li class="text">Date To:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." defaultDateRange="true" />
                    <%-- ValidationGroup="rpt" RangeErrorMessage="Date should be less or equal then current date."--%>
                </li>
            </ul>
            <div class="setbbb">
                <div class="float-margin">
                    <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                    <asp:Button ID="btnSearch" Text="Show" runat="server" ValidationGroup="rpt" ToolTip="Search"
                        CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                </div>
                <div class="float-margin">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                        OnClick="btnCancel_Click" CausesValidation="false" />
                </div>
            </div>

        </div>
    </div>
    <asp:Panel ID="pnlSearch" runat="server" Visible="false">
        <div class="mainheading">
            Export
        </div>
        <div class="contentbox">
            <div class="print-col">
                <ul>
                    <li class="frmtxt1">
                        <strong>Print options: </strong>
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
                        <asp:DropDownList ID="ddlExportFormat" runat="server" Style="vertical-align: middle"
                            ValueType="System.String" CssClass="form_select4">
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
            Target Vs. Achievement List
        </div>

        <%--      <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExprtToExcel_Click"
                                                                        CausesValidation="False" />--%>

        <div class="contentbox">
            <div class="PivotGrid">
                <dxwpg:ASPxPivotGrid Width="100%" ID="ASPxPvtGrd" runat="server" EnableViewState="false" OptionsView-ShowHorizontalScrollBar="false"
                    EnableRowsCache="false" OnCustomSummary="ASPxPvtGrd_CustomSummary">
                    <Fields>
                        <dxwpg:PivotGridField ID="PivotGridField1" Area="FilterArea" AreaIndex="0" EmptyValueText="N/A"
                            Caption="SalesChannel Name" FieldName="SalesChannelName">
                        </dxwpg:PivotGridField>
                        <%--#CC03 Add Start --%>
                        <dxwpg:PivotGridField ID="PivotGridField7" Area="FilterArea" AreaIndex="1" CellFormat-FormatType="Numeric"
                            FieldName="LocationName" Caption="Location Name">
                        </dxwpg:PivotGridField>
                        <%--#CC03 Add End --%>
                        <dxwpg:PivotGridField ID="PivotGridField2" Area="RowArea" AreaIndex="0" EmptyValueText="N/A"
                            Caption="SalesChannel Name" FieldName="SalesChannelName">
                        </dxwpg:PivotGridField>
                        <%--  --------#CC01 Started 16/11/2017 Vijay Kumar Prajapati--------------------------------------%>
                        <dxwpg:PivotGridField ID="PivotGridDistCode" Area="RowArea" AreaIndex="0" EmptyValueText="N/A"
                            Caption="SalesChannel Code" FieldName="SalesChannelCode">
                        </dxwpg:PivotGridField>
                        <%--  -------------------------------End---------------%><%--  ----------------------------------------------%>
                        <dxwpg:PivotGridField ID="PivotGridField3" Area="RowArea" AreaIndex="1" Caption="SalesChannelType"
                            FieldName="SalesChannelTypeName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField4" Area="RowArea" AreaIndex="0" EmptyValueText="N/A"
                            Caption="Target Name" FieldName="TargetName">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="pvtTarget" Area="DataArea" AreaIndex="0" CellFormat-FormatType="Numeric"
                            FieldName="TARGET" Caption="Target">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField5" Area="DataArea" AreaIndex="1" CellFormat-FormatType="Numeric"
                            FieldName="Achievement" Caption="Achievement">
                        </dxwpg:PivotGridField>
                        <dxwpg:PivotGridField ID="PivotGridField6" Area="DataArea" AreaIndex="2" CellFormat-FormatType="Numeric"
                            FieldName="achivementpercent" Caption="Achievement %" SummaryType="Custom">
                        </dxwpg:PivotGridField>
                        <%-- <dxwpg:PivotGridField ID="PivotGridField7" Area="DataArea" AreaIndex="2" CellFormat-FormatType="Numeric"
                                                                FieldName="LocationName" Caption="LocationName">
                                                            </dxwpg:PivotGridField> #CC03 Commented --%>
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
