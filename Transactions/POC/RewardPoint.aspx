<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RewardPoint.aspx.cs" Inherits="Transactions_POC_RewardPoint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search SalesChannel Code</title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
</head>

<script>
    function passvalue(Code, Name) {
    
        document.getElementById("hdnChildSalesChannelCode").value = Code
        document.getElementById("hdnChildSalesChannelName").value = Name
        window.parent.document.getElementById("ctl00_contentHolderMain_txtSearchedName").value = Name
        window.parent.document.getElementById("ctl00_contentHolderMain_hdnName").value = Name
        window.parent.document.getElementById("ctl00_contentHolderMain_hdnCode").value = Code
        parent.SearchSalesChannelCode.hide();
        return true;
        
    }
</script>

<body>
    <div align="center">
        <form id="form1" name="form1" runat="server">
        <asp:ScriptManager ID="s" runat="server">
        </asp:ScriptManager>
        <table cellspacing="0" cellpadding="0" width="770" border="0" style="float: left;">
            <tr>
                <td align="left" valign="top" width="10">&nbsp;
                    
                </td>
                <td align="left" valign="top" width="760">
                    <table cellspacing="0" cellpadding="0" width="700" border="0">
                        <tr>
                            <td valign="top" align="left">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td align="left" valign="top" height="20">
                                                      
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                  
                                       
                                   
                                             
                                                      
                                                            <asp:GridView ID="GridSales" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                CellSpacing="1" DataKeyNames="OfferRewardDetailID" EditRowStyle-CssClass="editrow"
                                                                EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                                                RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="gridrow1" Width="100%"
                                                                AllowPaging="True" >
                                                                <RowStyle CssClass="gridrow" />
                                                                <Columns>
                                                                 
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select Channel Code"
                                                                        ShowHeader="true" HeaderStyle-Width="110px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSalesChanneCode" runat="server" Text='<%# Eval("OfferName")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="SKU CODE"
                                                                        ShowHeader="true" HeaderStyle-Width="110px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSalesChanneName" runat="server" Text='<%# Eval("SKUCODE")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Reward Value"
                                                                        ShowHeader="true" HeaderStyle-Width="110px">
                                                                        <ItemTemplate>
                                                                             <asp:Label ID="lblSalesChanneName" runat="server" Text='<%# Eval("RewardValue")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="gridheader" />
                                                                <EditRowStyle CssClass="editrow" />
                                                                <AlternatingRowStyle CssClass="gridrow1" />
                                                                <PagerStyle CssClass="PagerStyle" />
                                                            </asp:GridView>
                                                     
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="left" valign="top" width="15%">&nbsp;
                    
                </td>
            </tr>
        </table>
       
        </form>
    </div>
</body>
</html>