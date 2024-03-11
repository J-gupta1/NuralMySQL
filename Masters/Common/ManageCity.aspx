<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageCity.aspx.cs" Inherits="Masters_HO_Common_ManageCity" %>

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
        Add / Edit City
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
                            <asp:Label ID="Label1" runat="server" Text="Country: <span class='error'>*</span>"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbinsCountry" runat="server" OnSelectedIndexChanged="cmbInsertCountry_SelectedIndexChanged"
                                    CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbinsCountry"
                                    CssClass="error" ErrorMessage="Please select a Country " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblstate" runat="server" Text="State: <span class='error'>*</span>"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertState" runat="server" OnSelectedIndexChanged="cmbInsertState_SelectedIndexChanged"
                                    CssClass="formselect" AutoPostBack="True">
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
                                <%--#CC03 START COMMENTED  <asp:DropDownList ID="cmbInsertDistrict" runat="server" CssClass="form_select4" AutoPostBack="True" OnSelectedIndexChanged="cmbInsertDistrict_SelectedIndexChanged">
                                                                                            </asp:DropDownList>  #CC03 END COMMENTED--%>
                                <asp:DropDownList ID="cmbInsertDistrict" runat="server" CssClass="formselect">
                                </asp:DropDownList><%--#CC03 ADDED--%>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valdist" ControlToValidate="cmbInsertDistrict"
                                    CssClass="error" InitialValue="0" ErrorMessage="Please select a District " ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul id="dvTehsil" runat="server" style="display: none;">
                        <li class="text">
                            <asp:Label ID="Label3" runat="server" Text="">Tehsil: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="drpTehsil" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="drpTehsil"
                                    CssClass="error" InitialValue="0" ErrorMessage="Please select a Tehsil " ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblcitycode" runat="server" Text="">City Code: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtInsertCode" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert code " ValidationGroup="insert" />

                            <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtInsertCode" Display="Dynamic"
                                CssClass="error" ErrorMessage="Invalid City Code" ValidationExpression="[^()<>/\@%]{1,20}"
                                ValidationGroup="insert" runat="server" />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblcityname" runat="server" Text="">City Name: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertName" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtInsertName" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert a name " ValidationGroup="insert" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtInsertName" Display="Dynamic"
                                CssClass="error" ErrorMessage="Invalid City Name" ValidationExpression="[^()<>/\@%]{1,70}"
                                ValidationGroup="insert" runat="server" />
                        </li>
                        <li class="text">
                            <%--<td class="formtext" valign="top" align="right">
                                                                            Status:
                                                                        </td>
                                                                        <td align="left" valign="top">
                                                                            <asp:CheckBox ID="chkstatus" runat="server" Checked="True" />
                                                                        </td>--%>
                                               
                         Status: 
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkstatus" runat="server" Checked="True" />
                        </li>
                        <li class="text">
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
                                                </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        Search City
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
                                <%--#CC03 START COMMENTED     <asp:DropDownList ID="cmbSerDistrict" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="cmbSerDistrict_SelectedIndexChanged">
                                                                                </asp:DropDownList> #CC03 END COMMENTED--%>
                                <asp:DropDownList ID="cmbSerDistrict" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <%--#CC03 ADDED--%>                            
                        </li>
                    </ul>
                    <ul id="dvSerTehsil" runat="server" style="display: none;">
                        <li class="text">
                            <asp:Label ID="lblTehsil" runat="server" Text="Tehsil:"></asp:Label>
                        </li>
                        <li class="field">                           
                                <asp:DropDownList ID="cmbSerTehsil" runat="server" CssClass="formselect">
                                </asp:DropDownList>                           
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblsrcitycode" runat="server" Text="City Code:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerCode" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsrcity" runat="server" Text="City Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerName" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSerchCity" Text="Search" runat="server" OnClick="btnSerchCity_Click"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="getalldata" Text="Show All Data" runat="server" OnClick="btngetalldata_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                    <%--<td align="left" valign="top" class="formtext">
                                                                <asp:Button ID="getalldata" Text="Show All Data" runat="server" OnClick="btngetalldata_Click"
                                                                    CssClass="buttonbg" />
                                                            </td>
                                                            <td align="left" valign="top"></td>
                                                            <td align="left" valign="top" class="formtext"></td>--%>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSerchCity" />
                <asp:AsyncPostBackTrigger ControlID="getalldata" />
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
                    <asp:GridView ID="grdCity" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="CityID" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" EmptyDataText="No record found"
                        RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                        RowStyle-CssClass="gridrow" PageSize='<%$ AppSettings:GridViewPageSize %>' FooterStyle-CssClass="gridfooter"
                        HeaderStyle-CssClass="gridheader" OnRowCommand="grdCity_RowCommand" OnPageIndexChanging="grdCity_PageIndexChanging">
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
                            <%--#CC03 START COMMENTED <asp:BoundField DataField="TehsilName" HeaderStyle-HorizontalAlign="Left" HeaderText="Tehsil"
                                                        HtmlEncode="true">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField> #CC03 END COMMENTED--%>
                            <asp:BoundField DataField="CityCode" HeaderStyle-HorizontalAlign="Left" HeaderText="City Code "
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("CityID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("CityID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                                                <asp:PostBackTrigger ControlID="grdCity" />
                                            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
