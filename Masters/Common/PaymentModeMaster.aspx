<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentModeMaster.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Masters_Common_PaymentModeMaster" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCUserDetail.ascx" TagName="UCUserDetail" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ucStatusControl.ascx" TagName="ucStatus" TagPrefix="uc5" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="box1">
        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div runat="server" id="dvMsg">
                    <uc4:ucMessage ID="ucMsg" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="subheading">
            Category  Save
           
        </div>
        <asp:Panel ID="pnlSubmit" runat="server" DefaultButton="btnSubmit">
            <asp:UpdatePanel ID="updSave" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="innerarea">
                        <div class="H25-C3">

                            <ul>
                                <li class="text">PaymentMode Name:<span class="error">*</span></li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtPaymentModeName" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvtxtcategoryname" runat="server" ErrorMessage="Please Enter PaymentMode Name"
                                            ControlToValidate="txtPaymentModeName" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </li>
                                <li class="text">PaymentMode Code:<span class="error">*</span></li>
                                <li class="field">
                                    <div>
                                         <asp:TextBox ID="txtPaymentModeCode" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                                    </div>
                                    <div>
                                       <asp:RequiredFieldValidator ID="rvgtxtPaymentModeCode" runat="server" ErrorMessage="Please Enter PaymentMode Code"
                                            ControlToValidate="txtPaymentModeCode" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </li>
                                <li class="text">
                                    <div class="float-margin padding-right">
                                        Active:<span class="optional-img">&nbsp;</span>
                                    </div>
                                    <div class="float-margin">
                                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                    </div>
                                </li>
                                <li class="field">
                                    <div class="float-margin">

                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" ValidationGroup="submit" OnClick="btnSubmit_Click" />
                                    </div>
                                    <div class="float-margin">

                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" OnClick="btnCancel_Click" />
                                    </div>
                                </li>
                            </ul>

                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <div class="clear">
    </div>
    <div class="box1">
        <div class="subheading">
            Search Payment Mode
        </div>
        <asp:UpdatePanel ID="updSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="innerarea">
                    <div class="H20-C3">
                        <ul>
                            <li class="text">PaymentMode Name:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtPaymentModeNameSearch" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                            </li>
                            <li class="text">PaymentMode Code:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtPaymentModeCodeSearch" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                            </li>
                            <li class="text">Status:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlStatusSearch" runat="server" CssClass="formselect">
                                    <asp:ListItem Text="Select" Value="255"></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="buttonbg"
                                        OnClick="btnShowAll_Click" />
                                </div>
                            </li>
                        </ul>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="updpnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="box1" runat="server" id="divgrd">
                <div class="subheading">
                    <div class="float-left">
                        Payment Mode List
                    </div>

                </div>
                <div class="export">
                    <asp:Button ID="Exporttoexcel" runat="server"
                        OnClick="Exporttoexcel_Click" CausesValidation="False" CssClass="excel" />
                </div>
                <div class="innerarea">
                    <div class="grid1">
                        <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="PaymentModeID" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" EmptyDataText="No record found"
                            RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" OnRowCommand="grdvList_RowCommand">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="PaymentModeName" HeaderText="PaymentMode Name" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PaymentModeCode" HeaderText="PaymentMode Code" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ActiveStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CreatedOn" HeaderText="Created On" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="80px" />
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("PaymentModeID") %>'
                                            CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("PaymentModeID") %>'
                                            CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Exporttoexcel" />
            <asp:PostBackTrigger ControlID="grdvList" />
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="btnCancel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
