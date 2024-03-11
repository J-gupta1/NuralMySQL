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
 * 26-Jul-2016, Sumit Maurya, #CC01, New dropdown added StockBintype.
 * 28-Jul-201, Sumit Maurya, #CC02, New function added to hide ucmsg on the change events of controls by javascript.
 * 05-Aug-2016, Sumit Maurya, #CC03, instead of "innerText", "value" is used because innerText was not working for Chrome.
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
using System.Text;
/* #CC01 Add Start */
using BusinessLogics;
using DataAccess;
using BussinessLogic;
/* #CC01 Add End */
namespace Web.Controls
{
    public partial class PartLookupClientSideOpeningStock : System.Web.UI.UserControl
    {

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
            }
        }



        protected void Page_PreRender(object s, EventArgs e)
        {

            if (Page.Request.Form["lbl"] == null)
            {

                _dtMyData = null;
            }
            if (IsBlankDataTable)
            {
                _dtMyData = null;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scriptNullDataTable", "$('#dtParts').dataTable().fnClearTable();", true);
            }



        }


        private string _SalesChannelID = BussinessLogic.PageBase.SalesChanelID.ToString();

        public string SalesChannelID
        {
            set
            {
                _SalesChannelID = value;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            /* #CC01 Add Start */
            if (!IsPostBack)
            {
                FillStockBinType();
                txtPartCode.Enabled = false;
            }
            /* #CC01 Add End */
            salesChannelID.Value = _SalesChannelID;
            salesChannelCode.Value = _SalesChannelCode;
            if (!IsPostBack)
            {
                hdnSerialNoLengthMin.Value = BussinessLogic.PageBase.SerialNoLength_Min.ToString();
                hdnSerialNoLengthMax.Value = BussinessLogic.PageBase.SerialNoLength_Max.ToString();
                hdnBatchNoLengthMin.Value = BussinessLogic.PageBase.BatchNoLength_Min.ToString();
                hdnBatchNoLengthMax.Value = BussinessLogic.PageBase.BatchNoLength_Max.ToString();
                litSerial_MaxL.Text = BussinessLogic.PageBase.SerialNoLength_Max.ToString();
                litSerial_MinL.Text = BussinessLogic.PageBase.SerialNoLength_Min.ToString();
                litBatch_MaxL.Text = BussinessLogic.PageBase.BatchNoLength_Max.ToString();
                litBatch_MinL.Text = BussinessLogic.PageBase.BatchNoLength_Min.ToString();
            }

            if (Page.Request.Form["lbl"] != null)
            {




                string[] rwData = Page.Request.Form["lbl"].ToString().Split('&');





                int a, b, c, d, ee, f;/* #CC01 "f" added */
                a = 0;
                b = 1;
                c = 2;
                d = 3;
                ee = 4;
                f = 5; /* #CC01 Added */
                string partCode;
                string batchno;
                string serialNo;
                string Qty;
                string skuname;
                string stockTypecode; /* #CC01 Added */




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

                dc = new DataColumn("StockType", typeof(string));
                _dtMyData.Columns.Add(dc);

                for (int i = 0; i < rwData.Length / 6; i++) /* #CC01 instead of 5 it is divided by 6 */
                {
                    partCode = Server.UrlDecode(rwData[a].Split('=')[1]);
                    skuname = Server.UrlDecode(rwData[b].Split('=')[1]);
                    Qty = rwData[c].Split('=')[1];
                    serialNo = Server.UrlDecode(rwData[d].Split('=')[1]);
                    batchno = rwData[ee].Split('=')[1];
                    stockTypecode = Server.UrlDecode(rwData[f].Split('=')[1]);/* #CC01 Added */

                    DataRow dr = _dtMyData.NewRow();

                    dr["skucode"] = partCode.Replace("%23", "#").Replace("%2B", "+");
                    dr["quantity"] = Convert.ToInt32(Qty);
                    dr["serialno"] = serialNo.Replace("%2F", "").Replace("%23", "#").Replace("%2B", "+").Replace("/", "");
                    dr["batchNo"] = batchno.Replace("%2F", "").Replace("%23", "#").Replace("%2B", "+");
                    dr["skuname"] = skuname.Replace("%23", "#").Replace("%2B", "+");
                    dr["StockType"] = stockTypecode; /* #CC01 Added */
                    _dtMyData.Rows.Add(dr);
                    /*
                     * #CC01 Comment Start 
                    a = a + 5;
                    b = b + 5;
                    c = c + 5;
                    d = d + 5;
                    ee = ee + 5;
                     #CC01 Comment End */
                    /* #CC01 Add Start */
                    a = a + 6;
                    b = b + 6;
                    c = c + 6;
                    d = d + 6;
                    ee = ee + 6;
                    f = f + 6;
                    /* #CC01 Add End */

                }


                //foreach (DataRow item in _dtMyData.Rows)
                //{
                //    Random r = new Random();
                //    str = r.Next(2000).ToString() + DateTime.Now.ToString();

                //    // str = "s" + str.ToString();
                //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), str, "giCount = 1; oTable = $('#dtParts').dataTable();fnClickAddRow('" + item["skucode"].ToString() + "','" + item["quantity"].ToString() + "','" + item["serialNo"].ToString() + "','" + item["batchno"].ToString() + "','1');", true);
                //    Thread.Sleep(200);
                //    b++;
                //}

                StringBuilder sb = new StringBuilder();
                foreach (DataRow item in _dtMyData.Rows)
                {

                    sb.Append("<tr>");
                    sb.Append("<td><span>" + item["skucode"].ToString() + "</span>");
                    sb.Append("<input type=hidden name=partcode value=" + item["skucode"].ToString() + " />");
                    sb.Append("</td>");

                    sb.Append("<td><span>" + item["skucode"].ToString() + "</span>");
                    sb.Append("<input type=hidden name=skuname value=" + item["skuname"].ToString() + " />");
                    sb.Append("</td>");

                    sb.Append("<td><span>");
                    sb.Append(item["quantity"].ToString());
                    sb.Append("</span><input type=hidden name=qty value=" + item["quantity"].ToString() + " /></td>");

                    sb.Append("<td><span>");
                    sb.Append(item["serialNo"].ToString());
                    sb.Append("</span><input type=hidden name=serialno value='" + item["serialNo"].ToString() + "' /></td>");

                    sb.Append("<td><span>");
                    sb.Append(item["batchno"].ToString());
                    /*sb.Append("</span><input type=hidden name=batchno value=" + item["batchno"].ToString() + " /></td></tr>"); #CC01 Commented */
                    sb.Append("</span><input type=hidden name=batchno value=" + item["batchno"].ToString() + " /></td>");

                    /*#CC01 Add Start */
                    sb.Append("<td><span>");
                    sb.Append(item["stocktype"].ToString());
                    sb.Append("</span><input type=hidden name=stockbintype value=" + item["stocktype"].ToString() + " /></td></tr>");
                    /*#CC01 Add End */

                }

                lit.Text = sb.ToString();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), this.Page.ClientID, " txtPartTextChanged();document.getElementById('lbl').value='" + Page.Request.Form["lbl"].ToString() + "';", true);

            }
        }
        /* #CC01 Add Start */
        void FillStockBinType()
        {
            try
            {
                String[] StrCol;
                DataSet dsStockBinType = new DataSet();
                using (SalesmanData ObjSalesman = new SalesmanData())
                {
                    ObjSalesman.Type = EnumData.eSearchConditions.Active;
                    ObjSalesman.SalesChannelID = BussinessLogic.PageBase.SalesChanelID;
                    ObjSalesman.MapwithRetailer = 1;
                    ObjSalesman.CompanyId = PageBase.ClientId;
                    dsStockBinType = ObjSalesman.GetSalesmanAndStockBinTypeInfo();
                    StrCol = new String[] { "StockBinTypeMasterID", "StockBinTypeDescWithCode" };
                    BussinessLogic.PageBase.DropdownBinding(ref ddlStockBinType, dsStockBinType.Tables[1], StrCol);

                }
            }
            catch (Exception ex)
            {

            }
        }


        /* #CC01 Add End */
    }




    //"<span>" + partCode + " <span/><input type=hidden name=partcode value=" + partCode + " />",
    //                    "<span>" + qty + " <span/><input type=hidden name=qty value=" + qty + " />",
    //                    "<span>" + serialno + " <span/><input type=hidden name=serialno value=" + serialno + " />",
    //                        "<span>" + batchNo + " <span/><input type=hidden name=batchno value=" + batchNo + " />"]);

    //StringBuilder sb = new StringBuilder();
    //       DataTable dt = MyData;
    //       int i = 0;
    //       if (dt != null)
    //           if (dt.Rows.Count > 0)
    //               foreach (DataRow dr in dt.Rows)
    //               {

    //                   sb.Append("<tr>");
    //                   sb.Append("<td>");
    //                   sb.Append("<input type=checkbox  class=checkbox />");
    //                   sb.Append("</td>");
    //                   sb.Append("<td><span>");
    //                   sb.Append((dr["serialno"]));
    //                   sb.Append("<span><input type=hidden name=serialno value=" + (dr["serialno"]) + " /></td>");
    //                   //sb.Append("<td>");
    //                   //sb.Append((dr["vchMiddleName"]));
    //                   //sb.Append("</td>");
    //                   //sb.Append("<td>");
    //                   //sb.Append((dr["vchLastName"]));
    //                   //sb.Append("</td>");

    //                   sb.Append("</tr>");

    //               }

    //        ltData.Text = sb.ToString();

}
