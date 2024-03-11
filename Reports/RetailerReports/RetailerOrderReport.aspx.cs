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

public partial class Reports_New_Reports_RetailerOrderReport : System.Web.UI.Page
{
    DataTable dt;
    DataSet Ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 900;
        ucMsg.Visible = false;
        if (!IsPostBack)
        {
            FillEntityType();
            ucDateTo.Date = PageBase.ToDate;
            ucDateFrom.Date = PageBase.Fromdate;


        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchData(ucPagingControl1.CurrentPage);
    }

    protected void lnkExportToExcel_Click(object sender, EventArgs e)
    {

        SearchData(-1);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ucDateFrom.Date != "" && ucDateTo.Date == "")
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning("Please Enter  To Date.");
            return;
        }
        if (ucDateTo.Date != "" && ucDateFrom.Date == "")
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning("Please Enter  From Date.");
            return;
        }
        ucMsg.Visible = false;

        SearchData(1);
    }

    private void SearchData(int pageno)
    {

        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            dt = new DataTable();

            using (ReportData obj = new ReportData())
            {
                obj.CompanyId = PageBase.ClientId;
                obj.SalesChannelId = PageBase.SalesChanelID;
                obj.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                obj.EntitytypeUserId = Convert.ToInt16(ddlEntityTypeName.SelectedValue);
                obj.ToDate = ucDateTo.Date;
                obj.FromDate = ucDateFrom.Date;
                obj.UserId = PageBase.UserId;
                obj.PageIndex = pageno;
                obj.PageSize = Convert.ToInt32(PageBase.PageSize);
                Ds = obj.getRetailerOrderReport();
                if ((obj.error == "") || (obj.error == null))
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        if (pageno > 0)
                        {
                            gvOrderDetail.DataSource = Ds;
                            gvOrderDetail.DataBind();
                            PnlGrid.Visible = true;
                            ViewState["TotalRecords"] = obj.TotalRecords;
                            ucPagingControl1.TotalRecords = obj.TotalRecords;
                            ucPagingControl1.PageSize = Convert.ToInt32(PageBase.PageSize);
                            ucPagingControl1.SetCurrentPage = pageno;
                            ucPagingControl1.FillPageInfo();
                            dvFooter.Visible = true;
                        }
                        else
                        {



                            DataSet dtcopy = new DataSet();

                            dtcopy.Merge(Ds.Tables[0]);
                            //dtcopy.Merge(Ds.Tables[1]);
                            dtcopy.AcceptChanges();
                            dtcopy.Tables[0].AcceptChanges();
                            //dtcopy.Tables[1].AcceptChanges();
                            String FilePath = Server.MapPath("../../");
                            string FilenameToexport = "RetailerOrder Report";
                            PageBase.RootFilePath = FilePath;
                            string[] strExcelSheetName = { "Detail", "Total" };
                            //PageBase.ExportToExecl(dtcopy, FilenameToexport);
                            ZedService.Utility.ZedServiceUtil.ExportToExecl(dtcopy, FilenameToexport);

                        }


                    }
                    else
                    {
                        ucMsg.ShowError(Resources.Messages.NoRecord);
                        gvOrderDetail.DataSource = null;
                        gvOrderDetail.DataBind();
                        dvFooter.Visible = false;
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
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    void FillEntityType()
    {
        using (ClsPaymentReport ObjEntityType = new ClsPaymentReport())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "EntityTypeID", "EntityType" };
            ObjEntityType.CompanyId = PageBase.ClientId;
            ObjEntityType.UserId = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlEntityType, ObjEntityType.GetEntityTypeV5API(), str);

        };
    }
    void FillEntityTypeName(int EntityTypeID)
    {
        using (ClsPaymentReport ObjEntityTypeName = new ClsPaymentReport())
        {

            ddlEntityTypeName.Items.Clear();
            ObjEntityTypeName.EntityTypeId = EntityTypeID;
            ObjEntityTypeName.UserId = PageBase.UserId;
            ObjEntityTypeName.CompanyId = PageBase.ClientId;
            string[] str = { "UserID", "EntityTypeName" };
            PageBase.DropdownBinding(ref ddlEntityTypeName, ObjEntityTypeName.GetEntityTypeName(), str);

        };
    }
}