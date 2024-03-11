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

public partial class Transactions_Common_AccountStatementRequest : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ucMsg.Visible = false;
                if (ViewState["EditUserId"] != null)
                { ViewState.Remove("EditUserId"); }

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);

        }

    }
    protected void btnCreateUser_Click(object sender, EventArgs e)
    {
        try
        {
            using (AccountStatementRequest obj = new AccountStatementRequest())
            {
                obj.UserId = PageBase.UserId;
                obj.FromDate = Convert.ToDateTime(ucFromDate.Date);
                obj.Todate = Convert.ToDateTime(ucToDate.Date);
                if (Convert.ToDateTime(ucFromDate.Date) > Convert.ToDateTime(ucToDate.Date))
                {
                    ucMsg.ShowError("To Date Must be Greater Then From Date.");
                    return;
                }
                Int16 Result = obj.SaveAccountStatementRequest();
                if (Result == 0)
                {
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

                }
                else if (Result == 2)
                {
                    ucMsg.ShowError("From Date Already Exist With User.");
                    return;
                }
                else if (Result == 1)
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearDate();

    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (ucSearchFromDate.Date == "" && ucSearchToDate.Date == "")
            {
                ucMsg.ShowError("Please Fill From Date and To Date.");
                return;
            }
            else
            {
                BindGridUserList();
            }
            //using (AccountStatementRequest obj = new AccountStatementRequest())
            //{
            //    obj.UserId = PageBase.UserId;
            //    obj.FromDate =Convert.ToDateTime(ucSearchFromDate.Date);
            //    obj.Todate = Convert.ToDateTime(ucSearchToDate.Date);

            //    dt=obj.SelectAllAccountRequestStatement();

            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        grdvwUserList.DataSource = dt;
            //        grdvwUserList.DataBind();
            //    }
            //    else
            //    {
            //        grdvwUserList.DataSource = null;
            //        grdvwUserList.DataBind();

            //    }
            //}
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message);
        }

    }

    private void ClearDate()
    {
        ucFromDate.Date = "";
        ucToDate.Date = "";

    }
    protected void grdvwUserList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string pagename = "";
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string IsPrint = grdvwUserList.DataKeys[e.Row.RowIndex]["RequestStatus"].ToString();
                Button BtnPrint = (Button)e.Row.FindControl("BtnPrint");
                string strAccountStatementReqId = Convert.ToString(Cryptography.Crypto.Encrypt(grdvwUserList.DataKeys[e.Row.RowIndex]["AccountStatementReqId"].ToString().Replace(" ", "+"), "Irf"));
                if (IsPrint == "Response received from Busy")
                {
                    pagename = "AccountStatementPrint.aspx";
                    grdvwUserList.Columns[3].Visible = true;
                    BtnPrint.Visible = true;
                    BtnPrint.Attributes.Add("OnClick", "return Popup('" + strAccountStatementReqId + "','" + pagename + "');");
                }
                else
                {
                    BtnPrint.Visible = false;
                    BtnPrint.Attributes.Add("OnClick", "return Popup('" + strAccountStatementReqId + "','" + pagename + "');");
                }

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
    }
    protected void grdvwUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvwUserList.PageIndex = e.NewPageIndex;
        BindGridUserList();
    }
    private DataTable BindGridUserList()
    {
        DataTable dt = new DataTable();
        if (ucSearchFromDate.Date == "" && ucSearchToDate.Date == "")
        {
            ;
        }
        else
        {
            using (AccountStatementRequest obj = new AccountStatementRequest())
            {
                obj.UserId = PageBase.UserId;
                obj.FromDate = Convert.ToDateTime(ucSearchFromDate.Date);
                obj.Todate = Convert.ToDateTime(ucSearchToDate.Date);

                dt = obj.SelectAllAccountRequestStatement();

                if (dt != null && dt.Rows.Count > 0)
                {
                    grdvwUserList.DataSource = dt;
                    grdvwUserList.DataBind();
                   
                }
                else
                {
                    grdvwUserList.DataSource = null;
                    grdvwUserList.DataBind();

                }
            }
        }
        return dt;
    }
}
