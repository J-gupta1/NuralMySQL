<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="Enquiry.aspx.cs" Inherits="ClientServices_Common_Enquiry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ucDatePicker2.ascx" TagName="ucDatePicker2" TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc5" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dx" %>
<%--12 May 2018, Rajnish Kumar, #CC01, Changes for image savings, and Upload Control  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/CSS/ErrorStyle.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/CSS/modal.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/CSS/Menu.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/CSS/style.css") %>" />


    <script src="../../Assets/Jscript/dhtmlwindow.js" type="text/javascript"></script>
    <script src="../../Assets/Jscript/modal.js" type="text/javascript"></script>
    <script src="../../Assets/Jscript/popuploading.js" type="text/javascript"></script>
    <script type="text/javascript">
        function popupimageuploader() {
            var UpperLimit = '3';
            var Rows = '6';
            var ImageType = '1';
            var WinImageUploader = dhtmlmodal.open("WinImageUploader", "iframe", "../../ClientServices/Common/MultipleFileUpload.aspx?UpperLimit=" + UpperLimit + "&Rows=" + Rows + "&ImageType=" + ImageType + "&Decider=1", 'Upload User File/Images', "width=665px,height=350px,top=25,resize=0,scrolling=auto ,center=1")
            WinImageUploader.onclose = function () {
                return true;
            }
            return false;
        }
    </script>
    <script type="text/javascript" language="javascript">
        function setVisibility(visibility) {
            document.getElementById('ctl00_contentHolderMain_ucMessage1_dvShowError').style.display = visibility;
        }

    </script>
    <script type="text/javascript">/*#CC01*/
        // <![CDATA[
        var fieldSeparator = "|";

        function FileUploadStart() {

        }

        //function onFileUploadComplete(s, e) {
        //    if (e.callbackData) {
        //        var fileData = e.callbackData.split('|');
        //        var fileName = fileData[0],
        //            fileUrl = fileData[1],
        //            fileSize = fileData[2];
        //        DXUploadedFilesContainer.AddFile(fileName, fileUrl, fileSize);
        //    }
        //}

        /*#CC01*/ function FileUploaded(s, e) {
            if (e.isValid) {

                var container = document.getElementById("uploadedListFiles");
                // var uploadDockType = document.getElementById("ctl00$contentHolderMain$ddlImageType");
                //if (uploadDockType.options[uploadDockType.selectedIndex].value == 0) {

                //    alert('Please Select Upload Document Type.');
                //    return;
                //}
                //var docType = uploadDockType.options[uploadDockType.selectedIndex].text;
                //var docType

                //if (container.getElementsByTagName("uploadedListFiles").length < =3) {//#CC01:commented by rajesh//
                var imageType = document.getElementById("ctl00_contentHolderMain_ddlImageType");
                if (imageType.value == 1) {

                    var linkFile = document.createElement("a");
                    var indexSeparator = e.callbackData.indexOf(fieldSeparator);
                    var fileName = e.callbackData.substring(0, indexSeparator);
                    var pictureUrl = e.callbackData.substring(indexSeparator + fieldSeparator.length);
                    var imgSrc = pictureUrl;
                    //fileName = fileName + "-" + docType;
                    fileName = fileName;
                    linkFile.innerHTML = fileName;
                    linkFile.setAttribute("href", imgSrc);
                    linkFile.setAttribute("target", "_blank");
                    container.appendChild(linkFile);
                    container.appendChild(document.createElement("br"));
                }
                else { alert('Please select image type') }
                //#CC01:commented by rajesh(start)//
                //}
              <%--  else {
                    alert('Trying To insert the images more than 3');
                }//#CC01:commented by rajesh(end)//--%>


            }
        }
        /*#CC01*/  function upload() {
            var btn = document.getElementById("ctl00_contentHolderMain_btnviewdocument");
            if (btn != null) {
                __doPostBack(btn.name, "OnClick");

            }
        }


        // ]]> 
    </script>


    <%--/*#CC01*/--%>
    <script type="text/javascript">
        function onFileUploadComplete(s, e) {
            if (e.callbackData) {
                var fileData = e.callbackData.split('|');
                var fileName = fileData[0],
                    fileUrl = fileData[1],
                    fileSize = fileData[2];
                dx: ASPxRoundPanel.AddFile(fileName, fileUrl, fileSize);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <%------------------------------------------------------------------------%>
    <asp:Panel runat="server" ID="PnlAdmin">
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc5:ucMessage ID="ucMessage1" runat="server" />
                <asp:HiddenField ID="hdnFinalList" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory            
            </div>
            <div class="H35-C3-S">
                <ul>
                    <li class="text">
                        <asp:Label ID="lblEnquiryType" CssClass="formtext" runat="server">Category Type:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlCategoryType" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="CategoryType" runat="server" ControlToValidate="ddlCategoryType"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select Category Type!" InitialValue="0"
                                ValidationGroup="holiday"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">
                        <asp:Label ID="lblDecision" runat="server" CssClass="formtext">Sub-Category:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="Decision" runat="server" ControlToValidate="ddlSubCategory"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select Sub-Category!" InitialValue="0"
                                ValidationGroup="holiday"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">
                        <asp:Label ID="lblCustomerQuery" runat="server" CssClass="formtext">Name:<span class="error">*</span></asp:Label>

                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="txtName" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="Name" runat="server" ControlToValidate="txtName"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Name!"
                                ValidationGroup="holiday"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexpName" runat="server"
                                ErrorMessage="Enter Valid Name."
                                ControlToValidate="txtName"
                                ValidationExpression="^[a-zA-Z'.\s]{1,40}$" />
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="text">
                        <asp:Label ID="lblImageType" runat="server" CssClass="formtext">Image type:<span class="error">*</span></asp:Label></li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlImageType" AutoPostBack="true" OnSelectedIndexChanged="ddlImageType_SelectedIndexChanged" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredddlImageType" runat="server" ControlToValidate="ddlImageType"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select Image Type!" InitialValue="0"
                                ValidationGroup="holiday"></asp:RequiredFieldValidator>
                        </div>
                        <%--<asp:Label ID="lblAttchment" runat="server" CssClass="formtext">Attchment:<font class="error"></font></asp:Label>--%>
                    </li>
                    <asp:Panel runat="server" ID="PnlAdminAttachement" Visible="false">
                        <li class="text" style="height: auto">Upload Images:</li>
                        <li class="field2" style="height: auto">
                            <%-- <div>
                                                                                        <asp:FileUpload ID="IpFile" runat="server" CssClass="fileuploads" Width="300px" />
                                                                                    </div>
                                                                                    <div>
                                                                                        (Upload only JPG, bmp, gif, PNG, PDF , doc, docx formate).
                                                                                        File Size Less Then 500 KB.
                                                                                    </div>--%>

                            <%--*#CC01 startrt*--%>

                            <div class="uploadContainer">
                                <%--<li class="text" style="height:auto">Upload Images :</li>--%>
                                <div class="float-margin" style="height: auto; width: 220px;">
                                    <dx:ASPxUploadControl ID="UploadControl" runat="server" Width="220px" ShowUploadButton="True" AddUploadButtonsHorizontalPosition="Left"
                                        ShowProgressPanel="True" ClientInstanceName="UploadControl" OnFileUploadComplete="UploadControl_FileUploadComplete" FileInputCount="3"
                                        ButtonStyle-Cursor="pointer" ValidationSettings-ErrorStyle-CssClass="error" ButtonStyle-Border-BorderWidth="0"
                                        ButtonStyle-Font-Underline="False" ButtonStyle-CssClass="buttonbg">
                                        <%--#CC02 : Width change "250px" to "220px"--%>
                                        <ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif,.png,.pdf,.Doc,.Docx,.xls,.xlsx,.rtf" MaxFileSize="500000">
                                        </ValidationSettings>
                                        <ButtonStyle Cursor="pointer" Font-Underline="False" ForeColor="White" Font-Size="11px" CssClass="buttonbg">
                                        </ButtonStyle>
                                        <ClientSideEvents FileUploadComplete="function(s, e) { FileUploaded(s, e) }" FileUploadStart="function(s, e) { FileUploadStart(); }" />
                                        <%--<ClientSideEvents FileUploadStart="function(s, e) { dx:ASPxRoundPanel.Clear(); }"
                              FileUploadComplete="onFileUploadComplete" />--%>
                                    </dx:ASPxUploadControl>
                                    <div class="note3">
                                        <b>Note</b>: The total size of files to upload is limited by 500kb.
                                    </div>
                                </div>
                                <div class="float-left">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" ClientInstanceName="RoundPanel"
                                        HeaderText="Uploaded files (jpeg,gif,png,pdf,docx,Doc,xlsx)" Height="100%">
                                        <PanelCollection>
                                            <dx:PanelContent runat="server" ID="MM">
                                                <div id="uploadedListFiles" style="height: 85px; overflow: auto; font-family: Arial;">
                                                </div>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                </div>
                                <div class="clear"></div>
                            </div>

                        </li>
                    </asp:Panel>
                    <li>
                        <asp:HiddenField ID="hdnEnquiryid" runat="server" />
                    </li>
                </ul>
                <ul>
                    <li class="text">
                        <asp:Label ID="lbldescription" CssClass="formtext" runat="server">Description:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                        <div>

                            <asp:TextBox ID="UcExecutiveRemark" runat="server" CssClass="form_textarea" MaxLength="50" TextMode="MultiLine"></asp:TextBox>
                            <%-- <uc2:ucTextboxMultiline ID="UcExecutiveRemark" runat="server" TextBoxWatermarkText="Enter Description"
                                                                                        CharsLength="300" IsRequired="true" />--%>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RfvDescription" runat="server" ControlToValidate="UcExecutiveRemark"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Description!"
                                ValidationGroup="holiday"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="text">
                        <asp:Label ID="lblStatus" runat="server" CssClass="formtext">Status:<span class="error"></span></asp:Label>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="text">
                        <asp:Label ID="lblEmail" CssClass="formtext" runat="server">Email Id:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="Emailid" runat="server" ControlToValidate="txtEmail"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Email!"
                                ValidationGroup="holiday"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionEmail" runat="server"
                                ErrorMessage="Please Enter Valid Email." ControlToValidate="txtEmail"
                                Display="Dynamic" ForeColor="#FF3300" SetFocusOnError="True"
                                ValidationExpression="^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z](?:[a-zA-Z-]{0,61}[a-zA-Z])?(?:\.[a-zA-Z](?:[a-zA-Z-]{0,61}[a-zA-Z])?)*$"></asp:RegularExpressionValidator>
                        </div>
                    </li>
                    <li class="text">
                        <asp:Label ID="lblContactNumber" CssClass="formtext" runat="server">Contact No:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="txtContactNumber" CssClass="formfields" runat="server" MaxLength="10"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="ContactName" runat="server" ControlToValidate="txtContactNumber"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Contact No.!"
                                ValidationGroup="holiday"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:RegularExpressionValidator ID="ContactNameValidator" runat="server"
                                ControlToValidate="txtContactNumber" ErrorMessage="Please Enter Valid Contact No.!"
                                ValidationExpression="[0-9]{10}" ValidationGroup="holiday"></asp:RegularExpressionValidator>
                        </div>
                    </li>
                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" ValidationGroup="holiday" OnClick="finalsubmission_Click"></asp:Button>
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" OnClick="btnCancel_Click"></asp:Button>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div id="dvPnlRemarksGrid" runat="server">
            <div class="contentbox">
                <div class="grid1">
                    <asp:GridView ID="grdDescriptionlist" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                        RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                        AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                        HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" 
                        BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                        OnPageIndexChanging="grdDescriptionlist_PageIndexChanging" OnRowDataBound="grdDescriptionlist_RowDataBound" PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected" DataKeyNames="EnquiryDetailRemarkid">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                        <Columns>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Remark By"
                                HeaderText="Remark By"></asp:BoundField>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Remark"
                                HeaderText="Remark"></asp:BoundField>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Remark Date"
                                DataFormatString="{0:dd/MM/yyyy}" HeaderText="Remark Date"></asp:BoundField>
                            <asp:TemplateField HeaderText="Attachment">
                                <ItemTemplate>
                                    <asp:GridView ID="gvAttachedRemarksImages" runat="server" AutoGenerateColumns="false" BorderWidth="0px"
                                        ShowHeader="false" CellPadding="0" GridLines="None" CssClass="table-panel" HeaderStyle-VerticalAlign="top"
                                        HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                        DataKeyNames="Attachement">
                                        <FooterStyle HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                        <Columns>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblImagePathRemarks" runat="server" Text='<%# Eval("Attachement") %>'
                                                        Style="display: none"></asp:Label>
                                                    <asp:LinkButton ID="lnkDownload" Text="View Attachment" CssClass="elink2" CommandArgument='<%# Eval("Attachement") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingRowStyle>
                                    </asp:GridView>
                                </ItemTemplate>


                                <%-- <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkQueryDownload" Text="View Attachment" CssClass="elink2" CommandArgument='<%# Eval("Attachement") %>' runat="server" OnClick="DownloadQueryFile"></asp:LinkButton>
                                                                        </ItemTemplate>--%>
                            </asp:TemplateField>

                        </Columns>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div>

        <%--<asp:Button ID="btnUploadImage" runat="server" Text="" ToolTip="Upload File/Image"
                                                                                            CausesValidation="false" OnClientClick="return popupimageuploader();" CssClass="upload" />--%>
    </div>

    <%-- ------------------------------------------------------------%>


    <div class="mainheading">
        Search Query View
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblSearchFromDate" CssClass="formtext" runat="server">From Date:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <%-- <uc4:ucDatePicker ID="ucDateFrom" runat="server" IsRequired="true" ErrorMessage="From date required." defaultDateRange="True"
                                                                ValidationGroup="AddUserSerarhValidationGroup" />--%>

                            <uc6:ucDatePicker2 ID="ucDateFrom" runat="server" IsRequired="true" ErrorMessage="From date required." defaultDateRange="True"
                                ValidationGroup="AddUserSerarhValidationGroup" />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblSearchToDate" runat="server" CssClass="formtext">To Date:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <%--<uc4:ucDatePicker ID="ucDateTo" runat="server" IsRequired="true" ErrorMessage="To date required." RangeErrorMessage="Date should be greater then equal to current date."
                                                                ValidationGroup="AddUserSerarhValidationGroup" />--%>
                            <uc6:ucDatePicker2 ID="ucDateTo" runat="server" IsRequired="true" ErrorMessage="To date required." RangeErrorMessage="Date should be greater then equal to current date."
                                ValidationGroup="AddUserSerarhValidationGroup" />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblSalectQueryStatus" CssClass="formtext" runat="server">Salect Query Status:<span class="error"></span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlQueryStatus" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="QueryStatus" runat="server" ControlToValidate="ddlQueryStatus"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please select Query Status!" InitialValue="-1"
                                    ValidationGroup="AddUserSerarhValidationGroup"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblDistributorCode" runat="server" CssClass="formtext">Distributor Name:<span class="error"></span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlDistributorCode" CssClass="formselect" runat="server"></asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:HiddenField ID="hdfenqurydetailid" runat="server" />
                        </li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearchUser" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                    OnClick="btnSearchUser_Click" CausesValidation="False"></asp:Button>
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnDownLoadQueryReport" Text="Download Query Report" runat="server"
                                    ToolTip="Download" CssClass="buttonbg" CausesValidation="true" OnClick="btnDownLoadQueryReport_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSearchUser" />
                <asp:PostBackTrigger ControlID="btnDownLoadQueryReport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div id="PnlSearch" runat="server">
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="grdQueryList" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                    RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                    AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                    HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4"
                    BorderWidth="0px" Width="100%" OnRowDataBound="grdQueryList_RowDataBound" OnRowCommand="grdQueryList_RowCommand" OnPageIndexChanging="grdQueryList_PageIndexChanging" AutoGenerateColumns="false" AllowPaging="True"
                    PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected" DataKeyNames="EnquiryDetailID">
                    <%-- OnSelectedIndexChanged="grdQueryList_SelectedIndexChanged">--%>
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkAction" CommandName="EditData" Text="View/Reply" CssClass="elink2" CommandArgument='<%# Eval("EnquiryDetailID") %>' Visible='<%# Convert.ToBoolean(Eval("SalesChannelTypeIDvisible")) %>' runat="server"></asp:LinkButton>
                                <asp:HiddenField ID="hdnComingFrom" runat="server" Value='<%# Eval("ComingFrom") %>' /> <%--#CC03 Added--%>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DistributorCode"
                            HeaderText="Distributor Code"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DistributorName"
                            HeaderText="Distributor Name"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EnquiryNumber"
                            HeaderText="Query Number"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EnquiryCategoryName"
                            HeaderText="Query Category"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EnquiryType"
                            HeaderText="Query Sub_Category"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ContactNumber"
                            HeaderText="Contact No"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Decription"
                            HeaderText="Query Description"></asp:BoundField>
                        <%-- <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ComioRemark"
                                                            HeaderText="Query Reply Remark's"></asp:BoundField>--%>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="CreatedOn"
                            DataFormatString="{0:dd/MM/yyyy}" HeaderText="Query Date"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Query Close Date"
                            DataFormatString="{0:dd/MM/yyyy}" HeaderText="Query Close Date"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EnquiryStatus"
                            HeaderText="Query Status"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EmailId"
                            HeaderText="Email"></asp:BoundField>
                        <asp:TemplateField HeaderText="Attachment">
                            <ItemTemplate>
                                <asp:GridView ID="gvAttachedImages" runat="server" AutoGenerateColumns="false" BorderWidth="0px"
                                    ShowHeader="false" CellPadding="0" GridLines="None" CssClass="table-panel" HeaderStyle-VerticalAlign="top"
                                    HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                    DataKeyNames="Attachement">
                                    <FooterStyle HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:Label ID="lblImagePath" runat="server" Text='<%# Eval("Attachement") %>'
                                                    Style="display: none"></asp:Label>
                                                <asp:LinkButton ID="lnkDownload" Text="View Attachment" CssClass="elink2" CommandArgument='<%# Eval("Attachement") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <AlternatingRowStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingRowStyle>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
        </div>
    </div>


    <%--     <uc3:footer ID="Footer1" runat="server" />--%>
</asp:Content>
