<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BulkRetailerTransferv2.aspx.cs" Inherits="Masters_SalesMan_BulkRetailerTransferv2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucPagingControl.ascx" TagName="ucPagingControl"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/GridClientSide.ascx" TagName="GridClientSide" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">


        var col1_InputName = "RetailerName";
        var col2_InputName = "RetailerCode";
        var col3_InputName = "Address";
        var RowsCount = 0;
        function SelectAll(chk, SelectFrom) {
            var grid = document.getElementById('ctl00_contentHolderMain_grdRetailerFrom');
            var SelectedCount = document.getElementById('ctl00_contentHolderMain_lblSelectedCount');
            var SelectedAll = document.getElementById('lnkSelectAll');
            var Clear = document.getElementById('lnkClear');
            document.getElementById('ctl00_contentHolderMain_hdnComingFrom').value = 0;
            var cells;

            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            if (SelectFrom == 1)/*User select all retailer to transfer*/ {
                                document.getElementById(chk.id).checked = true;
                                cell.childNodes[j].checked = document.getElementById(chk.id).checked;
                                document.getElementById(chk.id).disabled = true;
                                cell.childNodes[j].disabled = true;
                                document.getElementById('ctl00_contentHolderMain_cmbSalesManfrom').disabled = true;
                                document.getElementById('ctl00_contentHolderMain_btnSearchRetailer').disabled = true;
                            }
                            else if (SelectFrom == 2)/*User do clear*/ {
                                document.getElementById(chk.id).checked = false;
                                cell.childNodes[j].checked = document.getElementById(chk.id).checked;
                                document.getElementById(chk.id).disabled = false;
                                cell.childNodes[j].disabled = false;
                                location.reload();
                            }
                            else {
                                cell.childNodes[j].checked = document.getElementById(chk.id).checked;
                                document.getElementById(chk.id).disabled = false;
                                cell.childNodes[j].disabled = false;
                            }



                            if (document.getElementById(chk.id).checked) {
                                document.getElementById('addRetailer').style.display = 'Block';
                                SelectedCount.innerHTML = grid.rows.length - 1;
                                SelectedAll.style.display = 'Block';
                            }
                            else {
                                document.getElementById('addRetailer').style.display = 'none';
                                SelectedCount.innerHTML = '0';
                                SelectedAll.style.display = 'none';
                            }
                        }
                    }
                }
                document.getElementById('tblGrid').style.display = 'block';
                if (SelectFrom == 1) {
                    SelectedAll.style.display = 'none';
                    document.getElementById('addRetailer').style.display = 'none';
                    SelectedCount.innerHTML = grid.rows.length - 1;
                    document.getElementById('ctl00_contentHolderMain_hdnComingFrom').value = 1;
                    document.getElementById('tblGrid').style.display = 'none';
                    Clear.style.display = 'block';
                }
                if (SelectFrom == 2) {
                    SelectedAll.style.display = 'none';
                    document.getElementById('addRetailer').style.display = 'none';
                    SelectedCount.innerHTML = 0;
                    document.getElementById('ctl00_contentHolderMain_hdnComingFrom').value = 0;
                    document.getElementById('tblGrid').style.display = 'block';
                    Clear.style.display = 'none';
                    document.getElementById(chk.id).checked = false;
                }
            }
        }

        function SelectAllOneByOne() {

            var grid = document.getElementById('ctl00_contentHolderMain_grdRetailerFrom');
            document.getElementById('ctl00_contentHolderMain_grdRetailerFrom_ctl01_chkAll').checked = false;
            var SelectedCount = document.getElementById('ctl00_contentHolderMain_lblSelectedCount');
            var SelectedAll = document.getElementById('lnkSelectAll');
            SelectedAll.style.display = 'none';
            var cell, countFalseCheckbox = 1;
            if (grid.rows.length > 1) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            if (cell.childNodes[j].checked == false) {
                                countFalseCheckbox = countFalseCheckbox + 1;
                            }
                        }
                    }
                }
                SelectedCount.innerHTML = grid.rows.length - countFalseCheckbox;
                if (countFalseCheckbox == grid.rows.length)
                    document.getElementById('addRetailer').style.display = 'none';
                else
                    document.getElementById('addRetailer').style.display = 'block';
            }
        }

        function AddRetailer() {

            var grid = document.getElementById('ctl00_contentHolderMain_grdRetailerFrom');
            var cells;
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            if (cell.childNodes[j].checked) {
                                var table = document.getElementById('ctl00_contentHolderMain_grdRetailerFrom');
                                Row = table.rows[i];
                                var RetailerName = Row.cells[1].innerHTML;
                                var RetailerCode = Row.cells[2].innerHTML;
                                var Address = Row.cells[3].innerHTML;
                                //var ID= Row.cells[4].innerHTML.replace("/", "").split('<SPAN>');
                                //alert(ID[0].split('">')[1]);
                                $("input", oTable.fnGetNodes()).each(function () {

                                    var row = $(this).closest('tr').get(0);
                                    var aPos = oTable.fnGetPosition(row);
                                    var aData = oTable.fnGetData(aPos);
                                    var ParentRowText = $(row).text();
                                    var prtCodeArray = ParentRowText.split(' ');
                                });

                                var nNodes = oTable.fnGetNodes();
                                if (nNodes.length > 0) {
                                    for (var d = 0; d < nNodes.length; d++) {
                                        var dat = oTable.fnGetData(nNodes[d]);
                                        var FilteredString = dat[1].replace("/", "");
                                        var Code = FilteredString.split('<span>');
                                        if (RetailerCode.trim() == Code[1].trim()) {
                                            alert(RetailerName + ' ( ' + RetailerCode + ' ) ' + ' already exist.');
                                            return false;
                                        }
                                    }
                                }

                                $('#dtParts').dataTable().fnAddData([
                                    getRenderingRow(RetailerName, col1_InputName),
                                    getRenderingRow(RetailerCode, col2_InputName),
                                    getRenderingRow(Address, col3_InputName)]);
                                giCount++;

                                var sData = oTable.$('input:hidden').serialize();
                                document.getElementById('lbl').value = sData;
                                document.getElementById('tblGrid').style.display = 'block';

                            }
                        }
                    }
                }
            }

        }
        function getRenderingRow(ItsVal, ItsName) {
            return "<span>" + ItsVal + " <span/><input type=hidden name=" + ItsName + " value=" + ItsVal + " />"
        }

        //function hideControl()
        //{
        //    if (document.getElementById('ctl00_contentHolderMain_hdnXMLOccer').value == "1") {
        //        document.getElementById('ctl00_contentHolderMain_ucMessage1_pnlUcMessageBox').style.display = 'none';
        //    }
        //}

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnXMLOccer" runat="server" Value="0" />
            <uc1:ucMessage ID="ucMessage2" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updLoad" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnIndex" runat="server" Value="0" />
            <div class="mainheading">
                <asp:Label ID="lblStockTransfer2" Text=" Bulk Retailer Transfer" runat="server" />
            </div>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="hd5 margin-bottom">
                    Transfer From:
                </div>
                <div class="H35-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label1" runat="server" Text="">Sales Channel From :<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbTransferFrom" CssClass="formselect" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbTransferFrom_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="cmbTransferFrom"
                                    CssClass="error" ErrorMessage="Please select SalesChannel whose retailers you want to transfer"
                                    InitialValue="0" ValidationGroup="Add" />
                            </div>
                        </li>
                        <li class="text">Salesman From :<span class="error" id="saleFrom" runat="server">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbSalesManfrom" runat="server" CssClass="formselect" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbSalesManFrom_SelectedIndexChanged">
                                </asp:DropDownList>

                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequSalesManFrom" ControlToValidate="cmbSalesManfrom"
                                    CssClass="error" ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select from Sales Channel"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="field3">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                OnClick="BtnCancel_Click" CausesValidation="false" />
                        </li>
                    </ul>
                </div>
            </div>
            <div id="Pnlfrom" runat="server" visible="false">
                <div class="mainheading">
                    <asp:Label ID="Label2" Text=" Search Retailer" runat="server" />
                </div>
                <div class="contentbox">
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                            Retailer Name :</td>
                             <li class="field">
                                 <asp:TextBox ID="txtRetailerName" CssClass="formfields" runat="server"></asp:TextBox>
                             </li>
                            <li class="text">Retailer Code :</li>
                            <li class="field">
                                <asp:TextBox ID="txtRetailerCode" runat="server" CssClass="formfields"></asp:TextBox></li>
                            <li class="field3">
                                <asp:Button ID="btnSearchRetailer" runat="server" Text="Search" CssClass="buttonbg" OnClick="btnSearchRetailer_Click" />
                            </li>
                        </ul>

                        <%--    <asp:Button ID="exportToExel" Text=" " runat="server" OnClick="exportToExel_Click"
                                                CssClass="excel" />--%>
                    </div>
                    <div>
                        <div class="mainheading">
                            Selected
                                <asp:Label ID="lblSelectedCount" Font-Bold="true" Font-Size="14px" CssClass="error" runat="server" Text="0" />&nbsp;Out of
                                <asp:Label ID="lblTotalCount" Font-Bold="true" Font-Size="14px" CssClass="error" runat="server" />
                        </div>
                    </div>
                    <div class="export">
                        <a href="javascript:void(0);" class="elink2" style="display: none" onclick="SelectAll(ctl00_contentHolderMain_grdRetailerFrom,1);" id="lnkSelectAll" name="lnkSelectAll">Click to Select All for All Pages</a>
                        <asp:HiddenField ID="hdnComingFrom" runat="server" Value="0" />

                        <a href="javascript:void(0);" class="elink2" style="display: none" onclick="SelectAll(ctl00_contentHolderMain_grdRetailerFrom,2);" id="lnkClear" name="lnkClear">Clear</a>
                    </div>

                    <div class="grid1">
                        <asp:GridView ID="grdRetailerFrom" runat="server" FooterStyle-VerticalAlign="Top"
                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="None"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="False"
                            PageSize="10" SelectedStyle-CssClass="gridselected" DataKeyNames="RetailerID" EmptyDataText="No record found">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridrow"></RowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Retailer" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" onClick="SelectAll(this,0);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRetailerTransfer" runat="server" onClick="SelectAllOneByOne()" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Retailer Name" DataField="RetailerName" />
                                <asp:BoundField HeaderText="Retailer Code" DataField="RetailerCode" />
                                <asp:BoundField HeaderText="Address" DataField="address" />
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RetailerID")%>'
                                            Style="display: none"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridheader"></HeaderStyle>
                            <AlternatingRowStyle CssClass="Altrow"></AlternatingRowStyle>
                        </asp:GridView>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="pagination">
                        <div id="dvFooter" runat="server">
                            <uc2:ucPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                        </div>
                    </div>
                    <div class="margin-top">
                        <a href="javascript:void(0);" class="buttonbg" style="display: none; width:100px;" id="addRetailer" name="addSerials"
                            onclick="AddRetailer();">Add Retailer</a>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="tblGrid">
        <div class="contentbox">
            <div style="display: block;">
                <uc3:GridClientSide ID="GridClientSide1" runat="server" GridColumnNames="RetailerName,RetailerCode,Address"
                    DataTableColumnNames="RetailerName,RetailerCode,Address" DataTableColumnTypes="string,string,string"
                    ControlTypes="label,label,label"></uc3:GridClientSide>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="tblToDetails" runat="server" visible="false">
                <div class="contentbox">
                    <div class="hd5 margin-bottom">
                        Transfer To:
                    </div>
                    <div class="H35-C3">
                        <ul>
                            <li class="text">
                                <asp:Label ID="Label3" runat="server" Text="">Sales Channel To: <span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="cmbTransferTo" CssClass="formselect" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="cmbTransferTo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="cmbTransferTo"
                                        CssClass="error" ErrorMessage="Please select a SalesChannel whose retailers you want to transfer"
                                        InitialValue="0" ValidationGroup="Add" />
                                </div>
                            </li>
                            <li class="text">Salesman To: <span class="error" id="saleTo" runat="server">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="cmbSalesManTo" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="cmbSalesManTo" CssClass="error"
                                        ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel name"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                        </ul>
                        <ul>
                            <li class="text" runat="server" id="tdLabel">Reporting Hierarchy Name: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlOrghierarchy" runat="server" CssClass="formselect" ValidationGroup="Add">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqOrgnhierarchy" runat="server" ControlToValidate="ddlOrghierarchy"
                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please Select Orgn. Hierarchy Name."
                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updateButton" runat="server">
        <ContentTemplate>
            <div class="margin-bottom">
                <div class="float-margin">
                    <asp:Button ID="btnTransfer" runat="server" Text="Transfer Retailer" CssClass="buttonbg" OnClick="btnTransfer_Click" ValidationGroup="Add" />
                </div>
                <div class="float-margin">
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClick="BtnCancel_Click" CausesValidation="false" />
                </div>
                <div class="clear"></div>
            </div>
        </ContentTemplate>
        <%--#CC01 Add Start --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnTransfer" />
        </Triggers>
        <%--#CC01 Add End --%>
    </asp:UpdatePanel>
</asp:Content>
