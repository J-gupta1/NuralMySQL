<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageEntityTypeRelation.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Masters_Common_ManageEntityTypeRelation" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCUserDetail.ascx" TagName="UCUserDetail" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ucStatusControl.ascx" TagName="ucStatus" TagPrefix="uc5" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="box1">
        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div runat="server" id="dvMsg">
                    <uc4:ucMessage ID="ucMsg" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="subheading">
            Entity Type Relation Save
           
        </div>
        <asp:Panel ID="pnlSubmit" runat="server" DefaultButton="btnSubmit">
            <asp:UpdatePanel ID="updSave" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="innerarea">
                        <div class="H25-C3">

                            <ul>
                                <li class="text">Mapping Type:<span class="error">*</span></li>
                                <li class="field">
                                    <div>
                                        <asp:DropDownList ID="ddlMappingType" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlMappingType_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rvddlMappingType" runat="server" ErrorMessage="Select Mapping Type"
                                            ControlToValidate="ddlMappingType" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic"
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                    </div>
                                </li>
                                <li class="text">Primary Entity Type:<span class="error">*</span></li>
                                <li class="field">
                                    <div>
                                        <asp:DropDownList ID="ddlPrimaryEntityType" runat="server" CssClass="formselect">
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rvddlPrimaryEntityType" runat="server" ErrorMessage="Select Primary Entity Type"
                                            ControlToValidate="ddlPrimaryEntityType" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic"
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                    </div>
                                </li>
                                <li class="text">Secondary Entity Type:<span class="error">*</span></li>
                                <li class="field">
                                    <div>
                                        <asp:DropDownList ID="ddlSecondaryEntityType" runat="server" CssClass="formselect">
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rvddlSecondaryEntityType" runat="server" ErrorMessage="Select Secondary Entity Type"
                                            ControlToValidate="ddlSecondaryEntityType" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic"
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                    </div>
                                </li>
                                <li class="text">Mapping Mode:<span class="error">*</span></li>
                                <li class="field">
                                    <div>
                                        <asp:DropDownList ID="ddlMappingMode" runat="server" CssClass="formselect">
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rvddlMappingMode" runat="server" ErrorMessage="Select Mapping Mode"
                                            ControlToValidate="ddlMappingMode" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic"
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                    </div>
                                </li>
                                <li class="text">Remark:<span class="optional-img">&nbsp;</span></li>
                                <li class="field">
                                    <asp:TextBox CssClass="form_textarea" ID="txtRmark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </li>
                                <li class="text">
                                    <div class="float-margin padding-right">
                                        Active:<span class="optional-img">&nbsp;</span>
                                    </div>
                                    <div class="float-margin">
                                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                    </div>
                                </li>
                                <li class="field">
                                    <div class="float-margin">

                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" ValidationGroup="submit" OnClick="btnSubmit_Click" />
                                    </div>
                                    <div class="float-margin">

                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" OnClick="btnCancel_Click" />
                                    </div>
                                </li>
                            </ul>

                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <div class="clear">
    </div>
    <div class="box1">
        <div class="subheading">
            Entity Type Relation Search</div>
        <asp:UpdatePanel ID="updSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="innerarea">
                    <div class="H20-C3">
                        <ul>
                            <li class="text">Mapping Type:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlMappingTypeSearch" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlMappingTypeSearch_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Primary Entity Type:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlPrimaryEntityTypeSearch" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Secondary Entity Type:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSecondaryEntityTypeSearch" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="buttonbg"
                                        OnClick="btnShowAll_Click" />
                                </div>
                            </li>
                        </ul>
                       
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="updpnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="box1" runat="server" id="divgrd">
                <div class="subheading">
                    <div class="float-left">
                        Entity Type Relation List</div>
                    
                </div>
                <div class="export">
                        <asp:Button ID="Exporttoexcel" runat="server" 
                            OnClick="Exporttoexcel_Click" CausesValidation="False" CssClass="excel"  /></div>
                <div class="innerarea">
                    <div class="grid1">
                         <asp:GridView ID="grdvList" runat="server"  AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="EntityMappingTypeRelationID" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" EmptyDataText="No record found"
                            RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow"  FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" OnRowCommand="grdvList_RowCommand">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                 <asp:BoundField DataField="EntityMappingType" HeaderText="Entity Mapping Type" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PrimaryEntityType" HeaderText="Primary Entity Type" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="SecondaryEntityType" HeaderText="Secondary Entity Type"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="MappingMode" HeaderText="Mapping Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="UpdatedOn" HeaderText="Updated On" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="80px" />
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("EntityMappingTypeRelationID") %>'
                                            CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("EntityMappingTypeRelationID") %>'
                                            CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Active"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                    </div>
                    <div class="clear">
                    </div>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Exporttoexcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
