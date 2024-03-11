﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Collections;
using System.Configuration;

public partial class Transactions_POC_PrimarySalesPOC : System.Web.UI.Page
{
    SapIntegration objFtpFiles = new SapIntegration();
    string strUploadedFileName = string.Empty;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        if (!IsPostBack)
        {


        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int status = 0;
        int UploadCheck = 0;
        string str = ConfigurationSettings.AppSettings["Client"];
        try
        {
            UploadFile.RootFolerPath = HttpContext.Current.Server.MapPath(PageBase.SapDirectoryPath);
            UploadCheck = UploadFile.IsExcelSAPFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                objFtpFiles.ValidateExcelFileMOD(out status, strUploadedFileName, 2);        
                MsgDisplay(status);
            }
            else if (UploadCheck == 2)
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            else if (UploadCheck == 3)
                ucMsg.ShowInfo(Resources.Messages.SelectFile);
            else
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError("Please Check the Error log " + ex.Message.ToString());
            SapService objSapService = new SapService();
            objSapService.ModuleName = EnumData.EnumSAPModuleName.ExceptionOccured;
            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objSapService.StatusValue = "File has some corrupted data.";
            objSapService.MessageDetail = "File has some corrupted data.";
            objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
            objSapService.SapFileName = FileUpload1.FileName;
            objSapService.insertServiceTraceLog();

        }
    }
    void MsgDisplay(int value)
    {
        if (value == 0)
            ucMsg.ShowInfo("unSuccessful");
        else
            ucMsg.ShowInfo("Successful");
    }

    //void BindFileMode()
    //{
    //    List<string> lst = new List<string>();

    //    string[] strArray = Enum.GetNames(typeof(EnumData.EnumSAPModuleName));
    //    Array strArray1 = Enum.GetValues(typeof(EnumData.EnumSAPModuleName));
    //    var sort = from I in strArray
    //               orderby I
    //               select I;

    //    if (strArray != null)
    //    {
    //        foreach (string val in sort)
    //        {
    //            lst.Add(val.Substring(0,3));
    //        }
    //    }

    //    rdListMode.DataSource = lst;
    //    rdListMode.DataBind();
    //}
}