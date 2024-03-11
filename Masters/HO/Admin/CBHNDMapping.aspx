<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="CBHNDMapping.aspx.cs" Inherits="HO_Admin_CBHNDMapping" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%--#CC01 Added--%>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="mainheading">
        Manage CBH to ND Mapping
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updSave" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <%-- #CC01 Comment Start <tr>
                                                            <td class="formtext" valign="top" align="right" width="5%">
                                                                <asp:Label ID="lblrole" runat="server" Text="">CBH :</asp:Label>
                                                            </td>
                                                            <td width="20%" align="left" valign="top">
                                                                <asp:DropDownList ID="ddlOrgnHierarchy" runat="server" CssClass="form_select7">
                                                                </asp:DropDownList>
                                                                <br />
                                                                <asp:RequiredFieldValidator ID="reqOrgnHierarchySave" runat="server" ControlToValidate="ddlOrgnHierarchy"
                                                                    InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select CBH."
                                                                    ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                <asp:RequiredFieldValidator ID="rqOrgnHierarchySearch" runat="server" ControlToValidate="ddlOrgnHierarchy"
                                                                    InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select CBH."
                                                                    ValidationGroup="Search"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="formtext" valign="top" align="left">
                                                                <div class="float-margin">
                                                                    <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                                                        ValidationGroup="Search" CssClass="buttonbg" CausesValidation="True" />
                                                                </div>
                                                                <div class="float-margin">
                                                                    <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                                                        CssClass="buttonbg" CausesValidation="False" />
                                                                </div>
                                                            </td>
                                                        </tr> #CC01 Comment End--%>
                    <%--#CC01 Add Start --%>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblrole" runat="server" Text="">Select ND: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSalesChannel" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlSalesChannel_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rq" runat="server" ControlToValidate="ddlSalesChannel"
                                InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select ND."
                                ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </li>
                    </ul>
                    <ul>
                        <li class="field3">
                            <div>
                                <asp:DataList ID="dtListCBNDMapping" runat="server" RepeatColumns="5">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chckList" runat="server" Text='<%#Eval("LocationName")%>' />

                                        <asp:HiddenField ID="HdnLocaionID" runat="server" Value='<%#Eval("OrgnhierarchyID")%>' />
                                        <asp:HiddenField ID="hdnOrgnNDMappingStatus" runat="server" Value='<%#Eval("Status")%>' />
                                        <asp:HiddenField ID="hdnOrgnNDMapping" runat="server" Value='<%#Eval("OrgnHierarchySalechannelMappingID")%>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="field3">
                            <div id="dvBtn" runat="server" visible="false">
                                <div class="float-margin">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click"
                                        CssClass="buttonbg" ValidationGroup="Save" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btncancel" Text="Reset" runat="server" OnClick="btncancel_Click"
                                        CssClass="buttonbg" CausesValidation="False" />
                                </div>
                            </div>
                        </li>
                    </ul>
                    <%--#CC01 Add End --%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel runat="server" ID="updSearch" UpdateMode="Always">
        <ContentTemplate>
            <div class="mainheading">
                Search Mapping
            </div>
            <div class="contentbox">
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label1" runat="server" Text="">Select ND: <span class="error">*</span></asp:Label>
                        </li>
                         <li class="field">
                            <asp:DropDownList ID="ddlSalesChannelSearch" runat="server" CssClass="formselect">
                            </asp:DropDownList>

                            <%-- <br />
                                                                 <asp:RequiredFieldValidator ID="reqSalesChannel" runat="server" ControlToValidate="ddlSalesChannelSearch"
                                                                    InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select ND."
                                                                    ValidationGroup="Search"></asp:RequiredFieldValidator>--%>

                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                    CssClass="buttonbg" CausesValidation="false" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Always">
        <ContentTemplate>
            <div id="dvhide" runat="server" visible="false">
                <div class="mainheading">
                    Detail
                </div>
                <div class="export">
                    <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="btnExportToExcel_Click"
                        CssClass="excel" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <%--#CC01 Add Start --%>
                        <asp:GridView ID="gvCBHToND" runat="server" AlternatingItemStyle-CssClass="Altrow" GridLines="none"
                            AutoGenerateColumns="False" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" HeaderStyle-CssClass="gridheader"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="Middle" RowStyle-CssClass="gridrow"
                            meta:resourcekey="gvPartResource1" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            ShowFooter="false" Width="100%" OnRowCommand="gvCBHToND_RowCommand">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:TemplateField HeaderText="ND Name" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNDName" runat="server" Text='<%# Eval("NDName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                                                                                                                                                                           

                                <asp:TemplateField HeaderText="CBH Name" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCBHName" runat="server" Text='<%# Eval("CBHName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnNDIDEdit" runat="server" Value='<%#Eval("NDID")%>' />
                                        <asp:HiddenField ID="HdnLocaionIDEdit" runat="server" Value='<%#Eval("OrgnhierarchyID")%>' />
                                        <asp:HiddenField ID="hdnOrgnNDMappingStatusEdit" runat="server" Value='<%#Eval("Status")%>' />
                                        <asp:HiddenField ID="hdnOrgnNDMappingEdit" runat="server" Value='<%#Eval("OrgnHierarchySalechannelMappingID")%>' />
                                        <cc2:ZedImageButton ID="imgEdit" runat="server" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                            ToolTip="(Edit)" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex+1 %>'
                                            CausesValidation="False" AlternateText="Edit" ActionTag="Edit" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                        <%--#CC01 Add End --%>
                        <%-- #CC01 Comment Start <asp:GridView ID="gvCBHToND" runat="server" AlternatingItemStyle-CssClass="gridrow1" GridLines="none"
                                                    AutoGenerateColumns="False" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" HeaderStyle-CssClass="gridheader"
                                                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="Middle" RowStyle-CssClass="gridrow"
                                                    meta:resourcekey="gvPartResource1" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                                    ShowFooter="false" Width="100%" OnRowCommand="gvCBHToND_RowCommand">
                                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="State" HeaderStyle-Width="250px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNDName" runat="server" Text='<%# Eval("ND") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <div style="padding-bottom: 5px;">
                                                                    ND Name
                                                                </div>
                                                                <div>
                                                                    <asp:DropDownList ID="ddlND" runat="server" Width="200px" CssClass="form_select">
                                                                    </asp:DropDownList>
                                                                    <br />
                                                                    <asp:RequiredFieldValidator ID="rqddlState" runat="server" ControlToValidate="ddlND"
                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select ND." ValidationGroup="Save"
                                                                        InitialValue="0"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="gridheader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("OrgnHierarchySalechannelMappingID") %>'
                                                                    CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                                    ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                                <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("Status") %>' />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <div style="padding-bottom: 5px;">
                                                                    Status
                                                                </div>
                                                                <div>
                                                                    <asp:Button ID="btnAddPart" runat="server" CommandName="AddCBHtoND" CssClass="buttonbg"
                                                                        Text="Add" ValidationGroup="Save" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="gridheader" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <AlternatingRowStyle CssClass="gridrow1" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>  #CC01 Comment End--%>
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
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
            <asp:AsyncPostBackTrigger ControlID="gvCBHToND" EventName="RowEditing" />
            <%--  #CC01 Commented--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
