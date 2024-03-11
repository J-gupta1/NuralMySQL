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

public partial class Masters_Common_ManageVendor : PageBase
{
    DataTable productinfo;


    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            try
            {
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
        txtVendorCode.Text = "";
        txtVendorName.Text = "";

        btnCreate.Text = "Submit";
        updAddUserMain.Update();

    }

    public void blankserch()
    {
        txtSerName.Text = "";
        txtSerCode.Text = "";
        UpdSearch.Update();


    }

    public void databind()
    {
        using (ProductData objproduct = new ProductData())
        {
            try
            {
                ucMessage1.Visible = false;
                objproduct.VendorName = txtSerName.Text.Trim();
                objproduct.VendorCode = txtSerCode.Text.Trim();
                objproduct.VendorSelectionMode = 2;
                DataTable productserch = objproduct.SelectVendorInfo();


                ucMessage1.Visible = false;
                ViewState["Table"] = productserch;
                grdVendor.DataSource = productserch;
                grdVendor.DataBind();
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
        if (txtVendorCode.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Vendor Code");
            return false;
        }
        if (txtVendorName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Vendor Name");
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
                objproduct.VendorName = txtVendorName.Text;
                objproduct.VendorCode = txtVendorCode.Text;
                if (chkActive.Checked == true)
                {
                    objproduct.VendorStatus = 1;
                }
                else
                {
                    objproduct.VendorStatus = 0;
                }

                if (ViewState["VendorID"] == null || (int)ViewState["VendorID"] == 0)
                {
                    try
                    {
                        blankserch();
                        objproduct.error = "";
                        objproduct.InsertVendorInfo();

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
                        objproduct.VendorID = (int)ViewState["VendorID"];


                        objproduct.UpdateVendorInfo();
                        if (objproduct.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);

                            ViewState["VendorID"] = null;
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


        ucMessage1.Visible = false;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        blankinsert();
        if (txtSerCode.Text == "" && txtSerName.Text == "")
        {
            ucMessage1.ShowInfo("Please Enter atleast one searching parameter");
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

                    objproduct.VendorID = Convert.ToInt32(e.CommandArgument);
                    objproduct.VendorSelectionMode = 2;
                    productinfo = objproduct.SelectVendorInfo();
                    objproduct.VendorName = Convert.ToString(productinfo.Rows[0]["VendorName"]);
                    objproduct.VendorCode = Convert.ToString(productinfo.Rows[0]["VendorCode"]);

                    objproduct.VendorStatus = Convert.ToInt16(productinfo.Rows[0]["Status"]);

                    if (objproduct.VendorStatus == 1)
                    {
                        objproduct.VendorStatus = 0;
                    }
                    else
                    {
                        objproduct.VendorStatus = 1;
                    }
                    objproduct.error = "";
                    objproduct.UpdateVendorInfo();
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


                    objproduct.VendorID = Convert.ToInt32(e.CommandArgument);
                    ViewState["VendorID"] = objproduct.VendorID;
                    objproduct.VendorSelectionMode = 2;
                    productinfo = objproduct.SelectVendorInfo();

                    txtVendorName.Text = Convert.ToString(productinfo.Rows[0]["VendorName"]);
                    txtVendorCode.Text = Convert.ToString(productinfo.Rows[0]["VendorCode"]);
                    objproduct.VendorStatus = Convert.ToInt16(productinfo.Rows[0]["Status"]);

                    if (objproduct.VendorStatus == 1)
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
        grdVendor.PageIndex = e.NewPageIndex;
        databind();
    }



    #endregion

    # region export to excel


    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {

            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "VendorCode", "VendorName", "CurrentStatus" };
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
                    string FilenameToexport = "VendorDetails";
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
