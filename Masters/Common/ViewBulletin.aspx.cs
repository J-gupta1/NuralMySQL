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
public partial class BulletinBoard_ViewBulletin :PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                if (PageBase.SalesChanelID != 0)
                    LBAddBulletin.Visible = false;
                BindCategory();
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    
    }
 private void BindSubCategory()
    {
        try
        {
            DataTable dt = new DataTable();
       
            using (MastersData objMaster = new MastersData())
            {
                objMaster.UserId = PageBase.UserId;
                objMaster.CompanyId = PageBase.ClientId;
                objMaster.CategoryID = Convert.ToInt16(ddlCategory.SelectedValue);
                dt = objMaster.GetAllBulletinSubCategorybyCategoryId();
            };
            String[] colArray = { "SubCategoryID", "SubCategoryName" };
            PageBase.DropdownBinding(ref ddlSubCategory, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
 private void BindCategory()
 {
     try
     {
         DataTable dt = new DataTable();
         using (MastersData objMaster = new MastersData())
         {
             objMaster.UserId = PageBase.UserId;
             objMaster.CompanyId = PageBase.ClientId;
             dt = objMaster.GetAllBulletinCategory();
         };
         String[] colArray = { "CategoryID", "CategoryName" };
         PageBase.DropdownBinding(ref ddlCategory, dt, colArray);
     }
     catch (Exception ex)
     {
         ucMessage1.ShowError(ex.Message.ToString());
         PageBase.Errorhandling(ex);
     }
 }
 protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
 {
     BindSubCategory();

 }
 protected void btnSearch_Click(object sender, EventArgs e)
 {
     FillGrid();

 }
 void FillGrid()
 {
     DataTable Dt = new DataTable();
     using (BulletinData ObjBulletin = new BulletinData())
     {
         if (ddlSubCategory.SelectedIndex == -1)
             ObjBulletin.SubCategoryId = 0;
         else
         ObjBulletin.SubCategoryId=Convert.ToInt16(ddlSubCategory.SelectedValue);
         ObjBulletin.UserID = PageBase.UserId;
         Dt = ObjBulletin.GetBulletinInfoByUserId();
     };
     if (PageBase.AllowAllHierarchy == false)
     {
         GvBulletin.Columns[GvBulletin.Columns.Count - 1].Visible = false;
         GvBulletin.Columns[GvBulletin.Columns.Count - 2].Visible = false;
     }
     if (Dt != null && Dt.Rows.Count > 0)
     {
  
         GvBulletin.Visible = true;
         GvBulletin.DataSource = Dt;
         GvBulletin.DataBind();
      

     }
     else
     {
   
         GvBulletin.Visible = true;
         GvBulletin.DataSource = null;
         GvBulletin.DataBind();
     }
 }

 protected void LBAddBulletin_Click(object sender, EventArgs e)
 {
     Response.Redirect("ManageBulletin.aspx");

 }
 protected void GvBulletin_RowDataBound(object sender, GridViewRowEventArgs e)
 {
     try
     {
         if (e.Row.RowType == DataControlRowType.DataRow)
         {
             Int32 BulletinID = Convert.ToInt32(GvBulletin.DataKeys[e.Row.RowIndex].Value);
             GridViewRow GVR = e.Row;
             HyperLink HLDetails = default(HyperLink);
             HLDetails = (HyperLink)GVR.FindControl("HLDetails");
             string strViewBulletinDetail = null;
             strViewBulletinDetail = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(BulletinID), PageBase.KeyStr)).ToString().Replace("+", " ");
             {
                 HLDetails.Text = "Details";
                 HLDetails.Attributes.Add("onclick", "return BulletinDetail(\"" + strViewBulletinDetail + "\")");
                 //HLDetails.Attributes.Add("onClick", string.Format("return popup('" + strViewBulletinDetail + "')"));
             }
         }
     }
     catch (Exception ex)
     {
         ucMessage1.ShowError(ex.Message.ToString());
         PageBase.Errorhandling(ex);
     }


 }
 protected void GvBulletin_RowCommand(object sender, GridViewCommandEventArgs e)
 {
     Int32 BulletinId = Convert.ToInt16(e.CommandArgument);
     if (e.CommandName == "cmdEdit")
     {
         Response.Redirect("ManageBulletin.aspx?BulletinId=" + Server.UrlEncode(Crypto.Encrypt(Convert.ToString(BulletinId), PageBase.KeyStr)));
     }

 }
 protected void GvBulletin_PageIndexChanging(object sender, GridViewPageEventArgs e)
 {
     GvBulletin.PageIndex = e.NewPageIndex;
     FillGrid();

 }
 protected void btnActiveDeactive_Click(object sender, ImageClickEventArgs e)
 {
     try
     {
         ImageButton btnActiveDeactive = (ImageButton)sender;
         Int32 Result = 0;
         Int32 BulletinID = Convert.ToInt32(btnActiveDeactive.CommandArgument);
         using (BulletinData ObjBulletin = new BulletinData())
         {

             ObjBulletin.BulletinId = BulletinID;
             Result = ObjBulletin.UpdateStatusBulletinInfo();
         };
         if (Result >= 1)
         {

             ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
         }
         else
         {
             ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
         }
         FillGrid();
         
     }
     catch (Exception ex)
     {
         ucMessage1.ShowError(ex.Message.ToString());
         PageBase.Errorhandling(ex);
     }
 }
 protected void btnShowAll_Click(object sender, EventArgs e)
 {
     Clear();
     FillGrid();
 }
 void Clear()
 {
     ddlCategory.SelectedIndex = 0;
     ddlSubCategory.SelectedIndex =-1;
 }
 protected void btnCancel_Click(object sender, EventArgs e)
 {
     Clear();
     FillGrid();
 }

}
