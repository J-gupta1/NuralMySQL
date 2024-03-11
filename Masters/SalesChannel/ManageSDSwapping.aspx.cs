using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Text;

public partial class Masters_SalesChannel_ManageSDSwapping :PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            btnswapsd.Visible = false;
          
            fillSD();
        }
    }

    void fillSD()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            obj.SalesChannelTypeID = Convert .ToInt16( EnumData.eSalesChannelType.SS);
            DataTable dt = obj.GetSalesChannelInfo();
            String[] colArray = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref cmbSDFrom, dt, colArray);
            PageBase.DropdownBinding(ref cmbSDTo, dt, colArray);
        }
    }

    bool isvalidate()
    {
        if (cmbSDFrom.SelectedValue != "0" && cmbSDTo.SelectedValue != "0")
        {
            if (cmbSDTo.SelectedValue == cmbSDFrom.SelectedValue)
            {
                ucMsg.ShowInfo(GetLocalResourceObject("SelectOtherSalesChannel").ToString ());
                return false;
            }
        }
        return true;
    }

    void getMDFromdata()
    {
        DataTable dt = new DataTable();
        using (SalesChannelData obj = new SalesChannelData())
        {
            obj.SalesChannelID = Convert.ToInt32(cmbSDFrom.SelectedValue);
            dt = obj.GetSalesChannelChildInfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                grdSDFrom.DataSource = dt;
                grdSDFrom.DataBind();
              
                Pnlfrom.Visible = true;
                btnswapsd.Visible = true;
            }
            else
            {
                Pnlfrom.Visible = false;
                btnswapsd.Visible = false ;
                ucMsg.ShowInfo(GetLocalResourceObject("NoRecordTransfer").ToString());
            }
        }
    }

    void getMDTodata()
    {
        DataTable dt = new DataTable();
        using (SalesChannelData obj = new SalesChannelData())
        {
            obj.SalesChannelID = Convert.ToInt32(cmbSDTo.SelectedValue);
            dt = obj.GetSalesChannelChildInfo();
                grdSDTo.DataSource = dt;
                grdSDTo.DataBind();
              
                pnlto.Visible = true;
        }
    }

    protected void cmbSDFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnswapsd.Visible = false;

        ucMsg.Visible = false;
        if (!isvalidate())
        {
            return;
        }
        if (cmbSDFrom.SelectedValue == "0")
        {
            Pnlfrom.Visible = false;
          
            return;
        }
        else
        {
            getMDFromdata();
            
           
        }
    }

    protected void cmbSDTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        if (!isvalidate())
        {
            return;
        }
        if (cmbSDTo.SelectedValue == "0")
        {
            pnlto.Visible = false;
         
            return;
        }
        else
        {
            getMDTodata();
          
        }
    }
    
    protected void btnswapsd_Click(object sender, EventArgs e)
    {
     int intResult = 0;
     using (SalesChannelData obj = new SalesChannelData())
     {
         obj.SalesChannelID  = Convert.ToInt32(cmbSDFrom.SelectedValue);
         obj.ToSalesChannelID   = Convert.ToInt32(cmbSDTo.SelectedValue);
         intResult = obj.InsertInfoSDSwap();       
         if (intResult==0)
         {
             ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
             return;
         }
         else if (intResult==2)
         {
             ucMsg.ShowInfo(GetLocalResourceObject("Alreadytransfercurrentdate").ToString());
             return;
         }
         ucMsg.ShowInfo(GetLocalResourceObject("TransferSuccess").ToString());
         Pnlfrom.Visible = false;
         btnswapsd.Visible = false;
         getMDTodata();
        
        }
    }

   
    protected void grdfromPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSDFrom.PageIndex = e.NewPageIndex;
        getMDFromdata();
    }

    protected void grdToPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSDTo.PageIndex = e.NewPageIndex;
        getMDTodata();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
       
        cmbSDFrom.SelectedValue = "0";
        cmbSDTo.SelectedValue = "0";
        Pnlfrom.Visible = false;
        pnlto.Visible = false;
        btnswapsd.Visible = false;

    }
}
