<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadCurrentOutstanding.aspx.cs" Inherits="Transactions_Transactions_SalesChannel_UploadCurrentOutstanding" %>

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
    <div class="mainheading">
        Upload File
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">            
            <ul>
                <li class="text">Upload File:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
                <li class="link">
                    <a class="elink2" href="~/Excel/Templates/CurrentOutstandingBulkUpload.xlsx" runat="server"
                        id="lnkCurrentOutstandingBulkTemplate">Download Template</a>
                    <%--#CC01 Add Start --%>
                 </li>
                <li class="link">                                     
                    <asp:LinkButton ID="hlnkDownLoadRefCode" class="elink2" runat="server" Text="Download Referance Code" OnClick="hlnkDownLoadRefCode_Click"></asp:LinkButton>
                    <%--#CC01 Add End --%>
                </li>
            </ul>
        </div>
    </div>
    <div runat="server" id="dvhide" visible="false">
        <div class="mainheading">
            Details
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="grid1">
                        <asp:Panel ID="pnlGrid" Visible="false" runat="server">
                            <asp:GridView ID="GridCurrentOutstanding" runat="server" AutoGenerateColumns="true"
                                AlternatingRowStyle-CssClass="Altrow" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow"
                                RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected"
                                Width="100%">
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:Button ID="Btnsave" runat="server" Text="Save" CssClass="buttonbg" OnClick="btnSave_Click"
                Visible="false" />
            <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="buttonbg" OnClick="BtnCancel_Click" />
        </div>
    </div>
    <%--</ContentTemplate>
</asp:UpdatePanel>--%>
</asp:Content>
