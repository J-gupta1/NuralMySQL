using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using Cryptography;
using System.Data;

public partial class Transactions_POC_ManageRetailerIMEIWap : System.Web.UI.Page
{
    int RetailerId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                FillRetailer();
                FillModel();
            }
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);

        }
    }
    void FillRetailer()
    {
        using (RetailerData ObjRetaileType = new RetailerData())
        {
            ObjRetaileType.value = 1;
            String[] StrCol = new String[] { "RetailerID", "RetailerName" };
            PageBase.DropdownBinding(ref ddlRetailer, ObjRetaileType.GetAllRetailer(), StrCol);

        };
    }


    void FillModel()
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = 1; //dummy
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrice;
                objSalesData.SalesChannelID = 0; //Dummy
                objSalesData.Brand = 0; //Dummy            //Pankaj Dhingra
                dsReferenceCode = objSalesData.GetAllTemplateData();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {
                    String[] StrCol = new String[] { "skuid", "skuname" };
                    PageBase.DropdownBinding(ref ddlModel, dsReferenceCode.Tables[0], StrCol);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

  
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int Result = 0;
            using (RetailerData objRetailer = new RetailerData())
            {
                objRetailer.RetailerID = Convert.ToInt32(ddlRetailer.SelectedValue);
                objRetailer.SkuID = Convert.ToInt32(ddlModel.SelectedValue);
                objRetailer.SaleDate = Convert.ToDateTime(ucDatePicker.Date);
                objRetailer.IMEINo = Convert.ToString(txtIMEI.Text);
                Result = objRetailer.InsertRetailerIMEIInfo();
                if (objRetailer.Error == null)
                {
                    ucMessage1.ShowInfo("Tertiary Sales Recorded Successfully");
                    ClearForm();
                }
                else
                {
                    ucMessage1.ShowInfo(objRetailer.Error.ToString());
                }
               
            };
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void ClearForm()
    {

        ddlModel.SelectedValue = "0";
        ddlRetailer.SelectedValue = "0";
        txtIMEI.Text = "";
        ucDatePicker.Date = "";
     



    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();  
    }
   


}
