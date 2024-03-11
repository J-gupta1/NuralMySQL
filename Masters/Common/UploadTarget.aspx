<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="UploadTarget.aspx.cs" Inherits="UploadTarget" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .date_frrom_to div {
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uc4:ucMessage ID="uclblMessage" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="mainheading">
            Input Parameters
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <%--#CC02 Add Start--%>
                            <li class="text">Save/Update: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlTargetSaveUpdate" ValidationGroup="Upload" runat="server" AutoPostBack="true"
                                        CssClass="formselect" OnSelectedIndexChanged="ddlTargetSaveUpdate_SelectedIndexChanged">
                                        <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Save" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Update" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqDdlSaveUpdate" ValidationGroup="Upload"
                                        runat="server" Display="Dynamic" ControlToValidate="ddlTargetSaveUpdate" CssClass="error" InitialValue="-1"
                                        ErrorMessage="Please Select Save/Update"></asp:RequiredFieldValidator>
                                </div>

                            </li>
                            <li class="text">Target Name: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:TextBox ID="txttargetName" runat="server" MaxLength="80" CssClass="formfields"></asp:TextBox>

                                    <asp:DropDownList ID="ddlTarget" ValidationGroup="Upload" runat="server" CssClass="formselect" Visible="false"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlTarget_SelectedIndexChanged">
                                        <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="rqtargetName" ValidationGroup="Upload"
                                        runat="server" Display="Dynamic" ControlToValidate="txttargetName" CssClass="error"
                                        ErrorMessage="Enter Target Name."></asp:RequiredFieldValidator>
                                </div>
                            </li>

                            <%--#CC02 Add End--%>
                            <li class="text">Target Type: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlTargetType" ValidationGroup="Upload" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RFVddlTargetType" ValidationGroup="Upload" runat="server"
                                        Display="Dynamic" InitialValue="0" ControlToValidate="ddlTargetType" CssClass="error"
                                        ErrorMessage="Please Select Target Type."></asp:RequiredFieldValidator>
                                </div>

                            </li>
                        </ul>
                        <ul>
                            <li class="text">Template Category: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlTargetCategory" CssClass="formselect" ValidationGroup="Upload"
                                        runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RFVddlTemplateTypeUpload" runat="server" ValidationGroup="Upload"
                                        InitialValue="0" ControlToValidate="ddlTargetCategory" CssClass="error" Display="Dynamic"
                                        ErrorMessage="Please Select Template Category."></asp:RequiredFieldValidator>
                                </div>

                            </li>
                            <li class="text">Period (From- To): <span class="error">*</span>
                            </li>
                            <li class="field date_frrom_to">
                                <div class="datblk">
                                    <uc1:ucDatePicker ID="TargetFromDate" runat="server" IsRequired="true" ValidationGroup="Upload" />
                                    <uc1:ucDatePicker ID="TargetToDate" runat="server" IsRequired="true" ValidationGroup="Upload" />
                                </div>

                            </li>

                            <li class="text">Select Type: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlType" CssClass="formselect" ValidationGroup="Upload" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                        runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="rqDDLType" runat="server" ValidationGroup="Upload"
                                        InitialValue="0" ControlToValidate="ddlType" CssClass="error" Display="Dynamic"
                                        ErrorMessage="Please Select Type."></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <%--#CC01 Add End--%>
                        </ul>
                        <ul>
                            <%--<li class="text">Select Channel Type: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlUserType" ValidationGroup="Upload" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="rqDdlUserType" ValidationGroup="Upload"
                                        runat="server" Display="Dynamic" InitialValue="0" ControlToValidate="ddlUserType"
                                        CssClass="error" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </li>--%>


                            <li class="text">Target Based On: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlTargetBased" CssClass="formselect" ValidationGroup="Upload"
                                        runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="Upload"
                                        InitialValue="0" ControlToValidate="ddlTargetBased" CssClass="error" Display="Dynamic"
                                        ErrorMessage="Please Select Target Based On."></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">Upload file: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Upload"
                                        InitialValue="0" ControlToValidate="FileUpload1" CssClass="error" Display="Dynamic"
                                        ErrorMessage="Please Upload file."></asp:RequiredFieldValidator>
                                </div>
                            </li>

                            <%--#CC02 Add Start--%>

                            <%--#CC02 Add End--%>
                        </ul>
                        <ul>
                            <li class="text"></li>
                            <li class="field3">
                                <asp:Button ID="btnUpload" runat="server" CssClass="buttonbg" CausesValidation="true"
                                    Text="Upload" OnClick="btnUpload_Click" ValidationGroup="Upload" />

                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" CausesValidation="false"
                                    Text="Cancel" OnClick="btnCancel_Click" />
                                <%--#CC02 Added--%>

                            </li>
                            <%-- #CC02 Commented
                                                    </tr>
                                                <tr>
                                                    <td align="left" valign="top" colspan="6">
                                                  #CC03 Commented --%>
                        </ul>
                        <ul>
                            <li class="link">
                                <%--#CC02 Added--%>
                                <asp:LinkButton ID="lnkDownload" runat="server" CssClass="elink2" OnClick="lnkDownload_Click">Download SKU Wise Template</asp:LinkButton>
                            </li>
                            <li class="link">
                                <asp:LinkButton ID="lnksummeryDwnload" runat="server" CssClass="elink2" OnClick="lnksummeryDwnload_Click">Download Consolidated Template</asp:LinkButton>
                            </li>
                            <li class="link">
                                <asp:LinkButton ID="LnkDownloadRefCode" runat="server" CssClass="elink2" OnClick="LnkDownloadRefCode_Click">Download Reference Code</asp:LinkButton>
                            </li>
                            <li class="link">
                                <%-- #CC09 Add Start --%>
                                <asp:LinkButton ID="lnkProductCategoryWise" runat="server" CssClass="elink2" OnClick="lnkProductCategoryWise_Click">Download Product Category Wise Template</asp:LinkButton>

                                <%-- #CC09 Add End --%>  
                            </li>
                            <li class="link">
                                <asp:LinkButton ID="lnkWODWiseTemplate" runat="server" CssClass="elink2" OnClick="lnkWODWiseTemplate_Click">Download WOD Template</asp:LinkButton>
                            </li>
                        </ul>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lnkDownload" />
                    <asp:PostBackTrigger ControlID="lnksummeryDwnload" />
                    <asp:PostBackTrigger ControlID="LnkDownloadRefCode" />
                    <asp:PostBackTrigger ControlID="lnkProductCategoryWise" />
                    <asp:PostBackTrigger ControlID="lnkWODWiseTemplate" />
                    <%-- #CC09 Added --%>
                    <asp:PostBackTrigger ControlID="btnUpload" />

                    <asp:PostBackTrigger ControlID="ddlTargetSaveUpdate" />
                    <asp:PostBackTrigger ControlID="ddlTarget" />

                </Triggers>
            </asp:UpdatePanel>
        </div>

        <%--<asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>

        <asp:Panel ID="pnlGrid" runat="server" Visible="false">

            <div class="mainheading">
                Upload Preview
            </div>

            <div class="contentbox">
                <div class="grid1">
                    <asp:GridView ID="GridTarget" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                        bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                        HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                        RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="Left" RowStyle-VerticalAlign="top"
                        SelectedStyle-CssClass="gridselected" Width="100%" AutoGenerateColumns="true"
                        OnPageIndexChanging="GridTarget_PageIndexChanging">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                        <PagerStyle CssClass="gridfooter" />
                    </asp:GridView>
                </div>
            </div>
            <div class="margin-bottom">
                <div class="float-margin">
                    <asp:Button ID="btnSaveTarget" CssClass="buttonbg" runat="server" CausesValidation="true"
                        Text="Submit" OnClick="btnSaveTarget_Click" ValidationGroup="Upload" />
                    <%--#CC02 Add Start
                                                <asp:Button ID="btnUpdate" CssClass="buttonbg" runat="server" CausesValidation="true"
                                                    Text="Update" OnClick="btnUpdate_Click" ValidationGroup="Upload" />&nbsp;--%>
                    <%--#CC02 Add End--%>
                </div>
                <div class="float-margin">
                    <asp:Button ID="btnReset" CssClass="buttonbg" runat="server" OnClick="btnReset_Click"
                        CausesValidation="false" Text="Reset" />
                </div>
            </div>
            <div class="clear"></div>

        </asp:Panel>

        <%--    </ContentTemplate>
                  </asp:UpdatePanel>--%>
    </div>
</asp:Content>
