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
using Cryptography;

public partial class Reports_POC_SalesChannelLedger : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                FillSalesChannelType();
                if (PageBase.SalesChanelID == 0)
                {
                    cmbsaleschanneltype.Enabled = true;
                    cmbsaleschanneltype.SelectedValue = "0";
                    ddlSaleschannel.Enabled = true;
                }
                else
                {
                    cmbsaleschanneltype.SelectedValue = PageBase.SalesChanelTypeID.ToString();
                    cmbsaleschanneltype.Enabled = false;
                    BindSalesChannelList();
                    ddlSaleschannel.SelectedValue = PageBase.SalesChanelID.ToString();
                    ddlSaleschannel.Enabled = false;

                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }

    void FillSalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            DataTable dt = new DataTable();
            String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
            ObjSalesChannel.SalesChannelTypeID = 0;
            dt = ObjSalesChannel.GetSalesChannelType();
            PageBase.DropdownBinding(ref cmbsaleschanneltype, dt, StrCol);

        };
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbsaleschanneltype.SelectedValue == "0" && ddlSaleschannel.SelectedValue=="0")
            {
                ucMessage1.ShowInfo("Please enter some searching parameter ");
                return;
            }
            FillGrid();
        }


        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void FillGrid()
    {
        DataSet Dss = new DataSet();
       using (ReportData objRD = new ReportData())
        {
            
        objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
        objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
        objRD.SalesChannelID = PageBase.SalesChanelID;
        objRD.UserId = PageBase.UserId;
        objRD.SalesChannelID = Convert.ToInt16(ddlSaleschannel.SelectedValue);
        Dss = objRD.SelectSalesChannelLedgerInformation();
        };
       if (Dss == null)
       {
           ucMessage1.ShowInfo(Resources.Messages.NoRecord);
           return;
       }

       if (Dss.Tables[0] != null && Dss.Tables[0].Rows.Count > 0)
       {

           gvVoucher.DataSource = Dss.Tables[0];
           gvVoucher.DataBind();
           dvReport.Visible = true;
       }
       else
       {

           ucMessage1.ShowInfo(Resources.Messages.NoRecord);
           dvReport.Visible = false;
       }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlSaleschannel.SelectedValue = "0";
        cmbsaleschanneltype.SelectedValue = "0";
        ucMessage1.ShowControl = false;
    }
  
    protected void cmbsaleschanneltype_SelectedIndexChanged1(object sender, EventArgs e)
    {
           
        try
        {
            BindSalesChannelList();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    
    }
    void BindSalesChannelList()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            String[] StrCol = new String[] { "SalesChannelID", "SalesChannelName" };
            DataTable Dt = new DataTable();
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            Dt = ObjSalesChannel.GetSalesChannelBasedOnType();

            PageBase.DropdownBinding(ref ddlSaleschannel, Dt, StrCol);

            if (Dt != null && Dt.Rows.Count > 0)
            {
                ddlSaleschannel.Enabled = true;
            }
            else
            {
                ddlSaleschannel.Enabled = false;
            }
        };
    }
    private void Export(bool saveAs)
    {
      // ASPxGridExporter1.ReportHeader = "Customer Name : " + lblCustomer.Text + Environment.NewLine + "Customer Address : " + lblCustomerAddress.Text;

        string fileName = string.Format("VoucherReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
        switch (listExportFormat.SelectedIndex)
        {
            case 0:
                ASPxGridExporter1.WriteXlsToResponse(fileName, saveAs);

                break;
            case 1:
                ASPxGridExporter1.WriteXlsxToResponse(fileName, saveAs);
                break;
            case 2:
                ASPxGridExporter1.WritePdfToResponse(fileName, saveAs);
                break;
        }
    }
    protected void buttonOpen_Click(object sender, EventArgs e)
    {
        FillGrid();
        Export(false);
    }
    protected void buttonSaveAs_Click(object sender, EventArgs e)
    {
        FillGrid();
        Export(true);
    }
}
