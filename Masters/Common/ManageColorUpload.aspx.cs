#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 06-June-2018
 * Description : Uploading(Add/Edit) Color Records from Excel Sheet.
 * ================================================================================================
 * Change Log:
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ====================================================================================================
 * 6-July-2018, #CC01, Rakesh Raj, InvalidData Link Added and Hide Grid
 */
#endregion

using System;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Xml;
using System.IO;

public partial class Masters_Common_ManageColorUpload : PageBase
{
    #region Variables/ Instances

    string strUploadedFileName = string.Empty;
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
            //pnlGrid.Visible = false;
        }
    }

    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "2")
        {
            Response.Redirect("ManageColor.aspx");
        }

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //pnlGrid.Visible = false;
        try
        {
            DataSet objDS = new DataSet();
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);

              if (UploadCheck == 1)
              {
                UploadFile.UploadedFileName = strUploadedFileName;
              
                 isSuccess = UploadFile.uploadValidExcel(ref objDS, "ColorTemplateUpdate");
               
                  //#CC01 
                 if (isSuccess == 1)
                 {
                     InsertData(objDS);
                 }
                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        ////pnlGrid.Visible = false;
                        break;
                    case 2:

                        hlnkInvalid.Visible = true;
                        strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ucMsg.ShowInfo(Resources.Messages.PartialDataUpload);
                        DataView dvError = objDS.Tables[0].DefaultView;
                        dvError.RowFilter = "Error<>''";
                        DataTable dtError = dvError.ToTable();
                        DataSet dsError = new DataSet();
                        dsError.Tables.Add(dtError);
                        PageBase.ExportInExcel(dsError, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";

                        break;
                    case 1:
                        SaveData(objDS.Tables[0]);
                        break;
                   
                }

            }
            else if (UploadCheck == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);

            }
            else if (UploadCheck == 4)
            {
                ucMsg.ShowInfo("File size should be less than " + PageBase.ValidExcelLength + " KB");
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
                 
        }
        catch (Exception ex)
        {   
            ucMsg.ShowError(ex.Message.ToString());
            
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
                Tvp = ObjCommom.GettvpTableColorUpload();
            }


            foreach (DataRow dr in dtSave.Rows)
            {
                DataRow drow = Tvp.NewRow();

                drow[0] = dr["ColorName"].ToString();
                drow[1] = dr["Active"].ToString();
                Tvp.Rows.Add(drow);
            }
            Tvp.AcceptChanges();

            using (CommonData objPrimarySales = new CommonData())
            {
                DataSet dsResult = objPrimarySales.BulkUploadUsingTVP(Tvp, "prcTVPColorUpload", "@tvpColorUpload");

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

    private void InsertData(DataSet dsColor)
    {
        // int mbCount = 0;
        DataTable dtColor = dsColor.Tables[0];
        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (dtColor.Columns.Contains("Error") == false)
        {
            dtColor.Columns.Add(dcError);

        }
        for (int i = 0; i <= dtColor.Rows.Count - 1; i++)
        {
            if (dtColor != null && dtColor.Rows.Count > 0)
            {
               
                    string strColorName = "ColorName = '" + dtColor.Rows[i]["ColorName"].ToString().Trim() + "'";
                    DataRow[] dr2 = dtColor.Select(strColorName);
                    if (dr2.Length > 1)
                    {   
                        if (dtColor.Rows[i]["Error"].ToString() == "" && dtColor.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtColor.Rows[i]["Error"] = "ColorName supplied multiple times.";
                        }
                        else
                            dtColor.Rows[i]["Error"] += ";ColorName supplied multiple times.";

                         isSuccess = 2;
                    }
                    if (string.IsNullOrEmpty(dtColor.Rows[i]["Active"].ToString()))
                    {
                       
                        if (dtColor.Rows[i]["Error"].ToString() == "" && dtColor.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtColor.Rows[i]["Error"] = "Column Active cannot be left blank.";
                        }
                        else
                            dtColor.Rows[i]["Error"] += ";Column Active cannot be left blank.";
                        isSuccess = 2;
                    }

                    if (!string.IsNullOrEmpty(dtColor.Rows[i]["Active"].ToString()) && dtColor.Rows[i]["Active"].ToString().Trim().ToLower() != "yes")
                    {
                        if (dtColor.Rows[i]["Active"].ToString().Trim().ToLower() != "no")
                        {
                           
                            if (dtColor.Rows[i]["Error"].ToString() == "" && dtColor.Rows[i]["Error"].ToString() == string.Empty)
                            {
                                dtColor.Rows[i]["Error"] = "Only Yes or No allowed in Active.";
                            }
                            else
                                dtColor.Rows[i]["Error"] += ";Only Yes or No allowed in Active.";
                            isSuccess = 2;
                        }
                    }

               
            }

        }
      
    }

    void ClearForm()
    { 
        hlnkInvalid.Text = "";
    }

    #endregion

}
