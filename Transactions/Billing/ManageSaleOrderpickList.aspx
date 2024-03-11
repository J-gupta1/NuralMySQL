<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageSaleOrderPickList.aspx.cs" EnableEventValidation="false"
    Inherits="Order_Common_ManageSaleOrderPickList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessageExtender.ascx" TagName="ucmessage" TagPrefix="uc10" %>
<%@ Register Src="~/UserControls/ucStatusControl.ascx" TagName="ucStatus" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucEntityList.ascx" TagName="ucEntityList" TagPrefix="uc9" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="../../Assets/Css/dhtmlwindow.css" rel="stylesheet" />

    <link href="../../Assets/Css/modal.css" rel="stylesheet" />
    <script src="../../Assets/Scripts/dhtmlwindow1.js"></script>
    <script src="../../Assets/Scripts/modal.js"></script>
    <script src="../../Assets/Scripts/DevExValidationCampatibility.js"></script>

    <style type="text/css">
        .hide {
            display: none;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function SelectAllCheckboxes(spanChk) {
            var collection = document.getElementById('<%= grdvList.ClientID %>').getElementsByTagName('INPUT');
            for (var x = 0; x < collection.length; x++) {
                if (collection[x].type.toUpperCase() == 'CHECKBOX')
                    collection[x].checked = spanChk.checked;

            }
        }

        function OnCheckUnCheck(spanChk) {
            var varchkAll = document.getElementById('ctl00_contentHolderMain_chkAll');
            var varchkSelect = spanChk;
            if (!varchkSelect.checked) {
                varchkAll.checked = false;
            }
            else {
                var count = 1;
                var collection = document.getElementById('<%= grdvList.ClientID %>').getElementsByTagName('INPUT');
                for (var x = 0; x < collection.length; x++) {
                    if (collection[x].type.toUpperCase() == 'CHECKBOX') {
                        if (!collection[x].checked) {
                            count = 0;
                        }
                    }
                }
                if (count == 1)
                    varchkAll.checked = true;
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        function CheckData(openqty, txtbox1) {

            if (Number(txtbox1.value) > 0) {

                if (Number(txtbox1.value) > Number(openqty)) {
                    alert("The allocated quantity can't be greater than pending quantity");
                    txtbox1.value = "";
                    return false;
                }


                return true;
            }

        }
    </script>


    <script type="text/javascript">
        function SetBulkUploadOnChange(id) {
            var chkBulk = document.getElementById('ctl00_contentHolderMain_chkBoxBulkupload');
            var hidegrid = document.getElementById('ctl00_contentHolderMain_div2');
            var divBulk = document.getElementById('divBulk');
            if (id == 1) {
                //chkBulk.checked == false;
                divBulk.style.display = 'none';
                hidegrid.style.display = 'block';
            }
            else if (id == 3) {
                //chkBulk.checked == true;
                divBulk.style.display = 'none';
                hidegrid.style.display = 'none';
            }
            else {
                if (chkBulk.checked) {
                    divBulk.style.display = 'block';
                    hidegrid.style.display = 'none';
                }
                else {
                    divBulk.style.display = 'none';
                    hidegrid.style.display = 'block';
                }
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="box1">
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uc10:ucmessage ID="ucMessage1" runat="server" XmlErrorSource="" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:UpdatePanel ID="updpnlSaveData" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="innerarea">
                    <div class="H30-C3-S">
                        <ul>
                            <li class="text">Order To:<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">

                                <asp:DropDownList ID="cmbOrderTo" runat="server" ValidationGroup="search" CssClass="formselect" AutoPostBack="True" OnSelectedIndexChanged="cmbOrderTo_SelectedIndexChanged1" />

                                <asp:RequiredFieldValidator ID="ReqFcmbOrderTo" ControlToValidate="cmbOrderTo" Display="Dynamic"
                                    CssClass="error" ValidationGroup="search" InitialValue="0" runat="server" ErrorMessage="Please Select Order To."> 
                                </asp:RequiredFieldValidator>
                            </li>

                            <li class="text">Order From:</li>
                            <li class="field">
                                <asp:DropDownList ID="cmbOrderFrom" runat="server" CssClass="formselect" />
                            </li>
                            <li class="text">Order Number:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtOrderNumber" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOrderNumber"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="[^&lt;&gt;/\@%()]{1,50}"></asp:RegularExpressionValidator>
                            </li>
                            <li class="text">From Date:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <uc1:ucDatePicker ID="ucFromDate" runat="server" CssClass="formselect" RangeErrorMessage="Invalid date!"
                                    IsRequired="false" ErrorMessage="Please select date!" />
                            </li>
                            <li class="text">To Date:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <uc1:ucDatePicker ID="ucToDate" runat="server" CssClass="formselect" RangeErrorMessage="Invalid date!"
                                    IsRequired="false" ErrorMessage="Please select date!" />
                            </li>
                            <li class="text">Part Code:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtPartCode" runat="server" CssClass="formfields" MaxLength="50"
                                    ValidationGroup="search"></asp:TextBox>
                            </li>

                            <li>
                                <div class="float-margin">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="button1" Text="Search" ValidationGroup="search"
                                        OnClick="btnLoad_Click" OnClientClick="javascript:SetBulkUploadOnChange(1);" />
                                    <%--#CC17 Added OnClientClick="javascript:SetBulkUploadOnChange();" --%>
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="button1" OnClick="btnCancel_Click"
                                        Text="Reset All" CausesValidation="false" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnExport" runat="server" CssClass="button5" Text="Export To Excel"
                                        CausesValidation="false" OnClick="btnExport_Click" />
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="button2" Text="Run Auto Allocation"
                                        CausesValidation="false" OnClick="btnRefresh_Click" />
                                </div>
                            </li>
                            <li>
                                <asp:TextBox ID="textbox1" runat="server" Style="display: none"></asp:TextBox></li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="updSearch" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div runat="server" id="div2" class="box1">
                <div runat="server" id="divgrd">
                    <div class="subheading">
                        <div class="float-left">
                            Order List
                        </div>
                        <div class="export">
                        </div>
                    </div>
                </div>
                <div class="innerarea">
                    <div id="dvChkAll" runat="server" style="display: none;">
                        <div class="float-margin">
                            &nbsp;<input id="chkAll" runat="server" onclick="javascript: SelectAllCheckboxes(this);"
                                type="checkbox" />
                        </div>
                        <div class="float-margin">
                            <span class="frmtxt1">Select All</span>
                        </div>
                    </div>

                    <div class="clear">
                    </div>

                    <div class="grid1">
                        <asp:Panel ID="gvPanel" runat="server" ScrollBars="Auto">

                            <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" DataKeyNames="SalesOrderAllocationDetailID"
                                EmptyDataText="No Record Found" GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader"
                                RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow"
                                OnRowDataBound="grdvList_RowDataBound">
                                <RowStyle CssClass="gridrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBoxModule" runat="server" onclick="OnCheckUnCheck(this);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EntityName" HeaderText="Entity Name" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SONumber" HeaderText="Order Number" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SODate" HeaderText="Order Date" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="SalesOrderType" HeaderText="Order Type" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="150px" HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ReferenceType" HeaderText="Ref Type" HeaderStyle-HorizontalAlign="Left"><%--#CC22 Changed name Order Type to Ref Type--%>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ReferenceNumber" HeaderText="Reference Number" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="RequestedCode" HeaderText="Requested Part Code " HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>


                                    <asp:BoundField DataField="RequestedPartDesc" HeaderText="Requested Part Code Desc" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="SAPPartCode" HeaderText="Allocated Part Code " HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="StockQuantity" HeaderText="Current Good Stock" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RequestedQty" HeaderText="Requested Qty" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Pending Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPendingQty" runat="server" Text='<%# Bind("OpenQty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="StockMode" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStockModeMasterIDRevised" runat="server" Text='<%# Bind("StockModeMasterIDRevised") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblDropdown" runat="server" Visible="false" Text='<%# Bind("StockModeStatusForAllocate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Allocated Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty" runat="server" MaxLength="4" CssClass="formfields" Text='<%# Bind("AllocatedQtyRevised") %>'>
                                            </asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="fltrquantityt" runat="server" TargetControlID="txtQty"
                                                FilterMode="ValidChars" ValidChars="0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:Label ID="lblAllocationID" runat="server" Visible="false" Text='<%# Bind("SalesOrderAllocationDetailID") %>'></asp:Label>
                                            <asp:Label ID="lblSalesOrderDetailId" runat="server" Visible="false" Text='<%# Bind("SalesOrderDetailID") %>'></asp:Label>
                                            <asp:Label ID="lblStockModeMasterId" runat="server" Visible="false" Text='<%# Bind("StockModeMasterIDRevised") %>'></asp:Label>
                                            <asp:Label ID="lblRequestedQty" runat="server" Visible="false" Text='<%# Bind("RequestedQty") %>'></asp:Label>

                                            <asp:Label ID="lblFromEntityId" runat="server" Visible="false" Text='<%# Bind("EntityID") %>'></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField CausesValidation="false" CommandName="Select" Text="Select" Visible="false"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:ButtonField>
                                </Columns>
                                <SelectedRowStyle CssClass="selectedrow" />
                                <HeaderStyle CssClass="gridheader" />
                                <EditRowStyle CssClass="editrow" />
                                <AlternatingRowStyle CssClass="Altrow" />
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>

                </div>
                <div class="clear2">
                </div>
                <div id="dvButton" runat="server">
                    <div class="float-margin">
                        <asp:Button ID="btnSubmitCurrent" runat="server" Text="Generate Pick List(Current Page)"
                            CssClass="button2" ToolTip="Only Selected records of current page will be considered."
                            OnClick="btnSubmitCurrent_Click" />
                    </div>
                    <div class="float-margin" style="padding-left: 80px">
                        <asp:Button ID="btnSave" runat="server" Text="Save Current Page Changes" ToolTip="Only Selected records of current page will be considered. No picklist will be generated only allocation will be save."
                            CssClass="button2" OnClick="btnSave_Click" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnSubmitAll" runat="server" Text="Generate Pick List(All Page)"
                            CssClass="button2" ToolTip="Pick list will be generated for all records of all pages where allocated qty is available. No need to select the records."
                            OnClick="btnSubmitAll_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="clear">
    </div>
    <div class="box1">
        <div class="subheading padding-left">
            <asp:CheckBox ID="chkBoxBulkupload" runat="server" Text="Bulk Upload Sales Order Pick List" onclick="javascript:SetBulkUploadOnChange(2);" />
        </div>

        <div id="divBulk" style="display: none;">
            <div>
                <div class="innerarea">
                    <div>
                        <asp:UpdatePanel runat="server" ID="PnlUpload">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnexcelupload" />
                                <asp:PostBackTrigger ControlID="lnkDownloadTemplate" />

                            </Triggers>
                            <ContentTemplate>
                                <div class="H30-C3-S">
                                    <ul>
                                        <li class="text">Upload Excel : <span class="mandatory-img">&nbsp;</span></li>
                                        <li class="field">
                                            <div>

                                                <asp:FileUpload ID="fileUpdExcel" runat="server" CssClass="fileuploads" />
                                                <div>
                                                    <asp:RequiredFieldValidator ID="reqfileUpdExcel" Display="Dynamic" runat="server" ValidationGroup="val"
                                                        ControlToValidate="fileUpdExcel" ErrorMessage="Please select excel sheet!" CssClass="error"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                        </li>
                                        <li class="field3">

                                            <div class="float-margin">
                                                <cc2:ZedButton ID="btnexcelupload" runat="server" Text="Upload" CssClass="button1" ValidationGroup="val"
                                                    ActionTag="Add" OnClick="btnexcelupload_Click" />
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="clear">
                        </div>
                        <div class="formlink">
                            <ul>
                                <li>
                                    <div class="float-margin">
                                        <asp:HyperLink ID="HyperLinkTempDownload" runat="server" CssClass="link1">Click Here to Download Template</asp:HyperLink>

                                    </div>
                                </li>
                                <li>

                                    <div class="float-margin">
                                        <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="linkError" Visible="true"
                                            Style="font-size: 15px;"></asp:HyperLink>
                                    </div>
                                    <div class="float-margin">
                                        <asp:HyperLink ID="hlnkDuplicate" runat="server" CssClass="linkError"></asp:HyperLink>
                                    </div>
                                    <div class="float-margin">
                                        <asp:HyperLink ID="hlnkBlank" runat="server" CssClass="linkError"></asp:HyperLink>
                                    </div>

                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Style="display: none" />
</asp:Content>
