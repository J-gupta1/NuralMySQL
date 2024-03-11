<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserRoleModuleMapping.aspx.cs" Inherits="Masters_HO_Admin_UserRoleModuleMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">

        /* Check / Uncheck All checkboxes */
        function CheckAll(Checkbox) {
            var GridVwHeaderCheckbox = document.getElementById("<%=grdUser.ClientID %>");
              for (i = 2; i < GridVwHeaderCheckbox.rows.length + 1; i++) {

                  try {
                      if (i < 10) {
                          i = '0' + i;
                      }
                      var chck = document.getElementById('ctl00_contentHolderMain_grdUser_ctl' + i + '_chckSelectItem')
                      chck.checked = Checkbox.checked;
                  }
                  catch (err) {
                  }
              }
              return true;
          }
          function CheckOne(CheckBox) {
              if (CheckBox.checked == false) {
                  document.getElementById('ctl00_contentHolderMain_grdUser_ctl01_chckSelectAll').checked = false;
              }
              else {

                  var GridVwHeaderCheckbox = document.getElementById("<%=grdUser.ClientID %>");
                  var newCheck = 0;
                  for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                      try {
                          if (GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked != true) {
                              newCheck = 1;
                              break;
                          }
                      }
                      catch (err) {
                      }
                  }
                  if (newCheck == 0) {
                      document.getElementById('ctl00_contentHolderMain_grdUser_ctl01_chckSelectAll').checked = true;
                  }
                  else if (newCheck == 1) {
                      document.getElementById('ctl00_contentHolderMain_grdUser_ctl01_chckSelectAll').checked = false;
                  }
              }
              return true;
          }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGo" />
            <asp:PostBackTrigger ControlID="btnSubmit" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add/Update
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdUpdate" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory. (++) Marked fields are optional.
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Role: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList CausesValidation="false" ID="ddlRole" runat="server"
                                CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator ID="rqddlRole" runat="server" ControlToValidate="ddlRole"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Select Role." SetFocusOnError="true"
                                    InitialValue="0" ValidationGroup="Search"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Module: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlModule" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rqddlModule" runat="server" ControlToValidate="ddlModule"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Select Module." SetFocusOnError="true"
                                    InitialValue="0" ValidationGroup="Search"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Login Name: <span class="error">++</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtLoginName" runat="server" CssClass="formfields" MaxLength="100">
                            </asp:TextBox>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <asp:Button ID="btnGo" Text="Go" runat="server" ToolTip="Search" CssClass="buttonbg"
                                OnClick="btnGo_Click" ValidationGroup="Search"></asp:Button>
                        </li>
                    </ul>
                </div>
                <div class="grid1 margin-top">
                    <asp:GridView ID="grdUser" runat="server" FooterStyle-VerticalAlign="Top"
                        FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                        HeaderStyle-HorizontalAlign="left" EmptyDataText="No Record found" HeaderStyle-VerticalAlign="top"
                        GridLines="none" AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow"
                        FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" CellSpacing="0"
                        PageSize='<%$ AppSettings:GridViewPageSize %>' CellPadding="4" bgcolor="" BorderWidth="0px"
                        Width="100%" AutoGenerateColumns="false" AllowPaging="false" SelectedStyle-CssClass="gridselected">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                        <Columns>

                            <asp:TemplateField HeaderStyle-Width="100px" ShowHeader="true">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chckSelectAll" runat="server" Text="Select All" TextAlign="Right" onclick="CheckAll(this);" />
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:CheckBox ID="chckSelectItem" runat="server" onclick="CheckOne(this);" Checked='<%# !Convert.ToBoolean(Eval("IsExcluded")) %>' />

                                    <asp:HiddenField ID="hdnUserID" runat="server" Value='<%# Eval("UserID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="User Name" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblLoginName" runat="server" Text='<%# Eval("LoginName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Code" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("SalesChannelCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
                <div class="margin-top">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" OnClick="btnSubmit_Click" Visible="false" />
                </div>
                <div class="clear">
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
