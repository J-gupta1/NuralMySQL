<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucPagingControl.ascx.cs"
    Inherits="UserControls_ucPagingControl" %>
<div class="gridpager1" align="center">
    <asp:Panel ID="pnlPaging" runat="server" DefaultButton="Go">
        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
            <tr>
                <td width="4%" align="left">
                    <asp:LinkButton ID="lnkfirst" CssClass="normaltext" runat="server" CausesValidation="False"
                        OnClick="lnkfirst_Click">
                        <asp:Image ID="imgFirst" runat="server" alt="" border="0" hspace="5" />
                    </asp:LinkButton>
                </td>
                <td width="7%" align="left">
                    <asp:LinkButton ID="lnkPrev" class="whitetext" runat="server" CausesValidation="False"
                        OnClick="lnkPrev_Click">
                        <asp:Image ID="imgPrev" runat="server" alt="" border="0" />
                    </asp:LinkButton>
                </td>
                <td width="20%" align="left" valign="middle">
                    <asp:Label ID="lblPageNo" CssClass="whitetext" runat="server"></asp:Label>
                </td>
                <td width="25%" align="left" valign="middle">
                    <asp:Label ID="lblTotalRecord" CssClass="whitetext" runat="server"></asp:Label>
                </td>
                <td width="4%" align="left">
                    <asp:LinkButton ID="lnkNext" class="whitetext" runat="server" CausesValidation="False"
                        OnClick="lnkNext_Click">
                        <asp:Image ID="imgNaxt" runat="server" alt="" border="0" />
                    </asp:LinkButton>
                </td>
                <td width="15%" align="left">
                    <asp:LinkButton ID="lnkLast" runat="server" CausesValidation="False" OnClick="lnkLast_Click">
                        <asp:Image ID="imgLast" runat="server" alt="" border="0" />
                    </asp:LinkButton>
                </td>
                <td width="30%" align="left" class="whitetext" style="padding-left: 10px;">&nbsp;Jump to page&nbsp; 
                <asp:TextBox onkeypress="if (event.keyCode < 45 || event.keyCode > 57) event.returnValue = false;"
                    ID="txtGo" runat="server" CssClass="form_input2" MaxLength="4" Width="30px"></asp:TextBox>
                    <asp:ImageButton ID="Go" runat="server" OnClick="Go_Click"
                        ImageAlign="AbsMiddle" CausesValidation="False" BorderWidth="0px"></asp:ImageButton>
                    <asp:TextBox ID="txtHid" runat="server" Width="0px" BorderWidth="0" Visible="False"
                        Enabled="False" ReadOnly="True" Height="0px"></asp:TextBox>
                    <asp:CompareValidator ID="Comparevalidator2" CssClass="mandatory" runat="server"
                        Display="None" ControlToValidate="txtGo" ErrorMessage="Enter Valid Page Number"
                        ControlToCompare="txtHid" Type="Integer" Operator="LessThanEqual"></asp:CompareValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
