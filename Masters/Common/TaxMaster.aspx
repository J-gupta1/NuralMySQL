<%@ Page Title="" Language="C#"  MasterPageFile="~/CommonMasterPages/MasterPage.master"
    EnableEventValidation="false" AutoEventWireup="true" CodeFile="TaxMaster.aspx.cs"
    Inherits="Common_TaxMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCUserDetail.ascx" TagName="UCUserDetail" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ucStatusControl.ascx" TagName="ucStatus" TagPrefix="uc5" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
   <div class="box1">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <div class="subheading">
                    <div class="float-left">
                        Add Tax</div>
                <div class="clear">
                <uc4:ucMessage ID="ucMessage1" runat="server" />
                  </div>
          
                </div>
        
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
         
                <div class="innerarea"><%--#CC03: class changed--%>
                    <div class="H25-C3-S"><%--#CC03: class changed--%>
                        <%--#CC03: li class changed and style removed START--%>
                        <ul><%--<%=Resources.ApplicationKeyword.Country %>--%>
                            <li class="text">Country :<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="cmbCountry" runat="server" CssClass="formselect" />
                                <%--#CC03: <br /> commented--%>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cmbCountry"
                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please Enter Country Name."
                                    ValidationGroup="tax"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Tax Name:<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtTaxName" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox><%--#CC03: <br /> commented--%>
                                <asp:RequiredFieldValidator ID="reqTaxName" runat="server" ControlToValidate="txtTaxName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter tax name." ForeColor=""
                                    meta:resourcekey="ReqoldpassResource1" ValidationGroup="tax"></asp:RequiredFieldValidator></li>
                            <li class="text">Tax Type:<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="cmbTaxType" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <%--#CC03: <br /> commented--%>
                                <asp:RequiredFieldValidator ID="reqState" runat="server" ControlToValidate="cmbTaxType"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Select Tax Type." InitialValue="0"
                                    ValidationGroup="tax"></asp:RequiredFieldValidator>
                            </li>
                            <%--#Ch01: Tax Group added--%>
                            <li class="text">Tax Group:<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="cmbTaxGroup" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <%--#CC03: <br /> commented--%>
                                <asp:RequiredFieldValidator ID="reqTaxGroup" runat="server" ControlToValidate="cmbTaxGroup"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Select Tax Group." InitialValue="0"
                                    ValidationGroup="tax"></asp:RequiredFieldValidator>
                            </li>
                            <%--#Ch01: End-Tax Group added--%>
                            <li class="text">Display Order:</li>
                            <li class="field">
                                <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="formfields" MaxLength="3"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtDisplayOrder"
                                    ValidChars="0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </li>
                            <li class="text">Remarks:</li>
                            <li class="field">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                            </li>
                            <li class="text">
                                <div class="float-margin">
                                    Active:</div>
                                <div class="float">
                                    <asp:CheckBox ID="chkActive" runat="server" CssClass="CheckBoxList2" Checked="True" /></div>
                            </li>
                            <li class="field">
                                <div class="float-margin"><%--#CC03:style replaced with class--%>
                                    <cc2:ZedButton ID="Save" runat="server" CssClass="button1" OnClick="Save_Click" Text="Save"
                                        ValidationGroup="tax" ActionTag="Add" /></div>
                               <div class="float-left"><%--#CC03:style replaced with class--%>
                                    <asp:Button ID="btnCancel" runat="server" CssClass="button1" OnClick="btnCancel_Click"
                                        Text="Cancel" />
                                </div>
                            </li>
                        </ul>
                        <%--#CC03: li class changed and style removed  END--%>
                    </div>
                </div>
           
        </ContentTemplate>
    </asp:UpdatePanel>
     </div>
    <asp:UpdatePanel ID="updSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="box1">
                <div class="subheading">
                    Search Tax Information<br />
                </div>
                <div class="innerarea"><%--#CC03: class changed--%>
                    <div class="H20-C3-S"><%--#CC03: class changed--%>
                        <ul>
                            <li class="text">Country :<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="cmbSerCountry" runat="server" CssClass="formselect" />
                                <cc1:CascadingDropDown ID="csdCountry" runat="server" TargetControlID="cmbSerCountry"
                                    PromptText="Select" PromptValue="-1" EmptyValue="0" LoadingText="[Loading states...]"
                                    ServiceMethod="GetAllCountries" ServicePath="~/Services/Local/AutoCompleteService.asmx"
                                    Category="Country" />
                            </li>
                            <li class="text">Tax Name:<span class="optional-img">&nbsp;</span></li>
                          <li class="field">
                                <asp:TextBox ID="txtSerTax" runat="server" CssClass="formfields"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="error"
                                    Display="Dynamic" ControlToValidate="txtSerTax" ErrorMessage="Invalid char(s)!"
                                    ValidationExpression="[^<>@%]{1,50}$" SetFocusOnError="true"></asp:RegularExpressionValidator>
                            </li>
                            <li><%--#CC03: removed style="width: 250px;"--%>
                                <uc5:ucStatus ID="ucStatusSer" runat="server" />
                            </li>
                            <li class="text"></li><%--#CC03: li added--%>
                            <li class="field"><%--#CC03: class added--%>
                                <cc2:ZedButton ActionTag="View" ID="btnSearch" runat="server" Text="Search" CssClass="button1"
                                    OnClick="btnSearch_Click" />
                            <%--</li>
                            <li>--%><%--#CC03: li commented--%>
                                <cc2:ZedButton ActionTag="View" ID="btnShowAll" runat="server" Text="View All" CssClass="button1"
                                    OnClick="btnShowAll_Click" />
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="box1" runat="server" id="divgrd">
                <div class="subheading">
                    <div class="float-left">
                        Tax List</div>
                    <div class="export">
                        <asp:ImageButton ID="Exporttoexcel" runat="server"
                            OnClick="Exporttoexcel_Click" CausesValidation="False" AlternateText="Export to Excel" /></div>
                </div>
                <div class="innerarea"><%--#CC03: class changed--%>
                    <div class="grid1">
                        <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" DataKeyNames="taxmasterid"
                            EmptyDataText="No Record Found" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow" GridLines="None"
                            OnRowCommand="grdvList_RowCommand" Width="100%" OnRowDataBound="grdvList_RowDataBound">
                            <Columns>
                                <asp:ButtonField CausesValidation="false" CommandName="Select" HeaderStyle-HorizontalAlign="Left"
                                    Text="Select" Visible="false" />
                                <asp:BoundField DataField="TaxName" HeaderStyle-HorizontalAlign="Left" HeaderText="Tax Name" />
                                <asp:BoundField DataField="CountryName" HeaderStyle-HorizontalAlign="Left"  />
                                <asp:BoundField DataField="TaxGroupName" HeaderStyle-HorizontalAlign="Left" HeaderText="Tax Type" />
                                <asp:BoundField DataField="TaxTypeName" HeaderStyle-HorizontalAlign="Left" HeaderText="Tax Group" />
                                <asp:BoundField DataField="DisplayOrder" HeaderText="Display Order" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ShowHeader="False" HeaderText="Action" HeaderStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="110px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblactive" Visible="false" runat="server" Text='<%# Eval("Active") %>'
                                            CommandName="activeTax" CommandArgument='<%# Eval("TaxMasterID") %>'></asp:Label>
                                        <cc2:ZedImageButton ActionTag="Edit" ID="Active" runat="server" Text='<%# Eval("Active") %>'
                                            CommandName="activeTax" CommandArgument='<%# Eval("TaxMasterID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </div><%--#CC03: </div> tag added--%>
                        <div id="dvFooter" runat="server" class="pagination">
                            <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                        </div>
                    <%--#CC03: </div> commented--%>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="Exporttoexcel" />
            
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
