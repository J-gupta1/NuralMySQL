using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;
using DevExpress.XtraCharts;

/// <summary>
/// Developed By - Saurabh Tyagi
/// Functionality - Use to show the details of the company's sales at varrious levels 
/// 
/// </summary>


public partial class DashBoard_DashBoard : PageBase
{

    List<string> type = new List<string>();
       

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
               
                filllocations();
                fillyear();
                fillbrand();
                fillreports();
                showbranding();
        }
    }

public void clearseries()
    {
        WCInterSales.Series.Clear();
        WCPrimarySales.Series.Clear();
        WCRetailerBilled.Series.Clear();
        WCSecondarySales.Series.Clear();
        WCStock.Series.Clear();
    }



public void fillbrand()
{
    using(ProductData obj = new ProductData())
    {
        obj.BrandSelectionMode = 1 ;
        DataTable dt = obj.SelectAllBrandInfo();
        cmbBrand.DataSource = dt;
        cmbBrand.DataTextField = "BrandName";
        cmbBrand.DataValueField = "BrandID";
        cmbBrand.DataBind();
        cmbBrand.Items.Insert(0, new ListItem("All", "0"));
        updYear.Update();
    }


}


public void showbranding()
{
    int i = findbrandings();
    if (i == 1)
    {
        pnlBrand.Visible = true;
    }
    else
    {
        pnlBrand.Visible = false;
    }
}



    public void fillmonth(int j)
    {
       Dictionary<int, string> mon = new Dictionary<int, string>();
        DateTimeFormatInfo dtx = new DateTimeFormatInfo();
        mon.Add(0,"All"); 
        
        for (int i = 1; i < j; i++)
        {
         mon.Add(i, dtx.GetMonthName(i).ToString()); 
        }
        cmbMonth.DataSource = new BindingSource(mon, null);
        cmbMonth.DataValueField = "Key";
        cmbMonth.DataTextField = "Value";
        cmbMonth.DataBind();
        cmbMonth.SelectedValue = DateTime.Now.Month.ToString();
        updYear.Update();
    }

    public void fillyear()
    {
        cmbYear.Items.Clear();
        int j = 0;
        Dictionary<int,string> year = new Dictionary<int,string>();
        for (int i = (DateTime.Now.Year) - 5;  i <= DateTime.Now.Year; i++)
        {
            year.Add(j, i.ToString());
            j++;
        }
        cmbYear.DataSource = new BindingSource(year, null);
        cmbYear.DataTextField = "Value";
        cmbYear.DataValueField = "Key";
        cmbYear.DataBind();
        cmbYear.Items.FindByText(DateTime.Now.Year.ToString()).Selected=true;
        updYear.Update();
        cmbYear_SelectedIndexChanged(cmbYear, new EventArgs());
        
     }


    public int findbrandings()
    {
        using (ProductData obj = new ProductData())
        {
            obj.BrandSelectionMode = 1;
          DataTable dt =  obj.SelectBrandInfo();
          if (dt.Rows.Count > 1)
          {
              return 1;
          }
        }
        return 0;
    }

 public void filllocations()
    {
        using (dashBoard obj = new dashBoard())
        {
            DataTable dt1 = new DataTable();
            obj.UserId = PageBase.UserId;
            DataTable dt = obj.SelectOrganizationHierarchyinfo();
            cmbRegion.DataSource = dt;
            cmbRegion.DataTextField = "LocationName";
            cmbRegion.DataValueField = "OrgnhierarchyID";
            cmbRegion.DataBind();
            cmbRegion.Items.Insert(0, new ListItem("All", "0"));
            cmbRegion.SelectedValue = "0";
         }
      
    }


    public void fillreports()
    {
         DataSet ds;
        using (dashBoard obj = new dashBoard())
        {
           if (cmbMonth.SelectedValue == "0")
            {
                obj.FromDate = string.Format("1-1-{0}",(cmbYear.SelectedItem.ToString()));
                obj.ToDate = string.Format("12-31-{0}", (cmbYear.SelectedItem.ToString()));
            }
            else
            {
                obj.FromDate = string.Format("{0}-1-{1}", cmbMonth.SelectedValue ,cmbYear.SelectedItem.ToString());
                obj.ToDate = string.Format("{0}-{1}-{2}", cmbMonth.SelectedValue, DateTime.DaysInMonth(Convert.ToInt16(cmbYear.SelectedItem.ToString()), Convert.ToInt16(cmbMonth.SelectedValue)), cmbYear.SelectedItem.ToString());
            }
            obj.Location = Convert.ToInt16(cmbRegion.SelectedValue);
            obj.UserId = PageBase.UserId;
            obj.BrandId = Convert.ToInt16(cmbBrand.SelectedValue);
            ds = obj.SelectDashBoardPrimaryData();
            senddata(ref ds);
            ds = obj.SelectDashBoardSecondaryData();
            senddata(ref ds);
            ds = obj.SelectDashBoardRetailerData();
            senddata(ref ds);
            ds = obj.SelectDashBoardStockData();
            senddata(ref ds);
        }

    }

    public void senddata(ref DataSet ds)
    {
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            callchart(ds.Tables[i], Convert.ToInt16(ds.Tables[i].Rows[0][2]));

        }
    }

    protected void btnGetData_Click(object sender, EventArgs e)
    {
        clearseries();
        fillreports();
        
    }

    public void callchart(DataTable dtInhouse ,int k)
    {
       
        List<Series> str = new List<Series>();
        for (int i = 0; i < dtInhouse.Rows.Count; i++)
        {
            str.Add(new Series(dtInhouse.Rows[i][0].ToString(), DevExpress.XtraCharts.ViewType.Bar));
            str[i].Points.Add(new SeriesPoint(Convert.ToChar(i + 97).ToString(), dtInhouse.Rows[i][1]));
            str[i].ValueScaleType = ScaleType.Numerical;
            
            switch (k - 1)
            {
                case 0:
                    WCPrimarySales.Series.Add(str[i]);
                    WCPrimarySales.DataBind();
                    updPrimary.Update();
                    break;
                case 4:
                    WCStock.Series.Add(str[i]);
                    WCStock.DataBind();
                    updStock.Update();
                    break;

                case 1:
                    WCInterSales.Series.Add(str[i]);
                    WCInterSales.DataBind();
                    updInter.Update();
                    break;

                case 2:
                    WCSecondarySales.Series.Add(str[i]);
                    WCSecondarySales.DataBind();
                    updSecondry.Update();
                    break;

                case 3:
                    WCRetailerBilled.Series.Add(str[i]);
                    WCRetailerBilled.DataBind();
                    updRetailer.Update();
                    break;
            }

        }
        
    }


    protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbYear.SelectedItem.ToString() == DateTime.Now.Year.ToString())
        {
            fillmonth(DateTime.Now.Month + 1);
        }
        else
        {
            fillmonth(13);
        }

    }
}






