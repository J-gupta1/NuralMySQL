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

public partial class Masters_HO_Common_ManageProductCategory : PageBase
{
    DataTable prodcatinfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            if (!IsPostBack)
            {
                Page.Form.DefaultButton = getalldata.UniqueID; /* #CC01 Added */
                databind();
                chkActive.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ucMessage2.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }


    # region user functions 

    public void blankinsert()
    {
        txtProdCatCode.Text = "";
        txtProdCatName.Text = "";
       
        btnCreate.Text = "Submit";
        updAddUserMain.Update();
       
    }

    public void blankserch()
    {
        txtSerProdCatCode.Text = "";
        txtSerProdCatName.Text = "";
        UpdSearch.Update();

    }

    public void databind()
    {
       using( ProductData objproduct = new ProductData())
       {
           ucMessage2.Visible = false;
           try
           {
               UpdSearch.Update();
               objproduct.ProdCatName = txtSerProdCatName.Text;
               objproduct.ProdCatCode = txtSerProdCatCode.Text;
               objproduct.ProdCatSelectionMode = 2;
               prodcatinfo = objproduct.SelectProdCatInfo();
               
              
               
                   grdProdCat.DataSource = prodcatinfo;
                   grdProdCat.DataBind();
                   updgrid.Update();
                   ViewState["Table"] = prodcatinfo;
               
           }
           catch (Exception ex)
           {
               ucMessage2.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
               PageBase.Errorhandling(ex);
           }
         }
    }

     public bool insertvalidate()
    {
        if (txtProdCatCode.Text == "")
        {
            ucMessage2.ShowInfo("Please Insert Product Category Code");
            return false;
        }
        if (txtProdCatName.Text == "")
        {
            ucMessage2.ShowInfo("Please Insert Product Category Name");
            return false;
        }
        return true;


    }

#endregion

     # region control functions

     protected void btnCreate_Click(object sender, EventArgs e)
     {

         if (IsPageRefereshed == true)
         {
            return;
         }      //Pankaj Dhingra

         using (ProductData objproduct = new ProductData())
         {
             updAddUserMain.Update();
             if (!insertvalidate())
             {
                 return;
             }
             else
             {

                 objproduct.ProdCatName = txtProdCatName.Text;
                 objproduct.ProdCatCode = txtProdCatCode.Text;
                 if (chkActive.Checked == true)
                 {
                     objproduct.ProdCatStatus = 1;
                 }
                 else
                 {
                     objproduct.ProdCatStatus = 0;
                 }

                 if (ViewState["ProdCatID"] == null || (int)ViewState["ProdCatID"] == 0)
                 {
                     try
                     {
                         blankserch();
                         objproduct.error = "";
                         objproduct.InsertProdCatInfo();

                         if (objproduct.error == "")
                         {

                             databind();
                             ucMessage2.ShowSuccess(Resources.Messages.CreateSuccessfull);
                             blankinsert();

                             updgrid.Update();

                         }
                         else
                         {

                             ucMessage2.ShowInfo(objproduct.error);
                         }
                     }
                     catch (Exception ex)
                     {
                         ucMessage2.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                         PageBase.Errorhandling(ex);
                     }
                 }
                 else
                 {


                     objproduct.ProdCatName = txtProdCatName.Text.ToString();
                     objproduct.ProdCatCode = txtProdCatCode.Text.ToString();


                     objproduct.error = "";
                     objproduct.ProdCatId = (int)ViewState["ProdCatID"];


                     objproduct.UpdateProdCatInfo();
                     if (objproduct.error == "")
                     {
                         try
                         {
                             databind();
                             ucMessage2.ShowSuccess(Resources.Messages.EditSuccessfull);

                             ViewState["ProdCatID"] = null;
                             blankinsert();

                             updgrid.Update();

                         }
                         catch (Exception ex)
                         {
                             ucMessage2.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                             PageBase.Errorhandling(ex);
                         }

                     }
                     else
                     {
                         ucMessage2.ShowInfo(objproduct.error);
                     }

                 }
             }
         }
     }

    protected void btncancel_click(object sender, EventArgs e)
    {
        blankserch();
        blankinsert();
        databind();
        ucMessage2.Visible = false;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
              blankinsert();
              if (txtSerProdCatCode.Text == "" && txtSerProdCatName.Text == "")
              {
                  ucMessage2.ShowInfo("Please enter atleast one searching parameter ");
                  return;
              }
               databind ();           
        }
    

    protected void grdProdCat_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {


            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage2.Visible = false;
                    blankinsert();
                  
                    objproduct.ProdCatId = Convert.ToInt32(e.CommandArgument);
                    objproduct.ProdCatSelectionMode = 2;
                    prodcatinfo = objproduct.SelectProdCatInfo();
                    objproduct.ProdCatName = Convert.ToString(prodcatinfo.Rows[0]["ProductCategoryName"]);

                    objproduct.ProdCatCode = Convert.ToString(prodcatinfo.Rows[0]["ProductCategoryCode"]);

                    objproduct.ProdCatStatus = Convert.ToInt16(prodcatinfo.Rows[0]["Status"]);

                    if (objproduct.ProdCatStatus == 1)
                    {
                        objproduct.ProdCatStatus = 0;
                    }
                    else
                    {
                        objproduct.ProdCatStatus = 1;
                    }
                    objproduct.error = "";
                    objproduct.UpdateProdCatInfo();
                    if (objproduct.error == "")
                    {
                        databind();
                        ucMessage2.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                    else
                    {
                        ucMessage2.ShowInfo(objproduct.error);
                    }

                }
                catch (Exception ex)
                {
                    ucMessage2.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }


            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    ucMessage2.Visible = false;
                 

                    objproduct.ProdCatId = Convert.ToInt32(e.CommandArgument);
                    ViewState["ProdCatID"] = objproduct.ProdCatId;
                    objproduct.ProdCatSelectionMode = 2;
                    prodcatinfo = objproduct.SelectProdCatInfo();

                    txtProdCatName.Text = Convert.ToString(prodcatinfo.Rows[0]["ProductCategoryName"]);
                    txtProdCatCode.Text = Convert.ToString(prodcatinfo.Rows[0]["ProductCategoryCode"]);
                    objproduct.ProdCatStatus = Convert.ToInt16(prodcatinfo.Rows[0]["Status"]);

                    if (objproduct.ProdCatStatus == 1)
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
                    ucMessage2.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
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
        ucMessage2.Visible = false;
    }

    protected void grdProdCat_SelectedIndex(object sender, GridViewPageEventArgs e)
    {
        grdProdCat.PageIndex = e.NewPageIndex;
        databind();
    }


     #endregion 

    #region export to excel 

    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {

            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "ProductCategoryCode", "ProductCategoryName",  "CurrentStatus" };
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
                    string FilenameToexport = "ProductCategoryDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    ViewState["Table"] = null;
                }
                catch (Exception ex)
                {
                    ucMessage2.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }
                }
            else
            {
                ucMessage2.ShowError(Resources.Messages.NoRecord);

            }
            ViewState["Table"] = null;
        }
    }

    # endregion 

   
}

             