<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewTargetDetail.aspx.cs"
    Inherits="ViewTargetDetail" %>

<%--#CC01 Add Start --%>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--#CC01 Add End--%>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">--%>
<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <%--Add START  --%>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <%--Add END  --%>
    <title></title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

        <asp:Label ID="lblHeader" runat="server" CssClass="error"></asp:Label>
        <%--#CC01 Add Start--%>
        <asp:ScriptManager ID="scriptid" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMessage1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--#CC01 Add End--%>

        <div class="grid1">
            <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="False" AlternatingRowStyle-CssClass="Altrow"
                bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="top"
                RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="top"
                SelectedStyle-CssClass="gridselected" Width="100%" OnRowDataBound="grdDetail_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="TargetType" HeaderText="Target Type" />
                    <asp:BoundField DataField="TargetCategory" HeaderText="Target Category" />
                    <asp:BoundField DataField="TargetBasedOn" HeaderText="Target BasedOn" />
                    <asp:BoundField DataField="TargetFrom" HeaderText="Target From" />
                    <asp:BoundField DataField="TargetTo" HeaderText="Target To" />
                    <%-- #CC01 Comment Start   <asp:BoundField DataField="Target" HeaderText="Target" />  #CC01 Comment End --%>
                    <%-- #CC01 Add Start --%>
                    <asp:TemplateField HeaderText="Target">
                        <ItemTemplate>
                            <asp:Label ID="lblTarget" runat="server" Text='<%#Eval("Target")%>'></asp:Label>
                            <asp:TextBox ID="txtTarget" runat="server" Text='<%#Eval("Target")%>' Enabled="false" CssClass="formfields" MaxLength="10" Visible="false"></asp:TextBox>

                            <cc1:FilteredTextBoxExtender ID="txtTarget_FilteredTextBoxExtender" runat="server"
                                TargetControlID="txtTarget" ValidChars="0123456789.">
                            </cc1:FilteredTextBoxExtender>
                            <br />
                            <asp:RequiredFieldValidator ID="rqTarget" runat="server" ControlToValidate="txtTarget"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please enter target."
                                SetFocusOnError="true" ValidationGroup="Update"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hdnTargetDetailID" runat="server" Value='<%#Eval("TargetDetailID")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- #CC01 Add End --%>
                </Columns>
            </asp:GridView>
        </div>
        <div class="clear margin-top"></div>
        <%--#CC01 Add Start--%>
        <asp:Button ID="btnUpdate" CssClass="buttonbg" runat="server" Text="Update"
            ValidationGroup="Update" CausesValidation="true" OnClick="btnUpdate_Click" Visible="false" />
        <%--#CC01 Add End--%>
    </form>
</body>
</html>
