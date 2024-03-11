<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SonyDefault.aspx.cs" Inherits="Retailer_Specific_SonyDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="main-container" style="margin-top: 33px;">
        <div class="main-body">
            <div class="inner-wrapper">
                <%--Kalpana- all image path  temporally changed (img path is hard coded)--%>
                <div class="title" style="padding-bottom: 10px; padding-top: 10px;">
                    <img src="../../Assets/ZedSales/CSS/Images/title.gif" /></div>
                <div class="left-panel">
                    <div class="panel-list">
                        <div class="icon-float">
                            <img src='<%= "../../" + strAssets + "/CSS/Images/sales-icon.png"%>' /></div>
                        <div class="txt-float">
                            <a href='<%=strSiteUrl%>Retailer/Specific/RetailerSalesEntry.aspx'>Sales Entry</a></div>
                    </div>
                    <div class="panel-list">
                        <div class="icon-float">
                            <img src="../../Assets/ZedSales/CSS/images/scheme-icon.png" width="31" height="24" /></div>
                        <div class="txt-float">
                            <a href='<%=strSiteUrl%>Masters/HO/Admin/ViewSchemeDetails.aspx'>Scheme Details</a></div>
                    </div>
                    <div class="panel-list">
                        <div class="icon-float">
                            <img src="../../Assets/ZedSales/CSS/images/payout-icon.png" width="24" height="23" /></div>
                        <div class="txt-float">
                            <a href='<%=strSiteUrl%>Masters/HO/Admin/ViewSchemePayout.aspx'>Scheme Payouts </a>
                        </div>
                    </div>
                    <div class="panel-list" style="display: none">
                        <div class="icon-float">
                            <img src="../../Assets/ZedSales/CSS/images/report-icon.png" width="30" height="25" /></div>
                        <div class="txt-float">
                            <a href='<%=strSiteUrl%>Masters/Common/ViewPriceV2.aspx'>Reports</a></div>
                    </div>
                    <div class="panel-list">
                        <div class="icon-float">
                            <img src="../../Assets/ZedSales/CSS/images/price-list-icon.png" width="25" height="25" /></div>
                        <div class="txt-float">
                            <a href='<%=strSiteUrl%>Masters/Common/ViewPriceV2.aspx'>Current Price Lists</a></div>
                    </div>
                    <!--<div class="panel-list">
          <div class="icon-float"><img src="images/new-icon.png" width="27" height="25" /></div>
          <div class="txt-float">What’s New </div>
        </div> -->
                    <!--<div class="news-area">
          <div class="news-hd">What's New</div>
          <div class="marque"><marquee id="test" behavior="scroll" direction="up" height="150" scrolldelay="100" scrollamount="2" onMouseOver="document.all.test.stop()" onMouseOut="document.all.test.start()" HSpace="1">

  <div class="txt2">ABC enabling growth through technology</div><br />
            <div class="news-flash">
              <ul>
                <li>Microsoft SmartScreen Filter can help protect you from phishing attacks, online fraud, and spoofed websites, as well as websites that distribute malicious software. For more information, see SmartScreen Filter: frequently asked questions.</li>
                
                <li>InPrivate Browsing enables you to surf the web without leaving a trail in Internet Explorer. This helps prevent anyone else who might be using your computer from seeing where you visited and what you looked at on the web. For more information, see What is InPrivate Browsing?</li>
                
                <li>InPrivate Filtering helps you prevent websites from sharing your browsing habits. For more information see InPrivate: frequently asked questions. </li>
                
                <li>Higher security levels can help protect you from hackers and web attacks. The Internet Explorer Cross-site Scripting (XSS) Filter can help prevent malicious websites from stealing your personal information when you visit trusted sites. For more information, see How does Internet Explorer help protect me from cross-site scripting attacks?</li>
              </ul>
            </div>
            </marquee>
          </div>
        </div> -->
                    <!-- -->
                </div>
                <div class="right-panel">
                    <img src='<%= "../../" + strAssets + "/CSS/images/camp-pd.png"%>' border="0" style="float: right;
                        padding-right: 20px;" /></div>
                <div class="clear" style="padding-top: 2px;">
                </div>
                <div class="ad-section">
                    <div class="float-margin">
                        <a href="../../Masters/Common/ViewBulletin.aspx">
                            <img src='<%= "../../" + strAssets + "/CSS/images/wht-new.jpg"%>' border="0" /></a></div>
                    <div class="float-right">
                        <a href="ViewAdCampaigns.aspx">
                            <img src='<%= "../../" + strAssets + "/CSS/images/ad.jpg"%>' border="0" /></a></div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
