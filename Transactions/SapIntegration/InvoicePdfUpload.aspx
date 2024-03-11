<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoicePdfUpload.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SapIntegration_InvoicePdfUpload" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucNavigationLinks.ascx" TagName="ucLinks" TagPrefix="uc7" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                debugger;
                var container = document.getElementById("uploadedListFiles");
                // var uploadDockType = document.getElementById("ctl00$contentHolderMain$ddlImageType");
                //if (uploadDockType.options[uploadDockType.selectedIndex].value == 0) {

                //    alert('Please Select Upload Document Type.');
                //    return;
                //}
                //var docType = uploadDockType.options[uploadDockType.selectedIndex].text;
                //var docType

                //if (container.getElementsByTagName("uploadedListFiles").length < =3) {//#CC01:commented by rajesh//
                //var imageType = document.getElementById("ctl00_contentHolderMain_ddlImageType");
                //if (imageType.value == 1) {

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
                //}
                //else { alert('Please select image type') }
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
     <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="clear"></div>
     <div class="tabArea">
                    <uc7:uclinks ID="ucLinks" runat="server" XmlFilePath="../../Assets/XML/LinksXML.xml"/></div>
    <%--<div class="mainheading">
        Upload Invoice Info
    </div>--%>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text" style="height: auto">Upload Images:</li>
                <li class="field2" style="height: auto">
                    <div class="uploadContainer">
                        <div class="float-margin" style="height: auto; width: 220px;">
                            <dx:ASPxUploadControl ID="UploadControl" runat="server" Width="220px" ShowUploadButton="True" AddUploadButtonsHorizontalPosition="Left"
                                ShowProgressPanel="True" ClientInstanceName="UploadControl" OnFileUploadComplete="UploadControl_FileUploadComplete" FileInputCount="1"
                                ButtonStyle-Cursor="pointer" ValidationSettings-ErrorStyle-CssClass="error" ButtonStyle-Border-BorderWidth="0"
                                ButtonStyle-Font-Underline="False" ButtonStyle-CssClass="buttonbg">

                                <ValidationSettings AllowedFileExtensions=".pdf" MaxFileSize="500000">
                                </ValidationSettings>
                                <ButtonStyle Cursor="pointer" Font-Underline="False" ForeColor="White" Font-Size="11px" CssClass="buttonbg">
                                </ButtonStyle>
                                <ClientSideEvents FileUploadComplete="function(s, e) { FileUploaded(s, e) }" FileUploadStart="function(s, e) { FileUploadStart(); }" />

                            </dx:ASPxUploadControl>
                            <div class="note3">
                                <b>Note</b>: The total size of files to upload is limited by 500kb.
                            </div>
                        </div>
                        <div class="float-left">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" ClientInstanceName="RoundPanel"
                                HeaderText="Uploaded files (pdf)" Height="100%">
                                <PanelCollection>
                                    <dx:PanelContent runat="server" ID="MM">
                                        <div id="uploadedListFiles" style="height: 85px; width: 200px; overflow: auto; font-family: Arial;">
                                        </div>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </div>
                        <div class="clear"></div>
                    </div>

                </li>
                <li></li>
                <li>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="always">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save All File" OnClick="btnSave_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
