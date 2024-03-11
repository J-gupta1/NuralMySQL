<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageGiftsV2.aspx.cs" Inherits="Masters_Common_ManageGiftsV2" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function HideMessage() {
            //alert('hi');
            var msgs = document.getElementById('ctl00_contentHolderMain_ucMessage1_pnlUcMessageBox');
            msgs.style.display = "none";
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
   
        <%-- <uc2:header ID="Header1" runat="server" />--%>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMessage1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit Gifts
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblColornm" runat="server" AssociatedControlID="txtGiftName" CssClass="formtext">Gift Name: <span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtGiftName" runat="server" CssClass="formfields" MaxLength="30"
                                    ValidationGroup="AddSchemeGift"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqGiftName" runat="server" ControlToValidate="txtGiftName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Gift Name." SetFocusOnError="true"
                                    ValidationGroup="AddSchemeGift"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtGiftName"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                                    ValidationGroup="AddSchemeGift" runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtEligiblity" CssClass="formtext">Eligiblity Points: <span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtEligiblity" runat="server" CssClass="formfields" MaxLength="10"
                                    ValidationGroup="AddSchemeGift"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEligiblity"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter eligiblity points." SetFocusOnError="true"
                                    ValidationGroup="AddSchemeGift"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtEligiblity"
                                    Display="Dynamic" CssClass="error" ErrorMessage="Please enter numbers only" ValidationExpression="(^[0-9]\d*$)"
                                    ValidationGroup="AddSchemeGift" runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblSchemeHeading" runat="server" CssClass="formtext">Scheme: <span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlScheme" CssClass="formselect" runat="server" onChange="return HideMessage();">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rqScheme" ControlToValidate="ddlScheme"
                                    CssClass="error" ErrorMessage="Please select scheme." InitialValue="0"
                                    ValidationGroup="AddSchemeGift" />
                            </li>
                            <li class="text">
                                <div class="float-margin">
                                    <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status </asp:Label>
                                </div>
                                <div class="float-margin">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                </div>
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="True" ValidationGroup="AddSchemeGift"
                                        ToolTip="Add " CssClass="buttonbg" OnClick="btnCreate_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg" OnClick="btncancel_click" CausesValidation="False" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
        </asp:UpdatePanel>

        <div class="mainheading">
            Search Gifts
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">Gift Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSerName" runat="server" MaxLength="100" CssClass="formfields"> </asp:TextBox>
                            </li>
                            <li class="text">Scheme:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSchemeSearch" CssClass="formselect" runat="server" onChange="return HideMessage();">
                                </asp:DropDownList>
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnserch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click" CausesValidation="False"></asp:Button>
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="getalldata" Text="Show All Data" runat="server" ToolTip="Search"
                                        CssClass="buttonbg" OnClick="btngetalldta_Click" CausesValidation="False"></asp:Button>
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            List
        </div>
        <div class="export">
            <asp:Button ID="Exporttoexcel" runat="server" CausesValidation="False" CssClass="excel"
                OnClick="Exporttoexcel_Click" Text="" ToolTip="Export to Excel" OnClientClick="return HideMessage();" />
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grdGift" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                            RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                            AllowPaging="True" SelectedStyle-CssClass="gridselected" DataKeyNames="GiftID"
                            PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowCommand="grdColor_RowCommand"
                            EmptyDataText="No record found" OnPageIndexChanging="grdColor_PageIndexChanging">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="GiftName"
                                    HeaderText="Gift Name"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EligiblityPoint"
                                    HeaderText="Eligiblity Points"></asp:BoundField>

                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SchemeName"
                                    HeaderText="Scheme"></asp:BoundField>

                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnActiveDeactive" runat="server" CommandArgument='<%#Eval("GiftId") %>'
                                            CommandName="Active" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Active"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnSchemeID" runat="server" Value='<%#Eval("SchemeID") %>' />
                                        <asp:ImageButton CommandArgument='<%#Eval("GiftId") %>' runat="server" ID="btnEdit"
                                            CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                            ToolTip="Edit User" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <%-- <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
                </asp:UpdatePanel>
            </div>
        </div>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
</asp:Content>
