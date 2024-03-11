/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 14-Mar-2018, Sumit Maurya, #CC01, default button set as it was firing chnage status while pressing enter button.
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

public partial class Masters_Common_ManageCountry : PageBase
{

    DataTable countryinfo ;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
         if (!IsPostBack)
            {
                Page.Form.DefaultButton = btnGetallData.UniqueID; /* #CC01 Added */  
              binddata();
            }
          }
        catch (Exception ex)
        {
            
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
     }

    

    #region user functions
    public void binddata()
    {
        try
        {
            ucMessage1.Visible = false;
            using (MastersData objmaster = new MastersData())
            {
                objmaster.CountryName = txtSerName.Text.Trim();
                objmaster.CountrySelectionMode = 2;
                countryinfo = objmaster.SelectCountryInfo();
                
                
                    grdCountry.DataSource = countryinfo;
                    grdCountry.DataBind();
                    updgrid.Update();
                    //ViewState["Table"] = stateinfo;
               }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
  
    
    public void blankinsert()
    {
        try
        {
            txtInsertName.Text = "";
             btnsubmit.Text = "Submit";
             updAddUserMain.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            ucMessage1.ShowError(err);
        }
    }
    public void blanksearch()
    {
        try
        {

            txtSerName.Text = "";
            UpdSearch.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            ucMessage1.ShowError(err);
        }
    }

    public bool insertvalidate()
    {
       if (txtInsertName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert City Name");
            return false;
        }
        return true;

          }


    #endregion

    #region control functions

    protected void btninsert_click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }       //Pankaj Dhingra
        using (MastersData objmaster = new MastersData())
        {
            updAddUserMain.Update();
            if (!insertvalidate())
            {
                return;
            }
            else
            {
                objmaster.CountryName = txtInsertName.Text.Trim();

                if (chkstatus.Checked == true)
                {
                    objmaster.CountryStatus = 1;
                }
                else
                {
                    objmaster.CountryStatus = 0;
                }
                if (ViewState["CountryID"] == null || (int)ViewState["CountryID"] == 0)
                {
                    try
                    {
                        objmaster.InsertCountryInfo();
                        if (objmaster.error == "")
                        {
                            binddata();
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blankinsert();
                            blanksearch();
                            updgrid.Update();
                        }
                        else
                        {

                            ucMessage1.ShowInfo(objmaster.error);
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
                       objmaster.CountryId = (int)ViewState["CountryID"];
                        objmaster.InsertCountryInfo();
                        if (objmaster.error == "")
                        {
                            binddata();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            ViewState["CountryID"] = null;
                            blankinsert();
                            updAddUserMain.Update();
                        }
                        else
                        {
                            ucMessage1.ShowInfo(objmaster.error);
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

        
        blankinsert();
        blanksearch();
        binddata();
        ucMessage1.Visible = false;


    }




    protected void btnSearch_Click(object sender, EventArgs e)
    {
        blankinsert();
        binddata();
    }

    protected void grdCountry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinsert();
                    objmaster.CountryId = Convert.ToInt32(e.CommandArgument);
                    objmaster.CountrySelectionMode = 2;
                    countryinfo = objmaster.SelectCountryInfo();
                    objmaster.CountryName = Convert.ToString(countryinfo.Rows[0]["CountryName"]);
                    objmaster.CountryStatus = Convert.ToInt16(countryinfo.Rows[0]["Status"]);
                    if (objmaster.CountryStatus == 1)
                    {
                        objmaster.CountryStatus = 0;
                    }
                    else
                    {
                        objmaster.CountryStatus = 1;
                    }
                    
                    objmaster.InsertCountryInfo();

                    if (objmaster.error == "")
                    {
                        binddata();
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
                     }
                    else
                    {
                        ucMessage1.ShowInfo(objmaster.error);
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
                    ViewState["CountryID"] = objmaster.CountryId = Convert.ToInt32(e.CommandArgument);
                    objmaster.CountrySelectionMode = 2;
                    countryinfo = objmaster.SelectCountryInfo();
                    txtInsertName.Text = Convert.ToString(countryinfo.Rows[0]["CountryName"]);
                    chkstatus.Checked = Convert.ToBoolean(countryinfo.Rows[0]["Status"].ToString());
                    btnsubmit.Text = "Update";
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

         }




    protected void btnGetallData_Click(object sender, EventArgs e)
    {
       
        blankinsert();
        blanksearch();
        binddata();

        ucMessage1.Visible = false;

    }
    protected void grdCountry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCountry.PageIndex = e.NewPageIndex;
        binddata();
    }

    #endregion 

    # region export to excel 
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {

            //if (ViewState["Table"] != null)
            //{
            //DataTable dt = (DataTable)ViewState["Table"];
            binddata();         //Pankaj Dhingra
            DataTable dt = countryinfo.Copy();
            string[] DsCol = new string[] {"CountryName" , "CurrentStatus" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["CurrentStatus"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "CountryDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                //ViewState["Table"] = null;
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            //ViewState["Table"] = null;
            //}
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    #endregion 


}




