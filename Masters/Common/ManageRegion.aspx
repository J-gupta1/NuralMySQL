<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ManageRegion.aspx.cs" Inherits="Masters_Common_ManageRegion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>

<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="mainheading">
        Add / Edit Region
    </div>
    <div class="contentbox">

        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblCountryname" runat="server" Text="">Country Name:<span class="error">*</span></asp:Label></li>
                <li class="field">
                    <asp:DropDownList ID="ddlCountryName" CssClass="formselect" OnSelectedIndexChanged="ddlCountryName_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCountryName"
                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please Select Country Name."
                        ValidationGroup="insert"></asp:RequiredFieldValidator>
                </li>
                <li class="text">
                    <asp:Label ID="lblZonename" runat="server" Text="">Zone Name:<span class="error">*</span></asp:Label></li>
                <li class="field">
                    <asp:DropDownList ID="ddlZoneName" CssClass="formselect" runat="server">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlZoneName"
                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please Select Zone Name."
                        ValidationGroup="insert"></asp:RequiredFieldValidator>
                </li>
                <li class="text">
                    <asp:Label ID="lblRegionname" runat="server" Text="">Region Name:<span class="error">*</span></asp:Label></li>
                <li class="field">
                    <asp:TextBox ID="txtInsertRegionName"
                        runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="valzname" ControlToValidate="txtInsertRegionName" Display="Dynamic"
                        CssClass="error" ErrorMessage="Please insert Region Name" ValidationGroup="insert" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtInsertRegionName"
                        InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                    </cc1:FilteredTextBoxExtender>
                </li>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="lblRegionCode" runat="server" Text="">Region Code:</asp:Label></li>
                <li class="field">
                    <asp:TextBox ID="txtInsertRegionCode" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                </li>
                <li class="text">
                    <asp:Label ID="lblDisplayOrder" runat="server" Text="">Display Order:</asp:Label></li>
                <li class="field">
                    <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="formfields" MaxLength="4"></asp:TextBox>

                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtDisplayOrder"
                        ValidChars="0123456789">
                    </cc1:FilteredTextBoxExtender>
                </li>
                <li class="text">
                    <asp:Label ID="lblRemarks" runat="server" Text="">Remarks:</asp:Label></li>
                <li class="field">
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="error"
                                                                        Display="Dynamic" ControlToValidate="txtRemarks" ErrorMessage="Invalid char(s)!"
                                                                        ValidationExpression="[^<>@%]{1,50}$" SetFocusOnError="true" ValidationGroup="Zone"></asp:RegularExpressionValidator>--%>
                </li>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="lblActive" runat="server" Text="">Active:</asp:Label></li>
                <li class="field">
                    <asp:CheckBox ID="chkActive" runat="server" CssClass="CheckBoxList2" Checked="True" />
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnsubmit" runat="server" OnClick="btninsert_click" Text="Submit"
                            ValidationGroup="insert" CssClass="buttonbg" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"
                            Text="Cancel" CssClass="buttonbg" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="mainheading">
        Search Region Information
    </div>
    <div class="contentbox">
        <div class="H20-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblcountrysearch" runat="server" Text="Country Name:"></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlCountrySearch" CssClass="formselect" runat="server"></asp:DropDownList>
                </li>
                <li class="text">
                    <asp:Label ID="lblstatefnregionname" runat="server" Text="Region Name:"></asp:Label>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtSerregionName" runat="server" CssClass="formfields"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtSerregionName"
                        InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                    </cc1:FilteredTextBoxExtender>
                </li>
                <li class="text">
                    <asp:Label ID="lblstatus" runat="server" Text="Status:"></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlStatus" CssClass="formselect" runat="server"></asp:DropDownList>
                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                            CssClass="buttonbg" CausesValidation="False" ValidationGroup="search" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnSearchALL" Text="Show All" runat="server"
                            CssClass="buttonbg" CausesValidation="False" ValidationGroup="search" OnClick="btnSearchALL_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div id="Regiongrid" runat="server">
        <div class="mainheading">
            Region List
        </div>
        <div class="export">
            <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel"
                OnClick="btnExprtToExcel_Click" CausesValidation="False" />
        </div>
    </div>
    <div runat="server" id="gridid">
        <div class="contentbox" runat="server">
            <div class="grid1">
                <asp:GridView ID="grdRegion" runat="server" AllowPaging="false" AlternatingRowStyle-CssClass="Altrow"
                    AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                    DataKeyNames="RegionID" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                    FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                    OnRowCommand="grdRegion_RowCommand" RowStyle-CssClass="gridrow" EmptyDataText="No record found"
                    RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" Width="100%"
                    OnPageIndexChanging="grdRegion_PageIndexChanging">
                    <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <Columns>

                        <asp:BoundField DataField="RegionName" HeaderText="Region Name" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="RegionCode" HeaderText="Region Code" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="CountryName" HeaderText="Country Name" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ZoneName" HeaderText="Zone Name" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="DisplayOrder" HeaderText="Display Order" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" />

                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("RegionID") %>'
                                    CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                    ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Active"))) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                            <ItemTemplate>
                                <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("RegionID") %>'
                                    CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <div id="dvFooter" runat="server" class="pagination">
                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
            </div>
        </div>
    </div>
</asp:Content>
