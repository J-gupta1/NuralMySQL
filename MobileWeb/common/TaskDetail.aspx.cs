/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 ====================================================================================================================================
 */
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
using System.Globalization;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.ASPxClasses.Internal;

public partial class TaskDetail : PageBase
{
    DataTable dtImageDataActual;
    DataTable dtImageData;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                Session["ResponseImage"] = null;
                if (ViewState["EditTaskUserId"] != null)
                { ViewState.Remove("EditTaskUserId"); }
                //BindUser();
                //BindTaskPriority_Status(4);//CategoryForId 5= Task Status
                BindTaskPriority_Status(5);//CategoryForId 4= Task Priority
                if ((Request.QueryString["TaksUserID"] != null) && (Request.QueryString["TaksUserID"] != ""))
                {
                    Int64 TaksUserID = Convert.ToInt32(Convert.ToString(Cryptography.Crypto.Decrypt((Request.QueryString["TaksUserID"].ToString().Replace(" ", "+")), PageBase.KeyStr)));
                    ViewState["TaskUserId"] = TaksUserID;
                    BindData(TaksUserID,1);
                }
            }
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ucMsg.ShowInfo("start");
        SaveResponse();
    }

    private void SaveResponse()
    {
        ucMsg.ShowError("Start.");
        
        try
        {
            //if (IsPageRefereshed == true)
            //{
            //    return;
            //}
            Int64 TaskUserId;
            if (ViewState["TaskUserId"] != null)
                TaskUserId = Convert.ToInt64(ViewState["TaskUserId"].ToString());
            else
            {
                ucMsg.ShowError("Please reload the popup.");
                return;
            }

                if ( ddlTaskStatus.SelectedIndex==0 )
                {
                    ucMsg.ShowInfo(Resources.Messages.MandatoryField);

                    return;
                }
                using (Task objTask = new Task())
                {

                    objTask.UserID = PageBase.UserId;
                    objTask.TaskUserID = Convert.ToInt64(ViewState["EditTaskUserId"]);

                    objTask.StartDate = UcNextClosureDate.GetDate;
                    objTask.Remark = Server.HtmlEncode(txtRemark.Text.Trim());
                    objTask.TaskStatusId = Convert.ToInt32(ddlTaskStatus.SelectedValue);

                    if (Session["ResponseImage"] == null)
                        dtImageDataActual = CreateImageDataTable();
                    else
                        dtImageDataActual = (DataTable)Session["ResponseImage"];
                    if (dtImageDataActual.Rows.Count==0)
                    {
                        DataRow dr = dtImageDataActual.NewRow();
                        dtImageDataActual.Rows.Add(dr);
                    }

                    objTask.Dt = dtImageDataActual;

                    objTask.InsertTaskResponse();
                    if (objTask.OutParam == 0)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        BindData(TaskUserId,1);
                        ViewState["TaskUserId"] = null;
                        hdfSuccess.Value = "1";

                        return;
                    }
                    else if (objTask.OutParam > 0)
                    {
                        if (!string.IsNullOrEmpty(objTask.error))
                        {
                            ucMsg.ShowError(objTask.error);
                        }
                        else
                        {
                            ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        }

                    }

                }
                
            

        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            ucMsg.Visible = true;
            PageBase.Errorhandling(ex);

        }


    }
    protected override void OnPreRender(EventArgs e)
    {
        
        base.OnPreRender(e);
    }
   
    
   
    #region User Defind Function
    //private void ClearForm()
    //{
    //    ViewState["EditTaskUserId"] = null;
        
    //    txtTask.Text = "";
    //    txtRemark.Text = "";
       
        
    //}
   
    
    private void BindTaskPriority_Status(int CategoryForId)
    {
        try
        {
            //DataTable dt = new DataTable();

            
            using (Task objTask = new Task())
           
            {

                objTask.UserID = PageBase.UserId;
                objTask.CategoryForId = CategoryForId;
                String[] StrCol1 = new String[] { "CategoryID", "CategoryName" };
                DataSet dsResult = objTask.GetCategoryList();
                if (CategoryForId == 5)//CategoryForId 5= Task Status
                {
                    PageBase.DropdownBinding(ref ddlTaskStatus, dsResult.Tables[0], StrCol1);
                    
                    
                }
            };
            
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        if (ViewState["EditTaskUserId"] != null)
        {
            Int64 TaskUserID = Convert.ToInt64(ViewState["EditTaskUserId"]);
            ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
            BindData(TaskUserID, ucPagingControl1.CurrentPage);
        }
        else
            ucMsg.ShowError("Please re-load the pop up.");
    }
    private void BindData(Int64 TaskUserID, int pageno)
    {
        ViewState["TotalRecords"] = 0;
        ViewState["CurrentPage"] = pageno;
        DataSet dsTask = new DataSet();
        using (Task objTask = new Task())
        {
            objTask.UserID = PageBase.UserId;
            objTask.TaskPriorityId = 0;
            objTask.TaskStatusId =0;
            
            objTask.TaskUserID = TaskUserID;
            objTask.CompanyId = PageBase.ClientId;
            objTask.PageIndex = pageno;
            objTask.PageSize = Convert.ToInt32(PageBase.PageSize);
            dsTask = objTask.getTaskData();
            ViewState["TotalRecords"] = objTask.TotalRecords;
           // ViewState["CompanyImageFolder"] = objTask.CompanyImageFolder;
            Session["CompanyImageFolder"] = objTask.CompanyImageFolder;
        };
        if (dsTask != null && dsTask.Tables[0].Rows.Count > 0)
        {
            ViewState["EditTaskUserId"] = TaskUserID;
            txtTask.Text = Convert.ToString(dsTask.Tables[0].Rows[0]["Task"]);
            txtTaskRemark.Text = Convert.ToString(dsTask.Tables[0].Rows[0]["Remark"]);
            txtStartDate.Text = Convert.ToString(dsTask.Tables[0].Rows[0]["StartDate"]);
            txtEndDate.Text = Convert.ToString(dsTask.Tables[0].Rows[0]["EndDate"]);
            txtPriority.Text = Convert.ToString(dsTask.Tables[0].Rows[0]["TaskPriority"]);
            
            
           if (dsTask.Tables[1].Rows.Count > 0)
           {
               if (pageno > 0)
               {

                   ucPagingControl1.TotalRecords = Convert.ToInt32(ViewState["TotalRecords"]);
                   ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                   ucPagingControl1.SetCurrentPage = pageno;
                   ucPagingControl1.FillPageInfo();
                   grdvwTask.DataSource = dsTask.Tables[1];
                   grdvwTask.DataBind();
                   grdvwTask.Visible = true;
               }
           }
        }
        else
        {
            //ViewState["Dtexport"] = null;
            grdvwTask.DataSource = null;
            grdvwTask.DataBind();
        }
        
        //UpdSearch.Update();
    }
    protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        try
        {
            e.CallbackData = SavePostedFiles(e.UploadedFile);
        }
        catch (Exception ex)
        {
            e.IsValid = false;
            e.ErrorText = ex.Message;
        }
    }
    string SavePostedFiles(UploadedFile uploadedFile)
    {


        string strFilePath;

        strFilePath = ZedService.Utility.ZedServiceUtil.GetUploadFilePathV2(Convert.ToDateTime(System.DateTime.Now), uploadedFile.FileName);
        if (!uploadedFile.IsValid)
            return string.Empty;
        FileInfo fileInfo = new FileInfo(uploadedFile.FileName);
        string strTicks = System.DateTime.Now.Ticks.ToString();

        string strFileUploadedName = fileInfo.Name ;
        string strfileExtention = fileInfo.Extension;
        string ImgPath = Session["CompanyImageFolder"].ToString();

        strFileUploadedName = strFileUploadedName.Replace(strfileExtention, "").Replace(" ", "")+strTicks;
        strFileUploadedName = strFileUploadedName  + strfileExtention;
        string strTempPath = Server.MapPath(ImgPath + "UserImages/TaskResponseImages/");  
        if (!Directory.Exists(strTempPath))
            Directory.CreateDirectory(strTempPath );


        uploadedFile.SaveAs(strTempPath + strFileUploadedName);
        if (Session["ResponseImage"] == null)
        {
            
            dtImageDataActual = new DataTable();
            dtImageDataActual = CreateImageDataTable();
            DataRow dr = dtImageDataActual.NewRow();
            dr["FileLocation"] = "UserImages/TaskResponseImages/" + strFileUploadedName;
            dr["APPImageID"] = 15;
             ;
            dtImageDataActual.Rows.Add(dr);
            dtImageDataActual.AcceptChanges();
            Session["ResponseImage"] = dtImageDataActual;

        }
        else
        {
            //string removeextention = strFileUploadedName.Replace(".pdf", "");
            dtImageDataActual = (DataTable)Session["ResponseImage"];
            DataRow dr = dtImageDataActual.NewRow();

            dr["FileLocation"] = "UserImages/TaskResponseImages/" + strFileUploadedName;
            dr["APPImageID"] = 15;
           
            dtImageDataActual.Rows.Add(dr);
            dtImageDataActual.AcceptChanges();
            Session["ResponseImage"] = dtImageDataActual;


        }
        string fileLabel = fileInfo.Name;
        string fileLength = uploadedFile.FileBytes.Length / 1024 + "K";

        return string.Format("{0} ({1})|{2}", fileLabel, fileLength, VirtualPath + ImgPath + "/UserImages/TaskResponseImages/" + strFileUploadedName);

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["ResponseImage"] != null)
            {
                //Server.MapPath("~/UploadDownload/CMX/");
                string strTempPath = Server.MapPath(Convert.ToString(Session["CompanyImageFolder"])); 
                DataTable FinalFileData = (DataTable)Session["ResponseImage"];
                for (int cntr = 0; cntr < FinalFileData.Rows.Count;cntr++ )
                {
                    if (File.Exists(strTempPath + FinalFileData.Rows[cntr]["FileLocation"].ToString()))
                        File.Delete(strTempPath + FinalFileData.Rows[cntr]["FileLocation"].ToString());
                    FinalFileData.Rows.RemoveAt(cntr);
                    FinalFileData.AcceptChanges();
                }
                    FinalFileData.Rows.Clear();
                FinalFileData.AcceptChanges();
                Session["ResponseImage"] = FinalFileData;

            }
            txtRemark.Text = "";
            ddlTaskStatus.SelectedIndex = 0;
            UcNextClosureDate.Date = "";
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }

    }
    DataTable CreateImageDataTable()
    {
        dtImageData = new DataTable();
        DataColumn dc = new DataColumn("APPImageID",typeof(int));
        dtImageData.Columns.Add(dc);
        dtImageData.Columns.Add("FileLocation", typeof(string));
        return dtImageData;
    }
    
    #endregion






    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        try
        {

            string filePath = (sender as LinkButton).CommandArgument;
            string ExportFileLocation = Server.MapPath(Session["CompanyImageFolder"].ToString());
            string fullpath = ExportFileLocation + "/" + filePath;
            Response.ContentType = "image/jpg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullpath));
            Response.WriteFile(fullpath);
            Response.End();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
    }
    protected void grdvwTask_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string IsPrint = grdvwTask.DataKeys[e.Row.RowIndex]["ImageRelativePath"].ToString();
                LinkButton BtnPrint = (LinkButton)e.Row.FindControl("lnkDownload");
                if (IsPrint != "")
                {
                    BtnPrint.Visible = true;
                }
                else
                {
                    BtnPrint.Visible = false;

                }

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
    }

   
}
