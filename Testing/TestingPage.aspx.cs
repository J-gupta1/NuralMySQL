using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using System.Text;



public partial class Testing_TestingPage : PageBase
{
    string strUploadedFileName,handle;
    DataSet dsSap;
    int counter = 0;
    int status;
    bool success;
    SapService objSapService = new SapService();
    //string strPath = HttpContext.Current.Server.MapPath(PageBase.SapDirectoryPath);
    //DirectoryInfo drSourceInfo = new DirectoryInfo(strPath);
    //DirectoryInfo drTargetInfoFailure = new DirectoryInfo(strPath + "/Failure");
    SapIntegration objSapIntegr = new SapIntegration();
    string remoteFilePath;
    Chilkat.SFtp sftp = new Chilkat.SFtp();
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
       UcMessage1.Visible = false;
    }
    protected void ShowModal(object sender, EventArgs e)
    {
        // Thread.Sleep(6000);
    }
    //protected void asd_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    UpdatePanel1.Update();
    //}
    protected void lnkExport_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        UpdatePanel2.Update();
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            UcMessage1.Visible = false;
            SapIntegration obj = new SapIntegration();
            string dummy = "";
            obj.ValidateExcelFileMOD(out status, dummy,1);
            if (status == 1)
                UcMessage1.ShowInfo("Successfully updated");
            else
                UcMessage1.ShowInfo("Could not be uploaded successfully");
            
        }
        catch (Exception ex)
        {
            
                objSapService.ModuleName = EnumData.EnumSAPModuleName.MODDataUpload;
                objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
                objSapService.StatusValue = "Error in the Data";
                objSapService.MessageDetail = ex.Message+ "::" + ex.StackTrace;
                objSapService.SapServiceMethodName = "Mod Data";
                objSapService.SapFileName = UploadFile.UploadedFileName;
                objSapService.insertServiceTraceLog();
                objSapService.XMLData = "Exception has occured";
            //objSapIntegr.Copy(drSourceInfo, drTargetInfoFailure, UploadFile.UploadedFileName);    //PankajDhingra
            
        }
    }
    protected void btnUploadBTM_Click(object sender, EventArgs e)
    {
        try
        {
            UcMessage1.Visible = false;
            SapIntegration obj = new SapIntegration();
            string dummy = "";
            obj.ValidateExcelFileBTM(out status,dummy);
            if (status == 1)
                UcMessage1.ShowInfo("Successfully updated");
            else
                UcMessage1.ShowInfo("Could not be uploaded successfully");
        }
        catch (Exception ex)
        {

            objSapService.ModuleName = EnumData.EnumSAPModuleName.BTMDataUpload;
            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objSapService.StatusValue = "Error in the Data";
            objSapService.MessageDetail = ex.Message + "::" + ex.StackTrace;
            objSapService.SapServiceMethodName = "BTM Data";
            objSapService.SapFileName = UploadFile.UploadedFileName;
            objSapService.insertServiceTraceLog();
            objSapService.XMLData = "Exception has occured";
            //objSapIntegr.Copy(drSourceInfo, drTargetInfoFailure, UploadFile.UploadedFileName);    //PankajDhingra

        }
    }
    protected void btnUploadGrn_Click(object sender, EventArgs e)
    {
      try
        {
            UcMessage1.Visible = false;
            SapIntegration obj = new SapIntegration();
            string dummy = "";
            DirectoryInfo dir = new DirectoryInfo("d:/saptest");
            FileInfo[] drFiles = dir.GetFiles("*.xlsx");
            foreach (FileInfo fi in drFiles)
            {
                UploadFile.UploadedFileName = (fi.FullName).Remove(0, dir.FullName.Length + 1);

            }
            UploadFile.RootFolerPath = dir.FullName;
            obj.ValidateExcelFileGRN(out status,dummy);
            if (status == 1)
                UcMessage1.ShowInfo("Successfully updated");
            else
                UcMessage1.ShowInfo("Could not be uploaded successfully");
        }
        catch (Exception ex)
        {

            objSapService.ModuleName = EnumData.EnumSAPModuleName.GRNDataUpload;
            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objSapService.StatusValue = "Error in the Data";
            objSapService.MessageDetail = ex.Message + "::" + ex.StackTrace;
            objSapService.SapServiceMethodName = "BTM Data";
            objSapService.SapFileName = UploadFile.UploadedFileName;
            objSapService.insertServiceTraceLog();
            objSapService.XMLData = "Exception has occured";
            //objSapIntegr.Copy(drSourceInfo, drTargetInfoFailure, UploadFile.UploadedFileName);    //PankajDhingra

        }
    }
    protected void btnFTP_Click(object sender, EventArgs e)
    {
       // SapIntegration obj = new SapIntegration();
       // obj.SendMailInfo("");
       // return;
        objSapIntegr.GetDataFromFTPForProcessingForGfive();
        return;
        //string str = objSapIntegr.GetDataFromFTPForProcessing();
         DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + PageBase.SapDirectoryPath);
         FileInfo[] drFiles = dir.GetFiles("*.xlsx");
         if (drFiles.Length == 0)
         {
             objSapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
             objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
             objSapService.StatusValue = SapService.DownLoadFailed + " For further Processing!";
             objSapService.MessageDetail = SapService.DownLoadFailed;
             objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
             objSapService.SapFileName = "No File Exits";
             objSapService.insertServiceTraceLog();
         }
         else
         {
             foreach (FileInfo fi in drFiles)
             {
                 if (((fi.FullName).Remove(0, dir.FullName.Length + 1)).Substring(0, 4).ToUpper() == "IMEI")
                 {
                     UploadFile.RootFolerPath = dir.FullName;
                     objSapIntegr.ValidateExcelFileIMEI(out status, ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
                     if (status == 1)
                     {
                         // success = sftp.UploadFileByName("/success/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), Server.MapPath("../" + SapDirectoryPath) + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
                         //success = sftp.UploadFileByName("/home/salestracker/MobileSalesTracking/Success/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), AppDomain.CurrentDomain.BaseDirectory + PageBase.SapDirectoryPath + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
                         success = true;
                         if (success == false)
                         {
                             SapService objSapService = new SapService();
                             objSapService.ModuleName = EnumData.EnumSAPModuleName.GRNDataUpload;
                             objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
                             objSapService.StatusValue = SapService.NoFileExistUploading;
                             objSapService.MessageDetail = SapService.NoFileExistUploading;
                             objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.GRNData.ToString();
                             objSapService.SapFileName = (fi.FullName).Remove(0, dir.FullName.Length + 1);
                             objSapService.insertServiceTraceLog();
                         }
                         fi.Delete();
                         sftp.RemoveFile("/" + PageBase.sFtpServerRemoteDir + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
                     }
                     else
                     {
                         success = sftp.UploadFileByName("/home/salestracker/MobileSalesTracking/Failure/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), AppDomain.CurrentDomain.BaseDirectory + PageBase.SapDirectoryPath + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
                         if (success == false)
                         {
                             SapService objSapService = new SapService();
                             objSapService.ModuleName = EnumData.EnumSAPModuleName.GRNDataUpload;
                             objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
                             objSapService.StatusValue = SapService.NoFileExistUploading;
                             objSapService.MessageDetail = SapService.NoFileExistUploading;
                             objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.GRNData.ToString();
                             objSapService.SapFileName = (fi.FullName).Remove(0, dir.FullName.Length + 1);
                             objSapService.insertServiceTraceLog();
                         }
                         fi.Delete();
                         sftp.RemoveFile("/" + PageBase.sFtpServerRemoteDir + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
                     }
                 }
             }
         }
                            

        //string str=
       // objSapIntegr.testupload();
       // Response.Write(str);
        //try
        //{
        //    SapIntegration obj = new SapIntegration();
        //    if (AuthenticateAccess())
        //    {
        //        handle = sftp.OpenDir(PageBase.sFtpServerRemoteDir);
        //        if (handle == null)
        //        {
        //            objSapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
        //            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //            objSapService.StatusValue = SapService.NoFileExistDownLoad;
        //            objSapService.MessageDetail = SapService.NoFileExistDownLoad;
        //            objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
        //            objSapService.SapFileName = "NA";
        //            objSapService.XMLData = SapService.ExceptionOccured;
        //            objSapService.insertServiceTraceLog();
        //            return;
        //        }
        //        //  Download the directory listing:
        //        Chilkat.SFtpDir dirListing = null;
        //        dirListing = sftp.ReadDir(handle);
        //        if (dirListing == null)
        //        {
        //            objSapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
        //            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //            objSapService.StatusValue = SapService.NoFileExistDownLoad;
        //            objSapService.MessageDetail = SapService.NoFileExistDownLoad;
        //            objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
        //            objSapService.SapFileName = "NA";
        //            objSapService.XMLData = SapService.ExceptionOccured;
        //            objSapService.insertServiceTraceLog();
        //            return;
        //        }
        //        int n = dirListing.NumFilesAndDirs;
        //        if (n == 0)
        //        {
        //            objSapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
        //            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //            objSapService.StatusValue = SapService.NoFileExistDownLoad;
        //            objSapService.MessageDetail = SapService.NoFileExistDownLoad;
        //            objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
        //            objSapService.SapFileName = "NA";
        //            objSapService.XMLData = SapService.ExceptionOccured;
        //            objSapService.insertServiceTraceLog();

        //        }
        //        else
        //        {
        //            for (counter = 0; counter <= n - 1; counter++)
        //            {
        //                Chilkat.SFtpFile fileObj = null;
        //                fileObj = dirListing.GetFileObject(counter);
        //                //textBox1.Text += fileObj.Filename + "\r\n";
        //                //  Does this filename match the desired pattern?
        //                //  Write code here to determine if it's a match or not.
        //                //  Assuming it's a match, you would download the file
        //                //  like this:
        //                if (fileObj.FileType != "directory")
        //                {
        //                    //remoteFilePath = obj.ftpRemoteDir + "/";
        //                    remoteFilePath = PageBase.sFtpServerRemoteDir + "/";
        //                    remoteFilePath = remoteFilePath + fileObj.Filename;
        //                    string localFilePath;
        //                    localFilePath = fileObj.Filename;
        //                    success = sftp.DownloadFileByName(remoteFilePath, AppDomain.CurrentDomain.BaseDirectory + SapDirectoryPath+"/" + localFilePath);
        //                    //  success = sftp.DownloadFileByName(remoteFilePath, Server.MapPath("../" + SapDirectoryPath) + "/" + localFilePath);
        //                    if (success != true)
        //                    {
        //                        objSapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
        //                        objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //                        objSapService.StatusValue = SapService.NoFileExistDownLoad;
        //                        objSapService.MessageDetail = SapService.ExceptionOccured;
        //                        objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
        //                        objSapService.SapFileName = "NA";
        //                        objSapService.XMLData = SapService.ExceptionOccured;
        //                        objSapService.insertServiceTraceLog();
        //                        //return;
        //                    }
        //                }

        //            }
        //            //DirectoryInfo dir = new DirectoryInfo(Server.MapPath("../" + SapDirectoryPath));
        //            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + SapDirectoryPath);
                    
        //            FileInfo[] drFiles = dir.GetFiles("*.xlsx");
        //            if (drFiles.Length == 0)
        //            {
        //                objSapService.ModuleName = EnumData.EnumSAPModuleName.GRNDataUpload;
        //                objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //                objSapService.StatusValue = SapService.DownLoadFailed + " For further Processing!";
        //                objSapService.MessageDetail = SapService.DownLoadFailed;
        //                objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
        //                objSapService.SapFileName = "No File Exits";
        //                objSapService.insertServiceTraceLog();
        //            }
        //            else
        //            {
        //                foreach (FileInfo fi in drFiles)
        //                {
        //                    if (((fi.FullName).Remove(0, dir.FullName.Length + 1)).Substring(0, 3).ToUpper() == "GRN")
        //                    {
        //                        UploadFile.RootFolerPath = dir.FullName;
        //                        obj.ValidateExcelFileGRN(out status, ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                        if (status == 1)
        //                        {
        //                           // success = sftp.UploadFileByName("/success/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), Server.MapPath("../" + SapDirectoryPath) + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                            success = sftp.UploadFileByName("/home/salestracker/MobileSalesTracking/Success/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), AppDomain.CurrentDomain.BaseDirectory + SapDirectoryPath + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                            if (success == false)
        //                            {
        //                                objSapService.ModuleName = EnumData.EnumSAPModuleName.GRNDataUpload;
        //                                objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //                                objSapService.StatusValue = SapService.NoFileExistUploading;
        //                                objSapService.MessageDetail = SapService.NoFileExistUploading;
        //                                objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.GRNData.ToString();
        //                                objSapService.SapFileName = (fi.FullName).Remove(0, dir.FullName.Length + 1);
        //                                objSapService.insertServiceTraceLog();
        //                            }
        //                            fi.Delete();
        //                            sftp.RemoveFile("/"+PageBase.sFtpServerRemoteDir+"/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                        }
        //                        else
        //                        {
        //                            success = sftp.UploadFileByName("/home/salestracker/MobileSalesTracking/Failure/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), AppDomain.CurrentDomain.BaseDirectory + SapDirectoryPath + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                            if (success == false)
        //                            {
        //                                objSapService.ModuleName = EnumData.EnumSAPModuleName.GRNDataUpload;
        //                                objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //                                objSapService.StatusValue = SapService.NoFileExistUploading;
        //                                objSapService.MessageDetail = SapService.NoFileExistUploading;
        //                                objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.GRNData.ToString();
        //                                objSapService.SapFileName = (fi.FullName).Remove(0, dir.FullName.Length + 1);
        //                                objSapService.insertServiceTraceLog();
        //                            }
        //                            fi.Delete();
        //                            sftp.RemoveFile("/" + PageBase.sFtpServerRemoteDir + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                        }
        //                    }
        //                    if (((fi.FullName).Remove(0, dir.FullName.Length + 1)).Substring(0, 3).ToUpper() == "MOD")
        //                    {
        //                        UploadFile.RootFolerPath = dir.FullName;
        //                        obj.ValidateExcelFileMOD(out status, ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                        if (status == 1)
        //                        {
        //                            success = sftp.UploadFileByName("/home/salestracker/MobileSalesTracking/Success/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), AppDomain.CurrentDomain.BaseDirectory + SapDirectoryPath + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                            if (success == false)
        //                            {
        //                                objSapService.ModuleName = EnumData.EnumSAPModuleName.MODDataUpload;
        //                                objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //                                objSapService.StatusValue = SapService.NoFileExistUploading;
        //                                objSapService.MessageDetail = SapService.NoFileExistUploading;
        //                                objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
        //                                objSapService.SapFileName = (fi.FullName).Remove(0, dir.FullName.Length + 1);
        //                                objSapService.insertServiceTraceLog();
        //                            }
        //                            fi.Delete();
        //                            sftp.RemoveFile("/" + PageBase.sFtpServerRemoteDir + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                        }
        //                        else
        //                        {
        //                            success = sftp.UploadFileByName("/home/salestracker/MobileSalesTracking/Failure/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), AppDomain.CurrentDomain.BaseDirectory + SapDirectoryPath + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                            if (success == false)
        //                            {
        //                                objSapService.ModuleName = EnumData.EnumSAPModuleName.MODDataUpload;
        //                                objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //                                objSapService.StatusValue = SapService.NoFileExistUploading;
        //                                objSapService.MessageDetail = SapService.NoFileExistUploading;
        //                                objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
        //                                objSapService.SapFileName = (fi.FullName).Remove(0, dir.FullName.Length + 1);
        //                                objSapService.insertServiceTraceLog();
        //                            }
        //                            fi.Delete();
        //                            sftp.RemoveFile("/" + PageBase.sFtpServerRemoteDir + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                        }

        //                    }
        //                    if (((fi.FullName).Remove(0, dir.FullName.Length + 1)).Substring(0, 3).ToUpper() == "BTM")
        //                    {
        //                        UploadFile.RootFolerPath = dir.FullName;
        //                        obj.ValidateExcelFileBTM(out status, ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                        if (status == 1)
        //                        {
        //                            success = sftp.UploadFileByName("/home/salestracker/MobileSalesTracking/Success/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), AppDomain.CurrentDomain.BaseDirectory + SapDirectoryPath + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                            if (success == false)
        //                            {
        //                                objSapService.ModuleName = EnumData.EnumSAPModuleName.BTMDataUpload;
        //                                objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //                                objSapService.StatusValue = SapService.NoFileExistUploading;
        //                                objSapService.MessageDetail = SapService.NoFileExistUploading; 
        //                                objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.BTMData.ToString();
        //                                objSapService.SapFileName = (fi.FullName).Remove(0, dir.FullName.Length + 1);
        //                                objSapService.insertServiceTraceLog();
        //                            }
        //                            fi.Delete();
        //                            sftp.RemoveFile("/" + PageBase.sFtpServerRemoteDir + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                        }
        //                        else
        //                        {
        //                            success = sftp.UploadFileByName("/home/salestracker/MobileSalesTracking/Failure/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)), AppDomain.CurrentDomain.BaseDirectory + SapDirectoryPath + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                            if (success == false)
        //                            {
        //                                objSapService.ModuleName = EnumData.EnumSAPModuleName.BTMDataUpload;
        //                                objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //                                objSapService.StatusValue = SapService.NoFileExistUploading;
        //                                objSapService.MessageDetail = SapService.NoFileExistUploading;
        //                                objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.BTMData.ToString();
        //                                objSapService.SapFileName = (fi.FullName).Remove(0, dir.FullName.Length + 1);
        //                                objSapService.insertServiceTraceLog();
        //                            }
        //                            fi.Delete();
        //                            sftp.RemoveFile("/" + PageBase.sFtpServerRemoteDir + "/" + ((fi.FullName).Remove(0, dir.FullName.Length + 1)));
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    else
        //    {
        //        objSapService.ModuleName = EnumData.EnumSAPModuleName.ExceptionOccured;
        //        objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //        objSapService.StatusValue = SapService.ConnectionFailed;
        //        objSapService.MessageDetail = SapService.ConnectionFailed;
        //        objSapService.SapServiceMethodName = SapService.ConnectionFailed;
        //        objSapService.SapFileName = SapService.ConnectionFailed;
        //        objSapService.XMLData = SapService.ExceptionOccured;
        //        objSapService.insertServiceTraceLog();

        //    }
        //}
    
        //catch (Exception ex)
        //{
        //    objSapService.ModuleName = EnumData.EnumSAPModuleName.ExceptionOccured;
        //    objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
        //    objSapService.StatusValue = SapService.ExceptionOccured;
        //    objSapService.MessageDetail = ex.Message + "::" + ex.StackTrace;
        //    objSapService.SapServiceMethodName = SapService.ExceptionOccured;
        //    objSapService.SapFileName = UploadFile.UploadedFileName;
        //    objSapService.XMLData = SapService.ExceptionOccured;
        //    objSapService.insertServiceTraceLog();
            
        //}
        //finally
        //{
        //    sftp.CloseHandle(handle);
        //}
    }
 
    public Boolean AuthenticateAccess()
    {
        SapIntegration obj = new SapIntegration();
        //Chilkat.SFtp sftp = new Chilkat.SFtp();
        success = sftp.UnlockComponent("Anything for 30-day trial");
        if (success != true)
        {
            objSapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objSapService.StatusValue = SapService.ExceptionOccured + " about trial";
            objSapService.MessageDetail = SapService.ExceptionOccured + " about trial";
            objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
            objSapService.SapFileName = "NA";
            objSapService.XMLData = "30-days Trial has expired";
            objSapService.insertServiceTraceLog();
            return false;
        }
        sftp.ConnectTimeoutMs = 50000;
        sftp.IdleTimeoutMs = 10000;
        int port;
        string hostname;
        //hostname = obj.ftpServerLocalIP;
       hostname = PageBase.sFtpServerIP;
        //hostname = "210.18.116.115";
        //hostname = "192.168.0.14";
        port = 22;
        success = sftp.Connect(hostname, port);
        if (success != true)
        {
            objSapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objSapService.StatusValue = SapService.ExceptionOccured + " about trial";
            objSapService.MessageDetail = SapService.ConnectionFailed;
            objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
            objSapService.SapFileName = "NA";
            objSapService.XMLData = SapService.ConnectionFailed;
            objSapService.insertServiceTraceLog();
            return false;
        }
        for (int i = 0; i < 3; i++)
        {
            success = sftp.AuthenticatePw(PageBase.sFtpServerUserName, PageBase.sFtpServerPassword);
            if (success == true)
                break;
        }
            
               
        //success = sftp.AuthenticatePw("salestracker", "pass@123");
        //success = sftp.AuthenticatePw("pankaj", "zed-axis123");

        //success = sftp.AuthenticatePw(obj.ftpUserLocalID, obj.ftpPasswordLocal);
        if (success != true)
        {
            objSapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objSapService.StatusValue = SapService.ExceptionOccured;
            objSapService.MessageDetail = SapService.ConnectionFailed + " ,Wrong Credentials!";
            objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
            objSapService.SapFileName = "NA";
            objSapService.XMLData = SapService.ExceptionOccured;
            objSapService.insertServiceTraceLog();
            return false;
        }
        success = sftp.InitializeSftp();
        if (success != true)
        {
            objSapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objSapService.StatusValue = SapService.ExceptionOccured;
            objSapService.MessageDetail = SapService.ConnectionFailed + ", Initialization Failed!";
            objSapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
            objSapService.SapFileName = "NA";
            objSapService.XMLData = SapService.ExceptionOccured;
            objSapService.insertServiceTraceLog();
            return false;
        }
        return true;
    }
    protected void btnUploadServer_Click(object sender, EventArgs e)
    {
        if (AuthenticateAccess())
        {
            SapIntegration obj=new SapIntegration();
            remoteFilePath = PageBase.sFtpServerRemoteDir + "/";
            //handle = sftp.OpenFile("New folder/BTM_Amit.xlsx", "writeOnly", "createTruncate");
            //success = sftp.UploadFile(handle, drSourceInfo.ToString() + "/Success");

            //success = sftp.UploadFileByName(remoteFilePath+"/Success/gg.xlsx", Server.MapPath("../" + SapDirectoryPath) + "/Success");
            success = sftp.UploadFileByName("/Success/GRN_Amit.xlsx", Server.MapPath("../" + SapDirectoryPath) + "/Success/GRN_Amit.xlsx");
           
            
            //success = sftp.UploadFile("/Success/GRN_Amit.xlsx", Server.MapPath("../" + SapDirectoryPath) + "/Success/GRN_Amit.xlsx");
                      
            if (success != true)
            {
                return;
            }
            success = sftp.CloseHandle(handle);
            if (success != true)
            {
                return;
            }

        }
    }

    protected void btnBeetelFTP_Click(object sender, EventArgs e)
    {
        //Response.Write("<script>alert('Before Initialization');</script>");
        string str = objSapIntegr.GetDataFromFTPForProcessing();
    }
    protected void BeetelOnida_Click(object sender, EventArgs e)
    {
        try
        {
            UcMessage1.ShowInfo("workin progress");
            SapIntegrationService obj = new SapIntegrationService();
            obj.FillListofTables();
           
        }
        catch (Exception ex)
        {
            SapService objsapService = new SapService();
            objsapService.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
            objsapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objsapService.StatusValue = "Error";
            objsapService.StatusValue = "No Data";
            objsapService.MessageDetail = ex.Message + "::" + ex.StackTrace;
            objsapService.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
            objsapService.SapFileName = "No Data";
            objsapService.XMLData = "<DataSet></DataSet>";
            objsapService.insertServiceTraceLog();
        }
    }

}
