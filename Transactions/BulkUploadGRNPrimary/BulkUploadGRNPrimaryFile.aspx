<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BulkUploadGRNPrimaryFile.aspx.cs" Inherits="Transactions_SalesChannelSB_Interface_BulkUploadGRNPrimaryFile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucUploadMultipleExcelFile.ascx" TagName="ucUploadMultipleExcelFile"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.js"></script>

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.dataTables.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <%--<asp:UpdatePanel runat="server" ID="UpdatePanel1">
 <ContentTemplate>--%>
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <div>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
    <div class="mainheading">
        Manage Warehouse
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblOSDate" runat="server" AssociatedControlID="ucDatePicker" CssClass="formtext">Date:<span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Pleaes enter date."
                        ValidationGroup="Save" />
                </li>
                <li class="text">Warehouse : <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlWarehouse"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select Warehouse."
                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">NDS: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlNDS" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlNDS"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select NDS."
                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Invoice No.:<span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtInvoiceNo" Display="Dynamic"
                            CssClass="error" ValidationGroup="Save" runat="server" ErrorMessage="Please enter a invoice no."></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Vendor : <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="drpVender" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpVender"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select Vendor."
                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </li>
            </ul>
        </div>
        <div class="formlink">
            <ul>
                <li>
                    <a class="elink2" href="../../Excel/Templates/BulkGRNPrimary.xlsx">Download Template</a></li>
                <%--#CC03 Add Start --%>
                <li>
                    <asp:LinkButton ID="lnkReferanceCode" runat="server" CssClass="elink2" Text="Download Referance Code" OnClick="lnkReferanceCode_Click"></asp:LinkButton>
                </li>
                <%--#CC03 Add End --%>
            </ul>
        </div>
    </div>
    <div class="mainheading">
        Upload File       
    </div>
    <div class="contentbox">
        <uc3:ucUploadMultipleExcelFile ID="ucUploadMultipleExcelFile1" runat="server" />
    </div>
    <div class="margin-bottom">
        <div class="float-margin">
            <asp:Button ID="btnSave" runat="server" CssClass="buttonbg" ValidationGroup="Save"
                Text="Save" OnClick="btnSave_Click" />
        </div>
        <div class="float-margin">
            <asp:Button ID="btnReset" runat="server" CssClass="buttonbg" CausesValidation="false"
                Text="Reset" OnClick="btnReset_Click" />
        </div>
        <div class="clear"></div>
    </div>

    <%--</ContentTemplate>
</asp:UpdatePanel>--%>
</asp:Content>
