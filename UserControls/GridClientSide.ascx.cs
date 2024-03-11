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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using BusinessLogics;
using System.Text;
using System.Web.UI.HtmlControls;
using System.ComponentModel;


namespace Web.Controls
{


    public partial class GridClientSide : System.Web.UI.UserControl
    {


        private string _ControlTypes = "hidden,label,hidden,hidden,hidden";

        public string ControlTypes
        {
            get { return _ControlTypes; }
            set { _ControlTypes = value; }
        }

        private string _DtColumnNames = "Column1,Column2,Column3,Column4,Column5";
        private string _DtColumnTypes = "String,String,Int32,String,String";

        private string _ColumnNames = "Column1,Column2,Column3,Column4,Column5";

        public string DataTableColumnNames
        {
            get { return _DtColumnNames; }
            set { _DtColumnNames = value; }
        }

        public string DataTableColumnTypes
        {
            get { return _DtColumnTypes; }
            set { _DtColumnTypes = value; }
        }


        public string GridColumnNames
        {
            get { return _ColumnNames; }
            set { _ColumnNames = value; }
        }

      


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
            //txtPartCode.Text = "";
            //txtQuantity.Text = "";


        }

        void RenderColumnNames()
        {
            StringBuilder sbColumns = new StringBuilder();
            if (_ColumnNames.Contains(','))
            {
                string[] cl = _ColumnNames.Split(',');

                foreach (string item in cl)
                {
                    sbColumns.Append("<th>");
                    sbColumns.Append(item);
                    sbColumns.Append("</th>");
                }
            }
            else
            {
                sbColumns.Append("<th>");
                sbColumns.Append(_ColumnNames);
                sbColumns.Append("</th>");
            }
            litGridHeader.Text = sbColumns.ToString();
        }

   



      

        public DataTable GetDataTable
        {
            get
            {
                return _dtMyData;
            }
        }

        public DataTable SetDataTable
        {
            set
            {
                _dtMyData = value;
                RenderData();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            RenderIncludes("CSS", "~/" + BussinessLogic.PageBase.strAssets + "/media/css/demo_page.css",
                "~/" + BussinessLogic.PageBase.strAssets + "/media/css/demo_table.css",
                "~/" + BussinessLogic.PageBase.strAssets + "/CSS/dhtmlwindow.css",
                "~/" + BussinessLogic.PageBase.strAssets + "/CSS/modal.css");
                    
            RenderIncludes("JS", BussinessLogic.PageBase.siteURL + "Assets/Jscript/jquery.js",
                BussinessLogic.PageBase.siteURL + "Assets/Jscript/jquery.dataTables.js",
                BussinessLogic.PageBase.siteURL + "Assets/Jscript/dhtmlwindow.js",
                BussinessLogic.PageBase.siteURL + "Assets/Jscript/modal.js");
        
        }
        void RenderIncludes(string IncludeType, params string[] str)
        {
            switch (IncludeType)
            {
                case "JS":
                    foreach (string item in str)
                    {
                        var js = new HtmlGenericControl("script");
                        js.Attributes["type"] = "text/javascript";
                        js.Attributes["src"] =  item;
                        Page.Header.Controls.Add(js);
                    }
                    break;
                case "CSS":
                    foreach (string item in str)
                    {
                        HtmlLink myHtmlLink = new HtmlLink();
                        myHtmlLink.Href =  item;
                        myHtmlLink.Attributes.Add("rel", "stylesheet");
                        myHtmlLink.Attributes.Add("type", "text/css");
                        Page.Header.Controls.Add(myHtmlLink);
                    }
                    break;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
                ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference("~/Assets/Jscript/jquery.js"));
                ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference("~/Assets/Jscript/jquery.dataTables.js"));
                RenderColumnNames();

                BussinessLogic.PageBase obj = new BussinessLogic.PageBase();
                _dtMyData = obj.AddDataColumns(_DtColumnNames, _DtColumnTypes);
                

            }
          //  salesChannelID.Value = _SalesChannelID;
          //  salesChannelCode.Value = _SalesChannelCode;
            BindGrid();
            

        }
        void BindGrid()
        { 
            string sT= string.Empty;

            if (Page.Request.Form["lbl"] != null)
            {

                string[] rwData = Page.Request.Form["lbl"].ToString().Split('&');
                string[] clmName = { _DtColumnNames };
                clmName = _DtColumnNames.Split(',');
                int[] Index = new int[clmName.Length];
                for (int ii = 0; ii < clmName.Length; ii++)
                {
                    Index[ii] = ii + 1;
                }

                BussinessLogic.PageBase obj = new BussinessLogic.PageBase();
                _dtMyData = obj.AddDataColumns(_DtColumnNames, _DtColumnTypes);


                try
                {

                    for (int i = 0; i < rwData.Length / clmName.Length; i++)
                    {
                        DataRow dr = _dtMyData.NewRow();
                        object objInput;
                        for (int j = 0; j < clmName.Length; j++)
                        {
                            objInput = rwData[Index[j] - 1].Split('=')[1].Trim().Replace("%2F","");
                            dr[clmName[j]] =  objInput;

                            
                        }
                        _dtMyData.Rows.Add(dr);

                        for (int k = 0; k < Index.Length; k++)
                        {
                            Index[k] = Index[k] + clmName.Length;

                        }

                    }
                    RenderData();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RenderTableAgain", "window.parent.document.getElementById('lbl').value='" + Page.Request.Form["lbl"].ToString() + "';", true);
                }
                catch (Exception ex)
                {
                   // Response.Write(sT); 
                }

            }
           
        }

        private void RenderData()
        {
            if (!IsBlankDataTable)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow item in _dtMyData.Rows)
                {

                    string[] clmName1 = { _DtColumnNames };

                    clmName1 = _DtColumnNames.Split(',');

                    string[] ctrlType = { _ControlTypes };
                    ctrlType = _ControlTypes.Split(',');

                    sb.Append("<tr>");
                    int TypesIndex = 0;
                    foreach (string strDtCName in clmName1)
                    {
                        sb.Append("<td>");

                        sb.Append("<input type=hidden name=" + strDtCName + " value=" + item[strDtCName].ToString() + " />");

                        if (ctrlType[TypesIndex].ToLower() == "label")
                        {
                            sb.Append("<span>" + item[strDtCName].ToString() + "</span>");
                        }
                        else
                        {
                            
                            sb.Append("<input type=" + ctrlType[TypesIndex] + "  value=" + item[strDtCName].ToString() + " />");
                            if (ctrlType[TypesIndex].ToLower() == "checkbox")
                            {
                                sb.Append("<span>" + item[strDtCName].ToString() + "</span>");
                            }
                        }
                        sb.Append("</td>");
                        TypesIndex++;
                    }
                    sb.Append("</tr>");
                }

                lit.Text = sb.ToString();
            }
        }
        void BindGrid1()
        {
            if (Page.Request.Form["lbl"] != null)
            {

                int a, b, c, d, ee;
                a = 0;
                b = 1;
                c = 2;
                d = 3;
                ee = 4;

                string partCode;
                string batchno;
                string serialNo;
                string Qty;
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

                for (int i = 0; i < rwData.Length / 5; i++)
                {
                    partCode = rwData[a].Split('=')[1];
                    Qty = rwData[c].Split('=')[1];
                    serialNo = rwData[d].Split('=')[1];
                    batchno = rwData[ee].Split('=')[1];
                    skuName = rwData[b].Split('=')[1];

                    DataRow dr = _dtMyData.NewRow();

                    dr["skucode"] = partCode.Replace("%23", "#").Replace("%2B", "+");
                    dr["skuName"] = skuName.Replace("%23", "#");
                    dr["quantity"] = Convert.ToInt32(Qty);
                    dr["serialno"] = serialNo.Replace("%2F", "").Replace("%23", "#").Replace("%2B", "+");
                    dr["batchNo"] = batchno.Replace("%2F", "").Replace("%23", "#").Replace("%2B", "+");
                    _dtMyData.Rows.Add(dr);

                    a = a + 5;
                    b = b + 5;
                    c = c + 5;
                    d = d + 5;
                    ee = ee + 5;

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



