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
* Created By    : Pankaj Dhingra
* Module        : Sales
* Description   : used by jquery(Total Reference from "SerialLookupClientSide.ascx" this UC)
* Table Name    : 
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          -------------------------------------------------------------   
 * 22 Jan 2015, Karam Chand Sharma, #CC01, In grid detail binding pass stock bin type paramenter in object. 

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
    public partial class SerialLookupClientSideForReturn : System.Web.UI.UserControl
    {

        DataTable dtSerials = new DataTable();
        string partCode = string.Empty, InvoiceNumber = string.Empty, InvoiceDate = string.Empty;
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
                //hdnSkuCode.Value = Request.QueryString["prtcode"].ToString();
                hdnSkuCode.Value = Server.UrlDecode(Request.QueryString["prtcode"].ToString()).Replace(" ", "+");
                hdnInvoiceNumber.Value = Request.QueryString["InvoiceNumber"].ToString();
                hdnInvoiceDate.Value = Request.QueryString["InvoiceDate"].ToString();
                hdnTypeID.Value = Request.QueryString["TypeID"].ToString();

                litSerial_MaxL.Text = BussinessLogic.PageBase.SerialNoLength_Max.ToString();
                litSerial_MinL.Text = BussinessLogic.PageBase.SerialNoLength_Min.ToString();
                hdnStockBinType.Value = Request.QueryString["StockBinType"].ToString();
                hdnSalesChannelIDReturn.Value = Session["ReturnFromSalesChannelID"].ToString();
            }

        }

        private void BindGrid()
        {
            partCode = Server.UrlDecode(Request.QueryString["prtcode"].ToString()).Replace(" ", "+");
            SalesChannelID = Request.QueryString["SalesChannelID"].ToString();
            SalesChannelCode = Request.QueryString["SalesChannelCode"].ToString();
            InvoiceNumber = Server.UrlDecode(Request.QueryString["InvoiceNumber"].ToString());
            InvoiceDate = Request.QueryString["InvoiceDate"].ToString();
            using (DataAccess.ProductData obj = new DataAccess.ProductData())
            {

                obj.SalesChannelID = Convert.ToInt32(SalesChannelID);
                obj.SalesChannelCode = SalesChannelCode;
                obj.SKUCode = partCode;
                obj.InvoiceNumber = InvoiceNumber;
                obj.InvoiceDate = Convert.ToDateTime(InvoiceDate);
                obj.ReturnFromSalesChannelID = Convert.ToInt32(Session["ReturnFromSalesChannelID"]);
                obj.Value = Convert.ToInt16(hdnTypeID.Value);
                obj.StockBinType = Convert.ToInt16(hdnStockBinType.Value);/* #CC01 ADDED*/
                dtSerials = obj.GetSerialNosByCodeForReturn();
                LoadData(dtSerials);
            }
        }

        private void LoadData(DataTable MyData)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = MyData;
            if (dt != null)
                if (dt.Rows.Count > 0)
                    foreach (DataRow dr in dt.Rows)
                    {

                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<input type=checkbox  class=checkbox />");
                        sb.Append("</td>");
                        sb.Append("<td><span>");
                        sb.Append((dr["serialno"]));
                        sb.Append("<span><input type=hidden name=serialno value=" + (dr["serialno"]) + " /></td>");
                        sb.Append("</tr>");

                    }

            ltData.Text = sb.ToString();
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
