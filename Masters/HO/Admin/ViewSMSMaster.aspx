<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewSMSMaster.aspx.cs" Inherits="Masters_HO_Admin_ViewSMSMaster" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdncmd" runat="server" />
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="mainheading">
        Search panel
    </div>
    <div class="export">
        <a href="ManagerSMSMaster.aspx" class="elink7">Add SMS Master</a>
    </div>
    <div class="contentbox">
         <div class="mandatory">
                (*) Marked fields are mandatory
            </div>
        <div class="H35-C3-S">           
            <ul>
                <li class="text">SMS Description: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtSMSDesc" runat="server" CausesValidation="True" MaxLength="200"
                            ValidationGroup="aa" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter SMS Description"
                            ControlToValidate="txtSMSDesc" ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" ValidationGroup="aa"
                            OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonbg" OnClick="btnClear_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>

    <%--<uc1:ucMessage ID="ucMsg" runat="server" />--%>

    <div class="mainheading">
        View panel
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="gvSMSMaster" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="gvSMSMaster_PageIndexChanging"
                OnRowCommand="gvSMSMaster_RowCommand">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="SMSOutboundTransKeyword" HeaderStyle-HorizontalAlign="Left"
                        HeaderText="SMS Keyword" HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SMSOutboundTransDesc" HeaderStyle-HorizontalAlign="Left"
                        HeaderText="SMS Description" HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SMSFrom" HeaderStyle-HorizontalAlign="Left" HeaderText="SMS From"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SMSContent" HeaderStyle-HorizontalAlign="Left" HeaderText="SMS Content"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SMSOutboundTransID") %>'
                                CommandName="togglecmd" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SMSOutboundTransID") %>'
                                CommandName="editcmd" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                <AlternatingRowStyle CssClass="Altrow" />
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
            <div class="pagination">
            </div>
        </div>
        <div class="clear">
        </div>
    </div>

</asp:Content>
