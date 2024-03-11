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

public partial class Masters_Common_ManageNotification : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //BindISP();
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }

    public void BindISP()
    {
        try
        {
            tvISP.Nodes.Clear();
            using (CommonMaster objComman = new CommonMaster())
            {
                DataSet ds = objComman.GetISPForNotification();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tvISP.Nodes.Add(new TreeNode(
                        ds.Tables[0].Rows[i]["NDName"].ToString(), ds.Tables[0].Rows[i]["NDId"].ToString()));

                        DataView dv = ds.Tables[1].DefaultView;
                        dv.RowFilter = "NDid=" + ds.Tables[0].Rows[i]["NDid"].ToString();
                        DataTable dtRSP = dv.ToTable(true, "RDSId", "RDSName");
                        for (int j = 0; j < dtRSP.Rows.Count; j++)
                        {
                            tvISP.Nodes[i].ChildNodes.Add(new TreeNode(
                            dtRSP.Rows[j]["RDSName"].ToString(), dtRSP.Rows[j]["RDSId"].ToString()));

                            DataView dvState = ds.Tables[1].DefaultView;
                            dvState.RowFilter = "NDid=" + ds.Tables[0].Rows[i]["NDid"].ToString() + " AND RDSId=" + dtRSP.Rows[j]["RDSId"].ToString();
                            DataTable dtState = dvState.ToTable(true, "StateId", "StateName");
                            for (int k = 0; k < dtState.Rows.Count; k++)
                            {
                                tvISP.Nodes[i].ChildNodes[j].ChildNodes.Add(new TreeNode(
                                dtState.Rows[k]["StateName"].ToString(), dtState.Rows[k]["StateId"].ToString()));
                                DataView dvCity = ds.Tables[1].DefaultView;
                                dvCity.RowFilter = "NDid=" + ds.Tables[0].Rows[i]["NDid"].ToString() + " AND RDSId=" + dtRSP.Rows[j]["RDSId"].ToString() + "AND StateId=" + dtState.Rows[k]["StateId"].ToString();
                                DataTable dtCity = dvCity.ToTable(true, "CityId", "CityName");
                                for (int l = 0; l < dtCity.Rows.Count; l++)
                                {
                                    tvISP.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Add(new TreeNode(
                                    dtCity.Rows[l]["CityName"].ToString(), dtCity.Rows[l]["CityId"].ToString()));

                                    DataView dvISP = ds.Tables[1].DefaultView;
                                    dvISP.RowFilter = "NDid=" + ds.Tables[0].Rows[i]["NDid"].ToString() + " AND RDSId=" + dtRSP.Rows[j]["RDSId"].ToString() + "AND StateId=" + dtState.Rows[k]["StateId"].ToString() + "AND CityId=" + dtCity.Rows[l]["CityId"].ToString();
                                    DataTable dtISP = dvISP.ToTable(true, "ISPId", "ISPName");

                                    for (int m = 0; m < dtISP.Rows.Count; m++)
                                    {
                                        tvISP.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes.Add(new TreeNode(
                                        dtISP.Rows[m]["ISPName"].ToString(), dtISP.Rows[m]["ISPId"].ToString()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());

        }
    }
    public void DisableControl()
    {
        lblCount.Text = "Total Words = " + txtNotificationText.Text.Length.ToString() + " / 500";

        divTv.Visible = false;
        divtxt.Visible = false;
        txtISP.Text = "";
        //BindISP();
        dvRspLable.Visible = false;
        if (rblAccessType.SelectedValue == "2")
        {
            divTv.Visible = true;
        }
        else if (rblAccessType.SelectedValue == "3")
        {
            divtxt.Visible = true;
            dvRspLable.Visible = true;
        }
    }

    private DataTable GetISP(string strPo)
    {
        DataTable dt = new DataTable();
        string str = strPo.Replace("\r\n", ",");
        dt.Columns.Add("ReferenceID", typeof(int));
        dt.Columns.Add("ReferenceType", typeof(int));
        dt.Columns.Add("ParentID", typeof(int));
        dt.Columns.Add("RetailerCode", typeof(string));

        foreach (var strItem in str.Split(','))
        {
            dt.Rows.Add(0, 0, 0, strItem);
        }
        dt.AcceptChanges();
        return dt;
    }

    public void ClearControls()
    {
        rblAccessType.SelectedValue = "1";
        txtNotificationText.Text = "";
        chkActive.Checked = true;
        DisableControl();
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            lblCount.Text = "Total Words = " + txtNotificationText.Text.Length.ToString() + " / 500";
            if (txtNotificationText.Text == "")
            {
                ucMsg.ShowInfo("Please enter notification.");
                txtNotificationText.Focus();
                return;
            }
            else if (txtNotificationText.Text.Length > 500)
            {
                ucMsg.ShowInfo("Notification Text should be less than 500 words.");
                txtNotificationText.Focus();
                return;
            }

            DataSet dsResultISP = new DataSet();
            using (CommonMaster objCommonMaster = new CommonMaster())
            {
                objCommonMaster.NotificationText = txtNotificationText.Text;
                objCommonMaster.NoteStatus = chkActive.Checked;
                objCommonMaster.NoteCreatedBy = PageBase.UserId;
                objCommonMaster.CallingMode = 1;
                if (rblAccessType.SelectedValue == "1")
                {
                    objCommonMaster.AccessType = 1;
                    objCommonMaster.NotificationLevel = string.Empty;
                }
                else if (rblAccessType.SelectedValue == "2")
                {
                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("ReferenceID", typeof(int));
                    dtResult.Columns.Add("ReferenceType", typeof(int));
                    dtResult.Columns.Add("ParentID", typeof(int));
                    dtResult.Columns.Add("RetailerCode", typeof(string));
                    bool NDS = false, RDS = false, State = false, City = false, IsISPFound = false; ;
                    if (tvISP.Nodes.Count > 0)
                    {
                        for (int i_NDS = 0; i_NDS < tvISP.Nodes.Count; i_NDS++)
                        {
                            NDS = tvISP.Nodes[i_NDS].Checked;
                            for (int j_RDS = 0; j_RDS < tvISP.Nodes[i_NDS].ChildNodes.Count; j_RDS++)
                            {
                                RDS = tvISP.Nodes[i_NDS].ChildNodes[j_RDS].Checked;
                                for (int k_State = 0; k_State < tvISP.Nodes[i_NDS].ChildNodes[j_RDS].ChildNodes.Count; k_State++)
                                {
                                    State = tvISP.Nodes[i_NDS].ChildNodes[j_RDS].ChildNodes[k_State].Checked;
                                    for (int l_City = 0; l_City < tvISP.Nodes[i_NDS].ChildNodes[j_RDS].ChildNodes[k_State].ChildNodes.Count; l_City++)
                                    {
                                        City = tvISP.Nodes[i_NDS].ChildNodes[j_RDS].ChildNodes[k_State].ChildNodes[l_City].Checked;
                                        for (int m_ISP = 0; m_ISP < tvISP.Nodes[i_NDS].ChildNodes[j_RDS].ChildNodes[k_State].ChildNodes[l_City].ChildNodes.Count; m_ISP++)
                                        {
                                            if (tvISP.Nodes[i_NDS].ChildNodes[j_RDS].ChildNodes[k_State].ChildNodes[l_City].ChildNodes[m_ISP].Checked)
                                            {
                                                dtResult.Rows.Add(Convert.ToInt16(tvISP.Nodes[i_NDS].ChildNodes[j_RDS].ChildNodes[k_State].ChildNodes[l_City].ChildNodes[m_ISP].Value), 6, 0, "");
                                            }
                                            else
                                            {
                                                if (City == true)
                                                {
                                                    dtResult.Rows.Add(Convert.ToInt16(tvISP.Nodes[i_NDS].ChildNodes[j_RDS].ChildNodes[k_State].ChildNodes[l_City].Value), 5, Convert.ToInt16(tvISP.Nodes[i_NDS].ChildNodes[j_RDS].Value), "");
                                                    City = false;
                                                    //IsISPFound = true;
                                                }
                                                else if (State == true)
                                                {
                                                    dtResult.Rows.Add(Convert.ToInt16(tvISP.Nodes[i_NDS].ChildNodes[j_RDS].ChildNodes[k_State].ChildNodes[l_City].Value), 4, Convert.ToInt16(tvISP.Nodes[i_NDS].ChildNodes[j_RDS].Value), "");
                                                    State = false;
                                                    //IsISPFound = true;
                                                }
                                                else if (RDS == true)
                                                {
                                                    dtResult.Rows.Add(Convert.ToInt16(tvISP.Nodes[i_NDS].ChildNodes[j_RDS].Value), 3, 0, "");
                                                    RDS = false;
                                                    //IsISPFound = true;
                                                }
                                                else if (NDS == true)
                                                {
                                                    dtResult.Rows.Add(Convert.ToInt16(tvISP.Nodes[i_NDS].Value), 2, 0, "");
                                                    NDS = false;
                                                    IsISPFound = true;
                                                }
                                            }
                                        }
                                        if (IsISPFound == true)
                                            break;
                                    }
                                    if (IsISPFound == true)
                                        break;
                                }
                                if (IsISPFound == true)
                                    break;
                            }

                        }
                        dsResultISP.Tables.Add(dtResult);
                        dsResultISP.GetXml();
                        if (dsResultISP.Tables[0].Rows.Count > 0)
                        {
                            objCommonMaster.AccessType = 2;
                            objCommonMaster.NotificationLevel = dsResultISP.GetXml();
                        }
                        else
                        {
                            ucMsg.ShowInfo("Please select ISP.");
                            return;
                        }
                    }
                }
                else if (rblAccessType.SelectedValue == "3")
                {

                    if (txtISP.Text == "")
                    {
                        ucMsg.ShowInfo("Please enter ISP code.");
                        txtISP.Focus();
                        return;
                    }
                    DataTable dtResult = GetISP(txtISP.Text);
                    if (dtResult.Rows.Count > 0)
                    {
                        dsResultISP.Tables.Add(dtResult);
                        if (dsResultISP.Tables[0].Rows.Count > 0)
                        {
                            objCommonMaster.AccessType = 3;
                            objCommonMaster.NotificationLevel = dsResultISP.GetXml();
                        }
                    }
                }

                int Result = objCommonMaster.SaveNotification();
                if (objCommonMaster.Out_Param == 0)
                {
                    ucMsg.ShowSuccess("Notification saved successfully.");
                    ClearControls();
                }
                else
                {
                    if (objCommonMaster.Out_Param == 1)
                    {
                        if (objCommonMaster.XML_Error != "")
                        {
                            ucMsg.XmlErrorSource = objCommonMaster.XML_Error.ToString();
                        }
                        else
                        {
                            ucMsg.ShowError("Record not saved.");
                        }
                    }
                }             
                
                updMsg1.Update();
            }

        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }
    protected void rblAccessType_SelectedIndexChanged(object sender, EventArgs e)
    {        
        txtNotificationText.Text = "";
        updMsg1.Update();
        ucMsg.Visible = false;
        DisableControl();
      
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageNotification.aspx", false);
    }

}
