<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchParentRetailerInfo.aspx.cs"
    Inherits="Masters_Retailer_SearchParentRetailerInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%--<html xmlns="http://www.w3.org/1999/xhtml">--%>
<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <%--Add START  --%>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <%--Add END  --%>
    <title>Search Retailer Code</title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
</head>

<script>
    function passvalue(ID, Name) {
        document.getElementById("hdnChildSalesChannelID").value = ID
        document.getElementById("hdnChildSalesChannelName").value = Name
       //window.parent.document.getElementById("ctl00_contentHolderMain_txtSearchedName").value = Name
       //window.parent.document.getElementById("ctl00_contentHolderMain_hdnName").value = Name
        // window.parent.document.getElementById("ctl00_contentHolderMain_hdnID").value = ID

        window.parent.document.getElementById("contentHolderMain_txtSearchedName").value = Name
        window.parent.document.getElementById("contentHolderMain_hdnName").innerText = Name
        window.parent.document.getElementById("contentHolderMain_hdnID").innerText = ID
        
        parent.WinSearchChannelCode.hide();
        return true;

    }
</script>

<body>
    <div>
        <form id="form1" name="form1" runat="server">
            <asp:ScriptManager ID="s" runat="server">
            </asp:ScriptManager>
            <div>
                <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <uc1:ucMessage ID="ucMessage1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="mainheading">
                            Search Retailer Code
                        </div>
                        <div class="contentbox">
                            <div class="H20-C3-S">
                                <ul>
                                    <li class="text">Retailer Code:
                                    </li>
                                    <li class="field">
                                        <asp:TextBox ID="txtSalesChannelCode" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                                    </li>
                                    <li class="text">Retailer Name:
                                    </li>
                                    <li class="field">
                                        <asp:TextBox ID="txtSalesChannelName" runat="server" CssClass="formfields" MaxLength="40"></asp:TextBox>
                                    </li>
                                    <li class="field3">
                                        <div class="float-margin">
                                            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                                                Text="Search Sales Channel" OnClick="btnSearch_Click" />
                                        </div>
                                        <div class="float-margin">
                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                                                OnClick="btnCancel_Click" OnClientClick="self.close();" />
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
                            <div class="grid3" runat="server" id="dvGrid" visible="false">
                                <asp:GridView ID="GridSales" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    CellSpacing="1" DataKeyNames="RetailerID" EditRowStyle-CssClass="editrow"
                                    EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                    RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" Width="100%"
                                    AllowPaging="True" OnPageIndexChanging="GridSales_PageIndexChanging" OnRowDataBound="GridSales_RowDataBound">
                                    <RowStyle CssClass="gridrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Code"
                                            ShowHeader="False" HeaderStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRetailerCode" runat="server" Text='<%# Eval("RetailerCode")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Code" Visible="false"
                                            ShowHeader="False" HeaderStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRetailerID" runat="server" Text='<%# Eval("RetailerID")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Name"
                                            ShowHeader="False" HeaderStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRetailerName" runat="server" Text='<%# Eval("RetailerName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Action" ShowHeader="False"
                                            HeaderStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="SelectCode"
                                                    CssClass="buttonbg" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="110px" />
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
            </div>
            <asp:HiddenField ID="hdnChildSalesChannelID" runat="server" />
            <asp:HiddenField ID="hdnChildSalesChannelName" runat="server" />
        </form>
    </div>
</body>
</html>
