<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="StockAdjustmentUpload.aspx.cs" Inherits="Transactions_Common_StockAdjustmentUpload" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc8" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

       <%--
        #CC04 Comment Start
         function ValidateAdjustForType() {

            var e = document.getElementById('<%= ddlType.ClientID %>');
            var strUser = e.value;
            if (strUser == "0") {
                alert('Please select adjust for Type.');
                document.getElementById('<%= txtAdjustmentFor.ClientID %>').value = '';
                return false
            }
            else
                return true;
        }

        function Validation() {
            var AdjustmentForCode = document.getElementById('<%= txtAdjustmentFor.ClientID %>');
         if (AdjustmentForCode.value.indexOf('-') <= 0) {
             alert("Please Enter valid SalesChannel Code.");
             AdjustmentForCode.value = "";
             AdjustmentForCode.focus();
             return false;
         }
     }

     function txtAdjustmentForSalesChannelCodeTextChanged() {
         //debugger;

         var v = ValidateAdjustForType();

         if (v == false) {
             return;
         }

         var strUser = "0";
         var v = "";
         var Id = document.getElementById('<%= hdnAdjustmentForSalesChannelid.ClientID %>');

         var ChannelsTypeid = document.getElementById('<%= ddlType.ClientID %>');
         var SalesChannelTypeid = ChannelsTypeid.options[ChannelsTypeid.selectedIndex].value;

         Validation();


         var AdjustmentForCode = document.getElementById('<%= txtAdjustmentFor.ClientID %>').value;
         //if (AdjustmentForCode.indexOf('-') <= 0) {
         //  alert("Please Enter valid SalesChannel Code.");
         // return;
         //}
         //alert(AdjustmentForCode);
         Typeid = SalesChannelTypeid;
         CommonService.GetSalesChannelInformation(AdjustmentForCode, SalesChannelTypeid,
                 OnSTDLookupComplete = function OnSTDLookupComplete(result, userContext) {

                     var vv = result.toString();
                     //alert(vv);
                     if (vv != '') {
                         var lst = new Array();
                         lst = vv.split('/');
                         //Id.value = vv.split('/')[1];
                         Id.value = lst[lst.length - 1];
                         //alert(Id.value);
                     }
                     else
                         Id.value = "0";


                 }, OnError);

     } #CC04 Comment End --%>

        function OnError(result) {
            alert("Error: " + result.get_message());
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:TextBox ID="hdnAdjustmentForSalesChannelid" runat="server" Value="0" Style="display: none" />
    <div>
        <div>
            <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uc8:ucMessage ID="ucMessage1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="contentbox margin-top">
            <div class="mandatory">
                (*) Marked fields are mandatory
            </div>
            <div class="H30-C3-S">
                <ul>
                    <li class="text">Select Mode: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:RadioButtonList ID="rdModelList" runat="server" CssClass="radio-rs" TextAlign="Right" RepeatDirection="Horizontal"
                            CellPadding="2" CellSpacing="0" BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                            <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                            <%-- <asp:listitem text="interface" value="1"></asp:listitem>--%>
                        </asp:RadioButtonList>
                    </li>
                </ul>
                <div class="clear"></div>
                <ul>
                    <li class="text">Stock Adjustment Date: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <uc1:ucDatePicker ID="ucDatePicker1" runat="server" IsRequired="True" ValidationGroup="grpupld"
                            ErrorMessage="Please enter stock adjustment date" />
                    </li>
                    <li class="text">Sales Channel Type: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <div>
                            <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                                CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                SetFocusOnError="true" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <%-- #CC04 Comment Start
                                   
                                   <td width="17%" align="right" valign="top" >Adjustment for:<span class="error">*</span>
                                </td>
                                <td align="left" valign="top" >
                                    <asp:TextBox ID="txtAdjustmentFor" runat="server" MaxLength="30" onchange="javascript:txtAdjustmentForSalesChannelCodeTextChanged();"
                                        CssClass="form_input9" Width="150px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                                        MinimumPrefixLength="3" ServiceMethod="GetSalesChannelCodeList" ServicePath="../../CommonService.asmx"
                                        TargetControlID="txtAdjustmentFor" UseContextKey="true">
                                    </cc1:AutoCompleteExtender>
                                </td>

                                   #CC04 Comment End
                    --%>
                    <li class="text">Reason: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="cmbReason" runat="server" ValidationGroup="Save" CssClass="formselect">
                            </asp:DropDownList>
                        </div>

                        <asp:RequiredFieldValidator ID="Req2" runat="server" ErrorMessage="Please select reason"
                            Display="Dynamic" CssClass="error" InitialValue="0" ValidationGroup="Save" ControlToValidate="cmbReason"></asp:RequiredFieldValidator>
                    </li>
                    <li class="text">Remarks: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <uc2:ucTextboxMultiline ID="txtRemarks" runat="server" IsRequired="true" CharsLength="100"
                            TextBoxWatermarkText="Please input remarks" ErrorMessage="Please enter remarks."
                            ValidationGroup="Save" />
                    </li>
                </ul>
                <ul>
                    <li class="text">Upload Adjustment File:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:FileUpload ID="Fileupload1" CssClass="fileuploads" runat="server" />
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="reqFileUpload" ControlToValidate="Fileupload1" ValidationGroup="grpupld"
                                runat="server" CssClass="error" ErrorMessage="Please upload a valid .xlsx file"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:RegularExpressionValidator ID="regExFileUpload" ValidationExpression="[a-zA-Z0_9].*\b(.xlsx|.XLSX)\b"
                                ControlToValidate="Fileupload1" ValidationGroup="grpupld" runat="server" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please upload a valid .xlsx file"></asp:RegularExpressionValidator>
                        </div>
                        <%--#CC10 Add Start --%>

                        <div class="error">
                            <asp:Label ID="lblExcelRowsLimitMsg" runat="server"></asp:Label>
                        </div>
                        <%--#CC10 Add End --%>
                    </li>
                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnUploadData" runat="server" CssClass="buttonbg" Text="Upload" ValidationGroup="grpupld" OnClientClick="return Validation();"
                                OnClick="btnUploadData_Click2" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="Save"
                                Visible="false" CssClass="buttonbg" OnClick="btnSave_Click" />
                            <asp:Button ID="btnReset" runat="server" CssClass="buttonbg" Text="Reset" OnClick="btnReset_Click" />
                        </div>
                    </li>
                </ul>
            </div>
            <div class="formlink margin-top">
                <ul>
                    <li class="link">
                        <%--#CC07 START ADDED--%>
                        <asp:LinkButton ID="DwnldBindCode" runat="server" Text="Download Bin Code"
                            CssClass="elink2" OnClick="DwnldBindCode_Click"></asp:LinkButton>
                                           
                                            <%--#CC07 END ADDED--%>
                    </li>

                    <li class="link">
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="elink2">Download Stock Adjustment Template</asp:HyperLink>

                        <%--#CC04 Add Start--%>
                    </li>
                    <li class="link">
                        <asp:LinkButton ID="lnkRefCode" runat="server" CssClass="elink2" OnClick="lnkRefCode_Click" Text="Download Referance Code"></asp:LinkButton>
                        <%--#CC04 Add End--%>

                    </li>
                    <li class="link">
                        <div class="float-margin">
                            <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                        </div>
                        <div class="float-margin">
                            <asp:HyperLink ID="hlnkDuplicate" runat="server" CssClass="elink3"></asp:HyperLink>
                        </div>
                        <div class="float-left">
                            <asp:HyperLink ID="hlnkBlank" runat="server" CssClass="elink3"></asp:HyperLink>
                        </div>
                    </li>
                </ul>
            </div>
    </div>
    <div class="clear">
    </div>
    <asp:UpdatePanel ID="upSearchPartResult" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div runat="server" id="dvSearchResult">
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="gridStockAdjustment" runat="server" AlternatingRowStyle-CssClass="Altrow"
                                SelectedRowStyle-CssClass="selectedrow" AutoGenerateColumns="true" CellPadding="4"
                                CellSpacing="1" EditRowStyle-CssClass="editrow" GridLines="None" HeaderStyle-CssClass="gridheader"
                                RowStyle-CssClass="gridrow" Width="100%" OnDataBound="gridStockAdjustment_DataBound">
                                <RowStyle CssClass="gridrow" />
                                <Columns>
                                </Columns>
                                <HeaderStyle CssClass="gridheader" />
                                <EditRowStyle CssClass="editrow" />
                                <AlternatingRowStyle CssClass="Altrow" />
                            </asp:GridView>
                        </div>
                        <div id="dvSave" runat="server">
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
    </div>
</asp:Content>
