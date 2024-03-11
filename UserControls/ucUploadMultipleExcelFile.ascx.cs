using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using ExportExcelOpenXML;
using System.Text;
using BusinessLogics;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Data.OleDb;

using System.Xml; /* #CC02 Added */
/*
 * 28 Apr 2015, Karam Chand Sharma, #CC01 , Store file upload information in session instead of viewstate because viewstate not clear from primary file upload interface by which when we again upload next time
 *              then bind with old data if page is not refressed.     
 * 31-May-2016, Sumit Maurya, #CC02, To validate unique Carton number and SKU code.
 * 07-Jun-2016, Sumit Maurya, #CC03, New code implemented to filter data and remove from datatable which needed to save in database when user remove desired file.
 * 13-Jul-2016, Sumit Maurya, #CC04, New check added to show error message only if rows count > 0 of datatable.
 */
public partial class UserControls_ucUploadMultipleExcelFile : System.Web.UI.UserControl
{
    int counter = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            /*#CC01 COMMENTED ViewState["FileInfo"] = null;*/
            Session["FileInfo"] = null;/*#CC01 ADDED*/
            Session["DTCheckSKUCartonTemp"] = null;
            Session["DTCheckSKUCarton"] = null;

        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string PathToSave = string.Empty, FileName = string.Empty, SystemFileName = string.Empty, FilePath = string.Empty;
        try
        {
            if (FileUpload1.HasFile)
            {
                SystemFileName = Guid.NewGuid() + "_" + FileUpload1.FileName;
                FileName = FileUpload1.FileName;

                if (FileName.Length > 50)
                {
                    lblMsg.Text = "File name should not be more than 50 characters.";
                    return;
                }

                if (Path.GetExtension(FileName).ToLower() == ".xlsx")
                {
                    ValidateUploadFile objValidateFile = new ValidateUploadFile();

                    FilePath = PageBase.strExcelBulkUploadPath + SystemFileName;
                    FileUpload1.SaveAs(FilePath);
                    DataSet DsExcel = objValidateFile.GetExcelFileIntoDataset(FilePath);
                    if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                    {

                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = SystemFileName;
                            objValidateFile.ExcelFileNameInTable = "BulkFileUpload";
                            objValidateFile.ValidateSchemaFile(false, out objDS, out objSL, DsExcel);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                            {
                                File.Delete(FilePath);
                                lblMsg.Text = objValidateFile.Message;
                            }
                            else
                            {
                                for (int i = 0; i < grdvwFile.Rows.Count; i++)
                                {
                                    Label lblFileName = (Label)grdvwFile.Rows[i].FindControl("lblFileName");
                                    if (lblFileName.Text.Trim() == FileName.Trim())
                                    {
                                        lblMsg.Text = "File already added.";
                                        File.Delete(FilePath);
                                        return;
                                    }
                                }
                                DataTable dtMain = new DataTable();
                                /*#CC01 COMMENTED if (ViewState["FileInfo"] == null)*/
                                if (Session["FileInfo"] == null)
                                {
                                    dtMain.Columns.Add("FileName", typeof(string));
                                    dtMain.Columns.Add("SystemFileName", typeof(string));
                                    dtMain.Rows.Add(FileName, SystemFileName);
                                    /* #CC02 Add Start */
                                    if (validateCarton(dt1, FileName) == false)
                                    {
                                        return;
                                    }
                                    if (processBulkData(dtMain, FileName) == 0)
                                    {
                                        ViewState["FileInfo"] = null;
                                        /* #CC02 Add End */
                                        grdvwFile.DataSource = dtMain;
                                        grdvwFile.DataBind();
                                        /*#CC01 COMMENTED ViewState["FileInfo"] = dtMain;*/
                                        Session["FileInfo"] = dtMain;/*#CC01 ADDED*/
                                    }/* #CC02 Added */
                                }
                                else
                                {
                                    /*#CC01 COMMENTED dtMain = (DataTable)ViewState["FileInfo"];*/
                                    dtMain = (DataTable)Session["FileInfo"];/*#CC01 ADDED*/
                                    dtMain.Rows.Add(FileName, SystemFileName);
                                    /* #CC02 Add Start */
                                    if (validateCarton(dt1, FileName) == false)
                                    {
                                        return;
                                    }
                                    if (processBulkData(dtMain, FileName) == 0)
                                    {
                                        /* #CC02 Add End */
                                        grdvwFile.DataSource = dtMain;
                                        grdvwFile.DataBind();
                                        /*#CC01 COMMENTED ViewState["FileInfo"] = dtMain;*/
                                        Session["FileInfo"] = dtMain;/*#CC01 ADDED*/
                                    }/* #CC02 Added */
                                }
                            }
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Blank file can not be uploaded.";
                    }
                }
                else
                {
                    lblMsg.Text = "Invalid File Format.";
                }
            }
            else
            {
                lblMsg.Text = "Please Select File For Upload.";
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("No error message available"))
            {
                lblMsg.Text = "Please close excel file then upload.";
            }
            else
            {
                //FileUpload1.SaveAs(FilePath);
                lblMsg.Text = ex.Message;
            }
        }
    }





    protected void grdvwFile_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {

    }

    protected void grdvwFile_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                /*#CC01 COMMENTED  if (ViewState["FileInfo"] != null)*/

                if (Session["FileInfo"] != null)/*#CC01 ADDED*/
                {
                    /*#CC01 COMMENTEDDataTable dt = (DataTable)ViewState["FileInfo"];*/
                    DataTable dt = (DataTable)Session["FileInfo"];/*#CC01 ADDED*/
                    if (dt.Rows.Count > 0)
                    {
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = "FileName <>'" + e.CommandArgument.ToString().Trim() + "'";
                        DataTable dtMain = dv.ToTable();
                        grdvwFile.DataSource = dtMain;
                        grdvwFile.DataBind();
                        /*#CC01 COMMENTED ViewState["FileInfo"] = dtMain;*/
                        Session["FileInfo"] = dtMain;/*#CC01 ADDED*/

                        /*Delete Excel File from Folder*/
                        DataRow[] drDelSystemFile = dt.Select("FileName='" + e.CommandArgument.ToString().Trim() + "'");
                        string strDelSystemFile = drDelSystemFile[0]["SystemFileName"].ToString().Trim();
                        string CurrentFilePath = PageBase.strExcelBulkUploadPath + strDelSystemFile;
                        File.Delete(CurrentFilePath);


                        /* #CC03 Add Start */
                        hdnCurrentFileName.Value = e.CommandArgument.ToString().Trim();
                        btnConfCancel_Click(null, null);
                        /* #CC03 Add End */
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }
    /* #CC02 Add Start */
    public DataTable dtSKUCurtonCheck()
    {
        DataTable dtnew = new DataTable();
        DataColumn dc = new DataColumn("SRNO");
        dc.DataType = System.Type.GetType("System.Int32");
        dc.AutoIncrement = true;
        dc.AutoIncrementSeed = 1;
        dtnew.Columns.Add(dc);
        dtnew.Columns.Add("CartonNo", typeof(string));
        dtnew.Columns.Add("SKUCode", typeof(string));
        dtnew.Columns.Add("Qty", typeof(System.Int64));
        dtnew.Columns.Add("ISCartonDuplicate", typeof(string));

        dtnew.Columns.Add("FileName", typeof(string)); /* #CC03 Added */
        return dtnew;
    }
    public bool validateCarton(DataTable dtcheck, string FileName)
    {
        /* #CC02 Add Start */

        if (Session["DTCheckSKUCarton"] == null)
        {
            Session["DTCheckSKUCartonTemp"] = null;
            Session["DTCheckSKUCartonTemp"] = dtSKUCurtonCheck();

            string[] Columns = { "SKUCode", "Carton number" };
            //here we are fetching distinct record//
            DataTable objDtUniqueFile = dtcheck.DefaultView.ToTable(true, Columns);


            /*var vardistinctSKU = (from s in dtcheck.AsEnumerable()
                                  select s["SKUCode"]).Distinct().ToList();
            for (int i = 0; i < vardistinctSKU.Count; i++)*/
            for (int i = 0; i < objDtUniqueFile.Rows.Count; i++)
            {
                DataRow drSKUCarton = ((DataTable)Session["DTCheckSKUCartonTemp"]).NewRow();
                DataRow[] drFilterRows;
                drFilterRows = dtcheck.Select("[Carton number]='" + objDtUniqueFile.Rows[i]["Carton number"] + "'and " + "SKUCode='" + objDtUniqueFile.Rows[i]["SKUCode"] + "'"); /* #CC03 new check of Carton number added Added with skucode */
                drSKUCarton["CartonNo"] = drFilterRows[0]["Carton number"];
                drSKUCarton["SKUCode"] = objDtUniqueFile.Rows[i]["SKUCode"];
                drSKUCarton["Qty"] = drFilterRows.Count();
                drSKUCarton["FileName"] = FileName; /* #CC03 Added */
                ((DataTable)Session["DTCheckSKUCartonTemp"]).Rows.Add(drSKUCarton);
                ((DataTable)Session["DTCheckSKUCartonTemp"]).AcceptChanges();
            }
        }
        else
        {
            Session["DTCheckSKUCartonTemp"] = null;
            Session["DTCheckSKUCartonTemp"] = dtSKUCurtonCheck();
            counter++;
            string[] Columns = { "SKUCode", "Carton number" };
            DataTable objDtUniqueFile = dtcheck.DefaultView.ToTable(true, Columns);
            for (int i = 0; i < objDtUniqueFile.Rows.Count; i++)
            {
                DataRow drSKUCarton = ((DataTable)Session["DTCheckSKUCartonTemp"]).NewRow();
                DataRow[] drFilterRows;
                drFilterRows = dtcheck.Select("[Carton number]='" + objDtUniqueFile.Rows[i]["Carton number"] + "'and " + "SKUCode='" + objDtUniqueFile.Rows[i]["SKUCode"] + "'"); /* #CC03 new check of Carton number added Added with skucode */
                drSKUCarton["CartonNo"] = drFilterRows[0]["Carton number"];
                drSKUCarton["SKUCode"] = objDtUniqueFile.Rows[i]["SKUCode"];
                drSKUCarton["Qty"] = drFilterRows.Count();
                drSKUCarton["FileName"] = FileName; /* #CC03 Added */
                ((DataTable)Session["DTCheckSKUCartonTemp"]).Rows.Add(drSKUCarton);
                ((DataTable)Session["DTCheckSKUCartonTemp"]).AcceptChanges();

            }

            #region code commented for temp bases
            /*  var vardistinctSKU2 = (from s in dtcheck.AsEnumerable()
                                    select s["SKUCode"]).Distinct().ToList();
              for (int m = 0; m < vardistinctSKU2.Count; m++)
              {
                  DataRow[] drFilterRows;
                  //drFilterRows = dtcheck.Select("SKUCode='" + vardistinctSKU2[m] + "'");

                  ////foreach (DataRow drcheck2 in ((DataTable)Session["DTCheckSKUCarton"]).Rows )
                  //for (int n = 0; n < ((DataTable)Session["DTCheckSKUCarton"]).Rows.Count; n++)
                  //{
                  //    if (((DataTable)Session["DTCheckSKUCarton"]).Rows[n]["CartonNo"] == drFilterRows[0]["Carton number"].ToString())
                  //    {

                  //    }
                  //}


              }*/
            #endregion code commented for temp bases
        }

        var vardistinctCartonNo = (from r in ((DataTable)Session["DTCheckSKUCartonTemp"]).AsEnumerable()
                                   select r["CartonNo"]).Distinct().ToList();
        for (int j = 0; j < vardistinctCartonNo.Count; j++)
        {
            DataRow[] drCheckCartonUnique;
            drCheckCartonUnique = ((DataTable)Session["DTCheckSKUCartonTemp"]).Select("CartonNo='" + vardistinctCartonNo[j] + "'");
            // int k = 0;
            if (drCheckCartonUnique.Count() > 1)
            {
                for (int l = 0; l < ((DataTable)Session["DTCheckSKUCartonTemp"]).Rows.Count; l++)
                {
                    if (((DataTable)Session["DTCheckSKUCartonTemp"]).Rows[l]["CartonNo"].ToString() == drCheckCartonUnique[0]["CartonNo"].ToString())
                    {
                        ((DataTable)Session["DTCheckSKUCartonTemp"]).Rows[l]["ISCartonDuplicate"] = 1;
                        ((DataTable)Session["DTCheckSKUCartonTemp"]).AcceptChanges();
                    }
                }
            }
            else
            {
                drCheckCartonUnique[0]["ISCartonDuplicate"] = 0;
            }
            ((DataTable)Session["DTCheckSKUCartonTemp"]).AcceptChanges();

        }
        if (counter > 0)
        {
            int checkrepeater = 0;
            foreach (DataRow dr in ((DataTable)Session["DTCheckSKUCarton"]).Rows)
            {
                for (int p = 0; p < ((DataTable)Session["DTCheckSKUCartonTemp"]).Rows.Count; p++)
                {
                    if (((DataTable)Session["DTCheckSKUCartonTemp"]).Rows[p]["CartonNo"].ToString() == dr["CartonNo"].ToString())
                    {
                        ((DataTable)Session["DTCheckSKUCartonTemp"]).Rows[p]["ISCartonDuplicate"] = 1;
                        ((DataTable)Session["DTCheckSKUCartonTemp"]).AcceptChanges();
                        checkrepeater = 1;
                    }
                    /* else
                     {
                         ((DataTable)Session["DTCheckSKUCartonTemp"]).Rows[p]["ISCartonDuplicate"] = 0;
                         ((DataTable)Session["DTCheckSKUCartonTemp"]).AcceptChanges();
                     }*/
                }
            }
            //  ((DataTable)Session["DTCheckSKUCarton"]).Merge((DataTable)Session["DTCheckSKUCartonTemp"]);
            //ViewState["DTCheckSKUCartonTemp"] = null;

        }
        else
        {
            Session["DTCheckSKUCarton"] = ((DataTable)Session["DTCheckSKUCartonTemp"]);
            //Session["DTCheckSKUCartonTemp"] = null;
        }


        return ShowError((DataTable)Session["DTCheckSKUCartonTemp"]) == true ? true : false;
        #region extra code
        /*
    DataTable dtError = new DataTable();
    DataView dv = new DataView((DataTable)Session["DTCheckSKUCarton"]);
    dv.RowFilter = "ISCartonDuplicate=1";
    dtError = dv.ToTable();
    dtError.Columns.Remove("SRNO");
    dtError.Columns.Remove("Qty");
    dtError.Columns["ISCartonDuplicate"].ColumnName = "ErrorMsg";
    foreach (DataRow drError in dtError.Rows)
    {
        drError["ErrorMsg"] = "Duplicate Carton No.";
    }
    DataSet dsError = new DataSet();
    dsError.Tables.Add(dtError);
    string Errormsg = dsError.GetXml();

    this.Page.GetType().InvokeMember("DisplayMessage", System.Reflection.BindingFlags.InvokeMethod, null, this.Page, new object[] { Errormsg });

    return dtError.Rows.Count > 0 ? false : true;*/
        #endregion extra code
    }
    public bool ShowError(DataTable dtcheckError)
    {
        DataTable dtError = new DataTable();
        //DataView dv = new DataView((DataTable)Session["DTCheckSKUCarton"]);
        DataView dv = new DataView(dtcheckError);
        dv.RowFilter = "ISCartonDuplicate=1";
        dtError = dv.ToTable();


        dtError.Columns.Remove("SRNO");
        dtError.Columns.Remove("Qty");

        dtError.Columns.Remove("FileName");/* #CC03 Added */
        dtError.Columns["ISCartonDuplicate"].ColumnName = "ErrorMsg";
        foreach (DataRow drError in dtError.Rows)
        {
            drError["ErrorMsg"] = "Duplicate Carton No.";
        }
        DataSet dsError = new DataSet();
        dsError.Tables.Add(dtError);
        string Errormsg = dsError.GetXml();
        string MsgType = string.Empty;
        MsgType = "ShowXMLError";
        /*this.Page.GetType().InvokeMember("DisplayMessage", System.Reflection.BindingFlags.InvokeMethod, null, this.Page, new object[] { MsgType, Errormsg });*/
        if (dtError.Rows.Count > 0) /* #CC04 Added */
            ShowMsg(MsgType, Errormsg);

        if (dtError.Rows.Count == 0)
        {
            ((DataTable)Session["DTCheckSKUCarton"]).Merge((DataTable)Session["DTCheckSKUCartonTemp"]);
        }
        /* #CC03 Add Start */
        if (dtError.Rows.Count > 0)
        {
            btnConfCancel_Click(null, null);
        }
        /* #CC03 Add End */
        return dtError.Rows.Count > 0 ? false : true;

    }

    public void ShowMsg(string message, string msgtype)
    {
        this.Page.GetType().InvokeMember("DisplayMessage", System.Reflection.BindingFlags.InvokeMethod, null, this.Page, new object[] { message, msgtype });

    }


    public DataTable BuildDataTableFromXml(string Name, string XMLString)
    {
        DataTable Dt = new DataTable();
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(XMLString));

            XmlNode NodoEstructura = doc.FirstChild.FirstChild;
            foreach (XmlNode columna in NodoEstructura.ChildNodes)
            {
                Dt.Columns.Add(columna.Name, typeof(String));
            }
            XmlNode Filas = doc.FirstChild;
            foreach (XmlNode Fila in Filas.ChildNodes)
            {
                List<string> Valores = new List<string>();
                foreach (XmlNode Columna in Fila.ChildNodes)
                {
                    Valores.Add(Columna.InnerText);
                }
                Dt.Rows.Add(Valores.ToArray());
            }
        }
        catch (Exception ex)
        {

        }
        return Dt;
    }
    public int processBulkData(DataTable dtMain, string FileName)
    {
        Int16 result = 10;
        try
        {
            ViewState["FileInfo"] = dtMain;
            WarehouseTranaction objWarehouse = new WarehouseTranaction();
            DataTable dterror = new DataTable();
            string str = string.Empty;
            string XmlDetail = "";

            DataSet dsTempCurtonSku = new DataSet();
            dsTempCurtonSku.Tables.Add(((DataTable)Session["DTCheckSKUCartonTemp"]));
            XmlDetail = dsTempCurtonSku.GetXml();
            objWarehouse.CartonSkUXML = XmlDetail;
            objWarehouse.GetCartonSKUDetail();
            result = objWarehouse.Result;
            if (result == 2)
            {

                dterror = BuildDataTableFromXml("DTName", objWarehouse.CartonSkUXML);

                foreach (DataRow dr in dterror.Rows)
                {
                    str = str + dr["ErrorData"] + "\\n";
                }
                str = "Carton size is less than defined carton size in server \\n\\n" + str + "\\nPlease confirm if you want to proceed with same sheet?";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "Confirm", "showConfirm('" + str + "','" + FileName + "');", true);

            }
            else if (result == 1)
            {
                /* #CC03 Add Start */
                hdnCurrentFileName.Value = FileName;
                btnConfCancel_Click(null, null);
                /* #CC03 Add End */
                ShowMsg("ShowXMLError", objWarehouse.CartonSkUXML);
                //return;
            }
        }
        catch (Exception ex)
        {

        }

        return result;
    }

    /* #CC02 Add End */

    protected void btnAdd2_Click(object sender, EventArgs e)
    {
        try
        {
            grdvwFile.DataSource = (DataTable)ViewState["FileInfo"];
            grdvwFile.DataBind();
            Session["FileInfo"] = (DataTable)ViewState["FileInfo"];
        }
        catch (Exception ex)
        {

        }
    }

    /* #CC03 Add Start */
    protected void btnConfCancel_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = ((DataTable)Session["DTCheckSKUCarton"]);
            DataTable dtSessionFileName = (DataTable)Session["FileInfo"];
            Session["DTCheckSKUCarton"] = null;

            string strFilename = hdnCurrentFileName.Value;

            DataRow[] drRemoveFileData;
            drRemoveFileData = dt.Select("FileName='" + strFilename + "'");
            for (int i = 0; i < drRemoveFileData.Length; i++)
                drRemoveFileData[i].Delete();
            dt.AcceptChanges();


            #region Remove from Session

            DataRow[] drRemoveFileName;
            drRemoveFileName = dtSessionFileName.Select("FileName='" + strFilename + "'");
            for (int i = 0; i < drRemoveFileName.Length; i++)
                drRemoveFileName[i].Delete();
            dtSessionFileName.AcceptChanges();
            Session["FileInfo"] = dtSessionFileName;

            #endregion Remove from Session

            dt.AcceptChanges();
            Session["DTCheckSKUCarton"] = dt;


        }
        catch (Exception ex)
        {

        }
    }
    /* #CC03 Add End */



}
