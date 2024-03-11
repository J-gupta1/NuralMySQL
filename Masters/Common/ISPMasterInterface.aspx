<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    CodeFile="ISPMasterInterface.aspx.cs" EnableEventValidation="false" Inherits="Masters_Common_ISPMasterInterface" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPager" TagPrefix="dxwp" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Src="~/UserControls/ucPagingControl.ascx" TagName="ucPagingControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        var isListBoxSelectionChanged = false;
        function OnListBoxSelectionChanged(listBox, args, checkComboBox) {
            isListBoxSelectionChanged = true;
        }
        function OnCloseUp(s, e) {
            if (isListBoxSelectionChanged) {
                // btn.DoClick();
                __doPostBack(btn, "DoClick");
            }
        }
    </script>

    <%-- <script type="text/javascript">
        function GridRowAction(rowId) {
            var parameters = 'RowClick;' + rowId;
            callbackPanel.PerformCallback(parameters);
            var a = document.getElementById("ctl00_contentHolderMain_updpnlSaveData");
            //a.Update();
            //  return true;
        }
        function RefreshProductCategoryList(s, e) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm._doPostBack('ctl00_contentHolderMain_updpnlGrid1', '');
            __doPostBack('ctl00_contentHolderMain_updpnlSaveData', '');
            return true;
        }
    </script>--%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script type="text/javascript">

        function popupISP(ISPID, RetailerISPMappingID, PopupTitle, mode) {

            WinCallLogDetails = dhtmlmodal.open("ISPPopup", "iframe", "../../Popuppages/ISPPopup.aspx?Ispid=" + ISPID + "&retIspid=" + RetailerISPMappingID + "&mode=" + mode, PopupTitle, "width=900px,height=530px,top=25,resize=0,scrolling=auto ,center=1");
            WinCallLogDetails.onclose = function () {
                var record = this.contentDoc.getElementById("hdfSuccess");
                if (record.value == "1") {
                    var btn = document.getElementById("ctl00_contentHolderMain_btnSearch");
                    if (btn != null) {
                        __doPostBack(btn.name, "OnClick");
                    }
                    return true;
                }
                else {
                    return true;
                }
            };

            return false;
        }
        function popupRetailer() {
            var WinSearchRetailerCode = dhtmlmodal.open("WinSearchRetailerCode", "iframe", "../../Popuppages/SearchRetailer.aspx", "Retailer Detail", "width=800px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
            WinSearchRetailerCode.onclose = function () {

                return true;
            }
            return false;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <asp:UpdatePanel ID="UpdMain" runat="server">
        <ContentTemplate>
            <uc4:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Select Mode:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:RadioButtonList ID="rdobeautyAdvisorMode" runat="server" CssClass="radio-rs" RepeatDirection="Horizontal" CellPadding="2"
                                OnSelectedIndexChanged="rdobeautyAdvisorMode_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="1" Text="Excel Template"></asp:ListItem>
                                <asp:ListItem Selected="True" Value="2" Text="Interface"></asp:ListItem>
                            </asp:RadioButtonList>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <ul>
                        <li class="text">Retailer Name:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtRetailerName" runat="server" CssClass="formfields" Enabled="False"></asp:TextBox>
                            <asp:Label ID="lblI" runat="server"></asp:Label>
                            <%--#CC06 Add Start --%>
                            <div>
                                <asp:RequiredFieldValidator ID="rqfvRetailerName" runat="server" ControlToValidate="txtRetailerName"
                                    CssClass="error" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please Select Reatiler."
                                    ValidationGroup="AddBeautyAdvisor"></asp:RequiredFieldValidator>
                            </div><%--#CC06 Add End --%>
                        </li>
                        <li class="text-field">
                            <asp:Button ID="btnSearchRetailer" runat="server" Text="Search Retailer" CssClass="buttonbg" />
                        </li>
                        <li class="text">ISP Code:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtBeautyAdvisorCode" runat="server" CssClass="formfields" MaxLength="15"
                                ValidationGroup="AddBeautyAdvisor"></asp:TextBox>
                            <div>
                                <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtBeautyAdvisorCode"
                                    CssClass="error" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please Enter ISP Code."
                                    ValidationGroup="AddBeautyAdvisor"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                         <%--#CC07 Add Start --%>
                         <li class="text">Store Code:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtStoreCode" runat="server" CssClass="formfields"></asp:TextBox>
                            <div>
                                <asp:RequiredFieldValidator ID="rqtxtStoreCode" runat="server" ControlToValidate="txtStoreCode"
                                    CssClass="error" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please Enter Store Code."
                                    ValidationGroup="AddBeautyAdvisor"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <%--#CC07 Add End --%>

                    </ul>
                    <ul>
                        <li class="text">ISP Name:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtBeautyAdvisorName" runat="server" CssClass="formfields" MaxLength="70"
                                ValidationGroup="AddBeautyAdvisor"></asp:TextBox>
                            <div>
                                <asp:RequiredFieldValidator ID="req3" runat="server" ControlToValidate="txtBeautyAdvisorName"
                                    CssClass="error" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please Enter ISP Name."
                                    ValidationGroup="AddBeautyAdvisor"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Mobile No:<span class="error" id="spnMobileNumIcon" runat="server">*</span> <%--#CC06  id and runat Added--%>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtMobileNo" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox>
                            <div>
                                <asp:RequiredFieldValidator ID="req4" runat="server" ControlToValidate="txtMobileNo"
                                    CssClass="error" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please Enter Mobile No."
                                    ValidationGroup="AddBeautyAdvisor"></asp:RequiredFieldValidator>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtMobileNo"
                                    ValidChars="0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </li>
                    </ul>
                    <%-- #CC07 Comment Start <ul>
                        <li class="text" id="fromDateHead" runat="server">From Date:<span class="error">*</span>
                        </li>
                        <li class="field" id="fromDateField" runat="server">
                            <uc2:ucDatePicker ID="ucDatePickerFromDate" ErrorMessage="Please Enter From Date."
                                runat="server" ValidationGroup="AddBeautyAdvisor" RangeErrorMessage="Date can  not be less than todays." />
                        </li>
                    </ul> #CC07 Comment End --%>

                    <div id="tblGrid">
                        <asp:UpdatePanel ID="updGrid" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                    <ul>
                                        <li class="text">User Name:<span class="error">*</span>
                                        </li>
                                        <li class="field">
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                                            <div>
                                                <asp:RequiredFieldValidator ID="reqUserName" runat="server" ControlToValidate="txtUserName"
                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Enter User Name." SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </li>
                                        <li class="text" runat="server" id="tdPassword">Password:<span class="error">*</span>
                                        </li>
                                        <li class="field">
                                            <asp:TextBox ID="txtpassword" runat="server" CssClass="formfields" MaxLength="20"
                                                TextMode="Password" ValidationGroup="Add1"></asp:TextBox>
                                            <div>
                                                <asp:RequiredFieldValidator ID="reqpassword" runat="server" ControlToValidate="txtpassword"
                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Password." SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </li>
                                    </ul>
                                    <%-- <ul>
                                        <li class="text">Email ID:<span class="error"> &nbsp;</span>
                                        </li>
                                        <li class="field">
                                           <asp:TextBox ID="txtemail" runat="server" CssClass="formfields" MaxLength="80" ValidationGroup="Add"></asp:TextBox>
                                            <div>
                                                <asp:RegularExpressionValidator ID="RegularEmail" runat="server" ControlToValidate="txtemail"
                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid emailid."
                                                    ForeColor="" SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    ValidationGroup="AddBeautyAdvisor"></asp:RegularExpressionValidator>
                                            </div>
                                        </li>
                                    </ul>--%>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                         <ul id="ulEmail" runat="server">
                                        <li class="text">Email ID:<span id="emailMandateSign" runat="server" class="error">*</span>
                                        </li>
                                        <li class="field">
                                           <asp:TextBox ID="txtemail" runat="server" CssClass="formfields" MaxLength="80" ValidationGroup="Add"></asp:TextBox>
                                            <div>
                                                <asp:RegularExpressionValidator ID="RegularEmail" runat="server" ControlToValidate="txtemail"
                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid emailid."
                                                    ForeColor="" SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    ValidationGroup="AddBeautyAdvisor"></asp:RegularExpressionValidator>
                                            </div>
                                             <asp:RequiredFieldValidator ID="rqEmail" runat="server" ControlToValidate="txtemail"
                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Email ID." SetFocusOnError="true"
                                                 ValidationGroup="AddBeautyAdvisor"
                                                 ></asp:RequiredFieldValidator><%-- #CC09 ValidationGroup="AddBeautyAdvisor" Added--%>
                                        </li>
                                    </ul>

                        <ul>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSubmit" runat="server" CausesValidation="true" CssClass="buttonbg"
                                        OnClick="btnSubmit_Click" Text="Submit" ValidationGroup="AddBeautyAdvisor" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                                        OnClick="btnCancel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        ISP List
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updsearch" runat="server">
            <ContentTemplate>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">ISP Name:<span class="error"></span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSearchBeautyAdvisorName" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                         <%--#CC06 Add Start --%>
                         <li class="text">ISP Code:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtISPCode" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <%--#CC06 Add End --%>
                          <%--#CC08 Add Start --%>
                         <li class="text">Store Code:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtStoreCodeSearch" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <%--#CC08 Add End --%>

                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" CssClass="buttonbg" OnClick="btnSearch_Click"
                                    Text="Search" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnSearchALL" runat="server" Text="View All" OnClick="btnSearchALL_Click"
                                    CssClass="buttonbg"  />
                            </div> 

                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="margin-bottom">
        <asp:Button ID="btnExport" runat="server" CssClass="buttonbg" Text="Export To Excel"
            OnClick="btnExport_Click" />
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="grid1">
                    <dxwgv:ASPxGridView ID="grdvList" ClientInstanceName="grdvList" runat="server" KeyFieldName="ISPID"
                        Width="100%" OnDetailRowExpandedChanged="grdvList_DetailRowExpandedChanged" OnRowCommand="grdvList_RowCommand1"
                        OnHtmlRowPrepared="grdvList_HtmlRowPrepared" Styles-AlternatingRow-BackColor="Beige"
                        Styles-Header-HorizontalAlign="Left" Styles-Header-Font-Bold="true" Styles-Header-ForeColor="White"   >
                        <Columns>

                            <dxwgv:GridViewDataColumn FieldName="ISPCode" VisibleIndex="1"  />
                            <dxwgv:GridViewDataColumn FieldName="StoreCode" VisibleIndex="2" />
                            <dxwgv:GridViewDataColumn FieldName="ISPName" VisibleIndex="2" />
                            <dxwgv:GridViewDataColumn FieldName="LoginName" Caption="LogIn Name" VisibleIndex="3" />

                            <dxwgv:GridViewDataColumn Caption="Password" VisibleIndex="4" Name="Password">
                                <DataItemTemplate>
                                    <asp:Label ID="lblPassword" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Password"))%>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblPasswordSalt" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"PasswordSalt"))%>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="hlPassword" runat="server" Text="Password" Visible="false"></asp:LinkButton>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>

                            <dxwgv:GridViewDataColumn FieldName="Mobile" VisibleIndex="5" />
                            <dxwgv:GridViewDataColumn Name="Delete" VisibleIndex="6" Caption="Action" CellStyle-HorizontalAlign="Left">
                                <DataItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" valign="top">
                                                <dxe:ASPxButton ID="btnDteDetail" CommandName="edit" runat="server" Image-Url='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                                    Image-Height="15" Image-Width="15" EnableDefaultAppearance="false" EnableTheming="false"
                                                    ToolTip="Edit" CausesValidation="false" CommandArgument='<%# Eval("ISPID") %>' />
                                                <dxe:ASPxButton ID="btnUnlock" CommandName="Unlock" runat="server" Image-Url='<%#"~/" + strAssets + "/CSS/images/Lock.png"%>'
                                                    Image-Height="15" Image-Width="15" EnableDefaultAppearance="false" EnableTheming="false"
                                                    ToolTip="Unlock User" Visible='<%#Eval("isLockedOut") %>' CausesValidation="false" CommandArgument='<%# Eval("ISPID") %>' />

                                            </td>
                                            <td align="left" valign="top">
                                                <%--<dxe:ASPxButton ID="btnActive" CommandName="activeSymptomCode" runat="server" Image-Height="15"
                                                                        Image-Width="15" EnableDefaultAppearance="false" EnableTheming="false" CausesValidation="false"
                                                                        CommandArgument='<%# Eval("ISPID") %>' />--%>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:Literal ID="lblMapToRetailerDisplay" Visible="false" runat="server" Text='<%# Eval("MapDisplayCount") %>'></asp:Literal>
                                                <asp:Button ID="btnSwitchToRetailer" CssClass="buttonbg" CommandName="ExitISP" Width="100px"
                                                    ToolTip="Switch to Retailer" runat="server" Text="Switch to Retailer" CommandArgument='<%# Eval("switchDisplayCount") %>' />
                                                <asp:Button ID="btnISPExit" CssClass="buttonbg" CommandName="ExitISP" Width="100px"
                                                    ToolTip="Map to Retailer" runat="server" Text="Map to Retailer" CommandArgument='<%# Eval("ISPID") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="Active" VisibleIndex="7" Visible="false">
                                <DataItemTemplate>
                                    <asp:Label ID="lblActive" Visible="true" runat="server" Text='<%# Eval("Active") %>'
                                        CommandArgument='<%# Eval("ISPID") %>'></asp:Label>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Styles>
                            <Header Font-Bold="True" ForeColor="White" HorizontalAlign="Left">
                            </Header>
                            <AlternatingRow BackColor="Beige">
                            </AlternatingRow>
                        </Styles>
                        <Templates>
                            <DetailRow>
                                <dxwgv:ASPxGridView ID="detailGrid" runat="server" KeyFieldName="RetailerISPMappingID"
                                    Width="100%" OnBeforePerformDataSelect="detailGrid_DataSelect" EnableCallBacks="False"
                                    OnHtmlRowPrepared="detailGrid_HtmlRowPrepared" SettingsBehavior-AllowSort="false"
                                    OnCustomButtonInitialize="detailGrid_CustomButtonInitialize" OnCustomButtonCallback="ASPxGridView1_CustomButtonCallback"
                                    Styles-Header-HorizontalAlign="Left" Styles-Header-Font-Bold="true" Styles-Header-ForeColor="White"
                                    Styles-Header-BackColor="#02a234">
                                    <Columns>
                                        <%-- <dxwgv:GridViewDataColumn FieldName="ISPID" Caption="Product Category" VisibleIndex="4"
                                                                Visible="false" />--%>
                                        <dxwgv:GridViewDataColumn FieldName="RetailerName" Caption="Retailer Name" VisibleIndex="1" />
                                        <dxwgv:GridViewDataColumn FieldName="RetailerCode" Caption="Retailer Code" VisibleIndex="2" />
                                        <dxwgv:GridViewDataColumn FieldName="ActivationDate" Caption="ISP Activation Date"
                                            VisibleIndex="3" />
                                        <dxwgv:GridViewDataColumn FieldName="DeActivationDate" Caption="ISP DeActivation Date"
                                            VisibleIndex="4" />
                                        <dxwgv:GridViewDataTextColumn FieldName="RetailerISPMappingID" Name="Action" VisibleIndex="5"
                                            Caption="Action" Visible="true" CellStyle-HorizontalAlign="Center">
                                            <DataItemTemplate>
                                                <dxe:ASPxButton ID="btnActivePC" AutoPostBack="false" Visible="false" CommandName="DeleteDetail"
                                                    runat="server" Image-Url="../../../Assets/Images/active.gif" Image-Height="15"
                                                    Image-Width="15" EnableDefaultAppearance="false" EnableTheming="false" CausesValidation="false">
                                                </dxe:ASPxButton>
                                                <asp:Literal ID="lblDeleteMappingDisplayCount" Visible="false" runat="server" Text='<%# Eval("ActivationDate") %>'></asp:Literal>
                                                <asp:Literal ID="litExitISPDisplay" Visible="false" runat="server" Text='<%# Eval("DeActivationDate") %>'></asp:Literal>
                                                <asp:Literal ID="litPreRetailerISPMappingID" Visible="false" runat="server" Text='<%# Eval("PreviousRetailerISPMappingID") %>'></asp:Literal>
                                                <asp:Button ID="btnExitISP" CssClass="buttonbg" CommandName="ExitISP" Width="100px"
                                                    ToolTip="Exit ISP" runat="server" Text="Exit ISP" CommandArgument='<%# Eval("RetailerISPMappingID") %>' />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewCommandColumn Caption="Delete Mapping" VisibleIndex="5" ButtonType="Image">
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                            <ClearFilterButton Visible="True">
                                            </ClearFilterButton>
                                            <CustomButtons>
                                                <dxwgv:GridViewCommandColumnCustomButton Text="Delete Mapping" ID="ActivationDate" />
                                            </CustomButtons>
                                        </dxwgv:GridViewCommandColumn>
                                    </Columns>
                                    <Settings ShowFooter="True" />
                                    <SettingsDetail ShowDetailRow="False" IsDetailGrid="True" />
                                    <SettingsBehavior AllowFocusedRow="True" ProcessSelectionChangedOnServer="False" />
                                </dxwgv:ASPxGridView>
                            </DetailRow>
                        </Templates>
                        <SettingsBehavior AllowSort="False" /> <%-- #CC09 Added --%>
                        <SettingsDetail ShowDetailRow="true" />
                       
                    </dxwgv:ASPxGridView>
                </div>
                <div class="clear">
                </div>

                <div id="dvFooter" runat="server">
                    <uc2:ucPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                </div>

                <div class="clear">
                </div>
                <div>
                    <asp:HiddenField ID="hdnRetailerID" runat="server" />
                    <asp:HiddenField ID="hdnRetailerName" runat="server" />
                    <asp:HiddenField ID="hdnISPID" runat="server" />
                    <asp:HiddenField ID="hdnUniqueID" runat="server" />
                    <asp:HiddenField ID="hdnIndex" runat="server" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
                <%--#CC03 Add Start--%>
                <asp:PostBackTrigger ControlID="btnSubmit" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <%--#CC03 Add Start--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>

</asp:Content>
