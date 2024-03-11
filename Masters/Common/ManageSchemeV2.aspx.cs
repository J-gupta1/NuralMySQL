#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
/*/
* ================================================================================================
* ================================================================================================
*
* COPYRIGHT (c) 2012 Zed Axis Technologies (P) Ltd.
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* ================================================================================================
* Created By : Sumit Maurya
* Created On : 04-Apr-2016
* Module : Manage Scheme
* Description : This module is copy of Manage Scheme.
* ================================================================================================
* Change Log:
* ------------- 
* DD-MMM-YYYY, Name, #CCXX, Description
 * 25-Jul-2017, Sumit Maurya, #CC01, Implementation according to  scheme component. (done for comio)
 * 11-Jul-2018, Sumit Maurya, #CC02, User was not able to upload Invoice Value Based template/ Point or Rupees based template .This has been resolved if "Component Type" is not target based and PayOut Type is Slab Based.  (Done for Lemon)
====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Linq;
using ZedService;


public partial class Masters_Common_ManageSchemeV2 : PageBase
{
    string strUploadedFileName;
    UploadFile UploadFile = new UploadFile();
    OpenXMLExcel objexcel = new OpenXMLExcel();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            uclblMessage.ShowControl = false;
            if (!IsPostBack)
            {
                //cmbComponentType.Attributes.Add("onChange", "return OnSelectedIndexChange();"); 
                //BindHierarchyLevelLocationTree();
                BindSalesChannelLocationTree();
                fillComponent();
                BindTimeperiod();
                fillPayoutBase();
                updSeprate.Update();
                // ucFromDate.Date = Fromdate;
                ucFromDate.Date = ToDate;
                ucToDate.Date = ToDate;
            }
           /* ucFromDate.MinRangeValue = DateTime.Now.Date; */
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
        }
    }

    public void fillPayoutBase()
    {
        using (SchemeData obj = new SchemeData())
        {
            DataTable dt = obj.GetPaymentTypeDetails();
            cmbPayoutBase.DataSource = dt;
            cmbPayoutBase.DataTextField = "ComponentPayoutTypeName";
            cmbPayoutBase.DataValueField = "PaymentBaseCode";
            cmbPayoutBase.DataBind();
            cmbPayoutBase.Items.Insert(0, new ListItem("Select", "0"));
            updMain.Update();
        }
    }




    private void fillComponent()
    {
        using (SchemeData obj = new SchemeData())
        {
            DataTable dt = obj.GetSchemeComponentsTypeDetails();
            cmbComponentType.DataSource = dt;
            cmbComponentType.DataTextField = "ComponentCriteriaTypeName";
            cmbComponentType.DataValueField = "ComponentTypeCode";
            cmbComponentType.DataBind();
            cmbComponentType.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void BindSalesChannelLocationTree()
    {
        try
        {
            using (SchemeData objscheme = new SchemeData())
            {
                DataSet objDs = null;
                objDs = objscheme.GetSalesChannelDetailForExclude();
                if (objDs != null || objDs.Tables.Count > 0)
                {
                    if (objDs != null || objDs.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
                        {
                            chckUserType.DataSource = objDs.Tables[0];
                            chckUserType.DataTextField = "SalesChannelTypeName";
                            chckUserType.DataValueField = "SalesChannelTypeID";
                            chckUserType.DataBind();
                        }
                    }
                }
            }
            #region commented code
            /*
            using (BulletinData objBulletinData = new BulletinData())
            {
                DataSet objDs = null;
                objBulletinData.IsScheme = 1;
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
            }*/
            #endregion comment
        }
        catch (Exception ex)
        {
            // uclblMessage.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            PageBase.Errorhandling(ex);
        }
    }
    #region User Defined
    private void InsertData(DataSet objDS)
    {
        pnlgrid.Visible = true;
        btnSave.Visible = true;
        //objDS.Tables[0].Columns.Remove("Error");
        GrdScheme.DataSource = objDS.Tables[0];
        ViewState["Scheme"] = objDS.Tables[0];
        GrdScheme.DataBind();
        updscheme.Update();
    }





    #endregion

    #region Button Event
    protected void BtnUpload_Click(object sender, EventArgs e)
    {

        if (cmbPayoutBase.SelectedValue == "0")
        {
            uclblMessage.ShowError("Plaese Select a payout base type");
        }

        if ((ViewState["SalesChannelIds"] == null || Convert.ToString(ViewState["SalesChannelIds"]) == "")
                        && (ViewState["RetailerIds"] == null || Convert.ToString(ViewState["RetailerIds"]) == "")
                        )
        {
            uclblMessage.ShowInfo("Please upload user detail for scheme.");
            return;

        }
        pnlgrid.Visible = false;
        DataSet objDS = new DataSet();
        try
        {
            string strRootFolerPath;
            Int16 Upload = 0;
            byte isSuccess = 1;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            OpenXMLExcel objexcel = new OpenXMLExcel();
            Upload = UploadFile.IsExcelFile(flupdScheme, ref strUploadedFileName);
            if (Upload == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eScheme;
                UploadFile.TemplateType = EnumData.eTargetTemplateType.eSKUWise;

                if (cmbComponentType.Items.FindByText("Select").Selected == true)
                {
                    uclblMessage.ShowError("Please select componnent type");
                    return;
                }
                if (cmbPayoutBase.Items.FindByText("Select").Selected == true)
                {
                    uclblMessage.ShowError("Please select pay out base type");
                    return;
                }
                string[] strbase1 = cmbComponentType.SelectedValue.Split('-');
                string[] strbase = cmbPayoutBase.SelectedValue.Split('-');
                /* #CC01 Add Start */
                if (strbase1[1].ToString() == "1" && strbase[1].ToString() == "0")
                {
                    isSuccess = UploadFile.uploadValidExcel(ref objDS, "TargetItemWise");

                }
                else
                    /* #CC01 Add End */
                    if (strbase1[1].ToString() == "0")
                    {

                        if (strbase[1].ToString() == "0")
                        {
                            isSuccess = UploadFile.uploadValidExcel(ref objDS, "TargetItemWise");
                        }
                        /* #CC02 Add Start */
                        else if (strbase[1].ToString() == "1")
                        {
                            isSuccess = UploadFile.uploadValidExcel(ref objDS, "TargetRangeWise");
                        }
                        /* #CC02 Add End */
                        else
                        {
                            uclblMessage.ShowError("please select valid sheet");
                            return;
                        }
                    }
                    else if (strbase1[1].ToString() == "1")
                    {
                        if (strbase[1].ToString() == "1" || strbase[1].ToString() == "0")
                        {
                            isSuccess = UploadFile.uploadValidExcel(ref objDS, "TargetRangeWise");
                        }
                        else
                        {
                            uclblMessage.ShowError("Please Select Valid Sheet");
                            return;
                        }
                    }
                    else
                    {
                        uclblMessage.ShowError("Please Select Valid Sheet");
                        return;
                    }

                /* #CC01 Comment Start */
                //if (isSuccess == 1)
                //{

                //    var duplicates = objDS.Tables[0].AsEnumerable().GroupBy(i => new { StraightValue = i["StraightValue"] }).Where(g => g.Count() > 1).Select(g => new { g.Key.StraightValue }).ToList();
                //    if (duplicates.Count > 0)
                //    {
                //        string duplicaterecords = string.Empty;
                //        for (int i = 0; i < duplicates.Count; i++)
                //        {
                //            duplicaterecords = (duplicaterecords == string.Empty || duplicaterecords == "") ? duplicaterecords + " " + duplicates[i].StraightValue : duplicaterecords + ", " + duplicates[i].StraightValue;
                //            /*if(!objDS.Tables[0].Rows.Contains("Error"))
                //            {
                //                objDS.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
                //            }*/
                //        }
                //        uclblMessage.ShowInfo("Duplicate records :" + duplicaterecords);
                //        return;
                //    }

                //}
                /* #CC01 comment End */
                switch (isSuccess)
                {
                    case 0:
                        uclblMessage.ShowError(UploadFile.Message);
                        break;
                    case 2:
                        uclblMessage.ShowError(Resources.Messages.CheckErrorGrid);
                        pnlgrid.Visible = true;
                        GrdScheme.DataSource = objDS;
                        GrdScheme.DataBind();
                        updscheme.Update();
                        btnSave.Visible = false;
                        break;
                    case 1:
                        InsertData(objDS);

                        break;
                }

            }
            else if (Upload == 2)
            {
                uclblMessage.ShowError(Resources.Messages.UploadXlxs);
            }
            else if (Upload == 3)
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
            uclblMessage.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            PageBase.Errorhandling(ex);
        }
    }

    public DataTable getexcludedlist()
    {
        DataTable dt = new DataTable();
        DataColumn dc1 = new DataColumn("SKUID");
        DataColumn dc2 = new DataColumn("Status");
        dt.Columns.Add(dc1);
        dt.Columns.Add(dc2);
        return dt;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }
        if (txtScheme.Text == "")
        {
            uclblMessage.ShowError("please enter the scheme name");
            return;
        }

        if (Convert.ToDateTime(ucToDate.Date) < Convert.ToDateTime(ucFromDate.Date))
        {
            uclblMessage.ShowInfo("From date must not be greater than to date.");
            return;
        }
        /*if (Convert.ToDateTime(ucFromDate.Date) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
        {
            uclblMessage.ShowInfo("From date must not be less than current date. ");
            return;
        }*/
        DataSet DsXML = new DataSet();
        try
        {
            if (ViewState["Scheme"] != null)
            {
                DataTable DtDetail = new DataTable();
                DataTable dtScheme = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    dtScheme = ObjCommom.GettvpTableUploadScheme();
                }
                DtDetail = (DataTable)ViewState["Scheme"];
                string[] str1 = cmbComponentType.SelectedValue.Split('-');
                String[] str2 = cmbPayoutBase.SelectedValue.Split('-');
                if (str1[2] == "0")
                {
                    if (str2[1] == "0")
                    {
                        foreach (DataRow dr in DtDetail.Rows)
                        {
                            DataRow drow = dtScheme.NewRow();
                            drow[0] = " ";
                            drow[1] = dr["StraightValue"];
                            drow[2] = dr["StraightValue"].ToString();
                            drow[3] = dr["PayoutValue"];
                            dtScheme.Rows.Add(drow);
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in DtDetail.Rows)
                        {
                            DataRow drow = dtScheme.NewRow();
                            drow[0] = " ";
                            drow[1] = dr["MinSlab"];
                            drow[2] = dr["MaxSlab"].ToString();
                            drow[3] = dr["PayoutValue"];
                            dtScheme.Rows.Add(drow);
                        }
                    }

                }

                else
                {
                    if (str2[1] == "0")
                    {
                        foreach (DataRow dr in DtDetail.Rows)
                        {
                            DataRow drow = dtScheme.NewRow();
                            drow[0] = dr["ItemCode"].ToString();
                            drow[1] = dr["StrieghtValue"];
                            drow[2] = dr["StrieghtValue"].ToString();
                            drow[3] = dr["PayoutValue"];
                            dtScheme.Rows.Add(drow);
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in DtDetail.Rows)
                        {
                            DataRow drow = dtScheme.NewRow();
                            drow[0] = dr["ItemCode"].ToString();
                            drow[1] = dr["MinSlab"];
                            drow[2] = dr["MaxSlab"].ToString();
                            drow[3] = dr["PayoutValue"];
                            dtScheme.Rows.Add(drow);
                        }
                    }

                }

                dtScheme.AcceptChanges();

                DataTable dtEx = getexcludedlist();

                /*if (ViewState["ExTables"] != null)
                {
                    DataTable dtexdt = (DataTable)ViewState["ExTables"];
                    foreach (DataRow dr in dtexdt.Rows)
                    {
                        DataRow dr1 = dtEx.NewRow();
                        dr1[0] = dr["SKUID"].ToString();
                        dr1[1] = dr["Status"].ToString();
                        dtEx.Rows.Add(dr1);
                    }

                }
*/
                DataTable dtOfs = getFortnights();
                foreach (ListItem lst in chkLst.Items)
                {
                    if (lst.Selected)
                    {
                        DataRow dr3 = dtOfs.NewRow();
                        dr3["OFID"] = lst.Value;
                        dr3["Status"] = "1";
                        dtOfs.Rows.Add(dr3);
                    }
                }
                if (ViewState["ExcludeModel"] != null)
                {
                    DataTable dtexdt = (DataTable)ViewState["ExcludeModel"];
                    foreach (DataRow dr in dtexdt.Rows)
                    {
                        DataRow dr1 = dtEx.NewRow();
                        dr1[0] = dr["OFID"].ToString();
                        dr1[1] = dr["Status"].ToString();
                        dtEx.Rows.Add(dr1);
                    }

                }
                ///DataTable dtOfs = getFortnights();

                //dtOfs = (DataTable)ViewState["ExcludeModel"] == null ? getFortnights() : (DataTable)ViewState["ExcludeModel"];

                using (SchemeData objScheme = new SchemeData())
                {
                    string strRetailerlist = string.Empty;
                    objScheme.SchemeName = txtScheme.Text.Trim();
                    /*
                    string LevelIds = FindTreeRoots(tvLevel);
                    string LocationIds = FindTreeChild(tvLevel, ref strRetailerlist);
                    if (LevelIds.Length > 0 || LevelIds != null)
                        objScheme.LevelIds = LevelIds;
                    if (LocationIds.Length > 0 || LocationIds != null)
                        objScheme.LocationIds = LocationIds;
                    string SalesChannelTypeIds = FindTreeRoots(tvSalesChannel);
                    string SalesChannelIds = FindTreeChild(tvSalesChannel, ref strRetailerlist);//strRetailerlist will be used here

                    if (SalesChannelIds.Length > 0 || SalesChannelIds != null)
                        objScheme.SalesChannelIds = SalesChannelIds;
                    if (SalesChannelTypeIds.Length > 0 || SalesChannelTypeIds != null)
                        objScheme.SalesChannelTypeIds = SalesChannelTypeIds;
                    */

                    objScheme.IsTarget = Convert.ToInt16(ViewState["IsTarget"]);
                    objScheme.FromDate = ucFromDate.Date;
                    objScheme.ToDate = ucToDate.Date;
                    /*
                    if (strRetailerlist.Length > 0 || strRetailerlist != null)
                    {
                        objScheme.RetailersIds = strRetailerlist;
                    }
                     */
                    if ((ViewState["SalesChannelIds"] == null || Convert.ToString(ViewState["SalesChannelIds"]) == "")
                        && (ViewState["RetailerIds"] == null || Convert.ToString(ViewState["RetailerIds"]) == "")
                        )
                    {
                        uclblMessage.ShowInfo("Please upload user detail for scheme.");
                        return;

                    }
                    objScheme.SalesChannelIds = Convert.ToString(ViewState["SalesChannelIds"]);
                    objScheme.RetailersIds = Convert.ToString(ViewState["RetailerIds"]);

                    objScheme.ComponentTypeID = Convert.ToInt16(str1[0]);
                    objScheme.PayOutBase = Convert.ToInt32(str2[0]);
                    objScheme.UserId = PageBase.UserId;

                    /* #CC01 Add Start */

                    objScheme.LevelIds = Convert.ToString(ViewState["StrLevelIDs"]);
                    objScheme.SalesChannelTypeIds = Convert.ToString(ViewState["StrSalesChannelTypeIds"]);
                    /* objScheme.LocationIds = Convert.ToString(ViewState["LocationId"]); */

                    /* #CC01 Add End */
                    objScheme.InsertUpdateScheme(dtScheme, dtEx, dtOfs);
                    if (objScheme.ErrorMessage != string.Empty)
                    {
                        uclblMessage.ShowError(objScheme.ErrorMessage);
                        return;
                    }
                    else if (objScheme.ErrorXML != string.Empty && objScheme.ErrorXML != null)
                    {
                        uclblMessage.XmlErrorSource = objScheme.ErrorXML;
                        updMessage.Update();
                        return;
                    }
                    pnlgrid.Visible = false;
                    ViewState["Scheme"] = null;
                    uclblMessage.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    blankall();
                    ClearAllViewState();
                    updscheme.Update();

                    grdUser.DataSource = null;
                    grdUser.DataBind();

                    grdModelsExcluded.DataSource = null;
                    grdModelsExcluded.DataBind();
                };
            }


        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
        }
        finally
        {
            if (DsXML != null)
            {
                DsXML.Dispose();
            }
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        blankall();
    }
    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet DsSku = new DataSet();

            using (SchemeData ObjScheme = new SchemeData())
            {

                ObjScheme.SchemeType = EnumData.eSchemeTemplateType.eSKUWise;


                DsSku.Merge(ObjScheme.GetSchemeTemplate());
            };
            if (DsSku.Tables[0].Rows.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SKU Wise Scheme Template";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(DsSku, FilenameToexport, EnumData.eTemplateCount.eTarget);
            }
        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());

        }

    }

    #endregion

    #region Combo Events


    #endregion

    protected void LnkDownloadRefCode_Click(object sender, EventArgs e)
    {

        DataSet DsSku = new DataSet();
        try
        {
            using (SalesChannelData ObjSales = new SalesChannelData())
            {
                ObjSales.ReqType = EnumData.eControlRequestTypeForEntry.eScheme;

                DsSku = ObjSales.GetSchemeTemplateData();
            };
            if (DsSku.Tables.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "Reference Code list";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(DsSku, FilenameToexport, EnumData.eTemplateCount.eTarget);
            }
        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());

        }
    }
    protected void lnksummeryDwnload_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet DsSku = new DataSet();

            using (SchemeData ObjScheme = new SchemeData())
            {

                ObjScheme.SchemeType = EnumData.eSchemeTemplateType.eSummary;


                DsSku.Merge(ObjScheme.GetSchemeTemplate());
            };
            if (DsSku.Tables[0].Rows.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "Consolidated Scheme Template";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(DsSku, FilenameToexport, EnumData.eTemplateCount.eTarget);
            }
        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());

        }
    }

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

    private string FindTreeChild(TreeView treeView, ref string strRetailerInfo)
    {
        string Locations = "";

        foreach (TreeNode root in treeView.Nodes)
        {
            if (root.Checked == false)
            {
                foreach (TreeNode child in root.ChildNodes)
                {
                    if (child.Checked == true)
                    {
                        Locations += "," + child.Value.ToString();
                        if (child.Parent.Value == "101")
                        {
                            strRetailerInfo = "," + child.Value.ToString();      //means they are retailers
                        }
                    }



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



    protected void LBViewScheme_Click(object sender, EventArgs e)
    {
        Response.Redirect("SchemeView.aspx", false);
    }
    protected void btnExclude_click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["ExTables"];
        pnlExclude.Visible = false;
        //pnlExclude.Attributes.CssStyle.Add("display", "none");
        /// updExclude.Update();
    }

    public DataTable GetExcludedModels()
    {
        DataTable dt = new DataTable();
        DataColumn dc1 = new DataColumn("SKUID");
        DataColumn dc2 = new DataColumn("Status");
        dt.Columns.Add(dc1);
        dt.Columns.Add(dc2);
        return dt;
    }



    public void FillSKU()
    {

        using (ProductData obj = new ProductData())
        {

            if (cmbComponentType.SelectedValue == "12" || cmbComponentType.SelectedValue == "21")
            {
                obj.ExcludedSelectionMode = (int)EnumData.eExcludedModels.Model;
            }
            else if (cmbComponentType.SelectedValue == "14" || cmbComponentType.SelectedValue == "22")
            {
                obj.ExcludedSelectionMode = (int)EnumData.eExcludedModels.Product;
            }
            else if (cmbComponentType.SelectedValue == "15" || cmbComponentType.SelectedValue == "23")
            {
                obj.ExcludedSelectionMode = (int)EnumData.eExcludedModels.Brand;
            }

            else
            {
                obj.ExcludedSelectionMode = (int)EnumData.eExcludedModels.SKU;
            }



            DataTable skuinfo = obj.SelectExcludedInfo();
            // grdExcluded.DataSource = skuinfo;
            //grdExcluded.DataBind();
            ///  updExclude.Update();
        }
    }


    protected void excludeModels_click(object sender, EventArgs e)
    {
        try
        {
            pnlExclude.Visible = true;
            //pnlExclude.Attributes.CssStyle.Add("display", "block");
            ///   updExclude.Update();
        }
        catch (Exception ex)
        {

        }
    }

    protected void grdExclude_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdExcluded.PageIndex = e.NewPageIndex;
        FillSKU();
    }


    public void blankall()
    {
        foreach (ListItem lst in chkLst.Items)
        {
            if (lst.Selected)
            {
                lst.Selected = false;
            }
        }
        ucFromDate.Date = "";
        ucToDate.Date = "";
        cmbComponentType.SelectedValue = "0";
        cmbPayoutBase.SelectedValue = "0";
        txtScheme.Text = "";
        updMain.Update();
        pnlExclude.Visible = false;
        ///updExclude.Update();
        pnlgrid.Visible = false;
        updscheme.Update();
        tblSpecific.Visible = false;
        ///  updTree.Update();
        pnlExclude.Visible = false;
        /// updExclude.Update();
        pnlTargetdr.Visible = false;
        pnldaterng.Visible = false;
        updSeprate.Update();
    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        uclblMessage.ShowInfo("Go To hell");
    }

    public void BindTimeperiod()
    {
        using (TargetData objTarget = new TargetData())
        {
            chkLst.DataSource = objTarget.GetTimePeriod();
            chkLst.DataValueField = "FinancialCalenderID";
            chkLst.DataTextField = "FinancialCalenderFortnight";
            chkLst.DataBind();
        }

    }





    public DataTable getFortnights()
    {
        DataTable dt = new DataTable();
        DataColumn dc1 = new DataColumn("OFID");
        DataColumn dc2 = new DataColumn("Status");
        dt.Columns.Add(dc1);
        dt.Columns.Add(dc2);
        return dt;

    }

    protected void cmbComponentType_SelectedIndexChanged1(object sender, EventArgs e)
    {
        pnlExclude.Visible = false;
        pnlIsUserWise.Visible = true; /* temp Added */
        if (cmbComponentType.SelectedValue == "0")
        {
            pnlTargetdr.Visible = false;
            dvUpload.Visible = false;
            pnlexclBtn.Visible = false;
            pnldaterng.Visible = false;
            pnlExclude.Visible = false;
            pnlIsUserWise.Visible = false;
        }
        else
        {
            foreach (ListItem lst in chkLst.Items)
            {
                if (lst.Selected)
                {
                    lst.Selected = false;
                }
            }
            //pnlRefrenceTemplate.Visible = false;
            //pnlTempltatetarget.Visible = false;


            string[] strids = cmbComponentType.SelectedValue.Split('-');
            if (strids[1].ToString() == "1")
            {
                /* pnlIsUserWise.Visible = false; temp Commented */
                // updIsUserWise.Update();
                pnlTargetdr.Visible = true;
                pnldaterng.Visible = false;
                updSeprate.Update();
                ViewState["IsTarget"] = 1;

            }
            else
            {
                pnlIsUserWise.Visible = true;
                // updIsUserWise.Update();
                pnlTargetdr.Visible = false;
                pnldaterng.Visible = true;
                updSeprate.Update();
                ViewState["IsTarget"] = "2";
                dvUpload.Visible = true;
            }
            if (strids[2].ToString() == "1" || cmbComponentType.SelectedValue == "0")
            {
                //pnlRefrenceTemplate.Visible = true;
                dvUpload.Visible = true;
                // updTemplates.Update();
            }
            else
            {
                //pnlTempltatetarget.Visible = true;
                dvUpload.Visible = true;
                // updTemplates.Update();
            }

            pnlexclBtn.Visible = true;
            ///  updExclude.Update();
            FillSKU();
        }
    }


    protected void oncheckedchange(object sender, EventArgs e)
    {
        int trd = 1;
        DataTable dt = GetExcludedModels();
        if (ViewState["ExTables"] == null)
        {
            ViewState["ExTables"] = dt;
        }
        /*
        foreach (GridViewRow grv in grdExcluded.Rows)
        {
            CheckBox chk = (CheckBox)grv.FindControl("chkSelect");
            if (chk.Checked == true)
            {
                trd = 1;
                foreach (DataRow drexf in ((DataTable)ViewState["ExTables"]).Rows)
                {
                    if (drexf[0].ToString() == ((Label)grv.FindControl("lblID")).Text.Trim().ToString())
                    {
                        trd++;
                    }
                }
                if (trd == 1)
                {
                    DataRow dr12 = ((DataTable)ViewState["ExTables"]).NewRow();
                    dr12[0] = ((Label)grv.FindControl("lblID")).Text;
                    dr12[1] = "1";
                    ((DataTable)ViewState["ExTables"]).Rows.Add(dr12);
                }

            }
        }*/
    }
    protected void selectusers_click(object sender, EventArgs e)
    {
        tblSpecific.Visible = true;
        /// updTree.Update();

    }


    protected void BtnUploadModel_Click(object sender, EventArgs e)
    {
        try
        {

            Int16 UploadCheck = 0;
            UploadCheck = UploadFile.IsExcelFile(FlupldModel, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                DataSet Ds = new DataSet();
                ViewState["TobeUploaded"] = strUploadedFileName;
                Ds = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
                SchemeData objScheme = new SchemeData();
                objScheme.ModelDetailXML = Ds.GetXml();
                DataSet ds = objScheme.GetModelsDetail();
                if (objScheme.OutParam == 0)
                {
                    grdModelsExcluded.DataSource = ds.Tables[0];
                    grdModelsExcluded.DataBind();
                    DataTable dtExclude = getFortnights();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataRow dr3 = dtExclude.NewRow();
                        dr3["OFID"] = dr["ModelId"];
                        dr3["Status"] = "1";
                        dtExclude.Rows.Add(dr3);

                    }

                    ViewState["ExcludeModel"] = dtExclude;
                    // dvUpload.Visible = true;
                }
                else if (objScheme.OutParam == 1)
                {
                    // dvUpload.Visible = false;
                    uclblMessage.XmlErrorSource = ds.GetXml();
                    return;
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

        }
    }
    protected void LnkModelDetail_Click(object sender, EventArgs e)
    {
        try
        {
            using (ProductData obj = new ProductData())
            {
                if (cmbComponentType.SelectedValue == "12" || cmbComponentType.SelectedValue == "21")
                {
                    obj.ExcludedSelectionMode = (int)EnumData.eExcludedModels.Model;
                }
                else if (cmbComponentType.SelectedValue == "14" || cmbComponentType.SelectedValue == "22")
                {
                    obj.ExcludedSelectionMode = (int)EnumData.eExcludedModels.Product;
                }
                else if (cmbComponentType.SelectedValue == "15" || cmbComponentType.SelectedValue == "23")
                {
                    obj.ExcludedSelectionMode = (int)EnumData.eExcludedModels.Brand;
                }
                else
                {
                    obj.ExcludedSelectionMode = (int)EnumData.eExcludedModels.SKU;
                }
                DataTable DTskuinfo = obj.SelectExcludedInfo();
                if (DTskuinfo.Columns.Contains("ModelId"))
                {
                    DTskuinfo.Columns.Remove("ModelId");
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(DTskuinfo.Copy());
                if (DTskuinfo != null && DTskuinfo.Rows.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Model Detail List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.eRetailer + 2);
                }
            }
        }
        catch (Exception ex)
        {
            // ucmassege1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void lnkModelDetailtemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtExcludeModelTemplate = new DataTable();
            dtExcludeModelTemplate.Columns.Add("ModelName", typeof(string));
            dtExcludeModelTemplate.AcceptChanges();
            DataSet ds = new DataSet();
            ds.Tables.Add(dtExcludeModelTemplate.Copy());
            String FilePath = Server.MapPath("../../");
            string FilenameToexport = "Models to be excluded";
            PageBase.RootFilePath = FilePath;
            PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.eRetailer + 2);
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void lnkDownloadSalesChannelDetals_Click(object sender, EventArgs e)
    {
        try
        {
            using (SchemeData objscheme = new SchemeData())
            {
                DataSet objDs = null;
                objDs = objscheme.GetSalesChannelDetailForExclude();

                if (objDs != null || objDs.Tables[1].Rows.Count > 0)
                {
                    DataSet ds = new DataSet();
                    ds.Tables.Add(objDs.Tables[1].Copy());

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Sales Channel Detail List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.eRetailer + 2);


                }

            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }

    }
    protected void lnkDownloadSalesChannelTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtExcludeSAlesChannelTemplate = new DataTable();
            dtExcludeSAlesChannelTemplate.Columns.Add("ChannelCode", typeof(string));
            dtExcludeSAlesChannelTemplate.Columns.Add("ChannelType", typeof(string));
            dtExcludeSAlesChannelTemplate.AcceptChanges();
            DataSet ds = new DataSet();
            ds.Tables.Add(dtExcludeSAlesChannelTemplate.Copy());
            String FilePath = Server.MapPath("../../");
            /* string FilenameToexport = "Sales Channel to be excluded"; */
            string FilenameToexport = "Sales Channel to include_";
            PageBase.RootFilePath = FilePath;
            PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.eRetailer + 2);
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnUploadSalesChannel_Click(object sender, EventArgs e)
    {

        try
        {

            Int16 UploadCheckSales = 0;
            UploadCheckSales = UploadFile.IsExcelFile(flupldSalesChannel, ref strUploadedFileName);
            if (UploadCheckSales == 1)
            {
                DataSet DsImport = new DataSet();
                ViewState["TobeUploadedSalesChannel"] = strUploadedFileName;

                DsImport = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploadedSalesChannel"].ToString());


                var DistinctChannelType = (from r in DsImport.Tables[0].AsEnumerable()
                                           select r["ChannelType"]).Distinct().ToList();

                string incorrectChannelType = string.Empty;

                for (int j = 0; j < DistinctChannelType.Count; j++)
                {
                    if (DistinctChannelType[j].ToString().ToLower().Trim() == "")
                    {
                        uclblMessage.ShowInfo("Channel type cannot be blank.");
                        return;
                    }
                    Int16 iscorrectChannelType = 0;
                    foreach (ListItem li in chckUserType.Items)
                    {
                        if (li.Selected)
                        {
                            if (DistinctChannelType[j].ToString().ToLower() == li.Text.ToLower())
                            {
                                iscorrectChannelType = 1;
                                break;
                            }
                        }

                    }
                    if (iscorrectChannelType == 0)
                    {
                        //incorrectChannelType = incorrectChannelType + " " + DistinctChannelType[j].ToString();
                        incorrectChannelType = (incorrectChannelType == string.Empty || incorrectChannelType == "") ? incorrectChannelType + " " + DistinctChannelType[j].ToString() : incorrectChannelType + ", " + DistinctChannelType[j].ToString();
                    }
                }

                if (incorrectChannelType != string.Empty || incorrectChannelType != "")
                {
                    uclblMessage.ShowInfo("Incorrect Channel type " + incorrectChannelType);
                    return;
                }
                /* #CC01 Add Start */
                #region CheckingCheckboSelectedOptions
                foreach (ListItem li in chckUserType.Items)
                {
                    Int16 iscorrectChannelType = 0;

                    if (li.Selected)
                    {
                        for (int j = 0; j < DistinctChannelType.Count; j++)
                        {

                            if (DistinctChannelType[j].ToString().ToLower() == li.Text.ToLower())
                            {
                                iscorrectChannelType = 1;
                                break;
                            }
                        }
                        if (iscorrectChannelType == 0)
                        {
                            incorrectChannelType = (incorrectChannelType == string.Empty || incorrectChannelType == "") ? incorrectChannelType + " " + li.Text.ToLower() : incorrectChannelType + ", " + li.Text.ToLower();
                        }
                    }
                }
                if (incorrectChannelType != string.Empty || incorrectChannelType != "")
                {
                    uclblMessage.ShowInfo("Incorrect Channel type " + incorrectChannelType);
                    return;
                }
                #endregion CheckingCheckboSelectedOptions
                /* #CC01 Add End */
                SchemeData objScheme = new SchemeData();
                objScheme.GetSalesChannelDetailXML = DsImport.GetXml();
                DataSet ds = objScheme.GetSalesChannelDetail();
                if (objScheme.OutParam == 0)
                {
                    grdUser.DataSource = DsImport.Tables[0];
                    grdUser.DataBind();
                    string strSalesChannelIDs = string.Empty;
                    string strRetailerIds = string.Empty;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        /* strSalesChannelIDs = strSalesChannelIDs + "," + dr["ChannelID"].ToString(); #CC01 Comment */
                        if (dr["ChannelType"].ToString().ToLower() == "retailer" && dr["SalesChannelTypeID"].ToString() == "101")
                        {
                            strRetailerIds = strRetailerIds + "," + dr["ChannelID"].ToString();
                        }
                        /* #CC01 Add Start */
                        else
                        {
                            strSalesChannelIDs = strSalesChannelIDs + "," + dr["ChannelID"].ToString();
                        }/* #CC01 Add End */
                    }
                    /* #CC01 Add Start */

                    DataView view = new DataView(ds.Tables[0]);
                    DataTable distinctValues = view.ToTable(true, "SalesChannelTypeID", "SalesChannelLevel");
                    /*DataRow[] drr = distinctValues.Select("Student=' " + id + " ' ");*/
                    DataRow[] drr = distinctValues.Select("SalesChannelTypeID='101' ");
                    for (int i = 0; i < drr.Length; i++)
                        drr[i].Delete();
                    distinctValues.AcceptChanges();

                    string strLevelIDs = string.Empty, strSalesChannelTypeIds = string.Empty, strLocationIds = string.Empty;
                    foreach (DataRow dr in distinctValues.Rows)
                    {

                        strLevelIDs = strLevelIDs + "," + dr["SalesChannelLevel"].ToString();
                        strSalesChannelTypeIds = strSalesChannelTypeIds + "," + dr["SalesChannelTypeID"].ToString();

                    }

                    /* DataTable distinctLocationIds = view.ToTable(true, "OrgnhierarchyID");
                     DataRow[] drr2 = distinctLocationIds.Select("OrgnhierarchyID='0' ");
                     for (int i = 0; i < drr2.Length; i++)
                         drr2[i].Delete();
                     distinctLocationIds.AcceptChanges();
                     foreach (DataRow dr in distinctLocationIds.Rows)
                     {

                         strLocationIds = strLocationIds + "," + dr["OrgnhierarchyID"].ToString();

                     }
                       ViewState["LocationId"] = strLocationIds;
                     */

                    ViewState["StrLevelIDs"] = strLevelIDs;
                    ViewState["StrSalesChannelTypeIds"] = strSalesChannelTypeIds;
                    /* #CC01 Add End */
                    ViewState["SalesChannelIds"] = strSalesChannelIDs;
                    ViewState["RetailerIds"] = strRetailerIds;

                    /*
                    DataTable dtExclude = getFortnights();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataRow dr3 = dtExclude.NewRow();
                        dr3["OFID"] = dr["ModelId"];
                        dr3["Status"] = "1";
                        dtExclude.Rows.Add(dr3);

                    }

                    ViewState["ExcludeModel"] = dtExclude;
                    dvUpload.Visible = true;
                    */
                }
                else if (objScheme.OutParam == 1)
                {
                    //dvUpload.Visible = false;
                    uclblMessage.XmlErrorSource = ds.GetXml();
                    return;
                }
            }
            else if (UploadCheckSales == 2)
            {
                uclblMessage.ShowError(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheckSales == 3)
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

        }


    }

    public void ClearAllViewState()
    {
        try
        {
            ViewState["Scheme"] = null;
            ViewState["ExTables"] = null;
            ViewState["ExcludeModel"] = null;
            ViewState["IsTarget"] = null;
            ViewState["SalesChannelIds"] = null;
            ViewState["RetailerIds"] = null;
            ViewState["TobeUploaded"] = null;
            ViewState["TobeUploadedSalesChannel"] = null;
            /* #CC01 Add Start */
            ViewState["StrLevelIDs"] = null;
            ViewState["StrSalesChannelTypeIds"] = null;
            /* ViewState["LocationId"] = null;
              #CC01 Add End */
        }
        catch (Exception ex)
        {

        }
    }
}

