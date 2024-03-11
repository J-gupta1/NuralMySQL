<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPage.aspx.cs" Inherits="Masters_TestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

   <script type="text/javascript" src="../../Assets/Jscript/jquery-1.4.4.min.js"></script>
      <script type="text/javascript">
      
        $(document).ready(function() {
              $("[id$=BtnTransfer]").click(function() {


          });
      });
      
      </script>
    <style type="text/css">
        .style1
        {
            height: 243px;
        }
    </style>
</head>
<body dir="ltr">
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hdnName" runat="server" />
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="style1">
                            </td>
                            <td class="style1">
                                <asp:GridView ID="grdColor" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                                    RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" GridLines="None" AlternatingRowStyle-CssClass="gridrow1"
                                    RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                    CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="False"
                                    AllowPaging="True" SelectedStyle-CssClass="gridselected" DataKeyNames="ColorID"
                                    PageSize='<%$ AppSettings:GridViewPageSize %>' EmptyDataText="No record found">
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridrow"></RowStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Color Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ColorName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("ColorName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select" SortExpression="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                               
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Color ID">
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("ColorID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="PagerStyle" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridheader"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="gridrow1"></AlternatingRowStyle>
                                </asp:GridView>
                                <br />
                                <br />
                                &nbsp;<br />
                                <asp:Label ID="lb1" runat="server" Text="first"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lb2" runat="server" Text="second"></asp:Label>
                                <asp:Button ID="btn1" runat="server" Text="click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btn2" runat="server" Text="Count" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Btn3" runat="server" Text="check" />
                                &nbsp;<asp:TextBox ID="Txt1" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="hdnColumn" runat="server" />
                                <asp:CheckBox ID="CheckBox2" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="div1"  runat = "server">
     <asp:Label ID="Label4" runat="server" Text="first"></asp:Label>
        <asp:Button ID="Button1" runat="server" Text="check" />
    </div>
    </form>
</body>
</html>
