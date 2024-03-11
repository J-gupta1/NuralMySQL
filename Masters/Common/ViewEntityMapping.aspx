<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ViewEntityMapping.aspx.cs" Inherits="Masters_Common_ViewEntityMapping" %>
<%@ Register Src="~/UserControls/ucPagingControl.ascx" TagName="ucPagingControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updpnlData" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="padding">
                <div>
                    <uc3:ucMessage ID="ucMessage1" runat="server" />
                </div>
            </div>
            <div class="box1">
                <div class="innerarea">
                    <div class="H25-C3-S">
                        
                        <ul>
                            <li class="text">Entity Mapping Type:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEntityMappingTypeSearch" AutoPostBack="true" runat="server"
                                    CssClass="formselect" OnSelectedIndexChanged="ddlEntityMappingTypeSearch_SelectedIndexChanged">
                                </asp:DropDownList>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlEntityMappingTypeSearch"
                                        ErrorMessage="Select type." CssClass="error" InitialValue="0" Display="Dynamic"
                                        ValidationGroup="grpSearch"></asp:RequiredFieldValidator></div>
                            </li>
                            <li class="text">Relation :</li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEntityMappingTypeRelationSearch" runat="server" CssClass="formselect"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlEntityMappingTypeRelationSearch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Primary Entity :</li>
                            <li class="field">
                                <asp:DropDownList ID="ddlPrimaryEntitySearch" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Secondary Entity:</li>
                            <li class="field">
                                <asp:TextBox ID="txtsecondaryparty" AutoPostBack="true" runat="server" CssClass="formselect">
                                </asp:TextBox>
                            </li>
                              </ul>
                        <div class="setbbb">
                      
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" ValidationGroup="grpSearch"
                                    OnClick="btnSearch_Click"/>


                            </div>
                      
                       
                    </div>
                </div>
            </div>
            <div class="box1" runat="server" id="dvSearch" visible="false">
                <div>
                    <div class="float-left">
                        <span class="subheading">Mapping List </span>
                    </div>
                    <div class="export">
                         <asp:Button ID="Exporttoexcel" runat="server" 
                            OnClick="Exporttoexcel_Click" CausesValidation="False" CssClass="excel" />
                    </div>
                </div>
                <div class="clear"></div>
                <div class="innerarea">
                    <div class="grid1">
                        <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            CellSpacing="1" DataKeyNames="EntityMappingId,EntityMappingTypeID,EntityMappingTypeRelationID,PrimaryEntityID"
                            EmptyDataText="No Record Found" SelectedRowStyle-CssClass="selectedrow" HeaderStyle-CssClass="gridheader"
                            RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow"
                            GridLines="None" Width="100%">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField DataField="MappingType" HeaderStyle-HorizontalAlign="Left" HeaderText="Mapping Type">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Relation" ShowHeader="False"
                                    HeaderStyle-Width="110px">
                                    <ItemTemplate>
                                        <%# Eval("EntityType") +"-"+ Eval("SecondaryEntityType")%></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Pr_CompanyDisplayName" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Primary Entity">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="prUserName" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Primary User">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EntityType" DataFormatString="" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Primary Entity Type">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Default Parent"
                                    ShowHeader="False" HeaderStyle-Width="110px">
                                    <ItemTemplate>
                                        <%# Convert.ToBoolean(Eval("DefaultParent"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Scr_CompanyDisplayName" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Secondary Entity">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SecondaryEntityType" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Secondary Entity Type">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                        </asp:GridView>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>
                </div>

                
                   <div class="clear"></div>                          
            </div>
               
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Exporttoexcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
