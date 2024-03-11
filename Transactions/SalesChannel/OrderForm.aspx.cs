using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_POC_OrderForm : System.Web.UI.Page
{
    int orderid;
    protected void Page_Load(object sender, EventArgs e)
    {
        string str = Request.Url.ToString();
        string[] strln = str.Split('?');
        orderid = Convert.ToInt32(strln[1].ToString());
        GetData();
        lkclose.Attributes.Add("OnClick", string.Format("return funcwindowclose()"));

    }


    public void GetData()
    {
        using (SalesData obj = new SalesData())
        {
            obj.OrderId = orderid;
            DataSet ds = obj.GetPrimaryOrderInfoForReport();
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            bindheaderInfo(dt1);
            BindBaseInfo(dt2);
        }
    }


    public void bindheaderInfo(DataTable dt)
    {
        lblTINNo.Text = dt.Rows[0]["TinNumber"].ToString();
        lblPONo.Text = dt.Rows[0]["OrderNumber"].ToString();
        lblPODate.Text = dt.Rows[0]["OrderDate"].ToString();
        lblCST.Text = dt.Rows[0]["CstNumber"].ToString();
        lblServiceAddress.Text = dt.Rows[0]["Address1"].ToString();
        lblServicePhone.Text = dt.Rows[0]["PhoneNumber"].ToString();
        lblServiceMobile.Text = dt.Rows[0]["MobileNumber"].ToString();
        lblOrderFrom.Text = dt.Rows[0]["OrderFrom"].ToString();
        lblOrderto.Text = dt.Rows[0]["OrderTo"].ToString();
        lblTotalAmount.Text = dt.Rows[0]["TotalAmount"].ToString();
        Label1.Text = dt.Rows[0]["FromAddress"].ToString();
        Label3.Text = dt.Rows[0]["FromPhoneNo"].ToString();
        string strAmount = getword(Convert.ToDouble(dt.Rows[0]["TotalAmount"].ToString()));
        lblAmtinwords.Text = strAmount;

    }


    public void BindBaseInfo(DataTable dt)
    {
       
        gvPOPrint.DataSource = dt;
        gvPOPrint.DataBind();

    }

    #region "Convert decimal amount in word"
    public static String getword(double no)
    {

        long intPart = (long)no;
        //System.out.print("no="+no);
        //String noInStr =  new Float(no).toString();
        String noInStr = no.ToString();
        int ind = noInStr.IndexOf('.');
        int yy = Convert.ToString(intPart).Length;
        if (ind != -1)
        {
            yy = ind;
        }

        //ind=ind;
        //int l=noInStr.Length-1;
        //int l2 = l-ind;
        String dec = noInStr.Substring(ind + 1);
        int deciPart = int.Parse(dec);
        String noword = null;
        if (inWord(intPart, yy).Equals("") && inWord(deciPart, 2).Equals(""))
        {
            noword = "";
        }
        else if (!inWord(intPart, yy).Equals("") && inWord(deciPart, 2).Equals(""))
        {
            noword = inWord(intPart, yy) + "Only";
        }
        else if (inWord(intPart, yy).Equals("") && !inWord(deciPart, 2).Equals(""))
        {
            noword = "Paise " + inWord(deciPart, 2) + "Only";
        }
        else
        {
            noword = inWord(intPart, yy) + "and Paise " + inWord(deciPart, 2) + "Only";
        }

        return noword;
    }

    public static String inWord(long val, int len)
    {
        String strVal = null;
        if (len == 1)
        {
            strVal = oneD(val);
        }
        if (len == 2)
        {
            strVal = len_two(val);
        }
        if (len == 3)
        {
            strVal = len_three(val);
        }
        if (len == 4)
        {
            strVal = len_four(val);
        }
        if (len == 5)
        {
            strVal = len_five(val);
        }
        if (len == 6)
        {
            strVal = len_six(val);
        }
        if (len == 7)
        {
            strVal = len_seven(val);
        }
        if (len == 8)
        {
            strVal = len_eight(val);
        }
        if (len == 9)
        {
            strVal = len_nine(val);
        }
        return strVal;
    }

    static String len_two(long val)
    {
        String strVal = null;
        long x = val / 10;
        long y = val % 10;

        if (x == 1)
        {
            strVal = twoD(val);
        }
        else
        {
            strVal = twoDafter(x) + oneD(y);
        }
        return strVal;
    }

    static String len_three(long val)
    {
        String strVal = null;
        long x = val / 100;
        long y = val % 100;

        strVal = threeD(x) + len_two(y);

        return strVal;
    }

    static String len_four(long val)
    {
        String strVal = null;
        long x = val / 1000;
        long y = val % 1000;
        strVal = fourD(x) + len_three(y);

        return strVal;
    }

    static String len_five(long val)
    {
        String strVal = null;
        long x = val / 1000;
        long y = val % 1000;
        long r = x / 10;
        //int q=0;
        if (r > 0)
        {
            strVal = fiveD(x) + len_three(y);
        }
        else
        {
            strVal = fourD(x) + len_three(y);
        }

        return strVal;
    }

    static String len_six(long val)
    {
        String strVal = null;
        long x = val / 100000;
        long y = val % 100000;
        strVal = sixD(x) + len_five(y);

        return strVal;
    }

    static String len_seven(long val)
    {
        String strVal = null;
        long x = val / 100000;
        long y = val % 100000;
        long r = x / 10;
        //int q=0;
        if (r > 0)
        {
            strVal = sevenD(x) + len_five(y);
        }
        else
        {
            strVal = sixD(x) + len_five(y);
        }

        return strVal;
    }

    static String len_eight(long val)
    {
        String strVal = null;
        long x = val / 10000000;
        long y = val % 10000000;
        strVal = eightD(x) + len_seven(y);

        return strVal;
    }

    static String len_nine(long val)
    {
        String strVal = null;
        long x = val / 10000000;
        long y = val % 10000000;
        long r = x / 10;
        //int q=0;
        if (r > 0)
        {
            strVal = nineD(x) + len_seven(y);
        }
        else
        {
            strVal = eightD(x) + len_seven(y);
        }

        return strVal;
    }
    static String oneD(long val)
    {
        String oneDstr = null;
        if (val == 0)
        {
            oneDstr = "";
        }
        if (val == 1)
        {
            oneDstr = "One ";
        }
        if (val == 2)
        {
            oneDstr = "Two ";
        }
        if (val == 3)
        {
            oneDstr = "Three ";
        }
        if (val == 4)
        {
            oneDstr = "Four ";
        }
        if (val == 5)
        {
            oneDstr = "Five ";
        }
        if (val == 6)
        {
            oneDstr = "Six ";
        }
        if (val == 7)
        {
            oneDstr = "Seven ";
        }
        if (val == 8)
        {
            oneDstr = "Eight ";
        }
        if (val == 9)
        {
            oneDstr = "Nine ";
        }
        return oneDstr;
    }

    static String twoD(long val)
    {
        String Dstr = null;
        if (val == 10)
        {
            Dstr = "Ten ";
        }
        if (val == 11)
        {
            Dstr = "Eleven ";
        }
        if (val == 12)
        {
            Dstr = "Twelve ";
        }
        if (val == 13)
        {
            Dstr = "Thirteen ";
        }
        if (val == 14)
        {
            Dstr = "Fourteen ";
        }
        if (val == 15)
        {
            Dstr = "Fifteen ";
        }
        if (val == 16)
        {
            Dstr = "Sixteen ";
        }
        if (val == 17)
        {
            Dstr = "Seventeen ";
        }
        if (val == 18)
        {
            Dstr = "Eighteen ";
        }
        if (val == 19)
        {
            Dstr = "Nineteen ";
        }
        return Dstr;
    }

    static String twoDafter(long val)
    {
        String Dstr = null;
        if (val == 0)
        {
            Dstr = "";
        }
        if (val == 2)
        {
            Dstr = "Twenty ";
        }
        if (val == 3)
        {
            Dstr = "Thirty ";
        }
        if (val == 4)
        {
            Dstr = "Forty ";
        }
        if (val == 5)
        {
            Dstr = "Fifty ";
        }
        if (val == 6)
        {
            Dstr = "Sixty ";
        }
        if (val == 7)
        {
            Dstr = "Seventy ";
        }
        if (val == 8)
        {
            Dstr = "Eighty ";
        }
        if (val == 9)
        {
            Dstr = "Ninety ";
        }

        return Dstr;
    }

    static String threeD(long val)
    {
        String Dstr = null;
        if (val == 0)
        {
            Dstr = "";
        }
        else
        {
            Dstr = oneD(val) + "Hundred ";
        }
        return Dstr;
    }

    static String fourD(long val)
    {
        String Dstr = null;
        if (val == 0)
        {
            Dstr = "";
        }
        else
        {
            Dstr = oneD(val) + "Thousand ";
        }

        return Dstr;
    }

    static String fiveD(long val)
    {
        String Dstr = null;
        long p = 0;
        p = val / 10;
        if (p == 1)
        {
            Dstr = twoD(val) + "Thousand ";
        }
        else
        {
            Dstr = len_two(val) + "Thousand ";
        }

        return Dstr;
    }

    static String sixD(long val)
    {
        String Dstr = null;
        if (val == 0)
        {
            Dstr = "";
        }
        else
        {
            Dstr = oneD(val) + "Lakh ";
        }

        return Dstr;

    }
    static String sevenD(long val)
    {
        String Dstr = null;
        long p = 0;
        p = val / 10;
        if (p == 1)
        {
            Dstr = twoD(val) + "Lakh ";
        }
        else
        {
            Dstr = len_two(val) + "Lakh ";
        }

        return Dstr;
    }

    static String eightD(long val)
    {
        String Dstr = null;
        if (val == 0)
        {
            Dstr = "";
        }
        else
        {
            Dstr = oneD(val) + "Crore ";
        }

        return Dstr;

    }
    static String nineD(long val)
    {
        String Dstr = null;
        long p = 0;
        p = val / 10;
        if (p == 1)
        {
            Dstr = twoD(val) + "Crore ";
        }
        else
        {
            Dstr = len_two(val) + "Crore ";
        }

        return Dstr;
    }


    #endregion 
}