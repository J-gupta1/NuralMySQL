<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageEditPriceListWState.aspx.cs"
    Inherits="Masters_Common_ManageEditPriceListWState" %>

<!DOCTYPE html>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>

<%--<html xmlns="http://www.w3.org/1999/xhtml">--%>
<html lang="en">

<head id="Head1" runat="server">
    <%--Add START  --%>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <%--Add END  --%>
    <title>State Price List</title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />

    <script src="../../Assets/Jscript/jquery-1.4.4.min.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <%--<style type="text/css">
        .expand {
            width: auto !important;
            position: relative;
            min-width: 150px;
            z-index: 999;
            border: 1px solid #7e95d4;
            font-family: arial;
            font-size: 11px;
            color: #000000;
            font-weight: normal;
            background-color: #fff;
            float: left;
        }
    </style>--%>

    <script type="text/javascript">
        function changeCSSDropDown1() {
            try {
                $("select").focusin(function() {

                    $(this).removeClass('formselect');
                    $(this).addClass('expand');
                });
                $("select").focus(function() {
                    $(this).removeClass('formselect');
                    $(this).addClass('expand');
                });
                $("select").blur(function() {

                    $(this).removeClass('expand');
                    $(this).addClass('formselect');
                });
                $("select").focusout(function() {

                    $(this).removeClass('expand');
                    $(this).addClass('formselect');
                });

                $("select").change(function() {

                    $(this).removeClass('expand');
                    $(this).addClass('formselect');
                });

                $("select").change(function() {

                    $(this.blur()).removeClass('expand');
                    $(this.blur()).addClass('formselect');
                });
            } catch (e) {

            }
        }

    </script>

</head>
<body onload="changeCSSDropDown1();">
    <div>
        <form id="form1" name="form1" runat="server">
            <asp:ScriptManager ID="s" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uc1:ucMessage ID="ucMessage1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="mainheading">
                        Price List Information
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory            
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">
                                    <asp:Label ID="lblStateName" runat="server" Text="">State Name: <span class="error">*</span></asp:Label>
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlStateName" runat="server" CssClass="formselect" AutoPostBack="false">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="valstate" ControlToValidate="ddlStateName"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select a State " InitialValue="0" ValidationGroup="Price" />
                                </li>
                                <li class="text">Price list Name:
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlInsertPriceList" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="valpricename" ControlToValidate="ddlInsertPriceList"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select a Price Name " InitialValue="0"
                                        ValidationGroup="Price" />
                                </li>
                            </ul>
                            <ul>
                                <li class="text">
                                    <asp:Label ID="lblpreffdt" runat="server" Text="">Price Effective Date:<span class="error">*</span></asp:Label>
                                </li>
                                <li class="field">
                                    <uc2:ucDatePicker ID="ucEffectiveDate" runat="server" RangeErrorMessage="Invalid date."
                                        ErrorMessage="Please select a date" ValidationGroup="Price" />
                                </li>
                                <li class="text"></li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnMapping" runat="server" OnClick="btnSubmitPrice_click" Text="Submit"
                                            CausesValidation="True" ValidationGroup="Price" CssClass="buttonbg" />
                                    </div>
                                    <div class="float-margin">
                                        <input type="button" value="Close" class="buttonbg" onclick="parent.WinSearchChannelCode.hide();" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
