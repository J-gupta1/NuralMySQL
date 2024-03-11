<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageISPSalary.aspx.cs" Inherits="Masters_Common_ManageISPSalary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" charset="utf-8">


        function txtISPCodeChanged() {
            var ISPCode = document.getElementById('<%= txtISPCode.ClientID %>').value;
            var hdnISPCode = document.getElementById('<%= hdnISPCode.ClientID %>');
            hdnISPCode.value = ISPCode;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
                <asp:Label ID="lblAnotherMessage" runat="server"></asp:Label>
                <asp:HiddenField ID="hdnISPSalaryid" runat="server" Value="0" />
                <asp:HiddenField ID="hdnISPCode" runat="server" Value="0" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updISP" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit ISP Salary
                </div>
                <div runat="server" id="dvInterfaceSalary">
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">
                                    <asp:Label ID="lblISPName" runat="server" CssClass="formtext">ISP Name: <span class="error">*</span></asp:Label>
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtISPCode" onchange="txtISPCodeChanged();" runat="server" CssClass="formfields"
                                            MaxLength="20"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtISPCode"
                                        CssClass="error" Display="Dynamic" ValidationGroup="AddSalary" ErrorMessage="Please enter ISP Name."
                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regEDISPName" ControlToValidate="txtISPCode"
                                        CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,50}"
                                        ValidationGroup="AddSalary" runat="server" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                                        MinimumPrefixLength="3" ServiceMethod="GetISPCodesList" ServicePath="../../CommonService.asmx"
                                        TargetControlID="txtISPCode" UseContextKey="true">
                                    </cc1:AutoCompleteExtender>
                                </li>
                                <li class="text">
                                    <asp:Label ID="lblISPSalary" runat="server">ISP Salary: <span class="error">*</span></asp:Label>
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtISPSalary" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="ReqSalary" runat="server" ControlToValidate="txtISPSalary"
                                        CssClass="error" Display="Dynamic" ValidationGroup="AddSalary" ErrorMessage="Please enter ISP Salary."
                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regEReqSalary" ControlToValidate="txtISPSalary"
                                        CssClass="error" ErrorMessage="Invalid Salary" ValidationGroup="AddSalary" ValidationExpression="^(0|[1-9][0-9]*)$"
                                        runat="server" />
                                </li>
                                <li class="text">Effective Date: <span class="error">*</span>
                                </li>
                                <li class="field">
                                    <uc2:ucDatePicker ID="ucEffectiveFrom" runat="server" ValidationGroup="AddSalary1"
                                        ErrorMessage="date required." IsRequired="true" defaultDateRange="false" RangeErrorMessage="" />
                                </li>
                            </ul>
                            <ul>
                                <li class="text"></li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSubmitSalary" Text="Submit" runat="server" CausesValidation="true"
                                            ValidationGroup="AddSalary" OnClick="btnSubmitSalary_Click" ToolTip="Submit"
                                            CssClass="buttonbg" />
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                            OnClick="btnCancel_Click" CausesValidation="false" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div runat="server" id="dvUploadSalary">
                    <div class="mainheading">
                        Upload ISP Salary
                    </div>
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Mode:
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:DropDownList ID="ddlMode" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlMode_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Selected="True" Value="0" Text="Select"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Add"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Update"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Delete"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlMode"
                                        CssClass="error" InitialValue="0" Display="Dynamic" ValidationGroup="AddSalaryUpload"
                                        ErrorMessage="Please Select Any mode." SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </li>
                                <li class="text">Effective Date:
                                </li>
                                <li class="field">
                                    <uc2:ucDatePicker ID="ucEffectiveDAteUpload" runat="server" ErrorMessage="To date required."
                                        defaultDateRange="false" ValidationGroup="AddSalaryUpload1" IsRequired="true"
                                        RangeErrorMessage="" />
                                </li>
                                <li class="text">Upload File: <span class="error">*</span>
                                </li>
                                <li class="field">
                                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text="" Visible="true"></asp:Label>
                                </li>
                            </ul>
                            <ul>
                                <li class="field2">
                                    <div class="link float-margin">
                                        <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                            CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click" CausesValidation="false"></asp:LinkButton>
                                    </div>
                                    <div class="link float-margin">
                                        <a class="elink2" runat="server" id="InsertTemplate"
                                            href="../../Excel/Templates/ISPSalaryMaster.xlsx">Download Template</a>
                                    </div>
                                    <div class="link float-margin">
                                        <a class="elink2"
                                            runat="server" id="DeleteTemplate" href="../../Excel/Templates/ISPSalaryMasterDelete.xlsx">Download Delete Template</a>
                                    </div>
                                    <div class="link float-margin">
                                        <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>

                                        <asp:HyperLink ID="hlnkInvalidDataFromDB" runat="server" CssClass="elink3" Visible="false"></asp:HyperLink>
                                    </div>
                                </li>
                                <li class="text"></li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnUpload" runat="server" CausesValidation="true" CssClass="buttonbg"
                                            OnClick="btnUpload_Click" Text="Save" ToolTip="Save" ValidationGroup="AddSalaryUpload" />
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnUploadCancel" runat="server" CssClass="buttonbg" OnClick="btnUploadCancel_Click"
                                            Text="Cancel" CausesValidation="false" ToolTip="Cancel" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <%--  <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />--%>
                <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                <asp:PostBackTrigger ControlID="btnUpload" />
                <%--<asp:AsyncPostBackTrigger ControlID="DwnldReferenceCodeTemplate" EventName="click" />--%>
            </Triggers>
        </asp:UpdatePanel>
        <div class="mainheading">
            Search ISP
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">ISP Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSearchISPName" runat="server" MaxLength="100" CssClass="formfields">
                                </asp:TextBox>
                            </li>
                            <li class="text">ISP Code:
                            </li>
                            <li class="field">
                                <%-- <uc2:ucDatePicker ID="ucSearchEffectiveDate" runat="server" defaultDateRange="false" />--%>
                                <asp:TextBox ID="txtSearchISPCode" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearchISP" Text="Search" runat="server" ToolTip="Search ISP" CssClass="buttonbg"
                                        OnClick="btnSearchISP_Click" CausesValidation="False"></asp:Button>
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                        OnClick="btnShow_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            ISP List
        </div>
        <div class="export">
            <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
                OnClick="btnExprtToExcel_Click" />
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grdISPList" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                            RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4"
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                            PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                            OnPageIndexChanging="grdISPList_PageIndexChanging" DataKeyNames="ISPSalaryId"
                            OnRowDataBound="grdvwUserList_RowDataBound" OnRowCommand="grdISPList_RowCommand">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ISPName"
                                    HeaderText="ISP Name"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ISPCode"
                                    HeaderText="ISP Code"></asp:BoundField>
                                <asp:TemplateField HeaderText="Salary" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                            <asp:Label runat="server" Text='<%#Eval("SalaryAmt") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ActivationDate"
                                    HeaderText="Activation Date"></asp:BoundField>
                                <%--   <asp:TemplateField HeaderText="Password">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPassword" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Password"))%>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblPasswordSalt" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"PasswordSalt"))%>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:LinkButton ID="hlPassword" runat="server" Text="Password"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("ISPSalaryId") %>' runat="server" ID="btnEdit"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' CausesValidation="false"
                                            CommandName="cmdEdit" ToolTip="Edit Salary" Visible='<%#Convert.ToBoolean(Eval("displayModifyButton")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("ISPSalaryId") %>' runat="server" ID="btnDelete"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>' CausesValidation="false"
                                            CommandName="cmdDelete" ToolTip="Delete Salary" Visible='<%#Convert.ToBoolean(Eval("displayModifyButton")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <%--            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>
    <%--     <uc3:footer ID="Footer1" runat="server" />--%>
</asp:Content>
