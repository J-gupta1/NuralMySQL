<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadCityTravelRate.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Masters_Common_UploadCityTravelRate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
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
        Step 1 : Click On Button For Save Or Update Process
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">

            <ul>
                <li class="link">
                    <asp:RadioButtonList ID="Rbtdownloadtemplate" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="radio-rs" CellPadding="2" CellSpacing="0" OnSelectedIndexChanged="Rbtdownloadtemplate_SelectedIndexChanged">
                        <asp:ListItem Value="1" Text="Save" Selected="True">
                            
                        </asp:ListItem>
                        <asp:ListItem Value="2" Text="Update">
                            
                        </asp:ListItem>
                    </asp:RadioButtonList>
                </li>


            </ul>
        </div>
    </div>
    <div class="mainheading" runat="server" id="ForSaveTemplateheading" visible="false">
        Step 2 : Download  Template For Save Record 
    </div>
    <div class="contentbox" runat="server" id="ForSaveTemplatedownload" visible="false">
        <div class="H25-C3-S">

            <ul>
                <li class="link">
                    <asp:LinkButton ID="DwnldTemplate" runat="server" Text="Download Template File"
                        CssClass="elink2" OnClick="DwnldTemplate_Click" CausesValidation="false"></asp:LinkButton>
                </li>
                <li class="link">Note : In ValidFrom and ValidTo column  date format should be 'MM/dd/YYYY' 
                </li>

            </ul>
        </div>
    </div>
    <div class="mainheading" runat="server" id="ForUploadTemplateheading" visible="false">
        Step 2 : Download  Template For  Update Record  
    </div>
    <div class="contentbox" runat="server" id="ForUpdateTemplatedownload" visible="false">
        <div class="H25-C3-S">

            <ul>
                <li class="link">
                    <asp:LinkButton ID="UpdateTemplateFile" runat="server" Text="Download Template File"
                        CssClass="elink2" OnClick="UpdateTemplateFile_Click" CausesValidation="false"></asp:LinkButton>
                </li>

                <li class="link">Note : In Status column  enter Inactive or can be blank , if not enter anything that means you modified records else inactive this user.<br />
                    <br />
                    Note : In ValidFrom and ValidTo column  date format should be 'MM/dd/YYYY' 
                </li>
            </ul>
        </div>
    </div>
    <div class="mainheading" runat="server" id="ReferenceIdForsaveheading" visible="false">
        Step 3 : Download Reference Code For Save Record 
    </div>
    <div class="contentbox" runat="server" id="ReferenceIdForsave" visible="false">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DownloadReferenceCodeForSave" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DownloadReferenceCodeForSave_Click" CausesValidation="false"></asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
    <div class="mainheading" runat="server" id="ReferenceIdForupdateheading" visible="false">
        Step 3 : Download Reference Code For Update Record 
    </div>
    <div class="contentbox" runat="server" id="ReferenceIdForupdate" visible="false">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DownloadReferenceCodeForUpdate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DownloadReferenceCodeForUpdate_Click" CausesValidation="false"></asp:LinkButton>
                </li>

            </ul>
        </div>
    </div>
    <div class="mainheading">
        Step 4 : Upload Excel File
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">

            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CausesValidation="false" CssClass="buttonbg" runat="server" Text="Upload"
                        OnClick="btnUpload_Click" />
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" Font-Bold="true"  runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>

    <div class="mainheading">
        Search City Travel Rate
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblRole" runat="server" Text="Role:"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlRole" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblCityGroup" runat="server" Text="City Group Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlCityGroup" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>

                    </ul>
                    <ul>
                        <li class="text">Valid From:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." 
                                runat="server" />
                        </li>
                        <li class="text">Valid To:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." 
                                runat="server" />
                        </li>
                    </ul>

                <div class="setbbb">
                            <div class="float-margin">
                                <asp:Button ID="btnSerCode" Text="Search" runat="server" OnClick="btnSerCode_Click" CausesValidation="false"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="getalldata" Text="Show All Data" runat="server" OnClick="getalldata_Click" CausesValidation="false"
                                    CssClass="buttonbg" />
                            </div>
                       
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSerCode" />
                <asp:PostBackTrigger ControlID="getalldata" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="exportToExel" Text=" " runat="server" CausesValidation="false" OnClick="exportToExel_Click"
            CssClass="excel" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdCityTravelRate" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" EmptyDataText="No data founnd"
                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                        AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" PageSize='<%$ AppSettings:GridViewPageSize %>'
                        FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" RowStyle-VerticalAlign="top"
                        Width="100%" OnPageIndexChanging="grdCityTravelRate_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                        <Columns>
                            <asp:BoundField DataField="RoleName" HeaderStyle-HorizontalAlign="Left" HeaderText="Role"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>

                            <asp:BoundField DataField="CityGroupName" HeaderStyle-HorizontalAlign="Left" HeaderText="City Group"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DAAmount" HeaderStyle-HorizontalAlign="Left" HeaderText="DA Amount"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="conveyanceAmount" HeaderStyle-HorizontalAlign="Left" HeaderText="Convance Amount"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ValidFrom" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid From"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>

                            <asp:BoundField DataField="Validtill" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid Till"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="InCityKM" HeaderStyle-HorizontalAlign="Left" HeaderText="InCityKM"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="XTownKM" HeaderStyle-HorizontalAlign="Left" HeaderText="XTownKM"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="OutCityKM" HeaderStyle-HorizontalAlign="Left" HeaderText="OutCityKM"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Active" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdCityTravelRate" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
