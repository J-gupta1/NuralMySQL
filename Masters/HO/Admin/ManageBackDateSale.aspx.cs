/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 22-Mar-2016, Sumit Maurya, #CC01, Issue of page getting blocked on the execution of event(s) due to update panel resolved.
====================================================================================================================================
 */
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using BussinessLogic;
using DataAccess;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Masters_HO_Admin_ManageBackDateSale : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
      
        try
        {
            if (!IsPostBack)
            {

                FillSaleChanneType();
                fillGrid();
                
            }
        }
        catch (Exception ex)
        {
            //ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            ucMsg.ShowInfo(ex.Message.ToString());
        }
    }

   

    public void fillGrid()
    {
        ViewState["Table"] = null;
        DataTable Dt = new DataTable();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            
           Dt = ObjSalesChannel.GetSalesChannel();
            
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            ViewState["Table"] = Dt;

            grdBkSaleList.Visible = true;
            grdBkSaleList.DataSource = Dt;
            grdBkSaleList.DataBind();
            

        }
        else
        {
            grdBkSaleList.Visible = false;
            grdBkSaleList.DataSource = null;
            grdBkSaleList.DataBind();
            ucMsg.ShowInfo(Resources.Messages.NoRecord);
            
        }
        //updgrid.Update();
    }


    void FillSaleChanneType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            try
            {
                ddlSaleType.Items.Clear();
                String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
                PageBase.DropdownBinding(ref ddlSaleType, ObjSalesChannel.GetSalesChannelTypeName(), StrCol);
            }

            catch (Exception ex)
            {
                //ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                ucMsg.ShowInfo(ex.Message.ToString());
            }
        }
    }


   
    protected void grdBkSaleList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
         //int Result = 0;
         if (e.CommandName == "cmdEdit")
            {
                ViewState["Table"] = null;
               DataTable Dt = new DataTable();
               
              using (SalesChannelData ObjSalesChannel = new SalesChannelData())
              {
                try
                {
                    ucMsg.Visible = false;
                   

                   ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(e.CommandArgument);
                   ViewState["SalesChannelTypeID"] = ObjSalesChannel.SalesChannelTypeID;

                   Dt = ObjSalesChannel.GetSalesChannel();



                   ObjSalesChannel.BackDaysNumber = Convert.ToInt32(Dt.Rows[0]["NumberOfBackDays"]);
                   txtBackSaleDays.Text = Convert.ToString(Dt.Rows[0]["NumberOfBackDays"]);
                   txtBackSaleDaysSaleReturn.Text = Convert.ToString(Dt.Rows[0]["BackDaysAllowedForSaleReturn"]);
                   ddlSaleType.Enabled = false;
                   ddlSaleType.SelectedValue = e.CommandArgument.ToString(); 
                   
                   Updata.Visible = true;

                 }
                catch (Exception ex)
                {
                   //ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    ucMsg.ShowInfo(ex.Message.ToString());
                }


            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
        try
                {
                   
                    ObjSalesChannel.Error = "";
                    ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ViewState["SalesChannelTypeID"]);
                    ObjSalesChannel.BackDaysNumber = Convert.ToInt32(txtBackSaleDays.Text.ToString());
                    ObjSalesChannel.BackDaysNumberSaleReturn = Convert.ToInt32(txtBackSaleDaysSaleReturn.Text.ToString());
                    ObjSalesChannel.SalesChannelTypeName = Convert.ToString(ddlSaleType.SelectedValue);
                    if (txtBackSaleDays.Text.Contains("-") == true)
                    {
                        ucMsg.ShowInfo("Only positive value is allowed.");
                        return;
                    }
                    if (txtBackSaleDaysSaleReturn.Text.Contains("-") == true)
                    {
                        ucMsg.ShowInfo("Only positive value is allowed.");
                        return;
                    }
                    ObjSalesChannel.UpdateSalesChannel();


                    if (ObjSalesChannel.Error == "")
                    {

                        fillGrid();
                        Updata.Visible = false;
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        ViewState["SalesChannelTypeID"] = null;
                       
                    }
                    else
                    {
                        ucMsg.ShowError("Record Duplicated");
                    }
                }
                catch (Exception ex)
                {
                    ucMsg.ShowInfo(ex.Message.ToString());
                }
            }  
    }

    
    protected void grdBkSaleList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdBkSaleList.PageIndex = e.NewPageIndex;
        fillGrid();
    }
    
}
