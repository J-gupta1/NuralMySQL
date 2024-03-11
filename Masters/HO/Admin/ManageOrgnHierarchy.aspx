<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageOrgnHierarchy.aspx.cs" Inherits="Masters_HO_Admin_ManageOrgnHierarchy" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucNavigationLinks.ascx" TagName="ucLinks" TagPrefix="uc7" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    <div class="clear"></div>  
    <div class="tabArea">
                    <uc7:uclinks ID="ucLinks" runat="server" XmlFilePath="~/Assets/XML/LinksXML.xml"/></div>
        <%-- <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
        <div class="mainheading">
            Add / Edit Location
        </div>
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory            
            </div>
            <div class="H25-C3-S">
                <ul>
                    <li class="text">
                        <asp:Label ID="lblddlHierarchyLevel" CssClass="formtext" runat="server" AssociatedControlID="ddlHierarchyLevel">Hierarchy Level:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                        <asp:DropDownList CausesValidation="true" ID="ddlHierarchyLevel" runat="server" CssClass="formselect"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlHierarchyLevel_SelectedIndexChanged">
                        </asp:DropDownList>
                        <div>
                            <asp:Label Style="display: none;" runat="server" ID="lblHierarchyLevel" CssClass="error"></asp:Label><asp:RequiredFieldValidator
                                ID="ReqUserGroup" runat="server" ControlToValidate="ddlHierarchyLevel" CssClass="error"
                                Display="Dynamic" InitialValue="0" ErrorMessage="Please select hierarchy level."
                                SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">
                        <asp:Label ID="lblParentHierarchy" runat="server" AssociatedControlID="ddlParentHierarchy"
                            CssClass="formtext">Parent Hierarchy Name:</asp:Label>
                    </li>
                    <li class="field">
                        <asp:DropDownList CausesValidation="true" ID="ddlParentHierarchy" runat="server"
                            CssClass="formselect">
                        </asp:DropDownList>
                        <%--<br />
                                                                                <asp:Label Style="display: none;" runat="server" ID="Label1" CssClass="error"></asp:Label><asp:RequiredFieldValidator
                                                                                    ID="reqParentHierarchy" runat="server" ControlToValidate="ddlParentHierarchy"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select parent hierarchy."
                                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>--%>
                    </li>
                    <li class="text">
                        <asp:Label ID="lblLocationName" runat="server" AssociatedControlID="txtLocationName"
                            CssClass="formtext">Location Name:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtLocationName" runat="server" CssClass="formfields" MaxLength="50"
                            ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="reqVLocationName" runat="server" ControlToValidate="txtLocationName"
                            CssClass="error" Display="Dynamic" ErrorMessage="Please enter location Name."
                            SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="reqLocationName" ControlToValidate="txtLocationName" Display="Dynamic"
                            CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                            ValidationGroup="AddUserValidationGroup" runat="server" />
                    </li>
                </ul>
                <ul>
                    <li class="text">
                        <asp:Label ID="lblLocationCode" runat="server" AssociatedControlID="txtLocationCode"
                            CssClass="formtext">Location Code:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtLocationCode" runat="server" CssClass="formfields" MaxLength="20"
                            ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="reqLocationCode" runat="server" ControlToValidate="txtLocationCode"
                            CssClass="error" Display="Dynamic" ErrorMessage="Please enter location code."
                            SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regLocationCode" ControlToValidate="txtLocationCode" Display="Dynamic"
                            CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                            ValidationGroup="AddUserValidationGroup" runat="server" />
                    </li>
                     <li class="text">
                         <asp:Label ID="lblState" runat="server" 
                            CssClass="formtext">State:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                         <asp:DropDownList ID="ddlState" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"></asp:DropDownList>
                        <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlState" CssClass="error"
                                Display="Dynamic" InitialValue="0" ErrorMessage="Please select State."
                                SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                    </li>

                   <li class="text">
                         <asp:Label ID="Label1" runat="server" 
                            CssClass="formtext">City:<span class="error">*</span></asp:Label>
                    </li>
                    <li class="field">
                         <asp:DropDownList ID="ddlCity" runat="server" CssClass="formselect"></asp:DropDownList>
                        <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlCity" CssClass="error"
                                Display="Dynamic" InitialValue="0" ErrorMessage="Please select City."
                                SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                    </li>
                    <li class="text">
                        <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status :</asp:Label>
                    </li>
                    <li class="field">
                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                    </li>
                        </ul>
                <div class="setbbb">
                        <div class="float-margin">
                            <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="true" ValidationGroup="AddUserValidationGroup"
                                ToolTip="Add Location" CssClass="buttonbg" OnClick="btnCreate_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                OnClick="btnCancel_Click" />
                        </div>
                    </div>
            
            </div>
        </div>
        <%-- </ContentTemplate>--%>
        <%--#CC01 Add Start --%>
        <%--<Triggers>
                <asp:PostBackTrigger ControlID="btnCreate" />
            </Triggers>--%>
        <%--#CC01 Add End --%>
        <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
        <%--</asp:UpdatePanel>--%>

           <%--#CC07 Added Started--%>
        <div class="mainheading">
            If you want to save/update data in bulk then use below interface.
        </div>
        <div class="clear"></div>
        <div class="mainheading">
            Step 1 : Click On Button For Save Or Update Process
        </div>
        <div class="contentbox">
            <div class="H25-C3-S">

                <ul>
                    <li class="link">
                        <asp:RadioButtonList ID="Rbtdownloadtemplate" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="radio-rs" CellPadding="2" CellSpacing="0" OnSelectedIndexChanged="Rbtdownloadtemplate_SelectedIndexChanged">
                            <asp:ListItem Value="1" Text="Save" Selected="True">
                            
                            </asp:ListItem>
                            <asp:ListItem Value="2" Text="Update">
                            
                            </asp:ListItem>
                        </asp:RadioButtonList>
                    </li>


                </ul>
            </div>
        </div>
        <div class="mainheading" runat="server" id="ForSaveTemplateheading" visible="false">
            Step 2 : Download  Template For Save Record 
        </div>
        <div class="contentbox" runat="server" id="ForSaveTemplatedownload" visible="false">
            <div class="H25-C3-S">

                <ul>
                    <li class="link">
                        <asp:LinkButton ID="DwnldTemplate" runat="server" Text="Download Template File"
                            CssClass="elink2" OnClick="DwnldTemplate_Click"></asp:LinkButton>
                    </li>


                </ul>
            </div>
        </div>
        <div class="mainheading" runat="server" id="ForUploadTemplateheading" visible="false">
            Step 2 : Download  Template For  Update Record  
        </div>
        <div class="contentbox" runat="server" id="ForUpdateTemplatedownload" visible="false">
            <div class="H25-C3-S">

                <ul>
                    <li class="link">
                        <asp:LinkButton ID="UpdateTemplateFile" runat="server" Text="Download Template File"
                            CssClass="elink2" OnClick="UpdateTemplateFile_Click"></asp:LinkButton>
                    </li>

                    <li class="link">
                        Note : In ActionType column  enter Inactive or can be blank , if not enter anything that means you modified records else inactive this user.  
                        </li>


                </ul>
            </div>
        </div>
        <div class="mainheading" runat="server" id="ReferenceIdForsaveheading" visible="false">
            Step 3 : Download Reference Code For Save Record 
        </div>
        <div class="contentbox" runat="server" id="ReferenceIdForsave" visible="false">
            <div class="H25-C3-S">
                <ul>
                    <li class="link">
                        <asp:LinkButton ID="DownloadReferenceCodeForSave" runat="server" Text="Download Reference Code"
                            CssClass="elink2" OnClick="DownloadReferenceCodeForSave_Click"></asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
        <div class="mainheading" runat="server" id="ReferenceIdForupdateheading" visible="false">
        Step 3 : Download Reference Code For Update Record 
    </div>
    <div class="contentbox" runat="server" id="ReferenceIdForupdate" visible="false">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DownloadReferenceCodeForUpdate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DownloadReferenceCodeForUpdate_Click"></asp:LinkButton>
                </li>
                
            </ul>
        </div>
    </div>
        
        <div class="mainheading">
            Step 4 : Upload Excel File
        </div>
        <div class="contentbox">
            <div class="H25-C3-S">

                <ul>
                    <li class="text">Upload File: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                        <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                    </li>
                    <li class="field3">
                        <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" 
                            OnClick="btnUpload_Click" />
                    </li>
                    <li class="link">
                        <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                    </li>
                </ul>
            </div>
        </div>
         <%--#CC07 Added End--%>
        <div class="mainheading">
            Search
        </div>
        <div class="contentbox">
            <%-- <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
            <div class="H20-C3-S">
                <ul>
                    <li class="text">Hierarchy Level:
                    </li>
                    <li class="field">
                        <asp:DropDownList CausesValidation="true" ID="ddlSerHierarchyLevel" runat="server"
                            CssClass="formselect" AutoPostBack="false">
                        </asp:DropDownList>
                    </li>
                    <li class="text">Location Name:
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtLocationNameSearch" runat="server" MaxLength="100" CssClass="formfields">
                        </asp:TextBox>
                    </li>
                    <li class="text">Location Code:
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtLocationCodeSearch" runat="server" CssClass="formfields" MaxLength="100">
                        </asp:TextBox>
                    </li>
                </ul>
                <ul>
                    <%-- #CC03 Comment Start <td align="right" valign="top" width="12%" height="35" class="formtext">Parent Hierarchy Level:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                     <div style="float: left; width: 135px;">
                                                                        <asp:DropDownList CausesValidation="true" ID="ddlSerParentHierarchyLevel" runat="server"
                                                                            CssClass="form_select4" AutoPostBack="false">
                                                                        </asp:DropDownList>  
                                                                    <br />
                                                                    </div>
                                                                </td>#CC03 Comment End --%>
                    <%--#CC03 Add Start--%>
                    <li class="text">Parent Code:
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtParentCode" CssClass="formfields" runat="server" MaxLength="20"></asp:TextBox>
                    </li>
                    <%--#CC03 Add End--%>
                    <li class="text">Parent Location Name:
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtSerParentLocationName" runat="server" MaxLength="100" CssClass="formfields">
                        </asp:TextBox>
                    </li>
                    <%--#CC03 Comment Start
                                                                <td align="right" valign="top" width="15%" height="25" class="formtext">--%>
                    <%--Hierarchy Level--%>
                    <%--   <asp:Button ID="Button1" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                                                        OnClick="btnSearch_Click"></asp:Button>
                                                                </td>
                                                               <td align="left" valign="top" width="20%">
                                                                    <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" OnClick="btnShow_Click"
                                                                        Text="Show All" ToolTip="Search" Width="60px" />
                                                                </td>
                                                                #CC03 Comment End--%>
                    <%--#CC03 Add Start--%>
                    <li class="text">User Name:
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtUserName" CssClass="formfields" runat="server" MaxLength="100"></asp:TextBox>
                    </li>
                </ul>
              <div class="setbbb">
                        <%--Hierarchy Level--%>
                        <div class="float-margin">
                            <asp:Button ID="Button1" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                OnClick="btnSearch_Click"></asp:Button>
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" OnClick="btnShow_Click"
                                Text="Show All" ToolTip="Search" />
                        </div>
                    
                    <%--#CC03 Add End--%>
                </div>
            </div>
            <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
        <div class="mainheading">
            Location List
        </div>
        <div class="export">
            <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
                OnClick="btnExprtToExcel_Click" />
        </div>
        <div class="contentbox">
            <%--<asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>--%>
            <%--#CC03 Add Start--%>

            <%--#CC03 Add End--%>
            <div class="grid1">
                <asp:GridView ID="grdvLocationList" runat="server" FooterStyle-VerticalAlign="Top"
                    FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                    HeaderStyle-HorizontalAlign="left" EmptyDataText="No Record found" HeaderStyle-VerticalAlign="top"
                    GridLines="none" AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow"
                    FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" CellSpacing="1"
                    PageSize='<%$ AppSettings:GridViewPageSize %>' CellPadding="4" bgcolor="" BorderWidth="0px"
                    Width="100%" AutoGenerateColumns="false" AllowPaging="True" SelectedStyle-CssClass="gridselected"
                    DataKeyNames="OrgnhierarchyID"
                    OnRowDataBound="grdvLocationList_RowDataBound">
                    <%--#CC02  OnPageIndexChanging removed--%>
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                    <Columns>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left"
                            DataField="HierarchyLevelName" HeaderText="Hierarchy Level"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LocationName"
                            HeaderText="Location Name"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LocationCode"
                            HeaderText="Location Code"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LocationUsername"
                            HeaderText="User Name"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left"
                            DataField="ParentLocationName" HeaderText="Parent Location"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left"
                            DataField="ParentLocationUsername" HeaderText="Parent User Name"></asp:BoundField>

                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                            <ItemStyle Wrap="False"></ItemStyle>
                            <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%#Eval("OrgnhierarchyID") %>' runat="server" ID="btnEdit"
                                    ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' OnClick="btnEdit_Click"
                                    ToolTip="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                            <ItemStyle Wrap="False"></ItemStyle>
                            <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnActiveDeactive"
                                    runat="server" CommandArgument='<%#Eval("OrgnhierarchyID") %>' CommandName='<%#Eval("Status")%>'
                                    OnClick="btnActiveDeactive_Click" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                    ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
                <%--#CC03 Add Start--%>
            </div>
            <div class="clear">
            </div>
            <div id="dvFooter" runat="server" class="pagination">
                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
            </div>

            <%--#CC03 Add End--%>
            <%-- </ContentTemplate>--%>
            <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="grdvLocationList" EventName="DataBound" />--%>
            <%--#CC03 Add Start --%>
            <%-- <asp:PostBackTrigger ControlID="btnShow" />
                    <asp:PostBackTrigger ControlID="Button1" />--%>
            <%--#CC03 Add End --%>

            <%-- </Triggers>--%>
            <%-- </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>
