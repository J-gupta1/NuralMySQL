#region Copyright(c) 2012 Zed-Axis Technologies All rights are reserved
/*

     * ================================================================================================
     *
     * COPYRIGHT (c) 2012 Zed Axis Technologies (P) Ltd.
     * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN
       WHOLE OR IN PART,
     * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR
       OTHERWISE,
     * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
     *
     * ================================================================================================
     * Created By : Vivek Prakash Sharma
     * Modified BY : 
     * Role : Software Programmer
     * Module : User Control For Multiple File Upload
     * ================================================================================================
     * Reviewed By :    
     * ================================================================================================

     */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Configuration;
using BussinessLogic;

public partial class UserControl_ucMultipleFileUpload : System.Web.UI.UserControl
{
    /*
     * Created by Vijay Kumar Prajapati : 21/Nov/2017 , #CC0X 
     */
    //This is Click event defenition for MultipleFileUpload control.
    public event MultipleFileUploadClick Click;

    /// <summary>
    /// To get the final 
    /// panel for the added
    /// file list with image type
    /// </summary>
    public Panel FileListBox
    {
        get
        {
            return pnlListBox;
        }
    }

    public string FinalFileListWithImageType
    {
        get
        {
            return Convert.ToString(hdnFinalList.Value);
        }
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


    private Int64 _intReferenceTypeId = 0;
    public Int64 ReferenceTypeId
    {
        get { return _intReferenceTypeId; }
        set { _intReferenceTypeId = value; }
    }

    /// <summary>
    /// The no of visible rows to display.
    /// </summary>
    private int _Rows = 6;
    public int Rows
    {
        get { return _Rows; }
        set { _Rows = value < 6 ? 6 : value; }
    }

    /// <summary>
    /// The no of maximukm files to upload.
    /// </summary>
    private int _UpperLimit = 0;
    public int UpperLimit
    {
        get { return _UpperLimit; }
        set { _UpperLimit = value; }
    }

    /// <summary>
    /// The Image Type
    /// </summary>
    private int _ImageType = 0;
    private Int16 _Decider = 0;
    public int ImageType
    {
        get { return _ImageType; }
        set { _ImageType = value; }
    }
    public Int16 Decider
    {
        get { return _Decider; }
        set { _Decider = value; }
    }
    public DataTable dtDecider { get; set; }

    private string _SaveOn = Resources.AppConfig.SaveFileOn.ToString();
    public string SaveOn
    {
        get { return _SaveOn; }
        set { _SaveOn = value; }
    }

    /// <summary>
    /// Methos for page load event.
    /// </summary>
    /// <param name="sender">Reference of the object that raises this event.</param>
    /// <param name="e">Contains information regarding page load click event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        lblSizeDisplay.Text = Resources.AppConfig.UploadedImageSize + Resources.AppConfig.UploadedImageSizeType;
        BindImageType();
        lblCaption.Text = _UpperLimit == 0 ? "Maximum Files: No Limit" : string.Format("Maximum Files: {0}", _UpperLimit);
        pnlListBox.Attributes["style"] = "overflow:auto;";
        pnlListBox.Height = Unit.Pixel(20 * _Rows - 1);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "MyScript", GetJavaScript());

    }
    /// <summary>
    /// Method to get the 
    /// active Image Types for the 
    /// Drop Down
    /// </summary>
    private void BindImageType()
    {
        try
        {
            if (dtDecider.Rows.Count > 0)
            {
                clsEnquiryDetail obj = new clsEnquiryDetail();
                obj.dtDecider = dtDecider;
                obj.SalesChannelId = PageBase.SalesChanelID;
                obj.LoginUserId = PageBase.UserId;
                ddlImageType.DataSource = obj.GetImageTypes(Decider);
                ddlImageType.DataTextField = "ImageType";
                ddlImageType.DataValueField = "ImageTypeId";

                ddlImageType.DataBind();

                if (ddlImageType.Items.IndexOf(new ListItem(Resources.Messages.SelectFile, "-1")) == 0)
                {
                    ;
                }
                else
                {
                    ddlImageType.Items.Insert(0, new ListItem(Resources.Messages.SelectFile, "-1"));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Methods for btnUpload Click event. 
    /// </summary>
    /// <param name="sender">Reference of the object that raises this event.</param>
    /// <param name="e">Contains information regarding button click event data.</param>
    /// 


    protected void btnUpload_Click(object sender, EventArgs e)
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

            int FileSize = Convert.ToInt32(Resources.AppConfig.UploadedImageSize);
            if (Resources.AppConfig.UploadedImageSizeType.ToUpper() == "KB")
            {
                FileSize = FileSize * 1024;
            }
            else if (Resources.AppConfig.UploadedImageSizeType.ToUpper() == "MB")
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
                    _SaveOn = Resources.AppConfig.SaveFileOn.ToString();
                    if (Resources.AppConfig.SaveFileOn.ToString() == "0")
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
                    DataRow[] drImgTypeName = ((DataSet)ddlImageType.DataSource).Tables[0].Select("ImageTypeId=" + ImageType);
                    string ImageTypeName = "";
                    if (drImgTypeName.Length > 0)
                    {

                        ImageTypeName = oHttpPostedFile.FileName + " (" + drImgTypeName[0]["ImageType"].ToString() + " )";
                    }
                    string FileLocation = oHttpPostedFile.FileName;
                    string TempFileLocation = string.Empty;
                  //  string givenpath = Server.MapPath("~\\UploadDownload\\UploadPersistent\\QueryManagement");
                    TempFileLocation = FinalFileLocation;


                    if (_SaveOn == "0")
                    {

                        dt_File.Rows.Add(CtrlID, FileLocation, ImageType, ImageURL.ToLower(), TempFileLocation, ImageTypeName);
                    }
                    else if (_SaveOn == "1")
                    {

                        dt_File.Rows.Add(CtrlID, FileLocation, ImageType, FinalFileLocation, TempFileLocation, ImageTypeName);
                    }
                    dt_File.AcceptChanges();

                    oHttpPostedFile.SaveAs(TempFileLocation);
                }
            }
            if (SizeError != "")
            {
                lblError.Text = Resources.AppConfig.UploadImageError + ":-" + SizeError;
                dvError.Style.Add("display", "block");
            }
            else
            {
                dvError.Style.Add("display", "none");
            }
        }


        _FinalFileData = dt_File;
        // Fire the event.
        Click();

    }

    private string GetCurrentLocation()
    {
        int current_year = DateTime.Now.Year;
        string current_month = DateTime.Now.ToString("MMMM");
        string givenpath = Server.MapPath("~\\UploadDownload\\UploadPersistent\\QueryManagement");
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

    /// <summary>
    /// This method is used to generate javascript code for MultipleFileUpload control that execute at client side.
    /// </summary>
    /// <returns>Javascript as a string object.</returns>
    private string GetJavaScript()
    {
        StringBuilder JavaScript = new StringBuilder();

        JavaScript.Append("<script type='text/javascript'>");
        JavaScript.Append("var Id = 0;\n");
        JavaScript.Append("var decider = 0;\n");
        JavaScript.AppendFormat("var MAX = {0};\n", _UpperLimit);
        JavaScript.AppendFormat("var node = document.getElementById('UserControlMultipleFileUpload_IpFile')\n");

        JavaScript.AppendFormat("var DivFiles = document.getElementById('{0}');\n", pnlFiles.ClientID);
        JavaScript.AppendFormat("var DivListBox = document.getElementById('{0}');\n", pnlListBox.ClientID);
        JavaScript.AppendFormat("var BtnAdd = document.getElementById('{0}');\n", btnAdd.ClientID);
        JavaScript.AppendFormat("var ImageType = document.getElementById('{0}');\n", ddlImageType.ClientID);
        JavaScript.AppendFormat("var hdnFinalList = document.getElementById('{0}');\n", hdnFinalList.ClientID);

        JavaScript.Append("function Add()");
        JavaScript.Append("{\n");
        JavaScript.Append("if(ImageType.options[ImageType.selectedIndex].value==-1)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("alert('Please select a Image Type.');\n");
        JavaScript.Append("return;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("var IpFile = GetTopFile();\n");

        JavaScript.Append("if(IpFile == false)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("alert('Please Upload Correct File');\n");
        JavaScript.Append("return;\n");
        JavaScript.Append("}\n");

        JavaScript.Append("var SplitsforName = IpFile.value.split('\\\\');\n");

        JavaScript.Append("var FileNameforLength = (SplitsforName[SplitsforName.length - 1]);\n");
        JavaScript.Append("for (var i = 0; i < _validFileExtensions.length; i++)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("FileNameforLength = FileNameforLength.replace(_validFileExtensions[i],'');\n");
        JavaScript.Append("}\n");
        JavaScript.Append("if(FileNameforLength.length>100)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("alert('file Name can not be more than 100 characters');\n");
        JavaScript.Append("return;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("if(IpFile == null || IpFile.value == null || IpFile.value.length == 0)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("alert('Please select a file to add.');\n");

        JavaScript.Append("return;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("var NewIpFile = CreateFile();\n");
        JavaScript.Append("DivFiles.insertBefore(NewIpFile,IpFile);\n");
        JavaScript.Append("if(MAX != 0 && GetTotalFiles() - 1 == MAX)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("NewIpFile.disabled = true;\n");
        JavaScript.Append("BtnAdd.disabled = true;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("IpFile.style.display = 'none';\n");
        JavaScript.Append("DivListBox.appendChild(CreateItem(IpFile));\n");
        JavaScript.Append("}\n");
        JavaScript.Append("function CreateFile()");
        JavaScript.Append("{\n");
        JavaScript.Append("var IpFile = document.createElement('input');\n");
        JavaScript.Append("IpFile.id = IpFile.name ='IpFile_' + Id++;\n");
        JavaScript.Append("IpFile.type = 'file';\n");
        JavaScript.Append("IpFile.style.width = '300px';\n");
        JavaScript.Append("IpFile.style.border = '1px solid #7e95d4';\n");
        JavaScript.Append("IpFile.style.fontSize = '11px';\n");
        JavaScript.Append("return IpFile;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("function CreateItem(IpFile)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("var Item = document.createElement('div');\n");
        JavaScript.Append("Item.style.backgroundColor = '#ffffff';\n");
        JavaScript.Append("Item.style.fontWeight = 'normal';\n");
        JavaScript.Append("Item.style.fontSize = '12px';\n");
        JavaScript.Append("Item.style.padding = '5px';\n");
        JavaScript.Append("Item.style.textAlign = 'left';\n");
        JavaScript.Append("Item.style.verticalAlign = 'middle'; \n");
        JavaScript.Append("Item.style.cursor = 'default';\n");
        JavaScript.Append("Item.style.height = 20 + 'px';\n");
        JavaScript.Append("var Splits = IpFile.value.split('\\\\');\n");



        JavaScript.Append("Item.innerHTML =  ImageType.options[ImageType.selectedIndex].text +'&nbsp;&nbsp;:&nbsp;&nbsp;'+ Splits[Splits.length - 1] + '&nbsp;';\n");
        JavaScript.Append("Item.value = IpFile.id;\n");
        JavaScript.Append("Item.title = IpFile.value;\n");
        JavaScript.Append("hdnFinalList.value += IpFile.id+'|'+ImageType.options[ImageType.selectedIndex].value+'?';\n");

        JavaScript.Append("var A = document.createElement('a');\n");
        JavaScript.Append("A.innerHTML = 'Delete';\n");
        JavaScript.Append("A.id = 'A_' + Id++;\n");
        JavaScript.Append("A.href = '#';\n");
        JavaScript.Append("A.style.color = '#2aabe4';\n");
        JavaScript.Append("A.onclick = function()\n");
        JavaScript.Append("{\n");
        JavaScript.Append("DivFiles.removeChild(document.getElementById(this.parentNode.value));\n");
        JavaScript.Append("DivListBox.removeChild(this.parentNode);\n");
        JavaScript.Append("if(MAX != 0 && GetTotalFiles() - 1 < MAX)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("GetTopFile().disabled = false;\n");
        JavaScript.Append("BtnAdd.disabled = false;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("}\n");
        JavaScript.Append("Item.appendChild(A);\n");
        JavaScript.Append("Item.onmouseover = function()\n");
        JavaScript.Append("{\n");
        JavaScript.Append("Item.bgColor = Item.style.backgroundColor;\n");
        JavaScript.Append("Item.fColor = Item.style.color;\n");
        JavaScript.Append("Item.style.backgroundColor = '#8C8C8C';\n");
        JavaScript.Append("Item.style.color = '#ffffff';\n");
        JavaScript.Append("Item.style.fontWeight = 'bold';\n");
        JavaScript.Append("Item.style.padding = '5px';\n");
        JavaScript.Append("Item.style.fontSize = '12px';\n");
        JavaScript.Append("}\n");
        JavaScript.Append("Item.onmouseout = function()\n");
        JavaScript.Append("{\n");
        JavaScript.Append("Item.style.backgroundColor = Item.bgColor;\n");
        JavaScript.Append("Item.style.color = Item.fColor;\n");
        JavaScript.Append("Item.style.fontWeight = 'normal';\n");
        JavaScript.Append("Item.style.padding = '5px';\n");
        JavaScript.Append("Item.style.fontSize = '12px';\n");
        JavaScript.Append("}\n");
        JavaScript.Append("return Item;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("function Clear()\n");
        JavaScript.Append("{\n");
        JavaScript.Append("DivListBox.innerHTML = '';\n");
        JavaScript.Append("DivFiles.innerHTML = '';\n");
        JavaScript.Append("DivFiles.appendChild(CreateFile());\n");
        JavaScript.Append("BtnAdd.disabled = false;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("function GetTopFile()\n");
        JavaScript.Append("{\n");
        JavaScript.Append("var Inputs = DivFiles.getElementsByTagName('input');\n");
        JavaScript.Append("var IpFile = null;\n");
        JavaScript.Append("for(var n = 0; n < Inputs.length && Inputs[n].type == 'file'; ++n)\n");
        JavaScript.Append("{\n");



        JavaScript.Append("for(var j = 0; j < _validFileExtensions.length; j++)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("var sCurExtension = _validFileExtensions[j];\n");
        JavaScript.Append("if((Inputs[n].value.substr(Inputs[n].value.lastIndexOf('.'), Inputs[n].value.length)).toLowerCase() == sCurExtension.toLowerCase())\n");
        JavaScript.Append("{\n");
        JavaScript.Append("decider = 1;\n");
        JavaScript.Append("break;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("else\n");
        JavaScript.Append("decider = 0;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("if(decider==0)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("return false;\n");
        JavaScript.Append("}\n");


        JavaScript.Append("IpFile = Inputs[n];\n");

        JavaScript.Append("break;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("return IpFile;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("function GetTotalFiles()\n");
        JavaScript.Append("{\n");
        JavaScript.Append("var Inputs = DivFiles.getElementsByTagName('input');\n");
        JavaScript.Append("var Counter = 0;\n");
        JavaScript.Append("for(var n = 0; n < Inputs.length && Inputs[n].type == 'file'; ++n)\n");
        JavaScript.Append("Counter++;\n");
        JavaScript.Append("return Counter;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("function GetTotalItems()\n");
        JavaScript.Append("{\n");
        JavaScript.Append("var Items = DivListBox.getElementsByTagName('div');\n");
        JavaScript.Append("return Items.length;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("function DisableTop()\n");
        JavaScript.Append("{\n");
        JavaScript.Append("if(GetTotalItems() == 0)\n");
        JavaScript.Append("{\n");
        JavaScript.Append("alert('Please browse at least one file to upload.');\n");
        JavaScript.Append("return false;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("GetTopFile().disabled = true;\n");
        JavaScript.Append("return true;\n");
        JavaScript.Append("}\n");
        JavaScript.Append("</script>\n");



        return JavaScript.ToString();
    }

    protected void btnSaveImage_Click(object sender, EventArgs e)
    {
        try
        {

            clsEnquiryDetail obj = new clsEnquiryDetail();
            DataTable POFileData = new DataTable();
            using (DataSet ds = new DataSet())
            {
                if (Session["FinalFileData"] != null)
                {

                    POFileData = (DataTable)Session["FinalFileData"];
                    ds.Tables.Add(POFileData);
                    obj.WOFileXML = ds.GetXml();

                    obj.ReferenceType_id = ReferenceTypeId;

                    int strResult = obj.SaveImgSaperateByProcess();

                    if (strResult == 0)
                    {
                        MoveFileFromTemp(ds.Tables[0]);
                        ucMessage1.ShowSuccess("Images saved successfully..");
                        dvError.Style.Add("display", "block");
                        btnSaveImage.Visible = false;
                    }
                    else
                    {
                        ucMessage1.ShowError("Error..");
                        dvError.Style.Add("display", "block");
                    }

                }
            }
            Session.Remove("FinalFileData");

            ListBox ltImageDetails = Parent.FindControl("lsDetailImages") as ListBox;
            ltImageDetails.Items.Clear();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            dvError.Style.Add("display", "block");
        }

    }
    public void MoveFileFromTemp(DataTable dataTable)
    {

        try
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string strImageConString = ConfigurationManager.AppSettings["AzureConnectionString"].ToString();
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(strImageConString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(Resources.AppConfig.AzureContainerName);
                container.CreateIfNotExist();
                string[] Blog = dataTable.Rows[i]["ImageRelativePath"].ToString().ToLower().Split(':');
                string NewBlob = Blog[1].ToLower();
                CloudBlob blob = container.GetBlobReference(NewBlob);
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                using (var fileStream = System.IO.File.OpenRead(dataTable.Rows[i]["TempFileLocation"].ToString()))
                {
                    blob.UploadFromStream(fileStream);
                }
                File.Delete(dataTable.Rows[i]["TempFileLocation"].ToString());
            }
        }
        catch (Exception ex)
        {
            // clsException.fncHandleException(ex, webuserid, 1);

        }

    }
    public void MoveFileFromTemp(DataTable dataTable, int Server)
    {
        try
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                File.Move(dataTable.Rows[i]["TempFileLocation"].ToString(), dataTable.Rows[i]["ImageRelativePath"].ToString());
            }
        }
        catch (Exception ex)
        {
            // clsException.fncHandleException(ex, webuserid);
        }
    }
}

/// <summary>
/// EventArgs class that has some readonly properties regarding posted files corresponding to MultipleFileUpload control. 
/// </summary>
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
public delegate void MultipleFileUploadClick();
