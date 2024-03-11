<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewEmailMaster.aspx.cs" Inherits="Masters_HO_Admin_ViewEmailMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/Assets/Jscript/dhtmlwindow.js") %>"></script>

    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/Assets/Jscript/modal.js") %>"></script>

    <script language="javascript" type="text/javascript">
        function HideDiv() {
            document.getElementById("dvDsplayBody").style.display = "none";
            return false;
        }
        function doWin(Msgtext) {

            //debugger;
            //document.getElementById("ctl00_contentHolderMain_lblEmailContent").text = Msgtext;
            //document.getElementById("ctl00_contentHolderMain_hdnBodyPart").value = Msgtext;
            document.getElementById("ctl00_contentHolderMain_lblEmailContent").innerHTML = Msgtext;
            var orig_div_cont = document.getElementById("dvDsplayBody").innerHTML;
            var myWin = window.open("", "myWin", "menubar,scrollbars,left=30px,top=40px,height=400px,width=600px");
            myWin.document.write('<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN""http://www.w3.org/TR/html4/strict.dtd">' +
            '<html><head><title>Email Content</title></head><body><div id="dest_div">' +
            'Some original Email Content. <br /><br /></div></body></html>');
            myWin.document.close();
            myWin.document.getElementById("dest_div").innerHTML += orig_div_cont;
            return true;
        }
        function ViewEmailContent(EmailKeyword) {
            //debugger;
            //window.location = "BulletinDetail.aspx?Key=" + EmailKeyword;
            //return false;
            newwindow = window.open("PopupViewEmailContent.aspx?Key=" + EmailKeyword, "myWin", "menubar,scrollbars,left=30px,top=40px,height=400px,width=600px");
            if (window.focus) { newwindow.focus() }
            return false;
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="hdnPagingCommand" runat="server" Value="0" />
            <uc1:ucMessage ID="ucMsg" runat="server" />
            <%-- fgerg --%>

            <div class="mainheading">
                Search On Email Description
            </div>
            <div class="export">
                <a href="ManagerEmailMaster.aspx" class="elink7">Add Email Master</a>
            </div>
            <div class="contentbox">
                <div class="H35-C3-S">
                    <ul>
                        <li class="text">Email Description :
                        </li>
                        <li class="field" style="height:auto">
                            <div>
                                <asp:TextBox ID="txtEmailDese" runat="server" TextMode="MultiLine" CssClass="form_textarea" Height="60"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="error" runat="server" ErrorMessage="Enter The Email Description"
                                    ControlToValidate="txtEmailDese" ValidationGroup="aa"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" ValidationGroup="aa"
                                    OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnViewAll" runat="server" Text="View All" CssClass="buttonbg" OnClick="btnViewAll_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="mainheading">
                Email List
            </div>

            <%--  <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" />--%>

            <%--      <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
            <div class="contentbox">
                <div class="grid1">
                    <asp:GridView ID="gvViewEmailMaster" runat="server" AutoGenerateColumns="False"
                        Width="100%" CellSpacing="1" CellPadding="4"
                        EditRowStyle-CssClass="editrow" GridLines="None"
                        HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                        AlternatingRowStyle-CssClass="Altrow"
                        HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle"
                        OnRowDataBound="gvViewEmailMaster_RowDataBound" AllowPaging="True"
                        OnPageIndexChanging="gvViewEmailMaster_PageIndexChanging"
                        OnRowCommand="gvViewEmailMaster_RowCommand">
                        <RowStyle CssClass="gridrow" />
                        <Columns>
                            <asp:TemplateField HeaderText="Email Keyword">
                                <ItemTemplate>
                                    <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label ID="lblEmailKeyword" runat="server" Text='<%#Eval("EmailOutboundTransKeyword") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email Desecription">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" CssClass="formfields" runat="server" Text='<%#Eval("EmailOutboundTransDesc") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <div style="width: 220px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("EmailOutboundTransDesc") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email From">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="formfields" Text='<%#Eval("EmailFrom") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label ID="Label3" runat="server" Text='<%#Eval("EmailFrom") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email Subject">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="formfields" Text='<%#Eval("EmailSubjectLine") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <div style="width: 220px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label ID="Label4" runat="server" Text='<%#Eval("EmailSubjectLine") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email Expiry Hours">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox4" runat="server" CssClass="formfields" Text='<%#Eval("EmailExpiryHrs") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <div style="width: 220px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label ID="Label5" runat="server" Text='<%#Eval("EmailExpiryHrs") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email Body">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnEmailBody" runat="server" CommandName="Isbody" CssClass="elink5" CommandArgument='<%#Eval("EmailOutboundTransKeyword") %>'>View</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:HyperLinkField HeaderText="Email Body" Text="View" />--%>
                            <asp:TemplateField HeaderText="Status">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%#Eval("Status") %>' />
                                    <%--<asp:TextBox ID="TextBox4" runat="server" Text='<%#Eval("Status") %>'></asp:TextBox>--%>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtnStatus" runat="server" CommandName="Status" CommandArgument='<%#Eval("Status") %>' ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="elink5" PostBackUrl='<%#  "~/Masters/HO/Admin/ManagerEmailMaster.aspx?EmailKeyword=" +Eval("EmailOutboundTransKeyword")%>'>Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" />
                        <EditRowStyle CssClass="editrow" />
                    </asp:GridView>
                </div>
            </div>
            <%-- </ContentTemplate>`
                                                      
                                                    </asp:UpdatePanel>--%>

            <div id="dvDsplayBody" class="updateProgress" style="display: none; background-color: #d0e1ee; position: absolute; top: 195px; left: 70px; right: 73px; padding: 0px; border: 0px solid #7cbdef">
                <asp:Label ID="lblEmailContent" runat="server"></asp:Label>
            </div>
        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="ExportToExcel" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
