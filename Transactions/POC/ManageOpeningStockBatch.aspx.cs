using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using Cryptography;
using DataAccess;
using System.Data;
using System.IO;

public partial class Transactions_Common_ManageOpeningStockBatch : System.Web.UI.Page
{
    string strUploadedFileName = string.Empty;
    int counter = 0;
    int Saleschannelid = 0;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        bindAssets();

        //if (Request.QueryString["mid"] != null && Request.QueryString["mid"] != "")
        //{

        //    Saleschannelid = Convert.ToInt32(Crypto.Decrypt(Request.QueryString["mid"].Replace(" ", "+"), "Tseries"));

        //}
        if (Saleschannelid != 0)
        {
            using (SalesChannelData objsalesChannel = new SalesChannelData())
            {
                objsalesChannel.SalesChannelID = Saleschannelid;
                objsalesChannel.BlnShowDetail = true;
                DataTable Dt = objsalesChannel.GetSalesChannelInfo();

                if (Dt != null && Dt.Rows.Count > 0)
                {
                    lblUserNameDesc.Text = Dt.Rows[0]["SalesChannelName"].ToString();
                    if (Convert.ToBoolean(Dt.Rows[0]["IsOpeningStockEntered"].ToString()) == true)
                    {
                        ucMessage1.ShowInfo(Resources.Messages.OpeningStockAlreadyEnter);
                        return;
                    }

                }
                else
                {
                    lblUserNameDesc.Text = "";
                }

            }
        }

        ucDatePicker.Date = DateTime.Now.ToShortDateString();
        ucDatePicker.TextBoxDate.Enabled = false;
        ucDatePicker.imgCal.Enabled = false;
        ucMessage1.ShowControl = false;
        if (!IsPostBack)
        {
            ViewState["DtDetail"] = null;

            pnlGrid.Visible = false;



        }
    }
    void bindAssets()
    {
        ImgSideLogo.ImageUrl = "~/" + PageBase.strAssets + "/CSS/Images/zedsaleslogo.gif";
        hyplogo.ImageUrl = "~/" + PageBase.strAssets + "/CSS/Images/innerlogo.gif";
        hypfooterlogo.ImageUrl = "~/" + PageBase.strAssets + "/CSS/Images/footerimg.gif";
        hypfooterlogo.NavigateUrl = PageBase.redirectURL;
        hypfooterlogo.Target = "_blank";
        cssStyle.Attributes.Add("href", "~/" + PageBase.strAssets + "/CSS/Style.css");
        cssBootstrap.Attributes.Add("href", "~/" + PageBase.strAssets + "/CSS/bootstrap.min.css");
        //csswithoutmaster.Attributes.Add("href", "~/" + PageBase.strAssets + "/CSS/withoutmaster.css");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
           if (!PageValidatesave())
            {
                return;
            }
            DataTable Detail = new DataTable();
            using (POC ObjOpeningStock = new POC())
            {
                Detail = ObjOpeningStock.GettvpTableCommomForStockBatchEntry();
            }


            if (ViewState["Detail"] != null)
            {
                foreach (DataRow dr in ((DataTable)ViewState["Detail"]).Rows)
                {
                    DataRow drow = Detail.NewRow();
                    drow[0] = dr["SKUCode"].ToString();
                    drow[1] = dr["Quantity"].ToString();
                    drow[2] = dr["BatchNumber"].ToString();
                    Detail.Rows.Add(drow);
                }
                Detail.AcceptChanges();
            }


            using (POC ObjOpeningStock = new POC())
            {

                ObjOpeningStock.Error = "";
                ObjOpeningStock.SalesChannelID = PageBase.SalesChanelID;
                ObjOpeningStock.InsertOpeningStockBatch(Detail);
                if (ObjOpeningStock.XMLList != null && ObjOpeningStock.XMLList != string.Empty)
                {
                    ucMessage1.XmlErrorSource = ObjOpeningStock.XMLList;
                    return;
                }
                else if (ObjOpeningStock.Error != null && ObjOpeningStock.Error != "")
                {
                    ucMessage1.ShowError(ObjOpeningStock.Error);
                    return;
                }
                ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                ClearForm();
                updgrid.Update();

                //Response.Redirect("Thank.htm");
                Response.Redirect("~/Masters/SalesChannel/SalesChannelBranding.aspx", false);

            }
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    public String readHtmlPage(string url)
    {
        string FileName = (url);
        StreamReader ObjStrmReader;
        ObjStrmReader = File.OpenText(FileName);
        string strMail = ObjStrmReader.ReadToEnd();
        ObjStrmReader = null;
        return strMail;
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        ClearForm();

    }
    void ClearForm()
    {

        pnlGrid.Visible = false;
        ucDatePicker.Date = "";
        ViewState["Detail"] = null;


    }
    bool PageValidatesave()
    {

        if (ucDatePicker.Date == "")
        {
            ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        if (Convert.ToDateTime(ucDatePicker.Date) > System.DateTime.Now)
        {
            ucMessage1.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        }

        return true;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Detail"] = null;
            DataSet objDS = new DataSet();
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                isSuccess = UploadFile.uploadValidExcel(ref objDS, "GRNUploadBatchWise");

                switch (isSuccess)
                {
                    case 0:
                        ucMessage1.ShowInfo(UploadFile.Message);
                        break;
                    case 2:
                        ucMessage1.ShowInfo(Resources.Messages.CheckErrorGrid);
                        pnlGrid.Visible = true;
                        gvStockEntry.Columns[3].Visible = true;

                        gvStockEntry.DataSource = objDS;
                        gvStockEntry.DataBind();
                        updgrid.Update();
                        break;
                    case 1:
                        InsertData(objDS);
                        break;
                }
            }
            else if (UploadCheck == 2)
            {
                ucMessage1.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                ucMessage1.ShowInfo(Resources.Messages.SelectFile);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    private void InsertData(DataSet objDS)
    {
        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (objDS.Tables[0].Columns.Contains("Error") == false)
            objDS.Tables[0].Columns.Add(dcError);

        if (objDS.Tables[0].Rows.Count > 0)
        {

            pnlGrid.Visible = true;
            btnSave.Visible = true;
            btnReset.Visible = true;

            gvStockEntry.DataSource = objDS.Tables[0];
            gvStockEntry.DataBind();

            gvStockEntry.Columns[3].Visible = false;

        }
        else
        {
            pnlGrid.Visible = false;
        }

        ViewState["Detail"] = objDS.Tables[0];
        btnSave.Visible = true;
        btnReset.Visible = true;
        updgrid.Update();

    }




    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {

                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrice;

                dsReferenceCode = objSalesData.GetAllTemplateData();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
}


