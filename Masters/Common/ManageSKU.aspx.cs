/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 22-Mar-2016, Sumit Maurya, #CC01, Issue of page getting blocked on the execution of event(s) duue to update panel resolved.
 * 31-May-2016, Karam Chand Sharma, #CC02,  Insert and Update Counter Size base on applicationconfiguration master  'Config Key : SHOWCOUNTERSIZE'
 * 14-Mar-2018, Sumit Maurya, #CC03, default button set as it was firing chnage status while pressing enter button.
 * 05-Mar-2019,Vijay Kumar Prajapati,#CC04,Added Keyword (Done for brother)
 * 30-May-2019,Vijay Kumar Prajapati,#CC05,Added ExpiryDate Status (Done for Shivalik.)
====================================================================================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;
using ActiveXLS;
using ActiveXLS.Constants;

public partial class Masters_HO_Common_ManageSKU : PageBase
{



    DataTable skuinfo;
    int mode3 = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        CheckValidationColor();
        if (!IsPostBack)
        {
            Page.Form.DefaultButton = fillallgrid.UniqueID; /* #CC03 Added */            
            fillcombos();
            databind();
            chkstatus.Checked = true;
           
            
        }
        /* #CC02 START ADDED */
        if (PageBase.SHOWCARTONSIZE == 1)
        {
            dvCounterSize.Style.Add("display", "block");
            if (PageBase.REQCARTONSIZE == 0)
            {
                ReqtxtCartonSize.ValidationGroup = "temp";
                dvReq.Style.Add("display", "none");
            }
        }
        else
        {
            grdSKU.Columns[10].Visible = false;
            dvReq.Style.Add("display", "none");
            ReqtxtCartonSize.ValidationGroup = "temp";
            dvCounterSize.Style.Add("display", "none");
        }
        /* #CC02 START end */
        /*#CC05 Added Started*/
        if(PageBase.SHOWExpiryDate==1)
        {
            dvExpiryStatus.Visible = true;
        }
        else
        {
            dvExpiryStatus.Visible = false;
        }
        /*#CC05 Added End*/
    }


    # region user defined functions

    public void databind()
    {
        using (ProductData objproduct = new ProductData())
        {
            try
            {
                ucMessage1.Visible = false;
                objproduct.SKUName = txtSerName.Text.Trim();
                objproduct.SKUCode = txtSerCode.Text.Trim();

                if (cmbSerModel.SelectedValue != "0")
                {
                    objproduct.SKUModelId = Convert.ToInt16(cmbSerModel.SelectedValue.ToString());
                }
                else
                {
                    objproduct.SKUModelId = 0;
                }
                if (cmbSerProdCat.SelectedValue != "0")
                {
                    objproduct.SKUProdCatId = Convert.ToInt16(cmbSerProdCat.SelectedValue.ToString());
                }
                else
                {
                    objproduct.SKUProdCatId = 0;
                }
                if (cmbSercolor.SelectedValue != "0")
                {
                    objproduct.SKUColorId = Convert.ToInt16(cmbSercolor.SelectedValue.ToString());
                }
                else
                {
                    objproduct.SKUColorId = 0;
                }
                if (mode3 == 3)
                {
                    objproduct.SKUSelectionMode = 3;
                    skuinfo = objproduct.SelectSKUInfo();
                    return;
                }
                else
                {
                    objproduct.SKUSelectionMode = 2;
                }
                skuinfo = objproduct.SelectSKUInfo();


                grdSKU.DataSource = skuinfo;
                grdSKU.DataBind();
                updgrid.Update();
                //ViewState["Table"] = skuinfo;

            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }

    }



    public void fillcombos()
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                DataTable dt = objproduct.SelectAllProdCatInfo();
                DataTable ds = objproduct.SelectAllColorInfo();
                cmbInsertProdCat.Items.Clear();
                cmbSerProdCat.Items.Clear();
                cmbInsertColor.Items.Clear();
                cmbSercolor.Items.Clear();
                String[] colArray1 = { "ProductCategoryID", "ProductCategoryName" };
                String[] colArray = { "ColorID", "ColorName" };
                PageBase.DropdownBinding(ref cmbInsertProdCat, dt, colArray1);
                PageBase.DropdownBinding(ref cmbSerProdCat, dt, colArray1);
                PageBase.DropdownBinding(ref cmbInsertColor, ds, colArray);
                PageBase.DropdownBinding(ref cmbSercolor, ds, colArray);
                cmbInsertModel.Items.Insert(0, new ListItem("Select", "0"));
                cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
    }

    public void blanksertext()
    {
        txtSerName.Text = "";
        txtSerCode.Text = "";
        cmbSercolor.SelectedIndex = 0;
        cmbSerModel.Items.Clear();
        cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
        /* #CC02 START ADDED */
        if (PageBase.SHOWCARTONSIZE == 1)
            txtCartonSize.Text = "";
        /* #CC02 START END */
        cmbSerProdCat.SelectedIndex = 0;
        UpdSearch.Update();

    }

    public void blankinserttext()
    {

        txtInsertCode.Text = "";
        txtInsertName.Text = "";
        txtProperty1.Text = "";
        txtProperty2.Text = "";
        txtInsertDesc.Text = "";
        /* #CC02 START ADDED */
        if (PageBase.SHOWCARTONSIZE == 1)
            txtCartonSize.Text = "";
        /* #CC02 START END */
        cmbInsertColor.SelectedIndex = 0;
        cmbInsertModel.Items.Clear();
        cmbInsertModel.Items.Insert(0, new ListItem("Select", "0"));
        txtKeyword.Text = "";
        cmbInsertProdCat.SelectedIndex = 0;
        btnSubmit.Text = "Submit";
        updAddUserMain.Update();

    }

    public bool insertvalidate()
    {
        if (cmbInsertModel.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a Model");
            return false;
        }
        if (Session["SKUCOLORREQD"] != null)
        {
            if (Convert.ToInt16(Session["SKUCOLORREQD"]) == 1 && cmbInsertColor.SelectedIndex==0)
            {
                ucMessage1.ShowInfo("Please select a color");
                return false;
            }
           
        }
        if (cmbInsertProdCat.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a product category");
            return false;
        }

        if (txtInsertCode.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert SKU Code");
            return false;
        }
        if (txtInsertName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert SKU Name");
            return false;
        }
        /*#CC04 start*/
        string KeywordNumber = txtKeyword.Text.Trim();
        KeywordNumber = KeywordNumber.Replace("\r\n", ",");
        if (KeywordNumber.Length > 250)
        {
            ucMessage1.ShowInfo("All keywords length cannot be greater than 250.");
            return false;
        }
        string[] arKeyword = KeywordNumber.Split(new Char[] {','});
        for (int cnt = 0; cnt < arKeyword.Length;cnt++ )
        {
            if (arKeyword[cnt].Length > 50)
            {
                ucMessage1.ShowInfo("Single keywords length cannot be greater than 50.");
                return false;
            }
        }
            /*#CC04 end*/
            return true;
    }
    private void CheckValidationColor()
    {
        if (Session["SKUCOLORREQD"] != null)
        {
            if (Convert.ToInt16(Session["SKUCOLORREQD"]) == 0)
            {
                valColor.Enabled = false;
            }
            else
            {
                valColor.Enabled = true;
            }
        }
    }
    # endregion

    # region control functions

    protected void btninsert_click(object sender, EventArgs e)
    {
          
        if (IsPageRefereshed == true)
        {
            return;
        }           //Pankaj Dhingra
        /*#CC04 start*/
        string KeywordNumber = txtKeyword.Text.Trim();
        KeywordNumber = KeywordNumber.Replace("\r\n", ",");
        
        /*#CC04 end*/
        using (ProductData objproduct = new ProductData())
        {
            if (!insertvalidate())
            {
                return;
            }
            else
            {

                updAddUserMain.Update();
                objproduct.SKUName = txtInsertName.Text.Trim();
                objproduct.SKUCode = txtInsertCode.Text.Trim();
                objproduct.SKUModelId = Convert.ToInt16(cmbInsertModel.SelectedValue.ToString());
                objproduct.SKUAttribute1 = txtProperty1.Text.Trim();
                objproduct.SKUAttribute2 = txtProperty2.Text.Trim();
                objproduct.SKUColorId = Convert.ToInt16(cmbInsertColor.SelectedValue.ToString()) == 0 ? objproduct.SKUColorId : Convert.ToInt16(cmbInsertColor.SelectedValue.ToString());
                objproduct.SKUProdCatId = Convert.ToInt16(cmbInsertProdCat.SelectedValue.ToString());
                objproduct.SKUDesc = txtInsertDesc.Text.Trim();
                objproduct.KeyWord = KeywordNumber;/*#CC04 Added*/
                /* #CC02 START ADDED */
                if (PageBase.SHOWCARTONSIZE == 1)
                {
                    if (txtCartonSize.Text != "")
                        objproduct.CartonSIze = Convert.ToInt32(txtCartonSize.Text);
                }
                /* #CC02 START END */
                if (chkstatus.Checked == true)
                {
                    objproduct.SKUStatus = 1;
                }
                else
                {
                    objproduct.SKUStatus = 0;
                }
                /*#CC05 Added Started*/
                if (PageBase.SHOWExpiryDate==1)
                {
                    if (ChkExpiryStatus.Checked == true)
                    {
                        objproduct.SKUExpiryDateStatus = 1;
                    }
                    else
                    {
                        objproduct.SKUExpiryDateStatus = 0;
                    }
                }
                else
                {
                    objproduct.SKUExpiryDateStatus = 0;
                }
                /*#CC05 Added End*/
                if (ViewState["SKUID"] == null || (int)ViewState["SKUID"] == 0)
                {
                    objproduct.SKUId = 0; 
                }
                else
                    objproduct.SKUId = (int)ViewState["SKUID"];
                try
                {

                    objproduct.error = "";
                    objproduct.InsertSKUInfo();
                    if (objproduct.error == "")
                    {
                        databind();
                        ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        blankinserttext();

                    }


                    else
                    {

                        ucMessage1.ShowInfo(objproduct.error);
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }
                /*}

                else
                {
                    try
                    {

                        updAddUserMain.Update();


                        objproduct.error = "";
                        objproduct.SKUId = (int)ViewState["SKUID"];


                        //objproduct.UpdateSKUInfo();#CC04 comented
                        objproduct.InsertSKUInfo();//#CC04 Added in both function same procedure is called
                        if (objproduct.error == "")
                        {
                            databind();
                            blankinserttext();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);



                            ViewState["SKUID"] = null;



                        }
                        else
                        {

                            ucMessage1.ShowInfo(objproduct.error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                        PageBase.Errorhandling(ex);
                    }
                }*/
            }
        }

    }



    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {
            blanksertext();
            blankinserttext();
            databind();
            ucMessage1.Visible = false;
            txtInsertCode.Enabled = true;       //Pankaj Dhingra
            ViewState["SKUID"] = 0;//#CC04 added
            /*#CC05 Added Started*/
            if (PageBase.SHOWExpiryDate == 1)
            {
                ChkExpiryStatus.Checked = false;
            }
            /*#CC05 Added End*/
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }



    protected void btnSerchSku_Click(object sender, EventArgs e)
    {
        if (txtSerCode.Text == "" && txtSerName.Text == "" && cmbSercolor.SelectedValue == "0"
           && cmbSerProdCat.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please enter atleast one searching parameters");
            return;
        }

        blankinserttext();
        databind();
    }




    protected void grdSKU_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinserttext();


                    objproduct.SKUId = Convert.ToInt32(e.CommandArgument);
                    objproduct.SKUSelectionMode = 2;
                    skuinfo = objproduct.SelectSKUInfo();
                    objproduct.SKUName = Convert.ToString(skuinfo.Rows[0]["SKUName"]);
                    objproduct.SKUCode = Convert.ToString(skuinfo.Rows[0]["SKUCode"]);
                    objproduct.SKUModelId = Convert.ToInt16(skuinfo.Rows[0]["ModelID"]);
                    objproduct.SKUProdCatId = Convert.ToInt16(skuinfo.Rows[0]["ProductCategoryID"]);
                    objproduct.SKUColorId = Convert.ToInt16(skuinfo.Rows[0]["ColorID"]);
                    objproduct.SKUAttribute1 = Convert.ToString(skuinfo.Rows[0]["Attribute1"]);
                    objproduct.SKUAttribute2 = Convert.ToString(skuinfo.Rows[0]["Attribute2"]);
                    objproduct.SKUStatus = Convert.ToInt16(skuinfo.Rows[0]["Status"]);
                    objproduct.SKUExpiryDateStatus = Convert.ToInt32(skuinfo.Rows[0]["ExpiryDateStatus"]);/*#CC05 Added*/

                    if (objproduct.SKUStatus == 1)
                    {
                        objproduct.SKUStatus = 0;
                    }
                    else
                    {
                        objproduct.SKUStatus = 1;
                    }
                    /* #CC02 START ADDED */
                    if (PageBase.SHOWCARTONSIZE == 1)
                        if (txtCartonSize.Text!="")
                        objproduct.CartonSIze = Convert.ToInt32(txtCartonSize.Text);
                    /* #CC02 START END */
                    objproduct.error = "";
                    objproduct.UpdateSKUInfo();
                    if (objproduct.error == "")
                    {
                        databind();
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);

                        updgrid.Update();

                    }

                    else
                    {
                        ucMessage1.ShowInfo(objproduct.error);
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                }


            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {

                    ucMessage1.Visible = false;
                    txtInsertCode.Enabled = false;      //Pankaj Dhingra                 
                    objproduct.SKUId = Convert.ToInt32(e.CommandArgument);
                    ViewState["SKUID"] = objproduct.SKUId;
                    objproduct.SKUSelectionMode = 2;
                    skuinfo = objproduct.SelectSKUInfo();

                    txtInsertName.Text = Convert.ToString(skuinfo.Rows[0]["SKUName"]);
                    txtInsertCode.Text = Convert.ToString(skuinfo.Rows[0]["SKUCode"]);

                    txtProperty1.Text = Convert.ToString(skuinfo.Rows[0]["Attribute1"]);

                    txtProperty2.Text = Convert.ToString(skuinfo.Rows[0]["Attribute2"]);
                    txtInsertDesc.Text = Convert.ToString(skuinfo.Rows[0]["SKUDesc"]);
                    txtKeyword.Text = Convert.ToString(skuinfo.Rows[0]["Keyword"]);//#CC04 added
                    objproduct.SKUStatus = Convert.ToInt16(skuinfo.Rows[0]["Status"]);
                    objproduct.SKUExpiryDateStatus = Convert.ToInt16(skuinfo.Rows[0]["ExpiryDateStatus"]);/*#CC05 Added*/
                    cmbInsertProdCat.ClearSelection();
                    if (cmbInsertProdCat.Items.FindByValue(skuinfo.Rows[0]["ProductCategoryID"].ToString()) != null)
                    {
                        cmbInsertProdCat.Items.FindByValue(skuinfo.Rows[0]["ProductCategoryID"].ToString()).Selected = true;
                    }
                    else
                    {
                        cmbInsertProdCat.SelectedValue = "0";
                    }

                    cmbInsertModel.Items.Clear();
                    insertProdcat_selectedIndexChanged(cmbInsertProdCat, new EventArgs());
                    if (cmbInsertModel.Items.FindByValue(skuinfo.Rows[0]["ModelID"].ToString()) != null)
                    {

                        cmbInsertModel.Items.FindByValue(skuinfo.Rows[0]["ModelID"].ToString()).Selected = true;
                    }
                    else
                    {
                        cmbInsertModel.SelectedValue = "0";

                    }
                    cmbInsertColor.ClearSelection();
                    if (cmbInsertColor.Items.FindByValue(skuinfo.Rows[0]["ColorID"].ToString()) != null)
                    {

                        cmbInsertColor.SelectedValue = skuinfo.Rows[0]["ColorID"].ToString();

                    }
                    else
                    {
                        cmbInsertColor.SelectedValue = "0";
                    }
                    /* #CC02 START ADDED */
                    if (PageBase.SHOWCARTONSIZE == 1)
                        txtCartonSize.Text = skuinfo.Rows[0]["Carton SIze"].ToString();
                    /* #CC02 START END */

                    if (objproduct.SKUStatus == 1)
                    {
                        chkstatus.Checked = true;
                    }
                    else
                    {
                        chkstatus.Checked = false;
                    }
                    /*#CC05 Added Started*/
                    if(PageBase.SHOWExpiryDate==1)
                    {
                        if (objproduct.SKUExpiryDateStatus == 1)
                        {
                            ChkExpiryStatus.Checked = true;
                        }
                        else
                        {
                            ChkExpiryStatus.Checked = false;
                        }
                    }
                    /*#CC05 Added End*/

                    btnSubmit.Text = "Update";
                    updAddUserMain.Update();
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                }
            }
        }
    }


    protected void btngetdata_Click(object sender, EventArgs e)
    {
        blanksertext();
        blankinserttext();
        databind();
        ucMessage1.Visible = false;

    }
    protected void grdSKUpage_indexchanging(object sender, GridViewPageEventArgs e)
    {

        grdSKU.PageIndex = e.NewPageIndex;
        databind();

    }

    #endregion


    # region conbobox index functions

    protected void insertProdcat_selectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (ProductData objproduct = new ProductData())
            {
                if (cmbInsertProdCat.SelectedValue == "0")
                {
                    cmbInsertModel.Items.Clear();
                    cmbInsertModel.Items.Insert(0, new ListItem("Select", "0"));
                    cmbInsertModel.SelectedValue = "0";

                }
                else
                {
                    objproduct.ModelProdCatId = Convert.ToInt16(cmbInsertProdCat.SelectedValue.ToString());
                    objproduct.ModelSelectionMode = 1;
                    DataTable dtmodelfil = objproduct.SelectModelInfo();
                    String[] colArray1 = { "ModelID", "ModelName" };
                    PageBase.DropdownBinding(ref cmbInsertModel, dtmodelfil, colArray1);
                    //cmbInsertModel.SelectedValue = "0";
                    updAddUserMain.Update();

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }



    protected void cmbSerProdcat_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                if (cmbSerProdCat.SelectedValue == "0")
                {
                    cmbSerModel.Items.Clear();
                    cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
                    cmbSerModel.SelectedValue = "0";
                }
                else
                {
                    objproduct.ModelProdCatId = Convert.ToInt16(cmbSerProdCat.SelectedValue.ToString());
                    objproduct.ModelSelectionMode = 1;
                    DataTable dtmodelfil = objproduct.SelectModelInfo();
                    String[] colArray1 = { "ModelID", "ModelName" };
                    PageBase.DropdownBinding(ref cmbSerModel, dtmodelfil, colArray1);
                    cmbSerModel.SelectedValue = "0";
                    UpdSearch.Update();

                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            }
        }
    }

    #endregion


    # region export to Excel


    protected void exportToExel_Click(object sender, EventArgs e)
    {
        // if (ViewState["Table"] != null)
        //{
        //DataTable dt = (DataTable)ViewState["Table"];
        mode3 = 3;
        databind();
        mode3 = 0;
        DataTable dt = skuinfo;
        /*#CC02 START COMMENTED string[] DsCol = new string[] { "SKU Code", "SKU Name", "SKU Description", "Product Category Name", "Product Name", "Color Name", "Brand Name", "Model Name", "Attribute1", "Attribute2", "Current Status" }; #CC02 END COMMENTED */
        /* #CC02 START ADDED */
        if (PageBase.SHOWCARTONSIZE == 0)
        {
            string[] DsCol = new string[] { "SKU Code", "SKU Name", "SKU Description", "Product Category Name", "Product Name", "Color Name", "Brand Name", "Model Name", "Attribute1", "Attribute2", "Current Status", "keywords", "ExpiryDateStatus" };/*#CC05 Added ExpiryDateStatus*/
            dt = dt.DefaultView.ToTable(true, DsCol);
        }
        else
        {
            string[] DsCol = new string[] { "SKU Code", "SKU Name", "SKU Description", "Product Category Name", "Product Name", "Color Name", "Brand Name", "Model Name", "Attribute1", "Attribute2", "Carton Size", "Current Status", "keywords", "ExpiryDateStatus" };/*#CC05 Added ExpiryDateStatus*/
            dt = dt.DefaultView.ToTable(true, DsCol);
        }
        /* #CC02 START END */
        DataTable DsCopy = new DataTable();

        dt.Columns["Current Status"].ColumnName = "Status";
        if (dt.Rows.Count > 0)
        {
            try
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SKUDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
               
                // ViewState["Table"] = null;


              
                //esd_WriteXLSBFile_FromDataSet(System.IO.Stream f,
                //                            System.Data.DataSet ds,
                //                            ExcelAutoFormat xlsAutoFormat,
                //                            System.String sSheetName)
                DataSet obj= new DataSet();
                obj.Tables.Add(dt.Copy());
                //fncExportInExcelAcitveXLS(obj, "C:\\Samples\\Tutorial55.xlsb");
               ActiveXLS.ExcelDocument xls1 = new ActiveXLS.ExcelDocument(1);
                ExcelAutoFormat abc= new ExcelAutoFormat();
                xls1.esd_WriteXLSBFile_FromDataSet("C:\\Samples\\Tutorial55.xlsb",obj, abc,"hello");
               // Response.Redirect()
              //  String sError = xls1.esd_getError();
                //if (sError.Equals(""))
                //    Console.Write("\nFile successfully created. Press Enter to Exit...");
                //else
                //    Console.Write("\nError encountered: " + sError + "\nPress Enter to Exit...");

                //Dispose memory
               // xls1.Dispose();
               // //Create an instance of the object that generates Excel files, having 2 sheets
               // ActiveXLS.ExcelDocument xls = new ActiveXLS.ExcelDocument(2);

        
               // //Set the sheet names
               // xls.esd_getSheetAt(0).setSheetName("First tab");
               // xls.esd_getSheetAt(1).setSheetName("Second tab");

               // //Get the table of the first worksheet
               // ExcelTable xlsFirstTable = ((ExcelWorksheet)xls.esd_getSheetAt(0)).esd_getExcelTable();

               // //Add the cells for header
               // for (int column = 0; column < 5; column++)
               // {
               //     xlsFirstTable.esd_getCell(0, column).setValue("Column " + (column + 1));
               //     xlsFirstTable.esd_getCell(0, column).setDataType(DataType.STRING);
               // }

               // //Add the cells for data
               // for (int row = 0; row < 100; row++)
               // {
               //     for (int column = 0; column < 5; column++)
               //     {
               //         xlsFirstTable.esd_getCell(row + 1, column).setValue("Data " + (row + 1) + ", " + (column + 1));
               //         xlsFirstTable.esd_getCell(row + 1, column).setDataType(DataType.STRING);
               //     }
               // }

               // //Set column widths
               // xlsFirstTable.setColumnWidth(0, 70);
               // xlsFirstTable.setColumnWidth(1, 100);
               // xlsFirstTable.setColumnWidth(2, 70);
               // xlsFirstTable.setColumnWidth(3, 100);
               // xlsFirstTable.setColumnWidth(4, 70);

               // //Generate the file
               //// Console.WriteLine("Writing file C:\\Samples\\Tutorial25.xlsb.");
               // xls.esd_WriteXLSBFile("C:\\Samples\\Tutorial25.xlsb");

                //Confirm generation
                //String sError = xls.esd_getError();
                ////if (sError.Equals(""))
                ////    Console.Write("\nFile successfully created. Press Enter to Exit...");
                ////else
                ////    Console.Write("\nError encountered: " + sError + "\nPress Enter to Exit...");

                ////Dispose memory
                //xls.Dispose();

            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            }
        }
        else
        {
            ucMessage1.ShowError(Resources.Messages.NoRecord);

        }
        // ViewState["Table"] = null;
        //}
    }
    public void fncExportInExcelAcitveXLS(DataSet ds, string ReportName)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ActiveXLS.ExcelDocument xlsdoc = new ActiveXLS.ExcelDocument(1);
                int sheetCount = 0;
                ExcelWorksheet xlsSheet = new ExcelWorksheet();
                ExcelStyle xlsEvenStyle = new ExcelStyle();


                xlsEvenStyle.setBackground(System.Drawing.Color.Khaki);
                xlsEvenStyle.setBold(true);

                ExcelStyle xlsOddStyle = new ExcelStyle();
                xlsOddStyle.setBackground(System.Drawing.Color.DarkKhaki);
                xlsOddStyle.setBold(true);

                ExcelStyle xlsHeadStyle = new ExcelStyle();
                xlsHeadStyle.setBackground(System.Drawing.Color.MistyRose);
                xlsHeadStyle.setBold(true);

                ExcelAutoFormat xlsAutoFormat = new ExcelAutoFormat();
                xlsAutoFormat.setHeaderRowStyle(xlsHeadStyle);

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count <= 1048500)
                {
                    sheetCount = xlsdoc.SheetCount();
                    if (sheetCount > 1)
                    {
                        for (int i = 2; i <= sheetCount; i++)
                        {
                            xlsdoc.esd_removeSheet("Sheet" + i);
                        }
                    }
                    xlsSheet = (ExcelWorksheet)xlsdoc.esd_getSheet("Sheet1");
                 //   xlsSheet.esd_insertDataSet(ds, 0, 0, xlsAutoFormat, true);
                }


                Response.AppendHeader("content-disposition", "attachment; filename=" + ReportName + ".xls");
                Response.ContentType = "application/octetstream";
                Response.Clear();
                ExcelAutoFormat Ab = new ExcelAutoFormat();
                try
                {

                    xlsdoc.esd_WriteXLSBFile_FromDataSet("C:\\Samples\\Tutorial55.xlsb",ds, Ab,"hello");
                }
                catch (Exception ex)
                {
                    //lblMessage.Text = ex.Message;
                    Response.ClearHeaders();
                    Response.ClearContent();
                }

                finally
                {
                    if (xlsdoc != null)
                    { xlsdoc.Dispose(); }
                }

            }
        }
        catch (Exception ex)
        {
            clsException.clsHandleException.fncHandleException(ex, "User Id- '" + Session["webuserid"] + "' User - '" + Session["person_name"] + "'");
          //  lblMessage.Text = ex.Message;
        }
    }

    # endregion

}

