<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Key47CityState.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Masters_Common_Key47CityState" %>

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
        Add / Edit Key47CityState
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
                            <asp:Label ID="LblState" runat="server" Text="State: <span class='error'>*</span>"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlState" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"
                                    CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlState"
                                    CssClass="error" ErrorMessage="Please select  State " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblCity" runat="server" Text="City: <span class='error'>*</span>"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlCity" runat="server"
                                    CssClass="formselect">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valcity" ControlToValidate="ddlCity"
                                    CssClass="error" ErrorMessage="Please select City " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">Status: 
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkstatus" runat="server" Checked="True" />
                        </li>
                    </ul>
                    <ul>
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
        Search Key47 State/City
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblSerState" Text="State: " runat="server" />
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSerState" runat="server" OnSelectedIndexChanged="ddlSerState_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblSerCity" runat="server" Text="City:"></asp:Label>
                        </li>
                        <li class="field">

                            <asp:DropDownList ID="ddlSerCity" runat="server" CssClass="formselect">
                            </asp:DropDownList>

                        </li>
                         <li class="text">
                            <asp:Label ID="LblSerStatus" runat="server" Text="Status:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSearchStatus" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>   
                    <ul>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSerchStateCity" Text="Search" runat="server" OnClick="btnSerchStateCity_Click"
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
                <asp:AsyncPostBackTrigger ControlID="btnSerchStateCity" />
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
                    <asp:GridView ID="grdStateCity" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="CityId" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" EmptyDataText="No record found"
                        RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                        RowStyle-CssClass="gridrow" PageSize='<%$ AppSettings:GridViewPageSize %>' FooterStyle-CssClass="gridfooter"
                        HeaderStyle-CssClass="gridheader" OnRowCommand="grdStateCity_RowCommand" OnPageIndexChanging="grdStateCity_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                        <Columns>
                            
                            <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Cityname" HeaderStyle-HorizontalAlign="Left" HeaderText="City Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("CityId") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdStateCity" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>