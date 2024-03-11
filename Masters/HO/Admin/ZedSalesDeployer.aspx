<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master" AutoEventWireup="true" CodeFile="ZedSalesDeployer.aspx.cs" Inherits="Masters_HO_Admin_ZedSalesDeployer" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" Runat="Server">
<div class="contentbox">  
<div class="grid1">
  <dx:ASPxTreeList ID="ASPxTreeList1" runat="server" AutoGenerateColumns="False" KeyFieldName="DX_PATH_GUID_MAP" 
        onvirtualmodecreatechildren="ASPxTreeList1_VirtualModeCreateChildren" 
        onvirtualmodenodecreating="ASPxTreeList1_VirtualModeNodeCreating" 
        Width="100%" onselectionchanged="ASPxTreeList1_SelectionChanged">
        <SettingsSelection Enabled="True" Recursive="True" />
        <Columns>
        <dx:TreeListTextColumn FieldName="FileName" VisibleIndex="0">
                </dx:TreeListTextColumn>
        </Columns>
        <SettingsPager Mode="ShowPager" PageSize="20">
        </SettingsPager>
    </dx:ASPxTreeList>
    </div>
</div>
   <div class="clear padding-top"></div>
    <div class="margin-bottom">
        <asp:Button ID="btnZip" runat="server" Text="Submit" onclick="btnZip_Click" CssClass="buttonbg"  /></div>

</asp:Content>

