<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ApproveRetailer.aspx.cs" Inherits="Masters_HO_Retailer_ApproveRetailer"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucServiceEntity.ascx" TagName="ucServiceEntity"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script language="javascript" type="text/javascript">
        function popup(url) {
            var WinRetailerDetails = dhtmlmodal.open("ViewDetails", "iframe", "ViewRetailerDetail.aspx?RetailerId=" + url, "Retailer Detail", "width=500px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
        }


        function showallclick() {
            $("[id$=cmbsaleschannel]").val("0");


        }
    </script>

    <%-- <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/JsValidate.js"></script>--%>
    <%-- <script type="text/javascript" language="JavaScript">
		function popup(url)
		{
		    window.open(url, "ViewDetails", "width=600,height=500,scrollbars = 1,menubar=no,toolbar=no,top=100,left=200,screenX=200,screenY=200")
			
		}
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Search Retailer
    </div>
    <%-- <asp:LinkButton ID="LBAddRetailer" runat="server" CausesValidation="False" OnClick="LBAddRetailer_Click"
                                                                    CssClass="elink7">Add Retailer</asp:LinkButton>--%>


    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Channel:
                </li>
                <li class="field">
                    <asp:UpdatePanel ID="updateSales" runat="server">
                        <ContentTemplate>
                            <uc2:ucServiceEntity ID="ucServiceEntity" runat="server" BindSaleschannel="1" /> <%-- #CC20 BindSaleschannel Added --%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </li>
                <li class="text">Retailer Name:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtRetailername" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                </li>
                <li class="text">Retailer Code:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtRetailerCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                </li>
            </ul>
            <ul>
                <li class="text">Approval Status: <%--<span class="mandatory">&nbsp;</span>--%>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlApproveStatus" runat="server" CssClass="formselect">
                        <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Rejected" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                            Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnShowAll" OnClientClick="javascript:showallclick()" runat="server"
                            CssClass="buttonbg" Text="Show All" OnClick="btnShowAll_Click" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                            OnClick="btnCancel_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="dvhide" runat="server" visible="false">
                    <div class="mainheading">
                        List
                    </div>
                    <div class="export">
                        <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
                    </div>
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="GridRetailer" runat="server" AlternatingRowStyle-CssClass="Altrow"
                                AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                DataKeyNames="RetailerID" PageSize='<%$ AppSettings:GridViewPageSize %>' FooterStyle-CssClass="gridfooter"
                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                SelectedStyle-CssClass="gridselected" Width="100%" OnRowCommand="GridRetailer_RowCommand"
                                OnRowDataBound="GridRetailer_RowDataBound" AllowPaging="false">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField DataField="RetailerCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Code"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ParentRetailerName" HeaderStyle-HorizontalAlign="Left"
                                        HeaderText="Parent Retailer Name" HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LoginName" HeaderStyle-HorizontalAlign="Left" HeaderText="Login ID"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Password">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPassword" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Password"))%>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblPasswordSalt" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"PasswordSalt"))%>'
                                                Visible="false"></asp:Label>
                                            <asp:LinkButton ID="hlPassword" runat="server" Text="Password" Visible="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--#CC11 Add Start--%>
                                    <asp:TemplateField HeaderText="ND Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNDName" runat="server" Text='<%#Eval("ND")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--#CC11 Add End--%>

                                    <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Code"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="LocationName" HeaderStyle-HorizontalAlign="Left" HeaderText="Location Name"
                                                                                    HtmlEncode="true">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>--%>
                                    <%--<asp:BoundField DataField="SalesmanName" HeaderStyle-HorizontalAlign="Left" HeaderText="Salesman Name"
                                                                                    HtmlEncode="true">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>--%>
                                    <%--<asp:BoundField DataField="Email" HeaderStyle-HorizontalAlign="Left" HeaderText="Email"
                                                                                    HtmlEncode="true">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>--%>

                                    <asp:BoundField DataField="MobileNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <%-- #CC17 Add Start --%>
                                    <asp:BoundField DataField="FirstLevelStausBy" HeaderStyle-HorizontalAlign="Left" HeaderText="First Level Status By"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FirstLevelStaus" HeaderStyle-HorizontalAlign="Left" HeaderText="First Level Staus"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SecondLevelStausBy" HeaderStyle-HorizontalAlign="Left" HeaderText="Second Level StausBy"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SecondLevelStaus" HeaderStyle-HorizontalAlign="Left" HeaderText="Second Level Staus"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%-- #CC17 Add End --%>

                                    <%--#CC02 Add Start--%>
                                    <asp:BoundField DataField="ApprovalStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="Approval Status"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%--#CC02 Add End--%>
                                    <%--#CC01 Add Start--%>
                                    <asp:BoundField DataField="ApprovalRemarks" HeaderStyle-HorizontalAlign="Left" HeaderText="Approval/Rejection Remarks"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>



                                    <%--#CC01 Add End--%>

                                    <asp:TemplateField HeaderText="View Details">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg"></asp:HyperLink>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblParentSalesChannelId" runat="server" Text='<%#Eval("SalesChannelID")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("RetailerID") %>'
                                                            CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Approve" ItemStyle-Width="85px">
                                        <ItemStyle Wrap="False" />
                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                        <ItemTemplate>

                                            <cc2:ZedImageButton ID="img1" runat="server" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/Approval.png"%>'
                                                ToolTip="(Approve)" CommandName="cmdEdit" CommandArgument='<%#Eval("RetailerID") %>'
                                                CausesValidation="False" AlternateText="Approve" Visible='<%#Convert.ToBoolean(Eval("RetailerApprovalIcon"))%> '
                                                ActionTag="Add" />
                                            <%--#CC18 "RetailerApprovalIcon" is used instead of "ApproveStatus" --%>
                                            <%--#CC03  Action Tag changed from Edit to Add--%>
                                            <%--#CC03 Add Start--%>
                                            <cc2:ZedImageButton ID="ZedImgEdit" runat="server" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' ToolTip="(Edit)" CommandName="EditRetailer" CommandArgument='<%#Eval("RetailerID") %>' CausesValidation="False" AlternateText="Edit" ActionTag="Edit" Visible="false" />
                                            <%--#CC03 Add End--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="Altrow" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="dvFooter" runat="server" class="pagination">
                            <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnCancel" />
                <%--<asp:AsyncPostBackTrigger ControlID="btnSearch" />--%> <%--#CC06 Commented--%>
                <asp:PostBackTrigger ControlID="btnSearch" />
                <%--#CC06 Added--%>
                <asp:AsyncPostBackTrigger ControlID="btnShowAll" />
                <asp:PostBackTrigger ControlID="ExportToExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
