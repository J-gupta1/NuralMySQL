<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadLocalityMaster.aspx.cs" Inherits="Masters_Common_LocalityMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
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
                            CssClass="elink2" OnClick="DwnldTemplate_Click"></asp:LinkButton>
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
                            CssClass="elink2" OnClick="UpdateTemplateFile_Click"></asp:LinkButton>
                    </li>

                    <li class="link">
                        Note : In ActionType column  enter Inactive or can be blank , if not enter anything that means you modified records else inactive this user.  
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
                            CssClass="elink2" OnClick="DownloadReferenceCodeForSave_Click"></asp:LinkButton>
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
                        CssClass="elink2" OnClick="DownloadReferenceCodeForUpdate_Click"></asp:LinkButton>
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
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" Font-Bold="true" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>

     <div class="mainheading">
        Search 
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label2" runat="server" Text="Country:"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbSerCountry" runat="server" OnSelectedIndexChanged="cmbSerCountry_SelectedIndexChanged"
                                    CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserstate" runat="server" Text="State:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerState" runat="server" OnSelectedIndexChanged="cmbSerState_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserdist" runat="server" Text="District:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerDistrict" runat="server" OnSelectedIndexChanged="cmbSerDistrict_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblsercity" runat="server" Text="City:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerCity" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="cmbSerCity_SelectedIndexChanged">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul id="dvSerTehsil" runat="server" style="display: none">
                        <li class="text">
                            <asp:Label ID="Label4" runat="server" Text="Tehsil:"></asp:Label>
                        </li>
                        <li class="field">
                           
                            <asp:DropDownList ID="cmbSerTehsil" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                           
                        </li>
                    </ul>
                    <ul>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSerCode" Text="Search" runat="server" OnClick="btnSerCode_Click"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="getalldata" Text="Show All Data" runat="server" OnClick="getalldata_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
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
        <asp:Button ID="exportToExel" Text=" " runat="server" OnClick="exportToExel_Click"
            CssClass="excel" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdArea" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1"  EmptyDataText="No data founnd"
                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                        AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" PageSize='<%$ AppSettings:GridViewPageSize %>'
                        FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" RowStyle-VerticalAlign="top"
                        Width="100%"  OnPageIndexChanging="grdArea_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                        <Columns>
                            <asp:BoundField DataField="CountryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Country"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <%-- <asp:BoundField DataField="ZoneName" HeaderStyle-HorizontalAlign="Left" HeaderText="Zone Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="RegionName" HeaderStyle-HorizontalAlign="Left" HeaderText="Region Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="RegionCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Region Code"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="StateCode" HeaderStyle-HorizontalAlign="Left" HeaderText="State Code"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DistrictName" HeaderStyle-HorizontalAlign="Left" HeaderText="District Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DistrictCode" HeaderStyle-HorizontalAlign="Left" HeaderText="District Code"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>

                            <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                             <asp:BoundField DataField="CityCode" HeaderStyle-HorizontalAlign="Left" HeaderText="City Code"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TehsilName" HeaderStyle-HorizontalAlign="Left" HeaderText="Tehsil Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TehsilCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Tehsil Code"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="AreaCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Area Code "
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="AreaName" HeaderStyle-HorizontalAlign="Left" HeaderText="Area Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdArea" />     
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
