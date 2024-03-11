using System;
using System.Collections.Generic;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_TestPage :PageBase
{

    public Label lb = new Label();
    protected void Page_Load(object sender, EventArgs e)
    {
       if (!IsPostBack)
        {

            //btn1.Attributes.Add("onclick", "return getthat()");
            //Btn3.Attributes.Add("onclick", "return findcontrol()");
            fillGrid();
           Label4.Text = ((int)HttpContext.Current.Cache["Data1"]).ToString();
           int i = (int)HttpContext.Current.Cache["Data1"];   
            
        }
    
    }


    public void fillGrid()
    {

        ProductData obj = new ProductData();
        DataTable dt = obj.SelectAllColorInfo();
        grdColor.DataSource = dt;
        //grdColor2.DataSource = fillgrid();
        grdColor.DataBind();
        //grdColor2.DataBind();

    }

    public void retrieveValues()
    {
        int k = Convert.ToInt32(hdnColumn.Value);
        string[] str = new string[10];
        int i = 1;
        DataTable dt = new DataTable();
        DataColumn[] dc = new DataColumn[k];

        for (int c = 0; c < k; c++)
        {
            dc[c] = new DataColumn(c.ToString());
        }
            dt.Columns.AddRange(dc);
            while (Page.Request.Form[string.Format("TextBox{0}1", i)] != null)
            {
                DataRow dr = dt.NewRow();
                for (int c = 0; c < k; c++)
                {
                    dr[c.ToString()] = Page.Request.Form[string.Format("TextBox{0}{1}", i, (c + 1))];
                }
                //String text = Page.Request.Form[string.Format("TextBox{0}{1}", i, k)];
                //str[i - 1] = text;
                i++;
                dt.Rows.Add(dr);
            }

        //for (i =1 ; i < k; i++)
        //{
        //    string str1 = string.Format("TextBox{0}2", i);
        //    String text = Page.Request.Form[str1];
        //    if (text != null)
        //    {
        //        str[i - 1] = text;
        //    }

        //}

    }



    protected void btn1_Click(object sender, EventArgs e)
    {
        retrieveValues();
    }
}
