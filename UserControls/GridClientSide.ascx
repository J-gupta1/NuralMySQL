<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridClientSide.ascx.cs"
    Inherits="Web.Controls.GridClientSide" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript" charset="utf-8">
    /* Global var for counter */
    var giCount = 1;
    var oTable;
    var giRedraw = false;
    $(document).ready(function() {
        $('#dtParts').dataTable();

        /* Init the table */
        oTable = $('#dtParts').dataTable();

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
                TotalQuantity();
            }
        });


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

  

  
</script>

<div id="dt_example">
    <div>
        <input type="hidden" id="lbl" name="lbl" />
        <table width="100%" cellpadding="4" cellspacing="0" border="0">
            <tr>
                <td width="100%">
                    <a href="javascript:void(0);" id="delete">Click to delete selected row</a>
                </td>
            </tr>
            <tr>
            <td width="100%">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="display" id="dtParts">
                    <thead>
                        <tr>
                            <asp:Literal ID="litGridHeader" runat="server"></asp:Literal>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Literal ID="lit" runat="server"></asp:Literal>
                    </tbody>
                </table>
                </td>
            </tr>
        </table>
    </div>
    <div class="spacer">
    </div>
</div>
