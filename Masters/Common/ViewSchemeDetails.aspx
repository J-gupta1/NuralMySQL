<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewSchemeDetails.aspx.cs" Inherits="Masters_Common_ViewSchemeDetails" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
//<link rel="stylesheet" id="StyleCss" runat="server"  type="text/css"/>
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
                
            } else {
                document.getElementById('<%=lbltvErrorMsg.ClientID %>').style.display = "";
            }
            return isChecked;
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 122px;
        }
        .style2
        {
            width: 134px;
        }
        .form_file
        {
            margin-left: 44px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:Label ID="lbltvErrorMsg" runat="server" Visible="false"></asp:Label>
    <table cellspacing="0" cellpadding="0" width="965" border="0">
        <tr>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <uc1:ucMessage ID="uclblMessage" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="left" valign="top" class="tableposition">
                            <div class="mainheading">
                                 Details
                                <asp:Label ID="lblpage" runat="server"></asp:Label></div>
                        </td>
                        <td width="5">
                        </td>
                        <td align="left" style="padding-right: 5px;">
                            <asp:LinkButton ID="LBViewScheme" runat="server" CausesValidation="False" OnClick="LBViewScheme_Click"
                                CssClass="elink7">View Scheme</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div class="contentbox">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <asp:UpdatePanel ID="updmain" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>
                        <tr>
                                                                        <td align="right" width="90" valign="top" class="formtext">
                                                                            <asp:Label ID="lblSchemeName" runat="server" Text="Scheme Name:"></asp:Label>&nbsp;
                                                                        </td>
                                                                        <td width="200" align="left" valign="top">
                                                                            <asp:TextBox ID="txtScheme" CssClass="form_input2" MaxLength="100" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="right" width="80" height="35">
                                                                            <asp:Label ID="Label1" runat="server" Text="">Scheme Component Type:<font class="error">*</font></asp:Label>
                                                                        </td>
                                                                        <td width="25%" align="left" valign="top">
                                                                            <div style="float: left; width: 135px;">
                                                                                <asp:DropDownList ID="cmbComponentType" CssClass="form_select" runat="server" OnSelectedIndexChanged="cmbComponentType_SelectedIndexChanged1"
                                                                                    AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                            </div>
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="right" width="80" height="35">
                                                                            <asp:Label ID="Label3" runat="server" Text="">PayOut Type:<font class="error">*</font></asp:Label>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td align="left" valign="top" width="20%">
                                                                            <div style="float: left; width: 135px;">
                                                                                <asp:DropDownList ID="cmbPayoutBase" CssClass="form_select" runat="server">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                        <tr>
                                                            <asp:Panel ID="pnlTargetdr" runat="server" Visible="false">
                                                                <td class="formtext" valign="top" align="right" width="10%" height="35">
                                                                    <asp:Label ID="Label2" runat="server" Text="">FortNight:<font class="error">*</font></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <div style="height: 70px; overflow: auto;">
                                                                        <asp:CheckBoxList ID="chkLst" runat="server" />
                                                                    </div>
                                                                </td>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnldaterng" runat="server" Visible="false">
                                                                <td class="formtext" valign="top" align="right">
                                                                    <asp:Label ID="lblpreffdt" runat="server" Text="">Scheme Start Date:<font class="error">*</font>
                                                                    </asp:Label>
                                                                </td>
                                                                <td valign="top" align="left">
                                                                    <uc1:ucDatePicker ID="ucFromDate" runat="server" RangeErrorMessage="Invalid date."
                                                                        ErrorMessage="Please select a date" ValidationGroup="insert" />
                                                                </td>
                                                                <td class="formtext" valign="top" align="right">
                                                                    <asp:Label ID="Label4" runat="server" Text="">Scheme End Date:<font class="error">*</font>
                                                                    </asp:Label>
                                                                </td>
                                                                <td valign="top" align="left">
                                                                    <uc1:ucDatePicker ID="ucToDate" runat="server" RangeErrorMessage="Invalid date."
                                                                        ErrorMessage="Please select a date" ValidationGroup="insert" />
                                                                </td>
                                                            </asp:Panel>
                                                        </tr>
                         </ContentTemplate>
                         </asp:UpdatePanel>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="965" border="0">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlUpload" runat="server" Visible="false">
                                <table cellspacing="0" cellpadding="0" width="965" border="0">
                                    <tr>
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading">
                                                Upload</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="contentbox">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 102px">
                                                    <tr>
                                                        <td align="right" valign="middle" class="style2" colspan="4">
                                                            <asp:Label ID="Label7" runat="server" Text="Select Excel:"></asp:Label><font class="error">
                                                                *</font>
                                                        </td>
                                                        <td width="260" align="left" valign="middle" class="style1">
                                                            <asp:FileUpload ID="flupdScheme" CssClass="form_file" runat="server" />
                                                        </td>
                                                        <td width="60" align="left" valign="middle" class="style1">
                                                            <asp:Button ID="BtnUpload" CssClass="buttonbg" runat="server" Text="Upload" ValidationGroup="UploadGroup"
                                                                OnClick="BtnUpload_Click" />
                                                            <table border="0" cellpadding="0" cellspacing="0" width="965">
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top" colspan="6">
                                                            &nbsp; <a id="hlkSKU" runat="server" class="elink2" href="../../Excel/Templates/SKUWiseScheme.xlsx">
                                                                Download SKUWise Scheme Template </a>&nbsp;&nbsp; <a class="elink2" runat="server"
                                                                    id="hlkBrand" href="../../Excel/Templates/BrandWiseScheme.xlsx">Download BrandWise
                                                                    Scheme Template </a>&nbsp;&nbsp;
                                                            <asp:LinkButton ID="LnkDownloadRefCode" runat="server" CssClass="elink2" OnClick="LnkDownloadRefCode_Click">Download Reference Code</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:Panel ID="pnlTree" runat="server">
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="90%" align="left" class="tableposition">
                                <div class="mainheading">
                                    Select</div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="5" style="border: 1px solid #b9d0dd;">
                    <div style="float: left; width: 800px; height: 150px; overflow-y: scroll; overflow-x: hidden">
                        <div class="contentbox">
                            <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left" class="formtext1" style="border-right: 1px solid #b9d0dd;" valign="top"
                                        width="40%">
                                        <asp:TreeView ID="tvLevel" runat="server" cellpadding="0" cellspacing="0" ShowCheckBoxes="All">
                                        </asp:TreeView>
                                    </td>
                                    <td align="left" class="formtext1" valign="top" width="60%">
                                        <asp:TreeView ID="tvSalesChannel" runat="server" cellpadding="0" cellspacing="0"
                                            ShowCheckBoxes="All">
                                        </asp:TreeView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <%-- <asp:Button ID="btnTree" CssClass="buttonbg" runat="server" Text="Update" 
                                 OnClick="btnSaveTree_Click" Visible="true"/>--%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnTree" CssClass="buttonbg" runat="server" Text="Update" OnClick="btnSaveTree_Click"
                        Visible="true" />
                </td>
            </tr>
        </asp:Panel>
        <tr>
            <td>
                <asp:Panel ID="pnlGrid" runat="server">
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading">
                                                List</div>
                                        </td>
                                        <td width="10%" align="right">
                                            <asp:Button ID="exportToExel" Text=" " runat="server" OnClick="exportToExel_Click"
                                                CssClass="excel" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <tr>
                                    <td>
                                        <div class="contentbox">
                                            <div class="grid1">
                                                <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="False" AlternatingRowStyle-CssClass="gridrow1"
                                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                                    RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                                    SelectedStyle-CssClass="gridselected" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="SKUCode" HeaderText="SKU Code" />
                                                        <asp:BoundField DataField="BrandCode" HeaderText="Brand Code" />
                                                        <asp:BoundField DataField="MinSlab" DataFormatString="{0:N}" HeaderText="Min Slab" />
                                                        <asp:BoundField DataField="MaxSlab" DataFormatString="{0:N}" HeaderText="Max Slab" />
                                                        <asp:BoundField DataField="RewardedQuantity" DataFormatString="{0:N}" HeaderText="RewardedQuantity" />
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                                            HeaderText="Error" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="exportToExel" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="UploadGroup"
                    OnClick="btnSave_Click" />&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
