/*
===========================================================================================================================================
Copyright	: Zed-Axis Technologies, 2017
Created By	: Sumit Maurya
Create On	: 02-May-2017
Description	: This interface is copy of ViewSerialNumberMovement.aspx. Created for gionee.

===========================================================================================================================================
Change Log:
DD-MMM-YYYY, Name , #CCXX - Description

-------------------------------------------------------------------------------------------------------------------------------------------
*/
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


public partial class Transactions_Common_RevertTertiaryActivation : PageBase
{
    public Int32 IMEITrackingCount = Convert.ToInt32(ConfigurationManager.AppSettings["IMEITrackingCount"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            lblIMEItrackingMsg.Text = "Maximum " + IMEITrackingCount.ToString() + " serial no. allowed.";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string SerialNumber = txtSerialNumber.Text;
            string ErrorSerialNumber = PageBase.CheckSerialNo(SerialNumber);
            if (ErrorSerialNumber != "")
            {

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
            if (strSplitArray.Count() > IMEITrackingCount)
            {
                ucMsg.ShowInfo("Maximum " + IMEITrackingCount.ToString() + " serial no. allowed.");
                return;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("SN");
            foreach (var obj in strSplitArray.Distinct())
            {
                dt.Rows.Add(obj.Trim().ToString());
            }

            SqlParameter[] objSQLParam = new SqlParameter[5];
            objSQLParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSQLParam[0].Direction = ParameterDirection.Output;
            objSQLParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            objSQLParam[1].Direction = ParameterDirection.Output;
            objSQLParam[2] = new SqlParameter("@dtserialNumber", dt);
            objSQLParam[3] = new SqlParameter("@UserId", PageBase.UserId);
            objSQLParam[4] = new SqlParameter("@Remark", txtRemarks.TextBoxText.Trim());
            /*int intResult = DataAccess.DataAccess.Instance.DBInsertCommand("prcToolRemoveTertiaryActivation", objSQLParam);*/
            DataAccess.DataAccess.Instance.DBInsertCommand("prcToolRemoveTertiaryActivation", objSQLParam);
            int intResult = Convert.ToInt32(Convert.ToString(objSQLParam[0].Value));
            if (intResult == 0)
            {
                ucMsg.ShowSuccess("Records Updated Successfully.");
                txtRemarks.TextBoxText = string.Empty;
                txtSerialNumber.Text = string.Empty;
            }
            else
            {
                if (intResult == 1)
                    ucMsg.ShowInfo(Convert.ToString(objSQLParam[1].Value));
                else
                {
                    ucMsg.ShowError(Convert.ToString(objSQLParam[1].Value));
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSerialNumber.Text = "";
        ucMsg.Visible = false;

    }




    public DataSet GetViewSerialNumberMovementWithTransactionExcel(DataTable dtSN)
    {
        try
        {
            DataSet dsResult;
            string error;
            SqlParameter[] SqlParam;
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@dtserialNumberExcel", dtSN);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetViewSerialNumberMovementWithTransactionExcel", CommandType.StoredProcedure, SqlParam);
            error = Convert.ToString(SqlParam[1].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }





    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSerialNumber.Text = "";
        txtRemarks.TextBoxText = "";
        ucMsg.Visible = false;
    }
}
