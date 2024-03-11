<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageColor.aspx.cs" Inherits="Masters_HO_Common_ManageColor" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <%-- <uc2:header ID="Header1" runat="server" />--%>

        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMessage1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit Color
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblColornm" runat="server" AssociatedControlID="txtColorName" CssClass="formtext">Color Name:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtColorName" runat="server" CssClass="formfields" MaxLength="30"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                               
                                <asp:RequiredFieldValidator ID="reqColorName" runat="server" ControlToValidate="txtColorName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Color Name." SetFocusOnError="true"
                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtColorName" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                            </li>
                            <li class="text">
                                <div class="float-margin">
                                    <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status :</asp:Label>
                                </div>
                                <div class="float-margin">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                </div>
                            </li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="True" ValidationGroup="AddUserValidationGroup"
                                        ToolTip="Add " CssClass="buttonbg" OnClick="btnCreate_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                        OnClick="btncancel_click" CausesValidation="False" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <%--#CC01 Add Start --%>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnCreate" />
            </Triggers>
            <%--#CC01 Add End --%>
            <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
        </asp:UpdatePanel>

        <div class="mainheading">
            Search Color
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H20-C3-S">
                        <ul>
                            <li class="text">Color Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSerColorName" runat="server" MaxLength="100" CssClass="formfields"> </asp:TextBox>
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnserch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click" CausesValidation="False"></asp:Button>
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="getalldata" Text="Show All Data" runat="server" ToolTip="Search"
                                        CssClass="buttonbg" OnClick="btngetalldta_Click" CausesValidation="False"></asp:Button>
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
                <%--#CC01 Add Start --%>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnserch" />
                </Triggers>
                <%--#CC01 Add End --%>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            List
        </div>
        <div class="export">
            <asp:Button ID="Exporttoexcel" runat="server" CausesValidation="False" CssClass="excel"
                OnClick="Exporttoexcel_Click" Text="" ToolTip="Export to Excel" />
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grdColor" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                            RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                            AllowPaging="True" SelectedStyle-CssClass="gridselected" DataKeyNames="ColorID"
                            PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowCommand="grdColor_RowCommand"
                            EmptyDataText="No record found" OnPageIndexChanging="grdColor_PageIndexChanging">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ColorName"
                                    HeaderText="Color Name"></asp:BoundField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnActiveDeactive" runat="server" CommandArgument='<%#Eval("ColorID") %>'
                                            CommandName="Active" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("ColorID") %>' runat="server" ID="btnEdit"
                                            CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                            ToolTip="Edit User" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <%--#CC01 Add Start --%>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grdColor" />
                    </Triggers>
                    <%--#CC01 Add End --%>
                    <%-- <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
                </asp:UpdatePanel>
            </div>
        </div>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>
