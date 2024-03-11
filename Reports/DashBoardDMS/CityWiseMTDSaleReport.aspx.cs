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

public partial class Reports_DashBoardDMS_CityWiseMTDSaleReport : PageBase//System.Web.UI.Page
{
    DataTable dt;
    DataSet Ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 900;
        ucMsg.Visible = false;
        if (!IsPostBack)
        {

            ucDateTo.Date = PageBase.ToDate;
            ucDateFrom.Date = PageBase.Fromdate;


        }
    }
    protected void lnkExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            dt = new DataTable();

            using (ReportData obj = new ReportData())
            {

                obj.SalesChannelId = PageBase.SalesChanelID;

                obj.ToDate = ucDateTo.Date;
                obj.FromDate = ucDateFrom.Date;
                obj.UserId = PageBase.UserId;
                Ds = obj.getCityWiseMTDReport();
                if ((obj.error == "") || (obj.error == null))
                {
            if (Ds.Tables[0].Rows.Count > 0)
            {

                DataSet dtcopy = new DataSet();

                dtcopy.Merge(Ds.Tables[0]);
                //dtcopy.Merge(Ds.Tables[1]);
                dtcopy.AcceptChanges();
                dtcopy.Tables[0].AcceptChanges();
                //dtcopy.Tables[1].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "CityWiseMtdReport";
                PageBase.RootFilePath = FilePath;
                //PageBase.ExportToExecl(dtcopy, FilenameToexport);
                ZedService.Utility.ZedServiceUtil.ExportToExecl(dtcopy, FilenameToexport);



            }
            else
            {
                ucMsg.ShowError(Resources.Messages.NoRecord);

            }
                }
                else
                {
                    ucMsg.ShowError(obj.error);
                }
            }
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
        }
    }
}