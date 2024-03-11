<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrimarySalesInterfaceForAddV1.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SalesChannel_PrimarySalesInterfaceForAddV1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/PartLookupClientSide.ascx" TagName="PartLookupClientSide"
    TagPrefix="uc4" %>
<%--    
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral,PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

    
<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral,PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridLookup" tagprefix="dx" %>

    
<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>

    --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.js"></script>

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.dataTables.js"></script>

    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/Assets/Jscript/dhtmlwindow.js") %>"></script>

    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/Assets/Jscript/modal.js") %>"></script>

    <script type="text/javascript">
        function popup() {
            WinSearchChannelCode = dhtmlmodal.open("SearchSalesChannelCode", "iframe", "../../SalesChannel/SearchSalesChannelCode.aspx", "Sales Channel Detail", "width=800px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
            WinSearchChannelCode.onclose = function() {
                return true;
            }
            return false;
        }
        function BlankTo() {
            document.getElementById('<%= hdnName.ClientID  %>').value = '';

        }
    </script>

    <script type="text/javascript">
        // <![CDATA[ 
        var startTime;
        function OnBeginCallback(s, e) {
            startTime = new Date();
        }
        function OnEndCallback(s, e) {
            var result = new Date() - startTime;
            result /= 1000;
            result = result.toString();
            if (result.length > 4)
                result = result.substr(0, 4);
            ClientTimeLabel.SetText(result.toString() + " sec");
            ClientCaptionLabel.SetText("Time to retrieve the last data:");
        }
        function TableCount() {
            var DirectSalesOfSerialAllowed = document.getElementById('<%= hdnDirectSalesOfSerialAllowed.ClientID  %>');
            
            if (oTable.fnGetData().length == 0 & DirectSalesOfSerialAllowed.value=0) {
                alert('There is no row to be submitted.');

            }
        }
        // ]]> 
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="100%" border="0" align="center" style="float: left;">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                    <asp:HiddenField ID="hdnDirectSalesOfSerialAllowed" Value="0" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="mainheading_rpt">
                                            <div class="mainheading_rpt_left">
                                            </div>
                                            <div class="mainheading_rpt_mid">
                                                Primary Sales Entry</div>
                                            <div class="mainheading_rpt_right">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <%--  <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="5" align="left" valign="top" height="15" class="mandatory">
                                            (*) Marked fields are mandatory
                                        </td>
                                        <td align="right" valign="top">
                                            <asp:Button ID="btnsearch" runat="server" Text="Search Sales Channel" CssClass="buttonbg"
                                                OnClick="btnsearch_Click" Enabled="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="15%" class="formtext" valign="top" align="right">
                                            Select Mode:<font class="error">*</font>
                                        </td>
                                        <td width="25%" align="left" valign="top" class="formtext" style="padding-top: 0px;">
                                            <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal"
                                                CellPadding="0" CellSpacing="0" AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="Excel Template"></asp:ListItem>
                                                <asp:ListItem Selected="True" Value="2" Text="On-Screen Entry"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td width="20%" valign="top" align="right" class="formtext">
                                            Select
                                            <asp:Label ID="lblChange" runat="server"></asp:Label>
                                            From :<font class="error">*</font>
                                        </td>
                                        <td width="8%" valign="top" align="left" class="formtext">
                                            <div style="float: left; width: 135px;">
                                                <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form_select" OnSelectedIndexChanged="ddlWarehouse_SelectedIndexChanged"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div style="float: left; width: 150px;">
                                                <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlWarehouse" CssClass="error"
                                                    ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select Warehouse Name."></asp:RequiredFieldValidator></div>
                                        </td>
                                        <td width="14%" valign="top" align="right" class="formtext">
                                            <asp:Label ID="lblChangeTo" runat="server"></asp:Label>
                                            To:<font class="error">*</font>
                                        </td>
                                        <td valign="top" align="left" class="formtext">
                                            <%--<asp:DropDownList ID="ddlDistri" runat="server"  CssClass="form_select" 
                                                         ></asp:DropDownList>--%>
                                            <asp:TextBox ID="txtSearchedName" runat="server" CssClass="form_input2" Enabled="false"></asp:TextBox>
                                            <br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSearchedName"
                                                ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please Enter SalesChannelTo."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="right" class="formtext">
                                            Invoice Number:<font class="error">*</font>
                                        </td>
                                        <td valign="top" align="left">
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50" CssClass="form_input2"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtInvoiceNo"
                                                ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please enter invoice number."></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="right" valign="top" colspan="2">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <asp:Panel ID="PnlHide" runat="server" Visible="true">
                                                    <tr>
                                                        <td valign="top" align="right" class="formtext" width="52%">
                                                            Invoice Date:<font class="error">*</font>
                                                        </td>
                                                        <td valign="top" align="left" width="48%">
                                                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                                ValidationGroup="EntryValidation" />
                                                            <asp:Label ID="lblValidationDays" runat="server" Text="" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                            </table>
                                        </td>
                                        <td align="right" valign="top">
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:HiddenField ID="hdnCode" runat="server" />
                                            <asp:HiddenField ID="hdnName" runat="server" />
                                            <asp:HiddenField ID="hdnSalesChannelID" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <%-- </ContentTemplate>
                                
                                </asp:UpdatePanel>--%>
                    </tr>
                    <tr>
                        <td height="10">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <%-- <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                            <asp:Panel ID="pnlGrid" runat="server">
                                <table id="tblGrid" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading_rpt">
                                                <div class="mainheading_rpt_left">
                                                </div>
                                                <div class="mainheading_rpt_mid">
                                                    Enter Details
                                                </div>
                                                <div class="mainheading_rpt_right">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="contentbox">
                                                <uc4:PartLookupClientSide ID="PartLookupClientSide1" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="EntryValidation"
                                                CausesValidation="true" OnClientClick="TableCount();" />
                                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClientClick="BlankTo();"
                                                OnClick="btnReset_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblDirectSerialPanel" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading_rpt">
                                                <div class="mainheading_rpt_left">
                                                </div>
                                                <div class="mainheading_rpt_mid">
                                                    Enter Serial No.
                                                </div>
                                                <div class="mainheading_rpt_right">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="contentbox">
                                            <table cellpadding="4" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="right" valign="top" height="30" width="10%">
                                                        Serial No.:
                                                    </td>
                                                    <td align="right" valign="top" width="1%">
                                                        <span class="error">*</span>
                                                    </td>
                                                    <td align="left" valign="top" width="20%">
                                                        <div>
                                                            <asp:TextBox ID="txtDirectSerialNumber" runat="server" CssClass="form_textarea" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                        <div style="margin-top: 2px;" class="error">
                                                            (Comma separated)</div>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <div class="float-margin">
                                                            <asp:Button ID="btnSubmitDirectSerialSale" CssClass="buttonbg" runat="server" Text="Save"
                                                                ValidationGroup="EntryValidation" CausesValidation="true" OnClick="btnSubmitDirectSerialSale_Click" />
                                                        </div>
                                                        <div class="float-left">
                                                            <asp:Button ID="btnResetDirect" runat="server" Text="Reset" CssClass="buttonbg" OnClientClick="BlankTo();"
                                                                OnClick="btnReset_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%-- <td   valign="top" align="right" class="formtext">
                                                 Salesman: <font class="error">*</font>
                                                </td>
                                                <td  align="left"  valign="top">
                                                   <asp:TextBox ID="txtSalesman" runat="server" MaxLength="100" CssClass="form_input2"  ></asp:TextBox>
                                                    <br>
                                                   </br>
                                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSalesman" CssClass="error" ValidationGroup="Add"  runat="server" ErrorMessage="Please enter salesman name."></asp:RequiredFieldValidator>
                                                </td>--%>
</asp:Content>
