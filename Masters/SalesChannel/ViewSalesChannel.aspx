<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ViewSalesChannel.aspx.cs" Inherits="Masters_HO_SalesChannel_ViewSalesChannel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl" TagPrefix="uc3" %>
<%--#CC07 Added --%>


<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        function HideModalPopup() {
            if ($find('modPopup') == null) {
                return;
            }
            var modal = $find('modPopup');
            modal.hide();
        }

        function HideModalPopupConfirmation() {

            var modal = $find('ctl00_contentHolderMain_ModelPopJustConfirmation_backgroundElement');
            if (modal == null) {
                return;
            }
            modal.hide();
        }




    </script>

    <script language="javascript" type="text/javascript">


        function HideModalPopup_DispatchPopup() {
            var modal = $find('ModalPopupExtender1');
            modal.hide();
        }

    </script>

</asp:Content>
<%--* 14-June-2018, Rajnish, #CC01, Number Of Back Days for Sale Returns--%>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
            <asp:HiddenField ID="txt" runat="server" />
            <div class="mainheading">
                View Sales Channel
            </div>
            <div class="export">
                <asp:LinkButton ID="LBAddSalesChannel" runat="server" CausesValidation="False" OnClick="LBAddSalesChannel_Click"
                    CssClass="elink7">Add Sales Channel</asp:LinkButton>
            </div>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Sales Channel Type: <%--<span class="mandatory">*</span>--%>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbsaleschanneltype" runat="server" CssClass="formselect"
                                    OnSelectedIndexChanged="cmbsaleschanneltype_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <%-- <asp:RequiredFieldValidator ID="req" runat="server" CssClass="error" InitialValue="0"
                                                                        ControlToValidate="cmbsaleschanneltype" ErrorMessage="Please select sales channel type"
                                                                        Display="Dynamic" ValidationGroup="Serach"></asp:RequiredFieldValidator>--%>
                            </div>
                        </li>
                        <li class="text">Sales Channel Name:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtsaleschannelname" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                        </li>
                        <li class="text">Sales Channel Code:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtsaleschannelcode" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                            <asp:Label ID="lblSelectedSalesChannelId" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblSelectedSalesChannelNumberofbackdays" runat="server" Text="" Visible="false"></asp:Label>
                        </li>
                        <li class="text">Status: <span class="error">&nbsp;</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlactive" runat="server" CssClass="formselect">
                                <asp:ListItem Text="All" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </li>
                        <li class="text">Brand:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlBrand" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="text">Product Category:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlproductcategory" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                        <ul>
                            <li class="text">Brand:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="cmbBrandName" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                        </ul>
                    </asp:Panel>
                    <ul>
                        <li class=""></li>
                        <li class="field">
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
                    <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click2" />
                    <%-- #CC07 ExportToExcel_Click2 Added--%>
                </div>



                <%--      <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
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
                            OnRowDataBound="GridSalesChannel_RowDataBound">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true">
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
                                <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ParentName" HeaderStyle-HorizontalAlign="Left" HeaderText="Parent Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LocationName" HeaderStyle-HorizontalAlign="Left" HeaderText="Repo. Hierarchy Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="MobileNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>--%>
                                <asp:BoundField DataField="SalesChannelTypename" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Sales Channel Type" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Status Value" Visible="false">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Convert.ToInt16(Eval("Status"))==0?0:1%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="OpeningStockEntered" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Opening Stock" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BackDaysForListing" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Back Days" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--<asp:TemplateField HeaderText="BackDays">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNumberofBackDaysSCDisplay" runat="server" Text='<%#Convert.ToString(Eval("NumberOfBackDaysForSC"))=="-101"?"Default":Eval("NumberOfBackDaysForSC") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="BackDaysInternal" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNumberofBackDaysSC" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"NumberOfBackDaysForSC"))%>'
                                                                                Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SalesChannelID") %>'
                                            CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SalesChannelID") %>'
                                            CommandName="cmdEdit" ToolTip="Edit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                        <asp:ImageButton ID="imgEditNumberofBackdays" runat="server" CausesValidation="false"
                                            Text="Reset Number of back days" CommandArgument='<%#Eval("SalesChannelID") %>'
                                            CommandName="cmdEditNumberofBackDays" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/calendar-pre.png"%>'
                                            ToolTip="Edit Number of Back Days" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Online User" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblOnline" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"isLogin"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:ImageButton ID="imgOnline" runat="server" CausesValidation="false" CommandArgument='<%#Eval("UserID") %>'
                                            ImageAlign="Top" CommandName="Online" ToolTip='<%#LoginToolTip(Convert.ToInt16(Eval("isLogin"))) %>'
                                            ImageUrl='<%# LoginStatus(Convert.ToInt16(Eval("isLogin"))) %>' />&nbsp;
                                                                            <asp:Label ID="lblLocked" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"isLockedOut"))%>'
                                                                                Visible="false"></asp:Label>
                                        <asp:ImageButton ID="imgLocked" runat="server" CausesValidation="false" CommandArgument='<%#Eval("UserID") %>'
                                            ImageAlign="Top" CommandName="unlock" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Details">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Email" HeaderStyle-HorizontalAlign="Left" HeaderText="Email"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="ZSMName" HeaderStyle-HorizontalAlign="Left" HeaderText="ZSM Name"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ASOName" HeaderStyle-HorizontalAlign="Left" HeaderText="ASM Name"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>--%>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                        <%--#CC07 Add Start --%>
                        <div class="clear">
                        </div>
                        <%--#CC07 Add End--%>
                    </div>
                    <%--#CC07 Add Start --%>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>
                    <%--#CC07 Add End--%>
                </div>
                <%-- </ContentTemplate>
                                                      
                                                    </asp:UpdatePanel>--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Panel ID="pnlModelSwap" runat="server" CssClass="popupbg" Width="80%">
        <%--   <asp:UpdatePanel ID="updModelSwap" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>


        <div class="mainheading">
            Change number of back days:
        </div>
        <div class="contentbox">
            <div class="H25-C3">
                <ul>
                    <li class="text">
                        <asp:Label ID="lblNumberofBackDays" runat="server" Text="Number of back days :"></asp:Label>
                    </li>
                    <li class="field">
                        <%-- <asp:TextBox id="txtNumberofbackdays" runat="server" ></asp:TextBox>--%>
                        <input type="text" class="formfields" runat="server" id="txtNumberofbackdays" />
                        <asp:RegularExpressionValidator ID="rvDigits1" runat="server" ControlToValidate="txtNumberofbackdays"
                            ErrorMessage="Enter numbers only till 10 digit" ValidationGroup="Add" ForeColor="Red"
                            ValidationExpression="\d+" />
                    </li>

                    <%--   #CC01 start--%>

                    <li class="text">
                        <asp:Label ID="lblNumberofBackDaysReturns" runat="server" Text="Number of back days of Sale Return Invoice :"></asp:Label>
                    </li>
                    <li class="field">
                        <%-- <asp:TextBox id="txtNumberofbackdays" runat="server" ></asp:TextBox>--%>
                        <input type="text" class="formfields" runat="server" id="txtNumberofbackdaysSaleReturns" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNumberofbackdaysSaleReturns"
                            ErrorMessage="Enter numbers only till 10 digit" ValidationGroup="Add" ForeColor="Red"
                            ValidationExpression="\d+" />
                    </li>
                    <%--   #CC01 End--%>

                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnSubmitNumberofBackDays" runat="server" CssClass="buttonbg" OnClick="btnSubmitNumberofBackDays_Click"
                                Text="Submit" ValidationGroup="Add" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancelConfirmation" runat="server" CssClass="buttonbg" OnClientClick="HideModalPopupConfirmation()"
                                Text="Cancel" CausesValidation="false" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </asp:Panel>

    <cc1:ModalPopupExtender ID="ModelPopJustConfirmation" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancelConfirmation" Drag="True" OnCancelScript="HideModalPopupConfirmation()"
        PopupControlID="pnlModelSwap" TargetControlID="btnTarget1" DynamicServicePath=""
        Enabled="True">
    </cc1:ModalPopupExtender>
    <asp:LinkButton ID="btnTarget1" runat="server"></asp:LinkButton>

</asp:Content>
