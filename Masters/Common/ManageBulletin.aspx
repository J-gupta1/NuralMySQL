<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageBulletin.aspx.cs" Inherits="BulletinBoard_ManageBulletin" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%-- #CC03:Added Start--%>
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script type="text/javascript" language="javascript">

        function HideModalPopupConfirmation() {
            var modal = $find('ctl00_contentHolderMain_ModelPopJustConfirmation_backgroundElement');
            if (modal == null) {
                return;
            }
            modal.hide();
        }

        function HideModalPopupSubCatConfirmation() {
            // alert('Shashi')

            var modal = $find('ctl00_contentHolderMain_ModalPopupSubCategory_backgroundElement');
            // alert(modal);
            if (modal == null) {
                return;
            }
            modal.hide();
        }


    </script>

    <%-- #CC03:Added Start--%>

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:Label ID="lbltvErrorMsg" runat="server" Visible="false"></asp:Label>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="mainheading">
            Add / Edit Bulletin
        </div>
        <div class="export">
            <asp:LinkButton ID="LBViewBulletin" runat="server" CausesValidation="False" CssClass="elink7"
                OnClick="LBViewBulletin_Click">View List</asp:LinkButton>
        </div>
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory
            </div>
            <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblbulletinSubject" CssClass="formtext" runat="server" AssociatedControlID="txtSubject">Subject:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="formfields" MaxLength="50"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqSubject" runat="server" ControlToValidate="txtSubject"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter bulletiin subject."
                                        SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtSubject"
                                        CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^<>/\@%]{1,50}"
                                        ValidationGroup="AddUserValidationGroup" runat="server" />
                                </div>
                            </li>
                            <%--  #CC03:Added Start--%>
                            <li class="text">
                                <asp:Label ID="lblCategory" runat="server" AssociatedControlID="ddlCategory"
                                    CssClass="formtext">Category:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList CausesValidation="true" ID="ddlCategory" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqCategory" runat="server" ControlToValidate="ddlCategory"
                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select category."
                                        SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                </div>

                            </li>
                            <li class="text">
                                <div>

                                    <asp:ImageButton ID="imgAddCategory" runat="server" CausesValidation="false"
                                        OnClick="imgAddCategory_Click"
                                        ToolTip="Add Category Name" />
                                </div>
                            </li>
                            <li class="field"></li>

                            <%--  #CC03:Added End--%>

                            <li class="text">
                                <asp:Label ID="lblSubCategory" runat="server" AssociatedControlID="ddlSubCategory"
                                    CssClass="formtext">Sub Category:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList CausesValidation="true" ID="ddlSubCategory" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqSubCategory" runat="server" ControlToValidate="ddlSubCategory"
                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sub category."
                                        SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                </div>

                            </li>
                            <%--  #CC03:Added Start--%>
                            <li>
                                <div>

                                    <asp:ImageButton ID="imgAddSubCategory" runat="server" CausesValidation="false"
                                        OnClick="imgAddSubCategory_Click"
                                        ToolTip="Add Sub Category Name" />
                                </div>
                            </li>

                            <%--  #CC03:Added End--%>
                        </ul>
                        <div class="clear"></div>
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblPublishDate" runat="server" AssociatedControlID="txtSubject" CssClass="formtext">Publish Date:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucPublishDate" runat="server" IsRequired="true" ErrorMessage="Publish date required."
                                    ValidationGroup="AddUserValidationGroup" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblExpiryDate" runat="server" AssociatedControlID="txtSubject" CssClass="formtext">Expiry Date:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucExpiryDate" runat="server" IsRequired="true" ErrorMessage="Expiry date required." RangeErrorMessage="Date should be greater then equal to current date."
                                    ValidationGroup="AddUserValidationGroup" />
                            </li>
                        </ul>
                        <div class="clear"></div>
                        <%--#CC01 Add Start--%>
                        <ul id="trBrands" runat="server" visible="false">
                            <li class="text">
                                <asp:Label ID="lblBrandText" runat="server" CssClass="formtext">Brand:<span class="error">*</span></asp:Label>
                            </li>
                            <li style="height: auto">
                                <asp:CheckBoxList ID="chckBrandslist" Width="100%" CssClass="radio-rs" CellPadding="2" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"></asp:CheckBoxList>
                            </li>
                        </ul>
                        <%--#CC01 Add End--%>
                        <div class="clear"></div>
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblAccessType" runat="server" CssClass="formtext">Access Type: <span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:RadioButtonList ID="rblAccessType" CssClass="radio-rs" CellPadding="2" AutoPostBack="true" runat="server" OnSelectedIndexChanged="rblAccessType_SelectedIndexChanged"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Selected="True">Allow To All</asp:ListItem>
                                    <asp:ListItem Value="2">Specific</asp:ListItem>
                                </asp:RadioButtonList>
                            </li>
                            <li class="text">Status:
                            </li>
                            <li class="field">
                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                            </li>
                        </ul>
                        <div class="clear"></div>
                        <asp:Panel ID="tblSpecific" runat="server" Visible="false" BorderWidth="0">
                            <ul>
                                <li class="text"></li>
                                <li class="field2" style="height: auto;">
                                    <div class="grid2" style="border: 1px solid #dddddd; overflow: auto">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="4" class="gridrow">
                                            <tr>
                                                <td align="left" valign="top" width="40%" style="border-right: 1px solid #b9d0dd;"
                                                    class="formtext1">
                                                    <asp:TreeView ID="tvLevel" runat="server" cellpadding="2" cellspacing="0" ShowCheckBoxes="All">
                                                    </asp:TreeView>
                                                </td>
                                                <td align="left" width="60%" valign="top" class="formtext1">
                                                    <asp:TreeView ID="tvSalesChannel" runat="server" cellpadding="2" cellspacing="0" ShowCheckBoxes="All">
                                                    </asp:TreeView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </li>
                            </ul>
                        </asp:Panel>
                        <div class="clear"></div>
                        <ul>
                            <li class="text" style="float: left;">
                                <asp:Label ID="lblDescription" runat="server" CssClass="formtext">Description:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field2" style="height: auto;">
                                <FCKeditorV2:FCKeditor Width="100%" ID="FCKDetails" Height="350" runat="server" BasePath="~/Assets/FCKEditor/">
                                </FCKeditorV2:FCKeditor>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="margin-bottom">
                <div class="float-margin">
                    <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="true" ValidationGroup="AddUserValidationGroup"
                        ToolTip="Add Bulletin" CssClass="buttonbg" OnClick="btnCreate_Click" />
                </div>
                <div class="float-margin">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" ToolTip="Cancel" CssClass="buttonbg"
                        OnClick="btnCancel_Click" />
                </div>
                <div class="clear"></div>
            </div>
        </div>

        <%--  #CC03:Added Start --%>

        <asp:Panel ID="pnlModelSwap" runat="server" CssClass="popupbg" Width="80%">

            <asp:UpdatePanel ID="UpdCategory" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:ucMessage ID="UcMessage1" runat="server" />
                    <div class="mainheading">
                        Add / Edit Manage Category
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Category Name:<span class="error2">*</span>
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="formfields"></asp:TextBox>
                                    <div>
                                        <asp:RequiredFieldValidator runat="server" ID="valcategory" ControlToValidate="txtCategoryName"
                                            CssClass="error" ErrorMessage="Please insert a Category "
                                            ValidationGroup="insert" />
                                    </div>
                                </li>
                                <li class="text">Status:                         
                            <asp:CheckBox ID="chkStatus" runat="server" Checked="true" />
                                </li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSubmit" runat="server" CausesValidation="true"
                                            CssClass="buttonbg" Text="Submit" OnClick="btnSubmit_Click"
                                            ValidationGroup="insert" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnCatCancel" runat="server" CssClass="buttonbg"
                                            Text="Cancel" ToolTip="Cancel" OnClick="btnCatCancel_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnCancelConfirmation" runat="server" CssClass="buttonbg" OnClientClick="HideModalPopupConfirmation()"
                                            Text="Close" CausesValidation="false" />


                                    </div>

                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="mainheading">
                        Search Category
                    </div>
                    <div class="contentbox">
                        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="H25-C3-S">
                                    <ul>
                                        <li class="text">
                                            <asp:Label ID="lblSerCategory" Text="Category Name:" runat="server" />
                                        </li>
                                        <li class="field">
                                            <asp:TextBox ID="txtSerCAT" runat="server" CssClass="formfields"></asp:TextBox>
                                        </li>
                                        <li class="field3">
                                            <div class="float-margin">
                                                <asp:Button ID="btnserchC" Text="Search" runat="server" OnClick="btnserCategory_Click"
                                                    CssClass="buttonbg" />
                                            </div>
                                            <div class="float-margin">
                                                <asp:Button ID="getalldata" Text="View All " runat="server" OnClick="btngetalldata_click"
                                                    CssClass="buttonbg" />
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnserchC" />
                                <asp:PostBackTrigger ControlID="getalldata" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="mainheading">
                        List
                    </div>
                    <div class="export">
                        <asp:Button ID="exporttoexel" Text="" runat="server" OnClick="exporttoexel_Click"
                            CssClass="excel" />
                    </div>
                    <asp:UpdatePanel ID="updgrdView" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="contentbox">
                                <div class="grid1">
                                    <asp:GridView ID="grdCAT" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                                        AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                        DataKeyNames="CategoryID" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow"
                                        RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected"
                                        Width="100%" OnPageIndexChanging="grdCAT_PageIndexChanging" OnRowCommand="grdCAT_RowCommand"
                                        EmptyDataText="No Record" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                        <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                        <Columns>
                                            <asp:BoundField DataField="CategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Category Name"
                                                HtmlEncode="true">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                                <ItemStyle Wrap="False" />
                                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("CategoryID") %>'
                                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                                <ItemStyle Wrap="False" />
                                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false"
                                                        CommandArgument='<%#Eval("CategoryID") %>' CommandName="cmdEdit"
                                                        ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <PagerStyle CssClass="PagerStyle" />
                                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                        <AlternatingRowStyle CssClass="Altrow" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="grdCAT" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="exporttoexel" />
                    <asp:PostBackTrigger ControlID="btnSubmit" />
                    <asp:PostBackTrigger ControlID="btnCancel" />
                </Triggers>
            </asp:UpdatePanel>

        </asp:Panel>

        <cc1:ModalPopupExtender ID="ModelPopJustConfirmation" runat="server" BackgroundCssClass="modalBackground"
            Drag="True" OnCancelScript="HideModalPopupConfirmation()" CancelControlID="btnCancelConfirmation"
            PopupControlID="pnlModelSwap" TargetControlID="btnTarget1" DynamicServicePath=""
            Enabled="True">
        </cc1:ModalPopupExtender>
        <asp:LinkButton ID="btnTarget1" runat="server"></asp:LinkButton>

        <asp:Panel ID="pnlSubCategory" runat="server" CssClass="popupbg" Width="80%">

            <asp:UpdatePanel ID="UpdSubCategory" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <uc1:ucMessage ID="UcMessage2" runat="server" />

                    <div class="mainheading">
                        Add / Edit Manage Sub Category
                    </div>

                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">
                                    <asp:Label ID="lblcategoryname" runat="server" Text="">Category Name: <span class="error">*</span></asp:Label>
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:DropDownList ID="cmbSelectCat" runat="server" CssClass="formselect" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator runat="server" ID="valCat" ControlToValidate="cmbSelectCat"
                                            InitialValue="0" CssClass="error" ErrorMessage="Please select a Category " ValidationGroup="SubCatinsert" />
                                    </div>
                                </li>
                                <li class="text">
                                    <asp:Label ID="lblSubCatname" runat="server" Text="">Sub Category Name:<span class="error">*</span></asp:Label>
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtInsertSubCat" runat="server" CssClass="formfields" ValidationGroup="SubCatinsert"></asp:TextBox>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator runat="server" ID="valsubname" ControlToValidate="txtInsertSubCat"
                                            ErrorMessage="Please insert a sub category" ValidationGroup="SubCatinsert" />
                                    </div>
                                </li>
                            </ul>
                            <ul>
                                <li class="text">Status: <span class="error">&nbsp;</span>

                                    <asp:CheckBox ID="subchkbox" Text="Active" runat="server" Checked="true" />
                                </li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSubCatSubmit" Text="Submit" runat="server" ValidationGroup="SubCatinsert"
                                            CssClass="buttonbg" CausesValidation="true" OnClick="btnSubCatSubmit_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnSubCatCancel" Text="Cancel" runat="server" CssClass="buttonbg" OnClick="btnSubCatCancel_Click" />
                                    </div>


                                    <div class="float-margin">
                                        <asp:Button ID="btnSubCatCancelConfirm" runat="server" CssClass="buttonbg" OnClientClick="HideModalPopupSubCatConfirmation()"
                                            Text="Close" CausesValidation="false" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>

                    <div class="mainheading">
                        Search Sub Category
                    </div>
                    <div class="contentbox">
                        <asp:UpdatePanel ID="UpdSubCatSearch" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="H25-C3-S">
                                    <ul>
                                        <li class="text">
                                            <asp:Label ID="lblSerCat" Text="Category Name:" runat="server" />
                                        </li>
                                        <li class="field">
                                            <asp:DropDownList ID="cmbSerCat" runat="server" CssClass="formselect">
                                            </asp:DropDownList>
                                        </li>
                                        <li class="text">
                                            <asp:Label ID="lblSerSubCategory" Text="Sub Category Name:" runat="server" />
                                        </li>
                                        <li class="field">
                                            <asp:TextBox ID="txtSerSubCat" runat="server" CssClass="formfields"></asp:TextBox>
                                        </li>
                                        <li class="field3">
                                            <div class="float-margin">
                                                <asp:Button ID="btnSerchSubCatD" Text="Search" runat="server" CssClass="buttonbg" OnClick="btnSerchSubCatD_Click1" />
                                            </div>
                                            <div class="float-margin">
                                                <asp:Button ID="getallSubCatdata" Text="View All" runat="server" CssClass="buttonbg" OnClick="getallSubCatdata_Click" />
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSerchSubCatD" />
                                <asp:PostBackTrigger ControlID="getallSubCatdata" />

                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="mainheading">
                        List
                    </div>
                    <div class="export">
                        <asp:Button ID="Subexporttoexel" Text="" runat="server" OnClick="Subexporttoexel_Click"
                            CssClass="excel" CausesValidation="False" />
                    </div>
                    <asp:UpdatePanel ID="updgrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="contentbox">
                                <div class="grid1">
                                    <asp:GridView ID="grdSubCat" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="SubCategoryID"
                                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="None"
                                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                        RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                                        RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                        OnRowCommand="grdSubCat_RowCommand" OnPageIndexChanging="grdSubCat_PageIndexChanging"
                                        EmptyDataText="No Record">
                                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                        <Columns>
                                            <asp:BoundField DataField="CategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Category Name "
                                                HtmlEncode="true">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubCategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sub Category Name"
                                                HtmlEncode="true">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                                <ItemStyle Wrap="False" />
                                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SubCategoryID") %>'
                                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                                <ItemStyle Wrap="False" />
                                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SubCategoryID") %>'
                                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                                        <PagerStyle CssClass="PagerStyle" />
                                        <AlternatingRowStyle CssClass="Altrow" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="grdSubCat" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="Subexporttoexel" />
                    <asp:PostBackTrigger ControlID="btnSubCatSubmit" />
                    <asp:PostBackTrigger ControlID="btnSubCatCancel" />

                </Triggers>
            </asp:UpdatePanel>

        </asp:Panel>



        <cc1:ModalPopupExtender ID="ModalPopupSubCategory" runat="server" BackgroundCssClass="modalBackground"
            Drag="True" OnCancelScript="HideModalPopupSubCatConfirmation()" CancelControlID="btnSubCatCancelConfirm"
            PopupControlID="pnlSubCategory" TargetControlID="LinkButton2" DynamicServicePath=""
            Enabled="True">
        </cc1:ModalPopupExtender>
        <asp:LinkButton ID="LinkButton2" runat="server"></asp:LinkButton>

        <%--  #CC03:Added End --%>
    </div>
</asp:Content>
