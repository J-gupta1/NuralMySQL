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


    public partial class Masters_Common_ManageScheme : PageBase 
    {
        string strUploadedFileName;
        UploadFile UploadFile = new UploadFile();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               
                tvLevel.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                tvSalesChannel.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                uclblMessage.ShowControl = false;
               if (!IsPostBack)
                {

                    //cmbComponentType.Attributes.Add("onChange", "return OnSelectedIndexChange();"); 
                    BindHierarchyLevelLocationTree();
                    BindSalesChannelLocationTree();
                    fillComponent();
                    BindTimeperiod();
                    fillPayoutBase();
                    updSeprate.Update();
                    ucFromDate.Date = Fromdate;
                    ucToDate.Date = ToDate;

                }

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
                cmbPayoutBase.Items.Insert(0,new ListItem("Select","0"));
                updMain.Update();
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
                    if (strbase1[1].ToString() == "0")
                    {

                        if (strbase[1].ToString() == "0")
                        {
                            isSuccess = UploadFile.uploadValidExcel(ref objDS, "TargetItemWise");
                        }
                        else
                        {
                            uclblMessage.ShowError("please select valid sheet");
                            return;
                        }
                    }
                    else if (strbase1[1].ToString() == "1")
                    {
                        if (strbase[1].ToString() == "1")
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
                uclblMessage.ShowError("From date must not be greater than to date ");
                return;
            }

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

                    if (ViewState["ExTables"] != null)
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
                  

                    using (SchemeData objScheme = new SchemeData())
                    {
                        string strRetailerlist = string.Empty;
                        objScheme.SchemeName = txtScheme.Text.Trim();
                        string LevelIds = FindTreeRoots(tvLevel);
                        string LocationIds = FindTreeChild(tvLevel,ref strRetailerlist);
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
                        objScheme.IsTarget = Convert.ToInt16(ViewState["IsTarget"]);
                        objScheme.FromDate = ucFromDate.Date;
                        objScheme.ToDate = ucToDate.Date;
                        if (strRetailerlist.Length > 0 || strRetailerlist != null)
                        {
                            objScheme.RetailersIds = strRetailerlist;
                        }
                        objScheme.ComponentTypeID = Convert.ToInt16(str1[0]);
                        
                        objScheme.PayOutBase = Convert.ToInt32(str2[0]);

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
                        updscheme.Update();
                       


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

        private string FindTreeChild(TreeView treeView,ref string strRetailerInfo)
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
                            if (child.Parent.Value=="101")
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



        protected void LBViewScheme_Click(object sender, EventArgs e)
        {
            Response.Redirect("SchemeView.aspx", false);
        }
        protected void btnExclude_click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ExTables"];
            pnlExclude.Visible = false;
            updExclude.Update();
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
                grdExcluded.DataSource = skuinfo;
                grdExcluded.DataBind();
                updExclude.Update();
            }
        }

        
        protected void excludeModels_click(object sender, EventArgs e)
        {
           
            pnlExclude.Visible = true;
            updExclude.Update();
        }

        protected void grdExclude_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdExcluded.PageIndex = e.NewPageIndex;
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
            updExclude.Update();
            pnlgrid.Visible = false;
            updscheme.Update();
            tblSpecific.Visible = false;
            updTree.Update();
            pnlExclude.Visible = false;
            updExclude.Update();
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
                pnlIsUserWise.Visible = false;
                updIsUserWise.Update();
                pnlTargetdr.Visible = true;
                pnldaterng.Visible = false;
                updSeprate.Update();
                ViewState["IsTarget"] = 1;
            }
            else
            {
                pnlIsUserWise.Visible = true;
                updIsUserWise.Update();
                pnlTargetdr.Visible = false;
                pnldaterng.Visible = true;
                updSeprate.Update();
                ViewState["IsTarget"] = "2";
            }
            if (strids[2].ToString() == "1" || cmbComponentType.SelectedValue == "0")
            {
                //pnlRefrenceTemplate.Visible = true;
                updTemplates.Update();
            }
            else
            {
                //pnlTempltatetarget.Visible = true;
                updTemplates.Update();
            }

             pnlexclBtn.Visible = true;
             updExclude.Update();
             FillSKU();
        }


        protected void oncheckedchange(object sender, EventArgs e)
        {
            int trd = 1;
            DataTable dt = GetExcludedModels();
            if (ViewState["ExTables"] == null)
            {
                ViewState["ExTables"] = dt;
            }
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
                    if(trd == 1)
                    {
                        DataRow dr12 = ((DataTable)ViewState["ExTables"]).NewRow();
                        dr12[0] = ((Label)grv.FindControl("lblID")).Text;
                        dr12[1] = "1";
                        ((DataTable)ViewState["ExTables"]).Rows.Add(dr12);
                    }
                    
                }
            }
        }
        protected void selectusers_click(object sender, EventArgs e)
        {
            tblSpecific.Visible = true;
            updTree.Update();

        }
      

}

