/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 22-Jul-2016, Sumit Maurya, #CC01, New column in gridview added for EmailID. And validation added for EmailID.
 * 05-Aug-2016, Sumit Maurya, #CC02, SalesMancode removed and new checks added 
 *                                   1. Mobile Number or EmailID is mandatory to give.
 *                                   2. Mobile Number Regex added
 *                                   3. Mobile number and EmailID should be unique.
 * 19 Aug 2016, Karam Chand Sharma, #CC03, Pass status 1 in reference code     
 * 05-Oct-2016, Sumit Maurya, #CC04, Implementation of Update salesman applied.
 * 10-Oct-2016, Kalpana, #CC05, formatting in aspx page.
 * 10-Oct-2016, Sumit Maurya, #CC06, Implementation of Salesman creation / Updation according to logged in user implemented.
 * * 01-Feb-2016, Sumit Maurya, #CC07, Does not require filter.
 * 15-Nov-2018,Vijay Kumar Prajapati,#CC08,Add UserId in insert and upload saleman.
 * 
====================================================================================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Text.RegularExpressions; /* #CC01 Added */
using System.Data.SqlClient;
using System.Xml;
using System.IO; /* #CC04 Added */

public partial class Masters_SalesMan_UploadSalesMan : PageBase
{
    string strUploadedFileName = string.Empty;
    DataTable dtNew = new DataTable();
    int counter = 0;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        hlnkInvalid.Text = "";  /* #CC04 Added */
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            ucMsg.Visible = false;
            pnlGrid.Visible = false;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        GridSalesMan.Visible = false;
        try
        {
            DataSet dsGRN = null;
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                if (PageBase.SalesChanelID != 0)
                {
                    UploadFile.issaleschannel = true;
                }
                /* #CC04 Add Start */
                if (ddlUploadType.SelectedValue == "0")
                {
                    ucMsg.ShowInfo("Please select Uploadtype");
                    return;
                }
                string strExcelName = Convert.ToInt16(ddlUploadType.SelectedValue) == 1 ? "SalesMan" : "SalesManUpdate";
                isSuccess = UploadFile.uploadValidExcel(ref dsGRN, strExcelName);
                if (ddlUploadType.SelectedValue == "2")
                {
                    if (ddlUploadType.SelectedValue == "2")
                    {
                        BoundField Bfield = (BoundField)this.GridSalesMan.Columns[0];
                        Bfield.DataField = "SalesManCode";
                        Bfield.HeaderText = "SalesManCode";
                        if (this.GridSalesMan.Columns[5].ToString().ToLower() == "active")
                        {
                            this.GridSalesMan.Columns[5].Visible = true;
                            ((BoundField)GridSalesMan.Columns[5]).DataField = "Active";
                        }
                    }
                }
                else
                {
                    BoundField Bfield = (BoundField)this.GridSalesMan.Columns[0];
                    if (Bfield.DataField == "SalesManCode")
                    {
                        Bfield.DataField = "SalesChannelCode";
                        Bfield.HeaderText = "Sales Channel Code";
                    }
                    if (this.GridSalesMan.Columns[5].ToString().ToLower() == "active")
                    {
                        ((BoundField)GridSalesMan.Columns[5]).DataField = null;
                        GridSalesMan.Columns[5].Visible = false;
                    }
                }
                updGrid.Update();
                /* #CC04 Add End */
                /* isSuccess = UploadFile.uploadValidExcel(ref dsGRN, "SalesMan"); #CC04 Commented */

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        pnlGrid.Visible = false;
                        break;
                    case 2:

                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        Btnsave.Enabled = false;
                        pnlGrid.Visible = true;
                        GridSalesMan.Visible = true;
                        /* #CC04 Add Start*/
                        if (ddlUploadType.SelectedValue == "2")
                        {
                            GridSalesMan.Columns[6].Visible = true;
                        }
                        else/* #CC04 Add End*/
                            GridSalesMan.Columns[5].Visible = true;/*  #CC01 Commented */ /* #CC02 Uncommented*/
                        /* GridSalesMan.Columns[6].Visible = true;  #CC02 Commented */
                        GridSalesMan.DataSource = dsGRN;
                        GridSalesMan.DataBind();
                        updGrid.Update();
                        break;
                    case 1:
                        //if (ddlUploadType.SelectedValue == "2")
                        //{
                        //    Updatesalesman(dsGRN.Tables[0]);
                        //}
                        //else
                        InsertData(dsGRN);
                        break;
                    case 3:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;


                }

            }
            else if (UploadCheck == 2)
            {
                pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.SelectFile);

            }
            else if (UploadCheck == 4)
            {
                pnlGrid.Visible = false;
                ucMsg.ShowInfo("File size should be less than " + PageBase.ValidExcelLength + " KB");
            }
            else
            {
                pnlGrid.Visible = false;
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }


            updGrid.Update();
        }
        catch (Exception ex)
        {
            pnlGrid.Visible = false;
            ucMsg.ShowError(ex.Message.ToString());
            //clsException.clsHandleException.fncHandleException(ex, "");
        }
    }


    private void InsertData(DataSet dsSalesMan)
    {
        int mbCount = 0;
        DataTable dtSalesMan = dsSalesMan.Tables[0];
        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";



        if (dtSalesMan.Columns.Contains("Error") == false)
        {
            dtSalesMan.Columns.Add(dcError);

        }
        for (int i = 0; i <= dtSalesMan.Rows.Count - 1; i++)
        {
            if (dtSalesMan != null && dtSalesMan.Rows.Count > 0)
            {
                if (dtSalesMan.Rows[i]["MobileNumber"].ToString() != "" || string.IsNullOrEmpty(dtSalesMan.Rows[i]["MobileNumber"].ToString()) == false)
                {
                    try
                    {
                        Int64 t = Convert.ToInt64(dtSalesMan.Rows[i]["MobileNumber"].ToString());
                    }
                    catch (Exception ex)
                    {
                        counter = counter + 1;
                        if (dtSalesMan.Rows[i]["Error"] == "" && dtSalesMan.Rows[i]["Error"] == string.Empty)
                        {
                            dtSalesMan.Rows[i]["Error"] = "Mobile Number should only contain numeric digits";
                        }
                        else
                            dtSalesMan.Rows[i]["Error"] += ";Mobile Number should only contain numeric digits";

                    }

                    if (dtSalesMan.Rows[i]["MobileNumber"].ToString().Length != 10)
                    {
                        counter = counter + 1;
                        if (dtSalesMan.Rows[i]["Error"] == "" && dtSalesMan.Rows[i]["Error"] == string.Empty)
                        {
                            dtSalesMan.Rows[i]["Error"] = "Mobile Number should  be of exactly 10 digits";
                        }
                        else
                            dtSalesMan.Rows[i]["Error"] += ";Mobile Number should be of exactly 10 digits";
                    }

                    string st2 = "MobileNumber = '" + dtSalesMan.Rows[i]["MobileNumber"].ToString() + "'";
                    DataRow[] dr3 = dtSalesMan.Select(st2);
                    if (dr3.Length > 1)
                    {
                        counter = counter + 1;
                        if (dtSalesMan.Rows[i]["Error"] == "" && dtSalesMan.Rows[i]["Error"] == string.Empty)
                        {
                            dtSalesMan.Rows[i]["Error"] = "Mobile Number Already Existing";
                        }
                        else
                            dtSalesMan.Rows[i]["Error"] += ";Mobile Number Already Existing";

                    }


                }
                /* #CC04 Add Start */
                if (ddlUploadType.SelectedValue == "2")
                {
                    string strSalesMan = "SalesManName = '" + dtSalesMan.Rows[i]["SalesManName"].ToString().Trim() + "'and    SalesManCode = '" + dtSalesMan.Rows[i]["SalesManCode"].ToString().Trim() + "'";
                    DataRow[] dr2 = dtSalesMan.Select(strSalesMan);
                    if (dr2.Length > 1)
                    {
                        counter = counter + 1;
                        if (dtSalesMan.Rows[i]["Error"].ToString() == "" && dtSalesMan.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtSalesMan.Rows[i]["Error"] = "SalesMan supplied multiple times.";
                        }
                        else
                            dtSalesMan.Rows[i]["Error"] += ";SalesMan supplied multiple times.";
                    }
                    if (string.IsNullOrEmpty(dtSalesMan.Rows[i]["Active"].ToString()))
                    {
                        counter = counter + 1;
                        if (dtSalesMan.Rows[i]["Error"].ToString() == "" && dtSalesMan.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtSalesMan.Rows[i]["Error"] = "Column Active cannot be left blank.";
                        }
                        else
                            dtSalesMan.Rows[i]["Error"] += ";Column Active cannot be left blank.";
                    }

                    if (!string.IsNullOrEmpty(dtSalesMan.Rows[i]["Active"].ToString()) && dtSalesMan.Rows[i]["Active"].ToString().Trim().ToLower() != "yes")
                    {
                        if (dtSalesMan.Rows[i]["Active"].ToString().Trim().ToLower() != "no")
                        {
                            counter = counter + 1;
                            if (dtSalesMan.Rows[i]["Error"].ToString() == "" && dtSalesMan.Rows[i]["Error"].ToString() == string.Empty)
                            {
                                dtSalesMan.Rows[i]["Error"] = "Only Yes or No allowed in Active.";
                            }
                            else
                                dtSalesMan.Rows[i]["Error"] += ";Only Yes or No allowed in Active.";
                        }
                    }

                }
                else
                {
                    /* #CC04 Add End */
                    string srtwe = "SalesManName = '" + dtSalesMan.Rows[i]["SalesManName"].ToString() + "'and    SalesChannelCode = '" + dtSalesMan.Rows[i]["SalesChannelCode"].ToString() + "'";
                    DataRow[] dr1 = dtSalesMan.Select(srtwe);
                    if (dr1.Length > 1)
                    {
                        counter = counter + 1;
                        if (dtSalesMan.Rows[i]["Error"].ToString() == "" && dtSalesMan.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtSalesMan.Rows[i]["Error"] = "SalesMan Name Already Existing  under the current Sales Channel";
                        }
                        else
                            dtSalesMan.Rows[i]["Error"] += ";SalesMan Name Already Existing  under the current Sales Channel";
                    }
                } /* #CC04 Added */

                /* #CC04 Add End */
                /* #CC02 Comment Start 
                 if (dtSalesMan.Rows[i]["SalesManCode"].ToString() != "" || string.IsNullOrEmpty(dtSalesMan.Rows[i]["SalesManCode"].ToString()))
                  {
                      string st1 = "SalesManCode  = '" + dtSalesMan.Rows[i]["SalesManCode"].ToString() + "'";
                      DataRow[] dr2 = dtSalesMan.Select(st1);

                      if (dr2.Length > 1)
                      {
                          counter = counter + 1;
                          if (dtSalesMan.Rows[i]["Error"] == "" && dtSalesMan.Rows[i]["Error"] == string.Empty)
                          {
                              dtSalesMan.Rows[i]["Error"] = "SalesMan Code Already Existing";
                          }
                          else
                              dtSalesMan.Rows[i]["Error"] += ";SalesMan Code Already Existing";

                      }

                  }
                 #CC02 Comment End  */
                /* #CC01 Add Start */
                string RegexEmail = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";


                if (!Regex.IsMatch(dtSalesMan.Rows[i]["EmailID"].ToString().Trim(), RegexEmail) && dtSalesMan.Rows[i]["EmailID"].ToString().Trim() != "")
                {
                    counter = counter + 1;
                    if (dtSalesMan.Rows[i]["Error"].ToString() == "" || dtSalesMan.Rows[i]["Error"] == string.Empty)
                    {
                        dtSalesMan.Rows[i]["Error"] = "Invalid EmailID format.";
                    }
                    else
                        dtSalesMan.Rows[i]["Error"] += ", Invalid EmailID format.";
                }


                string strEmail = "EmailID = '" + dtSalesMan.Rows[i]["EmailID"].ToString().Trim() + "'";
                DataRow[] drEmail = dtSalesMan.Select(strEmail);
                if (drEmail.Length > 1)
                {
                    counter = counter + 1;


                    if (dtSalesMan.Rows[i]["Error"].ToString() == "" || dtSalesMan.Rows[i]["Error"] == string.Empty)
                    {
                        dtSalesMan.Rows[i]["Error"] = "EmailID Already Exists.";
                    }
                    else
                        dtSalesMan.Rows[i]["Error"] += ", EmailID Already Exists.";
                }

                /* #CC01 Add End */
                /* #CC02 Add Start */
                /*  string srtMobileAndEmail = "EmailID = '" + dtSalesMan.Rows[i]["EmailID"].ToString().Trim() + "'and    MobileNumber = '" + dtSalesMan.Rows[i]["MobileNumber"].ToString().Trim() + "'";
                  DataRow[] drMobileAndEmail = dtSalesMan.Select(srtwe);
                  if(drMobileAndEmail.Length>1)*/
                if (dtSalesMan.Rows[i]["EmailID"].ToString().Trim() == "" && dtSalesMan.Rows[i]["MobileNumber"].ToString().Trim() == "")
                {
                    counter = counter + 1;
                    if (dtSalesMan.Rows[i]["Error"].ToString() == "" || dtSalesMan.Rows[i]["Error"] == string.Empty)
                    {
                        dtSalesMan.Rows[i]["Error"] = "Please provide EmailID or Mobile number.";
                    }
                    else
                        dtSalesMan.Rows[i]["Error"] += ", Please provide EmailID or Mobile number.";
                }

                /* #CC02 Add End */
            }

        }
        hideUnhideControls(dtSalesMan);
    }

    private void hideUnhideControls(DataTable dtSalesMan)
    {
        dtNew = dtSalesMan.Clone();
        if (counter == 0)
        {
            Btnsave.Enabled = true;
            /* #CC04 Add Start*/
            if (ddlUploadType.SelectedValue == "2")
            {
                GridSalesMan.Columns[6].Visible = true;
            }
            else/* #CC04 Add End*/
                GridSalesMan.Columns[5].Visible = false; /*#CC01 Commented */
            /* #CC02 UnCommented */
            /* GridSalesMan.Columns[6].Visible = false; */
            /* #CC01 Added */
            /* #CC02 Commented */

        }

        else
        {
            /* #CC04 Add Start*/
            if (ddlUploadType.SelectedValue == "2")
            {
                GridSalesMan.Columns[6].Visible = true;
            }
            else/* #CC04 Add End*/
                GridSalesMan.Columns[5].Visible = true; /* #CC01 Commented */
            /* #CC02 UnCommented */
            /*GridSalesMan.Columns[6].Visible = true; */
            /* #CC01 Added */
            /* #CC02 Commented */
            Btnsave.Enabled = false;
        }
        if (dtSalesMan.Rows.Count > 0)
        {
            dvUploadPreview.Visible = true;
            pnlGrid.Visible = true;
            Btnsave.Visible = true;
            BtnCancel.Visible = true;
            GridSalesMan.Visible = true;
            ucMsg.Visible = false;

            GridSalesMan.DataSource = dtSalesMan;
            ViewState["SalesMan"] = dtSalesMan;
            GridSalesMan.DataBind();
            updGrid.Update();

        }
        else
            pnlGrid.Visible = false;
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            /* #CC04 Add Start */
            if (ddlUploadType.SelectedValue == "2")
            {
                Updatesalesman((DataTable)ViewState["SalesMan"]);
            }
            else
                /* #CC04 Add End */
                if (ViewState["SalesMan"] != null)
                {
                    int intResult = 0;
                    DataTable dtSalesMan = new DataTable();
                    DataTable Tvp = new DataTable();

                    dtSalesMan = (DataTable)ViewState["SalesMan"];
                    using (CommonData ObjCommom = new CommonData())
                    {
                        Tvp = ObjCommom.GettvpTableSalesManUpload();
                    }
                    /* #CC01 Add Start */
                    if (!Tvp.Columns.Contains("EmailID"))
                    {
                        Tvp.Columns.Add("EmailID", typeof(string));
                        Tvp.AcceptChanges();
                    }
                    /* #CC01 Add End */
                    foreach (DataRow dr in dtSalesMan.Rows)
                    {
                        DataRow drow = Tvp.NewRow();

                        drow[0] = dr["SalesChannelCode"].ToString();
                        drow[1] = dr["SalesManName"].ToString();
                        /*drow[2] = dr["SalesManCode"].ToString(); #CC02 Commented */
                        drow[3] = dr["Address"].ToString();
                        drow[4] = dr["MobileNumber"].ToString();
                        drow[5] = dr["EmailID"].ToString(); /* #CC01 Added */
                        //drow[5] = 0;


                        Tvp.Rows.Add(drow);
                    }
                    Tvp.AcceptChanges();
                    using (SalesmanData objPrimarySales = new SalesmanData())
                    {
                        objPrimarySales.SalesChannelID = Convert.ToInt32(PageBase.SalesChanelID); /* #CC06 Added */
                        objPrimarySales.UserID = PageBase.UserId;/* #CC08 Added */
                        intResult = objPrimarySales.InsertInfoSalesManUpload(Tvp);

                        if (objPrimarySales.ErrorDetailXML != null && objPrimarySales.ErrorDetailXML != string.Empty)
                        {
                            //ucMsg.XmlErrorSource = objPrimarySales.ErrorDetailXML;
                            //return;
                            StringReader theReader = new StringReader(objPrimarySales.ErrorDetailXML);
                            DataSet theDataSet = new DataSet();
                            theDataSet.ReadXml(theReader);
                            CreateInvalidDataLink(theDataSet);
                            GridSalesMan.DataSource = null;
                            GridSalesMan.DataBind();
                            pnlGrid.Visible = false;
                            updGrid.Update();
                            return;
                        }
                        if (objPrimarySales.Error != null && objPrimarySales.Error != "")
                        {
                            ucMsg.ShowError(objPrimarySales.Error);
                            return;
                        }
                        if (intResult == 2)
                        {
                            ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                            return;
                        }

                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        ClearForm();


                    }
                }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }


    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();

    }

    void ClearForm()
    {
        /* #CC04 Add Start */
        ddlUploadType.SelectedValue = "0";
        hlnkInvalid.Text = "";
        /* #CC04 Add End */
        pnlGrid.Visible = false;
        updGrid.Update();
    }


    protected void GridSalesMan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridSalesMan.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            GridSalesMan.DataSource = (DataTable)ViewState["SalesMan"];

            GridSalesMan.DataBind();
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        DataTable dt;
        DataSet ds = new DataSet();
        DataSet dsReferenceCode = new DataSet();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ObjSalesChannel.BilltoRetailer = true;
            ObjSalesChannel.BlnShowDetail = true;
            ObjSalesChannel.SearchType = EnumData.eSearchConditions.Active;
            ObjSalesChannel.StatusValue = 1;/*#CC03 ADDED*/
            /* #CC05 Add Start */
            ObjSalesChannel.SalesChannelID = Convert.ToInt32(PageBase.SalesChanelID);
            ObjSalesChannel.BindChild = Convert.ToInt16(PageBase.SalesChanelTypeID) == 6 ? 1 : 0;
            /* #CC05 Add End */
            dt = ObjSalesChannel.GetSalesChannelInfo();
            //dt.DefaultView.RowFilter = "(SalesChannelTypeID = 6 or SalesChannelTypeID = 7 or SalesChannelTypeID = 9) and Status = true ";/*#CC07 commented*/
            dt = dt.DefaultView.ToTable();
            string[] strCode = new string[] { "SalesChannelName", "SalesChannelCode" };
            dt = dt.DefaultView.ToTable(true, strCode);

            ds.Tables.Add(dt);

            dsReferenceCode = ds;
            if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "/2 List";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }

        }
    }
    /* #CC04 Add Start */

    public void Updatesalesman(DataTable dt)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();
            if (!dt.Columns.Contains("SessionID"))
                dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            else
            {
                dt.Columns.Remove("SessionID");
                dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            }
            if (!dt.Columns.Contains("CreatedBy"))
                dt.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
            if ((UpdatesalesManBCP(dt)) == true)
            {
                using (SalesmanData objsalesman = new SalesmanData())
                {
                    objsalesman.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                    objsalesman.SalesChannelID = Convert.ToInt32(PageBase.SalesChanelID); /* #CC06 Added */
                    objsalesman.UserID = PageBase.UserId;/*#CC08 Added*/
                    DataSet dsResult = objsalesman.BulkSalesmanUpdate();
                    if (objsalesman.intOutParam == 0)
                    {
                        ucMsg.ShowSuccess("Records updated successfully.");
                        pnlGrid.Visible = false;
                        GridSalesMan.DataSource = null;
                        GridSalesMan.DataBind();
                        ViewState["SalesMan"] = null;
                        updGrid.Update();
                    }
                    else if (objsalesman.intOutParam == 1)
                    {
                        ucMsg.ShowError(objsalesman.Error);
                        pnlGrid.Visible = false;
                        GridSalesMan.DataSource = null;
                        GridSalesMan.DataBind();
                        ViewState["SalesMan"] = null;
                        updGrid.Update();
                    }
                    else if (objsalesman.intOutParam == 2)
                    {
                       // ucMsg.ShowInfo("Partial data processed, click Invalid data link for invalid data.");
                        ucMsg.ShowInfo("Click Invalid data link for invalid data.");
                        pnlGrid.Visible = false;
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ExportInExcel(dsResult, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        GridSalesMan.DataSource = null;
                        GridSalesMan.DataBind();
                        ViewState["SalesMan"] = null;
                        updGrid.Update();
                    }
                }

            }
            else
            {
                ucMsg.ShowInfo("Unable to update Salesman.");
            }


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    public bool UpdatesalesManBCP(DataTable dtUpload)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadSalesman";
                bulkCopy.ColumnMappings.Add("SalesmanCode", "SalesmanCode");
                bulkCopy.ColumnMappings.Add("SalesmanName", "SalesmanName");
                bulkCopy.ColumnMappings.Add("Address", "Address");
                bulkCopy.ColumnMappings.Add("EmailID", "Email");
                bulkCopy.ColumnMappings.Add("MobileNumber", "MobileNumber");
                bulkCopy.ColumnMappings.Add("Active", "Active");
                bulkCopy.ColumnMappings.Add("SessionID", "SessionID");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            return false;
        }
    }
    private void ExportInExcel(DataSet DsExport, string strFileName)
    {
        try
        {
            if (DsExport != null && DsExport.Tables.Count > 0)
            {
                PageBase.ExportToExeclV2(DsExport, strFileName, DsExport.Tables.Count);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            // PageBase.Errorhandling(ex);
        }
    }

    /* #CC04 Add End */
    private void CreateInvalidDataLink(DataSet dsResult)
    {
        hlnkInvalid.Visible = true;
        strUploadedFileName = "InvalidData" + DateTime.Now.Ticks;
        ucMsg.ShowInfo(Resources.Messages.PartialDataUpload);
        PageBase.ExportInExcel(dsResult, strUploadedFileName);
        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strUploadedFileName + ".xlsx";
        hlnkInvalid.Text = "Invalid Data";

    }


}
