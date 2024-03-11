using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;

public partial class Transactions_DSR_DSR_Upload_interface : System.Web.UI.Page
{
    string strUploadedFileName = string.Empty;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindHierarchy();
            BindParentHierarchy();
            Bind_ddlMonth();
            
        }
    }
    private void Bind_ddlMonth()
    {
        try
        {
            ddl_month.Items.Clear();
            string[] monthOfYear = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;
            int i = 1;
            foreach (string monthName in monthOfYear)
            {
                if (monthName == "")
                    break;
                ddl_month.Items.Insert(0, new ListItem(monthName, i.ToString()));
                i++;

            }
            ddl_month.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlHierarchyLevel.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                //dt = objuser.GetAllHierarchyLevel();
                dt = objuser.GetHierarchyLevelConditional(2);
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlHierarchyLevel, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindParentHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlParentHierarchy.Items.Clear();
            using (OrgHierarchyData objOrg = new OrgHierarchyData())
            {
                objOrg.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);
                objOrg.UserID = PageBase.UserId;
                dt = objOrg.GetSelectedHierachyID_forDSR();
            };
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref ddlParentHierarchy, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ddlHierarchyLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindParentHierarchy();
    }
    protected void btn_upload_Click(object sender, EventArgs e)
    {
        try
        {

            string dsrpath = PageBase.strExcelPhysicalUploadPathSB.Replace("UploadExcelFiles", "DSR_Upload");
            UploadFile.DSR_Path = dsrpath;//Server.MapPath(dsrpath);
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (Upload == 1)
            {
                using (OrgHierarchyData objOrg_Prop = new OrgHierarchyData())
                {
                    objOrg_Prop.OrgnhierachyID_DSR = Convert.ToInt16(ddlParentHierarchy.SelectedValue);
                    objOrg_Prop.TxtMonth = ddl_month.SelectedValue.ToString();
                    objOrg_Prop.TxtYear = Txtbox_Year.Text;
                    objOrg_Prop.TxtPathLoc = dsrpath;//Server.MapPath(dsrpath);
                    objOrg_Prop.TxtFileName = strUploadedFileName;
                    objOrg_Prop.CreatedBy = PageBase.UserId;
                    objOrg_Prop.InsertdataToUploadDSR();
                    ucMsg.ShowSuccess(Resources.Messages.InsertSuccessfull);
                    Txtbox_Year.Text = "";
                    BindHierarchy();
                    BindParentHierarchy();
                    Bind_ddlMonth();
                }

            }
            else
            {
                ucMsg.ShowWarning(Resources.Messages.DSRUpload_Insert_ErrorMSG);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
}
