﻿/*
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 18-Jul-2016, Sumit Maurya, #CC01, pnlOutstandingAmount gets displayed according to Appconfig.
 
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
/* #CC01 Add Start */
using System.Data;
using DataAccess;
/* #CC01 Add Start */
public partial class _Default : PageBase
{


    protected void Page_Load(object sender, EventArgs e)
    {


        WelCss.Attributes.Add("href", strAssets + "/CSS/welcome.css?" + DateTime.Now.Ticks.ToString());
        bindAssets();
        if (Session["Count"] == null)
            showPasswordExpiry();
        else if (Session["Count"] == "2")
            Response.Redirect("ChangePassword.aspx");
        /* #CC01 Add Start */
        if (!IsPostBack)
            showTotalOutstandingAmount();
        /* #CC01 Add End */

    }
    void showPasswordExpiry()
    {
        if (PageBase.UserPasswordExpiredDate != null)
        {
            DateTime dt1 = Convert.ToDateTime(PageBase.UserPasswordExpiredDate);
            DateTime dt2 = System.DateTime.Now;
            TimeSpan ts = dt1 - dt2;
            int days = ts.Days;
            if (days <= 0)
            {
                Session["Count"] = "2";
                ModalPopupExtender.PopupControlID = "Panel2";
                ModalPopupExtender.OkControlID = "btnSubmit1";
                ModalPopupExtender.Show();


            }
            else if (days <= 3 && days > 0)
            {
                Session["Count"] = "1";
                ModalPopupExtender.PopupControlID = "Panel1";
                ModalPopupExtender.OkControlID = "btnSubmit";
                ModalPopupExtender.Show();
                lbldays.Text = days.ToString();
            }
        }
        else
        {
            Session["Count"] = "2";
            ModalPopupExtender.PopupControlID = "Panel2";
            ModalPopupExtender.OkControlID = "btnSubmit1";
            ModalPopupExtender.Show();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        dvBulletin.Visible = ucDisplayBoard1.isRepeaterHaveData;
        if (!string.IsNullOrEmpty(PageBase.ConfigKeysError))
        {
            Response.Write(PageBase.ConfigKeysError);
            Response.End();
        }

    }
    void bindAssets()
    {
        //welcomeleft.Src = strAssets + "/CSS/Images/welcomeleft.png";
        //welcomeright.Src = strAssets + "/CSS/Images/welcomeright.png";


    }
    protected void btnOkay_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChangePassword.aspx", false);

    }
    /* #CC01 Add Start */
    public void showTotalOutstandingAmount()
    {
        try
        {
            int intShowSalesChannenlOutstandingAmount = Convert.ToInt16(Resources.AppConfig.ShowSalesChannenlOutstandingAmount.ToString());
            if (intShowSalesChannenlOutstandingAmount == 1 && PageBase.SalesChanelTypeID == 7)
            {
                SalesChannelData objsales = new SalesChannelData();
                objsales.SalesChannelID = PageBase.SalesChanelID;
                DataSet dsResult = objsales.GetTotalOutstandingDetail();
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            pnlOutstandingAmount.Visible = true;
                            lblTotalOutstandingamt.Text = dsResult.Tables[0].Rows[0]["TotalPendingAmount"].ToString();
                            lblDate.Text = dsResult.Tables[0].Rows[0]["CreatedOn"].ToString();
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkOutstandingDownload_Click(object sender, EventArgs e)
    {
        try
        {
            SalesChannelData objsales = new SalesChannelData();
            objsales.SalesChannelID = PageBase.SalesChanelID;
            objsales.OutstandingDetailDownload = 1;
            DataSet dsResult = objsales.GetTotalOutstandingDetail();
            DataSet dsDownload = new DataSet();
            DataTable dtclone = new DataTable();
            dtclone = dsResult.Tables[1].Copy();

            dsDownload.Tables.Add(dtclone);

            if (dsDownload != null)
            {
                if (dsDownload.Tables.Count > 0)
                {
                    if (dsDownload.Tables[0].Rows.Count > 0)
                    {
                        String FilePath = Server.MapPath("");
                        string FilenameToexport = "Outstanding Amount";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dsDownload, FilenameToexport, EnumData.eTemplateCount.eNothing);
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }


    }
    /* #CC01 Add End */
}
