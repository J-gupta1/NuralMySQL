#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Vijay Kumar Prajapati
* Created On: 01-Aug-2017 
 * Description: This is a copy of Salesdata from DataAccess.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
*/
#endregion
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Text;

public partial class Transactions_Common_AccountStatementPrint : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                FillDataForPrintAccountStatement();
            }
        }
        catch (Exception ex)
        {

            lblmessage.Text=ex.Message.ToString();
        }
       
    }
    private void FillDataForPrintAccountStatement()
    {
        int intAccountStatementReqId = 0;
        try
        {
            if (Request.QueryString["AccountStatementReqId"] != null)
            {
                string strAccountStatementReqId = Cryptography.Crypto.Decrypt(Request.QueryString["AccountStatementReqId"].ToString().Replace(" ", "+"), "Irf");
                 intAccountStatementReqId =Convert.ToInt32(strAccountStatementReqId);
            }
             DataSet ds = new DataSet();
             AccountStatementRequest objRequest = new AccountStatementRequest();
             objRequest.AccountStatementReqId =intAccountStatementReqId;
             ds = objRequest.GetAccountRequestStatementPrintDetails();

            if(ds.Tables[0].Rows.Count>0)
            {
                LabelDisplayShow();
                lblsalesChannelName.Text = ds.Tables[0].Rows[0]["SalesChannelName"].ToString();
                lblCin.Text = "U51909DL2017PTC318494";
                lblFromdate.Text =DateTime.Parse(ds.Tables[0].Rows[0]["FromDate"].ToString()).ToString("dd/MM/yyyy");
                lbltodate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["ToDate"].ToString()).ToString("dd/MM/yyyy");
                lbltaxIndiaServices.Text = "Tax India Services";
            }
            else
            {
                lblsalesChannelName.Text = string.Empty;
                lblCin.Text = string.Empty;
                lblFromdate.Text = string.Empty;
                lbltodate.Text = "";
                lbltaxIndiaServices.Text = string.Empty;
                lblmessage.Text = "Record Not Found";

            }
            if(ds.Tables[1].Rows.Count>0)
            {
                LabelDisplayShow();
                totaldebit.Text = ds.Tables[1].Compute("SUM(DebitAmount)","").ToString();
                totalcreadit.Text = ds.Tables[1].Compute("SUM(CreditAmount)","").ToString();
                gvaccountdetails.DataSource = ds.Tables[1];
                gvaccountdetails.DataBind();
            }
            else
            {
                totalcreadit.Text = string.Empty;
                totalcreadit.Text = string.Empty;

                gvaccountdetails.DataSource = null;
                gvaccountdetails.DataBind();
            }

        }
        catch (Exception ex)
        {
            
            throw new Exception(ex.Message);
        }
    }
    private void LabelDisplayShow()
    {
        lblheadingCin.Visible = true;
        lblheadingAccount.Visible = true;
        lblheadingCin.Visible = true;
        lblheadingFrom.Visible = true;
        lblheadingLedger.Visible = true;
        lblheadingTO.Visible = true;
        lblheadingTotalCredit.Visible = true;
        lblheadingTotalDebit.Visible = true;
        lblsalesChannelName.Visible = true;
        lbltaxIndiaServices.Visible = true;
        lbltodate.Visible = true;
        lblCin.Visible = true;
        lblFromdate.Visible = true;
        lblheadingLedger.Visible = true;
        gvaccountdetails.Visible = true;
        totalcreadit.Visible = true;
        totaldebit.Visible = true;


    }
}