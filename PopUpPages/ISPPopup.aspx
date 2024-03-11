<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ISPPopup.aspx.cs"
    Inherits="PopUpPages_ISPPopup" meta:resourcekey="PageResource1" %>

<%@ Register Src="../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server" id="Head1">
    <script type="text/javascript">
        var baseUrl = '<%# ResolveUrl("~/") %>';
        
    </script>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/css/style.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/Assets/Jscript/dhtmlwindow.js") %>"></script>

    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/Assets/Jscript/modal.js") %>"></script>


    <script type="text/javascript">
        function popupRetailer() {
            WinSearchRetailerCode = dhtmlmodal.open("WinSearchRetailerCodeE", "iframe", "../Popuppages/SearchRetailer.aspx", "Retailer Detail", "width=800px,height=330px,top=25,resize=0,scrolling=auto ,center=1")
            WinSearchRetailerCode.onclose = function () {
                return true;
            }
            return false;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdMain" runat="server">
                    <ContentTemplate>
                        <div>
                            <uc4:ucMessage ID="ucMessage" runat="server" />
                            <div class="contentbox">
                                <div class="mandatory">
                                    (*) Marked fields are mandatory.
                                </div>
                                <div id="tblMap" runat="server" class="H25-C3-S">
                                    <ul runat="server">
                                        <li class="text" runat="server">New Retailer Name:<span class="error">*</span>
                                        </li>
                                        <li class="field" runat="server">
                                            <asp:TextBox ID="ctl00_contentHolderMain_txtRetailerName" runat="server" CssClass="formfields" Enabled="False"></asp:TextBox>
                                            <div>
                                                <asp:RequiredFieldValidator ID="reqRetailerName" CssClass="error" ValidationGroup="vgSubmitMap" runat="server" ControlToValidate="ctl00_contentHolderMain_txtRetailerName" ErrorMessage="Please select retailer." Display="Dynamic"></asp:RequiredFieldValidator></div>
                                        </li>
                                        <li class="text"></li>
                                        <li class="field" runat="server">
                                            <asp:Button ID="btnSearchRetailer" runat="server" Text="Search Retailer" CssClass="buttonbg" />

                                        </li>
                                        <li class="text" runat="server">From Date:<span class="error">*</span>
                                        </li>
                                        <li class="field" runat="server">
                                            <uc2:ucDatePicker ID="ucDatePickerFromDate" ErrorMessage="Please enter from date."
                                                runat="server" ValidationGroup="vgSubmitMap" RangeErrorMessage="Date can  not be less than todays." />
                                        </li>
                                        <li class="field3" runat="server">
                                            <asp:Button ID="btnSubmitMap" runat="server" CssClass="buttonbg" OnClick="btnSubmitMap_Click"
                                                Text="Submit" ValidationGroup="vgSubmitMap" />
                                            <asp:Button ID="btnClearMap" runat="server" CssClass="buttonbg" OnClick="btnClearMap_Click"
                                                Text="Cancel" />
                                        </li>
                                    </ul>
                                </div>
                                <div id="tblDelete" runat="server" class="H25-C3-S">
                                    <ul runat="server">
                                        <li class="text" runat="server">
                                            <asp:CheckBox ID="chkRetailRetailer" runat="server" Checked="True"
                                                Text="Retain Old Retailer" />
                                        </li>
                                    </ul>
                                    <ul runat="server">
                                        <li class="field" runat="server">
                                            <asp:Button ID="btnExitISPSubmit" runat="server" CssClass="buttonbg" OnClick="btnExitISPSubmit_Click"
                                                Text="Submit" ValidationGroup="vgSubmitExitISP" />
                                        </li>
                                    </ul>
                                </div>
                                <div id="tblExisting" runat="server" class="H25-C3-S">
                                    <ul runat="server">
                                        <li class="text" runat="server">End Date :<span class="error">*</span>
                                        </li>
                                        <li class="field" runat="server">
                                            <uc2:ucDatePicker ID="ucDateEndDate" runat="server" ErrorMessage="Please Enter Date."
                                                ValidationGroup="vgSubmitExitISP" />
                                        </li>                                   
                                        <li class="field3" runat="server">
                                            <asp:Button ID="btnExitingISP" runat="server" CssClass="buttonbg" OnClick="btnExitISPSubmit_Click"
                                                Text="Submit" ValidationGroup="vgSubmitExitISP" />
                                        </li>
                                    </ul>
                                </div>
                                <asp:HiddenField ID="ctl00_contentHolderMain_hdnRetailerID" runat="server" />
                                <asp:HiddenField ID="ctl00_contentHolderMain_hdnRetailerName" runat="server" />
                                <asp:HiddenField ID="hdfSuccess" runat="server" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
