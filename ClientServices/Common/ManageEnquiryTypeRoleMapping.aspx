<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ManageEnquiryTypeRoleMapping.aspx.cs" Inherits="ClientServices_Common_ManageEnquiryTypeRoleMapping" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker2.ascx" TagName="ucDatePicker2" TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript" src="../../Assets/Scripts/CommonFunctions.js"></script>

    <script type="text/javascript"> <%--#CC01 nonce Added --%>
        function changeCSSDropDown()
        { }


        function CheckBoxListSelect() {
            var sourceControl = document.getElementById("<%= chkAll.ClientID %>");
            var chkBoxList = document.getElementById("<%= chkRoles.ClientID %>");
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            for (var i = 0; i < chkBoxCount.length; i++) {
                chkBoxCount[i].checked = sourceControl.checked;
            }

            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <%-- <asp:UpdatePanel ID="updpnlSaveData" runat="server" UpdateMode="Always">
        <ContentTemplate>--%>
    <div>
        <uc4:ucMessage ID="ucMessage1" runat="server" />
    </div>
    <div>
        <div class="mainheading">
            <div>
                Add Enquiry Type Mapping
            </div>
            <%-- <uc4:ucMessage ID="ucMessage1" runat="server" />--%>
        </div>
        <asp:Panel ID="pnlSaveData" runat="server" DefaultButton="Save">
            <div class="contentbox">
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Enquiry Category:<span class="mandatory-img">&nbsp;</span></li>
                        <li class="field">
                            <asp:DropDownList ID="DdlEnquiryCategory" runat="server" CssClass="formselect" OnSelectedIndexChanged="DdlEnquiryCategory_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DdlEnquiryCategory"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select enquiry type."
                                InitialValue="0" ForeColor="" ValidationGroup="EnquiryMapping"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Enquiry SubCategory:<span class="mandatory-img">&nbsp;</span></li>
                        <li class="field">
                            <asp:DropDownList ID="ddlEnquiryType" runat="server" CssClass="formselect">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEnquiryType"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select enquiry type."
                                InitialValue="0" ForeColor="" ValidationGroup="EnquiryMapping"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Valid From:<span class="mandatory-img">&nbsp;</span></li>
                        <li class="field">
                            <uc1:ucDatePicker ID="ucValidFrom" runat="server" IsRequired="True" ErrorMessage="Please Enter date."
                                RangeErrorMessage="Invalid date" ValidationGroup="EnquiryMapping" />
                        </li>
                    </ul>
                    <ul id="idrole" runat="server" visible="false">
                        <li class="text">Roles:<span class="mandatory-img">&nbsp;</span></li>
                        <li class="field2" style="height: auto;">
                            <div class="boxlist padding" style="width: 100%; height: auto; max-height: 100px; overflow: auto; border: 1px solid #dddddd;">
                                <div style="height: 20px; margin-left: -3px">
                                    <%-- &nbsp; &nbsp;--%>&nbsp;
                                        <asp:CheckBox ID="chkAll" runat="server" Text="Select All" onclick="javascript: CheckBoxListSelect()" />
                                </div>
                                <asp:Panel ID="pnlRoles" runat="server" ScrollBars="Auto">
                                    <asp:CheckBoxList ID="chkRoles" runat="server" CssClass="CheckBoxList" CellPadding="2" RepeatColumns="3"
                                        RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </div>
                        </li>
                    </ul>
                    <div class="clear" style="height: 10px">
                    </div>
                    <ul>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <cc2:ZedButton ID="Save" runat="server" CssClass="buttonbg" OnClick="Save_Click" Text="Save"
                                    ValidationGroup="EnquiryMapping" ActionTag="Add" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" OnClick="btnCancel_Click"
                                    Text="Cancel" CausesValidation="false" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </asp:Panel>
    </div>
    <%--<div class="box1">
                <div class="subheading">
                    Search Intent Information<br />
                </div>
                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch" >
                    <div class="innerarea1">
                    <div class="fieldsarea1">
                        <ul>
                            <li class="text">Country:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtSerCountry" runat="server" CssClass="formfields"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="error"
                                    Display="Dynamic" ControlToValidate="txtSerCountry" ErrorMessage="Invalid char(s)!"
                                    ValidationExpression="[^<>@%]{1,50}$" SetFocusOnError="true"></asp:RegularExpressionValidator>
                            </li>
                            <li style="width: 250px;">
                                <uc5:ucStatus ID="ucStatusSer" runat="server" />
                            </li>
                            <li>
                                <cc2:ZedButton ActionTag="View" ID="btnSearch" runat="server" Text="Search" CssClass="button1"
                                    OnClick="btnSearch_Click" />
                            </li>
                            <li>
                                <cc2:ZedButton ActionTag="View" ID="btnShowAll" runat="server" Text="View All" CssClass="button1"
                                    OnClick="btnShowAll_Click" CausesValidation="false" />
                            </li>
                        </ul>
                    </div>
                </div>
                </asp:Panel>
            </div>--%>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <div class="export padding-top">
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export to Excel" CssClass="elink2" ForeColor="Green" OnClick="lnkExportToExcel_Click"></asp:LinkButton>
    </div>
    <%--<asp:UpdatePanel ID="updpnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>--%>
    <div runat="server" id="divgrd">
        <div class="mainheading">
            <div class="float-left">
                Entity Type Mapping List
            </div>
            <div class="export">
                <%--<asp:ImageButton ID="Exporttoexcel" runat="server" ImageUrl="~/Assets/images/excel.gif"
                            OnClick="Exporttoexcel_Click" CausesValidation="False" AlternateText="Export to Excel" />--%>
                <%-- <asp:LinkButton ID="lnkExportToExcel" runat="server" Text="Export to Excel"
                                                                     CssClass="elink2" Style="color: green" OnClick="lnkExportToExcel_Click"></asp:LinkButton>--%>
            </div>
            <%-- <asp:Button ID="Button1" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />--%>
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" DataKeyNames="EnquiryTypeRoleMappingID"
                    EmptyDataText="No Record Found" GridLines="None" OnRowCommand="grdvList_RowCommand"
                    OnRowDataBound="grdvList_RowDataBound" Width="100%" HeaderStyle-CssClass="gridheader"
                    RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow">
                    <Columns>
                        <asp:BoundField DataField="EnquiryType" HeaderText="Enquiry Type" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EntityTypeRoleName" HeaderText="Role Name" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ValidFrom" HeaderText="Valid From" HeaderStyle-HorizontalAlign="Left"
                            DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="ValidTill" HeaderText="Valid Till" HeaderStyle-HorizontalAlign="Left"
                            DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField ShowHeader="False" HeaderText="Action" HeaderStyle-HorizontalAlign="Left"
                            HeaderStyle-Width="110px">
                            <%--<ItemTemplate>
                                        <asp:Label ID="lblvalidfrom" Visible="false" runat="server" Text='<%# Eval("validfrom") %>'
                                            CommandName="activeTax"></asp:Label>
                                        <asp:Label ID="lblactive" Visible="false" runat="server" Text='<%# Eval("Active") %>'
                                            CommandName="activeZone" CommandArgument='<%# Eval("EnquiryTypeRoleMappingID") %>'></asp:Label>
                                        <cc2:ZedImageButton ActionTag="Edit" ID="Active" runat="server" Text='<%# Eval("Active") %>'
                                            CommandName="activeMapping" CommandArgument='<%# Eval("EnquiryTypeRoleMappingID") %>' />
                                        <%--<cc2:ZedImageButton ActionTag="Edit" ID="imgbtnEdit" runat="server" CommandArgument='<%# Eval("EnquiryTypeRoleMappingID") %>'
                                            CommandName="editintenttype" ImageUrl="~/Assets/images/edit.png" AlternateText="Edit"
                                            Visible="false" title="Edit" />--%>
                            <%-- <asp:ImageButton ID="imgbtnDelete" runat="server" CommandArgument='<%# Eval("ServiceChargesMasterID") %>'
                                            CommandName="deleteCode" ImageUrl="~/Assets/images/delete.png" AlternateText="Delete" Visible="false"  
                                            title="Delete" />
                                    </ItemTemplate>--%>
                            <ItemTemplate>
                                <asp:Label ID="lblvalidfrom" Visible="false" runat="server" Text='<%# Eval("validfrom") %>'
                                    CommandName="activeTax"></asp:Label>
                                <asp:Label ID="lblactive" Visible="false" runat="server" Text='<%# Eval("Active") %>'
                                    CommandName="activeZone" CommandArgument='<%# Eval("EnquiryTypeRoleMappingID") %>'></asp:Label>
                                <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("EnquiryTypeRoleMappingID") %>'
                                    CommandName="activeMapping" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                    ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Active"))) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="clear">
            </div>
            <div id="dvFooter" runat="server" class="pagination">
                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
            </div>
        </div>
    </div>
    <%--   </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

