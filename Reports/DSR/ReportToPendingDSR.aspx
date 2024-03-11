<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    AutoEventWireup="true" CodeFile="ReportToPendingDSR.aspx.cs" Inherits="Reports_DSR_ReportToPendingDSR" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function ShowDrillDown() {
            var mainTable = PivotGrid.GetMainTable();
            DrillDownWindow.ShowAtPos(_aspxGetAbsoluteX(mainTable), _aspxGetAbsoluteY(mainTable));
        }
        //        //Progress Bar code
        //        var _updateProgressDiv;
        //        function pageLoad(sender, args) {
        //            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        //            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        //            _updateProgressDiv = $get('updateProgressDiv');
        //        }

        //        function beginRequest(sender, args) {
        //            var updateProgressDivBounds = Sys.UI.DomElement.getBounds(_updateProgressDiv);
        //            var x = (screen.width / 2) - Math.round(updateProgressDivBounds.width / 2);
        //            var y = (screen.height / 2) - Math.round(updateProgressDivBounds.height / 2);
        //            _updateProgressDiv.style.display = '';
        //            _updateProgressDiv.style.zindex = 999;
        //            Sys.UI.DomElement.setLocation(_updateProgressDiv, x, y);
        //        }

        // function endRequest(sender, args) { _updateProgressDiv.style.display = 'none'; }
        //Progress Bar code end
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="PivotGrid">
    <dx:ASPxPivotGrid ID="rptPendingDSR" runat="server" Width="100%" ClientInstanceName="PivotGrid"
        EnableViewState="False" EnableRowsCache="False">
        <OptionsView ShowHorizontalScrollBar="false" ShowColumnHeaders="False" ShowFilterHeaders="False"/>
        <Fields>
            <dx:PivotGridField ID="PivotGridField1" Area="RowArea" AreaIndex="0" FieldName="HierarchyLevelName"
                Caption="Hierarchy Level" Options-AllowDrag="False">
            </dx:PivotGridField>
            <dx:PivotGridField ID="PivotGridField2" Area="RowArea" AreaIndex="1" FieldName="LocationName"
                Caption="Location Name" Options-AllowDrag="False">
            </dx:PivotGridField>
            <%--<dx:PivotGridField ID="PivotGridField3" Area="RowArea" AreaIndex="2" FieldName="DSRMissingDate"
                Caption="Pending Month/Year">
            </dx:PivotGridField>--%>
            <dx:PivotGridField ID="field" Area="DataArea" AreaIndex="0" CellFormat-FormatType="Numeric"
                Caption="Pending DSR" FieldName="TotalMissingDSR" Options-AllowDrag="False">
                <CellStyle Cursor="pointer">
                </CellStyle>
            </dx:PivotGridField>
        </Fields>
        <%-- Dril down CODE_BEGIN --%>
        <ClientSideEvents CellClick="function(s, e) { 
	                                                GridView.PerformCallback(&quot;D|&quot; + e.ColumnIndex + &quot;|&quot; + e.RowIndex); 
	                                                ShowDrillDown();
	                                                var columnIndex = document.getElementById('ColumnIndex'),
		                                                rowIndex = document.getElementById('RowIndex');
	                                                if(!_aspxIsExists(columnIndex)) {
		                                                columnIndex = document.createElement(&quot;input&quot;);
		                                                rowIndex = document.createElement(&quot;input&quot;);
                                                		
		                                                columnIndex.type = &quot;hidden&quot;;
		                                                columnIndex.id = &quot;ColumnIndex&quot;;
		                                                columnIndex.name = &quot;ColumnIndex&quot;;		
		                                                rowIndex.type = &quot;hidden&quot;;
		                                                rowIndex.id = &quot;RowIndex&quot;;
		                                                rowIndex.name = &quot;RowIndex&quot;;		
                                                		
		                                                GridView.GetRootTable().appendChild(columnIndex);
		                                                GridView.GetRootTable().appendChild(rowIndex);
	                                                }
	                                                columnIndex.value = e.ColumnIndex;
	                                                rowIndex.value = e.RowIndex;
                                                }" />
        <%-- Dril down CODE End --%>
    </dx:ASPxPivotGrid>
    </div>
    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server" ASPxPivotGridID="rptPendingDSR"
        Visible="False">
    </dx:ASPxPivotGridExporter>
    <dx:ASPxPopupControl ID="ASPxPopupControl1" Modal="true" runat="server" Height="1px"
        AllowDragging="True" ClientInstanceName="DrillDownWindow" Left="200" Top="200"
        CloseAction="CloseButton" Width="50%" HeaderText="Drill Down Window">
        <ContentCollection>
            <dx:PopupControlContentControl ID="Popupcontrolcontentcontrol1" runat="server">
                <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" ClientInstanceName="GridView"
                    OnCustomCallback="ASPxGridView1_CustomCallback">
                    <ClientSideEvents EndCallback="function(s, e) {DrillDownWindow.SetClientWindowSize(-1, 100, 100);}" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Hierarchy Level" FieldName="HierarchyLevelName"
                            VisibleIndex="0">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Location Name" FieldName="LocationName" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Pending Month/Year" FieldName="DSRMissingDate"
                            VisibleIndex="2">
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="Total Pending DSR" FieldName="TotalMissingDSR"
                            VisibleIndex="3" Visible="false">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsLoadingPanel Mode="ShowOnStatusBar" />
                    <Styles>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                    </Styles>
                    <SettingsPager RenderMode="Classic">
                    </SettingsPager>
                </dx:ASPxGridView>
                <%-- export code start --%>
                <br />
                <div>
                    <strong>Export Drill down data:</strong>
                    <asp:Button ID="buttonSaveAsDrillDown" runat="server" OnClick="buttonSaveAsDrillDown_Click"
                        Text="Export In Excel" CssClass="buttonbg" ToolTip="Export and save" />
                </div>
                <%-- export code start --%>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <%-- code to export excel gridwiev start --%>
    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="ASPxGridView1">
    </dx:ASPxGridViewExporter>
    <%-- code to export excel gridwiev end --%>
</asp:Content>
