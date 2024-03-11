using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Services;
using System.Collections;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Data;
using System.Collections.Generic;
using BussinessLogic;
/*
 * 22 Jan 2015, Karam Chand Sharma, #CC01, create ValidateAllSerialsReturn function for serail no check according to the invoice no
 * 23 Apr 2015, Karam Chand Sharma, #CC02, Pass value to the properties for excat match to the proc
 * 06 Aug 2015, Karam Chand Sharma, #CC03, create some new service for bind salechannel according to sale channel type
 * 26 July 2016, Karam Chand Sharma, #CC04, Pass stock bin type to  GetStockInHandByLogin function
 * 27-Jul-2016, Sumit Maurya, #CC05, StockBinTypeID supplied to validate Serials.
 * 2-Aug-2016, Karam Chand Sharma, #CC06,in  ValidateAllSerials replace ',' instead of '', Please check bug no : 17834 for more details
 */
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CommonService : System.Web.Services.WebService
{
    // Add [WebGet] attribute to use HTTP GET
    [OperationContract]
    public void DoWork()
    {
        // Add your operation implementation here
        return;
    }

    // Add more operations here and mark them with [OperationContract]


    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetISPCodesList(string prefixText, int count, string contextKey)
    {
        string[] strISPCodes = new string[0];
        DataAccess.BeautyAdvisorData obj = new DataAccess.BeautyAdvisorData();
        try
        {
            Hashtable htCodes;
            obj.ISPName = prefixText;
            //            Int64 intproductid = Convert.ToInt64(Session["ProductID"]);
            obj.ISPID = 0;
            if (Session["LoginID"] != null)
                obj.LoginId = Convert.ToInt32(Session["LoginID"]);
            else
                obj.LoginId = 0;
            using (System.Data.DataSet dtISPName = obj.GetISPList())
            {
                int intCounter = 0;
                if (dtISPName.Tables[0].Rows.Count > 0)
                {
                    strISPCodes = new string[dtISPName.Tables[0].Rows.Count];
                    htCodes = new Hashtable(dtISPName.Tables[0].Rows.Count);
                    foreach (DataRow drCityName in dtISPName.Tables[0].Rows)
                    {
                        strISPCodes.SetValue(drCityName["ISPDisplayName"], intCounter);
                        htCodes.Add(drCityName["ISPDisplayName"].ToString().Trim().ToLower(), drCityName["ISPCode"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strISPCodes = new string[1];
                    htCodes = new Hashtable(1);
                    strISPCodes.SetValue("No Match Found", 0);
                    htCodes.Add("No Match Found", "-1");
                }
                Session["LoginID"] = null;
                return strISPCodes;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strISPCodes;
        }
        catch (Exception ex)
        {

            return strISPCodes;
        }
        finally
        {
            // Do Nothing.
        }
    }



    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetSKUListByCodesList(string prefixText, int count, string contextKey)
    {
        string[] strSKUCodes = new string[0];
        DataAccess.ProductData obj = new DataAccess.ProductData();
        try
        {
            Hashtable htCodes;
            obj.SKUCode = prefixText;
            //            Int64 intproductid = Convert.ToInt64(Session["ProductID"]);
            obj.SalesChannelID = BussinessLogic.PageBase.SalesChanelID;
            obj.BrandId = PageBase.Brand;
            obj.CompanyId = PageBase.ClientId;
            using (System.Data.DataTable dtCityName = obj.GetSKUInfoByCode())
            {
                int intCounter = 0;
                if (dtCityName.Rows.Count > 0)
                {
                    strSKUCodes = new string[dtCityName.Rows.Count];
                    htCodes = new Hashtable(dtCityName.Rows.Count);
                    foreach (DataRow drCityName in dtCityName.Rows)
                    {
                        strSKUCodes.SetValue(drCityName["SKUCode"], intCounter);
                        htCodes.Add(drCityName["SKUCode"].ToString().Trim().ToLower(), drCityName["SKUID"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strSKUCodes = new string[1];
                    htCodes = new Hashtable(1);
                    strSKUCodes.SetValue("No Match Found", 0);
                    htCodes.Add("No Match Found", "-1");
                }
                return strSKUCodes;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strSKUCodes;
        }
        catch (Exception ex)
        {

            return strSKUCodes;
        }
        finally
        {
            // Do Nothing.
        }
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetSKUNameList(string prefixText, int count, string contextKey)
    {
        string[] strSKUCodes = new string[0];
        DataAccess.ProductData obj = new DataAccess.ProductData();
        try
        {
            Hashtable htCodes;
            obj.SKUCode = "";
            obj.SKUName = prefixText;
            obj.SalesChannelID = BussinessLogic.PageBase.SalesChanelID;
            obj.BrandId = PageBase.Brand;
            obj.CompanyId = PageBase.ClientId;
            using (System.Data.DataTable dtCityName = obj.GetSKUInfoByCode())
            {
                int intCounter = 0;
                if (dtCityName.Rows.Count > 0)
                {
                    strSKUCodes = new string[dtCityName.Rows.Count];
                    htCodes = new Hashtable(dtCityName.Rows.Count);
                    foreach (DataRow drCityName in dtCityName.Rows)
                    {
                        strSKUCodes.SetValue(drCityName["SKUName"], intCounter);
                        htCodes.Add(drCityName["SKUName"].ToString().Trim().ToLower(), drCityName["SKUID"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strSKUCodes = new string[1];
                    htCodes = new Hashtable(1);
                    strSKUCodes.SetValue("No Match Found", 0);
                    htCodes.Add("No Match Found", "-1");
                }
                return strSKUCodes;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strSKUCodes;
        }
        catch (Exception ex)
        {

            return strSKUCodes;
        }
        finally
        {
            // Do Nothing.
        }
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetModelNameList(string prefixText, int count, string contextKey)
    {
        string[] strModelName = new string[0];
        DataAccess.ProductData obj = new DataAccess.ProductData();
        try
        {
            Hashtable htCodes;
            obj.ModelCode = "";
            obj.ModelName = prefixText;
            obj.SalesChannelID = BussinessLogic.PageBase.SalesChanelID;
            obj.BrandId = PageBase.Brand;
            using (System.Data.DataTable dtModelName = obj.GetModelInfoByName())
            {
                int intCounter = 0;
                if (dtModelName.Rows.Count > 0)
                {
                    strModelName = new string[dtModelName.Rows.Count];
                    htCodes = new Hashtable(dtModelName.Rows.Count);
                    foreach (DataRow drModelName in dtModelName.Rows)
                    {
                        strModelName.SetValue(drModelName["ModelName"], intCounter);
                        htCodes.Add(drModelName["ModelName"].ToString().Trim().ToLower(), drModelName["ModelID"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strModelName = new string[1];
                    htCodes = new Hashtable(1);
                    strModelName.SetValue("No Match Found", 0);
                    htCodes.Add("No Match Found", "-1");
                }
                return strModelName;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strModelName;
        }
        catch (Exception ex)
        {

            return strModelName;
        }
        finally
        {
            // Do Nothing.
        }
    }



    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetSalesChannelCodeList(string prefixText, int count, string contextKey)
    {
        //Pankaj Dhingra
        string[] strSalesChannelCode = new string[0];
        SalesChannelData obj = new SalesChannelData();
        try
        {
            if (Convert.ToInt16(contextKey) == 0)
            {
                return strSalesChannelCode;
            }
            Hashtable htCodes;
            obj.SalesChannelCode = prefixText;
            obj.SalesChannelTypeID = Convert.ToInt16(contextKey);
            obj.ComingFrom = 1;
            obj.ActiveStatus = 255;  /*#27May14 - to allow adjustment for inactive partners as well*/
            using (System.Data.DataTable dtSalesChannelCode = obj.GetSalesChannelList())
            {
                int intCounter = 0;
                if (dtSalesChannelCode.Rows.Count > 0)
                {
                    strSalesChannelCode = new string[dtSalesChannelCode.Rows.Count];
                    htCodes = new Hashtable(dtSalesChannelCode.Rows.Count);
                    foreach (DataRow drCityName in dtSalesChannelCode.Rows)
                    {
                        if (Convert.ToInt16(contextKey) != 101)
                        {
                            strSalesChannelCode.SetValue(drCityName["SalesChannelName"], intCounter);
                            htCodes.Add(drCityName["SalesChannelName"].ToString().Trim().ToLower(), drCityName["SalesChannelID"].ToString());
                            intCounter++;
                        }
                        else
                        {
                            strSalesChannelCode.SetValue(drCityName["RetailerName"], intCounter);
                            htCodes.Add(drCityName["RetailerName"].ToString().Trim().ToLower(), drCityName["RetailerID"].ToString());
                            intCounter++;
                        }
                    }
                }
                else
                {
                    strSalesChannelCode = new string[1];
                    htCodes = new Hashtable(1);
                    strSalesChannelCode.SetValue("No Match Found", 0);
                    htCodes.Add("No Match Found", "-1");
                }
                return strSalesChannelCode;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strSalesChannelCode;
        }
        catch (Exception ex)
        {

            return strSalesChannelCode;
        }
        finally
        {
            // Do Nothing.
        }
    }





    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string GetStockInHandAndIsSerializedByCode(string SKUCode, int SalesChannelID, string SalesChannelCode,int CompanyId)
    {
        string strStdCode = string.Empty;
        try
        {
            using (DataAccess.ProductData obj = new DataAccess.ProductData())
            {
                obj.SalesChannelID = Convert.ToInt32(SalesChannelID);
                obj.SKUCode = SKUCode;
                obj.SalesChannelCode = SalesChannelCode;
                obj.CompanyId = CompanyId;

                System.Data.DataTable dtCity = obj.GetSKUStockInHandAndIsSerializedByCode();
                if (dtCity.Rows.Count > 0)
                    strStdCode = Convert.ToString(dtCity.Rows[0]["Details"]);
                return strStdCode;
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string GetStockInHandAndIsSerializedByName(string SKUName, int SalesChannelID, string SalesChannelCode)
    {
        string strStdName = string.Empty;
        try
        {
            using (DataAccess.ProductData obj = new DataAccess.ProductData())
            {
                obj.SalesChannelID = Convert.ToInt32(SalesChannelID);
                obj.SKUName = SKUName;
                obj.SalesChannelCode = SalesChannelCode;

                System.Data.DataTable dtCity = obj.GetSKUStockInHandAndIsSerializedByName();
                if (dtCity.Rows.Count > 0)
                    strStdName = Convert.ToString(dtCity.Rows[0]["Details"]);
                return strStdName;
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string GetSalesQuantityInformation(string SKUCode, string InvoiceNumber, string invoiceDate, string SalesType)
    {
        string strStdCode = string.Empty;
        try
        {
            using (DataAccess.ProductData obj = new DataAccess.ProductData())
            {
                obj.SalesChannelCode = BussinessLogic.PageBase.SalesChanelCode;
                obj.SalesChannelID = BussinessLogic.PageBase.SalesChanelID;
                obj.SKUCode = SKUCode;
                obj.InvoiceNumber = InvoiceNumber;
                obj.Value = Convert.ToInt16(SalesType);
                obj.InvoiceDate = Convert.ToDateTime(invoiceDate);
                System.Data.DataTable dtCity = obj.GetSKUSalesInformation();
                if (dtCity.Rows.Count > 0)
                    strStdCode = Convert.ToString(dtCity.Rows[0]["Details"]);
                return strStdCode;
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }



    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetInvoiceNumberList(string prefixText, int count, string contextKey)
    {
        string[] strSKUCodes = new string[0];
        DataAccess.SalesData obj = new DataAccess.SalesData();
        try
        {
            Hashtable htCodes;
            obj.InvoiceNumber = prefixText;
            obj.value = Convert.ToInt32(Session["ReturnSalesType"]);
            obj.SalesChannelID = BussinessLogic.PageBase.SalesChanelID;
            obj.ReturnFromSalesChannelID = Convert.ToInt32(contextKey);

            using (System.Data.DataTable dtInvoiceNumber = obj.GetInvoiceListFull())
            {
                int intCounter = 0;
                if (dtInvoiceNumber.Rows.Count > 0)
                {
                    strSKUCodes = new string[dtInvoiceNumber.Rows.Count];
                    htCodes = new Hashtable(dtInvoiceNumber.Rows.Count);
                    foreach (DataRow drInvoiceNumber in dtInvoiceNumber.Rows)
                    {
                        strSKUCodes.SetValue(drInvoiceNumber["InvoiceNumber"], intCounter);
                        htCodes.Add(drInvoiceNumber["InvoiceNumber"].ToString().Trim().ToLower(), drInvoiceNumber["PrimarySalesID"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strSKUCodes = new string[1];
                    htCodes = new Hashtable(1);
                    strSKUCodes.SetValue("No Match Found", 0);
                    htCodes.Add("No Match Found", "-1");
                }
                return strSKUCodes;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strSKUCodes;
        }
        catch (Exception ex)
        {

            return strSKUCodes;
        }
        finally
        {
            // Do Nothing.
        }
    }



    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public System.Data.DataTable GetSerialsBySku(string SKUCode, int SalesChannelID, string SalesChannelCode)
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        string strStdCode = string.Empty;
        try
        {

            using (DataAccess.ProductData obj = new DataAccess.ProductData())
            {

                obj.SalesChannelID = SalesChannelID;
                obj.SalesChannelCode = SalesChannelCode;
                // obj.SalesChannelID = 82;
                obj.SKUCode = SKUCode;
                dt = obj.GetSerialNosByCode();
            }
            return dt;
        }
        catch (Exception ex)
        {
            return dt;
        }
    }





    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string ValidateAllSerials(Int32 salesChannelID, string SalesChannelCode, string SkuCode, string StockSerialNo, string TypeID, string mode, string StockBinTypeID)  /* #C005 StockBinTypeID Added */
    {
        string result = "Success";

        StockSerialNo = StockSerialNo.Replace("\r\n", ",");
        /*#CC06 START COMMENTED StockSerialNo = StockSerialNo.Replace("\n", "");
        StockSerialNo = StockSerialNo.Replace("\r", "");#CC06 END COMMENTED  */
        StockSerialNo = StockSerialNo.Replace("\n", ",");/*#CC06 ADDED*/
        StockSerialNo = StockSerialNo.Replace("\r", ",");/*#CC06 ADDED*/
        StockSerialNo = StockSerialNo.Replace(",,", ",");

        string Message = String.Empty;
        string SerialXML = string.Empty;

        int quantity = 1;
        if (StockSerialNo.Contains(','))
        {
            quantity = StockSerialNo.Split(',').Length;
        }

        byte b = ValidateSerials(StockSerialNo, quantity, out Message, out SerialXML);

        if (b != 1)
        {
            result = Message;
        }
        else
        {

            using (DataAccess.BeautyAdvisorData service = new DataAccess.BeautyAdvisorData())
            {
                //res = service.ValidateStockOfUserSerialMaster(PartID, EngineerID, StockSerialNo);
                System.Data.DataTable DtInvaidSerials = service.InValidSerials(salesChannelID, SalesChannelCode, SkuCode, SerialXML, TypeID, StockBinTypeID); /* #C005 StockBinTypeID Added */

                string msg = string.Empty;

                //  Message = Message + "Invalid (" + Serials + ") Serial No length. \n";
                if (DtInvaidSerials != null && DtInvaidSerials.Rows.Count > 0)
                {

                    foreach (DataRow dtRow in DtInvaidSerials.Rows)
                    {
                        msg += dtRow["SerialNo"] + " " + dtRow["ErrorMessage"] + "\n";
                    }
                    result = msg;

                }
            };
        }

        return result; // 0: Serial does not exists or Invalid; 1: Serial No Exists & valid
    }

    /*#CC01 START ADDED*/

    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string ValidateAllSerialsReturn(int StockBinType, string InvoiceNo, Int32 salesChannelID, string SalesChannelCode, string SkuCode, string StockSerialNo, string TypeID, string mode)
    {
        string result = "Success";

        StockSerialNo = StockSerialNo.Replace("\r\n", ",");
        StockSerialNo = StockSerialNo.Replace("\n", "");
        StockSerialNo = StockSerialNo.Replace("\r", "");
        StockSerialNo = StockSerialNo.Replace(",,", ",");

        string Message = String.Empty;
        string SerialXML = string.Empty;

        int quantity = 1;
        if (StockSerialNo.Contains(','))
        {
            quantity = StockSerialNo.Split(',').Length;
        }

        byte b = ValidateSerials(StockSerialNo, quantity, out Message, out SerialXML);

        if (b != 1)
        {
            result = Message;
        }
        else
        {

            using (DataAccess.BeautyAdvisorData service = new DataAccess.BeautyAdvisorData())
            {
                //res = service.ValidateStockOfUserSerialMaster(PartID, EngineerID, StockSerialNo);
                System.Data.DataTable DtInvaidSerials = service.InValidSerialsReturn(salesChannelID, SalesChannelCode, SkuCode, SerialXML, TypeID, StockBinType, InvoiceNo);

                string msg = string.Empty;

                //  Message = Message + "Invalid (" + Serials + ") Serial No length. \n";
                if (DtInvaidSerials != null && DtInvaidSerials.Rows.Count > 0)
                {

                    foreach (DataRow dtRow in DtInvaidSerials.Rows)
                    {
                        msg += dtRow["SerialNo"] + " " + dtRow["ErrorMessage"] + "\n";
                    }
                    result = msg;

                }
            };
        }

        return result; // 0: Serial does not exists or Invalid; 1: Serial No Exists & valid
    }
    /*#CC01 START END*/

    byte ValidateSerials(string Serials, int Quantity, out string Message, out string SerialXML)
    {
        SerialXML = string.Empty;
        System.Text.StringBuilder strSerialXML = new System.Text.StringBuilder();
        strSerialXML.AppendLine("<table>");

        Message = string.Empty;
        byte b = 1; // 1- Valid and Exists;0- Not Exists; 2- Invalid Serial Length;3- Invalid Qty.  

        if (Serials == string.Empty)
        {
            b = 0; // input serial No.
            Message = "Input Serial Nos.";
        }
        else if (Serials.Contains(','))
        {
            string[] str_Serials = Serials.Split(',');
            object[] obj = (object[])str_Serials;
            System.Data.DataTable dtSerials = new System.Data.DataTable();
            DataColumn dc = new DataColumn("SerialNo", typeof(string));
            dtSerials.Columns.Add(dc);
            foreach (string str in str_Serials)
            {
                DataRow dr = dtSerials.NewRow();
                dr["SerialNo"] = str;
                dtSerials.Rows.Add(dr);
            }
            dtSerials.AcceptChanges();


            var duplicates = from r in dtSerials.AsEnumerable()
                             group r by r.Field<string>("SerialNo") into gp
                             where gp.Count() > 1
                             select new
                             {
                                 serial = gp.Key
                             };



            ArrayList aa = new ArrayList(duplicates.ToArray());
            //  Array aa = (Array)duplicates.ToArray();


            if (aa.Capacity > 0)
            {

                List<string> lstDuplicate = new List<string>();
                foreach (var g in duplicates)
                {
                    if (!string.IsNullOrEmpty(g.serial))
                        lstDuplicate.Add(Convert.ToString(g.serial));
                }

                if (lstDuplicate.Count > 0)
                {
                    foreach (string item in lstDuplicate)
                    {
                        b = 2;
                        Message = Message + "Duplicate (" + item + ") Serial No. \n";
                    }
                }
            }
            else
            {
                //if (str_Serials.Length == Quantity)
                //{

                foreach (string ar in str_Serials)
                {
                    if (ar.Length < PageBase.SerialNoLength_Min || ar.Length > PageBase.SerialNoLength_Max)
                    {
                        b = 2;
                        Message = Message + "Invalid (" + ar + ") Serial No length. \n";

                    }
                    else
                    {
                        strSerialXML.AppendLine("<rowse>");

                        strSerialXML.AppendFormat("<SerialNo>{0}</SerialNo>{1}", ar, Environment.NewLine);
                        strSerialXML.AppendLine("</rowse>");
                    }
                }

                //}
                //else
                //{
                //    b = 3;
                //    Message = "Serials qty does not matched.";

                //}
            }







        }
        else
        {
            if (Quantity == 1)
            {
                if (Serials.Length < PageBase.SerialNoLength_Min || Serials.Length > PageBase.SerialNoLength_Max)
                {
                    b = 2;
                    Message = Message + "Invalid (" + Serials + ") Serial No length. \n";
                }
                else
                {
                    strSerialXML.AppendLine("<rowse>");
                    strSerialXML.AppendFormat("<SerialNo>{0}</SerialNo>{1}", Serials, Environment.NewLine);
                    strSerialXML.AppendLine("</rowse>");
                }
            }
            else
            {

                b = 3; // Invalid Qty
                Message = "Serials qty does not matched.";
            }
        }


        strSerialXML.AppendLine("</table>");

        SerialXML = strSerialXML.ToString();

        return b;

    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetIMEIListByRetailer(string prefixText, int count, string contextKey)
    {
        string[] strSKUCodes = new string[0];
        DataAccess.ProductData obj = new DataAccess.ProductData();
        try
        {
            Hashtable htCodes;
            obj.IMEI = prefixText;
            obj.RetailerID = Convert.ToInt16(contextKey);
            using (System.Data.DataTable dtCityName = obj.GetIEMIInfoByRetilerID())
            {
                int intCounter = 0;
                if (dtCityName.Rows.Count > 0)
                {
                    strSKUCodes = new string[dtCityName.Rows.Count];
                    htCodes = new Hashtable(dtCityName.Rows.Count);
                    foreach (DataRow drCityName in dtCityName.Rows)
                    {
                        strSKUCodes.SetValue(drCityName["SerialNumber1"], intCounter);
                        htCodes.Add(drCityName["SerialNumber1"].ToString().Trim().ToLower(), drCityName["SerialNumber1"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strSKUCodes = new string[1];
                    htCodes = new Hashtable(1);
                    strSKUCodes.SetValue("No Match Found", 0);
                    htCodes.Add("No Match Found", "-1");
                }
                return strSKUCodes;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strSKUCodes;
        }
        catch (Exception ex)
        {

            return strSKUCodes;
        }
        finally
        {
            // Do Nothing.
        }
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetBatchListByRetailer(string prefixText, int count, string contextKey)
    {
        string[] strSKUCodes = new string[0];
        DataAccess.ProductData obj = new DataAccess.ProductData();
        try
        {
            Hashtable htCodes;
            obj.Batchcode = prefixText;
            obj.RetailerID = Convert.ToInt32(contextKey);
            using (System.Data.DataTable dtCityName = obj.GetBatchcodeInfoByRetilerID())
            {
                int intCounter = 0;
                if (dtCityName.Rows.Count > 0)
                {
                    strSKUCodes = new string[dtCityName.Rows.Count];
                    htCodes = new Hashtable(dtCityName.Rows.Count);
                    foreach (DataRow drCityName in dtCityName.Rows)
                    {
                        strSKUCodes.SetValue(drCityName["BatchCode"], intCounter);
                        htCodes.Add(drCityName["BatchCode"].ToString().Trim().ToLower(), drCityName["BatchCode"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strSKUCodes = new string[1];
                    htCodes = new Hashtable(1);
                    strSKUCodes.SetValue("No Match Found", 0);
                    htCodes.Add("No Match Found", "-1");
                }
                return strSKUCodes;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strSKUCodes;
        }
        catch (Exception ex)
        {

            return strSKUCodes;
        }
        finally
        {
            // Do Nothing.
        }
    }



    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string ValidateIEMIByRetailerID(string IEMI, int RetailerID)
    {
        string strSKUName = string.Empty;
        try
        {
            DataAccess.ProductData obj = new DataAccess.ProductData();
            {
                obj.IMEI = IEMI;
                obj.RetailerID = RetailerID;
                obj.IsValidate = 1;
                using (System.Data.DataTable dtResult = obj.GetIEMIInfoByRetilerID())
                {

                    strSKUName = Convert.ToString(dtResult.Rows[0]["SKUName"].ToString());
                    return strSKUName;
                }
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string ValidateBatchByRetailerID(string batchcode, int RetailerID)
    {
        string strSKUName = string.Empty;
        try
        {
            DataAccess.ProductData obj = new DataAccess.ProductData();
            {
                obj.Batchcode = batchcode;
                obj.RetailerID = RetailerID;
                obj.IsValidate = 1;
                using (System.Data.DataTable dtResult = obj.GetBatchcodeInfoByRetilerID())
                {

                    strSKUName = Convert.ToString(dtResult.Rows[0]["SKUName"].ToString());
                    return strSKUName;
                }
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }



    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string GetInvoiceNumberInfo(string InvoiceNumber, String salestype)
    {
        string strStdCode = string.Empty;
        try
        {
            using (DataAccess.SalesData obj = new DataAccess.SalesData())
            {
                obj.SalesChannelID = BussinessLogic.PageBase.SalesChanelID;
                obj.InvoiceNumber = InvoiceNumber;
                obj.value = Convert.ToInt32(salestype);
                System.Data.DataTable dtInvoice = obj.GetInvoiceList();
                if (dtInvoice.Rows.Count > 0)
                    strStdCode = Convert.ToString(dtInvoice.Rows[0][0]);
                return strStdCode;
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string GetStockInHandByLogin(string SKUCode, int SalesChannelID, string SalesChannelCode, string typeID, string StockBinType/*#CC04 ADDEd*/)
    {
        string strStdCode = string.Empty;
        try
        {
            using (DataAccess.ProductData obj = new DataAccess.ProductData())
            {
                obj.SalesChannelID = Convert.ToInt32(SalesChannelID);
                obj.SKUCode = SKUCode;
                obj.SalesChannelCode = SalesChannelCode;
                obj.TypeID = Convert.ToByte(typeID);
                obj.StockBinType = Convert.ToByte(StockBinType);/*#CC04 ADDEd*/
                System.Data.DataTable dtCity = obj.GetSKUStockInHandBySalesChannelOrRetailer();
                if (dtCity.Rows.Count > 0)
                    strStdCode = Convert.ToString(dtCity.Rows[0]["Details"]);
                return strStdCode;
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }



    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string GetSalesChannelInformation(string AdjustmentForCode, string typeID)
    {
        string strStdCode = string.Empty;
        try
        {
            using (SalesChannelData obj = new SalesChannelData())
            {
                if (!AdjustmentForCode.Contains("-"))
                {
                    return strStdCode;
                }
                string[] lstAdjustCode = AdjustmentForCode.Split('-');
                obj.SalesChannelCode = lstAdjustCode[lstAdjustCode.Length - 1];
                //obj.SalesChannelCode = AdjustmentForCode.Split('-')[1];
                obj.ComingFrom = 1;
                obj.ActiveStatus = 255; /*#27May14 - to allow adjustment for inactive partners as well*/
                obj.SalesChannelTypeID = Convert.ToByte(typeID);
                obj.GetSelectedTextId = 1;/*#CC02 ADDED*/
                System.Data.DataTable dtSalesChannelCode = obj.GetSalesChannelList();
                if (dtSalesChannelCode.Rows.Count > 0)
                {
                    if (Convert.ToByte(typeID) != 101)
                        strStdCode = Convert.ToString(dtSalesChannelCode.Rows[0]["SaleschannelName"]) + "/" + Convert.ToString(dtSalesChannelCode.Rows[0]["SaleschannelId"]);
                    else
                        strStdCode = Convert.ToString(dtSalesChannelCode.Rows[0]["RetailerName"]) + "/" + Convert.ToString(dtSalesChannelCode.Rows[0]["RetailerId"]);
                }

                return strStdCode;
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }

    /*#CC03 ADDED START*/
    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetSalesChannelList(string prefixText, int count)
    {
        string[] strSalesChannelCode = new string[0];
        SalesChannelData obj = new SalesChannelData();
        try
        {
            if (Session["EntityType"] == null)
            {
                return strSalesChannelCode;
            }
            if (Convert.ToInt16(Session["EntityType"].ToString()) == 0)
            {
                return strSalesChannelCode;
            }
            Hashtable htCodes;
            obj.UserID = Convert.ToInt32(Session["UserID"].ToString());
            obj.SalesChannelTypeID = Convert.ToInt16(Session["EntityType"]);
            obj.SalesChannelCode = prefixText;
            obj.ComingFrom = 0;
            using (System.Data.DataTable dtSalesChannelCode = obj.SalesChannelList())
            {
                int intCounter = 0;
                if (dtSalesChannelCode.Rows.Count > 0)
                {
                    strSalesChannelCode = new string[dtSalesChannelCode.Rows.Count];
                    htCodes = new Hashtable(dtSalesChannelCode.Rows.Count);
                    foreach (DataRow drSalesChannelName in dtSalesChannelCode.Rows)
                    {
                        strSalesChannelCode.SetValue(drSalesChannelName["SalesChannelName"], intCounter);
                        htCodes.Add(drSalesChannelName["SalesChannelName"].ToString().Trim().ToLower(), drSalesChannelName["SalesChannelID"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strSalesChannelCode = new string[1];
                    htCodes = new Hashtable(1);
                    strSalesChannelCode.SetValue("No Match Found", 0);
                    htCodes.Add("No Match Found", "-1");
                }
                return strSalesChannelCode;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strSalesChannelCode;
        }
        catch (Exception ex)
        {

            return strSalesChannelCode;
        }
        finally
        {
            // Do Nothing.
        }
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string GetSalesChannelInfo(string SalesChannelCode, string typeID)
    {
        string strStdCode = string.Empty;
        try
        {
            using (SalesChannelData obj = new SalesChannelData())
            {
                if (!SalesChannelCode.Contains("-"))
                {
                    return strStdCode;
                }
                string[] lstAdjustCode = SalesChannelCode.Split('-');
                obj.SalesChannelCode = lstAdjustCode[lstAdjustCode.Length - 1];
                obj.ComingFrom = 1;
                obj.UserID = Convert.ToInt32(Session["UserID"].ToString());
                obj.SalesChannelTypeID = Convert.ToInt16(Session["EntityType"]);
                System.Data.DataTable dtSalesChannelCode = obj.SalesChannelList();
                if (dtSalesChannelCode.Rows.Count > 0)
                {
                    strStdCode = Convert.ToString(dtSalesChannelCode.Rows[0]["SaleschannelName"]) + "/" + Convert.ToString(dtSalesChannelCode.Rows[0]["SaleschannelId"]);
                }
                return strStdCode;
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }
    /*#CC03 ADDED END*/
}
