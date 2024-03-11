<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BroadcastMessage.aspx.cs" Inherits="Masters_Broadcast_BroadcastMessage" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function showStates(control) {

            var ddlBroadcastType = document.getElementById("ctl00_contentHolderMain_ucMessage1_pnlUcMessageBox");
            if (ddlBroadcastType != null) {
                ddlBroadcastType.style.display = "none";
            }
            var ddlBroadcastType = control;
            var dvStates = document.getElementById('ctl00_contentHolderMain_dvState');
            var e = document.getElementById(ddlBroadcastType);
            var value = e.options[e.selectedIndex].value;
            var text = e.options[e.selectedIndex].text;
            if (text.toLowerCase() == "state") {
                document.getElementById('<%=dvState.ClientID %>').style.display = "block";
            }
            else {
                document.getElementById('<%=dvState.ClientID %>').style.display = "none";
            }

            return false;
        }

        function CheckUnCheckAll() {
            var ddlBroadcastType = document.getElementById("ctl00_contentHolderMain_ucMessage1_pnlUcMessageBox");
            if (ddlBroadcastType != null) {
                ddlBroadcastType.style.display = "none";
            }
            var chkselectall = document.getElementById('<%=chckAllState.ClientID%>');
            var chkStates = document.getElementById('<%=chcklistState.ClientID%>');
            var chkStatesCount = document.getElementById('<%=hdnStateCount.ClientID%>');

            var checkuncheck;
            if (chkselectall.checked == true) {
                checkuncheck = true;
            }
            else if (chkselectall.checked == false) {
                checkuncheck = false;
            }
           <%-- for (i = 0; i < chkStates.rows.length; i++) {
                document.getElementById('<%=chcklistState.ClientID%>' + '_' + i).checked = checkuncheck;
            }--%>
            for (i = 0; i < parseInt(chkStatesCount.value) ; i++) {
                document.getElementById('<%=chcklistState.ClientID%>' + '_' + i).checked = checkuncheck;
            }
            //return false;
        }

        function checkSelection(ControlID) {
            var ID = ControlID
            var chkselectall = document.getElementById('<%=chckAllState.ClientID%>');
            var chkStates = document.getElementById('<%=chcklistState.ClientID%>');
            var chkStatesCount = document.getElementById('<%=hdnStateCount.ClientID%>');
            var ischecked = false;
            if (document.getElementById(ControlID).checked == true) {

                for (i = 0; i < parseInt(chkStatesCount.value) ; i++) {
                    if (document.getElementById('<%=chcklistState.ClientID%>' + '_' + i).checked == false) {
                        ischecked = false;
                        break;
                    }
                    else if (document.getElementById('<%=chcklistState.ClientID%>' + '_' + i).checked == true) {
                        ischecked = true;
                    }
                }
                if (ischecked == true) {
                    chkselectall.checked = true;
                }
                else {
                    chkselectall.checked = false;
                }
            }
            else {
                chkselectall.checked = false;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="mainheading">
        Broadcast SMS
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lbltextMessageHeading" runat="server" Text="">Broadcast SMS: <span class="error">*</span></asp:Label>
                </li>
                <li class="field" style="height:auto">
                    <uc4:ucTextboxMultiline ID="txtMessage" runat="server" IsRequired="true" CharsLength="250"
                        TextBoxWatermarkText="Please enter Broadcast SMS." ErrorMessage="Please enter Broadcast SMS."
                        ValidationGroup="Add" />
                    <%-- <asp:TextBox ID="txtMessage" TextMode="MultiLine" runat="server" CssClass="form_textarea"
                                                                MaxLength="500"></asp:TextBox>
                                                        </div>
                                                        <div>
                                                            <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtMessage"
                                                                CssClass="error" Display="Dynamic" ErrorMessage="Please enter Broadcast SMS."
                                                                SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>--%>                                  
                </li>
                <li class="text">
                    <asp:Label ID="lblTypeHeading" runat="server" Text="">Broadcast Type: <span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlBroadcastType" runat="server" CssClass="formselect" onChange="return showStates(this.id);">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBroadcastType"
                            CssClass="error" Display="Dynamic" ErrorMessage="Please select Type." SetFocusOnError="true"
                            InitialValue="0" ValidationGroup="Add"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="buttonbg"
                            Text="Submit" CausesValidation="true" ValidationGroup="Add" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" CssClass="buttonbg"
                            Text="Reset" CausesValidation="false" />
                    </div>
                </li>
            </ul>
            <ul>
                 <li class="text-field3" style="height:auto">
                    <div id="dvState" runat="server" style="display: none;">
                        <div class="gridborder padding">
                            &nbsp;<asp:CheckBox ID="chckAllState" runat="server" Text="Select All" onclick=" CheckUnCheckAll();"></asp:CheckBox>
                            <br />
                            <asp:HiddenField ID="hdnStateCount" runat="server" Value="0" />
                            <asp:CheckBoxList ID="chcklistState" runat="server" CellPadding="3" CellSpacing="0" Width="100%"
                                RepeatDirection="Horizontal" RepeatColumns="4" CssClass="checkboxlist">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
