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
public partial class Masters_SalesChannel_BulkSalesChannelRetailerTransfer : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillsalesChannelfrom();

        }
    }

    public void fillsalesChannelfrom()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            DataTable dt = obj.GetParentForRetailer();
            String[] colArray = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref cmbTransferFrom, dt, colArray);
            
        }
        
    }


   
    public void fillgrid()
    {
        using (RetailerData obj = new RetailerData())
        {
            obj.SalesChannelID = Convert.ToInt32(cmbTransferFrom.SelectedValue);
            obj.Type = 1;
            DataTable dt = obj.GetRetailerInfo();
            grdRetailer.DataSource = dt;
            grdRetailer.DataBind();
            updgrid.Update();

         }

    }



 protected void cmbTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbTransferFrom.SelectedValue == "0")
        {
            cmbTransferTo.Items.Clear();
            cmbTransferTo.Items.Insert(0,new ListItem("Select" ,"0")); 
        }
        else 
        {
              using (SalesChannelData obj = new SalesChannelData())
        {
            DataTable dt = obj.GetParentForRetailer();
            dt.DefaultView.RowFilter = "SalesChannelID <> " + cmbTransferFrom.SelectedValue;
            dt = dt.DefaultView.ToTable();
            String[] colArray = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref cmbTransferTo, dt, colArray);
          
        }
              fillgrid();
              pnlGrid.Visible = true;

        
        }

    }


 protected void btntransfer_click(object sender, EventArgs e)
 {
      try
     {
         DataTable retailerInfo = new DataTable();
         DataColumn dc1 = new DataColumn("RetailerID", typeof(Int16));
         retailerInfo.Columns.Add(dc1);
         foreach (GridViewRow grv in grdRetailer.Rows)
         {
             Label lbId = (Label)grv.FindControl("lblID");
             DataRow dr = retailerInfo.NewRow();
             dr["RetailerID"] = Convert.ToInt16(lbId.Text);
             retailerInfo.Rows.Add(dr);
         }
         gettransferdata(retailerInfo);

     }

     catch (Exception ex)
     {
         ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
     }

}

 public void gettransferdata(DataTable retailerInfo)
 {
     int intResult = 0;
     DataTable Tvp = new DataTable();
     using (CommonData ObjCommom = new CommonData())
     {
         Tvp = ObjCommom.GettvpRetailerMap();
     }
     foreach (DataRow dr in retailerInfo.Rows)
     {
         DataRow drow = Tvp.NewRow();
         drow[0] = dr["RetailerID"].ToString();
         Tvp.Rows.Add(drow);

     }
     Tvp.AcceptChanges();

     using (SalesmanData objPrimarySales = new SalesmanData())
     {
         objPrimarySales.SalesChannelID = Convert.ToInt16(cmbTransferTo.SelectedValue);
         intResult = objPrimarySales.UpdateSalesChannelRetailerMap(Tvp);

         if (objPrimarySales.ErrorDetailXML != null && objPrimarySales.ErrorDetailXML != string.Empty)
         {
             ucMessage1.XmlErrorSource = objPrimarySales.ErrorDetailXML;
             return;
         }
         if (objPrimarySales.Error != null && objPrimarySales.Error != "")
         {
             ucMessage1.ShowError(objPrimarySales.Error);
             return;
         }
         if (intResult == 2)
         {
             ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
             return;
         }

         ucMessage1.ShowInfo("Retailers transferred succesfully");
         cleardata();
        
      }



 }

 public void cleardata()
 {
     cmbTransferFrom.SelectedValue = "0";
     cmbTransferTo.Items.Clear();
     cmbTransferTo.Items.Insert(0, new ListItem("Select", "0"));
     pnlGrid.Visible = false;

 }



 protected void btncancel_Click(object sender, EventArgs e)
 {
     cleardata();
     ucMessage1.Visible = false;
 }

 protected void grdRetailer_PageIndexChanging(object sender, GridViewPageEventArgs e)
 {
     grdRetailer.PageIndex = e.NewPageIndex;
     fillgrid();
 }

}
