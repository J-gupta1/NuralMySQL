﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ManageOrgnHierarchyUpload.aspx.cs"
    Inherits="Masters_HO_Admin_ManageOrgnHierarchyUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--#CC04 Add Start --%>
    <script type="text/javascript">
        function ShowHideTemplate() {
            var vrdvsavetemplate = document.getElementById("dvSaveTemplate");
            var vrdvupdatetemplate = document.getElementById("dvUpdateTemplate");
            var vrhlnkinvalid = document.getElementById('<%=hlnkInvalid.ClientID%>');
            // alert(ddlUploadTypeValue.value);
            if (vrddlUploadTypeValue.value == 0) {
                vrdvsavetemplate.style.display = "none";
                vrdvupdatetemplate.style.display = "none";
            }
            else if (vrddlUploadTypeValue.value == 1) {
                vrdvsavetemplate.style.display = "block";
                vrdvupdatetemplate.style.display = "none";
            }
            else if (vrddlUploadTypeValue.value == 2) {
                vrdvsavetemplate.style.display = "none";
                vrdvupdatetemplate.style.display = "block";
            }
            vrhlnkinvalid.style.display = "none";
            return false;
        }
    </script>

    <%--#CC04 Add End--%>
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
                <li class="text">Select Mode:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CssClass="radio-rs" CellPadding="2"
                        AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="1" Text="Excel Template"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Interface"></asp:ListItem>
                    </asp:RadioButtonList>
                </li>
            </ul>
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field"><%--#CC05 width reduced --%>
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <%--#CC04 Add Start--%>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
                <li><%--#CC05 width increased --%>
                    <div class="float-margin">
                        <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink><%--#CC04 Added--%>
                    </div>
                </li>
                <%--#CC04 Add End--%>

                <%-- <td align="left" valign="top"></td>--%> <%--#CC05 commented--%>
            </ul>
            <ul>
                <li class="link">
                    <div class="float-margin padding-right" id="dvUpdateTemplate">
                        <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                            CssClass="elink2" OnClick="DwnldReferenceCode_Click"></asp:LinkButton>                                                    
                    </div>
                    <div class="float-margin" id="dvSaveTemplate">
                        <a class="elink2" href="../../../Excel/Templates/OrgnHierarchyUpload.xlsx">Download Template</a>
                    </div>
                    <%-- Add End --%>

                </li>
            </ul>
        </div>
        <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
        --%>
        <%--  </Triggers>
                                </asp:UpdatePanel>--%>
    </div>
    <%--   <asp:Panel ID="pnlGrid" Visible="false" runat="server">
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        <div runat="server" id="dvUploadPreview" visible="false">
                                            Upload Preview
                                        </div>
                                    </div>
                                    <div class="mainheading_rpt_right">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="contentbox">
                                    <div class="grid2">
                                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridUpload" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    CellSpacing="1" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                    GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                    AlternatingRowStyle-CssClass="gridrow1" Width="100%" AllowPaging="false" OnPageIndexChanging="GridUpload_PageIndexChanging">
                                                    <%-- #CC02 DataKeyNames="SalesManCode" removed--
                                                    <RowStyle CssClass="gridrow" />
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LocationName"
                                                            HeaderText="Location Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LocationCode"
                                                            HeaderText="Location Code">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        
                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="HierarchyLevelName"
                                                            HeaderText="Hierarchy Level">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Active"
                                                            HeaderText="Active">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                                            HeaderText="Error">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridheader" />
                                                    <EditRowStyle CssClass="editrow" />
                                                    <AlternatingRowStyle CssClass="gridrow1" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" height="5"></td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <div class="float-margin">
                                    <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                                        OnClick="Btnsave_Click" />
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancel_Click" />
                                </div>
                            </td>
                        </tr>
                    </asp:Panel>--%>
</asp:Content>
