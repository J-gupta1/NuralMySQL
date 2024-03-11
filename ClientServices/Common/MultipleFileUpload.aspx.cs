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
     * Module : To Upload Multiple files
     * ================================================================================================
     * Reviewed By : 
     * Modify By : 
     * #CC01 , 06 Jan 2014, Karam Chand Sharma, To pass parameter  to user control for image save location 
                            SaveOn Type "Cloud/Server"
     *  
     * #CC02 , 02 Apr 2015, Sumit Maurya, Code added, commented and changed
     * #CC03 , 10 Apr 2015, Sumit Maurya, Code Added and commented
     * #CC04 , 14 Apr 2015, Sumit Maurya, Code Commented and Added 
     * #CC05 , 15 Apr 2015, Sumit Maurya, Code Commented and Added   
     * #CC06 , 17 Apr 2015, Sumit Maurya, Code Added    
     * #CC07,  02-Aug-2017, Vijay Katiyar, Added  distinct all imagetypeid in hidden field hdnImageType 
     * ================================================================================================

     */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZedService;
using System.Data;
using System.IO;
using BussinessLogic;

public partial class Common_MultipleFileUpload : PageBase
{
    Int16 decider = 0;
    string[] strFileName;
    string strImageName;
    string StrDecider;
    int UpperLimit = 0;
    int Rows = 0;
    int ImageType = 0;
    bool overriteUploadLimit = false; 
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
       // UserControlMultipleFileUpload.Click += new MultipleFileUploadClick(UserControlMultipleFileUpload_Click);
      
        StrDecider = Request.QueryString["Decider"] == null ? "0" : Request.QueryString["Decider"].ToString();
        if (StrDecider.Contains("1,NA"))
        {
            hdnRemoveDefaultSession.Value = "1";//Yes we are to remove the default Session
            StrDecider = StrDecider.Replace(",NA", "");
        }
      
        string strLoadFor = null;
        if (Request.QueryString["LoadFor"] != null)
        {
            overriteUploadLimit = true; 
            strLoadFor = Cryptography.Crypto.Decrypt(Request.QueryString["LoadFor"].ToString().Replace(" ", "+"), PageBase.KeyStr).ToString();
            string strReferenceTypeId = string.Empty;
            strReferenceTypeId = Cryptography.Crypto.Decrypt(Request.QueryString["ReferenceTypeId"].ToString().Replace(" ", "+"), "testEncrpt").ToString();
            if (strLoadFor == "1" || strLoadFor == "2" || strLoadFor == "3")
          
       
            {
                //UserControlMultipleFileUpload.ReferenceTypeId = Convert.ToInt64(strReferenceTypeId); 
                //UserControlMultipleFileUpload.FindControl("btnSaveImage").Visible = true;
                DataTable dtFatchDecider = new DataTable();
                clsEnquiryDetail obj = new clsEnquiryDetail();
                dtFatchDecider.Columns.Add("ImageTypeId", typeof(short));

                DataSet ds = new DataSet();
               
                obj.ImageLoadType = Convert.ToInt32(strLoadFor); 
                ds = obj.GetImageLoadType();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtr = dtFatchDecider.NewRow();
                    dtr["ImageTypeId"] = Convert.ToInt16(ds.Tables[0].Rows[i]["ImageTypeId"].ToString());
                    dtFatchDecider.Rows.Add(dtr);
                    dtFatchDecider.AcceptChanges();
                    UpperLimit++; 
                }
                //UserControlMultipleFileUpload.dtDecider = dtFatchDecider;
            }
        }
        else if (strLoadFor == null)
        {
            DataTable dtFatchDecider = new DataTable();

            dtFatchDecider.Columns.Add("ImageTypeId", typeof(short));
            for (int i = 0; i < StrDecider.Split(',').Length; i++)
            {
                DataRow dtr = dtFatchDecider.NewRow();
                dtr["ImageTypeId"] = Convert.ToInt16((StrDecider.Split(','))[i]);
                dtFatchDecider.Rows.Add(dtr);
                dtFatchDecider.AcceptChanges();
            }
            //UserControlMultipleFileUpload.dtDecider = dtFatchDecider;

        }


        if (Request.QueryString["UpperLimit"] != null && Request.QueryString["Rows"] != null && Request.QueryString["ImageType"] != null)
        {
           
            if (overriteUploadLimit == false)
            {
                UpperLimit = 3;
                int.TryParse(Request.QueryString["UpperLimit"], out UpperLimit);
            }
            else if (overriteUploadLimit == true)
                UpperLimit = UpperLimit * 3;
            
            Rows = 6;
            ImageType = 0;
           
            int.TryParse(Request.QueryString["Rows"], out Rows);
            int.TryParse(Request.QueryString["ImageType"], out ImageType);
          
            //try { UserControlMultipleFileUpload.SaveOn = Request.QueryString["SaveOn"].ToString(); }
            //catch (Exception ex) { UserControlMultipleFileUpload.SaveOn = "0"; }
           
            //UserControlMultipleFileUpload.UpperLimit = UpperLimit;
            //UserControlMultipleFileUpload.Rows = Rows;
            //UserControlMultipleFileUpload.ImageType = ImageType;
        }
        BindPageLoad();



    }
    private void BindPageLoad()
    {
        if (Session["FinalFileData"] != null)
        {
            if (lsDetailImages.SelectedItem == null)
            {
                bindFileList(((DataTable)Session["FinalFileData"]));
            }
            //FileUpload IpFile = (FileUpload)UserControlMultipleFileUpload.FindControl("IpFile");
            if (EnableDisableUploadImages(((DataTable)Session["FinalFileData"])).Rows.Count >= UpperLimit)
            {
                //IpFile.Enabled = false;
            }
            else
            {
                //IpFile.Enabled = true;
            }
        }
        if (Session["FinalFileData"] != null)
        {
            if (((DataTable)Session["FinalFileData"]).Rows.Count > 0)
            {
                btndeleteFile.Visible = true;
            }
            else
            {
                btndeleteFile.Visible = false;
            }
        }
        else
        {
            btndeleteFile.Visible = false;
        }
    }

    void UserControlMultipleFileUpload_Click()
    {
        try
        {

            DataTable FinalFileData = new DataTable();
            //FinalFileData = UserControlMultipleFileUpload.FinalFileData;
            if (Session["FinalFileData"] != null)
            {
              
                if (overriteUploadLimit == true)
                {


                    int rowCount = EnableDisableUploadImages((DataTable)Session["FinalFileData"]).Rows.Count + FinalFileData.Rows.Count;
                    int.TryParse((UpperLimit).ToString(), out UpperLimit);
                    if (rowCount > UpperLimit)
                    {
                        lblMessage.Text = "Not Upload More Then Limit...";
                        return;
                    }
                    else
                    {
                        ((DataTable)Session["FinalFileData"]).Merge(FinalFileData);
                    }
                }
                else if (overriteUploadLimit == false)
                {
                    int rowCount = EnableDisableUploadImages((DataTable)Session["FinalFileData"]).Rows.Count + FinalFileData.Rows.Count;
                    int.TryParse(Request.QueryString["UpperLimit"], out UpperLimit);
                    if (rowCount > UpperLimit)
                    {
                        lblMessage.Text = "Not Upload More Then Limit...";
                        return;
                    }
                    else
                    {
                        ((DataTable)Session["FinalFileData"]).Merge(FinalFileData);
                    }
                }
               
            }
            else
            {
                Session["FinalFileData"] = FinalFileData;       
            }
            if (Session["FinalFileData"] != null)
            {
               // FileUpload IpFile = (FileUpload)UserControlMultipleFileUpload.FindControl("IpFile");
                if (((DataTable)Session["FinalFileData"]).Rows.Count >= UpperLimit)
                {

                    //IpFile.Enabled = false;
                }
                else
                {
                    //IpFile.Enabled = true;
                }
            }
            if (hdnRemoveDefaultSession.Value == "1")
            {
                Session["FinalFileData"] = null;
            }
            if (Request.QueryString["Decider"] != null)
            {
              
                for (int i = 0; i < StrDecider.Split(',').Length; i++)
                {
                   
                    if (Convert.ToInt16(StrDecider.Split(',')[i]) == 1)
                      
                        Session["DopImage"] = FinalFileData;
                 
                    else if (Convert.ToInt16(StrDecider.Split(',')[i]) == 2)
                        Session["PysicalConditionImage"] = FinalFileData;
                }
            }
            BindImagesList();


          
            DataTable dtFinalSessionData = new DataTable();
            if (Session["FinalFileData"] != null)
            {
                if (((DataTable)Session["FinalFileData"]).Rows.Count > 0)
                {
                    dtFinalSessionData = (DataTable)Session["FinalFileData"];
                }
            }

            DataTable dtUniqRecords = new DataTable();
            if (dtFinalSessionData != null && dtFinalSessionData.Rows.Count > 0)
            {
                string[] tblCol = { "ImageTypeId" };/*do not change column name*/
                dtUniqRecords = dtFinalSessionData.DefaultView.ToTable(true, tblCol);
            }
            foreach (DataRow dtr in dtUniqRecords.Rows)
            {
                hdnImageType.Value = hdnImageType.Value.ToString() == "0" ? dtr["ImageTypeId"].ToString() : hdnImageType.Value.ToString() + "," + dtr["ImageTypeId"].ToString();
            }
          
            if (!IsPostBack)
            {
                //BindImagesList();
            }
            if (Session["FinalFileData"] != null)
            {
                if (((DataTable)Session["FinalFileData"]).Rows.Count > 0)
                {
                    btndeleteFile.Visible = true;
                }
                else
                {
                    btndeleteFile.Visible = false;
                }
                Session["FinalFileData"] = null;
            }
            else
            {
                btndeleteFile.Visible = false;
            }

        }
        catch (Exception)
        {

        }
    }
    void BindImagesList()
    {
        try
        {

            if (Session["DopImageFromDB"] != null)
            {
                lblPreviousFiles.Text = "Previous Files";
                lblAlreadyUploadedFiles.Text = Convert.ToString(Session["DopImageFromDB"]);
            }
            else
                lblPreviousFiles.Text = "No Files";


            if (Request.QueryString["Decider"] != null)
            {
                for (int i = 0; i < Request.QueryString["Decider"].Split(',').Length; i++)
                {
                  
                    if (Convert.ToInt16(Request.QueryString["Decider"].Split(',')[i]) == 1 | Convert.ToInt16(Request.QueryString["Decider"].Split(',')[i]) == 2)
                    {
                        if (Session["DopImage"] != null)
                            bindFileList((DataTable)Session["DopImage"]);
                        if (Session["PysicalConditionImage"] != null)
                            bindFileList((DataTable)Session["PysicalConditionImage"]);
                    }
                    else
                    {
                        bindFileList((DataTable)Session["FinalFileData"]);
                    }
                }
            }
            else if (Session["FinalFileData"] != null)
            {
                bindFileList((DataTable)Session["FinalFileData"]);
            }

        }
        catch (Exception ex)
        {

        }
    }
    void bindFileList(DataTable dt)
    {
        try
        {
            //Int16 i = 1;
            if (dt.Rows.Count > 0)
            {
                
                lblPreviousFiles.Text = "No. of Files " + dt.Rows.Count.ToString(); 
               lsDetailImages.DataSource = EnableDisableUploadImages(dt);
                lsDetailImages.DataTextField = "ImageTypeName";   
              
                lsDetailImages.DataValueField = "CtrlID";
                lsDetailImages.DataBind();

            }
            else
            {
                lblPreviousFiles.Text = "No Files";  
                lsDetailImages.Items.Clear();
                btndeleteFile.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btndeleteFile_Click(object sender, EventArgs e)
    {
        try
        {
            if (lsDetailImages.SelectedItem == null)
            {
                lblMessage.Text = "Please Select File...";
                return;
            }
           
            DataTable dtTemp = (DataTable)Session["FinalFileData"];
            DataRow[] dr = dtTemp.Select("ImageTypeName='" + lsDetailImages.SelectedItem.Text + "'");
            if (dr.Length > 0)
            {
                foreach (DataRow drr in dr)
                {
                    dtTemp.Rows.Remove(drr);

                }
                Session["FinalFileData"] = dtTemp;
                bindFileList(dtTemp);
                lblMessage.Text = "File Delete Successfully...";
                //FileUpload IpFile = (FileUpload)UserControlMultipleFileUpload.FindControl("IpFile");
                //IpFile.Enabled = true;

            }
          
        }
        catch (Exception ex)
        {
            lblMessage.Text = "File Not Delete Successfully...";
        }
    }

    private DataTable EnableDisableUploadImages(DataTable dt)
    {
        DataTable dtData = dt.Clone();
       // clsWorkorder obj = new clsWorkorder();

        string[] splitstrImageNameWithType;
        string strImageNameWithType = string.Empty;

        if (dt.Rows.Count > 0)
        {
            //foreach (DataRow drData in UserControlMultipleFileUpload.dtDecider.Rows)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //if (drData["ImageTypeid"].ToString() == dr["ImageTypeid"].ToString())
                    //{
                    //    strFileName = Convert.ToString(dr["Filelocation"]).Split('\\');
                    //    strImageName = strFileName[strFileName.Length - 1];

                    //    dr["Filelocation"] = strImageName;
                    //    splitstrImageNameWithType = Convert.ToString(dr["ImageTypeName"]).Split('\\');
                    //    strImageNameWithType = splitstrImageNameWithType[splitstrImageNameWithType.Length - 1];
                    //    dr["ImageTypeName"] = strImageNameWithType;

                    //    dtData.ImportRow(dr);
                    //}
                }
            }
            dtData.AcceptChanges();

        }
        return dtData;
    }
}
