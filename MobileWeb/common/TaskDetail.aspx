<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaskDetail.aspx.cs" Inherits="TaskDetail" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl" TagPrefix="uc3" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    
    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/" + strAssets + "/Jscript/jquery.jcarousel.min.js") %>"></script>

     <link href="~/Assets/ZedSales/CSS/popup.css" rel="stylesheet" type="text/css" />
    <link href="~/Assets/ZedSales/Css/style.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../Assets/ZedSales/Css/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <link href="../../Assets/ZedSales/Css/modal.css" rel="stylesheet" type="text/css" />--%>

    <%--<link rel="stylesheet" type="text/css" href="~/Assets/ZedSales/CSS/dhtmlwindow.css" />
    <link rel="stylesheet" type="text/css" href="~/Assets/ZedSales/CSS/modal.css" />--%>
    <%--Add START  --%>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <%--Add END  --%>

    <title></title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
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

        function FileUploaded(s, e) {
            if (e.isValid) {
                //debugger;
                var container = document.getElementById("uploadedListFiles");

                var linkFile = document.createElement("a");
                var indexSeparator = e.callbackData.indexOf(fieldSeparator);
                var fileName = e.callbackData.substring(0, indexSeparator);
                var pictureUrl = e.callbackData.substring(indexSeparator + fieldSeparator.length);
                var imgSrc = pictureUrl;
                fileName = fileName;
                linkFile.innerHTML = fileName;
                linkFile.setAttribute("href", imgSrc);
                linkFile.setAttribute("target", "_blank");
                container.appendChild(linkFile);
                container.appendChild(document.createElement("br"));


            }
        }
        function upload() {
            var btn = document.getElementById("ctl00_contentHolderMain_btnviewdocument");
            if (btn != null) {
                __doPostBack(btn.name, "OnClick");

            }
        }


    </script>
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
</head>
<body>

    <form id="form1" name="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <asp:HiddenField ID="hdfSuccess" runat="server" Value="0" />
            <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                <ContentTemplate>
                    <uc1:ucMessage ID="ucMsg" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
                    <div class="mainheading">
                        View / Update Task
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory
                        </div>
                        <div class="H25-C3-S">
                            <table cellpadding="4" cellspacing="0" border="0" width="100%">
                                <tr class="gridrow">
                                    <td align="left" valign="top" class="frmtxt1">Task
                                    </td>
                                    <td colspan="5" align="left" valign="top">
                                        <asp:Label ID="txtTask" CssClass="formtext" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="frmtxt1">Start Date
                                    </td>

                                    <td align="left" valign="top" class="frmtxt1">
                                        <asp:Label ID="txtStartDate" CssClass="formtext" runat="server"></asp:Label>

                                    </td>


                                    <td align="left" valign="top" class="frmtxt1">End Date:
                                    </td>
                                    <td align="left" valign="top" class="frmtxt1">
                                        <asp:Label ID="txtEndDate" runat="server" CssClass="formtext"></asp:Label>
                                    </td>
                                    <td align="left" valign="top" class="frmtxt1">Priority
                                    </td>
                                    <td align="left" valign="top" class="frmtxt1">
                                        <asp:Label ID="txtPriority" CssClass="formtext" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="frmtxt1">Task Remark

                                    </td>
                                    <td colspan="5" align="left" valign="top" class="frmtxt1">
                                        <asp:Label ID="txtTaskRemark" runat="server" CssClass="formtext"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="frmtxt1">
                                        <asp:Label ID="lblNewStatus" CssClass="formtext" runat="server" AssociatedControlID="ddlTaskStatus">Task Status:<span class="error">*</span></asp:Label>
                                    </td>
                                    <td class="field">
                                        <div>
                                            <asp:DropDownList ID="ddlTaskStatus" runat="server" CssClass="formselect"
                                                AutoPostBack="false">
                                            </asp:DropDownList>
                                        </div>
                                        <div>
                                            <asp:Label Style="display: none;" runat="server" ID="Label3" CssClass="error"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTaskStatus" CssClass="error"
                                                Display="Dynamic" InitialValue="0" ErrorMessage="Please select task status." SetFocusOnError="true"
                                                ValidationGroup="ResponseGroup"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td align="left" valign="top" class="frmtxt1">Update Remark

                                    </td>
                                    <td align="left" valign="top" class="frmtxt1">
                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="formfields" MaxLength="500" TextMode="MultiLine"
                                            ></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="frmtxt1">Next Closure Date:
                                    </td>
                                    <td align="left" valign="top" class="frmtxt1">
                                        <uc2:ucDatePicker ID="UcNextClosureDate" runat="server"
                                            IsRequired="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="frmtxt1">Upload Images:</td>
                                    <td align="left" colspan="5" valign="top" class="frmtxt1">
                                        <div class="uploadContainer">
                                            <div class="float-margin" style="height: auto; width: 220px;">
                                                <dx:ASPxUploadControl ID="UploadControl" runat="server" Width="220px" ShowUploadButton="True" AddUploadButtonsHorizontalPosition="Left"
                                                    ShowProgressPanel="True" ClientInstanceName="UploadControl" OnFileUploadComplete="UploadControl_FileUploadComplete" FileInputCount="1"
                                                    ButtonStyle-Cursor="pointer" ValidationSettings-ErrorStyle-CssClass="error" ButtonStyle-Border-BorderWidth="0"
                                                    ButtonStyle-Font-Underline="False" ButtonStyle-CssClass="buttonbg">
                                                    <ValidationSettings AllowedFileExtensions=".jpg,.JPG,.jpeg,.JPEG,.png,.PNG,.bmp,.BMP" MaxFileSize="500000">
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
                                                    HeaderText="Uploaded Images" Height="100%">
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

                                    </td>


                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="frmtxt1">
                                        <div class="float-margin">
                                            
                                            <asp:Button ID="Button1" Text="Submit" runat="server" CausesValidation="false"
                                                 OnClick="Button1_Click" ToolTip="Submit"
                                                CssClass="buttonbg" />
                                        </div>
                                    </td>
                                    <td align="left" valign="top" class="frmtxt1">
                                        <div class="float-margin">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg" OnClick="btnCancel_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                <%--</ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSaveResponse" />
                </Triggers>
            </asp:UpdatePanel>--%>
            <div class="mainheading">
                Task Updates
            </div>

            <div class="contentbox">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                <div class="grid1">

                    <asp:GridView ID="grdvwTask" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                        RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                        HeaderStyle-HorizontalAlign="left" DataKeyNames="ImageRelativePath" HeaderStyle-VerticalAlign="top" GridLines="none"
                        AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                        HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                        BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="true"
                        SelectedStyle-CssClass="gridselected" OnRowDataBound="grdvwTask_RowDataBound"
                        >
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                        <Columns>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TaskUser"
                                HeaderText="Task User"></asp:BoundField>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ResponseBy"
                                HeaderText="Update By"></asp:BoundField>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ResponseDate"
                                HeaderText="Update Date"></asp:BoundField>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TaskStatus"
                                HeaderText="Status"></asp:BoundField>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="NextClosureDate"
                                HeaderText="Next Closure Date"></asp:BoundField>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Remark"
                                HeaderText="Remark"></asp:BoundField>
                            <asp:TemplateField HeaderText="Image Capture">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" CssClass="elink" Text="Download" CommandArgument='<%# Eval("ImageRelativePath")%>' OnClick="lnkDownload_Click" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                    <div class="clear">
                        </div>
                </div>
                <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>

                </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grdvwTask"  />
                        
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <%--     <uc3:footer ID="Footer1" runat="server" />--%>
        </div>
    </form>
</body>
</html>
