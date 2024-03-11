<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DSR_Upload_interface.aspx.cs" Inherits="Transactions_DSR_DSR_Upload_interface" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

    </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
        <div class="mainheading">
            DSR Upload interface                        
        </div>
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory
            </div>
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Hierarchy Level: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlHierarchyLevel" runat="server" CssClass="formselect"
                                OnSelectedIndexChanged="ddlHierarchyLevel_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="error" Display="Dynamic"
                                ControlToValidate="ddlHierarchyLevel" ErrorMessage="Please Select a Hierarchy Level" InitialValue="0"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">Hierarchy Name: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlParentHierarchy" runat="server" CssClass="formselect"
                                CausesValidation="True">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                                ControlToValidate="ddlParentHierarchy" ErrorMessage="Please Select Hierarchy Name" CssClass="error" InitialValue="0"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">DSR Month: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddl_month" runat="server" CssClass="formselect" CausesValidation="True">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                                ControlToValidate="ddl_month" ErrorMessage="Please Select DSR Month" CssClass="error" InitialValue="0"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">DSR Year: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="Txtbox_Year" runat="server" Width="100px"
                                CssClass="formfields" CausesValidation="True" MaxLength="4"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                                ControlToValidate="Txtbox_Year" ErrorMessage="Please Enter DSR Year" CssClass="error" SetFocusOnError="True"></asp:RequiredFieldValidator>


                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                                ControlToValidate="Txtbox_Year" ErrorMessage="It's not a year" CssClass="error"
                                ValidationExpression="^[0-9]{4}$" SetFocusOnError="True"></asp:RegularExpressionValidator>


                            <asp:RangeValidator ID="RangeValidator1" runat="server" Display="Dynamic"
                                ControlToValidate="Txtbox_Year" ErrorMessage="Not In Date Range" CssClass="error"
                                MaximumValue="2050" MinimumValue="2000" SetFocusOnError="True"
                                Type="Integer"></asp:RangeValidator>
                        </div>
                    </li>
                    <li class="text">Choose File:
                    </li>
                    <li class="field">
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="fileuploads" />
                    </li>
                    <li class="field3">
                        <asp:Button ID="btn_upload" runat="server" Text="Submit" OnClick="btn_upload_Click" CausesValidation="true" CssClass="buttonbg" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
