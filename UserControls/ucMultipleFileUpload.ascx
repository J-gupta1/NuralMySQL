<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucMultipleFileUpload.ascx.cs"
    Inherits="UserControl_ucMultipleFileUpload" %>
     <%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc3" %> 

<script type="text/javascript">
    var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png", ".JPG", ".JPEG", ".pdf", ".doc", ".docx", ".xls", ".xlsx"];
        
</script>

<div class="innerarea">
    <div id="dvError" runat="server" style="display: none">
        <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>   
        <uc3:ucMessage ID="ucMessage1" runat="server" /> 
    </div>
    <asp:Panel ID="pnlParent" runat="server" Width="100%">
        <div class="padding">
            <div class="float-margin">
                <asp:DropDownList ID="ddlImageType" runat="server" CssClass="formselect" Width="150px">
                </asp:DropDownList>
                <br />
                <asp:Label runat="server" CssClass="error" ID="lblMessage" Text="" Style="display: none"></asp:Label>
            </div>
            <div class="float-margin">
                <asp:Panel ID="pnlFiles" runat="server" HorizontalAlign="Left">
                    <asp:FileUpload ID="IpFile" runat="server" CssClass="fileuploads" Width="300px" />
                </asp:Panel>
                <div class="gridrow_black">
                   ( File size should not exceed
                    <asp:Label ID="lblSizeDisplay" runat="server"></asp:Label>. Only .JPG, .JPEG, .BMP, .GIF, .PNG, .PDF, .DOC, .DOCX,.Xlxs, files are allowed)
                </div>
            </div>
            <div class="clear" style="padding-bottom: 10px;">
            </div>
            <div class="frmtxt" style="height: 100px; margin: 0px;">
                <asp:Panel ID="pnlListBox" runat="server" BorderStyle="Inset">
                </asp:Panel>
            </div>
            <asp:Panel ID="pnlButton" runat="server" HorizontalAlign="Right">
                <input id="btnAdd" onclick="javascript:Add();" style="width: 60px" type="button"
                    runat="server" value="Add"  class="button1" />
                <input id="btnClear" onclick="javascript:Clear();" style="width: 60px" type="button"
                    value="Clear" runat="server" class="button1" />
                <asp:Button ID="btnUpload" OnClientClick="javascript:return DisableTop();" runat="server"
                    Text="Upload" Width="60px" OnClick="btnUpload_Click" class="button1" />
                
                <asp:Button ID="btnSaveImage" runat="server" Text="Save" Width="60px"
                     class="button1" Visible="false" OnClick="btnSaveImage_Click"  />
                 
                <br />
                <asp:Label ID="lblCaption" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Smaller"
                    ForeColor="Gray"></asp:Label>&nbsp;
            </asp:Panel>
            <asp:HiddenField ID="hdnFinalList" runat="server" />
        </div>
    </asp:Panel>
</div>
