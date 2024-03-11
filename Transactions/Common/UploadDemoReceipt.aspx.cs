using BussinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transactions_Common_UploadDemoReceipt : PageBase
{
    private int _UpperLimit = 0;
    public class FileCollectionEventArgs : EventArgs
    {
        private HttpRequest _HttpRequest;

        public HttpFileCollection PostedFiles
        {
            get
            {
                return _HttpRequest.Files;
            }
        }

        public int Count
        {
            get { return _HttpRequest.Files.Count; }
        }

        public bool HasFiles
        {
            get { return _HttpRequest.Files.Count > 0 ? true : false; }
        }

        public double TotalSize
        {
            get
            {
                double Size = 0D;
                for (int n = 0; n < _HttpRequest.Files.Count; ++n)
                {
                    if (_HttpRequest.Files[n].ContentLength < 0)
                        continue;
                    else
                        Size += _HttpRequest.Files[n].ContentLength;
                }

                return Math.Round(Size / 1024D, 2);
            }
        }

        public FileCollectionEventArgs(HttpRequest oHttpRequest)
        {
            _HttpRequest = oHttpRequest;
        }
    }
    public int UpperLimit
    {
        get { return _UpperLimit; }
        set { _UpperLimit = value; }
    }
    private string _SaveOn = Resources.Config.SaveFileOn.ToString();
    public string SaveOn
    {
        get { return _SaveOn; }
        set { _SaveOn = value; }
    }
    private DataTable _FinalFileData;
    public DataTable FinalFileData
    {
        get
        {
            return _FinalFileData;
        }
        set
        {
            _FinalFileData = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindGrid();
        }
    }
    public string FinalFileListWithImageType
    {
        get
        {
            return Convert.ToString(hdnFinalList.Value);
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.IpFile.PostedFile.FileName == "" || txtRetailorCode.Text.Trim() == "")
            {
                ucMessage1.ShowError("Please Enter Retailor Code and select file for upload.");
                dvError.Style.Add("display", "block");
            }
            else
            {
                string ext = System.IO.Path.GetExtension(this.IpFile.PostedFile.FileName);
                if (ext != ".JPG" && ext != ".jpg" && ext != ".JPEG" && ext != ".jpeg" && ext != ".bmp" && ext != ".gif" && ext != ".GIF" && ext != ".png" && ext != ".PNG" && ext != ".pdf" && ext != ".PDF" && ext != ".doc" && ext != ".docx")
                {
                    ucMessage1.ShowError("Please Select Valid File.");
                    dvError.Style.Add("display", "block");
                }
                else
                {
                    FileCollectionEventArgs ee = new FileCollectionEventArgs(this.Request);


                    string temp_hdnFinalList = FinalFileListWithImageType;
                    string[] tempfile = temp_hdnFinalList.Split("?".ToCharArray());
                    DataTable dt_File = new DataTable();
                    dt_File.Columns.Add("CtrlID");
                    dt_File.Columns.Add("FileLocation");
                    dt_File.Columns.Add("ImageTypeId");
                    dt_File.Columns.Add("ImageRelativePath");
                    dt_File.Columns.Add("TempFileLocation");
                    dt_File.Columns.Add("ImageTypeName");
                    string[] allkeys = ee.PostedFiles.AllKeys;
                    HttpFileCollection oHttpFileCollection = ee.PostedFiles;
                    HttpPostedFile oHttpPostedFile = null;
                    if (ee.HasFiles)
                    {
                        int FileSize = Convert.ToInt32(Resources.Config.UploadedImageSize);
                        if (Resources.Config.UploadedImageSizeType.ToUpper() == "KB")
                        {
                            FileSize = FileSize * 1024;
                        }
                        else if (Resources.Config.UploadedImageSizeType.ToUpper() == "MB")
                        {
                            FileSize = (FileSize * 1024) * 1024;
                        }
                        else
                        {

                        }

                        string SizeError = "";
                        for (int n = 0; n < ee.Count; n++)
                        {
                            oHttpPostedFile = oHttpFileCollection[n];
                            if (oHttpPostedFile.ContentLength <= 0 || oHttpPostedFile.ContentLength >= FileSize)
                            {
                                if (oHttpPostedFile.ContentLength >= FileSize)
                                    SizeError = SizeError == "" ? oHttpPostedFile.FileName.Replace("\\", ",").Split(',')[oHttpPostedFile.FileName.Replace("\\", ",").Split(',').Length - 1] : SizeError + ", " + oHttpPostedFile.FileName.Replace("\\", ",").Split(',')[oHttpPostedFile.FileName.Replace("\\", ",").Split(',').Length - 1];
                                continue;
                            }
                            else
                            {
                                Guid uid = Guid.NewGuid();

                                string filenamewithtimestamp = "\\" + System.IO.Path.GetFileNameWithoutExtension(oHttpPostedFile.FileName) + "-" + uid.ToString().Replace("-", "") + System.IO.Path.GetExtension(oHttpPostedFile.FileName);
                                filenamewithtimestamp = filenamewithtimestamp.ToLower();
                                string FinalFileLocation = GetCurrentLocation() + filenamewithtimestamp;

                                string[] getYearAndMonth = FinalFileLocation.Split('\\');
                                string FileName = System.IO.Path.GetFileNameWithoutExtension(oHttpPostedFile.FileName) + "-" + uid.ToString().Replace("-", "") + System.IO.Path.GetExtension(oHttpPostedFile.FileName);
                                FileName = FileName.ToLower();
                                string YearAndMonth = string.Empty, ImageURL = string.Empty;
                                for (int i = 0; i < getYearAndMonth.Length; i++)
                                {
                                    if (getYearAndMonth[i] == "UploadPersistent")
                                    {
                                        YearAndMonth = getYearAndMonth[++i] + "/" + getYearAndMonth[++i] + "/" + FileName;
                                        i = getYearAndMonth.Length;
                                    }
                                }
                                _SaveOn = Resources.Config.SaveFileOn.ToString();
                                if (Resources.Config.SaveFileOn.ToString() == "0")
                                {
                                    ImageURL = "cloudstorage:" + YearAndMonth;

                                }
                                else
                                    ImageURL = YearAndMonth;
                                string CtrlID = allkeys[n];
                                CtrlID = CtrlID.Replace('$', '_');
                                string ImageType = string.Empty;
                                for (int i = 0; i < tempfile.Length; i++)
                                {
                                    if (tempfile[i].Length > 0)
                                    {
                                        string[] tr = tempfile[i].Split("|".ToCharArray());
                                        if (CtrlID == tr[0])
                                        {
                                            ImageType = tr[1];
                                            break;

                                        }
                                    }
                                }
                                string ImageTypeName = "";
                                string FileLocation = oHttpPostedFile.FileName;
                                string TempFileLocation = string.Empty;
                                // string givenpath = Server.MapPath("~\\UploadDownload\\UploadTemp");
                                TempFileLocation = "";
                                if (_SaveOn == "0")
                                {

                                    dt_File.Rows.Add(CtrlID, FileLocation, ImageType, ImageURL.ToLower(), TempFileLocation, ImageTypeName);
                                }
                                else if (_SaveOn == "1")
                                {

                                    dt_File.Rows.Add(CtrlID, FileLocation, ImageType, FinalFileLocation, TempFileLocation, ImageTypeName);
                                }
                                dt_File.AcceptChanges();
                                oHttpPostedFile.SaveAs(FinalFileLocation);
                            }
                        }
                        if (SizeError != "")
                        {
                            dvError.Style.Add("display", "block");
                        }
                        else
                        {
                            dvError.Style.Add("display", "none");
                        }
                    }


                    _FinalFileData = dt_File;
                    clsuploadDemoReceipt obj = new clsuploadDemoReceipt();
                    DataTable POFileData = new DataTable();

                    if (_FinalFileData.Rows.Count > 0)
                    {
                        obj.imagePath = _FinalFileData.Rows[0]["ImageRelativePath"].ToString();
                        obj.WebUserId = PageBase.UserId;
                        obj.RetailerCode = txtRetailorCode.Text.Trim();
                        int strResult = obj.SaveUploadDemoReceipt();

                        if (strResult == 0)
                        {

                            ucMessage1.ShowSuccess("Demo Receipt saved successfully..");
                            dvError.Style.Add("display", "block");
                            txtRetailorCode.Text = "";
                            BindGrid();
                        }
                        else if (strResult == 1)
                        {
                            ucMessage1.ShowError("Retailer Code does not exist, or retailer is inactive");
                            dvError.Style.Add("display", "block");
                        }
                        else
                        {
                            ucMessage1.ShowError("Error..");
                            dvError.Style.Add("display", "block");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string GetCurrentLocation()
    {
        int current_year = DateTime.Now.Year;
        string current_month = DateTime.Now.ToString("MMMM");
        string givenpath = Server.MapPath("~\\UploadDownload\\UploadPersistent");
        string[] diryearnames = Directory.GetDirectories(givenpath);
        string finallocation = string.Empty;
        bool yearfolder = false;
        bool monthfolder = false;
        for (int i = 0; i < diryearnames.Length; i++)
        {
            if (diryearnames[i].EndsWith(current_year.ToString()))
            {
                yearfolder = true;
                /*now check for the curren month*/
                string[] dirmonthnames = Directory.GetDirectories(diryearnames[i]);
                for (int j = 0; j < dirmonthnames.Length; j++)
                {
                    if (dirmonthnames[j].EndsWith(current_month))
                    {
                        finallocation = dirmonthnames[j];
                        monthfolder = true;
                        break;
                    }
                }
                if (monthfolder == true)
                {
                    break;
                }
                if (monthfolder == false && yearfolder == true)
                {
                    /*Create a new month folder*/
                    Directory.CreateDirectory(diryearnames[i] + "\\" + current_month);
                    finallocation = diryearnames[i] + "\\" + current_month;
                    break;
                }
            }
        }
        if (monthfolder == false && yearfolder == false)
        {
            /*Create a new year folder*/
            Directory.CreateDirectory(givenpath + "\\" + current_year);

            /*Create a new month folder*/
            Directory.CreateDirectory(givenpath + "\\" + current_year + "\\" + current_month);
            finallocation = givenpath + "\\" + current_year + "\\" + current_month;
        }
        return finallocation;
    }
    protected void DownloadFile(object sender, EventArgs e)
    {
        try
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
        catch (Exception ex)
        {

        }
        
    }
    private void BindGrid()
    {
        DataTable dtResult = new DataTable();
        try
        {
            using (clsuploadDemoReceipt obj = new clsuploadDemoReceipt())
            {
                obj.WebUserId = PageBase.UserId;


                dtResult = obj.SelectDemoReceiptData();

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    grdDemoReceiptList.DataSource = dtResult;
                    grdDemoReceiptList.DataBind();
                }
                else
                {
                    grdDemoReceiptList.DataSource = null;
                    grdDemoReceiptList.DataBind();

                }
            }
        }
        catch (Exception)
        {
            
            throw;
        }
    }
}
