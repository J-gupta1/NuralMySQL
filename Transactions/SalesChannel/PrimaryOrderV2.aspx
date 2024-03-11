<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PrimaryOrderV2.aspx.cs" Inherits="Transactions_SalesChannel_PrimaryOrderV2" %>

<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGrid.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../../Assets/Jscript/GridTransferFile.js">

     function  hithere(gridId1, btnID)
        {
            hithereforOutwards(gridId1, btnID)
            return false;
        }
    </script>
     

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="mainheading">
                                            Primary Order</div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="7" align="left" valign="top" height="15" class="mandatory">
                                            (*) Marked fields are mandatory
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="13%" height="35" valign="top" align="right" class="formtext">
                                            Select Warehouse: <font class="error">*</font>
                                        </td>
                                        <td width="15%" valign="top" align="left">
                                            <div style="float: left; width: 135px;">
                                                <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form_select">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div style="float: left; width: 170px;">
                                                <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlWarehouse" CssClass="error"
                                                    ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select Warehouse Name."></asp:RequiredFieldValidator></div>
                                        </td>
                                        <td width="15%" valign="top" align="right" class="formtext">
                                            Order Number: <font class="error">*</font>
                                        </td>
                                        <td width="15%" valign="top" align="left">
                                            <asp:TextBox ID="txtOrderNo" runat="server" MaxLength="20" CssClass="form_input2"  ></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtOrderNo"
                                                ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please enter Order number."></asp:RequiredFieldValidator>
                                        </td>
                                        <td height="35" width="15%" valign="top" align="right" class="formtext">
                                            Order Date: <font class="error">*</font>
                                        </td>
                                        <td valign="top" align="left" width="12%">
                                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                ValidationGroup="Add" />
                                        </td>
                                        <td align="left" valign="top" width="15%">
                                            <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Go&nbsp;" CssClass="buttonbg"
                                                CausesValidation="true" ValidationGroup="EntryValidation" OnClick="BtnSubmit_Click" />
                                            <asp:Label ID="label1" Text="Show Me" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                    <td>
                     <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                    
                    
                    <tr>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <asp:Panel ID = "pnlSearch" runat = "server" Visible = "false">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" class="tableposition">
                                                <div class="mainheading">
                                                    Search SKU
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" class="tableposition">
                                            <%--<asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
                                                <div class="contentbox">
                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                        <tr>
                                                            <td align="right" valign="top" width="12%" height="35" class="formtext">
                                                                <asp:Label ID="lblserprodcat" runat="server" Text="Product Category:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="23%">
                                                                <div style="float: left; width: 135px;">
                                                                    <asp:DropDownList ID="cmbSerProdCat" runat="server" OnSelectedIndexChanged="cmbSerProdcat_SelectedIndexChanged"
                                                                        CssClass="form_select4" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </td>
                                                            <td align="right" valign="top" width="10%" height="25" class="formtext">
                                                                <asp:Label ID="lblsermodel" runat="server" Text="Model:"></asp:Label>
                                                            </td>  
                                                            <td align="left" valign="top" width="20%">
                                                                <div style="float: left; width: 135px;">
                                                                    <asp:DropDownList ID="cmbSerModel" runat="server" CssClass="form_select4" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" valign="top" height="35" class="formtext">
                                                                <asp:Label ID="lblsername" runat="server" Text="SKU Name:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtSerName" runat="server" CssClass="form_input2"></asp:TextBox>
                                                            </td>
                                                            <td align="right" valign="top" height="25" class="formtext">
                                                                <asp:Label ID="lblserCode" runat="server" Text="SKU Code:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtSerCode" runat="server" CssClass="form_input2"></asp:TextBox>
                                                            </td>
                                                            <td align="right" valign="top" class="formtext">
                                                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerchSku_Click"
                                                                    CssClass="buttonbg" />
                                                            </td>
                                                            
                                                        </tr>
                                                    </table>
                                                </div>
                                              <%-- </ContentTemplate>
                                               </asp:UpdatePanel>--%>
                                            </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                                </ContentTemplate>
                             </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                         <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                          <ContentTemplate>
                            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                                Enter Primary Order Details
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="tableposition">
                                            
                                            <div class="contentbox">
                                                <div class="grid1">
                                                    <asp:HiddenField ID="hdnColumn" runat="server" />
                                                    <asp:GridView ID="grdSalesChannel" runat="server" FooterStyle-VerticalAlign="Top"
                                                        FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="None"
                                                        AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                                                        HeaderStyle-CssClass="gridheader" EmptyDataText="No data found" CellSpacing="1"
                                                        CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="False"
                                                         PageSize='5' SelectedStyle-CssClass="gridselected"
                                                        DataKeyNames="SKUID" pa >
                                                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                        <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="SKU Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("SKUCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SKU Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("SKUName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                          
                                                             <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label7" runat="server" Style="display: none" Text='<%# Bind("SerialisedMode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label6" runat="server" Style="display: none" Text='<%# Bind("SKUID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="PagerStyle" />
                                                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                                        <AlternatingRowStyle CssClass="gridrow1" />
                                                    </asp:GridView>
                                                    <div id="dvFooter" runat="server" class="pagination">
                                                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server"  OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                                                    </div>
                                                </div>
                                            </div>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <div id="div2" runat="server">
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClick="btnReset_Click" />
                                                <asp:Button ID="Button1" runat="server" Text="HitHere" CssClass="buttonbg" />
                                                <asp:Button ID="btnFind" runat="server" Text="Save" CssClass="buttonbg" OnClick="btnFind_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                             </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                  
                    </table>
                    </td>
                  
                   </tr>
                    <tr>
                        <td height="10">
                            <div class="contentbox" id="div1">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
