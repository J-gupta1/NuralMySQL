<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="SchemeView.aspx.cs" Inherits="Masters_HO_Common_SchemeView" %>

<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <%--
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>--%>
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script language="javascript" type="text/javascript">
        function popup(SchemeID) {
            var WinSalesChannelDetail = dhtmlmodal.open("ViewDetails", "iframe", "ViewSchemeDetailsPOC.aspx?SchemeID=" + SchemeID, "Scheme Detail", "width=900px,height=550px,top=25,resize=0,scrolling=auto ,center=1")
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc2:ucMessage ID="ucMessage" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Search
    </div>
    <div class="export">
        <asp:LinkButton ID="LBViewScheme" runat="server" CausesValidation="False" OnClick="LBViewScheme_Click"
            CssClass="elink7">Manage Scheme</asp:LinkButton>
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updmain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H25-C3">
                    <%--(<span class="error">*</span>) marked fields are mandatory.--%>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblSchemeName" runat="server" Text="Scheme Name:"></asp:Label>&nbsp;
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtScheme" CssClass="formfields" MaxLength="100" runat="server"></asp:TextBox>
                        </li>
                        <li class="text">
                            <%--<asp:Label ID="Label1" runat="server" Text="">Scheme Component Type:<span class="error">*</span></asp:Label>--%>
                            <asp:Label ID="Label1" runat="server" Text="">Scheme Component Type:</asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbComponentType" CssClass="formselect" runat="server" OnSelectedIndexChanged="cmbComponentType_SelectedIndexChanged1"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </li>
                        <li class="text">
                            <%-- <asp:Label ID="Label3" runat="server" Text="">PayOut Type:<span class="error">*</span></asp:Label>--%>
                            <asp:Label ID="Label3" runat="server" Text="">PayOut Type:</asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbPayoutBase" CssClass="formselect" runat="server">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" OnClick="btnSearch_Click"
                                    ValidationGroup="SearchGroup" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" OnClick="btnCancel_Click" />
                            </div>
                        </li>

                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <%--  <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />--%>

                <asp:PostBackTrigger ControlID="cmbComponentType" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updSeprate" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                 <div class="H25-C3-S">
                    <ul>
                        <asp:Panel ID="pnlTargetdr" runat="server" Visible="false">
                            <li class="text">
                                <asp:Label ID="Label2" runat="server" Text="">FortNight: <span class="error">*</span></asp:Label>
                            </li>
                            <li class="text-field" style="height: auto">
                                <div style="height: 155px; overflow: auto;">
                                    <asp:CheckBoxList ID="chkLst" CellPadding="2" runat="server" />
                                </div>
                            </li>
                        </asp:Panel>
                        <asp:Panel ID="pnldaterng" runat="server" Visible="false">
                            <li class="field"></li>
                            <li class="text">
                                <asp:Label ID="lblpreffdt" runat="server" Text="">Scheme Start Date:<span class="error">*</span>
                                </asp:Label>
                            </li>
                            <li class="field">
                                <uc1:ucDatePicker ID="ucFromDate" runat="server" RangeErrorMessage="Invalid date."
                                    ErrorMessage="Please select a date" ValidationGroup="insert" />
                            </li>                             
                             <li class="text">
                                <asp:Label ID="Label4" runat="server" Text="">Scheme End Date:<span class="error">*</span>
                                </asp:Label>
                            </li>
                             <li class="field">
                                <uc1:ucDatePicker ID="ucToDate" runat="server" RangeErrorMessage="Invalid date."
                                    ErrorMessage="Please select a date" ValidationGroup="insert" />
                            </li>
                        </asp:Panel>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--<td align="right" style="padding-top:10px; padding-right:10px;">
                      <asp:Button ID="Exporttoexcel" runat="server" CausesValidation="False" CssClass="excel"
                        OnClick="exportToExel_Click" Text="" ToolTip="Export to Excel" />
                          </td>--%>
    <asp:UpdatePanel runat="server" ID="updGrid" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" Visible="true">
                <div class="mainheading">
                    List
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdSchemeDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="SchemeID"
                            OnRowCommand="grdSchemeDetail_RowCommand" AlternatingRowStyle-CssClass="Altrow"
                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="None"
                            HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" VerticalAlign="top"
                            RowStyle-CssClass="gridrow" HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            SelectedStyle-CssClass="gridselected" Width="100%" OnRowDataBound="grdSchemeDetail_RowDataBound">
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:BoundField DataField="SchemeName" HeaderText="Scheme Name" />
                                <asp:BoundField DataField="SchemeStartDate" HeaderText="Scheme Start Date" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="SchemeEndDate" HeaderText="Scheme End Date" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="SchemeComponentType" HeaderText="Base Type" />
                                <asp:BoundField DataField="SchemeCategory" HeaderText="Scheme Level" Visible="false" />
                                <%--<asp:BoundField DataField="ComponentPayoutTypeName" HeaderText="Scheme BacedOn" />--%>
                                <asp:BoundField DataField="ComponentPayoutTypeName" HeaderText="PayOut Type" />
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SchemeID") %>'
                                            CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("SchemeStatus"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("SchemeStatus"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Details">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg"></asp:HyperLink>
                                        <%--       <asp:Button ID="btnDetails" runat="server" Text="Details" CommandArgument='<%#Eval("SchemeID") %>'
                                                                                CommandName="cmdDetails" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                        </asp:GridView>
                    </div>
                </div>
                <%-- <tr>
                                                            <td width="90%" align="left" class="tableposition">
                                                                <div class="mainheading">
                                                                    &nbsp;List</div>
                                                            </td>
                                                            <td width="10%" align="right">
                                                                <asp:Button ID="exportToExel" Text=" " runat="server" OnClick="exportToExel_Click"
                                                                    CssClass="excel" />
                                                                    <asp:Button ID= "one" runat = "server" onclick="exportToExel_Click" />
                                                            </td>
                                                        </tr>
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="contentbox">
                                                        <div class="grid2">
                                                            <asp:GridView ID="grdSchemeDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="SchemeID"
                                                                OnRowCommand="grdSchemeDetail_RowCommand" AlternatingRowStyle-CssClass="gridrow1"
                                                                bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"  CssClass="gridfooter"
                                                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="None"
                                                                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" 
                                                                VerticalAlign="top" RowStyle-CssClass="gridrow"  HorizontalAlign="left"
                                                                RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%">
                                                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="SchemeName" HeaderText="Scheme Name" />
                                                                    <asp:BoundField DataField="SchemeStartDate" HeaderText="Scheme Start Date" DataFormatString="{0:d}" />
                                                                    <asp:BoundField DataField="SchemeEndDate" HeaderText="Scheme End Date" DataFormatString="{0:d}" />
                                                                    <asp:BoundField DataField="SchemeComponentType" HeaderText="Base Type" />
                                                                    <asp:BoundField DataField="SchemeCategory" HeaderText="Scheme Level" />
                                                                    <asp:BoundField DataField="ComponentPayoutTypeName" HeaderText="Scheme BacedOn" />
                                                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                                                        <ItemStyle Wrap="False" />
                                                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SchemeID") %>'
                                                                                CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("SchemeStatus"))) %>'
                                                                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("SchemeStatus"))) %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="View Details">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btnDetails" runat="server" Text="Details" CommandArgument='<%#Eval("SchemeID") %>'
                                                                                CommandName="cmdDetails" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <AlternatingRowStyle CssClass="gridrow1" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>--%>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
