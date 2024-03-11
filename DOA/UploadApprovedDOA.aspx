<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadApprovedDOA.aspx.cs" Inherits="DOA_UploadApprovedDOA" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="updMessage" runat="server">
            <ContentTemplate>
                <uc4:ucMessage ID="uclblMessage" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="mainheading">
            Upload
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">Upload file:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                            </li>
                            <li class="field3">
                                <asp:Button ID="btnUpload" runat="server" CssClass="buttonbg" CausesValidation="true"
                                    Text="Upload" OnClick="btnUpload_Click" />
                            </li>
                            <li class="link">
                                <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>
                            </li>
                            <li class="link">
                                <a class="elink2" href="../Excel/Templates/UploadApprovedDOA.xlsx">Download Template</a>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUpload" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
