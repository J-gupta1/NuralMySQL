using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;

public partial class Masters_Common_BulletinCategory : PageBase
{
    MastersData objmaster = new MastersData();
    DataTable Category;

    protected void Page_Load(object sender, EventArgs e)
    {
        UcMsg.ShowControl = false;
        chkStatus.Checked = true;
        if (!IsPostBack)
        {
           ViewState["CategoryID"] = null;
           FillGrid();
           
        }
    }

    void FillGrid()
    {
        try
            {
                UcMsg.Visible = false;

                objmaster.CategoryName = txtSerCAT.Text.Trim();
                objmaster.CategorySelectionMode = 2;
                objmaster.CategoryID = 0;
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;
                Category = objmaster.SelectCategoryInfo();

                grdCAT.DataSource = Category;
                grdCAT.DataBind();
                updgrdView.Update();
                ViewState["Table"] = Category;
            }
            catch (Exception ex)
            {
                UcMsg.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
  
    public void blankinserttext()
    {
        try
        {
            txtCategoryName.Text = "";
            btnSubmit.Text = "Submit";
            chkStatus.Checked = true;
         }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            UcMsg.ShowError(err);
        }
    }

    public void blanksertext()
    {
        try
        {
            txtSerCAT.Text = "";
            UpdSearch.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            UcMsg.ShowError(err);
        }
    }

    protected void grdCAT_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Active")
            {
                try
                {
                    objmaster.UserId = PageBase.UserId;
                    objmaster.CompanyId = PageBase.ClientId;
                    objmaster.CategoryID = Convert.ToInt32(e.CommandArgument);
                    objmaster.CategorySelectionMode = 2;
                    Category = objmaster.SelectCategoryInfo();
                    objmaster.CategoryName = Convert.ToString(Category.Rows[0]["CategoryName"]);

                    objmaster.Status = Convert.ToBoolean(Category.Rows[0]["Status"]);
                    if (objmaster.Status == true)
                    {
                        objmaster.Status = false;
                    }
                    else
                    {
                        objmaster.Status = true;
                    }
                    objmaster.error = "";
                    
                    objmaster.InsertUpdateCategoryInfo();

                    if (objmaster.error == "")
                    {
                        objmaster.CategoryID = 0;
                        FillGrid();
                        UcMsg.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                    else
                    {
                        UcMsg.ShowInfo(objmaster.error);
                    }
                }
                catch (Exception ex)
                {
                    UcMsg.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    objmaster.CategoryID = Convert.ToInt32(e.CommandArgument);
                    ViewState["CategoryID"] = objmaster.CategoryID;

                    objmaster.CategorySelectionMode = 2;
                    objmaster.UserId = PageBase.UserId;
                    objmaster.CompanyId = PageBase.ClientId;
                    Category = objmaster.SelectCategoryInfo();

                    txtCategoryName.Text = Convert.ToString(Category.Rows[0]["CategoryName"]);

                    objmaster.Status = Convert.ToBoolean(Category.Rows[0]["Status"]);
                    if (objmaster.Status == true)
                    {
                        chkStatus.Checked = true;
                    }
                    else
                    {
                        chkStatus.Checked = false;
                    }

                    btnSubmit.Text = "Update";
                    updgrdView.Update();
                }
                catch (Exception ex)
                {
                    UcMsg.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
        }
    
     protected void grdCAT_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCAT.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }       //Pankaj Dhingra

        objmaster.CategoryName = txtCategoryName.Text.Trim();

            if (chkStatus.Checked == true)
            {
                objmaster.Status = true;
            }
            else
            {
                objmaster.Status = false;
            }

            if (ViewState["CategoryID"] == null || (int)ViewState["CategoryID"] == 0)
            {
                try
                {
                    objmaster.error = "";
                    objmaster.UserId = PageBase.UserId;
                    objmaster.CompanyId = PageBase.ClientId;
                    objmaster.InsertUpdateCategoryInfo();
                    if (objmaster.error == "")
                    {
                        FillGrid();
                        UcMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        ViewState["CategoryID"] = null;
                        txtCategoryName.Text = "";
                        chkStatus.Checked = true;
                    }
                    else
                    {
                        UcMsg.ShowError("Record Duplicated");
                    }
                }
                catch (Exception ex)
                {
                    UcMsg.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
            else
            {
                try
                {
                    updgrdView.Update();
                    objmaster.error = "";
                    objmaster.CategoryID = (int)ViewState["CategoryID"];
                    objmaster.CategoryName = txtCategoryName.Text.ToString();

                    objmaster.InsertUpdateCategoryInfo();
                    if (objmaster.error == "")
                    {
                        FillGrid();
                        UcMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        
                        blankinserttext();
                        ViewState["CategoryID"] = null;
                        btnSubmit.Text = "Submit";
                    }
                    else
                    {
                        UcMsg.ShowError(objmaster.error);
                    }
                }
                catch (Exception ex)
                {
                    UcMsg.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
        }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        blanksertext();
        blankinserttext();
        FillGrid();
        chkStatus.Checked = true;
        UpdSearch.Update();
        UcMsg.ShowControl = false;
        btnSubmit.Text = "Submit";
    }

    protected void btnserCategory_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtSerCAT.Text == "")
            {
                UcMsg.ShowInfo("Please enter search parameter");
                return;
            }
            blankinserttext();
            FillGrid();
        }
        catch (Exception ex)
        {
            UcMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    } 
     
    protected void btngetalldata_click(object sender, EventArgs e)
    {
        blankinserttext();
        blanksertext();
        FillGrid();
        UpdSearch.Update();
        UcMsg.ShowControl = false;
        btnSubmit.Text = "Submit";
    }

    protected void exporttoexel_Click(object sender, EventArgs e)
         {
             if (ViewState["Table"] != null)
             {
                 DataTable dt = (DataTable)ViewState["Table"];
                 string[] DsCol = new string[] { "CategoryName", "Status" };
                 DataTable DsCopy = new DataTable();
                 dt = dt.DefaultView.ToTable(true, DsCol);
                 dt.Columns["Status"].ColumnName = "Status";

                 if (dt.Rows.Count > 0)
                 {
                     DataSet dtcopy = new DataSet();
                     dtcopy.Merge(dt);
                     dtcopy.Tables[0].AcceptChanges();
                     String FilePath = Server.MapPath("../../");
                     string FilenameToexport = "CategoryDetails";
                     PageBase.RootFilePath = FilePath;
                     PageBase.ExportToExecl(dtcopy, FilenameToexport);
                     ViewState["Table"] = null;
                 }
                 else
                 {
                     UcMsg.ShowError(Resources.Messages.NoRecord);

                 }
                 ViewState["Table"] = null;
             }
         }

    //updAddUserMain.Update();


    //int CategoryId = 0;
    //objmaster.CategoryID = CategoryId;
    //updAddUserMain.Update();
    //UcMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
    //objmaster.CategorySelectionMode = 2;
    //Category = objmaster.SelectAllCategory();
    //grdCAT.DataSource = Category;
    //grdCAT.DataBind();


    //objmaster.CategorySelectionMode = 2;
    //Category = objmaster.SelectAllCategory();
    //grdCAT.DataSource = Category;

    //grdCAT.DataBind();
    //updgrdView.Update();


    //Category = objmaster.SelectAllCategory();
    //grdCAT.DataSource = Category;
    //grdCAT.DataBind();
    //updgrdView.Update();
    //    if (e.CommandName == "Active")
    //    {
    //        try
    //        {
    //            objmaster.CategoryID = Convert.ToInt32(e.CommandArgument);

    //            Category = objmaster.SelectCategoryInfo();
    //            objmaster.CategoryName = Convert.ToString(Category.Rows[0]["CategoryName"]);




    //            objmaster.Status = Convert.ToBoolean(Category.Rows[0]["Status"]);

    //            if (objmaster.Status == true)
    //            {
    //                objmaster.Status = false;
    //            }
    //            else
    //            {
    //                objmaster.Status = true;
    //            }
    //            objmaster.error = "";
    //            objmaster.InsertUpdateCategoryInfo();

    //            if (objmaster.error == "")
    //            {
    //                UcMsg.ShowSuccess(Resources.Messages.StatusChanged);
    //                Category = objmaster.SelectAllCategory();
    //                grdCAT.DataSource = Category;
    //                grdCAT.DataBind();
    //                updgrdView.Update();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }


    //    }

    //    if (e.CommandName == "cmdEdit")
    //    {
    //        try
    //        {

    //            objmaster.CategoryID = Convert.ToInt32(e.CommandArgument);
    //            ViewState["CategoryID"] = objmaster.CategoryID;

    //            Category = objmaster.SelectAllCategory();

    //            txtCategoryName.Text = Convert.ToString(Category.Rows[0]["CategoryName"]);


    //            objmaster.Status = Convert.ToBoolean(Category.Rows[0]["Status"]);

    //            if (objmaster.Status == true)
    //            {
    //                chkStatus.Checked = true;
    //            }
    //            else
    //            {
    //                chkStatus.Checked = false;
    //            }






    //            btnSubmit.Text = "Update";
    //            updAddUserMain.Update();
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }



    //    }
    //}
    //objmaster.CategoryName = txtSerCAT.Text.Trim();


    //try
    //{
    //    Category = objmaster.SelectCategoryInfo();

    //if (cmbSerCat.SelectedIndex > 0)
    //{
    //    objmaster.CatID = Convert.ToInt16(cmbSerCat.SelectedValue.ToString());
    //}
    
    // try
    //    {
    //        MastersData ObjCategory = new MastersData();
    //        if (ViewState["CategoryID"] != null)
    //        {
    //            objmaster.CategoryID = (((ViewState["CategoryID"] != null) || Convert.ToInt32(ViewState["CategoryID"]) != 0) ? Convert.ToInt32(ViewState["CategoryID"]) : 0);
    //        }
    //        //ObjCategory.CategoryName = txtCategoryName.Text.Trim();
    //        objmaster.CategoryName = txtCategoryName.Text.Trim();



    //        if (chkStatus.Checked)

    //            ObjCategory.Status = true;
    //        else
    //            ObjCategory.Status = false;
    //        Int32 Result = ObjCategory.InsertUpdateCategoryInfo();
    //        updgrdView.Update();
    //        objmaster.error = "";
    //        objmaster.CategoryID = (int)ViewState["CategoryID"];
    //        objmaster.CategoryName = txtCategoryName.Text.ToString();


    //        objmaster.InsertUpdateCategoryInfo();
    //        if (objmaster.error == "")
    //        {

    //            UcMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
    //            ViewState["CategoryID"] = 0;

    //            Category = objmaster.SelectAllCategory();
    //            grdCAT.DataSource = Category;
    //            grdCAT.DataBind();
    //            updgrdView.Update();
    //            btnSubmit.Text = "Submit";
    //        }
    //        else
    //        {

    //            UcMsg.ShowError("Record Duplicated");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //    if (ViewState["CategoryID"] == null || Convert.ToInt16(ViewState["CategoryID"]) == 0)
    //        UcMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
    //    else
    //        UcMsg.ShowSuccess(Resources.Messages.EditSuccessfull);


    //    ViewState["CategoryID"] = null;



    //}

    // public void filldata()
    //{
    //   // blankinserttext();
    //     objmaster.CategorySelectionMode = 2;
    //     Category = objmaster.SelectAllCategory();

    //     if (Category.Rows.Count > 0)
    //    {

    //        ViewState["Table"] = Category;

    //        grdCAT.DataSource = Category;
    //        grdCAT.DataBind();
    //        updgrdView.Update();

    //    }
    //    else
    //    {
    //        UcMsg.ShowInfo("No Previous Record");
    //    }

    //}

    //if (Category.Rows.Count == 0)
    //{
    //    grdCAT.Visible = false;
    //    UcMsg.ShowError(Resources.Messages.NoRecord);
    //}
    //else
    //{
    //UcMsg.Visible = false;
    //txtCategoryName.Text = "";
    //ViewState["CategoryID"] = null;
    //ViewState["Table"] = null;
    //Category = objmaster.SelectAllCategory();
    //grdCAT.DataSource = Category;
    //grdCAT.DataBind();
    //updgrdView.Update();

}