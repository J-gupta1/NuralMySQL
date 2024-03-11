<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageFinacialCalenderMaster.aspx.cs" Inherits="Masters_HO_Admin_ManageFinacialCalenderMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script language="javascript" type="text/javascript">

     function OnYearChange(id) {
         debugger;
         if ((id == null) || (id.value == '')) return;
         SapIntegrationService.ISCalenderExists(id.value,
            OnChange = function OnChange(result, userContext) {
                if (result) {
                    alert('Calender(' + id.value + ') already defined.If Proceed then calender will be update.');
//                    id.value = '';
                }
            }, OnError);
        }
        function OnError(result) {
            alert("Error: " + result.get_message());
        }

       </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uc4:ucMessage ID="uclblMessage" runat="server" />
            </ContentTemplate>
            <%--#CC01 Add Start--%>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
                <asp:PostBackTrigger ControlID="btnReset" />
                <asp:PostBackTrigger ControlID="btnSearch" />
                <asp:PostBackTrigger ControlID="btnSReset" />
                <asp:PostBackTrigger ControlID="GridCalender" />
            </Triggers>

            <%--#CC01 Add End --%>
        </asp:UpdatePanel>
        <div class="mainheading">
            Input Parameters
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdMain" runat="server">
                <ContentTemplate>
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">Financial Year:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtCalenderYear" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                               
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCalenderYear"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter calander name. "
                                    ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Quarter Name:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtQuarter" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtQuarter"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter quarter name. "
                                    ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Period Name:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtFortnightName" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFortnightName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter fortnight name. "
                                    ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </li>
                        </ul>
                        <ul>
                            <li class="text">Period Start Date:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <uc1:ucDatePicker ID="ucStartDate" ErrorMessage="Please enter date." ValidationGroup="Save" runat="server" />
                            </li>
                            <li class="text">Period End Date:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <uc1:ucDatePicker ID="ucEndDate" ErrorMessage="Please enter date." ValidationGroup="Save" runat="server" />
                            </li>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Submit" OnClick="btnSave_Click"
                                        ValidationGroup="Save" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnReset" CssClass="buttonbg" runat="server" OnClick="btnReset_Click" CausesValidation="false"
                                        Text="Reset" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="mainheading">
            Search
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server">
                <ContentTemplate>
                    <div class="H20-C3-S">
                        <ul>
                            <li class="text">Period Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtserchFortnightName" MaxLength="50" runat="server" CssClass="formfields"></asp:TextBox><br />
                            </li>
                            <li class="text">Financial Year:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtserchCalName" MaxLength="50" runat="server" CssClass="formfields"></asp:TextBox><br />
                            </li>
                        </ul>
                        <ul>
                            <li class="text">Date From:
                            </li>
                            <li class="field">
                                <uc1:ucDatePicker ID="ucSearchStartDate" runat="server" />
                            </li>
                            <li class="text">Date To:
                            </li>
                            <li class="field">
                                <uc1:ucDatePicker ID="ucSearchEndDate" runat="server" />
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                        CssClass="buttonbg" CausesValidation="False" ValidationGroup="search" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnSReset" Text="Reset" runat="server" CssClass="buttonbg" CausesValidation="False"
                                        OnClick="btnSReset_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            List
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GridCalender" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                            RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="Left" RowStyle-VerticalAlign="top"
                            SelectedStyle-CssClass="gridselected" Width="100%" AutoGenerateColumns="false"
                            OnRowCommand="GridCalender_RowCommand"
                            OnPageIndexChanging="GridCalender_PageIndexChanging">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <PagerStyle CssClass="gridfooter" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="FinancialCalenderYearName"
                                    HeaderText="Financial Year"><%--#CC01 Column Name Changed from "Calender Name" to "Financial Year"--%>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="FinancialCalenderFortnightName"
                                    HeaderText="Period Name">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="FinancialCalenderQuarterName"
                                    HeaderText="Quarter Name">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="FinancialCalenderFortnightStartDate"
                                    HeaderText="Period Start Date" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="FinancialCalenderFortnightEndDate"
                                    DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Period End Date">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" CommandArgument='<%#Eval("FinancialCalenderID") %>'
                                            CommandName="cmdDelete" ImageAlign="Top" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" CommandArgument='<%#Eval("FinancialCalenderID") %>'
                                            CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
