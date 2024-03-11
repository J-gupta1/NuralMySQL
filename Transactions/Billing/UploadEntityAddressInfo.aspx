<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadEntityAddressInfo.aspx.cs" Inherits="Transactions_Billing_UploadEntityAddressInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Upload
    </div>
    <div class="contentbox">
       
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
            </ul>
        </div>
        <div class="formlink">
            <ul>
                <li>
                  
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
                <li>
                    <a class="elink2" href="../../Excel/Templates/EntityDetailInfo.xlsx">Download Template</a>
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
     <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>  
                    <div class="clear"></div>
                    <div class="mainheading">
                        Search Entity Address Info
                    </div>
                    <div class="contentbox">
                        <div class="H25-C3-S">                          
                            <ul>
                                 <li class="text">State:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlstate" AutoPostBack="true" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>

                                <li class="text">City:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlcity"   runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">Sales Channel Type:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlsaleschannelType" AutoPostBack="true" OnSelectedIndexChanged="ddlsaleschannelType_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                               <li class="text">Sales Channel Name:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlsaleschannelname" Visible="false" runat="server">
                                        <asp:ListItem Text="Select" Value="0" ></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" Id="txt_saleschannelname" placeholder="Saleschannelname"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderPartName" runat="server" TargetControlID="txt_saleschannelname" 
                                        MinimumPrefixLength="3" ContextKey="saleschannelname"
                                        CompletionSetCount="5" ServiceMethod="GetSaleschannelname" ServicePath="../../CommonService.asmx">
                                    </cc1:AutoCompleteExtender>
                                </li>



                                <li class="text">Status<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlstatus" runat="server">
                                        <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                 

                                
                                                       
                               <li class="text"></li>                               
                               <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                    </div>
                                </li>
                               <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnExportexcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                            CausesValidation="true" OnClick="btnExportexcel_Click" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="PnlGrid" runat="server">
                        <div class="mainheading">
                            View Billing Address Info Details 
                        </div>
                        <div class="contentbox">
                            <div class="grid1">
                               <asp:GridView ID="gvAddressinfo" runat="server" EmptyDataText="No Record Found"  AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    DataKeyNames="EntityAddressID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowCommand="gvAddressinfo_RowCommand">
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"  />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                       <%--<asp:BoundField DataField="SalesChannelName" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Payment Date"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AddressType" HeaderStyle-HorizontalAlign="Left" HeaderText="Address Type"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EntityCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Entity Code"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FullAddress" HeaderStyle-HorizontalAlign="Left" HeaderText="Address"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                         
                                        <asp:BoundField DataField="EntityType" HeaderStyle-HorizontalAlign="Left" HeaderText="Entity Type"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                              <asp:ImageButton ID="imgStatus1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("EntityAddressID") %>'
                                        CommandName="AddressActive" ImageAlign="Top" ImageUrl='<%# BussinessLogic.PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                        ToolTip='<%#BussinessLogic.PageBase.ToolTipeChange(Convert.ToInt16(Eval("Active"))) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                    </Columns> 
                                </asp:GridView>
                            </div>
                             <div id="dvFooter" runat="server" class="pagination">
                            <uc4:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="ucPagingControl1_SetControlRefresh"/>
                        </div>
                        </div>
                       
                    </asp:Panel>
                </div>
            </ContentTemplate>
            <Triggers>

                <asp:PostBackTrigger ControlID="btnExportexcel" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <asp:PostBackTrigger ControlID="btnSearch" />
              <%--  <asp:PostBackTrigger ControlID="gvPaymentDetail" />--%>

            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
