<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="~/Masters/Common/ManageMarketing.aspx.cs" Inherits="ManageMarketing" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%--<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>--%>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagPrefix="TA" TagName="ucTextboxMultiline" %>
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:Label ID="lbltvErrorMsg" runat="server" Visible="false"></asp:Label>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit  
                </div>
                <%--<div class="export">
                    <asp:LinkButton ID="LBViewBulletin" runat="server" CausesValidation="False" CssClass="elink7"
                        OnClick="LBViewBulletin_Click">View List</asp:LinkButton>
                </div>--%>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H35-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="Label2" runat="server" AssociatedControlID="ddlCategoryFor"
                                    CssClass="formtext">Category For:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:UpdatePanel ID="UpdatePanelddlcf" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <asp:DropDownList AutoPostBack="true" CssClass="formselect" ID="ddlCategoryFor" runat="server"
                                                OnSelectedIndexChanged="ddlCategoryFor_SelectedIndexChanged">
                                                <asp:ListItem Text="select" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Feedback" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Marketing" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Branding" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>


                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="AddUserValidationGroup" runat="server"
                                            ControlToValidate="ddlCategoryFor" ErrorMessage="Please select CategoryFor." InitialValue="-1"
                                            CssClass="error" Display="Dynamic" ForeColor=""></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCategoryFor" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </li>


                            <li class="text">
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlCategory"
                                    CssClass="formtext">Parent Category :</asp:Label>
                            </li>
                            <li class="field">
                                <asp:UpdatePanel ID="UpddlCategory" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <asp:DropDownList AutoPostBack="true" CssClass="formselect" ID="ddlCategory" runat="server"
                                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                        <div>
                                            <asp:TextBox ID="txtCategory" runat="server" Visible="false" CssClass="formfields" placeholder="CategoryName"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredCategory" ValidationGroup="AddUserValidationGroup" runat="server"
                                            ControlToValidate="ddlCategory" ErrorMessage="Please select Category." InitialValue="-1"
                                            CssClass="error" Display="Dynamic" ForeColor=""></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblSubCategory" runat="server" AssociatedControlID="ddlSubCategory" CssClass="formtext">Category:</asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <div>

                                                <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                            <div>
                                                <asp:TextBox ID="txtSubCategory" runat="server" CssClass="formfields" Visible="false" placeholder="SubCategoryName"></asp:TextBox>
                                            </div>
                                            <div>
                                                <asp:RequiredFieldValidator ID="reqSubCategory" runat="server" ControlToValidate="ddlSubCategory"
                                                    CssClass="error" Display="Dynamic" InitialValue="-1" ErrorMessage="Please select sub category."
                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSubCategory" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </div>

                            </li>
                        </ul>

                        <div class="clear"></div>
                        <asp:UpdatePanel runat="server" ID="UPDATEPANELMARK">
                            <ContentTemplate>

                                <asp:Panel runat="server" ID="Mark">


                                    <ul>
                                        <li class="text">
                                            <asp:Label ID="lblbulletinSubject" CssClass="formtext" runat="server" AssociatedControlID="txtSubject">Subject:<span class="error">*</span></asp:Label>
                                        </li>
                                        <li class="field">
                                            <asp:TextBox ID="txtSubject" runat="server" CssClass="formfields" MaxLength="50"
                                                ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                                            <div>
                                                <asp:RequiredFieldValidator ID="reqSubject" runat="server" ControlToValidate="txtSubject"
                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Heading."
                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtSubject"
                                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^<>/\@%]{1,50}"
                                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                                            </div>
                                        </li>
                                        <li class="text">
                                            <asp:Label ID="lblDescription" runat="server" CssClass="formtext">Description:<span class="error">*</span></asp:Label>
                                        </li>
                                        <li class="field2" style="height: auto;">
                                            <TA:ucTextboxMultiline ID="FCKDetails" runat="server" IsRequired="true" CharsLength="250"
                                                TextBoxWatermarkText="Please enter description." ErrorMessage="Please enter Description." ValidationGroup="AddUserValidationGroup" />

                                        </li>


                                    </ul>
                                    <div class="clear"></div>

                                    <ul id="trBrands" runat="server" visible="false">

                                        <li class="text">
                                            <asp:Label ID="lblBrandText" runat="server" CssClass="formtext">Brand:<span class="error">*</span></asp:Label>
                                        </li>
                                        <li class="field" style="height: auto">
                                            <asp:CheckBoxList ID="chckBrandslist" Width="100%" CssClass="radio-rs" CellPadding="2" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"></asp:CheckBoxList>
                                        </li>
                                    </ul>

                                    <div class="clear"></div>
                                    <ul>
                                        <li class="text">FileUpload:
                                        </li>
                                        <li class="field">
                                            <asp:Panel ID="pnlFiles" runat="server" HorizontalAlign="Left">
                                                <asp:FileUpload ID="IpFile" runat="server" CssClass="fileuploads" ToolTip="Select Only PDF File" />
                                            </asp:Panel>
                                            <div>
                                                <asp:Label ID="lblSizeDisplay" runat="server" CssClass="error" Text="Only PDF files are allowed"></asp:Label>
                                            </div>
                                        </li>
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
                                    </ul>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="clear"></div>
        <div class="margin-bottom">
            <div class="float-margin">
                <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="true" ValidationGroup="AddUserValidationGroup"
                    ToolTip="Create" CssClass="buttonbg" OnClick="btnCreate_Click" />
            </div>
            <div class="float-margin">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" ToolTip="Cancel" CssClass="buttonbg"
                    OnClick="btnCancel_Click" />
            </div>
            <div class="clear"></div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="mainheading">
                Search 
            </div>

            <div class="contentbox">

                <div class="H35-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label3" runat="server" AssociatedControlID="ddlcatforsearch"
                                CssClass="formtext">Category For:</asp:Label>
                        </li>
                        <li class="field">
                            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <asp:DropDownList AutoPostBack="true" CssClass="formselect" ID="ddlcatforsearch" runat="server" OnSelectedIndexChanged="ddlcatforsearch_SelectedIndexChanged">
                                            <asp:ListItem Text="select" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Feedback" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Marketing" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Branding" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>


                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                        ControlToValidate="ddlcatforsearch" ErrorMessage="Please select CategoryFor." InitialValue="-1"
                                        CssClass="error" Display="Dynamic" ForeColor="" ValidationGroup="srch"></asp:RequiredFieldValidator>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlcatforsearch" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </li>
                        <li class="text">
                            <asp:Label ID="Label4" runat="server" CssClass="formtext">From Date:<span class="error">*</span></asp:Label>
                        </li>

                        <li class="field">
                            <uc2:ucDatePicker ID="UcPDate" runat="server" IsRequired="true" ErrorMessage="From date required." />
                        </li>
                        <li class="text">
                            <asp:Label ID="Label6" runat="server" CssClass="formtext">To Date:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="UcEDate" runat="server" IsRequired="true" ErrorMessage="To date required." RangeErrorMessage="Date should be greater then equal to current date." />

                        </li>


                        <li class="text">
                            <asp:Label ID="Label5" runat="server" AssociatedControlID="ddlstatusforsearch" CssClass="formtext">ParentCategory Status:</asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <div>
                                    <asp:DropDownList CausesValidation="true" ID="ddlstatusforsearch" runat="server" CssClass="formselect">
                                        <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="All" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlstatusforsearch"
                                        CssClass="error" Display="Dynamic" InitialValue="-1" ErrorMessage="Please select status for Parent category."
                                        SetFocusOnError="true" ValidationGroup="srch"></asp:RequiredFieldValidator>
                                </div>

                            </div>

                        </li>
                        <li class="text">
                            <asp:Label ID="Label7" runat="server" AssociatedControlID="ddlCstatusforsearch" CssClass="formtext">Category Status:</asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <div>
                                    <asp:DropDownList CausesValidation="true" ID="ddlCstatusforsearch" runat="server" CssClass="formselect">
                                        <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="All" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCstatusforsearch"
                                        CssClass="error" Display="Dynamic" InitialValue="-1" ErrorMessage="Please select status for   category."
                                        SetFocusOnError="true" ValidationGroup="srch"></asp:RequiredFieldValidator>
                                </div>

                            </div>

                        </li>
                        <li class="text">
                            <asp:Label ID="Label8" runat="server" AssociatedControlID="ddlMstatusforsearch" CssClass="formtext">Marketing Status:</asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <div>
                                    <asp:DropDownList CausesValidation="true" ID="ddlMstatusforsearch" runat="server" CssClass="formselect">
                                        <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="All" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlMstatusforsearch"
                                        CssClass="error" Display="Dynamic" InitialValue="-1" ErrorMessage="Please select status for market document."
                                        SetFocusOnError="true" ValidationGroup="srch"></asp:RequiredFieldValidator>
                                </div>

                            </div>

                        </li>


                    </ul>


                    <div class="clear"></div>

                </div>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>
    <div class="margin-bottom">
        <div class="float-margin">
            <asp:Button ID="btn_search" runat="server" Text="Search" ToolTip="Search" CssClass="buttonbg" CausesValidation="true" ValidationGroup="srch" OnClick="btn_search_Click" />
        </div>
        <div class="float-margin">
            <asp:Button ID="Button2" Text=" " CausesValidation="true" ValidationGroup="srch" runat="server" OnClick="Button2_Click" CssClass="excel" />
        </div>
        <div class="clear"></div>
    </div>


    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="contentbox">
                <div class="grid2">
                    <asp:GridView ID="gvsearchresult" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="gridrow1"
                        bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                        DataKeyNames="PCID,CID,MID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                        RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                        AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' CausesValidation="false" OnRowCommand="gvsearchresult_RowCommand">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                        <PagerStyle CssClass="gridfooter" />

                        <Columns>
                            <asp:BoundField DataField="ParentCategory" HeaderStyle-HorizontalAlign="Left" HeaderText="Parent Category"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Category" HeaderStyle-HorizontalAlign="Left" HeaderText="Category"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Heading" HeaderStyle-HorizontalAlign="Left" HeaderText="Heading"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Description" HeaderStyle-HorizontalAlign="Left" HeaderText="Description"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Parent Category Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("PCID") %>'
                                        CommandName="PCactiveState" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("PCActive"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("PCActive"))) %>' />

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Category Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("CID") %>'
                                        CommandName="CactiveState" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("CActive"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("CActive"))) %>' />

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Marketing Document Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus2" runat="server" CausesValidation="false" CommandArgument='<%#Eval("MID") %>'
                                        CommandName="MactiveState" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("MActive"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("MActive"))) %>' />

                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>


                    </asp:GridView>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="ucPagingControl1_SetControlRefresh" Visible="false" />
                    </div>

                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
