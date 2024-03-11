<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ISPMasterUploadV2.aspx.cs" Inherits="Masters_Common_ISPMasterUploadV2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucNavigationLinks.ascx" TagName="ucLinks" TagPrefix="uc7" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>   
    <div class="clear"></div>  
    <div class="tabArea">
                    <uc7:uclinks ID="ucLinks" runat="server" XmlFilePath="../../Assets/XML/LinksXML.xml"/></div>

    <div class="mainheading">
        Step 1 : Please Select For Save Or Update Process
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">

            <ul>
                <li class="text">Upload Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlUploadType" runat="server" CssClass="formselect">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Add" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Update" Value="2"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="reqddlUploadType" runat="server" ControlToValidate="ddlUploadType"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please select upload Type."
                        InitialValue="0" SetFocusOnError="true" ValidationGroup="Upload"></asp:RequiredFieldValidator>

                </li>
            </ul>
        </div>
    </div>

    <div class="mainheading">
        Step 2 : Download  Template For Add/Update Record 
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="lnkDownloadTemplate" runat="server" class="elink2" Text="Download Template" OnClick="lnkDownloadTemplate_Click" ValidationGroup="Upload"></asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>


    <div class="mainheading">
        Step 3 : Download Reference Code For Save Record  
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="LnkDownloadRefCode" runat="server" CssClass="elink2" OnClick="LnkDownloadRefCode_Click">Download Retailer Reference Code</asp:LinkButton>
                </li>
                <li class="link">
                    <asp:LinkButton ID="LnkDownloadISPMasterData" runat="server" CssClass="elink2" Text="Download ISP Master Data"
                        OnClick="LnkDownloadISPMasterData_Click"></asp:LinkButton>
                </li>

                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkDuplicate" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkBlank" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
                <li>
                    <asp:Label ID="lblUploadMsg" CssClass="error" runat="server" Text=""></asp:Label>
                </li>
            </ul>
        </div>
    </div>



    <div class="mainheading">
        Step 4 : Upload Excel File
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul id="idselectmode" runat="server" visible="false">
                <li class="text">Select Mode: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CssClass="radio-rs" CellPadding="2"
                            AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="1" Text="Excel Template"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Interface"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </li>
            </ul>
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>


                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" ValidationGroup="Upload" />
                </li>
            </ul>
        </div>
        <div class="formlink">
            <ul>
                <li class="link"></li>

            </ul>
        </div>
    </div>
    <div class="mainheading">
        Search 
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">ISP Name:&nbsp;
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtISPname" runat="server" MaxLength="100" CssClass="formfields">
                            </asp:TextBox>
                        </li>
                        <li class="text">ISP Code:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtISPCode" runat="server" MaxLength="100" CssClass="formfields">
                            </asp:TextBox>
                        </li>
                        <li class="text">Status:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlUserStatus" runat="server" CssClass="formselect">
                                <asp:ListItem Selected="True" Value="2" Text="All"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                <asp:ListItem Value="0" Text="Inactive"></asp:ListItem>
                            </asp:DropDownList>
                        </li>

                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblMobileNumberSearch" runat="server" CssClass="formtext">Mobile Number:</asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtMobileNumberSearch" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox>

                            <asp:RegularExpressionValidator ID="regexMobileNumberSearch" runat="server" ControlToValidate="txtMobileNumberSearch"
                                ValidationExpression="^[1-9]([0-9]{9})$" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                TargetControlID="txtMobileNumberSearch" ValidChars="0123456789.">
                            </cc1:FilteredTextBoxExtender>

                        </li>
                        <li class="text">
                            <asp:Label ID="lblEmailIDSearch" CssClass="formtext" runat="server">Email Id: </asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtEmailIDSearch" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="regexEmailIDSearch" runat="server" CssClass="error"
                                ControlToValidate="txtEmailIDSearch" meta:resourcekey="RegularEmail" Display="Dynamic"
                                ValidationExpression="^([0-9a-zA-Z.-]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{1,9})$"
                                ErrorMessage="Please enter valid email" SetFocusOnError="true"></asp:RegularExpressionValidator>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearchUser" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                    OnClick="btnSearchUser_Click" CausesValidation="False"></asp:Button>
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnSbtnSearchUserhow" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                    OnClick="btnShow_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>

        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        ISP List
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="GridISP" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                        RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                        AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                        HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                        BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                        PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                        OnPageIndexChanging="GridISP_PageIndexChanging" DataKeyNames="UserID,ISPID" OnRowDataBound="GridISP_RowDataBound"
                        OnRowCommand="GridISP_RowCommand">
                        <Columns>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ISPName"
                                HeaderText="ISP Name"></asp:BoundField>
                            <asp:TemplateField HeaderText="ISP Code" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label runat="server" Text='<%#Eval("ISPCode") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Full Name" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label runat="server" Text='<%#Eval("Fullname") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email ID" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mobile Number" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("Mobile") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Login Name" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label runat="server" Text='<%#Eval("LoginName") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Password">
                                <ItemTemplate>
                                    <asp:Label ID="lblPassword" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Password"))%>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblPasswordSalt" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"PasswordSalt"))%>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="hlPassword" runat="server" Text="Password"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemTemplate>
                                    <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                        <asp:Label runat="server" Text='<%#Eval("ISPStatus") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSbtnSearchUserhow" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
