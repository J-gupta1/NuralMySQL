<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ManageColorUpload.aspx.cs"
    Inherits="Masters_Common_ManageColorUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--#CC04 Add Start --%>
    <script type="text/javascript">
        function ShowHideTemplate() {
            var vrdvupdatetemplate = document.getElementById("dvColorUpdateTemplate");
            var vrhlnkinvalid = document.getElementById('<%=hlnkInvalid.ClientID%>');
            // alert(ddlUploadTypeValue.value);
            if (vrddlUploadTypeValue.value == 0) {
                vrdvupdatetemplate.style.display = "none";
            }
            else if (vrddlUploadTypeValue.value == 1) {
                vrdvupdatetemplate.style.display = "none";
            }
            else if (vrddlUploadTypeValue.value == 2) {
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
                <li class="link"><%--#CC05 width increased --%>
                    <%-- <asp:DropDownList ID="ddlUploadType" runat="server" CssClass="form_select" onChange="return ShowHideTemplate();">--%>
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
                <%--#CC04 Add End--%>

                <%--#CC04 Added--%>

                <%-- <td align="left" valign="top"></td>--%> <%--#CC05 commented--%>
            </ul>
            <ul>
                <li class="link">
                    <div id="dvColorUpdateTemplate">
                        <a class="elink2" href="../../Excel/Templates/ColorUpload.xlsx">Download Template</a>
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
</asp:Content>
