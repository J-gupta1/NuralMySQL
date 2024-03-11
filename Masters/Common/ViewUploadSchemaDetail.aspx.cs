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

public partial class Masters_Common_ViewUploadSchemaDetail : PageBase
{
    Dictionary<string, string> names = new Dictionary<string, string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            filltablecombo();
        }

    }

    public void filltablecombo()
    {
        using (UserData obj = new UserData())
        {
            DataTable dt = obj.GetTablesNameForSchema();
            String[] colArray = { "Sequence", "TableName" };
            PageBase.DropdownBinding(ref cmbTableName, dt, colArray);

        }

    }

    public Dictionary<int, string> fncolumnSequence(int i)
    {
        Dictionary<int, string> dccolSequence = new Dictionary<int, string>();
        for (int j = 0; j <= i - 1; j++)
        {
            dccolSequence.Add(j, j.ToString());
        }
        return dccolSequence;
    }


    private Dictionary<int, string> fncFillDicForDatatype()
    {
        Dictionary<int, string> dcLocal = new Dictionary<int, string>();
        dcLocal.Add(0, "Select");
        dcLocal.Add(1, "date");
        dcLocal.Add(2, "decimal");
        dcLocal.Add(3, "int");
        dcLocal.Add(4, "System.DateTime");
        dcLocal.Add(5, "varchar");
        return dcLocal;
    }

    private Dictionary<int, string> fncFillDicExcelSheetDatatype()
    {
        Dictionary<int, string> dicExcelSheetDataType = new Dictionary<int, string>();
        dicExcelSheetDataType.Add(0, "Select");
        dicExcelSheetDataType.Add(1, "System.DateTime");
        dicExcelSheetDataType.Add(2, "System.Decimal");
        dicExcelSheetDataType.Add(3, "System.Int32");
        dicExcelSheetDataType.Add(4, "System.Int64");
        dicExcelSheetDataType.Add(5, "System.String");
        return dicExcelSheetDataType;
    }


    private Dictionary<int, string> fncFillDicColumnConstraint()
    {
        Dictionary<int, string> dcLocal = new Dictionary<int, string>();
        dcLocal.Add(0, "Select");
        dcLocal.Add(1, "Primary");
        dcLocal.Add(2, "N");
       
        return dcLocal;
    }

    public void fillgrid()
    {
        using (UserData obj = new UserData())
        {
            obj.TableName = cmbTableName.SelectedItem.ToString();
            DataTable dt = obj.GetTablesNameForSchema();
            int i = dt.Rows.Count;
            ViewState["Counts"] = i  ;
            grdUploadSchema.DataSource = dt;
            grdUploadSchema.DataBind();
            pnlGrid.Visible = true;
            updGrid.Update();

        }

    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
       
        fillgrid();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlGrid.Visible = false;
        updGrid.Update();
        ucMessage1.Visible = false;
    }
    protected void grdUploadSchema_RowCommand(object sender, GridViewCommandEventArgs e)
    {
       try
        {
              if (e.CommandName == "Active")
              {
                int id = Convert.ToInt32(e.CommandArgument);
                Int16 status;
                using (UserData obj = new UserData())
                {
                    obj.UploadTableID = id;
                    DataTable dt = obj.GetTablesNameForSchema();
                    status = Convert.ToInt16(dt.Rows[0]["Status"]);
                    if (status == 1)
                    {
                        status = 0;
                    }
                    else
                    {
                        status = 1;
                    }
                    obj.Status = Convert.ToBoolean(status);
                    obj.UpdSchemaStatus();
                    if (obj.error != "")
                    {
                        ucMessage1.ShowInfo("Status cant be changed");
                        return;
                    }
                    fillgrid();
                    updGrid.Update();
                    ucMessage1.ShowInfo(Resources.Messages.StatusChanged);
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void grdUploadSchema_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdUploadSchema.EditIndex = -1;
        fillgrid();

    }
    protected void grdUploadSchema_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            grdUploadSchema.EditIndex = e.NewEditIndex;
            fillgrid();
        }

        catch (Exception ex)
        {
            throw;
        }


    }
    protected void grdUploadSchema_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow Row = (GridViewRow)grdUploadSchema.Rows[e.RowIndex];
        DropDownList cmbtblColumnType = (DropDownList)Row.FindControl("cmbTblColumnDataType");
        DropDownList cmbtblExcelDataType = (DropDownList)Row.FindControl("cmbExcelDataType");
        DropDownList cmbConatraints = (DropDownList)Row.FindControl("cmbColumnConstraints");
        DropDownList cmbSequence = (DropDownList)Row.FindControl("cmbColumnSequence");
        TextBox txtColumnname = (TextBox)Row.FindControl("txtTblColumnName");
        TextBox txtExcelColumnName = (TextBox)Row.FindControl("txtExcelColumnName");
        TextBox txtMaxLength = (TextBox)Row.FindControl("txtMaxLength");
        //int status = Convert.ToInt16(DataBinder.Eval(Row.DataItem, "Status"));
        Label lblTableID = (Label)Row.FindControl("lblTableID");

        using (UserData obj = new UserData())
        {
            obj.TblColumnName = txtColumnname.Text;
            obj.TblExcelColumnName = txtExcelColumnName.Text;
            obj.TblColumndataType = cmbtblColumnType.SelectedItem.ToString();
            obj.TblExcelColumndataType = cmbtblExcelDataType.SelectedItem.ToString();
            obj.TblColumnConstraints = cmbConatraints.SelectedItem.ToString();
            obj.CurrentIndex = Convert.ToInt16(cmbSequence.SelectedItem.ToString());
            obj.PreviousIndex = Convert.ToInt16(ViewState["ColumnSequence"]);
            obj.MaxLength = Convert.ToInt16(txtMaxLength.Text);
            obj.UploadTableID = Convert.ToInt32(lblTableID.Text);
            obj.UpdTahbleSchema();
            if (obj.error != "")
            {
                ucMessage1.ShowInfo("Records cant be updated");
                return;
            }
            //ucMessage1.ShowInfo(Resources.Messages.EditSuccessfull);          Pankaj Dhingra 09112011
            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
            ViewState["Counts"] = null;
            grdUploadSchema.EditIndex = -1;
            fillgrid();

        }




    }
    protected void grdUploadSchema_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if ((e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate)) || (e.Row.RowState == DataControlRowState.Edit))
            {
                GridViewRow Row = e.Row;
                Dictionary<int, string> dctblColumnType = fncFillDicForDatatype();
                Dictionary<int, string> dcExcelColumnType = fncFillDicExcelSheetDatatype();
                Dictionary<int, string> dcConstraints = fncFillDicColumnConstraint();
                Dictionary<int, string> dcColumnSequence = fncolumnSequence((int)ViewState["Counts"]);
                string strColumnType = Convert.ToString(DataBinder.Eval(Row.DataItem, "TableColumnDataType"));
                string strExcelColumnType = Convert.ToString(DataBinder.Eval(Row.DataItem, "ExcelSheetDataType"));
                string strColumnConstrains = Convert.ToString(DataBinder.Eval(Row.DataItem, "ColumnConstraint"));
                if (strColumnConstrains == "")
                {
                    strColumnConstrains = "N";
                }
                strColumnConstrains = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strColumnConstrains);
                string strColumnSequence = Convert.ToString(DataBinder.Eval(Row.DataItem, "ColumnSequence"));
                ViewState["ColumnSequence"] = strColumnSequence; 
                DropDownList cmbtblColumnType = (DropDownList)Row.FindControl("cmbTblColumnDataType");
                cmbtblColumnType.DataSource = new System.Windows.Forms.BindingSource(dctblColumnType, null);
                cmbtblColumnType.DataValueField = "Key";
                cmbtblColumnType.DataTextField = "Value";
                cmbtblColumnType.DataBind();
                cmbtblColumnType.Items.FindByText(strColumnType).Selected = true;
                DropDownList cmbtblExcelDataType = (DropDownList)Row.FindControl("cmbExcelDataType");
                cmbtblExcelDataType.DataSource = new System.Windows.Forms.BindingSource(dcExcelColumnType, null);
                cmbtblExcelDataType.DataValueField = "Key";
                cmbtblExcelDataType.DataTextField = "Value";
                cmbtblExcelDataType.DataBind();
                cmbtblExcelDataType.Items.FindByText(strExcelColumnType).Selected = true;
                DropDownList cmbConatraints = (DropDownList)Row.FindControl("cmbColumnConstraints");
                cmbConatraints.DataSource = new System.Windows.Forms.BindingSource(dcConstraints, null);
                cmbConatraints.DataValueField = "Key";
                cmbConatraints.DataTextField = "Value";
                cmbConatraints.DataBind();
                cmbConatraints.Items.FindByText(strColumnConstrains).Selected = true;
                DropDownList cmbSequence = (DropDownList)Row.FindControl("cmbColumnSequence");
                cmbSequence.DataSource = new System.Windows.Forms.BindingSource(dcColumnSequence, null);
                cmbSequence.DataValueField = "Key";
                cmbSequence.DataTextField = "Value";
                cmbSequence.DataBind();
                cmbSequence.Items.FindByText(strColumnSequence).Selected = true;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void cmbTableName_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlGrid.Visible = false;
        updGrid.Update();
        ucMessage1.Visible = false;
        ViewState["Counts"] = null;
    }
    protected void LBViewSchema_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageUploadSchema.aspx");
    }
}
