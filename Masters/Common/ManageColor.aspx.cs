/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 21-Mar-2016, Sumit Maurya, #CC01, Issue of page getting blocked on the execution of event(s) duue to update panel resolved.
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

public partial class Masters_HO_Common_ManageColor : PageBase
{


    DataTable colorinfo;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Page.Form.DefaultButton = getalldata.UniqueID; /* #CC02 Added */
            databind();
            chkActive.Checked = true;

        }

    }


    # region user functions

    public void blankinsert()
    {

        txtColorName.Text = "";
        //chkActive.Checked = false;
        btnCreate.Text = "Submit";
        updAddUserMain.Update();
    }

    public void blankserch()
    {

        txtSerColorName.Text = "";

        UpdSearch.Update();

    }

    public void databind()
    {


        using (ProductData objproduct = new ProductData())
        {
            try
            {
                ucMessage1.Visible = false;
                objproduct.ColorName = txtSerColorName.Text.Trim();

                objproduct.ColorId = 0;
                objproduct.ColorSelectionMode = 2;
                DataTable colorserch = objproduct.SelectColorInfo();


                ViewState["Table"] = colorserch;
                grdColor.DataSource = colorserch;
                grdColor.DataBind();
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
        if (txtColorName.Text == "")
        {
            ucMessage1.ShowError("Please Insert Color Name");
            return false;
        }
        return true;

    }

    # endregion 

    #region control functions 

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }       //Pankaj Dhingra
        using (ProductData objproduct = new ProductData())
        {
            if (!insertvalidate())
            {
                return;
            }
            else
            {

                objproduct.ColorName = txtColorName.Text.Trim();
                if (chkActive.Checked == true)
                {
                    objproduct.ColorStatus = 1;
                }
                else
                {
                    objproduct.ColorStatus = 0;
                }

                if (ViewState["ColorID"] == null || (int)ViewState["ColorID"] == 0)
                {
                    objproduct.error = "";
                    objproduct.InsertColorInfo();

                    if (objproduct.error == "")
                    {
                        databind();
                        ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        blankinsert();
                        blankserch();
                    }
                    else
                    {

                        ucMessage1.ShowInfo(objproduct.error);
                    }
                }
                else
                {


                    objproduct.ColorName = txtColorName.Text.Trim();



                    objproduct.error = "";
                    objproduct.ColorId = (int)ViewState["ColorID"];


                    objproduct.UpdateColorInfo();
                    if (objproduct.error == "")
                    {
                        databind();
                        blankinsert();
                        ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);

                        ViewState["ColorID"] = null;




                    }
                    else
                    {

                        ucMessage1.ShowInfo(objproduct.error);
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
        if (txtSerColorName.Text == "")
        {
            ucMessage1.ShowInfo("Please enter the search parameter");
            return;
        }

        databind();
    }
    protected void grdColor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    objproduct.ColorId = Convert.ToInt32(e.CommandArgument);
                    objproduct.ColorSelectionMode = 2;
                    colorinfo = objproduct.SelectColorInfo();
                    objproduct.ColorName = Convert.ToString(colorinfo.Rows[0]["ColorName"]);


                    objproduct.ColorStatus = Convert.ToInt16(colorinfo.Rows[0]["Status"]);

                    if (objproduct.ColorStatus == 1)
                    {
                        objproduct.ColorStatus = 0;
                    }
                    else
                    {
                        objproduct.ColorStatus = 1;
                    }
                    objproduct.error = "";
                    objproduct.UpdateColorInfo();
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

                    objproduct.ColorId = Convert.ToInt32(e.CommandArgument);
                    ViewState["ColorID"] = objproduct.ColorId;
                    objproduct.ColorSelectionMode = 2;
                    colorinfo = objproduct.SelectColorInfo();

                    txtColorName.Text = Convert.ToString(colorinfo.Rows[0]["ColorName"]);

                    objproduct.ColorStatus = Convert.ToInt16(colorinfo.Rows[0]["Status"]);

                    if (objproduct.ColorStatus == 1)
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

    protected void btngetalldta_Click(object sender, EventArgs e)
    {
        blankserch();
        blankinsert();
        databind();
    }

    protected void grdColor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdColor.PageIndex = e.NewPageIndex;
        databind();
    }

# endregion


    # region export to excel 

    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {

            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "ColorName", "CurrentStatus" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["CurrentStatus"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "ColorDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                ViewState["Table"] = null;
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            ViewState["Table"] = null;
        }


    }

    #endregion 

   
    protected void grdColor_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

