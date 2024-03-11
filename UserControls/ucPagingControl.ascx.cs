using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using System.Configuration;
using BussinessLogic;

public partial class UserControls_ucPagingControl : System.Web.UI.UserControl
{
    #region Delegate and Event
    public delegate void OnSetControlRefresh();
    public event OnSetControlRefresh SetControlRefresh;
    #endregion

    #region Private Class Members
    private int _intTotalRecords = 0;
    private int _intPageSize = 0;
    private int _intCurrentPage = 0;
    private int _intTotalPage = 0;

    #endregion

    #region Public Properties

    public int TotalRecords
    {
        get
        {
            return _intTotalRecords;
        }
        set
        {
            _intTotalRecords = value;
            ViewState["_totPage"] = value;
            //intTotalRecords = Convert.ToInt32(ViewState["TotalRecords"]);
        }
    }
    public int CurrentPage
    {
        get
        {
            return _intCurrentPage;
        }
        set
        {
            _intCurrentPage = value;
        }
    }
    public int SetCurrentPage
    {
        get
        {
            return _intCurrentPage;
        }
        set
        {
            if (Session["currentPage"] != null)
            {
                if (Convert.ToString(Session["currentPage"]) == "0")
                {
                    _intPageSize = Convert.ToInt32(Session["currentPage"]);
                    ViewState["_curPage"] = Session["currentPage"];
                    Session["currentPage"] = 1;
                }
            }
            else
            {
                _intCurrentPage = value;
            }
        }
    }
    public int PageSize
    {
        get
        {
            return _intPageSize;
        }
        set
        {
            _intPageSize = value;
        }
    }

    #endregion

    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindControls();
            FillPageInfo();
        }
    }
    public void BindControls()
    {
        imgFirst.ImageUrl = strSiteUrl + strAssets + "/CSS/Images/Paging/btn_first.gif";
        imgPrev.ImageUrl = strSiteUrl + strAssets + "/CSS/Images/Paging/btn_prev.gif";
        imgNaxt.ImageUrl = strSiteUrl + strAssets + "/CSS/Images/Paging/btn_next.gif";
        imgLast.ImageUrl = strSiteUrl + strAssets + "/CSS/Images/Paging/btn_last.gif";
        Go.ImageUrl = strSiteUrl + strAssets + "/CSS/Images/Paging/go.gif";
    }
    protected void lnkfirst_Click(object sender, System.EventArgs e)
    {
        lnkPrev.Enabled = false;
        lnkfirst.Enabled = false;
        lnkNext.Enabled = true;
        lnkLast.Enabled = true;
        CurrentPage = 1;
        ViewState["_curPage"] = "1";
        FillPageInfo();
        SetControlRefresh();
    }

    protected void lnkPrev_Click(object sender, System.EventArgs e)
    {
        int prevCount = Convert.ToInt32(ViewState["_curPage"].ToString()) - 1;
        ViewState["_curPage"] = prevCount.ToString();
        if (Convert.ToInt32(ViewState["_curPage"].ToString()) <= 1)
        {
            prevCount = 1;
            CurrentPage = 1;
            lnkPrev.Enabled = false;
            lnkfirst.Enabled = false;
        }
        else
        {
            CurrentPage = Convert.ToInt32(ViewState["_curPage"]);
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;
        }
        FillPageInfo();
        SetControlRefresh();
    }

    protected void lnkNext_Click(object sender, System.EventArgs e)
    {
        if (ViewState["_curPage"] != null && ViewState["_totPage"] != null)
        {
            int nextCount = Convert.ToInt32(ViewState["_curPage"].ToString()) + 1;
            ViewState["_curPage"] = nextCount.ToString();
            if (Convert.ToInt32(ViewState["_curPage"].ToString()) >= Convert.ToInt32(ViewState["_totPage"].ToString()))
            {
                ViewState["_curPage"] = ViewState["_totPage"];
                CurrentPage = Convert.ToInt32(ViewState["_curPage"]);
                lnkNext.Enabled = false;
                lnkLast.Enabled = false;
                lnkPrev.Enabled = true;
                lnkfirst.Enabled = true;
            }
            else
            {
                CurrentPage = Convert.ToInt32(ViewState["_curPage"]);
                lnkPrev.Enabled = true;
                lnkfirst.Enabled = true;
            }
            FillPageInfo();
            SetControlRefresh();
        }
    }

    protected void lnkLast_Click(object sender, System.EventArgs e)
    {
        lnkNext.Enabled = false;
        lnkLast.Enabled = false;
        lnkPrev.Enabled = true;
        lnkfirst.Enabled = true;
        CurrentPage = Convert.ToInt32(ViewState["_totPage"]);
        ViewState["_curPage"] = ViewState["_totPage"];
        FillPageInfo();
        SetControlRefresh();
    }

    protected void Go_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        int pagenumb = 1;
        if (txtGo.Text.Trim().Length > 0)
        {
            pagenumb = Convert.ToInt32(txtGo.Text);
        }

        if (pagenumb < 1)
        {
            pagenumb = 1;
        }
        ViewState["_curPage"] = Convert.ToString(pagenumb);
        if (ViewState["_curPage"].ToString() == "1")
        {
            lnkPrev.Enabled = false;
            lnkfirst.Enabled = false;
        }
        else
        {
            lnkPrev.Enabled = true;
            lnkfirst.Enabled = true;
        }
        if (Convert.ToInt32(ViewState["_totPage"]) < pagenumb)
        {
            ViewState["_curPage"] = ViewState["_totPage"];
        }
        if (Convert.ToInt32(ViewState["_curPage"]) < Convert.ToInt32(ViewState["_totPage"]))
        {
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;
        }
        else
        {
            lnkNext.Enabled = false;
            lnkLast.Enabled = false;
        }
        CurrentPage = Convert.ToInt32(ViewState["_curPage"]);
        FillPageInfo();
        SetControlRefresh();
    }

    public void FillPageInfo()
    {
        try
        {
            //TotalRecords = Convert.ToInt32(ViewState["TotalRecords"]);
            if (TotalRecords > 0)
            {
                _intTotalPage = TotalRecords / _intPageSize;
                int tmpReminder = TotalRecords % _intPageSize;
                if (tmpReminder > 0)
                {
                    _intTotalPage = _intTotalPage + 1;
                }
                ViewState["_totPage"] = Convert.ToString(_intTotalPage);
                ViewState["_totRecord"] = Convert.ToString(TotalRecords);
                if (Convert.ToInt32(ViewState["_totPage"]) < Convert.ToInt32(ViewState["_curPage"]))
                {
                    ViewState["_curPage"] = "1";
                }
                else if (CurrentPage > 0 && CurrentPage <= Convert.ToInt32(ViewState["_totPage"]))
                {
                    ViewState["_curPage"] = CurrentPage;
                }
            }
            else
            {
                ViewState["_totRecord"] = Convert.ToString(TotalRecords);
            }
        }
        catch { }
        try
        {
            ViewState["_PageSize"] = Convert.ToString(ConfigurationManager.AppSettings["PageSize"]);
            if (ViewState["_curPage"] == null)
            {
                ViewState["_curPage"] = "1";
                CurrentPage = Convert.ToInt32(ViewState["_curPage"]);
            }
            else if (Convert.ToInt32(ViewState["_curPage"].ToString()) < 1)
            {
                ViewState["_curPage"] = "1";
                CurrentPage = Convert.ToInt32(ViewState["_curPage"]);
            }
            else
            {
                CurrentPage = Convert.ToInt32(ViewState["_curPage"]);
            }
            this.lblPageNo.Text = "| page " + ViewState["_curPage"].ToString() + " of " + ViewState["_totPage"].ToString() + " |";
            this.lblTotalRecord.Text = string.Concat("| Record(s) found : ", Convert.ToString(ViewState["_totRecord"]), " |");
            if (Convert.ToInt32(ViewState["_curPage"]) < Convert.ToInt32(ViewState["_totPage"]))
                txtHid.Text = ViewState["_totPage"].ToString();
            if (CurrentPage <= 1)
            {
                lnkPrev.Enabled = false;
                lnkfirst.Enabled = false;
            }
            else
            {
                lnkPrev.Enabled = true;
                lnkfirst.Enabled = true;
            }
            if (Convert.ToInt32(ViewState["_curPage"]) < Convert.ToInt32(ViewState["_totPage"]))
            {
                lnkNext.Enabled = true;
                lnkLast.Enabled = true;
            }
            else
            {
                lnkNext.Enabled = false;
                lnkLast.Enabled = false;
            }
        }
        catch { }
    }
}
