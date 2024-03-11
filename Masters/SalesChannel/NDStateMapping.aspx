<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="NDStateMapping.aspx.cs" Inherits="SalesChannel_NDStateMapping" %>

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

    <div id="dvhide" runat="server" visible="true">
        <div class="mainheading">
            Data Export
        </div>
        <div class="contentbox">
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Saleschannel Type :</li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlExportoption" runat="server" CssClass="formselect">
                                <%-- <asp:ListItem Selected="True" Text="ND Wise" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </li>
                    <li class="field3">
                        <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="btnExportToExcel_Click"
                            CssClass="excel" />
                    </li>
                </ul>
            </div>
        </div>
    </div>

    <div class="mainheading">
        Manage SalesChannel State Mapping
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label1" runat="server" Text="">Saleschannel Type :</asp:Label><span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="DdlSaleschannelType" runat="server" CssClass="formselect" OnSelectedIndexChanged="DdlSaleschannelType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlSaleschannelType"
                                InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select SaleschannelType."
                                ValidationGroup="Search"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DdlSaleschannelType"
                                InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select SaleschannelType."
                                ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblrole" runat="server" Text="">Sales Channel :</asp:Label><span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSalesChannel" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator ID="reqSalesChannel" runat="server" ControlToValidate="ddlSalesChannel"
                                    InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select Sales Channel."
                                    ValidationGroup="Search"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rq" runat="server" ControlToValidate="ddlSalesChannel"
                                    InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select Sales Channel."
                                    ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                    ValidationGroup="Search" CssClass="buttonbg" CausesValidation="True" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                    CssClass="buttonbg" CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Always">
        <ContentTemplate>
            <div class="mainheading">
                List
            </div>
            <div class="contentbox">
                <div class="grid1">
                    <asp:GridView ID="gvNDToState" runat="server" AlternatingItemStyle-CssClass="Altrow" GridLines="none"
                        AutoGenerateColumns="False" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" HeaderStyle-CssClass="gridheader"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="Middle" RowStyle-CssClass="gridrow"
                        meta:resourcekey="gvPartResource1" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                        ShowFooter="True" Width="100%" OnRowCommand="gvNDToState_RowCommand">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <%-- <asp:TemplateField HeaderText="Sale Channel Name" HeaderStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSaleChannelName" runat="server" Text='<%# Eval("SalesChannelName") %>'></asp:Label>
                                                            </ItemTemplate>

                                                            <HeaderTemplate>
                                                                <div style="padding-bottom: 5px;">
                                                                    Sale Channel Name
                                                                </div>
                                                                <div>
                                                                    <asp:Label ID="lblSaleChannelNameHeading" runat="server"></asp:Label>
                                                                </div>
                                                            </HeaderTemplate>

                                                            <HeaderStyle CssClass="gridheader" />
                                                        </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="State" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblStateName" runat="server" Text='<%# Eval("StateName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <div class="float-margin padding-top">
                                        State Name
                                    </div>
                                    <div class="float-left">
                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="formselect">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rqddlState" runat="server" ControlToValidate="ddlState"
                                            CssClass="error" Display="Dynamic" ErrorMessage="Please select State." ValidationGroup="Save"
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                    </div>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="gridheader" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SalesChannelStateMappingID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <div class="float-margin padding-top">
                                        Status
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnAddPart" runat="server" CommandName="AddNDtoState" CssClass="buttonbg"
                                            Text="Add" ValidationGroup="Save" />
                                    </div>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="gridheader" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                        <AlternatingRowStyle CssClass="Altrow" />
                        <%-- <PagerStyle CssClass="PagerStyle" />--%>
                    </asp:GridView>
                </div>
                <div class="clear">
                </div>
                <div id="dvFooter" runat="server" class="pagination">
                    <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
            <asp:PostBackTrigger ControlID="gvNDToState" />
        </Triggers>
    </asp:UpdatePanel>



</asp:Content>
