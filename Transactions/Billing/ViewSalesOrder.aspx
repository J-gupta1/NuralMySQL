<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewSalesOrder.aspx.cs" Inherits="Order_Common_ViewSalesOrder"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessageExtender.ascx" TagName="ucMessage" TagPrefix="uc8" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucEntityList_ver2.ascx" TagName="ucEntityList" TagPrefix="uc9" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../Assets/Css/dhtmlwindow.css" rel="stylesheet" />
    <link href="../../Assets/Css/modal.css" rel="stylesheet" />
    <script src="../../Assets/Scripts/modal.js"></script>
    <script src="../../Assets/Scripts/dhtmlwindow1.js"></script>
    <script type="text/javascript">

        function findControlIDTable(ServerID) {
            var FinalID;
            var tblcol = document.getElementsByTagName("Table");
            var lenght1 = tblcol.length;
            for (var i = 0; i < lenght1; i++) {
                if (tblcol[i].id != "") {
                    var gridid = String(tblcol[i].id);
                    var str2 = gridid.split("_");
                    var strlength = str2.length;
                    if (str2[strlength - 1] == String(ServerID)) {
                        FinalID = gridid;
                    }
                }
            }
            return FinalID;
        }

        function popup(orderid) {
            var WinProductDetails = dhtmlmodal.open("WinPopup1", "iframe", "../../Transactions/Billing/SalesOrderPrint.aspx?ReferenceID=" + orderid, "Print Order", "width=800px,height=550px,top=25,resize=0,scrolling=auto ,center=1")
            WinProductDetails.onclose = function () {
                var btn = document.getElementById("ctl00_contentHolderMain_btn");
                __doPostBack(btn.name, "OnClick");
                return true;
            }
            return false;
        }

        function setVisibility(visibility) {
            document.getElementById('ctl00_contentHolderMain_ucMessage1_dvShowError').style.display = visibility;
        }

        function PopupInvoice(ReferenceID, InvoiceNumber) {
            var WinProductDetails = dhtmlmodal.open("WinPopup1", "iframe", "../../Sales/Common/SalesInvoicePrintWHOrder.aspx?ReferenceID=" + ReferenceID + "&InvoiceNumber=" + InvoiceNumber, "Print Invoice", "width=900px,height=600px,top=25,resize=0,scrolling=auto ,center=1")
            WinProductDetails.onclose = function () {
                return true;
            }
            return true;
        }

        function PopupViewDeliveryChallanPrint(StockNumber, VirtualPath) {
            var DeliveryChallanPrint1 = dhtmlwindow.open("WinPopup2", "iframe", VirtualPath + StockNumber, "Delivery Note", "width=900px,height=550px,top=25,resize=0,scrolling=auto ,center=1")
            DeliveryChallanPrint1.onclose = function () {
                return true;
            }
            return false;
        }

        function PopupGRNPrint(StockOfEntityReceiveID, VirtualPath) {
            var InterServiceCentreGRNNotePrint = dhtmlwindow.open("WinPopup1", "iframe", VirtualPath + "/Inventory/Common/GRNPrint.aspx?id=" + StockOfEntityReceiveID, "GRN Note", "width=770px,height=500px,top=25,resize=0,scrolling=auto ,center=1")
            InterServiceCentreGRNNotePrint.onclose = function () {
                return true;
            }
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="box1">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uc8:ucMessage ID="ucMessage1" runat="server" XmlErrorSource="" Style="display: none" />
                <uc8:ucMessage ID="ucMessage2" runat="server" XmlErrorSource="" />

            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="subheading">
            Search Order
             
        </div>
    </div>
    <div class="box1">
        <div class="innerarea">
            <asp:UpdatePanel ID="updpnlSaveData" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
            <div class="mandatory">
                (*) Marked fields are mandatory            
            </div>
            <div class="H30-C3-S">
                <ul>

                    <li class="text">Order Type:<span class="error">*</span></li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="formselect" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged" />
                        </div>
                        <asp:RequiredFieldValidator ID="reqOrderType" runat="server" ControlToValidate="ddlOrderType"
                            CssClass="error" Display="Dynamic" InitialValue="0" SetFocusOnError="true" ValidationGroup="V1"></asp:RequiredFieldValidator>
                    </li>

                    <li class="text"><span id="liParentHeader" runat="server">Order From:</span><span class="error">*</span></li>
                    <li class="field">

                        <uc9:ucEntityList ID="cmbOrderFrom" runat="server" ValidationGroup="V1" OnCheckedEntities="ucOrderFrom_SelectedEntity"
                            SelectionMode="CheckColumn" AutoPostBack="True" SetInitialValue="Please Select."
                            InitialValue="Please Select." />
                    </li>
                    <li class="text" id="liChildHeader" runat="server">Order To:</li>
                    <li class="field">
                        <uc9:ucEntityList ID="cmbOrderTo1" runat="server" ValidationGroup="V1" SetInitialValue="Please Select."
                            SelectionMode="CheckColumn" InitialValue="Please Select." />
                    </li>
                    <li class="text">Order Number:<font class="error">+</font></li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="txtPoNumber" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                        </div>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPoNumber"
                            CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="[^&lt;&gt;/\@%()]{1,50}"
                            ValidationGroup="order"></asp:RegularExpressionValidator>
                    </li>
                    <li class="text">From Date:<font class="error">+</font></li>
                    <li class="field">
                        <uc1:ucDatePicker ID="ucFromDate" runat="server" CssClass="formselect" RangeErrorMessage="Invalid date!"
                            IsRequired="false" ErrorMessage="Please select date!" />
                    </li>
                    <li class="text">To Date:<font class="error">+</font></li>
                    <li class="field">
                        <uc1:ucDatePicker ID="ucToDate" runat="server" CssClass="formselect" RangeErrorMessage="Invalid date!"
                            IsRequired="false" ErrorMessage="Please select date!" />
                    </li>
                    <li class="text">Allocation Status:<font class="error">+</font></li>
                    <li class="field">
                        <asp:DropDownList ID="cmbAllocStatus" runat="server" CssClass="formselect">
                            <asp:ListItem Text="Select" Value="255" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Partially Allocated" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Fully Allocated" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Under Approval For Cancel" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Cancelled" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Transferred" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li class="text">Credit Check:<font class="error">+</font></li>
                    <li class="field">
                        <asp:DropDownList ID="ddlCreditCheck" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </li>
                    <li class="text"></li>
                    <li class="field">
                        <div class="float-margin">

                            <asp:Button ID="btnSearch" runat="server" ValidationGroup="V1" CssClass="buttonbg"
                                Text="Search" OnClick="btnSearch_Click" />
                        </div>
                        <div class="float-left">
                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" OnClick="btnCancel_Click"
                                Text="Reset" CausesValidation="false" />
                        </div>
                    </li>
                </ul>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
     <asp:UpdatePanel ID="updSearch" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
    <div runat="server" id="div2" visible="false">
        <div class="box1" runat="server" id="divgrd">
            <div class="mainheading">
                Order List
            </div>
            <div class="export">
                <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="Exporttoexcel_Click" CausesValidation="False" />
            </div>
            <div class="innerarea">
                <div class="grid1">
                    <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" DataKeyNames="SalesOrderID"
                        EmptyDataText="No Record Found" GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader"
                        RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow"
                        OnRowCommand="grdvList_RowCommand" OnRowDataBound="grdvList_RowDataBound" OnRowCancelingEdit="grdvList_RowCancelingEdit">
                        <RowStyle CssClass="gridrow" />
                        <Columns>
                            <asp:BoundField DataField="OrderNumber" HeaderText="Order Number" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="PackingSlipNo" HeaderText="PackingSlip No" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="OrderDate" HeaderText="Order Date" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OrderFrom" HeaderText="Order From" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderId" runat="server" Text='<%# Bind("SalesOrderID")  %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="OrderTo" HeaderText="OrderTo" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CreditChkStatusText" HeaderText="Credit Check Status"
                                HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AlocationStatus" HeaderText="AlocationStatus" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:ButtonField CausesValidation="false" CommandName="Select" Text="Select" Visible="false"
                                HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:ButtonField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSOTOID" runat="server" Visible="false" Text='<%#Bind("ToId")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblApplyCancel" runat="server" Text='<%# Bind("IsApplyCancel") %>'></asp:Label>
                                    <asp:Label ID="lblCancelSOVisibility" runat="server" Visible="false" Text='<% #Bind("CancelSOVisibility") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False" HeaderText="Action" HeaderStyle-HorizontalAlign="Left"
                                HeaderStyle-Width="110px">
                                <ItemTemplate>
                                    <asp:ImageButton ActionTag="View" ID="btnPrint" runat="server" CommandArgument='<%# Eval("SalesOrderID") %>'
                                        CommandName="Print" ImageUrl="~/Assets/Images/print.gif" AlternateText="Print Sales Order"
                                        Visible="True" title="Allocate" ToolTip="Print Order" CausesValidation="false" />
                                    <asp:ImageButton ActionTag="View" ID="btnAdditionalDetails" runat="server" CommandArgument='<%# Eval("SalesOrderID") %>'
                                        CommandName="AdditionalDetails" ImageUrl="~/Assets/Images/Details1.png" AlternateText="View Additional Details"
                                        Visible="True" title="Additional Details" ToolTip="View Additional Details" CausesValidation="false" />
                                    <asp:ImageButton ActionTag="View" ID="btnItemDetails" runat="server" CommandArgument='<%# Eval("SalesOrderID") %>'
                                        CommandName="ItemDetails" ImageUrl="~/Assets/Images/Details2.png" AlternateText="ItemDetails"
                                        Visible="True" title="ItemDetails" ToolTip="View Item Details" CausesValidation="false" />
                                    <asp:ImageButton ActionTag="View" ID="btnPrintInvoice" runat="server" CommandArgument='<%# Eval("InvoiceNumber") %>'
                                        CommandName="PrintInvoice" ImageUrl="~/Assets/Images/WOPrint.gif" AlternateText="Print Invoice"
                                        Visible="false" title="Print Invoice" ToolTip="Print Invoice" CausesValidation="false" />
                                    <asp:ImageButton ActionTag="View" ID="btnPrintDispatch" runat="server" CommandArgument='<%# Eval("DispatchNo") %>'
                                        CommandName="PrintDispatch" ImageUrl="~/Assets/Images/print.gif" AlternateText="Print Dispatch"
                                        Visible="false" title="Print Dispatch" ToolTip="Print Dispatch" CausesValidation="false" />
                                    <asp:ImageButton ActionTag="View" ID="btnGrnPrint" runat="server" CommandArgument='<%# Eval("StockOfEntityReceiveID") %>'
                                        CommandName="PrintGRN" ImageUrl="~/Assets/Images/print.gif" AlternateText="Print GRN"
                                        Visible="false" title="Print GRN" ToolTip="Print GRN" CausesValidation="false" />
                                    <asp:ImageButton ActionTag="Edit" ID="imgCancel" runat="server" CommandArgument='<%# Eval("SalesOrderID") %>'
                                        CommandName="Cancel" ImageUrl="~/Assets/Images/icon_cancel.gif" AlternateText="Cancel"
                                        Visible="false" title="Cancel" ToolTip="Cancel" CausesValidation="false" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="110px" />
                            </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle CssClass="selectedrow" />
                        <HeaderStyle CssClass="gridheader" />
                        <EditRowStyle CssClass="editrow" />
                        <AlternatingRowStyle CssClass="Altrow" />
                    </asp:GridView>
                </div>
                <div class="clear">
                </div>
                <div id="dvFooter" runat="server" class="pagination">
                    <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                </div>
            </div>
        </div>
    </div>
     </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExprtToExcel" />
        </Triggers>
    </asp:UpdatePanel>
     <asp:UpdatePanel ID="updAdditionalDetails" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
    <div runat="server" id="divAdditionalDetails" visible="false">
        <div class="box1" runat="server">
            <div class="subheading">
                <div class="float-left">
                    Additional Details
                </div>
                <div class="export">
                </div>
            </div>
            <div class="innerarea">
                <div class="grid1">
                    <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" DataKeyNames="SalesOrderID"
                        EmptyDataText="No Record Found" GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader"
                        RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow">
                        <RowStyle CssClass="gridrow" />
                        <Columns>
                            <asp:BoundField DataField="SOTotalQuantity" HeaderText="Total Quantity" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SOOpenQuantity" HeaderText="Open Quantity" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SOGrossAmount" HeaderText="Gross Amount" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SOTaxAmount" HeaderText="Tax Amount" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SONetAmount" HeaderText="Net Amount" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ReferenceNumber" HeaderText="Reference Number" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DocumentName" HeaderText="Form Type" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FreightStatus" HeaderText="Freight Status" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BookingDetails" HeaderText="Booking Details" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExciseRegNo" HeaderText="Excise Reg No" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CargoTOT" HeaderText="Cargo/TOT" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CTRemarks" HeaderText="Cancel/Transfer Remarks" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CreatedOn" HeaderText="Created On" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ModifiedBy" HeaderText="Last Modified By" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ModifiedOn" HeaderText="Last Modified On" HeaderStyle-HorizontalAlign="Left">
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
    </div>
     </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updItemDetails" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
    <div runat="server" id="divItemDetails" visible="false">
        <div class="box1" runat="server" id="div4">
            <div class="subheading">
                <div class="float-left">
                    Item Details
                </div>
                <div class="export">
                </div>
            </div>
            <div class="innerarea">
                <div class="grid1">
                    <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" DataKeyNames="SalesOrderID"
                        EmptyDataText="No Record Found" GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader"
                        RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow">
                        <RowStyle CssClass="gridrow" />
                        <Columns>
                            <asp:BoundField DataField="SAPPartCode" HeaderText="Part Code" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PartName" HeaderText="Part Name" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UOMDescription" HeaderText="UOM" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OrderQty" HeaderText="Order Qty" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OpenQty" HeaderText="Open Qty" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GrossItemPrice" HeaderText="Gross Item Price" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ItemTax" HeaderText="Item Tax" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NetItemPrice" HeaderText="Net Item Price" HeaderStyle-HorizontalAlign="Left">
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
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
     <asp:UpdatePanel ID="updCancelRemarks" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
    <div runat="server" id="divCancel" visible="false">
        <div class="box1" runat="server" id="div3">
            <div class="subheading">
                <div class="float-left">
                    Cancel Remarks<span class="mandatory-img">&nbsp;</span>
                </div>
            </div>
            <div class="innerarea">
                <div class="H30-C3-S">
                    <ul>
                        <li class="field">
                            <uc6:ucTextboxMultiline ID="txtCancelAllocation" runat="server" CharsLength="250"
                                TextBoxWatermarkText="Enter Cancel Remarks" IsRequired="true" ValidationGroup="CancelOrder" ErrorMessage="Enter Remarks" />
                        </li>
                    </ul>
                </div>
            </div>
            <div class="innerarea-margin">
                <div>
                    <asp:Button ID="btnCancelProcess" runat="server" CssClass="buttonbg" Text="Save" OnClick="BtnCancelProcess_Click"
                        ValidationGroup="CancelOrder" />
                </div>
            </div>
        </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
