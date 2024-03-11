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

public partial class UserControls_SalesChannelInfoFilterControl : System.Web.UI.UserControl
{
    Int16 saleschanneltypeid;
    int saleschannelid;
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!IsPostBack)
        {

            cmbParentSalesChannel.Items.Insert(0, new ListItem("Select", "0"));
            cmbLocation.Items.Insert(0, new ListItem("Select", "0"));
            cmbParentLocation.Items.Insert(0, new ListItem("Select", "0"));
            fillGrid();
            fillLocations();
            fillparentLoction();
            fillParentsalesChannel();
        }
        
    }


    public Int16 SaleschannelTypeID
    {
        get
        {
            return saleschanneltypeid;
        }
        set
        {
            saleschanneltypeid = value;
        }
    }

    public int SalesChannelID
    {
        get
        {
            return saleschannelid;
        }
    }


   protected void  fillparentLoction()
    {
        using (OrgHierarchyData obj = new OrgHierarchyData())
        {
            obj.SalesChanelTypeID = saleschanneltypeid;
            obj.SearchMode = 1;
            DataTable dt = obj.GetOrgHierarchy();
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref cmbParentLocation, dt, colArray);
            
         }

    }


   protected void fillLocations()
   {
       using (OrgHierarchyData obj = new OrgHierarchyData())
       {
           obj.SalesChanelTypeID = saleschanneltypeid;
           obj.SearchMode = 2;
           DataTable dt = obj.GetOrgHierarchy();
           String[] colArray = { "OrgnhierarchyID", "LocationName" };
           PageBase.DropdownBinding(ref cmbLocation, dt, colArray);
       }
   }

   protected void fillParentsalesChannel()
   {
       using (SalesChannelData obj = new SalesChannelData())
       {
           obj.SalesChannelTypeID = saleschanneltypeid;
           DataTable dt = obj.GetSalesChannelParent();
           String[] colArray = { "SalesChannelID", "SalesChannelName" };
           PageBase.DropdownBinding(ref cmbParentSalesChannel, dt, colArray);

       }

   }


   protected void fillGrid()
   {
       using (SalesChannelData obj = new SalesChannelData())
       {
           obj.SalesChannelTypeID = saleschanneltypeid;
           obj.ParentSalesChannelID = Convert.ToInt16(cmbParentSalesChannel.SelectedValue);
           obj.OrgnhierarchyID = Convert.ToInt32(cmbLocation.SelectedValue);
           obj.ParentLocation = Convert.ToInt32(cmbParentLocation.SelectedValue);
           DataTable dt = obj.GetSalesChannelInfoForControl();
           grdSalesChannelDetails.DataSource = dt;
           grdSalesChannelDetails.DataBind();
           //pnlGrid.Visible = true;
           //updGrid.Update();

       }
   }


   protected void btnSearch_click(object sender, EventArgs e)
   {
       fillGrid();
   }



   protected void btnCancel_click(object sender, EventArgs e)
   {
       cmbLocation.SelectedValue = "0";
       cmbParentLocation.SelectedValue = "0";
       cmbParentSalesChannel.SelectedValue = "0";
      
   }
  

  

   protected void GrdRow_DataBound(object sender, GridViewRowEventArgs e)
   {
       if (e.Row.RowType == DataControlRowType.DataRow)
       {
           Label lblSaleschannelId = (Label)e.Row.FindControl("lblSalesChannelID");
           Label lblSaleschannelName = (Label)e.Row.FindControl("lblSaleschannelName");
           Button btnSelect = (Button)e.Row.FindControl("btnSelect");
           string strID = lblSaleschannelId.Text;
           btnSelect.Attributes.Add("OnClick",  string.Format("return funcwindowclose('" + strID + "','" + lblSaleschannelName.Text + "');"));
          
        }

   }
}
