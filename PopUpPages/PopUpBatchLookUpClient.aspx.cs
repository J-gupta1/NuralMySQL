using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopUpPages_PopUpBatchLookUpClient :  BussinessLogic.PageBase
{
    protected string strAssets = BussinessLogic.PageBase.strAssets;
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind();
    }
}
