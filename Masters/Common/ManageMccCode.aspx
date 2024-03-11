<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageMccCode.aspx.cs" Inherits="Masters_Common_ManageMccCode" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddMCC" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Add MCC Code
                </div>
                <div class="contentbox">
                    <asp:HiddenField ID="hdnMCCCodeid" runat="server" Value="0" />
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">MCC Code: <span class="error">* </span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:TextBox ID="txtMCCCode" runat="server" CssClass="formfields" MaxLength="50"
                                        ValidationGroup="AddMCCCode"></asp:TextBox>
                                </div>
                                <div class="clear">
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqMCCCode" runat="server" CssClass="error" ControlToValidate="txtMCCCode"
                                        ErrorMessage="Please insert MCC Code." SetFocusOnError="True" Display="Dynamic"
                                        ValidationGroup="AddMCCCode"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtMCCCode"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[0-9a-zA-Z' ']{1,25}"
                                        ValidationGroup="AddMCCCode"></asp:RegularExpressionValidator>
                                </div>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblOperatorName" runat="server">Operator Name :
                                </asp:Label><span class="error">* </span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:TextBox ID="txtOperatorName" runat="server" CssClass="formfields" MaxLength="100"
                                        ValidationGroup="AddMCCCode"></asp:TextBox>
                                </div>
                                <asp:Label Style="display: none;" runat="server" ID="lblddlCheck" CssClass="error"></asp:Label>
                                <asp:RequiredFieldValidator ID="reqSalesMan" runat="server" ControlToValidate="txtOperatorName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Operator Name."
                                    SetFocusOnError="true" ValidationGroup="AddMCCCode"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtOperatorName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[0-9a-zA-Z' ']{1,50}"
                                    ValidationGroup="AddMCCCode"></asp:RegularExpressionValidator>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblCircleName" runat="server" CssClass="formtext">Circle Name:</asp:Label>
                                <span class="error">* </span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:TextBox ID="txtCircleName" runat="server" CssClass="formfields" MaxLength="100"
                                        ValidationGroup="AddMCCCode"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="reqCircleName" runat="server" ControlToValidate="txtCircleName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Circle Name." SetFocusOnError="true"
                                    ValidationGroup="AddMCCCode"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCircleName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[0-9a-zA-Z' ']{1,50}"
                                    ValidationGroup="AddMCCCode"></asp:RegularExpressionValidator>
                            </li>
                        </ul>
                        <ul>
                            <li class="text">&nbsp;
                            </li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSubmit" Text="Submit" runat="server" CausesValidation="true" ValidationGroup="AddMCCCode"
                                        OnClick="btnSubmit_Click" ToolTip="Submit" CssClass="buttonbg" />
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                        OnClick="btnCancel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit"/>
                <asp:PostBackTrigger ControlID="btnCancel"/>
            </Triggers>
        </asp:UpdatePanel>
        <div class="mainheading">
            Search
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">MCC Code :
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtMCCCodeSearch" runat="server" MaxLength="50" CssClass="formfields">
                                </asp:TextBox>
                            </li>
                            <li class="text">Operator Name :
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtOperatorNameSearch" runat="server" CssClass="formfields" MaxLength="100">
                                </asp:TextBox>
                            </li>
                            <li class="text">Circle Name :
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtCircleNameSearch" runat="server" CssClass="formfields" MaxLength="100">
                                </asp:TextBox>
                            </li>
                            <li class="text">&nbsp;
                            </li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearchMCC" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearchMCC_Click" CausesValidation="False"></asp:Button>
                                </div>
                                <%--<div class="float-left">
                                                                       <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                                                            OnClick="btnShow_Click" />
                                                                            </div>--%>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSearchMCC" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            MCC Code List
        </div>
        <div class="export">
            <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
                OnClick="btnExprtToExcel_Click" />
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvMCCCode" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                            RowStyle-HorizontalAlign="left" EmptyDataText="No Record Found" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                            PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                            DataKeyNames="MCCMasterId" OnPageIndexChanging="gvSalesMan_PageIndexChanging">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MCCMNCCode"
                                    HeaderText="MCC Code"></asp:BoundField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Operator Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                            <%# DataBinder.Eval(Container.DataItem, "OperatorName")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="CircleName"
                                    HeaderText="Circle Name" NullDisplayText="N/A"></asp:BoundField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("MCCMasterId") %>' runat="server" ID="btnEdit"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' OnClick="btnEdit_Click"
                                            ToolTip="Edit MCC Code" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="gvMCCCode"/>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>
