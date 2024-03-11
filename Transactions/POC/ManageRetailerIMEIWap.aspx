<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageRetailerIMEIWap.aspx.cs"
    Inherits="Transactions_POC_ManageRetailerIMEIWap" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
       body 
       {
       	margin:0px;
       	padding:0px;
       	}
        .error
        {
            color: Red;
            font-size: 12px;
            font-style: italic;
        }
        .hd
        {
          font-weight: bold;
          height:30px;
          font-size:14px;
        }
        .formtext
        {
            color: #000000;
        
        }
        .form_select
        {
            width: 160px;
            border: 1px solid #c3c3c3;
            color: #333;
            background: #fff;
            height:20px;
        }
        .form_input2      
        {
        	width: 155px;
            border: 1px solid #c3c3c3;
            color: #333;
            background: #fff;           
            height:16px;
            line-height:16px;
            }
        .main-table
        {
            font-family: Arial;
            font-size: 12px;
            padding: 20px;
            width:70%;
            float:left;
           
        }
        .buttonbg
        {
            padding: 1px 3px;
            background: #59c7e8 url( 'Images/buttonbg.jpg' ) repeat-x left top;
            color: #fff;
            font-size: 12px;
            font-weight: normal;
            white-space: inherit;
            border: 1px #276679 solid;
            outline: none;
            cursor: pointer;
            height: 22px;
            width: 65px;
        }
        .float-margin { float:left; margin-right:10px;}
        .form-bg 
        {
        	 background-color: #f9f9f9;
        	 padding:10px;
        	 }
      .logo  
      { height:45px;
        background-color:Black;
        padding:10px;
      	
      	}
      	.clear 
      	{
      		clear:both;
      		height:20px;
      		}
      .float-txt
      		{ float:left;
      		  margin-right:10px;
      		 width:20%;
      			
      			}
       </style>
</head>
<body>
    <form id="form2" runat="server">
    <div style="width:100%; background-color: #f4f4f4;">
    <div> <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager></div>
                
            <div class="logo">
                <img src="../../Assets/ZedSales/CSS/Images/sony-logo.png" />
            </div>
            
             <div class="main-table">
             <div class="error">
                                <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <uc1:ucMessage ID="ucMessage1" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
             </div>
                     <div class="form-bg">
                                <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                    
                                   <div class="hd">  Retailer IMEI</div>
                                           
                                                    <div>
                                                       
                                                        <div style="height:30px;">(<font class="error">*</font>) marked fields are mandatory.</div>
                                                       <div class="float-txt">
                                                      
                                                                    Retailer:<font class="error">*</font>
                                                              </div>
                                                         <div class="float-margin"> <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="form_select" ValidationGroup="Add">
                                                                    </asp:DropDownList>
                                                                    <br />
                                                                    <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="ddlRetailer"
                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select retailer." InitialValue="0"
                                                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator></div>
                                                          <div class="clear"></div>
                                                             
                                                           
                                                          <div class="float-txt">
                                                               
                                                                    Model Name:<font class="error">*</font>
                                                             </div>
                                                                <div class="float-margin"> 
                                                                 
                                                                        <asp:DropDownList ID="ddlModel" runat="server" ValidationGroup="Add" CssClass="form_select"
                                                                            AutoPostBack="true">
                                                                        </asp:DropDownList> <br />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlModel"
                                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please select Model." InitialValue="0"
                                                                            SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                                                     
                                                                
                                                                </div>
                                                                <div class="clear"></div>
                                                             <div class="float-txt">
                                                                    Date: <font class="error">*</font>
                                                                </div>
                                                                 <div class="float-margin"> 
                                                                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="false"
                                                                        ValidationGroup="Add" IsRequired="true" />
                                                                </div>
                                                                <div class="clear"></div>
                                                         
                                                            <div class="float-txt">
                                                                    IMEI Number:<font class="error">*</font>
                                                                </div>
                                                               <div class="float-margin"> 
                                                                    <asp:TextBox ID="txtIMEI" runat="server" CssClass="form_input2" MaxLength="15"></asp:TextBox>
                                                                    <br />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtIMEI"
                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please Insert IMEI." SetFocusOnError="true"
                                                                        ValidationGroup="Add"></asp:RequiredFieldValidator>
                                                                </div>
                                                                  <div class="clear"></div>
                                                           <div style="padding-left:21%;">
                                                                   <div class="float-margin"> <asp:Button ID="btnSubmit" runat="server" CausesValidation="true" CssClass="buttonbg"
                                                                        OnClick="btnSubmit_Click" Text="Submit" ValidationGroup="Add" /></div>
                                                                    
                                                                   <div class="float-margin">  <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" OnClick="btnCancel_Click"
                                                                        Text="Cancel" ToolTip="Cancel" /></div>
                                                             </div>
                                                              <div class="clear"></div>
                                                        
                                                    </div>
                                       
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                  
                      </div>
                    </div>
                
           <div style="float:left; padding-top:30px;">
     
                <img src="../../Assets/ZedSales/CSS/Images/xperia-nxt-series-colourful1.png" />
                </div>
        
    </div>
    </form>
</body>
</html>
