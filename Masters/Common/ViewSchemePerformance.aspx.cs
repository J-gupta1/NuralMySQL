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
/*14-Nov-2017,#CC01,Vijay Kumar Prajapati ,Change code for user login like distributor,micro distributor,etc.*/

public partial class Masters_Common_ViewSchemePerformance : PageBase
{
    DataSet DsScheme;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                dvhide.Visible = false;
                ddlSalesChannel.Items.Insert(0, new ListItem("Select", "0"));
                BindScheme();
                FillSalesChannelType();
                if(PageBase.SalesChanelID>0)
                {
                    btnCalculateSchemePerformance.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        if (Validation())
            FillGrid(0);

    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        ucDatePicker1.Date = "";
        ucDatePicker2.Date = "";
        ddlScheme.SelectedValue = "0";
        ddlSalesChannel.SelectedValue = "0";
        ddlSaleschannelType.SelectedValue = "0";
        ddlSchemeStatus.SelectedIndex = 0;
       
        dvhide.Visible = false;
        ddlSalesChannel.DataSource = null;
        ddlSalesChannel.DataBind();
        if (PageBase.SalesChanelID != 0)
        {
            ddlSalesChannel.Items.Clear();
            ddlSaleschannelType.Items.FindByValue(PageBase.SalesChanelTypeID.ToString()).Selected = true;
            ddlSaleschannelType_SelectedIndexChanged(ddlSaleschannelType, new EventArgs());
        }
        else
        {
            ddlSalesChannel.Items.Clear();
            ddlSalesChannel.Items.Insert(0, new ListItem("Select", "0"));
            ddlSalesChannel.Enabled = false;
        }
        ddlSaleschannelType.Items.Insert(0, new ListItem("Retailer", "101"));

    }
    void FillGrid(Int16 value)
    {

        try
        {
            DsScheme = new DataSet();
           
            using (SchemeData ObjScheme = new SchemeData())
            {
                if (ddlSalesChannel.SelectedValue=="0")
                {
                    ObjScheme.salesChannelID = PageBase.SalesChanelID;
                }
                else
                {
                    ObjScheme.salesChannelID = Convert.ToInt16(ddlSalesChannel.SelectedValue);
                }         
                ObjScheme.SchemeID = Convert.ToInt16(ddlScheme.SelectedValue);
                ObjScheme.SchemeStartDate = null;
                ObjScheme.SchemeStartDate = ucDatePicker1.Date != "" ? Convert.ToDateTime(ucDatePicker1.Date) : ObjScheme.SchemeStartDate;
                ObjScheme.SchemeEndDate = ucDatePicker2.Date != "" ? Convert.ToDateTime(ucDatePicker2.Date) : ObjScheme.SchemeEndDate;
                ObjScheme.Status = Convert.ToInt16(ddlSchemeStatus.SelectedValue);
                ObjScheme.SalesChannelTypeID = Convert.ToInt32(ddlSaleschannelType.SelectedValue);
                ObjScheme.UserId = PageBase.UserId;
               // ObjScheme.Counter = 3;/*#CC01 Commented*/
                ObjScheme.Counter = 4;/*#CC01 Added*/
                DsScheme = ObjScheme.GetSchemeInfoDetails();
            };
            if (value == 0)
            {
                if (DsScheme != null && DsScheme.Tables[0].Rows.Count > 0)
                {
                    GridScheme.DataSource = DsScheme;
                    GridScheme.DataBind();
                    dvhide.Visible = true;
                }
                else
                {
                    GridScheme.DataSource = null;
                    GridScheme.DataBind();
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    dvhide.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindScheme()
    {
        try
        {
            ddlScheme.Items.Clear();
            DsScheme = new DataSet();
            using (SchemeData ObjScheme = new SchemeData())
            {
                ObjScheme.Counter = 0;
                DsScheme = ObjScheme.GetSchemeInfoDetails();
            };
            string[] str = { "SchemeID", "SchemeName" };
            PageBase.DropdownBinding(ref ddlScheme, DsScheme.Tables[0], str);


        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
        }

    }
    private void BindSalesChannel()
    {
        try
        {
            ddlSalesChannel.Items.Clear();
            DsScheme = new DataSet();
            using (SchemeData ObjScheme = new SchemeData())
            {
                ObjScheme.Counter = 1;
                DsScheme = ObjScheme.GetSchemeInfoDetails();
            };
            if (DsScheme.Tables[0].Rows.Count > 0)
            {
                string[] str = { "SalesChannelID", "SalesChannelName" };
                PageBase.DropdownBinding(ref ddlSalesChannel, DsScheme.Tables[0], str);
            }
            else
            {
                ddlSalesChannel.Items.Insert(0, new ListItem("Select", "0"));
            }



        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
        }

    }
   
   
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            FillGrid(1);
            DsScheme.Tables[0].Columns.Remove("SchemePerformanceCalculationID");
            if (DsScheme.Tables[0].Rows.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SchemePerformance";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(DsScheme, FilenameToexport);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);
            }

        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());

        }

    }
    protected void GridScheme_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridScheme.PageIndex = e.NewPageIndex;
        FillGrid(0);
    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            if (Convert.ToInt32(PageBase.SalesChanelTypeID) != 0)
            {

                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                if (PageBase.AllowAllHierarchy != true)
                    ObjSalesChannel.HierarchyLevelID = Convert.ToInt16(PageBase.HierarchyLevelID);
                else
                    ObjSalesChannel.HierarchyLevelID = 0;
            }


            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlScheme, ObjSalesChannel.GetSalesChannelType(), str);
        };
    }

   
    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlSaleschannelType.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                dt = objuser.GetAllHierarchyLevel();
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlSaleschannelType, dt, colArray);
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlSaleschannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlSalesChannel.Items.Clear();
            if (PageBase.SalesChanelID == 0)
            {
                if (ddlSaleschannelType.SelectedIndex != 0)
                {
                    using (SchemeData ObjScheme = new SchemeData())
                    {
                        ObjScheme.UserId = UserId;
                        ObjScheme.Counter = 2;
                        ObjScheme.SalesChannelTypeID = Convert.ToInt32(ddlSaleschannelType.SelectedValue);
                        DsScheme = ObjScheme.GetSchemeInfoDetails();
                    };
                    String[] colArray = { "SaleschannelID", "SalesChannelName" };
                    PageBase.DropdownBinding(ref ddlSalesChannel, DsScheme.Tables[0], colArray);
                    ddlSalesChannel.Enabled = true;
                }
                else
                {
                    ddlSalesChannel.Items.Insert(0, new ListItem("Select", "0"));
                    ddlSalesChannel.Enabled = false;
                }
            }
            else
            {
                if (ddlSaleschannelType.SelectedIndex != 0)
                {
                    using (SchemeData ObjScheme = new SchemeData())
                    {
                        ObjScheme.UserId = UserId;
                        ObjScheme.Counter = 3;
                        ObjScheme.salesChannelID = PageBase.SalesChanelID;
                        ObjScheme.SalesChannelTypeID = Convert.ToInt32(ddlSaleschannelType.SelectedValue);
                        DsScheme = ObjScheme.GetSchemeInfoDetails();
                    };
                    String[] colArray = { "SaleschannelID", "SalesChannelName" };
                    PageBase.DropdownBinding(ref ddlSalesChannel, DsScheme.Tables[0], colArray);
                    ddlSalesChannel.Enabled = true;
                }
                else
                {
                    ddlSalesChannel.Items.Insert(0, new ListItem("Select", "0"));
                    ddlSalesChannel.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void FillSalesChannelType()
    {
        using (SchemeData objScheme = new SchemeData())
        {
            DataTable dt = new DataTable();
            String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
            objScheme.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            objScheme.UserId = UserId;
            objScheme.Counter = 1;
            DsScheme = objScheme.GetSchemeInfoDetails();
            PageBase.DropdownBinding(ref ddlSaleschannelType, DsScheme.Tables[0], StrCol);
        };
        if (PageBase.SalesChanelID != 0)
        {
            ddlSaleschannelType.Enabled = false;
            ddlSaleschannelType.Items.FindByValue(PageBase.SalesChanelTypeID.ToString()).Selected = true;
            ddlSaleschannelType_SelectedIndexChanged(ddlSaleschannelType, new EventArgs());
           
        }
        if(PageBase.SalesChanelID==0)
        {
            ddlSaleschannelType.Items.Add(new ListItem("Retailer", "101"));
        }   
       // ddlSaleschannelType.Items.Insert(0, new ListItem("Retailer", "101"));
        ddlSaleschannelType.SelectedValue = "0";
        if (PageBase.BaseEntityTypeID == 3)
        {
            ddlSaleschannelType.SelectedValue = "101";
            ddlSaleschannelType.Enabled = false;
            ddlSaleschannelType_SelectedIndexChanged(ddlSaleschannelType, null);
        }
        else
        {
            ddlSaleschannelType.SelectedValue = "0";
            ddlSaleschannelType.Enabled = true;
        }

        
    }
    bool Validation()
    {
        //if (ddlScheme.SelectedValue == "0" && ddlSalesChannel.SelectedValue == "0" && ucDatePicker1.Date == "" && ucDatePicker2.Date == "")
        //{
        //    ucMessage1.ShowInfo("Please fill any criteria for searching");
        //    return false;
        //}
        //Due to POC

        if (ddlSaleschannelType.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please Select any SalesChannelType");
            return false;
        }
        //if (ddlSaleschannelType.SelectedValue != "0")
        //{
        //    if (ddlSalesChannel.SelectedValue == "0")
        //    {
        //        ucMessage1.ShowInfo("Please Select any SalesChannel");
        //        return false;
        //    }
        //}


        if (ddlSalesChannel.SelectedValue != "0")
        {
            if (ddlSaleschannelType.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please Select any SalesChannelType");
                return false;
            }
        }

        if (ucDatePicker1.Date == "")
        {
            if (ucDatePicker2.Date != "")
            {
                ucMessage1.ShowInfo("Please fill scheme Start Date");
                return false;
            }
        }
        if (ucDatePicker2.Date == "")
        {
            if (ucDatePicker1.Date != "")
            {
                ucMessage1.ShowInfo("Please fill scheme End Date");
                return false;
            }
        }

        return true;
    }

    protected void btnCalculateSchemePerformance_Click(object sender, EventArgs e)
    {
        try
        {
            using (SchemeData objScheme = new SchemeData())
            {
                int result = objScheme.ExecuteSchemePerforma();
                if(result==0)
                {
                    ucMessage1.ShowInfo("Execution done Successfully");
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    
}
