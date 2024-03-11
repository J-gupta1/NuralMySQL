<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ReportQueue.aspx.cs" Inherits="ClientServices_Common_ReportQueue" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker2.ascx" TagName="ucDatePicker2" TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/ucPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
      <div>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                     <tr>
                            <td align="left" valign="top">
                               <uc4:ucMessage ID="ucMessage1" runat="server" />
                            </td>
                        </tr>
                    </table>
            </div>
      <div class="float-right" style="padding-top:13px;">
    <asp:LinkButton ID="LnkExporttoExcel" runat="server" Text="Export to Excel" CssClass="elink2" Style="color: green" OnClick="LnkExporttoExcel_Click"></asp:LinkButton>
    </div>
            <div class="box1" runat="server" id="divgrd">
                <div class="mainheading" style="margin-top:5px;">
                    <div class="float-left">
                       Report Queue</div>
                    <div class="export">
                      
                    </div>
                </div>
                <div class="contentbox">
                    <div class="grid2">
                        <asp:GridView ID="grdvList" runat="server" AlternatingRowStyle-CssClass="Altrow"
                            AutoGenerateColumns="False" CellPadding="4" CellSpacing="1" DataKeyNames="FileName,status"
                            EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found" GridLines="None"
                            HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow" SelectedRowStyle-CssClass="selectedrow"
                            Width="100%" OnRowDataBound="grdvList_RowDataBound" OnRowCommand="grdvList_RowCommand">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:ButtonField CausesValidation="false" CommandName="Select" HeaderStyle-HorizontalAlign="Left"
                                    Text="Select" Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:ButtonField>
                               
                                <asp:BoundField DataField="RequestForReport" HeaderStyle-HorizontalAlign="Left" HeaderText="Request For Report">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RequestedProcedure" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Request For Procedure" Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                               
                                <asp:BoundField DataField="CreatedOn" HeaderStyle-HorizontalAlign="Left" HeaderText="Request Date">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CreatedBy" HeaderStyle-HorizontalAlign="Left" HeaderText="Request By">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProcessStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Left" ControlStyle-CssClass="elink2">
                                    <ItemTemplate>
                                       <asp:LinkButton ID="lnkbtnDownload" runat="server" Visible="false" Text="Click Here To Download" CommandName="Download" OnClick="lnkbtnDownload_Click" CommandArgument='<%# Eval("FileName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ProcessRemarks" HeaderStyle-HorizontalAlign="Left" HeaderText="Process Remarks"
                                    Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProcessOn" HeaderStyle-HorizontalAlign="Left" HeaderText="Process ON"
                                    Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <SelectedRowStyle CssClass="selectedrow" />
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                        </asp:GridView>
                    </div>
                    </div>
                    <div class="clear" style="height:0px">
                    </div>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>
                </div>
           
       
</asp:Content>
