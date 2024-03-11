<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageArea.aspx.cs" Inherits="Masters_HO_Common_ManageArea" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <style type="text/css">
        .style1 {
            height: 68px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add / Edit Area
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
                            <div>
                                <asp:DropDownList ID="cmbInsCountry" CssClass="formselect" runat="server" OnSelectedIndexChanged="cmbInsertCountry_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbInsCountry"
                                    CssClass="error" ErrorMessage="Please select a Country " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblstate" runat="server" Text="">State: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertState" CssClass="formselect" runat="server" OnSelectedIndexChanged="cmbInsertState_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valstate" ControlToValidate="cmbInsertState"
                                    CssClass="error" ErrorMessage="Please select a State " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lbldst" runat="server" Text="">District: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertDistrict" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbInsertDist_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valDist" ControlToValidate="cmbInsertDistrict"
                                    CssClass="error" ErrorMessage="Please select a District " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblinscity" runat="server" Text="">City: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertCity" runat="server" CssClass="formselect" AutoPostBack="True" OnSelectedIndexChanged="cmbInsertCity_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valcity" ControlToValidate="cmbInsertCity"
                                    CssClass="error" ErrorMessage="Please select a City " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul id="dvTehsil" runat="server" style="display: none">
                        <li class="text">
                            <asp:Label ID="Label3" runat="server" Text="">Tehsil: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <%--#CC03 START COMMENTED <asp:DropDownList ID="drpTehsil" runat="server" CssClass="form_select" AutoPostBack="True"
                                                                                                OnSelectedIndexChanged="drpTehsil_SelectedIndexChanged">
                                                                                            </asp:DropDownList>#CC03 END COMMENTED --%>
                                <asp:DropDownList ID="drpTehsil" runat="server" CssClass="formselect">
                                </asp:DropDownList><%-- #CC03 ADDED--%>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="cmbInsertDistrict"
                                    CssClass="error" ErrorMessage="Please select a Tehsil " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul>

                        <li class="text">
                            <asp:Label ID="lblinsname" runat="server" Text="">Area Name: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertName" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtInsertName" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert Area Name " ValidationGroup="insert" />
                            <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtInsertName" Display="Dynamic"
                                CssClass="error" ErrorMessage="Invalid Area Name" ValidationExpression="[^()<>/\@%]{1,70}"
                                ValidationGroup="insert" runat="server" />
                        </li>
                        <%--<td class="formtext" valign="top"  align="right">
                                                                            <asp:Label ID="labelinscode" runat="server" Text="">Area Code:<span class="error">*</span></asp:Label>
                                                                        </td>
                                                                        <td align="left" valign="top" class="formtext">
                                                                            <asp:TextBox ID="txtInsertCode" runat="server" CssClass="form_input2" MaxLength="20"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtInsertCode"
                                                                                CssClass="error" ErrorMessage="Please insert Area Code " ValidationGroup="insert" />
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtInsertCode"  CssClass="error"
                                                                                ErrorMessage="Invalid Area Code" ValidationExpression="[^()<>/\@%]{1,20}" ValidationGroup="insert"
                                                                                runat="server" />
                                                                        </td>--%>
                        <li class="text">
                            <asp:Label ID="labelinscode" runat="server" Text="">Area Code:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtInsertCode" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert Area Code " ValidationGroup="insert" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtInsertCode" Display="Dynamic"
                                CssClass="error" ErrorMessage="Invalid Area Code" ValidationExpression="[^()<>/\@%]{1,20}"
                                ValidationGroup="insert" runat="server" />
                        </li>
                        <li class="text">Status: </li>
                        <li class="field">
                            <asp:CheckBox ID="chkstatus" runat="server" />
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btninsert_click"
                                    ValidationGroup="insert" CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
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
        Search Area
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label2" runat="server" Text="Country:"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbSerCountry" runat="server" OnSelectedIndexChanged="cmbSerCountry_SelectedIndexChanged"
                                    CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserstate" runat="server" Text="State:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerState" runat="server" OnSelectedIndexChanged="cmbSerState_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserdist" runat="server" Text="District:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerDistrict" runat="server" OnSelectedIndexChanged="cmbSerdist_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblsercity" runat="server" Text="City:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerCity" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="cmbSerCity_SelectedIndexChanged">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul id="dvSerTehsil" runat="server" style="display: none">
                        <li class="text">
                            <asp:Label ID="Label4" runat="server" Text="Tehsil:"></asp:Label>
                        </li>
                        <li class="field">
                            <%--#CC03 START COMMENTED <asp:DropDownList ID="cmbSerTehsil" runat="server" CssClass="form_select" AutoPostBack="True"
                                                                        OnSelectedIndexChanged="cmbSerTehsil_SelectedIndexChanged">
                                                                    </asp:DropDownList>#CC03 END COMMENTED --%>
                            <asp:DropDownList ID="cmbSerTehsil" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                            <%--#CC03 ADDED--%>
                        </li>
                    </ul>
                    <ul>

                        <li class="text">
                            <asp:Label ID="lblserarea" runat="server" Text="Area Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerName" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblSerName" runat="server" Text="Area Code:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerCode" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSerCode" Text="Search" runat="server" OnClick="btnSerchArea_Click"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="getalldata" Text="Show All Data" runat="server" OnClick="btngetalldata_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSerCode" />
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
                    <asp:GridView ID="grdArea" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="AreaID" EmptyDataText="No data founnd"
                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                        AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" PageSize='<%$ AppSettings:GridViewPageSize %>'
                        FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" RowStyle-VerticalAlign="top"
                        Width="100%" OnRowCommand="grdArea_RowCommand" OnPageIndexChanging="grdArea_PageIndexChanging">
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

                            <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TehsilName" HeaderStyle-HorizontalAlign="Left" HeaderText="Tehsil Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="AreaCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Area Code "
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="AreaName" HeaderStyle-HorizontalAlign="Left" HeaderText="Area Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("AreaID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("AreaID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdArea" />
                    
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
