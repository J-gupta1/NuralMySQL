<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OrgnHierarchyUserUpload.aspx.cs" Inherits="Masters_HO_Admin_OrgnHierarchyUserUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--#CC04 Add Start --%>
    <script type="text/javascript">
        function ShowHideTemplate() {
            var vrddlUploadTypeValue = document.getElementById('<%=ddlUploadType.ClientID%>');
            var vrdvsavetemplate = document.getElementById("dvSalesmanSaveTemplate");
            var vrdvupdatetemplate = document.getElementById("dvSalesmanUpdateTemplate");
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
        <div class="mandatory">
                (*) Marked fields are mandatory            
            </div>
        <div class="H25-C3-S">            
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <li class="text">
                    <asp:Label ID="lblActivity" runat="server" Text="Activity"></asp:Label><span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlUploadType" runat="server" CssClass="formselect">
                        <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Add" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Update" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li class="field3">
                    <div>
                        <div class="float-margin">
                            <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                OnClick="btnUpload_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="BtnReset" CssClass="buttonbg" runat="server" Text="Reset"
                                OnClick="BtnReset_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
        <div class="clear"></div>
        <div class="formlink">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DwnldReferenceCode" runat="server" Text="Download Referance Code" CssClass="elink2" OnClick="DwnldReferenceCode_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <a class="elink2" href="../../../Excel/Templates/OrgnHierarchyUserAddTemplate.xlsx">Download Template (Add)</a>
                </li>
                <li class="link">
                    <a class="elink2" href="../../../Excel/Templates/OrgnHierarchyUserUpdateTemplate.xlsx">Download Template (Update)</a>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
