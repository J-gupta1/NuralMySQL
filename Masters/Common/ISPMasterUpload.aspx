<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ISPMasterUpload.aspx.cs" Inherits="Masters_Common_ISPMasterUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Select Mode: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CssClass="radio-rs" CellPadding="2"
                            AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="1" Text="Excel Template"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Interface"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </li>
            </ul>
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
            </ul>
        </div>
        <div class="formlink">
            <ul>
                <li class="link">
                   <%-- <a href="../../Excel/Templates/ISPMaster.xlsx" class="elink2">Download Template</a> #CC02 Commented --%>
                    <%--#CC02 Add Start --%>
                    <asp:LinkButton ID="lnkDownloadTemplate" runat="server" class="elink2" Text="Download Template" OnClick="lnkDownloadTemplate_Click" ></asp:LinkButton>

                    <%--#CC02 Add End --%>
                </li>
                <li class="link">
                    <asp:LinkButton ID="LnkDownloadRefCode" runat="server" CssClass="elink2" OnClick="LnkDownloadRefCode_Click">Download Retailer Reference Code</asp:LinkButton>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkDuplicate" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkBlank" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
                <li>
                    <asp:Label ID="lblUploadMsg" CssClass="error" runat="server" Text=""></asp:Label>
                </li>
            </ul>
        </div>
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="GridISP" runat="server" AutoGenerateColumns="true" AlternatingRowStyle-CssClass="Altrow" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                    GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                    RowStyle-VerticalAlign="top" Width="100%">
                <%--<Columns>
                    <asp:TemplateField HeaderText="SSSCode">
                    <ItemTemplate>
                    <asp:TextBox ID="TxtsssCode" runat="server" Text='<%#Eval("SSSCode")%>'></asp:TextBox>
                    </ItemTemplate>
                    
                    </asp:TemplateField>
                    
                        <asp:TemplateField HeaderText="BeautyAdvisorCode">
                    <ItemTemplate>
                    <asp:TextBox ID="txtISPcode" runat="server" Text='<%#Eval("BeautyAdvisorCode")%>'></asp:TextBox>
                    </ItemTemplate>
                    
                    </asp:TemplateField>
                          <asp:TemplateField HeaderText="BeautyAdvisorName">
                    <ItemTemplate>
                    <asp:TextBox ID="txtISPName" runat="server" Text='<%#Eval("BeautyAdvisorName")%>'></asp:TextBox>
                    </ItemTemplate>
                    
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Mobile">
                    <ItemTemplate>
                    <asp:TextBox ID="txtMobile" runat="server" Text='<%#Eval("Mobile")%>'></asp:TextBox>
                    </ItemTemplate>
                     </asp:TemplateField>
                       <asp:TemplateField HeaderText="Error">
                    <ItemTemplate>
                    <asp:Label ID="lblError" runat="server" Text='<%#Eval("Error")%>'></asp:Label>
                    </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>--%>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
