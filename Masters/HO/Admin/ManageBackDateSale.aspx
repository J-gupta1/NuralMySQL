<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageBackDateSale.aspx.cs" Inherits="Masters_HO_Admin_ManageBackDateSale"
    Title="Untitled Page" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updgrid" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Panel ID="Updata" Visible="false" runat="server">
                    <div class="mainheading">
                        Edit Back Date Sales
                    </div>
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Sales Channel Type:<span class="error"></span>
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlSaleType" runat="server" CssClass="formselect" AutoPostBack="True"
                                        ValidationGroup="Add">
                                    </asp:DropDownList>
                                    <%--<br />
                                                        <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="ddlSaleType"
                                                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                                             ValidationGroup="Add"></asp:RequiredFieldValidator>--%>
                                </li>
                                <li class="text">Number Of Back Days:<span class="error"></span>
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtBackSaleDays" runat="server" CssClass="formfields" Width="50px" MaxLength="4"
                                        ValidationGroup="Add"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="ftbeOs" runat="server" FilterType="Numbers" ValidChars="0123456789"
                                        TargetControlID="txtBackSaleDays" />
                                    <asp:RequiredFieldValidator ID="reqSale" runat="server" ControlToValidate="txtBackSaleDays"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Days" ValidationGroup="Add"> </asp:RequiredFieldValidator>
                                    <%--<asp:RequiredFieldValidator ID="reqSale" runat="server" ControlToValidate="txtBackSaleDays"
                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Days" ValidationGroup="Add"> </asp:RequiredFieldValidator>--%>
                                </li>
                                <li class="text">Back Days For Sale Return:<span class="error"></span>
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtBackSaleDaysSaleReturn" runat="server" CssClass="formfields" MaxLength="4"
                                        ValidationGroup="Add"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" ValidChars="0123456789"
                                        TargetControlID="txtBackSaleDaysSaleReturn" />
                                    <asp:RequiredFieldValidator ID="reqBackSaleReturn" runat="server" ControlToValidate="txtBackSaleDaysSaleReturn"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Days" ValidationGroup="Add"> </asp:RequiredFieldValidator>


                                    <asp:RegularExpressionValidator ID="rvDigits" runat="server" ControlToValidate="txtBackSaleDaysSaleReturn"
                                        ErrorMessage="Enter numbers only till 10 digit" ValidationGroup="Add" ForeColor="Red"
                                        ValidationExpression="\d+" />
                                </li>
                                <li class="field3">
                                    <asp:Button ID="btnAdd" runat="server" ValidationGroup="Add" CausesValidation="true"
                                        CssClass="buttonbg" Text="Update" OnClick="btnAdd_Click" />
                                </li>
                                <%--<asp:RegularExpressionValidator ID="ValidateDate" runat="server" ControlToValidate="txtBackSaleDays"
                                                            CssClass="error" ValidationGroup="Add" ErrorMessage="Invalid Date" ValidationExpression="^(((0[1-9]|[12][0-9]|3[01])[- /.](0[13578]|1[02])[- /.]([1-9][0-9])\d\d$)|((0[1-9]|[12][0-9]|30)[-/.](0[13456789]|1[012])[- /.]([1-9][0-9])\d\d$)|((0[1-9]|1[0-9]|2[0-8])[-/.](02)[-/.]([1-9][0-9])\d\d$)|((29)[-/.](02)[-/.]((1[0-9]|[2-9]\d)(0[048]|[2468][048]|[13579][26])|((10|[2468][048]|[3579][26])00))))">
                                                        </asp:RegularExpressionValidator>--%>
                                <%--<td align="left" class="style1" valign="top">
                                                        <asp:Button ID="BtnAdd" runat="server" CssClass="buttonbg" Text="Add" CausesValidation="true"
                                                            OnClick="BtnAdd_Click" ValidationGroup="Add" Width="75px" />
                                                    </td>--%>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
                <%--<tr>
                                       <td colspan="6" height="20" class="mandatory" valign="top">
                                            (<span class="error">*</span>) marked fields are mandatory.
                                        </td>
                                    </tr>--%>
                <div class="mainheading">
                    Back Date Sale List
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdBkSaleList" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                            RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left" PageSize='<%$ AppSettings:GridViewPageSize %>'
                            HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                            AllowPaging="True" SelectedStyle-CssClass="gridselected" OnPageIndexChanging="grdBkSaleList_PageIndexChanging"
                            DataKeyNames="SalesChannelTypeID" OnRowCommand="grdBkSaleList_RowCommand">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:BoundField DataField="SalesChannelTypeName" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText=" Sales Channel Type Name" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NumberOfBackDays" HeaderStyle-HorizontalAlign="Left" HeaderText="Back Days Number"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                  <asp:BoundField DataField="BackDaysAllowedForSaleReturn" HeaderStyle-HorizontalAlign="Left" HeaderText="Back Days Number For Sales Return"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                <%--<asp:BoundField DataField="SalesChannelTypename" HeaderStyle-HorizontalAlign="Left"
                                                                            HeaderText="Sales Channel Type" HtmlEncode="true">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>--%>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SalesChannelTypeID") %>'
                                            CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Delete" ItemStyle-Width="85px">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelete2" runat="server" CausesValidation="False" CommandName="Delete"
                                                                    ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>' ToolTip="Cancel" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                        </asp:GridView>
                        <%--<Triggers>
                                                     <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
                        <%--<div style="float: left; padding-top: 10px; width: 600px;">
                        </div>--%>
                    </div>
                </div>
            </ContentTemplate>

            <%--#CC01 Add Start --%>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnAdd" />
                <asp:PostBackTrigger ControlID="grdBkSaleList" />
            </Triggers>
            <%--#CC01 Add End --%>
        </asp:UpdatePanel>
    </div>
</asp:Content>
