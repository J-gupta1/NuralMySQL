<%--
Change Log:
14-Apr-14, Rakesh Goel, #CC01 - Commented validation stopping adjustment for stock with 0 stock as adjust in should be allowed
17-Jun-14, Rakesh Goel, #CC02 - Fixed validation for qty vs stock in hand

--%>

<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="StockAdjustmentInterface.aspx.cs" Inherits="Transactions_Common_StockAdjustmentInterface" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc8" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControls/GridClientSide.ascx" TagName="GridClientSide"
    TagPrefix="uc3" %>
<%--<%@ Register Src="../../UserControls/GridClientSide.ascx" TagName="MultiItemList"
    TagPrefix="uc3" %> #CC01 commented  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        var col1_InputName = "partcode";
        var col2_InputName = "skuname";
        var col3_InputName = "qty";
        var col4_InputName = "serialno";
        var col5_InputName = "batchno";
        var col6_InputName = "stocktype";/*#CC05 ADDED*/

        function VisibleControls(isStock, Type) {


            var QtyPositive = IsQtyPostive();

            var btnAdd = document.getElementById('addRow');


            var btnSerial = document.getElementById('addSerials');
            var btnBatch = document.getElementById('addBatch');

            var btnSerialManual = document.getElementById('addSerialsManual');
            var btnBatchManual = document.getElementById('addBatchManual');


            var tdSerialField = document.getElementById('tdSerialNoField');
            var tdSerialHead = document.getElementById('tdSerialNoHead');

            var tdBatchField = document.getElementById('tdBatchNoField');
            var tdBatchHead = document.getElementById('tdBatchNoHead');

            if (Type == "Serial") {
                tdBatchField.style.display = 'none';
                tdBatchHead.style.display = 'none';
                if (isStock == true) {


                    if (QtyPositive == false) {

                        btnSerial.style.display = '';
                        tdSerialField.style.display = 'none';
                        tdSerialHead.style.display = 'none';
                        btnSerialManual.style.display = 'none';
                    }
                    else {
                        btnSerialManual.style.display = '';
                        tdSerialField.style.display = '';
                        tdSerialHead.style.display = '';
                        btnSerial.style.display = 'none';
                    }
                }
                else {
                    btnSerialManual.style.display = '';
                    tdSerialField.style.display = '';
                    tdSerialHead.style.display = '';
                    btnSerial.style.display = 'none';
                }
                btnBatch.style.display = 'none';
                btnBatchManual.style.display = 'none';
                btnAdd.style.display = 'none';
            }
            else if (Type == "Batch") {

                btnSerial.style.display = 'none';
                btnSerialManual.style.display = 'none';
                tdSerialField.style.display = 'none';
                tdSerialHead.style.display = 'none';
                btnAdd.style.display = 'none';

                if (isStock == true) {

                    if (QtyPositive == false) {
                        btnBatch.style.display = '';
                        tdBatchField.style.display = 'none';
                        tdBatchHead.style.display = 'none';
                        btnBatchManual.style.display = 'none';
                    }
                    else {
                        btnBatch.style.display = 'none';
                        tdBatchHead.style.display = '';
                        tdBatchField.style.display = '';
                        btnBatchManual.style.display = '';
                    }
                }
                else {
                    btnBatch.style.display = 'none';
                    tdBatchHead.style.display = '';
                    tdBatchField.style.display = '';
                    btnBatchManual.style.display = '';
                }
            }

            else if (Type == "NonBatchAndSerial") {

                btnBatch.style.display = 'none';
                tdBatchHead.style.display = 'none';
                tdBatchField.style.display = 'none';
                btnBatchManual.style.display = 'none';

                btnSerial.style.display = 'none';
                btnSerialManual.style.display = 'none';
                tdSerialField.style.display = 'none';
                tdSerialHead.style.display = 'none';
                btnAdd.style.display = '';
            }
            else {
                btnBatch.style.display = 'none';
                tdBatchHead.style.display = 'none';
                tdBatchField.style.display = 'none';
                btnBatchManual.style.display = 'none';
                btnSerial.style.display = 'none';
                btnSerialManual.style.display = 'none';
                tdSerialField.style.display = 'none';
                tdSerialHead.style.display = 'none';
                btnAdd.style.display = '';

                document.getElementById('<%= txtPartCode.ClientID %>').value = '';
                document.getElementById('<%= txtQuantity.ClientID %>').value = '';
                document.getElementById('spnStockinHand').innerHTML = "0";
                document.getElementById('addRow').style.display = 'none';
                document.getElementById('addBatch').style.display = 'none';
                document.getElementById('addSerials').style.display = 'none';
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



function TotalQuantity() {
    debugger;
    var varTotalQuantity = 0;
    var nNodes = oTable.fnGetNodes();
    var etotalQuantity = document.getElementById('<%= txtShowTotal.ClientID %>');
    if (nNodes.length > 0) {
        for (var d = 0; d < nNodes.length; d++) {

            var dat = oTable.fnGetData(nNodes[d]);
            var FilteredString = dat[2].replace("/", "");
            var Quantity = FilteredString.split('<span>');

            varTotalQuantity = varTotalQuantity + Math.abs(Math.round(Quantity[1].replace("[,", "")));


        }
        etotalQuantity.value = varTotalQuantity;  /* #CC07 Added ( value used instead of innerText)  */

    }
    else {
        etotalQuantity.value = "0";   /* #CC07 Added ( value used instead of innerText)  */
    }

}


function ValidateAdjustFor() {
    var e = document.getElementById('<%= hdnAdjustmentForSalesChannelid.ClientID %>');
    //var strUser = e.options[e.selectedIndex].value;
    var strUser = e.value;
    if (strUser == "0") {
        alert('Please select adjust for properly.');
        document.getElementById('<%= txtPartCode.ClientID %>').value = '';
        return false
    }
    else
        return true;
}


function ValidateAdjustForType() {
    var e = document.getElementById('<%= ddlType.ClientID %>');
    var strUser = e.value;
    if (strUser == "0") {
        alert('Please select adjust for Type.');
        document.getElementById('<%= txtAdjustmentFor.ClientID %>').value = '';
        return false
    }
    else
        return true;
}


function txtAdjustmentForSalesChannelCodeTextChanged() {
    var v = ValidateAdjustForType();
    if (v == false) {
        return;
    }
    var strUser = "0";
    var v = "";
    var Id = document.getElementById('<%= hdnAdjustmentForSalesChannelid.ClientID %>');

    var ChannelsTypeid = document.getElementById('<%= ddlType.ClientID %>');
    var SalesChannelTypeid = ChannelsTypeid.options[ChannelsTypeid.selectedIndex].value;

    var AdjustmentForCode = document.getElementById('<%= txtAdjustmentFor.ClientID %>').value;
    if (AdjustmentForCode.indexOf('-') <= 0) {
        alert("Please Enter valid SalesChannel Code.");
        return;
    }
    Typeid = SalesChannelTypeid;
    CommonService.GetSalesChannelInformation(AdjustmentForCode, SalesChannelTypeid,
            OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {

                var vv = result.toString();
                // alert(vv);
                if (vv != '') {
                    var lst = new Array();
                    lst = vv.split('/');
                    //Id.value = vv.split('/')[1];
                    Id.value = lst[lst.length - 1];
                    // alert(Id.value);
                }
                else
                    Id.value = "0";


            }, OnError);


}






function txtPartTextChanged() {
    debugger;
    var v = ValidateAdjustFor();
    if (v == false) {
        return;
    }

    var v = "";
    var e = document.getElementById('<%= hdnAdjustmentForSalesChannelid.ClientID %>');
    var ChannelsTypeid = document.getElementById('<%= ddlType.ClientID %>');
    var SalesChannelTypeid = ChannelsTypeid.options[ChannelsTypeid.selectedIndex].value;
    if (SalesChannelTypeid == "0") {
        alert('Please select SalesChanneltype.');
        return;
    }
    //var strUser = e.options[e.selectedIndex].value;
    var strUser = e.value;
    var partcode = document.getElementById('<%= txtPartCode.ClientID %>').value;
            var StockBinType = document.getElementById('<%= ddlStockBinType.ClientID %>');/*#CC05 ADDED*/

    //var SelectedVal = strUser.split('/');
    //var SalesChannelID = Number(SelectedVal[0]);
    var SalesChannelID = strUser;
    var SalesChannelCode = "";
    var Typeid = "0";
    //            if (SelectedVal[1] != null) {
    //                Typeid = SelectedVal[1].toString();
    //            }
    Typeid = SalesChannelTypeid;

    CommonService.GetStockInHandByLogin(partcode, SalesChannelID, SalesChannelCode, Typeid, StockBinType.options[StockBinType.selectedIndex].value,/*#CC05 ADDED*/
            OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {

                var vv = result.toString();
                document.getElementById('<%= hdnSkuStatus.ClientID %>').value = vv;
                        VisibilityOnQtyChan();

                    }, OnError);
                }


                function VisibilityOnQtyChan() {
                    var txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
                    if (isNaN(txtQty)) {
                        alert('Invalid qty inputed.');
                        document.getElementById('<%= txtQuantity.ClientID %>').value = '';
                        return;
                    }

                    var partcode = document.getElementById('<%= txtPartCode.ClientID %>').value;

                    var vv = document.getElementById('<%= hdnSkuStatus.ClientID %>').value;

                    if (vv == "1") {
                        if (partcode != "") {
                            alert("Invalid SKU Code.");
                            VisibleControls(false, "");
                        }
                    }

                    else {
                        var valueSplits = vv.split('-');

                        var isStock = false;

                        if (valueSplits[0] != "0") {
                            isStock = true;
                        }

                        document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_hdnSkuName').value = valueSplits[2];
                        /*#CC01 comment start
                        if (valueSplits[0] == "0") {
        
                            if (partcode != "")
                                alert("Stock does not exist for this sku.");
        
                        }
                        else {
                            document.getElementById('spnStockinHand').innerHTML = valueSplits[0];
                        }
                        #CC01 comment end*/

                        document.getElementById('spnStockinHand').innerHTML = valueSplits[0];  /*#CC01 added*/

                        if (valueSplits[1] == "1") {
                            VisibleControls(isStock, "NonBatchAndSerial");

                        }
                        else {
                            document.getElementById('addRow').style.display = 'none';

                            if (valueSplits[1] == "3") {
                                VisibleControls(isStock, "Serial");
                            }
                            else {
                                document.getElementById('addSerials').style.display = 'none';
                            }
                            if (valueSplits[1] == "2") {

                                VisibleControls(isStock, "Batch");
                            }
                            else {
                                document.getElementById('addBatch').style.display = 'none';
                            }

                        }

                        validateQty();
                    }
                }

                function validateQty() {
                    txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;

                    qtyInHand = document.getElementById('spnStockinHand').innerHTML;
                    var QtyPositive = IsQtyPostive();
                    //alert(QtyPositive);
                    if (QtyPositive == false) {
                        if (Math.abs(Number(txtQty)) > Number(qtyInHand) && Number(qtyInHand) != "0")   //#CC02 added Math.abs
                        {
                            alert("Input qty can not be greater than Stock in Hand qty.");
                            document.getElementById('<%= txtQuantity.ClientID %>').value = "";

                        }
                    }


                }

                function fnAddNonBatchAndSerialize(partCodeControl, QtyControl) {
                    var v = ValidateAdjustFor();
                    if (v == false) {
                        return;
                    }

                    var isQuantityPositive = IsQtyPostive();




                    var qtyInHand = document.getElementById('spnStockinHand').innerHTML;
                    var pc = document.getElementById(partCodeControl).value;
                    var q = document.getElementById(QtyControl).value;
                    if (pc == "" || q == "") {
                        alert('Please input code and qty.');

                    }
                    else {

                        //#CC02 added check for 0 qty
                        if (Number(q) == 0) {
                            alert("Input qty can not be zero.");
                            return;
                        }

                        if (isQuantityPositive == false && Number(qtyInHand) == 0) {
                            alert("Input qty can not be minus as stock not available for the sku.");
                            return;
                        }


                        if (Math.abs(Number(q)) > Number(qtyInHand) && Number(qtyInHand) != "0" && isQuantityPositive == false)   //#CC02 added Math.abs and isQuantityPositive==false check 
                        {
                            alert("Input qty can not be greater than Stock in Hand qty.");
                            document.getElementById(QtyControl).value = "";

                        }
                        else {

                            document.getElementById('addRow').style.display = 'none';

                            fnClickAddRow(pc, q, '', '', '0');

                            document.getElementById(partCodeControl).value = "";
                            document.getElementById(QtyControl).value = "";
                        }
                    }
                    TotalQuantity();
                }



                function IsQtyPostive() {
                    TotalQuantity();/* #CC07 Added */
                    var txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
                    //alert(txtQty);
                    if (Number(txtQty) < 0) {
                        return false;
                    }
                    else
                        return true;
                }

                function fnClickAddRow(partCode, qty, serialNo, BatchNo, isBulk) {
                    var isInvalid = "1";

                    var skuName = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_hdnSkuName').value;
                    var StockBinType = document.getElementById('<%= ddlStockBinType.ClientID %>');/*#CC05 ADDED*/
                    var stocktype = StockBinType.options[StockBinType.selectedIndex].text.split('-');/*#CC05 ADDED*/

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

                    if (isInvalid == "1") {
                        $('#dtParts').dataTable().fnAddData([
                            getRenderingRow(partCode, col1_InputName),
                            getRenderingRow(skuName, col2_InputName),
                            getRenderingRow(qty, col3_InputName),
                            getRenderingRow(serialNo, col4_InputName),
                            getRenderingRow(BatchNo, col5_InputName), getRenderingRow(stocktype, col6_InputName/*#CC05 ADDED*/)]);
                        document.getElementById('spnStockinHand').innerHTML = "0";
                        giCount++;
                        VisibleControls(false, "");
                        var sData = oTable.$('input:hidden').serialize();
                        document.getElementById('lbl').value = sData;
                    }
                    else {
                        alert("Sku code added already!.");
                    }

                }

                function OnError(result) {
                    alert("Error: " + result.get_message());
                }



                function popupSerials(partCode) {
                    var v = ValidateAdjustFor();
                    if (v == false) {
                        return;
                    }

                    var SalesChannelCode = "";

                    var e = document.getElementById('<%= hdnAdjustmentForSalesChannelid.ClientID %>');
                    var ChannelsTypeid = document.getElementById('<%= ddlType.ClientID %>');
                    var SalesChannelTypeid = ChannelsTypeid.options[ChannelsTypeid.selectedIndex].value;
                    if (SalesChannelTypeid == "0") {
                        alert('Please select SalesChanneltype.');
                        return;
                    }
                    var strUser = e.value;
                    //var strUser = e.options[e.selectedIndex].value;
                    //var SelectedVal = strUser.split('/');
                    var SalesChannelID = strUser;
                    var SalesChannelCode = "";
                    var Typeid = "0";
                    //if (SelectedVal[1] != null) {
                    //  Typeid = SelectedVal[1].toString();
                    //}

                    /* #CC06 Add Start */
                    var StockBinType = document.getElementById('<%= ddlStockBinType.ClientID %>');
                    var stocktype = StockBinType.options[StockBinType.selectedIndex].text.split('-');
                    /* #CC06 Add End */

                    Typeid = SalesChannelTypeid;
                    WinCallLogDetails = dhtmlmodal.open("SerialLookUps", "iframe", "../../Popuppages/PopUpSerialLookUpClient.aspx?prtcode=" + partCode + "&SalesChannelID=" + SalesChannelID + "&SalesChannelCode=" + SalesChannelCode + "&TypeID=" + Typeid + "&Mode=2" + "&StockBinTypeID=" + StockBinType.value + "&StockBinTypeCode=" + stocktype[1], "Select SKU Serials", "width=900px,height=450px,top=25,resize=0,scrolling=auto ,center=1") /* #CC06 StockBinType added and supplied*/
                    WinCallLogDetails.onclose = function () {

                        VisibleControls(false, "");
                        var btn = document.getElementById("ctl00_contentHolderMain_btn");

                        __doPostBack(btn.name, "OnClick");


                        return true;
                    }
                    return false;
                }

                function popupBatch(partCode) {
                    var v = ValidateAdjustFor();
                    if (v == false) {
                        return;
                    }

                    var SalesChannelCode = "";

                    var e = document.getElementById('<%= hdnAdjustmentForSalesChannelid.ClientID %>');
                    var ChannelsTypeid = document.getElementById('<%= ddlType.ClientID %>');
                    var SalesChannelTypeid = ChannelsTypeid.options[ChannelsTypeid.selectedIndex].value;
                    var strUser = e.value;
                    if (strUser == "0") {
                        alert('Please select SalesChanneltype.');
                        return;
                    }
                    var AdjustmentForCode = document.getElementById('<%= txtAdjustmentFor.ClientID %>').value;
            var SalesChannelID = strUser;
            var Typeid = "0";
            Typeid = SalesChannelTypeid;




            partCode = encodeURIComponent(partCode);

            WinCallLogDetails = dhtmlmodal.open("BatchLookUps", "iframe", "../../Popuppages/PopUpBatchLookUpClient.aspx?prtcode=" + partCode + "&SalesChannelID=" + SalesChannelID + "&SalesChannelCode=" + SalesChannelCode + "&TypeID=" + Typeid + "&Mode=2", "Add SKU Batch", "width=900px,height=450px,top=25,resize=0,scrolling=auto ,center=1")


            WinCallLogDetails.onclose = function () {
                var btn = document.getElementById("ctl00_contentHolderMain_btn");
                VisibleControls(false, "");
                __doPostBack(btn.name, "OnClick");
                return true;
            }
            return false;
        }

        function OpenSerialsPopUp() {
            var v = ValidateAdjustFor();
            if (v == false) {
                return;
            }

            var txtPartCode = document.getElementById('<%= txtPartCode.ClientID %>').value;
            txtPartCode = encodeURIComponent(txtPartCode);
            var txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
            var qtyInHand = document.getElementById('spnStockinHand').innerHTML;
            document.getElementById('addSerials').style.display = 'none';
            popupSerials(txtPartCode);
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
        function OpenBatchPopUp() {
            var v = ValidateAdjustFor();
            if (v == false) {
                return;
            }
            var txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
            var qtyInHand = document.getElementById('spnStockinHand').innerHTML;
            document.getElementById('addBatch').style.display = 'none';
            var txtPartCode = document.getElementById('<%= txtPartCode.ClientID %>').value;
            popupBatch(txtPartCode);
        }

        var arr1 = [];
        function OnChangeSerials(isSubmit) {
            var v = ValidateAdjustFor();
            if (v == false) {
                return;
            }

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
                var stocktype = '';/*#CC05 ADDED*/
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
                    var SKuName = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_hdnSkuName').value;
                    var SKuName = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_hdnSkuName').value;



                    var StockBinType = document.getElementById('<%= ddlStockBinType.ClientID %>');/*#CC05 ADDED*/
                    var stocktype = StockBinType.options[StockBinType.selectedIndex].text.split('-');/*#CC05 ADDED*/

                    if (StockBinType.options[StockBinType.selectedIndex].value == 0) {
                        alert("Please select stock type.");
                        StockBinType.focus();
                        return false;
                    }

                    IndexSerialData = getRenderingRow(serialno, col4_InputName);
                    IndexPartCode = getRenderingRow(partCode, col1_InputName);
                    IndexQuantity = getRenderingRow(qty, col3_InputName);
                    IndexBatchNo = getRenderingRow(batchNo, col5_InputName);
                    IndexSkuName = getRenderingRow(SKuName, col2_InputName);
                    IndexStockType = getRenderingRow(stocktype[1], col2_InputName/*#CC05 ADDED*/);

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
                                arr2[index] = new Array(IndexPartCode, IndexSkuName, IndexQuantity, IndexSerialData, IndexBatchNo, IndexStockType/*#CC05 ADDED*/);
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
                    if (arr2 != null) {

                        if (c == 0) {
                            oTable.fnAddData(arr2);
                            VisibleControls(false, "");
                        }
                    }
                }

            }
            TotalQuantity();
        }


        function fnGetValueFromControls(partCodeControl, QtyControl, batchNo, isNonSerialized) {

            var v = ValidateAdjustFor();
            if (v == false) {
                return;
            }

            var pc = document.getElementById(partCodeControl).value;
            var q = document.getElementById(QtyControl).value;
            var bt = document.getElementById(batchNo).value
            var SKuName = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_hdnSkuName').value;

            var isQuantityPositive = IsQtyPostive();
            var qtyInHand = document.getElementById('spnStockinHand').innerHTML;

            if (isNonSerialized == "1") {
                if (pc == "" || q == "") {
                    alert('Please input sku code and quantity.');
                    bt = "";
                    return;

                }
                fnClickAddRow(pc, q, '', bt, '0', SKuName);
            }
            else {
                if (pc == "" || q == "" || bt == "") {
                    alert('Please input sku code, quantity and batch no');
                    return;

                }


                if (isQuantityPositive == false && Number(qtyInHand) == 0) {
                    alert("Input qty can not be minus as stock not available for the sku.");
                    return;
                }

                var BatchLen_Min = parseInt(document.getElementById('<%= hdnBatchNoLengthMin.ClientID  %>').value);
                var BatchLen_Max = parseInt(document.getElementById('<%= hdnBatchNoLengthMax.ClientID  %>').value);

                if (bt.replace(/(^\s*)|(\s*$)/g, '').length < BatchLen_Min || bt.replace(/(^\s*)|(\s*$)/g, '').length > BatchLen_Max) {
                    alert('Invalid batch no length.');
                    return;
                }

                fnClickAddbatch(pc, q, '', bt, SKuName);
            }


            document.getElementById('addRow').style.display = 'none';

            document.getElementById(partCodeControl).value = "";
            document.getElementById(QtyControl).value = "";
            document.getElementById(batchNo).value = "";
        }
        function fnClickAddbatch(partCode, qty, serialNo, BatchNo, skuname) {

            var v = ValidateAdjustFor();
            if (v == false) {
                return;
            }

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
                $(oTable.dataTable().fnAddData([
					getRenderingRow(partCode, col1_InputName),
					getRenderingRow(skuname, col2_InputName),
					getRenderingRow(qty, col3_InputName),
					getRenderingRow(serialNo, col4_InputName),
					getRenderingRow(BatchNo, col5_InputName),
                    getRenderingRow(stocktype, col6_InputName) /*#CC05 ADDED*/]));

                giCount++;
                VisibleControls(false, "");
                var sData = oTable.$('input:hidden').serialize();
                document.getElementById('lbl').value = sData;
            }
            else {
                alert("Sku code with this batch added already!.");
            }
            TotalQuantity();
        }

        function getRenderingRow(ItsVal, ItsName) {
            return "<span>" + ItsVal + " <span/><input type=hidden name=" + ItsName + " value=" + ItsVal + " />"
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <input type="hidden" runat="server" id="salesChannelID" name="salesChannelID" />
    <input type="hidden" runat="server" id="salesChannelCode" name="salesChannelCode" />
    <input type="hidden" id="ctl00_contentHolderMain_PartLookupClientSide1_hdnSkuName" />
    <asp:TextBox ID="hdnAdjustmentForSalesChannelid" runat="server" Value="0" Style="display: none" />
    <asp:HiddenField ID="hdnSerialNoLengthMin" runat="server" />
    <asp:HiddenField ID="hdnSerialNoLengthMax" runat="server" />
    <asp:HiddenField ID="hdnBatchNoLengthMin" runat="server" />
    <asp:HiddenField ID="hdnBatchNoLengthMax" runat="server" />
    <asp:HiddenField ID="hdnSkuStatus" runat="server" />
    <div>
        <div>

            <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uc8:ucMessage ID="ucMessage1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="contentbox margin-top">
            <div class="mandatory">
                (*) Marked fields are mandatory            
            </div>
            <div class="H30-C3-S">

                <ul>
                    <li class="text">Select Mode: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:RadioButtonList ID="rdModelList" runat="server" CssClass="radio-rs" TextAlign="Right" RepeatDirection="Horizontal"
                            CellPadding="2" CellSpacing="0" BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                            <asp:ListItem Text="Excel" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Interface" Value="1" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </li>
                </ul>
                <div class="clear"></div>
                <ul>
                    <li class="text">Stock Adjustment Date:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <uc1:ucDatePicker ID="ucDatePicker1" runat="server" IsRequired="True" ValidationGroup="grpupld"
                            ErrorMessage="Please enter stock adjustment date" />
                    </li>
                    <li class="text">Sales Channel Type: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <div>
                            <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                                CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                SetFocusOnError="true" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <%-- <td width="15%" align="right" valign="top">
                                    Adjustment for:<font class="error">*</font>
                                </td>
                                <td width="27%" align="left" class="formtext" valign="top">
                                    <div style="width: 170px;">
                                        <asp:DropDownList ID="ddlAdjustmentFor" runat="server" CssClass="form_select" ValidationGroup="Save"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlAdjustmentFor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="width: 180px;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select."
                                            Display="Dynamic" CssClass="error" InitialValue="0" ValidationGroup="Save" ControlToValidate="ddlAdjustmentFor"></asp:RequiredFieldValidator>
                                    </div>
                                </td>--%>
                    <li class="text">Adjustment for: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtAdjustmentFor" runat="server" MaxLength="30" onchange="javascript:txtAdjustmentForSalesChannelCodeTextChanged();"
                            CssClass="formfields"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                            CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                            MinimumPrefixLength="3" ServiceMethod="GetSalesChannelCodeList" ServicePath="../../CommonService.asmx"
                            TargetControlID="txtAdjustmentFor" UseContextKey="true">
                        </cc1:AutoCompleteExtender>
                    </li>
                </ul>
                <ul>
                    <li class="text">Reason: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="cmbReason" runat="server" ValidationGroup="Save" CssClass="formselect">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="Req2" runat="server" ErrorMessage="Please select reason"
                                Display="Dynamic" CssClass="error" InitialValue="0" ValidationGroup="Save" ControlToValidate="cmbReason"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">Remarks: <span class="error">*</span>
                    </li>
                    <li class="field" style="height: auto">
                        <uc2:ucTextboxMultiline ID="txtRemarks" runat="server" IsRequired="true" CharsLength="100"
                            TextBoxWatermarkText="Please input remarks" ErrorMessage="Please enter remarks."
                            ValidationGroup="Save" />
                    </li>
                </ul>
            </div>
        </div>

        <div class="contentbox">
            <div class="H25-C3-S">
                <ul>
                    <%--#CC05 START ADDED --%>
                    <li class="text">Select Stock Type: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlStockBinType" runat="server" CssClass="formselect" onchange="javascript:txtPartTextChanged();">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="reqStockBinType" ControlToValidate="ddlStockBinType"
                                CssClass="error" ValidationGroup="Save" InitialValue="0" runat="server" ErrorMessage="Please Select Stock Type"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <%--#CC05 END ADDED --%>
                    <li class="text">SKU Code: <span class="error">&nbsp;</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtPartCode" runat="server" MaxLength="20" onchange="javascript:txtPartTextChanged();"
                            CssClass="formfields"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderModel5" runat="server" CompletionListCssClass="wordWheel listMain .box"
                            CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                            MinimumPrefixLength="2" ServiceMethod="GetSKUListByCodesList" ServicePath="../../CommonService.asmx"
                            TargetControlID="txtPartCode" UseContextKey="true">
                        </cc1:AutoCompleteExtender>
                    </li>
                    <li class="text" id="tdQuantityHead">Quantity: <span class="error">&nbsp;</span>
                    </li>
                    <li class="field" id="tdQuantityField">
                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="4" CssClass="formfields"
                            Width="60px" onchange="VisibilityOnQtyChan();"></asp:TextBox>

                        <cc1:FilteredTextBoxExtender ID="ftextqty" runat="server" TargetControlID="txtQuantity"
                            FilterMode="ValidChars" ValidChars="0123456789-">
                        </cc1:FilteredTextBoxExtender>
                    </li>
                </ul>
                <ul>
                    <li class="text" id="tdSerialNoHead" style="display: none;">Serial Nos: <span class="error">&nbsp;</span>
                    </li>
                    <li class="field" id="tdSerialNoField" style="display: none;">
                        <asp:TextBox ID="txtSerialNos" runat="server" TextMode="MultiLine" CssClass="form_textarea"></asp:TextBox>
                        <div class="error">
                            1. Input comma or press enter between serial nos.<br />
                            2. SerialNo. Min length(<asp:Literal ID="litSerial_MinL" runat="server"></asp:Literal>)
                                        and Max Length(<asp:Literal ID="litSerial_MaxL" runat="server"></asp:Literal>)
                        </div>
                    </li>
                    <li class="text" id="tdBatchNoHead" style="display: none;">Batch No: <span class="error">&nbsp;</span>
                    </li>
                    <li class="field" id="tdBatchNoField" style="display: none;">
                        <asp:TextBox ID="txtBatchNo" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                        <div class="error">
                            BatchNo Min length(<asp:Literal ID="litBatch_MinL" runat="server"></asp:Literal>)
                                        and Max Length(<asp:Literal ID="litBatch_MaxL" runat="server"></asp:Literal>)
                        </div>
                    </li>
                    <li class="text" id="tdInHandHead">Stock in Hand:<span class="error">&nbsp;</span>
                    </li>
                    <li class="field" id="tdInHandField">
                        <span id="spnStockinHand" class="frmtxt1">0</span>
                    </li>
                    <li class="link">
                        <div class="float-left">
                            <span class="elink2">
                                <a href="javascript:void(0);" style="display: none;" id="addRow" name="addRow" onclick="fnAddNonBatchAndSerialize('<%= txtPartCode.ClientID %>','<%= txtQuantity.ClientID %>');">Add Code</a>
                            </span>
                        </div>
                        <div class="float-left">
                            <span class="elink2">
                                <a href="javascript:void(0);" style="display: none;" id="addSerials" name="addSerials"
                                    onclick="OpenSerialsPopUp();">Add Serials</a> <a href="javascript:void(0);" style="display: none;"
                                        id="addSerialsManual" name="addSerialsManual" onclick="OnChangeSerials('true');">Add Serials</a>
                            </span>
                        </div>
                        <div class="float-left">
                            <span class="elink2">
                                <a href="javascript:void(0);" style="display: none;" id="addBatch" name="addBatch"
                                    onclick="OpenBatchPopUp();">Add Batch</a> <a href="javascript:void(0);" style="display: none;"
                                        id="addBatchManual" name="addBatchManual" onclick="fnGetValueFromControls('<%= txtPartCode.ClientID %>','<%= txtQuantity.ClientID %>','<%= txtBatchNo.ClientID %>','2');">Add Batch</a>
                            </span>
                        </div>
                    </li>
                </ul>
            </div>
            <div class="clear"></div>
            <div>
                <%--#CC05 START COMMENTED   
                                     <uc3:GridClientSide ID="GridClientSide2" runat="server" GridColumnNames="SKUCode,SKU Name,Quantity,Serial No,Batch No"
                                        DataTableColumnNames="skucode,skuname,qty,serialno,batchno" DataTableColumnTypes="string,string,int32,string,string"
                                        ControlTypes="label,label,label,label,label"></uc3:GridClientSide> #CC05 END COMMENTED --%>
                <%--#CC05 START ADDED --%>
                <uc3:GridClientSide ID="GridClientSide1" runat="server" GridColumnNames="SKUCode,SKU Name,Quantity,Serial No,Batch No,Stock Type"
                    DataTableColumnNames="skucode,skuname,qty,serialno,batchno,stocktype" DataTableColumnTypes="string,string,int32,string,string,string"
                    ControlTypes="label,label,label,label,label,label"></uc3:GridClientSide>
                <%--#CC05 END ADDED--%>
                <div>
                    <div class="float-right">
                        <div class="float-margin">
                            <div class="form_text3">
                                Total :
                            </div>
                        </div>
                        <div class="float-margin">
                            <asp:TextBox ID="txtShowTotal" runat="server" Width="80" Value="0" CssClass="formfields"
                                Enabled="false" />
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
        </div>
        <div class="margin-bottom">
            <div class="float-margin">
                <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="Save"
                    CssClass="buttonbg" OnClick="btnSave_Click" />
            </div>
            <div class="float-margin">
                <asp:Button ID="btnReset" runat="server" CssClass="buttonbg" Text="Reset" OnClick="btnReset_Click" />
            </div>
            <div class="clear"></div>
        </div>

    </div>
</asp:Content>
