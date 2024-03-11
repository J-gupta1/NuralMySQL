<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DSR_Details.aspx.cs" Inherits="Transactions_DSR_DSR_Details" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucPagingControl.ascx" TagName="ucPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="conditionfield" runat="server" />
            <div>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </div>
            <div class="mainheading">
                Search Details
            </div>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Hierarchy Level: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlHierarchy_Level" runat="server" CssClass="formselect" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlHierarchy_Level_SelectedIndexChanged">
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlHierarchy_Level"
                                    ErrorMessage="Please Select Hierarchy Level" InitialValue="0" CssClass="error" Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Hierarchy Name: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlHierarchy_Name" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="error" Display="Dynamic"
                                    ControlToValidate="ddlHierarchy_Name" ErrorMessage="Please Select Hierarchy Name"
                                    InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">DSR Month:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlDSR_Month" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="text">DSR Year:
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtDSR_Year" runat="server" CssClass="formfields" Width="50px" CausesValidation="True"
                                    MaxLength="4"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="error" Display="Dynamic"
                                    ControlToValidate="txtDSR_Year" ErrorMessage="It's not a year" ValidationExpression="^[0-9]{4}$"></asp:RegularExpressionValidator>

                                <asp:RangeValidator ID="RangeValidator1" runat="server" CssClass="error" Display="Dynamic" ControlToValidate="txtDSR_Year"
                                    ErrorMessage="Not In Date Range" MaximumValue="2050" MinimumValue="2000" SetFocusOnError="True"
                                    Type="Integer"></asp:RangeValidator>
                            </div>
                        </li>
                        <li>
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnClear" runat="server" Text="View All" CssClass="buttonbg" OnClick="btnClear_Click"
                                    CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>

            <%--<uc1:ucMessage ID="ucMsg" runat="server" />--%>

            <div class="mainheading">
                Details
            </div>
            <div class="contentbox">
                <div class="grid1">
                    <asp:GridView ID="GvDSRDetails" runat="server" AutoGenerateColumns="False" AlternatingRowStyle-CssClass="Altrow"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                        HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                        RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                        SelectedStyle-CssClass="gridselected" Width="100%">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:TemplateField HeaderText="Hierarchy Level">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("HierarchyLevelName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hierarchy Name">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%#Eval("LocationName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DSR Year/Month">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%#Eval("DSR_Year_Month") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Upload Date">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%#Eval("RecordCreationDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" CssClass="elink2" runat="server" NavigateUrl='<%#  "../DSR/Downloading.aspx?FileName=" +Eval("FileName")+"&FilePath="+"~/Excel/Upload/DSR_Upload/"  %>'
                                        Text='<%#Eval("FileName") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                        <AlternatingRowStyle CssClass="Altrow" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
                    <div class="pagination">
                        <uc3:ucPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>
                
                <div class="clear">
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
