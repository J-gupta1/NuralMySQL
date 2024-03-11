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

public partial class Transactions_Billing_PriceTypeMaster : PageBase
{
    DataTable PriceTypeinfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
               
                binddata();
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btninsert_click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }
        using (ClsPricetypemaster objmaster = new ClsPricetypemaster())
        {
            updAddUserMain.Update();
            if (!insertvalidate())
            {
                return;
            }
            else
            {
                objmaster.PriceTypeKeyword = txtPricetypekeyword.Text.Trim();
                objmaster.PriceTypeDescription = txtPricetypeDescription.Text.Trim();

                if (chkstatus.Checked == true)
                {
                    objmaster.Status = 1;
                }
                else
                {
                    objmaster.Status = 0;
                }
                objmaster.UserId = PageBase.UserId;
                if (ViewState["PriceTypeID"] == null || (int)ViewState["PriceTypeID"] == 0)
                {
                    try
                    {
                        objmaster.InsertPriceTypeInfo();
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
                        objmaster.Pricetypeid = (int)ViewState["PriceTypeID"];
                        objmaster.InsertPriceTypeInfo();
                        if (objmaster.error == "")
                        {
                            binddata();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            ViewState["PriceTypeID"] = null;
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
    protected void btnGetallData_Click(object sender, EventArgs e)
    {
        blankinsert();
        blanksearch();
        binddata();
        ucMessage1.Visible = false;
    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            binddata();
            DataTable dt = PriceTypeinfo.Copy();
            string[] DsCol = new string[] { "PriceTypeKeyword", "PriceTypeDescription", "CurrentStatus" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["CurrentStatus"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "PriceTypeMasterDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void grdPriceType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPriceType.PageIndex = e.NewPageIndex;
        binddata();
    }
    protected void grdPriceType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (ClsPricetypemaster objmaster = new ClsPricetypemaster())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinsert();
                    objmaster.Pricetypeid = Convert.ToInt32(e.CommandArgument);
                    objmaster.Status = 2;
                    PriceTypeinfo = objmaster.SelectPriceTypemasterInfo();
                    objmaster.PriceTypeKeyword = Convert.ToString(PriceTypeinfo.Rows[0]["PriceTypeKeyword"]);
                    objmaster.Status = Convert.ToInt16(PriceTypeinfo.Rows[0]["Active"]);
                    if (objmaster.Status == 1)
                    {
                        objmaster.Status = 0;
                    }
                    else
                    {
                        objmaster.Status = 1;
                    }

                    objmaster.InsertPriceTypeInfo();

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
                    ViewState["PriceTypeID"] = objmaster.Pricetypeid = Convert.ToInt32(e.CommandArgument);
                    objmaster.Status = 2;
                    PriceTypeinfo = objmaster.SelectPriceTypemasterInfo();
                    txtPricetypekeyword.Text = Convert.ToString(PriceTypeinfo.Rows[0]["PriceTypeKeyword"]);
                    txtPricetypeDescription.Text = Convert.ToString(PriceTypeinfo.Rows[0]["PriceTypeDescription"]);
                    if(PriceTypeinfo.Rows[0]["Active"].ToString()=="1")
                    {
                        chkstatus.Checked = true;
                    }
                    else
                    {
                        chkstatus.Checked = false;
                    }
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
    public void binddata()
    {
        try
        {
            ucMessage1.Visible = false;
            using (ClsPricetypemaster objpricetypemaster = new ClsPricetypemaster())
            {
                objpricetypemaster.PriceTypeKeyword = txtSerPriceTypedescription.Text.Trim();
                objpricetypemaster.Status = 2;
                PriceTypeinfo = objpricetypemaster.SelectPriceTypemasterInfo();
                grdPriceType.DataSource = PriceTypeinfo;
                grdPriceType.DataBind();
                updgrid.Update();
                
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
            txtPricetypekeyword.Text = "";
            txtPricetypeDescription.Text = "";
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

            txtSerPriceTypedescription.Text = "";
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
        if (txtPricetypeDescription.Text == "" || txtPricetypekeyword.Text=="")
        {
            ucMessage1.ShowInfo("Please Insert Price Type Keyword and Price Type Description.");
            return false;
        }
        return true;

    }

}