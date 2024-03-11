using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using Cryptography;

public partial class BulletinBoard_ucDisplayBoard : System.Web.UI.UserControl
{
    BulletinData objBulletin = new BulletinData();
    protected void Page_Load(object sender, EventArgs e)
    {
   
        if (!IsPostBack)
        {
            try
            {
                objBulletin.UserID = PageBase.UserId;
                objBulletin.SearchType = 1;
                RptMarquee.DataSource = objBulletin.GetBulletinInfoByUserId();
                RptMarquee.DataBind();
            }
            catch (Exception ex)
            {

                PageBase.Errorhandling(ex);
            }
         
           
        }

     }

    public bool isRepeaterHaveData
    {
        get
        {
            if (RptMarquee.Items.Count == 0)
                return false;
            else return true;
        } 
    }


    public void RptMarquee_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HyperLink HLDetails = (HyperLink)e.Item.FindControl("hlEdit");
            Label lblBulletinId = (Label)e.Item.FindControl("lblBulletinID");
            Int32 BulleTinId = Convert.ToInt32(lblBulletinId.Text);
            Label lblExpiryDate = (Label)e.Item.FindControl("lblExpiryDate");
            DateTime daySpan = DateTime.Today.AddDays(3);
            if (Convert.ToDateTime(lblExpiryDate.Text) <= daySpan)
            {
                HLDetails.CssClass = "bulletinlnk1";
                HLDetails.Attributes.Add("style", "text-decoration:blink");
            }
            string strViewBulletinDetail = null;
            strViewBulletinDetail = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(BulleTinId), PageBase.KeyStr)).ToString().Replace("+", " ");
            {
               HLDetails.Attributes.Add("onclick", "return BulletinDetail(\"" + strViewBulletinDetail + "\")");
            }

        }

    } 
   
  }
