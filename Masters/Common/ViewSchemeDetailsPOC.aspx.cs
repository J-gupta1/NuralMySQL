using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using Cryptography;
using System.Data;
using DataAccess;

public partial class Masters_Common_ViewSchemeDetailsPOC : PageBase
{
    int SchemeID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/style.css");
            //StyleCss.Attributes.Add("href", @"http:\zEDSALESV2\Web\Assets\NexG\CSS\style.css");
           // StyleCss.Attributes.Add("href", @"file:///D:\zEDSALESV2\Web\Assets\NexG\CSS\popup.css");
            
            if ((Request.QueryString["SchemeID"] != null) && (Request.QueryString["SchemeID"] != ""))
            {
                SchemeID = Convert.ToInt32(Convert.ToString(Crypto.Decrypt((Request.QueryString["SchemeID"]).ToString().Replace(" ", "+"), PageBase.KeyStr)));   //Pankaj Kumar
            }
            if (!IsPostBack)
            {
                bindGrid(0);
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    protected void GridFilter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //GridSales.PageIndex = e.NewPageIndex;
        //FillGrid();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //GridSales.DataSource = null;
        //GridSales.DataBind();
        //updGrid.Update();
        //dvGrid.Visible = false;
    }
    protected void GridFilter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow GVR = e.Row;
                Button btnSelect = (Button)GVR.FindControl("btnSelect");
                Label lblSalesCode = ((Label)GVR.FindControl("lblSalesChanneCode"));
                Label lblSalesChannelName = ((Label)GVR.FindControl("lblSalesChanneName"));

            }
        }
        catch (Exception ex)
        {
        }
    }
    void bindGrid(Int32 Flag)
    {
        using (SchemeData objScheme = new SchemeData())
        {
            ucMessage1.Visible = false;
            objScheme.SchemeID = SchemeID;
            objScheme.Flag = Flag;
            DataSet dsScheme = objScheme.GetSchemeDetailInformation();
            if (dsScheme != null)
            {
                if (Flag == 1 | Flag == 0)
                {
                    lblSchemeLocation.Text = dsScheme.Tables[0].Rows[0]["LocationName"].ToString();
                    lblSchemeForType.Text = dsScheme.Tables[0].Rows[0]["SalesChannelTypeName"].ToString();
                }

                if (Flag == 2 | Flag == 0)
                {
                    Int32 index;
                    index = Flag == 2 ? 0 : 1;
                    dsScheme.Tables[index].Columns.Add("Syntax");
                    foreach(DataRow dr in dsScheme.Tables[index].Rows)
                    {
                        if (dr["DataType"].ToString() == "Numeric" | dr["DataType"].ToString() == "Date")
                        {
                            if(dr["FilterValue"].ToString().Contains("<"))
                                dr["Syntax"] = dr["Syntax"].ToString() + "<";
                            if (dr["FilterValue"].ToString().Contains(">"))
                                dr["Syntax"] = dr["Syntax"].ToString() + ">";
                            if (dr["FilterValue"].ToString().Contains("="))
                                dr["Syntax"] = dr["Syntax"].ToString() + "=";
                        }
                     
                        if (dr["DataType"].ToString() == "AlphaNumeric")
                        {
                            dr["Syntax"] = "=";
                        }
                    }
                    dsScheme.Tables[index].AcceptChanges();
                    dlstFilter.DataSource = dsScheme.Tables[index];
                    dlstFilter.DataBind();
                }

                if (Flag == 3 | Flag == 0)
                {
                    Int32 index;
                    index = Flag == 3 ? 0 : 2;
                    lblCriterialType.Text = dsScheme.Tables[index].Rows[0]["ComponentCriteriaTypeName"].ToString(); ;
                    lblPayoutType.Text = dsScheme.Tables[index].Rows[0]["ComponentPayoutTypeName"].ToString();
                    dlstPayout.DataSource = dsScheme.Tables[index];
                    dlstPayout.DataBind();

                }


            }
            else
            {
                //grdSchemeDetail.DataSource = null;
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);
            }

            pnlFilter.Visible = true;
            updGrid.Update();

            pnlPayout.Visible = true;
            updPayout.Update();
        }
    }
    protected void grdPayout_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //dlstPayout.PageIndex = e.NewPageIndex;
        //FillGrid();
    }
    protected void grdPayout_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow GVR = e.Row;
                Button btnSelect = (Button)GVR.FindControl("btnSelect");
                Label lblSalesCode = ((Label)GVR.FindControl("lblSalesChanneCode"));
                Label lblSalesChannelName = ((Label)GVR.FindControl("lblSalesChanneName"));

            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void dlstFilter_CancelCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        dlstFilter.EditItemIndex = -1;
        bindGrid(2);
    }
    protected void dlstFilter_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        dlstFilter.EditItemIndex = (int)e.Item.ItemIndex;
        bindGrid(2);
    }
    protected void dlstFilter_ItemDataBound1(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlComponentFilterName = (DropDownList)e.Item.FindControl("ddlComponentFilterName");
                using (SchemeData objFilter = new SchemeData())
                {
                    bindComponentFilter(ddlComponentFilterName, objFilter.GetSchemeComponentFilterMaster());
                }



            }
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                using (SchemeData objFilter = new SchemeData())
                {
                    DropDownList ddlComponentFilterName = (DropDownList)e.Item.FindControl("ddlComponentFilterName");
                    Label lblComponentFilterID = (Label)e.Item.FindControl("lblComponentFilterID");
                    bindComponentFilter(ddlComponentFilterName, objFilter.GetSchemeComponentFilterMaster());
                    ddlComponentFilterName.SelectedValue = lblComponentFilterID.Text;
                }
                RegularExpressionValidator regFilterValueAlpha = (RegularExpressionValidator)e.Item.FindControl("regFilterValue");
                //RegularExpressionValidator regFilterValueNumeric = (RegularExpressionValidator)e.Item.FindControl("regFilterValueNumeric");
                Label lblDatatype = (Label)e.Item.FindControl("lblDataType");
                //if (lblDatatype.Text == "Numeric")
                //{
                //    regFilterValueAlpha.ValidationExpression = @"^\d$";

                //}
                //if (lblDatatype.Text == "Date")
                //{
                //    regFilterValueAlpha.ValidationExpression = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";
                //}
                //if (lblDatatype.Text == "AlphaNumeric")
                //{
                //    regFilterValueAlpha.ValidationExpression = "[0-9a-zA-Z' ']{25,}";
                //    regFilterValueAlpha.Enabled = true;
                //}
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void dlstFilter_UpdateCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        int intResult = 0;
        try
        {
            if (((DropDownList)e.Item.FindControl("ddlComponentFilterName")).SelectedValue == "0" && ((TextBox)e.Item.FindControl("txtFilterValue")).Text == "")
            {
                ucMessage1.ShowError("Please fill all the Required Information");
                return;
            }
            SchemeData ObjData = new SchemeData();
            ObjData.SchemeComponentFilterValueID = Convert.ToInt32(dlstFilter.DataKeys[e.Item.ItemIndex]);
            ObjData.SchemeFilterName = ((DropDownList)e.Item.FindControl("ddlComponentFilterName")).SelectedItem.ToString();
            ObjData.SchemeFilterValue = ((TextBox)e.Item.FindControl("txtFilterValue")).Text;
            ObjData.SchemeFilterID = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlComponentFilterName")).SelectedValue);
            ObjData.SchemeID = SchemeID;
            ObjData.Flag = 1;
            intResult = ObjData.InsertUpdateSchemeFilter();
            if (intResult == 0)
            {
                ucMessage1.ShowSuccess(Resources.Messages.DataUploadSuccess);
                dlstFilter.EditItemIndex = -1;
                bindGrid(2);
            }
            if (intResult == 1)
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                return;
            }
            if (intResult == 3)
            {
                ucMessage1.ShowError("Filter already exist on the Same Scheme");
            }

        }

        catch (Exception Ex)
        {
            ucMessage1.ShowError(Ex.Message.ToString());
            PageBase.Errorhandling(Ex);

        }

    }
    protected void dlstFilter_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            int intResult = 0;
            if (e.CommandName == "addFilter")
            {
                try
                {
                    SchemeData ObjData = new SchemeData();
                    ObjData.SchemeComponentFilterValueID = 0;
                    ObjData.SchemeFilterName = ((DropDownList)e.Item.FindControl("ddlComponentFilterName")).SelectedItem.ToString();
                    ObjData.SchemeFilterValue = ((DropDownList)e.Item.FindControl("ddlSyntax")).SelectedItem.ToString()+((TextBox)e.Item.FindControl("txtFilterValue")).Text;
                    ObjData.SchemeFilterID = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlComponentFilterName")).SelectedValue);
                    ObjData.SchemeID = SchemeID;
                    ObjData.Flag = 2;
                    intResult = ObjData.InsertUpdateSchemeFilter();
                    if (intResult == 0)
                    {
                        dlstFilter.EditItemIndex = -1;
                        bindGrid(2);
                        ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    }
                    if (intResult == 1)
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                    if (intResult == 3)
                    {
                        ucMessage1.ShowError("Filter already exist on the Same Scheme");
                    }
                }
                catch (Exception ex)
                {
                    dlstFilter.EditItemIndex = -1;
                    ucMessage1.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.ToString());
        }
    }
    void bindComponentFilter(DropDownList ddlFilter, DataTable dt)
    {
        ddlFilter.DataSource = dt;
        ddlFilter.DataTextField = "ComponentFilterName";
        ddlFilter.DataValueField = "ComponentFilterID";
        ddlFilter.DataBind();
        ddlFilter.Items.Insert(0, new ListItem("Select", "0"));
    }
    protected void dlstPayout_CancelCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        dlstPayout.EditItemIndex = -1;
        bindGrid(3);
    }
    protected void dlstPayout_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        dlstPayout.EditItemIndex = (int)e.Item.ItemIndex;
        bindGrid(3);
    }
    protected void dlstPayout_ItemDataBound1(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
            {
                 Label lblAchievementTo = (Label)e.Item.FindControl("lblAchievementTo");
                if (lblAchievementTo.Text == "-1.00" | lblAchievementTo.Text == "-1")
                {
                    lblAchievementTo.Text = "No Limit";
                }
             
            }
            if (e.Item.ItemType == ListItemType.EditItem)
            {
               
                using (SchemeData objFilter = new SchemeData())
                {
                    //DropDownList ddlComponentFilterName = (DropDownList)e.Item.FindControl("ddlComponentFilterName");
                    //Label lblComponentFilterID = (Label)e.Item.FindControl("lblComponentFilterID");
                    //bindComponentFilter(ddlComponentFilterName, objFilter.GetSchemeComponentFilterMaster());
                    //ddlComponentFilterName.SelectedValue = lblComponentFilterID.Text;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void dlstPayout_UpdateCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        int intResult = 0;
        try
        {
            if (((TextBox)e.Item.FindControl("txtAchievementFrom")).Text == "" && ((TextBox)e.Item.FindControl("txtAchievementTo")).Text == "" && ((TextBox)e.Item.FindControl("txtPayoutRate")).Text == "")
            {
                ucMessage1.ShowError("Please fill all the Required Information");
                return;
            }
            if (Convert.ToDecimal(((TextBox)e.Item.FindControl("txtAchievementFrom")).Text) > Convert.ToDecimal(((TextBox)e.Item.FindControl("txtAchievementTo")).Text))
            {
                ucMessage1.ShowError("Achievement From would be less than Achievement To");
                return;
            }
            SchemeData ObjData = new SchemeData();
            ObjData.SchemeComponentPayoutSlabID = Convert.ToInt32(dlstPayout.DataKeys[e.Item.ItemIndex]);
            ObjData.AchievementFrom = Convert.ToDecimal(((TextBox)e.Item.FindControl("txtAchievementFrom")).Text);
            ObjData.AchievementTo = Convert.ToDecimal(((TextBox)e.Item.FindControl("txtAchievementTo")).Text);
            ObjData.SchemeID = SchemeID;
            ObjData.PayOutRate = Convert.ToDecimal(((TextBox)e.Item.FindControl("txtPayoutRate")).Text);
            ObjData.Flag = 1;
            intResult = ObjData.InsertUpdateSchemePayOut();
            if (intResult == 0)
            {
                ucMessage1.ShowSuccess(Resources.Messages.DataUploadSuccess);
                dlstPayout.EditItemIndex = -1;
                bindGrid(3);
            }
            if (intResult == 1)
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                return;
            }
            if (intResult == 3)
            {
                ucMessage1.ShowError("Payout already exist on the Same Scheme");
            }

        }

        catch (Exception Ex)
        {
            ucMessage1.ShowError(Ex.Message.ToString());
            PageBase.Errorhandling(Ex);

        }

    }
    protected void dlstPayout_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            int intResult = 0;
            if (e.CommandName == "addPayout")
            {
                try
                {

                    if (Convert.ToDecimal(((TextBox)e.Item.FindControl("txtAchievementFrom")).Text) > Convert.ToDecimal(((TextBox)e.Item.FindControl("txtAchievementTo")).Text))
                    {
                        ucMessage1.ShowError("Achievement From would be less than Achievement To");
                        return;
                    }
                    SchemeData ObjData = new SchemeData();
                    ObjData.SchemeComponentPayoutSlabID = 0;
                    ObjData.AchievementFrom = Convert.ToDecimal(((TextBox)e.Item.FindControl("txtAchievementFrom")).Text);
                    ObjData.AchievementTo = Convert.ToDecimal(((TextBox)e.Item.FindControl("txtAchievementTo")).Text);
                    ObjData.SchemeID = SchemeID;
                    ObjData.PayOutRate = Convert.ToDecimal(((TextBox)e.Item.FindControl("txtPayoutRate")).Text);
                    ObjData.Flag = 2;
                    intResult = ObjData.InsertUpdateSchemePayOut();
                    if (intResult == 0)
                    {
                        dlstPayout.EditItemIndex = -1;
                        bindGrid(3);
                        ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    }
                    if (intResult == 1)
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                    if (intResult == 3)
                    {
                        ucMessage1.ShowError("Payout already exist on the Same Scheme");
                    }
                }
                catch (Exception ex)
                {
                    dlstPayout.EditItemIndex = -1;
                    ucMessage1.Visible = true;
                }
            }

            if (e.CommandName == "Delete")
            {

                SchemeData ObjData = new SchemeData();
                ObjData.SchemeComponentPayoutSlabID = Convert.ToInt32(dlstPayout.DataKeys[e.Item.ItemIndex]);
                ObjData.SchemeID = SchemeID;
                intResult = ObjData.DeleteSchemePayOut();
                if (intResult == 0)
                {
                    dlstPayout.EditItemIndex = -1;
                    bindGrid(3);
                    ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                }
                if (intResult == 1)
                {
                    ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                }
                
            }

        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.ToString());
        }
    }
}
