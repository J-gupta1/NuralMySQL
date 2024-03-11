<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DealerVisitAnalysis.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="ReportsDealerVisitAnalysis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>
    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>
    <script language="javascript" type="text/javascript">
        function popup(url) {
            var WinSalesChannelDetail = dhtmlmodal.open("ViewDetails", "iframe", "DealervisitDetail.aspx?SelectedTSMUserId=" + url, "Dealer Visit Detail", "width=500px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
            WinSalesChannelDetail.onclose = function () {
                //window.location.reload();
                return true;
            }
            return false;
        }
    </script>
    <style type="text/css">
        .elink366 {
            font-size: 13px;
            color: #0090a6;
            font-weight: bold;
            text-decoration: underline #0090a6;
            padding-right: 10px;
        }

        .elink367 {
            font-size: 11px;
            color: #e0110e;
            font-weight: bold;
            text-decoration: underline #606060;
        }
    </style>
    }

     
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <uc4:ucMessage ID="ucMessage1" runat="server" />
                    <div class="clear"></div>
                    <div class="mainheading">
                        Dealer Visit Analysis
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
                                <li class="text">From Date:<font class="error">*</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">To Date:<font class="error">*</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
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
                            Visit Analysis
                        </div>
                        <div class="contentbox">
                            <div class="grid">
                                <asp:GridView ID="gvVisitAnalysis" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" DataKeyNames="UserId" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowCommand="gvVisitAnalysis_RowCommand">
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                        <asp:BoundField DataField="Designation" HeaderStyle-HorizontalAlign="Left" HeaderText="Designation"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmployeeName" HeaderStyle-HorizontalAlign="Left" HeaderText="Employee"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmployeeCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Employee Code"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmployeePhone" HeaderStyle-HorizontalAlign="Left" HeaderText="Employee Phone"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="State" HeaderStyle-HorizontalAlign="Left" HeaderText="State"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton runat="server" ID="btnDealerCount" CssClass="elink366" CommandName="btnDealerCount"
                                                    Text="Dealer Count" CommandArgument="0" />  
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btnDealerCountTSM" CssClass="elink367" CommandName="btnDealerCountTSM"
                                                    Text='<%#Eval("DealerCount") %>' CommandArgument='<%#Eval("UserId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gridheader6" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton runat="server" ID="btnMoreThan5Visit" CssClass="elink366" CommandName="btnMoreThan5Visit"
                                                    Text="More Than 5 Visit" CommandArgument="0" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btnMoreThan5VisitTSM" CssClass="elink367" CommandName="btnMoreThan5VisitTSM"
                                                    Text='<%#Eval("MoreThan5Visit") %>' CommandArgument='<%#Eval("UserId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gridheader6" />
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton runat="server" ID="btn5Visit" CssClass="elink366" CommandName="btn5Visit"
                                                    Text="5 Visit" CommandArgument="0" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btn5VisitTSM" CssClass="elink367" CommandName="btn5VisitTSM"
                                                    Text='<%#Eval("5Visit") %>' CommandArgument='<%#Eval("UserId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gridheader6" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton runat="server" ID="btn4Visit" CssClass="elink366" CommandName="btn4Visit"
                                                    Text="4 Visit" CommandArgument="0" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btn4VisitTSM" CssClass="elink367" CommandName="btn4VisitTSM"
                                                    Text='<%#Eval("4Visit") %>' CommandArgument='<%#Eval("UserId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gridheader6" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton runat="server" ID="btn3Visit" CssClass="elink366" CommandName="btn3Visit"
                                                    Text="3 Visit" CommandArgument="0" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btn3VisitTSM" CssClass="elink367" CommandName="btn3VisitTSM"
                                                    Text='<%#Eval("3Visit") %>' CommandArgument='<%#Eval("UserId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gridheader6" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton runat="server" ID="btn2Visit" CssClass="elink366" CommandName="btn2Visit"
                                                    Text="2 Visit" CommandArgument="0" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btn2VisitTSM" CssClass="elink367" CommandName="btn2VisitTSM"
                                                    Text='<%#Eval("2Visit") %>' CommandArgument='<%#Eval("UserId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gridheader6" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton runat="server" ID="btn1Visit" CssClass="elink366" CommandName="btn1Visit"
                                                    Text="1 Visit" CommandArgument="0" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btn1VisitTSM" CssClass="elink367" CommandName="btn1VisitTSM"
                                                    Text='<%#Eval("1Visit") %>' CommandArgument='<%#Eval("UserId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gridheader6" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton runat="server" ID="btn0Visit" CssClass="elink366" CommandName="btn0Visit"
                                                    Text="0 Visit" CommandArgument="0" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btn0VisitTSM" CssClass="elink367" CommandName="btn0VisitTSM"
                                                    Text='<%#Eval("0Visit") %>' CommandArgument='<%#Eval("UserId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gridheader6" />
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
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportexcel" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <asp:PostBackTrigger ControlID="btnSearch" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
