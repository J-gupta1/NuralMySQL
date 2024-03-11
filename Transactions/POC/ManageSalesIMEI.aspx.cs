using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_POC_ManageSalesIMEI : PageBase
{
    DateTime dt;
    char[] separator = new char[] { ',' };
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            dt = System.DateTime.Now.Date;      //Pankaj Kumar

            ucDatePicker.MaxRangeValue = dt;
            ucDatePicker.RangeErrorMessage = "Invalid Date.";

            //lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                FillModel();
                FillRetailer();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }

    private DataTable CreateBlankTable()
    {
        DataTable dt;
        dt = new DataTable();
        //DataBase TVP Name:tvpSalesReturnInvoice
        DataColumn dc = new DataColumn("InvoiceNumber");
        dc.DataType = System.Type.GetType("System.String");
        dt.Columns.Add(dc);

        dc = new DataColumn("RetailerID");
        dc.DataType = System.Type.GetType("System.Int64");
        dt.Columns.Add(dc);

        dc = new DataColumn("RetailerName");
        dc.DataType = System.Type.GetType("System.String");
        dt.Columns.Add(dc);

        dc = new DataColumn("ModelID");
        dc.DataType = System.Type.GetType("System.Int64");
        dt.Columns.Add(dc);

        dc = new DataColumn("ModelName");
        dc.DataType = System.Type.GetType("System.String");
        dt.Columns.Add(dc);

        dc = new DataColumn("Quantity");
        dc.DataType = System.Type.GetType("System.Int16");
        dt.Columns.Add(dc);

        dc = new DataColumn("SalesDate");
        dc.DataType = System.Type.GetType("System.DateTime");
        dt.Columns.Add(dc);

        dc = new DataColumn("IMEI");
        dc.DataType = System.Type.GetType("System.String");
        dt.Columns.Add(dc);

        return dt;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //PnlHide.Visible = true;
            DataTable dtNew = new DataTable();
            string[] strSerialno = txtSerialNumber.Text.Split(separator);
            if(strSerialno.Length!=Convert.ToInt32(txtQuantity.Text))
            {
                ucMsg.ShowInfo("Serial Number count not according to the Quantity");
                return;
            }
            if (ViewState["dtReturnSerial"] == null)
            {
                dtNew = CreateBlankTable();
            }
            if (ViewState["dtReturnSerial"] != null)
            {
                dtNew = (DataTable)ViewState["dtReturnSerial"];
            }
            DataRow drReturn = null;
            if (ViewState["dtReturnSerial"] != null)
            {
                //string[] strSerialno = txtSerialNumber.Text.Split(separator);
                //foreach (var strSerial in strSerialno)
                //{
                    //DataRow drReturn1 = null;
              drReturn = dtNew.NewRow();
                    drReturn["InvoiceNumber"] = txtInvoiceNumber.Text;
                    drReturn["RetailerID"] = Convert.ToInt32(ddlRetailer.SelectedValue);
                    drReturn["RetailerName"] = Convert.ToString(ddlRetailer.SelectedItem);
                    drReturn["ModelID"] = Convert.ToInt32(ddlModel.SelectedValue);
                    drReturn["ModelName"] = Convert.ToString(ddlModel.SelectedItem);
                    drReturn["Quantity"] = Convert.ToInt32(txtQuantity.Text);
                    drReturn["SalesDate"] = Convert.ToDateTime(ucDatePicker.Date).ToString("dd-MMM-yyyy");
                    drReturn["IMEI"] = Convert.ToString(txtSerialNumber.Text);
                    dtNew.Rows.Add(drReturn);
                    dtNew.AcceptChanges();
                

            }
            else
            {
               // string[] strSerialno = txtSerialNumber.Text.Split(separator);
                    //DataRow drReturn1 = null;
                    drReturn = dtNew.NewRow();
                    drReturn["InvoiceNumber"] = txtInvoiceNumber.Text;
                    drReturn["RetailerID"] = Convert.ToInt32(ddlRetailer.SelectedValue);
                    drReturn["RetailerName"] = Convert.ToString(ddlRetailer.SelectedItem);
                    drReturn["ModelID"] = Convert.ToInt32(ddlModel.SelectedValue);
                    drReturn["ModelName"] = Convert.ToString(ddlModel.SelectedItem);
                    drReturn["Quantity"] = Convert.ToInt32(txtQuantity.Text);
                    drReturn["SalesDate"] = Convert.ToDateTime(ucDatePicker.Date).ToString("dd-MMM-yyyy");
                    drReturn["IMEI"] = Convert.ToString(txtSerialNumber.Text);
                    dtNew.Rows.Add(drReturn);
                    dtNew.AcceptChanges();
                //}
            }
            grdvList.DataSource = dtNew;
            grdvList.DataBind();
            ClearForm();
            pnlGrid.Visible = true;
            updGrid.Update();
            ViewState["dtReturnSerial"] = null;
            ViewState["dtReturnSerial"] = dtNew;
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    bool pageValidateSave()
    {

        //if (ddlRetailer.SelectedIndex == 0 || ServerValidation.IsDate(ucDatePicker.Date, true) != 0 || ddlSalesman.SelectedValue == "0")
        //{
        //    ucMsg.ShowWarning(Resources.Messages.MandatoryField);
        //    return false;
        //}

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }


        return true;
    }
   
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
       
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }//Pankaj Kumar
            int Result = 0;
            //if (!pageValidateSave())
            //{
            //    return;
            //}
            DataTable DtDetail = new DataTable();
            DataTable Dt = CreateBlankTable();
            if (ViewState["dtReturnSerial"] != null)
            {
                DtDetail = (DataTable)ViewState["dtReturnSerial"];
            }
            string SumQTY = DtDetail.Compute("sum(Quantity)", "1=1").ToString();

            if (SumQTY == "0")
            {
                ucMsg.ShowInfo(Resources.Messages.Entersalesqty);
                return;
            }


            if (DtDetail.Rows.Count > 0)
            {
                DataRow drReturn = null;
                //foreach (var strSerial in strSerialno)
                foreach (DataRow dr in DtDetail.Rows)
                {
                    string[] strSerialno = Convert.ToString(dr["IMEI"]).Split(separator);
                    foreach (var strSerial in strSerialno)
                    {
                        drReturn = Dt.NewRow();
                        drReturn["InvoiceNumber"] = dr["InvoiceNumber"].ToString();
                        drReturn["RetailerID"] = Convert.ToInt32(dr["RetailerID"]);
                        drReturn["RetailerName"] = dr["RetailerName"];
                        drReturn["ModelID"] = dr["ModelID"];
                        drReturn["ModelName"] = dr["ModelName"];
                        drReturn["Quantity"] = dr["Quantity"];
                        drReturn["SalesDate"] = dr["SalesDate"];
                        drReturn["IMEI"] = strSerial;
                        Dt.Rows.Add(drReturn);
                    }
                }
            }

            Dt.AcceptChanges();
            using (SalesData ObjSales = new SalesData())
            {

                ObjSales.Error = "";
                ObjSales.UserID = PageBase.UserId;
                ObjSales.SalesChannelID = PageBase.SalesChanelID;
                Result = ObjSales.InsertSecondarySalesIMEI(Dt);

                if (ObjSales.ErrorDetailXML != null && ObjSales.ErrorDetailXML != string.Empty)
                {
                    ucMsg.XmlErrorSource = ObjSales.ErrorDetailXML;
                    return;
                }
                else if (ObjSales.Error != "" )
                {
                    ucMsg.ShowError(ObjSales.Error);
                    return;
                }

                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                ClearForm();

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void ClearForm()
    {
        ddlRetailer.SelectedValue = "0";
        ddlModel.SelectedValue = "0";
        ucDatePicker.imgCal.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;
        ucDatePicker.TextBoxDate.Text = "";
        txtInvoiceNumber.Text = "";
        txtQuantity.Text = "";
        txtSerialNumber.Text = "";
        pnlGrid.Visible = false;
        updGrid.Update();
        ViewState["dtReturnSerial"] = null;
        //PnlHide.Visible = false;
    }

    protected void ddlRetailer_SelectedIndexChanged(object sender, EventArgs e)
    {

        pnlGrid.Visible = false;

        updGrid.Update();
    }
    protected void ddlSalesman_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));

        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.SalesChannelID = PageBase.SalesChanelID;
          //  ObjRetailer.SalesmanID = Convert.ToInt32(ddlSalesman.SelectedValue);
            ObjRetailer.Type = 1;               //For 1 because we are going to get only active Retailers
            string[] str = { "RetailerCode", "Retailer" };
            PageBase.DropdownBinding(ref ddlRetailer, ObjRetailer.GetRetailerInfo(), str);
        };


        pnlGrid.Visible = false;

        updGrid.Update();
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
}
