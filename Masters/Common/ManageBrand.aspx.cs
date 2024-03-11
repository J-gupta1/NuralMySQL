/*
 * ----------------------------------------------------------------------------------------
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ----------------------------------------------------------------------------------------
 * 14-Mar-2018, Sumit Maurya, #CC01, default button set as it was firing chnage status while pressing enter button.
 * ----------------------------------------------------------------------------------------
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




public partial class Masters_HO_Common_ManageBrand : PageBase
{   
    DataTable brandinfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Form.DefaultButton = getalldata.UniqueID; /* #CC01 Added */
            databind();
            chkActive.Checked = true;
       }
    }
    # region user defined functions

    public void blankinsert()
    {
        txtBrandCode.Text = "";
        txtBrandName.Text = "";
      
        btnCreate.Text = "Submit";
        updAddUserMain.Update();

    }
    public void blankserch()
    {
        txtSerBrandCode.Text = "";
        txtSerBrandName.Text = "";
        UpdSearch.Update();

    }


    public void databind()
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                ucMessage1.Visible = false;
                objproduct.BrandName = txtSerBrandName.Text;
                objproduct.BrandCode = txtSerBrandCode.Text;
                objproduct.BrandId = 0;
                objproduct.BrandSelectionMode = 2;
                DataTable brandserch = objproduct.SelectBrandInfo();

              
                    
                    ViewState["Table"] = brandserch;
                    grdBrand.DataSource = brandserch;
                    grdBrand.DataBind();
                    blankinsert();
                    updgrid.Update();
                
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    public bool insertvalidate()
    {
        if (txtBrandCode .Text  == "")
        {
            ucMessage1.ShowInfo("Please Insert Brand Code");
            return false;
        }
        if (txtBrandName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Brand Name");
            return false;
        }
        return true;
    }
    
# endregion

    # region control functions 

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }           //Pankaj Kumar
        using (ProductData objproduct = new ProductData())
        {
            updAddUserMain.Update();
            if (!insertvalidate())
            {
                return;
            }
            else
            {
                objproduct.BrandName = txtBrandName.Text;
                objproduct.BrandCode = txtBrandCode.Text;
                if (chkActive.Checked == true)
                {
                    objproduct.BrandStatus = 1;
                }
                else
                {
                    objproduct.BrandStatus = 0;
                }

                if (ViewState["BrandID"] == null || (int)ViewState["BrandID"] == 0)
                {
                    try
                    {

                        blankserch();
                        objproduct.error = "";
                        objproduct.InsertBrandInfo();

                        if (objproduct.error == "")
                        {

                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blankinsert();

                        }
                        else
                        {

                            ucMessage1.ShowInfo(objproduct.error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ucMessage1.ShowInfo(ex.Message.ToString());
                        PageBase.Errorhandling(ex);
                    }
                }
                else
                {

                    try
                    {



                        objproduct.error = "";
                        objproduct.BrandId = (int)ViewState["BrandID"];


                        objproduct.UpdateBrandInfo();
                        if (objproduct.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            blankinsert();

                            ViewState["BrandID"] = null;



                        }
                        else
                        {

                            ucMessage1.ShowInfo(objproduct.error);
                        }
                    }

                    catch (Exception ex)
                    {
                        ucMessage1.ShowInfo(ex.Message.ToString());
                        PageBase.Errorhandling(ex);
                    }
                }
            }
        }
    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        blankinsert();
        blankserch();
        databind();
        
         
    }






    protected void btnSearch_Click(object sender, EventArgs e)
    {

        blankinsert();
        if (txtSerBrandCode.Text == "" && txtSerBrandName.Text == "")
        {
            ucMessage1.ShowInfo("Please enter atleast one searching parameter");
            return;
        }

        databind();

    }



    protected void grdBrand_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinsert();
                  
                    objproduct.BrandId = Convert.ToInt32(e.CommandArgument);
                    objproduct.BrandSelectionMode = 2;
                    brandinfo = objproduct.SelectBrandInfo();
                    objproduct.BrandName = Convert.ToString(brandinfo.Rows[0]["BrandName"]);
                    objproduct.BrandCode = Convert.ToString(brandinfo.Rows[0]["BrandCode"]);

                    objproduct.BrandStatus = Convert.ToInt16(brandinfo.Rows[0]["Status"]);

                    if (objproduct.BrandStatus == 1)
                    {
                        objproduct.BrandStatus = 0;
                    }
                    else
                    {
                        objproduct.BrandStatus = 1;
                    }
                    objproduct.error = "";
                    objproduct.UpdateBrandInfo();
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
                    ucMessage1.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }


            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    ucMessage1.Visible = false;
                    
                    objproduct.BrandId = Convert.ToInt32(e.CommandArgument);
                    ViewState["BrandID"] = objproduct.BrandId;
                    objproduct.BrandSelectionMode = 2;
                    brandinfo = objproduct.SelectBrandInfo();

                    txtBrandName.Text = Convert.ToString(brandinfo.Rows[0]["BrandName"]);
                    txtBrandCode.Text = Convert.ToString(brandinfo.Rows[0]["BrandCode"]);
                    objproduct.BrandStatus = Convert.ToInt16(brandinfo.Rows[0]["Status"]);

                    if (objproduct.BrandStatus == 1)
                    {
                        chkActive.Checked = true;
                    }
                    else
                    {
                        chkActive.Checked = false;
                    }



                    btnCreate.Text = "Update";
                    updAddUserMain.Update();
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
        }
    }
    protected void btngetalldata_Click(object sender, EventArgs e)
    {

        blankinsert();
        blankserch();
        databind();
       
       
        ucMessage1.Visible = false ;
    }
    protected void grdBrand_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdBrand.PageIndex = e.NewPageIndex;
        databind();
    }

# endregion 

    # region export  to excel 


    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {

            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "BrandCode", "BrandName",  "CurrentStatus" };
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
                    string FilenameToexport = "BrandDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    ViewState["Table"] = null;
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
                }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            ViewState["Table"] = null;
        }


    }
    #endregion 
}





