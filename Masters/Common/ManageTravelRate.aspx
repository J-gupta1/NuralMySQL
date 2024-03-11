<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageTravelRate.aspx.cs" Inherits="Admin_ManageTravelRate" %>

<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UCCurrency.ascx" TagName="UCCurrency" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="box1"><%--#CC02: class changed--%>
        <asp:UpdatePanel ID="updPnl2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="subheading">
                    <uc4:ucMessage ID="ucMessage1" runat="server" />
                    Add Travel Rate
                </div>
                <div class="innerarea"><%--#CC02: class changed--%>
                    <div class="H20-C3-S"><%--#CC02: class changed--%>
                        <ul>
                            <li class="text" style="display: none">Product Category:</li>
                            <li class="field" style="display: none">
                                <div>
                                    <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="formselect" AutoPostBack="False">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqProductcat" runat="server" ControlToValidate="ddlProductCategory"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Select Product Category." InitialValue="0"
                                        ValidationGroup="grptravelrate"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">Role :<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlrole" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Vehicle Type:<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlVehicalType" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlVehicalType"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Select Vehical Type." ValidationGroup="grptravelrate"
                                        InitialValue="Select"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">Amount:<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <div>
                                    <asp:TextBox ID="txtamount" runat="server" CssClass="formfields" MaxLength="8"></asp:TextBox>
                                </div>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                    FilterMode="ValidChars" ValidChars="0123456789." TargetControlID="txtamount">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtamount"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Enter Amount." ValidationGroup="grptravelrate"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtamount"
                                    ErrorMessage="Enter a valid amount like(1.00)" ValidationExpression="^\d{0,4}(\.\d{1,2})?$"
                                    CssClass="error" Display="Dynamic" ValidationGroup="grptravelrate" />
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtamount"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Amount should be 0.1 or less than 9999 rupees"
                                    MaximumValue="9999" MinimumValue="0.1" Type="Double" ValidationGroup="grptravelrate"></asp:RangeValidator>
                            </li>
                            <li class="text">Currency:<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <uc7:UCCurrency ID="UCCurrency1" runat="server" IsRequired="true" />
                            </li>
                            <li class="text" style="display: none"></li>
                            <li class="field" style="display: none">
                                <div>
                                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="formselect" AutoPostBack="False">
                                    </asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlProductCategory"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Product Category." InitialValue="0"
                                    ValidationGroup="grptravelrate"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Valid From:<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <uc1:ucDatePicker ID="ucRenualDate" runat="server" IsRequired="True" ErrorMessage="Please Enter date."
                                    RangeErrorMessage="Invalid date" ValidationGroup="grptravelrate" />
                            </li>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                  <asp:Button     ID="Save" runat="server" CssClass="buttonbg" Text="Save"
                                        ValidationGroup="grptravelrate" OnClick="Save_Click" />
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="btnCancel_Click" /></div>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdSrch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="box1"><%--#CC02: class changed--%>
                <div class="subheading">
                    Travel Rate Search
                </div>
                <div class="innerarea"><%--#CC02: class changed--%>
                    <div class="H25-C3-S"><%--#CC02: class changed--%>
                        <ul>
                            <li class="text">As On Date:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <uc1:ucDatePicker ID="UcDatePickerSearch" runat="server" IsRequired="false" ErrorMessage="Please Enter date."
                                    RangeErrorMessage="Invalid date" />
                            </li>
                            <li class="text">Role:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlrolesearch" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Vehicle Type:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlvehicletypesearch" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                   <asp:Button ID="btnSearch" runat="server" CssClass="buttonbg"
                                        OnClick="btnSearch_Click" Text="Search" />
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnSearchCancel" runat="server" Text="View All"
                                        CssClass="buttonbg" OnClick="btnSearchCancel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <asp:UpdatePanel ID="udtPnlGrd" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="box1" runat="server" id="divGrd"><%--#CC02: class changed--%>
                    <div class="subheading">
                        <div class="float-left">
                            Travel Rate List
                        </div>
                         </div>
                        <div class="export">
                            <asp:Button ID="Exporttoexcel" runat="server"   CssClass="excel"
                                Visible="false" OnClick="Exporttoexcel_Click" CausesValidation="False" AlternateText="Export to Excel" />
                        </div>
                   
                    <div class="innerarea"><%--#CC02: class changed--%>
                        <div class="grid1">
                            <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" DataKeyNames="TravelRateID"
                                EmptyDataText="No Record Found" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow" GridLines="None"
                                OnRowCommand="grdvList_RowCommand" Width="100%" OnRowDataBound="grdvList_RowDataBound">
                                <RowStyle CssClass="gridrow" />
                                <Columns>
                                    <asp:ButtonField CausesValidation="false" CommandName="Select" HeaderStyle-HorizontalAlign="Left"
                                        Text="Select" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="RoleId" HeaderStyle-HorizontalAlign="Left" HeaderText="Role Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TransportName" HeaderStyle-HorizontalAlign="Left" HeaderText="Transport Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TravelRateAmount" HeaderStyle-HorizontalAlign="Left" HeaderText="Travel Rate Amount">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CurrencyName" HeaderStyle-HorizontalAlign="Left" HeaderText="Currency Type">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ValidFrom" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid From"
                                        DataFormatString="{0:dd MMM yyyy}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ValidTill" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid Till"
                                        DataFormatString="{0:dd MMM yyyy}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Action" ShowHeader="False"
                                        HeaderStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblvalidfrom" Visible="false" runat="server" Text='<%# Eval("validfrom") %>'
                                                CommandName="activeTax"></asp:Label>
                                            <asp:ImageButton  ID="Active" runat="server" CommandName="activeUsed"
                                                CommandArgument='<%# Eval("TravelRateID") %>' Visible="false" />
                                           <asp:ImageButton  ID="imgbtnEdit" runat="server" CommandArgument='<%# Eval("TravelRateID") %>'
                                                CommandName="editUsed" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' ToolTip="Edit" title="Edit" />
                                           <asp:ImageButton  ID="imgbtnDelete" runat="server" Visible='<%# ShowDelete(Eval("ValidFrom")) %>'
                                                CommandArgument='<%# Eval("TravelRateID") %>' CommandName="deleteUsed" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gridheader" />
                                <EditRowStyle CssClass="editrow" />
                                <AlternatingRowStyle CssClass="Altrow" />
                            </asp:GridView>
                        </div><%--#CC02: </div> added--%>
                            <div id="dvFooter" runat="server" class="pagination">
                                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                            </div>
                        <%--#CC02: </div> commented--%>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="Exporttoexcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
