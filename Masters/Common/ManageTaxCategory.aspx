<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageTaxCategory.aspx.cs" Inherits="ManageTaxCategory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/ucStatusControl.ascx" TagName="ucStatus" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updpnlSaveData" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="box1">
                <asp:UpdatePanel ID="updpnlMSG" runat="server">
                    <ContentTemplate>
                        <uc4:ucMessage ID="ucMessage1" runat="server" />
                        <div class="clear"></div>
                            <div class="mainheading">
                                <div>
                                    Add Tax Category
                                </div>
                            </div>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="innerarea">
                    <%--#CC02: class changed--%>
                    <div class="H35-C3-S">
                        <%--#CC02: class changed--%>
                        <ul>
                            <%--#CC02: li class changed START--%>
                            <li class="text">Tax Category Name:<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <div>
                                    <cc2:ZedTextBox CssClass="form_textarea" ID="txtTaxGroup" runat="server" TextMode="MultiLine"
                                        MaxLength="100" ActionTag="Add"></cc2:ZedTextBox>
                                </div>
                                <div>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtTaxGroup"
                                        FilterMode="InvalidChars" InvalidChars="`~!@#$%^&*()_-+=[]{}:;'.<>/?">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="Reqoldpass" runat="server" ControlToValidate="txtTaxGroup"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Tax group name!"
                                        ForeColor="" meta:resourcekey="ReqoldpassResource1" ValidationGroup="state"></asp:RequiredFieldValidator>
                                </div>
                                <div class="clear">
                                </div>
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <cc2:ZedButton CssClass="buttonbg" ID="btnSave" runat="server" OnClick="btnSave_Click"
                                        Text="Save" ValidationGroup="state" ActionTag="Add" />
                                </div>
                                <div class="float-left">
                                    <asp:Button CssClass="buttonbg" ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click" />
                                </div>
                            </li>
                        </ul>
                        <%--#CC02: li class changed END--%>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdSrch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="box1">
                <div class="mainheading">
                    <%--#CC02: class changed--%>
                    Search
                </div>
            <div class="innerarea">
                <%--#CC02: class changed--%>
                <div class="H20-C3-S">
                    <%--#CC02: class changed--%>
                    <ul>
                        <li class="text">Tax Category Name:<span class="optional-img">&nbsp;</span></li>
                        <li class="field">
                            <asp:TextBox CssClass="formfields" ID="txtSearchTaxCategoryName" runat="server" MaxLength="100" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="error"
                                Display="Dynamic" ControlToValidate="txtSearchTaxCategoryName" ErrorMessage="Invalid char(s)!"
                                ValidationExpression="[^<>@%]{1,50}$" SetFocusOnError="true"></asp:RegularExpressionValidator>
                            <%--#CC02: <br /> commented--%>
                        </li>
                        <li>
                            <uc5:ucStatus ID="ucStatus" runat="server" />
                        </li>
                        <li class="field3"><%--#CH01--%><%--#CC02: class changed--%>
                            <div class="float-margin">
                                <cc2:ZedButton ActionTag="View" ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                    OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <cc2:ZedButton ActionTag="View" CssClass="buttonbg" ID="btnClear" runat="server" Text="View All"
                                    CausesValidation="false" OnClick="btnClear_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="box1">
        <asp:HiddenField ID="hdfCurrentPage" runat="server" Value="1" Visible="false" />
        <asp:UpdatePanel ID="updpnlGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="mainheading">
                    Tax Group List
                </div>
                <div class="export">
                    <cc2:ZedImageButton ID="Exporttoexcel" runat="server"
                        OnClick="Exporttoexcel_Click" CausesValidation="False" title="Export to Excel"
                        ActionTag="Export" />

                </div>
                <div class="innerarea">
                    <%--#CC02: class changed--%>
                    <div class="grid1">
                        <%--#CC02: class changed--%>
                        <asp:DataList runat="server" ID="dlList" RepeatColumns="4" RepeatDirection="Horizontal"
                            Width="100%" CellPadding="4" CellSpacing="1" HeaderStyle-CssClass="gridheader"
                            ItemStyle-CssClass="gridrow" AlternatingItemStyle-CssClass="Altrow" FooterStyle-CssClass="gridfooter"
                            ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="top" HeaderStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" DataKeyField="TaxCategoryID" SelectedItemStyle-CssClass="gridselected"
                            BorderWidth="0" GridLines="none" OnItemCommand="dlList_ItemCommand" OnItemDataBound="dlList_ItemDataBound">
                            <ItemTemplate>
                                <%--<table cellpadding="0" cellspacing="0" width="220" border="0">
                                    <tr>
                                        <td align="left" valign="top" width="150" height="30">--%>
                                <div class="float-left">
                                    <%--#CH01--%>
                                    <div style="word-break: break-all; width: 150px; overflow: hidden;">
                                        <%--#CH01--%>
                                        <%# Eval("Tax Group Name")%>
                                    </div>
                                </div>
                                <%--</td>
                                        <td align="left" valign="top" width="70">--%>
                                <div class="float-right">
                                    <%--#CH01--%>
                                    <asp:Label ID="lblactive" Visible="false" runat="server" Text='<%# Eval("Active") %>'
                                        CommandName="activeLocality" CommandArgument='<%# Eval("TaxCategoryID") %>'></asp:Label>
                                    <cc2:ZedImageButton ID="Active" CommandName="ActiveInactive" runat="server" CausesValidation="false"
                                        CommandArgument='<%# Eval("TaxCategoryID") %>' ActionTag="Edit" />
                                    <cc2:ZedImageButton ID="img1" CommandName="EditTaxGroup"
                                        runat="server" CausesValidation="false" ToolTip="Edit" ActionTag="Edit" />
                                </div>
                                <%-- </td>
                                    </tr>
                                </table>--%>
                            </ItemTemplate>
                            <FooterStyle CssClass="gridfooter" />
                            <AlternatingItemStyle CssClass="Altrow" />
                            <ItemStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <EditItemTemplate>
                                <%--<table cellpadding="0" cellspacing="0" width="220" border="0">
                                    <tr>
                                        <td align="left" valign="top" width="150">--%>
                                <div class="float-left">
                                    <%--#CH01--%>
                                    <cc2:ZedTextBox CssClass="formfields" runat="server" ID="txtTaxGroupE" Text='<%# Eval("Tax Group Name")%>'
                                        MaxLength="100" ActionTag="Edit"></cc2:ZedTextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtTaxGroupE"
                                        FilterMode="InvalidChars" InvalidChars="`~!@#$%^&*()_-+=[]{}:;,'.<>/?">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="reqTaxGroupName" runat="server" ControlToValidate="txtTaxGroupE"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter tax group name!"
                                        SetFocusOnError="true" ValidationGroup="taxgroupE"></asp:RequiredFieldValidator>
                                </div>
                                <%--</td>
                                        <td align="left" valign="top" width="70">--%>
                                <div class="float-right">
                                    <%--#CH01--%>
                                    <cc2:ZedImageButton AlternateText="Update" ImageUrl="~/Assets/ZedSales/CSS/images/icon_update.gif"
                                        ID="imgbtnUpdate" runat="server" CommandName="UpdateTaxGroup" ValidationGroup="taxgroupE"
                                        ToolTip="UpdateTaxGroup" ActionTag="Edit" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ImageUrl="~/Assets/ZedSales/CSS/images/icon_cancel.gif" ID="imgbtnCancelUpdate"
                                                runat="server" CommandName="CancelUpdate" CausesValidation="false" ToolTip="Cancel" />
                                </div>
                                <%--</td>
                                    </tr>
                                </table>--%>
                            </EditItemTemplate>
                            <SelectedItemStyle CssClass="gridselected" />
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:DataList>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="pagination">
                        <div id="dvFooter" runat="server">
                            <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:PostBackTrigger ControlID="Exporttoexcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
