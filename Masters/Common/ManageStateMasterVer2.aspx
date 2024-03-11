<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageStateMasterVer2.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" EnableEventValidation="false"
    Inherits="Masters_Common_ManageStateMasterVer2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Menu.css") %>" />
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script type="text/javascript">
        function popup(StateID, PriceListChangeLogid) {
            WinSearchChannelCode = dhtmlmodal.open("State Price List", "iframe", "ManageEditPriceListWState.aspx?StateID=" + StateID + "&PriceListChangeLogid=" + PriceListChangeLogid, "State Price List", "width=800px,height=430px,resize=0,scrolling=auto ,center=1", "recal")
            WinSearchChannelCode.onclose = function () {
                var btn = document.getElementById('<%= btnGetallData.ClientID %>');
                __doPostBack(btn.name, "OnClick");
                return true;
            }
            //return true;
            return false;
        }



    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">



    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="clear"></div>
    <div class="mainheading">
        Add / Edit State
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblstatename" runat="server" Text="">State Name:</asp:Label>
                            <span class="error">* </span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertName" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtInsertName" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert  Name " ValidationGroup="insert" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtInsertName"
                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                            </cc1:FilteredTextBoxExtender>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblcstatecode" runat="server" Text="">State Code:</asp:Label>
                            <span class="error">* </span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtInsertCode" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert  code " ValidationGroup="insert" />
                            <cc1:FilteredTextBoxExtender ID="txtnameValid" runat="server" TargetControlID="txtInsertCode"
                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                            </cc1:FilteredTextBoxExtender>
                        </li>
                        <li class="text">
                            <asp:Label ID="Label1" Text="" runat="server" />Country: <span class="error">* </span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbInsCountry" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbInsCountry_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbInsCountry" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Please select a Country " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <%--=================#CC03 Added Started=============================--%>
                        <li class="text">
                            <asp:Label ID="lblRegion" Text="Region:" runat="server" />
                            <span class="error">* </span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlRegion" runat="server" CssClass="formselect" AutoPostBack="false">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorddlRegion" ControlToValidate="ddlRegion" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Please select  Region " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <%--====================#CC03 Added End==========================--%>
                        <li class="text">Status:
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkstatus" runat="server" Checked="true" />
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnsubmit" runat="server" OnClick="btninsert_click" Text="Submit"
                                    ValidationGroup="insert" CssClass="buttonbg" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"
                                    Text="Cancel" CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>

            <%--#CC01 Add Start --%>
            <Triggers>
                    <%--   <asp:AsyncPostBackTrigger ControlID="btnsubmit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btncancel" EventName="Click" />--%>
         
                <asp:PostBackTrigger ControlID="btnsubmit"  />
                <asp:PostBackTrigger ControlID="btncancel"  />
            </Triggers>
            <%--#CC01 Add End --%>
        </asp:UpdatePanel>
    </div>

    <div class="mainheading">
        Search State
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblfndstatecode" runat="server" Text="State Code:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerCode" runat="server" CssClass="formfields"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtSerCode"
                                CssClass="error" ErrorMessage="Invalid  Searching parameter" ValidationExpression="[^()<>/\@%]{1,50}"
                                ValidationGroup="search" runat="server" />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblstatefnname" runat="server" Text="State Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerName" runat="server" CssClass="formfields"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtSerName"
                                CssClass="error" ErrorMessage="Invalid Serching Parameter " ValidationExpression="[^()<>/\@%]{1,50}"
                                ValidationGroup="search" runat="server" />
                        </li>
                        <li class="text">
                            <asp:Label ID="Label2" runat="server" Text="Country:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerCountry" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblserchprice" runat="server" Text="Price List Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerPriceList" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                    CssClass="buttonbg" CausesValidation="False" ValidationGroup="search" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnGetallData" runat="server" Text="Show All Data" OnClick="btnGetallData_Click"
                                    CssClass="buttonbg" CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <%--#CC01 Add Start --%>
            <Triggers>
                      <%-- <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnGetallData" EventName="Click" />--%>
         
                                <asp:PostBackTrigger ControlID="btnSearch"  />
                <asp:PostBackTrigger ControlID="btnGetallData"  />
            </Triggers>
            <%--#CC01 Add End --%>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExprtToExcel_Click"
            CausesValidation="False" />
    </div>
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="contentbox">
                <div class="grid1">
                    <dxwgv:ASPxGridView ID="grdState" ClientInstanceName="grdState" runat="server" KeyFieldName="StateID"
                        Width="100%" OnDetailRowExpandedChanged="grdState_DetailRowExpandedChanged" OnRowCommand="grdState_RowCommand1"
                        OnHtmlRowPrepared="grdState_HtmlRowPrepared" Styles-AlternatingRow-CssClass="Altrow"
                        Styles-Header-HorizontalAlign="Left" Styles-Header-Font-Bold="true" Styles-Header-ForeColor="White" Styles-Header-CssClass="gridheader"
                        OnHtmlRowCreated="grdState_HtmlRowCreated">
                        <Styles>
                            <Header CssClass="gridheader" Font-Bold="True" ForeColor="White" HorizontalAlign="Left">
                            </Header>
                        </Styles>
                        <Columns>
                            <dxwgv:GridViewDataColumn FieldName="StateCode" VisibleIndex="0" />
                            <dxwgv:GridViewDataColumn FieldName="StateName" VisibleIndex="1" />
                            <dxwgv:GridViewDataColumn FieldName="CountryName" VisibleIndex="2" />
                            <dxwgv:GridViewDataColumn FieldName="RegionName" VisibleIndex="3" />
                            <dxwgv:GridViewDataColumn FieldName="PriceListName" VisibleIndex="4" />
                            <dxwgv:GridViewDataTextColumn FieldName="StateID" VisibleIndex="5" Visible="false">
                                <DataItemTemplate>
                                    <asp:Label ID="lblSymptomCode" Visible="true" runat="server" Text='<%# Eval("StateID") %>'
                                        CommandArgument='<%# Eval("StateID") %>'></asp:Label>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="Active" VisibleIndex="5" Visible="false">
                                <DataItemTemplate>
                                    <asp:Label ID="lblActive" Visible="true" runat="server" Text='<%# Eval("Active") %>'
                                        CommandArgument='<%# Eval("StateID") %>'></asp:Label>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="StateID" Caption="StateID" Visible="false" />
                            <dxwgv:GridViewDataColumn Name="Delete" VisibleIndex="4" Caption="Action" CellStyle-HorizontalAlign="Left">
                                <DataItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxButton ID="btnDteDetail" CommandName="cmdEdit" runat="server" Image-Url='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                                    Image-Height="15" Image-Width="15" EnableDefaultAppearance="false" EnableTheming="false" Cursor="pointer"
                                                    ToolTip="Edit" CausesValidation="false" CommandArgument='<%# Eval("StateID") %>' />
                                            </td>
                                            <td>
                                                <dxe:ASPxButton ID="btnActive" CommandName="activeState" runat="server" Image-Height="15"
                                                    Image-Width="15" EnableDefaultAppearance="false" EnableTheming="false" CausesValidation="false" Cursor="pointer"
                                                    CommandArgument='<%# Eval("StateID") %>' Image-Url='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' />
                                            </td>
                                            <td>
                                                <dxe:ASPxButton ID="btnMap" runat="server" Image-Height="15" Image-Width="15" EnableDefaultAppearance="false" Cursor="pointer"
                                                    EnableTheming="false" CausesValidation="false" CommandArgument='<%# Eval("StateID") %>'
                                                    Image-Url='<%#"~/" + strAssets + "/CSS/Images/application_view_list.png"%>' />
                                            </td>
                                        </tr>

                                    </table>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                            </dxwgv:GridViewDataColumn>
                        </Columns>
                        <Templates>
                            <DetailRow>
                                <dxwgv:ASPxGridView ID="detailGrid" runat="server" KeyFieldName="PriceListChangeLogID"
                                    Width="100%" OnBeforePerformDataSelect="detailGrid_DataSelect" EnableCallBacks="False"
                                    OnRowCommand="detailGrid_RowCommand" OnHtmlRowPrepared="detailGrid_HtmlRowPrepared"
                                    SettingsBehavior-AllowSort="false" Styles-Header-HorizontalAlign="Left" Styles-Header-Font-Bold="true"
                                    Styles-Header-ForeColor="White" Styles-Header-BackColor="#02a234">
                                    <Columns>
                                        <dxwgv:GridViewDataColumn FieldName="FromDate" Caption="From Date" VisibleIndex="2" />
                                        <dxwgv:GridViewDataColumn FieldName="PriceListName" Caption="Price List Name" VisibleIndex="1" />

                                        <dxwgv:GridViewDataColumn Name="Delete" VisibleIndex="4" Caption="Action" CellStyle-HorizontalAlign="Left">
                                            <DataItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxLabel ID="lblEffectiveDate" runat="server" Text='<%# Eval("FromDate") %>'
                                                                Visible="false">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxButton ID="btnDeletePriceList" CommandName="DeletePriceList" runat="server"
                                                                Image-Url='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>' Image-Height="15"
                                                                Image-Width="15" EnableDefaultAppearance="false" EnableTheming="false" ToolTip="Delete"
                                                                CausesValidation="false" CommandArgument='<%# Eval("PriceListChangeLogID") %>'
                                                                Visible="false" />
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxButton ID="btnEditPriceList" CommandName="EditPriceList" runat="server"
                                                                Image-Height="15" Visible="false" Image-Width="15" EnableDefaultAppearance="false"
                                                                EnableTheming="false" CausesValidation="false" ToolTip="Edit" CommandArgument='<%# Eval("PriceListChangeLogID") %>'
                                                                Image-Url='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewCommandColumn Caption=" " VisibleIndex="5" ButtonType="Image">
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                            <ClearFilterButton Visible="True">
                                            </ClearFilterButton>
                                        </dxwgv:GridViewCommandColumn>
                                    </Columns>
                                    <Settings ShowFooter="True" />
                                    <SettingsDetail ShowDetailRow="False" IsDetailGrid="True" />
                                    <SettingsBehavior AllowFocusedRow="True" ProcessSelectionChangedOnServer="False" />
                                </dxwgv:ASPxGridView>
                            </DetailRow>
                        </Templates>
                        <SettingsDetail ShowDetailRow="true" />
                    </dxwgv:ASPxGridView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="grdState" />
        </Triggers>
    </asp:UpdatePanel>




</asp:Content>
