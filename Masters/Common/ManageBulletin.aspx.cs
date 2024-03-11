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
using System.Text.RegularExpressions;

#endregion

//======================================================================================
//* Developed By : Amit Srivastava
//* Role         : Software Engineer
//* Module       : Bulletin Board
//* Description  : This page is used for creation and managing of Bulletins information
//* ====================================================================================
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 10-Aug-2016, Sumit Maurya, #CC01, New checkboxlist added to bind Active Brands according to config (BRANDWISEBULLETIN = 1)Accesstype gets binded according to Brand Maping data.
 * 23-Aug-2016, Sumit Maurya, #CC02, Child of Hierarchy and Saleschannel doesnot gets displayed if config value of BRANDWISEBULLETIN is 1.
 * 05-Jun-2020, Shashikant Singh, #CC03, Added bullitin category and sub category on same page.

 */

public partial class BulletinBoard_ManageBulletin : PageBase
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    int BulletinId = 0;

    /*#CC03:Added Start*/
    MastersData objmaster = new MastersData();
    DataTable Category;
    DataTable SubCategory;

    /*#CC03:Added End*/

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ValidPageControl();
            if ((Request.QueryString["BulletinId"] != null) && (Request.QueryString["BulletinId"] != ""))
            {
                BulletinId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Request.QueryString["BulletinId"], PageBase.KeyStr)));
            }
            if (!IsPostBack)
            {
                /* #CC01 Add Start */
                if (PageBase.BRANDWISEBULLETIN == 1)
                {
                    trBrands.Visible = true;
                    BindActiveBrands();
                }
                /* #CC01 Add End */
                BindHierarchyLevelLocationTree();
                BindSalesChannelLocationTree();
                BindSubCategory();
                BindPageLoadCategory(); /*#CC03:Added*/
                if (BulletinId != 0)
                {
                    PouplateBulletinDetail(BulletinId);
                }
                updAddUserMain.Update();
                imgAddCategory.ImageUrl = strSiteUrl + strAssets + "/CSS/Images/edit.png";
                imgAddSubCategory.ImageUrl = strSiteUrl + strAssets + "/CSS/Images/edit.png";
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
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
            if (!ValidateControl(ref ErrorString))
            {
                ucMsg.ShowInfo(ErrorString);
                return;
            }
            using (BulletinData objBulletin = new BulletinData())
            {
                if ((Request.QueryString["BulletinId"] != null) && (Request.QueryString["BulletinId"] != ""))
                {
                    BulletinId = Convert.ToInt32(Crypto.Decrypt(Request.QueryString["BulletinId"], PageBase.KeyStr));
                    objBulletin.BulletinId = BulletinId;
                }
                else
                {
                    objBulletin.BulletinId = 0;
                }
                /* #CC01 Add Start */
                if (PageBase.BRANDWISEBULLETIN == 1)
                {
                    objBulletin.BrandAccessType = Convert.ToInt16(rblAccessType.SelectedValue);
                }
                else
                {
                    objBulletin.BrandAccessType = null;
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

                objBulletin.DTBrandID = DtBrandID;
                if (FCKDetails.Value.Trim() == "")
                {
                    ucMsg.ShowInfo("Please enter description.");
                    return;
                }
                /* #CC01 Add End */

                objBulletin.BulletinSubject = txtSubject.Text.Trim();
                objBulletin.SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue);
                string url = PageBase.siteURL;
                string description = FCKDetails.Value.Trim();
                string replacevalue = description.Replace("/ZedSalesV5/", url);
                // objBulletin.Description = FCKDetails.Value.Trim();
                objBulletin.Description = replacevalue;
                objBulletin.PublishDate = Convert.ToDateTime(ucPublishDate.Date);
                objBulletin.ExpiryDate = Convert.ToDateTime(ucExpiryDate.Date);
                objBulletin.AccessType = Convert.ToInt16(rblAccessType.SelectedValue);
                objBulletin.Status = chkActive.Checked;
                string LevelIds = FindTreeRoots(tvLevel);
                string LocationIds = FindTreeChild(tvLevel);
                if (LevelIds.Length > 0 || LevelIds != null)
                    objBulletin.LevelIds = LevelIds;
                if (LocationIds.Length > 0 || LocationIds != null)
                    objBulletin.LocationIds = LocationIds;
                string SalesChannelTypeIds = FindTreeRoots(tvSalesChannel);
                string SalesChannelIds = FindTreeChild(tvSalesChannel);
                if (SalesChannelIds.Length > 0 || SalesChannelIds != null)
                    objBulletin.SalesChannelIds = SalesChannelIds;
                if (SalesChannelTypeIds.Length > 0 || SalesChannelTypeIds != null)
                    objBulletin.SalesChannelTypeIds = SalesChannelTypeIds;

                objBulletin.UserID = PageBase.UserId;
                objBulletin.CompanyId = PageBase.ClientId;
                Result = objBulletin.InsertUpdateBulletininfo();

                if (Result > 0 && (objBulletin.Error == null || objBulletin.Error == ""))
                {
                    ClearFrom(); /* #CC01 Added */
                    if (BulletinId == 0)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    }
                    else
                    {
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                    }
                    /* ClearFrom(); #CC01 Commented */
                }
                else
                {
                    if (objBulletin.Error != null && objBulletin.Error != "")
                    {
                        ucMsg.ShowInfo(objBulletin.Error);
                    }
                    else
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                }

            };
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
        using (BulletinData ObjData = new BulletinData())
        {
            ObjData.BulletinId = Id;
            if (treeView == tvLevel)
            {
                Ds = ObjData.GetHierarchyLevelTreeByBulletinId();
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
    private void PouplateBulletinDetail(int BulletinID)
    {
        DataTable DtBulletinDetail;
        try
        {
            using (BulletinData ObjBulletin = new BulletinData())
            {
                ObjBulletin.BulletinId = BulletinID;
                DtBulletinDetail = ObjBulletin.GetBulletinInfo();
            };
            if (DtBulletinDetail.Rows.Count > 0 && DtBulletinDetail != null)
            {
                CheckValidDateOnEdit(DtBulletinDetail);
                /* #CC01 Add Start */
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
                /* #CC01 Add End */

                txtSubject.Text = DtBulletinDetail.Rows[0]["BulletinSubject"].ToString();
                ddlSubCategory.SelectedItem.Selected = false;
                ddlSubCategory.Items.FindByValue(DtBulletinDetail.Rows[0]["SubCategoryID"].ToString()).Selected = true;
                ucPublishDate.Date = DtBulletinDetail.Rows[0]["PublishDate"].ToString();
                ucExpiryDate.Date = DtBulletinDetail.Rows[0]["ExpiryDate"].ToString();
                FCKDetails.Value = DtBulletinDetail.Rows[0]["Description"].ToString();
                chkActive.Checked = Convert.ToBoolean(DtBulletinDetail.Rows[0]["Status"].ToString());
                rblAccessType.SelectedValue = DtBulletinDetail.Rows[0]["AccessType"].ToString();
                rblAccessType.Enabled = false;
                rblAccessType_SelectedIndexChanged(rblAccessType, new EventArgs());
                FillTreeNode(tvLevel, BulletinId);
                FillTreeNode(tvSalesChannel, BulletinId);
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
        ddlCategory.SelectedIndex = 0; /*#CC03:Added*/
        txtSubject.Text = "";
        ddlSubCategory.SelectedIndex = 0;
        ucExpiryDate.Date = "";
        ucPublishDate.Date = "";
        rblAccessType.SelectedValue = "1";
        FCKDetails.Value = "";
        btnCreate.Text = "Submit";
        rblAccessType.SelectedValue = "1";
        rblAccessType_SelectedIndexChanged(rblAccessType, new EventArgs());
        UnCheckTreeNode(tvLevel);
        UnCheckTreeNode(tvSalesChannel);
        rblAccessType.Enabled = true;
        /* #CC01 Add Start */
        if (PageBase.BRANDWISEBULLETIN == 1)
        {
            foreach (ListItem li in chckBrandslist.Items)
            {
                li.Selected = false;
            }
        }
        /* #CC01 Add End */


    }
    private bool ValidateControl(ref string ErrMessage)
    {
        ErrMessage = "";
        if ((ddlSubCategory.SelectedValue == "0") || (txtSubject.Text.Trim() == ""))
        {
            ErrMessage = Resources.Messages.MandatoryField;
            return false;
        }
        /*#CC03:Added Start*/
        if ((ddlCategory.SelectedValue == "0") || (txtSubject.Text.Trim() == ""))
        {
            ErrMessage = Resources.Messages.MandatoryField;
            return false;
        }
        /*#CC03:Added End*/

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

    private void BindSubCategory()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlSubCategory.Items.Clear();
            using (MastersData objMaster = new MastersData())
            {
                objMaster.UserId = PageBase.UserId;
                objMaster.CompanyId = PageBase.ClientId;
                dt = objMaster.GetAllBulletinSubCategory();
            };
            String[] colArray = { "SubCategoryID", "SubCategoryName" };
            PageBase.DropdownBinding(ref ddlSubCategory, dt, colArray);
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
            objBulletinData.UserID = PageBase.UserId;
            objBulletinData.CompanyId = PageBase.ClientId;
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
            /* #CC01 Add Start */
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
            /* #CC01 Add End */
            objBulletinData.UserID = PageBase.UserId;
            objBulletinData.CompanyId = PageBase.ClientId;
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

    /* #CC01 Add Start */
    public void BindActiveBrands()
    {
        try
        {
            ProductData objproduct = new ProductData();
            objproduct.BrandId = 0;
            objproduct.BrandSelectionMode = 1;
            objproduct.CompanyId = PageBase.ClientId;
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

    /* #CC01 Add END */

    /*#CC03:Added Start*/
    protected void imgAddCategory_Click(object sender, ImageClickEventArgs e)
    {
        UcMessage1.ShowControl = false;
        chkStatus.Checked = true;

        ViewState["CategoryID"] = null;
        ModelPopJustConfirmation.Show();
        FillGrid();



    }

    void FillGrid()
    {
        try
        {
            UcMessage1.Visible = false;

            objmaster.CategoryName = txtSerCAT.Text.Trim();
            objmaster.CategorySelectionMode = 2;
            objmaster.CategoryID = 0;
            objmaster.UserId = PageBase.UserId;
            objmaster.CompanyId = PageBase.ClientId;
            Category = objmaster.SelectCategoryInfo();

            grdCAT.DataSource = Category;
            grdCAT.DataBind();
            updgrdView.Update();
            ViewState["Table"] = Category;
        }
        catch (Exception ex)
        {
            UcMessage1.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    public void blankinserttext()
    {
        try
        {
            txtCategoryName.Text = "";
            btnSubmit.Text = "Submit";
            chkStatus.Checked = true;
            UpdCategory.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            UcMessage1.ShowError(err);
        }
    }

    public void blanksertext()
    {
        try
        {
            txtSerCAT.Text = "";
            UpdSearch.Update();
            UpdCategory.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            UcMessage1.ShowError(err);
        }
    }

    protected void grdCAT_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ModelPopJustConfirmation.Show();
        if (e.CommandName == "Active")
        {
            try
            {
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;
                objmaster.CategoryID = Convert.ToInt32(e.CommandArgument);
                objmaster.CategorySelectionMode = 2;
                Category = objmaster.SelectCategoryInfo();
                objmaster.CategoryName = Convert.ToString(Category.Rows[0]["CategoryName"]);

                objmaster.Status = Convert.ToBoolean(Category.Rows[0]["Status"]);
                if (objmaster.Status == true)
                {
                    objmaster.Status = false;
                }
                else
                {
                    objmaster.Status = true;
                }
                objmaster.error = "";

                objmaster.InsertUpdateCategoryInfo();

                if (objmaster.error == "")
                {
                    objmaster.CategoryID = 0;
                    FillGrid();
                    UcMessage1.ShowSuccess(Resources.Messages.StatusChanged);
                }
                else
                {
                    UcMessage1.ShowInfo(objmaster.error);
                }
            }
            catch (Exception ex)
            {
                UcMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }

        if (e.CommandName == "cmdEdit")
        {
            try
            {
                objmaster.CategoryID = Convert.ToInt32(e.CommandArgument);
                ViewState["CategoryID"] = objmaster.CategoryID;

                objmaster.CategorySelectionMode = 2;
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;
                Category = objmaster.SelectCategoryInfo();

                txtCategoryName.Text = Convert.ToString(Category.Rows[0]["CategoryName"]);

                objmaster.Status = Convert.ToBoolean(Category.Rows[0]["Status"]);
                if (objmaster.Status == true)
                {
                    chkStatus.Checked = true;
                }
                else
                {
                    chkStatus.Checked = false;
                }

                btnSubmit.Text = "Update";
                updgrdView.Update();
            }
            catch (Exception ex)
            {
                UcMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
        UpdCategory.Update();
    }

    protected void grdCAT_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCAT.PageIndex = e.NewPageIndex;
        ModelPopJustConfirmation.Show();
        UpdCategory.Update();
        FillGrid();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ModelPopJustConfirmation.Show();
        if (IsPageRefereshed == true)
        {
            return;
        }

        objmaster.CategoryName = txtCategoryName.Text.Trim();

        ViewState["CategoryName"] = objmaster.CategoryName;

        if (chkStatus.Checked == true)
        {
            objmaster.Status = true;
        }
        else
        {
            objmaster.Status = false;
        }

        if (ViewState["CategoryID"] == null || (int)ViewState["CategoryID"] == 0)
        {
            try
            {
                objmaster.error = "";
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;
                objmaster.InsertUpdateCategoryInfo();
                if (objmaster.error == "")
                {
                    FillGrid();
                    UcMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    ViewState["CategoryID"] = null;
                    txtCategoryName.Text = "";
                    chkStatus.Checked = true;
                    ////BindPageLoadCategory();
                    ////ddlCategory.SelectedItem.Text = Convert.ToString(ViewState["CategoryName"]);
                }
                else
                {
                    UcMessage1.ShowError("Record Duplicated");
                }
            }
            catch (Exception ex)
            {
                UcMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
        else
        {
            try
            {
                updgrdView.Update();
                objmaster.error = "";
                objmaster.CategoryID = (int)ViewState["CategoryID"];
                objmaster.CategoryName = txtCategoryName.Text.ToString();
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;

                objmaster.InsertUpdateCategoryInfo();
                if (objmaster.error == "")
                {
                    FillGrid();
                    UcMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);

                    blankinserttext();
                    ViewState["CategoryID"] = null;
                    btnSubmit.Text = "Submit";
                }
                else
                {
                    UcMessage1.ShowError(objmaster.error);
                }
            }
            catch (Exception ex)
            {
                UcMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
        UpdCategory.Update();
    }

    protected void btnCatCancel_Click(object sender, EventArgs e)
    {
        ModelPopJustConfirmation.Show();
        blanksertext();
        blankinserttext();
        FillGrid();
        chkStatus.Checked = true;
        UpdSearch.Update();
        UcMessage1.ShowControl = false;
        btnSubmit.Text = "Submit";
        UpdCategory.Update();
    }

    protected void btnserCategory_Click(object sender, EventArgs e)
    {
        try
        {
            ModelPopJustConfirmation.Show();
            if (txtSerCAT.Text == "")
            {
                UcMessage1.ShowInfo("Please enter search parameter");
                return;
            }
            blankinserttext();
            FillGrid();
        }
        catch (Exception ex)
        {
            UcMessage1.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        UpdCategory.Update();
    }

    protected void btngetalldata_click(object sender, EventArgs e)
    {
        ModelPopJustConfirmation.Show();
        blankinserttext();
        blanksertext();
        FillGrid();
        UpdSearch.Update();
        UcMessage1.ShowControl = false;
        btnSubmit.Text = "Submit";
        UpdCategory.Update();
    }

    protected void exporttoexel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {
            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "CategoryName", "Status" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["Status"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "CategoryDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                ViewState["Table"] = null;
            }
            else
            {
                UcMessage1.ShowError(Resources.Messages.NoRecord);

            }
            ViewState["Table"] = null;
        }
    }

    private void BindPageLoadCategory()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlCategory.Items.Clear();
            using (MastersData objMaster = new MastersData())
            {
                objMaster.UserId = PageBase.UserId;
                objMaster.CompanyId = PageBase.ClientId;
                dt = objMaster.SelectAllCategory();
            };
            String[] colArray = { "CategoryID", "CategoryName" };
            PageBase.DropdownBinding(ref ddlCategory, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    protected void imgAddSubCategory_Click(object sender, ImageClickEventArgs e)
    {

        UcMessage2.Visible = false;

        ViewState["SubCategoryID"] = null;
        ModalPopupSubCategory.Show();
        FillGridSub();
        fillcategory();
        updAddUserMain.Update();
        UpdSubCategory.Update();

    }

    void FillGridSub()
    {
        try
        {
            UcMessage2.Visible = false;

            objmaster.SubCategoryName = txtSerSubCat.Text.Trim();
            if (cmbSerCat.SelectedIndex > 0)
            {
                objmaster.CatID = Convert.ToInt16(cmbSerCat.SelectedValue.ToString());
            }
            else
            {
                objmaster.CatID = 0;
            }
            objmaster.SubCategorySelectionMode = 2;
            objmaster.SubCategoryID = 0;
            objmaster.UserId = PageBase.UserId;
            objmaster.CompanyId = PageBase.ClientId;
            SubCategory = objmaster.SelectSubCategoryInfo();

            grdSubCat.DataSource = SubCategory;
            grdSubCat.DataBind();
            updgrid.Update();
            ViewState["SubCatTable"] = SubCategory;
        }
        catch (Exception ex)
        {
            UcMessage2.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    public void fillcategory()
    {
        try
        {
            objmaster.UserId = PageBase.UserId;
            objmaster.CompanyId = PageBase.ClientId;
            DataTable Category = objmaster.SelectAllCategory();


            cmbSelectCat.DataSource = Category;
            cmbSelectCat.DataTextField = "CategoryName";
            cmbSelectCat.DataValueField = "CategoryID";

            cmbSelectCat.DataBind();
            cmbSelectCat.Items.Insert(0, new ListItem("Select", "0"));
            cmbSelectCat.SelectedIndex = 0;

            cmbSerCat.DataSource = Category;
            cmbSerCat.DataTextField = "CategoryName";
            cmbSerCat.DataValueField = "CategoryID";
            cmbSerCat.DataBind();
            cmbSerCat.Items.Insert(0, new ListItem("select", "0"));
            UpdSubCategory.Update();
            UpdSubCatSearch.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void SubCatblanksertext()
    {
        try
        {
            txtSerSubCat.Text = "";
            cmbSerCat.SelectedIndex = 0;
            UpdSubCatSearch.Update();
            UpdSubCategory.Update();
        }
        catch (Exception ex)
        {
            //string err = ex.Message.ToString();
            //ucMsg.ShowError(err);
            UcMessage2.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    public void SubCatblankinserttext()
    {
        try
        {
            txtInsertSubCat.Text = "";
            cmbSelectCat.SelectedIndex = 0;
            btnSubCatSubmit.Text = "Submit";
            subchkbox.Checked = false;
        }
        catch (Exception ex)
        {
            //string err = ex.Message.ToString();
            //ucMsg.ShowError(err);
            UcMessage2.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    protected void grdSubCat_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ModalPopupSubCategory.Show();
        if (e.CommandName == "Active")
        {
            try
            {
                UcMessage2.Visible = false;
                SubCatblankinserttext();
                objmaster.SubCategoryID = Convert.ToInt32(e.CommandArgument);
                objmaster.SubCategorySelectionMode = 2;
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;

                SubCategory = objmaster.SelectSubCategoryInfo();
                objmaster.SubCategoryName = Convert.ToString(SubCategory.Rows[0]["SubCategoryName"]);

                objmaster.CatID = Convert.ToInt16(SubCategory.Rows[0]["CategoryID"]);

                objmaster.SubStatus = Convert.ToBoolean(SubCategory.Rows[0]["Status"]);
                if (objmaster.SubStatus == true)
                {
                    objmaster.SubStatus = false;
                }
                else
                {
                    objmaster.SubStatus = true;
                }
                objmaster.error = "";
                objmaster.UpdateSubCategoryInfo();

                if (objmaster.error == "")
                {
                    objmaster.SubCategoryID = 0;
                    FillGridSub();
                    UcMessage2.ShowSuccess(Resources.Messages.StatusChanged);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        if (e.CommandName == "cmdEdit")
        {
            try
            {
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;
                objmaster.SubCategoryID = Convert.ToInt32(e.CommandArgument);
                ViewState["SubCategoryID"] = objmaster.SubCategoryID;
                objmaster.SubCategorySelectionMode = 2;
                SubCategory = objmaster.SelectSubCategoryInfo();

                txtInsertSubCat.Text = Convert.ToString(SubCategory.Rows[0]["SubCategoryName"]);

                subchkbox.Checked = Convert.ToBoolean(SubCategory.Rows[0]["Status"].ToString());

                if (cmbSelectCat.Items.FindByValue(SubCategory.Rows[0]["CategoryID"].ToString()) != null)
                {
                    cmbSelectCat.ClearSelection();
                    cmbSelectCat.Items.FindByValue(SubCategory.Rows[0]["CategoryID"].ToString()).Selected = true;
                }
                btnSubCatSubmit.Text = "Update";
                updgrid.Update();
            }
            catch (Exception ex)
            {
                UcMessage2.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
        UpdSubCategory.Update();
    }

    protected void grdSubCat_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSubCat.PageIndex = e.NewPageIndex;
        ModalPopupSubCategory.Show();
        UpdSubCategory.Update();
        FillGridSub();
    }

    protected void btnSubCatSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }
        ModalPopupSubCategory.Show();
        objmaster.SubCategoryName = txtInsertSubCat.Text.Trim();
        var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
        if (regexItem.IsMatch(objmaster.SubCategoryName))
        {

        }
        else
        {
            UcMessage2.Visible = true;
            UcMessage2.ShowError("Special Characters not allowed in SubCategoryname.");
            return;
        }

        ViewState["SubCategoryName"] = objmaster.SubCategoryName;

        objmaster.CatID = Convert.ToInt16(cmbSelectCat.SelectedValue.ToString());

        if (subchkbox.Checked == true)
        {
            objmaster.SubStatus = true;
        }
        else
        {
            objmaster.SubStatus = false;
        }
        if (ViewState["SubCategoryID"] == null || (int)ViewState["SubCategoryID"] == 0)
        {
            try
            {
                SubCatblanksertext();
                objmaster.error = "";
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;
                objmaster.InsertSubCategoryInfo();
                if (objmaster.error == "")
                {
                    FillGridSub();
                    UcMessage2.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    SubCatblankinserttext();
                    updgrid.Update();
                    ////BindSubCategory();
                    ////ddlSubCategory.SelectedItem.Text = Convert.ToString(ViewState["SubCategoryName"]);
                }
            }
            catch (Exception ex)
            {
                UcMessage2.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
        else
        {
            try
            {
                updgrid.Update();
                objmaster.error = "";
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;
                objmaster.SubCategoryID = (int)ViewState["SubCategoryID"];
                objmaster.SubCategoryName = txtInsertSubCat.Text.ToString();
                objmaster.CatID = Convert.ToInt16(cmbSelectCat.SelectedValue.ToString());
                objmaster.UpdateSubCategoryInfo();

                if (objmaster.error == "")
                {
                    objmaster.SubCategorySelectionMode = 2;
                    FillGridSub();
                    UcMessage2.ShowSuccess(Resources.Messages.EditSuccessfull);
                    SubCatblankinserttext();
                    ViewState["SubCategoryID"] = null;
                    btnSubCatSubmit.Text = "Submit";
                }
                else
                {
                    UcMessage2.ShowError("Record Duplicated");
                }
            }
            catch (Exception ex)
            {
                UcMessage2.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
        UpdSubCategory.Update();
    }

    protected void btnSubCatCancel_Click(object sender, EventArgs e)
    {
        ModalPopupSubCategory.Show();
        SubCatblankinserttext();
        SubCatblanksertext();
        FillGridSub();
        UpdSubCatSearch.Update();
        UcMessage2.Visible = false;
        btnSubCatSubmit.Text = "Submit";
        UpdSubCategory.Update();
    }

    protected void btnSerchSubCatD_Click1(object sender, EventArgs e)
    {
        ModalPopupSubCategory.Show();
        if (cmbSerCat.SelectedIndex == 0 && txtSerSubCat.Text == "")
        {
            UcMessage2.ShowInfo("Please enter atleast one searching parameter");
            return;
        }
       
        SubCatblankinserttext();
        FillGridSub();
    }

    protected void getallSubCatdata_Click(object sender, EventArgs e)
    {
        ModalPopupSubCategory.Show();
        SubCatblankinserttext();
        SubCatblanksertext();
        FillGridSub();
        UpdSubCatSearch.Update();
        UcMessage2.Visible = false;
        btnSubCatSubmit.Text = "Submit";
        UpdSubCategory.Update();
    }

    protected void Subexporttoexel_Click(object sender, EventArgs e)
    {
        try
        {
            ModalPopupSubCategory.Show();
            if (ViewState["SubCatTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["SubCatTable"];
                string[] DsCol = new string[] { "CategoryName", "SubCategoryName", "Status" };
                DataTable DsCopy = new DataTable();
                dt = dt.DefaultView.ToTable(true, DsCol);
                dt.Columns["Status"].ColumnName = "Status";

                if (dt.Rows.Count > 0)
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SubCategoryDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    ViewState["SubCatTable"] = null;
                }
                else
                {
                    UcMessage2.ShowError(Resources.Messages.NoRecord);
                }
                ViewState["SubCatTable"] = null;
            }
        }
        catch (Exception ex)
        {
            UcMessage2.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectBindSubCategory();

    }

    private void SelectBindSubCategory()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlSubCategory.Items.Clear();
            using (MastersData objMaster = new MastersData())
            {
                objMaster.CategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                objMaster.UserId = PageBase.UserId;
                objMaster.CompanyId = PageBase.ClientId;
                dt = objMaster.GetAllBulletinSubCategory();
            };
            String[] colArray = { "SubCategoryID", "SubCategoryName" };
            PageBase.DropdownBinding(ref ddlSubCategory, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    /*#CC03:Added End*/


}
