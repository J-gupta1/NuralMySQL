<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="UpdateCartonNumber.aspx.cs" Inherits="Transactions_BulkUploadGRNPrimaryPrimary_UpdateCartonNumber" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript">
        <%-- #CC02 Comment Start  function checkSerials() {
            var txtSerials = document.getElementById('<%=txtSerialNumber.ClientID%>');
            var varhdnSerialCountLimit = document.getElementById('<%=hdnSerialCountLimit.ClientID%>');
            if (txtSerials.value == "") {
                return false;
            }
            else {
                txtSerials.value = txtSerials.value.replace(/(?:\r\n|\r|\n|,,)/g, ",").replace(/(,,)/g, ",");
                /*var splitCount = txtSerials.value.split(/\r|\n/).replace(/\n/gi, ",");*/
                if (txtSerials.value.charAt(0) == ',') {
                    txtSerials.value = "";
                }
                var splitCount = txtSerials.value.split(",");
                if (splitCount.length > parseInt(varhdnSerialCountLimit.value)) {
                    alert("Maximum IMEI can be processed are " + varhdnSerialCountLimit.value);
                    // txtSerials.value = "";
                    txtSerials.focus();
                    return false;
                }
                return false;
            }
        }#CC02 Comment End--%>
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <uc1:ucMessage ID="ucMessege" runat="server" />
    </div>
    <div class="subheading">
        Update Carton Number
    </div>
    <div class="contentbox">     
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>           
        <div class="H30-C2-S">
            <ul>
                <%-- #CC02 Comment Start <td height="35" align="right" width="15%" class="formtext" valign="top">Carton Number:<font class="error">*</font>
                                        </td>
                                        <td width="20%" align="left" class="formtext" valign="top">
                                            <asp:TextBox ID="txtCartonNumber" runat="server" MaxLength="50" CssClass="form_input2"></asp:TextBox>
                                            <br />
                                            <asp:RequiredFieldValidator ID="reqCartonNumber" runat="server" ControlToValidate="txtCartonNumber"
                                                CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Carton Number."
                                                ForeColor="" ValidationGroup="UpdateCarton"></asp:RequiredFieldValidator>

                                        </td>

                                        <td height="35" align="right" width="15%" class="formtext" valign="top">Serial Number:<font class="error">*</font>
                                        </td>
                                        <td width="20%" align="left" class="formtext" valign="top">
                                            <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form_textarea" TextMode="MultiLine" onkeyup="checkSerials();"></asp:TextBox>
                                            <br />
                                            <asp:Label ID="lblNote" runat="server" CssClass="mandatory"></asp:Label>
                                            <br />
                                            <asp:RequiredFieldValidator ID="reqSerialNumbers" runat="server" ControlToValidate="txtSerialNumber"
                                                CssClass="error" Display="Dynamic" ErrorMessage="Please Enter IMEI/Serial Numbers."
                                                ForeColor="" ValidationGroup="UpdateCarton"></asp:RequiredFieldValidator>

                                        </td> #CC02 Comment End --%>
                <%--#CC02 Add Start --%>
                <li class="text">Upload Carton & Serial Number:<span class="error">*</span>
                </li>
                <li class="field" style="height: auto">
                    <asp:TextBox ID="txtCartonAndSerialNumber" runat="server" CssClass="form_textarea" Height="250px" TextMode="MultiLine"></asp:TextBox>
                    <div>
                    <asp:Label ID="lblNote" runat="server" CssClass="error"></asp:Label>
                    </div>
                    <div>
                    <asp:RequiredFieldValidator ID="reqSerialNumbers" runat="server" ControlToValidate="txtCartonAndSerialNumber"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter Carton Number and Primary IMEI number separated by comma."
                        ValidationGroup="UpdateCarton"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <%--#CC02 Add End --%>
                <li class="text">
                    <%-- #CC02 Comment Start    <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save"
                                                OnClick="btnSave_Click" ValidationGroup="UpdateCarton" />
                                            <asp:Button ID="btnCancel" CssClass="buttonbg" runat="server" Text="Cancel"
                                                OnClick="btnCancel_Click" CausesValidation="false" />
                                            <asp:HiddenField ID="hdnSerialCountLimit" runat="server" Value="0" />#CC02 Comment End --%>

                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>

                </li>
                <%--#CC02 Add Start --%>            
               
               <li class="field">
                    <div>
                        <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save"
                            OnClick="btnSave_Click" ValidationGroup="UpdateCarton" />
                        <asp:Button ID="btnCancel" CssClass="buttonbg" runat="server" Text="Cancel"
                            OnClick="btnCancel_Click" CausesValidation="false" />
                        <asp:HiddenField ID="hdnSerialCountLimit" runat="server" Value="0" />

                        <%-- <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>--%>
                    </div>
                </li>

            </ul>
            <%--#CC02 Add End --%>
        </div>
    </div>
    <div runat="server" id="dvhide" visible="false">
        <div class="mainheading">
            Details
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="grid1">
                        <asp:Panel ID="pnlGrid" Visible="false" runat="server">
                            <asp:GridView ID="GridError" runat="server" AutoGenerateColumns="true" AlternatingRowStyle-CssClass="Altrow"
                                BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-VerticalAlign="Top"
                                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%">
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
