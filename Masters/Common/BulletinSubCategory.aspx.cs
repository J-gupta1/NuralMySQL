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
using System.Text.RegularExpressions;

public partial class Masters_Common_BulletinSubCategory : PageBase
{
    MastersData objmaster = new MastersData();
    DataTable SubCategory;
 
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        if (!IsPostBack)
        {
            ViewState["SubCategoryID"] = null;
            FillGrid();
            fillcategory();
        }
    }

    void FillGrid()
    {
        try
            {
                ucMsg.Visible = false;

                objmaster.SubCategoryName = txtSerSubCat.Text.Trim();
                if (cmbSerCat.SelectedIndex > 0)
                {
                    objmaster.CatID = Convert.ToInt16(cmbSerCat.SelectedValue.ToString());
                }
                else
                {
                    objmaster.CatID = 0;
                }
                objmaster.SubCategorySelectionMode = 2;
                objmaster.SubCategoryID = 0;
                objmaster.UserId = PageBase.UserId;
                objmaster.CompanyId = PageBase.ClientId;
                SubCategory = objmaster.SelectSubCategoryInfo();

                grdSubCat.DataSource = SubCategory;
                grdSubCat.DataBind();
                updgrid.Update();
                ViewState["Table"] = SubCategory;
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    
    public void fillcategory()
    {
        try
            {
                DataTable Category = objmaster.SelectAllCategory();

                cmbSelectCat.DataSource = Category;
                cmbSelectCat.DataTextField = "CategoryName";
                cmbSelectCat.DataValueField = "CategoryID";

                cmbSelectCat.DataBind();
                cmbSelectCat.Items.Insert(0, new ListItem("Select", "0"));
                cmbSelectCat.SelectedIndex = 0;

                cmbSerCat.DataSource = Category;
                cmbSerCat.DataTextField = "CategoryName";
                cmbSerCat.DataValueField = "CategoryID";
                cmbSerCat.DataBind();
                cmbSerCat.Items.Insert(0, new ListItem("select", "0"));

                UpdSearch.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    public void blanksertext()
    {
        try
        {
            txtSerSubCat.Text = "";
            cmbSerCat.SelectedIndex = 0;
            UpdSearch.Update();
        }
        catch (Exception ex)
        {
            //string err = ex.Message.ToString();
            //ucMsg.ShowError(err);
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    public void blankinserttext()
    {
        try
        {
            txtInsertSubCat.Text = "";
            cmbSelectCat.SelectedIndex = 0;
            btnSubmit.Text = "Submit";
            chkstatus.Checked = false;
         }
        catch (Exception ex)
        {
            //string err = ex.Message.ToString();
            //ucMsg.ShowError(err);
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    protected void grdSubCat_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Active")
            {
                try
                {
                    ucMsg.Visible = false;
                    blankinserttext();
                    objmaster.SubCategoryID = Convert.ToInt32(e.CommandArgument);
                    objmaster.SubCategorySelectionMode = 2;
                    objmaster.UserId = PageBase.UserId;
                    objmaster.CompanyId = PageBase.ClientId;

                    SubCategory = objmaster.SelectSubCategoryInfo();
                    objmaster.SubCategoryName = Convert.ToString(SubCategory.Rows[0]["SubCategoryName"]);

                    objmaster.CatID = Convert.ToInt16(SubCategory.Rows[0]["CategoryID"]);

                    objmaster.SubStatus = Convert.ToBoolean(SubCategory.Rows[0]["Status"]);
                    if (objmaster.SubStatus == true)
                    {
                        objmaster.SubStatus = false;
                    }
                    else
                    {
                        objmaster.SubStatus = true;
                    }
                    objmaster.error = "";
                    objmaster.UpdateSubCategoryInfo();

                    if (objmaster.error == "")
                    {
                        objmaster.SubCategoryID = 0;
                        FillGrid();
                        ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    objmaster.UserId = PageBase.UserId;
                    objmaster.CompanyId = PageBase.ClientId;
                    objmaster.SubCategoryID = Convert.ToInt32(e.CommandArgument);
                    ViewState["SubCategoryID"] = objmaster.SubCategoryID;
                    objmaster.SubCategorySelectionMode = 2;
                    SubCategory = objmaster.SelectSubCategoryInfo();

                    txtInsertSubCat.Text = Convert.ToString(SubCategory.Rows[0]["SubCategoryName"]);

                    chkstatus.Checked = Convert.ToBoolean(SubCategory.Rows[0]["Status"].ToString());

                    if (cmbSelectCat.Items.FindByValue(SubCategory.Rows[0]["CategoryID"].ToString()) != null)
                    {
                        cmbSelectCat.ClearSelection();
                        cmbSelectCat.Items.FindByValue(SubCategory.Rows[0]["CategoryID"].ToString()).Selected = true;
                    }
                    btnSubmit.Text = "Update";
                    updgrid.Update();
                }
                catch (Exception ex)
                {
                    ucMsg.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
        }
           
    protected void grdSubCat_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSubCat.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }               //Pankaj Dhingra
        objmaster.SubCategoryName = txtInsertSubCat.Text.Trim();
        var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
          if(regexItem.IsMatch(objmaster.SubCategoryName))
          {

          }
          else
          {
              ucMsg.Visible = true;
              ucMsg.ShowError("Special Characters not allowed in SubCategoryname.");
              return;
          }
          
            objmaster.CatID = Convert.ToInt16(cmbSelectCat.SelectedValue.ToString());

            if (chkstatus.Checked == true)
            {
                objmaster.SubStatus = true;
            }
            else
            {
                objmaster.SubStatus = false;
            }
            if (ViewState["SubCategoryID"] == null || (int)ViewState["SubCategoryID"] == 0)
            {
                try
                {
                    blanksertext();
                    objmaster.error = "";
                    objmaster.UserId = PageBase.UserId;
                    objmaster.CompanyId = PageBase.ClientId;
                    objmaster.InsertSubCategoryInfo();
                    if (objmaster.error == "")
                    {
                        FillGrid();
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        blankinserttext();
                        updgrid.Update();
                    }
                }
                catch (Exception ex)
                {
                    ucMsg.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
            else
            {
                try
                {
                    updgrid.Update();
                    objmaster.error = "";
                    objmaster.UserId = PageBase.UserId;
                    objmaster.CompanyId = PageBase.ClientId;
                    objmaster.SubCategoryID = (int)ViewState["SubCategoryID"];
                    objmaster.SubCategoryName = txtInsertSubCat.Text.ToString();
                    objmaster.CatID = Convert.ToInt16(cmbSelectCat.SelectedValue.ToString());
                    objmaster.UpdateSubCategoryInfo();

                    if (objmaster.error == "")
                    {
                        objmaster.SubCategorySelectionMode = 2;
                        FillGrid();
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        blankinserttext();
                        ViewState["SubCategoryID"] = null;
                        btnSubmit.Text = "Submit";
                    }
                    else
                    {
                        ucMsg.ShowError("Record Duplicated");
                    }
                }
                catch (Exception ex)
                {
                    ucMsg.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
        }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        blankinserttext();
        blanksertext();
        FillGrid();
        UpdSearch.Update();
        ucMsg.Visible = false;
        btnSubmit.Text = "Submit";
    }
    
    protected void btnSerchD_Click1(object sender, EventArgs e)
      {
         if (cmbSerCat.SelectedIndex == 0 && txtSerSubCat.Text == "")
          {
                ucMsg.ShowInfo("Please enter atleast one searching parameter");
                return;
           }
            blankinserttext();
            FillGrid();
        }
   
    protected void getalldata_Click(object sender, EventArgs e)
    {
        blankinserttext();
        blanksertext();
        FillGrid();
        UpdSearch.Update();
        ucMsg.Visible = false;
        btnSubmit.Text = "Submit";
    }

    protected void exporttoexel_Click(object sender, EventArgs e)
    {
        try
        {
             if (ViewState["Table"] != null)
            {
            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "CategoryName", "SubCategoryName", "Status" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["Status"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SubCategoryDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                ViewState["Table"] = null;
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.NoRecord);
            }
            ViewState["Table"] = null;
        }
    }
    catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    //cmbSelectCat.SelectedIndex = 0;

    //blankinserttext();
    //if (SubCategory.Rows.Count == 0)
    //{
    //    grdSubCat.Visible = false;
    //    ucMsg.ShowError(Resources.Messages.NoRecord);
    //}
    //else
    //{
    //    ucMsg.Visible = false;
    //    grdSubCat.Visible = true;


    //objmaster.SubCategorySelectionMode = 1;

    // updAddUserMain.Update();

    //ViewState["SubCategoryID"] = null;


    //ViewState["SubCategoryID"] = null;
    //updAddUserMain.Update();


    //int SubCategoryId =0;

    //objmaster.SubCategoryID = SubCategoryId;


    //objmaster.SubCategorySelectionMode = 2;


    //FillGrid();

    //updAddUserMain.Update();


    //SubCategory = objmaster.SelectAllSubCategoryInfo();
    //grdSubCat.DataSource = SubCategory;
    //grdSubCat.DataBind();


    //else
    //{

    //    ucMsg.ShowError("Record Duplicated");
    //}

    //objmaster.CategorySelectionMode = 2;

    //objmaster.CategorySelectionMode = 2;

    //SubCategory = objmaster.SelectAllSubCategoryInfo();
    //grdSubCat.DataSource = SubCategory;
    //grdSubCat.DataBind();
    //updgrid.Update();


    //fillcategory();


    //objmaster.SubCategoryID = 0;
    //objmaster.SubCategorySelectionMode = 2;
    //SubCategory = objmaster.SelectAllSubCategoryInfo();
    //grdSubCat.DataSource = SubCategory;
    //grdSubCat.DataBind();
    //updgrid.Update();

    //blankinserttext();
    //objmaster.SubCategoryName = txtSerSubCat.Text.Trim();

    //if (cmbSerCat.SelectedIndex > 0)
    //{
    //    objmaster.CatID = Convert.ToInt16(cmbSerCat.SelectedValue.ToString());
    //}
    //SubCategory = objmaster.SelectSubCategoryInfo();

    //    if (SubCategory.Rows.Count == 0)
    //    {
    //        grdSubCat.Visible = false;
    //        ucMsg.ShowError(Resources.Messages.NoRecord);
    //    }
    //    else
    //    {
    //        ucMsg.ShowInfo("");
    //        grdSubCat.Visible = true;
    //        grdSubCat.DataSource = SubCategory;
    //        blankinserttext();
    //        grdSubCat.DataBind();



    //    }
    //    updgrid.Update();
    //}




    //blankinserttext();
    //objmaster.SubCategoryName = txtSerSubCat.Text.Trim();

    //if (cmbSerCat.SelectedIndex > 0)
    //{
    //    objmaster.CatID = Convert.ToInt16(cmbSerCat.SelectedValue.ToString());
    //}
    //else
    //{
    //    objmaster.CatID = 0;
    //}



    //try
    //{
    //    SubCategory = objmaster.SelectSubCategoryInfo();
    //    if (SubCategory.Rows.Count == 0)
    //    {
    //        grdSubCat.Visible = false;
    //        ucMsg.ShowError(Resources.Messages.NoRecord);
    //    }
    //    else
    //    {
    //        ucMsg.ShowInfo("");
    //        //ViewState["Table"] = SubCategory;
    //        grdSubCat.Visible = true;
    //        grdSubCat.DataSource = SubCategory;
    //        grdSubCat.DataBind();

    //        blankinserttext();
    //    }
    //    updgrid.Update();
    //}
    //catch (Exception ex)
    //{
    //    throw (ex);
    //}

    //public void filldata()
    //{
        //    SubCategory = objmaster.SelectAllSubCategoryInfo();
        //    if (SubCategory.Rows.Count > 0)
        //    {
        //        grdSubCat.DataSource = SubCategory;
        //        grdSubCat.DataBind();
        //        updgrid.Update();

        //    }
        //    else
        //    {
        //        ucMsg.ShowInfo("No Previous Record");
        //    }

    //}

}







