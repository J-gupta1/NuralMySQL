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

public partial class Masters_Common_ManageGifts : PageBase
{
    DataTable giftinfo;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            databind();
            chkActive.Checked = true;

        }

    }


    # region user functions

    public void blankinsert()
    {
        txtGiftName.Text = "";
        txtEligiblity.Text = "";
        btnCreate.Text = "Submit";
        updAddUserMain.Update();
    }

    public void blankserch()
    {
        txtSerName.Text = "";
        UpdSearch.Update();
    }

    public void databind()
    {


        using (SchemeData obj = new SchemeData())
        {
            try
            {
                ucMessage1.Visible = false;
                obj.GiftName = txtSerName.Text.ToString();
                obj.GiftID = 0;
                DataTable dt = obj.GetGiftInfo();
                ViewState["Table"] = dt;
                grdGift.DataSource = dt;
                grdGift.DataBind();
                blankinsert();
                updgrid.Update();

            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        }

    }
    
    # endregion

    #region control functions

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }       //Pankaj Dhingra
        using (SchemeData obj = new SchemeData())
        {
           obj.GiftName = txtGiftName.Text.Trim();
           obj.EligiblityPoints = Convert.ToInt32(txtEligiblity.Text.Trim());
                if (chkActive.Checked == true)
                {
                    obj.GiftStatus = 1;
                }
                else
                {
                    obj.GiftStatus = 0;
                }

                if (ViewState["GiftID"] == null || (int)ViewState["GiftID"] == 0)
                {
                    obj.InsUpdGiftInfo();

                    if (obj.Error == "")
                    {
                        databind();
                        ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        blankinsert();
                        blankserch();
                    }
                    else
                    {

                        ucMessage1.ShowInfo(obj.Error);
                    }
                }
                else
                {
                   obj.GiftID= (int)ViewState["GiftID"];
                   obj.InsUpdGiftInfo();
                    if (obj.Error== "")
                    {
                        databind();
                        blankinsert();
                        ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                        ViewState["GiftID"] = null;
                    }
                    else
                    {

                        ucMessage1.ShowInfo(obj.Error);
                    }
                }

            }
        
    }



    protected void btncancel_click(object sender, EventArgs e)
    {
        blankinsert();
        blankserch();
        databind();


    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        blankinsert();
        if (txtSerName.Text == "")
        {
            ucMessage1.ShowInfo("Please enter the search parameter");
            return;
        }

        databind();
    }
    protected void grdColor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (SchemeData obj = new SchemeData())
        {
            if (e.CommandName == "Active")
            {
                try
                {
                    obj.GiftID = Convert.ToInt32(e.CommandArgument);
                    DataTable dt = obj.GetGiftInfo();
                    obj.GiftName = Convert.ToString(dt.Rows[0]["GiftName"]);
                    obj.EligiblityPoints = Convert.ToInt32(dt.Rows[0]["EligiblityPoint"]);
                     obj.GiftStatus = Convert.ToInt16(dt.Rows[0]["Active"]);

                    if (obj.GiftStatus == 1)
                    {
                        obj.GiftStatus = 0;
                    }
                    else
                    {
                        obj.GiftStatus = 1;
                    }
                    obj.InsUpdGiftInfo();
                    if (obj.Error == "")
                    {
                        databind();
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
   
                    }
                    else
                    {
                        ucMessage1.ShowInfo(obj.Error);
                    }

          }
                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }


            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    obj.GiftID= Convert.ToInt32(e.CommandArgument);
                    ViewState["GiftID"] = obj.GiftID;
                    DataTable dt = obj.GetGiftInfo();

                    txtGiftName.Text = Convert.ToString(dt.Rows[0]["GiftName"]);
                    txtEligiblity.Text = Convert.ToString(dt.Rows[0]["EligiblityPoint"]);
                    obj.GiftStatus = Convert.ToInt16(dt.Rows[0]["Active"]);

                    if (obj.GiftStatus == 1)
                    {
                        chkActive.Checked = true;
                    }
                    else
                    {
                        chkActive.Checked = false;
                    }
                    btnCreate.Text = "Update";
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

    protected void btngetalldta_Click(object sender, EventArgs e)
    {
        blankserch();
        blankinsert();
        databind();
    }

    protected void grdColor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGift.PageIndex = e.NewPageIndex;
        databind();
    }

    # endregion


    # region export to excel

    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {

            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "GiftName", "GiftStatus" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["GiftStatus"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "GiftDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                ViewState["Table"] = null;
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            ViewState["Table"] = null;
        }


    }

    #endregion


    protected void grdColor_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
