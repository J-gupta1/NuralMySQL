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
/*Change Log:
 * DD-MMM-YYYY, Name, #CCXX - Desc
 */


public partial class Reports_StockAgeingSlab : PageBase
{

   


    DataTable roleinfo;

    protected void Page_Load(object sender, EventArgs e)
    {

        //try
        //{
        //    if (!IsPostBack)
        //    {

        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}

    }

    void fnchidecontrols()
    {
        ucMessage1.Visible  = false;
    }


    private void fncBindData()
    {
        try
        {
            DataTable dtStockAgeSlab;
            using (ReportData objRD = new ReportData())
            {


                objRD.BaseEntityTypeID = PageBase.BaseEntityTypeID;
                objRD.UserId = PageBase.UserId;
                objRD.ComingFrom = Convert.ToInt16( cmbRoleType.SelectedValue);
                dtStockAgeSlab = objRD.GetStockAgeSlabReport();
                if (dtStockAgeSlab != null )
                {
                    if (dtStockAgeSlab.Rows.Count > 0)
                    {

                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "StockAging";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dtStockAgeSlab.DataSet, FilenameToexport);

                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.NoRecord);

                    }
 
                    
                    
                }
                else
                {
                    if (!string.IsNullOrEmpty( objRD.error))
                        ucMessage1.ShowError(objRD.error);
                    else
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
                
                
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

     

    
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbRoleType.SelectedIndex == 0)
            {
                ucMessage1.ShowInfo("Please select report for");
                return ;
            }
            fncBindData();
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    

    
  
}
