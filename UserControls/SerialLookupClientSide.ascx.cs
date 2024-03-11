#region Copyright(c) 2012 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2012 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By    : Amit Agarwal
* Module        : Sales
* Description   : used by jquery
* Table Name    : 
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          -------------------------------------------------------------  
27-Jul-2016, Sumit Maurya, #CC01, New hidden field added to store StockBinTypeID and supplied to validate serial numbers.
 * 01 Aug 2016, Karam Chand Sharma, #CC02, Pass stock bin type id when they select check box
 ====================================================================================================
*/

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;
namespace Web.Controls
{
    public partial class SerialLookupClientSide : System.Web.UI.UserControl
    {

        DataTable dtSerials = new DataTable();
        string partCode = string.Empty;
        string SalesChannelID = "0";
        string SalesChannelCode = string.Empty;
        string TypeID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                chk_CheckedChanged(chk, null);
                hdnSalesChannelCode.Value = Request.QueryString["SalesChannelCode"].ToString();
                hdnSalesChannelID.Value = Request.QueryString["SalesChannelID"].ToString();
                hdnSkuCode.Value = Server.UrlDecode(Request.QueryString["prtcode"].ToString()).Replace(" ", "+");
                //hdnSkuCode.Value = Request.QueryString["prtcode"].ToString().Replace("%2F", "#").Replace("%2B", "+");
                hdnTypeID.Value = Request.QueryString["TypeID"].ToString();
                hdnMode.Value = Request.QueryString["Mode"].ToString();

                litSerial_MaxL.Text = BussinessLogic.PageBase.SerialNoLength_Max.ToString();
                litSerial_MinL.Text = BussinessLogic.PageBase.SerialNoLength_Min.ToString();

                hdnSerialNoLengthMin.Value = BussinessLogic.PageBase.SerialNoLength_Min.ToString();
                hdnSerialNoLengthMax.Value = BussinessLogic.PageBase.SerialNoLength_Max.ToString();


                //  BindGrid();
                /*#CC01 Add Start*/
                //if (Request.QueryString["StockBinTypeID"].ToString() != "")
                //{
                    hdnStockBinTypeID.Value ="2"/* Request.QueryString["StockBinTypeID"].ToString()*/;
                    hdnStockBinTypeCode.Value ="COFGD" /*Request.QueryString["StockBinTypeCode"].ToString()*/;
                //}
                /*#CC01 Add End*/

            }

            int i = 0;
            foreach (DataRow item in dtSerials.Rows)
            {
                //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), i.ToString(), "fnClickAddRow('" + partCode + "','1','" + item["serialNo"].ToString() + "','');window.onunload = null;", true);
                i++;
            }

            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), i.ToString(), "GetXmlDocument('" + partCode + "','" + SalesChannelID + "','"+SalesChannelCode+"');", true);


        }

        private void BindGrid()
        {
            partCode = Server.UrlDecode(Request.QueryString["prtcode"].ToString()).Replace(" ", "+");
            SalesChannelID = Request.QueryString["SalesChannelID"].ToString();
            SalesChannelCode = Request.QueryString["SalesChannelCode"].ToString();
            TypeID = Request.QueryString["TypeID"].ToString();


            // lblPrtCode.Text = partCode;

            using (DataAccess.ProductData obj = new DataAccess.ProductData())
            {

                obj.SalesChannelID = Convert.ToInt32(SalesChannelID);
                obj.SalesChannelCode = SalesChannelCode;
                obj.TypeID = Convert.ToByte(TypeID);
                obj.StockBinType = Convert.ToInt16(hdnStockBinTypeID.Value);/*#CC02 ADDED*/
                // obj.SalesChannelID = 82;
                obj.SKUCode = partCode;
                dtSerials = obj.GetSerialNosByCode();
                LoadData(dtSerials);
            }
        }

        private void LoadData(DataTable MyData)
        {


            StringBuilder sb = new StringBuilder();
            DataTable dt = MyData;

            if (dt != null)
                if (dt.Rows.Count > 5000)
                {
                    ucMsg.ShowInfo("There are to much records so, Enter serials in input box.");
                    chk.Checked = true;
                    chk_CheckedChanged(chk, null);
                    return;
                }




            if (dt.Rows.Count > 0)
            {


                foreach (DataRow dr in dt.Rows)
                {

                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append("<input type=checkbox id=chk class=checkbox />");
                    sb.Append("</td>");
                    sb.Append("<td><span>");
                    sb.Append((dr["serialno"]));
                    sb.Append("<span><input type=hidden name=serialno value=" + (dr["serialno"]) + " /></td>");
                    //sb.Append("<td>");
                    //sb.Append((dr["vchMiddleName"]));
                    //sb.Append("</td>");
                    //sb.Append("<td>");
                    //sb.Append((dr["vchLastName"]));
                    //sb.Append("</td>");

                    sb.Append("</tr>");

                }

            }

            ltData.Text = sb.ToString();

            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sdf", "document.getElementById('spn').innerText='"+sb.ToString()+ "';", true);
            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sdf", "getElementById('spn').innerText='" + sb.ToString() + "';", true);

        }
        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            if (chk.Checked)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scriptVis", "changeDisplay('false');", true);
            }
            else
            {
                BindGrid();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scriptVis", "changeDisplay('true');", true);
            }

        }
    }
}
