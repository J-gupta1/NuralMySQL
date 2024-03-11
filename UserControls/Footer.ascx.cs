using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BussinessLogic;

namespace Web.Controls
{
    public partial class Footer : System.Web.UI.UserControl
    {
        protected string siteURL = ConfigurationManager.AppSettings["siteurl"].ToString();
        protected string strSiteUrl = PageBase.siteURL;
        protected string strAssets = PageBase.strAssets;
        /*************************************************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            string strstrSiteUrl = strSiteUrl;
            string strstrAssets = strAssets;
        }
    }
}