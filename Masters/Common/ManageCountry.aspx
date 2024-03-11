<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageCountry.aspx.cs" Inherits="Masters_Common_ManageCountry" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>

<%@ Import Namespace="BussinessLogic" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add / Edit Country
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblCountryname" runat="server" Text="">Country Name: <span class="error">*</span></asp:Label></li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertName" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtInsertName" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert Country Name " ValidationGroup="insert" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtInsertName"
                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                            </cc1:FilteredTextBoxExtender>
                        </li>
                        <%-- <td class="formtext" valign="top" align="right" width="10%"><asp:Label ID="lblcstatecode" runat="server" Text="">Country Code:<span class="error">*</span></asp:Label></td>
                                                                        <td width="20%" align="left" valign="top"><asp:TextBox ID="txtInsertCode" runat="server" CssClass="form_input2"  MaxLength = "20" ></asp:TextBox><br />
                                                                            <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtInsertCode"
                                                                                CssClass="error" ErrorMessage="Please insert  code " ValidationGroup="insert" /><br />
                                                                               <cc1:FilteredTextBoxExtender ID = "txtnameValid"  runat = "server"   TargetControlID = "txtInsertName"
                                                                               InvalidChars = "<>!@#$%^&*(){}"  FilterType = "Custom"      FilterMode = "InvalidChars"                                                                                   
                                                                                  ></cc1:FilteredTextBoxExtender>   
                                                                                    </td>--%>
                        <%-- <td class="formtext" valign="top" align="right" width="12%"> <asp:Label ID="lblprlist" Text="" runat="server" />Price List Name:<span class="error">*</font>
                                                                        </td>--%>
                        <%--<td valign="top" align="left" width="23%">
                                                                        <div style="float:left; width:135px;">
                                                                        <asp:DropDownList ID="cmbInsertPriceList" 
                                                                                runat="server" CssClass="form_select"
                                                                                AutoPostBack="True">
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                            <asp:RequiredFieldValidator runat="server" ID="valpricename" ControlToValidate="cmbInsertPriceList"
                                                                                CssClass="error" ErrorMessage="Please select a Price Name " InitialValue="0"
                                                                                ValidationGroup="insert" /></div>
                                                                        </td>--%>

                        <%--<td class="formtext" valign="top" align="right"> <asp:Label ID="lblpreffdt" runat="server" Text="">Price Effective Date:<font class="error">*</font></asp:Label></td>
                                                                        <td valign="top" align="left">
                                                                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" RangeErrorMessage="Invalid date." ErrorMessage="Please select a date"
                                                                                       ValidationGroup="insert"   />
                                                                        </td>--%>
                        <li class="text">
                            <div class="float-margin">Status: </div>
                            <div class="float-left">
                                <asp:CheckBox ID="chkstatus" runat="server" Checked="true" />
                            </div>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnsubmit" runat="server" OnClick="btninsert_click" Text="Submit"
                                    ValidationGroup="insert" CssClass="buttonbg" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"
                                    Text="Cancel" CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnsubmit" />
                <asp:PostBackTrigger ControlID="btncancel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        Search Country
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <%--<td align="right" valign="top" width="13%" height="35" class="formtext">
                                                                <asp:Label ID="lblfndstatecode" runat="server" Text="Country Code:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="22%">
                                                                <asp:TextBox ID="txtSerCode" runat="server" CssClass="form_input2"></asp:TextBox><br />
                                                                  <cc1:FilteredTextBoxExtender ID = "FilteredTextBoxExtender2"  runat = "server"   TargetControlID = "txtSerCode"
                                                                               InvalidChars = "<>!@#$%^&*(){}"  FilterType = "Custom"      FilterMode = "InvalidChars"                                                                                   
                                                                                  ></cc1:FilteredTextBoxExtender>  
                                                            </td>--%>
                        <li class="text">
                            <asp:Label ID="lblstatefnname" runat="server" Text="Country Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerName" runat="server" CssClass="formfields"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtSerName"
                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                            </cc1:FilteredTextBoxExtender>
                        </li>
                        <%-- <td align="right" valign="top" width="12%" height="35" class="formtext">
                                                                <asp:Label ID="lblserchprice" runat="server" Text="Price List Name:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="23%" class="formtext">
                                                               <div style="float:left; width:135px;"> <asp:DropDownList ID="cmbSerPriceList" runat="server" CssClass="form_select">
                                                                </asp:DropDownList></div>
                                                            </td>--%>

                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                    CssClass="buttonbg" CausesValidation="False" ValidationGroup="search" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnGetallData" runat="server" Text="Show All Data"
                                    OnClick="btnGetallData_Click" CssClass="buttonbg" CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSearch" />
                <asp:PostBackTrigger ControlID="btnGetallData" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExprtToExcel_Click" CausesValidation="False" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdCountry" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                        AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                        DataKeyNames="CountryID" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                        OnRowCommand="grdCountry_RowCommand" RowStyle-CssClass="gridrow" EmptyDataText="No record found"
                        RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" Width="100%"
                        OnPageIndexChanging="grdCountry_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>

                            <asp:BoundField DataField="CountryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Country Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>


                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("CountryID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("CountryID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdCountry" />

                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

