using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Data;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Services;
using System.Collections;



public partial class Reports_Common_ViewUploadDocs : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DefaultBinding();
            BindRow();
            ScriptManager.RegisterStartupScript(this, typeof(string), "LoadScript", "BindGrid(1);", true);
        } Session["LoginID"] = PageBase.UserId;
    }
    private void BindRow()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DocumentType");
        dt.Columns.Add("ISPCode");
        dt.Columns.Add("ISPName");
        dt.Columns.Add("TransactionDate");
        dt.Columns.Add("Latitude");
        dt.Columns.Add("Longitude");
        dt.Columns.Add("Remarks");
        dt.Columns.Add("UploadPath");
        dt.Rows.Add();
        grdUpload.DataSource = dt;
        grdUpload.DataBind();
    }
    void DefaultBinding()
    {
        using (clsUploadDocs obj = new clsUploadDocs())
        {
            ddlUploadType.DataSource = obj.GetUploadType();
            ddlUploadType.DataTextField = "UploadDocReferenceType";
            ddlUploadType.DataValueField = "UploadDocReferenceTypeID";
            ddlUploadType.DataBind();
            if (ddlUploadType.Items.Count <= 0)
            {
                ddlUploadType.DataSource = null;
                ddlUploadType.DataBind();
            }
            ddlUploadType.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
}
