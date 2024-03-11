<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageAppMenuHelp.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Master_Common_ManageAppMenuHelp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>

<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagPrefix="TM" TagName="ucTextboxMultiline" %>

<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <uc4:ucMessage ID="ucMessage1" runat="server" />
                    <div class="clear"></div>
                    <div class="mainheading">
                        Manage AppMenuHelp Section
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory. (+) Fill at least one of them.
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">App Menu:<font class="error">*</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlAppMenu"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAppMenu_SelectedIndexChanged">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                          <asp:RequiredFieldValidator ID="Required_ddlAppMenu" ValidationGroup="createval" runat="server"
                                            ControlToValidate="ddlAppMenu" ErrorMessage="Please Select Menu Name" InitialValue="0"
                                            CssClass="error" Display="Dynamic" ForeColor=""></asp:RequiredFieldValidator>
                                </li>
                                <li class="text">Question:<font class="error">*</font>
                                </li>
                                <li class="field">
                                  <TM:ucTextboxMultiline runat="server" ID="txt_question"  TextBoxWatermarkText="Question"  CharsLength="500" ValidationGroup="createval" IsRequired="true" ErrorMessage="Please Enter Question"/>
                                <%--  <asp:RequiredFieldValidator ID="reqQuest" runat="server" ControlToValidate="txt_question"
                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Question."
                                                    SetFocusOnError="true" ValidationGroup="createval"></asp:RequiredFieldValidator>

                                                <asp:RegularExpressionValidator ID="regquest"ControlToValidate="txt_question" 
                                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^<>/\@%]{1,50}"
                                                    ValidationGroup="createval" runat="server" />--%>
                                </li>
                                 

                                <li class="text">Help Link:<font class="error">*</font>
                                </li>
                                <li class="field">
                                    <TM:ucTextboxMultiline runat="server" ID="txt_helplink" TextBoxWatermarkText="Help Link"  CharsLength="300" ValidationGroup="createval" IsRequired="true" ErrorMessage="Please Enter Link URL"/>
                                   
                                </li>
                                 <li class="text">Display Order:<font class="error">*</font>
                                </li>
                                <li class="field">
                                    
                                     <asp:TextBox ID="txtDO" runat="server" CssClass="formfields-W70" MaxLength="3" Width="30px"   ></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftorders" runat="server" FilterMode="ValidChars"
                                                        FilterType="Custom" TargetControlID="txtDO" ValidChars="0123456789" >
                                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="reqQuest" runat="server" ControlToValidate="txtDO"
                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter display order."
                                                    SetFocusOnError="true" ValidationGroup="createval"></asp:RequiredFieldValidator>
                                        
                                </li>
                                <li class="field3"  style="padding:8px">
                                    <div class="float-margin" >
                                        <asp:Button ID="btncreate" runat="server" Text="Create" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="createval" OnClick="btncreate_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnExportexcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                            CausesValidation="true" OnClick="btnExportexcel_Click" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="PnlGrid" runat="server">
                        <div class="mainheading">
                            View Attendance Details                                                    
                        </div>
                        <div class="contentbox">
                            <div class="grid">
                                <asp:GridView ID="gvAttendanceDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"  
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowCommand="gvAttendanceDetail_RowCommand">
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                       <%-- <asp:BoundField DataField="AttendanceDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Attendance Date"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="MenuId" Visible="false" />

                                        <asp:BoundField DataField="MenuName" HeaderStyle-HorizontalAlign="Left" HeaderText="Menu Name" 
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="QuestionText" HeaderStyle-HorizontalAlign="Left" HeaderText="Question Text"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HelpLink" HeaderStyle-HorizontalAlign="Left" HeaderText="Help Link "
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="DisplayOrder" HeaderStyle-HorizontalAlign="Left" HeaderText="Display Order"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="status" HeaderStyle-HorizontalAlign="Left" HeaderText="status"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                         <asp:TemplateField>
                                <HeaderTemplate>
                                   Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("AppMenuHelpID") %>'
                                        CommandName="Aphstatus" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("istatus"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("istatus"))) %>' />



                                </ItemTemplate>
                            </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                        <ItemStyle Wrap="False" />
                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="img1" runat="server" CausesValidation="false"
                                                CommandArgument='<%#Eval("AppMenuHelpID") %>' CommandName="cmdEdit"
                                                ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                              <%--#CC01 Added End--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="dvFooter" runat="server" class="pagination">
                                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportexcel" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <asp:PostBackTrigger ControlID="btnSearch" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

