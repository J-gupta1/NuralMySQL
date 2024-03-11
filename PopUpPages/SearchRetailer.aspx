<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchRetailer.aspx.cs"
    Inherits="PopUpPages_SearchRetailer" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%--<html xmlns="http://www.w3.org/1999/xhtml">--%>
<html lang="en">
<head id="Head1" runat="server">
    <%--Add START  --%>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <%--Add END  --%>
    <title>Search Retailer</title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
</head>

<script>
    function passvalue(RetailerID, Name) 
    {
        debugger
        document.getElementById("hdnRetailerID").value = RetailerID
        document.getElementById("hdnRetailerName").value = Name
          <%-- #CC01 Add Start
        window.parent.document.getElementById("contentHolderMain_txtRetailerName").value = Name;
        window.parent.document.getElementById("contentHolderMain_hdnRetailerName").value = Name
        window.parent.document.getElementById("contentHolderMain_hdnRetailerID").value = RetailerID --%>
        <%-- #CC01 Add End --%>
       <%-- #CC01 Comment Start  #CC01 Comment End --%>
        window.parent.document.getElementById("ctl00_contentHolderMain_txtRetailerName").value = Name
        window.parent.document.getElementById("ctl00_contentHolderMain_hdnRetailerName").value = Name
        window.parent.document.getElementById("ctl00_contentHolderMain_hdnRetailerID").value = RetailerID
        parent.WinSearchRetailerCode.hide();
        return true;
        
    }
</script>

<body>
    <div class="padding">
        <form id="form1" name="form1" runat="server">
            <asp:ScriptManager ID="s" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div>
                        <uc1:ucMessage ID="ucMessage1" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="mainheading">
                        Search Retailer Code
                    </div>
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                 <li class="text">Retailer Code:
                                </li>
                                 <li class="field">
                                    <asp:TextBox ID="txtRetailerCode" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                                </li>
                                <li class="text">Retailer Name:
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtRetailerName" runat="server" CssClass="formfields" MaxLength="40"></asp:TextBox>
                                </li>
                                <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Width="120px" CssClass="buttonbg"
                                            Text="Search Retailer" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                                            OnClick="btnCancel_Click" OnClientClick="self.close();" />
                                        <%--<input type=button onClick="self.close();" value="Cancel"> --%>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="mainheading" runat="server" id="dvSearchResult" visible="false">
                Search Result
            </div>
            <div class="contentbox">
                <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="grid1" runat="server" id="dvGrid" visible="false">
                            <asp:GridView ID="GridSales" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                CellSpacing="1" DataKeyNames="RetailerID" EditRowStyle-CssClass="editrow"
                                EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" Width="100%"
                                AllowPaging="True" OnPageIndexChanging="GridSales_PageIndexChanging" OnRowDataBound="GridSales_RowDataBound">
                                <RowStyle CssClass="gridrow" />
                                <Columns>
                                    <%--   <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                                            HeaderText="Sales Channel Code">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>--%>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select Retailer Code"
                                        ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRetailerCode" runat="server" Text='<%# Eval("RetailerCode")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Name"
                                        ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRetailerName" runat="server" Text='<%# Eval("RetailerName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select Retailer"
                                        ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="SelectCode"
                                                CssClass="buttonbg" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gridheader" />
                                <EditRowStyle CssClass="editrow" />
                                <AlternatingRowStyle CssClass="Altrow" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:HiddenField ID="hdnRetailerID" runat="server" />
            <asp:HiddenField ID="hdnRetailerName" runat="server" />
        </form>
    </div>
</body>
</html>
