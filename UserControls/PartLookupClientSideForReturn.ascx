<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PartLookupClientSideForReturn.ascx.cs"
    Inherits="Web.Controls.UserControls_PartLookupClientSideForReturn" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript" charset="utf-8">
    /* Global var for counter */
    var giCount = 1;
    var oTable;
    var giRedraw = false;

    $(document).ready(function() {
        $('#dtParts').dataTable();
    });
    $(document).ready(function() {
        $('#aspnetForm').submit(function() {
            // $('#Button1').click(function() {
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
            return true;
        });

        // oTable = $('#dtParts').dataTable();
    });





    function fnGetValueFromControls(partCodeControl, QtyControl, InvoiceNumber, InvoiceDate) {
        debugger;
        var qtyInHand = document.getElementById('spnStockinHand').innerHTML;
        var invoiceDate = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').value;
        var myDate = new Date();
        var sDateSet = new Date(Date.parse(invoiceDate, "dd/MM/yyyy"));
        var sCurrentDAte = Date.parse(myDate, "dd/MM/yyyy")
        if (sDateSet > sCurrentDAte) {
            alert("Invoice Date Can not be more than Current Date");
            return false;
        }

        var invoiceNumber = document.getElementById(InvoiceNumber).value;
        var pc = document.getElementById(partCodeControl).value;
        var q = document.getElementById(QtyControl).value;
        if (invoiceNumber == "" || invoiceDate == "") {
            alert("Please insert Invoice Number/InvoiceDate");
            return;
        }
        if (pc == "" || q == "") {
            alert('Please input code and qty.');

        }
        else {

            if (Number(q) > Number(qtyInHand) && Number(qtyInHand) != "0") {
                alert("Input qty can not be greater than Sold quantity.");
                document.getElementById(QtyControl).value = "";

            }
            else {


                var a = CheckGridRowCount(1);
                if (a == false) {
                    return;
                }
            
                fnClickAddRow(pc, q, '', '', '0', invoiceNumber, invoiceDate); 
                document.getElementById('addRow').style.display = 'none';

                document.getElementById(partCodeControl).value = "";
                document.getElementById(QtyControl).value = "";
            }
        }
    }

    function fnGetValueFromControlsOnPageBatch(partCodeControl, QtyControl, InvoiceNumber, InvoiceDate, BatchNumber) {


        var myDate = new Date();
        var sDateSet = new Date(Date.parse(InvoiceDate, "dd/MM/yyyy"));
        var sCurrentDAte = Date.parse(myDate, "dd/MM/yyyy")
        if (sDateSet > sCurrentDAte) {
            alert("Invoice Date Can not be more than Current Date");
            return false;
        }
        var qtyInHand = document.getElementById('spnStockinHand').innerHTML;
        var invoiceDate = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').value;
        var invoiceNumber = document.getElementById(InvoiceNumber).value;
        var pc = document.getElementById(partCodeControl).value;
        var q = document.getElementById(QtyControl).value;
        var bt = document.getElementById(BatchNumber).value
        if (invoiceNumber == "" || invoiceDate == "") {
            alert("Please insert Invoice Number/InvoiceDate");
            return;
        }
        if (pc == "" || q == "" || bt == "") {
            alert('Please input code and qty.');

        }
        else {

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
            
            fnClickAddbatch(pc, q, '', bt, invoiceNumber, invoiceDate);

            document.getElementById('addRow').style.display = 'none';

            document.getElementById(partCodeControl).value = "";
            document.getElementById(QtyControl).value = "";
            document.getElementById(BatchNumber).value = "";


        }
    }


    function fnClickAddbatch(partCode, qty, serialNo, BatchNo, invoiceNumber, invoiceDate) {



        var isInvalid = "1";

        $("input", oTable.fnGetNodes()).each(function() {
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
					"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + encodeURIComponent(partCode) + " />",
					"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
					"<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
					"<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />",
					"<span>" + invoiceNumber + " <span/><input type=hidden name=batchno value=" + invoiceNumber + " />",
					"<span>" + invoiceDate + " <span/><input type=hidden name=batchno value=" + invoiceDate + " />"
					]);

            giCount++;
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
        }
        else {
            alert("Sku code with this batch added already!.");
        }

    }




    function fnClickAddRowBulk(partCode, qty, serialNo, BatchNo, InvoiceNumber, InvoiceDate) {
        var isInvalid = "1";
        $("input", oTable.fnGetNodes()).each(function() {
            //  alert($(this).val());
            var row = $(this).closest('tr').get(0);
            var aPos = oTable.fnGetPosition(row);
            var aData = oTable.fnGetData(aPos);
            var ParentRowText = $(row).text();
            var prtCodeArray = ParentRowText.split(' ');




            if (prtCodeArray.length == 2) {

                if (partCode.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[0].replace(/(^\s*)|(\s*$)/g, '')) {
                    isInvalid = "0";
                    return;
                }
            }
            else if (prtCodeArray.length == 3) {

                if (partCode.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[0].replace(/(^\s*)|(\s*$)/g, '') && serialNo.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[2].replace(/(^\s*)|(\s*$)/g, '')) {
                    isInvalid = "0";
                    return;
                }
            }
            else if (prtCodeArray.length == 4) {

                if (partCode.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[0].replace(/(^\s*)|(\s*$)/g, '') && serialNo.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[2].replace(/(^\s*)|(\s*$)/g, '')) {
                    isInvalid = "0";
                    return;
                }
            }



        });
        if (isInvalid == "0") {
            return;
        }


        if (isInvalid == "1") {
            $('#dtParts').dataTable().fnAddData([
					"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + encodeURIComponent(partCode) + " />",
					"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
					"<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
					"<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />",
					"<span>" + InvoiceNumber + " <span/><input type=hidden name=InvoiceNumber value=" + InvoiceNumber + " />",
					"<span>" + InvoiceDate + " <span/><input type=hidden name=InvoiceDate value=" + InvoiceDate + " />"

					]);
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
        }




    }





    function fnClickAddRow(partCode, qty, serialNo, BatchNo, isBulk, InvoiceNumber, InvoiceDate) {

        var isInvalid = "1";
        $("input", oTable.fnGetNodes()).each(function() {
            //  alert($(this).val());
            var row = $(this).closest('tr').get(0);
            var aPos = oTable.fnGetPosition(row);
            var aData = oTable.fnGetData(aPos);
            var ParentRowText = $(row).text();
            var prtCodeArray = ParentRowText.split(' ');
          
            if (isBulk == '0') {
                if (partCode.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[0].replace(/(^\s*)|(\s*$)/g, '') && InvoiceNumber.replace(/(^\s*)|(\s*$)/g, '') == prtCodeArray[2].replace(/(^\s*)|(\s*$)/g, '')) {
                    isInvalid = "0";
                    return;
                }
            }

        });


        if (isBulk == '1') {
            $('#dtParts').dataTable().fnAddData([
					"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + encodeURIComponent(partCode) + " />",
					"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
					"<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
					"<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />",
					"<span>" + InvoiceNumber + " <span/><input type=hidden name=InvoiceNumber value=" + InvoiceNumber + " />",
					"<span>" + InvoiceDate + " <span/><input type=hidden name=InvoiceDate value=" + InvoiceDate + " />"
					]);
            giCount++;
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
        }
        else {
            if (isInvalid == "1") {
                $('#dtParts').dataTable().fnAddData([
					"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
					"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
					"<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
					"<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />",
					"<span>" + InvoiceNumber + " <span/><input type=hidden name=InvoiceNumber value=" + InvoiceNumber + " />",
					"<span>" + InvoiceDate + " <span/><input type=hidden name=InvoiceDate value=" + InvoiceDate + " />"
					]);

                giCount++;
                var sData = oTable.$('input:hidden').serialize();
                document.getElementById('lbl').value = sData;
            }
            else {
                alert("Sku code added already with Invoice!.");
            }
        }
    }

    function txtPartTextChanged() {


        var v = "";
        var partcode = document.getElementById('<%= txtPartCode.ClientID %>').value;
        var InvoiceDate = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').value;
        var InvoiceNumber = document.getElementById('<%= txtInvoiceNo.ClientID %>').value;
        var Salestype = document.getElementById('<%= hdnSalesType.ClientID %>').value;
        debugger;
        //   alert(salesChannelCode);
        //  SalesChannelID = "72";
        if (InvoiceDate == "" || InvoiceNumber == "") {
            alert('Please Insert Invoice Number/InvoiceDate');
            return
        }

        CommonService.GetSalesQuantityInformation(partcode, InvoiceNumber, InvoiceDate, Salestype,
                    OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {
                        debugger;
                        var vv = result.toString();
                        if (vv == "1" || vv == "2") {
                            document.getElementById('<%= txtPartCode.ClientID %>').value = '';
                            document.getElementById('<%= txtQuantity.ClientID %>').value = '';
                            document.getElementById('spnStockinHand').innerHTML = "0";
                            document.getElementById('spnAlreadyReturned').innerHTML = "0";

                            document.getElementById('addRow').style.display = 'none';
                            document.getElementById('addBatch').style.display = 'none';
                            document.getElementById('addSerials').style.display = 'none';
                        }

                        else {
                            var valueSplits = vv.split('-');
                            debugger;
                            if (valueSplits[0] == "0") {
                                alert("Invoice Not found");
                                document.getElementById('spnStockinHand').innerHTML = "0";
                                document.getElementById('spnAlreadyReturned').innerHTML = "0";

                                document.getElementById('addRow').style.display = 'none';
                                document.getElementById('addBatch').style.display = 'none';
                                document.getElementById('addSerials').style.display = 'none';
                            }
                            else {
                                var foundOrNot = document.getElementById('<%= hdnFountOrNot.ClientID %>').value;
                                if (foundOrNot == 0) {
                                    document.getElementById('tdInHandField').style.display = 'none';
                                    document.getElementById('tdAlreadyReturnedField').style.display = 'none';

                                }
                                else {
                                    document.getElementById('spnStockinHand').innerHTML = valueSplits[0];
                                    document.getElementById('spnAlreadyReturned').innerHTML = valueSplits[2];

                                }

                                if (valueSplits[1] == "1") {
                                    document.getElementById('addRow').style.display = '';
                                    document.getElementById('addBatch').style.display = 'none';
                                    document.getElementById('addSerials').style.display = 'none';
                                    document.getElementById('tdQuantityHead').style.display = '';
                                    document.getElementById('tdQuantityField').style.display = '';
                                    document.getElementById('tdInHandHead').style.display = '';
                                    document.getElementById('tdAlreadyReturned').style.display = '';
                                    document.getElementById('tdInHandField').style.display = '';
                                    document.getElementById('tdAlreadyReturnedField').style.display = '';
                                    document.getElementById('tdQuantityHeadOnPage').style.display = 'none';
                                    document.getElementById('tdQuantityFieldOnPage').style.display = 'none';
                                    document.getElementById('tdBatchNoHead').style.display = 'none';
                                    document.getElementById('tdBatchNoField').style.display = 'none';
                                    document.getElementById('tdSerialNoHead').style.display = 'none';
                                    document.getElementById('tdSerialNoField').style.display = 'none';
                                    document.getElementById('addSerialsOnPage').style.display = 'none';
                                    document.getElementById('addBatchOnpage').style.display = 'none';


                                }
                                else {
                                    document.getElementById('addRow').style.display = 'none';
                                    if (valueSplits[1] == "3") {
                                        if (foundOrNot == 0) {
                                            document.getElementById('addSerialsOnPage').style.display = '';
                                            document.getElementById('addSerials').style.display = 'none';
                                            document.getElementById('addBatch').style.display = 'none';
                                            document.getElementById('tdQuantityHead').style.display = 'none';
                                            document.getElementById('tdQuantityField').style.display = 'none';
                                            document.getElementById('tdInHandHead').style.display = 'none';
                                            document.getElementById('tdAlreadyReturned').style.display = 'none';
                                            document.getElementById('tdInHandField').style.display = 'none';
                                            document.getElementById('tdAlreadyReturnedField').style.display = 'none';
                                            document.getElementById('tdBatchNoHead').style.display = 'none';
                                            document.getElementById('tdBatchNoField').style.display = 'none';
                                            document.getElementById('tdQuantityHeadOnPage').style.display = '';
                                            document.getElementById('tdQuantityFieldOnPage').style.display = '';
                                            document.getElementById('tdSerialNoHead').style.display = '';
                                            document.getElementById('tdSerialNoField').style.display = '';
                                            document.getElementById('addBatchOnpage').style.display = 'none';


                                        }
                                        else {
                                            document.getElementById('addSerials').style.display = '';
                                            document.getElementById('addBatch').style.display = 'none';
                                            document.getElementById('tdQuantityHead').style.display = 'none';
                                            document.getElementById('tdQuantityField').style.display = 'none';
                                            document.getElementById('tdInHandHead').style.display = 'none';
                                            document.getElementById('tdAlreadyReturned').style.display = 'none';
                                            document.getElementById('tdInHandField').style.display = 'none';
                                            document.getElementById('tdAlreadyReturnedField').style.display = 'none';
                                            document.getElementById('tdQuantityHeadOnPage').style.display = 'none';
                                            document.getElementById('tdQuantityFieldOnPage').style.display = 'none';
                                            document.getElementById('tdBatchNoHead').style.display = 'none';
                                            document.getElementById('tdBatchNoField').style.display = 'none';
                                            document.getElementById('addSerialsOnPage').style.display = 'none';
                                            document.getElementById('tdSerialNoHead').style.display = 'none';
                                            document.getElementById('tdSerialNoField').style.display = 'none';

                                        }
                                    }
                                    else {
                                        document.getElementById('addSerials').style.display = 'none';
                                    }
                                    if (valueSplits[1] == "2") {
                                        if (foundOrNot == 0) {
                                            document.getElementById('tdBatchNoHead').style.display = '';
                                            document.getElementById('tdBatchNoField').style.display = '';
                                            document.getElementById('addBatchOnpage').style.display = '';
                                            document.getElementById('tdQuantityHeadOnPage').style.display = '';
                                            document.getElementById('tdQuantityFieldOnPage').style.display = '';
                                            document.getElementById('addSerialsOnPage').style.display = 'none';
                                            document.getElementById('tdSerialNoHead').style.display = 'none';
                                            document.getElementById('tdSerialNoField').style.display = 'none';
                                            document.getElementById('addBatch').style.display = 'none';
                                            document.getElementById('tdQuantityHead').style.display = 'none';
                                            document.getElementById('tdQuantityField').style.display = 'none';
                                            document.getElementById('tdInHandHead').style.display = 'none';
                                            document.getElementById('tdAlreadyReturned').style.display = 'none';
                                            document.getElementById('tdInHandField').style.display = 'none';
                                            document.getElementById('tdAlreadyReturnedField').style.display = 'none';

                                        }
                                        else {
                                            document.getElementById('addBatch').style.display = '';
                                            document.getElementById('tdQuantityHead').style.display = 'none';
                                            document.getElementById('tdQuantityField').style.display = 'none';
                                            document.getElementById('tdInHandHead').style.display = 'none';
                                            document.getElementById('tdAlreadyReturned').style.display = 'none';
                                            document.getElementById('tdInHandField').style.display = 'none';
                                            document.getElementById('tdAlreadyReturnedField').style.display = 'none';
                                            document.getElementById('tdBatchNoHead').style.display = 'none';
                                            document.getElementById('tdBatchNoField').style.display = 'none';
                                            document.getElementById('addBatchOnpage').style.display = 'none';
                                            document.getElementById('tdQuantityHeadOnPage').style.display = 'none';
                                            document.getElementById('tdQuantityFieldOnPage').style.display = 'none';
                                            document.getElementById('addSerialsOnPage').style.display = 'none';
                                            document.getElementById('tdSerialNoHead').style.display = 'none';
                                            document.getElementById('tdSerialNoField').style.display = 'none';


                                        }
                                    }
                                    else {
                                        document.getElementById('addBatch').style.display = 'none';
                                    }

                                }
                            }


                            validateQty();
                        }




                    }, OnError);


    }

    function validateQty() {

        var foundOrNot = document.getElementById('<%= hdnFountOrNot.ClientID %>').value;
        if (foundOrNot == 1) {
            txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
            qtyInHand = document.getElementById('spnStockinHand').innerHTML;
            if (Number(txtQty) > Number(qtyInHand) && Number(qtyInHand) != "0") {
                alert("Input qty can not be greater than Stock in Hand qty.");
                document.getElementById('<%= txtQuantity.ClientID %>').value = "";

            }
        }


    }

    function validateQtyOnPage() {

        txtQty = document.getElementById('<%= txtQuantityOnPage.ClientID %>').value;
        qtyInHand = document.getElementById('spnStockinHand').innerHTML;
        //         if (Number(txtQty) > Number(qtyInHand) && Number(qtyInHand) != "0") 
        //         {
        //             alert("Input qty can not be greater than Stock in Hand qty.");
        //             document.getElementById('<%= txtQuantityOnPage.ClientID %>').value = "";

        //         }


    }


    $(document).ready(function() {
        /* Add a click handler to the rows - this could be used as a callback */
        $("#dtParts tbody").click(function(event) {
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
    function txtInvoiceTextChanged() {


        var v = "";
        var InvoiceNumber = document.getElementById('<%= txtInvoiceNo.ClientID %>').value;
        var Salestype = document.getElementById('<%= hdnSalesType.ClientID %>').value;
        debugger;
        CommonService.GetInvoiceNumberInfo(InvoiceNumber,Salestype,
                    OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {
                        var vv = result.toString();
                        if (vv == "0") {
                            document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').innerText = "";
                            document.getElementById('<%= hdnFountOrNot.ClientID %>').value = "0";
                            alert("Invoice does not Exist. But you can continue with the Further Information for previous Sales Return");
                        }

                        else {

                            document.getElementById('<%= lblInvoiceDAte.ClientID %>').innerText = vv;
                            document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').innerText = vv;

                        }
                    }, OnError);


    }


    var SerializedObject = "";

    function popupSerials(partCode) {
        
        
        var SalesChannelID = document.getElementById('<%= salesChannelID.ClientID %>').value;
        var SalesChannelCode = document.getElementById('<%= salesChannelCode.ClientID %>').value;
        var hdnInvoiceNumber = document.getElementById('<%= txtInvoiceNo.ClientID %>').value;
        var hdnInvoiceDate = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').value;
        var Salestype = document.getElementById('<%= hdnSalesType.ClientID %>').value;
        /*#CC01 START ADDED*/
        var e = document.getElementById("ctl00_contentHolderMain_ddlStockBinType");
        var StockBinType = e.options[e.selectedIndex].value;
        /*#CC01 Commented var WinCallLogDetails = dhtmlmodal.open("SerialLookUps", "iframe", "../../../Popuppages/PopUpSerialLookUpClientForReturn.aspx?prtcode=" + encodeURIComponent(partCode) + "&SalesChannelID=" + SalesChannelID + "&SalesChannelCode=" + SalesChannelCode + "&InvoiceNumber=" + encodeURIComponent(hdnInvoiceNumber) + "&InvoiceDate=" + hdnInvoiceDate + "&TypeID=" + Salestype, "Select SKU Serials", "width=900px,height=450px,top=25,resize=0,scrolling=auto ,center=1")*/
        var WinCallLogDetails = dhtmlmodal.open("SerialLookUps", "iframe", "../../../Popuppages/PopUpSerialLookUpClientForReturn.aspx?prtcode=" + encodeURIComponent(partCode) + "&SalesChannelID=" + SalesChannelID + "&SalesChannelCode=" + SalesChannelCode + "&InvoiceNumber=" + encodeURIComponent(hdnInvoiceNumber) + "&InvoiceDate=" + hdnInvoiceDate + "&TypeID=" + Salestype + "&StockBinType=" + StockBinType, "Select SKU Serials", "width=900px,height=450px,top=25,resize=0,scrolling=auto ,center=1")
        /*#CC01 START END*/
        WinCallLogDetails.onclose = function() {
            document.getElementById('<%= txtPartCode.ClientID %>').value = '';
            document.getElementById('<%= txtQuantity.ClientID %>').value = '';
            var btn = document.getElementById("ctl00_contentHolderMain_btn");
            document.getElementById('spnStockinHand').innerHTML = "0";
            __doPostBack(btn.name, "OnClick");


            return true;
        }
        return false;
    }
    function popupBatch(partCode) {

        var SalesChannelID = document.getElementById('<%= salesChannelID.ClientID %>').value;
        var SalesChannelCode = document.getElementById('<%= salesChannelCode.ClientID %>').value;
        var hdnInvoiceNumber = document.getElementById('<%= txtInvoiceNo.ClientID %>').value;
        var hdnInvoiceDate = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').value;
        var Salestype = document.getElementById('<%= hdnSalesType.ClientID %>').value;        
        var WinCallLogDetails = dhtmlmodal.open("BatchLookUps", "iframe", "../../../Popuppages/PopUpBatchLookUpClientForReturn.aspx?prtcode=" + encodeURIComponent(partCode) + "&SalesChannelID=" + SalesChannelID + "&SalesChannelCode=" + SalesChannelCode + "&InvoiceNumber=" + encodeURIComponent(hdnInvoiceNumber) + "&InvoiceDate=" + hdnInvoiceDate + "&TypeID=" + Salestype, "Add SKU Batch", "width=900px,height=450px,top=25,resize=0,scrolling=auto ,center=1")
        WinCallLogDetails.onclose = function() {
            var btn = document.getElementById("ctl00_contentHolderMain_btn");
            document.getElementById('<%= txtPartCode.ClientID %>').value = '';
            document.getElementById('<%= txtQuantity.ClientID %>').value = '';
            document.getElementById('spnStockinHand').innerHTML = "0";
            __doPostBack(btn.name, "OnClick");
            return true;
        }
        return false;
    }

    function OpenSerialsPopUp() {
        
        var txtPartCode = document.getElementById('<%= txtPartCode.ClientID %>').value;
        var txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
        var qtyInHand = document.getElementById('spnStockinHand').innerHTML;
        document.getElementById('addSerials').style.display = 'none';

        var invoiceDate = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').value;
        var myDate = new Date();
        var sDateSet = new Date(Date.parse(invoiceDate, "dd/MM/yyyy"));
        var sCurrentDAte = Date.parse(myDate, "dd/MM/yyyy")
        if (sDateSet > sCurrentDAte) {
            alert("Invoice Date Can not be more than Current Date");
            return false;
        }
        popupSerials(txtPartCode);
    }
    function OpenBatchPopUp() {

        var txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
        var qtyInHand = document.getElementById('spnStockinHand').innerHTML;
        document.getElementById('addBatch').style.display = 'none';
        var txtPartCode = document.getElementById('<%= txtPartCode.ClientID %>').value;
        var invoiceDate = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').value;
        var myDate = new Date();
        var sDateSet = new Date(Date.parse(invoiceDate, "dd/MM/yyyy"));
        var sCurrentDAte = Date.parse(myDate, "dd/MM/yyyy")
        if (sDateSet > sCurrentDAte) {
            alert("Invoice Date Can not be more than Current Date");
            return false;
        }
        popupBatch(txtPartCode);
    }
    var arr1 = [];
    function OnChangeSerials(isSubmit) {
      
        var invoiceDate = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').value;
        var myDate = new Date();
        var sDateSet = new Date(Date.parse(invoiceDate, "dd/MM/yyyy"));
        var sCurrentDAte = Date.parse(myDate, "dd/MM/yyyy")
        if (sDateSet > sCurrentDAte) {
            alert("Invoice Date Can not be more than Current Date");
            return false;
        }

        var arr2 = [];

        var index = 0;

        CheckInputExpression();
        var serials = document.getElementById('<%= txtSerialNosOnPage.ClientID  %>').value;
        var partcode = document.getElementById('<%= txtPartCode.ClientID %>').value;

        var qtys = document.getElementById('<%= txtQuantityOnPage.ClientID  %>').value;


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
            var IndexInvoiceNumber = '';
            var IndexInvoiceDate = '';

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
                var InvoiceNumber = document.getElementById('<%= txtInvoiceNo.ClientID %>').value;
                var InvoiceDate = document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_ucInvoiceDate_txtDate').value;


                var qty = '1';
                var batchNo = '';
                var serialno = arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '');

                IndexSerialData = "<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />";
                IndexPartCode = "<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />";
                IndexQuantity = "<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />";
                IndexBatchNo = "<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />";
                IndexInvoiceNumber = "<span>" + InvoiceNumber + " <span/><input type=hidden name=InvoiceNumber value=" + InvoiceNumber + " />";
                IndexInvoiceDate = "<span>" + InvoiceDate + " <span/><input type=hidden name=InvoiceDate value=" + InvoiceDate + " />";

                var u = 0;

                var nNodes = oTable.dataTable().fnGetNodes();
                if (nNodes.length > 0) {
                    for (var d = 0; d < nNodes.length; d++) {

                        var dat = oTable.dataTable().fnGetData(nNodes[d]);

                        if (IndexSerialData == dat[2]) {
                            alert('Serials already exsts ' + serialno);
                            u = 1;
                            c = 1;
                            return false;
                        }
                    }
                }

                if (isSubmit == "true") {
                    if (isInvalid == "0" && isDuplicate == "0") {
                        if (u == 0) {
                            arr2[index] = new Array(IndexPartCode, IndexQuantity, IndexSerialData, IndexBatchNo, IndexInvoiceNumber, IndexInvoiceDate);
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
                document.getElementById('<%= txtSerialNosOnPage.ClientID  %>').value = "";
                document.getElementById('<%= txtQuantity.ClientID  %>').value = "";
                document.getElementById('<%= txtBatchNoOnPage.ClientID  %>').value = "";
                if (arr2 != null) {

                    // "aaData":

                    if (c == 0) {
                        oTable.fnAddData(arr2);
                    }
                    //  $('#dtParts').fnAddData(arr2);
                    var sData = oTable.$('input:hidden').serialize();
                    document.getElementById('lbl').value = sData;
                }
            }

        }
    }

    function CheckInputExpression() {
        var serials = document.getElementById('<%= txtSerialNosOnPage.ClientID  %>').value;

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
            alert('The grid can not contain more than 300 rows.');
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
        <input type="hidden" runat="server" id="hdnInvoiceNumber" name="InvoiceNumber" />
        <input type="hidden" runat="server" id="hdnInvoiceDate" name="InvoiceDate" />
        <input type="hidden" runat="server" id="hdnReturnFromSalesChannelID" name="ReturnFromSalesChannelID" />
        <input type="hidden" runat="server" id="hdnFountOrNot" name="hdnFountOrNot" value="1" />
        <input type="hidden" runat="server" id="hdnSalesType" name="hdnSalesType" value="0" />
        <asp:HiddenField ID="hdnSerialNoLengthMin" runat="server" />
        <asp:HiddenField ID="hdnSerialNoLengthMax" runat="server" />
        <asp:HiddenField ID="hdnBatchNoLengthMin" runat="server" />
        <asp:HiddenField ID="hdnBatchNoLengthMax" runat="server" />
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
        <td>
      <div class="return">
            <ul style="height:35px;">
                <li class="text">
                    Invoice Number:
                </li>
                 <li class="field">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="20" CssClass="form_input9"
                        onchange="txtInvoiceTextChanged();"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                        MinimumPrefixLength="3" ServiceMethod="GetInvoiceNumberList" ServicePath="../CommonService.asmx"
                        TargetControlID="txtInvoiceNo" UseContextKey="true">
                    </cc1:AutoCompleteExtender>
                    <br />
                    <asp:RequiredFieldValidator ID="ReqInvoice" runat="server" ControlToValidate="txtInvoiceNo"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please Insert Invoice Number."
                        SetFocusOnError="true" ValidationGroup="EntryValidation"></asp:RequiredFieldValidator>
                        <%--<asp:RegularExpressionValidator ID="re10" runat="server" ControlToValidate="txtInvoiceNo"
                                        CssClass="error" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Invalid Chars"
                                        ValidationExpression="[0-9a-zA-Z]{1,20}$" ValidationGroup="EntryValidation"></asp:RegularExpressionValidator>
                        --%>
                </li>
                 <li class="text">
                    Invoice Date:<font class="error">*</font>
               </li>
                <li class="field1">
                    <uc2:ucDatePicker ID="ucInvoiceDate" runat="server" ErrorMessage="Invalid date."
                        defaultDateRange="true" ValidationGroup="EntryValidation" RangeErrorMessage="Invalid Date" />
                    <asp:Label ID="lblInvoiceDAte" runat="server" Text="" CssClass="error"></asp:Label>
                </li>
              
            </ul>
            <div class="clear"></div>
            <ul>
             
                <li class="text">
                    SKU Code:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtPartCode" runat="server" MaxLength="20" onchange="txtPartTextChanged();"
                        CssClass="form_input9"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderModel5" runat="server" CompletionListCssClass="wordWheel listMain .box"
                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                        MinimumPrefixLength="2" ServiceMethod="GetSKUListByCodesList" ServicePath="../CommonService.asmx"
                        TargetControlID="txtPartCode" UseContextKey="true">
                    </cc1:AutoCompleteExtender>
                 </li>
                <li class="text" id="tdQuantityHead" style="display: none;">
                    Quantity:
                </li>
                <li class="field1" id="tdQuantityField" style="display: none;">
                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="4" onchange="validateQty()" Width="50px"
                        CssClass="form_input9"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="ftextqty" runat="server" TargetControlID="txtQuantity"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </cc1:FilteredTextBoxExtender>
                </li>
               <li class="text" id="tdQuantityHeadOnPage" style="display: none;">
                    Quantity:
                </li>
                <li  class="field1" id="tdQuantityFieldOnPage" style="display: none;">
                    <asp:TextBox ID="txtQuantityOnPage" runat="server" MaxLength="4" Width="50px" onchange="validateQtyOnPage()"
                        CssClass="form_input9"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtQuantityOnPage"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </cc1:FilteredTextBoxExtender>
                </li>
                <li class="text" id="tdSerialNoHead" style="display: none;">
                    Serial Nos:
               </li>
                <li id="tdSerialNoField" style="display: none;">
                    <asp:TextBox ID="txtSerialNosOnPage" runat="server" TextMode="MultiLine" onchange="OnChangeSerials('false')"
                        CssClass="form_textarea"></asp:TextBox>
                        <div style="width:165px;">
                    <div class="error">1. Input comma or press enter between serial nos.<br/>
                   2. SerialNo. Min length(<asp:Literal ID="litSerial_MinL" runat="server" ></asp:Literal>) and Max Length(<asp:Literal ID="litSerial_MaxL" runat="server" ></asp:Literal>)
                   </div>
                   </div>
                </li>
                <li class="text1" id="tdBatchNoHead" style="display: none;">
                    Batch No:
                </li>
                <li id="tdBatchNoField" style="display: none;">
                    <asp:TextBox ID="txtBatchNoOnPage" runat="server" CssClass="form_input9" MaxLength="20"></asp:TextBox>
                     <div style="width:165px;">
                    <div class="error">BatchNo Min length(<asp:Literal ID="litBatch_MinL" runat="server" ></asp:Literal>) and Max Length(<asp:Literal ID="litBatch_MaxL" runat="server" ></asp:Literal>)
                   </div>
                   </div>
               </li>
               
                <%--Pankaj--%>
            
                <li class="text1">
                    <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addBatch" name="addBatch"
                            onclick="OpenBatchPopUp();">Add Batch</a>
                    </div>
                     <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addSerials" name="addSerials"
                            onclick="OpenSerialsPopUp();">Add Serials</a>
                    </div>
                     <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addBatchOnpage" name="addBatch"
                            onclick="fnGetValueFromControlsOnPageBatch('<%= txtPartCode.ClientID %>','<%= txtQuantityOnPage.ClientID %>','<%= txtInvoiceNo.ClientID %>','<%= ucInvoiceDate.ClientID %>','<%= txtBatchNoOnPage.ClientID %>');">
                            Add Batch</a></div>
                    <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addRow" name="addRow" onclick="fnGetValueFromControls('<%= txtPartCode.ClientID %>','<%= txtQuantity.ClientID %>','<%= txtInvoiceNo.ClientID %>','<%= ucInvoiceDate.ClientID %>');">
                            Add Code</a></div>
                    <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addSerialsOnPage" name="addSerials"
                            onclick="OnChangeSerials('true');">Add Serials</a></div>
                </li>
                  <li class="text2" id="tdInHandHead" style="display: none;">
                    Sold Qty:
                </li>
                 <li class="text2"  id="tdInHandField" style="display: none;">
                  <strong>  <span id="spnStockinHand">0</span></strong>
                 </li>
                 <li class="field1" id="tdAlreadyReturned" style="display: none;">
                    Already Returned:
                 
                            
                            
                            
                            </li>
                            <li class="text2" id="tdAlreadyReturnedField" style="display: none;"><strong><span
                                id="spnAlreadyReturned">0</span></strong> </li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="5px">
                </td>
            </tr>
            <tr>
                <td height="30px">
                    <a href="javascript:void(0);" id="delete">Click to delete selected row</a>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" border="0" class="display" id="dtParts">
            <thead>
                <tr>
                    <th>
                        SKU Code
                    </th>
                    <th>
                        Quantity
                    </th>
                    <th>
                        Serial No
                    </th>
                    <th>
                        Batch No
                    </th>
                    <th>
                        InvoiceNumber
                    </th>
                    <th>
                        InvoiceDate
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Literal ID="lit" runat="server"></asp:Literal>
            </tbody>
        </table>
    </div>
    <div class="spacer">
    </div>
</div>
