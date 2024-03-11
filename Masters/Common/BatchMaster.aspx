<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BatchMaster.aspx.cs" Inherits="Masters_Common_BatchMaster" %>


<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMessage1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit Batch Master
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblColornm" runat="server" AssociatedControlID="txtBatchName"
                                    CssClass="formtext">Batch Name:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtBatchName" runat="server" CssClass="formfields" MaxLength="70"
                                    ValidationGroup="AddU"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqColorName" runat="server" ControlToValidate="txtBatchName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Batch Name."
                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtBatchCode" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}" ValidationGroup="Add"
                                    runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtBatchCode"
                                    CssClass="formtext">Batch Code:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtBatchCode" runat="server" CssClass="formfields" MaxLength="20"
                                    ValidationGroup="Add"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBatchCode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Batch Code."
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtBatchCode" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}" ValidationGroup="Add"
                                    runat="server" />
                            </li>
                        </ul>
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblpreffdt" runat="server" Text="">Batch Start Date:<span class="error">*</span></asp:Label></li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucStartDate" runat="server" RangeErrorMessage="Invalid date." ErrorMessage="Please select a date"
                                    ValidationGroup="insert" />
                            </li>
                            <li class="text">
                                <asp:Label ID="Label2" runat="server" Text="">Batch End Date:<span class="error">*</span></asp:Label></li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucEndDate" runat="server" RangeErrorMessage="Invalid date." ErrorMessage="Please select a date"
                                    ValidationGroup="insert" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status :</asp:Label>
                            </li>
                            <li class="field">
                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                            </li>
                        </ul>
                        <ul>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="True" ValidationGroup="Add"
                                        ToolTip="Add " CssClass="buttonbg" OnClick="btnCreate_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"
                                        CssClass="buttonbg" OnClick="btncancel_click" CausesValidation="False" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
        </asp:UpdatePanel>
        <div class="mainheading">
            Search Batch
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H20-C3-S">
                        <ul>
                            <li class="text">Batch Name: <span class="error">&nbsp;</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSerName" runat="server" MaxLength="70" CssClass="formfields"> </asp:TextBox>
                            </li>
                            <li class="text">Batch Code: <span class="error">&nbsp;</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSerCode" runat="server" MaxLength="100" CssClass="formfields"> </asp:TextBox>
                            </li>

                        </ul>
                        <ul>
                            <li class="text">
                                <asp:Label ID="Label3" runat="server" Text="">Batch Start Date:<span class="error">*</span></asp:Label></li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucSerFromdate" runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="Label4" runat="server" Text="">Batch End Date:<span class="error">*</span></asp:Label></li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucSerDateTo" runat="server" />
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnserch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click" CausesValidation="False"></asp:Button>
                                </div>
                                <%--</td>
                                                                
                                                                 <td align="left" valign="top" width="25%" height="25" class="formtext">--%>

                                <div class="float-margin">
                                    <asp:Button ID="getalldata" Text="Show All Data" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btngetalldta_Click" CausesValidation="False"></asp:Button>
                                </div>
                            </li>
                            <%-- 
                                                                <td align="left" valign="top" width="30%">&nbsp;
                                                                    
                                                                </td>--%>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            List
        </div>
        <div class="export">
            <asp:Button ID="Exporttoexcel" runat="server" CausesValidation="False"
                CssClass="excel" OnClick="Exporttoexcel_Click" Text=""
                ToolTip="Export to Excel" />
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grdBatch" runat="server" FooterStyle-VerticalAlign="Top"
                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                            SelectedStyle-CssClass="gridselected" DataKeyNames="BatchID" PageSize='<%$ AppSettings:GridViewPageSize %>'
                            OnRowCommand="grdBatch_RowCommand" EmptyDataText="No record found"
                            OnPageIndexChanging="grdBatch_PageIndexChanging">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>

                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchName"
                                    HeaderText="Batch Name"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchCode"
                                    HeaderText="Batch Code"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchStartdate"
                                    HeaderText="Batch Start Date"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchEndDate"
                                    HeaderText="Batch End Date"></asp:BoundField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnActiveDeactive" runat="server" CommandArgument='<%#Eval("BatchID") %>'
                                            CommandName="Active" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("BatchID") %>' runat="server" ID="btnEdit" CommandName="cmdEdit"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                            ToolTip="Edit User" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <%-- <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</asp:Content>

