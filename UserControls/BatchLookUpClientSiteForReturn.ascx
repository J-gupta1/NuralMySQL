<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BatchLookUpClientSiteForReturn.ascx.cs" 
Inherits="BatchLookupClientSideForReturn" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<script type="text/javascript" charset="utf-8">
    /* Global var for counter */
    var giCount = 1;
    var oTable;
    var giRedraw = false;

    $(document).ready(function() {
    $('#dtSerials').dataTable();
   
    });
    $(document).ready(function() {
        $('#aspnetForm').submit(function() {
            var sData = oTable.$('input:hidden').serialize();
            alert("The following data would have been submitted to the server: \n\n" + sData);
            document.getElementById('lbl').value = sData;
            alert(document.getElementById('lbl').value);
            return true;
        });

        oTable = $('#dtSerials').dataTable();
   
      
    });

    function fnClickAddRow(partCode, qty, serialNo, BatchNo, InHandQty,InvoiceNumber,InvoiceDate) {
    
    if (InHandQty=="0" || qty=="0")
    return;
    
        $('#dtSerials').dataTable().fnAddData([
					"<span>" + partCode + " <span/>",
					"<span>" + serialNo + " <span/>",
					"<span>" + BatchNo + " <span/>",
					"<span>" + InHandQty + " <span/>",
					"<span>" + InvoiceNumber + " <span/>",
					"<span>" + InvoiceDate + " <span/>",
					"<input type=text name=qty  />"]);

        giCount++;
        					
    }
  
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }



    $(document).ready(function() {
        /* Add a click handler to the rows - this could be used as a callback */
        $("#dtSerials tbody").click(function(event) {
            $(oTable.fnSettings().aoData).each(function() {
                $(this.nTr).removeClass('row_selected');
            });
            $(event.target.parentNode).addClass('row_selected');
        });
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
        
        var isAnyRowToSubmit = 0;
        var isDuplicate = "0";
        var partCode = document.getElementById('<%= lblPrtCode.ClientID %>').innerText;
        var InvoiceNumber = document.getElementById('<%= lblInvoiceNumber.ClientID %>').innerText;
        var InvoiceDate = document.getElementById('<%= lblInvoiceDate.ClientID %>').innerText;
        
        var qty = '';
        var batchNo = '';
        var serialno = '';
        var isInvalidQty = "0";
        var span = "";
        var values = {};

        var pattern = /^-?[-0-9]+([-0-9]{1,4})?$/;


        var OldTable = window.parent.$('#dtParts').dataTable();
        var InputCount = 0;
        var InvalidQtyFormat = 0;
        if (OldTable != null) {
            $("input[value!='']'", oTable.fnGetNodes()).each(function() {
                var row = $(this).closest('tr').get(0);

                var valInputed = $('input', row).val();
                if (valInputed.match(pattern) == null) {
                    InvalidQtyFormat = 1;
                }
             
                if (Number(valInputed) <= 0) {
                    InvalidQtyFormat = 1;
                    return;
                }


                InputCount = InputCount + 1;
            });
            var v = CheckGridRowCount(OldTable, InputCount);
            if (v == false) {
                return;
            }
        }

        if (InvalidQtyFormat != 0) {
            alert('Input Format is wrong');
            return;
        }

        $("input[value!='']'", oTable.fnGetNodes()).each(function() {


            values[this.name] = $(this).val();

          //  alert(values["qtyInhand"]);


            var cccc = window.parent.$('#dtParts').dataTable();

            var row = $(this).closest('tr').get(0);
            var aPos = oTable.fnGetPosition(row);

            var aData = oTable.fnGetData(aPos[0]);


            var jqTds = $('>td', row);




            var inHandQtyArr = $(row).text().split(' ');

            //            var inHandQty = values["qtyInhand"];
          

            var inHandQty = inHandQtyArr[1];

            // var inputQuantity = values["qty"];

            var inputQuantity = document.createElement('span');
            //var inputQuantity = document.createElement('span');
            inputQuantity.innerHTML = jqTds[2].innerHTML;
            row.appendChild(inputQuantity);
            //  alert(inputQuantity.firstChild.value);

            inputQuantity = inputQuantity.getElementsByTagName('input')[0].value;
            

            if ((Number(inputQuantity) > Number(inHandQty)) && (Number(inHandQty) != 0)) {
                isInvalidQty = "1";
                return;
            }
            $("input", cccc.fnGetNodes()).each(function() {
                var rowParent = $(this).closest('tr').get(0);
                var aPosParent = cccc.fnGetPosition(rowParent);
                var ParentRowText = $(rowParent).text();
                var serialnoArray = ParentRowText.split(' ');

                //    alert(serialnoArray);
                //             alert(serialnoArray[0] + "***********" + partCode + "********" + serialnoArray[2] + "**********" + inHandQtyArr[0]);

                // alert(serialnoArray[3].replace(/(^\s*)|(\s*$)/g, '')+' ------- ' + inHandQtyArr[0].replace(/(^\s*)|(\s*$)/g, ''));

                if (Number(inputQuantity) > 0) {
                    if (serialnoArray[0].replace(/(^\s*)|(\s*$)/g, '') == partCode.replace(/(^\s*)|(\s*$)/g, '') && serialnoArray[2].replace(/(^\s*)|(\s*$)/g, '') == inHandQtyArr[0].replace(/(^\s*)|(\s*$)/g, '')) {
                        isDuplicate = "1";
                        return;
                    }
                }

            });

            if (isDuplicate == "1") {

                // alert("Serial no " + serialno + " added already!");
                return;
            }
            else {
                if (Number(inputQuantity) > 0) {
                    isAnyRowToSubmit = 1;
                    cccc.fnAddData([
                       				"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
                       				"<span>" + inputQuantity + " <span/><input type=hidden name=qty value=" + inputQuantity + " />",
                      				"<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />",
                       					"<span>" + inHandQtyArr[0] + " <span/><input type=hidden name=batchno value=" + inHandQtyArr[0] + " />",
                       					"<span>" + InvoiceNumber + " <span/><input type=hidden name=InvoiceNumber value=" + InvoiceNumber + " />",
                       					"<span>" + InvoiceDate + " <span/><input type=hidden name=InvoiceDate value=" + InvoiceDate + " />"
                       					]);
                }
            }






        });

        if (isInvalidQty == "1") {
            alert("input qty should not be greater than Sold qty!");
        }
        if (isDuplicate == "1") {
            if (isInvalidQty == "0") {
                alert("SKU Code and BatchNo added already!");
            }
        }
        else {
            if (isAnyRowToSubmit > 0) {

                window.parent.BatchLookUps.hide();
            }
        }

        //   window.parent.document.getElementById('lblPrice').value = sData;



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
<div id="dt_example">
 <div class="contentbox3">
    <div id="container">
        <a href="javascript:void(0);" style="display: none;" id="delete">Click to Delete a new
            row</a>
        <input type="hidden" id="lbl" name="lbl" />
     
        <asp:Label ID="lblPrtCode" runat="server" style="display: none;"></asp:Label>
        <asp:Label ID="lblInvoiceNumber" runat="server" style="display: none;"></asp:Label>
        <asp:Label ID="lblInvoiceDate" runat="server" style="display: none;"></asp:Label>
        <asp:Label ID="lblTypeID" runat="server" style="display: none;"></asp:Label>
        <table cellpadding="0" cellspacing="0" border="0" class="display" id="dtSerials">
            <thead>
            
                <tr>
                    <th>
                        Batch No
                    </th>
                    <th>
                      Sold Quantity
                    </th> 
                     <th>
                      Return Quantity
                    </th>
                </tr>
            </thead>
              <asp:Literal runat="server" ID="ltData" EnableViewState="true"></asp:Literal>

            <tbody>
            </tbody>
        </table>
    </div>
    <div id="container">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
        <td height="20px">
        
        </td>
        </tr>
        <tr>
                <td>
                    <a href="javascript:void(0);" onclick="clickMe();" runat="server" id="submitme" class="buttonbg">Submit</a>
                </td>
            </tr>
        </table>
    </div>
   
    </div>
</div>
