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
/*
 * 22 Jan 2015, Karam Chand Sharma, #CC01, In pass stock bin type in query string in javascript.
 */
namespace Web.Controls
{
    public partial class UserControls_PartLookupClientSideForReturn : System.Web.UI.UserControl
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
        string _strInvoiceNumber=string.Empty;
        DateTime? _dtInvoiceDate;

        public string InvoiceNumber
        {
            set
            {
                _strInvoiceNumber = value;
                hdnInvoiceNumber.Value = Convert.ToString(_strInvoiceNumber);
                ViewState["InvoiceNumber"] = _strInvoiceNumber;
            }
            get
            {
                if (ViewState["InvoiceNumber"] != null)
                    return Convert.ToString(ViewState["InvoiceNumber"]);
                return null;
            }
        }

         public DateTime? InvoiceDate
        {
            set
            {
                _dtInvoiceDate = value;
                hdnInvoiceDate.Value = Convert.ToString(_dtInvoiceDate);
                ViewState["InvoiceDate"] = _dtInvoiceDate;
            }
            get
            {
                if (ViewState["InvoiceDate"] != null)
                    return Convert.ToDateTime(ViewState["InvoiceDate"]);
                return null;
            }
        }
         string _strReturnFromSalesChannelID = string.Empty;
         public string ReturnFromSalesChannelID
         {
             set
             {
                 _strReturnFromSalesChannelID = value;
                 AutoCompleteExtender1.ContextKey = _strReturnFromSalesChannelID;
                 ViewState["ReturnFromSalesChannelID"] = _strReturnFromSalesChannelID;
             }
             get
             {
                 if (ViewState["ReturnFromSalesChannelID"] != null)
                     return Convert.ToString(ViewState["ReturnFromSalesChannelID"]);
                 return null;
             }
         }


         Int16 _intSalesTypeID;
         public Int16 SalesTypeID
         {
             set
             {
                 _intSalesTypeID = value;
                 hdnSalesType.Value = Convert.ToString(_intSalesTypeID);
                 ViewState["SalesTypeID"] = _intSalesTypeID;
             }
             get
             {
                 if (ViewState["SalesTypeID"] != null)
                     return Convert.ToInt16(ViewState["SalesTypeID"]);
                 return 0;
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


        private void DoBlank()
        {



            _dtMyData = new DataTable();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scriptNullDataTable", "$('#dtParts').dataTable().fnClearTable();document.getElementById('lbl').value='';", true);
            txtPartCode.Text = "";
            txtQuantity.Text = "";
            txtInvoiceNo.Text = "";
            ucInvoiceDate.Date = "";


        }
        protected void Page_Load(object sender, EventArgs e)
        {

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



                int a, b, c, d,z,f;
                a = 0;
                b = 1;
                c = 2;
                d = 3;
                z = 4;
                f = 5;

                string partCode;
                string batchno;
                string serialNo;
                string Qty;
                string InvoiceNumber;
                string InvoiceDate;


                string[] rwData = Page.Request.Form["lbl"].ToString().Split('&');
                

                DataColumn dc = new DataColumn("SKUCode", typeof(string));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("quantity", typeof(int));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("SerialNo", typeof(string));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("BatchNo", typeof(string));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("InvoiceNumber", typeof(string));
                _dtMyData.Columns.Add(dc);
                dc = new DataColumn("InvoiceDate", typeof(string));
                _dtMyData.Columns.Add(dc);

                for (int i = 0; i < rwData.Length / 6; i++)
                {

                    partCode = Server.UrlDecode(rwData[a].Split('=')[1]);
                    Qty = rwData[b].Split('=')[1];
                    serialNo = Server.UrlDecode(rwData[c].Split('=')[1]);
                    batchno = rwData[d].Split('=')[1];
                    InvoiceNumber = Server.UrlDecode(rwData[z].Split('=')[1]);
                    InvoiceDate = Server.UrlDecode(rwData[f].Split('=')[1]);
                    DataRow dr = _dtMyData.NewRow();

//                    dr["skucode"] = partCode;
                    dr["skucode"] = partCode;
                    dr["quantity"] = Convert.ToInt32(Qty);
                    dr["serialno"] = serialNo.Replace("%2F", "").Replace("/", "");
                    dr["batchNo"] = batchno.Replace("%2F", "");
                    dr["InvoiceNumber"] = InvoiceNumber.Replace("%2F", "");
                    dr["InvoiceDate"] = InvoiceDate.Replace("%2F", "/");
                    _dtMyData.Rows.Add(dr);

                    a = a + 6;
                    b = b + 6;
                    c = c + 6;
                    d = d + 6;
                    z = z + 6;
                    f = f + 6;
                }

                StringBuilder sb = new StringBuilder();
                foreach (DataRow item in _dtMyData.Rows)
                {

                    sb.Append("<tr>");
                    sb.Append("<td><span>" + item["skucode"].ToString() + "</span>");
                    sb.Append("<input type=hidden name=partcode value=" + item["skucode"].ToString() + " />");
                    sb.Append("</td>");

                    sb.Append("<td><span>");
                    sb.Append(item["quantity"].ToString());
                    sb.Append("</span><input type=hidden name=qty value=" + item["quantity"].ToString() + " /></td>");

                    sb.Append("<td><span>");
                    sb.Append(item["serialNo"].ToString());
                    sb.Append("</span><input type=hidden name=serialno value='" + item["serialNo"].ToString() + "' /></td>");

                    sb.Append("<td><span>");
                    sb.Append(item["batchno"].ToString());
                    sb.Append("</span><input type=hidden name=batchno value=" + item["batchno"].ToString() + " /></td>");

                    sb.Append("<td><span>");
                    sb.Append(item["InvoiceNumber"].ToString());
                    sb.Append("</span><input type=hidden name=InvoiceNumber value='" + item["InvoiceNumber"].ToString() + "' /></td>");

                    sb.Append("<td><span>");
                    sb.Append(item["InvoiceDate"].ToString());
                    sb.Append("</span><input type=hidden name=InvoiceDate value='" + item["InvoiceDate"].ToString() + "' /></td></tr>");


                }

                lit.Text = sb.ToString();





            }

        }
    }
}
