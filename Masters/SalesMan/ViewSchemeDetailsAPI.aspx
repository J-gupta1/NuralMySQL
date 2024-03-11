<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewSchemeDetailsAPI.aspx.cs" Inherits="Masters_SalesMan_ViewSchemeDetailsAPI" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>


                <%-- <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                                <ContentTemplate>--%>
                <uc1:ucMessage ID="ucMsg" runat="server" />
                <%-- </ContentTemplate>
                                            </asp:UpdatePanel>--%>


                <div class="mainheading">
                    Search
                </div>
                <div class="contentbox">
                    <%-- <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>--%>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">Enrolled By:
                            </li>
                            <li class="field">
                                <asp:DropDownList CssClass="formselect" ID="ddlFOSTSM" runat="server">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <%-- <td align="right" valign="top" width="60" height="25" class="formtext">
                                                                    User Role:&nbsp;
                                                                </td>--%>
                            <li class="text">Sales Channel Type:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSalesChannelType" CssClass="formselect" runat="server">
                                </asp:DropDownList>
                            </li>

                            <%--# Add Start --%>
                            <li class="text">Sales Channel Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </li>
                            <li class="text">Scheme Date From:
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date!"
                                    runat="server" />
                            </li>
                            <%-- <td align="right" valign="top" width="60" height="25" class="formtext">
                                                                    User Role:&nbsp;
                                                                </td>--%>
                            <li class="text">Scheme Date To:
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date."
                                    runat="server" />
                            </li>
</ul>
                            <%--# Add Start --%>

                         <div class="setbbb">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearchUser" runat="server" CausesValidation="false" CssClass="buttonbg" OnClick="btnSearchUser_Click" Text="Search" ToolTip="Search" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnShow" runat="server"  CausesValidation="false" CssClass="buttonbg" OnClick="btnShow_Click" Text="Show All" ToolTip="Show All" />
                                </div>
                            </div>
                        
                    </div>
                    <%-- </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                </div>

                <div class="mainheading">
                    Scheme List
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
                        <asp:GridView ID="gvSalesMan" runat="server" EmptyDataText="No Record Found" EditRowStyle-CssClass="Emptyrow" AlternatingRowStyle-CssClass="Altrow"
                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            DataKeyNames="SchemeID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
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
                                        <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                            <%# DataBinder.Eval(Container.DataItem, "SalesChannelName")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="StateName"
                                    HeaderText="State Name" NullDisplayText="N/A"></asp:BoundField>

                                <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="CityName"
                                    HeaderText="City Name" NullDisplayText="N/A"></asp:BoundField>
                                
                                <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="SchemeName"
                                    HeaderText="Scheme Name" NullDisplayText="N/A"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="Period"
                                    HeaderText="Period" NullDisplayText="N/A"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="Slab"
                                    HeaderText="Slab" NullDisplayText="N/A"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Payout"
                                    HeaderText="Payout" NullDisplayText="N/A"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Achievement"
                                    HeaderText="Achievement" NullDisplayText="N/A"></asp:BoundField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                      Enrolled By
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                            <%# DataBinder.Eval(Container.DataItem, "SalesmanName")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderTemplate>
                                      Enrolled Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                            <%# DataBinder.Eval(Container.DataItem, "EnrolledDate")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                 <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Active"
                                    HeaderText="Active" NullDisplayText="N/A"></asp:BoundField>
                                 <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="ModifiedBy"
                                    HeaderText="Modified By" NullDisplayText="N/A"></asp:BoundField>

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
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnShow" />
                <asp:PostBackTrigger ControlID="btnSearchUser" />
                <asp:PostBackTrigger ControlID="btnExprtToExcel" />
                <asp:PostBackTrigger ControlID="gvSalesMan" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
