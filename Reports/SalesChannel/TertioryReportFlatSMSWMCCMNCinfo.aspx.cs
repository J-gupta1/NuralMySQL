using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;



using System.Data.SqlTypes;



/*Change Log:
 * 08-May-2014, Rakesh Goel, #CC01 - Changed export to excel call to EPP method instead of OpenXML.
 * Also added exception display in catch block
 * 27-May-2015,Karam Chand Sharma, #CC02, Export in csv format because old code not working
 * 22-Dec-2015,Karam Chand Sharma, #CC03, check Contains(",") || output.Contains("\"") || output.Contains("\n") || output.Contains("\r") in export to excel . If these character is coming then data convert into string because it break the data in export
 * 08-June-2016,Karam Chand Sharma, #CC04, Add model and sku in search filter and pass into function
 * 10-Sep-2018, Balram Jha, #CC05, CSV generation comented and exported XLSX file
 * 15-Nov-2018, Balram Jha, #CC06, #31 Days check added for pulling activation data
 */

public partial class Reports_SalesChannel_TertioryReportFlatSMSWMCCMNCinfo : PageBase
{
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        if (!IsPostBack)
        {
            // ucDateFrom.Date = PageBase.Fromdate;
            //ucDateTo.Date = PageBase.ToDate;
            // ddlSMSDateType.Items.Insert(0, new ListItem("Select", "0"));

            /*#CC04 START ADDED*/
            BindModel();
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
            /*#CC04 END ADDED*/
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
            /*#CC04 COMMENTED START ucMessage1.ShowInfo("Please Select any searching parameter."); #CC04 COMMENTED END */
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
            //#CC06 add start
            TimeSpan duration = Convert.ToDateTime(ucDateTo.Date) - Convert.ToDateTime(ucDateFrom.Date);
            double daysBetweenDates = duration.TotalDays;
            if (daysBetweenDates > 31)
            {
                ucMessage1.ShowInfo("Date range cant exceed 31 days.");
                return false;
            }
            //#CC06 add end
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
                if (ucDateFrom.Date != "" && ucDateTo.Date != "")
                {
                    if (Convert.ToInt16(ddlDateType.SelectedValue) == 0)
                    {
                        ucMessage1.ShowError("Please enter date type.");
                        return;
                    }
                }
                using (ReportData obj = new ReportData())
                {
                    obj.FromDate = ucDateFrom.Date;
                    obj.ToDate = ucDateTo.Date;
                    obj.UserId = PageBase.UserId;
                    obj.SalesChannelID = PageBase.SalesChanelID;
                    obj.SMSDateType = Convert.ToInt16(ddlDateType.SelectedValue);
                    obj.dtSerialNumber = SerialNumberList();
                    /*#CC04 START ADDED*/
                    obj.ModelId = Convert.ToInt16(ddlModelName.SelectedValue);
                    obj.SkuId = Convert.ToInt16(ddlSku.SelectedValue);
                    /*#CC04 END ADDED*/
                    dt = obj.GetSMSSystemPISTertiory(); //#CC05 comented
                                        
                    if (dt !=null)
                    {
                        if (dt.Rows.Count == 0)
                        {
                            ucMessage1.ShowError(Resources.Messages.NoRecord);
                            return;
                        }
                        //String FilePath = Server.MapPath("../../");/*#CC02 Cmmented*/
                        string FilenameToexport = "TertioryReportSMS"+ DateTime.Now.Ticks.ToString();
                        //PageBase.RootFilePath = FilePath;/*#CC02 Cmmented*/
                        //PageBase.ExportToExeclUsingOPENXMLV2(dt, FilenameToexport);  //#CC01 commented

                        //PageBase.ExportToExecl(dt.DataSet, FilenameToexport);  //#CC01 added/*#CC02 Cmmented*/


                        /*#CC02 START ADDED*/
                        /*Response.ContentType = "Application/x-msexcel";#CC03 COMMENTED*/
                        //#CC05 coment start
                        //Response.ContentType = "Application/CSV";/*#CC03 ADDED*/
                        //Response.AddHeader("content-disposition", "attachment;filename=" + FilenameToexport + ".csv");
                        //Response.Write(ExportToCSVFile(dt));
                        //#CC05 coment end
                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dt.DataSet, FilenameToexport);
                        //PageBase.ExportToExeclV2(dt.DataSet, FilenameToexport);//#CC05 added
                        Response.End();
                        /*#CC02 START END*/
                        //fncDownloadtxt(dt, FilenameToexport);/*#CC02 Cmmented*/
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.NoRecord);

                    }
                    updsearch.Update();
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());  //#CC01 added
            PageBase.Errorhandling(ex);  //#CC01 added
        }
    }
    /*#CC02 START ADDED*/
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

        //string ErrorSerialNumber = PageBase.CheckSerialNo(Serialnumber);
        //if (ErrorSerialNumber != "")
        //{
        //    if (ErrorSerialNumber.Replace(",", "").Trim() == string.Empty)
        //    {
        //        ucMessage1.ShowError("Blank SerialNumber is not allowed");
        //        return dt;
        //    }
        //    ucMessage1.ShowError(ErrorSerialNumber + " " + "Invalid SerialNumber");
        //    return dt;
        //}
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
