<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageScheme.aspx.cs" Inherits="Masters_Common_ManageScheme" %>

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
        function validateTreeView() {
            var isChecked = false;
            var tvNodes = document.getElementById('<%=tvLevel.ClientID %>');
            var chBoxes = tvNodes.getElementsByTagName("input");
            for (var i = 0; i < chBoxes.length; i++) {
                var chk = chBoxes[i];
                if (chk.type == "checkbox" && chk.checked == true) {
                    isChecked = true;
                }
            }
            if (isChecked == true) {
                document.getElementById('<%=lbltvErrorMsg.ClientID %>').style.display = "none";
                $("#trContent").hide();
                document.getElementById("tdContentImg").innerHTML = "<img src='Images/loading.gif' />";
                //$("#tdContentImg").append('<td align="center"><img src='Images/loading.gif' /></td>');
            } else {
                document.getElementById('<%=lbltvErrorMsg.ClientID %>').style.display = "";
            }
            return isChecked;
        }
    </script>

    <style type="text/css">
        .form_select
        {
            height: 22px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <asp:Label ID="lbltvErrorMsg" runat="server" Visible="false"></asp:Label>
        <table cellspacing="0" cellpadding="0" width="965" border="0">
            <tr>
                <td align="left" valign="top" height="420">
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <td align="left" valign="top">
                                        <uc2:ucMessage ID="uclblMessage" runat="server" />
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td width="90%" align="left" valign="top" class="tableposition">
                                            <div class="mainheading">
                                                Select
                                                <asp:Label ID="lblpage" runat="server"></asp:Label></div>
                                        </td>
                                        <td width="10%" align="right" style="padding-right: 10px;">
                                            <asp:LinkButton ID="LBViewScheme" runat="server" CausesValidation="False" OnClick="LBViewScheme_Click"
                                                CssClass="elink7">View Scheme</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="contentbox">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="6" height="20" class="mandatory" valign="top">
                                                        (<font class="error">*</font>) marked fields are mandatory.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="top" class="formtext" width="10%">
                                                        <asp:Label ID="Label8" runat="server" Text="Scheme Name:"></asp:Label><font class="error">*</font>
                                                    </td>
                                                    <td align="left" valign="top" width="20%">
                                                        <asp:TextBox ID="txtScheme" CssClass="form_input2" MaxLength="100" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequValidaSchemeName" runat="server" ControlToValidate="txtScheme"
                                                            CssClass="error" ErrorMessage="Please enter scheme name" ValidationGroup="SchemeEntry"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td class="formtext" valign="top" align="right" height="35" width="15%">
                                                        <asp:Label ID="Label1" runat="server" Text="">Scheme Component Type:<font class="error">*</font></asp:Label>
                                                    </td>
                                                    <td align="left" valign="top" width="20%">
                                                        <div style="float: left; width: 135px;">
                                                            <asp:DropDownList ID="cmbComponentType" CssClass="form_select" runat="server" OnSelectedIndexChanged="cmbComponentType_SelectedIndexChanged1"
                                                                AutoPostBack="true">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div style="float: left; width: 140px;">
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbComponentType"
                                                                CssClass="error" ErrorMessage="Please select a Scheme Component " InitialValue="0"
                                                                ValidationGroup="insert" /></div>
                                                    </td>
                                                    <td class="formtext" valign="top" align="right" height="35" width="10%">
                                                        <asp:Label ID="Label3" runat="server" Text="">PayOut Type:<font class="error">*</font></asp:Label>
                                                    </td>
                                                    <td align="left" valign="top" width="20%">
                                                        <div style="float: left; width: 135px;">
                                                            <asp:DropDownList ID="cmbPayoutBase" CssClass="form_select" runat="server">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div style="float: left; width: 140px;">
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="cmbPayoutBase"
                                                                CssClass="error" ErrorMessage="Please select a payout type " InitialValue="0"
                                                                ValidationGroup="insert" /></div>
                                                        <asp:Label ID="lblchk1" runat="server" Text="0" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updSeprate" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="contentbox">
                                            <table cellspacing="0" cellpadding="0" width="965" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlTargetdr" runat="server" Visible="false">
                                                            <table cellspacing="0" cellpadding="0" width="50%" border="0">
                                                                <tr>
                                                                    <td class="formtext" valign="top" align="right" width="10%" height="35">
                                                                        <asp:Label ID="Label2" runat="server" Text="">FortNight:<font class="error">*</font></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <div style="height: 70px; overflow: auto;">
                                                                            <asp:CheckBoxList ID="chkLst" runat="server" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnldaterng" runat="server" Visible="false">
                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td class="formtext" valign="top" align="right" width="15%">
                                                                        <asp:Label ID="lblpreffdt" runat="server" Text="">Scheme Start Date:<font class="error">*</font> </asp:Label>
                                                                    </td>
                                                                    <td valign="top" align="left" width="20%">
                                                                        <uc1:ucDatePicker ID="ucFromDate" runat="server" RangeErrorMessage="Invalid date."
                                                                            ErrorMessage="Please select a date" ValidationGroup="insert" />
                                                                    </td>
                                                                    <td class="formtext" valign="top" align="right" width="15%">
                                                                        <asp:Label ID="Label4" runat="server" Text="">Scheme End Date:<font class="error">*</font> </asp:Label>
                                                                    </td>
                                                                    <td valign="top" align="left" width="20%">
                                                                        <uc1:ucDatePicker ID="ucToDate" runat="server" RangeErrorMessage="Invalid date."
                                                                            ErrorMessage="Please select a date" ValidationGroup="insert" />
                                                                    </td>
                                                                    <td width="30%">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="updExclude" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <asp:Panel ID="pnlexclBtn" runat="server" Visible="false">
                                                <tr>
                                                    <td width="20%" align="left" style="padding-right: 10px;">
                                                        <div class="contentbox">
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClick="excludeModels_click"
                                                                CssClass="elink7">Select Models To Be Excluded</asp:LinkButton>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlExclude" runat="server" Visible="false">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="left" class="tableposition" width="90%">
                                                                    <div class="mainheading" style="margin-top: 10px;">
                                                                        &nbsp;Excluded Models</div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div class="contentbox">
                                                                        <div class="grid2">
                                                                            <asp:GridView ID="grdExcluded" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="gridrow1"
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
                                                                                <AlternatingRowStyle CssClass="gridrow1" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                        <div style="clear: both;">
                                                                            <div style="float: right; margin-right: 3px; margin-top: 10px">
                                                                                <asp:Button ID="BtnExclude" runat="server" CssClass="buttonbg" OnClick="btnExclude_click"
                                                                                    Text="Exclude" /></div>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updIsUserWise" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlIsUserWise" runat="server" BorderWidth="0" Visible="false">
                                            <div class="contentbox" style="margin-top: 10px;">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td width="20%" align="left" style="padding-right: 10px;">
                                                            <asp:LinkButton ID="lbactive" runat="server" CausesValidation="False" OnClick="selectusers_click"
                                                                CssClass="elink7">Select Users</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updTree" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="tblSpecific" runat="server" BorderWidth="0" Visible="false">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td style="border: 1px solid #b9d0dd;">
                                                        <div style="float: left; width: 800px; height: 150px; overflow-y: scroll; overflow-x: hidden">
                                                            <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td align="left" class="formtext1" valign="top" width="60%">
                                                                        <asp:TreeView ID="tvSalesChannel" runat="server" cellpadding="0" cellspacing="0"
                                                                            ShowCheckBoxes="All">
                                                                        </asp:TreeView>
                                                                    </td>
                                                                    <td visible="false" align="left" class="formtext1" style="border-right: 1px solid #b9d0dd;"
                                                                        valign="top" width="40%">
                                                                        <asp:TreeView ID="tvLevel" runat="server" cellpadding="0" Visible="false" cellspacing="0"
                                                                            ShowCheckBoxes="All">
                                                                        </asp:TreeView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <div class="contentbox">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="10%" align="right" valign="top" height="30">
                                                <asp:Label ID="Label7" runat="server" Text="Select Excel:"></asp:Label><font class="error">*</font>
                                            </td>
                                            <td width="30%" align="left" valign="top">
                                                <asp:FileUpload ID="flupdScheme" CssClass="form_file" runat="server" />
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:Button ID="BtnUpload" CssClass="buttonbg" runat="server" Text="Upload" ValidationGroup="UploadGroup"
                                                    OnClick="BtnUpload_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" height="20">
                                                <asp:UpdatePanel ID="updTemplates" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <%-- <tr>
                                        
                                           <asp:Panel ID= "pnlRefrenceTemplate" runat = "server"  Visible ="false">
                                              <td align="left" valign="top" colspan="6">
                                                &nbsp; <a class="elink2" id="temItemWise" runat="server" href="../../Excel/Templates/SchemeItemWise.xlsx">
                                                    Download InvoiceValue Based Template </a>&nbsp;&nbsp; <a class="elink2" runat="server"
                                                        id="tmpRangeWise" href="../../Excel/Templates/SchemeRangeWise.xlsx">Download SlabBased
                                                        Scheme Template </a>&nbsp;&nbsp;
                                                <asp:LinkButton ID="LnkDownloadRefCode" runat="server" CssClass="elink2" OnClick="LnkDownloadRefCode_Click">Download Reference Code</asp:LinkButton>
                                            </td>
                                             </asp:Panel> 
                                          </tr>--%>
                                                            <tr>
                                                                <td align="left" valign="top" colspan="6">
                                                                    &nbsp;
                                                                    <%-- <a class="elink2" runat="server"
                                                        id="A2" href="../../Excel/Templates/TargetWiseRangeValue.xlsx">Download Slab Based
                                                        Scheme Template </a>--%>&nbsp;&nbsp; <a class="elink2" id="A1" runat="server" href="../../Excel/Templates/InvoiceValueBaced.xlsx">
                                                            Download InvoiceValue Based Template </a>&nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <asp:UpdatePanel ID="updscheme" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlgrid" runat="server" Visible="false">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <div class="contentbox">
                                                            <div class="grid2">
                                                                <asp:GridView ID="GrdScheme" runat="server" AlternatingRowStyle-CssClass="gridrow1"
                                                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                                                    RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                                                    SelectedStyle-CssClass="gridselected" Width="100%">
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" height="5">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="UploadGroup"
                                                            OnClick="btnSave_Click" />&nbsp;
                                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnReset_Click"
                                                            CssClass="buttonbg" Text="Reset" />&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" height="10">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
