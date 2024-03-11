<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageSalesChannelProductCategory.aspx.cs" Inherits="Masters_SalesChannel_ManageSalesChannelProductCategory" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script type="text/javascript">
        var TotalChkBx;
        var Counter;
        window.onload = function() {
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.grdSalesChannelList.Rows.Count %>');
            //Get total no. of checked CheckBoxes in side the GridView.
            Counter = 0;
        }

        function HeaderClick(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl = document.getElementById('<%= this.grdSalesChannelList.ClientID %>');
            var TargetChildControl = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            //Reset Counter
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }

        function ChildClick(CheckBox, HCheckBox) {
            //get target base & child control.
            var HeaderCheckBox = document.getElementById(HCheckBox);

            //Modifiy Counter;            
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;

            //Change state of the header CheckBox.
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div>
                <uc1:ucMessage ID="ucMsg" runat="server" />
                <div class="mainheading">
                    Search Sales Channel
                </div>

                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">Product Category Name: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList CausesValidation="true" ID="ddlProductCategory" runat="server"
                                        CssClass="formselect">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="ReqUserGroup" runat="server" ControlToValidate="ddlProductCategory"
                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select Product Category."
                                        SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">Sales Channel Type: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlSalesChannelType" runat="server" CausesValidation="true"
                                        CssClass="formselect">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="ReqUserGroup0" runat="server" ControlToValidate="ddlSalesChannelType"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select Sales Channel type."
                                        InitialValue="0" SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">State:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlState" runat="server" CausesValidation="true" CssClass="formselect"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                </asp:DropDownList>
                            </li>
                        </ul>
                        <ul>
                            <li class="text">City: <span class="error">&nbsp;</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlCity" runat="server" CausesValidation="true" CssClass="formselect">
                                    <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li class="text">Sales Channel Name: <span class="error">&nbsp;</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSalesChannelName" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                            </li>
                            <li class="text">Sales Channel Code:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSalesChannelCode" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                            </li>
                        </ul>
                        <ul>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSubmit" runat="server" CausesValidation="true" CssClass="buttonbg"
                                        OnClick="btnSubmit_Click" Text="Add/Edit" ToolTip="Add/Edit" ValidationGroup="AddUserValidationGroup" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                                        OnClick="btnCancel_Click1" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>

                <asp:Panel ID="pnlHide" runat="server" Visible="false">


                    <div class="mainheading">
                        Sales Channel List
                    </div>

                    <%--<asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False" />--%>


                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="grdSalesChannelList" runat="server" FooterStyle-VerticalAlign="Top"
                                FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" EmptyDataText="No Record found"
                                RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                GridLines="none" AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow"
                                FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" CellSpacing="1"
                                CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                                AllowPaging="True" PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                                OnPageIndexChanging="grdSalesChannelList_PageIndexChanging" DataKeyNames="SalesChannelID"
                                OnRowDataBound="grdSalesChannelList_RowDataBound" OnRowCommand="grdSalesChannelList_RowCommand"
                                OnRowCreated="grdSalesChannelList_RowCreated">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductCategoryId" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"ProductCategoryID"))%>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblSalesChannelID" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"SalesChannelID"))%>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Status"))%>'
                                                Visible="false"></asp:Label>
                                            <asp:CheckBox ID="chkBxSelect" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkBxHeader" OnCheckedChanged="chkBxHeader_CheckedChanged" AutoPostBack="true"
                                                runat="server" />
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SalesChannelTypename" HeaderStyle-HorizontalAlign="Left"
                                        HeaderText="Sales Channel Type" HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Code"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ParentName" HeaderStyle-HorizontalAlign="Left" HeaderText="Parent Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LocationName" HeaderStyle-HorizontalAlign="Left" HeaderText="Repo. Hierarchy Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Email" HeaderStyle-HorizontalAlign="Left" HeaderText="Email"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MobileNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%--<asp:TemplateField HeaderText="View Details">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                </Columns>
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                        <div class="margin-top">
                            <div class="float-margin">
                                <asp:Button ID="btnSave" Text="Save" runat="server" ToolTip="Save" CssClass="buttonbg"
                                    OnClick="btnSave_Click" ValidationGroup="Save"></asp:Button>
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnReset" Text="Cancel" runat="server" ToolTip="Cancel" CssClass="buttonbg"
                                    OnClick="btnReset_Click"></asp:Button>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <%--<asp:PostBackTrigger ControlID="Button1" />--%>
            <%--<asp:PostBackTrigger ControlID="ExportToExcel" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
