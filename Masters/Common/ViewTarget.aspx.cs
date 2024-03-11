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
/*
 * --------------------------------------------------------------------------------------------------------
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description.
 * 04-Aug-2017, Sumit Maurya, #CC01, New columns added in Export to excel.
 * 14-Nov-2017,Vijay Kumar Prajapati,#CC02, view report for distributor wise.
 * 31-Jan-2018, Sumit Maurya, #CC03, Implementation done for Hierarchylevel also (Done for Comio).
 * 15-Feb-2018, Sumit Maurya, #CC04, New dropdown added for search parameter targetnames and filter data (Done for Comio).
 *  17-Feb-2018, Sumit Maurya, #CC05, Corrected for reatiler (Done for comio).
 *  12-May-2018, Sumit Maurya, #CC06, Saleschannel Typr dropdown binding method changed (Done for Motorola).
 *  23-May-2018, Sumit Maurya, #CC07, Userid provided to get data according to user (Done for motorola).
 * --------------------------------------------------------------------------------------------------------
 */


public partial class ViewTarget : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                if (PageBase.BaseEntityTypeID == 3)
                {
                    //ddlType.SelectedValue = PageBase.RetailerEntityTypeID.ToString(); ;
                    //ddlType.Enabled = false;
                    //ddlType_SelectedIndexChanged(ddlType, null);
                    //ddlUserType.SelectedValue = Convert.ToString(PageBase.RetailerEntityTypeID);
                    //ddlUserType.Enabled = false;
                }
                else
                {
                    //ddlType.SelectedValue = "0";
                    //ddlType.Enabled = true;
                    //ddlUserType.SelectedValue = "0";
                    //ddlUserType.Enabled = true;
                }
                dvhide.Visible = false;
                BindType();
                //ddlUserType.Items.Insert(0, new ListItem("Select", "0"));
                //BindLevel();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
            string str = ex.ToString().ToLower();
        }
    }

    public void BindType()
    {
        try
        {
            ddlUserType.Items.Clear();
            using (TargetData objTarget = new TargetData())
            {
                objTarget.UserType = 1;
                objTarget.showLevel = false;

                if (PageBase.SalesChanelID == 0)
                {
                    objTarget.UserTypeID = Convert.ToInt16(PageBase.HierarchyLevelID);
                    objTarget.OwnLevel = 1;
                }
                else
                {
                    objTarget.UserTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                }
                String[] colArray = { "TargetToType", "TargetUserType" };
                objTarget.UserId = PageBase.UserId; /* #CC08 Added  */
                DataTable dt = objTarget.GetTargetLevelUser();
                PageBase.DropdownBinding(ref ddlUserType, dt, colArray);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        if (ucDatePicker1.Date != "")
        {
            if (ucDatePicker2.Date == "")
            {
                ucMessage1.ShowInfo("Invalid Date Range");
                return;
            }
        }
        if (ucDatePicker2.Date != "")
        {
            if (ucDatePicker1.Date == "")
            {
                ucMessage1.ShowInfo("Invalid Date Range");
                return;
            }
        }
        FillGrid();
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        ucDatePicker1.Date = "";
        ucDatePicker2.Date = "";
        //ddlType.SelectedValue = "0";
        dvhide.Visible = false;
        //ucDatePicker1.Date = PageBase.Fromdate;
        //ucDatePicker2.Date = PageBase.ToDate;
        ddlUserType.Items.Clear();
        ddlUserType.Items.Insert(0, new ListItem("Select", "0"));

    }
    void FillGrid()
    {

        try
        {
            DataTable DtTarget = new DataTable();
            /* using (TargetData ObjTarget = new TargetData())*/
            using (TargetData ObjTarget = new TargetData())
            {
                if (ddlUserType.SelectedIndex != 0)
                    ObjTarget.UserType = ddlUserType.SelectedValue == Convert.ToString(PageBase.RetailerEntityTypeID) ? Convert.ToInt16(3) : Convert.ToInt16(ddlUserType.SelectedValue);
                ObjTarget.UserTypeID = Convert.ToInt16(ddlUserType.SelectedValue);
                ObjTarget.TargetFrom = ucDatePicker1.Date == "" ? ObjTarget.TargetFrom : Convert.ToDateTime(ucDatePicker1.Date);
                ObjTarget.TargetTo = ucDatePicker2.Date == "" ? ObjTarget.TargetTo : Convert.ToDateTime(ucDatePicker2.Date);
               /*#CC02 Added Started*/ if(PageBase.SalesChanelID>0)
                {
                    ObjTarget.TargetForID = PageBase.SalesChanelID;
                } /*#CC02 Added End*/
                ObjTarget.TargetID = 0;
                /* #CC04 Add Start */
                if (ddlTarget.SelectedIndex != 0)
                {
                    ObjTarget.TargetName = ddlTarget.SelectedValue;
                }
                /* #CC04 Add End */
                ObjTarget.UserId = PageBase.UserId;
                ObjTarget.CompanyId = PageBase.ClientId;
                DtTarget = ObjTarget.GetTargetInfo();
            };
            if (DtTarget != null && DtTarget.Rows.Count > 0)
            {
                GridTarget.DataSource = DtTarget;
                GridTarget.DataBind();

                dvhide.Visible = true;



            }
            else
            {
                GridTarget.DataSource = null;
                GridTarget.DataBind();
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                dvhide.Visible = false;
            }


        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    //private void BindLevel()
    //{
    //    try
    //    {
    //        ddlType.Items.Clear();
    //        using (TargetData ObjTarget = new TargetData())
    //        {
    //            ObjTarget.showLevel = false;
    //            if (PageBase.SalesChanelID == 0)
    //                ObjTarget.UserType = 1;
    //            else
    //                ObjTarget.UserType = 2;
    //            if (PageBase.SalesChanelID == 0)
    //            {
    //                ObjTarget.UserTypeID = Convert.ToInt16(PageBase.HierarchyLevelID);
    //            }
    //            else
    //            {
    //                ObjTarget.UserTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
    //            }
    //            string[] str = { "TargetToType", "TargetUserType" };
    //            PageBase.DropdownBinding(ref ddlType, ObjTarget.GetTargetLevelUser(), str);
    //        };

    //    }
    //    catch (Exception ex)
    //    {
    //        PageBase.Errorhandling(ex);
    //        ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
    //    }

    //}

    protected void GridTarget_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int Result = 0;
            Int32 TargetId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "cmdDelete")
            {
                using (TargetData ObjTarget = new TargetData())
                {
                    ObjTarget.TargetID = TargetId;
                    Result = ObjTarget.DeleteTargetInfo();
                    if (Result == 0)
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.Delete);
                        FillGrid();
                    }
                    else
                    {
                        ucMessage1.ShowError(ObjTarget.ErrorMessage);
                    }


                };
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());

        }
    }
    protected void GridTarget_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int32 intTargetID = Convert.ToInt16(GridTarget.DataKeys[e.Row.RowIndex].Value);

                GridViewRow GVR = e.Row;
                HyperLink HLDetails = default(HyperLink);
                HLDetails = (HyperLink)GVR.FindControl("HLDetails");
                string strViewDBranchDtlURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(intTargetID), PageBase.KeyStr)).ToString().Replace("+", " "); HLDetails.Text = "Details";
                HLDetails.Attributes.Add("onClick", string.Format("return popup('" + strViewDBranchDtlURL + "')"));

                /* #CC03 Add Start */

                ImageButton imgBtnEdit = (ImageButton)GVR.FindControl("imgBtnEdit");
                string strEditDBranchDtlURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(intTargetID), PageBase.KeyStr)).ToString().Replace("+", " ");
                imgBtnEdit.Attributes.Add("onClick", string.Format("return popupEdit('" + strEditDBranchDtlURL + "')"));


                ImageButton imgBtnDel = (ImageButton)GVR.FindControl("img2");
                HiddenField hdntargetToDate = (HiddenField)GVR.FindControl("hdnTargetTo");
                if(Convert.ToDateTime(hdntargetToDate.Value)>DateTime.Now)
                {
                    imgBtnEdit.Visible = true;
                    imgBtnDel.Visible = true;
                }
                else
                {
                    imgBtnEdit.Visible = false;
                    imgBtnDel.Visible = false;
                }

                /* #CC03 Add End */

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            using (TargetData objtarget = new TargetData())
            {
                //objtarget.UserType=Convert.ToInt16( ddlType.SelectedValue);
                // objtarget.UserTypeID=Convert.ToInt16( ddlUserType.SelectedValue);
                // objtarget.TargetFrom=Convert.ToDateTime(ucDatePicker1.Date);
                // objtarget.TargetTo=Convert.ToDateTime( ucDatePicker2.Date);


                // dt=objtarget.GetTargetInfo();
                if (ddlUserType.SelectedIndex != 0)
                    objtarget.UserType = ddlUserType.SelectedValue == Convert.ToString(PageBase.RetailerEntityTypeID) ? Convert.ToInt16(3) : Convert.ToInt16(ddlUserType.SelectedValue);
                objtarget.UserTypeID = Convert.ToInt16(ddlUserType.SelectedValue);
                objtarget.TargetFrom = ucDatePicker1.Date == "" ? objtarget.TargetFrom : Convert.ToDateTime(ucDatePicker1.Date);
                objtarget.TargetTo = ucDatePicker2.Date == "" ? objtarget.TargetTo : Convert.ToDateTime(ucDatePicker2.Date);
                objtarget.TargetID = 0;
                /* #CC04 Add Start */
                if (ddlTarget.SelectedIndex != 0)
                {
                    objtarget.TargetName = ddlTarget.SelectedValue;
                }
                /* #CC04 Add End */

                dt = objtarget.GetTargetInfo();

            };
            string[] DsCol = new string[] { "TargetName", "TargetFor",   "TargetFrom", "TargetTo", "TargetCategory", "TargetBasedOn", "TargetType", "TotalTarget" }

            ; /*#CC01 Added */
            /*{ "TargetFor", "TargetFrom", "TargetTo", "TargetType" }; #CC01 Commented */
            dt = dt.DefaultView.ToTable(true, DsCol);
            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "TargetList";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);

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
    protected void GridTarget_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridTarget.PageIndex = e.NewPageIndex;
        FillGrid();
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
            PageBase.DropdownBinding(ref ddlUserType, ObjSalesChannel.GetSalesChannelType(), str);
        };
    }
    void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();

            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.HierarchyLevelID = Convert.ToInt16(PageBase.HierarchyLevelID);
                objuser.AllownotOwn = true;
                objuser.allowhierarchy = PageBase.AllowAllHierarchy;
                dt = objuser.GetAllLowerHierarchyLevel();

            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlUserType, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
 //   protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
 //   {
 //       try
 //       {
 //           ddlUserType.Items.Clear();
 //           if (ddlType.SelectedValue != "0")
 //           {
 //               //using (TargetData objTarget = new TargetData())
 //               //{
 //               //    objTarget.UserType = 2;
 //               //    objTarget.showLevel = true;

 //               //    if (PageBase.SalesChanelID == 0)
 //               //    {
 //               //        objTarget.UserTypeID = Convert.ToInt16(PageBase.HierarchyLevelID);
 //               //        objTarget.OwnLevel = 1;
 //               //    }
 //               //    else
 //               //    {
 //               //        objTarget.UserTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
 //               //        objTarget.OwnLevel = 2;
 //               //    }

 //               //    String[] colArray = { "ID", "TYPEName" };
 //               //    DataTable dt = objTarget.GetTargetLevelUser();
 //               //    ddlUserType.DataSource = dt;
 //               //    ddlUserType.DataTextField = "TYPEName";
 //               //    ddlUserType.DataValueField = "ID";
 //               //    ddlUserType.DataBind();
 //               //    ddlUserType.Items.Insert(0, new ListItem("Select", "0"));
 //               //}

 //               using (TargetData objTarget = new TargetData())
 //               {
 ///* #CC05 Add Start */
 //                   if (ddlType.SelectedValue == Convert.ToString(PageBase.RetailerEntityTypeID))
 //                   {
 //                       ddlUserType.Items.Add(new ListItem("Select", "0"));
 //                       ddlUserType.Items.Add(new ListItem("Retailer", Convert.ToString(PageBase.RetailerEntityTypeID)));
 //                   }
 //                   else
 //                   {

 //                           objTarget.UserType = Convert.ToInt16(ddlType.SelectedValue);
 //                           objTarget.showLevel = true;
 //                           objTarget.SelectedTypeID = Convert.ToInt16(ddlType.SelectedValue);
 //                           objTarget.OwnLevel = 1;
 //                           objTarget.UserTypeID = 1;
 //                           objTarget.UserType = Convert.ToInt16(ddlType.SelectedValue);
 //                           objTarget.UserId = PageBase.UserId; /* #CC07 Added  */
 //                           String[] colArray = { "ID", "TYPEName" };
 //                           PageBase.DropdownBinding(ref ddlUserType, objTarget.GetTargetLevelUser(), colArray);
                       

 //                       /* #CC06 Add End */
                       

 //       } /* #CC05 Added */
 //               }
 //           }
 //           else
 //           {
 //               ddlUserType.Items.Insert(0, new ListItem("Select", "0"));
 //           }
 //       }
 //       catch (Exception ex)
 //       {
 //           ucMessage1.ShowError(ex.Message.ToString(), GlobalErrorDisplay());
 //           PageBase.Errorhandling(ex);
 //       }
 //   }

    /* #CC04 Add Start */
    public void BindTargetNames()
    {
        try
        {
            ddlTarget.Items.Clear();
            using (TargetData objTarget = new TargetData())
            {
                objTarget.UserId = PageBase.UserId;
                /* #CC04 Add Start */
                objTarget.TargetUserType = Convert.ToInt16(ddlUserType.SelectedValue == Convert.ToString(PageBase.RetailerEntityTypeID) ? "3" : ddlUserType.SelectedValue);
                objTarget.TargetUserTypeID = Convert.ToInt16(ddlUserType.SelectedValue);
                objTarget.TargetStatus = 255;
                /* #CC04 Add End */
                DataSet ds = objTarget.GetTargetName();
                String[] colArray = { "TargetName", "TargetName" };
                PageBase.DropdownBinding(ref ddlTarget, ds.Tables[0], colArray);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), GlobalErrorDisplay());
        }
    }
   
    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
          try
        {
        BindTargetNames();
        }
          catch (Exception ex)
          {
              ucMessage1.ShowError(ex.ToString(), GlobalErrorDisplay());
          }
    } 
    /* #CC04 Add End */
}

