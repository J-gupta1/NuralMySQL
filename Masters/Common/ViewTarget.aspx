<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewTarget.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    Inherits="ViewTarget" EnableEventValidation="false" %>

<%--#CC03  EnableEventValidation="false" Added--%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script type="text/javascript" language="JavaScript">

        function popup(url) {
            var WinDetails = dhtmlmodal.open("ViewDetails", "iframe", "ViewTargetDetail.aspx?TargetID=" + url, "Target Detail", "width=760px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
        }

        /* #CC03 Add Start */
        function popupEdit(url) {
            try {

                var WinDetails = dhtmlmodal.open("EditDetails", "iframe", "ViewTargetDetail.aspx?TargetID=" + url + "&Edit=1", "Target Detail", "width=760px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
                WinDetails.onclose = function () {

                    var btn = document.getElementById("ctl00_contentHolderMain_Search");
                    if (btn != null) {
                        __doPostBack(btn.name, "OnClick");
                        return true;
                    }
                }
                return false;
            }
            catch (err) {
                alert(err);
            }
        }
        /* #CC03 Add End */

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <uc4:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
                    <div class="mainheading">
                        View Target
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                             (*) Marked fields are mandatory. (+) Marked fields are optional.
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Select User Type: <span class="error">*</span>
                                </li>
                                <li class="field">
                                    <div>
                                        <%--<asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                            CssClass="formselect">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">Hierarchy Level</asp:ListItem>
                                            
                                            <asp:ListItem Value="2">SalesChannel Type</asp:ListItem>
                                            <asp:ListItem Value="101">Retailer</asp:ListItem>
                                            
                                        </asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged" AutoPostBack="true">
                                            
                                        </asp:DropDownList>
                                        <div>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Search"
                                                runat="server" Display="Dynamic" InitialValue="0" ControlToValidate="ddlType"
                                                CssClass="error" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Search"
                                            runat="server" Display="Dynamic" InitialValue="0" ControlToValidate="ddlUserType"
                                            CssClass="error" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                --%>

                                        </div>
                                    </div>
                                </li>
                                
                                <li class="text">Target Name: <span class="error">+</span></li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlTarget" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                </li>
                                <%-- #CC04 Add End --%>
                            <%--</ul>
                            <ul>--%>
                                <li class="text">From Date: <span class="error">+</span>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucDatePicker1" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">To Date: <span class="error">+</span>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucDatePicker2" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>

                               </ul>

                             <div class="setbbb">
                                    <div class="float-margin">
                                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="buttonbg"
                                        CausesValidation="true" ValidationGroup="Search" OnClick="Search_Click" />
                                    </div>
                                    <div class="float-margin">
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                        CausesValidation="false" OnClick="Cancel_Click" />
                                    </div>
                                </div>
                           
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="dvhide" runat="server" Visible="true">
                        <div class="mainheading">
                            List
                        </div>
                        <div class="export">
                            <asp:Button ID="btnExport" runat="server" CssClass="excel" Text="" OnClick="btnExport_Click" />
                        </div>
                        <div class="contentbox">
                            <div class="grid1">
                                <asp:GridView ID="GridTarget" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    DataKeyNames="TargetID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" OnRowCommand="GridTarget_RowCommand" OnRowDataBound="GridTarget_RowDataBound"
                                    OnPageIndexChanging="GridTarget_PageIndexChanging">
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                        <%--#CC01 Add Start--%>
                                        <asp:BoundField DataField="TargetName" HeaderStyle-HorizontalAlign="Left" HeaderText="Target Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <%--#CC01 Add End--%>
                                        <asp:BoundField DataField="TargetFor" HeaderStyle-HorizontalAlign="Left" HeaderText="Target For"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TargetType" HeaderStyle-HorizontalAlign="Left" HeaderText="Target Type"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TargetFrom" HeaderStyle-HorizontalAlign="Left" HeaderText="Target From"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TargetTo" HeaderStyle-HorizontalAlign="Left" HeaderText="Target To"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalTarget" HeaderStyle-HorizontalAlign="Left" HeaderText="Target"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="View Details">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="85px"><%-- #CC03 Headertext changed from Delete to Action --%>
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                            <ItemTemplate>
                                                <%--#CC03 Add Start--%>
                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false"
                                                    CommandArgument='<%#Eval("TargetID") %>' ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />

                                                <asp:HiddenField ID="hdnTargetTo" runat="server" Value='<%#Eval("TargetTo")%>' />
                                                <%--#CC03 Add End--%>
                                                <asp:ImageButton ID="img2" runat="server" CausesValidation="false" OnClientClick="return ConfirmDelete();"
                                                    CommandArgument='<%#Eval("TargetID") %>' CommandName="cmdDelete" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
                <asp:PostBackTrigger ControlID="Search" />
                <%--#CC01 Added --%>
                <asp:PostBackTrigger ControlID="GridTarget" />
                <%--#CC04 Added --%>
                <%--<asp:PostBackTrigger ControlID="ddlType" />--%>
                <asp:PostBackTrigger ControlID="ddlUserType" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
