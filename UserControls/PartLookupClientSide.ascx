﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PartLookupClientSide.ascx.cs"
    Inherits="Web.Controls.PartLookupClientSide" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!--
    Commented by Adnan 
    document.getElementById('addRow').style.display
    -->
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
            // $('#Button1').click(function() {
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
            return true;
        });

        // oTable = $('#dtParts').dataTable();
    });


    function fnGetValueFromControls(partCodeControl, QtyControl, partNameControl, rateContorl, AmountControl) {



        var qtyInHand = document.getElementById('spnStockinHand').innerHTML;
        var pc = document.getElementById(partCodeControl).value;
        var q = document.getElementById(QtyControl).value;
        var rate = document.getElementById(rateContorl).value;
        var amount = document.getElementById(AmountControl).value;
        if (pc == "" || q == "") {
            alert('Please input code and qty.');

        }
        else {

            if (Number(q) > Number(qtyInHand) && Number(qtyInHand) != "0") {
                alert("Input qty can not be greater than Stock in Hand qty.");
                document.getElementById(QtyControl).value = "";

            }
            else {
                if (Number(rate) != "0") {
                    amount = Number(rate) * Number(q);
                }
                else if (Number(amount) != "0") {
                    rate = Number(amount) / Number(q);
                }
                //document.getElementById('addRow').style.display = 'none';

                fnClickAddRow(pc, q, '', '', '0', rate, amount);

                document.getElementById(partCodeControl).value = "";
                document.getElementById(QtyControl).value = "";
                document.getElementById(partNameControl).value = "";
                document.getElementById(rateContorl).value = "";
                document.getElementById(AmountControl).value = "";
            }
        }
    }



    function fnClickAddRowBulk(partCode, qty, serialNo, BatchNo) {
        var isInvalid = "1";
        $("input", oTable.fnGetNodes()).each(function () {
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
                "<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
                "<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
                "<span>" + Rate + " <span/><input type=hidden name=rate value=" + Rate + " />",
                "<span>" + Amount + " <span/><input type=hidden name=amount value=" + Amount + " />",
                "<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
                "<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />"

            ]);
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
        }




    }





    function fnClickAddRow(partCode, qty, serialNo, BatchNo, isBulk, Rate, Amount) {
        var isInvalid = "1";
        var skuName = document.getElementById('<%= hdnSkuName.ClientID %>').value;

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
                "<span>" + skuName + " <span/><input type=hidden name=skuname value=" + skuName + " />",
                "<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
                "<span>" + Rate + " <span/><input type=hidden name=rate value=" + Rate + " />",
                "<span>" + Amount + " <span/><input type=hidden name=amount value=" + Amount + " />",
                "<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
                "<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />"

            ]);

            giCount++;
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
        }
        else {
            if (isInvalid == "1") {
                $('#dtParts').dataTable().fnAddData([
                    "<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
                    "<span>" + skuName + " <span/><input type=hidden name=skuname value=" + skuName + " />",
                    "<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
                    "<span>" + Rate + " <span/><input type=hidden name=rate value=" + Rate + " />",
                    "<span>" + Amount + " <span/><input type=hidden name=amount value=" + Amount + " />",
                    "<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />",
                    "<span>" + BatchNo + " <span/><input type=hidden name=batchno value=" + BatchNo + " />"
                ]);

                giCount++;
                var sData = oTable.$('input:hidden').serialize();
                document.getElementById('lbl').value = sData;
            }
            else {
                alert("Sku code added already!.");
            }
        }
    }

    function txtPartTextChanged() {

        var v = "";
        var partcode = document.getElementById('<%= txtPartCode.ClientID %>').value;
        var SalesChannelID = document.getElementById('<%= salesChannelID.ClientID %>').value;
        var CompanyId = document.getElementById('<%= hidenCompanyId.ClientID %>').value;
        var SalesChannelCode = document.getElementById('<%= salesChannelCode.ClientID %>').value;
        if (SalesChannelID == null || SalesChannelID == "0") {
            alert("Please Select From Sales Channel.");
            return false;
        }

        CommonService.GetStockInHandAndIsSerializedByCode(partcode, SalesChannelID, SalesChannelCode, CompanyId,
            OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {

                var vv = result.toString();
                if (vv == "1") {
                    if (partcode != "")
                        alert("Stock does not exist.");

                    document.getElementById('<%= txtPartCode.ClientID %>').value = '';
                            document.getElementById('<%= txtPartName.ClientID %>').value = '';
                            document.getElementById('<%= txtQuantity.ClientID %>').value = '';
                            document.getElementById('<%= txtRate.ClientID %>').value = '';
                            document.getElementById('<%= txtAmount.ClientID %>').value = '';
                            document.getElementById('spnStockinHand').innerHTML = "0";
                            //document.getElementById('addRow').style.display = 'none';
                            document.getElementById('addBatch').style.display = 'none';
                            document.getElementById('addSerials').style.display = 'none';
                        }

                        else {
                            var valueSplits = vv.split('-');
                            document.getElementById('<%= hdnSkuName.ClientID %>').value = valueSplits[2];
                            document.getElementById('<%= txtPartName.ClientID %>').value = valueSplits[2];
                    if (valueSplits[0] == "0" && valueSplits[3] == "1") {

                        if (partcode != "")
                            alert("Stock does not exist for this sku.");

                        document.getElementById('spnStockinHand').innerHTML = "0";
                        //document.getElementById('addRow').style.display = 'none';
                        document.getElementById('addBatch').style.display = 'none';
                        document.getElementById('addSerials').style.display = 'none';
                    }
                    else {


                        document.getElementById('spnStockinHand').innerHTML = valueSplits[0];


                        if (valueSplits[1] == "1") {
                            //document.getElementById('addRow').style.display = '';
                            document.getElementById('addBatch').style.display = 'none';
                            document.getElementById('addSerials').style.display = 'none';
                            document.getElementById('tdQuantityHead').style.display = '';
                            document.getElementById('tdQuantityField').style.display = '';
                            document.getElementById('tdInHandHead').style.display = '';
                            document.getElementById('tdInHandField').style.display = '';
                        }
                        else {
                            //document.getElementById('addRow').style.display = 'none';

                            if (valueSplits[1] == "3") {
                                document.getElementById('addSerials').style.display = '';
                                document.getElementById('addBatch').style.display = 'none';

                                document.getElementById('tdQuantityHead').style.display = 'none';
                                document.getElementById('tdQuantityField').style.display = 'none';
                                document.getElementById('tdInHandHead').style.display = 'none';
                                document.getElementById('tdInHandField').style.display = 'none';




                            }
                            else {
                                document.getElementById('addSerials').style.display = 'none';
                            }
                            if (valueSplits[1] == "2") {
                                document.getElementById('addBatch').style.display = '';
                                document.getElementById('tdQuantityHead').style.display = 'none';
                                document.getElementById('tdQuantityField').style.display = 'none';
                                document.getElementById('tdInHandHead').style.display = 'none';
                                document.getElementById('tdInHandField').style.display = 'none';
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


    function txtPartNameTextChanged() {

        var v = "";
        var partname = document.getElementById('<%= txtPartName.ClientID %>').value;
        var SalesChannelID = document.getElementById('<%= salesChannelID.ClientID %>').value;
        var SalesChannelCode = document.getElementById('<%= salesChannelCode.ClientID %>').value;

        if (SalesChannelID == null || SalesChannelID == "0") {
            alert("Please Select From Sales Channel.");
            return false;
        }


        CommonService.GetStockInHandAndIsSerializedByName(partname, SalesChannelID, SalesChannelCode,
            OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {

                var vv = result.toString();
                if (vv == "1") {
                    if (partname != "")
                        alert("Stock does not exist.");
                    document.getElementById('<%= txtPartCode.ClientID %>').value = '';
                            document.getElementById('<%= txtPartName.ClientID %>').value = '';
                            document.getElementById('<%= txtQuantity.ClientID %>').value = '';
                            document.getElementById('<%= txtRate.ClientID %>').value = '';
                            document.getElementById('<%= txtAmount.ClientID %>').value = '';
                            document.getElementById('spnStockinHand').innerHTML = "0";
                            //document.getElementById('addRow').style.display = 'none';
                            document.getElementById('addBatch').style.display = 'none';
                            document.getElementById('addSerials').style.display = 'none';
                        }

                        else {
                            var valueSplits = vv.split('-');
                            document.getElementById('<%= hdnSkuName.ClientID %>').value = partname;
                            document.getElementById('<%= txtPartCode.ClientID %>').value = valueSplits[2];
                    if (valueSplits[0] == "0" && valueSplits[3] == "1") {

                        if (partname != "")
                            alert("Stock does not exist for this sku.");

                        document.getElementById('spnStockinHand').innerHTML = "0";
                        //document.getElementById('addRow').style.display = 'none';
                        document.getElementById('addBatch').style.display = 'none';
                        document.getElementById('addSerials').style.display = 'none';
                    }
                    else {


                        document.getElementById('spnStockinHand').innerHTML = valueSplits[0];


                        if (valueSplits[1] == "1") {
                            //document.getElementById('addRow').style.display = '';
                            document.getElementById('addBatch').style.display = 'none';
                            document.getElementById('addSerials').style.display = 'none';
                            document.getElementById('tdQuantityHead').style.display = '';
                            document.getElementById('tdQuantityField').style.display = '';
                            document.getElementById('tdInHandHead').style.display = '';
                            document.getElementById('tdInHandField').style.display = '';
                        }
                        else {
                            //document.getElementById('addRow').style.display = 'none';

                            if (valueSplits[1] == "3") {
                                document.getElementById('addSerials').style.display = '';
                                document.getElementById('addBatch').style.display = 'none';

                                document.getElementById('tdQuantityHead').style.display = 'none';
                                document.getElementById('tdQuantityField').style.display = 'none';
                                document.getElementById('tdInHandHead').style.display = 'none';
                                document.getElementById('tdInHandField').style.display = 'none';




                            }
                            else {
                                document.getElementById('addSerials').style.display = 'none';
                            }
                            if (valueSplits[1] == "2") {
                                document.getElementById('addBatch').style.display = '';
                                document.getElementById('tdQuantityHead').style.display = 'none';
                                document.getElementById('tdQuantityField').style.display = 'none';
                                document.getElementById('tdInHandHead').style.display = 'none';
                                document.getElementById('tdInHandField').style.display = 'none';
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
        txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
        qtyInHand = document.getElementById('spnStockinHand').innerHTML;




        if (Number(txtQty) > Number(qtyInHand) && Number(qtyInHand) != "0") {
            alert("Input qty can not be greater than Stock in Hand qty.");
            document.getElementById('<%= txtQuantity.ClientID %>').value = "";

        }


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

    function popupSerials(partCode) {


        var SalesChannelID = document.getElementById('<%= salesChannelID.ClientID %>').value;
        var SalesChannelCode = document.getElementById('<%= salesChannelCode.ClientID %>').value;



       // document.getElementById('<%= txtPartCode.ClientID %>').value = '';
       // document.getElementById('<%= txtQuantity.ClientID %>').value = '';

        WinCallLogDetails = dhtmlmodal.open("SerialLookUps", "iframe", "../../../Popuppages/PopUpSerialLookUpClient.aspx?prtcode=" + encodeURIComponent(partCode) + "&SalesChannelID=" + SalesChannelID + "&SalesChannelCode=" + SalesChannelCode + "&TypeID=1" + "&Mode=1", "Select SKU Serials", "width=900px,height=450px,top=25,resize=0,scrolling=auto ,center=1") /* #CC01 correct path supplied */
        WinCallLogDetails.onclose = function () {

            document.getElementById('<%= txtPartCode.ClientID %>').value = '';
            document.getElementById('<%= txtPartName.ClientID %>').value = '';
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

        partCode = encodeURIComponent(partCode);

        WinCallLogDetails = dhtmlmodal.open("BatchLookUps", "iframe", "../../../Popuppages/PopUpBatchLookUpClient.aspx?prtcode=" + encodeURIComponent(partCode) + "&SalesChannelID=" + SalesChannelID + "&SalesChannelCode=" + SalesChannelCode + "&TypeID=1" + "&Mode=1", "Add SKU Batch", "width=900px,height=450px,top=25,resize=0,scrolling=auto ,center=1")


        WinCallLogDetails.onclose = function () {
            var btn = document.getElementById("ctl00_contentHolderMain_btn");
            document.getElementById('<%= txtPartCode.ClientID %>').value = '';
            document.getElementById('<%= txtPartName.ClientID %>').value = '';
            document.getElementById('<%= txtQuantity.ClientID %>').value = '';
            document.getElementById('spnStockinHand').innerHTML = "0";
            __doPostBack(btn.name, "OnClick");
            return true;
        }
        return false;
    }

    function OpenSerialsPopUp() {

        var txtPartCode = document.getElementById('<%= txtPartCode.ClientID %>').value;
        var txtPartName = document.getElementById('<%= txtPartName.ClientID %>').value;
        txtPartCode = encodeURIComponent(txtPartCode);

        var txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
        var qtyInHand = document.getElementById('spnStockinHand').innerHTML;

        //       if (txtQty=="")
        //       {
        //        alert("Please input serials qty.");
        //        return;
        //       }
        // else
        // {
        //        if (Number(txtQty)>100 || Number(qtyInHand)>100)
        //        {
        //        alert("Use upload interface to upload more than 100 serials.");
        //        return;
        //        }
        //  }
        document.getElementById('addSerials').style.display = 'none';
        popupSerials(txtPartCode);
    }
    function OpenBatchPopUp() {
        debugger
        var txtQty = document.getElementById('<%= txtQuantity.ClientID %>').value;
        var qtyInHand = document.getElementById('spnStockinHand').innerHTML;

        //        if (txtQty == "") {
        //            alert("Please input batch qty.");
        //            return;
        //        }
        //        else {
        //            if (Number(txtQty) > 100 || Number(qtyInHand) > 100) {
        //               // alert("Use upload interface if sku code has more than 100 batches.");
        //               // return;
        //            }
        //        }

        document.getElementById('addBatch').style.display = 'none';
        var txtPartCode = document.getElementById('<%= txtPartCode.ClientID %>').value;
        popupBatch(txtPartCode);
    }


</script>

<div id="dt_example">
    <div>
        <input type="hidden" id="lbl" name="lbl" />
        <input type="hidden" runat="server" id="salesChannelID" name="salesChannelID" />
        <input type="hidden" runat="server" id="salesChannelCode" name="salesChannelCode" />
        <input type="hidden" runat="server" id="hdnSkuName" />
        <input type="hidden" runat="server" id="hdnSkucode" />
        <input type="hidden" runat="server" id="hidenCompanyId" />
        <table width="100%" cellpadding="4" cellspacing="0" border="0">
            <tr>
                <td class="formtext" align="left" valign="top" width="10%" height="35px">SKU Code:
                </td>
                <td align="left" valign="top" width="100px">
                    <asp:TextBox ID="txtPartCode" runat="server" MaxLength="20" onchange="javascript:txtPartTextChanged();"
                        CssClass="formfields"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderModel5" runat="server" CompletionListCssClass="wordWheel listMain .box"
                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected"
                        CompletionListItemCssClass="wordWheel itemsMain" MinimumPrefixLength="1"
                        ServiceMethod="GetSKUListByCodesList" ServicePath="~/CommonService.asmx" TargetControlID="txtPartCode"
                        UseContextKey="true">
                    </cc1:AutoCompleteExtender>
                </td>

                <td class="formtext" align="left" valign="top" width="10%" height="35px">SKU Name:
                </td>
                <td align="left" valign="top" width="100px">
                    <asp:TextBox ID="txtPartName" runat="server" MaxLength="20"
                        CssClass="formfields" onchange="javascript:txtPartNameTextChanged();"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected"
                        CompletionListItemCssClass="wordWheel itemsMain" MinimumPrefixLength="1"
                        ServiceMethod="GetSKUNameList" ServicePath="~/CommonService.asmx" TargetControlID="txtPartName"
                        UseContextKey="true">
                    </cc1:AutoCompleteExtender>
                </td>
                <td class="formtext" align="left" valign="top" id="tdQuantityHead" width="100px">Quantity:
                </td>
                <td align="left" valign="top" width="15%" id="tdQuantityField">
                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="4" onchange="validateQty()"
                        CssClass="formfields"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="ftextqty" runat="server" TargetControlID="txtQuantity"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </cc1:FilteredTextBoxExtender>
                </td>
                <td class="formtext" align="left" valign="top" id="tdRateHead" width="100px">Rate:
                </td>
                <td align="left" valign="top" width="15%" id="tdRate">
                    <asp:TextBox ID="txtRate" runat="server" MaxLength="20"
                        CssClass="formfields"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="ftxtRate" runat="server" TargetControlID="txtRate"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </cc1:FilteredTextBoxExtender>
                </td>
                <td class="formtext" align="left" valign="top" id="tdAmountHead" width="100px">Amount:
                </td>
                <td align="left" valign="top" width="15%" id="tdAmount">
                    <asp:TextBox ID="txtAmount" runat="server" MaxLength="25"
                        CssClass="formfields"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="ftxtAmount" runat="server" TargetControlID="txtAmount"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </cc1:FilteredTextBoxExtender>
                </td>
                <td align="left" valign="top">
                    <%--Commented by Adnan
                        <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addRow" name="addRow" onclick="fnGetValueFromControls('<%= txtPartCode.ClientID %>','<%= txtQuantity.ClientID %>','<%= txtPartName.ClientID %>','<%= txtRate.ClientID %>','<%= txtAmount.ClientID %>');">Add To List</a>
                    </div>--%>
                    <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addSerials" name="addSerials"
                            onclick="OpenSerialsPopUp();">Add Serials</a>
                    </div>
                    <div class="float-left">
                        <a href="javascript:void(0);" style="display: none;" id="addBatch" name="addBatch"
                            onclick="OpenBatchPopUp();">Add Batch</a>
                    </div>
                </td>

            </tr>
            <tr>
                <td colspan="7" height="5px"></td>
            </tr>
            <tr>
                <td class="formtext" align="left" valign="top" width="200px" id="tdInHandHead">Stock in Hand:
                </td>
                <td class="formtext" align="left" valign="top" id="tdInHandField">
                    <span id="spnStockinHand">0</span>
                </td>
                <td colspan="7" height="30px">
                    <a href="javascript:void(0);" id="addRow" name="addRow" onclick="fnGetValueFromControls('<%= txtPartCode.ClientID %>','<%= txtQuantity.ClientID %>','<%= txtPartName.ClientID %>','<%= txtRate.ClientID %>','<%= txtAmount.ClientID %>');">Add To List</a><!-- Added by Adnan -->
                    &emsp;
                    <a href="javascript:void(0);" id="delete">Click to delete selected row</a>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" border="0" class="display" id="dtParts">
            <thead>
                <tr>
                    <th>SKU Code
                    </th>
                    <th>SKU Name
                    </th>
                    <th>Quantity
                    </th>
                    <th>Rate
                    </th>
                    <th>Amount
                    </th>
                    <th>Serial No
                    </th>
                    <th>Batch No
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
