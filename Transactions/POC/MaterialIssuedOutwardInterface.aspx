<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
 AutoEventWireup="true" CodeFile="MaterialIssuedOutwardInterface.aspx.cs"  enableEventValidation="true"
 Inherits="Transactions_POC_MaterialIssuedOutwardInterface" %>

<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" src="../../Assets/Jscript/GridTransferFile.js">

     function  hithere(gridId1, btnID)
        {
            hithereforOutwards(gridId1, btnID)
            return false;
        }

      
        
    </script>
    
    
     <%--<script type ="text/javascript">

         function forsave() {
             debugger;
             var button = document.getElementById("ctl00_contentHolderMain_Button2");
             var button1 = document.getElementById('<%= Button2.ClientID %>');
             __doPostBack(button1.id, 'OnClick');
             var div1 = document.getElementById("div1");
             div1.style.visibility = 'hidden';
            
         }
     </script>--%>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" Runat="Server">
<table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
                            <ContentTemplate>
                                <td align="left" valign="top">
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                </td>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </tr>
                    <tr>
                    <td>
                    <div id="message" style="padding-bottom:4px; float:left; margin:0px;"  >
                    <asp:Label ID = "lblMsg" runat= "server" Visible = "false" ></asp:Label>
                            </div>
                    </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                <tr>
                                    <td align="left" valign="top" class="tableposition" width="70%">
                                        <div class="mainheading">
                                            Upload</div>
                                    </td>
                                    <td width="30%" align="right" style="padding-right: 10px;">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                         <ContentTemplate>
                          <div class="contentbox">
                                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="6" align="left" valign="top" height="20" class="mandatory">
                                            (<font class="error">*</font>) Marked fields are mandatory
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="always">
                            <ContentTemplate>--%>
                                        <td width="14%" align="right" class="formtext" valign="top">
                                            Select Warehouse: <font class="error">*</font>
                                        </td>
                                        <td width="20%" align="left" valign="top">
                                            <div style="float: left; width: 135px;">
                                                <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form_select" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlWarehouse_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div style="float: left; width: 170px;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlWarehouse"
                                                    CssClass="error" ValidationGroup="EntryValidation" runat="server" InitialValue="0"
                                                    ErrorMessage="Please Select Warehouse."></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td width="10%" align="right" class="formtext" valign="top">
                                            User Name:<font class="error">*</font>
                                        </td>
                                        <td width="20%" align="left" valign="top">
                                            <asp:DropDownList ID="ddlUser" runat="server" CssClass="form_select" >
                                            </asp:DropDownList>
                                            <br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlUser"
                                                CssClass="error" ValidationGroup="EntryValidation" runat="server" InitialValue="0"
                                                ErrorMessage="Please Select User."></asp:RequiredFieldValidator>
                                        </td>
                                        <td width="16%" align="right" class="formtext" valign="top">
                                            Remarks:<font class="error">*</font>
                                        </td>
                                        <td width="20%" align="left" valign="top">
                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="20" CssClass="form_textarea6"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="formtext" valign="top">
                                            Docket Number:<font class="error">*</font>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txtDocketNo" runat="server" MaxLength="20" CssClass="form_input9"></asp:TextBox>
                                            <br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtDocketNo"
                                                CssClass="error" ValidationGroup="EntryValidation" runat="server" ErrorMessage="Please Insert Docket Number."></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="right" class="formtext" valign="top" visible="false">
                                            Docket Date:<font class="error">*</font>
                                        </td>
                                        <td align="left" valign="top" >
                                            <uc2:ucDatePicker ID="ucDateForGRN" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                IsRequired="true" ValidationGroup="EntryValidation" />
                                        </td>
                                        <td align="Right" valign="top" class="formtext" rowspan="2">
                                                         Mode of Receipt:<font class="error">*</font>
                                        </td>
                                        <td align="left" valign="top">
                                        <asp:DropDownList ID="ddlmodeofReceipt" runat="server" CssClass="form_select" 
                                                >
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="By Hand" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="By Courier" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  align="right" class="formtext" valign="top">
                              
                                             Courier Name:<font class="error">*</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCourierName" runat="server" MaxLength="20" CssClass="form_input9"></asp:TextBox> 
                                        </td>
                                        <td>
                                         <asp:Button ID="btnProceed" runat="server" CssClass="buttonbg" Text="Proceed" ValidationGroup="EntryValidation"
                                    OnClick="BtnProceed_Click" />
                                   
                                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClick="btnReset_Click" />
                                       </td>
                                    </tr>
                                    
                                </table>  
                            </div>
                       </ContentTemplate>
                       </asp:UpdatePanel>
                       
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
                                                                    <asp:Label ID="Label6" runat="server" Style="display: none" Text='<%# Bind("SKUCode") %>'></asp:Label>
                                                                  
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
                                        <td align="left" valign="top">
                                      
                                            <div id="div2" runat="server">
                                                <asp:Button ID="Button1" runat="server" Text="Transfer" CssClass="buttonbg" />
                                               <%-- <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonbg" />--%>
                                                 <asp:Button ID="Button2" runat="server" Text="Save" CssClass="buttonbg" 
                                                    onclick="Button2_Click"   />
                                            </div>
                                           
                                        </td>
                                    </tr> 
                                     
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                </table>
                               </asp:Panel>
                             </ContentTemplate>
                             <%-- <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                            </Triggers>--%>

                            </asp:UpdatePanel>
                        </td>
                       
                    </tr>
                    
                     
                    </table>
                    </td>
                  
                   </tr>
                    <tr>
                        <td height="10">
                         
                        <asp:Label ID = "lblCheck" runat = "server" Text = "0" Style="display: none"></asp:Label>
                         <%--<asp:UpdatePanel ID="updDiv" runat="server" UpdateMode="Conditional">
                          <ContentTemplate>--%>   
                            <div class="contentbox" id="div1"   >
                            </div>
                            
                            <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

