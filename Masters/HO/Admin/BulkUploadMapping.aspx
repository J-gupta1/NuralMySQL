<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="BulkUploadMapping.aspx.cs" Inherits="Masters_HO_Admin_BulkUploadMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Upload                   
    </div>
    <div class="contentbox">
        <%--<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Mapping Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlSaleChannelMappingType" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlSaleChannelMappingType_SelectedIndexChanged" AutoPostBack="true">
                        <%-- <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="FOS to Retailer" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="TSM to Retailer" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="ASM to RDS" Value="3"></asp:ListItem>

                                                <asp:ListItem Text="ASM to TSM" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="ZSM to ASM" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="CBH to ZSM" Value="6"></asp:ListItem>--%>
                    </asp:DropDownList>
                  
                    <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="ddlSaleChannelMappingType"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please select Mapping Type."
                        InitialValue="0" SetFocusOnError="true" ValidationGroup="Upload"></asp:RequiredFieldValidator>
                </li>
                <%--===========#CC02 Added Started================--%>
                <li class="text">Relation Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlSaleChannelRelationType" runat="server" AutoPostBack="true" CssClass="formselect">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="req3" runat="server" ControlToValidate="ddlSaleChannelRelationType"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please select Relation Type."
                        InitialValue="0" SetFocusOnError="true" ValidationGroup="Upload"></asp:RequiredFieldValidator>
                </li>
                <%--===========#CC02 Added End================--%>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                </li>
                  </ul>
                <div class="setbbb">
                    <div class="float-margin">
                        <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                            OnClick="btnUpload_Click" ValidationGroup="Upload" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                            OnClick="btnCancel_Click" />
                    </div>
                </div>
          
            <ul class="linklist">
                <li class="link">
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <a class="elink2" href="../../../Excel/Templates/BulkUploadEntityMapping.xlsx">Download Template </a>
                </li>
                <li class="link">
                    <asp:LinkButton ID="DownloadMappedData" runat="server" Text="Download Mapped Data"
                        CssClass="elink2" OnClick="DownloadMappedData_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <asp:LinkButton ID="DownloadUnMappedData" runat="server" Text="Download UnMapped Data"
                        CssClass="elink2" OnClick="DownloadUnMappedData_Click"></asp:LinkButton>
                    <%--=============#CC02 Commented Started============================--%>
                    <%--  <a class="elink2" href="../../../Excel/Templates/SalesHierarchyMapping.xlsx">Download Template (Sales Hierarchy Mapping) </a>--%>
                    <%--=============#CC02 Commented End============================--%>
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>

            </ul>
            <%-- <tr><td><asp:LinkButton ID="DownloadMappedData" runat="server" Text="Download Mapped Data"
                                                CssClass="elink2" ></asp:LinkButton></td>
                                        <td><asp:LinkButton ID="DownloadUnMappedData" runat="server" Text="Download UnMapped Data"
                                                CssClass="elink2" ></asp:LinkButton></td></tr>--%>
        </div>
    </div>

</asp:Content>
