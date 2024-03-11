<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageSalesChannelUpload.aspx.cs" Inherits="Masters_SalesChannel_ManageSalesChannelUpload" %>

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
                <li class="link"><%--#CC05 width increased --%>
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink><%--#CC04 Added--%>
                                            
                </li>
                <%--#CC04 Add End--%>

                <%-- <td align="left" valign="top"></td>--%> <%--#CC05 commented--%>
            </ul>
            <ul>
                <li>
                    <div class="float-margin" id="dvUpdateTemplate">
                        <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                            CssClass="elink2" OnClick="DwnldReferenceCode_Click"></asp:LinkButton>
                    </div>

                    <div class="float-margin" id="dvSaveTemplate">
                        <a class="elink2" href="../../Excel/Templates/UploadSalesChannel.xlsx">Download Template</a>
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
    <%--<asp:Panel ID="pnlGrid" Visible="false" runat="server">
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
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelType"
                                                            HeaderText="SalesChannelType">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                                            HeaderText="SalesChannelCode">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        
                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelName"
                                                            HeaderText="SalesChannelName">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ContactPerson"
                                                            HeaderText="ContactPerson">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BusinessStartDate"
                                                            HeaderText="BusinessStartDate">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LoginName"
                                                            HeaderText="LoginName">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Password"
                                                            HeaderText="Password">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StateName"
                                                            HeaderText="StateName">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="CityName"
                                                            HeaderText="CityName">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="AreaName"
                                                            HeaderText="AreaName">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="AddressLine1"
                                                            HeaderText="AddressLine1">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="AddressLine2"
                                                            HeaderText="AddressLine2">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="PinCode"
                                                            HeaderText="PinCode">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="PhoneNo"
                                                            HeaderText="PhoneNo">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MobileNo1"
                                                            HeaderText="MobileNo1">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MobileNo2"
                                                            HeaderText="MobileNo2">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Fax"
                                                            HeaderText="Fax">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="PanNo"
                                                            HeaderText="PanNo">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                          <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Email"
                                                            HeaderText="Email">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                          <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="GSTNo"
                                                            HeaderText="GSTNo">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                          <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TINNo"
                                                            HeaderText="TINNo">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                          <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DOB"
                                                            HeaderText="DOB">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                          <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DateofAnniversary"
                                                            HeaderText="DateofAnniversary">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                          <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ReportHierarchyCode"
                                                            HeaderText="ReportHierarchyCode">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ParentSalesChannelCode"
                                                            HeaderText="ParentSalesChannelCode">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OpeningStockDate"
                                                            HeaderText="OpeningStockDate">
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
