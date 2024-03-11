<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageSchemeV2.aspx.cs" Inherits="Masters_Common_ManageSchemeV2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallback" TagPrefix="dx" %>
<%@ Register Src="~/UserControls/ucPagingControl.ascx" TagName="ucPagingControl"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function OnCheckBoxCheckChanged(evt) {

            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }
        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }
        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;
            if (parentNodeTable) {
                var checkUncheckSwitch;
                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }
                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }
        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

    </script>

   <%-- <style type="text/css">
        .form_select {
            height: 22px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:Label ID="lbltvErrorMsg" runat="server" Visible="false"></asp:Label>
        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uc2:ucMessage ID="uclblMessage" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="mainheading">
            Select
            <asp:Label ID="lblpage" runat="server"></asp:Label>
        </div>
        <div class="export">
            <asp:LinkButton ID="LBViewScheme" runat="server" CausesValidation="False" OnClick="LBViewScheme_Click"
                CssClass="elink7">View Scheme</asp:LinkButton>
        </div>
        <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C3">
                        <ul>
                            <li class="text">
                                <asp:Label ID="Label8" runat="server" Text="Scheme Name:"></asp:Label> <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtScheme" CssClass="formfields" MaxLength="100" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequValidaSchemeName" runat="server" ControlToValidate="txtScheme"
                                    CssClass="error" ErrorMessage="Please enter scheme name" ValidationGroup="SchemeEntry"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">
                                <asp:Label ID="Label1" runat="server" Text="">Scheme Component Type: <span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="cmbComponentType" CssClass="formselect" runat="server" OnSelectedIndexChanged="cmbComponentType_SelectedIndexChanged1"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbComponentType"
                                        CssClass="error" ErrorMessage="Please select a Scheme Component " InitialValue="0"
                                        ValidationGroup="insert" />
                                </div>
                            </li>
                            <li class="text">
                                <asp:Label ID="Label3" runat="server" Text="">PayOut Type: <span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="cmbPayoutBase" CssClass="formselect" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="cmbPayoutBase"
                                        CssClass="error" ErrorMessage="Please select a payout type " InitialValue="0"
                                        ValidationGroup="insert" />
                                </div>
                                <asp:Label ID="lblchk1" runat="server" Text="0" Visible="false"></asp:Label>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updSeprate" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="contentbox">
                    <div class="H25-C3-S">
                        <ul>
                            <asp:Panel ID="pnlTargetdr" runat="server" Visible="false">
                                <li class="text">
                                    <asp:Label ID="Label2" runat="server" Text="">FortNight: <span class="error">*</span></asp:Label>
                                </li>
                                <li class="text-field" style="height: auto">
                                    <div style="height: 155px; overflow: auto;">
                                        <asp:CheckBoxList ID="chkLst" runat="server" CellPadding="2" />
                                    </div>
                                </li>
                            </asp:Panel>
                            <asp:Panel ID="pnldaterng" runat="server" Visible="false">
                                <li class="field"></li>
                                <li class="text">
                                    <asp:Label ID="lblpreffdt" runat="server" Text="">Scheme Start Date: <span class="error">*</span> </asp:Label>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucFromDate" runat="server" RangeErrorMessage="Date should be greater then or equal to current date."
                                        ErrorMessage="Please select a date" ValidationGroup="insert" />
                                </li>
                                <li class="text">
                                    <asp:Label ID="Label4" runat="server" Text="">Scheme End Date: <span class="error">*</span> </asp:Label>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucToDate" runat="server" RangeErrorMessage="Invalid date."
                                        ErrorMessage="Please select a date" ValidationGroup="insert" />
                                </li>
                            </asp:Panel>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updTree" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <%--      <asp:Panel ID="tblSpecific" runat="server" BorderWidth="0" Visible="false">--%>

                <div id="pnlIsUserWise" runat="server" borderwidth="0" visible="false">
                    <div class="contentbox">
                        <asp:LinkButton ID="lbactive" runat="server" CausesValidation="False" OnClick="selectusers_click"
                            CssClass="elink7">Select Users</asp:LinkButton>
                    </div>
                </div>
                <div id="tblSpecific" runat="server" borderwidth="0" visible="false">
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">
                                    <asp:Label ID="Label9" runat="server" Text="Select Channel Type:"></asp:Label>
                                    <span class="error">*</span>
                                </li>
                                <li class="field">
                                    <asp:CheckBoxList ID="chckUserType" runat="server" CellPadding="2" class="formtext2" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                </li>
                            </ul>
                            <ul>
                                <li class="text">
                                    <asp:Label ID="Label6" runat="server" Text="Select Excel:"></asp:Label>
                                    <span class="error">*</span>
                                </li>
                                <li class="field">
                                    <asp:FileUpload ID="flupldSalesChannel" CssClass="fileuploads" runat="server" />
                                </li>
                                <li class="field3">
                                <li class="text">
                                    <asp:Button ID="btnUploadSalesChannel" CssClass="buttonbg" runat="server" Text="Upload Sales Channel" OnClick="btnUploadSalesChannel_Click" />
                                </li>
                            </ul>
                        </div>
                        <div class="formlink">
                            <ul>
                                <li>
                                    <asp:LinkButton ID="lnkDownloadSalesChannelTemplate" runat="server" CssClass="elink2" OnClick="lnkDownloadSalesChannelTemplate_Click">Download User Template</asp:LinkButton>

                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkDownloadSalesChannelDetals" runat="server" CssClass="elink2" OnClick="lnkDownloadSalesChannelDetals_Click">Download Sales Channel Detail</asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                        <div class="grid1">
                            <asp:GridView ID="grdUser" runat="server" AlternatingRowStyle-CssClass="Altrow"
                                bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                SelectedStyle-CssClass="gridselected" Width="100%">
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <%--  </asp:Panel>--%>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkDownloadSalesChannelTemplate" />
                <asp:PostBackTrigger ControlID="lnkDownloadSalesChannelDetals" />
                <asp:PostBackTrigger ControlID="btnUploadSalesChannel" />
                <asp:PostBackTrigger ControlID="cmbComponentType" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updExclude" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div id="pnlexclBtn" runat="server" visible="false">
                    <div class="contentbox">
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClick="excludeModels_click"
                            CssClass="elink7">Models To Be Excluded</asp:LinkButton>
                    </div>
                </div>
                <div id="pnlExclude" runat="server" visible="false">
                    <%--<div id="pnlExclude" runat="server" style="display: none">--%>
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">
                                    <asp:Label ID="Label5" runat="server" Text="Select Excel:"></asp:Label><span class="error">*</span>
                                </li>
                                <li class="field">
                                    <asp:FileUpload ID="FlupldModel" CssClass="fileuploads" runat="server" />
                                </li>
                                <li class="field3">
                                    <asp:Button ID="BtnUploadModel" CssClass="buttonbg" runat="server" Text="Upload Model" OnClick="BtnUploadModel_Click" />
                                </li>
                                <li class="link">
                                    <asp:LinkButton ID="lnkModelDetailtemplate" runat="server" CssClass="elink2" OnClick="lnkModelDetailtemplate_Click">Download Model Template</asp:LinkButton>
                                </li>
                                <li class="link">
                                    <asp:LinkButton ID="LnkModelDetail" runat="server" CssClass="elink2" OnClick="LnkModelDetail_Click">Download Model Detail</asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                        <div class="grid1">
                            <asp:GridView ID="grdModelsExcluded" runat="server" AlternatingRowStyle-CssClass="Altrow"
                                bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                SelectedStyle-CssClass="gridselected" Width="100%">
                            </asp:GridView>

                        </div>
                    </div>

                    <%-- <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="left" class="tableposition" width="90%">
                                                                    <div class="mainheading" style="margin-top: 10px;">
                                                                        &nbsp;Excluded Models
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div class="contentbox">
                                                                        <div class="grid2">
                                                                            <asp:GridView ID="grdExcluded" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                                                                                AutoGenerateColumns="False" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                                                                DataKeyNames="ModelId" EmptyDataText="No record found" FooterStyle-CssClass="gridfooter"
                                                                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="None"
                                                                                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                                                                OnPageIndexChanging="grdExclude_PageIndexChanging" PageSize="3" RowStyle-CssClass="gridrow"
                                                                                RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected"
                                                                                Width="100%">
                                                                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="SKU Id" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("ModelId") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Refrenced Code">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("ModelName") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Select">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="oncheckedchange" AutoPostBack="true" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <PagerStyle CssClass="PagerStyle" />
                                                                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                <AlternatingRowStyle CssClass="Altrow" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                        <div style="clear: both;">
                                                                            <div style="float: right; margin-right: 3px; margin-top: 10px">
                                                                                <asp:Button ID="BtnExclude" runat="server" CssClass="buttonbg" OnClick="btnExclude_click"
                                                                                    Text="Exclude" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>--%>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="LnkModelDetail" />
                <asp:PostBackTrigger ControlID="lnkModelDetailtemplate" />
                <asp:PostBackTrigger ControlID="BtnUploadModel" />
                <asp:PostBackTrigger ControlID="cmbComponentType" />
            </Triggers>
        </asp:UpdatePanel>
        <div id="dvUpload" runat="server" visible="false">
            <div class="contentbox">
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label7" runat="server" Text="Select Excel:"></asp:Label><span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:FileUpload ID="flupdScheme" CssClass="fileuploads" runat="server" />
                        </li>
                        <li class="field3">
                            <asp:Button ID="BtnUpload" CssClass="buttonbg" runat="server" Text="Upload" ValidationGroup="UploadGroup"
                                OnClick="BtnUpload_Click" />
                        </li>
                        <li class="link"><%--<a class="elink2" id="A1" runat="server" href="../../Excel/Templates/InvoiceValueBaced.xlsx">Download InvoiceValue Based Template </a> #CC01 Commented --%>
                            <%--#CC01 Add Start --%>
                            <a class="elink2" id="hrefInvoiceBasedTemplate" runat="server" href="../../Excel/Templates/InvoiceValueBased.xlsx">Download Template </a></li>
                        <li class="link">
                            <a class="elink2" id="hrefPointsOrRupeesTemplate" visible="false" runat="server" href="../../Excel/Templates/PointsOrRupeesBased.xlsx">Download Points / Rupees Based Template </a>
                            <%--#CC01 Add End --%>

                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updscheme" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlgrid" runat="server" Visible="false">
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="GrdScheme" runat="server" AlternatingRowStyle-CssClass="Altrow"
                                bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                SelectedStyle-CssClass="gridselected" Width="100%">
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="margin-bottom">
                        <div class="float-margin">
                            <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="UploadGroup"
                                OnClick="btnSave_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnReset_Click"
                                CssClass="buttonbg" Text="Reset" />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <%--#CC01 Add Start--%>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
            <%--#CC01 Add End--%>
        </asp:UpdatePanel>
    </div>
</asp:Content>
