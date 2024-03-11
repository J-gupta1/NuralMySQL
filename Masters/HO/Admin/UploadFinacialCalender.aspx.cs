using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;

public partial class Masters_HO_Admin_ManageFinacialCalender : PageBase
{
      #region "Page Level Variables"
        string strUploadedFileName = string.Empty;
        UploadFile UploadFile = new UploadFile();
        #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
       

        uclblMessage.ShowControl = false;
        if (!IsPostBack)
        {
            ViewState["Detail"] = null;
        }
    }
  
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucEndDate.Date == "")
            {
                uclblMessage.ShowInfo("Enter year end date");
                return;
            }
            ViewState["Detail"] = null;
            DataSet objDS = new DataSet();
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../../");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
               UploadFile.UploadedFileName = strUploadedFileName;
                   
                    isSuccess = UploadFile.uploadValidExcel(ref objDS,"FinacialCalender");
                    switch (isSuccess)
                    {
                        case 0:
                            uclblMessage.ShowWarning(UploadFile.Message);
                            break;
                        case 2:
                            uclblMessage.ShowWarning(Resources.Messages.CheckErrorGrid);
                            pnlGrid.Visible = true;

                            GridCalender .DataSource = objDS;
                            GridCalender.DataBind();
                          
                            break;
                        case 1:
                           
                            InsertData(objDS);

                            break;
                    }

                }
                else if (UploadCheck == 2)
                {
                    uclblMessage.ShowError(Resources.Messages.UploadXlxs);
                }
                else if (UploadCheck == 3)
                {
                    uclblMessage.ShowError(Resources.Messages.SelectFile);
                }
                else
                {
                    uclblMessage.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                }
            }
        
        catch (Exception ex)
        {
            uclblMessage.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
  private void InsertData(DataSet objDS)
        {
            pnlGrid.Visible = true;
            if (objDS.Tables[0].Columns.Contains("Error") == false)
                objDS.Tables[0].Columns.Add("Error");
           
            int ErrorCount = 0;

            for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
            {
                if (objDS.Tables[0].Rows.Count-1 != i)
                {
                    if (Convert.ToDateTime(objDS.Tables[0].Rows[i]["StartDate"]) > Convert.ToDateTime(objDS.Tables[0].Rows[i + 1]["StartDate"]))
                    {
                        if (objDS.Tables[0].Rows[i]["Error"] == DBNull.Value && objDS.Tables[0].Rows[i]["Error"].ToString () == "")
                            objDS.Tables[0].Rows[i]["Error"] = "Start date should be less than start date of next fortnight";
                        else
                        objDS.Tables[0].Rows[i]["Error"] += "; Start date should be less than start date of next fortnight";
                        ErrorCount = ErrorCount + 1;
                    }
                    
                }
                if (objDS.Tables[0].Rows.Count - 1 == i)
                {
                    if (Convert.ToDateTime(objDS.Tables[0].Rows[i]["StartDate"])>Convert.ToDateTime(ucEndDate.Date))
                    {
                           if (objDS.Tables[0].Rows[i]["Error"] == DBNull.Value && objDS.Tables[0].Rows[i]["Error"].ToString () == "")
                            objDS.Tables[0].Rows[i]["Error"] = "Start date should be less than year end date";
                           else
                           objDS.Tables[0].Rows[i]["Error"] += "; Start date should be less than year end date";
                        ErrorCount = ErrorCount + 1;
                        
                    }
                }
            }

                GridCalender.DataSource = objDS;
                if (ErrorCount == 0)
                {
                    GridCalender.Columns[3].Visible = false;
                    btnSave.Visible = true ;
                    btnReset.Visible = true;
                }
                else
                {
                    GridCalender.Columns[3].Visible = true ;
                    btnSave.Visible = false;
                    btnReset.Visible = false;
                }
            ViewState["Detail"] = objDS.Tables[0];
            GridCalender.DataBind();
            
        }
    protected void btnSave_Click(object sender, EventArgs e)
  {
      if (IsPageRefereshed == true)
      {
          return;
      }
      if (!pagevalidate())
      {
          return;
      }
      try
      {
          if (ViewState["Detail"] != null)
          {

              using (TargetData ObjRTarget = new TargetData())
              {

                  DataTable DtDetail = new DataTable();
                  DataTable dtCalender = new DataTable();
                  using (CommonData ObjCommom = new CommonData())
                  {
                      dtCalender = ObjCommom.GettvpTableFinacialCalender();
                  }
                  DtDetail = (DataTable)ViewState["Detail"];


                 
                  foreach (DataRow dr in DtDetail.Rows)
                  {
                      DataRow drow = dtCalender.NewRow();                     
                      drow[1] = dr["Fortnight"].ToString();
                      drow[2] = dr["StartDate"].ToString();
                      drow[3] = dr["Quarter"];
                      dtCalender.Rows.Add(drow);
                  }
                  dtCalender.AcceptChanges();
                  using (FinacialCalender ObjCal = new FinacialCalender())
                  {
                      ObjCal.Error = "";
                      ObjCal.CalenderYear =txtCalenderYear.Text.Trim () ;
                      //ObjCal.YearEndDate = Convert.ToDateTime(ucEndDate.Date);
                      //ObjCal.UploadCalender(dtCalender);
                      if (ObjCal.ErrorXML != null && ObjCal.ErrorXML != string.Empty)
                      {
                          uclblMessage.XmlErrorSource = ObjCal.ErrorXML ;
                          return;
                      }
                      else if (ObjCal.Error != null && ObjCal.Error != "")
                      {
                          uclblMessage.ShowError(ObjCal.Error);
                          return;
                      }
                      uclblMessage.ShowSuccess(Resources.Messages.CreateSuccessfull);
                      ClearForm();
                  }
              }

          }


      }
      catch (Exception ex)
      {
          PageBase.Errorhandling(ex);
          uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());
      }
          

    }

    private void ClearForm()
    {
        ViewState["Detail"] = null;
        txtCalenderYear.Text = string.Empty;
        pnlGrid.Visible = false;
        ucEndDate.Date = string.Empty;
    }

    private bool pagevalidate()
    {
        if (txtCalenderYear.Text == "")
        {
            uclblMessage.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        if (ucEndDate.Date == "" || ServerValidation.IsDate(ucEndDate.Date)!=true )
        {
            uclblMessage.ShowInfo(Resources.Messages.InvalidDateEntered);
            return false;
        }
        return true;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        uclblMessage.ShowControl = false;
        ClearForm();
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        ScriptManager Src = (ScriptManager)Master.FindControl("ctl00$ScriptManager1");
        ServiceReference re = new ServiceReference("~/SapIntegrationService.asmx");
        Src.Services.Add(re);



    }
}
