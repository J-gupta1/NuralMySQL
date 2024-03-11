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
using System.Collections;
using System.Windows.Forms;

public partial class Masters_Common_ManageUploadSchema : PageBase
{
    DataTable dtBlank = new DataTable();
    Dictionary<int, string> dicDataType = new Dictionary<int, string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["TableData"] = dtBlank;
            BlankDatatable((DataTable)ViewState["TableData"]);
            fillDataType();
            fillExcelSheetDataType();
            fillColumnConstraintDataType();
            
        }
    }


    public void BlankDatatable(DataTable dt)
    {
       
        try
        {
            dt.Columns.Add(new DataColumn("TableName", typeof(string)));
            dt.Columns.Add(new DataColumn("TableColumnName", typeof(string)));
            dt.Columns.Add(new DataColumn("TableDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("TableColumnDataType", typeof(string)));
            dt.Columns.Add(new DataColumn("ExcelSheetColumnName", typeof(string)));
            dt.Columns.Add(new DataColumn("ExcelSheetDataType", typeof(string)));
            dt.Columns.Add(new DataColumn("ColumnConstraint", typeof(string)));
            dt.Columns.Add(new DataColumn("Validate", typeof(System.Byte)));
            dt.Columns.Add(new DataColumn("MaxLength", typeof(Int32)));
            dt.Columns.Add(new DataColumn("ColumnSequence", typeof(Int32))); 
            List<DataColumn> cols = new List<DataColumn>();
            cols.Add(dt.Columns["TableName"]);
            cols.Add(dt.Columns["TableColumnName"]);
            dt.PrimaryKey = cols.ToArray();
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
       
        try
        {
            InsertData((DataTable)ViewState["TableData"]);
            grdUploadSchema.DataSource = (DataTable)ViewState["TableData"];
            grdUploadSchema.DataBind();
            pnlgrid.Visible = true;
            updGrid.Update();
            clearform();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
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
        dcLocal.Add(1, "primary");
        return dcLocal;
    }
    public void fillDataType()
    {
        dicDataType = fncFillDicForDatatype();
        if (dicDataType.Count > 0)
        {
            ddlTableColDataType.DataSource = new BindingSource(dicDataType, null);
            ddlTableColDataType.DataValueField = "Key";
            ddlTableColDataType.DataTextField = "Value";
            ddlTableColDataType.DataBind();
        }
        updGrid.Update();
    }
    public void fillExcelSheetDataType()
    {
        dicDataType = new Dictionary<int, string>();
        dicDataType = fncFillDicExcelSheetDatatype();
        if (dicDataType.Count > 0)
        {
            DDlExcelSheetDataType.DataSource = new BindingSource(dicDataType, null);
            DDlExcelSheetDataType.DataValueField = "Key";
            DDlExcelSheetDataType.DataTextField = "Value";
            DDlExcelSheetDataType.DataBind();
        }
        updGrid.Update();
    }

    public void fillColumnConstraintDataType()
    {
        dicDataType = new Dictionary<int, string>();
        dicDataType = fncFillDicColumnConstraint();
        if (dicDataType.Count > 0)
        {

            ddlColumnContraint.DataSource = new BindingSource(dicDataType, null);
            ddlColumnContraint.DataValueField = "Key";
            ddlColumnContraint.DataTextField = "Value";
            ddlColumnContraint.DataBind();
        }
        updGrid.Update();
    }
    public void InsertData(DataTable dt)
    {
        try
        {
            DataRow drow = dt.NewRow();
            if (dt.Columns.Count > 0)
            {
                if (txtTableName.Text != "")
                {
                    drow["TableName"] = txtTableName.Text.Trim();
                }
                else
                {
                    drow["TableName"] = cmbTableName.SelectedItem.ToString();

                }
                drow["TableColumnName"] = txtColumnName.Text.Trim();
                if (txtTableDescription.Text != "")
                {
                    drow["TableDescription"] = txtTableDescription.Text.Trim();
                }
                else
                {
                    drow["TableDescription"] = ((DataTable)ViewState["TableData"]).Rows[0]["TableDescription"].ToString();
                }
                drow["TableColumnDataType"] = ddlTableColDataType.SelectedItem.Text.Trim();
                drow["ExcelSheetColumnName"] = txtExcelSheetColumnName.Text.Trim();
                drow["ExcelSheetDataType"] = DDlExcelSheetDataType.SelectedItem.Text.Trim();
                if (ddlColumnContraint.SelectedItem.Text.Trim() == "Select")
                {
                    drow["ColumnConstraint"] = "N/A";
                }
                drow["ColumnConstraint"] = ddlColumnContraint.SelectedItem.Text.Trim();
                drow["Validate"] = chkValidate.Checked;

                drow["MaxLength"] = txtMaxLength.Text.Trim();
                if (ViewState["Count"] == null)
                {
                    drow["ColumnSequence"] = 0;
                }
                else
                {
                    drow["ColumnSequence"] = (int)ViewState["Count"] + 1 ;
                   ViewState["Count"] = (int)ViewState["Count"] + 1;
                }
                dt.Rows.Add(drow);
            }
            ViewState["Count"] = null;
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError("Trying to insert duplicate TableName");

        }

    }

    private void clearform()
    {
        txtColumnName.Text = "";
        ddlTableColDataType.SelectedIndex = 0;
        txtExcelSheetColumnName.Text = "";
        DDlExcelSheetDataType.SelectedIndex = 0;
        ddlColumnContraint.SelectedIndex = 0;
        chkValidate.Checked = false;
        txtMaxLength.Text = "";


    }
   
    protected void btnSave_Click(object sender, EventArgs e)
    {
        pnlAddNewColumn.Visible = false;
        DataSet Ds = new DataSet();
        try
        {
            if (ViewState["TableData"] != null)
            {
                using (UserData objUploadSchema = new UserData())
                {
                    string XmlDetail = "";
                    Ds.Merge((DataTable)ViewState["TableData"]);
                    XmlDetail = Ds.GetXml();
                    objUploadSchema.UploadSchemaXml = XmlDetail;
                    int result = objUploadSchema.InsertUpdateUploadSchema();
                    if (objUploadSchema.UploadSchemaXml != null && objUploadSchema.UploadSchemaXml != string.Empty)
                    {
                        ucMessage1.XmlErrorSource = objUploadSchema.UploadSchemaXml;
                        return;
                    }
                    if (objUploadSchema.Error != null && objUploadSchema.Error != "")
                    {
                        ucMessage1.ShowError(objUploadSchema.Error);
                        return;
                    }
                    ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    pnlUpdate.Visible = false;
                    blankupdate();
                    pnlgrid.Visible = false;
                    updGrid.Update();
                    txtTableName.Text = "";
                    txtTableDescription.Text = "";
                    txtTableName.Enabled = true;
                    txtTableDescription.Enabled = true;

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void btnProceed_Click(object sender, EventArgs e)
    {
        pnlUpdate.Visible = true;
        updGrid.Update();
        txtTableName.Enabled = false;
        txtTableDescription.Enabled = false;
    }
    protected void btnunProced_click(object sender, EventArgs e)
    {
        txtTableName.Text = "";
        ((DataTable)ViewState["TableData"]).Rows.Clear();
        txtTableDescription.Text = "";
        pnlUpdate.Visible = false;
        pnlgrid.Visible = false;
        updGrid.Update();
        ucMessage1.Visible = false;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        blankupdate();
        ucMessage1.Visible = false;
    }
    public void blankupdate()
    {
        txtMaxLength.Text = "";
        txtExcelSheetColumnName.Text = "";
        txtColumnName.Text = "";
        ddlColumnContraint.SelectedValue = "0";
        DDlExcelSheetDataType.SelectedValue = "0";
        ddlTableColDataType.SelectedValue = "0";
        
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        pnlgrid.Visible = false;
        updGrid.Update();
        ((DataTable)ViewState["TableData"]).Rows.Clear();
        blankupdate();
        ucMessage1.Visible = false;
        txtTableName.Text = "";
        txtTableDescription.Text = "";
        txtTableName.Enabled = true;
        txtTableDescription.Enabled = true;

    }

    protected void LBViewSchema_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewUploadSchemaDetail.aspx");
    }
    protected void btnCreateColumn_click(object sender, EventArgs e)
    {
        pnlAddNewColumn.Visible = true;
        using (UserData obj = new UserData())
        {
            DataTable dt = obj.GetTablesNameForSchema();
            String[] colArray = { "Sequence", "TableName" };
            PageBase.DropdownBinding(ref cmbTableName, dt, colArray);

        }
    }



    public void fillgrid()
    {
        using (UserData obj = new UserData())
        {
            obj.TableName = cmbTableName.SelectedItem.ToString();
            DataTable dt = obj.GetTablesNameForSchema();
            ViewState["Count"] = dt.Rows.Count -1 ;
            dt.Columns.Remove("UploadTableID");
            dt.Columns.Remove("Status");
            ViewState["TableData"] = dt;
            ((DataTable)ViewState["TableData"]).TableName = "Table1";
            grdUploadSchema.DataSource = dt;
            grdUploadSchema.DataBind();
            pnlgrid.Visible = true;
            updGrid.Update();

        }
    }

    protected void btnCreateColumn_Click(object o, EventArgs e)
    {
        pnlUpdate.Visible = true;
        fillgrid();
        pnlAddNewColumn.Visible = false;
       
       
    }

    protected void btncancelCreateColumn_click(object o, EventArgs e)
    {
        ((DataTable)ViewState["TableData"]).Rows.Clear(); ;
        pnlAddNewColumn.Visible = false;
        pnlgrid.Visible = false;
        updGrid.Update();
        pnlUpdate.Visible = false;

    }
}
