<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageSalesChannelType.aspx.cs" Inherits="Masters_SalesChannel_ManageSalesChannelType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add / Edit Sales Channel Type
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="H25-C2">
                    <ul>
                        <li class="text">
                            <asp:Label ID="label1" runat="server" Text="">Sales Channel Type Name:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtsalesChanneltypeName" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtsalesChanneltypeName"
                                CssClass="error" ErrorMessage="Please insert Sales Channel Type Name " ValidationGroup="insert" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtsalesChanneltypeName"
                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                            </cc1:FilteredTextBoxExtender>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblstate" runat="server" Text="Parent Sales Channel Type:<span class='error'>*</span>"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbParentSalesChannelType" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valstate" ControlToValidate="cmbParentSalesChannelType"
                                    CssClass="error" ErrorMessage="Please select a Parent Sales Channel Type " InitialValue="0"
                                    ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="Label2" runat="server" Text="Hierarchy Level:<span class='error'>*</span>"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbHierarchyLevel" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbHierarchyLevel"
                                    CssClass="error" ErrorMessage="Please select a Hierarchy " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="label3" runat="server" Text="">Sales Channel Type Group Name:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtgroupName" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtgroupName"
                                CssClass="error" ErrorMessage="Please insert Sales channel Type Name " ValidationGroup="insert" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtgroupName"
                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                            </cc1:FilteredTextBoxExtender>
                        </li>
                        <li class="text-field">
                            <span class="padding-right">
                                <asp:Label ID="Label4" runat="server" Text="Bill To Retailer:"></asp:Label>
                            </span>
                            <span class="padding-right">
                                <asp:CheckBox ID="chkBilToRetailer" runat="server" Checked="true" />
                           </span>
                            <span class="padding-right">
                                <asp:Label ID="Label5" runat="server" Text="Is Auto Generate:"></asp:Label>
                            </span>
                            <span class="padding-right">
                                <asp:CheckBox ID="chkIsAutoGenerate" runat="server" Checked="true" />
                           </span>
                            <span class="padding-right">
                                <asp:Label ID="Label6" runat="server" Text="Is Return For Invoice PTO:"></asp:Label>
                           </span>
                            <span class="padding-right">
                                <asp:CheckBox ID="ChkPTO" runat="server" Checked="true" />
                           </span>
                        </li>
                    </ul>
                    <ul>                       
                        <li>
                              <div class="float-margin">
                            <asp:Button ID="btnsubmit" runat="server" OnClick="btninsert_click" Text="Submit"
                                ValidationGroup="insert" CssClass="buttonbg" />
                                  </div>
                              <div class="float-margin">
                            <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"
                                Text="Cancel" CssClass="buttonbg" />
                                  </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        List
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdSalesChannelType" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                        AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                        DataKeyNames="SalesChannelTypeID" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" OnRowCommand="grdSalesChannelType_RowCommand"
                        RowStyle-CssClass="gridrow" EmptyDataText="No record found" RowStyle-HorizontalAlign="left"
                        RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdSalesChannelType_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:BoundField DataField="SalesChannelTypeName" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="Sales Channel Type Name" HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ParentSalesChannelTypeName" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="Parent Sales Channel Type Name" HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="HierarchyLevelName" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="Hierarchy Level" HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SalesChannelTypeGroupName" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="Sales Channel Type Group Name" HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BillToRetailer1" HeaderStyle-HorizontalAlign="Left" HeaderText="Bill To Retailer"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IsAutoGenerate1" HeaderStyle-HorizontalAlign="Left" HeaderText="Is Auto Generate"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ReturnForInvoicePTO1" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="Return for Invoice PTO" HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SalesChannelTypeID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SalesChannelTypeID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <%--<Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="img1" EventName="cmdEdit" />
                                                 <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                      </Triggers>     --%>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
