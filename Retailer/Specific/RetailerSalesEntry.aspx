<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="RetailerSalesEntry.aspx.cs"
    Inherits="Retailer_Specific_RetailerSalesEntry" MasterPageFile="~/CommonMasterPages/MasterPage.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Header.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/PartLookupClientSideOpeningStock.ascx" TagName="PartLookupClientSide"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />

    <script type="text/javascript" language="javascript" src="../../Assets/Jscript/jquery.js"></script>

    <script type="text/javascript" language="javascript" src="../../Assets/Jscript/jquery.dataTables.js"></script>

    <%--    <style type="text/css">
        .dataTables_wrapper {
            border: none;
            font-size: 11px;
        }
    </style>--%>

    <script type="text/javascript">

        var giCount = 1;
        var oTable;
        var giRedraw = false;


        function OnError(result) {
            alert("Error: " + result.get_message());
        }


        function VaidateIEMIServer() {
            var s = document.getElementById('<%= txtIMEI.ClientID %>');
            var txtIEMI = s.value;
            var RetailerID = document.getElementById('ctl00_contentHolderMain_hdnRetailerID').value;
            CommonService.ValidateIEMIByRetailerID(txtIEMI, RetailerID, OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {
                if (result == "0") {
                    document.getElementById('ctl00_contentHolderMain_txtSkuName').value = "";
                    alert("Invalid IMEI(" + txtIEMI + ") entered.");
                    s.value = "";
                }
                else {
                    document.getElementById('ctl00_contentHolderMain_txtSkuName').value = result;
                }
            });
        }
        function VaidateIEMI(s) {
            var txtIEMI = s.value;
            var RetailerID = document.getElementById('ctl00_contentHolderMain_hdnRetailerID').value;
            CommonService.ValidateIEMIByRetailerID(txtIEMI, RetailerID, OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {
                if (result == "0") {
                    document.getElementById('ctl00_contentHolderMain_txtSkuName').value = "";
                    alert("Invalid IMEI(" + txtIEMI + ") entered.");
                    s.value = "";
                }
                else {
                    document.getElementById('ctl00_contentHolderMain_txtSkuName').value = result;
                }
            });
        }

        function VaidateBatchcode(s) {
            var txtBatchcode = s.value;
            var RetailerID = document.getElementById('ctl00_contentHolderMain_hdnRetailerID').value;
            CommonService.ValidateBatchByRetailerID(txtBatchcode, RetailerID, OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {
                if (result == "0") {
                    document.getElementById('ctl00_contentHolderMain_txtSkuName').value = "";
                    alert("Invalid Batchcode(" + txtBatchcode + ") entered.");
                    s.value = "";
                }
                else {
                    document.getElementById('ctl00_contentHolderMain_txtSkuName').value = result;
                }
            });
        }


        var cntDetail = 0;


        function VisibleFirstNameMsg() {
            var mob = document.getElementById('ctl00_contentHolderMain_txtMobileNo');
            var fname = document.getElementById('ctl00_contentHolderMain_txtFirstName');
            var txtMiddleName = document.getElementById('ctl00_contentHolderMain_txtMiddleName');
            var txtLastName = document.getElementById('ctl00_contentHolderMain_txtLastName');
            var spnFirstName = document.getElementById('spnFirstName');
            var spnMobileNo = document.getElementById('spnMobileNo');

            if (fname.value != "" && mob.value != "" && txtLastName.value != "" & txtMiddleName.value != "") {
                spnMobileNo.style.display = 'none';
                spnFirstName.style.display = 'none';
            }
            else {
                if (fname.value == "" && mob.value != "") {
                    spnFirstName.style.display = '';
                }
                else {
                    if (fname.value == "" && (mob.value != "" | txtLastName.value != "" | txtMiddleName.value != ""))
                        spnFirstName.style.display = '';
                    else
                        spnFirstName.style.display = 'none';
                }
                if (mob.value == "" & fname.value != "")
                    spnMobileNo.style.display = '';
                else {
                    spnMobileNo.style.display = 'none';
                }
            }

        }





        function ValidateFirstName(s, e) {
            var fname = document.getElementById('ctl00_contentHolderMain_txtFirstName')
            var mob = document.getElementById('ctl00_contentHolderMain_txtMobileNo');
            if (s.innerHTML == "") {
                e.IsValid = false;
            }
            else {
                if (mob.value == "" & s.innerHTML != "") {
                    e.IsValid = false;
                }
                else {
                    e.IsValid = true;
                }
            }
            VisibleFirstNameMsg();


        }
        function ValidateMobile(s, e) {
            var mob = document.getElementById('ctl00_contentHolderMain_txtMobileNo');
            var firstName = document.getElementById('ctl00_contentHolderMain_txtFirstName');
            if (s.innerHTML == "") {
                e.IsValid = false;
            }
            else {
                if (firstName.value == "" & s.innerHTML != "") {
                    e.IsValid = false;
                }
                else {
                    e.IsValid = true;
                }
            }
            VisibleFirstNameMsg();
        }

        function ValidateMiddleName(s, e) {
            var spnFirstName = document.getElementById('spnFirstName');
            var firstName = document.getElementById('ctl00_contentHolderMain_txtFirstName');
            if (s.innerHTML == "") {
                e.IsValid = false;
            }
            else {
                if (firstName.value == "") {
                    e.IsValid = false;
                }
                else {
                    e.IsValid = true;
                }
            }
            VisibleFirstNameMsg();
        }

        $(document).ready(function () {
            /* Add a click handler to the rows - this could be used as a callback */
            $("#dtSalesEntry tbody").click(function (event) {
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
            //oTable = $('#dtSalesEntry').dataTable();
        });

        $(document).ready(function () {
            //  $('#dtSalesEntry').dataTable();
            oTable = $('#dtSalesEntry').dataTable({ "sScrollX": "100%", "bScrollCollapse": true });
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

        function ValidateISPActivationDate() {


            
            var hdnActivationDate = document.getElementById("<%= hdnActivationDate.ClientID %>").value;
            var hdnDeActivationDate = document.getElementById("<%= hdnDeActivationDate.ClientID %>").value;

            var listID = document.getElementById("<%= ddlISPByRetailer.ClientID %>");
            var strActivationDate = listID.options[listID.selectedIndex].getAttribute('ActivationDate');
            var strDeActivationDate = listID.options[listID.selectedIndex].getAttribute('DeActivationDate');

            if (listID.value != "0") {
                var strSelectedText = listID.options[listID.selectedIndex].text;

                var strSplitValue = strSelectedText.split('|');

                var StrDates = strSplitValue[1].split('-');
            }
            else {
                if (hdnActivationDate != "")
                    StrDates = hdnActivationDate;
            }


            if (hdnActivationDate != "")
                strActivationDate = hdnActivationDate;


            if (hdnDeActivationDate != "")
                strDeActivationDate = hdnDeActivationDate;


            var SalesDateSet = document.getElementById('ctl00_contentHolderMain_ucDtPickerSalesDate_txtDate').value;
            var vInputFormat = SalesDateSet.split('/')

            var vActFormat = StrDates[0].split('/');
            var vDecFromat = StrDates[1].split('/');

            var sDateSet = SalesDateSet;
            var sDateAct = vActFormat[1] + "/" + vActFormat[0] + "/" + vActFormat[2];
            var sDateDeAct = vDecFromat[1] + "/" + vDecFromat[0] + "/" + vDecFromat[2];





            var svISPDate = listID.options[listID.selectedIndex].value;

            var ReqISPDate = document.getElementById('<%= reqISP.ClientID %>');


            var ComInputDate = new Date(Date.parse(sDateSet, "MM/dd/YYYY"));
            var ComActDate = new Date(Date.parse(sDateAct, "MM/dd/YYYY"));
            var ComDecDate = new Date(Date.parse(sDateDeAct, "MM/dd/YYYY"));

            VisibleFirstNameMsg();
            return true;

            /*if (listID.all.length > 1 && svISPDate == "0") {
                alert('Please select ISP Name.');
                return false;
            }
            else {
                if (ComInputDate < ComActDate | ComInputDate > ComDecDate) {
                    alert(sDateSet + " Sales date should be between " + sDateAct + " - " + sDateDeAct + ".");
                    return false;
                }
                else {
                    VisibleFirstNameMsg();
                    return true;
                }
            }*/

        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnActivationDate" runat="server" />
    <asp:HiddenField ID="hdnDeActivationDate" runat="server" />
    <div class="container">
        <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
                <input type="hidden" runat="server" id="hdnRetailerID" name="hdnRetailerID" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--    <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>

        <div class="mainheading">
            Enter Sales Entry
        </div>
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory
            </div>
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Select Retailer: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:UpdatePanel ID="updtPnlRetailer" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="formselect" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlRetailer_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlRetailer" CssClass="error"
                                            ValidationGroup="grppr" InitialValue="0" runat="server" ErrorMessage="Please select retailer."></asp:RequiredFieldValidator>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </li>
                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnProceed" runat="server" Text="Proceed" CssClass="buttonbg" OnClick="btnProceed_Click"
                                ValidationGroup="grppr" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" CausesValidation="false"
                                OnClick="btnReset_Click" />
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="text-field">
                        <asp:Label ID="lblMsg" runat="server" CssClass="error" Text="There is no Retailer for Opening Stock Entry."
                            Visible="False"></asp:Label>
                    </li>
                </ul>
            </div>
        </div>
        <div>
            <div id="tblGridPanel" runat="server">

                <div class="mainheading">
                    List
                </div>
                <div class="contentbox">
                    <div>
                        <div class="float-right">
                            <div class="float-margin">
                                View Sales:
                            </div>
                            <div class="float-left">
                                <asp:DropDownList ID="ddlSearchDate" runat="server" CssClass="formselect" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlSearchDate_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <div>
                        <table cellpadding="0" class="display" cellspacing="0" border="0" id="dtSalesEntry">
                            <thead>
                                <tr>
                                    <th>Sales Date <span class="error">*</span>
                                    </th>
                                    <th>IMEI <%--<span class="error">*</span>--%>
                                    </th>
                                    <th>Batch Code <%--<span class="error">*</span>--%>
                                    </th>
                                    <th>SKU Name
                                    </th>
                                    <th>Quantity <%--<span class="error">*</span>--%>
                                    </th>
                                    <th>First Name
                                    </th>
                                    <th>Middle Name
                                    </th>
                                    <th>Last Name
                                    </th>
                                    <th>Mobile No
                                    </th>
                                    <th>ISP Name
                                    </th>
                                    <th>Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal runat="server" ID="ltData" EnableViewState="true"></asp:Literal>
                            </tbody>
                            <tr class="even">
                                <td align="left" valign="top">
                                    <div style="width: 120px">
                                        <uc2:ucDatePicker ID="ucDtPickerSalesDate" ErrorMessage="Enter sales date." runat="server"
                                            ValidationGroup="insert" />
                                    </div>
                                </td>
                                <td align="left" valign="top"><%--onchange="VaidateIEMI(this);"--%>
                                    <asp:TextBox ID="txtIMEI" runat="server" CssClass="formfields" Width="150px"
                                        ValidationGroup="insert" MaxLength="17"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator runat="server" ID="reqIMEI" ControlToValidate="txtIMEI" Display="Dynamic"
                                        ErrorMessage="Enter IMEI." ValidationGroup="" CssClass="error" />--%>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderModel5" runat="server" CompletionListCssClass="wordWheel listMain .box"
                                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                                        MinimumPrefixLength="3" ServiceMethod="GetIMEIListByRetailer" ServicePath="~/CommonService.asmx"
                                        TargetControlID="txtIMEI" UseContextKey="true">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtBatchcode" runat="server" onchange="VaidateBatchcode(this);" CssClass="formfields" Width="150px"
                                        ValidationGroup="insert" MaxLength="17"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator runat="server" ID="reqBatchcode" ControlToValidate="txtBatchcode" Display="Dynamic"
                                        ErrorMessage="Enter Batchcode." ValidationGroup="insert" CssClass="error" />--%>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                                        MinimumPrefixLength="3" ServiceMethod="GetBatchListByRetailer" ServicePath="~/CommonService.asmx"
                                        TargetControlID="txtBatchcode" UseContextKey="true">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtSkuName" Enabled="true" runat="server" CssClass="formfields"
                                        ValidationGroup="insert" MaxLength="50"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteSkuName" runat="server" CompletionListCssClass="wordWheel listMain .box"
                                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                                        MinimumPrefixLength="3" ServiceMethod="GetSKUNameList" ServicePath="~/CommonService.asmx"
                                        TargetControlID="txtSkuName" UseContextKey="true">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="formfields"
                                        ValidationGroup="insert" MaxLength="5"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtQuantity" Display="Dynamic"
                                        ErrorMessage="Enter Quantity." ValidationGroup="insert" CssClass="error" />--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="txtQuantity"
                                        ErrorMessage="Only numeric allowed." ForeColor="Red"
                                        ValidationExpression="^[0-9]*$" ValidationGroup="insert">
                                    </asp:RegularExpressionValidator>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="formfields" ValidationGroup="insert"
                                        MaxLength="50" onchange="VisibleFirstNameMsg();"></asp:TextBox>

                                    <span id="spnFirstName" class="error" style="display: none;">Enter first name.</span>
                                    <asp:CustomValidator ID="cValMobile" runat="server" Display="Dynamic" ValidationGroup="insert"
                                        ControlToValidate="txtMobileNo" ClientValidationFunction="ValidateMobile" CssClass="error"
                                        ErrorMessage="&nbsp;"></asp:CustomValidator>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMiddleName" runat="server" CssClass="formfields" ValidationGroup="insert"
                                        onchange="VisibleFirstNameMsg();"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic" ValidationGroup="insert"
                                        ControlToValidate="txtMiddleName" ClientValidationFunction="ValidateMiddleName"
                                        CssClass="error" ErrorMessage="&nbsp;"></asp:CustomValidator>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="formfields" ValidationGroup="insert"
                                        onchange="VisibleFirstNameMsg();"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" Display="Dynamic" ValidationGroup="insert"
                                        ControlToValidate="txtLastName" ClientValidationFunction="ValidateMiddleName"
                                        CssClass="error" ErrorMessage="&nbsp;"></asp:CustomValidator>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMobileNo" runat="server" CssClass="formfields" MaxLength="10"
                                        onchange="VisibleFirstNameMsg();"></asp:TextBox>
                                    <asp:CustomValidator ID="cValFirstName" runat="server" Display="Dynamic" ValidationGroup="insert"
                                        ControlToValidate="txtFirstName" ClientValidationFunction="ValidateFirstName"
                                        CssClass="error" ErrorMessage="&nbsp;"></asp:CustomValidator>
                                    <asp:RegularExpressionValidator ID="regExpMobile" runat="server" ControlToValidate="txtMobileNo"
                                        ErrorMessage="Invalid length" Display="Dynamic" ValidationExpression="^[0-9]{10,10}$"
                                        ValidationGroup="insert" CssClass="error"></asp:RegularExpressionValidator>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtMobileNo"
                                        ValidChars="0123456789">
                                    </cc1:FilteredTextBoxExtender>
                                    <span id="spnMobileNo" class="error" style="display: none;">Enter mobile no.</span>
                                </td>
                                <td align="left" valign="top">
                                    <asp:DropDownList ID="ddlISPByRetailer" runat="server" CssClass="formselect" onchange="ValidateISPActivationDate();">
                                        <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:RequiredFieldValidator runat="server" ID="reqISP" ControlToValidate="ddlISPByRetailer"
                                        ErrorMessage="Select ISP." ValidationGroup="insert" InitialValue="0" CssClass="error" />
                                </td>
                                <td align="left" valign="top">
                                    <asp:Button ID="btnInsert" Text="Add" runat="server" ToolTip="Add" CssClass="buttonbg"
                                        ValidationGroup="insert" OnClick="btnInsert_Click" OnClientClick="return ValidateISPActivationDate(); "></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

            </div>
            <div class="clear">
            </div>
            <table id="td" runat="server" width="100%">
                <tr>
                    <td></td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>
