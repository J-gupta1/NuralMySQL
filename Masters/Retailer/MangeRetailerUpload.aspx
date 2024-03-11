<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="MangeRetailerUpload.aspx.cs" Inherits="Masters_HO_Retailer_MangeRetailerUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <%--<asp:Label ID="lblUploadMsg" CssClass="error" runat="server" Text=""></asp:Label>--%>
    <uc1:ucMessage ID="ucmassege1" runat="server" />
    <div class="mainheading">
        Upload
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Select Mode: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdoSelectMode" runat="server" CssClass="radio-rs" RepeatDirection="Horizontal"
                        AutoPostBack="true" CellPadding="2" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="1" Text="Excel Template"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Interface"></asp:ListItem>
                    </asp:RadioButtonList>
                </li>
            </ul>
            <div class="clear"></div>
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <li class="text">
                    <asp:Label ID="lblUploadType" runat="server" Text="Upload Type:"></asp:Label> <span class="error">*</span>
                </li>
                <li class="field"><%--#CC05 width increased --%>
                    <asp:DropDownList ValidationGroup="vgMain" ID="ddlUploadType" runat="server" CssClass="formselect">
                        <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Add" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Update" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                    <div>
                    <asp:RequiredFieldValidator ID="rfvUploadType" Display="Dynamic" ErrorMessage="Please select Upload Type!"
                        runat="server" ControlToValidate="ddlUploadType" InitialValue="0" ValidationGroup="vgMain"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="field3">
                    <%--<a class="elink2" href="../../Excel/Templates/RetailerMaster.xlsx" runat="server" id="lnkRetailerTemplate">Download TemplateWov</a>--%>
                    <asp:Button ID="btnUpload" CssClass="buttonbg" ValidationGroup="vgMain" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click"  />
                </li>
                <li class="link">
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <div id="dvSalesmanSaveTemplate">
                        <asp:LinkButton ID="lnkRetailerTemplateWV" runat="server" Text="Download Template"
                            CssClass="elink2" OnClick="lnkRetailerTemplateWV_Click"></asp:LinkButton>
                    </div>
                </li>
                <%--#CC04 Add End --%>

                <li class="link"><%--#CC05 width increased --%>
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink><%--#CC04 Added--%>
                </li>

            </ul>
        </div>
    </div>


    <%-- #CC22 Start <div runat="server" id="dvhide" visible="false">
                     
                                            <div class="mainheading">
                                                Details
                                            </div>
                                       
                                <div class="contentbox">
                                    <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="grid1">
                                                <asp:Panel ID="pnlGrid" Visible="false" runat="server">
                                                    <asp:GridView ID="GridRetailer" runat="server" AutoGenerateColumns="true" AlternatingRowStyle-CssClass="Altrow"
                                                        BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-VerticalAlign="Top"
                                                        GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                                        HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                                        RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%">
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
                    </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
               	</div>
                <div class="margin-bottom">
            		<div class="float-margin">
                                <asp:Button ID="Btnsave" runat="server" Text="Save" CssClass="buttonbg" OnClick="Btnsave_Click" Visible="false" />
	    		</div>
            		<div class="float-margin">
                                <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="buttonbg" OnClick="BtnCancel_Click" />
                       	</div>
            		<div class="clear"></div>
        	</div>
      </div> #CC22 END--%>
</asp:Content>
