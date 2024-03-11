#region NameSpace
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

#endregion
/*Change Log:
 * 08-May-2014, Rakesh Goel, #CC01 - Changed export to excel call to EPP method instead of OpenXML.
 * Also added exception display in catch block
 * 
 */
public partial class Reports_SalesChannel_TertioryReportFlatSMS : PageBase
{
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        if (!IsPostBack)
        {
            BindModalName();
           // ucDateFrom.Date = PageBase.Fromdate;
            //ucDateTo.Date = PageBase.ToDate;
           // ddlSMSDateType.Items.Insert(0,new ListItem("Select", "0"));
        }
    }


    bool isvalidate()
    {

        if (ucDateFrom.Date != "" && ucDateTo.Date == "")
        {

            ucMessage1.ShowInfo("Invalid Date Range");
            return false;
        }
        if (ucDateFrom.Date == "" && ucDateTo.Date != "")
        {
            ucMessage1.ShowInfo("Invalid Date Range");
            return false;
        }
        if (ucDateFrom.Date == "" && ucDateTo.Date == "" && txtSerialNumber.Text.Trim() == string.Empty )
        {
            ucMessage1.ShowInfo("Please Select any searching parameter.");
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
        ddlSMSDateType.SelectedValue = "0";
        txtSerialNumber.Text = string.Empty;
        ucMessage1.Visible = false;
        
    }

    protected void btnCount_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            if (isvalidate())
            {
                dt = GetData(1);
                ucMessage1.ShowInfo("Found Record :" + dt.Rows[0][0].ToString());
            }
        }
        catch (Exception ex)
        {

        }

    }

    protected void btnSerch_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            DataTable dtIMEI = new DataTable();
            if (isvalidate())
            {
                //using (ReportData obj = new ReportData())
                //{
                //    obj.FromDate = ucDateFrom.Date;
                //    obj.ToDate = ucDateTo.Date;
                //    obj.UserId = PageBase.UserId;
                //    obj.SalesChannelId = PageBase.SalesChanelID;
                //    obj.SMSDateType = Convert.ToInt16(ddlSMSDateType.SelectedValue);
                //    obj.dtSerialNumber = SerialNumberList();
                //    obj.ForSearchOrForCount = 2;
                //    dt = obj.GetSMSTertiorySalesData();
                dt = GetData(2);
                if (dt.Rows.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "TertioryReportSMS";
                    PageBase.RootFilePath = FilePath;
                    //PageBase.ExportToExeclUsingOPENXMLV2(dt, FilenameToexport); //#CC01 commented

                    PageBase.ExportToExecl(dt.DataSet, FilenameToexport);  //#CC01 added


                }
                else
                {
                    ucMessage1.ShowError(Resources.Messages.NoRecord);

                }
                updsearch.Update();
                //}

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }


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
    DataTable GetData(int FromSearchOrCount)
    {
        //2-for Search
        //1--for count
        DataTable dtData = new DataTable();
        using (ReportData obj = new ReportData())
        {
            obj.FromDate = ucDateFrom.Date;
            obj.ToDate = ucDateTo.Date;
            obj.UserId = PageBase.UserId;
            obj.SalesChannelId = PageBase.SalesChanelID;
            obj.SMSDateType = Convert.ToInt16(ddlSMSDateType.SelectedValue);
            obj.dtSerialNumber = SerialNumberList();
            obj.ForSearchOrForCount = FromSearchOrCount;
            obj.ModelId = Convert.ToInt32(ddlModalName.SelectedValue);
            dtData = obj.GetSMSTertiorySalesData();
           
        }
        return dtData;
    }
    void BindModalName()
    {
        using (ProductData objproduct = new ProductData())
        {

            objproduct.TertiaryType = 1;
            DataTable dtmodelfil = objproduct.SelectModelList();
            String[] colArray1 = { "TertiarySMSModalId", "TertiarySMSModalName" };
            PageBase.DropdownBinding(ref ddlModalName, dtmodelfil, colArray1);
        }
    }
    protected void ddlSMSDateType_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (ddlSMSDateType.SelectedValue == "1")
        {
            tdName.Visible = true;
            tdNameid.Visible = true;
        }
        else
        {
            tdName.Visible = false;
            tdNameid.Visible = false;
        }
    }
}
