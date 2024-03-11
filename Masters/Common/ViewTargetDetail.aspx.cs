using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using Cryptography;
using DataAccess;
using System.Data;
/*
 * ------------------------------------------------------------------------------------------------
 * Change Log
 * ------------------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description.
 * 31-Jan-2018, Sumit Maurya, #CC01, user can edit Target now (Done for Comio).
 * ------------------------------------------------------------------------------------------------
 */

public partial class ViewTargetDetail : PageBase
    {
        int intTargetId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css");
                if (!IsPostBack)
                {

                    if ((Request.QueryString["TargetID"] != null) && (Request.QueryString["TargetID"] != ""))
                    {
                        intTargetId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt((Request.QueryString["TargetID"].ToString().Replace(" ", "+")), PageBase.KeyStr)));
                       
                        bindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                lblHeader.Text=(Resources.Messages.ErrorMsgTryAfterSometime);
                PageBase.Errorhandling(ex);
            }
        }
        void bindGrid()
        {
            using (TargetData objTarget = new TargetData())
            {
              
                objTarget.TargetID = intTargetId;
                objTarget.CompanyId = PageBase.ClientId;
                objTarget.UserId = PageBase.UserId;
               
                DataTable dtdetail = objTarget.GetTargetInfo();
                if (dtdetail.Rows.Count > 0)
                {
                    grdDetail.DataSource = dtdetail;
                    grdDetail.DataBind();
                }
                else
                {
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();
                    lblHeader.Text=(Resources.Messages.NoRecord);
                }
            }
        }
        
    /* #CC01 Add Start */
        protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Label LblTarget = (Label)e.Row.FindControl("lblTarget");
                TextBox TxtTarget = (TextBox)e.Row.FindControl("txtTarget");
                if(Request.QueryString["Edit"]!=null)
                {
                    if(Convert.ToString(Request.QueryString["Edit"])=="1")
                    {
                        LblTarget.Visible = false;
                        TxtTarget.Visible = true;
                        TxtTarget.Enabled = true;
                        btnUpdate.Visible = true;
                    }
                    else
                    {
                        LblTarget.Visible = true;
                        TxtTarget.Visible = false;
                        TxtTarget.Enabled = false;
                        btnUpdate.Visible = false;
                    }
                }

            }
            catch (Exception ex )
            {                
                 PageBase.Errorhandling(ex);
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTargetDetail = new  DataTable();
                dtTargetDetail.Columns.Add("TargetID", typeof(System.Int64));
                dtTargetDetail.Columns.Add("TargetDetaiID", typeof(System.Int64));
                dtTargetDetail.Columns.Add("Target", typeof(System.Int64));
                
                foreach(GridViewRow gr in grdDetail.Rows)
                {
                    HiddenField hdnTargetDetailID = (HiddenField)gr.FindControl("hdnTargetDetailID");
                    TextBox txtTarget = (TextBox)gr.FindControl("txtTarget");
                    DataRow dr = dtTargetDetail.NewRow();
                    dr["TargetID"] = Convert.ToInt32(Convert.ToString(Crypto.Decrypt((Request.QueryString["TargetID"].ToString().Replace(" ", "+")), PageBase.KeyStr)));
                    dr["TargetDetaiID"] = Convert.ToInt64(hdnTargetDetailID.Value);
                    dr["Target"] = txtTarget.Text;
                    
                    dtTargetDetail.Rows.Add(dr);
                    dtTargetDetail.AcceptChanges();
                }

                using(TargetData objTarget= new TargetData())
                {
                    objTarget.UserId = PageBase.UserId;
                    objTarget.Dt = dtTargetDetail;
                    int intResult= objTarget.UpdateTarget();
                    if(intResult==0)
                    {
                        ucMessage1.ShowSuccess("Records Updated Successfully.");
                    }
                    else
                    {
                        ucMessage1.ShowInfo(objTarget.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);
            }
        }
        /* #CC01 Add End */
        
}
