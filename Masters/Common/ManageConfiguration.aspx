<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ManageConfiguration.aspx.cs" Inherits="MastersManageConfiguration" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updpnlSaveData" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="box1">
                <asp:UpdatePanel ID="UpdateMessage" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div>
                            <uc2:ucMessage ID="ucMessage1" runat="server" />

                        </div>
                        <div class="subheading">
                            <div class="float-left">
                                Application Configuration
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="innerarea">
                    <div class="H25-C3-S">

                        <ul>
                            <li class="text">Licence User count:<span class="error">*</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtMaxUsers" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMaxUsers"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Enter." ForeColor=""
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                                
                            </li>
                            <li class="text">Password Expiry Days:<span class="error">*</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtPWDEXPY" runat="server" CssClass="formfields" MaxLength="5"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqfldtxtEntityType" runat="server" ControlToValidate="txtPWDEXPY"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Enter." ForeColor=""
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                                
                            </li>
                            <li class="text">Failed Password count To Lock User:<span class="error">*</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtFailPasswordCount" runat="server" CssClass="formfields" MaxLength="5"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFailPasswordCount"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Enter." ForeColor=""
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                                
                            </li>
                            <li class="text">Min Serial Length:</li>
                            <li class="field">
                                <asp:TextBox ID="txtMinSerailLength" runat="server" CssClass="formfields" MaxLength="5"></asp:TextBox>

                            </li>
                            <li class="text">Max Serial Length:</li>
                            <li class="field">
                                <asp:TextBox ID="txtMaxSerailLength" runat="server" CssClass="formfields" MaxLength="5"></asp:TextBox>

                            </li>
                            <li class="text">Min Batch Length:</li>
                            <li class="field">
                                <asp:TextBox ID="txtMinBatchLength" runat="server" CssClass="formfields" MaxLength="5"></asp:TextBox>

                            </li>
                            <li class="text">Max Batch Length:</li>
                            <li class="field">
                                <asp:TextBox ID="txtMaxBatchLength" runat="server" CssClass="formfields" MaxLength="5"></asp:TextBox>
                            </li>
                            <li class="text">Sales Channel Approval Levels:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSalesChannelApprovalLevels" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>

                            <li class="text">Retailer Approval:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlRetailerApprovalLevels" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                
                            </li>
                            <li class="text">Retailer Unique Mobile:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlRetailerUniqueMobile" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                
                            </li>
                            <li class="text">ISP(ISD/CSA) Unique Mobile:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlISPUniqueMobile" runat="server" CssClass="formselect">
                                </asp:DropDownList>


                            </li>
                            <li class="text">Secondary Sale Return Parent Check:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSecSaleReturnParentCheck" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                            </li>
                            <li class="text">Secondary SalesReturn Approval:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSecondarySalesReturnApproval" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            
                            <li class="text">Intermediary Sales Return Approval:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlIntermediarySalesReturnApproval" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">ON CSA/ISD Sale Stock Out:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlISDSaleStockOutUsingAPI" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                            </li>
                            <li class="text">Retailer APK AUTO Receive Days:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtRETAPPAUTORECDAYS" runat="server" CssClass="formfields" MaxLength="5"></asp:TextBox>
                            </li>
                            <li class="text">Upload Date Format:
                            </li>
                            <li class="field">
                               <asp:DropDownList ID="ddlUploadDateFormat" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">BeatPlanAutoApprove:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlBeatPlanAutoApprove" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Physical Stock Auto Stock Adjustment:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlPhysicalStockAutoStockAdjustment" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                
                            </li>
                            <li class="text">Physical Stock Adjustment Reason:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlAPIStockAdjustmentResonID" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <%--<div>
                                    <asp:RequiredFieldValidator ID="reqfldddlBrandCategoryMappingMode" runat="server"
                                        ControlToValidate="ddlAPIStockAdjustmentResonID" CssClass="error" Display="Dynamic"
                                        ErrorMessage="Please Select" InitialValue="0" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                                </div>--%>
                            </li>
                            <li class="text">Number of Top Sales Channel for Dashboard:</li>
                            <li class="field">
                                <asp:TextBox ID="txtTopSalesChannel" runat="server" CssClass="formfields" MaxLength="5"></asp:TextBox>
                            </li>


                            <li class="field"></li>
                             <li class="text">Number of Top Retailer for Dashboard:</li>
                            <li class="field">
                                <asp:TextBox ID="txtTopRetailer" runat="server" CssClass="formfields" MaxLength="5"></asp:TextBox>
                            </li>
                            <%--<li class="text">Travel Claim Process Type:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlTravelClaimProcessType" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                
                            </li>--%>
                            <li class="text">Back Date Expense:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlBackDateExpense" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <%--<div>
                                    <asp:RequiredFieldValidator ID="reqfldWeeklyOffMode" runat="server" ControlToValidate="ddlBackDateExpense"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Select Weekly Off Mode." InitialValue="100"
                                        ValidationGroup="grpv"></asp:RequiredFieldValidator>
                                </div>--%>
                            </li>
                            
                        </ul>
                        
                        <ul>
                            <li class="field">
                                <div class="float-margin">

                                    <asp:Button ID="btnSave" runat="server" CssClass="buttonbg" Text="Save" OnClick="btnSave_Click" ValidationGroup="grpv" />
                                </div>
                                
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
   
</asp:Content>
