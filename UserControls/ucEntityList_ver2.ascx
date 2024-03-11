<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucEntityList_ver2.ascx.cs"
    Inherits="ucEntityList_ver2" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript">
    //To display the Product category which are selected by end user we attach Item selected event
    function ItemSelected(s, e, dropDown) {
        var selectedItems = s.GetSelectedItems();
        var dropDownText = '';
        if (selectedItems.length > 0) {
            dropDownText = selectedItems[0].text;
            for (var i = 1; i < selectedItems.length; i++)
                dropDownText += ',' + selectedItems[i].text;
        }
        if (dropDownText.length > 0) {
            dropDown.SetText(dropDownText);
        }
        else {
            dropDownText = "Please choose";
            dropDown.SetText(dropDownText);
        }
        // OnLinkButtonClick();
    }
    function FirePostBack(s, e, dropDown) {
        var v = document.getElementById('<%= Button1.ClientID %>');
        if (dropDown.autoPostBack = true) {
            if (v != null) {
                __doPostBack(v.name, "OnClick");
                var selectedItems = s.GetSelectedItems();
                var dropDownText = '';
                if (selectedItems.length > 0) {

                    dropDownText = selectedItems[0].text;
                    for (var i = 1; i < selectedItems.length; i++)
                        dropDownText += ',' + selectedItems[i].text;

                    dropDown.SetText(selectedItems[0].text);
                }
            }
        }
    }
    var nullText = "Please Select";

    function OnLostFocus(s, e) {
        if (s.GetValue() != "" && s.GetValue() != null)
            return;

        var input = s.GetInputElement();
        input.style.color = "gray";
        input.value = nullText;
    }

    function OnGotFocus(s, e) {
        var input = s.GetInputElement();
        if (input.value == nullText) {
            input.value = "";
            input.style.color = "black";
        }
    }

    function OnInit(s, e) {
        OnLostFocus(s, e);
    }
    function ValChanged(s, e) {
        var v = document.getElementById('<%= Button1.ClientID %>');
        if (s.autoPostBack == true) {
            if (v != null) {
                __doPostBack(v.name, "OnClick");
            }
        }
    }
</script>

<dxe:ASPxComboBox runat="server" ID="ZedComboBoxDevEx" DropDownStyle="DropDownList"
     EnableSynchronization="False" CssClass="formselect" Width="93%" TextField="CompanyDisplayName"
    ValueField="EntityID" OnSelectedIndexChanged="ZedComboBoxDevEx_SelectedIndexChanged"> <%--#CC11: Style="width: 150px;" removed CssClass and Width added--%>
    <ClientSideEvents Init="OnInit" LostFocus="OnLostFocus" GotFocus="OnGotFocus" ValueChanged="ValChanged" />
    <ValidationSettings ErrorFrameStyle-Font-Size="11px" Display="Dynamic" ErrorTextPosition="Bottom" ErrorFrameStyle-Font-Italic="true"
        ErrorDisplayMode="Text"> <%--#CC11: added Display="Dynamic" changed ErrorFrameStyle-Font-Size="7.5" to "11px"--%>
    </ValidationSettings>
</dxe:ASPxComboBox>
<%-- #Ch06: Event OnValueChanged="ddlSC_SelectedIndexChanged" created. --%>
<dxe:ASPxDropDownEdit ID="ddlSC" Text="Please Choose" runat="server" Font-Size="11px"
    CssClass="formselect" ReadOnly="True" meta:resourcekey="ddlSCResource1"><%-- #CC11: removed Style="border: 1px solid #a2a7a0" added CssClass--%>
    <DropDownWindowTemplate>
        <dxe:ASPxListBox ID="lstSC" ClientInstanceName="lstProducts" runat="server" SelectionMode="Multiple"
             ValueField="EntityIDTypeId" TextField="CompanyDisplayName" meta:resourcekey="lstSCResource1"
            OnValueChanged="ddlSC_SelectedIndexChanged">
        </dxe:ASPxListBox>
    </DropDownWindowTemplate>
    <ValidationSettings ErrorFrameStyle-Font-Size="11px" Display="Dynamic" ErrorTextPosition="Bottom" ErrorFrameStyle-Font-Italic="true"
        ErrorDisplayMode="Text"><%--#CC11: added Display="Dynamic" and changed ErrorFrameStyle-Font-Size="7.5" to "11px"--%>
    </ValidationSettings>
</dxe:ASPxDropDownEdit>
<asp:RequiredFieldValidator ID="reqddlSC" runat="server" ControlToValidate="ddlSC"
    ErrorMessage="Please select!" meta:resourcekey="reqddlSCResource1"></asp:RequiredFieldValidator>
<asp:Button ID="Button1" Style="display: none;" runat="server" OnClick="Button1_Click"
    Text="Button123" meta:resourcekey="Button1Resource1" />