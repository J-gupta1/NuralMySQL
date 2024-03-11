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

public partial class Masters_Common_Key47CityState : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
            ddlSerCity.Items.Insert(0, new ListItem("Select", "0"));
            fillState();
            fillSearchCity();
            BindStatus();
            databind();
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillCity();
    }
    
   
    public void fillState()
    {   
        using (MastersData objmaster = new MastersData()) 
        {
            try
            {
                DataTable dt;
                objmaster.State_Id = 0;
                objmaster.ComingFor = 0;
                dt = objmaster.Select47KeyStateInfo();
                ddlState.DataSource = dt;
                ddlState.DataValueField = "StateID";
                ddlState.DataTextField = "StateName";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("Select", "0"));

                ddlSerState.DataSource = dt;
                ddlSerState.DataValueField = "StateID";
                ddlSerState.DataTextField = "StateName";
                ddlSerState.DataBind();
                ddlSerState.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        }
    }
    public void fillCity()
    
    {
        using (MastersData objmaster = new MastersData())
        {
            ddlCity.Items.Clear();
            try
            {
                DataTable dt;
                objmaster.State_Id = Convert.ToInt32(ddlState.SelectedValue);
                objmaster.ComingFor = 1;
                dt = objmaster.Select47KeyStateInfo();
                ddlCity.DataSource = dt;
                ddlCity.DataValueField = "CityID";
                ddlCity.DataTextField = "CityName";
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        }
    }
    public void fillSearchCity()
    {
        using (MastersData objmaster = new MastersData())
        {
            ddlSerCity.Items.Clear();
            try
            {
                DataTable dt; 
                objmaster.State_Id = Convert.ToInt32(ddlSerState.SelectedValue);
                objmaster.ComingFor = 1;
                dt = objmaster.Select47KeyStateInfo();
                ddlSerCity.DataSource = dt;
                ddlSerCity.DataValueField = "CityID";
                ddlSerCity.DataTextField = "CityName";
                ddlSerCity.DataBind();
                ddlSerCity.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        }
    }
    protected void ddlSerState_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillSearchCity();
    }
    protected void btngetalldata_Click(object sender, EventArgs e)
    {
        databind();
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
                objmaster.CityName = ddlCity.SelectedItem.Text;
                objmaster.CityId =Convert.ToInt32(ddlCity.SelectedValue);
                objmaster.StateId = Convert.ToInt16(ddlState.SelectedValue);
                objmaster.StateName = ddlState.SelectedItem.Text;
                objmaster.UniqueID = PageBase.UserId;
                if (chkstatus.Checked == true)
                {
                    objmaster.StateStatus = 1;
                }
                else
                {
                    objmaster.StateStatus = 0;
                }
                if (ViewState["CityID"] == null || (int)ViewState["CityID"] == 0)
                {
                    try
                    {
                        objmaster.error = "";
                        objmaster.InsertKey47CityStateInfo();
                        if (objmaster.error == "")
                        {

                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blankinserttext();
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

                else
                {
                    try
                    {

                        objmaster.error = "";
                        objmaster.CityId = (int)ViewState["CityID"];
                        objmaster.ComingFor = 1;
                        objmaster.InsertKey47CityStateInfo();
                        if (objmaster.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            blankinserttext();
                            ViewState["CityID"] = null;


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
        blankinserttext();
    }
    protected void btnSerchStateCity_Click(object sender, EventArgs e)
    {
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
    protected void grdStateCity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        using (MastersData objmaster = new MastersData())
        {
            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    ucMessage1.Visible = false;
                    objmaster.CityId = Convert.ToInt32(e.CommandArgument);
                    ViewState["CityID"] = objmaster.CityId;
                    dt = objmaster.Select47KeyFillCityStateInfo();
                    ddlState.SelectedValue = Convert.ToString(dt.Rows[0]["StateId"]);
                    ddlCity.ClearSelection();
                    ddlState_SelectedIndexChanged(ddlState, new EventArgs());
                    ddlCity.SelectedValue = Convert.ToString(dt.Rows[0]["CityId"]);
                    if (dt.Rows[0]["Status"].ToString()=="1")
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
    protected void grdStateCity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdStateCity.PageIndex = e.NewPageIndex;
        databind();
    }
    public void databind()
    {
        DataTable dt = new DataTable();
        using (MastersData objmaster = new MastersData()) 
        {
            ucMessage1.Visible = false;
            objmaster.State_Id = Convert.ToInt32(ddlSerState.SelectedValue);
            objmaster.CityId = Convert.ToInt32(ddlSerCity.SelectedValue);
            objmaster.StateStatus = Convert.ToInt32(ddlSearchStatus.SelectedValue);
            objmaster.ComingFor = 0;
            try
            {
                dt = objmaster.SelectKey47CityStateInfo();
                grdStateCity.DataSource = dt;
                grdStateCity.DataBind();
                updgrid.Update();

            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        using (MastersData objmaster = new MastersData())
        {
            ucMessage1.Visible = false;
            objmaster.State_Id = Convert.ToInt32(ddlSerState.SelectedValue);
            objmaster.CityId = Convert.ToInt32(ddlSerCity.SelectedValue);
            objmaster.StateStatus = Convert.ToInt32(ddlSearchStatus.SelectedValue);
            objmaster.ComingFor = 1;
            dt = objmaster.SelectKey47CityStateInfo();
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        DataSet dtcopy = new DataSet();
                        dtcopy.Merge(dt);
                        dtcopy.Tables[0].AcceptChanges();
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "Key47CityDetails";
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
    public bool insertvalidate()
    {
        if (ddlCity.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a City");
            return false;
        }
        if (ddlState.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a State");
            return false;
        }
        return true;
    }
    public void blankinserttext()
    {
        ddlState.SelectedValue = "0";
        if(ddlState.SelectedValue=="0")
        {
            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
        }
        chkstatus.Checked = true;
        btnSubmit.Text = "Submit";
        updAddUserMain.Update();

    }
}