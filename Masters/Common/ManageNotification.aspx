<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageNotification.aspx.cs" Inherits="Masters_Common_ManageNotification" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <script type="text/javascript">
        //    $(document).ready(function() {
        //    $("#<%=divTv.ClientID %>").hide();
        //    $("#<%=divtxt.ClientID %>").hide();
        //});

        function disableControl() {
            $("#<%=divTv.ClientID %>").hide();
            $("#<%=divtxt.ClientID %>").hide();
            var SelValue = $('#<%=rblAccessType.ClientID %>').find(":checked").val();
            if (SelValue == 2) {
                $("#<%=divTv.ClientID %>").show();
            }
            else if (SelValue == 3) {
                $("#<%=divtxt.ClientID %>").show();
            }
        }


        function CountNotification() {
            var TextValue = $('#<%=txtNotificationText.ClientID %>').val();
            if (TextValue.length > 500) {
                $('#<%=txtNotificationText.ClientID %>').val(TextValue.substr(0, 500))
            }
            var TextValueNew = $('#<%=txtNotificationText.ClientID %>').val();
            $('#<%=lblCount.ClientID %>').text('Total Words = ' + TextValueNew.length + ' / 500')
        }
        
    </script>

    <asp:Label ID="lbltvErrorMsg" runat="server" Visible="false"></asp:Label>
    <asp:UpdatePanel runat="server" ID="updMsg1" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add Notification                                           
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C2">
            <ul>
                <li class="text">
                    <asp:Label ID="lblAccessType" runat="server" CssClass="formtext">Access Type: <span class="error">*</span>
                    </asp:Label>
                </li>
                <li class="field">
                    <div>
                        <asp:RadioButtonList ID="rblAccessType" CssClass="radio-rs" CellPadding="2" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="True" OnSelectedIndexChanged="rblAccessType_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="1">Allow To All</asp:ListItem>
                            <%--<asp:ListItem Value="2">Specific</asp:ListItem>--%>
                            <asp:ListItem Value="3">Specific RSP</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </li>
                <li class="text">Status:
                </li>
                <li class="field">
                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                </li>
            </ul>
            <div class="clear"></div>
            <ul>
                <li class="text" style="height: auto">
                    <div dir="ltr" id="dvRspLable" runat="server" visible="false">
                        RSP Code:<span class="error">*</span>
                        <br />
                        <span class="error">(Put Comma Separated or each one in new line)</span>
                    </div>
                </li>
                <li class="text-field" style="height: auto">
                    <div id="divTv" runat="server" visible="false">
                        <div style="overflow-y: scroll; overflow-x: hidden; height: 220px; width: 100%; border: 1px solid #dddddd">
                            <asp:TreeView ID="tvISP" runat="server" ExpandDepth="0" ShowCheckBoxes="All">
                            </asp:TreeView>
                        </div>
                    </div>
                    <div id="divtxt" runat="server" visible="false">
                        <asp:TextBox ID="txtISP" runat="server" TextMode="MultiLine" Height="110px"></asp:TextBox>
                    </div>
                </li>
            </ul>
            <div class="clear"></div>
            <ul>
                <li class="text" style="height: auto">
                    <asp:Label ID="lblDescription" runat="server" CssClass="formtext">Notification:<span class="error">*</span></asp:Label>
                </li>
                <li class="text-field" style="height: auto">
                    <asp:TextBox ID="txtNotificationText" onkeyup="CountNotification()" onblur="CountNotification()"
                        runat="server" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>
                    <asp:Label ID="lblCount" runat="server" ForeColor="Red">Total Words = 0 / 500</asp:Label>
                </li>
            </ul>
        </div>
    </div>
    <div class="margin-bottom">
        <div class="float-margin">
            <asp:Button ID="btnCreate" runat="server" CausesValidation="true" CssClass="buttonbg"
                Text="Submit" ToolTip="Add Bulletin" ValidationGroup="AddUserValidationGroup"
                OnClick="btnCreate_Click" />
        </div>
        <div class="float-margin">
            <asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="buttonbg"
                Text="Reset" ToolTip="RESET" OnClick="btnReset_Click" />
        </div>
        <div class="clear"></div>
    </div>

</asp:Content>
