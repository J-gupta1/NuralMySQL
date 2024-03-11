<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewProspectDetails.aspx.cs" Inherits="Masters_SalesMan_ViewProspectDetails" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                            <li class="text">Sale Channel Type:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSalesChannelType" CssClass="formselect" runat="server">
                                </asp:DropDownList>
                            </li>

                            <%--# Add Start --%>
                            <li class="text">Company Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </li>
                            <li class="text">Contact No.:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtContactNo" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="fltrExtenderSalesmanMobileNumber" runat="server" TargetControlID="txtContactNo"
                                    ValidChars="0123456789+-().,/">
                                </cc1:FilteredTextBoxExtender>

                                <asp:RegularExpressionValidator ID="regexSalesManMobileSearch" runat="server" ControlToValidate="txtContactNo"
                                    ValidationExpression="^[1-9]([0-9]{9})$" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                    ValidationGroup="SearchSalesman" CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>

                            </li>
                            <li class="text">Person Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtPersonName" runat="server" CssClass="formfields" MaxLength="80"></asp:TextBox>
                                <div>
                                    <%-- <asp:RegularExpressionValidator ID="regexEmailSearch" runat="server" ControlToValidate="txtEmailIDSearch"
                                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid emailid." ValidationGroup="SearchSalesman"
                                                                            ForeColor="" meta:resourcekey="RegularEmail" SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>--%>
                                </div>
                            </li>

                            <%--# Add End --%>
                            <li class="text">Creation From Date:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                        </li>
                        <li class="text">To Date:<font class="error">+</font>
                        </li>
                            <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                        </li>
</ul>
                          <div class="setbbb">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearchUser" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearchUser_Click" ValidationGroup="SearchSalesman"></asp:Button>
                                    <%--# Validation group added and causevalidation="False" removed--%>
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                        OnClick="btnShow_Click" />
                                </div>
                            </div>
                        
                    </div>
                    <%-- </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                </div>
                <div class="mainheading">
                    Prospect List
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
                        <asp:GridView ID="gvSalesMan" runat="server" EmptyDataText="No Record Found" EmptyDataRowStyle-CssClass="Emptyrow" AlternatingRowStyle-CssClass="Altrow"
                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            DataKeyNames="ProspectId" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                            AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>

                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelType"
                                    HeaderText="Sales Channel Type"></asp:BoundField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Company Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                            <%# DataBinder.Eval(Container.DataItem, "CompanyName")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                
                                <asp:BoundField HtmlEncode="true" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" DataField="PersonName"
                                    HeaderText="Person Name" NullDisplayText="N/A"></asp:BoundField>

                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ContactNo"
                                    HeaderText="Contact No." NullDisplayText="N/A"></asp:BoundField>

                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EmailID"
                                    HeaderText="Email ID" NullDisplayText="N/A"></asp:BoundField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Address
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                            <%# (Eval("Address")==null || Eval("Address")=="") ? "N/A" :Eval("Address")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       Created By
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                            <%# DataBinder.Eval(Container.DataItem, "SalesmanName")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="CreatedOn"
                                    HeaderText="Creation Date" NullDisplayText="N/A"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Brands"
                                    HeaderText="Brand" NullDisplayText="N/A"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Remarks"
                                    HeaderText="Remarks" NullDisplayText="N/A"></asp:BoundField>
                                

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Business Card Image
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 150px; text-align: center">
                                            <a href="javascript:popup('<%# (Eval("BusinessCardImage") == null || Eval("BusinessCardImage")== "" ) ? "" : ConfigurationSettings.AppSettings["siteurl"]+DataBinder.Eval(Container.DataItem, "BusinessCardImage")%>')"><%# (Eval("BusinessCardImage")==null || Eval("BusinessCardImage")=="") ? "" :"View"%></a>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Shop Image
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; overflow: hidden; width: 150px; text-align: center">
                                            <a href="javascript:popup('<%# (Eval("ShopImage") == null || Eval("ShopImage")== "" ) ? "" : ConfigurationSettings.AppSettings["siteurl"]+DataBinder.Eval(Container.DataItem, "ShopImage")%>')"><%# (Eval("ShopImage")==null || Eval("ShopImage")=="") ? "" :"View"%></a>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

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
