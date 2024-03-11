<%--====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          ------------------------------------------------------------- 
03-Aug-2016         Kalpana              #CC01: CssClass added
 ====================================================================================================--%>

<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="DirectSalesUpload.aspx.cs" Inherits="Order_Common_DirectSalesUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc8" %>
<%@ Register Src="~/UserControls/ucEntityList.ascx" TagName="ucEntityList" TagPrefix="uc9" %>

<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc6" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="box1">
        <div class="subheading">
            <div class="float-left">
                Order
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uc8:ucMessage ID="ucMessage1" runat="server" Style="display: none" />
                    <%--  <uc8:ucMessage ID="ucMessage2" runat="server" XmlErrorSource="" />
                    <uc8:ucMessage ID="ucMessage3" runat="server" Visible="true" />--%>
                    <div class="clear">
                    </div>
                    <div class="success-msg" id="divMsg" runat="server" style="display: none;">
                        <span class="float-margin">
                            <asp:Label ID="lblMessageSuccess" runat="server"></asp:Label></span> <span class="float-left">
                                <asp:LinkButton ID="hlkFinal" runat="server"></asp:LinkButton></span>
                    </div>
                    <div class="clear">
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="innerarea" id="PnlOrder" runat="server"><%--#CC05: class changed--%>
            <div class="H30-C3-S"><%--#CC05: class changed--%>

                <asp:Panel ID="pmlCmbControls" runat="server">
                    <asp:Label ID="Button1" Text="0" runat="server" Style="display: none"></asp:Label>
                    <ul>
                        <li class="text">Sales From:<span class="mandatory-img">&nbsp;</span></li>
                        <li class="field"><%--#CC05: class changed--%>
                            <uc9:ucEntityList ID="cmbOrderFrom" runat="server" ValidationGroup="upload"
                                SetInitialValue="Please Select." InitialValue="Please Select." />
                        </li>
                        <li class="text">Issue Stock Mode: <span class="mandatory-img">&nbsp;</span></li><%--#CC05: class changed--%> <%--#CC06: Added Issue--%>
                        <li class="field"><%--#CC05: class changed--%>
                            <div>
                                <asp:DropDownList ID="ddlStockMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="reqddlStockMode" ControlToValidate="ddlStockMode"
                                    InitialValue="0" ValidationGroup="upload" runat="server" CssClass="error" ErrorMessage="Please select Stock Mode."></asp:RequiredFieldValidator><%--#CC05: class changed--%>
                            </div>
                        </li>
                        <%--#CC06: Added Start--%>
                        <li class="text">Receive Stock Mode: <span class="mandatory-img">&nbsp;</span></li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlReceiveStockMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="ReqddlReceiveStockMode" ControlToValidate="ddlReceiveStockMode"
                                    InitialValue="0" ValidationGroup="upload" runat="server" CssClass="error" ErrorMessage="Please select Stock Mode."></asp:RequiredFieldValidator>
                            </div>
                        </li>

                        <%--#CC06: Added End--%>
                        <li class="text">Ref Number:<span class="mandatory-img">&nbsp;</span></li>
                        <li class="field"><%--#CC05: class changed--%>
                            <div>
                                <asp:TextBox ID="txtPoNumber" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                            </div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPoNumber"
                                CssClass="error" Display="Dynamic" ValidationExpression="[^&lt;&gt;/\@%()]{1,50}"
                                ValidationGroup="upload"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPoNumber"
                                CssClass="error" Display="Dynamic" ForeColor="" ValidationGroup="upload" ErrorMessage="Please enter ref no."></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Remarks:</li><%--#CC05:  style="height: 45px;" removed--%>
                        <li class="field"><%--#CC05: class changed and style="height: 45px;" removed --%>
                            <uc6:ucTextboxMultiline ID="txtRemarks" runat="server" CharsLength="250" TextBoxWatermarkText="Enter Order Remarks" />
                        </li>
                    </ul>
                </asp:Panel>
                <asp:UpdatePanel ID="UpdExcel" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <ul>
                            <li class="text">Upload Excel:<span class="mandatory-img">&nbsp;</span></li><%--#CC05: class changed--%>
                            <li class="field"> <%--#CC05: class changed and removed style="width: 300px; height: 45px;"--%>
                                <asp:FileUpload ID="uploadExcel" runat="server" CssClass="fileuploads" />
                                <div>
                                    <asp:RequiredFieldValidator ID="requploadExcell" runat="server" ValidationGroup="upload"
                                        ControlToValidate="uploadExcel" CssClass="error" Display="Dynamic" ErrorMessage="Please select an excel file!"
                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text"><%--#CC05: class changed and removed style="height: 45px;"--%>
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button1"
                                    ValidationGroup="upload"  OnClick="btnUpload_Click" />
                                <%--<cc2:ZedButton ID="btnUpload" runat="server" Text="Upload" CssClass="button1"
                                    ValidationGroup="upload" ActionTag="Add" OnClick="btnUpload_Click" />--%>
                            </li>
                            <li>
                                <div class="float-margin">
                                    <%-- <asp:HyperLink ID="hplinkDownloadTemplate" runat="server" CssClass="link1" Text="Download Template"></asp:HyperLink>  #CC03 Commented --%><%--#CC01--%>
                                    <%--<asp:LinkButton ID="lnkDownloadTemplate" runat="server" CssClass="link1" OnClick="lnkDownloadTemplate_Click" Text="Download Template" CausesValidation="false"></asp:LinkButton>--%>
                                    <a class="elink2" href="../../Excel/Templates/DirectSalesTemplate.xlsx">
                                                Download Template</a>
                                    <%--#CC03 Added--%>
                                </div>
                            </li>
                            <%--#CC04 Commented start--%>
                            <%--<li>
                                <div class="float-margin">
                                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="link1" Visible="true" ForeColor="Red"></asp:HyperLink>
                                </div>
                            </li>
                            <li>
                                <div class="float-margin">
                                    <asp:HyperLink ID="hlnkDuplicate" runat="server" CssClass="link1"></asp:HyperLink>
                                </div>
                            </li>
                            <li>
                                <div class="float-margin">
                                    <asp:HyperLink ID="hlnkBlank" runat="server" CssClass="link1"></asp:HyperLink>
                                </div>
                            </li>--%>
                            <%--#CC04 Commented end--%>
                            <%--#CC04 Added start--%>
                            <li>
                                <div class="float-margin">
                                    <asp:LinkButton ID="hlnkInvalid" runat="server" CssClass="linkError" CausesValidation="false" OnClick="hlnkInvalid_Click" ></asp:LinkButton>
                                </div>
                            </li>
                            <li>
                                <div class="float-margin">
                                    <asp:LinkButton ID="hlnkDuplicate" runat="server" CssClass="linkError"  CausesValidation="false" OnClick="hlnkDuplicate_Click" ></asp:LinkButton>
                                </div>
                            </li>
                            <li>
                                <div class="float-margin">
                                    <asp:LinkButton ID="hlnkBlank" runat="server" CssClass="linkError"  CausesValidation="false" OnClick="hlnkBlank_Click" ></asp:LinkButton>
                                </div>
                            </li>
                            <%--#CC04 Added end--%>
                        </ul>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUpload" />
                         <%--<asp:PostBackTrigger ControlID="lnkDownloadTemplate" /> #CC03 Added--%>
                        <%--#CC04 Added start--%>
                        <asp:PostBackTrigger ControlID="hlnkInvalid" />
                        <asp:PostBackTrigger ControlID="hlnkDuplicate" />
                        <asp:PostBackTrigger ControlID="hlnkBlank" />
                        <%--#CC04 Added end--%>
                    </Triggers>
                </asp:UpdatePanel>
                <ul>
                    <li>
                        <div class="float-margin">
                            <cc2:ZedLinkButton ID="DownloadSalesToEntityCode" runat="server" CssClass="link1" Text="Download SalesTo EntityCode"
                                OnClick="DownloadSalesToEntityCode_Click" ActionTag="Export" CausesValidation="false"></cc2:ZedLinkButton>
                        </div>
                    </li>
                </ul>

            </div>
        </div>
    </div>

</asp:Content>
