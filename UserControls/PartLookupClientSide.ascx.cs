﻿#region Copyright(c) 2012 Zed-Axis Technologies All rights are reserved
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
 * DD-MMM-YYYY, Name, CCXX, Description 
 * 25-May-2016, Sumit Maurya, #CC01, IsPostBack added to set Saleschannelid and saleschannelcode from page. Path of popuppage corrected in function popupSerials(partCode).
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using BusinessLogics;
using System.Text;

namespace Web.Controls
{
    public partial class PartLookupClientSide : System.Web.UI.UserControl
    {

        public static string strAssets = BussinessLogic.PageBase.strAssets;

        private DataTable _dtMyData = new DataTable();

        public System.Data.DataTable SubmittingTable
        {
            get
            {

                return _dtMyData;
            }
            set
            {
                _dtMyData = value;
            }
        }
        private bool _isBlankDataTable = false;
        public bool IsBlankDataTable
        {
            get
            {
                return _isBlankDataTable;
            }
            set
            {
                _isBlankDataTable = value;
                DoBlank();
            }
        }


        protected void Page_PreRender(object s, EventArgs e)
        {
            if (Page.Request.Form["lbl"] == null)
            {

                _dtMyData = null;
            }
            // DoBlank();

        }

        public string CompanyId { set { hidenCompanyId.Value = value; }
            get
            {
                return hidenCompanyId.Value;
            }
        }

        private string _SalesChannelID = BussinessLogic.PageBase.SalesChanelID.ToString();
        
        public string SalesChannelID
        {
            set
            {
                salesChannelID.Value = value;
                //ViewState["LookupSalesChannelID"] = value;
                _SalesChannelID = value;
            }
            get
            {
                //if (ViewState["LookupSalesChannelID"] != null)
                //    return Convert.ToString(ViewState["LookupSalesChannelID"]);
                //else
                    return _SalesChannelID;
            }
        }

        private string _SalesChannelCode = BussinessLogic.PageBase.SalesChanelCode.ToString();

        public string SalesChannelCode
        {
            set
            {
                _SalesChannelCode = value;
            }
        }


        private void DoBlank()
        {



            _dtMyData = new DataTable();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scriptNullDataTable", "$('#dtParts').dataTable().fnClearTable();document.getElementById('lbl').value='';", true);
            txtPartCode.Text = "";
            txtPartName.Text = "";
            txtQuantity.Text = "";
            txtRate.Text = "";
            txtAmount.Text = "";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            /* #CC01 Add Start */
            if (!IsPostBack)
            {
                /* #CC01 Add End */
                salesChannelID.Value = _SalesChannelID;
                salesChannelCode.Value = _SalesChannelCode;
            } /* #CC01 Added */

            if (Page.Request.Form["lbl"] != null)
            {
                string partCode;
                string batchno;
                string serialNo;
                string Qty,Rate,Amount;
                string skuName;


                string[] rwData = Page.Request.Form["lbl"].ToString().Split('&');


                DataColumn dc = new DataColumn("SKUCode", typeof(string));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("SKUName", typeof(string));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("quantity", typeof(int));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("SerialNo", typeof(string));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("BatchNo", typeof(string));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("Rate", typeof(string));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("Amount", typeof(string));
                _dtMyData.Columns.Add(dc);

                int a,a1, b, c, d, sr, bc;
                a = 0;//Part code
                a1 = 1;//Part name
                b = 2;//Qty
                c = 3;//Rate
                d = 4;//amount
                sr = 5;//serial
                bc = 6;//batch
                for (int i = 0; i < rwData.Length / 7; i++)
                {
                    partCode = Server.UrlDecode(rwData[a].Split('=')[1]);
                    Qty = rwData[b].Split('=')[1];
                    Rate = rwData[c].Split('=')[1];
                    Amount = rwData[d].Split('=')[1];
                    serialNo = Server.UrlDecode(rwData[sr].Split('=')[1]);
                    batchno = rwData[bc].Split('=')[1];
                    skuName = Server.UrlDecode(rwData[a1].Split('=')[1]);

                    DataRow dr = _dtMyData.NewRow();

                    dr["skucode"] = partCode.Replace("%23", "#").Replace("%2B", "+");
                    dr["skuName"] = skuName.Replace("%23", "#");
                    dr["quantity"] = Qty /*Convert.ToInt32(Qty)*/;
                    dr["Rate"] = Rate;
                    dr["Amount"] = Amount;
                    dr["serialno"] = serialNo.Replace("%2F", "").Replace("%23", "#").Replace("%2B", "+").Replace("/", "");
                    dr["batchNo"] = batchno.Replace("%2F", "").Replace("%23", "#").Replace("%2B", "+");
                    _dtMyData.Rows.Add(dr);

                    a = a + 7;
                    b = b + 7;
                    c = c + 7;
                    d = d + 7;
                    sr = sr + 7;
                    bc = bc + 7;
                }

                string str = "sr";
                int P = 0;
                //foreach (DataRow item in _dtMyData.Rows)
                //{
                //    Random r = new Random();
                //    str = r.Next(2000).ToString() + DateTime.Now.ToString();

                //    // str = "s" + str.ToString();
                //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), str, "giCount = 1; oTable = $('#dtParts').dataTable();fnClickAddRowBulk('" + item["skucode"].ToString() + "','" + item["quantity"].ToString() + "','" + item["serialNo"].ToString() + "','" + item["batchno"].ToString() + "');", true);
                //    Thread.Sleep(200);
                //    b++;
                //}



                if (!IsBlankDataTable)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow item in _dtMyData.Rows)
                    {

                        sb.Append("<tr>");
                        sb.Append("<td><span>" + item["skucode"].ToString() + "</span>");
                        sb.Append("<input type=hidden name=partcode value=" + item["skucode"].ToString() + " />");
                        sb.Append("</td>");

                        sb.Append("<td><span>" + item["skuName"].ToString() + "</span>");
                        sb.Append("<input type=hidden name=skuname value=" + item["skuName"].ToString() + " />");
                        sb.Append("</td>");


                        sb.Append("<td><span>");
                        sb.Append(item["quantity"].ToString());
                        sb.Append("</span><input type=hidden name=serialno value=" + item["quantity"].ToString() + " /></td>");

                        sb.Append("<td><span>");
                        sb.Append(item["rate"].ToString());
                        sb.Append("</span><input type=hidden name=rate value=" + item["rate"].ToString() + " /></td>");

                        sb.Append("<td><span>");
                        sb.Append(item["amount"].ToString());
                        sb.Append("</span><input type=hidden name=amount value=" + item["amount"].ToString() + " /></td>");

                        sb.Append("<td><span>");
                        sb.Append(item["serialNo"].ToString());
                        sb.Append("</span><input type=hidden name=serialno value=" + item["serialNo"].ToString() + " /></td>");

                        sb.Append("<td><span>");
                        sb.Append(item["batchno"].ToString());
                        sb.Append("</span><input type=hidden name=batchno value=" + item["batchno"].ToString() + " /></td></tr>");


                    }

                    lit.Text = sb.ToString();
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), this.Page.ClientID, " txtPartTextChanged();", true);

            }

        }


    }



}