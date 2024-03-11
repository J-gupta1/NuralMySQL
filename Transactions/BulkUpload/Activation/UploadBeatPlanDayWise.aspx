<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadBeatPlanDayWise.aspx.cs" Inherits="Transactions_BulkUpload_Activation_UploadBeatPlanDayWise" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="clear"></div>

    <div class="mainheading">
        Step 1 : Please Select For Save BeatPlan
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">

            <ul>
                <li class="text">Upload Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlUploadType" runat="server" CssClass="formselect">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Add" Value="1"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="reqddlUploadType" runat="server" ControlToValidate="ddlUploadType"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please select upload Type."
                        InitialValue="0" SetFocusOnError="true" ValidationGroup="Upload"></asp:RequiredFieldValidator>

                </li>
            </ul>
            <ul>
                <li class="link">
                     <asp:LinkButton ID="lnkDownloadTemplate" runat="server" class="elink2" Text="Download Template" OnClick="lnkDownloadTemplate_Click" ValidationGroup="Upload"></asp:LinkButton>
                </li>
               <%-- <li class="link">
                    <a>Note:In Excel Column  BeatPlanDate  Should be MM/DD/YYYY Formate. </a>
                </li>
                <li class="link">
                     <a>Note:In Excel Column BeatPlanPurpose Should be enter ADD/Update/Delete </a>
                </li>--%>
            </ul>
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DwnldReferenceCode" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCode_Click"></asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
    <%--<div class="mainheading">
        Step 2 : Download  Template For Save BeatPlan
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                     <asp:LinkButton ID="lnkDownloadTemplate" runat="server" class="elink2" Text="Download Template" OnClick="lnkDownloadTemplate_Click" ValidationGroup="Upload"></asp:LinkButton>
                </li>
                <li class="link">
                    <a>Note:In Excel Column  BeatPlanDate  Should be MM/DD/YYYY Formate. </a>
                </li>
                <li class="link">
                     <a>Note:In Excel Column BeatPlanPurpose Should be enter ADD/Update/Delete </a>
                </li>
            </ul>
        </div>
    </div>--%>
   <%-- <div class="mainheading">
        Step 3 : Download Reference Code For Save BeatPlan  
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DwnldReferenceCode" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCode_Click"></asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>--%>
    <div class="mainheading">
        Step 4 : Upload Excel File
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
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" ValidationGroup="Upload" Text="Upload"
                        OnClick="btnUpload_Click" />
                </li>
            </ul>
            <ul>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                    <asp:HyperLink ID="hlnkDuplicateNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                    <asp:HyperLink ID="hlnkBlankNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
    <div class="mainheading">
                        View Beat Plan 
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory. (+) Marked fields are optional.
                        </div>
                        <div class="H30-C3">
                            <ul>
                                <li class="text">State:<span class="error">*</span>
                                </li>
                                <li class="field">
                            <asp:DropDownList ID="cmbSerState" runat="server" OnSelectedIndexChanged="cmbSerState_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                                <li class="text">District:<span class="error">*</span>
                                </li>
                                <li class="field">
                            <asp:DropDownList ID="cmbSerDistrict" runat="server" OnSelectedIndexChanged="cmbSerDistrict_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                                <li class="text">City:<span class="error">*</span>
                                </li>
                               <li class="field">
                            <asp:DropDownList ID="cmbSerCity" runat="server" CssClass="formselect" AutoPostBack="true">
                            </asp:DropDownList>
                        </li>
                               <li class="text">Status:<span class="error">+</span></li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formselect"></asp:DropDownList></li>
                                <li class="text"></li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click"  />
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                            CausesValidation="false"  />
                                    </div>
                                </li>
                                <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnExportexcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                            CausesValidation="true" OnClick="btnExportexcel_Click"  />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
     <div>
                    <asp:Panel ID="PnlGrid" runat="server">
                        <div class="mainheading">
                            View Beat Plan  Details                                                    
                        </div>
                        <div class="contentbox">
                            <div class="grid">
                                <asp:GridView ID="gvUploadBeatplan" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"  
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false"  PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                       
                                        <asp:BoundField DataField="GenericBeatPlanName" HeaderStyle-HorizontalAlign="Left" HeaderText="Beat Plan Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="DistrictName"  HeaderStyle-HorizontalAlign="Left" HeaderText="District"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                          <asp:BoundField DataField="AreaName" HeaderStyle-HorizontalAlign="Left" HeaderText="Area"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <%--<asp:BoundField DataField="BeatPlanForCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Beat Plan For Code"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>--%>

                                      
                                        <asp:BoundField DataField="Validfrom" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid From"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ValidTill" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid Till"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                     <asp:BoundField DataField="Planneddate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Beat Plan Date"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="plannedday" HeaderStyle-HorizontalAlign="Left" HeaderText="Beat Plan Day"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                     <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Status"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:ImageButton ID="btnActiveDeactive" runat="server"
                                            CommandArgument='<%#Eval("GenericBeatPlanId") %>' CommandName='<%#Eval("Status")%>' ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                        
                                    
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="dvFooter" runat="server" class="pagination">
                                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
</asp:Content>
