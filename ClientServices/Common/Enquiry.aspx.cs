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
* Created On: 04-Sept-2017 
 * Description: This is a Distributor Query and Admin Query Page..
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 29-Aug-2018, Kalpana, #CC02: Width change "250px" to "220px"
 * 23-Oct-2018, Sumit Maurya, #CC03, Interface further modified for enquiryies created from API, (Fields are non mandatory) (Done for ZedsalesV5 TSM APP)
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




using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.ASPxClasses.Internal;
using System.IO;
using System.Web.Caching;
/*#CC07 Added start*/
using System.Configuration;
using System.Net;
using System.Collections.Specialized;
/*12 May 2018, Rajnish Kumar, #CC01, Changes for image savings, and Upload Control*/     
public partial class ClientServices_Common_Enquiry : PageBase
{
    public DataTable _GetEnquiryDetailId;
    public Int64 EnqueryDetailId { get; set; }
    DataTable dtApprovalDetails;
    DataTable dtApprovalDetailsAdmin;
    DataTable dtApprovalRemarksImage;
    DataTable dtImageData; /*#CC01*/  
    DataTable dtImageDataActual; /*#CC01*/  
    clsEnquiryDetail obj = new clsEnquiryDetail();
    protected string strAssets = PageBase.strAssets;
    private int _ImageType = 0; /*#CC01*/  
    private Int16 _Decider = 0;
    public string XMLImage
    {
        get;
        set;

    }
    public DataTable dtDecider { get; set; }
    public Int16 Decider
    {
        get { return _Decider; } /*#CC01*/
        set { _Decider = value; } /*#CC01*/  
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //    //dtDecider.Columns.Add("ImageTypeId", typeof(short));

            //    //    DataSet ds = new DataSet();

            //    //    obj.ImageLoadType = Convert.ToInt32(strLoadFor); 
            //    //    ds = obj.GetImageLoadType();

            //    //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    //    {
            //    //        DataRow dtr = dtDecider.NewRow();
            //    //        dtr["ImageTypeId"] = Convert.ToInt16(ds.Tables[0].Rows[i]["ImageTypeId"].ToString());
            //    //        dtDecider.Rows.Add(dtr);
            //    //        dtDecider.AcceptChanges();
            //    //        UpperLimit++; 
            //    //    }
            //        //UserControlMultipleFileUpload.dtDecider = dtFatchDecider;
            if (!IsPostBack)
            {
                Session["Table"] = null;

                ucMessage1.Visible = false;
                Int64 SalesChannelId = PageBase.SalesChanelID;
                Int64 RetailerRoleId = PageBase.RoleID;
                BindImageType(); /*#CC01*/
                PnlAdminAttachement.Visible = false;
                if ((SalesChannelId > 0) && (SalesChannelLevel == 2))
                {
                    ucDateFrom.Date = PageBase.Fromdate;
                    ucDateTo.Date = PageBase.ToDate;
                    BindCategoryType();
                    GetQueryType();
                    QueryTypeDistributor();
                    ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
                    btnDownLoadQueryReport.Visible = false;
                   // PnlAdminAttachement.Visible = true;
                    PnlAdmin.Visible = true;
                    GetDistributorCode();
                    lblStatus.Visible = false;
                    ddlStatus.Visible = false;
                    PnlSearch.Visible = false;
                    dvPnlRemarksGrid.Visible = false;
                }
                else if ((SalesChannelId > 0) && (SalesChannelLevel == 3))
                {
                    ucDateFrom.Date = PageBase.Fromdate;
                    ucDateTo.Date = PageBase.ToDate;
                    BindCategoryType();
                    GetQueryType();
                    QueryTypeRDS();
                    ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
                    btnDownLoadQueryReport.Visible = false;
                   // PnlAdminAttachement.Visible = true;
                    PnlAdmin.Visible = true;
                    GetDistributorCode();
                    lblStatus.Visible = false;
                    ddlStatus.Visible = false;
                    PnlSearch.Visible = false;
                    dvPnlRemarksGrid.Visible = false;
                }
                //else if (RetailerRoleId == 47)
                //{
                //    ucDateFrom.Date = PageBase.Fromdate;
                //    ucDateTo.Date = PageBase.ToDate;
                //    BindCategoryType();
                //    GetQueryType();
                //    QueryTypeDistributor();
                //    ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
                //    btnDownLoadQueryReport.Visible = false;
                //    PnlAdminAttachement.Visible = true;
                //    PnlAdmin.Visible = true;
                //    GetDistributorCode();
                //    lblStatus.Visible = false;
                //    ddlStatus.Visible = false;
                //    PnlSearch.Visible = false;
                //    dvPnlRemarksGrid.Visible = false;
                //}
                else
                {
                    // DisableControlForAdmin();
                    BindCategoryType();
                    GetQueryType();
                    QueryTypeAdmin();
                    GetQueryType();
                   // PnlAdminAttachement.Visible = true;
                    ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
                    PnlAdmin.Visible = false;
                    GetDistributorCode();
                    ucDateFrom.Date = PageBase.Fromdate;
                    ucDateTo.Date = PageBase.ToDate;
                    PnlSearch.Visible = false;
                    dvPnlRemarksGrid.Visible = false;
                }
            }
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    private void BindCategoryType()
    {

        using (clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
        {
            try
            {
                DataSet ds = ObjMapping.SelectCategoryType();

                DataTable dt = ds.Tables[0];
                ddlCategoryType.DataSource = dt;

                ddlCategoryType.DataTextField = "EnquiryCategoryName";
                ddlCategoryType.DataValueField = "EnquiryCategoryId";


                ddlCategoryType.DataBind();
                ddlCategoryType.Items.Insert(0, new ListItem("Select", "0"));

            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.Message.ToString());
            }
        }
    }
    private void BindSubCategory()
    {

        using (clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
        {
            try
            {
                ObjMapping.CategoryTypeId = Convert.ToInt16(ddlCategoryType.SelectedValue.ToString());
                DataSet ds = ObjMapping.SelectSubCategory();

                DataTable dt = ds.Tables[0];

                ddlSubCategory.DataSource = dt;

                ddlSubCategory.DataTextField = "EnquiryType";
                ddlSubCategory.DataValueField = "EnquiryTypeMasterID";


                ddlSubCategory.DataBind();
                ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));

            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.Message.ToString());
            }
        }
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
  /*#CC01*/  string SavePostedFiles(UploadedFile uploadedFile)
    {
       
        //upSearchResults.Update();
        //UpdImage.Update();
        string strFilePath;
        //if (Convert.ToString(Session["DocTypeId"]) == "")
        //{
        //    //ucMessage1.ShowWarning("Please select upload document type.");
        //    //ucMessage1.Visible = true;
        //    return string.Empty;

        //}
        strFilePath = ZedService.Utility.ZedServiceUtil.GetUploadFilePath(Convert.ToDateTime(System.DateTime.Now),  "../../UploadDownload/UploadPersistent/QueryManagement/");
        if (!uploadedFile.IsValid)
            return string.Empty;
        FileInfo fileInfo = new FileInfo(uploadedFile.FileName);
        string strTicks = System.DateTime.Now.Ticks.ToString();

        string strFileUploadedName = strTicks + fileInfo.Name;
        //string strFileUploadedName = fileInfo.Name;
        string strTempPath = Server.MapPath("../../UploadDownload/UploadPersistent/QueryManagement/");


        if (!Directory.Exists(Server.MapPath(strFilePath)))
            Directory.CreateDirectory(Server.MapPath(strFilePath));


        uploadedFile.SaveAs(strTempPath + strFileUploadedName);
        if (Session["Table"] == null)
        {
            dtImageDataActual = new DataTable();
            dtImageDataActual = CreateImageDataTable();

            DataRow dr = dtImageDataActual.NewRow();
            
          
            dr["FileLocation"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;
            //dr["ImageTypeId"] = Convert.ToInt16(ddlImageType.SelectedValue);
            dr["ImageTypeId"] = 1;
            dr["ImageRelativePath"] = strTempPath + strFileUploadedName;
            dr["TempFileLocation"] = strTempPath + strFileUploadedName;
            dr["ImageTypeName"] = ddlImageType.SelectedItem;
            //dr["BinaryChangedImage"] = ImageToBinary(strTempPath + strFileUploadedName);
            //dr["UploadDocTypeId"] = Convert.ToInt16(Session["DocTypeId"]) == 0 ? 2 : Convert.ToInt16(Session["DocTypeId"]);
            dtImageDataActual.Rows.Add(dr);
            dtImageDataActual.AcceptChanges();
            Session["Table"] = dtImageDataActual;

        }
        else
        {
            dtImageDataActual = (DataTable)Session["Table"];
            DataRow dr = dtImageDataActual.NewRow();
           
            dr["FileLocation"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;
            dr["ImageTypeId"] = 1;
            dr["ImageRelativePath"] = strTempPath + strFileUploadedName;
            dr["TempFileLocation"] = strTempPath + strFileUploadedName;
            dr["ImageTypeName"] = ddlImageType.SelectedItem;
            dtImageDataActual.Rows.Add(dr);
            dtImageDataActual.AcceptChanges();
            Session["Table"] = dtImageDataActual;

            
        }

        string fileLabel = fileInfo.Name;
        string fileLength = uploadedFile.FileBytes.Length / 1024 + "K";

        return string.Format("{0} ({1})|{2}", fileLabel, fileLength, VirtualPath + "/UploadDownload/UploadPersistent/" + strFileUploadedName);

    }
    protected void finalsubmission_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();       
        Int64 SalesChanalid = PageBase.SalesChanelID;
        Int64 RetailerRoleId = PageBase.RoleID;
        try
        {
            DataSet ds = new DataSet();
            DataTable FinalFileData = new DataTable();
            if (Session["Table"] != null)
            {
                FinalFileData = (DataTable)Session["Table"];
                FinalFileData.TableName = "Table1";
                dt = FinalFileData.Copy();
                ds.Tables.Add(dt);
            }
            XMLImage = ds.GetXml();
            //if (UcExecutiveRemark.TextBoxText.Trim() == "")
            //{
            //    ucMessage1.ShowError("Please Enter Description!");
            //    return;
            //}
            if (UcExecutiveRemark.Text.Trim() == "")
            {
                ucMessage1.ShowError("Please Enter Description!");
                Session["Table"] = null;
                return;
            }
            else
            {     
                Int64 Enquirydetailid = 0;
                if (hdnEnquiryid.Value != "")
                {
                    Enquirydetailid = Convert.ToInt64(hdnEnquiryid.Value);
                }
                using (clsEnquiryDetail ObjDetail = new clsEnquiryDetail())
                {
                    ObjDetail.LoginUserId = PageBase.UserId;
                    ObjDetail.SalesChannelId = PageBase.SalesChanelID;
                    if (SalesChanalid == 0)
                    {
                        //if (RetailerRoleId == 47)
                        //{
                        //    ObjDetail.SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue.ToString());
                        //    ObjDetail.contactnumber = txtContactNumber.Text.Trim();
                        //    ObjDetail.name = txtName.Text.Trim();
                        //    ObjDetail.Emailid = txtEmail.Text.Trim();
                        //    if (ddlStatus.SelectedValue == "255")
                        //    {
                        //        ucMessage1.ShowWarning("Please Select Status");
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        //    ObjDetail.SubCategoryId = 0;
                        //    ObjDetail.contactnumber = null;
                        //    ObjDetail.name = null;
                        //    ObjDetail.Emailid = null;
                        //    if (ddlStatus.SelectedValue == "255")
                        //    {
                        //        ucMessage1.ShowWarning("Please Select Status");
                        //        return;
                        //    }
                        //}

                        ObjDetail.SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue.ToString());
                        ObjDetail.contactnumber = txtContactNumber.Text.Trim();
                        ObjDetail.name = txtName.Text.Trim();
                        ObjDetail.Emailid = txtEmail.Text.Trim();
                        if (ddlStatus.SelectedValue == "255")
                        {
                            ucMessage1.ShowWarning("Please Select Status");
                            Session["Table"] = null;
                            return;
                        }
                    }
                    else
                    {
                        if (clsEnquiryDetail.IsDecimal(txtContactNumber.Text.Trim()) == false)
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowError("Please Enter Valid Contact Number.");
                            Session["Table"] = null;
                            return;
                        }
                        else
                        {
                            ObjDetail.SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue.ToString());
                            ObjDetail.name = txtName.Text.Trim();
                            ObjDetail.contactnumber = txtContactNumber.Text.Trim();
                            ObjDetail.Emailid = txtEmail.Text.Trim();
                        }
                    }
                    ObjDetail.QueryType = Convert.ToInt16(ddlStatus.SelectedValue.ToString());
                    //ObjDetail.Description = UcExecutiveRemark.TextBoxText.Trim();

                    ObjDetail.Description = UcExecutiveRemark.Text.Trim();
                    ObjDetail.EnquiryDetailid = Enquirydetailid;
                    ObjDetail.RetailerRoleId = PageBase.RoleID;
                    int result = ObjDetail.Insert();
                    if (SalesChanalid == 0)
                    {
                        if (result == 0)
                        {
                            if (XMLImage.ToString() == "")
                            {

                            }
                            else
                            {
                                using (clsEnquiryDetail objuploadfile = new clsEnquiryDetail())
                                {
                                    int resultimg = 1;
                                    objuploadfile.SalesChannelId = PageBase.SalesChanelID;
                                    objuploadfile.LoginUserId = PageBase.UserId;
                                    objuploadfile.ReferenceType_id = ObjDetail.GetEnquirydetailid;
                                    objuploadfile.WOFileXML = XMLImage;
                                    resultimg = objuploadfile.SaveImgSaperateByProcess();
                                    if (resultimg == 0)
                                    {
                                        PageBase objbase = new PageBase();
                                        objbase.MoveFileFromTemp(FinalFileData);
                                        Session["FinalFileData"] = null;
                                    }
                                }
                            }
                            ucMessage1.Visible = true;
                            ucMessage1.ShowSuccess("Query created successfully!");
                            BindSearchQueryList();
                            BindRemarkGrid();
                            Clear();
                            Session["Table"] = null;
                           
                        }
                        else
                        {
                            ucMessage1.Visible = true;
                           
                            ucMessage1.ShowError("Exception In Procedure/Function[" + ObjDetail.Error + "]");
                            Session["Table"] = null;
                        }
                    }
                    //else if (RetailerRoleId == 47)
                    //{
                    //    if (result == 0)
                    //    {
                    //        if (XMLImage.ToString() == "")
                    //        {

                    //        }
                    //        else
                    //        {
                    //            using (clsEnquiryDetail objuploadfile = new clsEnquiryDetail())
                    //            {
                    //                int resultimg = 1;
                    //                objuploadfile.SalesChannelId = PageBase.SalesChanelID;
                    //                objuploadfile.LoginUserId = PageBase.UserId;
                    //                objuploadfile.ReferenceType_id = ObjDetail.GetEnquirydetailid;
                    //                objuploadfile.WOFileXML = XMLImage;
                    //                resultimg = objuploadfile.SaveImgSaperateByProcess();
                    //                if (resultimg == 0)
                    //                {
                    //                    PageBase objbase = new PageBase();
                    //                    objbase.MoveFileFromTemp(FinalFileData);
                    //                    Session["FinalFileData"] = null;
                    //                }
                    //            }
                    //        }
                    //        ucMessage1.Visible = true;
                    //        ucMessage1.ShowSuccess("Query created successfully!");
                    //        BindSearchQueryList();
                    //        BindRemarkGrid();
                    //        Clear();

                    //    }
                    //    else
                    //    {
                    //        ucMessage1.Visible = true;
                    //        ucMessage1.ShowError("Exception In Procedure/Function[" + ObjDetail.Error + "]");
                    //    }
                    //}
                    else
                    {
                        if (result == 0)
                        {
                            ucMessage1.Visible = true;
                            if (Enquirydetailid > 0)
                            {
                                if (XMLImage.ToString() == "")
                                {

                                }
                                else
                                {
                                    using (clsEnquiryDetail objuploadfile = new clsEnquiryDetail())
                                    {
                                        int resultimg = 1;
                                        objuploadfile.SalesChannelId = PageBase.SalesChanelID;
                                        objuploadfile.LoginUserId = PageBase.UserId;
                                        objuploadfile.ReferenceType_id = ObjDetail.GetEnquirydetailid;
                                        objuploadfile.WOFileXML = XMLImage;
                                        resultimg = objuploadfile.SaveImgSaperateByProcess();
                                        if (resultimg == 0)
                                        {
                                            PageBase objbase = new PageBase();
                                            objbase.MoveFileFromTemp(FinalFileData);
                                            Session["FinalFileData"] = null;
                                        }
                                    }
                                }
                                ucMessage1.ShowSuccess("Enquiry created successfully.");
                                BindSearchQueryList();
                                BindRemarkGrid();
                                Clear();
                                Session["Table"] = null;
                               
                            }
                            else
                            {
                                if (XMLImage.ToString() == "")
                                {

                                }
                                else
                                {
                                    using (clsEnquiryDetail objuploadfile = new clsEnquiryDetail())
                                    {
                                        int resultimg = 1;
                                        objuploadfile.SalesChannelId = PageBase.SalesChanelID;
                                        objuploadfile.LoginUserId = PageBase.UserId;
                                        objuploadfile.ReferenceType_id = ObjDetail.GetEnquirydetailid;
                                        objuploadfile.WOFileXML = XMLImage;
                                        resultimg = objuploadfile.SaveImgSaperateByProcess();
                                        if (resultimg == 0)
                                        {
                                            PageBase objbase = new PageBase();
                                            objbase.MoveFileFromTemp(FinalFileData);
                                            Session["FinalFileData"] = null;
                                        }
                                    }
                                }
                                ucMessage1.ShowSuccess("Enquiry created successfully with [" + ObjDetail.EnquiryNumber + "] ");
                                BindSearchQueryList();
                                Clear();
                                Session["Table"] = null;
                            }
                        }
                        else
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowError(" " + ObjDetail.Error + " ");
                            Session["Table"] = null;
                           // ucMessage1.ShowError("Exception In Procedure/Function[" + ObjDetail.Error + "]");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());

        }

    }


    /*#CC01*/
    DataTable CreateImageDataTable()
    {

       
        dtImageData = new DataTable();
        DataColumn dc = new DataColumn("CtrlID");
        dc.DataType = System.Type.GetType("System.Int32");
        dc.AutoIncrement = true;
        dc.AutoIncrementSeed = 1;
        dtImageData.Columns.Add(dc);
        dtImageData.Columns.Add("FileLocation", typeof(string));
        dtImageData.Columns.Add("TempFileLocation", typeof(string));
        dtImageData.Columns.Add("ImageTypeId", typeof(Int16));
        dtImageData.Columns.Add("ImageRelativePath", typeof(string));
       
        dtImageData.Columns.Add("ImageTypeName", typeof(string));
        return dtImageData;
    }

    /*#CC01*/
    private void BindImageType()
    {
        try
        {
          
                clsEnquiryDetail obj = new clsEnquiryDetail();
                obj.dtDecider = dtDecider;
                obj.SalesChannelId = PageBase.SalesChanelID;
                obj.LoginUserId = PageBase.UserId;
                ddlImageType.DataSource = obj.GetImageTypesV1(Decider);
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
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void Clear()
    {
        try
        {
            Int64 SalesChanelID = PageBase.SalesChanelID;
            Int64 RetailerRoleId = PageBase.RoleID;
            if (SalesChanelID == 0 || RetailerRoleId==47)
            {
                //UcExecutiveRemark.TextBoxText = "Enter Description";
                UcExecutiveRemark.Text = "Enter Description";
               
            }
            else
            {
                //UcExecutiveRemark.TextBoxText = "Enter Description";
                UcExecutiveRemark.Text = "Enter Description";
                ddlSubCategory.SelectedIndex = 0;
                ddlCategoryType.SelectedIndex = 0;
                txtContactNumber.Text = "";
                txtName.Text = "";
                txtEmail.Text = "";
                Session["Table"] = null; /*#CC01*/  
                //ucMessage1.ShowSuccess(" ");
                ddlImageType.SelectedIndex = 0;
                PnlAdminAttachement.Visible = false;

               
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
            Session["Table"] = null; /*#CC01*/  
        }
    }
    protected void ddlCategoryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSubCategory();
        ucMessage1.Visible = false;
    }
    private void GetQueryType()
    {
        DataTable dtquery = new DataTable();
        try
        {
            dtquery = clsEnquiryDetail.GetEnumByTableName("XML_Enum", "QueryStatusType");
            ddlQueryStatus.DataSource = dtquery;
            ddlQueryStatus.DataTextField = "Description";
            ddlQueryStatus.DataValueField = "Value";
            ddlQueryStatus.DataBind();
            ddlQueryStatus.Items.Insert(0, new ListItem("Select", "255"));
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {

        ucMessage1.Visible = false;
        grdQueryList.Visible = true;
        PnlSearch.Visible = true;
        try
        {
            if (ucDateFrom.Date == "" && ucDateTo.Date == "")
            {
                ucMessage1.ShowError("Please Fill From Date and To Date.");
                return;
            }
            else
            {
                BindSearchQueryList();
                UpdSearch.Update();
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
        }
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
            ucMessage1.ShowError(ex.Message.ToString());
        }

    }
    private void BindRemarkGrid()
    {
        DataTable dtresult = new DataTable();
        try
        {
            using (clsEnquiryDetail objdetail = new clsEnquiryDetail())
            {

                objdetail.LoginUserId = PageBase.UserId;
                objdetail.SalesChannelId = PageBase.SalesChanelID;
                Int64 Enquirydetailid = 0;
                if (hdnEnquiryid.Value != "")
                {
                    Enquirydetailid = Convert.ToInt64(hdnEnquiryid.Value);
                }
                objdetail.EnquiryDetailid = Enquirydetailid;
                dtresult = objdetail.GetAllQuery();
                if (dtresult.Rows.Count > 0)
                {

                    grdDescriptionlist.DataSource = dtresult;
                    grdDescriptionlist.DataBind();

                }
                else
                {
                    grdDescriptionlist.DataSource = null;
                    grdDescriptionlist.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    private void QueryTypeAdmin()
    {
        DataTable dtquery = new DataTable();
        try
        {
            dtquery = clsEnquiryDetail.GetEnumByTableName("XML_Enum", "QueryTypeAdmin");
            ddlStatus.DataSource = dtquery;
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataValueField = "Value";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("Select", "255"));
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    private void QueryTypeDistributor()
    {
        DataTable dtquery = new DataTable();
        try
        {
            dtquery = clsEnquiryDetail.GetEnumByTableName("XML_Enum", "QueryTypeDistributor");
            ddlStatus.DataSource = dtquery;
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataValueField = "Value";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }

    private void QueryTypeRDS()
    {
        DataTable dtquery = new DataTable();
        try
        {
            dtquery = clsEnquiryDetail.GetEnumByTableName("XML_Enum", "QueryTypeRDS");
            ddlStatus.DataSource = dtquery;
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataValueField = "Value";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void DownloadQueryFile(object sender, EventArgs e)
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
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void grdDescriptionlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int64 EnueiryId = Convert.ToInt64(hdfenqurydetailid.Value);
                Int64 EnquiryDetailRemarkid = Convert.ToInt64(grdDescriptionlist.DataKeys[e.Row.RowIndex].Value);
                if (EnueiryId != 0)
                {
                    using (clsEnquiryDetail objdetail = new clsEnquiryDetail())
                    {
                        objdetail.LoginUserId = PageBase.UserId;
                        objdetail.SalesChannelId = PageBase.SalesChanelID;
                        objdetail.EnquiryDetailid = EnueiryId;
                        dtApprovalRemarksImage = objdetail.GetRemarksImagePath();
                    }
                    DataRow[] drv = dtApprovalRemarksImage.Select("EnquiryDetailRemarkid=" + EnquiryDetailRemarkid);
                    if (drv.Length > 0)
                    {
                        DataTable dtTemp = dtApprovalRemarksImage.Clone();

                        for (int cntr = 0; cntr < drv.Length; cntr++)
                        {
                            dtTemp.ImportRow(drv[cntr]);
                        }

                        GridView gvAttachedRemarksImages = (GridView)e.Row.FindControl("gvAttachedRemarksImages");
                        gvAttachedRemarksImages.DataSource = dtTemp;
                        gvAttachedRemarksImages.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }

    }
    protected void grdDescriptionlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdDescriptionlist.PageIndex = e.NewPageIndex;
        BindRemarkGrid();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ddlCategoryType.Enabled = true;
            ddlSubCategory.Enabled = true;
            ddlStatus.Enabled = true;
            txtContactNumber.Enabled = true;
            txtName.Enabled = true;
            txtEmail.Enabled = true;
            ddlCategoryType.SelectedValue = "0";
            ddlSubCategory.Items.Clear();
            ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
            ddlSubCategory.SelectedValue = "0";
            ddlStatus.SelectedValue = "0";
            txtContactNumber.Text = "";
            txtName.Text = "";
            ddlQueryStatus.SelectedValue = "255";
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            txtEmail.Text = "";
            hdnEnquiryid.Value = "";
            dvPnlRemarksGrid.Visible = false;
            PnlSearch.Visible = false;
            ucMessage1.ShowSuccess("");
            Response.Redirect("Enquiry.aspx");
            Int64 saleschannelid = PageBase.SalesChanelID;
            if (saleschannelid > 0)
            {
                PnlAdmin.Visible = true;
                grdDescriptionlist.Visible = false;
                grdQueryList.Visible = false;
                lblStatus.Visible = false;
                ddlStatus.Visible = false;
            }
            else
            {
                PnlAdmin.Visible = false;
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message);
        }


    }
    protected void grdQueryList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdQueryList.PageIndex = e.NewPageIndex;
        BindSearchQueryList();
        //UpdSearch.Update();

    }
    private void BindSearchQueryList()
    {
        DataSet dsresult = new DataSet();
        try
        {
            using (clsEnquiryDetail objdetail = new clsEnquiryDetail())
            {
                objdetail.FromDate = Convert.ToDateTime(ucDateFrom.Date);
                objdetail.Todate = Convert.ToDateTime(ucDateTo.Date);
                objdetail.QueryType = Convert.ToInt16(ddlQueryStatus.SelectedValue);
                objdetail.Distributorcode = ddlDistributorCode.SelectedValue.ToString();
                objdetail.LoginUserId = PageBase.UserId;
                objdetail.SalesChannelId = PageBase.SalesChanelID;
                objdetail.RetailerRoleId = PageBase.RoleID;
                objdetail.DownLoadReport = 0;
                dsresult = objdetail.SelectQueryResult();
                if(SalesChanelID>0)
                {
                   
                        if (dsresult.Tables[0].Rows.Count > 0)
                        {
                            grdQueryList.DataSource = dsresult.Tables[0];
                            grdQueryList.DataBind();

                        }
                        else
                        {
                            grdQueryList.DataSource = null;
                            grdQueryList.DataBind();
                        }
                   
                }
                else
                {
                    if (dsresult.Tables[0].Rows.Count > 0)
                    {
                        grdQueryList.DataSource = dsresult.Tables[0];
                        grdQueryList.DataBind();

                    }
                    else
                    {
                        grdQueryList.DataSource = null;
                        grdQueryList.DataBind();
                    }
                }
               
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }

    }
    protected void grdQueryList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Int64 EnueiryId = Convert.ToInt64(grdQueryList.DataKeys[e.Row.RowIndex].Value);
                if (EnueiryId != 0)
                {
                    using (clsEnquiryDetail objdetail = new clsEnquiryDetail())
                    {
                        objdetail.LoginUserId = PageBase.UserId;
                        objdetail.SalesChannelId = PageBase.SalesChanelID;
                        objdetail.QueryType = Convert.ToInt16(ddlQueryStatus.SelectedValue.ToString());
                        objdetail.FromDate = Convert.ToDateTime(ucDateFrom.Date);
                        objdetail.Todate = Convert.ToDateTime(ucDateTo.Date);
                        dtApprovalDetails = objdetail.GetImagePath();
                    }
                    DataRow[] drv = dtApprovalDetails.Select("EnquiryDetailID=" + EnueiryId);
                    if (drv.Length > 0)
                    {
                        DataTable dtTemp = dtApprovalDetails.Clone();

                        for (int cntr = 0; cntr < drv.Length; cntr++)
                        {
                            dtTemp.ImportRow(drv[cntr]);
                        }

                        GridView gvAttachedImages = (GridView)e.Row.FindControl("gvAttachedImages");
                        gvAttachedImages.DataSource = dtTemp;
                        gvAttachedImages.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }

    }
    private void DisableControlForAdmin()
    {
        ddlCategoryType.Visible = false;
        ddlSubCategory.Visible = false;
        txtContactNumber.Visible = false;
        txtName.Visible = false;
       // IpFile.Visible = false;
        // lblAttchment.Visible = false;
        lblContactNumber.Visible = false;
        lblEnquiryType.Visible = false;
        lblDecision.Visible = false;
        lblCustomerQuery.Visible = false;
        ContactName.Visible = false;
        Name.Visible = false;

    }
    protected void grdQueryList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            DataTable dtresult = new DataTable();

            if (e.CommandName == "Insert")
            {

            }
            if(e.CommandName=="Edit")
            {

            }
            if (e.CommandName=="EditData")
            {
                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                for (int i = 0; i < grdQueryList.Rows.Count; i++)
                {
                    grdQueryList.Rows[i].BackColor = System.Drawing.Color.White;
                }
                grdQueryList.Rows[RowIndex].BackColor = System.Drawing.Color.YellowGreen;
                ucMessage1.Visible = false;
                dvPnlRemarksGrid.Visible = true;
                Int64 EnquiryDetailID = Convert.ToInt32(e.CommandArgument);
                hdfenqurydetailid.Value =Convert.ToString(EnquiryDetailID);
                clsEnquiryDetail objdetail = new clsEnquiryDetail();
                objdetail.EnquiryDetailid = EnquiryDetailID;
                objdetail.SalesChannelId = PageBase.SalesChanelID;
                dtresult = objdetail.ViewReplyFillData();
                Int16 QueryStatus = Convert.ToInt16(dtresult.Rows[0]["EnquiryStatus"].ToString());
                if (dtresult.Rows.Count > 0)
                {
                    if (SalesChanelID > 0)
                    {
                        hdnEnquiryid.Value = EnquiryDetailID.ToString();
                        ddlCategoryType.SelectedValue = dtresult.Rows[0]["EnquiryCategoryId"].ToString();
                        ddlCategoryType_SelectedIndexChanged(null, null);
                        ddlSubCategory.SelectedValue = dtresult.Rows[0]["EnquiryTypeMasterID"].ToString();
                        txtContactNumber.Text = dtresult.Rows[0]["EnquiryPersonContactNumber"].ToString();
                        txtName.Text = dtresult.Rows[0]["EnquiryPersonName"].ToString();
                        txtEmail.Text = dtresult.Rows[0]["EmailId"].ToString();
                        ddlImageType.SelectedValue = dtresult.Rows[0]["ImageTypeId"].ToString();
                        ddlImageType.Enabled = false;
                        ddlCategoryType.Enabled = false;
                        ddlSubCategory.Enabled = false;
                        txtContactNumber.Enabled = false;
                        txtEmail.Enabled = false;
                        txtName.Enabled = false;
                        BindRemarkGrid();
                        PnlAdmin.Visible = true;
                        grdDescriptionlist.Visible = true;
                        lblStatus.Visible = true;
                        ddlStatus.Visible = true;
                    }
                    else
                    {
                        hdnEnquiryid.Value = EnquiryDetailID.ToString();
                        ddlCategoryType.SelectedValue = dtresult.Rows[0]["EnquiryCategoryId"].ToString();
                        ddlCategoryType_SelectedIndexChanged(null, null);
                        ddlSubCategory.SelectedValue = dtresult.Rows[0]["EnquiryTypeMasterID"].ToString();
                        txtContactNumber.Text = dtresult.Rows[0]["EnquiryPersonContactNumber"].ToString();
                        txtName.Text = dtresult.Rows[0]["EnquiryPersonName"].ToString();
                        ddlStatus.SelectedValue = dtresult.Rows[0]["EnquiryStatus"].ToString();
                        txtEmail.Text = dtresult.Rows[0]["EmailId"].ToString();
                        ddlCategoryType.Enabled = false;
                        ddlSubCategory.Enabled = false;
                        // ddlStatus.Enabled = false;
                        txtContactNumber.Enabled = false;
                        txtName.Enabled = false;
                        txtEmail.Enabled = false;
                        BindRemarkGrid();
                        PnlAdmin.Visible = true;
                    }


                    /*#CC03 Add Start */
                    if (dtresult.Columns.Contains("ComingFrom") == true && dtresult.Rows[0]["ComingFrom"].ToString() == "1")
                    {

                        Name.Enabled = false;
                        regexpName.Enabled = false;
                        RegularExpressionEmail.Enabled = false;
                        Emailid.Enabled = false;
                        ContactNameValidator.Enabled = false;
                        ContactName.Enabled = false;
                    }
                    else
                    {
                        Name.Enabled = true;
                        regexpName.Enabled = true;
                        RegularExpressionEmail.Enabled = true;
                        Emailid.Enabled = true;
                        ContactNameValidator.Enabled = true;
                        ContactName.Enabled = true;
                    }
                    /* #CC03 Add End*/
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void btnDownLoadQueryReport_Click(object sender, EventArgs e)
    {
        try
        {
            string strExportFileName = string.Empty;
            DataSet dsresult = new DataSet();
            if (ucDateFrom.Date == "" && ucDateTo.Date == "")
            {
                ucMessage1.ShowError("Please Fill From Date and To Date.");
                return;
            }
            else
            {
                if (pageValidate())
                {
                    using (clsEnquiryDetail objReport = new clsEnquiryDetail())
                    {

                        objReport.FromDate = Convert.ToDateTime(ucDateFrom.Date);
                        objReport.Todate = Convert.ToDateTime(ucDateTo.Date);
                        objReport.QueryType = Convert.ToInt16(ddlQueryStatus.SelectedValue);
                        objReport.Distributorcode = ddlDistributorCode.SelectedValue.ToString();
                        objReport.LoginUserId = PageBase.UserId;
                        objReport.SalesChannelId = PageBase.SalesChanelID;
                        objReport.DownLoadReport = 1;
                        dsresult = objReport.SelectQueryResult();
                        strExportFileName = PageBase.importExportCSVFileName;
                        string FilenameToexport = "QueryReportData";
                        if (objReport.Result == 0)
                            //PageBase.ExportToExecl(dsresult, FilenameToexport);
                            ZedService.Utility.ZedServiceUtil.ExportToExecl(dsresult, FilenameToexport);
                        else if (objReport.Result == 1)
                            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                        else
                            ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    bool pageValidate()
    {
        int _date = 0;
        if ((Convert.ToDateTime(ucDateFrom.Date) > DateTime.Now.Date) || (Convert.ToDateTime(ucDateTo.Date) > DateTime.Now.Date))
        {
            ucMessage1.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        }

        _date = DateTime.Compare(Convert.ToDateTime(ucDateTo.Date), Convert.ToDateTime(ucDateFrom.Date));
        if (_date < 0)
        {
            ucMessage1.ShowInfo(Resources.Messages.InvalidDate);
            return false;
        }
        return true;
    }
    private void GetDistributorCode()
    {
        using (clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
        {
            DataTable dtresult = new DataTable();
            try
            {
                ObjMapping.LoginUserId = PageBase.UserId;
                ObjMapping.SalesChannelId = PageBase.SalesChanelID;
                DataSet ds = ObjMapping.SelectDistributorCode();
                if(SalesChanelID>0)
                {
                    dtresult = ds.Tables[0];
                }
                else
                {
                    dtresult = ds.Tables[0];
                }
                if (dtresult.Rows.Count > 0)
                {
                    ddlDistributorCode.DataSource = dtresult;
                    ddlDistributorCode.DataTextField = "SalesChannelName";
                    ddlDistributorCode.DataValueField = "SalesChannelID";
                    ddlDistributorCode.DataBind();
                    ddlDistributorCode.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    ddlDistributorCode.Items.Insert(0, new ListItem("Select", "0"));
                }


            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
            }
        }
    }


    protected void ddlImageType_SelectedIndexChanged(object sender, EventArgs e)
    {
        PnlAdminAttachement.Visible = true;
    }
}