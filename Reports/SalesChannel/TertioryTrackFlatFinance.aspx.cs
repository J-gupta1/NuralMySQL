#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 26 Oct 2018
 * Description : Download Tertiary Track Report Based on Finance Date and Primary Sale Date 
 * ================================================================================================
 * Change Log: 
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ====================================================================================================
 */
#endregion

using BussinessLogic;
using DataAccess;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;


public partial class Reports_SalesChannel_TertioryTrackFlatFinance : PageBase
{
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        if (!IsPostBack)
        {
            BindModel();
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
          
        }
    }


    bool isvalidate()
    {

        if (ucDateFrom.Date != "" && ucDateTo.Date == "")
        {

            ucMessage1.ShowInfo("Invalid date range");
            return false;
        }
        if (ucDateFrom.Date == "" && ucDateTo.Date != "")
        {
            ucMessage1.ShowInfo("Invalid date range");
            return false;
        }
        if (ucDateFrom.Date == "" && ucDateTo.Date == "" && txtSerialNumber.Text.Trim() == string.Empty)
        {
            ucMessage1.ShowInfo("Please Select atleast one search parameter from date or IMEI.");/*#CC04 ADDED */
            return false;
        }

        if (ucDateFrom.Date != "" && ucDateTo.Date != "")
        {
            if (Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
            {
                ucMessage1.ShowInfo("The date from cant exceed the to  date");
                return false;
            }
        }
        //if (ddlSMSDateType.SelectedValue == "0" && (ucDateFrom.Date != "" || ucDateTo.Date != ""))
        //{
        //    ucMessage1.ShowInfo("Please Select valid from and To Date");
        //    return false;
        //}
        //if (ddlSMSDateType.SelectedValue != "0" && (ucDateFrom.Date == "" || ucDateTo.Date == ""))
        //{
        //    ucMessage1.ShowInfo("Please Select valid from and To Date");
        //    return false;
        //}

        return true;
    }

    void blankall()
    {

        ucDateFrom.Date = "";
        ucDateTo.Date = "";
        /*#CC04 START ADDED*/
        BindModel();
        ddlModelName_SelectedIndexChanged(null, null);
        ddlDateType.SelectedIndex = 0;
        /*#CC04 END ADDED*/
        txtSerialNumber.Text = string.Empty;

        ucMessage1.Visible = false;
    }

    protected void btnSerch_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtIMEI = new DataTable();
            if (isvalidate())
            {
                //if (ucDateFrom.Date != "" && ucDateTo.Date != "")
                //{
                //    if (Convert.ToInt16(ddlDateType.SelectedValue) == 0)
                //    {
                //        ucMessage1.ShowError("Please enter date type.");
                //        return;
                //    }
                //}
                using (ReportData obj = new ReportData())
                {
                    obj.FromDate = ucDateFrom.Date;
                    obj.ToDate = ucDateTo.Date;
                    obj.UserId = PageBase.UserId;
                    obj.SalesChannelID = PageBase.SalesChanelID;
                    obj.SMSDateType = Convert.ToInt16(ddlDateType.SelectedValue);
                    obj.dtSerialNumber = SerialNumberList();
                    obj.ModelId = Convert.ToInt16(ddlModelName.SelectedValue);
                    obj.SkuId = Convert.ToInt16(ddlSku.SelectedValue);
                    dt = obj.GetSMSSystemPISTertioryFIN(); //#CC05 comented


                    if (dt != null)
                    {
                        if (dt.Rows.Count == 0)
                        {
                            ucMessage1.ShowError(Resources.Messages.NoRecord);
                            return;
                        }
                        //String FilePath = Server.MapPath("../../");/*#CC02 Cmmented*/
                        string FilenameToexport = "TertioryReportFIN";
                     
                        PageBase.ExportToExecl(dt.DataSet, FilenameToexport);//#CC05 added
                        Response.End();
                   
                    }
                    else
                    {
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);

                    }
                    updsearch.Update();
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());  
            PageBase.Errorhandling(ex);  
        }
    }
 
    private string ExportToCSVFile(DataTable dtTable)
    {
        StringBuilder sbldr = new StringBuilder();
        if (dtTable.Columns.Count != 0)
        {
            foreach (DataColumn col in dtTable.Columns)
            {
                sbldr.Append(col.ColumnName + ',');
            }
            sbldr.Append("\r\n");
            foreach (DataRow row in dtTable.Rows)
            {
                /*#CC03 COMMENTED sbldr.Append(row[column].ToString() + ',');*/
                /*#CC03 START ADDED*/
                foreach (DataColumn column in dtTable.Columns)
                {
                    var output = row[column].ToString();
                    if (output.Contains(",") || output.Contains("\"") || output.Contains("\n") || output.Contains("\r"))
                    {
                        output = '"' + output.Replace("\"", "\"\"") + '"' + ',';
                        sbldr.Append(output);
                    }
                    else
                        sbldr.Append(row[column].ToString() + ',');
                }
                /*#CC03 START END*/
                sbldr.Append("\r\n");
            }
        }
        return sbldr.ToString();
    }

    /*#CC02 START END*/
    protected void btncancel_Click(object sender, EventArgs e)
    {
        blankall();


    }
    DataTable SerialNumberList()
    {
        string Serialnumber = txtSerialNumber.Text.Trim();
        DataTable dt = new DataTable();
        dt.Columns.Add("SN");
        dt.AcceptChanges();

     
        Serialnumber = Serialnumber.Replace("\r\n", ",");
        string[] strSplitArray = Serialnumber.Split(',');
        foreach (var obj in strSplitArray.Distinct())
            dt.Rows.Add(obj.Trim().ToString());
        return dt;
    }



    protected void ddlSMSDateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlSMSDateType.SelectedValue != "0")
        //{
        //    txtSerialNumber.Enabled = false;
        //    txtSerialNumber.Text = string.Empty;
        //    ucDateTo.TextBoxDate.Enabled = true;
        //    ucDateTo.imgCal.Enabled = true;
        //    ucDateFrom.TextBoxDate.Enabled = true;
        //    ucDateFrom.imgCal.Enabled = true;
        //}
        //else
        //{
        //    ucDateFrom.Date = string.Empty;
        //    ucDateTo.Date = string.Empty;
        //    txtSerialNumber.Enabled = true;
        //    txtSerialNumber.Text = string.Empty;
        //    ucDateTo.TextBoxDate.Enabled = false;
        //    ucDateTo.imgCal.Enabled = false;
        //    ucDateFrom.TextBoxDate.Enabled = false;
        //    ucDateFrom.imgCal.Enabled = false;
        //}

    }

    protected void ddlModelName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlModelName.SelectedValue == "0")
            {
                ddlSku.Items.Clear();
                ddlSku.Items.Insert(0, new ListItem("Select", "0"));
                ddlSku.SelectedValue = "0";
            }
            else
            {

                BindSku();
            }

        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
        }
    }
    void BindSku()
    {
        using (RetailerData objsku = new RetailerData())
        {
            objsku.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
            DataTable dtmodelfil = objsku.GetAllActiveSKU();
            String[] colArray1 = { "Skuid", "SkuName" };
            PageBase.DropdownBinding(ref ddlSku, dtmodelfil, colArray1);


        }
    }
    void BindModel()
    {
        using (ProductData objproduct = new ProductData())
        {
            objproduct.ModelProdCatId = 0;
            objproduct.ModelSelectionMode = 1;
            DataTable dtmodelfil = objproduct.SelectModelInfo();
            String[] colArray1 = { "ModelID", "ModelName" };
            PageBase.DropdownBinding(ref ddlModelName, dtmodelfil, colArray1);


        }
    }
}
