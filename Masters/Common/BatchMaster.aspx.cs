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

public partial class Masters_Common_BatchMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkActive.Checked = true;
            filldata();
            ViewState["BatchId"] = null;
        }
    }


   
    public void filldata()
    {
        using (POC obj = new POC())
        {
            obj.BatchName = txtSerName.Text;
            obj.BatchCode = txtSerCode.Text;
            obj.BatchStartDate = ucSerFromdate.Date;
            obj.BatchEndDate = ucSerDateTo.Date;
            obj.BatchselectionMode = 2;
            DataTable dt = obj.SelectBatchInfo();
            ViewState["Table"] = dt;
            grdBatch.DataSource = dt;
            grdBatch.DataBind();
            updgrid.Update();

        }

    }

    public void blankinsert()
    {
        txtBatchName.Text = "";
        txtBatchCode.Text = "";
        ucStartDate.Date = "";
        ucEndDate.Date = "";
        chkActive.Checked = true;
        updAddUserMain.Update();
        

    }


    public void blankserch()
    {
        txtSerName.Text = "";
        txtSerCode.Text = "";
        ucStartDate.Date = "";
        ucEndDate.Date = "";
        UpdSearch.Update();

    }


    protected void btnCreate_Click(object sender, EventArgs e)
    {
        using (POC obj = new POC())
        {
            obj.BatchName = txtBatchName.Text;
            obj.BatchCode = txtBatchCode.Text;
            obj.BatchStartDate = ucStartDate.Date;
            obj.BatchEndDate = ucEndDate.Date;
            obj.Status = chkActive.Checked;
           if (ViewState["BatchId"] == null)
            {
                
                obj.InsUpdBatchInfo();
                if (obj.error != "")
                {
                    ucMessage1.ShowInfo(obj.error);
                    return;
                }
                ucMessage1.ShowInfo(Resources.Messages.InsertSuccessfull);
                filldata();
                blankinsert();
                blankserch();
                UpdSearch.Update();
                updAddUserMain.Update();
             }

            else
            {
                obj.BatchID = (int)ViewState["BatchId"];
                obj.InsUpdBatchInfo();
                if (obj.error != "")
                {
                    ucMessage1.ShowInfo(obj.error);
                    return;
                }
                ucMessage1.ShowInfo(Resources.Messages.EditSuccessfull);
                filldata();
                blankserch();
                blankinsert();
               


            }


        }

      }


    protected void btncancel_click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        blankserch();
        blankserch();
        

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        blankinsert();
        ucMessage1.Visible = false;
        if (txtSerCode.Text == "" && txtSerName.Text == "" && ucSerDateTo.Date == ""
                  && ucSerFromdate.Date == "")
        {
            ucMessage1.ShowInfo("Please enter atleast one searching parameter");
            return;
        }
        if ((ucSerDateTo.Date == "" && ucSerFromdate.Date != "") || (ucSerDateTo.Date != "" && ucSerFromdate.Date == ""))
        {
            ucMessage1.ShowInfo("Please enter both the searching dates");
            return;
        }
        filldata();

    }
    protected void btngetalldta_Click(object sender, EventArgs e)
    {

        blankserch();
        filldata();

    }
    protected void grdBatch_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        using (POC objmaster = new POC())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinsert();
                    objmaster.BatchID = Convert.ToInt32(e.CommandArgument);
                    objmaster.BatchselectionMode = 2;
                    DataTable dt  = objmaster.SelectBatchInfo();
                    objmaster.BatchName = Convert.ToString(dt.Rows[0]["BatchName"]);
                    objmaster.BatchCode = Convert.ToString(dt.Rows[0]["BatchCode"]);
                    objmaster.BatchStartDate = Convert.ToString(dt.Rows[0]["BatchStartdate"]);
                    objmaster.BatchEndDate = Convert.ToString(dt.Rows[0]["BatchEnddate"]);
                    objmaster.Status = Convert.ToBoolean(dt.Rows[0]["Status"]);
                    if (objmaster.Status == true)
                    {
                        objmaster.Status = false;
                    }
                    else
                    {
                        objmaster.Status = true;
                    }

                    objmaster.InsUpdBatchInfo();

                    if (objmaster.error == "")
                    {
                        filldata();
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
                    ViewState["BatchID"]  = Convert.ToInt32(e.CommandArgument);
                    objmaster.BatchselectionMode = 2;
                    DataTable dt = objmaster.SelectBatchInfo();
                    txtBatchName.Text = Convert.ToString(dt.Rows[0]["BatchName"]);
                    txtBatchCode.Text = Convert.ToString(dt.Rows[0]["BatchCode"]);
                    ucStartDate.Date = Convert.ToString(dt.Rows[0]["BatchStartDate"]);
                    ucEndDate.Date = Convert.ToString(dt.Rows[0]["BatchEndDate"]);
                    chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["Status"].ToString());
                    btnCreate.Text = "Update";
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
    protected void grdBatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdBatch.PageIndex = e.NewPageIndex;
        filldata();
    }


    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {

            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "BatchName","BatchCode","BatchStartDate","BatchEndDate", "CurrentStatus" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["CurrentStatus"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "BatchDetails";
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
}
