<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucUploadMultipleExcelFile.ascx.cs"
    Inherits="UserControls_ucUploadMultipleExcelFile" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%-- #CC02 Add Start --%>
<script type="text/javascript">
    function showConfirm(tblstring, filename) {
        var hdnCurrentfilename = document.getElementById("<%= hdnCurrentFileName.ClientID %>")
        hdnCurrentfilename.value = filename;
        var msgbox = document.getElementById('ctl00_contentHolderMain_ucMsg_pnlUcMessageBox');
        <%-- #CC04 Add Start --%>
        if (msgbox != null)
            if (msgbox.style.display == "none") <%-- #CC04 Add End --%>
                msgbox.style.display = "none";
        if (confirm(tblstring) == true) {
            var clickButton = document.getElementById("<%= btnAdd2.ClientID %>");
            clickButton.click();
            return true;
        }
        else {
            var ConfCancel = document.getElementById("<%= btnConfCancel.ClientID %>");
            ConfCancel.click();
            return false;
        }
        return false;
    }
</script>
<%-- #CC02 Add End --%>
<asp:UpdatePanel ID="UpdatePanelUC" runat="server">
    <ContentTemplate>
        <table cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td>
                    <table cellpadding="4" cellspacing="0" width="100%">
                        <tr>
                            <td align="right" valign="top" width="2%"></td>
                            <td align="left" valign="top" width="28%">
                                <div>
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form_file" />
                                </div>
                                <div>
                                    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:Label runat="server" ID="lblMsg" CssClass="error" EnableViewState="false"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td align="right" valign="top" width="2%"></td>
                            <td align="left" valign="top">
                                <asp:Button ID="btnAdd" runat="server" CssClass="buttonbg" OnClick="btnAdd_Click"
                                    Text="Add Excel File" CausesValidation="false" ToolTip="Add Excel File" Width="94px" />
                                <asp:Button ID="btnAdd2" Style="display: none" runat="server" CssClass="buttonbg" OnClick="btnAdd2_Click"
                                    Text="Add Excel File2" CausesValidation="false" ToolTip="Add Excel File2" Width="94px" />
                                <%--#CC03 Add Start--%>
                                <asp:Button ID="btnConfCancel" Style="display: none" runat="server" CssClass="buttonbg" OnClick="btnConfCancel_Click"
                                    CausesValidation="false" />
                                <asp:HiddenField ID="hdnCurrentFileName" runat="server" />
                                <%--#CC03 Add End--%>

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div class="grid2">
                        <asp:GridView ID="grdvwFile" runat="server" AlternatingRowStyle-CssClass="gridrow1"
                            AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                            EmptyDataText="No Record found" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" OnRowCommand="grdvwFile_RowCommand"
                            OnRowDeleting="grdvwFile_RowDeleting" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:TemplateField HeaderText="S.No." ItemStyle-Width="5%">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Wrap="False" />
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                        <asp:HiddenField ID="hdnPath" runat="server" Value='<%#Eval("SystemFileName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Uploaded File">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("FileName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="10%">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblDelete" runat="server" CssClass="elink2" CommandArgument='<%#Eval("FileName") %>'
                                            CommandName="Delete">Delete</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="gridrow1" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnAdd" />
        <asp:PostBackTrigger ControlID="btnAdd2" />
    </Triggers>
</asp:UpdatePanel>
