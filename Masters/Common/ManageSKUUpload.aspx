<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageSKUUpload.aspx.cs" Inherits="Masters_Common_ManageSKUUpload" %>


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
                <li class="text">Select Mode: <span class="error">*</span>
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
            <div class="clear"></div>
            
            <div class="clear"></div>
            <ul>
                <li>
                    <div class="float-margin" id="dvUpdateTemplate">
                        <asp:LinkButton ID="DwnldRefCodePC" runat="server" Text="Download Reference Code"
                            CssClass="elink2" OnClick="DwnldRefCodePC_Click"></asp:LinkButton>&nbsp;
                    </div>
                    <div class="float-margin" id="dvSaveTemplate">
                        <a class="elink2" href="../../Excel/Templates/UploadSKU.xlsx">Download SKU Template</a>
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

    <div class="mainheading">
        Upload Tally Item
    </div>
    <div class="contentbox">
         <div class="H25-C3-S">
             <ul>
                <li class="text">Upload Keyword File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload2" CssClass="fileuploads" runat="server" />
                </li>
                <li class="field3">
                    <asp:Button ID="btnUploadKeyWord" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUploadKeyWord_Click" />
                </li>
               
                
            </ul>
             <div class="clear"></div>
             <ul>
                 <li>
                     <div class="float-margin" id="dvKeyWordTemplate">
                        <a class="elink2" href="../../Excel/Templates/UploadSKUKeyWord.xlsx">Download Keyword Template</a>
                    </div>
                 </li></ul>
                 <div class="clear"></div>
             <ul>
                 <li>
                     <div class="float-margin" id="dvTallyItem">
                        <asp:LinkButton ID="LinkButtonDownLoadTallyItem" runat="server" Text="Download Tally Item-SKU Mapping"
                            CssClass="elink2" OnClick="LinkButtonDownLoadTallyItem_Click"></asp:LinkButton>&nbsp;
                    </div>
                 </li>
             </ul>
             </div>
        </div>
    <%-- #CC01<asp:Panel ID="pnlGrid" Visible="false" runat="server">
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
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUName"
                                                            HeaderText="SKU Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                                            HeaderText="SKU Code">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        
                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUDesc"
                                                            HeaderText="SKU Desc">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ModelName"
                                                            HeaderText="Model Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ColorName"
                                                            HeaderText="Color Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="CartonSize"
                                                            HeaderText="Carton Size">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Attribute1"
                                                            HeaderText="Attribute1">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Attribute2"
                                                            HeaderText="Attribute2">
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
                    </asp:Panel>#CC01--%>
</asp:Content>
