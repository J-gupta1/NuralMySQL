<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard_DashBoard" MasterPageFile="~/CommonMasterPages/ReportPage.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dxpgex" %>
 <%@ Register Assembly="DevExpress.XtraCharts.v10.1.Web, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v10.1.Web, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v10.1.Web, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>


<%@ Register Assembly="DevExpress.XtraCharts.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="dxcharts" %>
<%@ Register Assembly="DevExpress.XtraCharts.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.XtraCharts.v10.1.Web, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
   
    <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="965" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                   <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <%--    <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0" style="float: left;">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" class="tableposition">
                                                                            <div class="mainheading">
                                                                               &nbsp;Select</div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" class="tableposition">
                                                              <asp:UpdatePanel runat="server" ID="updYear" UpdateMode="Conditional">
                                                                          <ContentTemplate>
                                                                <div class="contentbox">
                                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                       
                                                                        <tr>
                                                                       
                                                                            <td class="formtext" valign="top" align="right"  >
                                                                               <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr class="searchbg">
                                                                                        <td width="5%">
                                                                                        </td>
                                                                                        <td align="left" valign="middle" height="35" width="4%">
                                                                                            Year
                                                                                        </td>
                                                                                        
                                                                                        <td width="15%" align="left" valign="middle">
                                                                                             <div style="float:left; width:135px;"> <asp:DropDownList ID="cmbYear" 
                                                                                                     runat="server" CssClass="form_select" AutoPostBack="True" onselectedindexchanged="cmbYear_SelectedIndexChanged" 
                                                                                                >
                                                                                            </asp:DropDownList></div>
                                                                                            
                                                                                        </td>
                                                                                       
                                                                                        <td width="5%" align="left" valign="middle">
                                                                                            Month
                                                                                        </td>
                                                                                        <td width="15%" align="left" valign="middle">
                                                                                             <div style="float:left; width:135px;"> <asp:DropDownList ID="cmbMonth" runat="server" CssClass="form_select">
                                                                                            </asp:DropDownList></div>
                                                                                        </td>
                                                                                        
                                                                                        <td width="5%" align="left" valign="middle">
                                                                                            Location</td>
                                                                                        
                                                                                        <td width="15%" align="left" valign="middle">
                                                                                            <div style="float:left; width:135px;">  <asp:DropDownList ID="cmbRegion" runat="server" CssClass="form_select5">
                                                                                            </asp:DropDownList></div>
                                                                                        </td>
                                                                                          <asp:Panel ID = "pnlBrand"  runat = "server" >
                                                                                          <td width="5%" align="left" valign="middle">
                                                                                            Brand
                                                                                        </td>
                                                                                        <td width="15%" align="left" valign="middle">
                                                                                             <div style="float:left; width:135px;"> <asp:DropDownList ID="cmbBrand" runat="server" CssClass="form_select">
                                                                                            </asp:DropDownList></div>
                                                                                        </td>
                                                                                         </asp:Panel>
                                                                                        <td width="40%" align="left" valign="middle">
                                                                                            <asp:Button ID="btnGetData" runat="server" CssClass="buttonbg" OnClick="btnGetData_Click"
                                                                                                Text="Show" />
                                                                                        </td>
                                                                                      
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>   </ContentTemplate>
                                                                <Triggers>
                                                    <asp:PostBackTrigger  ControlID="btnGetData"  />
                                                </Triggers>
                                                                                        </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" height="10">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="tableposition">
                                        <div class="mainheading">
                                            List</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="contentbox">
                                         <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td colspan="3"  height="10">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" colspan="3" style="width: 100%;">
                                                                            <table cellpadding="0" cellspacing="0" width="100%" class="tablebackgrond">
                                                                                <tr>
                                                                                    <td colspan="2" height="10">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 50%" align="center">
     <asp:UpdatePanel ID="updPrimary" runat="server" UpdateMode="Conditional">
     <ContentTemplate>         
        <dxchartsui:WebChartControl ID="WCPrimarySales" runat="server" 
             AppearanceName="Light" Height="350px" PaletteName="Metro" SeriesDataMember="type"
   SeriesSorting="Ascending" Width="400px">
     
<SeriesTemplate><ViewSerializable><cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView></ViewSerializable>
<LabelSerializable><cc1:SideBySideBarSeriesLabel LineVisible="True"><FillStyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable></FillStyle></cc1:SideBySideBarSeriesLabel></LabelSerializable>
<PointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></PointOptionsSerializable>
<LegendPointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></LegendPointOptionsSerializable>
</SeriesTemplate>

<FillStyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</FillStyle>
     <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
         direction="LeftToRight" equallyspaceditems="False"></legend>
 <Titles>


<cc1:ChartTitle Text="Primary Sales"  Font="Tahoma, 15pt" ></cc1:ChartTitle>

</Titles>
<DiagramSerializable>
<cc1:XYDiagram>
<AxisX VisibleInPanesSerializable="-1"><Range SideMarginsEnabled="True"></Range></AxisX>

<AxisY VisibleInPanesSerializable="-1"><Range SideMarginsEnabled="True"></Range></AxisY>
</cc1:XYDiagram>
</DiagramSerializable>
 </dxchartsui:WebChartControl>                                                               
 </ContentTemplate>
 </asp:UpdatePanel>
 </td>
    <td style="width: 50%">
    <asp:UpdatePanel ID="updStock" runat="server" UpdateMode="Conditional" > 
     <ContentTemplate> 
<dxchartsui:WebChartControl ID="WCStock" runat="server" AppearanceName="Light" 
            Height="350px" PaletteName="Office" SeriesDataMember="type"
   SeriesSorting="Ascending" Width="500px">
     
<SeriesTemplate><ViewSerializable>
<cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView>
</ViewSerializable>
<LabelSerializable>
<cc1:SideBySideBarSeriesLabel LineVisible="True">
<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
</cc1:SideBySideBarSeriesLabel>
</LabelSerializable>
<PointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</PointOptionsSerializable>
<LegendPointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>

<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
     <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
         direction="LeftTORight" equallyspaceditems="False"></legend>
    <Titles>


<cc1:ChartTitle Text="Stock In Hand"  Font="Tahoma, 15pt"></cc1:ChartTitle>

</Titles>
<DiagramSerializable>
<cc1:XYDiagram>
<AxisX VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
</AxisX>

<AxisY VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
</AxisY>
</cc1:XYDiagram>
</DiagramSerializable>
  </dxchartsui:WebChartControl>
 </ContentTemplate>
  </asp:UpdatePanel>
 </td>
 </tr>
 <tr>
  <td style="width: 30%" colspan="2">&nbsp;</td>
    
       </tr>
   
  </table>
    </td>
  </tr>
  <tr>
  <td align="center" style="width: 33%"> 
  <asp:UpdatePanel ID="updInter" runat="server"  UpdateMode="Conditional"> 
  <ContentTemplate> 
<dxchartsui:WebChartControl ID="WCInterSales" runat="server" AppearanceName="Light" 
          Height="300px" PaletteName="Module" SeriesDataMember="type" Width="300px">

<SeriesTemplate><ViewSerializable>
<cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView>
</ViewSerializable>
<LabelSerializable>
<cc1:SideBySideBarSeriesLabel LineVisible="True">
<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
</cc1:SideBySideBarSeriesLabel>
</LabelSerializable>
<PointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</PointOptionsSerializable>
<LegendPointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>

<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
      <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
          direction="LeftToRight" equallyspaceditems="False"></legend>
  <Titles>


<cc1:ChartTitle Text="Intermediatery Sales"  Font="Tahoma, 15pt"></cc1:ChartTitle>

</Titles>
<DiagramSerializable>
<cc1:XYDiagram>
<AxisX VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
</AxisX>

<AxisY VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
</AxisY>
</cc1:XYDiagram>
</DiagramSerializable>
   </dxchartsui:WebChartControl>
 </ContentTemplate>
  </asp:UpdatePanel>
  </td>
   <td align="center" style="width: 35%">
   <asp:UpdatePanel ID="updSecondry" runat="server"  UpdateMode="Conditional"> 
   <ContentTemplate> 
<dxchartsui:WebChartControl ID="WCSecondarySales" runat="server" 
           AppearanceName="Light" Height="300px" PaletteName="Apex" SeriesDataMember="type"
 SeriesSorting="Ascending" Width="325px">
      
<SeriesTemplate><ViewSerializable>
<cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView>
</ViewSerializable>
<LabelSerializable>
<cc1:SideBySideBarSeriesLabel LineVisible="True">
<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
</cc1:SideBySideBarSeriesLabel>
</LabelSerializable>
<PointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</PointOptionsSerializable>
<LegendPointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>

<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
       <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
           direction="LeftToRight" equallyspaceditems="False"></legend>
  <Titles>


<cc1:ChartTitle Text="Secondary Sales"  Font="Tahoma, 15pt"></cc1:ChartTitle>

</Titles>
 
<DiagramSerializable>
<cc1:XYDiagram>
<AxisX VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
</AxisX>

<AxisY VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
</AxisY>
</cc1:XYDiagram>
</DiagramSerializable>
 
   </dxchartsui:WebChartControl>
 </ContentTemplate>
   </asp:UpdatePanel>
 </td>
    <td align="center" style="width: 32%">
    <asp:UpdatePanel ID="updRetailer" runat="server" UpdateMode="Conditional" > 
    <ContentTemplate> 
<dxchartsui:WebChartControl ID="WCRetailerBilled" runat="server" 
            AppearanceName="Light" Height="300px" PaletteName="Foundry" SeriesDataMember="type"
 SeriesSorting="Ascending" Width="280px">
       
<SeriesTemplate><ViewSerializable>
<cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView>
</ViewSerializable>
<LabelSerializable>
<cc1:SideBySideBarSeriesLabel LineVisible="True">
<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
</cc1:SideBySideBarSeriesLabel>
</LabelSerializable>
<PointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</PointOptionsSerializable>
<LegendPointOptionsSerializable>
<cc1:PointOptions></cc1:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>

<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
        <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
            direction="LeftToRight" equallyspaceditems="False"></legend>
  <Titles>


<cc1:ChartTitle Text="Retailers Billed" Font="Tahoma, 15pt"></cc1:ChartTitle>

</Titles>
 
<DiagramSerializable>
<cc1:XYDiagram>
<AxisX VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
</AxisX>

<AxisY VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
</AxisY>
</cc1:XYDiagram>
</DiagramSerializable>
 
   </dxchartsui:WebChartControl>
 </ContentTemplate>
   </asp:UpdatePanel>
     </td>
  
      
   </tr>
    <tr>
    <td align="center" >
    </td>
      <td >&nbsp; 
   </td>
   <td >
         </td>
         </tr>
         </table>  
          </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" height="5" class="formtext">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" height="5" class="formtext">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
</asp:Content>
