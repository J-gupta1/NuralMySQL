<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MultipleFileUpload.aspx.cs"
    Inherits="Common_MultipleFileUpload" %>

<%@ Register Src="~/UserControls/ucMultipleFileUpload.ascx" TagName="ucMultipleFileUpload"
    TagPrefix="ucMFU" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link rel="stylesheet" type="text/css" href="../../Assets/Comio/CSS/modal.css" />

</head>
<body style="background-color: White">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptid" runat="server"></asp:ScriptManager>
    <div>
        <asp:HiddenField ID="hdnImageType" runat="server" Value="0" />
         <asp:HiddenField ID="hdnRemoveDefaultSession" runat="server" Value="0" />
         <asp:Label runat="server" ID="lblMessage" CssClass="error"></asp:Label>
      <%--  <ucMFU:ucMultipleFileUpload ID="UserControlMultipleFileUpload" runat="server" />--%>
    </div>
    <asp:HiddenField ID="hdnfFileList" runat="server" />
          <div class="contentbox">
    <table cellpadding="4" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" valign="top" width="15%">
                <asp:Label runat="server" CssClass="frmtxt1" ID="labelheadertext" Text="Upload Files"></asp:Label>
            </td>
            <td align="left" valign="top" width="35%" class="frmtxt">
                <div style="border: none; outline: none; background-color: White; font-size: 11px;
                    font-family: Arial;">
                    <asp:ListBox ID="lsDetailImages" runat="server" Font-Size="11px" ForeColor="Black"
                        Enabled="true" CssClass="list-border" Font-Strikeout="False"></asp:ListBox>
                </div>
            </td>
            <td align="left" valign="top" width="5%">
            </td>
            <td align="left" valign="top" width="10%">
                <asp:Label runat="server" CssClass="frmtxt1" ID="lblPreviousFiles" Text="Previous Files"></asp:Label>
            </td>
            <td align="left" valign="top">
                <asp:Label runat="server" CssClass="frmtxt" ID="lblAlreadyUploadedFiles" Text=""></asp:Label>
            </td>
            <td align="left" valign="top">
                
            </td>
        </tr>
        <tr>
        <td>
             <ul>
                            <li class="text" style="height:auto">Upload Images :
                            <li class="field" style="height:auto">                            
                                <dx:ASPxUploadControl ID="UploadControl" runat="server" Width="300px" ShowUploadButton="True" ShowAddRemoveButtons="True" AddUploadButtonsHorizontalPosition="Left"
                                    ShowProgressPanel="True" ClientInstanceName="UploadControl"
                                    OnFileUploadComplete="UploadControl_FileUploadComplete" ButtonStyle-Cursor="pointer"
                                    ButtonStyle-ForeColor="White" ButtonStyle-Font-Underline="False" ButtonStyle-CssClass="button1">

                                    <ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif,.png,.pdf,.Doc,.Docx,.xls,.xlsx,.rtf" MaxFileSize="500000">
                                    </ValidationSettings>
                                    <ButtonStyle Cursor="pointer" Font-Underline="False" ForeColor="White" CssClass="button2">
                                    </ButtonStyle>
                                    <ClientSideEvents FileUploadComplete="function(s, e) { FileUploaded(s, e) }" FileUploadStart="function(s, e) { FileUploadStart(); }" />
                                </dx:ASPxUploadControl>
                                <div class="note3">
                                    <b>Note</b>: The total size of files to upload is limited by 500kb.
                                </div>
                            </li>
                            <li class="text" style="height:auto"></li>
                            <li class="field" style="height:auto">
                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="300px" ClientInstanceName="RoundPanel"
                                    HeaderText="Uploaded files (jpeg,gif,png,pdf,docx,Doc,xlsx)" Height="100%">
                                    <PanelCollection>
                                        <dx:PanelContent runat="server" ID="MM">
                                            <div id="uploadedListFiles" style="height: 65px; font-family: Arial;">
                                            </div>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxRoundPanel>
                            </li>
                        </ul>
        </td>
        <td>
             
          <asp:Button ID="btndeleteFile" runat="server" CausesValidation="False" CssClass="button1"
                                                    Text="Delete" 
                      onclick="btndeleteFile_Click" />
        </td>
        </tr>
    </table>
              </div>
    </form>
</body>
</html>
