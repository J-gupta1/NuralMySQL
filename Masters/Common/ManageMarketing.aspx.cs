#region All NameSpace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using System.Data;
using System.Resources;
using DataAccess;
using System.IO;
using DevExpress.Web.ASPxTreeList;
using System.Text;
using Cryptography;
#endregion

//======================================================================================
//* Developed By : Gaurav Tyagi
//* Role         : Software Engineer
//* Module       : Marketing  
//* Description  : This page is used for creation and managing of marketing information
//* ====================================================================================
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 
 */

public partial class ManageMarketing : PageBase
{

    int marketingdocumentid = 0;



    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ValidPageControl();
            if ((Request.QueryString["marketingdocumentid"] != null) && (Request.QueryString["marketingdocumentid"] != ""))
            {
                marketingdocumentid = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Request.QueryString["marketingdocumentid"], PageBase.KeyStr)));
            }
            if (!IsPostBack)
            {
                ViewState["PageIndex"] = 1;
                TextBox T = (TextBox)FCKDetails.FindControl("txtMultiline");
                T.Width = 600;
                tblSpecific.Height = 300;

                if (PageBase.BRANDWISEBULLETIN == 1)
                {
                    trBrands.Visible = true;
                    BindActiveBrands();
                }

                BindHierarchyLevelLocationTree();
                BindSalesChannelLocationTree();
                ddlCategoryFor.SelectedValue = "1";
                BindCategory(Convert.ToInt32(ddlCategoryFor.SelectedValue));
                ucPublishDate.Date = DateTime.Now.ToShortDateString();
                ucExpiryDate.Date = DateTime.Now.ToShortDateString();
                UcPDate.Date = DateTime.Now.ToShortDateString();
                UcEDate.Date = DateTime.Now.ToShortDateString();
                if (marketingdocumentid != 0)
                {
                    PouplateBulletinDetail(marketingdocumentid);
                }

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindCategory(int categoryfor)
    {
        try
        {


            DataTable dt = new DataTable();
            ddlCategory.Items.Clear();
            using (marketingmanagementdata objMaster = new marketingmanagementdata())
            {
                dt = objMaster.GetAllParentCategory(categoryfor);
            };
            String[] colArray = { "ParentCategoryID", "ParentCategoryName" };
            PageBase.DropdownBinding(ref ddlCategory, dt, colArray);
            ddlCategory.Items.Add(new ListItem { Text = "Other", Value = "-1" });
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    string SavePostedFile(FileUpload uploadedFile)
    {
        try
        {


            FileInfo fileInfo = new FileInfo(uploadedFile.FileName);
            string strTicks = System.DateTime.Now.Ticks.ToString();

            string strFileUploadedName = strTicks + fileInfo.Name;

            string strTempPath = Server.MapPath("../../UploadDownload/MarketingDocuments/");


            if (!Directory.Exists(strTempPath))
                Directory.CreateDirectory(strTempPath);


            uploadedFile.SaveAs(strTempPath + strFileUploadedName);


            string fileLabel = fileInfo.Name;
            string fileLength = uploadedFile.FileBytes.Length / 1024 + "K";

            return string.Format("{0}", "/UploadDownload/MarketingDocuments/" + strFileUploadedName);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
            return "";
        }

    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {

            if (IsPageRefereshed == true)
            {
                return;
            }
            int Result = 0;
            string ErrorString = "";
            if (ddlCategoryFor.SelectedValue == "1")
            {
                if (!ValidateControl(ref ErrorString))
                {
                    ucMsg.ShowInfo(ErrorString);
                    return;
                }
                FileUpload FileUpload1 = (FileUpload)pnlFiles.FindControl("IpFile");
                string filePath = FileUpload1.PostedFile.FileName;// getting the file path of uploaded file  
                string filename1 = Path.GetFileName(filePath); // getting the file name of uploaded file  
                string ext = Path.GetExtension(filename1); // getting the file extension of uploaded file  
                string type = String.Empty;
                if (!FileUpload1.HasFile)
                {
                    ucMsg.ShowInfo("Please Select PDF File");
                    return; //if file uploader has no file selected  
                }
                if (ext != ".pdf") // this switch code validate the files which allow to upload only PDF file   
                {
                    ucMsg.ShowInfo("Please Select Only PDF File");
                    return;
                }



                using (marketingmanagementdata objmarketing = new marketingmanagementdata())
                {
                    objmarketing.UserID = PageBase.UserId;
                    string path = SavePostedFile(FileUpload1);
                    if (path.Contains("/UploadDownload/MarketingDocuments/"))
                    {
                        objmarketing.DocumentPath = path;
                    }
                    else { return; }
                    if ((Request.QueryString["marketingdocumentid"] != null) && (Request.QueryString["marketingdocumentid"] != ""))
                    {
                        marketingdocumentid = Convert.ToInt32(Crypto.Decrypt(Request.QueryString["marketingdocumentid"], PageBase.KeyStr));
                        objmarketing.MarketingDocumentID = marketingdocumentid;
                    }
                    else
                    {
                        objmarketing.MarketingDocumentID = 0;
                    }

                    if (PageBase.BRANDWISEBULLETIN == 1)
                    {
                        objmarketing.BrandAccessType = Convert.ToInt16(rblAccessType.SelectedValue);
                    }
                    else
                    {
                        objmarketing.BrandAccessType = null;
                    }

                    DataTable DtBrandID = new DataTable();
                    DtBrandID = tempDtBtandID();
                    if (PageBase.BRANDWISEBULLETIN == 1)
                    {
                        foreach (ListItem li in chckBrandslist.Items)
                        {
                            if (li.Selected == true)
                            {
                                DataRow dr = DtBrandID.NewRow();
                                dr["Id"] = li.Value;
                                DtBrandID.Rows.Add(dr);
                                DtBrandID.AcceptChanges();
                            }
                        }
                        if (DtBrandID.Rows.Count == 0)
                        {
                            ucMsg.ShowInfo("Please select brand.");
                            return;
                        }

                    }

                    objmarketing.DTBrandID = DtBrandID;
                    if (FCKDetails.TextBoxText.Trim() == "")
                    {
                        ucMsg.ShowInfo("Please enter description.");
                        return;
                    }
                    /*catsub start*/
                    if (Convert.ToInt32(ddlCategory.SelectedValue) == -1)
                    {
                        if (txtCategory.Text.Trim().Length == 0)
                        {
                            ucMsg.ShowInfo("Please Enter Other Category Name");
                            return;
                        }
                        else if (txtSubCategory.Text.Trim().Length == 0)
                        {
                            ucMsg.ShowInfo("Please Enter Other SubCategory Name");
                            return;
                        }
                        else
                        {
                            using (marketingmanagementdata msData = new marketingmanagementdata())
                            {
                                msData.subcategoryname = txtSubCategory.Text.Trim();
                                msData.categoryname = txtCategory.Text.Trim();

                                msData.InsertCatSubCategory();
                                if (msData.Error == "" && msData.SubCategoryId != 0)
                                {
                                    objmarketing.SubCategoryId = msData.SubCategoryId;
                                }
                                else
                                {
                                    ucMsg.ShowInfo(msData.Error);
                                    return;
                                }


                            }

                        }
                    }
                    else if (Convert.ToInt32(ddlSubCategory.SelectedValue) == -1)
                    {
                        if (txtSubCategory.Text.Trim().Length == 0)
                        {
                            ucMsg.ShowInfo("Please Enter Other SubCategory Name");
                            return;
                        }
                        else
                        {
                            using (marketingmanagementdata msData = new marketingmanagementdata())
                            {
                                msData.subcategoryname = txtSubCategory.Text.Trim();
                                msData.categoryname = ddlCategory.SelectedItem.ToString();

                                msData.InsertCatSubCategory();
                                if (msData.Error == "" && msData.SubCategoryId != 0)
                                {
                                    objmarketing.SubCategoryId = msData.SubCategoryId;
                                }
                                else
                                {
                                    ucMsg.ShowInfo(msData.Error);
                                    return;
                                }


                            }
                        }
                    }

                    /*catsub start*/

                    objmarketing.Heading = txtSubject.Text.Trim();

                    if (objmarketing.SubCategoryId == 0)
                    {
                        objmarketing.SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue);
                    }
                    objmarketing.Description = FCKDetails.TextBoxText.Trim();
                    objmarketing.PublishDate = Convert.ToDateTime(ucPublishDate.Date);
                    objmarketing.ExpiryDate = Convert.ToDateTime(ucExpiryDate.Date);
                    objmarketing.AccessType = Convert.ToInt16(rblAccessType.SelectedValue);
                    objmarketing.Status = true;
                    string LevelIds = FindTreeRoots(tvLevel);
                    string LocationIds = FindTreeChild(tvLevel);
                    if (LevelIds.Length > 0 || LevelIds != null)
                        objmarketing.LevelIds = LevelIds;
                    if (LocationIds.Length > 0 || LocationIds != null)
                        objmarketing.LocationIds = LocationIds;
                    string SalesChannelTypeIds = FindTreeRoots(tvSalesChannel);
                    string SalesChannelIds = FindTreeChild(tvSalesChannel);
                    if (SalesChannelIds.Length > 0 || SalesChannelIds != null)
                        objmarketing.SalesChannelIds = SalesChannelIds;
                    if (SalesChannelTypeIds.Length > 0 || SalesChannelTypeIds != null)
                        objmarketing.SalesChannelTypeIds = SalesChannelTypeIds;
                    Result = objmarketing.InsMarketingDocument();
                    if (Result > 0 && (objmarketing.Error == null || objmarketing.Error == ""))
                    {
                        ClearFrom();
                        if (marketingdocumentid == 0)
                        {
                            ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        }
                        else
                        {
                            ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        }

                    }
                    else
                    {
                        if (objmarketing.Error != null && objmarketing.Error != "")
                        {
                            ucMsg.ShowInfo(objmarketing.Error);
                        }
                        else
                        {
                            ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        }
                    }
                }
            }
            else
            {

                /*catsub start*/
                if (Convert.ToInt32(ddlCategory.SelectedValue) == -1)
                {
                    if (txtCategory.Text.Trim().Length == 0)
                    {
                        ucMsg.ShowInfo("Please Enter Other Category Name");
                        return;
                    }
                    else if (txtSubCategory.Text.Trim().Length == 0)
                    {
                        ucMsg.ShowInfo("Please Enter Other SubCategory Name");
                        return;
                    }
                    else
                    {
                        using (marketingmanagementdata msData = new marketingmanagementdata())
                        {
                            msData.subcategoryname = txtSubCategory.Text.Trim();
                            msData.categoryname = txtCategory.Text.Trim();

                            msData.InsertCatSubCategory();
                            if (msData.Error == "" && msData.SubCategoryId != 0)
                            {
                                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                                // objmarketing.SubCategoryId = msData.SubCategoryId;
                            }
                            else
                            {
                                ucMsg.ShowInfo(msData.Error);
                                return;
                            }


                        }

                    }
                }
                else if (Convert.ToInt32(ddlSubCategory.SelectedValue) == -1)
                {
                    if (txtSubCategory.Text.Trim().Length == 0)
                    {
                        ucMsg.ShowInfo("Please Enter Other SubCategory Name");
                        return;
                    }
                    else
                    {
                        using (marketingmanagementdata msData = new marketingmanagementdata())
                        {
                            msData.subcategoryname = txtSubCategory.Text.Trim();
                            msData.categoryname = ddlCategory.SelectedItem.ToString();

                            msData.InsertCatSubCategory();
                            if (msData.Error == "" && msData.SubCategoryId != 0)
                            {
                                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                                // objmarketing.SubCategoryId = msData.SubCategoryId;
                            }
                            else
                            {
                                ucMsg.ShowInfo(msData.Error);
                                return;
                            }


                        }
                    }
                }

                /*catsub start*/

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFrom();

    }
    protected void rblAccessType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblAccessType.SelectedValue == "1")
        {
            tblSpecific.Visible = false;
        }
        if (rblAccessType.SelectedValue == "2")
        {
            tblSpecific.Visible = true;
        }
        /* #CC01 Add Start */
        ucMsg.Visible = false;
        updMsg.Update();
        /* #CC01 Add End */

    }
    protected void LBViewBulletin_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewBulletin.aspx");

    }

    #endregion

    #region Tree Control Events
    private string FindTreeRoots(TreeView treeView)
    {
        string Levels = "";
        if (treeView.CheckedNodes.Count > 0)
        {
            foreach (TreeNode root in treeView.Nodes)
            {
                if (root.Checked == true)
                    Levels += "," + root.Value.ToString();
            }
        }
        return Levels;
    }
    private string FindTreeChild(TreeView treeView)
    {
        string Locations = "";
        foreach (TreeNode root in treeView.Nodes)
        {
            if (root.Checked == true || root.Checked == false)
            {
                foreach (TreeNode child in root.ChildNodes)
                {
                    if (child.Checked == true)
                        Locations += "," + child.Value.ToString();

                }
            }
        }

        return Locations;
    }
    void UnCheckTreeNode(TreeView treeView)
    {
        foreach (TreeNode root in treeView.Nodes)
        {
            root.Checked = false;
            foreach (TreeNode child in root.ChildNodes)
            {
                child.Checked = false;
            }
        }

    }

    private void FillTreeNode(TreeView treeView, int Id)
    {
        DataSet Ds = null;
        using (marketingmanagementdata ObjData = new marketingmanagementdata())
        {
            ObjData.MarketingDocumentID = Id;
            if (treeView == tvLevel)
            {
                Ds = ObjData.GetHierarchyLevelTreeByMARKETINGDOCUMENTId();
                if (Ds != null || Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                        {
                            foreach (TreeNode root in treeView.Nodes)
                            {
                                if (root.Value == Ds.Tables[0].Rows[i]["HierarchyLevelID"].ToString())
                                    root.Checked = true;
                                if (root.Checked == true)
                                {
                                    foreach (TreeNode child in root.ChildNodes)
                                    {
                                        child.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                    if (Ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds.Tables[1].Rows.Count; i++)
                        {
                            foreach (TreeNode root in treeView.Nodes)
                            {
                                if (root.Checked == false)
                                {
                                    foreach (TreeNode child in root.ChildNodes)
                                    {
                                        if (child.Value == Ds.Tables[1].Rows[i]["LocationID"].ToString())
                                            child.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (treeView == tvSalesChannel)
            {

                Ds = ObjData.GetSalesChannelTreeByBulletinId();
                if (Ds != null || Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                        {
                            foreach (TreeNode root in treeView.Nodes)
                            {
                                if (root.Value == Ds.Tables[0].Rows[i]["LevelID"].ToString())
                                    root.Checked = true;
                                if (root.Checked == true)
                                {
                                    foreach (TreeNode child in root.ChildNodes)
                                    {
                                        child.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                    if (Ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds.Tables[1].Rows.Count; i++)
                        {
                            foreach (TreeNode root in treeView.Nodes)
                            {
                                if (root.Checked == false)
                                {
                                    foreach (TreeNode child in root.ChildNodes)
                                    {
                                        if (child.Value == Ds.Tables[1].Rows[i]["SalesChannelID"].ToString())
                                            child.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region User defined Function
    private void PouplateBulletinDetail(Int64 pMarketingDocumentID)
    {
        DataTable DtBulletinDetail;
        try
        {
            using (marketingmanagementdata ObjMManagemnet = new marketingmanagementdata())
            {
                ObjMManagemnet.MarketingDocumentID = pMarketingDocumentID;
                DtBulletinDetail = ObjMManagemnet.GetMmanagementInfo();
            };
            if (DtBulletinDetail.Rows.Count > 0 && DtBulletinDetail != null)
            {
                CheckValidDateOnEdit(DtBulletinDetail);

                if (PageBase.BRANDWISEBULLETIN == 1)
                {
                    chckBrandslist.Enabled = false;
                    if (Convert.ToString(DtBulletinDetail.Rows[0]["BrandID"]) != "")
                    {
                        string[] strBrandID = Convert.ToString(DtBulletinDetail.Rows[0]["BrandID"]).Split(',');
                        if (strBrandID.Length > 0)
                        {
                            foreach (ListItem li in chckBrandslist.Items)
                            {
                                for (int i = 0; i < strBrandID.Length; i++)
                                {
                                    if (strBrandID[i] == li.Value)
                                    {
                                        li.Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }


                txtSubject.Text = DtBulletinDetail.Rows[0]["HEADING"].ToString();
                ddlCategory.SelectedItem.Selected = false;
                ddlCategory.Items.FindByText(DtBulletinDetail.Rows[0]["Categoryname"].ToString()).Selected = true;
                BindSubCategory(Convert.ToInt32(ddlCategory.SelectedValue));
                ddlSubCategory.SelectedItem.Selected = false;
                ddlSubCategory.Items.FindByValue(DtBulletinDetail.Rows[0]["CategoryID"].ToString()).Selected = true;
                ucPublishDate.Date = DtBulletinDetail.Rows[0]["PublishDate"].ToString();
                ucExpiryDate.Date = DtBulletinDetail.Rows[0]["ExpiryDate"].ToString();
                FCKDetails.TextBoxText = DtBulletinDetail.Rows[0]["Description"].ToString();
                //chkActive.Checked = Convert.ToBoolean(DtBulletinDetail.Rows[0]["Status"]);
                rblAccessType.SelectedValue = DtBulletinDetail.Rows[0]["AccessType"].ToString();
                rblAccessType.Enabled = false;
                rblAccessType_SelectedIndexChanged(rblAccessType, new EventArgs());
                FillTreeNode(tvLevel, marketingdocumentid);
                FillTreeNode(tvSalesChannel, marketingdocumentid);
                btnCreate.Text = "Update";



            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void ClearFrom()
    {
        txtSubject.Text = "";
        ddlSubCategory.SelectedIndex = 0;
        ucExpiryDate.Date = "";
        ucPublishDate.Date = "";
        rblAccessType.SelectedValue = "1";
        FCKDetails.TextBoxText = "";
        btnCreate.Text = "Submit";
        rblAccessType.SelectedValue = "1";
        rblAccessType_SelectedIndexChanged(rblAccessType, new EventArgs());
        UnCheckTreeNode(tvLevel);
        UnCheckTreeNode(tvSalesChannel);
        rblAccessType.Enabled = true;

        if (PageBase.BRANDWISEBULLETIN == 1)
        {
            foreach (ListItem li in chckBrandslist.Items)
            {
                li.Selected = false;
            }
        }



    }
    private bool ValidateControl(ref string ErrMessage)
    {
        ErrMessage = "";
        if ((ddlSubCategory.SelectedValue == "0") || (txtSubject.Text.Trim() == ""))
        {
            ErrMessage = Resources.Messages.MandatoryField;
            return false;
        }
        if ((Convert.ToDateTime(ucExpiryDate.Date)) < (Convert.ToDateTime(ucPublishDate.Date)))
        {
            ErrMessage = "Expiry date should be greater then publish date";
            return false;
        }
        if (rblAccessType.SelectedValue == "2")
        {
            int i = 0;
            i = tvLevel.CheckedNodes.Count + tvSalesChannel.CheckedNodes.Count;
            if (i <= 0)
            {
                ErrMessage = "Please choose at least one node for specific Access Type";
                return false;
            }
        }
        return true;

    }
    void ValidPageControl()
    {

        ucExpiryDate.MinRangeValue = System.DateTime.Now.Date;
        ucPublishDate.MinRangeValue = System.DateTime.Now.Date;
        if (btnCreate.Text == "Submit")
            ucPublishDate.RangeErrorMessage = "Date should be greater then equal to current date.";
        tvLevel.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
        tvSalesChannel.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
        ucMsg.ShowControl = false;
    }
    void CheckValidDateOnEdit(DataTable Dt)
    {
        if (Dt.Rows.Count > 0)
        {
            DateTime dtime = System.DateTime.Now;
            if (dtime >= Convert.ToDateTime(Dt.Rows[0]["PublishDate"].ToString()))
            {
                ucPublishDate.TextBoxDate.Enabled = false;
                ucPublishDate.imgCal.Enabled = false;
                ucPublishDate.MinRangeValue = Convert.ToDateTime(Dt.Rows[0]["PublishDate"].ToString());
                ucPublishDate.RangeErrorMessage = "Date should be greater then or equal to Publish date.";
            }
        }

    }

    private void BindSubCategory(int cat_id)
    {
        try
        {
            DataTable dt = new DataTable();
            ddlSubCategory.Items.Clear();
            if (cat_id > 0)
            {
                using (marketingmanagementdata objMaster = new marketingmanagementdata())
                {


                    dt = objMaster.GetAllCategoryMasterByParentId(cat_id);
                };
                String[] colArray = { "CategoryID", "CategoryName" };
                PageBase.DropdownBinding(ref ddlSubCategory, dt, colArray);

            }

            ddlSubCategory.Items.Add(new ListItem { Text = "Other", Value = "-1" });

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindHierarchyLevelLocationTree()
    {
        using (BulletinData objBulletinData = new BulletinData())
        {
            DataSet objDs = null;
            objDs = objBulletinData.GetAllHierarchyLevelwithLocation();
            if (objDs != null || objDs.Tables.Count > 0)
            {
                if (objDs != null || objDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
                    {
                        TreeNode parentnode = new TreeNode();
                        parentnode.Text = objDs.Tables[0].Rows[i][1].ToString();
                        parentnode.Value = objDs.Tables[0].Rows[i][0].ToString();
                        tvLevel.Nodes.Add(parentnode);
                        if (objDs.Tables.Count > 1) /* #CC01 Added */
                            if (PageBase.BRANDWISEBULLETIN == 0) /* #CC02 Added */
                                for (int j = 0; j < objDs.Tables[1].Rows.Count; j++)
                                {
                                    if (objDs.Tables[0].Rows[i][0].ToString() == objDs.Tables[1].Rows[j][0].ToString())
                                    {
                                        TreeNode childnode = new TreeNode();
                                        childnode.Text = objDs.Tables[1].Rows[j][2].ToString();
                                        childnode.Value = objDs.Tables[1].Rows[j][1].ToString();
                                        parentnode.ChildNodes.Add(childnode);
                                    }
                                }
                    }
                }
            }
        }
    }
    private void BindSalesChannelLocationTree()
    {
        using (BulletinData objBulletinData = new BulletinData())
        {
            DataSet objDs = null;

            DataTable DtBrandID = new DataTable();
            DtBrandID = tempDtBtandID();
            if (PageBase.BRANDWISEBULLETIN == 1)
            {
                foreach (ListItem li in chckBrandslist.Items)
                {
                    if (li.Selected == true)
                    {
                        DataRow dr = DtBrandID.NewRow();
                        dr["Id"] = li.Value;
                        DtBrandID.Rows.Add(dr);
                        DtBrandID.AcceptChanges();
                    }
                }
            }
            tvSalesChannel.Nodes.Clear();
            objBulletinData.DTBrandID = DtBrandID;

            objDs = objBulletinData.GetAllSalesChannelwithLocation();
            if (objDs != null || objDs.Tables.Count > 0)
            {
                if (objDs != null || objDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
                    {
                        TreeNode parentnode = new TreeNode();
                        parentnode.Text = objDs.Tables[0].Rows[i][1].ToString();
                        parentnode.Value = objDs.Tables[0].Rows[i][0].ToString();
                        tvSalesChannel.Nodes.Add(parentnode);
                        if (objDs.Tables.Count > 1) /* #CC01 Added */
                            if (PageBase.BRANDWISEBULLETIN == 0) /* #CC02 Added */
                                for (int j = 0; j < objDs.Tables[1].Rows.Count; j++)
                                {
                                    if (objDs.Tables[0].Rows[i][0].ToString() == objDs.Tables[1].Rows[j][0].ToString())
                                    {
                                        TreeNode childnode = new TreeNode();
                                        childnode.Text = objDs.Tables[1].Rows[j][1].ToString();
                                        childnode.Value = objDs.Tables[1].Rows[j][2].ToString();
                                        parentnode.ChildNodes.Add(childnode);
                                    }

                                }
                    }
                }
            }

        }
    }
    #endregion


    public void BindActiveBrands()
    {
        try
        {
            ProductData objproduct = new ProductData();
            objproduct.BrandId = 0;
            objproduct.BrandSelectionMode = 1;
            DataTable dtbrandserch = objproduct.SelectBrandInfo();
            if (dtbrandserch != null)
            {
                if (dtbrandserch.Rows.Count > 0)
                {
                    chckBrandslist.DataSource = dtbrandserch;
                    chckBrandslist.DataTextField = "BrandName";
                    chckBrandslist.DataValueField = "BrandID";
                    chckBrandslist.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    public DataTable tempDtBtandID()
    {
        DataTable dtbrandid = new DataTable();
        dtbrandid.Columns.Add("Id", typeof(System.Int64));
        return dtbrandid;
    }


    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlCategory.SelectedValue) == -1)
            {
                txtCategory.Visible = true;
                txtSubCategory.Visible = true;
            }
            else
            {
                txtSubCategory.Text = "";
                txtSubCategory.Visible = false;

                txtCategory.Text = "";
                txtCategory.Visible = false;
            }
            BindSubCategory(Convert.ToInt32(ddlCategory.SelectedValue));
            UpddlCategory.Update();
            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlSubCategory.SelectedValue) == -1)
            {

                txtSubCategory.Visible = true;
            }
            else
            {
                txtSubCategory.Text = "";
                txtSubCategory.Visible = false;


            }

            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ddlCategoryFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCategory.Items.Clear();
            ddlSubCategory.Items.Clear();
            txtCategory.Visible = false;
            txtSubCategory.Visible = false;

            if (Convert.ToInt32(ddlCategoryFor.SelectedValue) != -1)
            {
                BindCategory(Convert.ToInt32(ddlCategoryFor.SelectedValue));
            }
            Mark.Visible = (ddlCategoryFor.SelectedValue == "1" ? true : false);

            UpddlCategory.Update();
            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    protected void ddlcatforsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcatforsearch.SelectedValue == "1" || ddlcatforsearch.SelectedValue == "-1")
            {
                Label4.Visible = true;
                Label6.Visible = true;
                Label8.Visible = true;
                UcPDate.Visible = true;
                UcEDate.Visible = true;
                ddlMstatusforsearch.Visible = true;
                RequiredFieldValidator4.Visible = true;
            }
            else
            {
                Label4.Visible = false;
                Label6.Visible = false;
                Label8.Visible = false;
                UcPDate.Visible = false;
                UcEDate.Visible = false;
                ddlMstatusforsearch.Visible = false;
                RequiredFieldValidator4.Visible = false;
            }
            UpdatePanel2.Update();
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }


    protected void btn_search_Click(object sender, EventArgs e)
    {
        bindgrid();
    }

    void bindgrid()
    {
        try
        {
            if (ViewState["PageIndex"] == null)
            {

                ViewState["PageIndex"] = 1;
            }
            if (ddlcatforsearch.SelectedValue != "-1")
            {
                using (marketingmanagementdata mmd = new marketingmanagementdata())
                {
                    mmd.categoryfor = Convert.ToByte(ddlcatforsearch.SelectedValue);
                    if (ddlcatforsearch.SelectedValue == "1")
                    {
                        if (Convert.ToDateTime(UcPDate.Date) > Convert.ToDateTime(UcEDate.Date))
                        {
                            ucMsg.ShowInfo("To Date Should be greater then From date");
                            return;
                        }
                        mmd.PublishDate = Convert.ToDateTime(UcPDate.Date);
                        mmd.ExpiryDate = Convert.ToDateTime(UcEDate.Date);
                        mmd.MSearchType = Convert.ToByte(ddlMstatusforsearch.SelectedValue);
                    }
                    else
                    {
                        mmd.PublishDate = Convert.ToDateTime("1900-01-01");
                        mmd.ExpiryDate = Convert.ToDateTime("1900-01-01");
                        mmd.MSearchType = 0;
                    }
                    mmd.PCSearchType = Convert.ToByte(ddlstatusforsearch.SelectedValue);
                    mmd.CSearchType = Convert.ToByte(ddlCstatusforsearch.SelectedValue);
                    mmd.PageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    mmd.PageSize =  Convert.ToInt32(PageBase.PageSize);
                    DataSet ds = mmd.SearchResult();
                    gvsearchresult.DataSource = ds.Tables[ds.Tables.Count - 1];
                    gvsearchresult.DataBind();
                    if (mmd.Totalrecords > 0)
                    {
                        int total = mmd.Totalrecords;
                        ucPagingControl1.Visible = true;
                        ViewState["TotalRecords"] = total;
                        ucPagingControl1.TotalRecords = total;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = Convert.ToInt32(ViewState["PageIndex"]);
                        ucPagingControl1.FillPageInfo();
                    }
                    updgrid.Update();

                }

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }



    }
    protected void gvsearchresult_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            ViewState["PageIndex"] = null;
            using (marketingmanagementdata objmarket = new marketingmanagementdata())
            {
                ucMsg.Visible = false;
                if (e.CommandName == "PCactiveState")
                {
                    objmarket.Changecode = 1;
                    objmarket.MarketingDocumentID = Convert.ToInt64(e.CommandArgument.ToString());
                    objmarket.statusupdator();
                    bindgrid();
                }
                else if (e.CommandName == "CactiveState")
                {
                    try
                    {
                        objmarket.Changecode = 2;
                        objmarket.MarketingDocumentID = Convert.ToInt64(e.CommandArgument.ToString());
                        objmarket.statusupdator();
                        bindgrid();

                    }
                    catch (Exception ex)
                    {
                        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                        PageBase.Errorhandling(ex);
                    }
                }
                else if (e.CommandName == "MactiveState")
                {
                    try
                    {
                        ucMsg.Visible = false;
                        objmarket.Changecode = 3;
                        objmarket.MarketingDocumentID = Convert.ToInt64(e.CommandArgument.ToString());
                        objmarket.statusupdator();
                        bindgrid();
                    }
                    catch (Exception ex)
                    {
                        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                        PageBase.Errorhandling(ex);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
            //updpnlSaveData.Update();

        }
        updgrid.Update();
    }
    protected void ucPagingControl1_SetControlRefresh()
    {
        ViewState["PageIndex"] = null;
        ViewState["PageIndex"] = ucPagingControl1.CurrentPage;
        bindgrid();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        if (ddlcatforsearch.SelectedValue != "-1")
        {
            using (marketingmanagementdata mmd = new marketingmanagementdata())
            {
                mmd.categoryfor = Convert.ToByte(ddlcatforsearch.SelectedValue);
                if (ddlcatforsearch.SelectedValue == "1")
                {
                    if (Convert.ToDateTime(UcPDate.Date) > Convert.ToDateTime(UcEDate.Date))
                    {
                        ucMsg.ShowInfo("To Date Should be greater then From date");
                        return;
                    }
                    mmd.PublishDate = Convert.ToDateTime(UcPDate.Date);
                    mmd.ExpiryDate = Convert.ToDateTime(UcEDate.Date);
                    mmd.MSearchType = Convert.ToByte(ddlMstatusforsearch.SelectedValue);
                }
                else
                {
                    mmd.PublishDate = Convert.ToDateTime("1900-01-01");
                    mmd.ExpiryDate = Convert.ToDateTime("1900-01-01");
                    mmd.MSearchType = 0;
                }
                mmd.PCSearchType = Convert.ToByte(ddlstatusforsearch.SelectedValue);
                mmd.CSearchType = Convert.ToByte(ddlCstatusforsearch.SelectedValue);
                mmd.PageIndex = -1;
                mmd.PageSize = 0;// Convert.ToInt32(PageBase.PageSize);
                DataSet ds = mmd.SearchResult();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //DataSet dtcopy = new DataSet();
                    //dtcopy.Merge(ds.Tables[0]);
                    //dtcopy.Tables[0].AcceptChanges();
                    //String FilePath = Server.MapPath("../../");
                     
                    string FilenameToexport = "ManageMarketing";
                    //PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(ds, FilenameToexport);
                     
                }
            }
        }
    }
}