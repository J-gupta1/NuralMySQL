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

    public partial class BatchLookupClientSideForReturn : System.Web.UI.UserControl
    {

        DataTable dtSerials = new DataTable();
        string partCode = string.Empty;
        string SalesChannelID = "0";
        string SalesChannelCode = string.Empty;
        string strInvoiceNumber = string.Empty,strInvoiceDate=string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                partCode = Request.QueryString["prtcode"].ToString();
                SalesChannelID = Request.QueryString["SalesChannelID"].ToString();
                SalesChannelCode = Request.QueryString["SalesChannelCode"].ToString();
                strInvoiceNumber=Request.QueryString["InvoiceNumber"].ToString();
                strInvoiceDate=Request.QueryString["InvoiceDate"].ToString();
                lblTypeID.Text = Request.QueryString["TypeID"].ToString();
                lblPrtCode.Text = partCode;
                lblInvoiceNumber.Text = strInvoiceNumber;
                lblInvoiceDate.Text = strInvoiceDate;

                using (DataAccess.ProductData obj = new DataAccess.ProductData())
                {

                    obj.SalesChannelID = Convert.ToInt32(SalesChannelID);
                    obj.SalesChannelCode = SalesChannelCode;
                    obj.SKUCode = partCode;
                    obj.InvoiceDate=Convert.ToDateTime(strInvoiceDate);
                    obj.InvoiceNumber=strInvoiceNumber;
                    obj.ReturnFromSalesChannelID=Convert.ToInt32(Session["ReturnFromSalesChannelID"]);
                    obj.Value = Convert.ToInt16(lblTypeID.Text);
                    dtSerials = obj.GetBatchNosByCodeForReturn();
                    RenderBatches(dtSerials);
                }

            }

            int i = 0;
            foreach (DataRow item in dtSerials.Rows)
            {
                //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), i.ToString(), "fnClickAddRow('" + partCode + "','','','" + item["batchNo"].ToString() + "','" + item["stockQuantity"].ToString() + "');", true);
                i++;
            }
        }


        void RenderBatches(DataTable MyData)
        {

            StringBuilder sb = new StringBuilder();
            DataTable dt = MyData;
            int i = 0;
            if (dt != null)
                if (dt.Rows.Count > 0)
                    foreach (DataRow dr in dt.Rows)
                    {

                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append("<span>" + dr["batchno"] + " <span/>");
                        sb.Append("</td>");
                        sb.Append("<td>");
                        sb.Append("<span>" + dr["stockquantity"] + " <span/>");
                        sb.Append("</td>");
                        sb.Append("<td>");
                        sb.Append("<input type=text name=qty   />");
                        sb.Append("</td>");


                        sb.Append("</tr>");

                    }

            ltData.Text = sb.ToString();


        }
    }
