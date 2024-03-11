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
namespace Web.Controls
{
    public partial class BatchLookupClientSide : System.Web.UI.UserControl
    {

        DataTable dtSerials = new DataTable();
        string partCode = string.Empty;
        string SalesChannelID = "0";
        string SalesChannelCode = string.Empty;
        string TypeID = "0";
        string Mode = "1";  /* 1: for sales, 2 for adjust -->  used fro qunatity in or out */ 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                partCode = Server.HtmlDecode(Request.QueryString["prtcode"].ToString());
                partCode = partCode.Replace("%2F", "#").Replace("%2B", "+").Replace("%20", " ");
                //partCode = Request.QueryString["prtcode"].ToString().Replace("%2F", "#").Replace("%2B", "+");
                SalesChannelID = Request.QueryString["SalesChannelID"].ToString();
                SalesChannelCode = Request.QueryString["SalesChannelCode"].ToString();
                TypeID = Request.QueryString["TypeID"].ToString();
                Mode = Request.QueryString["Mode"].ToString();

                hdnMode.Value = Mode;


                lblPrtCode.Text = partCode;

                using (DataAccess.ProductData obj = new DataAccess.ProductData())
                {

                    obj.SalesChannelID = Convert.ToInt32(SalesChannelID);
                    obj.SalesChannelCode = SalesChannelCode;
                    obj.TypeID = Convert.ToByte(TypeID);
                    // obj.SalesChannelID = 83;
                    obj.SKUCode = partCode;
                    dtSerials = obj.GetBatchNosByCode();
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
                if (dt.Rows.Count > 5000)
                {
                    ucMsg.ShowInfo("There are to much records so, Enter serials in input box.");
                    return;
                }
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
                        sb.Append("<input type=text name=qty maxlength='6'  />");
                        sb.Append("</td>");
                      

                        sb.Append("</tr>");

                    }

            ltData.Text = sb.ToString();


        }
    }
}
