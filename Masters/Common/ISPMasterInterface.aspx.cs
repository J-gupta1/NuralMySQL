using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;


/*
 ============================================================================================================================================                                    
Change Log:                                    
dd-MMM-yy, Name, #CCx - Description                                    
--------------------------------------------------------------------------------------------------------------------------------------------                                    
20-Dec-13,Pankaj Mittal,#cc01 - Provide unlock ISP facility
05-Feb-14,Pankaj Mittal,#cc02 - Provide user name and password in export to excel
07-Oct-2016, Sumit Maurya, #CC03, Loading issue resolved for click event of Submit and Cancel button.
 * 28-Apr-2018, Sumit Maurya, #CC04, UserID provided (done for Zedsalesv5).
 * 30-Oct-2018, Sumit Maurya, #CC05, Design changed as per requirement (Done for mototrola).
 * 02-Nov-2018, Sumit Maurya, #CC06, Reatiler name validation added, Mobile number validation modified acording to config. search criteria / Parameters added (Done for Motorola) 
 * 13-Oct-2018, Sumit Maurya, #CC07, From date is removed as it is not required (Done for Inovu)
 * 27-Dec-2018, Sumit Maurya, #CC08, StoreCode Added (Done for mototrola).
 * 06-Feb-2019, Sumit Maurya, #CC09, Issues resolved related to search, update (Done for mototrola).
*/
public partial class Masters_Common_ISPMasterInterface : PageBase
{
    Int16 CompanyID = 0;
    string RetailerISPMappingID, intISPID;
    int intSelectedValue = 0;

    protected string strAsset = PageBase.strAssets;

    protected void detailGrid_DataSelect(object sender, EventArgs e)
    {
        try
        {
            intSelectedValue = Convert.ToInt32((sender as ASPxGridView).GetMasterRowKeyValue());
            DataTable DtBeautyAdvisor = new DataTable();
            using (BeautyAdvisorData ObjBA = new BeautyAdvisorData())
            {


                int RetailerID = 0;

                ObjBA.ISPName = txtBeautyAdvisorName.Text.Trim();
                ObjBA.Mobile = txtMobileNo.Text.Trim();
                ObjBA.RetailerID = RetailerID;
                ObjBA.ISPID = intSelectedValue;
                ObjBA.PageIndex = 1;
                ObjBA.PageSize = 100;
                DtBeautyAdvisor = ObjBA.ISP_SelectMapping();
                // grdvList.DetailRows.CollapseAllRows();
            };
            ASPxGridView objDetail = (ASPxGridView)(sender as ASPxGridView);
            if (DtBeautyAdvisor.Rows.Count > 0)
            {
                Session["Detail"] = DtBeautyAdvisor;

                ViewState["uniqueValue"] = intSelectedValue.ToString();


                objDetail.DataSource = Session["Detail"];
                // objDetail.Visible = true;
            }
            else
            {
                //  objDetail.Visible = false;
                if (ViewState["Search"] != null)
                {
                    ucMessage1.ShowInfo(Resources.GlobalMessages.NoRecord);
                }

            }
            ////  updGrid.Update();
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }

    }

    protected void grdvList_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
    {
        try
        {
            if (hdnIndex.Value == "")
                BindISP(1);
            else
                BindISP(Convert.ToInt32(hdnIndex.Value));
            //PKD

            ClearForm();
            //updGrid.Update();

            if (!e.Expanded)
                return;



            Session["Visible"] = e.VisibleIndex;
            //object keys = (sender as ASPxGridView).GetRowValues(e.VisibleIndex, new string[] { "SymptomMasterID" });
            //using (clsSymptomMaster objSymptomMasterMaster = new clsSymptomMaster())
            //{
            //    objSymptomMasterMaster.SymptomMasterId = Convert.ToInt16((sender as ASPxGridView).GetMasterRowKeyValue());
            //    dt_Modess = objSymptomMasterMaster.SelectAllProductCategoriesByDefectCodeId();
            //}
            grdvList.DetailRows.CollapseAllRows();
            grdvList.DetailRows.ExpandRow(e.VisibleIndex);
            grdvList.DetailRows.IsVisible(e.VisibleIndex);
            ASPxGridView objDetail = (ASPxGridView)(sender as ASPxGridView).FindDetailRowTemplateControl(e.VisibleIndex, "detailGrid");
            objDetail.DataSource = Session["Detail"];
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);

        }

    }
    protected void grdvList_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
    {

        try
        {
            if (e.CommandArgs.CommandName == "edit")
            {
                Int16 ISPID = Int16.Parse(Convert.ToString(e.CommandArgs.CommandArgument));
                hdnISPID.Value = ISPID.ToString();
                DataTable DtISP = new DataTable();

                using (BeautyAdvisorData objBA = new BeautyAdvisorData())
                {
                    objBA.ISPID = ISPID;
                    objBA.CompanyID = CompanyID;
                    DtISP = objBA.GetISPInformation(ISPID);
                };
                if (DtISP.Rows.Count > 0)
                {
                    UserPanel(false);

                    txtBeautyAdvisorName.Text = Convert.ToString(DtISP.Rows[0]["ISPName"]);
                    txtBeautyAdvisorCode.Text = Convert.ToString(DtISP.Rows[0]["ISPCode"]);
                    txtStoreCode.Text = Convert.ToString(DtISP.Rows[0]["StoreCode"]); /* #CC08 Added */

                    txtMobileNo.Text = Convert.ToString(DtISP.Rows[0]["Mobile"]);
                    string MappedRetaileruniqueName = Convert.ToString(DtISP.Rows[0]["RetailerName"]) + "[" + Convert.ToString(DtISP.Rows[0]["RetailerCode"]) + "]";
                   /* ucDatePickerFromDate.TextBoxDate.Text = Convert.ToString(DtISP.Rows[0]["ActivationDate"]);  #CC07 Commented */
                    txtemail.Text = Convert.ToString(DtISP.Rows[0]["Email"]); /* #CC05 Added */
                    /* fromDateHead.Visible = false;
                    fromDateField.Visible = false; #CC07 Commented */
                    btnSubmit.Text = "Update";



                    hdnRetailerID.Value = DtISP.Rows[0]["RetailerID"].ToString();
                    txtRetailerName.Text = DtISP.Rows[0]["RetailerName"].ToString();
                    btnSearchRetailer.Enabled = false;
                    txtBeautyAdvisorCode.Enabled = false;

                    //  hdnIndex.Value = ucPagingControl1.CurrentPage.ToString();
                    //    UCPagingControl1_SetControlRefresh();
                    ////    updGrid.Update();

                    // FillUnMappedRetailer();
                    //hdn
                    //ddlSSSName.Items.Add(new ListItem(MappedRetaileruniqueName, Convert.ToString(DtISP.Rows[0]["RetailerID"])));
                    //ddlSSSName.SelectedItem.Selected = false;
                    //ddlSSSName.Items.FindByValue(Convert.ToString(DtISP.Rows[0]["RetailerID"])).Selected = true;
                }
            }
            if (e.CommandArgs.CommandName == "delMapping")
            {


                Int16 id = Int16.Parse(Convert.ToString(e.CommandArgs.CommandArgument));
                using (DataAccess.BeautyAdvisorData obj = new DataAccess.BeautyAdvisorData())
                {
                    byte b = 1;
                    DateTime dtEnd = DateTime.Now.Date;

                    obj.Mode = b;
                    obj.RetailerISPMappingID = Convert.ToInt32(Request.QueryString["retIspid"].ToString());
                    obj.EndDate = dtEnd;
                    /* #CC07 Add Start */
                    obj.Userid = BussinessLogic.PageBase.UserId;
                    int result = obj.deleteISPMapingOrRetainingV2(); /* #CC07 Add End */
                    /* int result = obj.deleteISPMapingOrRetaining(); #CC07  Commented  */
                    if (result == 0)
                    {
                        ucMessage1.ShowSuccess(Resources.GlobalMessages.Delete);
                          updGrid.Update();

                    }
                    else
                    {
                        ucMessage1.ShowError(obj.Error);
                         updGrid.Update();
                    }

                }



            }
            /*#cc01 unlock argument added : Code start*/
            if (e.CommandArgs.CommandName == "Unlock")
            {
                Int16 ISPID = Int16.Parse(Convert.ToString(e.CommandArgs.CommandArgument));
                hdnISPID.Value = ISPID.ToString();
                DataTable DtISP = new DataTable();
                int intStatus = 0;
                using (BeautyAdvisorData objBA = new BeautyAdvisorData())
                {
                    objBA.ISPID = ISPID;
                    intStatus = objBA.UnlockISP();
                };
                if (intStatus > 0)
                {
                    ucMessage1.ShowSuccess(Resources.Messages.LockedOut);
                }
                else
                {
                    ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                }
                //  hdnIndex.Value = ucPagingControl1.CurrentPage.ToString();
                //    UCPagingControl1_SetControlRefresh();
                ////    updGrid.Update();

                // FillUnMappedRetailer();
                //hdn
                //ddlSSSName.Items.Add(new ListItem(MappedRetaileruniqueName, Convert.ToString(DtISP.Rows[0]["RetailerID"])));
                //ddlSSSName.SelectedItem.Selected = false;
                //ddlSSSName.Items.FindByValue(Convert.ToString(DtISP.Rows[0]["RetailerID"])).Selected = true;

            }
            /*#cc01 unlock argument added : Code END*/

            ////using (clsSymptomMaster objSymptomMaster = new clsSymptomMaster())
            ////{
            ////    if (e.CommandArgs.CommandName == "editSymptomCode")
            ////    {
            ////        txtSymptomCode.Enabled = false;
            ////        Int16 id = Int16.Parse(Convert.ToString(e.CommandArgs.CommandArgument));
            ////        ViewState["SymptomMasterId"] = id;
            ////        ListItem item = new ListItem();
            ////        objSymptomMaster.SymptomMasterId = id;
            ////        objSymptomMaster.CreatedBy = Convert.ToInt16(Pagebase.UserId);
            ////        DataTable dt = objSymptomMaster.SelectById();
            ////        btnSave.Text = "Update";
            ////        //((Button)btnSearchCancel.FindControl("btnCancel")).Visible = true;
            ////        txtSymptomCode.Text = Convert.ToString(dt.Rows[0]["SymptomCode"]);
            ////        txtSymptomDescription.Text = Convert.ToString(dt.Rows[0]["SymptomDescription"]);
            ////        ddlJobSheetType.SelectedValue = Convert.ToString(dt.Rows[0]["JobSheetTypeMasterID"]);
            ////        ddlJobSheetType.Enabled = false;
            ////        //chkProductCategory.Items.Clear();
            ////        //BindDataForCategory(0);
            ////        DataTable dtProductMapping = objSymptomMaster.SelectMappedProductsWithSymptom();
            ////        if (dtProductMapping.Rows.Count > 0)
            ////        {
            ////            foreach (ListItem lst in chkProductCategory.Items)
            ////            {
            ////                string productvalue = Convert.ToString(lst.Value);
            ////                for (int i = 0; i < dtProductMapping.Rows.Count; i++)
            ////                {
            ////                    Int64 intProductID = Int64.Parse(Convert.ToString(dtProductMapping.Rows[i]["ProductCategoryID"]));
            ////                    string abc = Convert.ToString(intProductID);
            ////                    if (abc == productvalue)
            ////                    {
            ////                        lst.Selected = true;
            ////                        lst.Enabled = false;

            ////                    }

            ////                }
            ////            }
            ////        }
            ////        //if (ViewState["PageIndex"] == null)
            ////        //    BindListSearch(1);
            ////        //else
            ////        //    BindListSearch(Convert.ToInt32(ViewState["PageIndex"]));
            ////        //PKD
            ////        ucMessage1.Visible = false;
            ////        grdvList.DetailRows.CollapseAllRows();
            ////        updpnlSaveData.Update();
            ////    }
            ////    else if (e.CommandArgs.CommandName == "activeSymptomCode")
            ////    {
            ////        Int16 id = Int16.Parse(Convert.ToString(e.CommandArgs.CommandArgument));
            ////        objSymptomMaster.SymptomMasterId = id;
            ////        DataTable dt = objSymptomMaster.SelectById();
            ////        objSymptomMaster.CreatedBy = Convert.ToInt16(Pagebase.UserId);
            ////        //objSymptomMaster.ProductID = Int64.Parse(dt.Rows[0]["ProductID"].ToString());
            ////        bool action = Convert.ToBoolean(dt.Rows[0]["Active"]);
            ////        if (action)
            ////            objSymptomMaster.Active = false;
            ////        else
            ////            objSymptomMaster.Active = true;
            ////        int result = objSymptomMaster.UpdateActive();
            ////        if (result == 0)
            ////        {
            ////            if (objSymptomMaster.Active)
            ////                ucMessage1.ShowSuccess(SuccessMessages.ToggleSuccess);
            ////            else
            ////                ucMessage1.ShowSuccess(SuccessMessages.ToggleSuccess);
            ////        }
            ////        else if (result == 1)
            ////            ucMessage1.ShowError(objSymptomMaster.Error.ToString());
            ////        BindListSearch(1);
            ////    }
            ////    updpnlGrid1.Update();
            ////    updpnlSaveData.Update();
            ////}
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
            ////updGrid.Update();
            //  ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
        }

    }
    protected void grdvList_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        try
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data) return;
            ASPxGridView grid = (ASPxGridView)sender;
            object obj = grid.GetRow(e.VisibleIndex);


            Button btnExitISP = grdvList.FindRowCellTemplateControl(e.VisibleIndex,
            grdvList.Columns["Delete"] as GridViewDataColumn, "btnISPExit") as Button;


            Button btnSwitchToRetailer = grdvList.FindRowCellTemplateControl(e.VisibleIndex,
            grdvList.Columns["Delete"] as GridViewDataColumn, "btnSwitchToRetailer") as Button;


            Literal lblmapToRetailerDisp = grdvList.FindRowCellTemplateControl(e.VisibleIndex,
            grdvList.Columns["Delete"] as GridViewDataColumn, "lblMapToRetailerDisplay") as Literal;

            btnExitISP.Visible = lblmapToRetailerDisp.Text == "0";

            btnSwitchToRetailer.Visible = btnSwitchToRetailer.CommandArgument == "0";


            string ISPID = btnExitISP.CommandArgument;


            btnExitISP.Attributes.Add("onclick", "return popupISP('" + ISPID + "','0','Map to Retailer','1');"); // Mode: 1 for Map to


            btnSwitchToRetailer.Attributes.Add("onclick", "return popupISP('" + ISPID + "','0','Switch to Retailer','2');"); // Mode: 2 for switch

            // Label lblPassword = (Label)grid.FindControl("lblPassword");

            Label lblPassword = grdvList.FindRowCellTemplateControl(e.VisibleIndex,
            grdvList.Columns["Password"] as GridViewDataColumn, "lblPassword") as Label;

            if (lblPassword.Text != "")
            {
                LinkButton hlPassword = default(LinkButton);
                //hlPassword = (LinkButton)grid.FindControl("hlPassword");
                hlPassword = grdvList.FindRowCellTemplateControl(e.VisibleIndex,
            grdvList.Columns["Password"] as GridViewDataColumn, "hlPassword") as LinkButton;
                hlPassword.Visible = true;
                string strPassword = null;

                //Label lblPasswordSalt = (Label)grid.FindControl("lblPasswordSalt");
                Label lblPasswordSalt = grdvList.FindRowCellTemplateControl(e.VisibleIndex,
            grdvList.Columns["Password"] as GridViewDataColumn, "lblPasswordSalt") as Label;

                strPassword = fncChangePwd(lblPassword.Text, lblPasswordSalt.Text);
                hlPassword.Attributes.Add("onclick", "javascript:alert('Password is : " + strPassword + "');{return false;}");

            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            //updpnlSaveData.Update();
        }

    }

    DevExpress.Web.ASPxGridView.GridViewCustomButtonVisibility
getVisibility(object ActivationDate)
    {

        return DevExpress.Web.ASPxGridView.GridViewCustomButtonVisibility.AllDataRows;


    }


    protected void detailGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        try
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data) return;
            ASPxGridView grid = (ASPxGridView)sender;
            object obj = grid.GetRow(e.VisibleIndex);
            ASPxButton btnActive = (ASPxButton)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "btnActivePC");


            ImageButton btndeleteMapping = (ImageButton)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "btnDeleteMapping");
            Button btnExitISP = (Button)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "btnExitISP");


            Literal litDeactivationDate = (Literal)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "lblDeleteMappingDisplayCount");
            Literal litExitISPDisplay = (Literal)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "litExitISPDisplay");

            Literal litPreRetailerISPMappingID = (Literal)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "litPreRetailerISPMappingID");


            if (litExitISPDisplay.Text == "")
            {
                if (btnExitISP != null)
                    btnExitISP.Visible = true;
            }
            else
            {
                if (btnExitISP != null)
                    btnExitISP.Visible = false;
            }


            if (Convert.ToDateTime(litDeactivationDate.Text) > DateTime.Now)
            {
                if (litPreRetailerISPMappingID.Text != "")
                {
                    if (btndeleteMapping != null)
                        btndeleteMapping.Visible = true;
                }
                else
                {
                    if (btndeleteMapping != null)
                        btndeleteMapping.Visible = false;
                }
            }
            else
            {
                if (btndeleteMapping != null)
                    btndeleteMapping.Visible = false;
            }


            if (btndeleteMapping != null)
                RetailerISPMappingID = btndeleteMapping.CommandArgument;
            if (btnExitISP != null)
                intISPID = btnExitISP.CommandArgument;

            //if (litPreRetailerISPMappingID.Text != "")
            //{
            //    btndeleteMapping.Attributes.Add("onclick", "return popupISP('0','" + RetailerISPMappingID + "','Delete Mapping','3');");
            //}
            if (btnExitISP != null)
                btnExitISP.Attributes.Add("onclick", "return popupISP('0','" + intISPID + "','Exiting Mapping','4');");
            if (btndeleteMapping != null)
                btndeleteMapping.ImageUrl = "~/" + strAssets + "/CSS/Images/delete.png";

            //if (Convert.ToInt16(((System.Data.DataRowView)(obj)).Row.ItemArray[3]) == 0)
            //{
            //    btnActive.ImageUrl = "~/" + strAssets + "/CSS/Images/decative.png";
            //    btnActive.ToolTip = "Inactive";
            //}
            //else
            //{
            //    btnActive.ImageUrl = "~/" + strAssets + "/CSS/Images/active.png";
            //    btnActive.ToolTip = "Active";
            //}




        }
        catch (Exception ex)
        {
            //ucMessage1.ShowAppError(ex);
            // updpnlSaveData.Update();
        }
    }


    public string fncChangePwd(string vPassword, string vPasswordSalt)
    {
        string vMailPassword = string.Empty;
        try
        {
            using (Authenticates objAuth = new Authenticates())
            {
                vMailPassword = objAuth.DecryptPassword(vPassword, vPasswordSalt);
            };
        }
        catch (Exception ex)
        {


            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        return vMailPassword;
    }

    protected void detailGrid_BeforePerformDataSelect(object sender, EventArgs e)
    {
        try
        {
            //Session["ID"] = (sender as DevExpress.Web.ASPxGridView.ASPxGridView).GetMasterRowKeyValue();
            //using (clsSymptomMaster obj = new clsSymptomMaster())
            //{
            //    obj.UniqueID = Convert.ToInt16(Session["ID"]);
            //    obj.ModifyBy = Convert.ToInt16(Pagebase.UserId);
            //    bool chkActive = obj.ToggleActivation();
            //    if (chkActive)
            //    {
            //        ucMessage1.ShowSuccess(SuccessMessages.ToggleSuccess);
            //    }
            //    ASPxGridView objDetail = new ASPxGridView();
            //    //objDetail.FindDetailRowTemplateControl(Convert.ToInt32(ViewState["VisibleValue"]), "detailGrid");
            //    objDetail.FindDetailRowTemplateControl(0, "detailGrid");
            //    using (clsSymptomMaster objSymptomMasterMaster = new clsSymptomMaster())
            //    {
            //        objSymptomMasterMaster.SymptomMasterId = Convert.ToInt16(ViewState["UniqueID"]);
            //        objDetail.DataSource = objSymptomMasterMaster.SelectAllProductCategoriesByDefectCodeId();
            //        objDetail.DataBind();
            //    }
            //    updpnlGrid1.Update();
            //    updpnlSaveData.Update();

            //}

        }
        catch (Exception ex)
        {

        }
    }

    protected void ASPxGridView1_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        ASPxGridView detailGridView = sender as ASPxGridView;

        try
        {
            //  

            string s = ViewState["uniqueValue"].ToString();

            RebindDetailGrid(Convert.ToInt16(ViewState["uniqueValue"].ToString()), detailGridView);

            using (DataAccess.BeautyAdvisorData obj = new DataAccess.BeautyAdvisorData())
            {
                byte b = 1;
                DateTime dtEnd = DateTime.Now.Date;

                obj.Mode = b;
                obj.RetailerISPMappingID = Convert.ToInt32(detailGridView.GetRowValues(e.VisibleIndex, new string[] { "RetailerISPMappingID" }));
                obj.EndDate = dtEnd;
                /* #CC07 Add Start */
                obj.Userid = BussinessLogic.PageBase.UserId;
                int result = obj.deleteISPMapingOrRetainingV2(); /* #CC07 Add End */
                /* int result = obj.deleteISPMapingOrRetaining(); #CC07  Commented  */
                
                if (result == 0)
                {
                    ucMessage1.ShowSuccess(Resources.GlobalMessages.Delete);
                    ////  updGrid.Update();
                    RebindDetailGrid(Convert.ToInt16(ViewState["uniqueValue"]), detailGridView);
                    grdvList.DetailRows.CollapseAllRows();


                }
                else
                {
                    ucMessage1.ShowError(obj.Error);
                    ////    updGrid.Update();
                }

            }
            BindISP(Convert.ToInt32(hdnIndex.Value));
            //// updGrid.Update();




            ////using (clsSymptomMaster obj = new clsSymptomMaster())
            ////{

            ////    obj.UniqueID = Convert.ToInt32(detailGridView.GetRowValues(e.VisibleIndex, new string[] { "RetailerISPMappingID" }));
            ////    obj.ModifyBy = Convert.ToInt16(Pagebase.UserId);
            ////    bool chkActive = obj.ToggleActivation();
            ////    if (chkActive)
            ////    {
            ////        // ucMessage1.ShowSuccess(SuccessMessages.ToggleSuccess);
            ////    }


            ////    RebindDetailGrid(Convert.ToInt16(ViewState["UniqueID"]), detailGridView);
            ////    //if (ViewState["PageIndex"] == null)
            ////    //    BindListSearch(1);
            ////    //else
            ////    //    BindListSearch(Convert.ToInt32(ViewState["PageIndex"]));
            ////    updpnlSaveData.Update();
            ////}
        }
        catch (Exception ex)
        {
            // ucMessage1.ShowAppError(ex);
        }

    }
    void RebindDetailGrid(Int16 ID, ASPxGridView detail)
    {
        DataTable DtBeautyAdvisor = new DataTable();
        using (BeautyAdvisorData ObjBA = new BeautyAdvisorData())
        {
            ObjBA.ISPName = txtBeautyAdvisorName.Text.Trim();
            ObjBA.Mobile = txtMobileNo.Text.Trim();
            ObjBA.RetailerID = 0;
            ObjBA.ISPID = Convert.ToInt16(ViewState["uniqueValue"].ToString()); ;
            DtBeautyAdvisor = ObjBA.ISP_Select();
            detail.DataSource = DtBeautyAdvisor;
            detail.DataBind();
        }

    }

    protected void detailGrid_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
    {
        try
        {


            // "~/" + strAssets + "/CSS/Images/zedsaleslogo.gif";
            ASPxGridView grid = (ASPxGridView)sender;
            object objAct = grid.GetRowValues(e.VisibleIndex, "ActivationDate");
            object objPre = grid.GetRowValues(e.VisibleIndex, "PreviousRetailerISPMappingID");


            if (objAct == null && objPre == null)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            }
            else
            {
                if (Convert.ToDateTime(objAct.ToString()) > DateTime.Now && (objAct != null || string.Empty != objAct.ToString()))
                {
                    if (objPre != null && objPre.ToString() != "")
                    {
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    }
                    else
                    {
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                    }
                }
                else
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                }
            }

            //// updGrid.Update();


            e.Image.Url = String.Format("~/{0}/CSS/Images/delete.png", strAssets);
            e.Image.ToolTip = "Delete mapping";

        }
        catch (Exception ex)
        {

        }


    }
    protected void Page_PreLoad(object sender, EventArgs e)
    {

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //    Response.Write(strAsset);

            /* #CC07 Comment Start
             ucDatePickerFromDate.MinRangeValue = DateTime.Now;
            ucDatePickerFromDate.MaxRangeValue = DateTime.MaxValue;
            #CC07 Comment End */
            txtRetailerName.Text = hdnRetailerName.Value;

            btnSearchRetailer.Attributes.Add("onclick", "return popupRetailer();");
            ucMessage1.ShowControl = false;
            //FillGrid();
            // CompanyID = PageBase.CompanyID;


            if (!IsPostBack)
            {
                // RenderIncludes("JS", BussinessLogic.PageBase.siteURL + "Assets/Jscript/dhtmlwindow.js");

                ViewState["CurrentPage"] = "1";
                UserPanel(true);
                // BindISP(1);
                FillMappedRetailer();
                FillUnMappedRetailer();
                // 
                ValidationsControls(); /* #CC06 Added */

            }
        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
        }
    }

    void RenderIncludes(string IncludeType, params string[] str)
    {
        switch (IncludeType)
        {
            case "JS":
                foreach (string item in str)
                {
                    var js = new HtmlGenericControl("script");
                    js.Attributes["type"] = "text/javascript";
                    js.Attributes["src"] = item;
                    js.Attributes["id"] = "abc";
                    Page.Header.Controls.Add(js);
                }
                break;
            case "CSS":
                foreach (string item in str)
                {
                    HtmlLink myHtmlLink = new HtmlLink();
                    myHtmlLink.Href = item;
                    myHtmlLink.Attributes.Add("rel", "stylesheet");
                    myHtmlLink.Attributes.Add("type", "text/css");
                    Page.Header.Controls.Add(myHtmlLink);
                }
                break;
        }
    }

    void UserPanel(bool visible)
    {
        try
        {
            if (Convert.ToString(Session["ISDLogin"]) == "0")
            {
                pnlGrid.Visible = false;
                reqpassword.Enabled = false;
                reqUserName.Enabled = false;
                reqpassword.ValidationGroup = "";
                reqUserName.ValidationGroup = "";
                ulEmail.Visible = false;
                rqEmail.Enabled = false;
                emailMandateSign.Visible = false;
               
            }
            else
            {
                pnlGrid.Visible = visible;
                reqpassword.Enabled = visible;
                reqUserName.Enabled = visible;
                reqpassword.ValidationGroup = "AddBeautyAdvisor";
                reqUserName.ValidationGroup = "AddBeautyAdvisor";
                ulEmail.Visible = true;
                rqEmail.Enabled = true;
                emailMandateSign.Visible = true;


            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {


        try
        {
            if (hdnRetailerID.Value != "" && txtBeautyAdvisorCode.Text != "" && txtBeautyAdvisorName.Text != "" /* && txtMobileNo.Text != ""  #CC06 Commented */) /* #CC06 txtMobileNo.Text Commented */
            {
                Int32 Result = 0;


                string errorMsg = string.Empty;
                if (ValidateControl(ref errorMsg) == true)
                {
                    ucMessage1.ShowWarning(errorMsg);

                    return;
                }

                using (BeautyAdvisorData ObjBA = new BeautyAdvisorData())
                {

                    if (hdnISPID.Value != "")
                    {
                        ObjBA.ISPID = Convert.ToInt16(hdnISPID.Value);
                    }
                    else
                    {
                        ObjBA.ISPID = 0;
                    }


                    string StrPSalt = "";
                    if (Convert.ToInt16(Session["ISDLogin"]) == 1)
                    {
                        using (Authenticates ObjAuth = new Authenticates())
                        {
                            if (txtpassword.Text != "")
                            {
                                StrPSalt = ObjAuth.GenerateSalt(txtpassword.Text.Trim().Length);
                                ObjBA.Password = ObjAuth.EncryptPassword(txtpassword.Text.Trim(), StrPSalt);
                            }
                        };
                        ObjBA.PasswordSalt = StrPSalt;
                    }
                    ObjBA.UserName = txtUserName.Text.Trim();
                    ObjBA.PasswordExpiryDays = Convert.ToInt16(Application["ExpiryDays"].ToString());
                    ObjBA.CreateLoginOrNot = Convert.ToInt16(Session["ISDLogin"].ToString());
                    //ObjBA.Email = string.Empty;
                    ObjBA.Email = txtemail.Text.Trim();
                    ObjBA.CompanyID = CompanyID;
                    ObjBA.ISPName = txtBeautyAdvisorName.Text.Trim();
                    ObjBA.ISPCode = txtBeautyAdvisorCode.Text.Trim();
                    ObjBA.Mobile = txtMobileNo.Text.Trim();
                    ObjBA.RetailerID = Convert.ToInt32(hdnRetailerID.Value);
                  /*   ObjBA.FromDate = Convert.ToDateTime(ucDatePickerFromDate.Date);  #CC07 Commented */ 
                    ObjBA.Userid = PageBase.UserId; /* #CC04 Added  */
                    ObjBA.StoreCode = txtStoreCode.Text.Trim(); /* #CC08 Added */
                    Result =  ObjBA.InsertUpdateISPinfo();
                    if (ObjBA.ErrorMessage != string.Empty)
                    {
                        ucMessage1.ShowInfo(ObjBA.ErrorMessage);
                        return;
                    }
                };

                if (Result > 0)
                {
                    UserPanel(true);
                    if (hdnISPID.Value == "")
                    {
                        ucMessage1.ShowSuccess(Resources.GlobalMessages.CreateSuccessfull);
                    }
                    else
                    {
                        ucMessage1.ShowSuccess(Resources.GlobalMessages.EditSuccessfull);
                        btnSubmit.Text = "Submit";
                        if (hdnIndex.Value == "")
                            BindISP(1);
                        else
                            BindISP(Convert.ToInt32(hdnIndex.Value));



                    }



                    rdobeautyAdvisorMode.SelectedValue = "2";
                    btnSearchRetailer.Enabled = true;
                    ucMessage1.ShowCloseButton = true;
                    PageBase.ResetPageControl(this.Controls);
                    btnSubmit.Text = "Submit";
                    hdnISPID.Value = "";
                    txtBeautyAdvisorCode.Enabled = true;



                }
                else
                {
                    ucMessage1.ShowError(Resources.GlobalMessages.ErrorMsgTryAfterSometime);
                }
            }
            else
            {
                ucMessage1.ShowInfo(Resources.GlobalMessages.MandatoryFieldValidation);
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    private bool ValidateControl(ref String ErrMessage)
    {
        bool check = false;

        if (ServerValidation.IsMobileNo(txtMobileNo.Text.Trim(), true) > 0 && Convert.ToInt16(HttpContext.Current.Session["ISPUNIQUEMOBILE"].ToString()) > 0) /* #CC06 Convert.ToInt16(HttpContext.Current.Session["ISPUNIQUEMOBILE"].ToString()) > 0 Added */
        {
            ErrMessage = Resources.GlobalMessages.MobileValidate;
            check = true;
        }

        return check;
    }

    private void BindISP(int index)
    {
        try
        {
            DataTable DtBeautyAdvisor = new DataTable();


            using (BeautyAdvisorData ObjBA = new BeautyAdvisorData())
            {


                int RetailerID = 0;


                ObjBA.ISPName = txtSearchBeautyAdvisorName.Text.Trim();
                ObjBA.Mobile = txtMobileNo.Text.Trim();
                ObjBA.RetailerID = RetailerID;
                ObjBA.ISPID = 0;
                ObjBA.PageIndex = index;
                ObjBA.PageSize = 10;
                ObjBA.ISPCode = txtISPCode.Text.Trim(); /* #CC06 Added */
                ObjBA.StoreCode = txtStoreCodeSearch.Text.Trim(); /* #CC08 Added */
                DtBeautyAdvisor = ObjBA.ISP_Select();

                if (DtBeautyAdvisor.Rows.Count > 0)
                {
                    // ViewState["DtExport"] = DtBeautyAdvisor;

                    grdvList.DataSource = DtBeautyAdvisor;
                    grdvList.DataBind();

                    ucPagingControl1.CurrentPage = index;
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize = 10;
                    ucPagingControl1.TotalRecords = ObjBA.TotalRecords;
                    ucPagingControl1.FillPageInfo();



                }

                else
                {
                    ucPagingControl1.Visible = false;
                    grdvList.DataSource = null;
                    grdvList.DataBind();
                    if (ViewState["Search"] != null)
                    {
                        ucMessage1.ShowInfo(Resources.GlobalMessages.NoRecord);
                    }

                }
            };
            ////    updGrid.Update();
            ////   updsearch.Update();
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);


        }

    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        hdnIndex.Value = ucPagingControl1.CurrentPage.ToString();
        ClearForm();
        // BindISP(ucPagingControl1.CurrentPage);
        //// updGrid.Update();

    }

    private void FillGrid()
    {
        try
        {
            DataTable DtBeautyAdvisor = new DataTable();


            using (BeautyAdvisorData ObjBA = new BeautyAdvisorData())
            {


                int RetailerID = 0;
                ObjBA.CompanyID = 0;

                DtBeautyAdvisor = ObjBA.GetISPInformation(RetailerID, txtSearchBeautyAdvisorName.Text.Trim());
            };
            if (DtBeautyAdvisor.Rows.Count > 0)
            {
                ViewState["DtExport"] = DtBeautyAdvisor;
                grdvList.DataSource = DtBeautyAdvisor;
                grdvList.DataBind();

            }
            else
            {
                if (ViewState["Search"] != null)
                {
                    ucMessage1.ShowInfo(Resources.GlobalMessages.NoRecord);
                }

            }
            //// updGrid.Update();
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);


        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {



        ucMessage1.ShowCloseButton = true;
        rdobeautyAdvisorMode.SelectedValue = "2";
        PageBase.ResetPageControl(this.Controls);
        btnSubmit.Text = "Submit";
        /*fromDateHead.Visible = true;
        fromDateField.Visible = true;
         #CC07 Commented */
        UserPanel(true);
        hdnISPID.Value = "";
        btnSearchRetailer.Enabled = true;
        txtBeautyAdvisorCode.Enabled = true;


    }
    protected void rdobeautyAdvisorMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdobeautyAdvisorMode.SelectedValue == "1")
        {
            Response.Redirect("ISPMasterUploadV2.aspx");
        }

    }
    private void FillUnMappedRetailer()
    {
        try
        {

            //DataTable dtRetailerUnMapped;
            //using (RetailerData ObjRetailer = new RetailerData())
            //{
            //    ObjRetailer.CompanyID = 0;
            //    dtRetailerUnMapped = ObjRetailer.GetRetailerForISP(false);

            //};

            //string[] strRetailer = new string[] { "RetailerID", "RetailerUniqueName" };
            //PageBase.DropdownBinding(ref ddlSSSName, dtRetailerUnMapped, strRetailer);

        }


        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);


        }

    }
    private void FillMappedRetailer()
    {
        try
        {

            //DataTable dtRetailerMapped;
            //using (RetailerData ObjRetailer = new RetailerData())
            //{
            //    ObjRetailer.CompanyID = 0;
            //    dtRetailerMapped = ObjRetailer.GetRetailerForISP(true);

            //};

            //string[] strRetailer = new string[] { "RetailerID", "RetailerUniqueName" };
            //PageBase.DropdownBinding(ref ddlSearchSSSName, dtRetailerMapped, strRetailer);
            //updsearch.Update();
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);


        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            ControlCollection cc = new ControlCollection(txtRetailerName);
            PageBase.ResetPageControl(cc);
            hdnIndex.Value = "1";

            // ViewState["Search"] = "Search";
            // ucMessage1.ShowCloseButton = true;
            rdobeautyAdvisorMode.SelectedValue = "2";
            ClearForm();
            btnSubmit.Text = "Submit";
          /*  fromDateHead.Visible = true;
            fromDateField.Visible = true; #CC07 Commented */

            hdnISPID.Value = "";
            BindISP(1);

            txtRetailerName.Text = "";
            grdvList.DetailRows.CollapseAllRows();
            ////  updsearch.Update();


            // ViewState["Search"] = null;
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);


        }

    }
    //protected void GridBeautyAdvisor_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    int Result = 0;
    //    try
    //    {
    //        if (e.CommandName == "Active")
    //        {
    //            Int32 ISPID = Convert.ToInt32(e.CommandArgument);
    //            if (ISPID > 0)
    //            {
    //                using (BeautyAdvisorData objBA = new BeautyAdvisorData())
    //                {

    //                    objBA.ISPID = ISPID;
    //                    Result = objBA.DeleteUpdateStatusISPInfo(1);

    //                };
    //                if (Result > 0)
    //                {
    //                    ucMessage1.ShowSuccess(Resources.GlobalMessages.StatusChanged);


    //                }
    //                else
    //                {
    //                    ucMessage1.ShowError(Resources.GlobalMessages.ErrorMsgTryAfterSometime);


    //                }
    //                FillGrid();
    //            }
    //        }
    //        //if (e.CommandName == "cmdDelete")
    //        //{
    //        //    lblMessage.Text = "";
    //        //    Int32 ISPID = Convert.ToInt32(e.CommandArgument);
    //        //    using (BeautyAdvisorData objBA = new BeautyAdvisorData())
    //        //    {
    //        //        objBA.ISPID = ISPID;
    //        //        Result = objBA.DeleteUpdateStatusISPInfo(2);

    //        //    };
    //        //    if (Result == 1)
    //        //    {

    //        //        lblMessage.Text = GetGlobalResourceObject("GlobalMessages", "Delete").ToString();
    //        //        FillGrid();
    //        //    }
    //        //    else
    //        //    {

    //        //        lblMessage.Text = GetGlobalResourceObject("GlobalMessages", "ErrorMsgTryAfterSometime").ToString();

    //        //    }
    //        //}


    //        if (e.CommandName == "cmdEdit")
    //        {


    //            btnSubmit.Text = "Update";
    //            Int32 ISPID = Convert.ToInt32(e.CommandArgument);
    //            DataTable DtISP;
    //            ViewState["ISPID"] = ISPID;
    //            using (BeautyAdvisorData objBA = new BeautyAdvisorData())
    //            {
    //                objBA.ISPID = ISPID;
    //                objBA.CompanyID = CompanyID;
    //                DtISP = objBA.GetISPInformation(ISPID);
    //            };
    //            if (DtISP.Rows.Count > 0)
    //            {

    //                txtBeautyAdvisorName.Text = Convert.ToString(DtISP.Rows[0]["ISPName"]);
    //                txtBeautyAdvisorCode.Text = Convert.ToString(DtISP.Rows[0]["ISPCode"]);
    //                txtMobileNo.Text = Convert.ToString(DtISP.Rows[0]["Mobile"]);
    //                string MappedRetaileruniqueName = Convert.ToString(DtISP.Rows[0]["RetailerName"]) + "[" + Convert.ToString(DtISP.Rows[0]["RetailerCode"]) + "]";
    //                FillUnMappedRetailer();
    //                ddlSSSName.Items.Add(new ListItem(MappedRetaileruniqueName, Convert.ToString(DtISP.Rows[0]["RetailerID"])));
    //                ddlSSSName.SelectedItem.Selected = false;
    //                ddlSSSName.Items.FindByValue(Convert.ToString(DtISP.Rows[0]["RetailerID"])).Selected = true;
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        PageBase.Errorhandling(ex);


    //    }


    //}
    protected void GridBeautyAdvisor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // try
        //{
        //    int CheckResult = 0;
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Int32 ISPID = Convert.ToInt32(GridBeautyAdvisor.DataKeys[e.Row.RowIndex].Value);
        //        using (BeautyAdvisorData objBA = new BeautyAdvisorData())
        //        {
        //            objBA.ISPID = ISPID;
        //            CheckResult = objBA.CheckISPMasterExistence();
        //        };
        //        GridViewRow GVR = e.Row;
        //        ImageButton btnDelete = (ImageButton)GVR.FindControl("ImgDelete");

        //        if (CheckResult > 0)
        //        {

        //            if (btnDelete != null)
        //            {
        //                btnDelete.Attributes.Add("Onclick", "javascript:alert('This ISP is linked to existing data.You can not delete it.');{return false;}");

        //            }

        //        }
        //        else
        //        {
        //            if (btnDelete != null)
        //            {
        //                btnDelete.Attributes.Add("OnClick", "if(!confirm('Are you sure want to delete?')){return false;}");
        //            }

        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    lblMessage.Text = ex.Message.ToString();
        //    PageBase.Errorhandling(ex);
        //}


    }


    DataTable DataToExport()
    {

        DataTable DtBeautyAdvisor = new DataTable();
        using (BeautyAdvisorData ObjBA = new BeautyAdvisorData())
        {
            /* #CC08 Add Start  */
            ObjBA.ISPCode = txtISPCode.Text.Trim();
            ObjBA.StoreCode = txtStoreCodeSearch.Text.Trim();
            /* #CC08 Add End  */
            ObjBA.ISPName = txtSearchBeautyAdvisorName.Text.Trim();
            DtBeautyAdvisor = ObjBA.ISPExport();
        }

        return DtBeautyAdvisor;
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = DataToExport();
            /*#cc02 added start*/
            string Password = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    using (Authenticates ObjAuth = new Authenticates())
                    {
                        Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                    };
                    dr["Password"] = Password;
                    Password = string.Empty;
                }
            }
            dt.Columns.Remove("PasswordSalt");
            dt.AcceptChanges();
            /*#cc02 added END*/
            if (dt.Rows.Count > 0)
            {
                try
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ISPDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    ViewState["DtExport"] = null;
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.Message);
                    PageBase.Errorhandling(ex);
                }
            }
            else
            {
                ucMessage1.ShowInfo(Resources.GlobalMessages.NoRecordexport);
            }




            //if (ViewState["DtExport"] != null)
            //{
            //    DataTable Dt = (DataTable)ViewState["DtExport"];
            //    string[] StrCol = new string[] { "RegionName", "CircleName", "AreaPositionName", "RetailerName", "ISPCode", "ISPName", "Mobile", "StatusName" };
            //    DataTable Dtexport;
            //    Dtexport = Dt.DefaultView.ToTable(true, StrCol);
            //    Dtexport.Columns["RegionName"].ColumnName = "Zone";
            //    Dtexport.Columns["CircleName"].ColumnName = "ASM";
            //    Dtexport.Columns["AreaPositionName"].ColumnName = "TIC";
            //    Dtexport.Columns["RetailerName"].ColumnName = "SSSName";
            //    Dtexport.Columns["ISPCode"].ColumnName = "BeautyAdvisorCode";
            //    Dtexport.Columns["ISPName"].ColumnName = "BeautyAdvisorName";
            //    Dtexport.Columns["StatusName"].ColumnName = "Status";
            //    DataSet Ds = new DataSet();
            //    Ds.Merge(Dtexport);
            //    String FilePath = Server.MapPath("../../");
            //    string FilenameToexport = "BeautyAdvisorList";
            //    PageBase.RootFilePath = FilePath;
            //    PageBase.ExportToExecl(Ds, FilenameToexport);
            //    ViewState["DtExport"] = null;
            //}
            //else
            //{

            //    ucMessage1.ShowInfo(Resources.GlobalMessages.NoRecordexport);

            //}
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);

        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {

        try
        {

            if (hdnIndex.Value == "")
                BindISP(1);
            else
                BindISP(Convert.ToInt32(hdnIndex.Value));


        }
        catch (Exception ex) { }
    }


    void ClearForm()
    {
        txtBeautyAdvisorName.Text = "";
        txtMobileNo.Text = "";
        txtBeautyAdvisorCode.Text = "";
        /* ucDatePickerFromDate.TextBoxDate.Text = ""; #CC07 Commented */
        btnSearchRetailer.Enabled = true;
        txtRetailerName.Text = string.Empty;
        hdnRetailerID.Value = string.Empty;
        UserPanel(true);

    }


    protected void btnSearchALL_Click(object sender, EventArgs e)
    {
        ucMessage1.ShowCloseButton = true;
        rdobeautyAdvisorMode.SelectedValue = "2";
        ClearForm();
        btnSubmit.Text = "Submit";
        /*fromDateHead.Visible = true;
        fromDateField.Visible = true; #CC07 Commented */
        /* #CC09 Add Start */
        txtISPCode.Text = "";
        hdnIndex.Value = "1"; /* #CC09 Add End */
        hdnISPID.Value = "";
        txtSearchBeautyAdvisorName.Text = "";
        BindISP(1);
        hdnISPID.Value = "";
        btnSearchRetailer.Enabled = true;
        txtRetailerName.Text = "";

        txtStoreCodeSearch.Text = "";
        txtStoreCode.Text = "";
        ////  updsearch.Update();
    }



    /* #CC06 Add Start */

    public void ValidationsControls()
    {
        try
        {
            if(Convert.ToInt16(HttpContext.Current.Session["ISPUNIQUEMOBILE"].ToString())>0)
            {
                req4.Enabled = true;
                FilteredTextBoxExtender1.EnableClientState = true;
                    spnMobileNumIcon.Visible=true;
            }
            else
            {
                req4.Enabled = false;
                FilteredTextBoxExtender1.EnableClientState = false;
                spnMobileNumIcon.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            PageBase.Errorhandling(ex);
        }
    }

    /* #CC06 Add End */
   
}


