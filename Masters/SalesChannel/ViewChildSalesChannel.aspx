<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewChildSalesChannel.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Masters_SalesChannel_ViewChildSalesChannel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/JsValidate.js"></script>

    <script type="text/javascript" language="JavaScript">
        function popup(url) {
            window.open(url, "ViewDetails", "width=600,height=500,scrollbars = 1,menubar=no,toolbar=no,addressbar=no,top=100,left=200,screenX=200,screenY=200")

        }
    </script>--%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script language="javascript" type="text/javascript">
        function popup(url) {
            var WinSalesChannelDetail = dhtmlmodal.open("ViewDetails", "iframe", "ViewSalesChannelDetail.aspx?SalesChannelId=" + url, "Sales Channel Detail", "width=500px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
        }
    </script>

    <script language="javascript" type="text/javascript">
        function ShowMyModalPopup() {

            var modal = $find('modPopup');
            modal.show();

        }



        function HideModalPopup() {
            if ($find('modPopup') == null) {
                return;
            }
            var modal = $find('modPopup');
            modal.hide();
        }
        function ShowMyModalPopup2() {

            var modal = $find('ModelDOAAlternate');
            modal.show();

        }
        function HideModalPopupConfirmation() {
            var modal = $find('ModelPopJustConfirmation');
            modal.hide();
        }

        function HideModalPopup2() {
            var modal = $find('ModelDOAAlternate');
            modal.hide();
        }
  

          
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        View Sales Channel
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblType" runat="server"></asp:Label>
                    Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbsaleschanneltype" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                </li>
                <li class="text">Sales Channel Name:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsaleschannelname" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                    <asp:Label ID="lblSelectedSalesChannelId" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblSelectedSalesChannelNumberofbackdays" runat="server" Text="" Visible="false"></asp:Label>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                            CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnShowAll" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                            OnClick="btnShowAll_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                            OnClick="btnCancel_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div id="dvhide" runat="server" visible="false">
        <div class="mainheading">
            List
        </div>
        <div class="export">
            <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
        </div>
        <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GridSalesChannel" runat="server" AlternatingRowStyle-CssClass="Altrow"
                            AutoGenerateColumns="false" bgcolor="" AllowPaging="true" PageSize='<%$ AppSettings:GridViewPageSize %>'
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="SalesChannelID"
                            FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                            GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                            OnPageIndexChanging="GridSalesChannel_PageIndexChanging" OnRowCommand="GridSalesChannel_RowCommand"
                            OnRowDataBound="GridSalesChannel_RowDataBound1">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LoginName" HeaderStyle-HorizontalAlign="Left" HeaderText="LogIn Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Password">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPassword" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Password"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblPasswordSalt" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"PasswordSalt"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:LinkButton ID="hlPassword" runat="server" CssClass="elink" Text="Password"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales ChannelCode"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ParentName" HeaderStyle-HorizontalAlign="Left" HeaderText="Parent Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OrgnHierarchyName" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="OrgnHierarchy Name" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Email" HeaderStyle-HorizontalAlign="Left" HeaderText="Email"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MobileNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesChannelTypename" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Sales Channel Type" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="View Details">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg" Text="View Details"></asp:HyperLink>
                                        <asp:HiddenField ID="HiddenApprovelLevel2" runat="server" Value='<%# Eval("ApprovalLevel2") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px" Visible="false">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblMapping" runat="server" Text='<%#Eval("Mapping") %>' Visible="false"></asp:Label>
                                        <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SalesChannelID") %>'
                                            CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px" Visible="false">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SalesChannelID") %>'
                                            CommandName="cmdEdit" ToolTip="Edit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                            Visible="false" />
                                        <asp:Label ID="lblLocked" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"isLockedOut"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:ImageButton ID="imgLocked" runat="server" CausesValidation="false" CommandArgument='<%#Eval("UserID") %>'
                                            ImageAlign="Top" CommandName="unlock" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px" Visible="false">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Button ID="imgEditNumberofBackdays" runat="server" CausesValidation="false"
                                            Text="Reset Number of back days" CommandArgument='<%#Eval("SalesChannelID") %>'
                                            CommandName="cmdEditNumberofBackDays" ToolTip="Edit Number of Back Days" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BackDays">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNumberofBackDaysSCDisplay" runat="server"></asp:Label>
                                        <asp:Label ID="lblNumberofBackDaysSC" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"NumberOfBackDaysForSC"))%>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ExportToExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:Panel ID="pnlModelSwap" runat="server" CssClass="popupbg" Width="80%">
        <asp:UpdatePanel ID="updModelSwap" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Change number of back days
                </div>
                <<div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblNumberofBackDays" runat="server" Text="Number of back days"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtNumberofbackdays" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmitNumberofBackDays" runat="server" CssClass="buttonbg" OnClick="btnSubmitNumberofBackDays_Click"
                                    Text="Submit" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancelConfirmation" runat="server" CssClass="buttonbg" OnClientClick="HideModalPopupConfirmation()"
                                    Text="Cancel" CausesValidation="false" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="ModelPopJustConfirmation" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancelConfirmation" Drag="True" OnCancelScript="HideModalPopupConfirmation()"
        PopupControlID="pnlModelSwap" TargetControlID="btnTarget1" DynamicServicePath=""
        Enabled="True">
    </cc1:ModalPopupExtender>
    <asp:LinkButton ID="btnTarget1" runat="server"></asp:LinkButton>
</asp:Content>
