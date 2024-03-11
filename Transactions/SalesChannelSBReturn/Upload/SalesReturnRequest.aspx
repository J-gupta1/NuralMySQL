<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="SalesReturnRequest.aspx.cs" Inherits="Transactions_SalesReturnRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
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
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Return Remark: <span class="error">*</span>
                </li>
                <li class="field"><asp:TextBox ID="txtRemark" TextMode="MultiLine" MaxLength="250" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ReqRemark" runat="server" ControlToValidate="txtRemark" CssClass="error"
                                        Display="Dynamic" ErrorMessage="Please enter sales return remark." SetFocusOnError="true"
                                        ValidationGroup="SR"></asp:RequiredFieldValidator>
                    </li>
            </ul>
            <div class="clear"></div>
            <ul>
                
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text="" Visible="true"></asp:Label>
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11" CausesValidation="true" ValidationGroup="SR"
                        OnClick="btnUpload_Click" />
                </li>
            </ul>
            <div class="clear"></div>
            
            
            
        </div>
        <div class="formlink">
            <ul>
                <li class="link">
                    <%--#CC04 START ADDED--%>
                    <asp:LinkButton ID="DwnldBindCode" runat="server" Text="Download Bin Code"
                        CssClass="elink2" OnClick="DwnldBindCode_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <%--#CC04 END ADDED--%>
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <a class="elink2" href="../../../Excel/Templates/SalesReturnRequest.xlsx"
                         id="lnkFullTemplate" runat="server">Download Template</a>

                    
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
    
</asp:Content>
