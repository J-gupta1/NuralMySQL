<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControls/ucDisplayBoard.ascx" TagName="ucDisplayBoard" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("../" + strAssets + "/CSS/welcome.css") %>" />--%>
    <link id="WelCss" runat="server" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="welcomepagebg">
        <div class="wel-block">
            <span class="block">Welcome to</span> <span class="wel-txt">Distribution Management
                System</span>
        </div>
        <div class="welcomeleft">
           <%-- <img id="welcomeleft" runat="server" />--%>
        </div>
        <div class="welcomeright">
            <%--<img id="welcomeright" runat="server" />--%>
        </div>
        <div runat="server" id="dvBulletin">
            <div class="bulletinbox">
                <div class="bulletinboxtop">
                    <div class="bulletinboxhd">
                        Latest Bulletin
                    </div>
                </div>
                <div class="bulletinboxmid">
                    <uc1:ucDisplayBoard ID="ucDisplayBoard1" runat="server" />
                    <br />
                    <div class="float-right">
                        <a href="Masters/Common/ViewBulletin.aspx" class="elink4">read more</a>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="bulletinboxbot">
                </div>
            </div>
            <div class="bulletinshadow">
            </div>
        </div>
        <div class="clear"></div>

        <%--
            #CC01 Comment Start
            <div class="notice">Dear Partner,
        <br />
        <br />
        Your A/c balance with us is xxxxx.xx as on 18th July, 2016 4:00 PM. Please <span class="read">click here</span> to download details.
        </div>
            #CC01 Comment End
        --%>
        <%--#CC01 Add Start --%>
        <asp:Panel ID="pnlOutstandingAmount" runat="server" Visible="false">
            <div class="notice">
                Dear Partner,       
                <br />
                <br />
                Your A/c balance with us is 
                <asp:Label ID="lblTotalOutstandingamt" runat="server" Text="0"></asp:Label>
                as on 
                <asp:Label ID="lblDate" runat="server"></asp:Label>. Please 
                <asp:LinkButton ID="lnkOutstandingDownload" runat="server" CssClass="read" Text="click here" OnClick="lnkOutstandingDownload_Click"></asp:LinkButton>
                to download details.
            </div>

        </asp:Panel>
        <%--#CC01 Add End --%>
    </div>
    <div style="float: left; width: 976px; height: 2px; padding: 0px; margin: 0px;">
    </div>
    <%--<asp:Button ID="Button1" runat="server" Text="Click here to show the modal" Style="display: none" />--%>
    <%-- <cc1:ModalPopupExtender ID="ModalPopupExtender1" BackgroundCssClass="ModalPopupBG"
        BehaviorID="modalpopup" runat="server" CancelControlID="btnCancel" OkControlID="btn"
        TargetControlID="Button1" Drag="false" PopupControlID="pnl" PopupDragHandleControlID="PopupHeader">
    </cc1:ModalPopupExtender>--%>
    <asp:Button ID="Button1" runat="server" Text="Click here to show the modal" Style="display: none" />
    <cc1:ModalPopupExtender ID="ModalPopupExtender" BackgroundCssClass="ModalPopupBG"
        BehaviorID="modalpopup" runat="server" CancelControlID="btnCancel" OkControlID="btnSubmit"
        TargetControlID="Button1" Drag="false" PopupControlID="Panel1" PopupDragHandleControlID="PopupHeader">
    </cc1:ModalPopupExtender>
    <div id="Panel1" style="display: none;" class="popupConfirmation">
        <div class="popup_Container">
            <div class="popup_Titlebar" id="PopupHeader">
                <div class="TitlebarLeft">
                    Password Expiry Alert
                </div>
                <div class="TitlebarRight">
                </div>
            </div>
            <div class="popup_Body">
                <p>
                    Your password will expire in
                    <asp:Label ID="lbldays" runat="server"></asp:Label>
                    days. Do you want to change it now?
                </p>
                <div class="popup_Buttons">
                    <input id="btnSubmit" value="Change Now" class="buttonbg" type="button" onclick="location.href = 'ChangePassword.aspx';" />
                    <input id="btnCancel" value="Remind me later" class="buttonbg" type="button" />
                </div>
            </div>
        </div>
    </div>
    <div id="Panel2" style="display: none;" class="popupConfirmation">
        <div class="popup_Container">
            <div class="popup_Titlebar" id="Div2">
                <div class="TitlebarLeft">
                    Password Expiry Alert
                </div>
                <div class="TitlebarRight">
                </div>
            </div>
            <div class="popup_Body">
                <p>
                    Dear User,
                    <br />
                    Your password is expired! Kindly change your password. This is mandatory process
                    from security perspective. Your immediate action would be highly appreciated.
                </p>
                <div class="popup_Buttons">
                    <input id="btnSubmit1" value="Proceed" type="button" class="buttonbg" onclick="location.href = 'ChangePassword.aspx';" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
