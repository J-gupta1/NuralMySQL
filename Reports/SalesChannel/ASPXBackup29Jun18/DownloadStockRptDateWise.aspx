﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownloadStockRptDateWise.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Reports_SalesChannel_DownloadStockRptDateWise" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">

    function ValidateForType() {

        var e = document.getElementById('<%= cmbSalesChannelType.ClientID %>');
        var strUser = e.value;
        if (strUser == "0") {
            alert('Please select Sales Channel Type');
            document.getElementById('<%= txtsaleschannel.ClientID %>').value = '';
            return false
        }
        else
            return true;
    }

    function txtsaleschannelCodeTextChanged() {

        var v = ValidateForType();
        if (v == false) {
            return;
        }
        $("[id$=btnSearch]").attr("disabled", "disabled");  //disable search button till service response comes
        var strUser = "0";
        var v = "";
        var Id = document.getElementById('<%= hdnReportForSalesChannelid.ClientID %>');

        $("[id$=updMsg]").hide();  //hide the ucmessage to remove any previous error
        
        
               

        var ChannelsTypeid = document.getElementById('<%= cmbSalesChannelType.ClientID %>');
        var SalesChannelTypeid = ChannelsTypeid.options[ChannelsTypeid.selectedIndex].value;

        var AdjustmentForCode = document.getElementById('<%= txtsaleschannel.ClientID %>').value;
        if (AdjustmentForCode.indexOf('-') <= 0) {
            alert("Please enter valid SalesChannel Code.");
            $("[id$=btnSearch]").removeAttr("disabled"); //enable back search button
            return;
        }
        Typeid = SalesChannelTypeid;
        CommonService.GetSalesChannelInformation(AdjustmentForCode, SalesChannelTypeid,
                    OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {

                        var vv = result.toString();
                        // alert(vv);
                        if (vv != '') {
                            var lst = new Array();
                            lst = vv.split('/');
                            //Id.value = vv.split('/')[1];
                            Id.value = lst[lst.length - 1];
                            // alert(Id.value);
                            
                        }
                        else
                            Id.value = "0";

                        $("[id$=btnSearch]").removeAttr("disabled"); //enable back search button
                    }, OnError);

                    function OnError(result) {
                        alert("Error: " + result.get_message());
                        $("[id$=btnSearch]").removeAttr("disabled"); //enable back search button
                        
                    }


    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" Runat="Server">
 <asp:TextBox ID="hdnReportForSalesChannelid" runat="server" Value="0" Style="display: none" />
 <table cellspacing="0" cellpadding="0" width="965" border="0">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                               
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                            <ContentTemplate>
                                                <uc1:ucMessage ID="ucMessage1" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="mainheading">
                                                        Search</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" class="tableposition">
                                        <div class="contentbox">
                                            <%--<asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                      
                                                        <tr>
                                                            <td class="formtext" valign="top" align="right" width="10%" >
                                                                <asp:Label ID="lblrole" runat="server" Text="">Sales Channel Type:</asp:Label><font class="error">*</font>
                                                            </td>
                                                            <td width="25%" align="left" valign="top"><asp:UpdatePanel ID="updSalesChannelType" runat="server" >
                                                <ContentTemplate>
                                                               <div style="float:left; width:160px;"> <asp:DropDownList ID="cmbSalesChannelType" CssClass="form_select4" 
                                                                    runat="server" OnSelectedIndexChanged="cmbSalesChannelType_SelectedIndexChanged"
                                        AutoPostBack="true" 
                                                                    >
                                                                </asp:DropDownList><br /></div>  <div style="float:left; width:180px;">
                                                                 <asp:RequiredFieldValidator runat="server" ID="valtype" ControlToValidate="cmbSalesChannelType"
                                                                                CssClass="error" ErrorMessage="Please select a sales Channel Type " InitialValue="0" ValidationGroup="Search" /></div>
                                                                
                                                           </ContentTemplate></asp:UpdatePanel> </td>
<td class="formtext" valign="top" align="right" width="10%" >
                                                                <asp:Label ID="lblsaleschannelname" runat="server" Text="">Sales Channel:<font class="error">*</font></asp:Label>
                                                            </td>
                                                            <td width="25%" align="left" valign="top">
                                                               <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                                                <ContentTemplate> <asp:TextBox ID="txtsaleschannel" runat="server" onchange="javascript:txtsaleschannelCodeTextChanged();"
                                        CssClass="form_input9" Width="150px"></asp:TextBox><br />
                                         <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtsaleschannel"
                                                                                CssClass="error" ErrorMessage="Please enter Sales Channel" ValidationGroup="Search" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                                        MinimumPrefixLength="3" ServiceMethod="GetSalesChannelCodeList" ServicePath="../../CommonService.asmx"
                                        TargetControlID="txtsaleschannel" UseContextKey="true">
                                    </cc1:AutoCompleteExtender></ContentTemplate></asp:UpdatePanel>
                                                                
                                                            </td>
                                                        </tr>
                                                     
                                                        <tr>
                                                            <td align="right" valign="top" class="formtext">
                                                                <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label><font class="error">*</font>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Invalid from date."
                                                                    defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date."   ValidationGroup = "Search"/>
                                                            </td>
                                                            <td align="right" valign="top" class="formtext">
                                                                <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label><font class="error">*</font></td><td valign="top" align="left">
                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Invalid to  date."
                                                                     defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." ValidationGroup = "Search"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formtext" valign="top" align="right" colspan="3">
                                                            </td>
                                                            <td class="formtext" valign="top" align="left" >
                                                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                                                    ValidationGroup="Search" CssClass="buttonbg" CausesValidation="True"   AutoPostBack="false"  />
                                                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                               <%-- </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSalesChannelType"  />
                                                    <asp:AsyncPostBackTrigger ControlID="txtsaleschannel" />
                                                </Triggers></asp:UpdatePanel>--%></div></td></tr></table></td></tr><tr>
                        <td align="left" height="10">
                        </td>
                    </tr>
                    <tr>
                    <td>
                    
                    </td>
                    </tr>
                    <tr>
                        <td align="left" height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>