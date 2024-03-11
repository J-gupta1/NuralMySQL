using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data;
using Cryptography;
using BussinessLogic;
public partial class Masters_Common_ViewSchemeDetails : PageBase
{
    DataTable schemeInfo;
    string scehemename;
    string strUploadedFileName;
    UploadFile UploadFile = new UploadFile();
    int intSchemeID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //rdoAccessType.SelectedValue = "1";
                pnlUpload.Visible = false;
                BindHierarchyLevelLocationTree();
                BindSalesChannelLocationTree();
                string strURL = Request.Url.ToString();
                char c = '=';
                string[] strp = strURL.Split(c);
                ViewState["SchemeID"] = Convert.ToInt32(strp[1]);
                getinfo();
                btnSave.Visible = false;
            }

    }
     public void getinfo()
        {
            using (SchemeData obj = new SchemeData())
            {
                obj.SchemeID = Convert.ToInt32(ViewState["SchemeID"]);
                DataSet ds = obj.GetSchemeDetailsInformation();
                schemeInfo = ds.Tables[0];

                ViewState["IsSkuWise"] = obj.BasedOn;
                if (Convert.ToInt16(ViewState["IsSkuWise"]) == 2)
                {
                    grdDetail.Columns[0].Visible = false;
                    hlkSKU.Visible = false;
                    
                }
                else if (Convert.ToInt16(ViewState["IsSkuWise"]) == 1) 
                {
                    grdDetail.Columns[1].Visible = false;
                    hlkBrand.Visible = false;
                }
                grdDetail.DataSource = ds.Tables[0];
                grdDetail.DataBind();
                pnlGrid.Visible = true;
                updGrid.Update();
                pnlTree.Visible = true;
                btnTree.Visible = false;
                Filllevels(ds.Tables[1]);
                fillusers(ds.Tables[2]);
               
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

        private string FindTreeChild(TreeView treeView)
        {
            string Locations = "";
            foreach (TreeNode root in treeView.Nodes)
            {
                if (root.Checked == false)
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
        public void Filllevels(DataTable dt)
        {
            if (tvLevel.Nodes.Count > 0)
            {
                foreach (TreeNode node in tvLevel.Nodes)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (node.Value == dr["HierarchyLevelID"].ToString())
                        {
                            node.Checked = true;
                            foreach (TreeNode child in node.ChildNodes)
                            {
                               child.Checked = true;
                                
                            }
                        }

                    }
                }
            }

            if (tvSalesChannel.Nodes.Count > 0)
            {
                foreach (TreeNode node in tvSalesChannel.Nodes)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (node.Value == dr["SalesChannelTypeID"].ToString())
                        {
                            node.Checked = true;
                            foreach (TreeNode child in node.ChildNodes)
                            {
                               
                                child.Checked = true;
                            }
                        }

                    }
                }

            }

           }

        public void fillusers(DataTable dt)
        {
            foreach (TreeNode node in tvSalesChannel.Nodes)
            {
                foreach (TreeNode child in node.ChildNodes)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (child.Value == dr["SalesChannelID"].ToString())
                        {
                           
                            
                                child.Checked = true;
                            
                        }
                    }
                }

            }

            foreach (TreeNode node in tvLevel.Nodes)
            {
                foreach (TreeNode child in node.ChildNodes)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (child.Value == dr["OrgnhierarchyID"].ToString())
                        {
                            child.Checked = true;
                        }
                    }

                }

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
        private void BindSalesChannelLocationTree()
        {
            using (BulletinData objBulletinData = new BulletinData())
            {
                DataSet objDs = null;
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
        
        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            DataSet objDS = new DataSet();
            try
            {
                Int16 Upload = 0;
                byte isSuccess = 1;
                String RootPath = Server.MapPath("../../");
                UploadFile.RootFolerPath = RootPath;

                Upload = UploadFile.IsExcelFile(flupdScheme, ref strUploadedFileName);
                if (Upload == 1)
                {
                    UploadFile.UploadedFileName = strUploadedFileName;
                    UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eScheme;

                    if (Convert.ToInt16(ViewState["IsSkuWise"]) == 2)
                    {
                        isSuccess = UploadFile.uploadValidExcel(ref objDS, "SchemeBrandWise");
                    }
                    else
                    {
                        isSuccess = UploadFile.uploadValidExcel(ref objDS, "SchemeSKUWise");
                    }
                    switch (isSuccess)
                    {
                        case 0:
                            uclblMessage.ShowError(UploadFile.Message);
                            break;
                        case 2:
                            uclblMessage.ShowError(Resources.Messages.CheckErrorGrid);
                            pnlGrid.Visible = true;
                            grdDetail.Columns[5].Visible = true;
                            grdDetail.DataSource = objDS;
                            grdDetail.DataBind();
                            updGrid.Update();
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
        private void InsertData(DataSet objDS)
        {
            ViewState["Table"] = objDS.Tables[0];
            grdDetail.DataSource = objDS.Tables[0];
            grdDetail.DataBind();
            pnlGrid.Visible = true;
            updGrid.Update();
            btnSave.Visible = true;
        }

        protected void rblAccessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rdoAccessType.SelectedValue == "1")
            //{
            //    pnlUpload.Visible = false;
            //    btnSave.Visible = false;
            //    pnlTree.Visible = true;
            //    //btnTree.Visible = false;
            //    getinfo();  
            //}
            //if (rdoAccessType.SelectedValue == "2")
            //{
            //    pnlGrid.Visible = false;
            //    updGrid.Update();
            //    pnlUpload.Visible = true;
            //    pnlTree.Visible = false;
                
            //}
            //if (rdoAccessType.SelectedValue == "3")
            //{
            //    pnlGrid.Visible = false;
            //    updGrid.Update();
            //   // btnTree.Visible = true;
            //    pnlUpload.Visible = false;
            //    pnlTree.Visible = true;
            //}
            

        }


        protected void btnSaveTree_Click(object sender, EventArgs e)
        {
            using (SchemeData objScheme = new SchemeData())
            {
                objScheme.SchemeID = Convert.ToInt32(ViewState["SchemeID"]);
                string LevelIds = FindTreeRoots(tvLevel);
                string LocationIds = FindTreeChild(tvLevel);
                if (LevelIds.Length > 0 || LevelIds != null)
                    objScheme.LevelIds = LevelIds;
                if (LocationIds.Length > 0 || LocationIds != null)
                    objScheme.LocationIds = LocationIds;
                string SalesChannelTypeIds = FindTreeRoots(tvSalesChannel);
                string SalesChannelIds = FindTreeChild(tvSalesChannel);
                if (SalesChannelIds.Length > 0 || SalesChannelIds != null)
                    objScheme.SalesChannelIds = SalesChannelIds;
                if (SalesChannelTypeIds.Length > 0 || SalesChannelTypeIds != null)
                    objScheme.SalesChannelTypeIds = SalesChannelTypeIds;
                objScheme.UpdateSchemeMappingInfo();
                uclblMessage.ShowInfo(Resources.Messages.EditSuccessfull);
                getinfo();
            }

        }
       protected void btnSave_Click(object sender, EventArgs e)
        {

            DataTable DtDetail = new DataTable();
            DataTable dtScheme = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                dtScheme = ObjCommom.GettvpTableUploadScheme();
            }
            DtDetail = (DataTable)ViewState["Table"];
            foreach (DataRow dr in DtDetail.Rows)
            {
                DataRow drow = dtScheme.NewRow();

                if (Convert.ToInt16(ViewState["IsSkuWise"]) == 2)
                {
                    drow[0] = dr["BrandCode"].ToString();
                }
                else
                {
                    drow[0] = dr["SKUCode"].ToString();
                }
                drow[1] = dr["MinSlab"];
                drow[2] = dr["MaxSlab"].ToString();
                drow[3] = dr["RewardedQuantity"];
                dtScheme.Rows.Add(drow);
            }
            dtScheme.AcceptChanges();
            using (SchemeData objScheme = new SchemeData())
            {
                objScheme.SchemeID = Convert.ToInt32(ViewState["SchemeID"]);
                objScheme.UpdateSchemeProductInfo(dtScheme);
                if (objScheme.ErrorDetailXML != null && objScheme.ErrorDetailXML != string.Empty)
                {
                    uclblMessage.XmlErrorSource = objScheme.ErrorDetailXML;
                    return;
                }
                if (objScheme.Error != null && objScheme.Error != "")
                {
                    uclblMessage.ShowError(objScheme.Error);
                    return;
                }
              
                uclblMessage.ShowInfo(Resources.Messages.EditSuccessfull);
                getinfo();
            }



        }
       protected void LnkDownloadRefCode_Click(object sender, EventArgs e)
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


       protected void LBViewScheme_Click(object sender, EventArgs e)
       {
           Response.Redirect("SchemeView.aspx", false);
       }
       protected void exportToExel_Click(object sender, EventArgs e)
       {
           getinfo();
           DataTable dt = schemeInfo.Copy();
           scehemename = dt.Rows[0]["SchemeName"].ToString();
           string[] DsCol;
           if (Convert.ToInt16(ViewState["IsSkuWise"]) == 2)
           {
               DsCol = new string[] { "BrandCode", "MinSlab", "MaxSlab", "RewardedQuantity", "CurrentStatus" };
           }
           else
           {
               DsCol = new string[] { "SKUCode", "MinSlab", "MaxSlab", "RewardedQuantity", "CurrentStatus" };
           }
           DataTable DsCopy = new DataTable();
           dt = dt.DefaultView.ToTable(true, DsCol);
           dt.Columns["CurrentStatus"].ColumnName = "Status";

           if (dt.Rows.Count > 0)
           {
               try
               {
                   DataSet dtcopy = new DataSet();
                   dtcopy.Merge(dt);
                   dtcopy.Tables[0].AcceptChanges();
                   String FilePath = Server.MapPath("../../");
                   string strName = string.Format("{0}- Details",scehemename); 
                   string FilenameToexport = strName;
                   PageBase.RootFilePath = FilePath;
                   PageBase.ExportToExecl(dtcopy, FilenameToexport);
                   //ViewState["Table"] = null;
               }
               catch (Exception ex)
               {
                   uclblMessage.ShowInfo(ex.Message.ToString());
                   PageBase.Errorhandling(ex);
               }
           }
           else
           {
               uclblMessage.ShowError(Resources.Messages.NoRecord);

           }




       }
       protected void cmbComponentType_SelectedIndexChanged1(object sender, EventArgs e)
       {
       }
}
