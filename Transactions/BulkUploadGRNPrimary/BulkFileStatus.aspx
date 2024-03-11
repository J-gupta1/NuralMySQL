<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BulkFileStatus.aspx.cs" Inherits="Transactions_BulkUploadGRNPrimary_BulkFileStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <script language="javascript" type="text/javascript">
        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            //            $("#<%=grdvwFile.ClientID%> div").each(function() {
            //            
            //            });

            if (div.style.display == "none") {
                div.style.display = "block";
                img.src = "../../Assets/DHTML_img/minus.gif";
            } else {
                div.style.display = "none";
                img.src = "../../Assets/DHTML_img/plus.gif";
            }
        }
    </script>
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>          
                <uc1:ucMessage ID="ucMsg" runat="server" />           
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        View Status
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C4-S">            
            <ul>
                <li class="text"> From <asp:Label ID="lblOSDate" runat="server" AssociatedControlID="ucDatePicker" CssClass="formtext">Date:<span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Pleaes enter date."
                        ValidationGroup="Save" />
                </li>
                <li class="text">To Date : <span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePickerTo" runat="server" ErrorMessage="Pleaes enter date."
                        ValidationGroup="Save" />
                </li>             
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" Text="Show" OnClick="btnShow_Click" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnReset" runat="server" CssClass="buttonbg" CausesValidation="false"
                            Text="Reset" OnClick="btnReset_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="grdvwFile" runat="server" AlternatingRowStyle-CssClass="Altrow"
                AutoGenerateColumns="false" CellPadding="4"
                EmptyDataText="No Record found" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                FooterStyle-VerticalAlign="Top" GridLines="Both" HeaderStyle-CssClass="gridheader"
                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow"
                RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" SelectedStyle-CssClass="selectedrow"
                Width="100%" OnRowDataBound="grdvwFile_RowDataBound" DataKeyNames="BulkFileId">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="20px">
                        <ItemTemplate>
                            <a href="JavaScript:divexpandcollapse('div<%# Eval("BulkFileId") %>');">
                                <img id="imgdiv<%# Eval("BulkFileId") %>" width="9px" border="0" src="../../Assets/DHTML_img/plus.gif" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No." HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="NDS" HeaderText="NDS" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SalesChannelName" HeaderText="Warehouse" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="UploadStatus" HeaderText="Processing Status" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="TotalFiles" HeaderText="No. of Files" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" HeaderStyle-HorizontalAlign="Left" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <tr>
                                <td colspan="100%">
                                    <div id="div<%# Eval("BulkFileId") %>" style="display: none;">
                                        <asp:GridView ID="gvChildGrid" runat="server" CellPadding="4" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1"
                                            GridLines="Both" Width="90%" AlternatingRowStyle-CssClass="Altrow" SelectedStyle-CssClass="selectedrow" RowStyle-CssClass="gridrow"
                                            HeaderStyle-CssClass="gridheader">
                                            <RowStyle />
                                            <AlternatingRowStyle />
                                            <HeaderStyle Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File">
                                                    <ItemTemplate>
                                                        <div class="elink2">
                                                            <%#Eval("ExeclFileName")%>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="UploadStatus" HeaderText="Processing Status" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle" />
                                                <asp:TemplateField HeaderText="Error File">
                                                    <ItemTemplate>
                                                        <div class="elink2">
                                                        <%#Eval("ErrorFileName")%>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="PagerStyle" />
                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                <AlternatingRowStyle CssClass="Altrow" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
