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

public partial class Masters_HO_Admin_Mailing :PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                //pageInfo();
                HideControls();
                FillsalesChannelType();

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.Type = 5;           //For Mapping
            if (Convert.ToInt32(PageBase.SalesChanelTypeID) != 0)
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            }

            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref cmbsaleschanneltype, ObjSalesChannel.GetSalesChannelType(), str);
        };
    }
    void HideControls()
    {
        //ExportToExcel.Visible = false;
        //GridSalesChannel.Visible = false;
        // UpdGrid.Update();

    }
   

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbsaleschanneltype.SelectedIndex == 0)
            {
                ucMessage1.ShowInfo("Please select Sales Channel Type");
                HideControls();
                return;
            }
            FillGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    void FillGrid()
    {
     
        DataTable Dt = new DataTable();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

          
            if (cmbsaleschanneltype.SelectedValue != "0")
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
            }

            Dt = ObjSalesChannel.GetSalesChannelInfo();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            foreach (DataRow dr in Dt.Rows)
            {
                SendMailToUser(Convert.ToInt16(dr["SalesChannelID"]));
            }
            ucMessage1.ShowInfo("Mail Send Successfully");
        }
        else
        {
           ucMessage1.ShowInfo(Resources.Messages.NoRecord);

        }
    }
    private void SendMailToUser(int ChannelID)
    {
        try
        {
            DataTable dtSalesChannelInfo;
            using (SalesChannelData objSalesChannel = new SalesChannelData())
            {
                objSalesChannel.SalesChannelID = ChannelID;
                objSalesChannel.BlnShowDetail = true;
                dtSalesChannelInfo = objSalesChannel.GetSalesChannelInfo();
            };
            if (dtSalesChannelInfo != null && dtSalesChannelInfo.Rows.Count > 0)
            {
                string ErrDesc = string.Empty;
                string Password = string.Empty;
                using (Authenticates ObjAuth = new Authenticates())
                {
                    Password = ObjAuth.DecryptPassword(Convert.ToString(dtSalesChannelInfo.Rows[0]["Password"]), Convert.ToString(dtSalesChannelInfo.Rows[0]["PasswordSalt"]));
                };
                Mailer.LoginName = Convert.ToString(dtSalesChannelInfo.Rows[0]["LoginName"].ToString());
                Mailer.Password = Password;
                Mailer.EmailID = dtSalesChannelInfo.Rows[0]["Email"].ToString();
                Mailer.UserName = Convert.ToString(dtSalesChannelInfo.Rows[0]["SalesChannelName"].ToString());
                Mailer.sendmail("../../../" + strAssets + "/Mailer/CreateUser.htm");
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }


    

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ucMessage1.ShowControl = false;
        cmbsaleschanneltype.SelectedValue = "0";
       
        HideControls();
        dvhide.Visible = false;
    }
  
    protected void GridSalesChannel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
       
       
    }

    //void pageInfo()
    //{
    //    this.GridSalesChannel.Columns[0].HeaderText = Resources.Messages.SalesEntity + " Name";
    //    this.GridSalesChannel.Columns[3].HeaderText = Resources.Messages.SalesEntity + " Code";
    //    this.GridSalesChannel.Columns[8].HeaderText = Resources.Messages.SalesEntity + " Type";
    //}

    
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            cmbsaleschanneltype.SelectedValue = "0";
           
            FillGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
}
