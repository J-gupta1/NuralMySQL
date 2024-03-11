using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
/* ==============================================================================================================
 * Change Log
 * ==============================================================================================================
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 13-Feb-2018, Sumit Maurya, #CC01, Trigger Added to render complete page.
 * ==============================================================================================================
 */

public partial class Masters_HO_Admin_ManageFinacialCalenderMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uclblMessage.ShowControl = false;
        if (!IsPostBack)
        {
            ViewState["Search"] = null;
            ViewState["FinacialCalenderID"] = null;
            /* #CC01 Add Start */
            ucEndDate.MinRangeValue = DateTime.Now.Date;
            ucEndDate.RangeErrorMessage = "Date should be greater then or equal to current date.";
            /* #CC01 Add End */
           
            BindGrid();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateSave())
        {
            return;
        }
        using (FinacialCalender ObjCal = new FinacialCalender())
        {
            if (ViewState["FinacialCalenderID"] == null)
            {
                ObjCal.FinacialCalenderID = 0;
            }
            else
            {
                ObjCal.FinacialCalenderID = Convert.ToInt32(ViewState["FinacialCalenderID"]);
            }
            ObjCal.CalenderYear = txtCalenderYear.Text.Trim();
            ObjCal.QuarterName = txtQuarter.Text.Trim();
            ObjCal.FortnightStartDate = Convert.ToDateTime(ucStartDate.Date);
            ObjCal.FortnightEndDate = Convert.ToDateTime(ucEndDate.Date);
            ObjCal.FortnightName=txtFortnightName.Text.Trim();
            ObjCal.InsertCalender();
            if (ObjCal.Error != null && ObjCal.Error != "")
            {
                uclblMessage.ShowInfo(ObjCal.Error);
                return;
            }
            if (ViewState["FinacialCalenderID"] == null)
            {
                uclblMessage.ShowSuccess(Resources.Messages.CreateSuccessfull);
                
            }
            else
            {
                uclblMessage.ShowSuccess(Resources.Messages.EditSuccessfull);
            }
            ClearForm();
            BindGrid();
        }

       
    }
    void ClearForm()
    {
        txtQuarter.Text = "";
        txtCalenderYear.Text = "";
        ucStartDate.Date = "";
        ucEndDate.Date = "";
        txtFortnightName.Text = "";
        ViewState["FinacialCalenderID"] = null;
        btnSave.Text = "Submit";
        ViewState["Search"] = null;
      
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        uclblMessage.ShowControl = false;
        ClearForm();
    }
    bool ValidateSave()
    {
        if (txtCalenderYear.Text == "" || txtQuarter.Text == "" || ucStartDate.Date == "" || ucEndDate.Date == "")
        {
            uclblMessage.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        if (Convert.ToDateTime(ucStartDate.Date) >= Convert.ToDateTime(ucEndDate.Date))
        {
            uclblMessage.ShowInfo(Resources.Messages.InvalidDate);
            return false;
        }
        return true;
    }
    protected void GridCalender_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
        if (e.CommandName.ToLower() == "cmdedit")
        {
            btnSave.Text = "Update";
                uclblMessage.ShowControl = false;

            using (FinacialCalender ObjCal= new FinacialCalender())
            {
                ObjCal.FinacialCalenderID = Convert.ToInt32(e.CommandArgument);
               
                DataTable DtDetail = ObjCal.GetCalenderDetail();
               
                 ViewState["FinacialCalenderID"] = ObjCal.FinacialCalenderID;
                 txtCalenderYear.Text=DtDetail.Rows[0]["FinancialCalenderQuarterName"].ToString();
                 txtCalenderYear.Text=DtDetail.Rows[0]["FinancialCalenderYearName"].ToString();
                 ucStartDate.Date=DtDetail.Rows[0]["FinancialCalenderFortnightStartDate"].ToString();
                 ucEndDate.Date=DtDetail.Rows[0]["FinancialCalenderFortnightEndDate"].ToString();
                 txtFortnightName.Text=DtDetail.Rows[0]["FinancialCalenderFortnightName"].ToString();
                 txtQuarter.Text = DtDetail.Rows[0]["FinancialCalenderQuarterName"].ToString();
            }
        }
            if (e.CommandName.ToLower() =="cmddelete")
            {
                 using (FinacialCalender ObjCal= new FinacialCalender())
            {
                ObjCal.FinacialCalenderID = Convert.ToInt32(e.CommandArgument);
                ObjCal.DeleteCalenderInfo();
              if (ObjCal.Error != null && ObjCal.Error != "")
            {
                uclblMessage.ShowInfo(ObjCal.Error);
                return;
            }
              uclblMessage.ShowSuccess(Resources.Messages.Delete);
            }
                 BindGrid();
            }
        }
            
       
            catch (Exception ex)
            {
                uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
    }


    void BindGrid()
    {
         using (FinacialCalender ObjCal = new FinacialCalender())
        {
            ObjCal.CalenderYear = txtserchCalName.Text.Trim();
            ObjCal.FortnightName = txtserchFortnightName.Text.Trim();
            if (ucSearchStartDate.Date != "")
            {
                ObjCal.FortnightStartDate = Convert.ToDateTime(ucSearchStartDate.Date);
            }
            if (ucSearchEndDate.Date != "")
            {
                ObjCal.FortnightEndDate = Convert.ToDateTime(ucSearchEndDate.Date);

            }
          DataTable Dt=  ObjCal.GetCalenderDetail();
          if (Dt != null && Dt.Rows.Count > 0)
          {
              GridCalender.DataSource = Dt;
              GridCalender.DataBind();
          }
          else
          {
              if (ViewState["Search"] != null)
              {
                  uclblMessage.ShowInfo(Resources.Messages.NoRecord);
              }

              GridCalender.DataSource = null;
              GridCalender.DataBind();

          }
          ViewState["Search"] = null;
          UpdGrid.Update();
    }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["Search"] = "S";
        if (ServerValidation.IsDate(ucSearchStartDate.Date,false) != 0|| ServerValidation.IsDate(ucSearchEndDate.Date,false) != 0)
        {
            uclblMessage.ShowInfo(Resources.Messages.InvalidDateEntered);
            return;
        }
        if (ucSearchStartDate.Date != "" && ucSearchEndDate.Date != "")
        {
            if (Convert.ToDateTime(ucSearchStartDate.Date) > Convert.ToDateTime(ucSearchEndDate.Date))
            {
                uclblMessage.ShowInfo(Resources.Messages.InvalidDate);
                return;
            }
        }


        BindGrid();
        
    }

    protected void btnSReset_Click(object sender, EventArgs e)
    {
        ViewState["Search"] = null;
        uclblMessage.ShowControl = false;
        txtserchCalName.Text = "";
        txtserchFortnightName.Text = "";
        ucSearchStartDate.Date = "";
        ucSearchEndDate.Date = "";
        BindGrid();
    }
    protected void GridCalender_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridCalender.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}


