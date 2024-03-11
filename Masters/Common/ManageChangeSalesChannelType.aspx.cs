using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using Cryptography;


public partial class Masters_Common_ManageChangeSalesChannelType : PageBase
{
    DataSet dsTwo;
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        if (!IsPostBack)
        {
            ucMessage1.Visible = false;

            FillSalesChannelType();
            ddlSalesChannelTypeTo.Items.Insert(0, new ListItem("Select", "0"));
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearData();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            DataTable SalesChannelInfo = new DataTable();
            DataColumn dc1 = new DataColumn("SalesChannelID", typeof(Int16));
            SalesChannelInfo.Columns.Add(dc1);


            foreach (GridViewRow grv in grdSalesChannelFrom.Rows)
            {

                CheckBox chk = (CheckBox)grv.FindControl("chkRetailerTransfer");
                if (chk.Checked == true)
                {
                    Label lbId = (Label)grv.FindControl("lblID");
                    DataRow dr = SalesChannelInfo.NewRow();
                    dr["SalesChannelID"] = Convert.ToInt16(lbId.Text);
                    SalesChannelInfo.Rows.Add(dr);

                }


            }
            if (SalesChannelInfo.Rows.Count > 0)
                gettransferdata(SalesChannelInfo);
            else
                ucMessage1.ShowInfo("Please Select any SalesChannel Type");


        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
   


    public void gettransferdata(DataTable SalesChannelTypeInfo)
    {
        int intResult = 0;
        DataTable Tvp = new DataTable();
        using (CommonData ObjCommom = new CommonData())
        {
            Tvp = ObjCommom.GettvpRetailerMap();
        }
        foreach (DataRow dr in SalesChannelTypeInfo.Rows)
        {
            DataRow drow = Tvp.NewRow();
            drow[0] = dr["SalesChannelID"].ToString();
            Tvp.Rows.Add(drow);

        }
        Tvp.AcceptChanges();

        using (SalesmanData objMapping = new SalesmanData())
        {
            objMapping.SalesChannelID = PageBase.SalesChanelID;
            objMapping.SalesChannelTypeID = Convert.ToInt16(ddlSalesChannelTypeTo.SelectedValue);

            intResult = objMapping.InsertInfoSalesChannelTypeMap(Tvp);

            if (objMapping.ErrorDetailXML != null && objMapping.ErrorDetailXML != string.Empty)
            {
                ucMessage1.XmlErrorSource = objMapping.ErrorDetailXML;
                return;
            }
            if (objMapping.Error != null && objMapping.Error != "")
            {
                ucMessage1.ShowError(objMapping.Error);
                return;
            }
            if (intResult == 2)
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                return;
            }

            ucMessage1.ShowSuccess("Sales Channel Type Changed Successfully");

            ClearData();

        }



    }


    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ClearData();
    }

  
    protected void grdfromPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSalesChannelFrom.PageIndex = e.NewPageIndex;
        BindSalesChannel();
    }


    protected void grdToPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSalesChannelFrom.PageIndex = e.NewPageIndex;
        BindSalesChannel();
    }
    protected void ddlSalesChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSalesChannelType.SelectedValue != "0")
        {
            BindSalesChannel();
        }
        else
        {
            grdSalesChannelFrom.DataSource = null;
            grdSalesChannelFrom.DataBind();
            Pnlfrom.Visible = false;
        }
    }
    public void fillsalesChannel()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            //DataSet ds = obj.GetParentForRetailerTransfer();
            //String[] colArray = { "SalesChannelID", "SalesChannelName" };
            //PageBase.DropdownBinding(ref ddlSalesChannelType, ds.Tables[0], colArray);
            //PageBase.DropdownBinding(ref cmbTransferTo, ds.Tables[1], colArray);

        }

    }
    
   
    void ClearData()
    {
        ddlSalesChannelType.SelectedValue = "0";
        ddlSalesChannelTypeTo.Items.Clear();
        grdSalesChannelFrom.DataSource = null;
        grdSalesChannelFrom.DataBind();
        Pnlfrom.Visible = false;
        ddlSalesChannelTypeTo.Items.Insert(0, new ListItem("Select", "0"));
 
    }
   
    void FillSalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            DataTable dt = new DataTable();
            String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
          
            dt = ObjSalesChannel.GetSalesChannelTypeV2();
            PageBase.DropdownBinding(ref ddlSalesChannelType, dt, StrCol);
        
    


        };
    }
    void BindSalesChannel()
    {
        try
        {
            dsTwo = new DataSet();
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlSalesChannelType.SelectedValue);
                ObjSalesChannel.ActiveStatus = 1;
                dsTwo = ObjSalesChannel.GetprcGetSalesChannelInformationList();
                if (dsTwo.Tables[0].Rows.Count > 0)
                {
                    grdSalesChannelFrom.DataSource = dsTwo.Tables[0];
                    grdSalesChannelFrom.DataBind();
                    Pnlfrom.Visible = true;
                }
                else
                {
                    Pnlfrom.Visible = false;
                }

                String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
                PageBase.DropdownBinding(ref ddlSalesChannelTypeTo, dsTwo.Tables[1], StrCol);

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
   
}
