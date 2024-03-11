#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 08-June-2018
 * Description : Uploading(Add/Edit) District Records from Excel Sheet.
 * ================================================================================================
 * Change Log:
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 6-July-2018, #CC01, RAKESH RAJ, InvalidData Link Added and Hide Grid
 * ====================================================================================================
 */
#endregion

using System;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Xml;
using System.IO;


public partial class Masters_Common_ManageDistrictUpload : PageBase
{
    #region Variables/ Instances

    string strUploadedFileName = string.Empty;
    //int counter = 0;
    string strFileName = string.Empty;
    byte isSuccess = 1;
    UploadFile UploadFile = new UploadFile();

    #endregion
   
    #region Events 
    
    protected void Page_Load(object sender, EventArgs e)
    {
        hlnkInvalid.Text = "";
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            ucMsg.Visible = false;
           // pnlGrid.Visible = false;
        }
    }

    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "2")
        {
            Response.Redirect("ManageDistrict.aspx");
        }

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {        
       //// pnlGrid.Visible = false;
        try
        {
            DataSet dsUpload = null;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                              
                isSuccess = UploadFile.uploadValidExcel(ref dsUpload, "DistrictTemplateEdit");

                if (isSuccess == 1)
                {
                    InsertData(dsUpload);
                }
                //update
                   // #CC01
                    //BoundField Bfield0 = (BoundField)this.//GridUpload.Columns[1];
                    //Bfield0.DataField = "DistrictCode";
                    //Bfield0.HeaderText = "District Code";

                   
                    //if (this.//GridUpload.Columns[3].ToString().ToLower() == "active")
                    //{
                    //    this.//GridUpload.Columns[3].Visible = true;
                    //    ((BoundField)//GridUpload.Columns[3]).DataField = "Active";
                    //}
                    
            
                //updGrid.Update();
            

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                       //// pnlGrid.Visible = false;
                        break;
                    case 2:

                        hlnkInvalid.Visible = true;
                        strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ucMsg.ShowInfo(Resources.Messages.PartialDataUpload);
                        DataView dvError = dsUpload.Tables[0].DefaultView;
                        dvError.RowFilter = "Error<>''";
                        DataTable dtError = dvError.ToTable();
                        DataSet dsError = new DataSet();
                        dsError.Tables.Add(dtError);
                        PageBase.ExportInExcel(dsError, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";

                        // #CC01
                        //ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        //Btnsave.Enabled = false;
                       //// pnlGrid.Visible = true;
                        ////GridUpload.Visible= true;
                        //GridUpload.Columns[4].Visible = true;
                        
                        //GridUpload.DataSource = dsUpload;
                        //GridUpload.DataBind();
                        //updGrid.Update();
                        break;
                    case 1:
                        SaveData(dsUpload.Tables[0]);
                        break;
                    case 3:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;


                }

            }
            else if (UploadCheck == 2)
            {
               //// pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
               //// pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.SelectFile);

            }
            else if (UploadCheck == 4)
            {
               //// pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.FileSize + PageBase.ValidExcelLength + " KB");
            }
            else
            {
               //// pnlGrid.Visible = false;
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }


            //updGrid.Update();
        }
        catch (Exception ex)
        {
           //// pnlGrid.Visible = false;
            ucMsg.ShowError(ex.Message.ToString());
            //clsException.clsHandleException.fncHandleException(ex, "");
        }
    }


    // #CC01 Start
    //protected void BtnCancel_Click(object sender, EventArgs e)
    //{
    //    ucMsg.ShowControl = false;
    //    ClearForm();

    //}
    
    //protected void GridUpload_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        //GridUpload.PageIndex = e.NewPageIndex;
    //        DataTable dt = new DataTable();
    //        //GridUpload.DataSource = (DataTable)ViewState["UploadData"];

    //        //GridUpload.DataBind();
    //    }

    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //    }
    //}
    // #CC01 End

    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        using (CommonData obj = new CommonData())
        {
            DataSet dsReferenceCode = obj.DownloadRefCodeExcel(3);
            if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
            {
                try
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "RefCodeDistrict";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport);
                    // ViewState["Table"] = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.NoRecord);

            }
        }

    }

    #endregion

    #region Methods

    //#CC01
    private void SaveData(DataTable dtSave)
    {
        try
        {
            DataTable Tvp = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                Tvp = ObjCommom.GettvpTableDistrictUpload();
            }

            foreach (DataRow dr in dtSave.Rows)
            {
                DataRow drow = Tvp.NewRow();

                drow[0] = dr["DistrictName"].ToString();
                drow[1] = dr["DistrictCode"].ToString();
                drow[2] = dr["StateName"].ToString();
                drow[3] = dr["Active"].ToString();
                Tvp.Rows.Add(drow);
            }
            Tvp.AcceptChanges();

            using (CommonData objPrimarySales = new CommonData())
            {
                DataSet dsResult = objPrimarySales.BulkUploadUsingTVP(Tvp, "prcTVPDistrictUpload", "@tvpDistrictUpload");

                if (objPrimarySales.ErrorDetailXML != null && objPrimarySales.ErrorDetailXML != string.Empty)
                {
                    dsResult.ReadXml(new XmlTextReader(new StringReader(objPrimarySales.ErrorDetailXML)));
                    CreateInvalidDataLink(dsResult);
                    return;
                }
                else if (objPrimarySales.Error != null && objPrimarySales.Error != "")
                {
                    CreateInvalidDataLink(dsResult);
                    return;
                }
                else if (objPrimarySales.Error == "2")
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    return;
                }

                ucMsg.ShowSuccess(Resources.Messages.DataUploadSuccess);
                ClearForm();


            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    private void CreateInvalidDataLink(DataSet dsResult)
    {
        hlnkInvalid.Visible = true;
        strFileName = "InvalidData" + DateTime.Now.Ticks;
        ucMsg.ShowInfo(Resources.Messages.PartialDataUpload);
        
        PageBase.ExportInExcel(dsResult, strFileName);
        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
        hlnkInvalid.Text = "Invalid Data";

    }

    private void InsertData(DataSet dsUpload)
    {
        try
        {
            DataTable dtUpload = dsUpload.Tables[0];
            DataColumn dcError = new DataColumn();
            dcError.DataType = System.Type.GetType("System.String");
            dcError.ColumnName = "Error";

            if (dtUpload.Columns.Contains("Error") == false)
            {
                dtUpload.Columns.Add(dcError);

            }

            for (int i = 0; i <= dtUpload.Rows.Count - 1; i++)
            {
                if (dtUpload != null && dtUpload.Rows.Count > 0)
                {
                    string strColName2= "DistrictName = '" + dtUpload.Rows[i]["DistrictName"].ToString().Trim() + "'";
                    DataRow[] dr2 = dtUpload.Select(strColName2);

                    if (dr2.Length > 1)
                    {
                        if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtUpload.Rows[i]["Error"] = "DistrictName " + Resources.Messages.UniqeMsg;
                        }
                        else
                            dtUpload.Rows[i]["Error"] += ";DistrictName " + Resources.Messages.UniqeMsg;
                        isSuccess = 2;
                    }

                        string strColName1 = "DistrictCode = '" + dtUpload.Rows[i]["DistrictCode"].ToString().Trim() + "'";
                        DataRow[] dr3 = dtUpload.Select(strColName1);
                 
                        if (dr3.Length > 1)
                        { 
                            if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                            {
                                dtUpload.Rows[i]["Error"] = "DistrictCode " + Resources.Messages.UniqeMsg;
                            }
                            else
                                dtUpload.Rows[i]["Error"] += ";DistrictCode " + Resources.Messages.UniqeMsg;
                            isSuccess = 2;
                        }

                 
                        if (string.IsNullOrEmpty(dtUpload.Rows[i]["Active"].ToString()))
                        {
                           
                            if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                            {
                                dtUpload.Rows[i]["Error"] = "Active " + Resources.Messages.BlankMsg;
                            }
                            else
                                dtUpload.Rows[i]["Error"] += ";Active " + Resources.Messages.BlankMsg;
                            isSuccess = 2;
                        }

                        if (!string.IsNullOrEmpty(dtUpload.Rows[i]["Active"].ToString()) && dtUpload.Rows[i]["Active"].ToString().Trim().ToLower() != "yes")
                        {
                            if (dtUpload.Rows[i]["Active"].ToString().Trim().ToLower() != "no")
                            {   
                                if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                                {
                                    dtUpload.Rows[i]["Error"] = Resources.Messages.YesNoOnly + " in Active!";
                                }
                                else
                                    dtUpload.Rows[i]["Error"] += ";" + Resources.Messages.YesNoOnly + " in Active!";
                                isSuccess = 2;
                            }
                        }

                
              
                }

            }
       //     hideUnhideControls(dtUpload);

        }
        catch (Exception)
        {

            throw;
        }
    }
    // #CC01 Start
    //private void hideUnhideControls(DataTable dtUpload)
    //{
    //   // dtNew = dtUpload.Clone();
    //    if (counter == 0)
    //    {
    //        //Btnsave.Enabled = true;
    //        //GridUpload.Columns[4].Visible = false;
    //        ucMsg.Visible = false;
    //     }

    //    else
    //    {
    //        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
    //        ucMsg.Visible = true;
    //        //Btnsave.Enabled = false;
    //        //GridUpload.Columns[4].Visible = true;
    //    }
    //    if (dtUpload.Rows.Count > 0)
    //    {
    //        dvUploadPreview.Visible = true;
    //       // pnlGrid.Visible = true;
    //        //Btnsave.Visible = true;
    //        BtnCancel.Visible = true;
    //        ////GridUpload.Visible= true;
            

    //        //GridUpload.DataSource = dtUpload;
    //        ViewState["UploadData"] = dtUpload;
    //        //GridUpload.DataBind();
    //        //updGrid.Update();

    //    }
    //   // else
    //       // pnlGrid.Visible = false;
    //}
    // #CC01 End
    void ClearForm()
    {
      
        hlnkInvalid.Text = "";
        /* Add End */
       // pnlGrid.Visible = false;
        //updGrid.Update();
    }


    #endregion

}
