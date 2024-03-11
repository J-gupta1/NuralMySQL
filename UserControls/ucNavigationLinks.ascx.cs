/*
=====================================================================================================================================
 Change Log:
 ----------
 * 28-Aug-2017, Kalpana,  #CC02: hardcoded style removed and applied responsive css.
=====================================================================================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using ZedEBS.Admin;
//using ZedEBS.ZedService;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
//using LuminousSMS;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace ZedEBS.Controls
{
    public partial class UserControls_ucNavigationLinks : System.Web.UI.UserControl
    {
        private int _intRecordkey;
        private int _intUserRecordkey;
        private string _strXMLFilePath;
        public int RecordUserKey
        {
            set
            {
                _intUserRecordkey = value;
                Session["UserRecordKey"] = _intUserRecordkey;
                if (Session["UserRecordKey"] != null)
                {
                   // EnableDisableUserControls();
                }
            }
            get
            {
                if (Session["UserRecordKey"] != null)
                    return Convert.ToInt32(Session["UserRecordKey"]);
                return 0;
            }
        }
        public int RecordKey
        {
            set
            {
                _intRecordkey = value;
                Session["RecordKey"] = _intRecordkey;
                if (Session["RecordKey"] != null)
                {
                   // EnableDisableControls();
                }
            }
            get
            {
                if (Session["RecordKey"] != null)
                    return Convert.ToInt32(Session["RecordKey"]);
                return 0;
            }
        }
        public string XmlFilePath
        { 
            set {
                if (_strXMLFilePath != value)
                {
                   
                    _strXMLFilePath = value;
                   // GenerateLinks();
                  
                }
            }
            get { return _strXMLFilePath; }
           
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            //if (Session["PatientID"] == null)
            //{
            //    btnlcrequest.Enabled = false;
            //    btnOtherFund.Enabled = false;
            //}
            //else
            //{
            //    btnlcrequest.Enabled = true;
            //    btnOtherFund.Enabled = true;
            //}


            }
        }

        /// <summary>
        /// will generate the links according to the xml files
        /// </summary>
        //private void GenerateLinks()
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(Server.MapPath(XmlFilePath));
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            pnlDynamicControl.Visible = true;

        //            Button btnlink = new Button();
        //            pnlDynamicControl.Controls.Add(btnlink);
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                HyperLink hypLink = new HyperLink();
        //                hypLink.NavigateUrl = Convert.ToString(dr["LinkUrl"]);
        //                hypLink.NavigateUrl = "../other/ManageFundsRequest?EntityID=10&UserID=1&RoleID=1";
        //                hypLink.Text = Convert.ToString(dr["LinkText"]);
        //                hypLink.ToolTip = Convert.ToString(dr["LinkAlternateText"]); //tooltip
        //                hypLink.ID = Convert.ToString(dr["LinkControlID"]);
        //                // hypLink.Enabled = false;
        //                hypLink.Enabled = true;
        //                pnlDynamicControl.Controls.Add(hypLink);

        //            }
        //            GenerateColors();
        //        }
        //        else
        //        {
        //            Label lbl = new Label();
        //            lbl.Text = "No Data in File";       //This can be pick from the messages file
        //            pnlDynamicControl.Controls.Add(lbl);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //private void EnableDisableControls()
        //{
        //   // hyBack.NavigateUrl = "../Admin/Common/ManageEntityView.aspx";
        //    if (RecordKey == 0)
        //    {
        //        ((HyperLink)pnlDynamicControl.FindControl("ManagePatient")).Enabled = true;
        //    }
        //    else
        //    {
        //        Hashtable hshMode = new Hashtable();
        //        if (Session["EntityMode"] != null)
        //        {
        //            hshMode = (Hashtable)Session["EntityMode"];
        //            ///These are fetching but useless for here
        //           // hshMode["StockMaintainedBySystem"].ToString();      //Till now useless
        //           // hshMode["GroupMappingMode"].ToString();
        //           // hshMode["WeeklyOffMode"].ToString();
        //            /////End
        //            ((HyperLink)pnlDynamicControl.FindControl("ManagePatient")).Enabled = true;
        //            ((HyperLink)pnlDynamicControl.FindControl("ManageFundsRequest")).Enabled = Convert.ToBoolean(Convert.ToInt16(hshMode["ManageFundsRequest"]));
        //            ((HyperLink)pnlDynamicControl.FindControl("ManageFundingLC")).Enabled = Convert.ToBoolean(Convert.ToInt16(hshMode["ManageFundingLC"]));
        //        }

        //    }
        //}
        //private void EnableDisableUserControls()
        //{
        //    hyBack.NavigateUrl = "../Admin/Common/ManageUserInformationView.aspx";
        //    if (RecordUserKey == 0)
        //    {
        //        ((HyperLink)pnlDynamicControl.FindControl("ManageUserVer2")).Enabled = true;
        //        ((HyperLink)pnlDynamicControl.FindControl("ManageUserMapping")).Enabled = true;
        //    }
        //    else
        //    {
        //        Hashtable hshMode = new Hashtable();
        //        if (Session["UserMode"] != null)
        //        {
        //            hshMode = (Hashtable)Session["UserMode"];
        //            ((HyperLink)pnlDynamicControl.FindControl("ManageUserVer2")).Enabled = true;
        //            ((HyperLink)pnlDynamicControl.FindControl("ManageUserMapping")).Enabled = true;
        //            ((HyperLink)pnlDynamicControl.FindControl("ManageModificationUserMapping")).Enabled = true;
        //            ((HyperLink)pnlDynamicControl.FindControl("ManageUserBrandCategoryMapping")).Enabled = Convert.ToBoolean(Convert.ToInt16(hshMode["BrandCategoryMappingMode"]));
        //            ((HyperLink)pnlDynamicControl.FindControl("ManageUserBankDetail")).Enabled = Convert.ToBoolean(Convert.ToInt16(hshMode["UserBankMode"]));
        //            //((HyperLink)pnlDynamicControl.FindControl("EngineerMode")).Enabled = Convert.ToBoolean(Convert.ToInt16(hshMode["EngineerMode"]));
        //        }

        //    }
        //}

        //private void GenerateColors()
        //{
        //    try
        //    {
        //       string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        //        System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
        //        string sRet = oInfo.Name.Replace(".aspx", "");
        //        //if (sRet == "ManagePatient")
        //        //{
        //        //    hyBack.NavigateUrl = "../Admin/Common/ManageEntityView.aspx";
        //        //}
        //        //if (sRet == "ManageUserVer2")
        //        //{
        //        //    hyBack.NavigateUrl = "../Admin/Common/ManageUserInformationView.aspx";
        //        //}
              
        //        //((HyperLink)pnlDynamicControl.FindControl(sRet)).Attributes.Add("style", "color:#fdfea8");
        //        ((HyperLink)pnlDynamicControl.FindControl(sRet)).Style.Add("background-color", "#d0d0d0");
        //        ((HyperLink)pnlDynamicControl.FindControl(sRet)).Style.Add("color", "Black");
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        protected void btnOrganHierarchyUpload_Click(object sender, EventArgs e)
        {
            //D:\Works\ZedSalesSAAS\Masters\HO\Admin\ManageOrgnHierarchy.aspx
            Response.Redirect("~/Masters/HO/Admin/ManageOrgnHierarchy.aspx");
        }
        protected void btnSalesChannelUpload_Click(object sender, EventArgs e)
        {
            //D:\Works\ZedSalesSAAS\Masters\SalesChannel\ManageSalesChannel.aspx
            Response.Redirect("~/Masters/SalesChannel/ManageSalesChannel.aspx");
        }
        protected void btnISPUpload_Click(object sender, EventArgs e)
        {
            //D:\Works\ZedSalesSAAS\Masters\Common\ISPMasterUploadV2.aspx
            Response.Redirect("~/Masters/Common/ISPMasterUploadV2.aspx");
        }
        protected void btnHierarchyUser_Click(object sender, EventArgs e)
        {
            //D:\Works\ZedSalesSAAS\Masters\HO\Admin\ManageUser.aspx
            Response.Redirect("~/Masters/HO/Admin/ManageUser.aspx");
        }
}
}