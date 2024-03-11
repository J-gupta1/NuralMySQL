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
* Created On: 03-April-2020
 * Description: This is a Entity type Master Interface.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;

public partial class MastersManageConfiguration : PageBase
{
    static bool boolsearchflag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindAllDropDown();
                BindConfig();
            }
        }
        catch (Exception ex)
        {
           
            ucMessage1.ShowError(ex.Message);
            
        }
    }
    protected void BindAllDropDown()
    {
        DataTable dt = new DataTable();
        using (CommonMaster obj = new CommonMaster())
        {
            obj.CompanyID = PageBase.ClientId;
            dt = obj.GetStockAdjReason(1);
            ddlAPIStockAdjustmentResonID.DataSource = dt;
            ddlAPIStockAdjustmentResonID.DataTextField = "ReasonDesc";
            ddlAPIStockAdjustmentResonID.DataValueField = "StockAdjustmentReasonID";
            ddlAPIStockAdjustmentResonID.DataBind();
            ddlAPIStockAdjustmentResonID.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));
        }
        ddlSalesChannelApprovalLevels.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "SALESCHANNELAPPROVALVALUES");
        ddlSalesChannelApprovalLevels.DataTextField = "Description";
        ddlSalesChannelApprovalLevels.DataValueField = "Value";
        ddlSalesChannelApprovalLevels.DataBind();
        ddlSalesChannelApprovalLevels.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));

        ddlRetailerApprovalLevels.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "RETAILERAPPROVALLEVELS");
        ddlRetailerApprovalLevels.DataTextField = "Description";
        ddlRetailerApprovalLevels.DataValueField = "Value";
        ddlRetailerApprovalLevels.DataBind();
        ddlRetailerApprovalLevels.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));

        ddlRetailerUniqueMobile.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "RetailerUniqueMobile");
        ddlRetailerUniqueMobile.DataTextField = "Description";
        ddlRetailerUniqueMobile.DataValueField = "Value";
        ddlRetailerUniqueMobile.DataBind();
        ddlRetailerUniqueMobile.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));
        ddlISPUniqueMobile.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "ISPUniqueMobile");
        ddlISPUniqueMobile.DataTextField = "Description";
        ddlISPUniqueMobile.DataValueField = "Value";
        ddlISPUniqueMobile.DataBind();
        ddlISPUniqueMobile.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));
        ddlSecSaleReturnParentCheck.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "ALLOWRETPARENTCHECK");
        ddlSecSaleReturnParentCheck.DataTextField = "Description";
        ddlSecSaleReturnParentCheck.DataValueField = "Value";
        ddlSecSaleReturnParentCheck.DataBind();
        ddlSecSaleReturnParentCheck.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));
        ddlSecondarySalesReturnApproval.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "SecondarySalesReturnApproval");
        ddlSecondarySalesReturnApproval.DataTextField = "Description";
        ddlSecondarySalesReturnApproval.DataValueField = "Value";
        ddlSecondarySalesReturnApproval.DataBind();
        ddlSecondarySalesReturnApproval.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));
        ddlIntermediarySalesReturnApproval.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "IntermediarySalesReturnApproval");
        ddlIntermediarySalesReturnApproval.DataTextField = "Description";
        ddlIntermediarySalesReturnApproval.DataValueField = "Value";
        ddlIntermediarySalesReturnApproval.DataBind();
        ddlIntermediarySalesReturnApproval.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));
        ddlISDSaleStockOutUsingAPI.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "ISDSalePunchStockOutUsingCSAAPI");
        ddlISDSaleStockOutUsingAPI.DataTextField = "Description";
        ddlISDSaleStockOutUsingAPI.DataValueField = "Value";
        ddlISDSaleStockOutUsingAPI.DataBind();
        ddlISDSaleStockOutUsingAPI.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));
        ddlUploadDateFormat.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "UploadDateFormat");
        ddlUploadDateFormat.DataTextField = "Description";
        ddlUploadDateFormat.DataValueField = "Value";
        ddlUploadDateFormat.DataBind();
        ddlUploadDateFormat.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));
        ddlBeatPlanAutoApprove.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "BeatPlanAutoApprove");
        ddlBeatPlanAutoApprove.DataTextField = "Description";
        ddlBeatPlanAutoApprove.DataValueField = "Value";
        ddlBeatPlanAutoApprove.DataBind();
        ddlBeatPlanAutoApprove.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));

        ddlPhysicalStockAutoStockAdjustment.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "PhysicalStockAutoStockAdjustment");
        ddlPhysicalStockAutoStockAdjustment.DataTextField = "Description";
        ddlPhysicalStockAutoStockAdjustment.DataValueField = "Value";
        ddlPhysicalStockAutoStockAdjustment.DataBind();
        ddlPhysicalStockAutoStockAdjustment.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));

        ddlBackDateExpense.DataSource = PageBase.GetEnumByTableName("XML_AppConfig", "BackDateExpense");
        ddlBackDateExpense.DataTextField = "Description";
        ddlBackDateExpense.DataValueField = "Value";
        ddlBackDateExpense.DataBind();
        ddlBackDateExpense.Items.Insert(0, new ListItem(Resources.Messages.Select, "-1"));


    }
    private void BindConfig()
    {
        try
        {
            using (clsEntityTypeMaster objEntityType = new clsEntityTypeMaster())
            {
                objEntityType.UserId = PageBase.UserId;
                objEntityType.ActiveStatus = 255;
                objEntityType.CompanyId = PageBase.ClientId;
                DataSet ds = objEntityType.SelectAapplicationConfiguration();

                if (ds !=null && ds.Tables[0].Rows.Count>0)
                {
                    txtMaxUsers.Text = Convert.ToString(ds.Tables[0].Select("ConfigKey='MaxUsers'")[0]["ConfigValue"]);
                    txtPWDEXPY.Text = Convert.ToString(ds.Tables[0].Select("ConfigKey='PWDEXPY'")[0]["ConfigValue"]);
                    txtFailPasswordCount.Text = Convert.ToString(ds.Tables[0].Select("ConfigKey='PWDCNT'")[0]["ConfigValue"]);
                    txtMinSerailLength.Text = Convert.ToString(ds.Tables[0].Select("ConfigKey='SERIALLENGTHMIN'")[0]["ConfigValue"]);
                    txtMaxSerailLength.Text = Convert.ToString(ds.Tables[0].Select("ConfigKey='SERIALLENGTHMAX'")[0]["ConfigValue"]);
                    txtMinBatchLength.Text = Convert.ToString(ds.Tables[0].Select("ConfigKey='BATCHLENGTHMIN'")[0]["ConfigValue"]);
                    txtMaxBatchLength.Text = Convert.ToString(ds.Tables[0].Select("ConfigKey='BATCHLENGTHMAX'")[0]["ConfigValue"]);
                    string SALESCHANNELAPPROVALVALUES = Convert.ToString(ds.Tables[0].Select("ConfigKey='SALESCHANNELAPPROVALVALUES'")[0]["ConfigValue"]);
                    if (SALESCHANNELAPPROVALVALUES != "0")
                        ddlSalesChannelApprovalLevels.SelectedValue = SALESCHANNELAPPROVALVALUES;
                    string RETAILERAPPROVALLEVELS = Convert.ToString(ds.Tables[0].Select("ConfigKey='RETAILERAPPROVALLEVELS'")[0]["ConfigValue"]);
                    if (RETAILERAPPROVALLEVELS != "0")
                        ddlRetailerApprovalLevels.SelectedValue = RETAILERAPPROVALLEVELS;
                    string RetailerUniqueMobile = Convert.ToString(ds.Tables[0].Select("ConfigKey='RetailerUniqueMobile'")[0]["ConfigValue"]);
                    if (RETAILERAPPROVALLEVELS != "0")
                        ddlRetailerApprovalLevels.SelectedValue = RETAILERAPPROVALLEVELS;

                    ddlRetailerUniqueMobile.SelectedValue = Convert.ToString(ds.Tables[0].Select("ConfigKey='RetailerUniqueMobile'")[0]["ConfigValue"]);
                    ddlISPUniqueMobile.SelectedValue = Convert.ToString(ds.Tables[0].Select("ConfigKey='ISPUniqueMobile'")[0]["ConfigValue"]);

                    ddlSecSaleReturnParentCheck.SelectedValue = Convert.ToString(ds.Tables[0].Select("ConfigKey='ALLOWRETPARENTCHECK'")[0]["ConfigValue"]);
                    ddlSecondarySalesReturnApproval.SelectedValue = Convert.ToString(ds.Tables[0].Select("ConfigKey='SecondarySalesReturnApproval'")[0]["ConfigValue"]);
                    ddlIntermediarySalesReturnApproval.SelectedValue = Convert.ToString(ds.Tables[0].Select("ConfigKey='IntermediarySalesReturnApproval'")[0]["ConfigValue"]);

                    ddlISDSaleStockOutUsingAPI.SelectedValue = "";
                    txtRETAPPAUTORECDAYS.Text = "";
                    ddlUploadDateFormat.SelectedValue = "";
                    ddlBeatPlanAutoApprove.SelectedValue = "";
                    ddlPhysicalStockAutoStockAdjustment.SelectedValue = "";
                    ddlAPIStockAdjustmentResonID.SelectedValue = "";

                    txtTopSalesChannel.Text = "";
                    txtTopRetailer.Text = "";
                    ddlBackDateExpense.SelectedValue = "";
                }

            }
            
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsEntityTypeMaster objEntityType = new clsEntityTypeMaster())
            {
                Int16 result = 1;


                objEntityType.CompanyId = PageBase.ClientId;
                int intPWDEXPY;
                Int32.TryParse(txtPWDEXPY.Text, out intPWDEXPY);
                if (intPWDEXPY > 0)
                    objEntityType.PWDEXPY = intPWDEXPY;
                else
                    objEntityType.PWDEXPY = -1;
                objEntityType.SERIALLENGTHMIN = -1;
                int intSERIALLENGTHMIN;
                Int32.TryParse(txtMinSerailLength.Text, out intSERIALLENGTHMIN);
                if (intSERIALLENGTHMIN > 0)
                    objEntityType.SERIALLENGTHMIN = intSERIALLENGTHMIN;

                objEntityType.SERIALLENGTHMAX = -1;
                int intSERIALLENGTHMax;
                Int32.TryParse(txtMaxSerailLength.Text, out intSERIALLENGTHMax);
                if (intSERIALLENGTHMax > 0)
                    objEntityType.SERIALLENGTHMAX = intSERIALLENGTHMax;

                objEntityType.BATCHLENGTHMIN = -1;
                int intBatchENGTHMIN;
                Int32.TryParse(txtMinBatchLength.Text, out intBatchENGTHMIN);
                if (intBatchENGTHMIN > 0)
                    objEntityType.BATCHLENGTHMIN = intBatchENGTHMIN;

                objEntityType.BATCHLENGTHMAX = -1;
                int intBatchENGTHMax;
                Int32.TryParse(txtMaxBatchLength.Text, out intBatchENGTHMax);
                if (intBatchENGTHMax > 0)
                    objEntityType.BATCHLENGTHMAX = intBatchENGTHMax;
                objEntityType.PWDCNT = -1;

                int intPWDCNT;
                Int32.TryParse(txtFailPasswordCount.Text, out intPWDCNT);
                if (intPWDCNT > 0)
                    objEntityType.PWDCNT = intPWDCNT;

                objEntityType.RetailerUniqueMobile = -1;
                if (ddlRetailerUniqueMobile.SelectedIndex > 0)
                    objEntityType.RetailerUniqueMobile = Convert.ToInt32(ddlRetailerUniqueMobile.SelectedValue);
                objEntityType.ISPUniqueMobile = -1;

                if (ddlISPUniqueMobile.SelectedIndex > 0)
                    objEntityType.ISPUniqueMobile = Convert.ToInt32(ddlISPUniqueMobile.SelectedValue);

                objEntityType.ALLOWRETPARENTCHECK = -1;
                if (ddlSecSaleReturnParentCheck.SelectedIndex > 0)
                    objEntityType.ALLOWRETPARENTCHECK = Convert.ToInt32(ddlSecSaleReturnParentCheck.SelectedValue);

                objEntityType.SecondarySalesReturnApproval = -1;
                if (ddlSecondarySalesReturnApproval.SelectedIndex > 0)
                    objEntityType.SecondarySalesReturnApproval = Convert.ToInt32(ddlSecondarySalesReturnApproval.SelectedValue);

                objEntityType.RETAILERAPPROVALLEVELS = -1;
                if (ddlRetailerApprovalLevels.SelectedIndex > 0)
                    objEntityType.RETAILERAPPROVALLEVELS = Convert.ToInt32(ddlRetailerApprovalLevels.SelectedValue);

                objEntityType.SALESCHANNELAPPROVALVALUES = -1;
                if (ddlSalesChannelApprovalLevels.SelectedIndex > 0)
                    objEntityType.SALESCHANNELAPPROVALVALUES = Convert.ToInt32(ddlSalesChannelApprovalLevels.SelectedValue);

                objEntityType.IntermediarySalesReturnApproval = -1;
                if (ddlIntermediarySalesReturnApproval.SelectedIndex > 0)
                    objEntityType.IntermediarySalesReturnApproval = Convert.ToInt32(ddlIntermediarySalesReturnApproval.SelectedValue);

                objEntityType.ISDSalePunchStockOutUsingCSAAPI = -1;
                if (ddlISDSaleStockOutUsingAPI.SelectedIndex>0)
                    objEntityType.ISDSalePunchStockOutUsingCSAAPI = Convert.ToInt32(ddlISDSaleStockOutUsingAPI.SelectedValue);

                objEntityType.RETAPPAUTORECDAYS = -1;
                int intRETAPPAUTORECDAYS;
                Int32.TryParse(txtRETAPPAUTORECDAYS.Text, out intRETAPPAUTORECDAYS);
                if (intRETAPPAUTORECDAYS > 0)
                    objEntityType.RETAPPAUTORECDAYS = intRETAPPAUTORECDAYS;

                objEntityType.UploadDateFormat = -1;
                if(ddlUploadDateFormat.SelectedIndex>0)
                    objEntityType.UploadDateFormat = Convert.ToInt32(ddlUploadDateFormat.SelectedValue);

                objEntityType.BeatPlanAutoApprove = -1;
                if (ddlBeatPlanAutoApprove.SelectedIndex>0)
                    objEntityType.BeatPlanAutoApprove = Convert.ToInt32(ddlBeatPlanAutoApprove.SelectedValue);

                objEntityType.PhysicalStockAutoStockAdjustment = -1;
                if(ddlPhysicalStockAutoStockAdjustment.SelectedIndex>0)
                    objEntityType.PhysicalStockAutoStockAdjustment = Convert.ToInt32(ddlPhysicalStockAutoStockAdjustment.SelectedValue);

                objEntityType.APIStockAdjustmentResonID = -1;
                if(ddlAPIStockAdjustmentResonID.SelectedIndex>0)
                    objEntityType.APIStockAdjustmentResonID = Convert.ToInt32(ddlAPIStockAdjustmentResonID.SelectedValue);

                objEntityType.TopSalesChannel = -1;
                int intTopSalesChannel;
                Int32.TryParse(txtTopSalesChannel.Text, out intTopSalesChannel);
                if (intTopSalesChannel > 0)
                    objEntityType.TopSalesChannel = intTopSalesChannel;

                objEntityType.TopRetailer = -1;
                intTopSalesChannel=-1;
                Int32.TryParse(txtTopRetailer.Text, out intTopSalesChannel);
                if (intTopSalesChannel > 0)
                    objEntityType.TopRetailer = intTopSalesChannel;
                objEntityType.BackDateExpense = -1;
                if(ddlBackDateExpense.SelectedIndex>0)
                    objEntityType.BackDateExpense = Convert.ToInt32(ddlBackDateExpense.SelectedValue);

                result = objEntityType.UpdateAppConfig();
                if (result == 0)
                {

                    ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);


                }
                if (result == 2 && objEntityType.Error != "")
                {
                    ucMessage1.ShowError(objEntityType.Error.ToString());
                    
                }
                else if (result == 1)
                {
                    ucMessage1.ShowError(objEntityType.Error.ToString());
                }


            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();
            
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cleardata();
    }
    
    void clearInputControls()
    {
        


    }
    public void Cleardata()
    {
        try
        {
            
            
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();
           
        }
    }
    
   
    
    
    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        try
        {
            //using (clsEntityTypeMaster ObjEntityType = new clsEntityTypeMaster())
            //{
                
                
            //}
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();    
        }
    }
    

}