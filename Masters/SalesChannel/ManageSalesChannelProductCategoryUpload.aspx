<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ManageSalesChannelProductCategoryUpload.aspx.cs" Inherits="Masters_SalesChannel_ManageSalesChannelProductCategoryUpload" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

     
    <%--<asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>--%>
            <%--<div align="center">--%>
                <table cellspacing="0" cellpadding="0" width="965" border="0">
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="965" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                     <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <uc1:ucMessage ID="ucMsg" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                   
                                                                                

                                <tr>
                                                <td align="left" valign="top">
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" class="tableposition">
                                                                            <div class="mainheading">
                                                                                Upload Sales Channel Product Category Mapping</div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" class="tableposition">
                                                                <div class="contentbox">
                                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                        <tr>
                                                                            <td colspan="6" height="20" class="mandatory" valign="top">
                                                                                (*) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            
                                                                            <td valign="top" align="right" width="15%">
                                                                                Sales Channel Type:<font class="error">*</font>
                                                                            </td>
                                                                            <td valign="top" align="left" width="19%">
                                                                                <div style="width: 135px;">
                                                                                    <asp:DropDownList ID="ddlRefSaleschanneltype" runat="server" CausesValidation="true"
                                                                                        CssClass="form_select">
                                                                                    </asp:DropDownList>
                                                                                  
                                                                                </div>
                                                                                <div style="width: 160px;">
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlRefSaleschanneltype"
                                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select Sales Channel type."
                                                                                        InitialValue="0" SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator></div>
                                                                            </td>
                                                                            <td align="right" valign="top" width="15%">
                                                                                State:
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:DropDownList ID="ddlRefState" runat="server" CausesValidation="true" CssClass="form_select"
                                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlRefState_SelectedIndexChanged" >
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="right" valign="top" height="35">
                                                                                City: <span class="error">&nbsp;</span>
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:DropDownList ID="ddlRefCity" runat="server" CausesValidation="true" CssClass="form_select">
                                                                                    <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                       
                                                                        <tr>
                                                                            <td align="left" valign="top" height="35">
                                                                                <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                            &nbsp;&nbsp; &nbsp;
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                           
                                            
                                             <a class="elink2" href="../../Excel/Templates/SaleschannelProductCategoryMapping.xlsx">Download Template </a>
                                          
                                            &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
                                             <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>
                                       
                                                                            </td>
                                                                             <td height="35" align="right" width="10%" class="formtext" valign="top">
                                            Upload File:<font class="error">*</font>
                                        </td>
                                        <td width="30%" align="left" class="formtext" valign="top">
                                            <asp:FileUpload ID="FileSalesChannelProductMappingUpload" CssClass="form_file" runat="server" />
                                            <asp:Label ID="Label1" runat="server" CssClass="error" Text=""></asp:Label>
                                        </td>
                                        <td align="left" width="10%" class="formtext" valign="top">
                                            <asp:Button ID="Button1" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                OnClick="btnUpload_Click" ValidationGroup="Upload" />
                                        </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" class="tableposition">
                                                                            <div class="mainheading">
                                                                                Search Sales Channel</div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" class="tableposition">
                                                                <div class="contentbox">
                                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                        <tr>
                                                                            <td colspan="6" height="20" class="mandatory" valign="top">
                                                                                (*) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" align="right" width="17%" height="35">
                                                                                Product Category Name:<font class="error">*</font>
                                                                            </td>
                                                                            <td width="19%" align="left" valign="top">
                                                                                <div style="width: 160px;">
                                                                                    <asp:DropDownList CausesValidation="true" ID="ddlProductCategory" runat="server"
                                                                                        CssClass="form_select">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                                <div style="width: 160px;">
                                                                                    <asp:RequiredFieldValidator ID="ReqUserGroup" runat="server" ControlToValidate="ddlProductCategory"
                                                                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select Product Category."
                                                                                        SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator></div>
                                                                            </td>
                                                                            <td valign="top" align="right" width="15%">
                                                                                Sales Channel Type:<font class="error">*</font>
                                                                            </td>
                                                                            <td valign="top" align="left" width="19%">
                                                                                <div style="width: 135px;">
                                                                                    <asp:DropDownList ID="ddlSalesChannelType" runat="server" CausesValidation="true"
                                                                                        CssClass="form_select">
                                                                                    </asp:DropDownList>
                                                                                  
                                                                                </div>
                                                                                <div style="width: 160px;">
                                                                                    <asp:RequiredFieldValidator ID="ReqUserGroup0" runat="server" ControlToValidate="ddlSalesChannelType"
                                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select Sales Channel type."
                                                                                        InitialValue="0" SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator></div>
                                                                            </td>
                                                                            <td align="right" valign="top" width="15%">
                                                                                State:
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:DropDownList ID="ddlState" runat="server" CausesValidation="true" CssClass="form_select"
                                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" valign="top" height="35">
                                                                                City: <span class="error">&nbsp;</span>
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:DropDownList ID="ddlCity" runat="server" CausesValidation="true" CssClass="form_select">
                                                                                    <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <%--<td align="right" valign="top">
                                                                                Sales Channel Name: <span class="error">&nbsp;</span>
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:TextBox ID="txtSalesChannelName" runat="server" CssClass="form_input2" MaxLength="100"></asp:TextBox>
                                                                            </td>
                                                                            <td align="right" valign="top">
                                                                                Sales Channel Code:
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:TextBox ID="txtSalesChannelCode" runat="server" CssClass="form_input2" MaxLength="100"></asp:TextBox>
                                                                            </td>--%>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" valign="top" height="35">
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                                                                                     Text="Search" ToolTip="Search" OnClick="btnSearch_Click"  />
                                                                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                                                                                    OnClick="btnCancel_Click1" />
                                                                            </td>
                                                                             <td width="10%" align="right">
                                            <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False" OnClick="btnExprtToExcel_Click" Visible="false"
                                                />
                                        </td>
                                                                            <td align="left" valign="top" colspan="3">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" height="10">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" height="10">
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlHide" runat="server" Visible="false">
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="90%" align="left" class="tableposition">
                                                        <div class="mainheading">
                                                            Sales Channel List</div>
                                                    </td>
                                                    <td width="10%" align="right">
                                                        <%--<asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="tableposition">
                                            <div class="contentbox">
                                                <div class="grid2">
                                                    <asp:GridView ID="grdSalesChannelList" runat="server" FooterStyle-VerticalAlign="Top"
                                                        FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" EmptyDataText="No Record found"
                                                        RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                                        GridLines="none" AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow"
                                                        FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" CellSpacing="1"
                                                        CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                                                        AllowPaging="True" PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                                                        OnPageIndexChanging="grdSalesChannelList_PageIndexChanging" DataKeyNames="SalesChannelID"
                                                         OnRowCommand="grdSalesChannelList_RowCommand">
                                                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                        <Columns>
                                                            <%--<asp:TemplateField HeaderText="Select">
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
                                                            </asp:TemplateField>--%>
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
                                                    <tr>
                                                        <td align="right" valign="top" height="5" class="formtext">
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td align="left" valign="top" class="style1">
                                                            <asp:Button ID="btnSave" Text="Save" runat="server" ToolTip="Save" CssClass="buttonbg"
                                                                OnClick="btnSave_Click" ValidationGroup="Save"></asp:Button>
                                                            <asp:Button ID="btnReset" Text="Cancel" runat="server" ToolTip="Cancel" CssClass="buttonbg"
                                                                OnClick="btnReset_Click"></asp:Button>
                                                        </td>
                                                    </tr>--%>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td align="left" height="10">
                                    </td>
                                </tr>
                            </table>
                       
           <%-- </div>--%>
       <%-- </ContentTemplate>
        <%-- <Triggers>
            <asp:PostBackTrigger ControlID="ExportToExcel" />
        </Triggers>--%>
   <%-- </asp:UpdatePanel>--%>
</asp:Content>

