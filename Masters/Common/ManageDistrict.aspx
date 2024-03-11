<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageDistrict.aspx.cs" Inherits="Masters_HO_Common_ManageDistrict_" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <style type="text/css">
        .form_select
        {
            height: 22px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add / Edit District
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label1" runat="server" Text="">Country: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbInsCountry" runat="server" CssClass="formselect" AutoPostBack="True" OnSelectedIndexChanged="cmbInsCountry_SelectedIndexChanged">
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbInsCountry" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Please select a Country " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblstatename" runat="server" Text="">State: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbInsertState" runat="server" CssClass="formselect"
                                AutoPostBack="True">
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valstate" ControlToValidate="cmbInsertState" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Please select a State " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblDistCode" runat="server" Text="">District Code: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertCode" runat="server" CssClass="formfields"
                                MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtInsertCode" CssClass="error" Display="Dynamic"
                                ErrorMessage="Please insert code " ValidationGroup="insert" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtInsertCode" Display="Dynamic"
                                CssClass="error" ErrorMessage="Invalid District Code" ValidationExpression="[^()<>/\@%]{1,20}" ValidationGroup="insert"
                                runat="server" />
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblDistname" runat="server" Text="">District Name: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertName" runat="server" CssClass="formfields"
                                MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtInsertName" CssClass="error" Display="Dynamic"
                                ErrorMessage="Please insert name " ValidationGroup="insert" />
                            <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtInsertName" CssClass="error" Display="Dynamic"
                                ErrorMessage="Invalid District Name" ValidationExpression="[^()<>/\@%]{1,70}" ValidationGroup="insert"
                                runat="server" />
                        </li>
                        <li class="text">Status:
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkstatus" runat="server" />
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btninsert_click"
                                    ValidationGroup="insert" CssClass="buttonbg" CausesValidation="True" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="buttonbg" OnClick="btncancel_Click" />
                            </div>
                        </li>

                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div class="mainheading">
        Search District
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label2" Text="Country:" runat="server" />
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerCountry"
                                runat="server" CssClass="formselect"
                                OnSelectedIndexChanged="cmbSerCountry_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblSerState" Text="State:" runat="server" />
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerState" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label3" runat="server" Text="District Code:" />
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerCode" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblSerDistrict" runat="server" Text="District Name:" />
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerName" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSerchD" runat="server" CssClass="buttonbg"
                                    OnClick="btnSerchDistrict_Click" Text="Search" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="getalldata" runat="server" CssClass="buttonbg"
                                    OnClick="btngetalldata_click" Text="Show All Data" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSerchD" />
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
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdDistrict" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="DistrictID" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="None" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                        Width="100%" AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" EmptyDataText="No record found"
                        FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" OnRowCommand="grdState_RowCommand"
                        OnPageIndexChanging="grdDistrict_PageIndexChanging" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                        <Columns>
                            <asp:BoundField DataField="CountryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Country"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DistrictCode" HeaderStyle-HorizontalAlign="Left" HeaderText="District Code "
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DistrictName" HeaderStyle-HorizontalAlign="Left" HeaderText="District Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("DistrictID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("DistrictID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                        <AlternatingRowStyle CssClass="Altrow" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdDistrict" />

                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:Label ID="lblerror" Text="" runat="server" />
</asp:Content>
