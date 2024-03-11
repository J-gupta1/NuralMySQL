/* 
 * #Change Log
 * dd-MMM-yyyy, Name , #CCxx, Description.
 * 26-Dec-2018, Sumit Maurya, #3, New methods called according to ZedSalesv5.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopUpPages_ISPPopup : BussinessLogic.PageBase
{
    protected string strAssets = BussinessLogic.PageBase.strAssets;
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind();
        ucDatePickerFromDate.MinRangeValue = DateTime.Now;
        ucDatePickerFromDate.MaxRangeValue = DateTime.MaxValue;
        if (!IsPostBack)
        {
            ucDatePickerFromDate.MinRangeValue = DateTime.Now;
            ucDatePickerFromDate.MaxRangeValue = DateTime.MaxValue;
        
           // ucDatePickerEndDate.MinRangeValue = DateTime.Now;
           // ucDatePickerEndDate.MaxRangeValue = DateTime.MaxValue;

           // ucDatePickerEndDate.Date = DateTime.Now.Date.ToString();
            ucDatePickerFromDate.Date = DateTime.Now.Date.ToString();
        
    

            btnSearchRetailer.Attributes.Add("onclick", "return popupRetailer();");
           // ucDatePickerActivationDate.Date = DateTime.Now.Date.ToString();
            if (Request.QueryString["mode"] == "1" || Request.QueryString["mode"] == "2")
            {
                tblMap.Visible = true;
                tblDelete.Visible = false;
                tblExisting.Visible = false;
              
            }
            else
            {
                if (Request.QueryString["mode"] == "4")
                {
                    tblExisting.Visible = true;
                    tblDelete.Visible = false;
                    tblMap.Visible = false;
                }
                else
                {
                    tblExisting.Visible = false;
                    tblDelete.Visible = true;
                    tblMap.Visible = false;
                }
            }

            // if (Request.QueryString["retIspid"] == "0")
            //{
                
                
            //    trDeActivationDate.Visible = true;
            //    ucDatePickerActivationDate.ValidationGroup = "abc";
               
            //}
            //else
            //{
            //    trDeActivationDate.Visible = false;
                
            //}

          //  Response.Write(Request.QueryString["Ispid"]);
        }
      //  Response.Write(Request.QueryString["Ispid"]);
    }
    protected void btnExitISPSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            using (DataAccess.BeautyAdvisorData obj = new DataAccess.BeautyAdvisorData())
            {
                byte b = 1;
                if (chkRetailRetailer.Checked)
                    b = 2;

                if (Request.QueryString["mode"] == "4")
                    b = 4;


                DateTime dtEnd = DateTime.Now.Date;

                if (ucDateEndDate.TextBoxDate.Text!="")
                    dtEnd = Convert.ToDateTime(ucDateEndDate.Date);

                obj.Mode = b;
                obj.RetailerISPMappingID = Convert.ToInt32(Request.QueryString["retIspid"].ToString());
                //obj.FromDate = Convert.ToDateTime(ucDatePickerActivationDate.Date);
              //  obj.ISPID = Convert.ToInt32(Request.QueryString["Ispid"].ToString());
                obj.EndDate = dtEnd;
              /*  int result = obj.deleteISPMapingOrRetaining();  #CC01 Commented */
                /* #CC01 Add Start */
                obj.Userid = BussinessLogic.PageBase.UserId;
                int result = obj.deleteISPMapingOrRetainingV2(); /* #CC01 Add End */

                if (result == 0)
                {
                    hdfSuccess.Value = "1";
                    ucMessage.ShowSuccess(Resources.Messages.EditSuccessfull);
                    btnExitISPSubmit.Enabled = false;
                    ControlSetUp();
                }
                else if (result == 2)
                {
                    ucMessage.ShowInfo("End date should be greater than start date.");
                    ControlSetUp();
                }
                else if (result == 3)
                {
                    ucMessage.ShowInfo("This exit date is not valid as sales is already booked after this date.");
                    ControlSetUp();
                }
                else if (result == 1)
                {
                    ucMessage.ShowInfo(obj.Error);
                }
                else
                {
                    ucMessage.ShowError(obj.Error);
                }

            }
        }

        catch (Exception ex)
        {
            ucMessage.ShowError(ex.Message);
        }


    }
    protected void rblAsk_SelectedIndexChanged(object sender, EventArgs e)
    {
        ControlSetUp();
    }
    void ControlSetUp()
    {
        //if (Request.QueryString["Ispid"] != "0")
        //{
            
        //        trDeActivationDate.Visible = true;
            

        //}
        //else
        //{
        //    trDeActivationDate.Visible = false;
        //}

    }

    protected void btnClearMap_Click(object sender, EventArgs e)
    {
        ctl00_contentHolderMain_txtRetailerName.Text = "";
        ctl00_contentHolderMain_hdnRetailerID.Value = "0";
        ctl00_contentHolderMain_hdnRetailerName.Value = "";
        //ucDatePickerFromDate.TextBoxDate.Text = "";
        //ucDatePickerEndDate.TextBoxDate.Text = "";
     //   ucDatePickerEndDate.Date = DateTime.Now.Date.ToString();
        ucDatePickerFromDate.Date = DateTime.Now.Date.ToString();
    }
    protected void btnSubmitMap_Click(object sender, EventArgs e)
    {
        try /* #CC01 Added */
        {
            using (DataAccess.BeautyAdvisorData obj = new DataAccess.BeautyAdvisorData())
            {
                obj.RetailerID = Convert.ToInt32(ctl00_contentHolderMain_hdnRetailerID.Value);
                obj.FromDate = Convert.ToDateTime(ucDatePickerFromDate.Date);
                obj.ISPID = Convert.ToInt32(Request.QueryString["Ispid"].ToString());
                // obj.EndDate = Convert.ToDateTime(ucDatePickerEndDate.Date);
                obj.Mode = Convert.ToByte(Request.QueryString["mode"].ToString());
                /* #CC01 Add Start */
                obj.Userid = BussinessLogic.PageBase.UserId;
                int result = obj.UpdateISPMapingToNewRetialerV2(); /* #CC01 Add End */
                /* int result = obj.UpdateISPMapingToNewRetialer();  #CC01 Commented */
                if (result == 0)
                {
                    hdfSuccess.Value = "1";
                    ucMessage.ShowSuccess(Resources.Messages.EditSuccessfull);
                    btnExitISPSubmit.Enabled = false;
                    ControlSetUp();
                }
                else if (result == 2)
                {
                    ucMessage.ShowInfo("There is already a mapping with future effective date as date entered.");
                    ControlSetUp();
                }
                else
                {
                    ucMessage.ShowError(obj.Error);
                }
            }
            /* #CC01 Add Start */
        }
        catch (Exception ex)
        {
            ucMessage.ShowError(ex.Message);
        }  /* #CC01 Add End */
    }
}
