<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageRetailer.aspx.cs" Inherits="Masters_HO_Retailer_ManageRetailer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dx" %>
<%--#CC30 Added--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script type="text/javascript">
        function popup() {
            WinSearchChannelCode = dhtmlmodal.open("SearchParentRetailerInfo", "iframe", "SearchParentRetailerInfo.aspx", "Retailer Detail", "width=790px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
            WinSearchChannelCode.onclose = function () {
                return true;
            }
            return false;
        }
<%-- #CC54 Add Start --%>
        function popup(RetailerID) {
            WinSearchChannelCode = dhtmlmodal.open("SearchParentRetailerInfo", "iframe", "SearchParentRetailerInfo.aspx?RetID="+RetailerID, "Retailer Detail", "width=790px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
            WinSearchChannelCode.onclose = function () {
                return true;
            }
            return false;
        }
<%-- #CC54 Add End --%>

        function UpdateBankDetailsCheck(IsUpdateCheck) {
            if (IsUpdateCheck.checked) {
                document.getElementById('ctl00_contentHolderMain_txtBankName').disabled = false;
                document.getElementById('ctl00_contentHolderMain_txtAccountHolder').disabled = false;
                document.getElementById('ctl00_contentHolderMain_txtBankAccountNumber').disabled = false;
                document.getElementById('ctl00_contentHolderMain_txtBranchLocation').disabled = false;
                document.getElementById('ctl00_contentHolderMain_txtIfscCode').disabled = false;
                document.getElementById('ctl00_contentHolderMain_txtPANNo').disabled = false;  <%-- #CC10 Added  --%>
            }
            else {
                document.getElementById('ctl00_contentHolderMain_txtBankName').disabled = true;
                document.getElementById('ctl00_contentHolderMain_txtAccountHolder').disabled = true;
                document.getElementById('ctl00_contentHolderMain_txtBankAccountNumber').disabled = true;
                document.getElementById('ctl00_contentHolderMain_txtBranchLocation').disabled = true;
                document.getElementById('ctl00_contentHolderMain_txtIfscCode').disabled = true;
                document.getElementById('ctl00_contentHolderMain_txtPANNo').disabled = true; <%-- #CC10 Added  --%>
            }
        }

        function UpdateWhatsappNumber(IsUpdateCheck) {
            if (IsUpdateCheck.checked) {
                document.getElementById('ctl00_contentHolderMain_txtWhatsNumber').disabled = false;

            }
            else {
                document.getElementById('ctl00_contentHolderMain_txtWhatsNumber').disabled = true;

            }
        }

        function UpdateRetailerCheckImage(IsUpdateCheck) {
            debugger;
            if (IsUpdateCheck.checked) {
                //document.getElementById('ctl00_contentHolderMain_UploadControl_UploadIframe').disabled = false;


            }
            else {
                document.getElementById('ctl00_contentHolderMain_UploadControl_UploadIframe').disabled = true;

            }
        }
        /*#CC28 START ADDED */
        function allowAlphaNumaric(inputtxt, showMsg) {
            var textnumber = inputtxt.value.trim();
            var i = inputtxt.value.trim().length, j = 0, totalNumber = 0;
            var numbers = /^[0-9 ]+$/;
            while (i > 0) {
                if (textnumber.charAt(j).match(numbers)) {
                    totalNumber = totalNumber + 1;
                }
                i = i - 1;
                j = j + 1


            }
            if (totalNumber == inputtxt.value.trim().length && inputtxt.value.length > 0) {
                if (showMsg == 1) {
                    document.getElementById("divContactPerson").style.display = 'block';
                    document.getElementById("divContactPerson").innerHTML = 'Only number is not accepted.';

                }
                else if (
                    showMsg == 2) {
                    document.getElementById("divRetailerName").style.display = 'block';
                    document.getElementById("divRetailerName").innerHTML = 'Only number is not accepted.';
                }
                inputtxt.select();
            }
            else {
                if (showMsg == 1) {
                    document.getElementById("divContactPerson").style.display = 'none';
                    document.getElementById("divContactPerson").innerHTML = '';
                }
                else if (
                  showMsg == 2) {
                    document.getElementById("divRetailerName").style.display = 'none';
                    document.getElementById("divRetailerName").innerHTML = '';
                }
            }
        }  /*#CC28 END ADDED */
    </script>


    <script type="text/javascript">/*#CC01*/
        // <![CDATA[
        var fieldSeparator = "|";

        function FileUploadStart() {
            debugger;
        }


        /*#CC01*/ function FileUploaded(s, e) {
            debugger;
            if (e.isValid) {

                var container = document.getElementById("uploadedListFiles");
                var linkFile = document.createElement("a");
                var indexSeparator = e.callbackData.indexOf(fieldSeparator);
                var fileName = e.callbackData.substring(0, indexSeparator);
                var pictureUrl = e.callbackData.substring(indexSeparator + fieldSeparator.length);
                var imgSrc = pictureUrl;
                //fileName = fileName + "-" + docType;
                fileName = fileName;
                linkFile.innerHTML = fileName;
                linkFile.setAttribute("href", imgSrc);
                linkFile.setAttribute("target", "_blank");
                container.appendChild(linkFile);
                container.appendChild(document.createElement("br"));
            }

        }
        /*#CC01*/  function upload() {
            var btn = document.getElementById("ctl00_contentHolderMain_btnviewdocument");
            if (btn != null) {
                __doPostBack(btn.name, "OnClick");

            }
        }
    </script>
    <script type="text/javascript">/*#CC01*/
        // <![CDATA[
        var fieldSeparator1 = "|";
        function FileUploadedshopVisitingstart() {
            debugger;
            alert("FileUploadedshopVisitingstart");
        }
        function test(s, e) {
            alert("test");
        }
        function test1() {
            alert("Test1");
        }

        /*#CC01*/ function FileUploadedshopVisiting(s, e) {
            debugger;
            if (e.isValid) {

                var container1 = document.getElementById("uploadedListFiles1");
                var linkFile1 = document.createElement("b");
                var indexSeparator1 = e.callbackData.indexOf(fieldSeparator1);
                var fileName1 = e.callbackData.substring(0, indexSeparator1);
                var pictureUrl1 = e.callbackData.substring(indexSeparator + fieldSeparator1.length);
                var imgSrc1 = pictureUrl1;

                fileName1 = fileName1;
                linkFile1.innerHTML = fileName1;
                linkFile1.setAttribute("href", imgSrc1);
                linkFile1.setAttribute("target", "_blank");
                container1.appendChild(linkFile1);
                container1.appendChild(document.createElement("br"));
            }

        }
        /*#CC01*/  function upload() {
            var btn = document.getElementById("ctl00_contentHolderMain_btnviewdocument");
            if (btn != null) {
                __doPostBack(btn.name, "OnClick");

            }
        }


        // ]]> 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="mainheading">
                Manage Retailer
                                                                <%-- <asp:Label ID="lblHeading" runat="server" Text="Mange Retailer"></asp:Label>--%>
            </div>
            <div class="export">
                <asp:LinkButton ID="LBViewRetailer" runat="server" CausesValidation="False" OnClick="LBViewRetailer_Click"
                    CssClass="elink7">View List</asp:LinkButton>
            </div>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="H30-C3-S">
                    <ul>
                        <li class="text">Select Mode:<span class="error">*</span> </li>
                        <li class="field">
                            <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CssClass="radio-rs"
                                CellPadding="2" CellSpacing="0" AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                                <%-- <asp:ListItem Value="1" Text="Excel Template"></asp:ListItem>
                                                                                <asp:ListItem Value="2" Text="Interface" Selected="True"></asp:ListItem>--%>
                            </asp:RadioButtonList>
                        </li>
                    </ul>
                    <div class="clear">
                    </div>
                    <ul>
                        <li class="text">Retailer Type:<span class="error">*</span> </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbRetailerType" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbRetailerType_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Add">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="cmbRetailerType"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please select retailer type."
                                    InitialValue="0" SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <%--CC31 Commented--%> <%-- <li class="text2">ISP/ISD on Counter: <font class="error"></font></li>--%>
                        <li class="text"><%--CSA on Counter: <font class="error"></font>--%>
                            <asp:Label runat="server" Text="<%$ Resources:GlobalChangeLabel, CSAName %>"></asp:Label>
                            : <span class="error"></span>
                        </li>
                        <li class="field">
                            <asp:RadioButtonList ID="rblCounterIsp" runat="server" RepeatDirection="Horizontal" CssClass="radio-rs"
                                CellPadding="2" CellSpacing="0">
                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="0" Text="No" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                            <%-- <asp:CheckBox ID="chkIsp" runat="server" Checked="false" />--%>
                        </li>
                        <li class="text" id="liSaleschannelHeading" runat="server">Sales Channel:<span class="error" id="saleschannelMandatorySign" runat="server">*</span> </li>
                        <%--#CC33 display:none Added--%>  <%-- #CC34 id and runat Added , display:none removed --%>
                        <li class="field" id="liSaleschannelddl" runat="server"><%--#CC33 display:none Added--%> <%-- #CC34 id and runat Added , display:none removed --%>
                            <div>
                                <asp:DropDownList ID="cmbsaleschannel" runat="server" ValidationGroup="Add" CssClass="formselect" AutoPostBack="true"
                                    OnSelectedIndexChanged="cmbsaleschannel_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="cmbsaleschannel"
                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel."
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Is Child:<span class="error">*</span> </li>
                        <li class="field">
                            <asp:RadioButtonList ID="rdoChild" runat="server" CssClass="radio-rs" OnSelectedIndexChanged="rdoChild_SelectedIndexChanged"
                                CellPadding="2" CellSpacing="0" AutoPostBack="true" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Text="No" Value="0"> </asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblParentRetailer" runat="server" Text="Parent Retailer:" Visible="false"> </asp:Label>
                            <span class="error"></span></li>
                        <li class="field">
                            <div>
                                 <asp:HiddenField ID="hdnID" runat="server" />
                                <%--<asp:Label ID="hdnID" runat="server" ></asp:Label>--%>
                                <asp:HiddenField ID="hdnName" runat="server" />
                               <%--  <asp:Label ID="hdnName" runat="server" ></asp:Label>--%>
                                <%--    <asp:DropDownList ID="ddlParentRetailer" CssClass="form_select" runat="server" Visible="false"
                                                                                    ValidationGroup="Add">
                                                                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txtSearchedName" runat="server" CssClass="formfields" MaxLength="60"
                                    Enabled="false" Visible="false" ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="ReqParent" runat="server" ControlToValidate="txtSearchedName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Select Parent Retailer."
                                    SetFocusOnError="true" Visible="false"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="field3">
                            <asp:Button ID="btnsearch" runat="server" Text="Search Parent Retailer" CssClass="buttonbg"
                                Visible="false" />
                        </li>
                    </ul>
                    <div class="clear">
                    </div>
                    <ul runat="server" id="tdLabel">
                        <li class="text">Reporting Hierarchy Name:<span class="error" id="OrgnHierarchayMandatorySign" runat="server">*</span> </li>
                        <%--#CC34 id and runat added--%>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlOrghierarchy" runat="server" CssClass="formselect" ValidationGroup="Add">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="reqOrgnhierarchy" runat="server" ControlToValidate="ddlOrghierarchy"
                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please Select Orgn. Hierarchy Name."
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                    </ul>
                    <%--     <td valign="top" class="formtext"  colspan="2">
                                                                           <div style="float:left; text-align:right;width:150px;">
                                                                              Reporting Hierarchy Name:<font class="error">*</font>
                                                                              </div>
                                                                               <div style="float:left;  text-align:left;width:150px; padding-left:10px;">
                                                                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form_select" ValidationGroup="Add">
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlOrghierarchy"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please Select Orgn. Hierarchy Name."
                                                                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                           </td>--%>
                    <ul>
                        <div id="tdsalesman" runat="server">
                            <li class="text">FOS/Salesman:<span class="error"> <span runat="server" id="divRetailerMandatory">*</span> </span></li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlSalesman" runat="server" CssClass="formselect" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlSalesman"
                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select salesman."
                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                        </div>
                        <li class="text">Contact Person:<span class="error">*</span> </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtcontactperson" runat="server" CssClass="formfields" MaxLength="60" onblur="return allowAlphaNumaric(this,1);"></asp:TextBox>
                                <%--#CC28  ADDED onblur="return allowAlphaNumaric(this,1); function--%>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtcontactperson"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter contact person name."
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>

                            </div>
                            <div class="error" id="divContactPerson">
                            </div>
                        </li>
                        <li class="text">Retailer Code:<span class="error">*</span> </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtretailercode" runat="server" CssClass="formfields" MaxLength="20"
                                    Enabled="false"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="rqRetailerCode" runat="server" ControlToValidate="txtretailercode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Retailer Code."
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Retailer Name:<span class="error">*</span> </li>
                        <li class="field">
                            <asp:TextBox ID="txtretailername" runat="server" CssClass="formfields" MaxLength="90" onblur="return allowAlphaNumaric(this,2);"></asp:TextBox>
                            <%--#CC28  ADDED onblur="return allowAlphaNumaric(this,1); function--%>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtretailername"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Retailer name."
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtretailername"
                                    FilterMode="InvalidChars" InvalidChars="[,%,]">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                            <div>
                                <div class="error" id="divRetailerName">
                                </div>
                            </div>
                        </li>
                        <li class="text">Country:<span class="error">*</span> </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbCountry" runat="server" AutoPostBack="true" CssClass="formselect"
                                    OnSelectedIndexChanged="cmbCountry_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="cmbCountry"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please select Country." InitialValue="0"
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">State:<span class="error">*</span> </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbstate" runat="server" AutoPostBack="true" CssClass="formselect"
                                    OnSelectedIndexChanged="cmbstate_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="cmbstate"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please select state." InitialValue="0"
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                    </ul>
                    <%-- #CC21 Comment Start <ul id="tdTehsil" style="display: none" runat="server">
                                                                        <li class="text2">Tehsil:<span class="error">*</span> </li>
                                                                        <li class="field">
                                                                            <div style="width: 135px;">
                                                                                <asp:DropDownList ID="cmbTehsil" runat="server" AutoPostBack="true" CssClass="form_select"
                                                                                    OnSelectedIndexChanged="cmbTehsil_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div style="width: 140px;">
                                                                                <asp:RequiredFieldValidator ID="rfvTehsil" runat="server" ControlToValidate="cmbTehsil"
                                                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please select tehsil." InitialValue="0"
                                                                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </li>
                                                                    </ul> #CC21 Comment End --%>
                    <ul>
                        <li class="text">City:<span class="error">*</span> </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbcity" runat="server" AutoPostBack="true" CssClass="formselect"
                                    OnSelectedIndexChanged="cmbcity_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="req" runat="server" ControlToValidate="cmbcity" CssClass="error"
                                    Display="Dynamic" ErrorMessage="Please select city" InitialValue="0" SetFocusOnError="true"
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <%--#CC21 Add Start--%>
                    </ul>
                    <ul>
                        <div id="tdTehsil" style="display: none" runat="server">
                            <li class="text">Tehsil:<span class="error">*</span> </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="cmbTehsil" runat="server" AutoPostBack="true" CssClass="formselect"
                                        OnSelectedIndexChanged="cmbTehsil_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvTehsil" runat="server" ControlToValidate="cmbTehsil"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select tehsil." InitialValue="0"
                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                </div>
                            </li>

                        </div>
                    </ul>
                    <ul>
                        <%--#CC21 Add End--%>
                        <li class="text">Area:<span class="error" id="dvAreaMandotery" runat="server">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbArea" runat="server" CssClass="formselect">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <%--#CC02 START ADDED--%>
                                <asp:RequiredFieldValidator ID="RequiredArea" runat="server" ControlToValidate="cmbArea"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please select area" InitialValue="0"
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                            <%--#CC02 START END--%>
                        </li>
                    </ul>
                    <div class="clear">
                    </div>
                    <ul>
                        <li class="text" style="height: 40px">Address Line 1:<span class="error">*</span> </li>
                        <li class="field" style="height: 40px"><%--#CC47 style changed--%>
                            <div>
                                <asp:TextBox ID="txtAddress1" runat="server" CssClass="form_textarea" TextMode="MultiLine"
                                    ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <div>
                                <%-- #CC31 Added Started--%>
                                <%--<asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "txtAddress1" ID="RequiredFieldValidator1" ValidationExpression = "^[a-zA-Z0-9\s]{25,250}$" runat="server" ErrorMessage="Minimum 25 Characters and Maximum 250 characters required."></asp:RegularExpressionValidator>--%>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ControlToValidate="txtAddress1"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter address1." ForeColor=""
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="fncChkSize"
                                    ControlToValidate="txtAddress1" CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Address1."
                                    ForeColor="" ValidationGroup="Add">
                                </asp:CustomValidator>

                            </div>
                        </li>
                        <li class="text" style="height: 40px">Address Line 2:<span class="error"> </span></li>
                        <li class="field" style="height: 40px">
                            <asp:TextBox ID="txtAddress2" runat="server" CssClass="form_textarea" TextMode="MultiLine"
                                ValidationGroup="Add"></asp:TextBox>

                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="fncChkSize"
                                ControlToValidate="txtAddress2" CssClass="error" Display="Dynamic" ErrorMessage="Address2 should not be greater than 250"
                                ForeColor="" ValidationGroup="Add">
                            </asp:CustomValidator>
                        </li>
                        <li class="text" style="height: 40px">Pin Code:<span class="error">*</span></li>
                        <li class="field" style="height: 40px">
                            <div>
                                <asp:TextBox ID="txtpincode" runat="server" CssClass="formfields" MaxLength="6"
                                    ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <div>
                                <div class="float-left">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtpincode"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter pin code." ForeColor=""
                                        ValidationGroup="Add"></asp:RequiredFieldValidator>
                                </div>
                                <div class="float-left">
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtpincode"
                                        ValidChars="0123456789">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <div class="float-left">
                                    <asp:RangeValidator ID="rngpin" runat="server" ControlToValidate="txtpincode" CssClass="error"
                                        Display="Dynamic" ErrorMessage="Pin code should be Proper 6 digits" ForeColor=""
                                        MaximumValue="999999" MinimumValue="100000" SetFocusOnError="True" Type="Integer"
                                        ValidationGroup="Add"></asp:RangeValidator>
                                </div>
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">Phone No:<span class="error"> </span></li>
                        <li class="field">
                            <asp:TextBox ID="txtphone" runat="server" CssClass="formfields"></asp:TextBox>
                            <%--#CC06 Maxlength Changed from 15 to 10--%> <%--#CC25 MaxLength set from AppConfig.regex--%>

                            <%--#CC06 Comment Start --%>
                            <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtphone"
                                                                    ValidChars="0123456789+-().,/">
                                                                </cc1:FilteredTextBoxExtender>--%>
                            <%--#CC06 Comment End --%>
                            <%--#CC06 Add Start --%>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtphone"
                                ValidChars="0123456789">
                            </cc1:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ID="regexPhoneNumber" runat="server" ControlToValidate="txtphone"
                                ValidationGroup="Add"
                                CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                            <%--#CC25 Error message & RegularExpresion set by AppConfig.regex--%>
                            <%--#CC06 Add End --%>
                        </li>
                        <li class="text">Mobile No:<span class="error">*</span> </li>
                        <li class="field">
                            <asp:TextBox ID="txtmobile" runat="server" CssClass="formfields" MaxLength="10"
                                ValidationGroup="All"></asp:TextBox>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtmobile"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter mobile no." ForeColor=""
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>
                                <%--#CC06 Add Start --%>
                                <asp:RegularExpressionValidator ID="regexValidatorMobileNo" runat="server" ControlToValidate="txtmobile"
                                    ValidationExpression="^[1-9]([0-9]{9})$" ValidationGroup="Add" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                    CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                                <%--#CC06 Add Start --%>
                            </div>
                            <div>
                                <cc1:FilteredTextBoxExtender ID="txtmobile_FilteredTextBoxExtender" runat="server"
                                    TargetControlID="txtmobile" ValidChars="0123456789.">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </li>
                    </ul>
                    <%-- <ul>  #CC47--%>
                    <div id="dvWhatsNumber" style="display: none" runat="server">
                        <div id="tblUpdateWhatsNumber" runat="server" style="display: none">
                            <%--<div class="fieldarea">  #CC47--%>
                            <ul id="trUpdateWhatsNumber" runat="server">
                                <li class="text">Update Whats Number: </li>
                                <li class="field">
                                    <asp:CheckBox ID="CheckBox1" runat="server" onclick="return UpdateWhatsappNumber(this);" />
                                </li>
                            </ul>
                            <%--</div> #CC47--%>
                        </div>
                        <%-- <div class="fieldarea">  #CC47--%>
                        <ul>
                            <li class="text">Whatsapp Number:<span class="error">*</span> </li>
                            <%--#CC47--%>
                            <li class="field"><%--#CC47--%>
                                <asp:TextBox ID="txtWhatsNumber" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvWhatsNumber" runat="server" ControlToValidate="txtWhatsNumber"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter whatsapp mobile no." ForeColor=""></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RevWhatsAppNumber" runat="server" ControlToValidate="txtWhatsNumber"
                                    ValidationExpression="^[1-9]([0-9]{9})$" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                    CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                            </li>
                        </ul>
                        <%-- </div> #CC47--%>
                    </div>
                    <%--</ul> #CC47--%>
                    <ul>
                        <li class="text">Email ID:<span class="error"> </span>
                            <%--<font class="error">*</font>--%>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtemail" runat="server" CssClass="formfields" MaxLength="80" ValidationGroup="Add"></asp:TextBox>

                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtemail"
                                                                                CssClass="error" Display="Dynamic" ErrorMessage="Please enter Contact Person Email ID."
                                                                                ForeColor="" ValidationGroup="Add"></asp:RequiredFieldValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularEmail" runat="server" ControlToValidate="txtemail"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid emailid."
                                ForeColor="" meta:resourcekey="RegularEmail" SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ValidationGroup="Add"></asp:RegularExpressionValidator>
                        </li>
                        <li class="text">Date of Birth:<span class="error"> </span></li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDOB" ValidationGroup="Add" runat="server" IsRequired="false"
                                defaultDateRange="True" RangeErrorMessage="Date should be less then equal to current date." />
                        </li>
                        <li class="text">
                            <%-- Counter Size: --%>Counter Potential in Volume:<%--/*#CC36 Added Started*/ --%><span class="error" id="counterpotentialvolumnmandatsign" runat="server">*</span><%--/*#CC36 Added End*/ --%></li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtcountersize" runat="server" CssClass="formfields" MaxLength="20"
                                    ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtcountersize"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please enter counter size."
                                ForeColor="" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            <%-- <asp:RegularExpressionValidator ID="rdNumeric" runat="server"  CssClass="error" ControlToValidate="txtcountersize" ValidationGroup="Add" ValidationExpression="^\d+$" ErrorMessage="Please Enter Number only"></asp:RegularExpressionValidator>--%>
                        </li>
                    </ul>
                    <%--#CC18 Add Start--%>
                    <ul id="trPotentalValue" runat="server">
                        <li class="text">Counter Potential in Value:<span class="error" id="counterpotentialmandatsign"
                            runat="server">*</span> </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtCounterValue" runat="server" CssClass="formfields" MaxLength="10"
                                    ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rqValidateCounterValue" runat="server" ControlToValidate="txtCounterValue"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please enter counter value."
                                ForeColor="" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regCounterValue" runat="server" CssClass="error"
                                ErrorMessage="Enter numbers only." ControlToValidate="txtCounterValue" ValidationGroup="Add"
                                ValidationExpression="(^[0-9]\d*$)"></asp:RegularExpressionValidator>
                        </li>
                    </ul>
                    <%--#CC18 Add End--%>
                    <ul>
                        <li class="text">
                            <%--#CC22 Add Start --%>
                            <asp:Label ID="txtTinNoHeading" runat="server" class="text2" Text="TIN No:"></asp:Label>
                            <%--#CC22 Add End --%>
                            <%-- TIN No: #CC22 Commented --%><span class="error"> </span></li>
                        <li class="field">
                            <asp:TextBox ID="txttinno" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                        </li>
                        <%--#CC27 Add Start--%>
                        <li class="text" id="liReferanceCodeHeading" runat="server"><%--#CC30 added id and server tag in li--%>
                            <asp:Label ID="lblReferanceCode" runat="server" class="text2" Text="Reference code:"></asp:Label>
                        </li>
                        <li class="field" id="liReferanceCode" runat="server"><%--#CC30 added id and server tag in li--%>
                            <asp:TextBox ID="txtReferanceCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegexRefCode" runat="server" CssClass="error"
                                ErrorMessage="Enter alplhanumeric values only." ControlToValidate="txtReferanceCode" ValidationGroup="Add"
                                ValidationExpression="(^[a-zA-Z0-9]+$)"></asp:RegularExpressionValidator>

                        </li>

                        <%--#CC27 Add End--%>
                        <li class="text" id="liStatusHeading" runat="server"><%--#CC30 added id and server tag in li--%>
                            <div id="tdStatusLable" runat="server" style="display: none;">
                                <asp:Label ID="lblUserStatus" runat="server" AssociatedControlID="chkactive" CssClass="formtext">Status :  <span class="error"> </span></asp:Label>
                            </div>
                        </li>
                        <li class="field" id="liStatus" runat="server"><%--#CC30 added id and server tag in li--%>
                            <div id="tdStatus" runat="server" style="display: none;">
                                <asp:CheckBox ID="chkactive" runat="server" Checked="false" />
                            </div>
                            <%--   <asp:CheckBox ID="chkactive" runat="server" Checked="true" />--%>
                        </li>
                    </ul>
                    <%-----Pankaj     --%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="tblGrid">
        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">User Name:<span class="error">*</span> </li>
                                <li class="field">
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqUserName" runat="server" ControlToValidate="txtUserName"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please Select User Name." SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </li>
                                <%--    <td height="35" align="right" valign="top" class="formtext" width="14%">
                                                    Email ID:
                                                </td>
                                                <td width="20%" align="left" valign="top">
                                                    <asp:TextBox ID="txtEmailID" runat="server" CssClass="form_input2" MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqEmailID" runat="server" ControlToValidate="txtEmailID"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please Insert Email ID."
                                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </td>--%>
                            </ul>
                            <ul>
                                <li class="text" runat="server" id="tdPassword">Password:<span class="error">*</span>
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtpassword" runat="server" CssClass="formfields" MaxLength="20"
                                        TextMode="Password" ValidationGroup="Add1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqpassword" runat="server" ControlToValidate="txtpassword"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Password." SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </li>
                            </ul>
                        </div>
                        <%-- <div class="error" style="font-style:italic;">(With Zero Quantity)</div>--%>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%--   <td height="10"><asp:HiddenField ID="hdnRetailercode" runat="server" /></td>--%>
        <div>
            <asp:Label ID="hdnRetailercode" runat="server" Visible="false" />
        </div>


        <div class="contentbox" id="tblGrid">
            <div class="H25-C3-S">           
            <ul>
               <li class="text">
                        <asp:Label ID="lblSubmitOpeningStock" runat="server" Text="Opening Stock Date:"></asp:Label><span
                            class="error"></span> </li>
                    <li class="field">
                        <uc2:ucDatePicker ID="ucOpeningStock" ValidationGroup="Add" runat="server" IsRequired="True"
                            RangeErrorMessage="invalid Date" ErrorMessage="Invalid Date" />
                    </li>
                </ul>
            </div>
        </div>


        <%--#CC07 Add Start--%>
        <asp:UpdatePanel ID="UpdateBankdetails" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="contentbox" id="dvBankDetail" style="display: none" runat="server">

                    <div id="tblUpdateBankDetail" runat="server" style="display: none">
                        <div class="H25-C3-S">
                            <ul id="trUpdateBankDetail" runat="server">
                                <li class="text">Update Bank Detail: </li>
                                <li class="field">
                                    <asp:CheckBox ID="ChkBankDetail" runat="server" onclick="return UpdateBankDetailsCheck(this);" />
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="H25-C3-S">
                        <ul id="tblBankDetail" runat="server">
                            <li class="text">Name of Bank:<span class="error" id="nameofbank" runat="server">*</span><%--===#CC46 Added===--%></li>
                            <li class="field">
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                                <%--===#CC46 Added Started===--%>
                                <div>
                                    <asp:RequiredFieldValidator ID="RQFNameofBank" runat="server" ControlToValidate="txtBankName"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter bank name."
                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNameofBank" runat="server" TargetControlID="txtBankName"
                                        FilterMode="InvalidChars" InvalidChars="[,%,]">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <%--===#CC46 Added End===--%>
                            </li>
                            <li class="text">Account Holder Name:<span class="error" id="Accountholdername" runat="server">*</span><%--===#CC46 Added===--%></li>
                            <li class="field">
                                <asp:TextBox ID="txtAccountHolder" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                                <%--===#CC46 Added Started===--%>
                                <div>
                                    <asp:RequiredFieldValidator ID="RQFAccountholderName" runat="server" ControlToValidate="txtAccountHolder"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter account holder name."
                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderAccountholderName" runat="server" TargetControlID="txtAccountHolder"
                                        FilterMode="InvalidChars" InvalidChars="[,%,]">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <%--===#CC46 Added End===--%>
                            </li>
                            <li class="text">Bank Account Number:<span class="error" id="BankAccountnumber" runat="server">*</span><%--===#CC46 Added===--%></li>
                            <li class="field">
                                <asp:TextBox ID="txtBankAccountNumber" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                                <%--===#CC46 Added Started===--%>
                                <div>
                                    <asp:RequiredFieldValidator ID="RQFbankaccountnumber" runat="server" ControlToValidate="txtBankAccountNumber"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter bank account number."
                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RQFaccounternumberValue" runat="server" CssClass="error"
                                        ErrorMessage="Enter numbers only." ControlToValidate="txtBankAccountNumber" ValidationGroup="Add"
                                        ValidationExpression="(^[0-9]\d*$)"></asp:RegularExpressionValidator>
                                </div>
                                <%--===#CC46 Added End===--%>
                            </li>
                            <li class="text">Branch Location:<span class="error" id="Branchlocation" runat="server">*</span><%--===#CC46 Added===--%></li>
                            <li class="field">
                                <asp:TextBox ID="txtBranchLocation" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                                <%--===#CC46 Added Started===--%>
                                <div>
                                    <asp:RequiredFieldValidator ID="RQFBranchLocation" runat="server" ControlToValidate="txtBranchLocation"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter branch location."
                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderBranchlocation" runat="server" TargetControlID="txtBranchLocation"
                                        FilterMode="InvalidChars" InvalidChars="[,%,]">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <%--===#CC46 Added End===--%>
                            </li>
                            <li class="text">IFSC Code:<span class="error" id="IFSCcode" runat="server">*</span><%--===#CC46 Added===--%></li>
                            <li class="field">
                                <asp:TextBox ID="txtIfscCode" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                                <%--===#CC46 Added Started===--%>
                                <div>
                                    <asp:RequiredFieldValidator ID="RQFIFSCCode" runat="server" ControlToValidate="txtIfscCode"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter IFSC code."
                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderIfsccode" runat="server" TargetControlID="txtIfscCode"
                                        FilterMode="InvalidChars" InvalidChars="[,%,]">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <%--===#CC46 Added End===--%>
                            </li>
                            <%--#CC10 Add Start--%>
                            <li class="text">PAN No.<span class="error" id="Pannumber" runat="server">*</span><%--===#CC46 Added===--%></li>
                            <li class="field">
                                <asp:TextBox ID="txtPANNo" runat="server" CssClass="formfields" MaxLength="15"></asp:TextBox>
                                <%--===#CC46 Added Started===--%>
                                <div>
                                    <asp:RequiredFieldValidator ID="RQFPannumber" runat="server" ControlToValidate="txtPANNo"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter IFSC code."
                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPannumber" runat="server" TargetControlID="txtPANNo"
                                        FilterMode="InvalidChars" InvalidChars="[,%,]">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <%--===#CC46 Added End===--%>
                            </li>
                            <%--#CC10 Add End--%>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--#CC07 Add Start--%>

        <%-- <tr>
                        <td>
                           
                            <div class="contentbox" id="dvWhatsNumber" style="display: none" runat="server">
                                <div id="tblUpdateWhatsNumber" runat="server" style="display: none">
                                    <div class="fieldarea">
                                        <ul id="trUpdateWhatsNumber" runat="server">
                                            <li class="text2">Update Whats Number: </li>
                                            <li class="field">
                                                <asp:CheckBox ID="CheckBox1" runat="server" onclick="return UpdateWhatsappNumber(this);" />
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="fieldarea">
                                    <ul>
                                        <li class="text3">Whatsapp Number:<font class="error">*</font> </li>
                                        <li class="field2">
                                            <asp:TextBox ID="txtWhatsNumber" runat="server" CssClass="form_input2" MaxLength="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvWhatsNumber" runat="server" ControlToValidate="txtWhatsNumber"
                                                CssClass="error" Display="Dynamic" ErrorMessage="Please enter whatsapp mobile no." ForeColor=""></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RevWhatsAppNumber" runat="server" ControlToValidate="txtWhatsNumber"
                                                ValidationExpression="^[1-9]([0-9]{9})$" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                                CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </li>
                                    </ul>
                                </div>
                                </div>
                            
                            </td>
                        </tr>--%>


        <div class="contentbox">
            <asp:GridView ID="gvAttachedImages" runat="server" AutoGenerateColumns="false" BorderWidth="0px"
                ShowHeader="TRUE" CellPadding="0" GridLines="None" CssClass="table-panel" HeaderStyle-VerticalAlign="top"
                HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                DataKeyNames="IMAGETTYPE" Visible="false">
                <FooterStyle HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="ImageType">
                        <ItemTemplate>
                            <asp:Label ID="lblImageType" runat="server" Text='<%# Eval("IMAGETTYPE") %>'></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Image Path">
                        <ItemTemplate>

                            <asp:LinkButton ID="lnkDownload" Text="View Attachment" CssClass="elink2" CommandArgument='<%# Eval("ImageRelativePath") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingRowStyle>
            </asp:GridView>
        </div>

        <%--#CC07 Add Start--%>
        <asp:UpdatePanel ID="UpdateChequedetail" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="contentbox" id="dvRetailerCheckUpload" style="display: none" runat="server">
                    <div id="tblRetailerCheckUpload" runat="server" style="display: none">
                        <div class="fieldarea">
                            <%-- <ul id="Ul1" runat="server">
                                            <li class="text2">Update Retailer Check Image: </li>--%>
                            <%--<li class="field">
                                                <asp:CheckBox ID="CheckBox2" runat="server" onclick="return UpdateRetailerCheckImage(this);" />
                                            </li>--%>
                            <%--  </ul>--%>
                        </div>
                    </div>
                    <div class="H35-C2">
                        <ul>
                            <li class="text">Retailer cheque & PAN upload:<span class="error" id="Retailerchequevalid" runat="server">*</span><%--#CC47--%></li>
                            <%--#CC47 : START--%>
                            <li class="field" style="height: auto">
                                <asp:FileUpload ID="FileUploadRetailerCheck" CssClass="fileuploads" runat="server"  />
                                <div>
                                    <asp:Label ID="lblRetaillerchequemsg" CssClass="error" runat="server" Text="(File Size Max 2MB And File Type bmp, gif, png, jpg, jpeg)"></asp:Label>
                                </div>
                                <div style="width: 460px">
                                    <asp:Label ID="lblRetailerCheck" runat="server" Visible="false" />
                                </div>
                                <%--#CC47 : END--%>
                                <%--<asp:RequiredFieldValidator ID="REQFileUploadRetailerCheck" ErrorMessage="Please select Retailer cheque & PAN upload" ValidationGroup="Add" ControlToValidate="FileUploadRetailerCheck" runat="server" Display="Dynamic" ForeColor="Red" />--%>
                            </li>

                          <li class="field3"><%--#CC47--%>
                                <asp:Button ID="Button2" runat="server" CssClass="buttonbg" CausesValidation="false" Text="Upload" OnClick="RetailerCheck_FileUploadComplete" />
                                <%--<asp:LinkButton ID="lnkretailerchequeimage" runat="server" OnClick="lnkretailerchequeimage_Click" Text="Download Retailer Cheque & PAN Sample Image. " CausesValidation="false"></asp:LinkButton>--%><%--#CC47--%>
                            </li>
                            <%--#CC47 : START--%>
                          <li class="link">
                                <asp:LinkButton ID="lnkretailerchequeimage" CssClass="elink2" runat="server" OnClick="lnkretailerchequeimage_Click" Text="Download Retailer Cheque & PAN Sample Image. " CausesValidation="false"></asp:LinkButton>
                            </li>
                            <%--#CC47 : END--%>
                            <li class="field">
                                <%-- <asp:Panel runat="server" ID="PnlAdminAttachement">
                                <div class="uploadContainer">
                          
                            <li class="field" style="height:auto">                            
                                <dx:ASPxUploadControl ID="UploadControl" runat="server" Width="300px" ShowUploadButton="True"  AddUploadButtonsHorizontalPosition="Left"
                                    ShowProgressPanel="True" ClientInstanceName="UploadControl" OnFileUploadComplete="UploadControl_FileUploadComplete" FileInputCount="1"   
                                   ButtonStyle-Cursor="pointer"
                                    ButtonStyle-ForeColor="White" ButtonStyle-Font-Underline="False" ButtonStyle-CssClass="buttonbg" >
                                   
                                      

                                    <ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif,.png" MaxFileSize="500000">
                                    </ValidationSettings>
                                    <ButtonStyle Cursor="pointer" Font-Underline="False" ForeColor="White" CssClass="buttonbg">
                                    </ButtonStyle>
                                    <ClientSideEvents FileUploadComplete="function(s, e) { FileUploaded(s, e) }" FileUploadStart="function(s, e) { FileUploadStart(); }" />
                                     <%-- <ClientSideEvents FileUploadComplete="functionshopVisting(s, e) { FileUploadedshopVisiting(s, e) }" FileUploadStart="functionshopVisting(s, e) { FileUploadedshopVisitingstart(); }" />--%>
                                <%--<ClientSideEvents FileUploadStart="function(s, e) { dx:ASPxRoundPanel.Clear(); }"
                              FileUploadComplete="onFileUploadComplete" />--%>
                                <%--</dx:ASPxUploadControl>--%>
                                <%--<div class="note3">
                                    <b>Note</b>: The total size of files to upload is limited by 500kb.
                                </div>
                            </li>
                            <li class="text" style="height:auto"></li>
                            <li class="field" style="height:auto">
                                <dx:ASPxRoundPanel ID="ASPxRoundPanel" runat="server" Width="300px" ClientInstanceName="RoundPanel"
                                    HeaderText="Uploaded files (jpeg,gif,png)" Height="100%">
                             
                                
                                    <PanelCollection>
                                        <dx:PanelContent runat="server" ID="MM">
                                            <div id="uploadedListFiles" style="height: 65px; font-family: Arial;">
                                            </div>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxRoundPanel>
                            </li>--%>
                                <%-- </div>
                                                                     
                   
   </asp:Panel>--%>
                            </li>
                        </ul>
                    </div>
                </div>

                <div class="clear" style="height: 10px"></div>
                <div class="contentbox" id="dvShopImagesVisitingCard" style="display: none" runat="server">
                    <div id="tblShopImageVisitingCard" runat="server" style="display: none">
                        <div class="fieldarea">
                        </div>
                    </div>
                   <div class="H35-C2">
                        <ul>
                            <%--#CC47 : START--%>
                            <li class="text">Retailer Shop  Image Upload:<span class="error" id="RetailerShopImageRequired" runat="server">*</span> </li>
                            <li class="field" style="height: auto">
                                <asp:FileUpload ID="FileUploadShopImage" CssClass="fileuploads" runat="server" />
                                <div>
                                    <asp:Label ID="lblretailershopmsg" CssClass="error" runat="server" Text="(File Size Max 2MB And File Type bmp, gif, png, jpg, jpeg)"></asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="lblRetailerShop" runat="server" Visible="false" />
                                </div>
                            </li>
                            <li class="field3">
                                <asp:Button ID="btnsave" runat="server" CssClass="buttonbg" Text="Upload" CausesValidation="false" OnClick="UploadControl_RetailerShopImageuPLoad" />
                            </li>
                            <li class="link">
                                <asp:LinkButton ID="Lnkdownloadshopimage" CssClass="elink2" runat="server" OnClick="Lnkdownloadshopimage_Click" Text="Download Retailer Shop Sample Image. " CausesValidation="false"></asp:LinkButton>
                            </li>
                        </ul>
                        <div class="clear"></div>
                        <ul>
                            <li class="text">Visiting Card/Stationary Having Postal Address:<span class="error" id="VisitingCardRequired" runat="server">*</span> </li>
                            <li class="field" style="height: auto">
                                <asp:FileUpload ID="FileUploadVisiting" CssClass="fileuploads" runat="server" />
                                <div>
                                    <asp:Label ID="lblfileuploadmsg" CssClass="error" runat="server" Text="(File Size Max 2MB And File Type bmp, gif, png, jpg, jpeg)"></asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="lblVisiting" runat="server" Visible="false" />
                                </div>
                            </li>
                           <li class="field3">
                                <asp:Button ID="Button1" runat="server" CssClass="buttonbg" CausesValidation="false" Text="Upload" OnClick="UploadControl_VisitingCardUpload" />
                            </li>
                           <li class="link">
                                <asp:LinkButton ID="lnkdownloadvisitingcard" CssClass="elink2" runat="server" OnClick="lnkdownloadvisitingcard_Click" CausesValidation="false" Text="Download Visiting Card Sample Image."></asp:LinkButton>
                            </li>
                            <%--#CC47 : END--%>
                        </ul>
                    </div>
                </div>

                <%-- <li class="field2">
                            <asp:panel runat="server" id="panel1">
                                 
                                <div class="uploadcontainer">
                          
                            <li class="field" style="height:auto">                            
                                <dx:ASPxUploadControl id="UploadControl1" runat="server" width="300px" showuploadbutton="true"  adduploadbuttonshorizontalposition="left"
                                    showprogresspanel="true" clientinstancename="UploadControl1" onfileuploadcomplete="UploadControl1_FileUploadComplete"    
                                   buttonstyle-cursor="pointer"
                                    buttonstyle-forecolor="white" buttonstyle-font-underline="false" buttonstyle-cssclass="buttonbg" >
                                   
                                      

                                    <validationsettings allowedfileextensions=".jpg,.jpeg,.jpe,.gif,.png" maxfilesize="500000">
                                    </validationsettings>
                                    <buttonstyle cursor="pointer" font-underline="false" forecolor="white" cssclass="buttonbg">
                                    </buttonstyle>
                                     <ClientSideEvents fileuploadcomplete="function(s, e) { fileuploadedshopvisiting(s, e) }" fileuploadstart="function(s, e) { fileuploadedshopvisitingstart(); }" />--%>
                <%-- <clientsideevents fileuploadcomplete="function(s, e) { fileuploaded(s, e) }" fileuploadstart="function(s, e) { fileuploadstart(); }" />
                                     <ClientSideEvents fileuploadcomplete="function(s,e){Test(s,e)}" fileuploadstart="function(s, e) { test1(); }" />--%>


                <%-- </dx:ASPxUploadControl>
                                <div class="note3">
                                    <b>note</b>: the total size of files to upload is limited by 500kb.
                                </div>
                            </li>
                            <li class="text" style="height:auto"></li>
                            <li class="field" style="height:auto">
                                <dx:ASPxRoundPanel id="aspxroundpanel2" runat="server" width="300px" clientinstancename="RoundPanel1"
                                    headertext="uploaded files (jpeg,gif,png)" height="100%">
                             
                                
                                    <PanelCollection>
                                        <dx:PanelContent runat="server" ID="PanelContent1">
                                            <div id="uploadedListFiles1" style="height: 65px; font-family: Arial;">
                                            </div>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxRoundPanel>
                            </li>
                        </div>
                   
                                                                     
                   
                        </asp:panel>
                                        </li>
                                </ul>
                                </div>--%>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkretailerchequeimage" />
                <asp:PostBackTrigger ControlID="Lnkdownloadshopimage" />
                <asp:PostBackTrigger ControlID="lnkdownloadvisitingcard" />
                <asp:PostBackTrigger ControlID="Button2" />
                <asp:PostBackTrigger ControlID="btnsave" />
                <asp:PostBackTrigger ControlID="Button1" />

            </Triggers>
        </asp:UpdatePanel>

        <div id="trProceedAction" runat="server" style="display: none;">
            <div class="float-margin">

                <%--#CC14 Added --%>
                <asp:Button ID="btnProceed" runat="server" CssClass="buttonbg" CausesValidation="true"
                    ValidationGroup="Add" Text="Proceed" OnClick="btnProceed_Click" /><%--#CC05 ADDED--%>
                <%--#CC14 Add Start --%>
            </div>
            <div class="float-left">
                <asp:Button ID="BtnBack" runat="server" CssClass="buttonbg" Text="Back" Style="display: none" />
            </div>
            <%--#CC14 Add End --%>
            <div class="clear"></div>
        </div>
        <div class="contentbox margin-top">
            <div class="mainheading2">
                <asp:Label ID="lblIsRetailerFound" runat="server"></asp:Label>
            </div>
            <div class="clear" style="height: 10px">
            </div>
            <div id="trRetailerGrid" runat="server" style="display: none;">
                <div class="grid1">
                    <asp:GridView ID="GridRetailer" runat="server" AllowPaging="false" AlternatingRowStyle-CssClass="Altrow"
                        AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                        FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                        GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                        RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:BoundField DataField="RetailerCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Code"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sale Channel Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Email" HeaderStyle-HorizontalAlign="Left" HeaderText="Email"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MobileNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile No"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PhoneNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Phone No"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Address1" HeaderStyle-HorizontalAlign="Left" HeaderText="Address"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                        <AlternatingRowStyle CssClass="Altrow" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </div>
        <div id="trApproval" runat="server" style="display: none;">
            <div class="contentbox">
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Remark:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtApproalRemarks" CssClass="form_textarea" runat="server" TextMode="MultiLine"
                                MaxLength="500"></asp:TextBox>                         
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div id="trAction" runat="server" style="display: none;">
            <div class="float-margin">
                <%--   <asp:Button ID="btnSubmit" runat="server" CausesValidation="true" CssClass="buttonbg"
                                                OnClick="btnSubmit_Click" Text="Submit" ValidationGroup="Add" /> #CC30 Commented  --%>
                <cc2:ZedButton ID="btnSubmit" runat="server" CausesValidation="true" CssClass="buttonbg"
                    OnClick="btnSubmit_Click" Text="Submit" ValidationGroup="Add" ActionTag="Add" />
                <%--#CC30 Added --%>
            </div>
            <div class="float-margin">
                <asp:Button ID="btnReject" runat="server" CausesValidation="true" CssClass="buttonbg"
                    Visible="false" Text="Reject" ValidationGroup="Add" OnClick="btnReject_Click" /><%--#CC05 ADDED--%>
            </div>
            <div class="float-left">
                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" OnClick="btnCancel_Click"
                    Text="Cancel" ToolTip="Cancel" />
                <asp:Label ID="hdnApproveReject" runat="server" Visible="false" Value="0" />
                <%-- <asp:HiddenField ID="hdnApproveReject" runat="server" Value="0" />--%>
                <%--#CC05 ADDED--%>
                <%--#CC14 Add Start --%>
                <div class="float-margin">
                    <asp:Button ID="btnReset" runat="server" CssClass="buttonbg" Text="Back" Style="display: none" />
                </div>
                <%--#CC14 Add End --%>
            </div>
        </div>
    </div>
</asp:Content>
