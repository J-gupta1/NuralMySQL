<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadSalesMan.aspx.cs" Inherits="Masters_SalesMan_UploadSalesMan" %>

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
        <%--<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field"><%--#CC05 width reduced --%>
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <%--#CC04 Add Start--%>
                <li class="text">
                    <asp:Label ID="lblUploadType" runat="server" Text="Upload type:"></asp:Label> <span class="error">*</span>
                </li>
                <li class="field"><%--#CC05 width increased --%>
                    <%-- <asp:DropDownList ID="ddlUploadType" runat="server" CssClass="form_select" onChange="return ShowHideTemplate();">--%>
                    <asp:DropDownList ID="ddlUploadType" runat="server" CssClass="formselect">
                        <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Add" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Update" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <%--#CC04 Add End--%>
                <li class="field3"><%--#CC05 width removed --%>
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink><%--#CC04 Added--%>
                </li>
                <%-- <td align="left" valign="top"></td>--%> <%--#CC05 commented--%>
            </ul>
        </div>
        <div class="formlink">
            <ul>

                <%--#CC04 Comment Start  <td valign="top" align="left" colspan="4">
                                            <div>
                                                <div class="float-left">
                                                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Sales Channel Code"
                                                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                            &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                            <%--<asp:LinkButton ID="DwnldSKUCodeTemplate" runat="server" Text="Download SKUCode"
                                                        OnClick="DwnldSKUCodeTemplate_Click" CssClass="elink2"></asp:LinkButton>--%>

                <%-- &nbsp;&nbsp;&nbsp; <a class="elink2" href="../../Excel/Templates/SalesMan.xlsx">Download
                                                Template</a>
                                           #CC04 Comment End--%>
                <%--#CC04 Add Start --%>



                <li>
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Sales Channel Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
                <li id="dvSalesmanSaveTemplate">
                    <a class="elink2" href="../../Excel/Templates/SalesMan.xlsx">Download Template(Add)</a>
                </li>
                <li id="dvSalesmanUpdateTemplate">
                    <a class="elink2" href="../../Excel/Templates/UpdateSalesManTemplate.xlsx">Download Template(Update)</a>
                </li>

                <%--#CC04 Add End --%>
            </ul>
        </div>
        <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                                         <asp:PostBackTrigger ControlID="DwnldDustributorCodeTemplate" />--%>
        <%--  </Triggers>
                                </asp:UpdatePanel>--%>
    </div>

    <asp:Panel ID="pnlGrid" Visible="false" runat="server">
        <div class="mainheading">
            <div runat="server" id="dvUploadPreview" visible="false">
                Upload Preview
            </div>
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GridSalesMan" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            CellSpacing="1" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" Width="100%" AllowPaging="false" OnPageIndexChanging="GridSalesMan_PageIndexChanging">
                            <%-- #CC02 DataKeyNames="SalesManCode" removed--%>
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                    HeaderText="Sales Channel Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesManName"
                                    HeaderText="SalesMan  Name">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%-- #CC02 Comment Start <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesManCode"
                                                            HeaderText="Sales Man Code">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>  #CC02 Comment End --%>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Address"
                                    HeaderText="Address">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MobileNumber"
                                    HeaderText="Mobile Number">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--#CC01 Add Start--%>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EmailID"
                                    HeaderText="Email ID">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--#CC01 Add End--%>

                                <%--#CC04 Add Start--%>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Active"
                                    HeaderText="Active">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--#CC04 Add End--%>


                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                    HeaderText="Error">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="margin-bottom">
            <div class="float-margin">
                <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                    OnClick="Btnsave_Click" />
            </div>
            <div class="float-left">
                <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancel_Click" />
            </div>
        </div>
        <div class="clear"></div>
    </asp:Panel>
</asp:Content>
