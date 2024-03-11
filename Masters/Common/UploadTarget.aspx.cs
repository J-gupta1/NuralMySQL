#region "Using"
using System;
using System.Drawing;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Microsoft.ApplicationBlocks.Data;
using BussinessLogic;
using DataAccess;
using System.Linq;
using ExportExcelOpenXML;
using BusinessLogics;


#endregion

#region ChangeLog
/* ------------------------------------------------------------------------------------------
 * Change Log 
 * ------------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 31-Jan-2018, Sumit Maurya, #CC01, Modified for both saleschannel and orgn Hierarchy (done for Comio).
 * 05-Feb-2018, Sumit Maurya, #CC02, New button added to update Target.
 * 17-Feb-2018, Sumit Maurya, #CC03, Corrected for reatiler (Done fro comio).
 * 02-May-2018, Sumit Maurya, #CC04, Changes implemented according to BrandGroup (Done for Motorola)
 * 07-May-2018, Sumit Maurya, #CC05, Tablename to check validation provided according to dropdown option "Template Category"
 *                                    selected by user.
 * 23-May-2018, Sumit Maurya, #CC08, Userid provided to get data according to user (Done for motorola).
 * 24-Jul-2018, Sumit Maurya, #CC09, Change log was not mentioned for Categorywise Target. It is mentioned to get changed for it (Done for Karbonn).
 * 17-Nov-2022, Rinku Sharma, #CC10, Add WOD .
 * ------------------------------------------------------------------------------------------
 */
#endregion ChangeLog

public partial class UploadTarget : PageBase
    {
        #region "Page Level Variables"
        string strUploadedFileName = string.Empty;
        UploadFile UploadFile = new UploadFile();
        #endregion

        #region "Page Load events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                uclblMessage.ShowControl = false ;
                if (!Page.IsPostBack)
                {
                    BindType();/* #CC01 Added */
                    //ddlUserType.Items.Insert(0, new ListItem("Select", "0")); /* #CC01 Added */
                    ViewState["Detail"] = null;
                    BindTargetCategory();
                    BindTargetType();
                    /* BindChannelType(); #CC01 Commented */
                    BindTargetBased();
                    //BindTimePeriod();
                   

                }

            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);
                uclblMessage.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }

        }
        //private void BindTimePeriod()
        //{
        //    using (TargetData objTarget = new TargetData())
        //    {


        //        string[] str = { "FinancialCalenderID", "FinancialCalenderFortnight" };
        //            PageBase.DropdownBinding(ref ddlPeriod, objTarget.GetTimePeriod(), str);
               
        //    };
        //}
        #endregion

        #region "User Defined Methods"
        void ClearForm()
        {
            pnlGrid.Visible = false;
            ViewState["Detail"] = null;

            //ddlUserType.SelectedValue = "0";
            ddlTargetCategory.SelectedValue = "0";
            ddlTargetType.SelectedValue = "0";
            //ddlPeriod.SelectedValue = "0";
            txttargetName.Text = "";
            ddlTargetBased.SelectedValue = "0";
            //ucfromDate.Date = "";
            //ucToDate.Date = "";
           
        }
        private void BindTargetType()
        {
            try
            {
                ddlTargetType.Items.Clear();

                ddlTargetType.Items.Add(new ListItem("Quantity", "1"));
                ddlTargetType.Items.Add(new ListItem("Value", "2"));
                ddlTargetType.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);
                uclblMessage.ShowError(ex.Message);
            }
        }
        private void BindTargetBased()
        {
            try
            {
                ddlTargetBased.Items.Clear();

                ddlTargetBased.Items.Add(new ListItem("Sales", "1"));
                ddlTargetBased.Items.Add(new ListItem("Purchase", "2"));
                ddlTargetBased.Items.Add(new ListItem("WoD", "3"));/* #CC10 Added */
                ddlTargetBased.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);
                uclblMessage.ShowError(ex.Message);
            }
        }
        private void BindTargetCategory()
        {
            try
            {
                ddlTargetCategory.Items.Clear();
                ddlTargetCategory.Items.Add(new ListItem("SKU Wise", "1"));
                ddlTargetCategory.Items.Add(new ListItem("Consolidated", "2")); /* #CC09 Added */
                ddlTargetCategory.Items.Add(new ListItem("Product Category Wise", "3"));
                ddlTargetCategory.Items.Add(new ListItem("WOD", "4"));/* #CC10 Added */

                ddlTargetCategory.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);
                uclblMessage.ShowError(ex.Message);
            }

        }
        //private void BindLevel()
        //{
        //    try
        //    {
                
        //        using (TargetData ObjTarget = new TargetData())
        //        {
        //            ObjTarget.showLevel = false;
        //            if (PageBase.SalesChanelID==0)
        //                ObjTarget.UserType=1;
        //            else
        //                ObjTarget.UserType =2;
        //            if (PageBase.SalesChanelID == 0)
        //            {
        //                ObjTarget.UserTypeID = Convert.ToInt16(PageBase.HierarchyLevelID);
        //            }
        //            else
        //            {
        //                ObjTarget.UserTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
        //            }
        //            string[] str = { "TargetToType", "TargetUserType" };
        //            PageBase.DropdownBinding(ref ddlLvelType, ObjTarget.GetTargetLevelUser(), str);
        //        };
              
        //    }
        //    catch (Exception ex)
        //    {
        //        PageBase.Errorhandling(ex);
        //        uclblMessage.ShowError(ex.Message);
        //    }

        //}
        private void InsertData(DataSet objDS)
        {

            var query = from r in objDS.Tables[0].AsEnumerable()
                        /* where ((Convert.ToInt32(r["Target"]) <= 0)) #CC02 Commented */
                        where ((Convert.ToInt32(r["Target"]) < 0)) /* #CC02 Added */
                        select new
                        {
                            Target = Convert.ToString(r["Target"])
                        };
            if (query != null)
            {
                if (query.Count() > 0)
                {
                    uclblMessage.ShowInfo("Negative Quantity is Not Allowed");
                    return;
                }
            }


            pnlGrid.Visible = true;
            btnSaveTarget.Visible = true;
            GridTarget.DataSource = objDS;
            ViewState["Detail"] = objDS.Tables[0];
            GridTarget.DataBind();

        }
        bool pagevalidate()
        {
            //if (ServerValidation.IsDate(ucfromDate.Date, true) != 0 || ServerValidation.IsDate(ucToDate.Date, true) != 0)
            //{
            //    uclblMessage.ShowWarning(Resources.Messages.InvalidDate);
            //    return false;
            //}
            //if (Convert.ToDateTime(ucfromDate.Date) > Convert.ToDateTime(ucToDate.Date))
            //{
            //    uclblMessage.ShowWarning(Resources.Messages.InvalidDate);
            //    return false;
            //}
            if (ddlType.SelectedValue == "0" || ddlTargetType.SelectedValue == "0"||ddlTargetCategory.SelectedValue=="0" ||ddlTargetBased.SelectedValue=="0" ||txttargetName.Text=="" )
            {
                uclblMessage.ShowWarning(Resources.Messages.MandatoryField);
                return false;
            }
            return true;
        }
        #endregion

        #region Button Event
        protected void btnSaveTarget_Click(object sender, EventArgs e)
        {
            if (IsPageRefereshed == true)          
            {
                return;
            }
            if (!pagevalidate())
            {
                return;
            }
            try
            {
                if (ViewState["Detail"] != null)
                {
                
                using (TargetData ObjRTarget = new TargetData())
                {

                    DataTable DtDetail = new DataTable();
                    DataTable dtTarget = new DataTable();
                    using (CommonData ObjCommom = new CommonData())
                    {
                        dtTarget = ObjCommom.GettvpTableUploadTarget();
                    }
                    DtDetail = (DataTable )ViewState["Detail"];
                    foreach (DataRow dr in DtDetail.Rows)
                    {
                        DataRow drow = dtTarget.NewRow();
                        drow[0] = dr["TargetFor"].ToString();
                        if (ddlTargetCategory.SelectedIndex == 1)
                            drow[1] = dr["SKUCode"].ToString();
                        /* #CC09 Add Start */
                        else  if(ddlTargetCategory.SelectedIndex == 3)
                            drow[1] = Convert.ToString(dr["ProductCategoryCode"]);/* #CC09 Add End */
                        else if (ddlTargetCategory.SelectedIndex == 4)
                            drow[1] = Convert.ToString(dr["SKUCode/ProductCategoryCode/BrandGroupCode"]);/* #CC09 Add End */
                        else
                            drow[1] = Convert.ToString(dr["BrandGroupCode"]);/* #CC04 Added */ /* dr["BrandCode"].ToString(); #CC04 Commented */
                        drow[2] = dr["Target"];
                        drow[3] = ddlTargetType.SelectedValue;
                      /*  drow[4] = txttargetName .Text.Trim(); #CC02 Commented */
                        drow[4] = ddlTargetSaveUpdate.SelectedValue == "2" ? ddlTarget.SelectedValue : txttargetName.Text.Trim(); /* #CC02 Added */

                        drow[5] = "";// (ddlPeriod.SelectedItem.Value);
                        drow[6] = ddlTargetCategory.SelectedValue;
                       /* drow[7] = ddlUserType.SelectedValue; #CC03 Commented */
                       /* drow[8] = ddlUserType.SelectedValue=="101"?3: 2;   //I found there is no code for hierarchy #CC01 Commented  */
                        /* drow[8] = ddlType.SelectedValue;  #CC07 Commented *//* #CC01 Added */
                        /* #CC07 Add Start */

                        drow[7] = ddlType.SelectedValue;
                        drow[8] = ddlType.SelectedValue ==Convert.ToString( PageBase.RetailerEntityTypeID) ? "3" : ddlType.SelectedValue; 
                        /* #CC07 Add End */
                        
                        drow[9] = ddlTargetBased.SelectedValue;
                        dtTarget.Rows.Add(drow);
                    }
                    dtTarget.AcceptChanges();
                    using (TargetData ObjTarget = new TargetData())
                    {
                        ObjTarget.ErrorMessage = "";
                        ObjTarget.UserId = PageBase.UserId;
                        ObjTarget.TargetFrom = TargetFromDate.GetDate;
                        ObjTarget.TargetTo = TargetToDate.GetDate;
                        ObjTarget.InsUpdTarget = Convert.ToInt16(ddlTargetSaveUpdate.SelectedValue); /* #CC02  Added */
                        ObjTarget.UploadTarget(dtTarget);

                        if (ObjTarget.ErrorDetailXML != null && ObjTarget.ErrorDetailXML != string.Empty)
                        {
                            uclblMessage.XmlErrorSource = ObjTarget.ErrorDetailXML;
                            return;
                        }
                        else if (ObjTarget.ErrorMessage != null && ObjTarget.ErrorMessage != "")
                        {
                            uclblMessage.ShowError(ObjTarget.ErrorMessage);
                            return;
                        }
                        uclblMessage.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        if (ddlTargetSaveUpdate.SelectedValue == "1")
                        {
                            uclblMessage.ShowSuccess(Resources.Messages.UpdateSuccessfull);
                        }
                        ClearForm();
                     }
                 }

                }


            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);
                uclblMessage.ShowError(ex.ToString(),GlobalErrorDisplay());
            }
          
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            uclblMessage.ShowControl = false;
            ClearForm();
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            btnSaveTarget.Visible = true;
          
            DataSet objDS = new DataSet();
            try
            {
                ViewState["Detail"] = null;
                GridTarget.DataSource = null;
                GridTarget.DataBind();
                pnlGrid.Visible = false;
                Int16 Upload = 0;
                byte isSuccess = 1;

                String RootPath = Server.MapPath("../../");
                UploadFile.RootFolerPath = RootPath;

                if (ddlTargetCategory.SelectedIndex == 0)
                {
                    uclblMessage.ShowInfo("Please Select Any target Category");
                    return;
                }
                Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
                if (Upload == 1)
                {
                    UploadFile.UploadedFileName = strUploadedFileName;
                    UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eTarget;
                    /* #CC05 Comment Start
                     if (ddlTargetCategory.SelectedIndex==1)
                        UploadFile.TemplateType = EnumData.eTargetTemplateType.eSKUWise;
                    else
                        UploadFile.TemplateType = EnumData.eTargetTemplateType.eSummary;
                    isSuccess = UploadFile.uploadValidExcel(ref objDS,"Target");
                     #CC05 Comment End  */
                    /* #CC09 Add Start */
                    string TargetType = string.Empty;
                    if (ddlTargetCategory.SelectedIndex == 1)
                    {
                        UploadFile.TemplateType = EnumData.eTargetTemplateType.eSKUWise;
                        TargetType = "SKUWiseTarget";
                    }
                    /* #CC09 Add Start */
                    else  if (ddlTargetCategory.SelectedIndex == 3)
                    {
                        UploadFile.TemplateType = EnumData.eTargetTemplateType.eProductCategoryWise;
                        TargetType = "ProductCategoryWiseTarget";
                    }/* #CC06 Add End */
                    else if (ddlTargetCategory.SelectedIndex == 4)/* #CC10 Added */
                    {
                        UploadFile.TemplateType = EnumData.eTargetTemplateType.eWOD;
                        TargetType = "WODWiseTarget";
                    }
                    else
                    {
                        UploadFile.TemplateType = EnumData.eTargetTemplateType.eSummary;
                        TargetType = "BrandWiseTarget";
                    }
                    isSuccess = UploadFile.uploadValidExcel(ref objDS, TargetType);

                    /* #CC05 Add End */


                    switch (isSuccess)
                    {
                        case 0:
                           /* uclblMessage.ShowWarning(UploadFile.Message); #CC02 Commented */
                            /* uclblMessage.ShowWarning("Invalid excel sheet,BrandCode/SKUCode missing or not in correct sequense, kindly download template again."); #CC04 Commented  */ /* #CC02 Added */

                             uclblMessage.ShowWarning("Invalid excel sheet,BrandGroupCode/SKUCode missing or not in correct sequense, kindly download template again."); /* #CC04 Added */
                            break;
                        case 2:
                            uclblMessage.ShowWarning(Resources.Messages.CheckErrorGrid);
                            pnlGrid.Visible = true;
                            btnSaveTarget.Visible = false;
                            GridTarget.DataSource = objDS;
                            GridTarget.DataBind();
                          
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

                PageBase.Errorhandling(ex);
                uclblMessage.ShowError(ex.ToString());
            }
        }
        protected void lnkDownload_Click(object sender, EventArgs e)
        {
          
            try
            {
                DataSet DsSku = new DataSet();

                using (TargetData ObjRTarget = new TargetData())
                {
                    
                        ObjRTarget.TemplateType = EnumData.eTargetTemplateType.eSKUWise;
                   
                 
                    DsSku.Merge(ObjRTarget.GetTargetTemplate());
                };
                if (DsSku.Tables[0].Rows.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SKU Wise Target Template";
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
        protected void LnkDownloadRefCode_Click(object sender, EventArgs e)
        {
           
            DataSet DsSku = new DataSet();
            try
            {
                using (TempSalesChannelData ObjSales = new TempSalesChannelData())
                {
                    ObjSales.ReqType = EnumData.eControlRequestTypeForEntry.eTarget;
                    ObjSales.UserID = PageBase.UserId;
                    if (ddlTargetSaveUpdate.SelectedValue == "1" && ddlTarget.SelectedIndex != 0)/* #CC02 Added */
                    ObjSales.TargetName = Convert.ToString(ddlTarget.SelectedValue); /* #CC02 Added */
                    DsSku = ObjSales.GetAllTemplateData();
                };
                if (DsSku.Tables.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code list";
                    PageBase.RootFilePath = FilePath;
                    //PageBase.ExportToExecl(DsSku, FilenameToexport, EnumData.eTemplateCount.eTarget+1);
                    string[] strExcelSheetName = { "SalesChannel", "OrganizationHierarchy", "SKU", "BrandGroup", "Retailer","ProductCatGroup" };
                    ChangedExcelSheetNames(ref DsSku, strExcelSheetName, 6);
                    if (DsSku.Tables.Count > 6)
                        DsSku.Tables.RemoveAt(6);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(DsSku, FilenameToexport, 6, strExcelSheetName);
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

                using (TargetData ObjRTarget = new TargetData())
                {

                    ObjRTarget.TemplateType = EnumData.eTargetTemplateType.eSummary;

                    DsSku.Merge(ObjRTarget.GetTargetTemplate());
                };
                if (DsSku.Tables[0].Rows.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Consolidated Target Template";
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

        //public void BindChannelType()
        //{
        //    using (TargetData objTarget = new TargetData())
        //    {
        //        objTarget.UserType = /* 2; #CC01 Commented */ Convert.ToInt16(ddlType.SelectedValue); /* #CC01 Added */
        //        objTarget.showLevel = true;
        //        objTarget.UserId = PageBase.UserId; /* #CC08 Added  */
        //        if (PageBase.SalesChanelID == 0)
        //        {
        //            objTarget.UserTypeID = Convert.ToInt16(PageBase.HierarchyLevelID);

        //            objTarget.OwnLevel = 1;
        //        }
        //        else
        //        {
        //            objTarget.UserTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
 
        //        }

        //        String[] colArray = { "ID", "TYPEName" };
        //        DataTable dt = objTarget.GetTargetLevelUser();
        //        ddlUserType.DataSource = dt;
        //        ddlUserType.DataTextField = "TYPEName";
        //        ddlUserType.DataValueField = "ID";
        //        ddlUserType.DataBind();
        //        ddlUserType.Items.Insert(0, new ListItem("Select","0"));

        //        if (ddlType.SelectedValue == "101")
        //        {
        //            /*ddlUserType.Items.Add(new ListItem("Retailer", "101"));*/
        //            ddlUserType.Items.Clear();
        //            ddlUserType.Items.Insert(0, new ListItem("Select", "0"));
        //            ddlUserType.Items.Add(new ListItem("Retailer", "3"));
        //        }
        //        //This page works on the Zadmin login
        //        // So i am coding according to this point
        //        //and till now it is for the SalesChannel Persons
        //    }
        //}
        //protected void ddlLvelType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ddlUserType.Items.Clear();
        //        if (ddlLvelType.SelectedIndex != 0)
        //        {
        //            using (TargetData objTarget = new TargetData())
        //            {
        //                objTarget.UserType = Convert.ToInt16(ddlLvelType.SelectedValue);
        //                objTarget.showLevel = true;

        //                if (PageBase.SalesChanelID == 0)
        //                {
        //                    objTarget.UserTypeID = Convert.ToInt16(PageBase.HierarchyLevelID);

        //                    objTarget.OwnLevel = 1;
        //                }
        //                else
        //                {
        //                    objTarget.UserTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
        //                    objTarget.OwnLevel = 2;
        //                }

        //                String[] colArray = { "ID", "TYPEName" };
        //                PageBase.DropdownBinding(ref ddlUserType, objTarget.GetTargetLevelUser(), colArray);
        //            }


        //        }
        //        else
        //        {
        //            ddlUserType.Items.Insert(0, new ListItem("Select", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PageBase.Errorhandling(ex);
        //        uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());

        //    }

        //}
        protected void GridTarget_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridTarget.PageIndex = e.NewPageIndex;
                if (ViewState["Detail"] != null)
                {
                    GridTarget.DataSource = (DataTable)ViewState["Detail"];
                    GridTarget.DataBind();
                }
            }
            catch (Exception ex)
            {
                uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            }

        }

        /* #CC01 Add Start */

        public void BindType()
        {
            try
            {
                ddlType.Items.Clear();
                using (TargetData objTarget = new TargetData())
                {
                    objTarget.UserType = 1;
                    objTarget.showLevel = false;

                    if (PageBase.SalesChanelID == 0)
                    {
                        objTarget.UserTypeID = Convert.ToInt16(PageBase.HierarchyLevelID);
                        objTarget.OwnLevel = 1;
                    }
                    else
                    {
                        objTarget.UserTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                    }
                    String[] colArray = { "TargetToType", "TargetUserType" };
                    objTarget.UserId = PageBase.UserId; /* #CC08 Added  */
                    DataTable dt = objTarget.GetTargetLevelUser();
                    PageBase.DropdownBinding(ref ddlType, dt, colArray);
                }
            }
            catch (Exception ex)
            {
                uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
             try
             {
                 //ddlUserType.Items.Clear();
                 //BindChannelType();
                 pnlGrid.Visible = false;                
             }
             catch (Exception ex)
             {
                 uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
             }
        }
        /* #CC01 Add End */

    
        protected void ddlTargetSaveUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                txttargetName.Text = string.Empty;
                if (ddlTargetSaveUpdate.SelectedValue == "0")
                {  
                    txttargetName.Visible = true;
                    txttargetName.Text = string.Empty;
                    ddlTarget.Visible = false;
                    rqtargetName.ControlToValidate = "txttargetName";

                    ddlTargetType.Enabled = true;
                    ddlTargetType.SelectedValue = "0";
                    ddlTargetCategory.Enabled = true;
                    ddlTargetCategory.SelectedValue = "0";
                    //ddlPeriod.Enabled = true;
                    //ddlPeriod.SelectedValue = "0";
                    ddlType.Enabled = true;
                    ddlType.SelectedValue = "0";
                    ddlType_SelectedIndexChanged(null, null);
                    //ddlUserType.Enabled = true;
                    //ddlUserType.SelectedValue = "0";
                    ddlTargetBased.Enabled = true;
                    ddlTargetBased.SelectedValue = "0";
                }
                else  {
                    txttargetName.Visible = false;
                    ddlTarget.Visible = true;
                    rqtargetName.ControlToValidate = "ddlTarget";
                    rqtargetName.InitialValue = "0";
                    BindTargetNames();
                }
                pnlGrid.Visible = false;
             }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);
                uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());
            }

        }


    public void BindTargetNames()
        {
            try
            {
                ddlTarget.Items.Clear();
                using(TargetData objTarget= new TargetData())
                {
                    objTarget.UserId = PageBase.UserId;
                    DataSet ds =  objTarget.GetTargetName();
                    String[] colArray = { "TargetName", "TargetName" };
                    PageBase.DropdownBinding(ref ddlTarget, ds.Tables[0], colArray);
                }
            }
            catch (Exception ex)
            {
                uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());
            }
        }
   
    protected void ddlTarget_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlTarget.SelectedValue != "0")
            {
                using (TargetData objTarget = new TargetData())
                {
                    objTarget.UserId = PageBase.UserId;
                    objTarget.TargetName = Convert.ToString(ddlTarget.SelectedValue);
                    DataSet ds = objTarget.GetTargetName();
                    ddlTargetType.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["TargetType"]);
                    ddlTargetType.Enabled = false;
                    ddlTargetCategory.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["TargetCategory"]);
                    ddlTargetCategory.Enabled = false;
                    //ddlPeriod.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["FinancialCalenderID"]);
                    //ddlPeriod.Enabled = false;

                    //ddlType.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["TargetUserType"]) == "3" && Convert.ToString(ds.Tables[0].Rows[0]["TargetUserTypeID"]) == "101" ? "101" : Convert.ToString(ds.Tables[0].Rows[0]["TargetUserType"]);
                    //ddlType.Enabled = false;
                    //ddlType_SelectedIndexChanged(null, null);
                    //ddlUserType.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["TargetUserTypeID"]) == "101" ? "3" : Convert.ToString(ds.Tables[0].Rows[0]["TargetUserTypeID"]); ;
                    //ddlUserType.Enabled = false;
                    ddlType.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["TargetUserTypeID"]) == "101" ? "3" : Convert.ToString(ds.Tables[0].Rows[0]["TargetUserTypeID"]); ;
                    ddlType.Enabled = false;
                    ddlTargetBased.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["TargetBaseOn"]);
                    ddlTargetBased.Enabled = false;
                    txttargetName.Text = Convert.ToString(ddlTarget.SelectedValue);
                }
            }
            else
            {
                ddlTargetType.Enabled = true;
                ddlTargetType.SelectedValue = "0";
                ddlTargetCategory.Enabled = true;
                ddlTargetCategory.SelectedValue = "0";
                //ddlPeriod.Enabled = true;
                //ddlPeriod.SelectedValue = "0";
                ddlType.Enabled = true;
                ddlType.SelectedValue = "0";
                ddlType_SelectedIndexChanged(null, null);
                //ddlUserType.Enabled = true;
                //ddlUserType.SelectedValue = "0";
                ddlTargetBased.Enabled = true;
                ddlTargetBased.SelectedValue = "0";

                }

        }
        catch (Exception ex)
        {
            uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());
        }

       
    } 
    void ClearFields()
        {
            try
            {
                ddlTargetSaveUpdate.SelectedValue = "0";
                ddlTargetType.Enabled = true;
                ddlTargetType.SelectedValue = "0";
                ddlTargetCategory.Enabled = true;
                ddlTargetCategory.SelectedValue = "0";
                //ddlPeriod.Enabled = true;
                //ddlPeriod.SelectedValue = "0";
                ddlType.Enabled = true;
                ddlType.SelectedValue = "0";
                ddlType_SelectedIndexChanged(null, null);
                //ddlUserType.Enabled = true;
                //ddlUserType.SelectedValue = "0";
                ddlTargetBased.Enabled = true;
                ddlTargetBased.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());
            }
       
        }
   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("UploadTarget.aspx",true);
        }
        catch (Exception ex)
        {

        }
    } /* #CC02 Add End */
    /* #CC09 Add Start */
    protected void lnkProductCategoryWise_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet DsSku = new DataSet();

            using (TargetData ObjRTarget = new TargetData())
            {

                ObjRTarget.TemplateType = EnumData.eTargetTemplateType.eProductCategoryWise;


                DsSku.Merge(ObjRTarget.GetTargetTemplate());
            };
            if (DsSku.Tables[0].Rows.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "Product Category Wise Target Template";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(DsSku, FilenameToexport, EnumData.eTemplateCount.eTarget);
            }
        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), GlobalErrorDisplay());

        }
    }/* #CC09 Add End */
    /* #CC10 Added */
    protected void lnkWODWiseTemplate_Click(object sender, EventArgs e)
    {

        try
        {
            DataSet DsSku = new DataSet();

            using (TargetData ObjRTarget = new TargetData())
            {

                ObjRTarget.TemplateType = EnumData.eTargetTemplateType.eWOD;


                DsSku.Merge(ObjRTarget.GetTargetTemplate());
            };
            if (DsSku.Tables[0].Rows.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "WOD Target Template";
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
        /* #CC10 Added End*/
}
