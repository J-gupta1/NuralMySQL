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
using System.Windows.Forms;
using System.Globalization;
using System.Collections;

public partial class Reports_Common_UploadSchemaReport : PageBase
{
    Dictionary<string, string> names = new Dictionary<string, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillTableName();
        }
    }

    public void fillTableName()
    {
       
        names.Add("0", "Select");
        names.Add("Retailer", "Upload Retailer");
        names.Add("Price", "Manage Price");
        names.Add("IntermediarySales", "Upload Primary Sales-2");
        names.Add("PrimarySales", "Upload Primary Sales");
        names.Add("SecondarySalesReturn", "Upload Secondary Sales");
        names.Add("PrimarySalesReturn", "Upload Primary Sales Return");
        names.Add("Warehouse Transaction(Sap-BTM)", "SAP-BTM");
        names.Add("SecondarySalesUpload", "Upload Secondary Sales");
        names.Add("Primary Sales(Sap-MOD)", "SAP-Primary Sales");
        names.Add("IMEI(Sap-IMEI)", "SAP-IMEI");
        names.Add("GRN(Sap-GRN)", "SAP-GRN");
        names.Add("GRN", "GRN");
        names.Add("SalesMan", "Upload SalesMan");
        names.Add("CustomPrice", "Manage Price");
        cmbTableName.DataSource = new BindingSource(names, null);
        cmbTableName.DataValueField = "Key";
        cmbTableName.DataTextField = "Value";
        cmbTableName.DataBind();
        cmbTableName.SelectedValue = "0";
    }

   public void importdata()
    {
        using (ReportData obj = new ReportData())
        {
            obj.TableName = cmbTableName.SelectedValue;
            if (PageBase.Client == "B")
            {
                obj.CompanyType = 1;

            }
            else
            {
                obj.CompanyType = 2;
            }
            DataTable dt = obj.GetUploadSchemaTable();
            grdUploadSchema.DataSource = dt;
             grdUploadSchema.DataBind();
            updgrid.Update();
            pnlGrid.Visible = true;

        }
    }

     protected void btnSerch_Click(object sender, EventArgs e)
    {
        importdata();
    }

    protected void grdUploadSchema_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdUploadSchema.PageIndex = e.NewPageIndex;
        importdata();
    }


    protected void cmbTableName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbTableName.SelectedValue == "0")
        {
            pnlGrid.Visible = false;
        }
        
    }
}
