<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ViewSalesChannelStockSB.aspx.cs" Inherits="Reports_Inventory_ViewSalesChannelStockSB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>

<%@ Register Src="~/UserControls/ucEntityType.ascx" TagName="ucEntityType" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <%--<link href="../../Assets/Beetel/CSS/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <link href="../../Assets/Beetel/CSS/modal.css" rel="stylesheet" type="text/css" />--%>

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <%--#CC01 ADDED START    --%>    <%--#CC01 ADDED END    --%>

    <script type="text/javascript">



        function OnChangetxtSalesChannelName() {


            var strUser = "0";
            var v = "";
            var Id = document.getElementById('<%= hdnAdjustmentForSalesChannelid.ClientID %>');


            var SalesChannelCode = document.getElementById('<%= txtSalesChannelName.ClientID %>').value;

            if (SalesChannelCode.indexOf('-') <= 0) {
                alert("Please Enter valid Code or Name.");
                return;
            }

            CommonService.GetSalesChannelInfo(SalesChannelCode, "0",
                     OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {
                         var vv = result.toString();
                         if (vv != '') {
                             var lst = new Array();
                             lst = vv.split('/');
                             Id.value = lst[lst.length - 1];
                         }
                         else
                             Id.value = "0";

                     }, OnError);
        }
        function OnError(result) {
            alert("Error: " + result.get_message());
        }
    </script>
    <%--<span class="error">*</span>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:TextBox ID="hdnAdjustmentForSalesChannelid" runat="server" Value="0" Style="display: none" />
    <%-- #CC04 <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>--%>

    <uc1:ucMessage ID="ucMessage1" runat="server" />
    <div class="mainheading">
        SalesChannel Stock Status
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">SalesChannel Type:
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbsaleschanneltype" runat="server" ValidationGroup="Add" AutoPostBack="true"
                        Style="display: none" CssClass="formselect" OnSelectedIndexChanged="cmbsaleschanneltype_SelectedIndexChanged">
                    </asp:DropDownList>
                    <%--#CC01 ADDED START    --%>
                    <uc2:ucEntityType ID="ddlEntityType" runat="server" AutoPostBack="true" />
                    <%--#CC01 ADDED END    --%>
                </li>
                <li class="text">SalesChannel Name: <%--<span class="error">*</span>--%>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlSalesChannelName" runat="server" CssClass="formselect" Style="display: none">
                        </asp:DropDownList>

                        <%--#CC01 COMMETED  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSalesChannelName"
                                                                        CssClass="error" InitialValue="0" Display="Dynamic" ErrorMessage="Please Select Saleschannel Name."
                                                                        ValidationGroup="Serach1"></asp:RequiredFieldValidator>--%>
                        <%--#CC01 ADDED START    --%>
                        <asp:TextBox ID="txtSalesChannelName" runat="server" MaxLength="30"
                            CssClass="formfields"></asp:TextBox>
                        <%-- onchange="javascript:OnChangetxtSalesChannelName();"--%>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                            CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                            MinimumPrefixLength="3" ServiceMethod="GetSalesChannelList" ServicePath="~/CommonService.asmx"
                            TargetControlID="txtSalesChannelName">
                        </cc1:AutoCompleteExtender>
                        <%--#CC01 ADDED END    --%>
                    </div>
                </li>
                <li class="text">Stock Status :
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlStockStatus" runat="server" CssClass="formselect" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlStockStatus_SelectedIndexChanged">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Good" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Defective" Value="2"></asp:ListItem>
                        <%--<asp:ListItem Text="Scrap" Value="3"></asp:ListItem>--%>
                    </asp:DropDownList>
                </li>
                <li class="text">Bin Type:
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlStockBinType" runat="server" CssClass="formselect">
                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                    </asp:DropDownList>
                </li>
                <li class="text">Sku Code:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtSkuCode" runat="server" CssClass="formfields" />
                </li>
                <li class="text">Sku Name:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtSkuName" runat="server" CssClass="formfields" />
                </li>
            </ul>
            <%--  #CC02 start--%>
            <ul>
                <li class="text">
                    <asp:Label ID="lblsku" runat="server" Text="">Model Name:<span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlModelName" CssClass="formselect" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator runat="server" ID="valprodcat" ControlToValidate="ddlModelName"
                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a Model Name "
                            InitialValue="0" />
                    </div>
                </li>
            </ul>
            <%--  #CC02 END--%>
            <ul>
                <li class="text">Serial Number:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="formfields" />
                </li>
                <li class="text">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                            CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnShowAll" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                            OnClick="btnShowAll_Click" Visible="false" />
                    </div>
                     <div class="float-margin">
                        <asp:Button ID="ExportToExcel" CssClass="buttonbg" runat="server" Text="Download Report" OnClick="ExportToExcel_Click" />
                    </div>
                </li>
                <li class="field"> 
                    <div class="float-margin">
                        <asp:Button ID="RequestForData" CssClass="buttonbg" runat="server" Text="Generate Request" OnClick="RequestForData_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                            OnClick="btnCancel_Click" Visible="false" />
                    </div>

                </li>
               
            </ul>
        </div>
        <div>
     <span style="background-color: #FFFF00">Note: If report is not downloaded, please click generate request Button. Report will be generated and you can download the report from Help-> Report queue menu option.</span>
        </div>
    </div>

    <div id="dvhide" runat="server" visible="false">
        <div class="mainheading">
            List
        </div>
        <%--<asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />--%>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="grdvResult" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    CellSpacing="1" EmptyDataText="No Record Found" HeaderStyle-CssClass="gridheader"
                    RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" SelectedRowStyle-CssClass="selectedrow"
                    DataKeyNames="SkuID,Saleschannelid,StockStatusID,StockBinTypeMasterID,SaleschanneltypeID" EditRowStyle-CssClass="editrow"
                    GridLines="None" Width="100%" OnRowCommand="grdvResult_RowCommand" OnRowDataBound="grdvResult_RowDataBound"
                    AllowPaging="True" OnPageIndexChanging="grdvResult_PageIndexChanging" PagerStyle-CssClass="PagerStyle">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="View Serial Number"
                            ShowHeader="true" HeaderStyle-Width="110px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSerial" Visible="true" runat="server" CommandArgument='<%# Eval("skuid") %>'
                                    CommandName="BindSerialDetail"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="SaleschannelName" HeaderStyle-HorizontalAlign="Left"
                            DataField="SaleschannelName">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="SalesChannelType" HeaderStyle-HorizontalAlign="Left"
                            DataField="SalesChannelTypeName">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Sku Code" HeaderStyle-HorizontalAlign="Left" DataField="SkuCode">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Sku Name" HeaderStyle-HorizontalAlign="Left" DataField="SkuName">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Bin Type" HeaderStyle-HorizontalAlign="Left" DataField="StockBinTypeDesc">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Status" HeaderStyle-HorizontalAlign="Left" DataField="StockStatusDesc">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left" DataField="Quantity">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Entity21" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SerialisedMode") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="selectedrow" />
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                    <AlternatingRowStyle CssClass="Altrow" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
        </div>
        <%--      <div class="contentbox" style="margin-bottom: 10px;">
                                                        <div class="grid2">
                                                            <asp:GridView ID="GrdSerialDetail" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                CellSpacing="1" EmptyDataText="No Record Found" HeaderStyle-CssClass="gridheader"
                                                                RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" SelectedRowStyle-CssClass="selectedrow"
                                                                EditRowStyle-CssClass="editrow" GridLines="None" Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Serial/Batch Number" HeaderStyle-HorizontalAlign="Left"
                                                                        DataField="StockSerialNo" />
                                                                    <asp:BoundField HeaderText="Sku Code" HeaderStyle-HorizontalAlign="Left" DataField="SkuCode" />
                                                                    <asp:BoundField HeaderText="Sku Name" HeaderStyle-HorizontalAlign="Left" DataField="SkuName" />
                                                                    <asp:BoundField HeaderText="Bin Type" HeaderStyle-HorizontalAlign="Left" DataField="StockBinTypeDesc" />
                                                                    <asp:BoundField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left" DataField="Quantity" />
                                                                    <asp:BoundField HeaderText="GRN Date" HeaderStyle-HorizontalAlign="Left" DataField="GRNDate" />
                                                                    <asp:BoundField HeaderText="Stock Status" HeaderStyle-HorizontalAlign="Left" DataField="StockStatusDesc" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>--%>
    </div>



    <%--#CC04 </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <asp:UpdatePanel ID="updDetail" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div id="dvDetail" runat="server" visible="false">
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GrdSerialDetail" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            CellSpacing="1" EmptyDataText="No Record Found" HeaderStyle-CssClass="gridheader"
                            RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" SelectedRowStyle-CssClass="selectedrow"
                            EditRowStyle-CssClass="editrow" GridLines="None" Width="100%"
                            AllowPaging="True" OnPageIndexChanging="GrdSerialDetail_PageIndexChanging">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField HeaderText="Serial/Batch Number" HeaderStyle-HorizontalAlign="Left"
                                    DataField="StockSerialNo">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Sku Code" HeaderStyle-HorizontalAlign="Left"
                                    DataField="SkuCode">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Sku Name" HeaderStyle-HorizontalAlign="Left"
                                    DataField="SkuName">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Bin Type" HeaderStyle-HorizontalAlign="Left"
                                    DataField="StockBinTypeDesc">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left"
                                    DataField="Quantity">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="GRN Date" HeaderStyle-HorizontalAlign="Left"
                                    DataField="GRNDate">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Stock Status" HeaderStyle-HorizontalAlign="Left"
                                    DataField="StockStatusDesc">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <SelectedRowStyle CssClass="selectedrow" />
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
