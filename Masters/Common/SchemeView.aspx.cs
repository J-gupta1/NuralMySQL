using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data;
using Cryptography;
using BussinessLogic;

public partial class Masters_HO_Common_SchemeView  : PageBase
    {

    DataTable schemeinfo ;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                fillComponent();
                BindTimeperiod();
                fillPayoutBase();
                blankall();
            }
        }


        public void blankall()
        {
            ucFromDate.Date = "";
            ucToDate.Date = "";
            txtScheme.Text = "";
            cmbComponentType.SelectedValue = "0";
            cmbPayoutBase.SelectedValue = "0";
            pnldaterng.Visible = false;
            pnlTargetdr.Visible = false;
            updSeprate.Update();
            pnlGrid.Visible = false;
            updGrid.Update();

        }

        private void fillComponent()
        {
            using (SchemeData obj = new SchemeData())
            {
                obj.SelectionType = 1;
                DataTable dt = obj.GetSchemeComponentsTypeDetails();
                cmbComponentType.DataSource = dt;
                cmbComponentType.DataTextField = "ComponentCriteriaTypeName";
                cmbComponentType.DataValueField = "ComponentTypeCode";
                cmbComponentType.DataBind();
                cmbComponentType.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        public void BindTimeperiod()
        {
            using (TargetData objTarget = new TargetData())
            {
                chkLst.DataSource = objTarget.GetTimePeriod();
                chkLst.DataValueField = "FinancialCalenderID";
                chkLst.DataTextField = "FinancialCalenderFortnight";
                chkLst.DataBind();
            }

        }

        public void fillPayoutBase()
        {
            using (SchemeData obj = new SchemeData())
            {
                obj.SelectionType = 1;
                DataTable dt = obj.GetPaymentTypeDetails();
                cmbPayoutBase.DataSource = dt;
                cmbPayoutBase.DataTextField = "ComponentPayoutTypeName";
                cmbPayoutBase.DataValueField = "PaymentBaseCode";
                cmbPayoutBase.DataBind();
                cmbPayoutBase.Items.Insert(0, new ListItem("Select", "0"));
                updmain.Update();
            }
        }

        public DataTable getFortnights()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("OFID");
            DataColumn dc2 = new DataColumn("Status");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            return dt;

        }


        void bindGrid()
        {
            using (SchemeData objScheme = new SchemeData())
            {

                ucMessage.Visible = false;
                //objScheme.CompanyID = PageBase.CompanyID;
                objScheme.SchemeName = txtScheme.Text;
                objScheme.FromDate = ucFromDate.Date;
                objScheme.ToDate = ucToDate.Date;
                if (ucFromDate.Date != "" && ucToDate.Date != "")
                {
                    if (Convert.ToDateTime(ucFromDate.Date) > Convert.ToDateTime(ucToDate.Date))
                    {
                        ucMessage.ShowInfo(Resources.Messages.InvalidDate);
                        return;
                    }
                }
                
                if (cmbComponentType.SelectedValue != "0")
                {
                    string[] one = cmbComponentType.SelectedValue.Split('-');
                    objScheme.ComponentTypeID = Convert.ToInt32(one[0]);
                }
                if (cmbPayoutBase.SelectedValue != "0")
                {
                    string[] two = cmbPayoutBase.SelectedValue.Split('-');
                    objScheme.PayOutBase = Convert.ToInt32(two[0]);
                }
                    objScheme.SelectionMode = 1;
                DataTable dtOfs = getFortnights();
                foreach (ListItem lst in chkLst.Items)
                {
                    if (lst.Selected)
                    {
                        DataRow dr3 = dtOfs.NewRow();
                        dr3["OFID"] = lst.Value;
                        dr3["Status"] = "1";
                        dtOfs.Rows.Add(dr3);
                    }
                }
                DataTable dtScheme = objScheme.GetSchemeInformation(dtOfs);
                schemeinfo = dtScheme;
                
                if (dtScheme != null && dtScheme.Rows.Count > 0)
                {
                    ViewState["DtExport"] = dtScheme;
                    grdSchemeDetail.DataSource = dtScheme;
                }
                else
                {
                    grdSchemeDetail.DataSource = null;
                    ucMessage.ShowInfo(Resources.Messages.NoRecord);
                }

                grdSchemeDetail.DataBind();
                pnlGrid.Visible = true;
                updGrid.Update();
            }
        }
      
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindGrid();

        }
        protected void grdSchemeDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 intScheme = Convert.ToInt16(e.CommandArgument);
                if (e.CommandName == "Active")
                {
                   using(SchemeData obj = new SchemeData())
                    {
                        obj.SchemeID = intScheme;
                        DataTable dtOfs = getFortnights();
                        DataTable dt = obj.GetSchemeInformation(dtOfs);
                        obj.Status = Convert.ToInt16(dt.Rows[0]["SchemeStatus"]);
                        if(obj.Status == 1)
                        {
                            obj.Status = 0;
                        }
                        else
                        {
                            obj.Status = 1;
                        }
                        obj.SchemeID = Convert.ToInt32(dt.Rows[0]["SchemeID"]);
                        obj.UpdateSchemeStatus();
                        bindGrid();
                        ucMessage.ShowInfo(Resources.Messages.StatusChanged);

                    }
                }
                

                if (e.CommandName == "cmdDetails")
                {
                    string str = string.Format("ViewSchemeDetails.aspx?ID ={0}", e.CommandArgument);
                    Response.Redirect(str);

                }
 
            }
            catch (Exception ex)
            {


                PageBase.Errorhandling(ex);
                ucMessage.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
           
        }
        protected void grdSchemeDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string strViewDBranchDtlURL = null;
                    Int32 intSchemeID = Convert.ToInt16(grdSchemeDetail.DataKeys[e.Row.RowIndex].Value);
                    GridViewRow GVR = e.Row;
                    HyperLink HLDetails = default(HyperLink);
                    HLDetails = (HyperLink)GVR.FindControl("HLDetails");
                    //if (PageBase.OfficeLevel != 5)
                    //{
                    //    imgbtn.Enabled = false;
                    //}
                    string strURL = Crypto.Encrypt(Convert.ToString(intSchemeID), PageBase.KeyStr);
                    //HLDetails.NavigateUrl = "#";
                    //HLDetails.Attributes.Add("OnClick", "popup('" + strURL + "')");

                    //strViewDBranchDtlURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(intSchemeID), PageBase.KeyStr)).ToString().Replace("+", " ");
                    {
                        HLDetails.Text = "Details";
                        HLDetails.Attributes.Add("onClick", string.Format("return popup('" + strURL + "')"));
                    }


                    //strViewDBranchDtlURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(SalesChannelID), PageBase.KeyStr)).ToString().Replace("+", " ");
                    //{
                    //    HLDetails.Text = "Details";
                    //    HLDetails.Attributes.Add("onClick", string.Format("return popup()"));
                    //}
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                PageBase.Errorhandling(ex);
            }


        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["DtExport"] != null)
                {
                    DataTable Dt = (DataTable)ViewState["DtExport"];
                    DataTable Dtexport;  
                    string[] StrCol = new string[] { "SchemeName", "ComponentType", "SchemeStartDate", "SchemeEndDate", "SchemePeriod", "StatusName" };
                    Dtexport = Dt.DefaultView.ToTable(true, StrCol);
                    Dtexport.Columns["StatusName"].ColumnName = "Status";
                    Dtexport.Columns["ComponentType"].ColumnName = "SchemeBasedOn";
                    DataSet Ds = new DataSet();
                    Ds.Merge(Dtexport);
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SchemeList";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(Ds, FilenameToexport);
                    ViewState["DtExport"] = null;
                }
                else
                {

                    ucMessage.ShowInfo(Resources.Messages.NoRecord);

                }
            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);

            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            blankall();
            ucMessage.Visible = false;

        }
        protected void LBCreateScheme_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageScheme.aspx");
        }


        protected void exportToExel_Click(object sender, EventArgs e)
        {
            bindGrid();
            DataTable dt = schemeinfo.Copy();
            string[] DsCol;
            DsCol = new string[] { "SchemeName", "SchemeStartDate", "SchemeEndDate", "SchemeComponentType", "SchemeCategory", "ComponentPayoutTypeName", "CurrentStatus" };

            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["CurrentStatus"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                try
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string strName = "SchemeDetails";
                    string FilenameToexport = strName;
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    //ViewState["Table"] = null;
                }
                catch (Exception ex)
                {
                    ucMessage.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
            else
            {
                ucMessage.ShowError(Resources.Messages.NoRecord);

            }


        }
        protected void cmbComponentType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            string[] strids = cmbComponentType.SelectedValue.Split('-');
            foreach (ListItem lst in chkLst.Items)
            {
                if (lst.Selected)
                {
                    lst.Selected = false;
                }
            }
            ucFromDate.Date = "";
            ucToDate.Date = "";

            if (strids[1].ToString() == "1")
            {
                pnlTargetdr.Visible = true;
                pnldaterng.Visible = false;
                updSeprate.Update();
            }
            else
            {
                pnlTargetdr.Visible = false;
                pnldaterng.Visible = true;
                updSeprate.Update();
            }
        }
        protected void LBViewScheme_Click(object sender, EventArgs e)
        {
            //Response.Redirect("ManageScheme.aspx", false);
            Response.Redirect("ManageSchemeV2.aspx", false);
        }
      


}
