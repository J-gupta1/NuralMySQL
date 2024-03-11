<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageTehsill.aspx.cs" Inherits="Masters_HO_Common_ManageTehsill" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add / Edit Tehsil
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
                            <asp:Label ID="Label1" runat="server" Text="Country:<span class='error'>*</span>"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbinsCountry" runat="server" OnSelectedIndexChanged="cmbInsertCountry_SelectedIndexChanged"
                                    CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbinsCountry" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Please select a Country " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblstate" runat="server" Text="State:<span class='error'>*</span>"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertState" runat="server" OnSelectedIndexChanged="cmbInsertState_SelectedIndexChanged"
                                    CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>

                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valstate" ControlToValidate="cmbInsertState" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Please select a State " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lbldst" runat="server" Text="">District:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertDistrict" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbInsertDist_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valdist" ControlToValidate="cmbInsertDistrict" Display="Dynamic"
                                    CssClass="error" InitialValue="0" ErrorMessage="Please select a District " ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <%--#CC02 ADDED START--%>
                        <li class="text">
                            <asp:Label ID="Label3" runat="server" Text="">City:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbInsertCity" runat="server" CssClass="formselect">
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator runat="server" ID="valcity" ControlToValidate="cmbInsertCity" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please select a City " InitialValue="0" ValidationGroup="insert" />
                        </li>
                        <%--#CC02 ADDED END--%>
                        <li class="text">
                            <asp:Label ID="lbltehsillcode" runat="server" Text="">Tehsil Code:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtInsertCode" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert code " ValidationGroup="insert" />

                            <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtInsertCode" Display="Dynamic"
                                CssClass="error" ErrorMessage="Invalid Tehsill Code" ValidationExpression="[^()<>/\@%]{1,20}"
                                ValidationGroup="insert" runat="server" />
                        </li>
                        <li class="text">
                            <asp:Label ID="lbltehsillname" runat="server" Text="">Tehsil Name:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertName" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtInsertName" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert a name " ValidationGroup="insert" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtInsertName" Display="Dynamic"
                                CssClass="error" ErrorMessage="Invalid Tehsil Name" ValidationExpression="[^()<>/\@%]{1,70}"
                                ValidationGroup="insert" runat="server" />
                        </li>
                    </ul>
                    <ul>
                        <li class="text">Status:
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkstatus" runat="server" Checked="True" />
                        </li>
                    </ul>
                    <ul>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btninsert_click"
                                    ValidationGroup="insert" CssClass="buttonbg" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit" />
                 <asp:PostBackTrigger ControlID="btnCancel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div class="mainheading">
        Search Tehsil
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label2" Text="Country: " runat="server" />
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerCountry" runat="server" OnSelectedIndexChanged="cmbserCountry_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblSerState" Text="State: " runat="server" />
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerState" runat="server" OnSelectedIndexChanged="cmbserstate_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lbldistrict" runat="server" Text="District:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerDistrict" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbSerdist_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <%--#CC02 ADDED START--%>
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label4" runat="server" Text="City:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerCity" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <%--#CC02 ADDED END--%>
                        <li class="text">
                            <asp:Label ID="lblsrteshsillcode" runat="server" Text="Teshsil Code:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerCode" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsrteshsill" runat="server" Text="Teshsil Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerName" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                    </ul>
                    <ul>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSerchTeshsill" Text="Search" runat="server" OnClick="btnSerchTeshsill_Click"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="getalldata" Text="Show All Data" runat="server" OnClick="btngetalldata_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>                        
                    </ul>
                </div>
            </ContentTemplate>
             <Triggers>
                <asp:PostBackTrigger ControlID="btnSerchTeshsill" />
                 <asp:PostBackTrigger ControlID="getalldata" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="exportToExel" Text=" " runat="server" OnClick="exportToExel_Click"
            CssClass="excel" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdTehsill" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="TehsilID" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" EmptyDataText="No record found"
                        RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                        RowStyle-CssClass="gridrow" PageSize='<%$ AppSettings:GridViewPageSize %>' FooterStyle-CssClass="gridfooter"
                        HeaderStyle-CssClass="gridheader" OnRowCommand="grdTehsill_RowCommand" OnPageIndexChanging="grdTehsill_PageIndexChanging">
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
                            <asp:BoundField DataField="DistrictName" HeaderStyle-HorizontalAlign="Left" HeaderText="District"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <%--#CC02 ADDED START--%>
                            <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <%--#CC02 ADDED END--%>
                            <asp:BoundField DataField="TehsilCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Tehsil Code "
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TehsilName" HeaderStyle-HorizontalAlign="Left" HeaderText="Tehsil Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("TehsilID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("TehsilID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdTehsill" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
