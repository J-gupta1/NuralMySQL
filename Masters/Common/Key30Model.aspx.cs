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

public partial class Masters_Common_Key30Model : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            fillModel();
            BindStatus();
            databind();
        }
    }
    protected void btnSerchModel_Click(object sender, EventArgs e)
    {
        databind();
    }
    protected void grd30KeyModel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        using (MastersData objmaster = new MastersData())
        {
            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    ucMessage1.Visible = false;
                    objmaster.ModelId = Convert.ToInt32(e.CommandArgument);
                    ViewState["KeyModelId"] = objmaster.ModelId;
                    dt = objmaster.Select30KeyFillModelInfo();
                    ddlModel.SelectedValue = Convert.ToString(dt.Rows[0]["ModelID"]);
                    if (dt.Rows[0]["Status"].ToString() == "True")
                    {
                        chkstatus.Checked = true;
                    }
                    else
                    {
                        chkstatus.Checked = false;
                    }
                    btnSubmit.Text = "Update";
                    updAddUserMain.Update();
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
        }
    }
    protected void grd30KeyModel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd30KeyModel.PageIndex = e.NewPageIndex;
        databind();
    }
    public void fillModel()
    {
        using (MastersData objmaster = new MastersData())
        {
            try
            {
                DataTable dt;
                objmaster.State_Id = 0;
                objmaster.ComingFor = 0;
                dt = objmaster.Select30KeyModelInfo();
                ddlModel.DataSource = dt;
                ddlModel.DataValueField = "ModelID";
                ddlModel.DataTextField = "ModelName";
                ddlModel.DataBind();
                ddlModel.Items.Insert(0, new ListItem("Select", "0"));

                ddlSerModel.DataSource = dt;
                ddlSerModel.DataValueField = "ModelID";
                ddlSerModel.DataTextField = "ModelName";
                ddlSerModel.DataBind();
                ddlSerModel.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        }
    }
    protected void btninsert_click(object sender, EventArgs e)
    {
         if (IsPageRefereshed == true)
        {
            return;
        }
         using (MastersData objmaster = new MastersData())
         {
             updAddUserMain.Update();
             if (!insertvalidate())
             {
                 return;
             }
             else
             {
                 objmaster.ModelName = ddlModel.SelectedItem.Text;
                 objmaster.ModelId = Convert.ToInt32(ddlModel.SelectedValue);
                 objmaster.UniqueID = PageBase.UserId;
                 objmaster.KeyModelId = 0;
                 if (chkstatus.Checked == true)
                 {
                     objmaster.StateStatus = 1;
                 }
                 else
                 {
                     objmaster.StateStatus = 0;
                 }
                 if (ViewState["KeyModelId"] == null || (int)ViewState["KeyModelId"] == 0)
                 {
                     try
                     {
                         objmaster.error = "";
                         objmaster.InsertKey30ModelInfo();
                         if (objmaster.error == "")
                         {

                             databind();
                             ucMessage1.Visible = true;
                             ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                             blankinserttext();
                         }
                         else
                         {
                             ucMessage1.Visible = true;
                             ucMessage1.ShowInfo(objmaster.error);
                         }
                     }
                     catch (Exception ex)
                     {
                         ucMessage1.Visible = true;
                         ucMessage1.ShowInfo(ex.Message.ToString());
                         PageBase.Errorhandling(ex);
                     }
                 }

                 else
                 {
                     try
                     {

                         objmaster.error = "";
                         objmaster.KeyModelId = (int)ViewState["KeyModelId"];
                         objmaster.ModelId = Convert.ToInt32(ddlModel.SelectedValue);
                         objmaster.ComingFor = 1;
                         objmaster.InsertKey30ModelInfo();
                         if (objmaster.error == "")
                         {
                             databind();
                             ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                             blankinserttext();
                             ViewState["KeyModelId"] = null;
                         }
                         else
                         {

                             ucMessage1.ShowInfo(objmaster.error);
                         }
                     }
                     catch (Exception ex)
                     {
                         ucMessage1.ShowInfo(ex.Message.ToString());
                         PageBase.Errorhandling(ex);
                     }

                 }
             }
         }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        blankinserttext();
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        using (MastersData objmaster = new MastersData())
        {
            ucMessage1.Visible = false;
            objmaster.ModelId = Convert.ToInt32(ddlSerModel.SelectedValue);
            objmaster.StateStatus = Convert.ToInt32(ddlSearchStatus.SelectedValue);
            objmaster.ComingFor = 1;
            dt = objmaster.SelectKey30ModelGridInfo();
            if (dt.Rows.Count > 0)
            {
                try
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Key30ModelDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
        }
    }
    protected void btngetalldata_Click(object sender, EventArgs e)
    {
        blankinserttext();
        databind();
    }
    private void BindStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = GetEnumbyTableName("XML_Enum", "Key47CityStateStatus");
            if (dtresult.Rows.Count > 0)
            {
                ddlSearchStatus.DataSource = dtresult;
                ddlSearchStatus.DataTextField = "Description";
                ddlSearchStatus.DataValueField = "Value";
                ddlSearchStatus.DataBind();
                ddlSearchStatus.Items.Insert(0, new ListItem("Select", "255"));
            }
            else
            {
                ddlSearchStatus.Items.Insert(0, new ListItem("Select", "255"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    public static DataTable GetEnumbyTableName(string Filename, string TableName)
    {
        DataTable dt = new DataTable();
        using (DataSet ds = new DataSet())
        {
            string filename = HttpContext.Current.Server.MapPath("~/Assets/XML/" + Filename + ".xml");
            ds.ReadXml(filename);
            dt = ds.Tables[TableName];
            if (dt == null || dt.Rows.Count == 0)
                return null;
        }
        try
        {
            dt = dt.Select("Active=1").CopyToDataTable();
            return dt;
        }
        catch (Exception)
        {
            return null;
        }
    }
    public void databind()
    {
        DataTable dt = new DataTable();
        using (MastersData objmaster = new MastersData())
        {
            ucMessage1.Visible = false;
            objmaster.ModelId = Convert.ToInt32(ddlSerModel.SelectedValue);
            objmaster.StateStatus = Convert.ToInt32(ddlSearchStatus.SelectedValue);
            objmaster.ComingFor = 0;
            try
            {
                dt = objmaster.SelectKey30ModelGridInfo();
                grd30KeyModel.DataSource = dt;
                grd30KeyModel.DataBind();
                updgrid.Update();

            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    public bool insertvalidate()
    {
        if (ddlModel.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a Model Name.");
            return false;
        }
        return true;
    }
    public void blankinserttext()
    {
        ddlModel.SelectedValue = "0";
        ddlSerModel.SelectedValue = "0";
        ddlSearchStatus.SelectedValue = "255";
        chkstatus.Checked = true;
       
        btnSubmit.Text = "Submit";
        updAddUserMain.Update();

    }
}