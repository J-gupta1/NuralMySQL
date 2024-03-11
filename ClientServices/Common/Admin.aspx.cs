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
using System.Net;
using System.Web.Services;
using System.Collections;
using System.Web.Script.Serialization;

/* ===============================================================================
 * Change Log
 * ===============================================================================
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 06-Jul-2016, Sumit Maurya, #CC01, New Webmethod created to get Pricelist Detail. 
 
 */

public partial class ClientServices_Common_Admin : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod()]
    public static object GetUploadTypeData(int PageIndex, int PageSize, int UploadType, string FromDate, string ToDate, string ISPCode)
    {

        string strError = string.Empty, strPagesize = PageSize.ToString(), strPageIndex = PageIndex.ToString(), strTotalCount = string.Empty, url = string.Empty;
        DataTable dt = new DataTable();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;
        JavaScriptSerializer json = new JavaScriptSerializer();
        try
        {
            using (DataAccess.clsUploadDocs obj = new DataAccess.clsUploadDocs())
            {
                if (FromDate != "")
                    obj.DateFrom = Convert.ToDateTime(FromDate);
                if (ToDate != "")
                    obj.DateTo = Convert.ToDateTime(ToDate);
                obj.ISPCode = (ISPCode == "undefined" ? "0" : ISPCode);
                obj.UploadTypeId = UploadType;
                obj.PageIndex = PageIndex;
                obj.PageSize = PageSize;
                obj.UserId = PageBase.UserId;
                dt = obj.GetUploadTypeData();
                strTotalCount = obj.TotalCount.ToString();
                strError = obj.Error;

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
            }
        }
        catch (Exception ex)
        {
        }
        url = ConfigurationManager.AppSettings["siteurl"] + "\\Excel\\Upload\\RSPUpload\\";
        return new { Records = rows, Error = strError, RTotal = strTotalCount, RPageSize = strPagesize, RPageIndex = strPageIndex, RUrl = url };
    }
    [WebMethod()]
    public void GetDownloadImageFromURL(string ImageURL)
    {
        WebClient wc = new WebClient();
        string fileName = Path.GetFileName(ImageURL);
        byte[] data = wc.DownloadData(ImageURL);
        HttpContext.Current.Response.ContentType = "application/octet-stream";
        Response.OutputStream.Write(data, 0, data.Length);
        Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.End();
    }
    /* #CC01 Add Start */
    [WebMethod()]
    public static object GetPriceList(int PageIndex, int PageSize, int PriceList, int SKUCode, string FromDate, string ToDate)
    {

        string strError = string.Empty, strPagesize = PageSize.ToString(), strPageIndex = PageIndex.ToString(), strTotalCount = string.Empty, url = string.Empty;
        DataTable dt = new DataTable();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;
        JavaScriptSerializer json = new JavaScriptSerializer();
        try
        {
            using (ProductData obj = new ProductData())
            {

                obj.PriceListID = PriceList;
                obj.SKUId = SKUCode;
                obj.PageIndex = PageIndex;
                obj.PageSize = PageSize;
                if (FromDate != "")
                {
                    obj.FromDate = Convert.ToDateTime(FromDate);
                }
                if (ToDate != "")
                {
                    obj.ToDate = Convert.ToDateTime(ToDate);
                }
                if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
                    obj.Condition = 1;
                if ( PageBase.SalesChanelID == 0)
                    obj.Condition = 0;
                if ( PageBase.SalesChanelID > 0)
                    obj.Condition = 2;
                obj.UserId = PageBase.UserId;
                dt = obj.GetPriceInfoV3();
                strTotalCount = obj.TotalCount.ToString();
                strError = obj.error;
               
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

            }
        }
        catch (Exception ex)
        {
        }
        url = ConfigurationManager.AppSettings["siteurl"] + "\\Excel\\Upload\\RSPUpload\\";
        return new { Records = rows, Error = strError, RTotal = strTotalCount, RPageSize = strPagesize, RPageIndex = strPageIndex, RUrl = url };
    }
    /* #CC01 Add End */

}
