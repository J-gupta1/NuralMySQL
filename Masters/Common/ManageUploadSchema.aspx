<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageUploadSchema.aspx.cs" Inherits="Masters_Common_ManageUploadSchema" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Specify Table
    </div>
    <div class="export">
        <asp:LinkButton ID="LBViewSalesChannel" runat="server" CausesValidation="False" OnClick="LBViewSchema_Click"
            CssClass="elink7">View List</asp:LinkButton>
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Table Name:<span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtTableName" runat="server" CssClass="formselect" MaxLength="40"></asp:TextBox>

                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtTableName"
                            CssClass="error" Display="Dynamic" ErrorMessage="Please Insert Table Name."
                            SetFocusOnError="true" ValidationGroup="Start"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Table Description:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtTableDescription" runat="server" CssClass="formselect" MaxLength="40"></asp:TextBox>

                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnProceed" runat="server" CausesValidation="true" CssClass="buttonbg"
                            Text="Proceed" ValidationGroup="Start" OnClick="btnProceed_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCreateColumn" runat="server" CssClass="buttonbg" Text="Create New Column" ToolTip="Create Column"
                            OnClick="btnCreateColumn_click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="Button2" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                            OnClick="btnunProced_click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlAddNewColumn" runat="server" Visible="false">
        <div class="mainheading">
            Select Table
        </div>
        <div class="contentbox">
            <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Table Name:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="cmbTableName" runat="server" CssClass="formselect"
                                ValidationGroup="Add">
                                <%-- <asp:ListItem Selected="True" Value="0" Text="Select"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="req" runat="server" CssClass="error" InitialValue="0"
                            ControlToValidate="cmbTableName" ErrorMessage="Please select sales channel type"
                            Display="Dynamic" ValidationGroup="AddColumn"></asp:RequiredFieldValidator>
                    </li>
                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnAddColumn" runat="server" CausesValidation="true" CssClass="buttonbg"
                                Text="Proceed" ValidationGroup="AddColumn" OnClick="btnCreateColumn_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancelColumnAdd" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                                OnClick="btncancelCreateColumn_click" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlUpdate" Visible="false">
        <div class="mainheading">
            Column Description
        </div>
        <div class="contentbox">
           <div class="H25-C3">
                <ul>
                    <li class="text">Table Column Name:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="txtColumnName" runat="server" CssClass="formselect" MaxLength="40"></asp:TextBox>

                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="req3" runat="server" ControlToValidate="txtColumnName"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select Table Column Name."
                                SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">
                        <asp:Label ID="Label2" runat="server" Text="" Visible="true">Table Column Data Type:<span class="error">*</span></asp:Label>

                    </li>
                    <li class="field">
                        <asp:DropDownList ID="ddlTableColDataType" runat="server" CssClass="formselect"
                            ValidationGroup="Add">
                            <%-- <asp:ListItem Selected="True" Value="0" Text="Select"></asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTableColDataType"
                            CssClass="error" Display="Dynamic" ErrorMessage="Please Select DataType." InitialValue="0"
                            SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                    </li>
                    <li class="text">
                        <asp:Label ID="Label3" runat="server" Text="" Visible="true">Excel Sheet Column Name:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtExcelSheetColumnName" runat="server" CssClass="formselect" MaxLength="40"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtExcelSheetColumnName"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please Insert Table Column Name."
                                SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="text">
                        <asp:Label ID="lblExcelSheetDataType" runat="server" Text="" Visible="true">Excel Sheet Data Type:<span class="error">*</span></asp:Label>

                    </li>
                    <li class="field">
                        <asp:DropDownList ID="DDlExcelSheetDataType" runat="server" CssClass="formselect"
                            ValidationGroup="Add">
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDlExcelSheetDataType"
                            CssClass="error" Display="Dynamic" ErrorMessage="Please Select DataType." InitialValue="0"
                            SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                    </li>
                    <li class="text">
                        <asp:Label ID="Label1" runat="server" Text="" Visible="true">Column Constraints:<span class="error">*</span></asp:Label>

                    </li>
                    <li class="field">
                        <asp:DropDownList ID="ddlColumnContraint" runat="server" CssClass="formselect" ValidationGroup="Add">
                            <asp:ListItem Selected="True" Value="0" Text="Select"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li class="text">Maximum Length:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtMaxLength" runat="server" CssClass="formselect" MaxLength="30"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="req5" runat="server" ControlToValidate="txtMaxLength"
                            CssClass="error" Display="Dynamic" ErrorMessage="Please enter Maximum length."
                            SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender ID="txtMaxLengthFilteredBox" runat="server"
                            TargetControlID="txtMaxLength" ValidChars="0123456789.">
                        </cc1:FilteredTextBoxExtender>
                    </li>
                </ul>
                <ul>
                    <li class="text">Validate:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:CheckBox ID="chkValidate" runat="server" Text="" />
                    </li>
                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnAdd" runat="server" CausesValidation="true" CssClass="buttonbg"
                                Text="Add" ValidationGroup="Add" OnClick="btnAdd_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel"
                                ToolTip="Cancel" OnClick="btnCancel_Click" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <asp:Panel ID="pnlgrid" runat="server" Visible="false">
            <div class="mainheading">
                List
            </div>
            <asp:UpdatePanel runat="server" ID="updGrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="grdUploadSchema" runat="server" FooterStyle-VerticalAlign="Top"
                                FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                                AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                                HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                                BorderWidth="0px" Width="100%" AutoGenerateColumns="true" AllowPaging="True"
                                SelectedStyle-CssClass="gridselected">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>          
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="margin-bottom">
                <div class="float-margin">
                    <asp:Button ID="btnSave" Text="Submit" runat="server"
                        CssClass="buttonbg" OnClick="btnSave_Click" />
                </div>
                <div class="float-margin">
                    <asp:Button ID="btnReset" Text="Cancel" runat="server"
                        CssClass="buttonbg" OnClick="btnReset_Click" />
                </div>
                <div class="clear"></div>
            </div>

        </asp:Panel>

    </asp:Panel>



    <%-- <td align="left" height="10">
                            ddd</td>--%>
</asp:Content>

