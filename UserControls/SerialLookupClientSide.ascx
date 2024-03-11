<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SerialLookupClientSide.ascx.cs"
    Inherits="Web.Controls.SerialLookupClientSide" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Src="ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>

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


    $(document).ready(function () {
        $('#dtSerials').dataTable();

    });
    $(document).ready(function () {
        $('#aspnetForm').submit(function () {
            var sData = oTable.$('input:hidden').serialize();
            document.getElementById('lbl').value = sData;
            return true;
        });

        oTable = $('#dtSerials').dataTable();
        //  oTable.fnSetColumnVis(0, false);
        //  oTable.fnSetColumnVis(1, false);
        //  oTable.fnSetColumnVis(3, false); 
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


    //    function fnClickAddRow(serialNo) {
    //        $('#dtSerials').dataTable().fnAddData([
    //					"<input type=checkbox name=chk  />", "<span>" + serialNo + " <span/><input type=hidden name=serialno value=" + serialNo + " />"]);

    //        giCount++;
    //    }

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



    $(document).ready(function () {
        /* Add a click handler to the rows - this could be used as a callback */
        $("#dtSerials tbody").click(function (event) {
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
        debugger
        
        var chkOP = document.getElementById('<%= chk.ClientID %>');
        if (chkOP.checked) {
            return ValidateSerials();

        }
        var arr2 = [];
        var isDuplicate = "0";
        var partCode = document.getElementById('<%= hdnSkuCode.ClientID %>').value;

        var Mode = document.getElementById('<%= hdnMode.ClientID %>').value;

        var qty = '1';
        if (Mode == "2") {
            qty = "-1";
        }

        var batchNo = '';
        var serialno = '';

        var cccc = window.parent.$('#dtParts').dataTable();
        var chkCount = 0;

        if (cccc != null) {
            $("input:checked", oTable.fnGetNodes()).each(function () {
                chkCount = chkCount + 1;

            });
            var v = CheckGridRowCount(cccc, chkCount);
            if (v == false) {
                return;
            }
        }


        var i = 0;
        $("input:checked", oTable.fnGetNodes()).each(function () {



            var row = $(this).closest('tr').get(0);
            var aPos = oTable.fnGetPosition(row);
            serialno = $(row).text();
            //            $("input", cccc.fnGetNodes()).each(function() {
            //                var rowParent = $(this).closest('tr').get(0);
            //                var aPosParent = cccc.fnGetPosition(rowParent);
            //                var ParentRowText = $(rowParent).text();
            //                var serialnoArray = ParentRowText.split(' ');


            //                if (serialnoArray[2].replace(/(^\s*)|(\s*$)/g, '') == serialno.replace(/(^\s*)|(\s*$)/g, '')) {
            //                    isDuplicate = "1";
            //                    return;
            //                }

            //            });
            
           var  SkuName = window.parent.document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_hdnSkuName').value;
            /* #CC06 Add Start */
            var IndexStockBinTypeID = '';
            var vrStockBinTypeID = document.getElementById('<%= hdnStockBinTypeID.ClientID %>');
            var vrStockBinTypeCode = document.getElementById('<%= hdnStockBinTypeCode.ClientID %>'); 
            /* #CC06 Add End */


            var IndexPartCode = "<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />";
            var IndexSkuName = "<span>" + SkuName + " <span/><input type=hidden name=skuname value=" + SkuName + " />";
            var IndexQuantity = "<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />";
            var IndexSerialData = "<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />";
            var IndexBatchNo = "<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />";
            var IndexStockBinTypeCode = "<span>" + vrStockBinTypeCode.value + " <span/><input type=hidden name=hdnstockbinyypeCode value=" + vrStockBinTypeCode.value + " />";/* #CC06 Added */


            if (cccc.fnGetData().length > 0) {
                var nNodes = cccc.dataTable().fnGetNodes();
                if (nNodes.length > 0) {
                    for (var d = 0; d < nNodes.length; d++) {

                        var dat = cccc.fnGetData(nNodes[d]);

                        if (IndexSerialData == dat[3]) {
                            isDuplicate = "1";
                            return false;
                        }
                    }
                }
            }

            if (isDuplicate == "1") {

                // alert("Serial no " + serialno + " added already!");
                return;
            }
            else {



                //                cccc.fnAddData([
                //        				"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
                //        				"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
                //        				"<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />",
                //        					"<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />"]);

                arr2[i] = new Array(IndexPartCode, IndexSkuName, IndexQuantity, IndexSerialData, IndexBatchNo, IndexStockBinTypeCode); /* #CC06 IndexStockBinTypeID Added */

                i = i + 1;



            }






        });

        if (isDuplicate == "1") {
            alert("Serial no " + serialno + " added already!");
        }
        else {
            cccc.fnAddData(arr2);
            parent.WinCallLogDetails.hide();
        }

        //   window.parent.document.getElementById('lblPrice').value = sData;



    }


    function OnChangeSerials(isSubmit) {
        var arr2 = [];
        var serials = document.getElementById('<%= txtSerials.ClientID  %>').value;
        var inputvalue = serials;
        var isDuplicate = "0";

        var arrCurrvalueTemp = "";
        var a = serials.replace(',', '').replace(/(^|\r\n|\n)([^*]|$)/g, "$1,$2");

        a = a.substring(1, a.length);
        arrCurrvalueTemp = a.split(',');

        var arrCurrvalue = arrCurrvalueTemp;
        var Olddt = window.parent.$('#dtParts').dataTable();


        CheckGridRowCount(Olddt, arrCurrvalue.length);


        for (var i = 0; i < arrCurrvalue.length; i++) {

            var cccc = window.parent.$('#dtParts').dataTable();

            //            $("input", cccc.fnGetNodes()).each(function() {
            //                var rowParent = $(this).closest('tr').get(0);
            //                var aPosParent = oTable.fnGetPosition(rowParent);
            //                var ParentRowText = $(rowParent).text();
            //                var serialno = ParentRowText.split(' ');


            //                if (serialno[2] == arrCurrvalue[i]) {
            //                    isDuplicate = "1";
            //                    isSubmit = "false";
            //                    return;
            //                }

            //            });





            if (isDuplicate == "1") {
                alert("Serial no.(" + arrCurrvalue[i] + ") added already.");
                return;
            }


        }
        var IndexSerialData = '';
        var IndexPartCode = '';
        var IndexQuantity = '';
        var IndexBatchNo = '';
        /* #CC06 Add Start */
        var IndexStockBinTypeID = '';
        var vrStockBinTypeID = document.getElementById('<%= hdnStockBinTypeID.ClientID %>');
        var vrStockBinTypeCode = document.getElementById('<%= hdnStockBinTypeCode.ClientID %>');
        /* #CC06 Add End*/
        var SkuName = window.parent.document.getElementById('ctl00_contentHolderMain_PartLookupClientSide1_hdnSkuName').value;

        var IndexSkuName = "<span>" + SkuName + " <span/><input type=hidden name=skuname value=" + SkuName + " />";


        if (isDuplicate == "0") {
            IndexSerialData = "<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />";
            IndexPartCode = "<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />";
            IndexQuantity = "<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />";
            IndexBatchNo = "<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />";
            IndexStockBinTypeCode = "<span>" + vrStockBinTypeCode.value + " <span/><input type=hidden name=hdnstockbinyypeCode value=" + vrStockBinTypeCode.value + " />";/* #CC06 Added */
            for (var i = 0; i < arrCurrvalue.length; i++) {

                var partCode = document.getElementById('<%= hdnSkuCode.ClientID %>').value;
                var Mode = document.getElementById('<%= hdnMode.ClientID %>').value;

                var qty = '1';
                if (Mode == "2") {
                    qty = "-1";
                }
                var batchNo = '';
                var serialno = arrCurrvalue[i].replace(/(^\s*)|(\s*$)/g, '');
                //                    cccc.fnAddData([
                //        				"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
                //        				"<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
                //        				"<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />",
                //        					"<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />"]);


                IndexPartCode = "<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />";
                IndexQuantity = "<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />";
                IndexSerialData = "<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />";
                IndexBatchNo = "<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />";
                IndexStockBinTypeCode = "<span>" + vrStockBinTypeCode.value + " <span/><input type=hidden name=hdnstockbinyypeCode value=" + vrStockBinTypeCode.value + " />";/* #CC06 Added */
                var nNodes = cccc.dataTable().fnGetNodes();
                if (nNodes.length > 0) {
                    for (var d = 0; d < nNodes.length; d++) {

                        var dat = cccc.dataTable().fnGetData(nNodes[d]);

                        if (IndexSerialData == dat[3]) {
                            alert('Serials already exsts ' + serialno);
                            u = 1;
                            c = 1;
                            return false;
                        }
                    }
                }

                if (isSubmit == "true") {

                    arr2[i] = new Array(IndexPartCode, IndexSkuName, IndexQuantity, IndexSerialData, IndexBatchNo, IndexStockBinTypeCode); /* #CC06 IndexStockBinTypeID Added */

                }
            }
            cccc.fnAddData(arr2);
            parent.WinCallLogDetails.hide();

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
        
        var SalesChannelID = document.getElementById('<%= hdnSalesChannelID.ClientID %>');
        var SalesChannelCode = document.getElementById('<%= hdnSalesChannelCode.ClientID %>');
        var SkuCode = document.getElementById('<%= hdnSkuCode.ClientID %>');
        var TypeID = document.getElementById('<%= hdnTypeID.ClientID %>').value;
        var Mode = document.getElementById('<%= hdnMode.ClientID %>').value;

        //  var StockBinTypeID = document.getElementById('<%= hdnStockBinTypeID.ClientID %>'); /* #CC01 Added */
        var StockBinTypeID = 2;

        CommonService.ValidateAllSerials(SalesChannelID.value, SalesChannelCode.value, SkuCode.value, Serials.value, TypeID, Mode, StockBinTypeID,
                    OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {/* #CC01 StockBinTypeID.value Added */

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

    function chk(ch) {
        $('input', oTable.fnGetNodes()).each(function () {
            $('input', oTable.fnGetFilteredNodes()).attr('checked', ch.checked);
        });


    }

    function chk1() {
        var nNodes = $('input', oTable.fnGetNodes());
        for (var i = 0; i < nNodes.length; i++) {
            $("chk").setAttribute('checked');
        }
        var nHiddenNodes = $('input', oTable.fnHiddenGetNodes());
        for (var i = 0; i < nHiddenNodes.length; i++) {
            $("chk").setAttribute('checked');
        }
    }

    //    $('#check_all').click(function() { var oTable1 = $('#dtSerials').dataTable(); $('input', oTable1.fnGetFilteredNodes()).attr('checked', this.checked); }); 

    //  $('#check_all').onchange(function () {   
    //       $('input', oTable.fnGetFilteredNodes()).attr('checked', this.checked); 
    //          }); 
    //          




</script>
<div class="contentbox3">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div>
        <asp:CheckBox ID="chk" runat="server" Checked="false"
            Text="Do you want to enter serials nos.?" AutoPostBack="true"
            OnCheckedChanged="chk_CheckedChanged" />
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
                        <th>Select 
                        </th>
                        <th>Serial No
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
                    <td class="formtext" align="left" valign="top" width="5%">Serials
                    </td>
                    <td align="left" valign="top">
                        <%--  <asp:TextBox ID="txtSerials" CssClass="form_textarea2" runat="server" TextMode="MultiLine" Width="250px" onchange="OnChangeSerials('false');"></asp:TextBox>--%>
                        <asp:TextBox ID="txtSerials" CssClass="form_textarea2" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
                        <div class="error">
                            Input comma or press enter between serial nos.<br />
                            2. SerialNo. Min length(<asp:Literal ID="litSerial_MinL" runat="server"></asp:Literal>) and Max Length(<asp:Literal ID="litSerial_MaxL" runat="server"></asp:Literal>)
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="15px"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <a href="javascript:void(0);" onclick="clickMe();" runat="server" class="buttonbg"
                            id="submitme">Add Serials 
                   
                        </a>

                        <asp:HiddenField ID="hdnSalesChannelID" runat="server" />
                        <asp:HiddenField ID="hdnSalesChannelCode" runat="server" />
                        <asp:HiddenField ID="hdnSkuCode" runat="server" />
                        <asp:HiddenField ID="hdnTypeID" runat="server" />
                        <input type="hidden" id="hdnMode" runat="server" name="hdnMode" />
                        <%--#CC01 Add Start --%>
                        <asp:HiddenField ID="hdnStockBinTypeID" runat="server" Value="2" />
                        <asp:HiddenField ID="hdnStockBinTypeCode" runat="server" />
                        <%--#CC01 Add End --%>
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear"></div>
    </div>
    <asp:HiddenField ID="hdnSerialNoLengthMin" runat="server" />
    <asp:HiddenField ID="hdnSerialNoLengthMax" runat="server" />
</div>
