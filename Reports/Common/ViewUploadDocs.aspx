<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewUploadDocs.aspx.cs" Inherits="Reports_Common_ViewUploadDocs" %>

<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("[id$=btnSearch]").click(function() {
                if ($("#ctl00_contentHolderMain_ddlUploadType").val() == "0"
                && document.getElementById('<%= txtISPCode.ClientID %>').value == '' &&
                $("[id$=ctl00_contentHolderMain_ucDateFrom_txtDate]").val() == '' &&
                $("[id$=ctl00_contentHolderMain_ucDateTo_txtDate]").val() == ''
                ) {
                    alert('Please select or input in optional field');
                    return false;
                }
                if ($("[id$=ctl00_contentHolderMain_ucDateFrom_txtDate]").val() != '') {
                    if ($("[id$=ctl00_contentHolderMain_ucDateTo_txtDate]").val() == '') {
                        alert('Please input To Date.');
                        return false;
                    }
                }

                if ($("[id$=ctl00_contentHolderMain_ucDateTo_txtDate]").val() != '') {
                    if ($("[id$=ctl00_contentHolderMain_ucDateFrom_txtDate]").val() == '') {
                        alert('Please input From Date.');
                        return false;
                    }
                }
                BindGrid(1);
                return false;
            });



            $("[id$=btngo]").click(function() {
                var total = $("#txtpage").attr("Total");
                if (parseInt($("#txtpage").val()) > parseInt(total)) {
                    alert("please enter less than or equal " + total + "");
                    $("#txtpage").val('');
                    $("#txtpage").focus();
                    return false;
                }
                if (parseInt($("#txtpage").val()) <= 0) {
                    alert("please enter greater than or equal " + total + "");
                    $("#txtpage").val('');
                    $("#txtpage").focus();
                    return false;
                }
                if ($("#txtpage").val() == '') {
                    alert("please enter page index for jump.");
                    $("#txtpage").val('');
                    $("#txtpage").focus();
                    return false;
                }
                BindGrid(parseInt($("#txtpage").val()));
            });

            $("[id$=btnCancel]").click(function() {
                var ExitClass = $("#ClientMessage").attr("class");
                $("#ClientMessage").removeClass(ExitClass);
                $("#ClientMessage").html('');
                Reset();
                BindGrid(1);
                return false;
            });

            $("[id$=txtISPCode]").keypress(function() {
                if (event.keyCode == 13) {
                    document.getElementById('<%= txtISPCode.ClientID %>').focus();
                    return false;
                }
            });

        });
        function Download() {

            location.href = 'http:\\zedsalesV4\\Excel\\Upload\\RSPUpload\\6.jpg';

            return false;
        }
        function Reset() {
            var ExitClass = $("#ClientMessage").attr("class");
            $("#ClientMessage").removeClass(ExitClass);
            $("#ClientMessage").html('');
            $("[id$=divgrd]").hide();
            $("[id$=ctl00_contentHolderMain_ucDateFrom_txtDate]").val('');
            $("[id$=ctl00_contentHolderMain_ucDateTo_txtDate]").val('');
            document.getElementById('<%= txtISPCode.ClientID %>').value = '';
            document.getElementById('<%= hdnISPCode.ClientID %>').value = '';
            $("#ctl00_contentHolderMain_ddlUploadType").val('0');
        }


        function bindgridingwithIndex(obj) {
            BindGrid(parseInt($(obj).attr('Page')));
        }
        function BindGrid(pageIndex) {
            $.ajax({
                type: "POST",
                url: "../../ClientServices/Common/Admin.aspx/GetUploadTypeData",
                data: '{PageIndex: ' + parseInt(pageIndex) + ',PageSize: 10,UploadType: ' + parseInt($("#ctl00_contentHolderMain_ddlUploadType").val()) + ',FromDate: "' + $("#ctl00_contentHolderMain_ucDateFrom_txtDate").val() + '",ToDate: "' + $("#ctl00_contentHolderMain_ucDateTo_txtDate").val() + '",ISPCode: "' + document.getElementById('<%= hdnISPCode.ClientID %>').value + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                error: function(xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                }
            });
        }

        function OnSuccess(response) {
            if (response.d.RTotal != "0") {
                $("[id$=divgrd]").show();
                var JsonDoc = (response.d);
                var DJson = JsonDoc.Records;
                var row = $("[id*=grdUpload] tr:last-child").clone(true);
                $("[id*=grdUpload] tr").not($("[id*=grdUpload] tr:first-child")).remove();
                $.each(DJson, function(index, item) {
                    var length1 = item.UploadPath.split('\\');
                    var ImageUrl = response.d.RUrl + item.UploadPath.split('\\')[length1.length - 1];
                    
                    $("td", row).eq(0).html(item.DocumentType);
                    $("td", row).eq(1).html(item.ISPCode);
                    $("td", row).eq(2).html(item.ISPName);
                    $("td", row).eq(3).html(item.TransactionDate);
                    $("td", row).eq(4).html(item.Latitude);
                    $("td", row).eq(5).html(item.Longitude);
                    $("td", row).eq(6).html(item.Remarks);
                    $("td", row).eq(7).html('<a href=' + ImageUrl + ' class=delbutton id=' + item.UploadPath + ' target=_blank>' + 'View Image' + '</a>');
                    $("[id*=grdUpload]").append(row);
                    row = $("[id*=grdUpload] tr:last-child").clone(true);
                });
                SetPageer(response.d.RPageIndex, response.d.RTotal, response.d.RPageSize);
                var ExitClass = $("#ClientMessage").attr("class");
                $("#ClientMessage").removeClass(ExitClass);
            }
            else {
                $("[id$=divgrd]").hide();
                ClientMessageDisplay("No Record Found", "info", 1)
            }
        }
        function SetPageer(PageIndex, TotalRecords, PageSize) {
            var temppageset = parseFloat(TotalRecords / parseInt(PageSize))
            var objpageset = temppageset.toString().split('.');
            var PageSet = 0;
            if (objpageset[1] != undefined) {
                PageSet = parseInt(TotalRecords / parseInt(PageSize)) + 1;
            }
            else {
                PageSet = parseInt(TotalRecords / parseInt(PageSize));
            }


            $("[id$=hypfirst]").attr("Page", 1);
            $("[id$=hypPrev]").attr("Page", parseInt(parseInt(PageIndex) - 1));
            $("[id$=hypNext]").attr("Page", parseInt(parseInt(PageIndex) + 1));
            $("[id$=hypLast]").attr("Page", PageSet);
            $("[id$=txtpage]").attr("Total", PageSet);
            $("[id$=spntotal]").text(TotalRecords);
            $("[id$=spnpgefrom]").text(PageIndex);
            $("[id$=spnto]").text(PageSet);


            if (parseInt(parseInt(PageIndex) - 1) == 0) {
                $("[id$=hypPrev]").hide();
            }
            else {
                $("[id$=hypPrev]").show();
            }
            if (parseInt(parseInt(PageIndex)) == PageSet) {
                $("[id$=hypNext]").hide();
            }
            else {
                $("[id$=hypNext]").show();
            }
        }
        function ClientMessageDisplay(Message, ClassType, ErrorType) {
            var ExitClass = $("#ClientMessage").attr("class");
            $("#ClientMessage").removeClass(ExitClass);
            var htmls = '<p style="float: left; ">'
            htmls += '<div style="float: left; margin-left: 40px; width: 80%;">';
            htmls += '<span>' + Message + '</span></div>';
            htmls += '</p>';

            $("#ClientMessage").html(htmls);
            $("#ClientMessage").addClass(ClassType);
        }

        function txtISPCodeChanged() {
            try {
                var ISPCode = document.getElementById('<%= txtISPCode.ClientID %>').value;
                var hdnISPCode = document.getElementById('<%= hdnISPCode.ClientID %>');
                hdnISPCode.value = ISPCode.split('-')[1];
            }
            catch (err) {
                hdnISPCode.value = '';
            }

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div id="ClientMessage" class="">
    </div>
    <div>
        <asp:HiddenField ID="hdnISPCode" runat="server" Value="" />
        <asp:Label ID="lbltvErrorMsg" runat="server" Visible="false"></asp:Label>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="mainheading">
            View RSP Upload
        </div>
        <div class="contentbox">
            <div class="mandatory">
                (+) Marked fields are Optional.
            </div>
            <div class="H25-C3-S">             
                <ul>
                    <li class="text">Date From: <span class="error">+</span>
                    </li>
                    <li class="field">
                        <uc2:ucDatePicker ID="ucDateFrom" runat="server" defaultDateRange="true" RangeErrorMessage="Date should be less or equal to current date." />
                    </li>
                    <li class="text">Date To: <span class="error">+</span>
                    </li>
                   <li class="field">
                        <uc2:ucDatePicker ID="ucDateTo" runat="server" defaultDateRange="true" RangeErrorMessage="Date should be less or equal to current date." />
                    </li>                   
                </ul>
                <ul>
                    <li class="text">Upload Type: <span class="error">+</span>
                    </li>
                    <li class="field">
                        <asp:DropDownList ID="ddlUploadType" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </li>
                    <li class="text">RSP Code: <span class="error">+</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtISPCode" onchange="txtISPCodeChanged();" runat="server" CssClass="formfields"
                            MaxLength="20"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                            CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                            MinimumPrefixLength="3" ServiceMethod="GetISPCodesList" ServicePath="../../CommonService.asmx"
                            TargetControlID="txtISPCode" UseContextKey="true">
                        </cc1:AutoCompleteExtender>
                    </li>
                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnSearch" runat="server" CausesValidation="false" CssClass="buttonbg"
                                Text="Search" ToolTip="Search" />
                        </div>
                        <div class="float-left">
                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" CausesValidation="false"
                                Text="Cancel" ToolTip="Cancel" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>

        <div runat="server" id="divgrd" style="display: none;">
            <div class="mainheading">
                Upload Details
            </div>
            <div class="contentbox">
                <div class="grid1">
                    <asp:GridView ID="grdUpload" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" EmptyDataText="No Record Found"
                        GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                        AlternatingRowStyle-CssClass="Altrow">
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:BoundField DataField="DocumentType" HeaderStyle-HorizontalAlign="Left" HeaderText="Document Type">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ISPCode" HeaderStyle-HorizontalAlign="Left" HeaderText="RSP Code">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ISPName" HeaderStyle-HorizontalAlign="Left" HeaderText="RSP Name">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TransactionDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Date">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Latitude" HeaderStyle-HorizontalAlign="Left" HeaderText="Latitude">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Longitude" HeaderStyle-HorizontalAlign="Left" HeaderText="Longitude">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderStyle-HorizontalAlign="Left" HeaderText="Remarks">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UploadPath" HeaderStyle-HorizontalAlign="Left" HeaderText="View"
                                ItemStyle-CssClass="elink2">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" />
                        <EditRowStyle CssClass="editrow" />
                        <AlternatingRowStyle CssClass="Altrow" />
                    </asp:GridView>
                    <div id="dvFooter" visible="true" runat="server" class="pagination">
                        <div class="pager">
                            <ul>
                                <li class="block"><a id="hypfirst" onclick="javascript:bindgridingwithIndex(this);">&laquo; First</a></li>
                                <li class="block"><a id="hypPrev" onclick="javascript:bindgridingwithIndex(this);">&#8249;
                                                Prev</a></li>
                                <li class="range">page&nbsp;<span id="spnpgefrom"></span>&nbsp; of &nbsp;<span id="spnto">6</span></li>
                                <li class="record">Record(s) found : <span id="spntotal"></span></li>
                                <li class="block"><a id="hypNext" onclick="javascript:bindgridingwithIndex(this);">Next
                                                &#8250;</a></li>
                                <li class="block"><a id="hypLast" onclick="javascript:bindgridingwithIndex(this);">Last
                                                &raquo;</a></li>
                                <li class="range">Jump to page </li>
                                <li class="range">
                                    <input id="txtpage" value="" class="formfields-W70" type="text" />
                                </li>
                                <li>
                                    <input type="button" class="btn page" id="btngo" value="GO" />
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
