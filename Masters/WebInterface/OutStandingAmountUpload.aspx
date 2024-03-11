<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="OutStandingAmountUpload.aspx.cs" Inherits="Masters_Common_OutStandingAmountUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--# Add Start --%>
    <script type="text/javascript">
        function ShowHideTemplate() {
            var vrdvsavetemplate = document.getElementById("dvSaveTemplate");
            var vrdvupdatetemplate = document.getElementById("dvUpdateTemplate");
            var vrhlnkinvalid = document.getElementById('<%=hlnkInvalid.ClientID%>');
            // alert(ddlUploadTypeValue.value);
            if (vrddlUploadTypeValue.value == 0) {
                vrdvsavetemplate.style.display = "none";
                vrdvupdatetemplate.style.display = "none";
            }
            else if (vrddlUploadTypeValue.value == 1) {
                vrdvsavetemplate.style.display = "block";
                vrdvupdatetemplate.style.display = "none";
            }
            else if (vrddlUploadTypeValue.value == 2) {
                vrdvsavetemplate.style.display = "none";
                vrdvupdatetemplate.style.display = "block";
            }
            vrhlnkinvalid.style.display = "none";
            return false;
        }
    </script>

    <%--# Add End--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="mainheading">
        Upload
    </div>

    <div class="contentbox">
        <%--<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Select Mode:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CssClass="radio-rs" CellPadding="2"
                        AutoPostBack="false">
                        <asp:ListItem Selected="True" Value="1" Text="Excel Template"></asp:ListItem>
                        <%--<asp:ListItem Value="2" Text="Interface"></asp:ListItem>--%>
                    </asp:RadioButtonList>
                </li>
                <li class="text">Outstanding Date:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucOutStandingDate" runat="server" ErrorMessage="Outstanding Date required!"
                        ValidationGroup="vgStockRpt"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date!" />
                </li>
            </ul>
            <div class="clear"></div>
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field"><%--#CC05 width reduced --%>
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <%--# Add Start--%>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" ValidationGroup="vgStockRpt" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
                <li class="link"><%--#CC05 width increased --%>
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink><%--# Added--%>
                                            
                </li>
                <%--# Add End--%>

                <%-- <td align="left" valign="top"></td>--%> <%--#CC05 commented--%>

                <li class="link">
                    <div class="float-margin" id="dvUpdateTemplate">
                        <asp:LinkButton ID="DwnldReferenceCodeTemplate" CausesValidation="false" runat="server" Text="Download Reference Code"
                            CssClass="elink2" OnClick="DwnldReferenceCode_Click"></asp:LinkButton>&nbsp;
                                                    
                    </div>
                </li>
                <li class="link">
                    <div class="float-margin" id="dvSaveTemplate">
                        <a class="elink2" href="../../Excel/Templates/UploadOutStandingAmount.xlsx">Download Template</a>
                    </div>

                    <%-- Add End --%>

                </li>
            </ul>
        </div>
        <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
        --%>
        <%--  </Triggers>
                                </asp:UpdatePanel>--%>
    </div>

    <%--#CC01 Start--%>


    <div class="mainheading">
        Search
    </div>
    <div class="contentbox">
        <%-- <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>--%>
        <div class="H20-C3-S">
            <ul>
                <li class="text">Sales Channel Type:
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlSalesChannelType" CssClass="formselect" runat="server">
                    </asp:DropDownList>
                </li>
                <%-- <td align="right" valign="top" width="60" height="25" class="formtext">
                                                                    User Role:&nbsp;
                                                                </td>--%>
                <li class="text">Sales Channel Name:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                </li>

                <%--# Add Start --%>           
                <li class="text">Outstanding Date From:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date!"
                        runat="server" />
                </li>
                <%-- <td align="right" valign="top" width="60" height="25" class="formtext">
                                                                    User Role:&nbsp;
                                                                </td>--%>
                <li class="text">Outstanding Date To:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." runat="server" />
                </li>
</ul>
                <%--# Add Start --%>
               <div class="setbbb">
                    <div class="float-margin">
                        <asp:Button ID="btnSearchUser" runat="server" CausesValidation="false" CssClass="buttonbg" OnClick="btnSearchUser_Click" Text="Search" ToolTip="Search" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnShow" runat="server" CausesValidation="false" CssClass="buttonbg" OnClick="btnShow_Click" Text="Show All" ToolTip="Show All" />
                    </div>
                </div>
            
        </div>
        <%-- </ContentTemplate>
                                                </asp:UpdatePanel>--%>
    </div>
    <div class="mainheading">
        Outstanding List
    </div>
    <div class="export">
        <asp:Button ID="btnExprtToExcel" Text="" Visible="false" runat="server" CssClass="excel" CausesValidation="False"
            OnClick="btnExprtToExcel_Click" />
    </div>
    <div class="contentbox">

        <%--<%# DataBinder.Eval(Container.DataItem, "SalesmanName")%>--%>
        <%--   <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
        <%--#CC04 START COMMENTED AllowPaging="True" OnPageIndexChanging="gvSalesMan_PageIndexChanging"  #CC04 END COMMENTED--%>
        <div class="grid1">
            <asp:GridView ID="gvSalesMan" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow" EmptyDataRowStyle-CssClass="Emptyrow"
                bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                DataKeyNames="OutStandingAmountID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>

                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                <Columns>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelType"
                        HeaderText="Sales Channel Type"></asp:BoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Sales Channel Name
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="word-wrap: break-word; overflow: hidden; width: auto;">
                                <%# DataBinder.Eval(Container.DataItem, "SalesChannelName")%>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="StateName"
                        HeaderText="State Name" NullDisplayText="N/A"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="CityName"
                        HeaderText="City Name" NullDisplayText="N/A"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="Narration"
                        HeaderText="Narration" NullDisplayText="N/A"></asp:BoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Outstanding Amount
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="word-wrap: break-word; overflow: hidden; width: auto;">
                                <%#  DataBinder.Eval(Container.DataItem, "Amount","{0:n}")%>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="OutstandingDate"
                        HeaderText="Outstanding Date" NullDisplayText="N/A"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="Remarks"
                        HeaderText="Remarks" NullDisplayText=""></asp:BoundField>


                </Columns>
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
            <%--#CC04 START ADDED--%>
            <div class="clear">
            </div>
        </div>
        <div id="dvFooter" runat="server" class="pagination">
            <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
        </div>


        <%--   </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:PostBackTrigger ControlID="gvSalesMan" />
                                             
                                            </Triggers>
                                        </asp:UpdatePanel>--%>
    </div>

    <%--#CC01 End--%>
</asp:Content>
