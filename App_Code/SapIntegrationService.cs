using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for SapIntegrationService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SapIntegrationService : System.Web.Services.WebService
{
    DataSet dsList;
    SapService objsapServiceOnida;
    DataSet dsSapInfoOnida;
    static string ServiceDocNoSap = string.Empty;
    //@FromErrorOrNot=0 successfully inserted  
    //@FromErrorOrNot=1 Exception occured  
    //@FromErrorOrNot=2 Selection 
    string strConnectionString = string.Empty;
    
    
    public SapIntegrationService()
    {
        strConnectionString = ConfigurationManager.ConnectionStrings["SapConString"].ToString();
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
  
    [WebMethod]
    public void FillListofTables()
    {
        
        try
        {
            int havingData = 0;
            dsList = new DataSet();
            using (POC objSapSelect = new POC())
            {
                ServiceDocNoSap = string.Empty;
                dsList = objSapSelect.GetUpdateSelectRawData(strConnectionString,0,2);
                ServiceDocNoSap = objSapSelect.GenServiceDocNo;
            }
            for(int i=0;i<dsList.Tables.Count;i++)
            {
                if (dsList.Tables[i].Rows.Count <= 0)
                {
                    havingData = havingData + 1;
                }
            }
            if (havingData != dsList.Tables.Count)
            {
                if (dsList.Tables[0].Rows.Count > 0)
                {
                    insertGrnData();
                }
                else
                {
                    objsapServiceOnida = new SapService();
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
                    objsapServiceOnida.StatusValue = "No Data in GRN Table";
                    objsapServiceOnida.StatusValue = "Grn Table has no data";
                    objsapServiceOnida.MessageDetail = "GRN(No Data)";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.GRNData.ToString();
                    objsapServiceOnida.SapFileName = "GRN";
                    objsapServiceOnida.XMLData = "<Dataset></Dataset>";
                    objsapServiceOnida.insertServiceTraceLog();
                }
                if (dsList.Tables[1].Rows.Count > 0)
                {
                    insertBTMData();
                }
                else
                {
                    objsapServiceOnida = new SapService();
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
                    objsapServiceOnida.StatusValue = "No Data in BTM Table";
                    objsapServiceOnida.StatusValue = "BTM Table has no data";
                    objsapServiceOnida.MessageDetail = "BTM(No Data)";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.BTMData.ToString();
                    objsapServiceOnida.SapFileName = "BTM";
                    objsapServiceOnida.XMLData = "<Dataset></Dataset>";
                    objsapServiceOnida.insertServiceTraceLog();
                }
                if (dsList.Tables[2].Rows.Count > 0)
                {
                    insertPrimarySalesData();
                }
                else
                {
                    objsapServiceOnida = new SapService();
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
                    objsapServiceOnida.StatusValue = "No Data in PrimarySales/Return Table";
                    objsapServiceOnida.StatusValue = "PrimarySales/Return Table has no data";
                    objsapServiceOnida.MessageDetail = "PrimarySales/Return(No Data)";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
                    objsapServiceOnida.SapFileName = "PrimarySales/Return";
                    objsapServiceOnida.XMLData = "<Dataset></Dataset>";
                    objsapServiceOnida.ServiceDocNo = ServiceDocNoSap;
                    objsapServiceOnida.insertServiceTraceLog();
                }
                if (dsList.Tables[3].Rows.Count > 0)
                {
                    insertPrimarySalesReturnData();
                }
                else
                {
                    objsapServiceOnida = new SapService();
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
                    objsapServiceOnida.StatusValue = "No Data in Primary Sales Return Table";
                    objsapServiceOnida.StatusValue = "Primary Sales Return Table has no data";
                    objsapServiceOnida.MessageDetail = "Primary Sales Return(No Data)";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
                    objsapServiceOnida.SapFileName = "Primary Sales Return";
                    objsapServiceOnida.XMLData = "<Dataset></Dataset>";
                    objsapServiceOnida.ServiceDocNo = ServiceDocNoSap;
                    objsapServiceOnida.insertServiceTraceLog();
                }

            }
            else
            {
                objsapServiceOnida = new SapService();
                objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.NoFileToUpload;
                objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
                objsapServiceOnida.StatusValue = "Data is not in any table";
                objsapServiceOnida.StatusValue = "No Data";
                objsapServiceOnida.MessageDetail = "No Data";
                objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.Downloading_Uploading.ToString();
                objsapServiceOnida.SapFileName = "No Data";
                objsapServiceOnida.XMLData = dsList.GetXml();
                objsapServiceOnida.ServiceDocNo = ServiceDocNoSap;
                objsapServiceOnida.insertServiceTraceLog();

            }
       
        }
        catch (Exception ex)
        {
            throw ex;
        }
    
    }
    public void UpdateStatus(int value, int FromErrorOrNo, String FileName, EnumData.EnumSAPMethodName strMethodName, EnumData.EnumSAPModuleName strModuleName)
    {
        DataSet dsUpdate = new DataSet();
        objsapServiceOnida = new SapService();
        using (POC objSapSelect = new POC())
        {
            dsUpdate = objSapSelect.GetUpdateSelectRawData(strConnectionString, value, FromErrorOrNo);
            if (objSapSelect.Error != String.Empty || objSapSelect.Error!="")
            {
                objsapServiceOnida.ModuleName = strModuleName;
                objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
                objsapServiceOnida.StatusValue = "Data may be inserted successfully,but while status updation error occured";
                objsapServiceOnida.MessageDetail = objSapSelect.Error;
                objsapServiceOnida.SapServiceMethodName = strMethodName.ToString();
                objsapServiceOnida.SapFileName = FileName;
                objsapServiceOnida.ServiceDocNo = ServiceDocNoSap;
                objsapServiceOnida.XMLData = "No Error";
                objsapServiceOnida.insertServiceTraceLog();
            }
        }

    }
    private void insertGrnData()
    {
        try
        {

            using (SalesData objGrn = new SalesData())
            {

                dsSapInfoOnida = new DataSet();
                objsapServiceOnida = new SapService();
                dsSapInfoOnida.Merge(dsList.Tables[0]);
                objGrn.InsertInfoGRNUploadOnida(dsList.Tables[0]);
                if (objGrn.ErrorDetailXML == null)
                {
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.GRNDataUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.SuccessWithData;
                    objsapServiceOnida.StatusValue = "Successfully inserted/Updated.";
                    objsapServiceOnida.MessageDetail = "Successfully inserted/Updated.";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.GRNData.ToString();
                    objsapServiceOnida.SapFileName = dsList.Tables[0].TableName;
                    objsapServiceOnida.XMLData = dsSapInfoOnida.GetXml();
                    objsapServiceOnida.insertServiceTraceLog();
                    objsapServiceOnida = null;
                    UpdateStatus(1, 1, "GRN", EnumData.EnumSAPMethodName.GRNData, EnumData.EnumSAPModuleName.GRNDataUpload);
                }
                else
                {
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.GRNDataUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Failure;
                    objsapServiceOnida.StatusValue = "Error in the Data";
                    objsapServiceOnida.MessageDetail = objGrn.ErrorDetailXML + "Data Corrupt";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.GRNData.ToString();
                    objsapServiceOnida.SapFileName = dsList.Tables[0].TableName + " GRN";
                    objsapServiceOnida.insertServiceTraceLog();
                    UpdateStatus(1, 0, "GRN", EnumData.EnumSAPMethodName.GRNData, EnumData.EnumSAPModuleName.GRNDataUpload);
                }

            }
        }
        catch (Exception ex)
        {
            objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.GRNDataUpload;
            objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
            objsapServiceOnida.StatusValue = "Date is not in correct format";
            objsapServiceOnida.StatusValue = ex.Message;
            objsapServiceOnida.MessageDetail = ex.Source + " :: " + ex.StackTrace;
            objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.BTMData.ToString();
            objsapServiceOnida.SapFileName = dsList.Tables[0].TableName + "GRN";
            objsapServiceOnida.XMLData = dsSapInfoOnida.GetXml();
            objsapServiceOnida.insertServiceTraceLog();
            objsapServiceOnida = null;
            UpdateStatus(1, 0, "GRN", EnumData.EnumSAPMethodName.GRNData, EnumData.EnumSAPModuleName.GRNDataUpload);

        }

    }
    private void insertBTMData()
    {
        try
        {
            using (WarehouseTranaction objSapInsert = new WarehouseTranaction())
            {
                dsSapInfoOnida = new DataSet();
                objsapServiceOnida = new SapService();
                dsSapInfoOnida.Merge(dsList.Tables[1]);
                dsSapInfoOnida.Tables[0].TableName = "Table";
                objSapInsert.BTMSapDetailXML = dsSapInfoOnida.GetXml();
                objSapInsert.UploadBTMSapDataOnida();
                if (objSapInsert.BTMSapDetailXML == null)
                {
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.BTMDataUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.SuccessWithData;
                    objsapServiceOnida.StatusValue = "Successfully inserted/Updated.";
                    objsapServiceOnida.MessageDetail = "Successfully inserted/Updated.";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.BTMData.ToString();
                    objsapServiceOnida.SapFileName = dsList.Tables[1].TableName + " BTM";
                    objsapServiceOnida.XMLData = "No Error";
                    objsapServiceOnida.insertServiceTraceLog();
                    objsapServiceOnida = null;
                    UpdateStatus(2, 1, "BTM", EnumData.EnumSAPMethodName.BTMData, EnumData.EnumSAPModuleName.BTMDataUpload);

                }
                else
                {
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.BTMDataUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Failure;
                    objsapServiceOnida.StatusValue = "Error in the Data";
                    objsapServiceOnida.MessageDetail = objSapInsert.BTMSapDetailXML + " Data Corrupt for BTM";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.BTMData.ToString();
                    objsapServiceOnida.SapFileName = dsList.Tables[1].TableName + " BTM";
                    objsapServiceOnida.XMLData = dsSapInfoOnida.GetXml();
                    objsapServiceOnida.insertServiceTraceLog();
                    objsapServiceOnida = null;
                    UpdateStatus(2, 0, "BTM", EnumData.EnumSAPMethodName.BTMData, EnumData.EnumSAPModuleName.BTMDataUpload);

                }
            }
        }
        catch (Exception ex)
        {
            objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.BTMDataUpload;
            objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
            objsapServiceOnida.StatusValue = "Data is not in correct format";
            objsapServiceOnida.StatusValue = ex.Message;
            objsapServiceOnida.MessageDetail = ex.Source + " :: " + ex.StackTrace;
            objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.BTMData.ToString();
            objsapServiceOnida.SapFileName = dsList.Tables[1].TableName + " BTM";
            objsapServiceOnida.XMLData = dsSapInfoOnida.GetXml();
            objsapServiceOnida.insertServiceTraceLog();
            objsapServiceOnida = null;
            UpdateStatus(2, 0, "BTM", EnumData.EnumSAPMethodName.BTMData, EnumData.EnumSAPModuleName.BTMDataUpload);

        }
    }
    private void insertPrimarySalesData()
    {
        //Mod
        try
        {
            using (SalesData objMODSales = new SalesData())
            {

                dsSapInfoOnida = new DataSet();
                objsapServiceOnida = new SapService();
                dsSapInfoOnida.Merge(dsList.Tables[2]);
                dsSapInfoOnida.Tables[0].TableName = "Table";
                objMODSales.InsertPrimarySalesInfoMODUploadOnida(dsList.Tables[2]);
                if (objMODSales.ErrorDetailXML == null)
                {
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.MODDataUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.SuccessWithData;
                    objsapServiceOnida.StatusValue = "Successfully inserted/Updated.";
                    objsapServiceOnida.MessageDetail = "Successfully inserted/Updated.";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
                    objsapServiceOnida.SapFileName = dsList.Tables[1].TableName;
                    objsapServiceOnida.XMLData = "No Error";
                    objsapServiceOnida.ServiceDocNumber = ServiceDocNoSap;
                    objsapServiceOnida.insertServiceTraceLog();
                    objsapServiceOnida = null;
                    UpdateStatus(3, 1, "PrimarySales/Return", EnumData.EnumSAPMethodName.MODData, EnumData.EnumSAPModuleName.MODDataUpload);

                }
                else
                {
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.MODDataUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Failure;
                    objsapServiceOnida.StatusValue = "Error in the Data";
                    objsapServiceOnida.MessageDetail = objMODSales.ErrorDetailXML + " Data Corrupt";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
                    objsapServiceOnida.SapFileName = dsList.Tables[1].TableName;
                    objsapServiceOnida.ServiceDocNumber = ServiceDocNoSap;
                    objsapServiceOnida.insertServiceTraceLog();
                    objsapServiceOnida = null;
                    UpdateStatus(3, 0, "PrimarySales/Return", EnumData.EnumSAPMethodName.MODData, EnumData.EnumSAPModuleName.MODDataUpload);

                }
            }
        }

        catch (Exception ex)
        {
            objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.MODDataUpload;
            objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
            objsapServiceOnida.StatusValue = "Data is not in correct format";
            objsapServiceOnida.StatusValue = ex.Message;
            objsapServiceOnida.MessageDetail = ex.Source + " :: " + ex.StackTrace;
            objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
            objsapServiceOnida.SapFileName = dsList.Tables[2].TableName + "MOD";
            objsapServiceOnida.XMLData = dsSapInfoOnida.GetXml();
            objsapServiceOnida.ServiceDocNumber = ServiceDocNoSap;
            objsapServiceOnida.insertServiceTraceLog();
            objsapServiceOnida = null;
            UpdateStatus(3, 0, "PrimarySales/Return", EnumData.EnumSAPMethodName.MODData, EnumData.EnumSAPModuleName.MODDataUpload);


        }
   }
    private void insertPrimarySalesReturnData()
    {
        //Mod
        try
        {
            using (SalesData objMODSales = new SalesData())
            {
                dsList = null;
                using (POC objSapSelect = new POC())
                {
                    dsList = objSapSelect.GetUpdateSelectRawData(strConnectionString, 4, 2);
                }
                dsSapInfoOnida = new DataSet();
                objsapServiceOnida = new SapService();
                dsSapInfoOnida.Merge(dsList.Tables[0]);
                dsSapInfoOnida.Tables[0].TableName = "PrimarySalesReturn";
                objMODSales.InsertPrimarySalesInfoMODUploadOnida(dsList.Tables[0]);
                if (objMODSales.ErrorDetailXML == null)
                {
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.MODDataUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.SuccessWithData;
                    objsapServiceOnida.StatusValue = "Successfully inserted/Updated.";
                    objsapServiceOnida.MessageDetail = "Successfully inserted/Updated.";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
                    objsapServiceOnida.SapFileName = dsList.Tables[0].TableName;
                    objsapServiceOnida.XMLData = "No Error";
                    objsapServiceOnida.ServiceDocNumber = ServiceDocNoSap;
                    objsapServiceOnida.insertServiceTraceLog();
                    objsapServiceOnida = null;
                    UpdateStatus(5, 1, "PrimarySalesReturn", EnumData.EnumSAPMethodName.MODData, EnumData.EnumSAPModuleName.MODDataUpload);

                }
                else
                {
                    objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.MODDataUpload;
                    objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Failure;
                    objsapServiceOnida.StatusValue = "Error in the Data";
                    objsapServiceOnida.MessageDetail = objMODSales.ErrorDetailXML + " Data Corrupt";
                    objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
                    objsapServiceOnida.SapFileName = dsList.Tables[0].TableName;
                    objsapServiceOnida.ServiceDocNumber = ServiceDocNoSap;
                    objsapServiceOnida.insertServiceTraceLog();
                    objsapServiceOnida = null;
                    UpdateStatus(5, 0, "PrimarySalesReturn", EnumData.EnumSAPMethodName.MODData, EnumData.EnumSAPModuleName.MODDataUpload);

                }
            }
        }

        catch (Exception ex)
        {
            objsapServiceOnida.ModuleName = EnumData.EnumSAPModuleName.MODDataUpload;
            objsapServiceOnida.LogType = (int)EnumData.EnumSAPLogType.Error;
            objsapServiceOnida.StatusValue = "Data is not in correct format";
            objsapServiceOnida.StatusValue = ex.Message;
            objsapServiceOnida.MessageDetail = ex.Source + " :: " + ex.StackTrace;
            objsapServiceOnida.SapServiceMethodName = EnumData.EnumSAPMethodName.MODData.ToString();
            objsapServiceOnida.SapFileName = dsList.Tables[0].TableName + "MOD";
            objsapServiceOnida.XMLData = dsSapInfoOnida.GetXml();
            objsapServiceOnida.ServiceDocNumber = ServiceDocNoSap;
            objsapServiceOnida.insertServiceTraceLog();
            objsapServiceOnida = null;
            UpdateStatus(5, 0, "PrimarySales/Return", EnumData.EnumSAPMethodName.MODData, EnumData.EnumSAPModuleName.MODDataUpload);


        }
    }
   
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public bool ISCalenderExists(string CalenderName)
    {
        bool result = true;
        using (FinacialCalender ObjCal = new FinacialCalender())
        {
            ObjCal.CalenderYear = CalenderName;
            result = ObjCal.ISCalenderExists();
        };
        return result;
    }
}

