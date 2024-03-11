using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;
using System.Globalization;
using System.Collections;
using ZedService;


public partial class Masters_Common_UploadMappingSchema : PageBase
{
    UploadFile UploadFile = new UploadFile();
    string strUploadedFileName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            filmasterscombo();
        }
    }

   public void filmasterscombo()
    {
        Dictionary<int, string> master = new Dictionary<int, string>();
        master.Add(0, "Select");
        master.Add(1, "State");
        master.Add(2, "District");
        master.Add(3, "City");
        master.Add(4, "Area");
        master.Add(5, "Product");
        master.Add(6, "ProductCategory");
        master.Add(7, "Brand");
        master.Add(8, "Model");
        master.Add(9, "SKU");
        cmbTableName.DataSource = new System.Windows.Forms.BindingSource(master, null);
        cmbTableName.DataTextField = "Value";
        cmbTableName.DataValueField = "Key";
        cmbTableName.DataBind();
        cmbTableName.SelectedValue = "0";
    }


    protected void btnUpload_Click(object sender, EventArgs e)
    {

        try
        {
            string RootPath;
            cmbTableName.Enabled = false;
            DataSet dsmap = null;
            Int16 UploadCheck = 0;
            RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                fillExcelCombo(ref dsmap, RootPath);
                fillsystemcombo();
                pnlMapppingdata.Visible = true;
                getmappingtable();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.ToString());

        }
    }

    public void fillsystemcombo()
    {
        using (UserData obj = new UserData())
        {
            obj.TableName = cmbTableName.SelectedItem.ToString();
            DataTable dt = obj.GetTablesNameForSchema();
            
            cmbSystemSequence.DataSource = dt;
            String[] colArray = { "TableColumnName","TableColumnName" };
            PageBase.DropdownBinding(ref cmbSystemSequence, dt, colArray);
            cmbSystemSequence.SelectedValue = "0";
            


        }

    }



    public void fillExcelCombo(ref DataSet ds, string path)
    {
        OpenXMLExcel objexcel = new OpenXMLExcel();
        ds = objexcel.ImportExcelFileV2(path + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
        ViewState["Table"] = ds.Tables[0];
        Dictionary<int, string> excelcolumns = new Dictionary<int, string>();
        excelcolumns.Add(0, "select");
        for (int i = 1; i <= ds.Tables[0].Columns.Count; i++)
        {
            excelcolumns.Add(i, ds.Tables[0].Columns[i - 1].ColumnName);

        }
        cmbExcelSheetSequence.DataSource = new System.Windows.Forms.BindingSource(excelcolumns, null);
        cmbExcelSheetSequence.DataTextField = "Value";
        cmbExcelSheetSequence.DataValueField = "Key";
        cmbExcelSheetSequence.DataBind();
        cmbExcelSheetSequence.SelectedValue = "0";

    }

    public void getmappingtable()
    {
        DataTable dtmapp = new DataTable();
        DataColumn dc1 = new DataColumn();
        dc1.ColumnName = "SystemColumn";
        DataColumn dc2 = new DataColumn();
        dc2.ColumnName = "ExcelSheetColumn";
        dtmapp.Columns.Add(dc1);
        dtmapp.Columns.Add(dc2);
        ViewState["GRDTable"] = dtmapp;
    }


    protected void BtnMap_Click(object o, EventArgs e)
    {

        DataRow dr1 = ((DataTable)ViewState["GRDTable"]).NewRow();
        dr1["SystemColumn"] = cmbSystemSequence.SelectedItem.ToString();
        dr1["ExcelSheetColumn"] = cmbExcelSheetSequence.SelectedItem.ToString();
        ((DataTable)ViewState["GRDTable"]).Rows.Add(dr1);
        grdMappedData.DataSource = (DataTable)ViewState["GRDTable"];
        grdMappedData.DataBind();
        pnlMappedGrid.Visible = true;
        updMappedGrd.Update();

    }


    public bool checkvalidations()
    {
        using (UserData obj = new UserData())
        {
            obj.TableName = cmbTableName.SelectedItem.ToString();
            DataTable dt = obj.GetTablesNameForSchema();
            dt.DefaultView.RowFilter = "Validate = 1";
            dt = dt.DefaultView.ToTable();
            string[] reqiredcol = (from dr in dt.AsEnumerable()
                                   select (string)dr["TableColumnName"]).ToArray<string>();

            string[] mapcol = (from dr in ((DataTable)ViewState["GRDTable"]).AsEnumerable()
                               select (string)dr["SystemColumn"]).ToArray<string>();

            for (int i = 0; i < reqiredcol.Length; i++)
            {
                int check = 0;
                for (int j = 0; j < mapcol.Length; j++)
                {
                    if ((string)reqiredcol[i] == (string)mapcol[j])
                    {
                        check = 1;
                    }
                }
                if (check == 0)
                {
                    ucMsg.ShowInfo(string.Format("The mandatory column {0} is not mapped", reqiredcol[i]));
                    return false;
                }


            }
            return true;


        }



    }

    public bool checkduplicacy(DataTable dt)
    {
        int check = 0;
        string[] code = (from dr in dt.AsEnumerable() select (string)dr[1]).ToArray<string>();
        foreach (string str in code)
        {
            foreach (string jtr in code)
            {
                if (str == jtr)
                {
                    check ++;

                }
            }
            if (check > 1)
            {
                ucMsg.ShowInfo("Cannot insert duplicate code in sheet");
                return false;

            }
            check = 0;
        }

        return true;
    }

    protected void BtnProceedmap_Click(object o, EventArgs e)
    {

        pnlMappedGrid.Visible = false;
        pnlMapppingdata.Visible = false;
        cmbExcelSheetSequence.Items.Clear();
        cmbSystemSequence.Items.Clear();
        pnlMapppingdata.Visible = false;
        byte isSuccess = 0;
        if (!checkvalidations())
        {
            return;
        }
        String RootPath = Server.MapPath("../../");
        UploadFile.RootFolerPath = RootPath;
        DataSet dst = new DataSet();
        using (UserData obj = new UserData())
        {
            DataTable finaltable = new DataTable();
            obj.TableName = cmbTableName.SelectedItem.ToString();
            DataTable dt = obj.GetTablesNameForSchema();
            foreach (DataRow dr in dt.AsEnumerable())
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = dr["TableColumnName"].ToString();
                finaltable.Columns.Add(dc);
            }

           

            foreach (DataRow dr1 in ((DataTable)ViewState["Table"]).AsEnumerable())
            {
                DataRow drs = finaltable.NewRow();
                foreach (GridViewRow grv in grdMappedData.Rows)
                {
                    Label systemname = (Label)grv.FindControl("lblSysName");
                    Label excelname = (Label)grv.FindControl("lblExcelName");
                    drs[systemname.Text] = dr1[excelname.Text];

                }
                finaltable.Rows.Add(drs);
            }

            grdFinal.DataSource = finaltable;
            ViewState["Table1"] = finaltable;
            grdFinal.DataBind();
            pnlFinalGrid.Visible = true;
            updFinalGrid.Update();
            ViewState["Table"] = null;

          
         }

        }


    


    protected void BtnFinalSubmit_Click(object o, EventArgs e)
    {
        if(!checkduplicacy((DataTable)ViewState["Table1"]))
        {
            return;
        }
        DataSet ds = new DataSet();
       using(MastersData obj = new MastersData())
        {
           ds.Merge((DataTable)ViewState["Table1"]);
           string xmldetail = "";
           xmldetail = ds.GetXml();
           obj.TableID = Convert.ToInt32(cmbTableName.SelectedValue);
           obj.UploadSchemaXml = xmldetail;
           obj.InsertUploadSchema();
           pnlFinalGrid.Visible = false;
           updFinalGrid.Update();
           if (obj.ErrorXmlDetail != null && obj.ErrorXmlDetail != string.Empty)
           {
               ucMsg.XmlErrorSource = obj.ErrorXmlDetail;
               return;
           }
          ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

        }


    }

    protected void BtnCancelMapping_Click(object o, EventArgs e)
    {
        blankmappingdata();
        pnlMapppingdata.Visible = false;
        pnlMappedGrid.Visible = false;

    }


    public void blankmappingdata()
    {

        cmbExcelSheetSequence.Items.Clear();
        cmbSystemSequence.Items.Clear();
        pnlMapppingdata.Visible = false;
    }


    protected void BtnFinalCancel_Click(object o, EventArgs e)
    {
        pnlFinalGrid.Visible = false;
        pnlMappedGrid.Visible = false;
        pnlMapppingdata.Visible = false;
        updFinalGrid.Update();
        updMappedGrd.Update();
        ucMsg.Visible = false;

    }

    protected void btnCancelAll_Click(object o, EventArgs e)
    {
        pnlFinalGrid.Visible = false;
        pnlMappedGrid.Visible = false;
        pnlMapppingdata.Visible = false;
        updFinalGrid.Update();
        updMappedGrd.Update();
        ucMsg.Visible = false;
        cmbTableName.SelectedValue = "0";
        cmbTableName.Enabled = true;
    }



}