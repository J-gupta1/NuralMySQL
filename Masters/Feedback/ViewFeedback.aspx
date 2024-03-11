<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewFeedback.aspx.cs" Inherits="Masters_Feedback_ViewFeedback" %>

<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc4" %>

<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Revert Feedback
    </div>
    <asp:UpdatePanel ID="updFeedbackgrd" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">From Date:<span class="error">*</span></li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucFromDate" runat="server" IsRequired="true"
                                RangeErrorMessage="invalid Date" ErrorMessage="Invalid Date" />
                        </li>
                        <li class="text">To Date:<span class="error">*</span></li>
                        <li class="field">
                            <uc2:ucDatePicker ID="UcToDate" runat="server" IsRequired="false"
                                RangeErrorMessage="invalid Date" ErrorMessage="Invalid Date" />
                        </li>
                        <li class="text">
                            <asp:Label ID="Label2" runat="server" Text="Date Filter on :"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlDateFilterType" runat="server" CssClass="formselect">
                                <asp:ListItem Text="Created On" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Replied On" Value="2"></asp:ListItem>

                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="Label1" runat="server" Text="Feedback Status :"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlFeedbackStatus" runat="server" CssClass="formselect">
                                <asp:ListItem Text="All" Value="2"></asp:ListItem>
                                <asp:ListItem Text="New" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Replied" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <asp:Button ID="btnSearch" runat="server" CssClass="buttonbg" CausesValidation="true" Text="Search" OnClick="btnSearch_Click" />
                        </li>
                    </ul>
                </div>
            </div>
            <div id="dvhide" runat="server" visible="false">
                <div class="mainheading">
                    List
                </div>
                <div class="export">
                    <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GvViewFeedback" runat="server" AlternatingRowStyle-CssClass="Altrow"
                            AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                            PageSize='<%$ AppSettings:GridViewPageSize %>' FooterStyle-CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                            RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            SelectedStyle-CssClass="gridselected" Width="100%" OnRowCommand="GvViewFeedback_RowCommand"
                            OnRowDataBound="GvViewFeedback_RowDataBound" AllowPaging="false">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>


                                <asp:TemplateField HeaderText="S.No.">
                                    <HeaderStyle Width="20px" Wrap="False" />
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1%>.
                                                                                        <asp:HiddenField ID="hdnFeedbackId" runat="server" Value='<%#Eval("FeedbackId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Feedback" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />

                                    <ItemTemplate>

                                        <%-- #CC01 Comment Start  <asp:Label ID="lblFeedbackText" runat="server" Text='<%#Eval("FeedbackText") %>'   ></asp:Label>
                                                                                            
                                                                                            #CC01 Comment End
                                        --%>
                                        <%--#CC01 Add Start--%>
                                        <div style="word-wrap: break-word; word-break: break-all; width: 50px">
                                            <asp:Label ID="lblFeedbackText" runat="server" CssClass="break-word" Text='<%# System.Web.HttpUtility.HtmlEncode(Convert.ToString(Eval("FeedbackText"))) %>'></asp:Label>
                                        </div>
                                        <%--#CC01 Add End--%>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Created On" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />

                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedOn" runat="server" Text='<%#Eval("CreatedOn") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Feedback Reverted Text" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />

                                    <ItemTemplate>
                                        <asp:Label ID="lblFeedbackRevertText" runat="server" Text='<%#Eval("FeedbackRevertText") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Feedback Revert Date" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />

                                    <ItemTemplate>
                                        <asp:Label ID="lblFeedbackRevertDate" runat="server" Text='<%#Eval("FeedbackRevertDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Feedback Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />

                                    <ItemTemplate>
                                        <asp:Label ID="lblFeedbackStatus" runat="server" Text='<%#Eval("FeedbackStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Feedback Created By" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />

                                    <ItemTemplate>
                                        <asp:Label ID="lblFeedbackCreatedBy" runat="server" Text='<%#Eval("FeedbackCreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>

                                        <asp:Button ID="btnRevert" runat="server" CssClass="buttonbg" Text="Revert Feedback" CauseValidation="false" CommandArgument='<%#Eval("FeedbackId") %>' CommandName="cmdEdit" />



                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <div class="clear">
                        </div>
                    </div>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="ucPagingControl1_SetControlRefresh" />
                    </div>
                </div>
            </div>
            <div id="dvUpdFeedback" runat="server" style="display: none">

                <%--  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="80%" align="left" class="tableposition">
                                                                <div class="mainheading">
                                                                    &nbsp;Revert Feedback
                                                                </div>
                                                            </td>
                                                            <td width="20%" align="right"></td>
                                                        </tr>
                                                    </table>--%>
                <div class="contentbox">
                    <div class="H35-C3-S">
                        <ul>
                            <li class="text-field">
                                <asp:HiddenField ID="hdnFeedbackID" runat="server" Value="0" />

                                <uc4:ucTextboxMultiline ID="txtRevertfeedback" runat="server" IsRequired="true" CharsLength="500"
                                    TextBoxWatermarkText="Please enter feeback." ErrorMessage="Please enter feeback."
                                    ValidationGroup="Add" />

                                <%--  <asp:TextBox ID="txtRevertfeedback" runat="server" CssClass="form_textarea" TextMode="MultiLine"
                                                                        ValidationGroup="Add"></asp:TextBox>
                                                                    <br />
                                                                    <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtRevertfeedback"
                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter feeback."
                                                                        SetFocusOnError="true" ValidationGroup="Add" MaxLength="500"></asp:RequiredFieldValidator>--%>
                            </li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnRevertFeedback" runat="server" CssClass="buttonbg" CausesValidation="true" ValidationGroup="Add" Text="Save" OnClick="btnRevertFeedback_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnRevertFeedback" />
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="ExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
