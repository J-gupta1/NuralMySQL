<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaskManagementReport.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Reports_Common_TaskManagementReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <uc4:ucMessage ID="ucMessage1" runat="server" />
                    <div class="clear"></div>
                    <div class="mainheading">
                        View Task Detail 
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory. (+) Fill at least one of them.
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Entity Type:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlEntityType" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">Entity Type Name:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlEntityTypeName" CssClass="formselect" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">Task Priority:<font class="error">+</font></li>
                                <li class="field">

                                    <asp:DropDownList CausesValidation="true" ID="ddlTaskPriority" runat="server" CssClass="formselect">
                                    </asp:DropDownList>

                                </li>
                                <li class="text">Task Status:<span class="error">+</span>
                                </li>
                                <li class="field">

                                    <asp:DropDownList ID="ddlTaskStatus" runat="server" CssClass="formselect">
                                    </asp:DropDownList>

                                </li>
                                <li class="text">Start Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">End Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>

                                <li class="text">Task Group:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:DropDownList ID="ddlTaskGroup" runat="server" CssClass="formselect">
                                        </asp:DropDownList>
                                    </div>

                                </li>
                                <li class="text"></li>
                                <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnExportexcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                            CausesValidation="true" OnClick="btnExportexcel_Click" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="PnlGrid" Visible="false" runat="server">
                        <div class="mainheading">
                            View Task Details                                                    
                        </div>
                        <div class="contentbox">
                            <div class="grid">
                                <asp:GridView ID="gvTaskDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" DataKeyNames="TaskUserId" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowCommand="gvTaskDetail_RowCommand">
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                        <asp:BoundField DataField="Task" HeaderStyle-HorizontalAlign="Left" HeaderText="Task"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TaskGroupName" HeaderStyle-HorizontalAlign="Left" HeaderText="Task GroupName"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CreatedByEntityType" HeaderStyle-HorizontalAlign="Left" HeaderText="Task CreatedBy EntityType"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TaskCreatedByUserName" HeaderStyle-HorizontalAlign="Left" HeaderText="Task CreatedBy User Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CreatedToEntityType" HeaderStyle-HorizontalAlign="Left" HeaderText="Task CreatedTo EntityType"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TaskCreatedTouserName" HeaderStyle-HorizontalAlign="Left" HeaderText="Task CreatedTo User Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="TaskStartDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Task StartDate"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="TaskEndDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Task EndDate"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TaskPriorty" HeaderStyle-HorizontalAlign="Left" HeaderText="Task Priorty"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TaskStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="Task Status"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="View Response Detail" ItemStyle-Width="85px">
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnViewTransactions" CssClass="buttonbg" runat="server" CommandName="ViewDetail"
                                                    CommandArgument='<%#Eval("TaskUserId") %>'>View Response Detail</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="dvFooter" runat="server" class="pagination">
                                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <div class="clear"></div>

                <div class="clear"></div>

                <div id="tblResponsedetail" runat="server" visible="false">
                    <div class="mainheading">
                        Response Detail
                    </div>
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="gvResponseDetail" runat="server" DataKeyNames="TaskResponseID" EmptyDataText="No Response Detail Found" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                                AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                RowStyle-VerticalAlign="top" Width="100%" PageSize="50" OnRowDataBound="gvResponseDetail_RowDataBound">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField DataField="CreatedByName" HeaderStyle-HorizontalAlign="Left" HeaderText="User Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TaskStatusName" HeaderStyle-HorizontalAlign="Left" HeaderText="Task Status"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderStyle-HorizontalAlign="Left" HeaderText="Remark"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NextClosureDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Next Closure Date"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CreatedOn" HeaderStyle-HorizontalAlign="Left" HeaderText="CreatedOn"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Image Attachment">
                                        <ItemTemplate>
                                            <asp:GridView ID="gvAttachedImages" runat="server" AutoGenerateColumns="false" BorderWidth="0px"
                                                ShowHeader="false" CellPadding="0" GridLines="None" CssClass="table-panel" HeaderStyle-VerticalAlign="top"
                                                HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                                DataKeyNames="Attachement">
                                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblImagePath" runat="server" Text='<%# Eval("Attachement") %>'
                                                                Style="display: none"></asp:Label>
                                                            <asp:LinkButton ID="lnkDownload" Text="View Attachment" CssClass="elink2" CommandArgument='<%# Eval("Attachement") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="Altrow" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportexcel" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <asp:PostBackTrigger ControlID="btnSearch" />
                <asp:PostBackTrigger ControlID="gvTaskDetail" />
                <asp:PostBackTrigger ControlID="gvResponseDetail" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
