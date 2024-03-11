<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewFeedbackDetails.aspx.cs" Inherits="Masters_SalesMan_ViewFeedbackDetails" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <script language="javascript" type="text/javascript">
        function popup(url) {//"width=500px,height=430px,top=25,resize=0,scrolling=auto ,center=1"
            var title = 'ViewDetails';
            var w = '600';
            var h = '600';
            // Fixes dual-screen position                         Most browsers      Firefox
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : window.screenX;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : window.screenY;

            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }

        }
    </script>
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
                            <li class="text">Created By:
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
                            <li class="text">Feedback Date From:
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date!"
                                    runat="server" />
                            </li>
                            <%-- <td align="right" valign="top" width="60" height="25" class="formtext">
                                                                    User Role:&nbsp;
                                                                </td>--%>
                            <li class="text">Feedback Date To:
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date."
                                    runat="server" />
                            </li>

                            <%--# Add Start --%>
                            <li class="text">Category:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="formselect">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li class="text">Model Name:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlModel" runat="server" CssClass="formselect">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>

                            </li>
                            <li class="text">Feedback No:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtFeedbackNo" runat="server" CssClass="formfields" MaxLength="80"></asp:TextBox>
                                <div>
                                    <cc1:FilteredTextBoxExtender ID="fltrExtenderSalesmanMobileNumber" runat="server" TargetControlID="txtFeedbackNo"
                                        ValidChars="0123456789+-().,/">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                            </li>

                            <li class="text">Brand:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlBrand" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Product Category:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlproductcategory" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
      </ul>
                            <%--# Add End --%>


                          <div class="setbbb">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearchUser" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearchUser_Click" CausesValidation="false"></asp:Button>
                                    <%--# Validation group added and causevalidation="False" removed--%>
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnShow" runat="server" CausesValidation="false" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                        OnClick="btnShow_Click" />
                                </div>
                            </div>
                  
                    </div>
                    <%-- </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                </div>

                <div class="mainheading">
                    Feedback List
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
                        <asp:GridView ID="gvSalesMan" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            DataKeyNames="FeedbackId" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                            AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>

                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <%--<asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelType"
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
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="StateName"
                                        HeaderText="State Name" NullDisplayText=""></asp:BoundField>

                                    <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="CityName"
                                        HeaderText="City Name" NullDisplayText=""></asp:BoundField>
                                    <asp:TemplateField>--%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Created By
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 100px;">
                                            <%# DataBinder.Eval(Container.DataItem, "SalesmanName")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        SalesChannel/Org Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 100px;">
                                            <%# DataBinder.Eval(Container.DataItem, "SaleschannelName")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        SalesChannel/Org Code
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 100px;">
                                            <%# DataBinder.Eval(Container.DataItem, "SaleschannelCode")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        SalesChannel/Org Type
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 100px;">
                                            <%# DataBinder.Eval(Container.DataItem, "SalesChannelType")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="FeedbackId"
                                    HeaderText="Feedback No." NullDisplayText=""></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="CategoryName"
                                    HeaderText="Category" NullDisplayText=""></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="FeedbackDate"
                                    HeaderText="Feedback Date" NullDisplayText=""></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="ModelName"
                                    HeaderText="Model" NullDisplayText=""></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="AppFeedbackText"
                                    HeaderText="Remarks" NullDisplayText=""></asp:BoundField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Image 1
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 50px; text-align: center">

                                            <a href="javascript:popup('<%# (Eval("SavedImagePath1") == null || Eval("SavedImagePath1")== "" ) ? "" :ConfigurationSettings.AppSettings["siteurl"]+DataBinder.Eval(Container.DataItem, "SavedImagePath1")%>')"><%# (Eval("SavedImagePath1")==null || Eval("SavedImagePath1")=="") ? "" :"View"%></a>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Image 2
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 50px; text-align: center">
                                            <a href="javascript:popup('<%# (Eval("SavedImagePath2") == null || Eval("SavedImagePath2")== "" ) ? "" :ConfigurationSettings.AppSettings["siteurl"]+DataBinder.Eval(Container.DataItem, "SavedImagePath2")%>')"><%# (Eval("SavedImagePath2")==null || Eval("SavedImagePath2")=="") ? "" :"View"%></a>

                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Image 3
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 50px; text-align: center">
                                            <a href="javascript:popup('<%# (Eval("SavedImagePath3") == null || Eval("SavedImagePath3")== "" ) ? "" :ConfigurationSettings.AppSettings["siteurl"]+DataBinder.Eval(Container.DataItem, "SavedImagePath3")%>')"><%# (Eval("SavedImagePath1")==null || Eval("SavedImagePath3")=="") ? "" :"View"%></a>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%--  <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="FeedbackRevertDate"
                                        HeaderText="Revert Date" NullDisplayText=""></asp:BoundField>--%>
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