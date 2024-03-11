<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchSalesChannelCode.aspx.cs"
    Inherits="Transactions_SalesChannel_SearchSalesChannelCode" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<html lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Search SalesChannel Code</title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
</head>

<script>
    function passvalue(Code, Name, SalesChannelID) {
        document.getElementById("hdnChildSalesChannelCode").value = Code
        document.getElementById("hdnChildSalesChannelName").value = Name.replace("@", "'").replace(/~/gi, " ")  <%-- #CC01 .replace("@", "'").replace(/~/gi, " ") Added--%>
        document.getElementById("hdnChildSalesChannelID").value = SalesChannelID
        window.parent.document.getElementById("ctl00_contentHolderMain_txtSearchedName").value = Name.replace("@", "'").replace(/~/gi, " ")  <%-- #CC01 .replace("@", "'").replace(/~/gi, " ") Added--%>
        window.parent.document.getElementById("ctl00_contentHolderMain_hdnName").value = Name.replace("@", "'").replace(/~/gi, " ")  <%-- #CC01 .replace("@", "'").replace(/~/gi, " ") Added--%>
        window.parent.document.getElementById("ctl00_contentHolderMain_hdnCode").value = Code
        window.parent.document.getElementById("ctl00_contentHolderMain_hdnSalesChannelID").value = SalesChannelID
        parent.WinSearchChannelCode.hide();
        return true;

    }
</script>

<body>
    <div>
        <form id="form1" name="form1" runat="server">
            <asp:ScriptManager ID="s" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uc1:ucMessage ID="ucMessage1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="mainheading">
                        Search Sales Channel Code
                    </div>
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Sales Channel Code:
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtSalesChannelCode" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                                </li>
                                <li class="text">Sales Channel Name:
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
            <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="contentbox">
                        <div class="grid1" runat="server" id="dvGrid" visible="false">
                            <asp:GridView ID="GridSales" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                CellSpacing="1" DataKeyNames="SalesChannelID" EditRowStyle-CssClass="editrow"
                                EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" Width="100%"
                                AllowPaging="True" OnPageIndexChanging="GridSales_PageIndexChanging" OnRowDataBound="GridSales_RowDataBound">
                                <RowStyle CssClass="gridrow" />
                                <Columns>
                                    <%--   <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                                            HeaderText="Sales Channel Code">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>--%>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select Channel Code"
                                        ShowHeader="False" HeaderStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesChanneCode" runat="server" Text='<%# Eval("SalesChannelCode")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name"
                                        ShowHeader="False" HeaderStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesChanneName" runat="server" Text='<%# Eval("SalesChannelName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel ID"
                                        ShowHeader="False" HeaderStyle-Width="110px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesChannelID" runat="server" Text='<%# Eval("SalesChannelID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select Channel"
                                        ShowHeader="False" HeaderStyle-Width="110px">
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
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:HiddenField ID="hdnChildSalesChannelCode" runat="server" />
            <asp:HiddenField ID="hdnChildSalesChannelName" runat="server" />
            <asp:HiddenField ID="hdnChildSalesChannelID" runat="server" />
        </form>
    </div>
</body>
</html>
