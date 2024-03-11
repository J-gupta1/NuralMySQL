#region NameSpace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Data;
using System.Configuration;

#endregion
/*Change Log:
 * 08-May-2014, Rakesh Goel, #CC01 - Changed export to excel call to EPP method instead of OpenXML.
 * Also added exception display in catch block
 * 27-Feb-2019,Vijay Kumar Prajapati,#CC02--Added New Dropdown.
 */
public partial class Reports_SalesChannel_ManageFullSMSRptGionee : PageBase
{
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            ucMessage1.ShowControl = false;
        }
    }


    bool isvalidate()
    {

        if (ucDateFrom.Date != "" && ucDateTo.Date == "")
        {

            ucMessage1.ShowInfo("Invalid Date Range");
            return false;
        }
        if (ucDateFrom.Date == "" && ucDateTo.Date != "")
        {
            ucMessage1.ShowInfo("Invalid Date Range");
            return false;
        }
        /*#CC02 Added Started*/
        if (ddlActivationType.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select activation type.");
            return false;
        }
        /*#CC02 Added End*/
        if (ucDateFrom.Date == "" && ucDateTo.Date == "")
        {
            ucMessage1.ShowInfo("Please Select any searching parameter.");
            return false;
        }

        if (ucDateFrom.Date != "" && ucDateTo.Date != "")
        {
            if (Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
            {
                ucMessage1.ShowInfo("The date from cant exceed the to  date");
                return false;
            }
        }
        

        return true;
    }

    void blankall()
    {

        ucDateFrom.Date = "";
        ucDateTo.Date = "";
        ucMessage1.Visible = false;
    }

    protected void btnSerch_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtIMEI = new DataTable();
            if (isvalidate())
            {
                using (ReportData obj = new ReportData())
                {
                    obj.FromDate = ucDateFrom.Date;
                    obj.ToDate = ucDateTo.Date;
                    obj.ActivationFrom = Convert.ToInt16(ddlActivationType.SelectedValue);/*#CC02 Added Started*/
                    dt = obj.GetFullSMSList();
                    if (dt.Rows.Count > 0)
                    {

                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "Full SMS List";
                        PageBase.RootFilePath = FilePath;
                        //PageBase.ExportToExeclUsingOPENXMLV2(dt, FilenameToexport);  //#CC01 commented
                       

                        PageBase.ExportToExecl(dt.DataSet, FilenameToexport);  //#CC01 added

                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.NoRecord);

                    }
                    updsearch.Update();
                }

            }
        }
        catch (Exception ex)
        {

        }
    }


    protected void btncancel_Click(object sender, EventArgs e)
    {
        blankall();


    }
   
}
