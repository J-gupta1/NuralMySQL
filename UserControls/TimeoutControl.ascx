<%@ Control Language="C#" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxGlobalEvents"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>

<script runat="server">
    public string TimeOutUrl = "~\\Logout.aspx";
    public int PopupShowDelay
    {
        get { return 60000 * (Session.Timeout - 1); }
    }
    protected string QuotedTimeOutUrl
    {
        get { return '"' + ResolveClientUrl(TimeOutUrl).Replace("\"", "\\\"") + '"'; }
    }
</script>

<script type="text/javascript">

    window.SessionTimeout = (function() {
        var _timeLeft, _popupTimer, _countDownTimer;

        var stopTimers = function() {
            window.clearTimeout(_popupTimer);
            window.clearTimeout(_countDownTimer);
        };

        var updateCountDown = function() {
            var min = Math.floor(_timeLeft / 60);
            var sec = _timeLeft % 60;
            if(sec < 10)
                sec = "0" + sec;

            document.getElementById("CountDownHolder").innerHTML = min + ":" + sec;

            if(_timeLeft > 0) {
                _timeLeft--;
                _countDownTimer = window.setTimeout(updateCountDown, 1000);
            } else  {
                document.location = <%= QuotedTimeOutUrl %>;
            }            
        };

        var showPopup = function() {
            _timeLeft = 60;
            updateCountDown();
            ClientTimeoutPopup.Show();
        };

        var schedulePopup = function() {       
            stopTimers();
            _popupTimer = window.setTimeout(showPopup, <%= PopupShowDelay %>);
        };

        var sendKeepAlive = function() {
            stopTimers();
            ClientTimeoutPopup.Hide();
            ClientKeepAliveHelper.PerformCallback();
        };

        return {
            schedulePopup: schedulePopup,
            sendKeepAlive: sendKeepAlive
        };

    })();    

</script>

<dx:ASPxPopupControl runat="server" ID="TimeoutPopup" ClientInstanceName="ClientTimeoutPopup"
    CloseAction="None" HeaderText="Session Expiring" Modal="True" PopupHorizontalAlign="WindowCenter"
    PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="274px" ShowFooter="True"
    AllowDragging="True" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
    <contentstyle verticalalign="Top">
    </contentstyle>
    <contentcollection>
        <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            Your session is about to expire!
            <br /><br />
            <span id="CountDownHolder"></span>
            <br /><br />
            Click OK to continue your session.
        </dx:PopupControlContentControl>
    </contentcollection>
    <footertemplate>
        <dx:ASPxButton runat="server" ID="OkButton" Text="OK" AutoPostBack="false" >
            <ClientSideEvents Click="SessionTimeout.sendKeepAlive" />
        </dx:ASPxButton>
    </footertemplate>
</dx:ASPxPopupControl>
<dx:ASPxGlobalEvents runat="server" ID="GlobalEvents">
    <ClientSideEvents ControlsInitialized="SessionTimeout.schedulePopup" />
</dx:ASPxGlobalEvents>
<dx:ASPxCallback runat="server" ID="KeepAliveHelper" ClientInstanceName="ClientKeepAliveHelper">
</dx:ASPxCallback>
