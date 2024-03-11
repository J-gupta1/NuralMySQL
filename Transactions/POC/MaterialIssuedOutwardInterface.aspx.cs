using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Data.SqlClient;

public partial class Transactions_POC_MaterialIssuedOutwardInterface : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucDateForGRN.MaxRangeValue = System.DateTime.Now;
        ucDateForGRN.RangeErrorMessage = "Date Can't be more than current Date.";
        ucMsg.Visible = false;
        txtCourierName.Enabled = true;
        string strUrl = Request.Url.ToString();
        string[] str = strUrl.Split('?');
        if (str[1] != "0")
        {
            ucMsg.ShowInfo("Records saved sucessfully");
        }
        if (!IsPostBack)
        {
            bindWarehouse();
            txtCourierName.Enabled = false;
            ddlUser.Items.Insert(0, new ListItem("Select", "0"));
        }
        Button1.Attributes.Add("onclick", "return hithereforOutwards('" + grdSalesChannel.ID + "','" + lblCheck.ID + "')");
        //btnSave.Attributes.Add("onclick", "return forsave()");
    }

    void bindWarehouse()
    {
        DataTable dt = new DataTable();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = 5;

            dt = ObjSalesChannel.GetSalesChannelInfo();
            string[] colArray = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref ddlWarehouse, dt, colArray);
            if (PageBase.SalesChanelID != 0)
            {
                ddlWarehouse.SelectedValue = Convert.ToString(PageBase.SalesChanelID);
                ddlWarehouse.Enabled = false;
            }
        }
    }


    //public void fillgrid()
    //{
    //    using (ProductData obj = new ProductData())
    //    {
    //        obj.SKUSelectionMode = 1;
    //        DataTable dt = obj.SelectSKUModelwiseInfo();
    //        GridGRN.DataSource = dt;
    //        GridGRN.DataBind();
    //    }

    //}


    private bool PageValidatesave()
    {

        if (ucDateForGRN.Date != "")
        {
            if (Convert.ToDateTime(ucDateForGRN.Date) > Convert.ToDateTime(ucDateForGRN.Date))
            {
                ucMsg.ShowInfo("Invoice Date can't be greater than Grn Date");
                return false;
            }
        }
        if (txtDocketNo.Text != "")
        {
            if (ucDateForGRN.Date == "")
            {
                ucMsg.ShowInfo("Please insert Invoice Date for invoice Number");
                return false;
            }
        }

        if (ucDateForGRN.Date != "")
        {
            if (txtDocketNo.Text == "")
            {
                ucMsg.ShowInfo("Please insert Invoice Number for invoice Date");
                return false;
            }
        }
        return true;
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        //getdata();
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearControls();
        //Btnsave.Enabled = true;

    }

    void ClearControls()
    {
        txtDocketNo.Text = "";
        ddlmodeofReceipt.SelectedValue = "0";
        ddlUser.SelectedValue = "0";
        ddlUser.Enabled = false;
        txtCourierName.Text = "";
        ucDateForGRN.Date = "";
        //ucDateForGCN.Date = "";
        txtRemarks.Text = "";
        ddlWarehouse.SelectedIndex = 0;
        //GridGRN.DataSource = null;
        //GridGRN.DataBind();
        updGrid.Update();
        pnlGrid.Visible = false;
        ViewState["MaterialIssue"] = null;
        txtCourierName.Enabled = false;
    }




    protected void GridGRN_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RequiredFieldValidator reqval = new RequiredFieldValidator();
                //reqval = (RequiredFieldValidator)

                e.Row.FindControl("reqSerialNo");
                TextBox txtSerial = (TextBox)e.Row.FindControl("txtSerial");
                TextBox txtBatch = (TextBox)e.Row.FindControl("txtBatch");
                int inStatus = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SerialisedMode"));
                if (inStatus == 1)
                {
                    //reqval.Visible = true;
                    txtSerial.Enabled = false;
                    txtBatch.Enabled = false;
                }
                else if (inStatus == 2)
                {
                    txtBatch.Enabled = true;
                    txtSerial.Enabled = false;
                }
                else if (inStatus == 3)
                {
                    txtSerial.Enabled = true;
                    txtBatch.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            using (ProductData objProductData = new ProductData())
            {
                objProductData.SalesChannelID = Convert.ToInt32(ddlWarehouse.SelectedValue);
                dt = objProductData.SelectUserBasedonSalesChannelID();
                String[] StrCol = new String[] { "UserID", "UserName" };
                if (dt != null && dt.Rows.Count > 0)
                {
                    PageBase.DropdownBinding(ref ddlUser, dt, StrCol);
                    ddlUser.Enabled = true;
                }
                else
                    ddlUser.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    

    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        GetSearchData(ucPagingControl1.CurrentPage);
        updGrid.Update();

    }

    protected void BtnProceed_Click(object sender, EventArgs e)
    {

        fillcombos();
        pnlSearch.Visible = true;
        UpdSearch.Update();
        //fillgrid();
        //pnlGrid.Visible = true;
    }

    protected void btnSerchSku_Click(object sender, EventArgs e)
    {
        GetSearchData(1);
        pnlGrid.Visible = true;
       
        updGrid.Update();
       
    }

    public void GetSearchData(int pageno)
    {
        using (ProductData obj = new ProductData())
        {
            obj.SKUProdCatId = Convert.ToInt32(cmbSerProdCat.SelectedValue);
            obj.SKUModelId = Convert.ToInt32(cmbSerModel.SelectedValue);
            obj.SKUName = txtSerName.Text.ToString();
            obj.SKUCode = txtSerCode.Text.ToString();
            obj.PageNo = pageno;
            obj.PageSize = 5;
            DataTable dt = obj.SelectSKUBlockwiseInfo();
            ucPagingControl1.TotalRecords = obj.Elements;
            ucPagingControl1.PageSize = 5;
            ucPagingControl1.SetCurrentPage = pageno;
            grdSalesChannel.DataSource = dt;
            grdSalesChannel.DataBind();
            pnlGrid.Visible = true;
        }
    }

    protected void cmbSerProdcat_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                if (cmbSerProdCat.SelectedValue == "0")
                {
                    cmbSerModel.Items.Clear();
                    cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
                    cmbSerModel.SelectedValue = "0";
                }
                else
                {
                    objproduct.ModelProdCatId = Convert.ToInt16(cmbSerProdCat.SelectedValue.ToString());
                    objproduct.ModelSelectionMode = 1;
                    DataTable dtmodelfil = objproduct.SelectModelInfo();
                    String[] colArray1 = { "ModelID", "ModelName" };
                    PageBase.DropdownBinding(ref cmbSerModel, dtmodelfil, colArray1);
                    cmbSerModel.SelectedValue = "0";
                    UpdSearch.Update();

                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            }
        }
    }

    public void fillcombos()
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                DataTable dt = objproduct.SelectAllProdCatInfo();


                cmbSerProdCat.Items.Clear();
                String[] colArray1 = { "ProductCategoryID", "ProductCategoryName" };
                PageBase.DropdownBinding(ref cmbSerProdCat, dt, colArray1);
                cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
    }

    public void finddata()
    {
        int k = 4;
        string[] str = new string[10];
        int i = 1;
        DataTable dt = new DataTable();
        DataColumn[] dc = new DataColumn[k];

        for (int c = 0; c < k; c++)
        {
            dc[c] = new DataColumn(c.ToString());
        }
        dt.Columns.AddRange(dc);
        while (Page.Request.Form[string.Format("TextBox{0}1", i)] != null)
        {
            DataRow dr = dt.NewRow();
            for (int c = 0; c < k; c++)
            {
                dr[c.ToString()] = Page.Request.Form[string.Format("TextBox{0}{1}", i, (c + 1))];
            }

            i++;
            dt.Rows.Add(dr);
        }
        DataTable dt1 = getfinaltable(dt);
        ViewState["Table1"] = dt1;

    }


    public DataTable getfinaltable(DataTable dt)
    {
        DataTable dtout = new DataTable();
        DataColumn[] dc = new DataColumn[4];
        dc[0] = new DataColumn("Quantity");
        dc[1] = new DataColumn("BatchNumber");
        dc[2] = new DataColumn("SerialNumber");
        dc[3] = new DataColumn("SKUCode");
        dtout.Columns.AddRange(dc);
        foreach (DataRow dr in dt.AsEnumerable())
        {
            DataRow dr1 = dtout.NewRow();
            dr1["Quantity"] = dr["0"].ToString();
            dr1["BatchNumber"] = dr["1"].ToString();
            dr1["SerialNumber"] = dr["2"].ToString();
            dr1["SKUCode"] = dr["3"].ToString();
            dtout.Rows.Add(dr1);
        }
        return dtout;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        blankall();
    }

    public void blankall()
    {
        txtDocketNo.Text = "";
        ddlmodeofReceipt.SelectedValue = "0";
        ddlUser.SelectedValue = "0";
        ddlUser.Enabled = false;
        txtCourierName.Text = "";
        ucDateForGRN.Date = "";
        //ucDateForGCN.Date = "";
        txtRemarks.Text = "";
        ddlWarehouse.SelectedIndex = 0;
        //GridGRN.DataSource = null;
        //GridGRN.DataBind();
        updGrid.Update();
        pnlGrid.Visible = false;
        ViewState["MaterialIssue"] = null;
        txtCourierName.Enabled = false;
        pnlSearch.Visible = false;
        pnlGrid.Visible = false;
        updGrid.Update();
        UpdSearch.Update();
        //pnlButton.Visible = false;
        updAddUserMain.Update();
        //updDiv.Update();
   }


   
    public void  methodsave()
    {

        finddata();
        DataTable dt1 = (DataTable)ViewState["Table1"];
        POC ObjCommom = new POC();
        DataTable tvp = ObjCommom.GettvpTableMaterialIssueBatchOrSerialWise();
        foreach (DataRow dr in dt1.Rows)
        {
            if (dr["Quantity"].ToString() != null && dr["Quantity"].ToString() != "0")
            {
                DataRow drow = tvp.NewRow();
                {
                    drow[0] = Convert.ToInt32(ddlWarehouse.SelectedValue);
                    drow[1] = txtDocketNo.Text.Trim();
                    drow[2] = Convert.ToDateTime(ucDateForGRN.Date);
                    drow[3] = Convert.ToInt32(ddlUser.SelectedValue);
                    drow[4] = txtRemarks.Text.Trim();
                    drow[5] = dr["Quantity"].ToString();
                    drow[6] = ddlmodeofReceipt.SelectedValue;
                    drow[7] = txtCourierName.Text.Trim();
                    drow[8] = dr["SKUCode"].ToString();
                    drow[9] = dr["SerialNumber"].ToString();
                    drow[10] = dr["BatchNumber"].ToString();
                    drow[11] = PageBase.UserId;
                    tvp.Rows.Add(drow);
                }
            }

        }

        tvp.AcceptChanges();
        using (POC objGrnUpload = new POC())
        {
            int intResult = objGrnUpload.InsertInfoMaterialIssueBatchWiseOrSerialWise(tvp);

            //if (objGrnUpload.ErrorDetailXML != null && objGrnUpload.ErrorDetailXML != string.Empty)
            //{
            //    ucMsg.XmlErrorSource = objGrnUpload.ErrorDetailXML;
            //    btnSave.Enabled = false;
            //    return ;
            //}
            //if (objGrnUpload.Error != null && objGrnUpload.Error != "")
            //{
            //    ucMsg.ShowError(objGrnUpload.Error);
            //    btnSave.Enabled = false;
            //    return ;
            //}
            //if (intResult == 2)
            //{
            //    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            //    btnSave.Enabled = false;
            //    return ;
            //}
            ClearControls();
            ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
            blankall();


            return ;

        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        methodsave();
        Response.Redirect("MaterialIssuedOutwardInterface.aspx?id=1");

    }
}
