<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PartLookupClientSideOpeningStock.ascx.cs"
    Inherits="Web.Controls.PartLookupClientSideOpeningStock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript" charset="utf-8">
    /* Global var for counter */
    var giCount = 1;
    var oTable;
    var giRedraw = false;

    $(document).ready(function () {
        $('#dtParts').dataTable();
    });
    $(document).ready(function () {
        $('#aspnetForm').submit(function () {
            //$('#btnSave').click(function() {
            var sData = oTable.$('input:hidden').serialize();
            //alert("The following data would have been submitted to the server: \n\n" + sData);
            document.getElementById('lbl').value = sData;
            return true;
        });

        // oTable = $('#dtParts').dataTable();
    });
    /* #CC01 Add Start */

    function EnableSKUTextbox() {
        var txtSku = document.getElementById('<%= txtPartCode.ClientID %>');
        var StockBinType = document.getElementById('<%= ddlStockBinType.ClientID %>');

        if (StockBinType.value == "0") {
            txtSku.disabled = true;
        }
        else {
            txtSku.disabled = false;
        }

        HideUCMsgBox(); /* #CC02 Added */
    }
    /* #CC01 Add End */
    /* #CC02 Add Start */
    function HideUCMsgBox() {
        var vrUCMsg = document.getElementById('ctl00_contentHolderMain_ucMsg_pnlUcMessageBox');
        if (vrUCMsg != null) {
            vrUCMsg.style.display = "none";
        }
    }
    /* #CC02 Add End */
    function fnGetValueFromControls(partCodeControl, QtyControl, batchNo, isNonSerialized) {


        var pc = document.getElementById(partCodeControl).value;
        var q = document.getElementById(QtyControl).value;
        var bt = document.getElementById(batchNo).value
        var SKuName = document.getElementById('<%= hdnSkuName.ClientID %>').value;
        /* #CC01 Add Start */
        var StockBinType = document.getElementById('<%= ddlStockBinType.ClientID %>');
        var stocktype = StockBinType.options[StockBinType.selectedIndex].text.split('-');
        /* stocktype[1]
         alert(StockType.value);
         */
        /* #CC01 Add End */
        if (isNonSerialized == "1") {
            if (pc == "" || q == "") {
                alert('Please input sku code and quantity.');
                bt = "";
                return;

            }
            var a = CheckGridRowCount(1);
            if (a == false) {
                return;
            }
            fnClickAddRow(pc, q, '', bt, '0', SKuName, stocktype[1]); /* #CC01 stocktype[1] Added */
        }
        else {
            if (pc == "" || q == "" || bt == "") {
                alert('Please input sku code, quantity and batch no');
                return;

            }

            var BatchLen_Min = parseInt(document.getElementById('<%= hdnBatchNoLengthMin.ClientID  %>').value);
            var BatchLen_Max = parseInt(document.getElementById('<%= hdnBatchNoLengthMax.ClientID  %>').value);

            if (bt.replace(/(^\s*)|(\s*$)/g, '').length < BatchLen_Min || bt.replace(/(^\s*)|(\s*$)/g, '').length > BatchLen_Max) {
                alert('Invalid batch no length.');
                return;
            }

            var a = CheckGridRowCount(1);
            if (a == false) {
                return;
            }
            fnClickAddbatch(pc, q, '', bt, SKuName, stocktype[1]);
        }


        document.getElementById('addRow').style.display = 'none';
        document.getElementById(partCodeControl).value = "";
        document.getElementById(QtyControl).value = "";
        document.getElementById(batchNo).value = "";
        HideUCMsgBox(); /* #CC02 Added */
    }


    function fnClickAddbatch(partCode, qty, serialNo, BatchNo, skuname, stocktype) { /* #CC01 stocktype Added */



        var isInvalid = "1";

        $("input", oTable.fnGetNodes()).each(function () {
            //  alert($(this).val());
            var row = $(this).closest('tr').get(0);
            var aPos = oTable.fnGetPosition(row);
            var aData = oTable.fnGetData(aPos);
            var ParentRowText = $(row).text();
            var prtCodeArray = ParentRowText.split(' ');

            if (partCode.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[0].replace(/(^\s*)|(\s*$)/g, '') && BatchNo.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[2].replace(/(^\s*)|(\s*$)/g, '')) {
                isInvalid = "0";
                return;
            }



        });

        if (isInvalid == "1") {
            $('#dtParts').dataTable().fnAddData([
					"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
					"<span>" + skuname + " <span/><input type=hidden name=skuname value=" + skuname + " />",
					"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
					"<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
					/*"<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />"]);
             #CC01 Commented */
             /* #CC01 Add Start */
            "<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />",
            "<span>" + stocktype + " <span/><input type=hidden name=stockbintype value=" + stocktype + " />"]);
            /* #CC01 Add End */
            giCount++;
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
        }
        else {
            alert("Sku code with this batch added already!.");
        }

    }

    function TotalQuantity() {
        var varTotalQuantity = 0;
        var nNodes = oTable.fnGetNodes();
        var etotalQuantity = document.getElementById('<%= txtShowTotal.ClientID %>');
        if (nNodes.length > 0) {
            for (var d = 0; d < nNodes.length; d++) {

                var dat = oTable.fnGetData(nNodes[d]);
                var FilteredString = dat[2].replace("/", "");
                var Quantity = FilteredString.split('<span>');
                varTotalQuantity = varTotalQuantity + Math.round(Quantity[1].replace("[,", ""));


            }
            etotalQuantity.value = varTotalQuantity;/* #CC03 value is used instead of innerText as it was not reflecting count in Chrome. */

        }
        else {
            etotalQuantity.value = "0";/* #CC03 value is used instead of innerText as it was not reflecting count in Chrome. */
        }


    }

    function fnClickAddRow(partCode, qty, serialNo, BatchNo, isBulk, skuname, stocktype) { /* #CC01 stocktype Added */
        var isInvalid = "1";
        $("input", oTable.fnGetNodes()).each(function () {
            //  alert($(this).val());
            var row = $(this).closest('tr').get(0);
            var aPos = oTable.fnGetPosition(row);
            var aData = oTable.fnGetData(aPos);
            var ParentRowText = $(row).text();
            var prtCodeArray = ParentRowText.split(' ');



            if (isBulk == '0') {
                if (partCode.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[0].replace(/(^\s*)|(\s*$)/g, '')) {
                    isInvalid = "0";
                    return;
                }
            }

        });


        if (isBulk == '1') {

            $('#dtParts').dataTable().fnAddData([
					"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
					"<span>" + skuname + " <span/><input type=hidden name=skuname value=" + skuname + " />",
					"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
					"<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
					/* "<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />"]); #CC01 Commented */
/* #CC01 Add Start */  "<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />",
                    "<span>" + stocktype + " <span/><input type=hidden name=stockbintype value=" + stocktype + " />"]); /* #CC01 Add End */


            giCount++;
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
        }
        else {
            if (isInvalid == "1") {
                $('#dtParts').dataTable().fnAddData([
					"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
					"<span>" + skuname + " <span/><input type=hidden name=skuname value=" + skuname + " />",
					"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
					"<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
					/* "<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />"]); #CC01 Commented */
/* #CC01 Add Start */  "<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />",
                    "<span>" + stocktype + " <span/><input type=hidden name=stockbintype value=" + stocktype + " />"]); /* #CC01 Add End */

                giCount++;
                var sData = oTable.$('input:hidden').serialize();
                document.getElementById('lbl').value = sData;
            }
            else {
                alert("Sku code added already!.");
            }
        }


        TotalQuantity();

    }

    function txtPartTextChanged() {

        var v = "";
        var partcode = document.getElementById('<%= txtPartCode.ClientID %>').value;

        var SalesChannelID = document.getElementById('<%= salesChannelID.ClientID %>').value;
        var SalesChannelCode = document.getElementById('<%= salesChannelCode.ClientID %>').value;
        var tdSerialNoHead = document.getElementById('tdSerialNoHead');
        var tdSerialNoField = document.getElementById('tdSerialNoField');
        var tdBatchNoHead = document.getElementById('tdBatchNoHead');
        var tdBatchNoField = document.getElementById('tdBatchNoField');



        CommonService.GetStockInHandAndIsSerializedByCode(partcode, "0", "0",
                    OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {
                        var vv = result.toString();
                        if (vv == "1") {
                            tdSerialNoHead.style.display = 'none';
                            tdSerialNoField.style.display = 'none';
                            tdBatchNoHead.style.display = 'none';
                            tdBatchNoField.style.display = 'none';

                            if (partcode != "")
                                alert("Stock does not exist.");

                            document.getElementById('<%= txtPartCode.ClientID %>').value = '';
                            document.getElementById('<%= txtQuantity.ClientID %>').value = '';
                            //  document.getElementById('spnStockinHand').innerHTML = "0";
                            document.getElementById('addRow').style.display = 'none';
                            document.getElementById('addBatch').style.display = 'none';
                            document.getElementById('addSerials').style.display = 'none';
                        }
                        else {


                            var valueSplits = vv.split('-');
                            //  document.getElementById('spnStockinHand').innerHTML = valueSplits[0];
                            document.getElementById('<%= hdnSkuName.ClientID %>').value = valueSplits[2];

                            if (valueSplits[1] == "1") {
                                tdSerialNoHead.style.display = 'none';
                                tdSerialNoField.style.display = 'none';
                                tdBatchNoHead.style.display = 'none';
                                tdBatchNoField.style.display = 'none';

                                document.getElementById('addRow').style.display = '';
                                document.getElementById('addBatch').style.display = 'none';
                                document.getElementById('addSerials').style.display = 'none';
                            }
                            else {
                                document.getElementById('addRow').style.display = 'none';

                                if (valueSplits[1] == "3") {
                                    document.getElementById('addSerials').style.display = '';
                                    document.getElementById('addBatch').style.display = 'none';

                                    tdBatchNoHead.style.display = 'none';
                                    tdBatchNoField.style.display = 'none';

                                    tdSerialNoHead.style.display = '';
                                    tdSerialNoField.style.display = '';

                                }
                                else {
                                    tdSerialNoHead.style.display = 'none';
                                    tdSerialNoField.style.display = 'none';
                                    tdBatchNoHead.style.display = '';
                                    tdBatchNoField.style.display = '';

                                    document.getElementById('addSerials').style.display = 'none';
                                }
                                if (valueSplits[1] == "2") {
                                    document.getElementById('addBatch').style.display = '';
                                    tdBatchNoHead.style.display = '';
                                    tdBatchNoField.style.display = '';
                                }
                                else {
                                    document.getElementById('addBatch').style.display = 'none';
                                    tdBatchNoHead.style.display = 'none';
                                    tdBatchNoField.style.display = 'none';
                                }

                            }


                            //  document.getElementById('<%= txtPartCode.ClientID %>').value = "";
                            document.getElementById('<%= txtSerialNos.ClientID  %>').value = "";
                            document.getElementById('<%= txtQuantity.ClientID  %>').value = "";
                            document.getElementById('<%= txtBatchNo.ClientID  %>').value = "";
                        }




                    }, OnError);
                }

                function validateQty() {
                    txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
                    //        qtyInHand = document.getElementById('spnStockinHand').innerHTML;
                    //        if (Number(txtQty) > Number(qtyInHand))
                    //            alert("Inputed qty can not be greater than Stock in Hand qty.");

                }


                $(document).ready(function () {
                    /* Add a click handler to the rows - this could be used as a callback */
                    $("#dtParts tbody").click(function (event) {
                        $(oTable.fnSettings().aoData).each(function () {
                            $(this.nTr).removeClass('row_selected');
                        });
                        $(event.target.parentNode).addClass('row_selected');
                    });

                    /* Add a click handler for the delete row */

                    $('#delete').click(function () {
                        var anSelected = fnGetSelected(oTable);
                        if (anSelected != "") {
                            oTable.fnDeleteRow(anSelected[0]);
                            TotalQuantity();
                        }
                    });

                    /* Init the table */
                    oTable = $('#dtParts').dataTable();

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
                function fnGetAll(oTableLocal) {
                    var aReturn = new Array();
                    var aTrs = oTableLocal.fnGetNodes();

                    for (var i = 0; i < aTrs.length; i++) {
                        aReturn.push(aTrs[i]);
                    }
                    return aReturn;
                }

                function OnError(result) {
                    alert("Error: " + result.get_message());
                }

                var SerializedObject = "";







                function indexOfRowContainingId(id, matrix) {
                    for (var i = 0, len = matrix.length; i < len; i++) {
                        for (var j = 0, len2 = matrix[i].length; j < len2; j++) {
                            if (matrix[i][j].id === id)
                            { return i; }
                        }
                    } return -1;
                }


                var arr1 = [];
                function OnChangeSerials(isSubmit) {

                    var arr2 = [];

                    var index = 0;

                    CheckInputExpression();
                    var serials = document.getElementById('<%= txtSerialNos.ClientID  %>').value;
                    var partcode = document.getElementById('<%= txtPartCode.ClientID %>').value;

                    var qtys = document.getElementById('<%= txtQuantity.ClientID  %>').value;


                    var SerialLen_Min = parseInt(document.getElementById('<%= hdnSerialNoLengthMin.ClientID  %>').value);
                    var SerialLen_Max = parseInt(document.getElementById('<%= hdnSerialNoLengthMax.ClientID  %>').value);



                    var inputvalue = serials;
                    var isDuplicate = "0";
                    var isInvalid = "0";

                    if (serials == "" || qtys == "" || partcode == "") {
                        alert('Input sku, quantity and serials!');
                        return;
                    }

                    if (inputvalue.replace(/(^\s*)|(\s*$)/g, '') != '') {


                        var a = inputvalue.replace(/(^|\r\n|\n)([^*]|$)/g, "$1,$2").replace(',', '');

                        //    a = a.substring(1, a.length);



                        var arrCurrvalue = a.split(',');


                        if (Number(qtys) != arrCurrvalue.length) {
                            alert('Invalid quantity input!');
                            return;

                        }

                        var IndexSerialData = '';
                        var IndexPartCode = '';
                        var IndexQuantity = '';
                        var IndexBatchNo = '';
                        var IndexSkuName = '';
                        var IndexSkuTypeCode = ''; /* #CC01 Added */
                        var c = 0;

                        var a = CheckGridRowCount(arrCurrvalue.length);
                        if (a == false) {
                            return;
                        }

                        for (var i = 0; i < arrCurrvalue.length; i++) {
                            if (arrCurrvalue[arrCurrvalue.length - 1] == "") {
                                alert('Blank serial no.!');
                                isSubmit = "false";
                                return;
                            }
                            if (arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '').length > SerialLen_Max || arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '').length < SerialLen_Min) {
                                alert('Invalid Serial No("' + arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '') + '") length.!');
                                isInvalid = "1";
                                return;
                            }


                            if (arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '') == '') {
                                alert('Invalid serial no(,) entered for part ' + arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '') + '!');
                                // hdn.value = "1";
                                isInvalid = "1";
                                return;
                            }

                            for (var j = i + 1; j < arrCurrvalue.length; j++) {
                                if (arrCurrvalue[j].replace(/(^\s*)|(\s*$)/g, '') == arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '')) {
                                    alert('Duplicate serial no ' + arrCurrvalue[j] + ' entered for part code!');
                                    return;
                                }
                            }

                            var partCode = document.getElementById('<%= txtPartCode.ClientID %>').value;
                            var qty = '1';
                            var batchNo = '';
                            var serialno = arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '');
                            var SKuName = document.getElementById('<%= hdnSkuName.ClientID %>').value;
                            /* #CC01 Add Start */
                            var StockBinType = document.getElementById('<%= ddlStockBinType.ClientID %>');
                            var stocktype = StockBinType.options[StockBinType.selectedIndex].text.split('-');

                            /* #CC01 Add End */


                            IndexSerialData = "<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />";
                            IndexPartCode = "<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />";
                            IndexQuantity = "<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />";
                            IndexBatchNo = "<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />";
                            IndexSkuName = "<span>" + SKuName + " <span/><input type=hidden name=skuname value=" + SKuName + " />";

                            IndexSkuTypeCode = "<span>" + stocktype[1] + " <span/><input type=hidden name=StockTypeCode value=" + stocktype[1] + " />";/* #CC01 Added */

                            var u = 0;

                            var nNodes = oTable.fnGetNodes();
                            if (nNodes.length > 0) {
                                for (var d = 0; d < nNodes.length; d++) {

                                    var dat = oTable.fnGetData(nNodes[d]);

                                    if (IndexSerialData == dat[3]) {
                                        alert('Serials already exist ' + serialno);
                                        u = 1;
                                        c = 1;
                                        return false;
                                    }
                                }
                            }

                            if (isSubmit == "true") {
                                if (isInvalid == "0" && isDuplicate == "0") {
                                    if (u == 0) {
                                        arr2[index] = new Array(IndexPartCode, IndexSkuName, IndexQuantity, IndexSerialData, IndexBatchNo, IndexSkuTypeCode); /* #CC01 IndexSkuTypeCode Added */
                                        index = index + 1;
                                    }
                                }
                            }
                            //  });
                            if (isDuplicate == "1") {
                                alert("Serial no.(" + arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '') + ") added already.");

                                return;
                            }
                            else if (isInvalid == "1") {
                                alert("Invaid serial no.(" + arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '') + ") input.");
                                return;
                            }
                            else {
                                if (isSubmit == "true") {

                                    //  window.parent.SerialLookUps.hide();
                                }
                            }

                        }
                        if (isSubmit == "true") {
                            document.getElementById('<%= txtPartCode.ClientID %>').value = "";
                            document.getElementById('<%= txtSerialNos.ClientID  %>').value = "";
                            document.getElementById('<%= txtQuantity.ClientID  %>').value = "";
                            document.getElementById('<%= txtBatchNo.ClientID  %>').value = "";
                            document.getElementById('<%= ddlStockBinType.ClientID  %>').value = "0"; /* #CC01 Added */
                            if (arr2 != null) {

                                // "aaData":

                                if (c == 0) {



                                    oTable.fnAddData(arr2);

                                }
                                //  $('#dtParts').fnAddData(arr2);
                                //  var sData = oTable.$('input:hidden').serialize();
                                //  document.getElementById('lbl').value = sData;
                            }
                        }

                    }
                    TotalQuantity(); /* #CC01 Added */
                    HideUCMsgBox(); /* #CC02 Added */
                }

                function CheckInputExpression() {
                    var serials = document.getElementById('<%= txtSerialNos.ClientID  %>').value;

                    var iChars = "!@#$%^&*()+=-[]\\\';./{}|\":<>?~_";
                    var InvalidChars = '';

                    for (var i = 0; i < serials.length; i++) {
                        if (iChars.indexOf(serials.charAt(i)) != -1) {
                            InvalidChars = InvalidChars + serials.charAt(i);

                        }
                    }
                    if (InvalidChars != "") {
                        alert("special characters(" + InvalidChars + ") input in serials. \nThese are not allowed.");
                        return;
                    }
                }

                function CheckGridRowCount(qty) {

                    var oldLength = oTable.fnGetData().length;

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

<div id="dt_example">
    <div>
        <input type="hidden" id="lbl" name="lbl" />
        <input type="hidden" runat="server" id="salesChannelID" name="salesChannelID" />
        <input type="hidden" runat="server" id="salesChannelCode" name="salesChannelCode" />
        <input type="hidden" runat="server" id="hdnSkuName" />
        <div class="H25-C3-S">
            <ul>
                <%--#CC01 Add Start --%>
                <li class="text">Select Stock Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlStockBinType" runat="server" CssClass="formselect" onchange="return EnableSKUTextbox();">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="reqStockBinType" ControlToValidate="ddlStockBinType" Display="Dynamic"
                            CssClass="error" ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please Select Stock Type"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <%--#CC01 Add End --%>
                <li class="text">SKU Code:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtPartCode" runat="server" MaxLength="20" onchange="txtPartTextChanged();"
                        CssClass="formfields"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderModel5" runat="server" CompletionListCssClass="wordWheel listMain .box"
                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                        MinimumPrefixLength="2" ServiceMethod="GetSKUListByCodesList" ServicePath="../CommonService.asmx"
                        TargetControlID="txtPartCode" UseContextKey="true">
                    </cc1:AutoCompleteExtender>
                </li>
                <li class="text">Quantity:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="4" Width="50px" onchange="validateQty()"
                        CssClass="formfields"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="ftextqty" runat="server" TargetControlID="txtQuantity"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </cc1:FilteredTextBoxExtender>
                </li>
                <li class="text" id="tdSerialNoHead" style="display: none;">Serial Nos:
                </li>
                <li  class="field" id="tdSerialNoField" style="display: none;">
                    <asp:TextBox ID="txtSerialNos" runat="server" TextMode="MultiLine" CssClass="form_textarea"></asp:TextBox>
                    <div class="error">
                        1. Input comma or press enter between serial nos.<br />
                        2. SerialNo. Min length(<asp:Literal ID="litSerial_MinL" runat="server"></asp:Literal>)
                        and Max Length(<asp:Literal ID="litSerial_MaxL" runat="server"></asp:Literal>)
                    </div>
                </li>
                <li class="text" id="tdBatchNoHead" style="display: none;">Batch No:
                </li>
                <li  class="field" id="tdBatchNoField" style="display: none;">
                    <asp:TextBox ID="txtBatchNo" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                    <div class="error">
                        BatchNo Min length(<asp:Literal ID="litBatch_MinL" runat="server"></asp:Literal>)
                        and Max Length(<asp:Literal ID="litBatch_MaxL" runat="server"></asp:Literal>)
                    </div>
                </li>
                <li>
                    <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addRow" name="addRow" onclick="fnGetValueFromControls('<%= txtPartCode.ClientID %>','<%= txtQuantity.ClientID %>','<%= txtBatchNo.ClientID %>','1');">Add Code</a>
                    </div>
                    <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addSerials" name="addSerials"
                            onclick="OnChangeSerials('true');">Add Serials</a>
                    </div>
                    <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addBatch" name="addBatch"
                            onclick="fnGetValueFromControls('<%= txtPartCode.ClientID %>','<%= txtQuantity.ClientID %>','<%= txtBatchNo.ClientID %>','0');">Add Batch</a>
                    </div>
                </li>
            </ul>
        </div>
        <div>
            <a href="javascript:void(0);" id="delete">Click to delete selected row</a>
        </div>
          
        <table cellpadding="0" cellspacing="0" border="0" class="display" id="dtParts">
            <thead>
                <tr>
                    <th>SKU Code
                    </th>
                    <th>SKU Name
                    </th>
                    <th>Quantity
                    </th>
                    <th>Serial No
                    </th>
                    <th>Batch No
                    </th>
                    <%--#CC01 Add Start--%>
                    <th>Stock Type
                    </th>
                    <%--#CC01 Add End--%>
                </tr>
            </thead>
            <tbody>
                <asp:Literal ID="lit" runat="server"></asp:Literal>
            </tbody>
        </table>
    </div>
    <div class="spacer">
    </div>
    <asp:HiddenField ID="hdnSerialNoLengthMin" runat="server" />
    <asp:HiddenField ID="hdnSerialNoLengthMax" runat="server" />
    <asp:HiddenField ID="hdnBatchNoLengthMin" runat="server" />
    <asp:HiddenField ID="hdnBatchNoLengthMax" runat="server" />
    <div class="clear">
    </div>
    <div class="formtext">
        <div class="float-right">
            <div class="float-margin">
                <div class="form_text3">
                    Total :
                </div>
            </div>
            <div class="float-margin">
                <asp:TextBox ID="txtShowTotal" runat="server" CssClass="formfields" Width="80" Value="0"
                    Enabled="false" />
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
</div>
