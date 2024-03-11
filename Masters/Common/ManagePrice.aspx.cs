#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*
 * Change Log:
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 03-Apr-2016, Sumit Maurya, #CC01, Date validation of current date is commented as previous date can be saved if user
 *                                   try to save fresh price for Sku which doesnot exists in PriceList Table.
 * 
 * ==================================================================================================
*/
#endregion
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Text;
using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

public partial class Masters_HO_Common_ManagePrice : PageBase
{
    #region Upload Variables
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                pnlGrid.Visible = false;
                BindPriceList();
            }
            /* ucDatePicker.MinRangeValue = DateTime.Now.Date; #CC01 Commented*/
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindPriceList()
    {
        try
        {
            DataTable dt = new DataTable();
            cmbPriceList.Items.Clear();
            using (ProductData ObjProduct = new ProductData())
            {
                ObjProduct.Status = true;
                ObjProduct.CompanyId = PageBase.ClientId;
                dt = ObjProduct.GetPriceListInfo();

            };
            String[] colArray = { "PriceListID", "PriceListName" };
            PageBase.DropdownBinding(ref cmbPriceList, dt, colArray);
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        DataSet objDS = new DataSet();
        try
        {
            ViewState["Price"] = null;
            byte IsSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            UploadCheck = UploadFile.IsExcelFile(fuploadPrice, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.ePriceUpload;
                IsSuccess = UploadFile.uploadValidExcel(ref objDS, "Price");

               

                switch (IsSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        gvPrice.DataSource = objDS;
                        gvPrice.DataBind();
                        gvPrice.Columns[7].Visible = true;
                        pnlGrid.Visible = true;
                        break;
                    case 1:
                        InsertData(objDS);
                        break;
                }

            }
            else if (UploadCheck == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }

        }
        catch (Exception Ex)
        {
            ucMsg.ShowError(Ex.Message.ToString());
            PageBase.Errorhandling(Ex);

        }
    }
    private void InsertData(DataSet objds)
    {
        if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
        {
            {
                pnlGrid.Visible = true;
                ViewState["Price"] = objds;
                gvPrice.DataSource = objds;
                gvPrice.Columns[7].Visible = false; 
                gvPrice.DataBind();
            }
        }

    }

    bool pageValidate()
    {
        if (cmbPriceList.SelectedIndex == 0 || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
        {
            ucMsg.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        /* #CC01 Comment Start
         if (Convert.ToDateTime(ucDatePicker.Date) < DateTime.Now.Date)
        {
            ucMsg.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        } 
         #CC01 Comment End */
        return true;
    }

    void Clear()
    {
        cmbPriceList.SelectedIndex = 0;
        ucDatePicker.TextBoxDate.Text = "";
        pnlGrid.Visible = false;
        ViewState["Price"] = null;
    }

    



    protected void Btnsave_Click(object sender, EventArgs e)
    {
        //if (IsPageRefereshed == true)
        //{
        //    return;
        //}         
        if (!pageValidate())
        {
            return;
        }
        try
        {
            if (ViewState["Price"] != null)
            {
                DataSet Ds = new DataSet();
                Ds = (DataSet)ViewState["Price"];
                string strXML = string.Empty;
                int intResult = 0;
                //strXML = Ds.GetXml();
                
                

                strXML = strXML.Replace("T00:00:00+05:30", "");
                ProductData ObjData = new ProductData();
                ObjData.PriceListID = Convert.ToInt32(cmbPriceList.SelectedValue);
                ObjData.EffectiveDate = Convert.ToDateTime(ucDatePicker.Date);
                ObjData.XMLList = strXML;
                ObjData.Status = true;
                intResult = ObjData.InsertUpdatePriceInfo();
                if (ObjData.XMLList != null && ObjData.XMLList != string.Empty)
                {
                    ucMsg.XmlErrorSource = ObjData.XMLList;
                    return;
                }
                if (intResult == 0)
                {
                    ucMsg.ShowSuccess(Resources.Messages.DataUploadSuccess);

                }
                if (intResult == 1)
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                }
                Clear();

            }
        }

        catch (Exception Ex)
        {
            ucMsg.ShowError(Ex.Message.ToString());
            PageBase.Errorhandling(Ex);

        }
    }
    #region TO CREATED BY JITENDRA IN THE PLACE OF XML
    public bool UserDetailBCP(DataTable Ds)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();
            string strPrimarySessionName = null;
            ViewState[strPrimarySessionName] = guid;
            Ds.Columns.Add(AddColumn(guid, "TransactionUploadSessionId", typeof(System.String)));
            string connectMySQL = PageBase.ConStr;
            string virtualFilePath = "~/Excel/Upload/UploadExcelFiles/" + DateTime.Now.Ticks.ToString() + ".csv";

            CreateCSVfile(Ds, Server.MapPath(virtualFilePath));
            using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
            {
                
                cn1.Open();
                MySqlBulkLoader bulkCopy = new MySqlBulkLoader(cn1);
                bulkCopy.TableName = "PriceBulkuploader"; //Create ProductOrder table into MYSQL database...
                bulkCopy.FieldTerminator = ",";
                bulkCopy.LineTerminator = "\r\n";
                bulkCopy.FileName = Server.MapPath(virtualFilePath);
                bulkCopy.NumberOfLinesToSkip = 0;
                bulkCopy.Columns.AddRange(new[] {
                            "PriceBulkupload",
                            "SKUCode",
                            "SDPrice",
                            "MDPrice",
                            "RetailerPrice",
                            "MOP",
                            "MRP",
                            "WHPrice",
                            "TransactionUploadSessionId"
                    });

                bulkCopy.Load();

                //Once data write into db then delete file..
                try
                {
                    File.Delete(Server.MapPath(virtualFilePath));
                }
                catch (Exception ex)
                {
                    ucMsg.ShowError(ex.Message);
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            return false;
        }
    }

    private void CreateCSVfile(DataSet ds, string v)
    {
        throw new NotImplementedException();
    }
    #endregion TO CREATED BY JITENDRA IN THE PLACE OF XML


    protected void LBViewPrice_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewPricev2.aspx");

    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void DwnldSKUCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet DsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrice;          //1-TD Code download  2-SKU Download  3-Download Sales Template 4-Download Salesman 
                DsReferenceCode = objSalesData.GetAllTemplateData();
                if (DsReferenceCode != null && DsReferenceCode.Tables[0].Rows.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SKUCode List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(DsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrice);

                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.NoRecord);

                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }


    }

    // CreateCSVfile class define by jitendra
    public static void CreateCSVfile(DataTable dtable, string strFilePath)
    {
        StreamWriter sw = new StreamWriter(strFilePath, false);
        int icolcount = dtable.Columns.Count;
        foreach (DataRow drow in dtable.Rows)
        {
            for (int i = 0; i < icolcount; i++)
            {
                if (!Convert.IsDBNull(drow[i]))
                {
                    sw.Write(drow[i].ToString());
                }
                if (i < icolcount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
        }
        sw.Close();
        sw.Dispose();
    }
    // CreateCSVfile class define by jitendra
}
