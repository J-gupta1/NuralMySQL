using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using Cryptography;
//======================================================================================
//* Developed By : Amit Srivastava
//* Role         : Software Engineer
//* Module       : Bulletin Board
//* Description  : This page is used for  managing of Bulletins information
//* ====================================================================================
public partial class BulletinBoard_BulletinDetail :PageBase
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if ((Request.QueryString["BulletinId"] != null) && (Request.QueryString["BulletinId"] != ""))
            {
                Int32 BulletinId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Request.QueryString["BulletinId"], PageBase.KeyStr)));
                if (BulletinId != 0)
                {
                    using (BulletinData objBulletin = new BulletinData()) ;
                    {
                        DetailPanel.Visible = true;
                        BulletinData objBulletin = new BulletinData();
                        objBulletin.BulletinId = BulletinId;
                        RptDisplay.DataSource = objBulletin.GetBulletinInfo();
                        RptDisplay.DataBind();
                    }
                }
            }
        }
    }
    protected void LBViewBulletin_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewBulletin.aspx",false);

    }
}
