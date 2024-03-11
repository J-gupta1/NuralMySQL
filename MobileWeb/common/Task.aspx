<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="Task.aspx.cs" Inherits="CreateTask" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="../../UserControls/UCPagingControl.ascx" TagName="UCPagingControl" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <%-- <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/" + strAssets + "/Jscript/jquery.jcarousel.min.js") %>"></script>
    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/" + strAssets + "/Jscript/dhtmlwindow.js") %>"></script>
    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/" + strAssets + "/Jscript/modal.js") %>"></script>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />--%>

    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>
    <script language="javascript" type="text/javascript">
        function popup(url) {
            wd1 = dhtmlmodal.open('TaskDetail', 'iframe', 'TaskDetail.aspx?TaksUserID=' + url, 'Task Detail', 'width=500px,height=430px,top=25,resize=0,scrolling=auto ,center=1', 'recal')
            wd1.onclose = function () {
                //alert("POP up close");
                __doPostBack('contentHolderMain_btnShow', '');
                return true;



            }
            return true;
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Create / View Task
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblTask" CssClass="formtext" runat="server" AssociatedControlID="txtTask">Task:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtTask" runat="server" CssClass="formfields" MaxLength="500" TextMode="MultiLine"
                                    ValidationGroup="AddGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTask"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter task." SetFocusOnError="true"
                                    ValidationGroup="AddGroup"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblPriority" CssClass="formtext" runat="server" AssociatedControlID="ddlPriority">Priority:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList CausesValidation="true" ID="ddlPriority" runat="server" CssClass="formselect"
                                        AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:Label Style="display: none;" runat="server" ID="lblddlCheck" CssClass="error"></asp:Label>
                                    <asp:RequiredFieldValidator ID="ReqPriority" runat="server" ControlToValidate="ddlPriority" CssClass="error"
                                        Display="Dynamic" InitialValue="0" ErrorMessage="Please select priority." SetFocusOnError="true"
                                        ValidationGroup="AddGroup"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">
                                <asp:Label ID="Label1" CssClass="formtext" runat="server" AssociatedControlID="ddlTaskStatus">Task Status:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlTaskStatus" runat="server" CssClass="formselect" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:Label Style="display: none;" runat="server" ID="Label3" CssClass="error"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTaskStatus" CssClass="error"
                                        Display="Dynamic" InitialValue="0" ErrorMessage="Please select task status." SetFocusOnError="true" ValidationGroup="AddGroup"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">Task Group:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlTaskGroup" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:Label Style="display: none;" runat="server" ID="Label5" CssClass="error"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlTaskGroup" CssClass="error"
                                        Display="Dynamic" InitialValue="0" ErrorMessage="Please select task Group." SetFocusOnError="true"
                                        ValidationGroup="AddGroup"></asp:RequiredFieldValidator>
                                </div>
                            </li>

                        </ul>
                        <ul>

                            <li class="text">
                                <asp:Label ID="lblStartDate" runat="server" CssClass="formtext">Start Date:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucStartDate" runat="server"
                                    ValidationGroup="AddGroup" ErrorMessage="Invalid Date!" IsRequired="true" />
                            </li>


                            <li class="text">
                                <asp:Label ID="lblEndDate" runat="server" CssClass="formtext">End Date:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="UcEndDate" runat="server"
                                    ValidationGroup="AddGroup" ErrorMessage="Invalid Date!" IsRequired="true" />
                            </li>


                            <li class="text">
                                <asp:Label ID="lblRemark" runat="server" AssociatedControlID="txtRemark" CssClass="formtext"> Remark</asp:Label>

                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="formfields" MaxLength="500" TextMode="MultiLine"
                                    ValidationGroup="AddGroup"></asp:TextBox>


                            </li>
                        </ul>
                        <caption>
                            <br />

                        </caption>

                        <asp:Panel ID="pnlRegion" runat="server">
                            <div class="clear"></div>
                            <ul>
                                <li class="text">Select User:<span class="error">*</span>
                                </li>
                                <li class="text-field3" style="height: auto">
                                    <div style="width: 99%; overflow: auto;">
                                        <asp:CheckBoxList ID="chkUser" runat="server" CssClass="gridspace" RepeatColumns="4"
                                            RepeatDirection="Horizontal" Width="100%">
                                        </asp:CheckBoxList>
                                    </div>
                                </li>
                            </ul>
                        </asp:Panel>
                        <ul>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnCreateTask" Text="Submit" runat="server" CausesValidation="true"
                                        ValidationGroup="AddGroup" OnClick="btnCreateTask_Click" ToolTip="Submit"
                                        CssClass="buttonbg" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                        OnClick="btnCancel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvwTask" EventName="DataBound" />
                <asp:PostBackTrigger ControlID="btnCreateTask" />

            </Triggers>
        </asp:UpdatePanel>
        <div class="mainheading">
            Search Task
        </div>
        <div class="contentbox">
            <div class="H20-C3-S">
                <ul>

                    <li class="text">
                        <asp:Label ID="lblStartDateSearch" CssClass="formtext" runat="server">Start Date:<span class="error">+</span></asp:Label>
                    </li>
                    <li class="field">
                        <uc2:ucDatePicker ID="UcStartDateSearch" runat="server"
                            IsRequired="false" />
                    </li>
                    <li class="text">
                        <asp:Label ID="lblEndDateSearch" CssClass="formtext" runat="server">End Date:<span class="error">+</span></asp:Label>
                    </li>
                    <li class="field">
                        <uc2:ucDatePicker ID="UcEndDateSearch" runat="server"
                            IsRequired="false" />
                    </li>
                    <li class="text">
                        <asp:Label ID="Label2" CssClass="formtext" runat="server">Priority:<span class="error">+</span></asp:Label>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlPrioritySearch" runat="server" CssClass="formselect"
                                AutoPostBack="false">
                            </asp:DropDownList>
                        </div>

                    </li>

                    <li class="text">
                        <asp:Label ID="lblStatusSearch" CssClass="formtext" runat="server">Task Status:<span class="error">+</span></asp:Label>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlTaskStatusSearch" runat="server" CssClass="formselect"
                                AutoPostBack="false">
                            </asp:DropDownList>
                        </div>

                    </li>
                    <li class="text">
                        <asp:Label ID="Label4" CssClass="formtext" runat="server">Task User:<span class="error">+</span></asp:Label>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList CausesValidation="true" ID="ddlUserSearch" runat="server" CssClass="formselect"
                                AutoPostBack="false">
                            </asp:DropDownList>
                        </div>
                    </li>
                    <li class="text">Task Group:<span class="error">+</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList CausesValidation="true" ID="ddlTaskGroupSearch" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </div>
                    </li>
                    <li class="text"></li>
                    <li class="field">
                        <div class="float-margin">
                            <asp:Button ID="btnSearch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                OnClick="btnSearch_Click" CausesValidation="False"></asp:Button>
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                OnClick="btnShow_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnExprtToExcel" Text="Export In Excel" runat="server" CssClass="buttonbg" CausesValidation="False"
                                OnClick="btnExprtToExcel_Click" />
                        </div>
                    </li>
                </ul>

            </div>
        </div>
        <div class="mainheading">
            Task List
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="grdvwTask" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                    RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                    AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                    HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                    BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                    SelectedStyle-CssClass="gridselected"
                    DataKeyNames="TaskUserID" OnRowDataBound="grdvwTask_RowDataBound"
                    OnRowCommand="grdvwTask_RowCommand">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                    <Columns>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Task"
                            HeaderText="Task"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TaskGroupName"
                            HeaderText="Task GroupName"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TaskForUser"
                            HeaderText="User"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TaskPriority"
                            HeaderText="Priority"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TaskStatus"
                            HeaderText="Status"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StartDate"
                            HeaderText="Start Date"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EndDate"
                            HeaderText="End Date"></asp:BoundField>
                        <asp:TemplateField HeaderText="View Details">
                            <ItemTemplate>
                                <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg"></asp:HyperLink>

                                <asp:Label ID="lblTaskUserID" runat="server" Text='<%#Eval("TaskUserID")%>'
                                    Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <div id="dvFooter" runat="server" class="pagination">
                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
            </div>
        </div>
    </div>
</asp:Content>