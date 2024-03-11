using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data.SqlClient;
using System.Data;
/*Change Log:
 * 31-Dec-14, Rakesh Goel, #CC01 - Added additional parameter "source" in querystring.
 * 22-Mar-2017, Sumit Maurya,  #CC02, Checked whether mobile number is null or not.
 * 28-Jan-2020, Balram Jha, #CC03, changes for Inone
 */



public partial class Integration_SMSParsing : System.Web.UI.Page
{
    string strCustomerMobileNumber, strSerialNumberWithModelContent, strSource;
    SqlParameter[] objSqlParam;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["mb"] != null && Request.QueryString["mb"].ToString() != string.Empty)
                        strCustomerMobileNumber = HttpUtility.HtmlDecode(Request.QueryString["mb"].ToString());

                    if (Request.QueryString["ms"] != null && Request.QueryString["ms"].ToString() != string.Empty)
                        strSerialNumberWithModelContent = HttpUtility.HtmlDecode(Request.QueryString["ms"].ToString());

                    /*#CC01 add start*/
                    if (Request.QueryString["source"] != null && Request.QueryString["source"].ToString() != string.Empty)
                        strSource = HttpUtility.HtmlDecode(Request.QueryString["source"].ToString());
                    /*#CC01 add end*/

                    // string ouputErrorMessage;
                    Int32 result;

                    //We have changed this for gionee only
                    //undo check out we will find the proper calling of this page
                    //ObjSMS.MobileNumber = strCustomerMobileNumber;
                    //ObjSMS.SerialNumber = strSerialNumber;
                    //ObjSMS.SMSContent =Request.QueryString.ToString();
                    //result = InsertTertiorySalesThruSMS(strCustomerMobileNumber, strSerialNumber,out ouputErrorMessage);
                    //if (result == 1)
                    //{
                    //    Response.Write("@Success@ @autoreply@ Welcome to the world of Gionee mobile! For any query, please call on our toll free No.: 18002081166.");
                    //}

                    //else 
                    //{
                    //    Response.Write("Some error has occured"+ouputErrorMessage);
                    //}
                    //Data will come in the string like this
                    //http://localhost/ZedSalesV4/integration/smsparsing.aspx?mb=%2b919810318906&ms=Gpad_G1,868750010052612,868750011052611
                    using (SMSData ObjSMS = new SMSData())
                    {
                        ObjSMS.MobileNumber = strCustomerMobileNumber;
                        ObjSMS.SerialNumberWithModelContent = strSerialNumberWithModelContent;
                        ObjSMS.SMSContent = Request.QueryString.ToString();
                        result = ObjSMS.InsertTertiorySalesThruSMS();

                        /*#CC01 add start*/
                        if (strSource == "bsmart")
                        {
                            Response.Write("Welcome to the world of Gionee mobile! For any query, please call on our toll free No.: 18002081166.");
                        }
                        else
                        /*#CC01 add end*/
                        {
                            if (result == 1)    //This is the actual success
                            {
                                /*#CC03 comented
                                // #CC02 Add Start 
                                if (string.IsNullOrEmpty(ObjSMS.MobileNumber) != true)
                                { // #CC02 Add End 
                                    if (ObjSMS.MobileNumber.StartsWith("+91"))
                                        Response.Write("@success@ @autoreply@ Welcome to the world of Gionee mobile! For any query, please call on our toll free No.: 18002081166. @endautoreply@");
                                }// #CC02 Added 
                                else*/

                                    Response.Write("@success@");


                            }
                            else////This is the Not success but client was demanding for this text
                            {
                                //Response.Write("@success@");#CC03 Comented
                                Response.Write(ObjSMS.Error);//#CC03 added
                            }
                        }
                    }


                }
                else
                    Response.Write("No query string found .");

            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

    }
    //public Int32 InsertTertiorySalesThruSMS(string strCustomerMobileNumber, string strSerialNumber, out string ErrorMessage)
    //{
    //    try
    //    {
    //        Int32 result;

    //        objSqlParam = new SqlParameter[5];
    //        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
    //        objSqlParam[0].Direction = ParameterDirection.Output;
    //        objSqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
    //        objSqlParam[1].Direction = ParameterDirection.Output;
    //        objSqlParam[2] = new SqlParameter("@SMSMobileNumber", strCustomerMobileNumber);
    //        objSqlParam[3] = new SqlParameter("@SMSSerialNumber", strSerialNumber);
    //        objSqlParam[4] = new SqlParameter("@SMSContent", Request.QueryString.ToString());
    //        DataAccess.DataAccess.Instance.DBInsertCommand("prcInsertTertiorySalesThruSMS", objSqlParam);
    //        if (objSqlParam[1].Value != System.DBNull.Value && objSqlParam[1].Value.ToString() != "")
    //        {
    //            ErrorMessage = Convert.ToString(objSqlParam[1].Value);
    //        }
    //        else
    //            ErrorMessage = null;
    //        result = Convert.ToInt32(objSqlParam[0].Value);
    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}
