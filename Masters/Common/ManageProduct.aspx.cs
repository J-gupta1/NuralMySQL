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

public partial class Masters_HO_Common_ManageProduct : PageBase
{

    
    DataTable productinfo;


    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            try
            {
                Page.Form.DefaultButton = getalldata.UniqueID; /* #CC01 Added */
               databind();
               chkActive.Checked = true;
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }


        }

    }


    # region user defined functions

    public void blankinsert()
    {
        txtProductCode.Text = "";
        txtProductName.Text = "";
       
        btnCreate.Text = "Submit";
        updAddUserMain.Update();
       
    }

    public void blankserch()
    {
        txtSerProductCode.Text = "";
        txtSerProductName.Text = "";
        UpdSearch.Update();


    }

    public void databind()
    {
       using (ProductData objproduct = new ProductData())
            {
           try 
           {
               ucMessage1.Visible = false;
                objproduct.ProductName = txtSerProductName.Text.Trim();
                objproduct.ProductCode = txtSerProductCode.Text.Trim();
                objproduct.ProductSelectionMode = 2;
                DataTable productserch = objproduct.SelectProductInfo();

               
                    ucMessage1.Visible = false;
                    ViewState["Table"] = productserch;
                    grdProduct.DataSource = productserch;
                    grdProduct.DataBind();
                    blankinsert();
                    updgrid.Update();
                }
            

        
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
}

     public bool insertvalidate()
    {
        if (txtProductCode.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Product Code");
            return false;
        }
        if (txtProductName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Product Name");
            return false;
        }
        return true;


    }

    # endregion 



     # region user defined functions 


     protected void btnCreate_Click(object sender, EventArgs e)
     {
         if (IsPageRefereshed == true)
         {
            return;
         }              //Pankaj Dhingra

         using (ProductData objproduct = new ProductData())
         {
             if (!insertvalidate())
             {
                 return;
             }
             else
             {
                 objproduct.ProductName = txtProductName.Text;
                 objproduct.ProductCode = txtProductCode.Text;
                 if (chkActive.Checked == true)
                 {
                     objproduct.ProductStatus = 1;
                 }
                 else
                 {
                     objproduct.ProductStatus = 0;
                 }

                 if (ViewState["ProductID"] == null || (int)ViewState["ProductID"] == 0)
                 {
                     try
                     {
                         blankserch();
                         objproduct.error = "";
                         objproduct.InsertProductInfo();

                         if (objproduct.error == "")
                         {
                            
                             blankserch();
                             blankinsert();

                             databind();
                             ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
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
                         updAddUserMain.Update();


                         objproduct.error = "";
                         objproduct.ProductId = (int)ViewState["ProductID"];


                         objproduct.UpdateProductInfo();
                         if (objproduct.error == "")
                         {
                             databind();
                             ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);

                             ViewState["ProductID"] = null;
                             blankinsert();




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
    protected void btncancel_click(object sender, EventArgs e)
    {

        blankinsert();
        blankserch();
        databind();
       
        
       ucMessage1.Visible = false ;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        blankinsert();
        if (txtSerProductCode.Text == "" && txtSerProductName.Text == "")
        {
            ucMessage1.ShowInfo("Please Enetr atleast one searching parameter");
            return;
        }
            databind();
    }

    protected void grdProduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {

            if (e.CommandName == "Active")
            {
                try
                {

                    ucMessage1.Visible = false;
                    
                    blankinsert();

                    objproduct.ProductId = Convert.ToInt32(e.CommandArgument);
                    objproduct.ProductSelectionMode = 2;
                    productinfo = objproduct.SelectProductInfo();
                    objproduct.ProductName = Convert.ToString(productinfo.Rows[0]["ProductName"]);
                    objproduct.ProductCode = Convert.ToString(productinfo.Rows[0]["ProductCode"]);

                    objproduct.ProductStatus = Convert.ToInt16(productinfo.Rows[0]["Status"]);

                    if (objproduct.ProductStatus == 1)
                    {
                        objproduct.ProductStatus = 0;
                    }
                    else
                    {
                        objproduct.ProductStatus = 1;
                    }
                    objproduct.error = "";
                    objproduct.UpdateProductInfo();
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
                   

                    objproduct.ProductId = Convert.ToInt32(e.CommandArgument);
                    ViewState["ProductID"] = objproduct.ProductId;
                    objproduct.ProductSelectionMode = 2;
                    productinfo = objproduct.SelectProductInfo();

                    txtProductName.Text = Convert.ToString(productinfo.Rows[0]["ProductName"]);
                    txtProductCode.Text = Convert.ToString(productinfo.Rows[0]["ProductCode"]);
                    objproduct.ProductStatus = Convert.ToInt16(productinfo.Rows[0]["Status"]);

                    if (objproduct.ProductStatus == 1)
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
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }

            }
        }
    
    }
    protected void btnGetAlldata_Click(object sender, EventArgs e)
    {
        blankserch();

        blankinsert();
        databind();
       
        ucMessage1.Visible = false;


    }
    protected void grdProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdProduct.PageIndex = e.NewPageIndex;
        databind();
    }



     #endregion 

    # region export to excel 


    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {

            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "ProductCode", "ProductName",  "CurrentStatus" };
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
                    string FilenameToexport = "ProductDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    ViewState["Table"] = null;
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
            ViewState["Table"] = null;
        }
    }


    # endregion 
    protected void grdProduct_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
