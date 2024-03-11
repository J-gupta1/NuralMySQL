using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;
using System.Data.SqlClient;
using System.Configuration;
using ZedService;
/*
 * 17 July 2015, Karam Chand Sharma, #CC01, In serail search allow search accordnig to configuration define in web config.
 * 08-Jun-2016, Sumit Maurya, #CC02, Carton number and Sofware version displayed in search grid.
 * 14-May-2018,Vijay Kumar Prajapati,#CC03, Add userid for track serialnumbermovementscope for motorola.
 */ 

public partial class Reports_Inventory_ViewSerialNumberMovement : PageBase
{
    public Int32 IMEITrackingCount = Convert.ToInt32(ConfigurationManager.AppSettings["IMEITrackingCount"]);/*#CC01 ADDED*/
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tblCurrentOwner.Visible = false;
            tblTransactions.Visible = false;
            lblIMEItrackingMsg.Text = "Maximum " + IMEITrackingCount.ToString() + " serial no allowed."; /*#CC01 ADDED*/
        }
    }
    private void gvCurrentOwnerBind(Int32 ComingFrom)
    {
        try
        {
            string SerialNumber = txtSerialNumber.Text;
            string ErrorSerialNumber = PageBase.CheckSerialNo(SerialNumber);
            if (ErrorSerialNumber != "")
            {
                tblCurrentOwner.Visible = false;
                tblTransactions.Visible = false;

                if (ErrorSerialNumber.Replace(",", "").Trim() == string.Empty)
                {
                    ucMsg.ShowError("Blank SerialNumber is not allowed");
                    return;
                }
                ucMsg.ShowError(ErrorSerialNumber + " " + "Invalid SerialNumber");
                return;
            }
            SerialNumber = SerialNumber.Replace("\r\n", ",");
            string[] strSplitArray = SerialNumber.Split(',');
            /*#CC01 START ADDED*/
            if (strSplitArray.Count() > IMEITrackingCount)
            {
                ucMsg.ShowInfo("Maximum " + IMEITrackingCount.ToString() + " serial no allowed.");
                return;
            }
            /*#CC01 END ADDED*/
            DataTable dt = new DataTable();
            dt.Columns.Add("SN");
            foreach (var obj in strSplitArray.Distinct())
            {
                dt.Rows.Add(obj.Trim().ToString());
            }
            using (ReportData objViewSN = new ReportData())
            {
                if (ComingFrom == 1)
                {
                    DataTable dtResult = new DataTable();
                    objViewSN.UserId = PageBase.UserId;/*#CC03 Added*/
                    dtResult = objViewSN.GetViewSerialNumberMovement(dt);
                    tblCurrentOwner.Visible = true;
                    if (dtResult.Rows.Count == 0)
                    {
                        //ucMsg.ShowError(Resources.Messages.NoRecord);/*#CC03 Commented*/
                        ucMsg.Visible = true;/*#CC03 Added*/
                        ucMsg.ShowError("No Record Found.");/*#CC03 Added*/
                        tblCurrentOwner.Visible = false;/*#CC03 Added*/
                        Exporttoexcel.Visible = false;
                        return;/*#CC03 Added*/
                    }
                    else
                    {
                        Exporttoexcel.Visible = true;
                    }
                    gvCurrentOwner.DataSource = dtResult;
                    gvCurrentOwner.DataBind();
                    ucMsg.Visible = false;
                }
                else if (ComingFrom == 2)//Export To excel
                {
                    DataSet ds = new DataSet();
                    objViewSN.UserId = PageBase.UserId;/*#CC03 Added*/
                    ds = GetViewSerialNumberMovementWithTransactionExcel(dt);
                    tblCurrentOwner.Visible = true;

                    if (ds != null)
                    {
                        ds.Tables[0].TableName = "SerialNumberOwner";//here will always be two tables
                        ds.Tables[1].TableName = "SerialNumberTransaction";
                        int SheetCount = 2;
                        if (ds.Tables.Count > 1)
                        {

                            if (ds.Tables[1].Rows.Count <= 0)
                            {
                                SheetCount = 1;
                                ds.Tables.Remove(ds.Tables[1]);// we are removing because it was throwing exception
                                ds.AcceptChanges();

                            }
                        }

                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "SerialDetails";
                        PageBase.RootFilePath = FilePath;
                        ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport);
                    }
                    else
                    {
                        ucMsg.ShowError(Resources.Messages.NoRecord);
                    }


                }


            };
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    private void gvTransactionsBind(Int64 SNID)
    {
        try
        {
            using (ReportData objViewSN = new ReportData())
            {

                DataTable dtResult = new DataTable();
                objViewSN.UserId = PageBase.UserId;
                dtResult = objViewSN.GetSerialTrackGetTransactions(SNID);
                //tblCurrentOwner.Visible = true;
                tblTransactions.Visible = true;
                if (dtResult.Rows.Count == 0)
                {
                    ucMsg.ShowError(Resources.Messages.NoRecord);
                }
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                gvTransactions.DataSource = dtResult;
                gvTransactions.DataBind();
                ucMsg.Visible = false;
            };
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvCurrentOwnerBind(1);//for grid
        tblTransactions.Visible = false;

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSerialNumber.Text = "";
        ucMsg.Visible = false;
        tblCurrentOwner.Visible = false;
        tblTransactions.Visible = false;
    }
    protected void gvCurrentOwner_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("ViewTransactions") && e.CommandArgument != "")
        {
            GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int RowIndex = oItem.RowIndex;
            for (int i = 0; i < gvCurrentOwner.Rows.Count; i++)
            {
                gvCurrentOwner.Rows[i].BackColor = System.Drawing.Color.White;
            }
            gvCurrentOwner.Rows[RowIndex].BackColor = System.Drawing.Color.YellowGreen;

            gvTransactionsBind(Convert.ToInt64(e.CommandArgument));
        }
        else
        {
            ucMsg.ShowError("Data Not In System");
            tblTransactions.Visible = false;
        }
    }
    protected void gvCurrentOwner_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCurrentOwner.PageIndex = e.NewPageIndex;
        gvCurrentOwnerBind(1);//for grid
    }

    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        try
        {
            gvCurrentOwnerBind(2);


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    public DataSet GetViewSerialNumberMovementWithTransactionExcel(DataTable dtSN)
    {
        try
        {
            DataSet dsResult;
            string error;
            SqlParameter[] SqlParam;
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@dtserialNumberExcel", dtSN);
            SqlParam[3] = new SqlParameter("@UserId", PageBase.UserId);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetViewSerialNumberMovementWithTransactionExcel", CommandType.StoredProcedure, SqlParam);
            error = Convert.ToString(SqlParam[1].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



}
