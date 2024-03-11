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

public partial class Masters_Common_ManageEntityTypeMaster : PageBase
{
    static bool boolsearchflag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindAllDropDown();
                BindList(1);
            }
        }
        catch (Exception ex)
        {
           
            ucMessage1.ShowError(ex.Message);
            
        }
    }
    protected void BindAllDropDown()
    {
        DataSet dt = new DataSet();
        using (clsEntityTypeMaster obj = new clsEntityTypeMaster())
        {
            obj.PageIndex = -2;
            
            dt = obj.SelectAllBaseEntityType();
            ddlBaseEntityType.DataSource = dt.Tables[0];
            ddlBaseEntityType.DataTextField = "BaseEntityTypeName";
            ddlBaseEntityType.DataValueField = "BaseEntityTypeID";
            ddlBaseEntityType.DataBind();
            ddlBaseEntityType.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            ddlsearchBaseEntityType.DataSource = dt.Tables[0];
            ddlsearchBaseEntityType.DataTextField = "BaseEntityTypeName";
            ddlsearchBaseEntityType.DataValueField = "BaseEntityTypeID";
            ddlsearchBaseEntityType.DataBind();
            ddlsearchBaseEntityType.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlStockMaintainedBySystem.DataSource = PageBase.GetEnumByTableName("XML_Enum", "StockMaintainedBySystem");
            //ddlStockMaintainedBySystem.DataTextField = "Description";
            //ddlStockMaintainedBySystem.DataValueField = "Value";
            //ddlStockMaintainedBySystem.DataBind();
            //ddlStockMaintainedBySystem.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlBrandCategoryMappingMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "BrandCategoryMappingMode");
            //ddlBrandCategoryMappingMode.DataTextField = "Description";
            //ddlBrandCategoryMappingMode.DataValueField = "Value";
            //ddlBrandCategoryMappingMode.DataBind();
            //ddlBrandCategoryMappingMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlGroupMappingMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "GroupMappingMode");
            //ddlGroupMappingMode.DataTextField = "Description";
            //ddlGroupMappingMode.DataValueField = "Value";
            //ddlGroupMappingMode.DataBind();
            //ddlGroupMappingMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlWeeklyOffMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "WeeklyOffMode");
            //ddlWeeklyOffMode.DataTextField = "Description";
            //ddlWeeklyOffMode.DataValueField = "Value";
            //ddlWeeklyOffMode.DataBind();
            //ddlWeeklyOffMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlEntityContactMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "EntityContactMode");
            //ddlEntityContactMode.DataTextField = "Description";
            //ddlEntityContactMode.DataValueField = "Value";
            //ddlEntityContactMode.DataBind();
            //ddlEntityContactMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlEntityDetailMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "EntityDetailMode");
            //ddlEntityDetailMode.DataTextField = "Description";
            //ddlEntityDetailMode.DataValueField = "Value";
            //ddlEntityDetailMode.DataBind();
            //ddlEntityDetailMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlApplicationWorkingMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "ApplicationWorkingMode");
            //ddlApplicationWorkingMode.DataTextField = "Description";
            //ddlApplicationWorkingMode.DataValueField = "Value";
            //ddlApplicationWorkingMode.DataBind();
            //ddlApplicationWorkingMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlEntityStatutoryMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "EntityStatutoryMode");
            //ddlEntityStatutoryMode.DataTextField = "Description";
            //ddlEntityStatutoryMode.DataValueField = "Value";
            //ddlEntityStatutoryMode.DataBind();
            //ddlEntityStatutoryMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlEntityBankMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "EntityBankMode");
            //ddlEntityBankMode.DataTextField = "Description";
            //ddlEntityBankMode.DataValueField = "Value";
            //ddlEntityBankMode.DataBind();
            //ddlEntityBankMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlAccessType.DataSource = PageBase.GetEnumByTableName("XML_Enum", "AccessType");
            //ddlAccessType.DataTextField = "Description";
            //ddlAccessType.DataValueField = "Value";
            //ddlAccessType.DataBind();
            //ddlAccessType.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlCreditTermMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "CreditTermMode");
            //ddlCreditTermMode.DataTextField = "Description";
            //ddlCreditTermMode.DataValueField = "Value";
            //ddlCreditTermMode.DataBind();
            //ddlCreditTermMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlJournalMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "JournalMode");
            //ddlJournalMode.DataTextField = "Description";
            //ddlJournalMode.DataValueField = "Value";
            //ddlJournalMode.DataBind();
            //ddlJournalMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            ddlTargetMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "TargetMode");
            ddlTargetMode.DataTextField = "Description";
            ddlTargetMode.DataValueField = "Value";
            ddlTargetMode.DataBind();
            ddlTargetMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlServiceCustomerMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "ServiceCustomerMode");
            //ddlServiceCustomerMode.DataTextField = "Description";
            //ddlServiceCustomerMode.DataValueField = "Value";
            //ddlServiceCustomerMode.DataBind();
            //ddlServiceCustomerMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlServicePJPMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "ServicePJPMode");
            //ddlServicePJPMode.DataTextField = "Description";
            //ddlServicePJPMode.DataValueField = "Value";
            //ddlServicePJPMode.DataBind();
            //ddlServicePJPMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            ddlSAPCodeMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "AutoCodeGenerateMode");
            ddlSAPCodeMode.DataTextField = "Description";
            ddlSAPCodeMode.DataValueField = "Value";
            ddlSAPCodeMode.DataBind();
            ddlSAPCodeMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlAuthoriseForLeadPass.DataSource = PageBase.GetEnumByTableName("XML_Enum", "AuthoriseForLeadPass");
            //ddlAuthoriseForLeadPass.DataTextField = "Description";
            //ddlAuthoriseForLeadPass.DataValueField = "Value";
            //ddlAuthoriseForLeadPass.DataBind();
            //ddlAuthoriseForLeadPass.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlCityMappingMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "CityMappingMode");
            //ddlCityMappingMode.DataTextField = "Description";
            //ddlCityMappingMode.DataValueField = "Value";
            //ddlCityMappingMode.DataBind();
            //ddlCityMappingMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlEntityMilMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "EntityMilMode");
            //ddlEntityMilMode.DataTextField = "Description";
            //ddlEntityMilMode.DataValueField = "Value";
            //ddlEntityMilMode.DataBind();
            //ddlEntityMilMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlPriceGroupMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "PriceGroupMode");
            //ddlPriceGroupMode.DataTextField = "Description";
            //ddlPriceGroupMode.DataValueField = "Value";
            //ddlPriceGroupMode.DataBind();
            //ddlPriceGroupMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlEntityAddressMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "EntityAddressMode");
            //ddlEntityAddressMode.DataTextField = "Description";
            //ddlEntityAddressMode.DataValueField = "Value";
            //ddlEntityAddressMode.DataBind();
            //ddlEntityAddressMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            //ddlCalender.DataSource = PageBase.GetEnumByTableName("XML_Enum", "HolidayCalendarRequired");
            //ddlCalender.DataTextField = "Description";
            //ddlCalender.DataValueField = "Value";
            //ddlCalender.DataBind();
            //ddlCalender.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));
            ddlSalesChannelLavel.DataSource = PageBase.GetEnumByTableName("XML_Enum", "SalesChannelLevel");
            ddlSalesChannelLavel.DataTextField = "Description";
            ddlSalesChannelLavel.DataValueField = "Value";
            ddlSalesChannelLavel.DataBind();
            ddlSalesChannelLavel.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));


            ddlBilltoretailer.DataSource = PageBase.GetEnumByTableName("XML_Enum", "BillToRetailer");
            ddlBilltoretailer.DataTextField = "Description";
            ddlBilltoretailer.DataValueField = "Value";
            ddlBilltoretailer.DataBind();
            ddlBilltoretailer.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));

            ddlStockTransfermode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "StockTransferMode");
            ddlStockTransfermode.DataTextField = "Description";
            ddlStockTransfermode.DataValueField = "Value";
            ddlStockTransfermode.DataBind();
            ddlStockTransfermode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));

            ddlReportHierarchyLevel.DataSource = PageBase.GetEnumByTableName("XML_Enum", "ReportHierarchyLevel");
            ddlReportHierarchyLevel.DataTextField = "Description";
            ddlReportHierarchyLevel.DataValueField = "Value";
            ddlReportHierarchyLevel.DataBind();
            ddlReportHierarchyLevel.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));

            //ddlIsPanMandatory.DataSource = PageBase.GetEnumByTableName("XML_Enum", "IsPanMandatory");
            //ddlIsPanMandatory.DataTextField = "Description";
            //ddlIsPanMandatory.DataValueField = "Value";
            //ddlIsPanMandatory.DataBind();
            //ddlIsPanMandatory.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));

            ddlAppRoleTypeID.DataSource = dt.Tables[1];
            ddlAppRoleTypeID.DataTextField = "AppRoleTypeName";
            ddlAppRoleTypeID.DataValueField = "AppRoleTypeID";
            ddlAppRoleTypeID.DataBind();
            ddlAppRoleTypeID.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));


            ddlStockMaintainMode.DataSource = PageBase.GetEnumByTableName("XML_Enum", "StockMaintainMode");
            ddlStockMaintainMode.DataTextField = "Description";
            ddlStockMaintainMode.DataValueField = "Value";
            ddlStockMaintainMode.DataBind();
            ddlStockMaintainMode.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));

            ddlShowInApp.DataSource = PageBase.GetEnumByTableName("XML_Enum", "ShowInApp");
            ddlShowInApp.DataTextField = "Description";
            ddlShowInApp.DataValueField = "Value";
            ddlShowInApp.DataBind();
            ddlShowInApp.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));

            DataTable DtRole = new DataTable();
            DtRole.Columns.Add("RoleId", typeof(Int32));
            DtRole.Columns.Add("RoleName", typeof(string));
            DtRole.Columns.Add("Status", typeof(Int16));
            DtRole.AcceptChanges();
            ViewState["DtRole"] = DtRole;

        }
        using(clsEntityMappingTypeRelationMaster objRelation=new clsEntityMappingTypeRelationMaster())
        {
            //DataSet ds;
            objRelation.EntityMappingID = 1;
            objRelation.CompanyId = PageBase.ClientId;
            dt = objRelation.getEntityMappingTypeRelationMasterDropdowns();
            String[] StrCol1 = new String[] { "EntityTypeID", "EntityType" };
            PageBase.DropdownBinding(ref ddlParentHierarchyType, dt.Tables[0], StrCol1);
            //ddlParentHierarchyType.DataSource=dt.Tables[0];
            //ddlParentHierarchyType.DataTextField = "EntityType";
            //ddlShowInApp.DataValueField = dt.Tables[0].Columns[0].ColumnName;//"EntityTypeID";
            //ddlShowInApp.DataBind();
            //ddlShowInApp.Items.Insert(0, new ListItem(Resources.Messages.Select, "100"));

            objRelation.EntityMappingID = 2;
            
            dt = objRelation.getEntityMappingTypeRelationMasterDropdowns();
            chkParentSalesChannelType.DataSource=dt.Tables[0];
            chkParentSalesChannelType.DataValueField = "EntityTypeID";
            chkParentSalesChannelType.DataTextField = "EntityType";
            chkParentSalesChannelType.DataBind();

            objRelation.EntityMappingID = 3;

            dt = objRelation.getEntityMappingTypeRelationMasterDropdowns();
            chkStockTransferType.DataSource = dt.Tables[0];
            chkStockTransferType.DataValueField = "EntityTypeID";
            chkStockTransferType.DataTextField = "EntityType";
            chkStockTransferType.DataBind();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsEntityTypeMaster objEntityType = new clsEntityTypeMaster())
            {
                Int16 result = 1;
                string ParentSalesChannelTypes = "", StockTransferSCTypes = "";

                for (int cntr = 0; cntr < chkParentSalesChannelType.Items.Count; cntr++)
                {
                    if (chkParentSalesChannelType.Items[cntr].Selected)
                        ParentSalesChannelTypes = ParentSalesChannelTypes + chkParentSalesChannelType.Items[cntr].Value + ",";

                }
                for (int cntr = 0; cntr < chkStockTransferType.Items.Count; cntr++)
                {
                    if (chkStockTransferType.Items[cntr].Selected)
                        StockTransferSCTypes = StockTransferSCTypes + chkStockTransferType.Items[cntr].Value + ",";

                }

                objEntityType.CompanyId = PageBase.ClientId;
                DataTable dt = (DataTable)ViewState["DtRole"];
                objEntityType.Dt = dt;
                objEntityType.CreatedBy = PageBase.UserId;
                objEntityType.String1 = ParentSalesChannelTypes;
                objEntityType.String2 = StockTransferSCTypes;
                objEntityType.ParentHierarchyTypeId = Convert.ToInt32(ddlParentHierarchyType.SelectedValue);

                if (ucInTime.TextBoxTime.Text != "")
                {
                    if (ucOutTime.TextBoxTime.Text == "")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowInfo("Please Enter Working To Time");
                        return;
                    }
                }
                if (ucOutTime.TextBoxTime.Text != "")
                {
                    if (ucInTime.TextBoxTime.Text == "")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowInfo("Please Enter Working From Time");
                        return;
                    }
                }


                if (btnSave.Text == "Save")
                {


                    
                    txtEntityType.Enabled = true;
                    objEntityType.EntityType = txtEntityType.Text.Trim();
                    objEntityType.BaseEntityTypeID = Convert.ToInt16(ddlBaseEntityType.SelectedValue);
                    //objEntityType.AccessType = Convert.ToInt16(ddlAccessType.SelectedValue);
                    objEntityType.ActiveStatus = Convert.ToInt16(chkActive.Checked);
                    //objEntityType.ApplicationWorkingMode = Convert.ToInt16(ddlApplicationWorkingMode.SelectedValue);
                    //objEntityType.AuthoriseForLeadPass = Convert.ToInt16(ddlAuthoriseForLeadPass.SelectedValue);
                    //objEntityType.BrandCategoryMappingMode = Convert.ToInt16(ddlBrandCategoryMappingMode.SelectedValue);
                    //objEntityType.CityMappingMode = Convert.ToInt16(ddlCityMappingMode.SelectedValue);
                    objEntityType.CreatedBy = PageBase.UserId;
                    objEntityType.CreatedOn = DateTime.Today;
                    //objEntityType.CreditTermMode = Convert.ToInt16(ddlCreditTermMode.SelectedValue);
                    //objEntityType.EntityAddressMode = Convert.ToInt16(ddlEntityAddressMode.SelectedValue);
                    //objEntityType.EntityBankMode = Convert.ToInt16(ddlEntityBankMode.SelectedValue);
                    //objEntityType.EntityContactMode = Convert.ToInt16(ddlEntityContactMode.SelectedValue);
                    //objEntityType.EntityDetailMode = Convert.ToInt16(ddlEntityDetailMode.SelectedValue);
                    //objEntityType.EntityMilMode = Convert.ToInt16(ddlEntityMilMode.SelectedValue);
                    //objEntityType.EntityStatutoryMode = Convert.ToInt16(ddlEntityStatutoryMode.SelectedValue);
                    //objEntityType.GroupMappingMode = Convert.ToInt16(ddlGroupMappingMode.SelectedValue);
                    //objEntityType.JournalMode = Convert.ToInt16(ddlJournalMode.SelectedValue);
                    objEntityType.ModifiedBy = PageBase.UserId; ;
                    objEntityType.ModifiedOn = DateTime.Today;
                    //objEntityType.PriceGroupMode = Convert.ToInt16(ddlPriceGroupMode.SelectedValue);
                    objEntityType.SAPCodeMode = Convert.ToInt16(ddlSAPCodeMode.SelectedValue);
                    //objEntityType.ServiceCustomerMode = Convert.ToInt16(ddlServiceCustomerMode.SelectedValue);
                    //objEntityType.ServicePJPMode = Convert.ToInt16(ddlServicePJPMode.SelectedValue);
                    //objEntityType.StockMaintainedBySystem = Convert.ToInt16(ddlStockMaintainedBySystem.SelectedValue);
                    objEntityType.TargetMode = Convert.ToInt16(ddlTargetMode.SelectedValue);
                    //objEntityType.WeeklyOffMode = Convert.ToInt16(ddlWeeklyOffMode.SelectedValue);
                    //objEntityType.HolidayCalendarRequired = Convert.ToInt16(ddlCalender.SelectedValue);
                    if (ddlSalesChannelLavel.SelectedValue!="100")
                    {
                        objEntityType.SalesChannelLavel = Convert.ToInt16(ddlSalesChannelLavel.SelectedValue);
                    }  
                    objEntityType.BilltoRetailor = Convert.ToInt16(ddlBilltoretailer.SelectedValue);
                    objEntityType.StockTransferMode = Convert.ToInt16(ddlStockTransfermode.SelectedValue);
                    if (ddlReportHierarchyLevel.SelectedValue!="100")
                    {
                        objEntityType.ReportHierarchyLavel = Convert.ToInt16(ddlReportHierarchyLevel.SelectedValue);
                    }
                    //objEntityType.IsPanMandatory = Convert.ToInt16(ddlIsPanMandatory.SelectedValue);
                    if (ddlAppRoleTypeID.SelectedValue!="100")
                    {
                        objEntityType.ApprolTypeId = Convert.ToInt16(ddlAppRoleTypeID.SelectedValue);
                    }
                    objEntityType.StockMantainMode = Convert.ToInt16(ddlStockMaintainMode.SelectedValue);
                    objEntityType.ShowinApp = Convert.ToInt16(ddlShowInApp.SelectedValue);
                    if (txtNumberofBackDays.Text!="")
                    {
                        objEntityType.BackDayAllowforSale = Convert.ToInt32(txtNumberofBackDays.Text);
                    }

                    if (txtBackDaysAllowedForSaleReturn.Text!="")
                    {
                        objEntityType.BackDayAllowforSaleReturn = Convert.ToInt32(txtBackDaysAllowedForSaleReturn.Text);
                    }

                    objEntityType.InTime = ucInTime.GetTime;
                    objEntityType.OutTime = ucOutTime.GetTime;
                    result = objEntityType.Insert();
                    if (result == 0)
                    {
                        dt.Rows.Clear();
                        ViewState["DtRole"] = dt;
                        ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        Cleardata();

                    }
                    if (result == 2 && objEntityType.Error!="")
                    {
                        ucMessage1.ShowError(objEntityType.Error.ToString());
                        btnCancel.Visible = true;
                    }
                    else if (result == 1)
                    {
                        ucMessage1.ShowError(objEntityType.Error.ToString());
                    }

                }
                else if (btnSave.Text == "Update")
                {
                    objEntityType.EntityType = txtEntityType.Text.Trim();
                    objEntityType.BaseEntityTypeID = Convert.ToInt16(ddlBaseEntityType.SelectedValue);
                    //objEntityType.AccessType = Convert.ToInt16(ddlAccessType.SelectedValue);
                    objEntityType.ActiveStatus = Convert.ToInt16(chkActive.Checked); ;
                    //objEntityType.ApplicationWorkingMode = Convert.ToInt16(ddlApplicationWorkingMode.SelectedValue);
                    //objEntityType.AuthoriseForLeadPass = Convert.ToInt16(ddlAuthoriseForLeadPass.SelectedValue);
                    //objEntityType.BrandCategoryMappingMode = Convert.ToInt16(ddlBrandCategoryMappingMode.SelectedValue);
                    //objEntityType.CityMappingMode = Convert.ToInt16(ddlCityMappingMode.SelectedValue);
                    objEntityType.CreatedBy = PageBase.UserId;
                    objEntityType.CreatedOn = DateTime.Today;
                    //objEntityType.CreditTermMode = Convert.ToInt16(ddlCreditTermMode.SelectedValue);
                    //objEntityType.EntityAddressMode = Convert.ToInt16(ddlEntityAddressMode.SelectedValue);
                    //objEntityType.EntityBankMode = Convert.ToInt16(ddlEntityBankMode.SelectedValue);
                    //objEntityType.EntityContactMode = Convert.ToInt16(ddlEntityContactMode.SelectedValue);
                    //objEntityType.EntityDetailMode = Convert.ToInt16(ddlEntityDetailMode.SelectedValue);
                    //objEntityType.EntityMilMode = Convert.ToInt16(ddlEntityMilMode.SelectedValue);
                    //objEntityType.EntityStatutoryMode = Convert.ToInt16(ddlEntityStatutoryMode.SelectedValue);
                    //objEntityType.GroupMappingMode = Convert.ToInt16(ddlGroupMappingMode.SelectedValue);
                    //objEntityType.JournalMode = Convert.ToInt16(ddlJournalMode.SelectedValue);
                    objEntityType.ModifiedBy = PageBase.UserId; ;
                    objEntityType.ModifiedOn = DateTime.Today;
                    //objEntityType.PriceGroupMode = Convert.ToInt16(ddlPriceGroupMode.SelectedValue);
                    objEntityType.SAPCodeMode = Convert.ToInt16(ddlSAPCodeMode.SelectedValue);
                    //objEntityType.ServiceCustomerMode = Convert.ToInt16(ddlServiceCustomerMode.SelectedValue);
                    //objEntityType.ServicePJPMode = Convert.ToInt16(ddlServicePJPMode.SelectedValue);
                   // objEntityType.StockMaintainedBySystem = Convert.ToInt16(ddlStockMaintainedBySystem.SelectedValue);
                    objEntityType.TargetMode = Convert.ToInt16(ddlTargetMode.SelectedValue);
                    //objEntityType.WeeklyOffMode = Convert.ToInt16(ddlWeeklyOffMode.SelectedValue);
                    objEntityType.EntityTypeID = Convert.ToInt16(Convert.ToString(ViewState["EntityTypeID"]));
                    //objEntityType.HolidayCalendarRequired = Convert.ToInt16(ddlCalender.SelectedValue);
                    if (ddlSalesChannelLavel.SelectedValue != "100")
                    {
                        objEntityType.SalesChannelLavel = Convert.ToInt16(ddlSalesChannelLavel.SelectedValue);
                    }
                    else
                    {
                        objEntityType.SalesChannelLavel = 0;
                    }
                    objEntityType.BilltoRetailor = Convert.ToInt16(ddlBilltoretailer.SelectedValue);
                    objEntityType.StockTransferMode = Convert.ToInt16(ddlStockTransfermode.SelectedValue);
                    if (ddlReportHierarchyLevel.SelectedValue != "100")
                    {
                        objEntityType.ReportHierarchyLavel = Convert.ToInt16(ddlReportHierarchyLevel.SelectedValue);
                    }
                    else
                    {
                        objEntityType.ReportHierarchyLavel = 0;
                    }
                    //objEntityType.IsPanMandatory = Convert.ToInt16(ddlIsPanMandatory.SelectedValue);
                    if (ddlAppRoleTypeID.SelectedValue != "100")
                    {
                        objEntityType.ApprolTypeId = Convert.ToInt16(ddlAppRoleTypeID.SelectedValue);
                    }
                    else
                    {
                        objEntityType.ApprolTypeId = 0;
                    }
                    objEntityType.StockMantainMode = Convert.ToInt16(ddlStockMaintainMode.SelectedValue);
                    objEntityType.ShowinApp = Convert.ToInt16(ddlShowInApp.SelectedValue);
                    if (txtNumberofBackDays.Text != "")
                    {
                        objEntityType.BackDayAllowforSale = Convert.ToInt32(txtNumberofBackDays.Text);
                    }

                    if (txtBackDaysAllowedForSaleReturn.Text != "")
                    {
                        objEntityType.BackDayAllowforSaleReturn = Convert.ToInt32(txtBackDaysAllowedForSaleReturn.Text);
                    }
                    objEntityType.InTime = ucInTime.GetTime;
                    objEntityType.OutTime = ucOutTime.GetTime;
                    objEntityType.CompanyId = PageBase.ClientId;
                    result = objEntityType.Update();


                    if (result == 0)
                    {
                        txtEntityType.Enabled = true;
                        if (btnSave.Text == "Save")
                        {
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        }
                        else
                        {
                            ucMessage1.ShowSuccess("Record updated succesfully.");
                        }
                        clearInputControls();
                        
                    }
                    else if (result == 1)
                        ucMessage1.ShowError(objEntityType.Error.ToString());
                    else if (result == 2)
                    {
                        ucMessage1.ShowError(Resources.Messages.DuplicateEntityType);
                        btnCancel.Visible = true;
                    }
                }
                BindListSearch(Convert.ToInt32(hdfCurrentPage.Value));

                updpnlgrdvList.Update();
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlActive.SelectedIndex == 0 && txtSearchEntityType.Text.Trim() == "" && ddlsearchBaseEntityType.SelectedIndex == 0)
            {
                ucMessage1.ShowWarning(Resources.Messages.SearchCriteriaBlank);
                updpnlSaveData.Update();
                return;
            }
            hdfCurrentPage.Value = "1";
            ucMessage1.Visible = false;
            updpnlSaveData.Update();
            boolsearchflag = true;
            BindListSearch(1);
            dvGrid.Visible = true;
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();
        }
    }
    protected void btnViewAll_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            hdfCurrentPage.Value = "1";
            Cleardata();
            ddlActive.SelectedIndex = 0;
            ddlsearchBaseEntityType.SelectedIndex = 0;
            txtSearchEntityType.Text = "";
            BindList(1);
            dvGrid.Visible = true;
            UpdatePanel1.Update();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();
        }
    }
    void clearInputControls()
    {
        txtEntityType.Text = "";
        //ddlWeeklyOffMode.SelectedValue = "100";
        ddlTargetMode.SelectedValue = "100";
       // ddlStockMaintainedBySystem.SelectedValue = "100";
        //ddlServicePJPMode.SelectedValue = "100";
        //ddlServiceCustomerMode.SelectedValue = "100";
        ddlSAPCodeMode.SelectedValue = "100";
        //ddlPriceGroupMode.SelectedValue = "100";
        //ddlJournalMode.SelectedValue = "100";
        //ddlGroupMappingMode.SelectedValue = "100";
        //ddlEntityStatutoryMode.SelectedValue = "100";
        //ddlEntityMilMode.SelectedValue = "100";
        //ddlEntityDetailMode.SelectedValue = "100";
        //ddlEntityContactMode.SelectedValue = "100";
        //ddlEntityBankMode.SelectedValue = "100";
        //ddlEntityAddressMode.SelectedValue = "100";
        //ddlCreditTermMode.SelectedValue = "100";
        //ddlCityMappingMode.SelectedValue = "100";
        //ddlBrandCategoryMappingMode.SelectedValue = "100";
        ddlBaseEntityType.SelectedValue = "100";
        //ddlAuthoriseForLeadPass.SelectedValue = "100";
        //ddlApplicationWorkingMode.SelectedValue = "100";
        //ddlAccessType.SelectedValue = "100";
        //ddlCalender.SelectedValue = "100";
        txtEntityType.Enabled = true;
        btnCancel.Visible = true;
        btnSave.Text = "Save";
        ddlReportHierarchyLevel.SelectedValue = "100";
        ddlSalesChannelLavel.SelectedValue = "100";
        ddlShowInApp.SelectedValue = "100";
        ddlStockMaintainMode.SelectedValue = "100";
        //ddlIsPanMandatory.SelectedValue = "100";
        ddlBilltoretailer.SelectedValue = "100";
        ddlAppRoleTypeID.SelectedValue = "100";
        ddlStockTransfermode.SelectedValue = "100";
        txtBackDaysAllowedForSaleReturn.Text = "";
        txtNumberofBackDays.Text = "";
        ucInTime.TextBoxTime.Text = "";
        ucOutTime.TextBoxTime.Text = "";
        grdvwRoleList.DataSource = null;
        grdvwRoleList.DataBind();
        txtRole.Text = "";
        ddlParentHierarchyType.SelectedValue = "0";
        chkParentSalesChannelType.SelectedIndex= -1;
        chkStockTransferType.SelectedIndex = -1;


    }
    public void Cleardata()
    {
        try
        {
            txtEntityType.Text = "";
            txtSearchEntityType.Text = "";
            ddlTargetMode.SelectedValue = "100";
            ddlsearchBaseEntityType.SelectedValue = "100";
            ddlSAPCodeMode.SelectedValue = "100";
            ddlBaseEntityType.SelectedValue = "100";
            ddlActive.SelectedValue = "100";
            txtEntityType.Enabled = true;
            txtSearchEntityType.Enabled = true;
            btnCancel.Visible = true;
            btnSave.Text = "Save";
            ddlReportHierarchyLevel.SelectedValue = "100";
            ddlSalesChannelLavel.SelectedValue = "100";
            ddlShowInApp.SelectedValue = "100";
            ddlStockMaintainMode.SelectedValue = "100";
            ddlBilltoretailer.SelectedValue = "100";
            ddlAppRoleTypeID.SelectedValue = "100";
            ddlStockTransfermode.SelectedValue = "100";
            txtBackDaysAllowedForSaleReturn.Text = "";
            txtNumberofBackDays.Text = "";
            txtRole.Text = "";
            ucInTime.TextBoxTime.Text = "";
            ucOutTime.TextBoxTime.Text = "";
            grdvwRoleList.DataSource = null;
            grdvwRoleList.DataBind();
            txtRole.Text = "";
            ddlParentHierarchyType.SelectedValue = "0";
            chkParentSalesChannelType.SelectedIndex = -1;
            chkStockTransferType.SelectedIndex = -1;
            //ddlWeeklyOffMode.SelectedValue = "100";
            //ddlStockMaintainedBySystem.SelectedValue = "100";
            //ddlServicePJPMode.SelectedValue = "100";
            //ddlServiceCustomerMode.SelectedValue = "100";
            //ddlPriceGroupMode.SelectedValue = "100";
            //ddlJournalMode.SelectedValue = "100";
            //ddlGroupMappingMode.SelectedValue = "100";
            //ddlEntityStatutoryMode.SelectedValue = "100";
            //ddlEntityMilMode.SelectedValue = "100";
            //ddlEntityDetailMode.SelectedValue = "100";
            //ddlEntityContactMode.SelectedValue = "100";
            //ddlEntityBankMode.SelectedValue = "100";
            //ddlEntityAddressMode.SelectedValue = "100";
            //ddlCreditTermMode.SelectedValue = "100";
            //ddlCityMappingMode.SelectedValue = "100";
            //ddlBrandCategoryMappingMode.SelectedValue = "100";
            //ddlAuthoriseForLeadPass.SelectedValue = "100";
            //ddlApplicationWorkingMode.SelectedValue = "100";
            //ddlAccessType.SelectedValue = "100";
            //ddlCalender.SelectedValue = "100";
            //ddlIsPanMandatory.SelectedValue = "100";
            
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();
           
        }
    }
    void BindListSearch(int index)
    {
        try
        {
            using (clsEntityTypeMaster objEntityType = new clsEntityTypeMaster())
            {
                objEntityType.LoginedEntityTypeId = PageBase.EntityTypeID;
                objEntityType.LoginEntityId = PageBase.BaseEntityTypeID;
                objEntityType.EntityType = txtSearchEntityType.Text.Trim();
                objEntityType.EntityTypeID = Convert.ToInt16(ddlsearchBaseEntityType.SelectedValue) == 100 ? Convert.ToInt16(0) : Convert.ToInt16(ddlsearchBaseEntityType.SelectedValue);
                objEntityType.PageSize =Convert.ToInt32(PageBase.PageSize);
                objEntityType.PageIndex = index;
                objEntityType.ActiveStatus = (ddlActive.SelectedValue == "233" || ddlActive.SelectedValue == "255") ? Convert.ToInt16(255) : Convert.ToInt16(ddlActive.SelectedValue);
                objEntityType.CompanyId = PageBase.ClientId;
                DataSet ds = objEntityType.SelectAllEntity();
                grdvList.Visible = true;
                grdvList.DataSource = ds.Tables[0];
                grdvList.DataBind();
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    ucPagingControl1.Visible = false;
                    Exporttoexcel.Visible = false;
                }
                else
                {
                    //Paging
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize =Convert.ToInt32(PageBase.PageSize);
                    ucPagingControl1.TotalRecords = objEntityType.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                    Exporttoexcel.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();
        }
    }

   
    void BindList(int index)
    {
        try
        {
            using (clsEntityTypeMaster objEntityType = new clsEntityTypeMaster())
            {
                boolsearchflag = false;
                objEntityType.PageSize =Convert.ToInt32(PageBase.PageSize);
                objEntityType.PageIndex = index;
                objEntityType.ActiveStatus = 255;
                objEntityType.CompanyId = PageBase.ClientId;
                DataSet ds = objEntityType.SelectAllEntity();
                if (index > 1 && ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    index--;
                    objEntityType.PageIndex = index;
                    ds = objEntityType.SelectAllEntity();
                }
                grdvList.DataSource = ds.Tables[0];
                grdvList.DataBind();
               
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    ucPagingControl1.Visible = false;
                }
                else
                {
                    //Paging
                    ucPagingControl1.CurrentPage = index;
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageBase.PageSize);
                    ucPagingControl1.TotalRecords = objEntityType.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                }
                updpnlgrdvList.Update();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();
           
        }
    }
    protected void grdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            using (clsEntityTypeMaster objEntityType = new clsEntityTypeMaster())
            {
                objEntityType.CreatedBy = PageBase.UserId;
                if (e.CommandName == "editEntityType")
                {
                    txtEntityType.Enabled = true;
                    string id = Convert.ToString(e.CommandArgument);
                    ViewState["EntityTypeID"] = id;
                    objEntityType.EntityTypeID = Convert.ToInt32(id);
                    DataSet ds = objEntityType.SelectById();
                    btnSave.Text = "Update";
                    txtEntityType.Text = Convert.ToString(ds.Tables[0].Rows[0]["EntityType"]);
                    //ddlWeeklyOffMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["WeeklyOffMode"]);
                    ddlTargetMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["TargetMode"]);
                   // ddlStockMaintainedBySystem.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["StockMaintainedBySystem"]);
                    //ddlServicePJPMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ServicePJPMode"]);
                    //ddlServiceCustomerMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ServiceCustomerMode"]);
                    ddlSAPCodeMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["SAPCodeMode"]);
                    //ddlPriceGroupMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["PriceGroupMode"]);
                    //ddlJournalMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["JournalMode"]);
                    //ddlGroupMappingMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["GroupMappingMode"]);
                    //ddlEntityStatutoryMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["EntityStatutoryMode"]);
                    //ddlEntityMilMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["EntityMilMode"]);
                    //ddlEntityDetailMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["EntityDetailMode"]);
                    //ddlEntityContactMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["EntityContactMode"]);
                    //ddlEntityBankMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["EntityBankMode"]);
                    //ddlEntityAddressMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["EntityAddressMode"]);
                    //ddlCreditTermMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["CreditTermMode"]);
                    //ddlCityMappingMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["CityMappingMode"]);
                    //ddlBrandCategoryMappingMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["BrandCategoryMappingMode"]);
                    ddlBaseEntityType.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["BaseEntityTypeID"]);
                    //ddlAuthoriseForLeadPass.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["AuthoriseForLeadPass"]);
                    //ddlApplicationWorkingMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ApplicationWorkingMode"]);
                    //ddlAccessType.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["AccessType"]);
                    //ddlCalender.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["HolidayCalendarRequired"]);
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["SalesChannelLevel"])>0)
                    {
                        ddlSalesChannelLavel.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["SalesChannelLevel"]);
                    }
                    else
                    {
                        ddlSalesChannelLavel.SelectedValue = "100";
                    }
                    ddlBilltoretailer.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["BilltoRetailer"]);
                    ddlStockTransfermode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["StockTransferMode"]);
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["ReportHierarchyLevel"]) > 0)
                    {
                        ddlReportHierarchyLevel.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ReportHierarchyLevel"]);
                    }
                    else
                    {
                        ddlReportHierarchyLevel.SelectedValue = "100";
                    }
                    //ddlIsPanMandatory.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["IsPanMandatory"]);
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["AppRoleTypeID"]) > 0)
                    {
                        ddlAppRoleTypeID.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["AppRoleTypeID"]);
                    }
                    else
                    {
                        ddlAppRoleTypeID.SelectedValue = "100";
                    }
                    ddlStockMaintainMode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["StockMaintainMode"]);
                    ddlShowInApp.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ShowInApp"]);
                    txtBackDaysAllowedForSaleReturn.Text = Convert.ToString(ds.Tables[0].Rows[0]["BackDaysAllowedForSaleReturn"]);
                    txtNumberofBackDays.Text = Convert.ToString(ds.Tables[0].Rows[0]["NumberofBackDays"]);
                    ucInTime.Time = GetFormattedTime(ds.Tables[0].Rows[0]["OfficeInTime"]);
                    ucOutTime.Time = GetFormattedTime(ds.Tables[0].Rows[0]["OfficeOutTime"]);
                    for (int cntr = 0; cntr < ddlParentHierarchyType.Items.Count; cntr++)
                    {
                        if (ddlParentHierarchyType.Items[cntr].Value == Convert.ToString(ds.Tables[0].Rows[0]["HierarchyEnitytypeID"]))
                        {
                            ddlParentHierarchyType.SelectedIndex = cntr;
                            break;
                        }
                    }
                    for (int cntr = 0; cntr < ds.Tables[1].Rows.Count; cntr++)
                    {
                        for (int cntrChk = 0; cntrChk < chkParentSalesChannelType.Items.Count; cntrChk++)
                        {
                            if (chkParentSalesChannelType.Items[cntrChk].Value == Convert.ToString(ds.Tables[1].Rows[cntr]["PrimaryEntityTypeID"]))
                            {
                                chkParentSalesChannelType.Items[cntrChk].Selected = true;
                            }
                            
                        }
                    }

                    for (int cntr = 0; cntr < ds.Tables[2].Rows.Count; cntr++)
                    {
                        for (int cntrChk = 0; cntrChk < chkStockTransferType.Items.Count; cntrChk++)
                        {
                            if (chkStockTransferType.Items[cntrChk].Value == Convert.ToString(ds.Tables[2].Rows[cntr]["PrimaryEntityTypeID"]))
                            {
                                chkStockTransferType.Items[cntrChk].Selected = true;
                            }

                        }
                    }
                    //Role Grid
                    if (ds.Tables[3].Rows.Count>0)
                    {
                        grdvwRoleList.DataSource = ds.Tables[3];
                        grdvwRoleList.DataBind();
                        ViewState["DtRole"] = ds.Tables[3];
                    }
                    txtRole.Text = "";
                    ucMessage1.Visible = false;
                }
                else if (e.CommandName == "deleteEntityType")
                {
                    string id = Convert.ToString(e.CommandArgument);
                    objEntityType.EntityTypeID = Int32.Parse(id);
                    bool result = objEntityType.Delete();
                    if (result)
                        ucMessage1.ShowSuccess(Resources.Messages.AdminDelete);
                    else
                        ucMessage1.ShowError(Resources.Messages.NotDeleted);

                    BindList(Convert.ToInt32(hdfCurrentPage.Value));
                    if (boolsearchflag)
                        BindListSearch(Convert.ToInt32(hdfCurrentPage.Value));
                    else
                        BindList(Convert.ToInt32(hdfCurrentPage.Value));
                    updpnlgrdvList.Update();
                    updpnlSaveData.Update();
                }
                else if (e.CommandName == "activeEntityType")
                {
                    string id = Convert.ToString(e.CommandArgument);
                    objEntityType.EntityTypeID = Convert.ToInt32(id);
                    DataSet ds = objEntityType.SelectById();
                    bool action = true;
                    action = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0]["Active"]));
                    if (action)
                        objEntityType.Active = false;
                    else
                        objEntityType.Active = true;

                    bool result = objEntityType.ToggleActivation();
                    if (result)
                    {
                        if (objEntityType.Active)
                            ucMessage1.ShowSuccess(Resources.Messages.ToggleSuccess);
                        else
                            ucMessage1.ShowSuccess(Resources.Messages.ToggleSuccess);
                        Cleardata();
                    }

                    BindListSearch(Convert.ToInt32(hdfCurrentPage.Value));
                    updpnlgrdvList.Update();
                }
            }
            updpnlSaveData.Update();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();
            
        }
    }

    protected void grdvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                using (clsEntityTypeMaster objEntityTypeMaster = new clsEntityTypeMaster())
                {

                    ImageButton objDeleteConfirm = (ImageButton)e.Row.FindControl("imgbtnDelete");
                    if (objDeleteConfirm != null)
                    {
                        objDeleteConfirm.Attributes.Add("Onclick", "if(!confirm('Are you sure you want to delete this record?')){return false;}");
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowWarning(ex.Message);
                updpnlSaveData.Update();
                
            }
        }
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        try
        {
            ucMessage1.Visible = false;
            hdfCurrentPage.Value = Convert.ToString(ucPagingControl1.CurrentPage);
            updpnlgrdvList.Update();
            
            BindListSearch(ucPagingControl1.CurrentPage);
            
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
            using (clsEntityTypeMaster ObjEntityType = new clsEntityTypeMaster())
            {
                
                ObjEntityType.EntityType = txtSearchEntityType.Text.Trim();
                ObjEntityType.EntityTypeID = Convert.ToInt16(ddlsearchBaseEntityType.SelectedValue) == 100 ? 0 : Convert.ToInt16(ddlsearchBaseEntityType.SelectedValue);
                ObjEntityType.PageSize =Convert.ToInt32(PageBase.PageSize);
                ObjEntityType.PageIndex = -1;
                ObjEntityType.ActiveStatus = (ddlActive.SelectedValue == "233" || ddlActive.SelectedValue == "255") ? Convert.ToInt16(255) : Convert.ToInt16(ddlActive.SelectedValue);
                ObjEntityType.CompanyId = PageBase.ClientId;
                DataSet ds = ObjEntityType.SelectAllEntity();
                
                //ds.Merge(dt);
                //PageBase.ExportToExecl(ds, "EntityTypeDetail");  
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "EntityType";
                PageBase.RootFilePath = FilePath;
                string[] strExcelSheetName = { "EntityType","EntityRelation","UserRole" };
                ChangedExcelSheetNames(ref ds, strExcelSheetName, 3);
                ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport, 3, strExcelSheetName);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            updpnlSaveData.Update();    
        }
    }
    protected void btnCreateRole_Click(object sender, EventArgs e)
    {
        DataTable DtRole = (DataTable)ViewState["DtRole"];
        if(btnCreateRole.Text == "Update")
        {
            Int32 RoleID=Convert.ToInt32( ViewState["EditRoleID"]);
            DataRow[] dr = DtRole.Select("RoleId=" + RoleID.ToString());
            if(dr.Length>0)
            {
                dr[0]["RoleName"] = Server.HtmlEncode( txtRole.Text.Trim());
                DtRole.AcceptChanges();
                
            }
        }
        else
        {
            DataRow dr = DtRole.NewRow();
            dr["RoleId"] = 0;
            dr["RoleName"] = Server.HtmlEncode(txtRole.Text.Trim());
            dr["Status"] = 1;
            DtRole.Rows.Add(dr);
            DtRole.AcceptChanges();
        }
        grdvwRoleList.DataSource = DtRole;
        grdvwRoleList.DataBind();
        ViewState["DtRole"] = DtRole;

    }
    protected void btnEditRole_Click(object sender, ImageClickEventArgs e)
    {
        //chkActive.Enabled = true;
        ImageButton btnEditRole = (ImageButton)sender;
        DataTable dtUser;
        using (UserData objuser = new UserData())
        {
            objuser.CompanyID = PageBase.ClientId;
            dtUser = objuser.GetUserRole(Convert.ToInt32(btnEditRole.CommandArgument));
        };
        ViewState["EditRoleID"] = Convert.ToInt32(btnEditRole.CommandArgument);
        if (dtUser != null && dtUser.Rows.Count > 0)
        {
            
            txtRole.Text = (dtUser.Rows[0]["RoleName"].ToString());
            //chkActive.Checked = Convert.ToBoolean(dtUser.Rows[0]["Status"]);
            //chkWAPAccess.Checked = Convert.ToString(dtUser.Rows[0]["HasWAPAccess"]) == "Yes" ? true : false;
            btnCreateRole.Text = "Update";

        }
    }
    protected void btnActiveDeactive_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btnActiveDeactive = (ImageButton)sender;
            Int32 Result = 0;
            Int16 RoleID = Convert.ToInt16(btnActiveDeactive.CommandArgument);
            using (UserData ObjUser = new UserData())
            {

                ObjUser.UserRoleID = RoleID;
                Result = ObjUser.UpdateStatusRoleInfo();
                if (ObjUser.ErrorMessage != null && ObjUser.ErrorMessage != "")
                {
                    ucMessage1.ShowInfo(ObjUser.ErrorMessage);
                    UpdateMessage.Update();
                    return;
                }

            };
            if (Result == 1)
            {

                ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
            //FillRoleGrid();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        
    }
    protected void grdvwRoleList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Int32 CheckResult = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Int16 RoleID = Convert.ToInt16(grdvwRoleList.DataKeys[e.Row.RowIndex].Value);
            using (UserData ObjUser = new UserData())
            {
                ObjUser.UserRoleID = RoleID;
                CheckResult = ObjUser.CheckRoleExistence();
            };
            GridViewRow GVR = e.Row;
            ImageButton btnStatus = (ImageButton)GVR.FindControl("btnActiveDeactive");
            if (CheckResult > 0)
            {
                if (btnStatus != null)
                {
                    btnStatus.Attributes.Add("Onclick", "javascript:alert('Users already map with this role.You can not deactivate it.');{return false;}");

                }

            }
        }

    }
    public static string GetFormattedTime(object longTime)
    {
        string strformattedTime = string.Empty;
        try
        {
            if (longTime != null && longTime != DBNull.Value)
            {
                strformattedTime = (new DateTime(((TimeSpan)longTime).Ticks).ToString("hh:mm tt"));
            }
        }
        catch { }
        return strformattedTime;
    }

}