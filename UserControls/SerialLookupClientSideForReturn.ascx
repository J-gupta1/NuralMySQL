<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SerialLookupClientSideForReturn.ascx.cs"
    Inherits="Web.Controls.SerialLookupClientSideForReturn" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript" charset="utf-8">
    /* Global var for counter */
    var giCount = 1;
    var oTable;
    var giRedraw = false;


    function changeDisplay(display) {

        var dtSerials = document.getElementById('container');
        var trSerials = document.getElementById('trSerials');
        if (display == "false") {
            dtSerials.style.display = 'none';
            trSerials.style.display = '';
        }
        else {
            trSerials.style.display = 'none';
            dtSerials.style.display = '';
        }
    }


    function eventFired(type)
    { var n = document.getElementById('demo_info'); n.innerHTML += '<:div>:' + type + ' event - ' + new Date().getTime() + '<:/div>:'; n.scrollTop = n.scrollHeight; }


    $(document).ready(function() {
        $('#dtSerials').dataTable();

    });
    $(document).ready(function() {
        $('#aspnetForm').submit(function() {
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
            return true;
        });

        oTable = $('#dtSerials').dataTable();
    });


    function fnGetValueFromControls(partCodeControl, QtyControl) {


        var pc = document.getElementById(partCodeControl).value;
        var q = document.getElementById(QtyControl).value;
        if (pc == "" || q == "") {
            alert('Please input code and qty.');

        }
        else {
            fnClickAddRow(pc, q, '', '');
            document.getElementById(partCodeControl).value = "";
            document.getElementById(QtyControl).value = "";
        }
    }

    function fnClickAddRow(partCode, qty, serialNo, BatchNo) {
        $('#dtSerials').dataTable().fnAddData([
					"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
					"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
					"<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
					"<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />",
					"<input type=checkbox name=chk id=chk  />"
					]);

        giCount++;
    }



    $(document).ready(function() {
        /* Add a click handler to the rows - this could be used as a callback */
        $("#dtSerials tbody").click(function(event) {
            $(oTable.fnSettings().aoData).each(function() {
                $(this.nTr).removeClass('row_selected');
            });
            $(event.target.parentNode).addClass('row_selected');
        });





        /* Add a click handler for the delete row */

        $('#delete').click(function() {
            var anSelected = fnGetSelected(oTable);
            if (anSelected != "") {
                oTable.fnDeleteRow(anSelected[0]);
            }
        });

        /* Init the table */
        oTable = $('#dtSerials').dataTable();
    });



    function fnGetSelected(oTableLocal) {
        var aReturn = new Array();
        var aTrs = oTableLocal.fnGetNodes();

        for (var i = 0; i < aTrs.length; i++) {
            if ($(aTrs[i]).hasClass('row_selected')) {
                aReturn.push(aTrs[i]);
            }
        }
        return aReturn;
    }

    function OnError(result) {
        alert("Error: " + result.get_message());
    }

    function clickMe() {

        var chkOP = document.getElementById('<%= chk.ClientID %>');
        if (chkOP.checked) {
            /*  return ValidateSerials();
            29 Jan 2015, Karam Chand Sharma,#CC01 now bypass serial validation check at the time of punching serial*/
            OnChangeSerials('true'); /*Added #CC01*/
        }
        var isDuplicate = "0";
        var partCode = document.getElementById('<%= hdnSkuCode.ClientID %>').value;
        var InvoiceNumber = document.getElementById('<%= hdnInvoiceNumber.ClientID %>').value;
        var InvoiceDate = document.getElementById('<%= hdnInvoiceDate.ClientID %>').value;

        var qty = '1';
        var batchNo = '';
        var serialno = '';

        var cccc = window.parent.$('#dtParts').dataTable();
        var chkCount = 0;

        if (cccc != null) {
            $("input:checked", oTable.fnGetNodes()).each(function() {
                chkCount = chkCount + 1;

            });
            var v = CheckGridRowCount(cccc, chkCount);
            if (v == false) {
                return;
            }
        }


        $("input:checked", oTable.fnGetNodes()).each(function() {

            var cccc = window.parent.$('#dtParts').dataTable();

            var row = $(this).closest('tr').get(0);
            var aPos = oTable.fnGetPosition(row);
            serialno = $(row).text();
            $("input", cccc.fnGetNodes()).each(function() {
                var rowParent = $(this).closest('tr').get(0);
                var aPosParent = cccc.fnGetPosition(rowParent);
                var ParentRowText = $(rowParent).text();
                var serialnoArray = ParentRowText.split(' ');


                if (serialnoArray[2].replace(/(^\s*)|(\s*$)/g, '') == serialno.replace(/(^\s*)|(\s*$)/g, '')) {
                    isDuplicate = "1";
                    return;
                }

            });

            if (isDuplicate == "1") {
                return;
            }
            else {



                cccc.fnAddData([
        				"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
        				"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
        				"<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />",
        					"<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />",
        					"<span>" + InvoiceNumber + " <span/><input type=hidden name=InvoiceNumber value=" + InvoiceNumber + " />",
        					"<span>" + InvoiceDate + " <span/><input type=hidden name=InvoiceDate value=" + InvoiceDate + " />"
        					]);

            }






        });

        if (isDuplicate == "1") {
            alert("Serial no " + serialno + " added already!");
        }
        else {
            window.parent.SerialLookUps.hide();
        }
    }


    function OnChangeSerials(isSubmit) {
        var serials = document.getElementById('<%= txtSerials.ClientID  %>').value;
        var InvoiceNumber = document.getElementById('<%= hdnInvoiceNumber.ClientID %>').value;
        var InvoiceDate = document.getElementById('<%= hdnInvoiceDate.ClientID %>').value;
        var inputvalue = serials;
        var isDuplicate = "0";

        var arrCurrvalueTemp = "";
        var a = serials.replace(',', '').replace(/(^|\r\n|\n)([^*]|$)/g, "$1,$2");

        a = a.substring(1, a.length);
        arrCurrvalueTemp = a.split(',');

        var arrCurrvalue = arrCurrvalueTemp;

        var Olddt = window.parent.$('#dtParts').dataTable();
        var v = CheckGridRowCount(Olddt, arrCurrvalue.length);
        if (v == false) {
            return;
        }

        for (var i = 0; i < arrCurrvalue.length; i++) {

            var cccc = window.parent.$('#dtParts').dataTable();

            $("input", cccc.fnGetNodes()).each(function() {
                var rowParent = $(this).closest('tr').get(0);
                var aPosParent = oTable.fnGetPosition(rowParent);
                var ParentRowText = $(rowParent).text();
                var serialno = ParentRowText.split(' ');


                if (serialno[2] == arrCurrvalue[i]) {
                    isDuplicate = "1";
                    isSubmit = "false";
                    return;
                }

            });
            if (isDuplicate == "1") {
                alert("Serial no.(" + arrCurrvalue[i] + ") added already.");
                return;
            }


        }

        if (isDuplicate == "0") {
            for (var i = 0; i < arrCurrvalue.length; i++) {


                if (isSubmit == "true") {
                    var partCode = document.getElementById('<%= hdnSkuCode.ClientID %>').value;
                    var qty = '1';
                    var batchNo = '';
                    var serialno = arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '');
                    cccc.fnAddData([
        				"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
        				"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
        				"<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />",
        					"<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />",
        					"<span>" + InvoiceNumber + " <span/><input type=hidden name=InvoiceNumber value=" + InvoiceNumber + " />",
        					"<span>" + InvoiceDate + " <span/><input type=hidden name=InvoiceDate value=" + InvoiceDate + " />"
        					]);
                }
            }
            window.parent.SerialLookUps.hide();

        }



    }




    function GetXmlDocument(skuCode, salesChannelID, salesChannelCode) {
        CommonService.GetSerialsBySku(skuCode, salesChannelID, salesChannelCode, onGetComplete);
    }
    var myarray = new Array();
    function onGetComplete(result) {
        if (result.rows == null) {
            alert('No serials Exist');

            return false;
        }
        var dataTable = result;
        var rowCount = dataTable.rows.length;
        SetRows(result);


    }

    function SetRows(result) {


        var loadRowArray = new Array();
        var sku = document.getElementById('<%= hdnSkuCode.ClientID %>').value;
        //        for (i = 0; i < 159; i++)

        var v = 0;
        var partCode = "ddd";
        var serialNo = "";
        while (v < result.rows.length) {

            serialNo = result.rows[v]["serialNo"];
            fnClickAddRow(sku, "1", result.rows[v]["serialNo"], "");
            v++;

        }




    }

    function ValidateSerials() {

        var Serials = document.getElementById('<%= txtSerials.ClientID %>');
        //var SalesChannelID = document.getElementById('<%= hdnSalesChannelID.ClientID %>');
        var SalesChannelID = document.getElementById('<%= hdnSalesChannelIDReturn.ClientID %>');
        var SalesChannelCode = document.getElementById('<%= hdnSalesChannelCode.ClientID %>');
        var SkuCode = document.getElementById('<%= hdnSkuCode.ClientID %>');
        var TypeID = document.getElementById('<%= hdnTypeID.ClientID %>');
        var InvoiceNo = document.getElementById('<%= hdnInvoiceNumber.ClientID %>');
        var StockBinType = document.getElementById('<%= hdnStockBinType.ClientID %>');
        CommonService.ValidateAllSerialsReturn(parseInt(StockBinType.value), InvoiceNo.value, SalesChannelID.value, SalesChannelCode.value, SkuCode.value, Serials.value, TypeID.value, "",
                    OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {

                        var vv = result.toString();
                        if (vv == "Success") {
                            OnChangeSerials('true');
                        }
                        else {
                            alert(vv);
                        }
                    }, OnError);

        return false;

    }
    function CheckGridRowCount(dt, qty) {

        var oldLength = dt.fnGetData().length;

        oldLength = oldLength + Number(qty);

        if (oldLength > 300) {
            alert('The grid on previous page can not contain more than 300 rows.');
            return false;

        }
        else {
            true;
        }
    }
    
</script>

<div class="contentbox3">
    <div>
        <asp:CheckBox ID="chk" runat="server" Checked="true" Text="Do you want to enter serials nos.?"
            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
    </div>
    <div id="dt_example">
        <div id="container">
            <a href="javascript:void(0);" style="display: none;" id="delete">Click to Delete a new
                row</a>
            <input type="hidden" id="lbl" name="lbl" />
            <asp:Label ID="lblPrtCode" runat="server" Style="display: none;"></asp:Label>
            <table cellpadding="0" cellspacing="0" border="0" class="display" id="dtSerials">
                <thead>
                    <%--  <tr>
                    <th>
                        Select
                    </th>
                    <th>
                        Serial No
                    </th>
                    
                </tr>--%>
                    <tr>
                        <th>
                            Select
                        </th>
                        <th>
                            Serial No
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Literal runat="server" ID="ltData" EnableViewState="true"></asp:Literal>
                </tbody>
            </table>
        </div>
        <span id="spn" name="spn"></span>
        <div id="container">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr id="trSerials" style="display: none;">
                    <td class="formtext" align="left" valign="top" width="5%">
                        Serials
                    </td>
                    <td align="left" valign="top">
                        <%--  <asp:TextBox ID="txtSerials" CssClass="form_textarea2" runat="server" TextMode="MultiLine" Width="250px" onchange="OnChangeSerials('false');"></asp:TextBox>--%>
                        <asp:TextBox ID="txtSerials" CssClass="form_textarea2" runat="server" TextMode="MultiLine"
                            Width="250px"></asp:TextBox>
                        <div class="error">
                            Input comma or press enter between serial nos.<br />
                            2. SerialNo. Min length(<asp:Literal ID="litSerial_MinL" runat="server"></asp:Literal>)
                            and Max Length(<asp:Literal ID="litSerial_MaxL" runat="server"></asp:Literal>)</div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="15px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <a href="javascript:void(0);" onclick="clickMe();" runat="server" class="buttonbg"
                            id="submitme">Submit </a>
                        <asp:HiddenField ID="hdnSalesChannelID" runat="server" />
                        <asp:HiddenField ID="hdnSalesChannelCode" runat="server" />
                        <asp:HiddenField ID="hdnSkuCode" runat="server" />
                        <asp:HiddenField ID="hdnInvoiceNumber" runat="server" />
                        <asp:HiddenField ID="hdnInvoiceDate" runat="server" />
                        <asp:HiddenField ID="hdnTypeID" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnStockBinType" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnSalesChannelIDReturn" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear">
        </div>
    </div>
</div>
