/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 22-Mar-2016, Sumit Maurya, #CC01, Issue of page getting blocked on the execution of event(s) duue to update panel resolved.
 * 14-Mar-2018, Sumit Maurya, #CC02, default button set as it was firing chnage status while pressing enter button.
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

public partial class Masters_HO_Common_ManageModel : PageBase

{
    

    DataTable modelinfo;
    DataTable modelserch = new DataTable(); 



    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Page.Form.DefaultButton = btnshowall.UniqueID; /* #CC02 Added */
            fillcombos();
            databind();
            chkstatus.Checked = true;
            BindEnumToListControls(typeof(EnumData.ModelType), ddlModeltype);
            BindEnumToListSerialisedMode(typeof(EnumData.SerializedMode), ddlModelMode);
            ddlModelMode.SelectedIndex = 0;

        }
    }


    # region user defined functions

    public void databind()
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                ucMessage1.Visible = false;
                blankinserttext();
                objproduct.ModelName = txtserName.Text.Trim();
                objproduct.ModelCode = txtserCode.Text.Trim();
                objproduct.ModelId = 0;

                if (cmbserBrand.SelectedValue != "0")
                {
                    objproduct.ModelBrandId = Convert.ToInt16(cmbserBrand.SelectedValue.ToString());
                }
                else
                {
                    objproduct.ModelBrandId = 0;

                }

                if (cmbSerProdCat.SelectedValue != "0")
                {
                    objproduct.ModelProdCatId = Convert.ToInt16(cmbSerProdCat.SelectedValue.ToString());
                }
                else
                {
                    objproduct.ModelProdCatId = 0;
                }

                if (cmbserproduct.SelectedValue != "0")
                {
                    objproduct.ModelProdId = Convert.ToInt16(cmbserproduct.SelectedValue.ToString());
                }
                else
                {
                    objproduct.ModelProdId = 0;
                }
                objproduct.ModelSelectionMode = 2;
                modelserch = objproduct.SelectModelInfo();
                //ViewState["Table"] = modelserch;      //Pankaj Dhingra
                grdModel.DataSource = modelserch;
                grdModel.DataBind();
                blankinserttext();
                updgrid.Update();
                updAddUserMain.Update();

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

                DataTable dtbrandfil = objproduct.SelectAllBrandInfo();
                String[] colArray = { "BrandID", "BrandName" };
                PageBase.DropdownBinding(ref cmbInsertBrand, dtbrandfil, colArray);
                PageBase.DropdownBinding(ref cmbserBrand, dtbrandfil, colArray);
                cmbInsertBrand.SelectedValue = "0";
                cmbserBrand.SelectedValue = "0";

                DataTable dtprodfil = objproduct.SelectAllProductInfo();
                String[] colArray1 = { "ProductID", "ProductName" };
                PageBase.DropdownBinding(ref cmbinsertProduct, dtprodfil, colArray1);
                PageBase.DropdownBinding(ref cmbserproduct, dtprodfil, colArray1);
                cmbinsertProduct.SelectedValue = "0";
                cmbserproduct.SelectedValue = "0";


                DataTable dtprodcatfil = objproduct.SelectAllProdCatInfo();
                String[] colArray2 = { "ProductCategoryID", "ProductCategoryName" };
                PageBase.DropdownBinding(ref cmbInsertProdCat, dtprodcatfil, colArray2);
                PageBase.DropdownBinding(ref cmbSerProdCat, dtprodcatfil, colArray2);
                cmbInsertProdCat.SelectedValue = "0";
                cmbSerProdCat.SelectedValue = "0";
                updAddUserMain.Update();
                updgrid.Update();
           
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
        txtserName.Text = "";
        txtserCode.Text = "";
        cmbserBrand.SelectedIndex = 0;
        cmbserproduct.SelectedIndex = 0;
        cmbSerProdCat.SelectedIndex = 0;
        UpdSearch.Update();
        

    }


    public void blankinserttext()
    {

        txtModelCode.Text = "";
        txtModelName.Text = "";
        cmbInsertBrand.SelectedIndex = 0;
        cmbinsertProduct.SelectedIndex = 0;
        cmbInsertProdCat.SelectedIndex = 0;
        ddlModelMode.SelectedIndex = 0;
        ddlModeltype.SelectedValue = EnumData.ModelType.Saleable.ToString(); 
        btnSubmit.Text = "Submit";
        updAddUserMain.Update();
    }

    public bool insertvalidate()
    {
        if (cmbInsertBrand.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a State");
            return false;
        }
        if (cmbinsertProduct.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a District");
            return false;
        }
        if (cmbInsertProdCat.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a District");
            return false;
        }

        if (txtModelCode.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Model Code");
            return false;
        }
        if (txtModelName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Model Name");
            return false;
        }
        return true;
    }


    # endregion 


    # region control functions 

    protected void btninsert_click(object sender, EventArgs e)
    {
        EnumData.SerializedMode enumSerializedMode = (EnumData.SerializedMode)Enum.Parse(typeof(EnumData.SerializedMode), ddlModelMode.SelectedValue.Replace(" ", "_"));
        Int16 value = (Int16)enumSerializedMode;
        if (IsPageRefereshed == true)
        {
           return;
        }           //Pankaj Dhingra
        using (ProductData objproduct = new ProductData())
        {

            updAddUserMain.Update();
            if (!insertvalidate())
            {
                return;
            }
            else
            {
                objproduct.ModelName = txtModelName.Text.Trim();
                objproduct.ModelCode = txtModelCode.Text.Trim();
                objproduct.ModelBrandId = Convert.ToInt16(cmbInsertBrand.SelectedValue.ToString());
                objproduct.ModelProdId = Convert.ToInt16(cmbinsertProduct.SelectedValue.ToString());
                objproduct.ModelProdCatId = Convert.ToInt16(cmbInsertProdCat.SelectedValue.ToString());
                objproduct.ModelType = Convert.ToInt32((EnumData.ModelType)Enum.Parse(typeof(EnumData.ModelType), ddlModeltype.SelectedValue));
                objproduct.ModelMode = value;
                //Convert.ToInt32((EnumData.SerializedMode)Enum.Parse(typeof(EnumData.SerializedMode), ddlModelMode.SelectedValue)); 
                if (chkstatus.Checked == true)
                {
                    objproduct.ModelStatus = 1;
                }
                else
                {
                    objproduct.ModelStatus = 0;
                }

                if (ViewState["ModelID"] == null || (int)ViewState["ModelID"] == 0)
                {
                    try
                    {

                        objproduct.error = "";
                        objproduct.InsertModelInfo();
                        if (objproduct.error == "")
                        {
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blanksertext();
                            modelinfo = objproduct.SelectAllModelInfo();
                            grdModel.DataSource = modelinfo;
                            grdModel.DataBind();

                            updgrid.Update();
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
                }
                else
                {


                    try
                    {

                        objproduct.error = "";
                        objproduct.ModelId = (int)ViewState["ModelID"];


                        objproduct.UpdateModelInfo();
                        if (objproduct.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            blankinserttext();

                            ViewState["ModelID"] = null;




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
                }

            }
        }
    }





    protected void btncancel_Click(object sender, EventArgs e)
    {
        blanksertext();
             blankinserttext();
             databind();
          
             ucMessage1.Visible = false;


    }
    protected void btnSerchBodel_Click(object sender, EventArgs e)
    {
        blankinserttext();
        if (txtserCode.Text == "" && txtserName.Text == "" && cmbserBrand.SelectedValue == "0"
            && cmbSerProdCat.SelectedValue == "0" && cmbserproduct.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please enter atleast one searching parameter ");
            return;
        }

        databind();
    }
    protected void grdModel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinserttext();
                    
                    objproduct.ModelId = Convert.ToInt32(e.CommandArgument);
                    objproduct.ModelSelectionMode = 2;
                    modelinfo = objproduct.SelectModelInfo();
                    objproduct.ModelName = Convert.ToString(modelinfo.Rows[0]["ModelName"]);
                    objproduct.ModelCode = Convert.ToString(modelinfo.Rows[0]["ModelCode"]);
                    objproduct.ModelProdId = Convert.ToInt16(modelinfo.Rows[0]["ProductID"]);
                    objproduct.ModelProdCatId = Convert.ToInt16(modelinfo.Rows[0]["ProductCategoryID"]);
                    objproduct.ModelBrandId = Convert.ToInt16(modelinfo.Rows[0]["BrandID"]);
                    objproduct.ModelStatus = Convert.ToInt16(modelinfo.Rows[0]["Status"]);
                    objproduct.ModelType = Convert.ToInt32(modelinfo.Rows[0]["ModelTypeID"]);
                    objproduct.ModelMode = Convert.ToInt32(modelinfo.Rows[0]["ModelModeID"]);
                    if (objproduct.ModelStatus == 1)
                    {
                        objproduct.ModelStatus = 0;
                    }
                    else
                    {
                        objproduct.ModelStatus = 1;
                    }
                    objproduct.error = "";
                    objproduct.UpdateModelInfo();
                    if (objproduct.error == "")
                    {
                        databind();
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
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


            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    ucMessage1.Visible = false;
                    objproduct.ModelId = Convert.ToInt32(e.CommandArgument);
                    ViewState["ModelID"] = objproduct.ModelId;
                    objproduct.ModelSelectionMode = 2;
                    modelinfo = objproduct.SelectModelInfo();

                    txtModelName.Text = Convert.ToString(modelinfo.Rows[0]["ModelName"]);
                    txtModelCode.Text = Convert.ToString(modelinfo.Rows[0]["ModelCode"]);
                    objproduct.ModelStatus = Convert.ToInt16(modelinfo.Rows[0]["Status"]);


                    if (cmbinsertProduct.Items.FindByValue(modelinfo.Rows[0]["ProductID"].ToString()) != null)
                    {
                        cmbinsertProduct.ClearSelection();
                    }
                    cmbinsertProduct.Items.FindByValue(modelinfo.Rows[0]["ProductID"].ToString()).Selected = true;
                    if (cmbInsertProdCat.Items.FindByValue(modelinfo.Rows[0]["ProductCategoryID"].ToString()) != null)
                    {
                        cmbInsertProdCat.ClearSelection();
                    }
                    cmbInsertProdCat.Items.FindByValue(modelinfo.Rows[0]["ProductCategoryID"].ToString()).Selected = true;
                    if (cmbInsertBrand.Items.FindByValue(modelinfo.Rows[0]["BrandID"].ToString()) != null)
                    {
                        cmbInsertBrand.ClearSelection();
                    }
                    
                    cmbInsertBrand.Items.FindByValue(modelinfo.Rows[0]["BrandID"].ToString()).Selected = true;
                    ddlModelMode.SelectedValue = GetSerializedMode(modelinfo.Rows[0]["ModelModeID"]);
                    //((EnumData.SerializedMode)Enum.Parse(typeof(EnumData.SerializedMode), modelinfo.Rows[0]["ModelModeID"].ToString())).ToString();
                    ddlModeltype.SelectedValue = ((EnumData.ModelType)Enum.Parse(typeof(EnumData.ModelType), modelinfo.Rows[0]["ModelTypeID"].ToString())).ToString(); 
                    //Pankaj Dhingra

                    chkstatus.Checked = Convert.ToBoolean(objproduct.ModelStatus);


                    btnSubmit.Text = "Update";

                    updAddUserMain.Update();

                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }
            }

        }


    }

    protected void btnshowalldata_Click(object sender, EventArgs e)
    {

        blanksertext();
        blankinserttext();
        databind();

        ucMessage1.Visible = false;

    }
    protected void grdModel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdModel.PageIndex = e.NewPageIndex;
        databind();
    }

    # endregion 

    # region export to excel 

    protected void exportToExel_Click(object sender, EventArgs e)
    {

        //if (ViewState["Table"] != null)
        //{
        //DataTable dt = (DataTable)ViewState["Table"];
        databind();     //Pankaj Dhingra
        DataTable dt = modelserch.Copy();
        string[] DsCol = new string[] { "ModelCode", "ModelName", "ProductCategoryName", "ProductName", "BrandName", "CurrentStatus", "ModelType", "ModelMode" };
        DataTable DsCopy = new DataTable();
        dt = dt.DefaultView.ToTable(true, DsCol);
        dt.Columns["CurrentStatus"].ColumnName = "Status";
        if (dt.Rows.Count > 0)
        {
            try
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "ModelDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                //ViewState["Table"] = null;
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }
        else
        {
            ucMessage1.ShowError(Resources.Messages.NoRecord);
        }
        //ViewState["Table"] = null;
        //}
    }

    # endregion 
  
    protected void cmbInsertProdCat_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
   
    public void BindEnumToListSerialisedMode(Type enumType, DropDownList ddlControl)
    {
        try
        {
            ddlControl.DataSource = GetEnumCollection();
            ddlControl.DataBind();
          
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());

        }
    }
    public void BindEnumToListControls(Type enumType, DropDownList ddlControl)
    {
        try
        {
            string[] names;
            Array values;
            int countElements;
            names = Enum.GetNames(enumType);
            values = Enum.GetValues(enumType);
            for (countElements = 0; countElements <= names.Length - 1; countElements++)
            {
                ddlControl.Items.Add(new ListItem(names[countElements].ToString(), values.GetValue(countElements).ToString()));
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }

    }
    public List<String> GetEnumCollection()
    {
        List<string> lst = new List<string>();
        string[] strArray = Enum.GetNames(typeof(EnumData.SerializedMode));

        if (strArray != null)
        {
            foreach (string val in strArray)
            {
                lst.Add(val.Replace("_", " "));
            }
        }
        return lst;
    }
    public string GetSerializedMode(object obj)
    {
        EnumData.SerializedMode EnumSerializedMode = (EnumData.SerializedMode)Enum.Parse(typeof(EnumData.SerializedMode), obj.ToString());
       string value = (string)EnumSerializedMode.ToString().Replace("_", " ");
        return value;
    }

}
