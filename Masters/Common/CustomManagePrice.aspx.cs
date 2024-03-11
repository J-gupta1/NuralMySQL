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

public partial class Masters_HO_Common_CustomManagePrice : PageBase
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
            ucDatePicker.MinRangeValue = DateTime.Now.Date;
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
            byte IsSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            UploadCheck = UploadFile.IsExcelFile(fuploadPrice, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.ePriceUpload;
                IsSuccess = UploadFile.uploadValidExcel(ref objDS, "CustomPrice");
                switch (IsSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        gvPrice.DataSource = objDS;
                        gvPrice.DataBind();
                        gvPrice.Columns[5].Visible = true;
                        Btnsave.Enabled = false;
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
                gvPrice.Columns[5].Visible = false;
                gvPrice.DataBind();
                Btnsave.Enabled = true;
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
        if (Convert.ToDateTime(ucDatePicker.Date) < DateTime.Now.Date)
        {
            ucMsg.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        }
        return true;
    }

    void Clear()
    {
        cmbPriceList.SelectedIndex = 0;
        ucDatePicker.TextBoxDate.Text = "";
        pnlGrid.Visible = false;
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
                strXML = Ds.GetXml();
                strXML = strXML.Replace("T00:00:00+05:30", "");
                ProductData ObjData = new ProductData();
                ObjData.PriceListID = Convert.ToInt32(cmbPriceList.SelectedValue);
                ObjData.EffectiveDate = Convert.ToDateTime(ucDatePicker.Date);
                ObjData.XMLList = strXML;
                ObjData.Status = true;
                intResult = ObjData.InsertUpdateCustomPriceInfo();
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


    protected void LBViewPrice_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewPrice.aspx");

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

                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrice;          //1-TD Code download  2-SKU Download  3-Download Sales Template 4-Download Salesman 
                DsReferenceCode = objSalesData.GetAllTemplateData();
                if (DsReferenceCode!=null && DsReferenceCode.Tables[0].Rows.Count > 0)
                {
                    
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SKUCode List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(DsReferenceCode, FilenameToexport,EnumData.eTemplateCount.ePrice);

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
}
