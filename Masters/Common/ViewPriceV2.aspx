<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewPriceV2.aspx.cs" Inherits="Masters_HO_Admin_ViewPriceV2" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>

<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--#CC02 Add Start --%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("[id$=btnSearch]").click(function () {
                if ($("#contentHolderMain_cmbPriceList").val() == "0"
                 && $("#contentHolderMain_cmbSKUName").val() == "0" &&
                $("[id$=contentHolderMain_ucDateFrom_txtDate]").val() == '' &&
                $("[id$=contentHolderMain_ucDateTo_txtDate]").val() == ''
                ) {
                    alert('Please select or input in optional field');
                    return false;
                }

                if ($("[id$=contentHolderMain_ucDateFrom_txtDate]").val() != '') {
                    if ($("[id$=contentHolderMain_ucDateTo_txtDate]").val() == '') {
                        alert('Please input To Date.');
                        return false;
                    }
                }

                if ($("[id$=contentHolderMain_ucDateTo_txtDate]").val() != '') {
                    if ($("[id$=contentHolderMain_ucDateFrom_txtDate]").val() == '') {
                        alert('Please input From Date.');
                        return false;
                    }
                }
                if ($("[id$=contentHolderMain_ucDateFrom_txtDate]").val() != '' && $("[id$=contentHolderMain_ucDateTo_txtDate]").val() != '') {

                    var datefrom = new Date($("[id$=contentHolderMain_ucDateFrom_txtDate]").val());
                    var dateto = new Date($("[id$=contentHolderMain_ucDateTo_txtDate]").val());
                    /* var timeDiff = Math.abs(dateto.getTime() - datefrom.getTime());
                     var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                     alert(diffDays);*/
                    if (datefrom > dateto) {
                        alert("from date cannot be greater than to date.");
                        return false;
                    }
                }


                BindGrid(1);
                return false;
            });
            $("[id$=btnViewAll]").click(function () {
                var ExitClass = $("#ClientMessage").attr("class");
                $("#ClientMessage").removeClass(ExitClass);
                $("#ClientMessage").html('');
                Reset();
                BindGrid(1);
                return false;
            });
            function Reset() {
                var ExitClass = $("#ClientMessage").attr("class");
                $("#ClientMessage").removeClass(ExitClass);
                $("#ClientMessage").html('');
                $("[id$=divgrd]").hide();
                $("[id$=contentHolderMain_ucDateFrom_txtDate]").val('');
                $("[id$=contentHolderMain_ucDateTo_txtDate]").val('');
                $("#contentHolderMain_cmbPriceList").val('0');
                $("#contentHolderMain_cmbSKUName").val('0');
            }
            $("[id$=btngo]").click(function () {
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


        });
        function bindgridingwithIndex(obj) {
            BindGrid(parseInt($(obj).attr('Page')));
        }
        function BindGrid(pageIndex) {
            $.ajax({
                type: "POST",
                url: "../../ClientServices/Common/Admin.aspx/GetPriceList",
                data: '{PageIndex:  ' + parseInt(pageIndex) + ',PageSize: 10,PriceList: ' + parseInt($("#contentHolderMain_cmbPriceList").val()) + ',SKUCode:' + parseInt($("#contentHolderMain_cmbSKUName").val()) + ',FromDate:"' + $("#contentHolderMain_ucDateFrom_txtDate").val() + '",ToDate:"' + $("#contentHolderMain_ucDateTo_txtDate").val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                }
            });
        }

        function OnSuccess(response) {
            if (response.d.RTotal != "0") {
                $("[id$=divgrd]").show();
                var JsonDoc = (response.d);
                var DJson = JsonDoc.Records;
                var row = $("[id*=GridPrice] tr:last-child").clone(true);
                $("[id*=GridPrice] tr").not($("[id*=GridPrice] tr:first-child")).remove();
                $.each(DJson, function (index, item) {
                    /* alert(item.PriceListName + "," + item.SKUCode + "," + item.WHPrice + "," + item.SDPrice + "," + item.MDPrice + "," + item.RetailerPrice + "," + item.MOP + "," + item.MRP + "," + item.EffectiveDate + "," + item.ValidTill);
                      var length1 = item.UploadPath.split('\\');
                      var ImageUrl = response.d.RUrl + item.UploadPath.split('\\')[length1.length - 1];*/
                    $("td", row).eq(0).html(item.PriceListName)
                    $("td", row).eq(1).html(item.SKUCode)
                    $("td", row).eq(2).html(item.WHPrice)
                    $("td", row).eq(3).html(item.SDPrice)
                    $("td", row).eq(4).html(item.MDPrice)
                    $("td", row).eq(5).html(item.RetailerPrice)
                    $("td", row).eq(6).html(item.MOP)
                    $("td", row).eq(7).html(item.MRP)
                    $("td", row).eq(8).html(item.EffectiveDate)
                    $("td", row).eq(9).html(item.ValidTill)
                    /* $("td", row).eq(7).html('<a href=' + ImageUrl + ' class=delbutton id=' + item.UploadPath + ' target=_blank>' + 'View Image' + '</a>');*/
                    $("[id*=GridPrice]").append(row);
                    row = $("[id*=GridPrice] tr:last-child").clone(true);
                });
                SetPageer(response.d.RPageIndex, response.d.RTotal, response.d.RPageSize);
                var ExitClass = $("#ClientMessage").attr("class");
                $("#ClientMessage").removeClass(ExitClass);
                var htmls = '<p style="float: left; ">'
                htmls += '<div style="float: left; margin-left: 40px; width: 80%;">';
                htmls += '<span></span></div>';
                htmls += '</p>';

                $("#ClientMessage").html(htmls);
                checkGrid();


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
        function checkGrid() {
            col_num = document.getElementById('<%=hdnHideColumns.ClientID%>').value;
            rowsCount = document.getElementById('<%=GridPrice.ClientID%>').rows;
            for (i = 0; i < rowsCount.length; i++) {
                var res = col_num.split(",");
                for (j = 0; j < res.length; j++) {
                    var hidecolumns = parseInt(res[j]);
                    if (hidecolumns > 2) {
                        hidecolumns = hidecolumns - 1;
                    }
                    rowsCount[i].cells[hidecolumns].style.display = "none";
                }
            }
        }
    </script>
    <%--#CC02 Add End --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <%--#CC02 Add Start --%>
    <div id="ClientMessage" class="">
    </div>
    <%--#CC02 Add End --%>


    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Search Price
    </div>

    <%--<asp:LinkButton ID="LBViewPrice" runat="server" CausesValidation="False" CssClass="elink7"
                                                        OnClick="LBViewPrice_Click">Add Price</asp:LinkButton>--%>


    <div class="contentbox">
        <div class="H20-C3-S">
            <ul>
                <li class="text">Price List Name:
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbPriceList" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                </li>
                <li class="text">SKU Code:
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbSKUName" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                </li>
                <li class="text">From Date:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Invalid date." IsRequired="true"
                        RangeErrorMessage="Date should be greater then equal to current date." ValidationGroup="Add" />
                </li>
            </ul>
            <ul>
                <li class="text">To Date:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Invalid date." IsRequired="true"
                        RangeErrorMessage="Date should be greater then equal to current date." ValidationGroup="Add" />
                </li>
                <li class="text"></li>
                <li class="field">
                    <%--#CC02 Add Start --%>
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                            Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnViewAll" runat="server" CssClass="buttonbg" Text="View All Data"
                            ToolTip="ViewAll" />
                        <asp:HiddenField ID="hdnHideColumns" runat="server" />
                    </div>
                    <%--#CC02 Add End --%>

                    <%-- #CC02 Comment Start <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                                                                OnClick="btnSearch_Click" Text="Search" />
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="View All Data"
                                                                ToolTip="Cancel" OnClick="btnCancel_Click" /> #CC02 Comment End --%>
                </li>
            </ul>
        </div>
    </div>

    <%--#CC02 Comment Start 
                                    
                                    <asp:Panel ID="pnlsearch" runat="server" Visible="false">
                                    <tr>
                                        <td align="left" valign="top" class="tableposition">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="90%" align="left" class="tableposition">
                                                        <div class="mainheading_rpt">
                                                            <div class="mainheading_rpt_left">
                                                            </div>
                                                            <div class="mainheading_rpt_mid">
                                                                Price List</div>
                                                            <div class="mainheading_rpt_right">
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td width="10%" align="right">
                                                        <asp:Button ID="ExportToExcel" runat="server" CssClass="excel" Text="" OnClick="ExportToExcel_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                                <contenttemplate>
                                                    <div class="contentbox">
                                                        <div class="grid2">
                                                            <asp:GridView ID="GridPrice" runat="server" AlternatingRowStyle-CssClass="gridrow1"
                                                                AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                                                DataKeyNames="PriceMasterID" AllowPaging="True" PageSize='<%$ AppSettings:GridViewPageSize %>'
                                                                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                                                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                                                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                                                RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                                                OnPageIndexChanging="GridPrice_PageIndexChanging"  OnRowDataBound="GridPrice_RowDataBound">
                                                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="PriceListName" HeaderStyle-HorizontalAlign="Left" HeaderText="PriceList Name"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="WHPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="Warehouse Price"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    
                                                                  
                                                                       <asp:TemplateField ShowHeader="true" HeaderText="Effective Date" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEffectiveDateCheck" runat="server" Text='<%# Eval("EffectiveDateCheck") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="SDPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="SD Price"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MDPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="MD Price"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RetailerPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Price"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MOP" HeaderStyle-HorizontalAlign="Left" HeaderText="MOP"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MRP" HeaderStyle-HorizontalAlign="Left" HeaderText="MRP"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" /></asp:BoundField>
                                                                          <asp:TemplateField ShowHeader="true" HeaderText="Effective Date" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Eval("EffectiveDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    
                                                                          <asp:BoundField DataField="ValidTill" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid Till"
                                                                        HtmlEncode="true">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="85px">
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                        <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnActiveDeactive" OnClick="btnActiveDeactive_Click" runat="server"
                                                                                CommandArgument='<%#Eval("PriceMasterID") %>' CommandName='<%#Eval("Status")%>'
                                                                                ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' Visible="false" />
                                                                         <asp:ImageButton ID="btnDelete" OnClick="btnDelete_Click" runat="server"
                                                                                CommandArgument='<%#Eval("PriceMasterID") %>' CommandName='<%#Eval("Status")%>'
                                                                                ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>' ToolTip="Delete" />       
                                                                                </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <AlternatingRowStyle CssClass="gridrow1" />
                                                                <PagerStyle CssClass="PagerStyle" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </contenttemplate>
                                                <%--<triggers>
                                                    <asp:PostBackTrigger ControlID="ExportToExcel" />
                                                </triggers>--%>
    <%-- </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                    #CC02 Comment End --%>

    <%-- #CC02 Add Start --%>

    <div runat="server" id="divgrd" style="display: none;">
        <div class="mainheading">
            Price List
        </div>
        <div class="export">
            <asp:Button ID="ExportToExcel2" runat="server" CssClass="excel" Text="" OnClick="ExportToExcel2_Click" />
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="GridPrice" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" EmptyDataText="No Record Found"
                    GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                    AlternatingRowStyle-CssClass="Altrow">
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <Columns>
                        <asp:BoundField DataField="PriceListName" HeaderStyle-HorizontalAlign="Left" HeaderText="PriceList Name"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WHPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="Warehouse Price"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SDPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="SD Price"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MDPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="MD Price"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RetailerPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Price"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MOP" HeaderStyle-HorizontalAlign="Left" HeaderText="MOP"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MRP" HeaderStyle-HorizontalAlign="Left" HeaderText="MRP"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EffectiveDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Effective Date"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ValidTill" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid Till"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle CssClass="gridheader" />
                    <EditRowStyle CssClass="editrow" />
                    <AlternatingRowStyle CssClass="Altrow" />
                </asp:GridView>
            </div>
            <div id="dvFooter" visible="true" runat="server">
                <div class="pager">
                    <ul>
                        <li class="block"><a id="hypfirst" onclick="javascript:bindgridingwithIndex(this);">&laquo; First</a></li>
                        <li class="block"><a id="hypPrev" onclick="javascript:bindgridingwithIndex(this);">&#8249;  Prev</a></li>
                        <li class="range">page&nbsp;<span id="spnpgefrom"></span>&nbsp; of &nbsp;<span id="spnto">6</span></li>
                        <li class="record">Record(s) found : <span id="spntotal"></span></li>
                        <li class="block"><a id="hypNext" onclick="javascript:bindgridingwithIndex(this);">Next  &#8250;</a></li>
                        <li class="block"><a id="hypLast" onclick="javascript:bindgridingwithIndex(this);">Last   &raquo;</a></li>
                        <li class="range">Jump to page
                        </li>
                        <li class="range">
                            <input id="txtpage" value="" class="formfields-W70" type="text" /></li>
                        <li>
                            <input type="button" class="btn page" id="btngo" value="GO" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>

    </div>

    <%-- #CC02 Add End--%>
</asp:Content>
