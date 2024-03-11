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
using MySql.Data.MySqlClient;
using DataAccess;
using BussinessLogic;


namespace DataAccess
{

    public partial class Masters_HO_ManagePriceList : PageBase
    {
        DataTable pricelistinfo;



        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Page.Form.DefaultButton = Getalldata.UniqueID; /* #CC01 Added */
                databind();
                chkActive.Checked = true;
            }

        }
        # region user functions
        public void blankinsert()
        {

            txtInsertName.Text = "";
           
            btnSubmit.Text = "Submit";
            updAddUserMain.Update();
        }

        public void blankserch()
        {

            txtSerName.Text = "";

            UpdSearch.Update();

        }

        public void databind()
        {
            using (ProductData objproduct = new ProductData())
            {

                try
                {
                    ucMessage1.Visible = false;
                    blankinsert();

                    objproduct.PriceListName = txtSerName.Text.Trim();
                    objproduct.CompanyId = PageBase.ClientId;
                    objproduct.PriceListSelectionMode = 2;
                    DataTable pricelistserch = objproduct.SelectPriceListInfo();


                    ViewState["Table"] = pricelistserch;


                    grdPriceList.DataSource = pricelistserch;
                    grdPriceList.DataBind();

                    updgrid.Update();
                    blankinsert();

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
            if (txtInsertName.Text == "")
            {
                ucMessage1.ShowError("Please Insert PriceList Name");
                return false;
            }
            return true;


        }

        # endregion

        #  region  control functions

        protected void btn_sumbitprice_Click(object sender, EventArgs e)
        {
            if (IsPageRefereshed == true)
            {
                return;
            }               //Pankaj Dhingra
            using (ProductData objproduct = new ProductData())
            {

                updAddUserMain.Update();

                objproduct.PriceListName = txtInsertName.Text.Trim();
                if (chkActive.Checked == true)
                {
                    objproduct.PriceListStatus = 1;
                }
                else
                {
                    objproduct.PriceListStatus = 0;
                }

                if (ViewState["PriceListID"] == null || (int)ViewState["PriceListID"] == 0)
                {
                    try
                    {
                        objproduct.error = "";
                        objproduct.CompanyId = PageBase.ClientId;
                        objproduct.InsertPriceListInfo();

                        if (objproduct.error == "")
                        {
                            databind();
                            blankserch();
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
                        ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                        PageBase.Errorhandling(ex);
                    }
                }
                else
                {
                    try
                    {

                        objproduct.error = "";
                        objproduct.PriceListId = (int)ViewState["PriceListID"];
                        objproduct.CompanyId = PageBase.ClientId;

                        objproduct.UpdatePriceListInfo();
                        if (objproduct.error == "")
                        {


                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);

                            blankinsert();
                            ViewState["PriceListID"] = null;



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


        protected void btn_cancel_click(object sender, EventArgs e)
        {
            blankserch();
            blankinsert();
            databind();

            ucMessage1.Visible = false;
        }
        protected void btn_searchclick(object sender, EventArgs e)
        {
            if (txtSerName.Text == "")
            {
                ucMessage1.ShowInfo("Please enter a searching parameter ");
                return;
            }

            databind();
        }


        protected void grdPrice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            using (ProductData objproduct = new ProductData())
            {
                if (e.CommandName == "Active")
                {
                    try
                    {
                        ucMessage1.Visible = false;
                        blankinsert();

                        objproduct.PriceListId = Convert.ToInt32(e.CommandArgument);
                        objproduct.PriceListSelectionMode = 2;
                        objproduct.CompanyId = PageBase.ClientId;
                        pricelistinfo = objproduct.SelectPriceListInfo();
                        objproduct.PriceListName = Convert.ToString(pricelistinfo.Rows[0]["PriceListName"]);


                        objproduct.PriceListStatus = Convert.ToInt16(pricelistinfo.Rows[0]["Status"]);

                        if (objproduct.PriceListStatus == 1)
                        {
                            objproduct.PriceListStatus = 0;
                        }
                        else
                        {
                            objproduct.PriceListStatus = 1;
                        }
                        objproduct.error = "";
                        objproduct.UpdatePriceListInfo();
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


                        objproduct.PriceListId = Convert.ToInt32(e.CommandArgument);
                        ViewState["PriceListID"] = objproduct.PriceListId;
                        objproduct.PriceListSelectionMode = 2;
                        objproduct.CompanyId = PageBase.ClientId;
                        pricelistinfo = objproduct.SelectPriceListInfo();

                        txtInsertName.Text = Convert.ToString(pricelistinfo.Rows[0]["PriceListName"]);

                        //chkActive.Checked = Convert.ToBoolean(pricelistinfo.Rows[0]["Status"].ToString());





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


        protected void btn_Getalldataclick(object sender, EventArgs e)
        {

            blankinsert();
            blankserch();
            databind();

            ucMessage1.Visible = false;

        }
        protected void grdPriceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPriceList.PageIndex = e.NewPageIndex;
            databind();
        }




        # endregion


        # region export to excel
        protected void exportToExel_Click(object sender, EventArgs e)
        {
            if (ViewState["Table"] != null)
            {

                DataTable dt = (DataTable)ViewState["Table"];
                string[] DsCol = new string[] { "PriceListName", "CurrentStatus" };
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
                        string FilenameToexport = "PriceListDetails";
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


        protected void grdPriceList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
}
}

   
    
   



