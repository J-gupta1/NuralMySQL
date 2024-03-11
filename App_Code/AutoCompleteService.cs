#region Comments
/*
 * 07-Nov-2014 Ajeet Mishra :#CC01 Context key value and when -1#0 then ACtive state and city when 1#1 then both come
 * 05-Sep-2015, Sumit Maurya, #CC02, New webmethod created to Return SAPPartCode with ID.
 * 08-Sep-2015, Priya Bhatia, #CC03, New webmethod created to Return ProductSAPCode with ID for Product Master Interface.
 * 10-Dec-2015, Sumit Maurya, #CC04, option "Other" will not be binded in city if Locality is mandatory according to UIConfig.
 * 29-Dec-2015, Sumit Maurya, #CC05, option "Other" will not be binded in city if Locality is mandatory according to UIConfig.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Data;
using AjaxControlToolkit;
using System.Collections.Specialized;
//using ZedEBS.Admin;
using System.Xml.Serialization;
using System.Web.Script.Services;
using System.Xml;
//using ZedEBS;
using System.Configuration;
using DataAccess;

/// <summary>
/// Summary description for AutoCompleteService
/// </summary>
[ScriptService]
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
//[System.Web.Script.Services.ScriptService()]
public class AutoCompleteService : System.Web.Services.WebService
{
    public AutoCompleteService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetAllCountries(string knownCategoryValues, string category)
    {
        try
        {
            DataTable dtresult = null;
            using (MastersData state = new MastersData())
            {
                state.Active = 255;
                dtresult = state.SelectStateList();
            }

            if (dtresult != null)
            {
                List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
                foreach (DataRow dr in dtresult.Rows)
                {
                    values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["CountryName"]), Convert.ToString(dr["CountryID"])));
                }
                return values.ToArray();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return null;
    }


    [WebMethod]
    public CascadingDropDownNameValue[] GetAllStateForCountry(string knownCategoryValues, string category, string contextKey)
    {
        try
        {
            if (contextKey == null || contextKey == "0")
            {
                contextKey = "-1#1";
            }
            /*#CC01 Added Start */
            string[] Contextobj = contextKey.Split('#');


            string StrContextKey = Contextobj[0];
            string ContextValue = Contextobj[1];
            Int16 ActiveStaus = 255;

            if (ContextValue == "0")
            {
                ActiveStaus = 1;
            }
            else if (ContextValue == "1")
            {
                ActiveStaus = 255;
            }

            /*#CC01 Added End */
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            if (kv == null)
            {
                return null;
            }
            Int16 CountryID;
            if (!kv.ContainsKey("Country") || !Int16.TryParse(kv["Country"], out CountryID))
            {
                return null;
            }
            DataTable dtresult = null;
            using (MastersData objstate = new MastersData())
            {
                // objstate.Active = 255; /*#CC01 Comment*/
                objstate.Active = ActiveStaus; /*#CC01 Added*/

                objstate.CountryID = CountryID;
                dtresult = objstate.SelectStateList();
            }

            if (dtresult != null)
            {
                List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
                foreach (DataRow dr in dtresult.Rows)
                {
                    values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["StateName"]), Convert.ToString(dr["StateID"])));
                }
                if (StrContextKey == "-1")
                    values.Add(new CascadingDropDownNameValue("Other", "-1"));
                return values.ToArray();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return null;
    }



    //[WebMethod]
    //public CascadingDropDownNameValue[] GetAllParts(string knownCategoryValues, string category)
    //{
    //    try
    //    {
    //        DataSet dsresult = null;
    //        using (clsPromotion objParts = new clsPromotion())
    //        {
    //            objParts.Active = 1;
    //            objParts.ComingFrom = 1;

    //            dsresult = objParts.GetAllPartsForApplyPromotion();
    //        }

    //        if (dsresult != null)
    //        {
    //            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
    //            if (dsresult.Tables[0].Rows.Count > 0)
    //            {
    //                foreach (DataRow dr in dsresult.Tables[0].Rows)
    //                {
    //                    string strPartId = Convert.ToString(dr["ChangedPartID"]).Split('@')[0];
    //                    values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["sappartcode"]), Convert.ToString(dr["ChangedPartID"])));
    //                }
    //            }
    //            return values.ToArray();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return null;
    //}


    //[WebMethod]
    //public CascadingDropDownNameValue[] GetAllUomBasedOnPart(string knownCategoryValues, string category, string contextKey)
    //{
    //    try
    //    {
    //        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
    //        if (kv == null)
    //        {
    //            return null;
    //        }
    //        Int16 PartID;
    //        //if (!kv.ContainsKey("Part") || !Int16.TryParse(kv["Part"], out PartID))
    //        if (!kv.ContainsKey("Part"))
    //        {
    //            return null;
    //        }
    //        string strPartId = Convert.ToString(kv["Part"]).Split('@')[0];
    //        DataSet dsresult = null;
    //        using (clsPromotion objParts = new clsPromotion())
    //        {
    //            objParts.Active = 1;
    //            objParts.ComingFrom = 2;
    //            objParts.PartID = Convert.ToInt32(strPartId);
    //            dsresult = objParts.GetAllPartsForApplyPromotion();
    //        }


    //        if (dsresult != null)
    //        {
    //            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
    //            if (dsresult.Tables[0].Rows.Count > 0)
    //            {
    //                foreach (DataRow dr in dsresult.Tables[0].Rows)
    //                {
    //                    values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["UOMDescription"]), Convert.ToString(dr["UOMID"])));
    //                }
    //                if (contextKey == "-1")
    //                    values.Add(new CascadingDropDownNameValue("Other", "-1"));
    //            }
    //            return values.ToArray();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return null;
    //}



    /* [WebMethod]*/
    /*#CC04 Commened*/
    /* #CC04 Add Start*/
    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    /* #CC04 Add End*/
    public CascadingDropDownNameValue[] GetAllCityForStates(string knownCategoryValues, string category, string contextKey)
    {
        try
        {
            if (contextKey == null || contextKey == "0")
            {
                contextKey = "-1#1";
            }
            /*#CC01 Added Start */
            string[] Contextobj = contextKey.Split('#');
            string StrContextKey = Contextobj[0];
            string ContextValue = Contextobj[1];
            Int16 ActiveStaus = 255;

            if (ContextValue == "0")
            {
                ActiveStaus = 1;
            }
            else if (ContextValue == "1")
            {
                ActiveStaus = 255;
            }

            /*#CC01 Added End */
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            if (kv == null)
            {
                return null;
            }
            Int16 StateID;
            if (!kv.ContainsKey("State") || !Int16.TryParse(kv["State"], out StateID))
            {
                return null;
            }
            DataTable dtresult = null;
            using (MastersData objcity = new MastersData())
            {
                //objcity.Active = 255; /*#CC01 Comment*/
                objcity.Active = ActiveStaus; /*#CC01 Added*/
                objcity.State_Id = StateID;
                dtresult = objcity.SelectCityList();
                //dtresult = objcity.SelectById();
                //objCityMaster.PageIndex = 1;
                //objCityMaster.PageSize = 1000;
                //objCityMaster.Active = 1; /*#CC01 Comment*/
                //objCityMaster.Active = ActiveStaus; /*#CC01 Added*/
                //objCityMaster.ActiveStatus = ActiveStaus; /*#CC01 Added*/
                //objCityMaster.StateID = StateID;
                //dtresult = objCityMaster.SelectByStateId();

            }

            if (dtresult != null)
            {
                List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
                foreach (DataRow dr in dtresult.Rows)
                {
                    values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["CityName"]), Convert.ToString(dr["CityID"])));
                }

                ///* #CC04 Add Start */
                //DataTable objdtLocalityMandatory = null;
                //objdtLocalityMandatory = PageBase.GetEnumByTableName("UIConfig", "LocalityRequired");
                //if (objdtLocalityMandatory != null)
                //{
                //    if (objdtLocalityMandatory.Rows[0]["Value"].ToString() != "1")
                //    {
                //        if (StrContextKey == "-1")
                //        {
                //            values.Add(new CascadingDropDownNameValue("Other", "-1"));
                //        }
                //    }
                //}
                /* #CC04 Add End */
                //#CC01 Comment if (contextKey == "-1")
                // if (StrContextKey == "-1") /*#CC01 Added */  /* #CC04 Commented */
                //  values.Add(new CascadingDropDownNameValue("Other", "-1")); /* #CC04 Commented */
                return values.ToArray();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return null;
    }

    /* [WebMethod]*/
    /*#CC05 Commened*/
    /* #CC05 Add Start*/
    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    /* #CC05 Add End*/
    public CascadingDropDownNameValue[] GetAllCityForState(string knownCategoryValues, string category, string contextKey)
    {
        try
        {

            /*#CC01 Added Start */
            if (contextKey == null || contextKey == "0")
            {
                contextKey = "-1#1";
            }
            string[] Contextobj = contextKey.Split('#');
            string StrContextKey = Contextobj[0];
            string ContextValue = Contextobj[1];
            Int16 ActiveStaus = 255;

            if (ContextValue == "0")
            {
                ActiveStaus = 1;
            }
            else if (ContextValue == "1")
            {
                ActiveStaus = 255;
            }

            /*#CC01 Added End */
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            Int16 StateID;
            if (!kv.ContainsKey("State") || !Int16.TryParse(kv["State"], out StateID))
            {
                return null;
            }
            DataTable dtresult = null;
            using (MastersData objCityMaster = new MastersData())
            {
                //objCityMaster.Page = 1;
                //objCityMaster.PageSize = 1000;
                //objCityMaster.Active = 1; /*#CC01 Comment*/
                objCityMaster.Active = ActiveStaus; /*#CC01 Added*/
                //objCityMaster.ActiveStatus = ActiveStaus; /*#CC01 Added*/
                objCityMaster.State_Id = StateID;
                dtresult = objCityMaster.SelectAllCityInfo();
            }

            if (dtresult != null)
            {
                List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
                foreach (DataRow dr in dtresult.Rows)
                {
                    values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["CityName"]), Convert.ToString(dr["CityID"])));
                }
                ///* #CC05 Add Start */
                //DataTable objdtLocalityMandatory = null;
                //objdtLocalityMandatory = Pagebase.GetEnumByTableName("UIConfig", "LocalityRequired");
                //if (objdtLocalityMandatory != null)
                //{
                //    if (objdtLocalityMandatory.Rows[0]["Value"].ToString() != "1")
                //    {
                //        if (StrContextKey == "-1")
                //        {
                //            values.Add(new CascadingDropDownNameValue("Other", "-1"));
                //        }
                //    }
                //}
                /* #CC05 Add End */

                //#CC01 Comment if (contextKey == "-1")
                // if (StrContextKey == "-1") /*#CC01 Added */ /* #CC05 Commented */
                //  values.Add(new CascadingDropDownNameValue("Other", "-1")); /* #CC05 Commented */

                return values.ToArray();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return null;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetLocationList(string prefixText, int count, string contextKey)
    {
        string[] strLocationName = new string[0];
        try
        {
            MastersData objLocality = new MastersData();
            Hashtable htLocalityName;
         //   objLocality.PageSize = 1;
            objLocality.CityId= Convert.ToInt32(contextKey);
            //objLocality.PageIndex = 1;
            objLocality.State_Id = 0;
            objLocality.CountryId = 0;
            objLocality.Active = 1;
            objLocality.RegionName = prefixText;
            using (DataTable dtLocationName = objLocality.SelectRegionInfo())
            {
                int intCounter = 0;
                if (dtLocationName.Rows.Count > 0)
                {
                    strLocationName = new string[dtLocationName.Rows.Count];
                    htLocalityName = new Hashtable(dtLocationName.Rows.Count);
                    foreach (DataRow drPartCode in dtLocationName.Rows)
                    {
                        strLocationName.SetValue(drPartCode["LocalityName"], intCounter);
                        htLocalityName.Add(drPartCode["LocalityName"].ToString().Trim().ToLower(), drPartCode["LocalityID"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strLocationName = new string[1];
                    htLocalityName = new Hashtable(1);
                    strLocationName.SetValue("No Match Found", 0);
                    htLocalityName.Add("No Match Found", "-1");
                }
                return strLocationName;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strLocationName;
        }
        catch (Exception ex)
        {

            return strLocationName;
        }
        finally
        {
            // Do Nothing.
        }
    }


    [WebMethod]
    public CascadingDropDownNameValue[] GetAllTaxesForCountry(string knownCategoryValues, string category, string contextKey)
    {
        try
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            if (kv == null)
            {
                return null;
            }
            Int16 CountryID;
            if (!kv.ContainsKey("Country") || !Int16.TryParse(kv["Country"], out CountryID))
            {
                return null;
            }
            DataTable dtresult = null;
            using (clsTaxmaster obj = new clsTaxmaster())
            {
                obj.Status = 255;
                obj.CountryId = CountryID;
                dtresult = obj.SelectList();
            }

            if (dtresult != null)
            {
                List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
                foreach (DataRow dr in dtresult.Rows)
                {
                    values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["TaxName"]), Convert.ToString(dr["TaxMasterID"])));
                }
                if (contextKey == "-1")
                    values.Add(new CascadingDropDownNameValue("Other", "-1"));
                return values.ToArray();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return null;
    }



    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod]
    //public string[] GetPartCodeForSapList(string prefixText, int count, string contextKey)
    //{
    //    string[] strPartCode = new string[0];
    //    try
    //    {
    //        clsProductMaster objPartcode = new clsProductMaster();
    //        Hashtable htPartCode;
    //        DataTable dtPartCode;
    //        objPartcode.ProductSAPCode = prefixText;
    //        using (dtPartCode = objPartcode.SelectPartList())
    //        {
    //            int intCounter = 0;
    //            if (dtPartCode.Rows.Count > 0)
    //            {
    //                strPartCode = new string[dtPartCode.Rows.Count];
    //                htPartCode = new Hashtable(dtPartCode.Rows.Count);
    //                foreach (DataRow drPartCode in dtPartCode.Rows)
    //                {
    //                    strPartCode.SetValue(drPartCode["SapPartCode"], intCounter);
    //                    htPartCode.Add(drPartCode["SapPartCode"].ToString().Trim().ToLower(), drPartCode["PartID"].ToString());
    //                    intCounter++;
    //                }
    //            }
    //            else
    //            {
    //                strPartCode = new string[1];
    //                htPartCode = new Hashtable(1);
    //                strPartCode.SetValue("No Match Found", 0);
    //                htPartCode.Add("No Match Found", "-1");
    //            }
    //            return strPartCode;
    //        }
    //    }
    //    catch (System.Data.SqlClient.SqlException ex)
    //    {
    //        if (ex.Number == 1205)// 1205 = deadlock
    //        {

    //        }
    //        else
    //        {

    //        }
    //        return strPartCode;
    //    }
    //    catch (Exception ex)
    //    {
    //        return strPartCode;
    //    }
    //    finally
    //    {
    //        // Do Nothing.
    //    }
    //}

    //ForNew Requirement

    [WebMethod(EnableSession = true)]
    public DataTable BindStatesListDrop(int strCountrySelectedValue, int SelectedValue)
    {

        DataTable dt = new DataTable();
        DataRow drAdd = null;
        using (MastersData objstate = new MastersData())
        {
            //   objstate.PageIndex = 1;
            //  objstate.PageSize = 1000;
            objstate.Active = 255;
            objstate.CountryID = Convert.ToInt32(strCountrySelectedValue);
            objstate.State_Id = Convert.ToInt16(SelectedValue);
            dt = objstate.SelectStateInfo();
            drAdd = dt.NewRow();
            drAdd["StateName"] = "Select";
            drAdd["StateID"] = 0;
            dt.Rows.Add(drAdd);
            dt.AcceptChanges();
            return dt;
        }
    }

    [WebMethod(EnableSession = true)]
    public DataTable BindCityListDrop(int StrStateSelectedValue, int SelectedValue)
    {
        DataRow drAdd = null;
        DataTable dt = new DataTable();
        using (MastersData objcity = new MastersData())
        {
            //   objcity.PageIndex = 1;
            // objcity.PageSize = 1000;
            objcity.Active = 255;
            objcity.StateId = Convert.ToInt32(StrStateSelectedValue);
            dt = objcity.SelectCityInfo();
            drAdd = dt.NewRow();
            drAdd["CityName"] = "Select";
            drAdd["CityID"] = 0;
            dt.Rows.Add(drAdd);
            dt.AcceptChanges();
            return dt;
        }
    }  //Not in use till now

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCityNameByCountryID(string prefixText, int count, string contextKey)
    {
        string[] strLocationName = new string[0];
        try
        {

            MastersData objCityMaster = new MastersData();
            objCityMaster.CountryId = Convert.ToInt32(contextKey);
            objCityMaster.CityName = prefixText;
            //objCityMaster.CountryId= Convert.ToInt32(contextKey);



            Hashtable htLocalityName;

            //using (DataTable dtLocationName = objCityMaster.SelectCitiesByCountryID())
            using (DataTable dtLocationName = objCityMaster.SelectCityInfoTehsilWise())
            {
                int intCounter = 0;
                if (dtLocationName.Rows.Count > 0)
                {
                    strLocationName = new string[dtLocationName.Rows.Count];
                    htLocalityName = new Hashtable(dtLocationName.Rows.Count);
                    foreach (DataRow drPartCode in dtLocationName.Rows)
                    {
                        strLocationName.SetValue(drPartCode["CityName"], intCounter);
                        htLocalityName.Add(drPartCode["CityDesc"].ToString().Trim().ToLower(), drPartCode["CityID"].ToString());
                        intCounter++;
                    }
                }
                else
                {
                    strLocationName = new string[1];
                    htLocalityName = new Hashtable(1);
                    strLocationName.SetValue("No Match Found", 0);
                    htLocalityName.Add("No Match Found", "-1");
                }
                return strLocationName;
            }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            if (ex.Number == 1205)// 1205 = deadlock
            {

            }
            else
            {

            }
            return strLocationName;
        }
    }

    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod]
    //public string[] GetProductSAPCodeListBySAPCode(string prefixText, int count, string contextKey)
    //{
    //    string[] strPartCode = new string[0];
    //    try
    //    {
    //        clsProductMaster objPartcode = new clsProductMaster();
    //        Hashtable htPartCode;
    //        DataTable dtPartCode;
    //        objPartcode.ProductCategoryID = 0;
    //        using (dtPartCode = objPartcode.GetProductNameListByCategoryIdforAutoComplete(prefixText))
    //        {
    //            int intCounter = 0;
    //            if (dtPartCode.Rows.Count > 0)
    //            {
    //                strPartCode = new string[dtPartCode.Rows.Count];
    //                htPartCode = new Hashtable(dtPartCode.Rows.Count);
    //                foreach (DataRow drPartCode in dtPartCode.Rows)
    //                {
    //                    strPartCode.SetValue(drPartCode["ProductSAPCode"], intCounter);
    //                    htPartCode.Add(drPartCode["ProductSAPCode"].ToString().Trim().ToLower(), drPartCode["ProductID"].ToString());
    //                    intCounter++;
    //                }
    //            }
    //            else
    //            {
    //                strPartCode = new string[1];
    //                htPartCode = new Hashtable(1);
    //                strPartCode.SetValue("No Match Found", 0);
    //                htPartCode.Add("No Match Found", "-1");
    //            }
    //            return strPartCode;
    //        }
    //    }
    //    catch (System.Data.SqlClient.SqlException ex)
    //    {
    //        if (ex.Number == 1205)// 1205 = deadlock
    //        {

    //        }
    //        else
    //        {

    //        }
    //        return strPartCode;
    //    }
    //    catch (Exception ex)
    //    {
    //        return strPartCode;
    //    }
    //    finally
    //    {
    //        // Do Nothing.
    //    }
    //}
    //// added by Prashant on 10-Aug-2011.
    //[WebMethod]
    //public CascadingDropDownNameValue[] GetAllActivePartGroup(string knownCategoryValues, string category)
    //{
    //    try
    //    {
    //        DataTable dtresult = null;
    //        using (clsPartGroupMaster objPartGroup = new clsPartGroupMaster())
    //        {
    //            objPartGroup.PageIndex = 1;
    //            objPartGroup.PageSize = 500;
    //            dtresult = objPartGroup.SelectAll(1);
    //        }

    //        if (dtresult != null)
    //        {
    //            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
    //            foreach (DataRow dr in dtresult.Rows)
    //            {
    //                values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["PartGroupName"]), Convert.ToString(dr["PartGroupID"])));
    //            }
    //            return values.ToArray();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return null;
    //}

    //[WebMethod]
    //public CascadingDropDownNameValue[] GetAllActivePartSubGroup_FofpartGroupID(string knownCategoryValues, string category, string contextKey)
    //{
    //    try
    //    {
    //        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

    //        Int32 PartGroupID;
    //        if (!kv.ContainsKey("PartGroup") || !Int32.TryParse(kv["PartGroup"], out PartGroupID))
    //        {
    //            return null;
    //        }
    //        DataTable dtresult = null;
    //        using (clsPartSubGroupMaster objPartSubGroup = new clsPartSubGroupMaster())
    //        {
    //            objPartSubGroup.PageIndex = 1;
    //            objPartSubGroup.PageSize = 1000;
    //            objPartSubGroup.PartGroupID = PartGroupID;
    //            dtresult = objPartSubGroup.SelectByPartGroupID();
    //        }

    //        if (dtresult != null)
    //        {
    //            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
    //            foreach (DataRow dr in dtresult.Rows)
    //            {
    //                values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["PartSubGroupName"]), Convert.ToString(dr["PartSubGroupID"])));
    //            }
    //            return values.ToArray();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return null;
    //}

    [WebMethod]
    public CascadingDropDownNameValue[] GetAllStates(string knownCategoryValues, string category)
    {
        try
        {
            DataTable dtresult = null;

            //if (contextKey == null || contextKey == "0")
            //{
            //    contextKey = "-1#1";
            //}
            ///*#CC01 Added Start */
            //string[] Contextobj = contextKey.Split('#');


            //string StrContextKey = Contextobj[0];
            //string ContextValue = Contextobj[1];
            //Int16 ActiveStaus = 255;

            //if (ContextValue == "0")
            //{
            //    ActiveStaus = 1;
            //}
            //else if (ContextValue == "1")
            //{
            //    ActiveStaus = 255;
            //}

            /*#CC01 Added End */
            using (MastersData state = new MastersData())
            {
                // state.PageIndex = 1;
                //state.PageSize = 500;
                dtresult = state.SelectStateInfo();
            }

            if (dtresult != null)
            {
                List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
                foreach (DataRow dr in dtresult.Rows)
                {
                    values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["StateName"]), Convert.ToString(dr["StateID"])));
                }
                return values.ToArray();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return null;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string GetSTDCodeForCity(string CityId)
    {
        string strStdCode = string.Empty;
        try
        {
            using (MastersData objCityMaster = new MastersData())
            {
                objCityMaster.CityId = Convert.ToInt32(CityId);
                DataTable dtCity = objCityMaster.SelectCityInfo();
                if (dtCity.Rows.Count > 0)
                    strStdCode = Convert.ToString(dtCity.Rows[0]["STDCode"]);
                return strStdCode;
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
    }

    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod]

    //public string[] GetRegionList(string prefixText, int count, string contextKey)
    //{
    //    string[] strLocations = new string[0];
    //    clsLocalityMaster objLocation = new clsLocalityMaster();
    //    try
    //    {
    //        Hashtable htLocations;
    //        //using (DataTable dtProducts = Customer.GetProductListForAutoComplete(prefixText))
    //        using (DataTable dtProducts = objLocation.GetLocalityforAutoComplete(Convert.ToInt32(contextKey), prefixText))
    //        {
    //            int intCounter = 0;
    //            if (dtProducts.Rows.Count > 0)
    //            {
    //                strLocations = new string[dtProducts.Rows.Count];
    //                htLocations = new Hashtable(dtProducts.Rows.Count);
    //                foreach (DataRow drProducts in dtProducts.Rows)
    //                {
    //                    strLocations.SetValue(drProducts["LocalityName"], intCounter);
    //                    htLocations.Add(drProducts["LocalityName"].ToString().Trim().ToLower(), drProducts["LocalityID"].ToString());
    //                    intCounter++;
    //                }
    //            }
    //            else
    //            {
    //                strLocations = new string[1];
    //                htLocations = new Hashtable(1);
    //                strLocations.SetValue("No Match Found", 0);
    //                htLocations.Add("No Match Found", "-1");
    //            }
    //            return strLocations;
    //        }
    //    }
    //    catch (System.Data.SqlClient.SqlException ex)
    //    {
    //        if (ex.Number == 1205)// 1205 = deadlock
    //        {

    //        }
    //        else
    //        {

    //        }
    //        return strLocations;
    //    }
    //    catch (Exception ex)
    //    {

    //        return strLocations;
    //    }
    //    finally
    //    {
    //        // Do Nothing.
    //    }
    //}

    ///*#CC01: add Function For Entity Mapping Interface (start) shilpi sharma on 06-Jun-2013 */
    //[WebMethod]
    //public CascadingDropDownNameValue[] GetEntityType(string knownCategoryValues, string category)
    //{
    //    try
    //    {
    //        DataTable dtresult = null;
    //        using (clsEntityMappingTypeMaster obj = new clsEntityMappingTypeMaster())
    //        {
    //            obj.EntityMappingTypeID = 0;
    //            dtresult = obj.Select();
    //        }

    //        if (dtresult != null)
    //        {
    //            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
    //            foreach (DataRow dr in dtresult.Rows)
    //            {
    //                values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["Description"]), Convert.ToString(dr["EntityMappingTypeID"])));
    //            }
    //            return values.ToArray();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return null;
    //}
    //[WebMethod]
    //public CascadingDropDownNameValue[] GetEntityTypeRelation(string knownCategoryValues, string category)
    //{
    //    try
    //    {
    //        DataTable dtresult = null;
    //        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
    //        int EntityTypeId;
    //        if (!kv.ContainsKey("EntityType") || !Int32.TryParse(kv["EntityType"], out EntityTypeId))
    //        {
    //            return null;
    //        }
    //        using (clsEntityMappingTypeRelationMaster obj = new clsEntityMappingTypeRelationMaster())
    //        {
    //            obj.EntityMappingTypeRelationID = 0;
    //            obj.EntityMappingTypeID = EntityTypeId;
    //            dtresult = obj.Select(Convert.ToInt16(EntityTypeId));
    //        }

    //        if (dtresult != null)
    //        {
    //            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
    //            foreach (DataRow dr in dtresult.Rows)
    //            {
    //                values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["EntityTypeRelation"]), Convert.ToString(dr["EntityMappingTypeRelationID"])));
    //            }
    //            return values.ToArray();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return null;
    //}


    //[WebMethod]
    //public CascadingDropDownNameValue[] GetPrimaryEntity(string knownCategoryValues, string category, string contextKey)
    //{
    //    try
    //    {
    //        /*Added By rajesh start*/
    //        DataTable dtresult = null;
    //        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

    //        //StringDictionary ck = CascadingDropDown.ParseKnownCategoryValuesString(contextKey);

    //        int EntityTypeId;
    //        if (!kv.ContainsKey("EntityType") || !Int32.TryParse(kv["EntityType"], out EntityTypeId))
    //        {
    //            return null;
    //        }

    //        Int16 RelationId;
    //        if (!kv.ContainsKey("EntityTypeRelation") || !Int16.TryParse(kv["EntityTypeRelation"], out RelationId))
    //        {
    //            return null;
    //        }
    //        //string SomeOtherThing = "";

    //        //if (!ck.ContainsKey("SomeOtherThing") || ck["SomeOtherThing"] == null)
    //        //{
    //        //    return null;
    //        //}

    //        //using (clsTemp obj = new clsTemp())/*Added By rajesh*/
    //        using (clsEntityMappingTypeRelationMaster obj = new clsEntityMappingTypeRelationMaster())/*Commented By rajesh*/
    //        {

    //            //SomeOtherThing = ck["SomeOtherThing"];
    //            string[] Contextobj = contextKey.Split('#');
    //            string StrContextKey = Contextobj[0];
    //            string ContextValue = Contextobj[1];


    //            obj.LoginEntityId = Convert.ToInt32(StrContextKey);
    //            obj.LoginEntityTypeId = Convert.ToInt32(ContextValue);
    //            obj.PageIndex = 1;
    //            obj.PageSize = 1000;
    //            obj.EntityMappingTypeRelationID = RelationId;
    //            int PrimaryEntityID = 0;
    //            dtresult = obj.SelectByEntityMappingForLoggedInUser(Convert.ToInt16(EntityTypeId), PrimaryEntityID);
    //        }
    //        /*Added By rajesh end*/
    //        if (dtresult != null)
    //        {
    //            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
    //            foreach (DataRow dr in dtresult.Rows)
    //            {
    //                values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["CompanyDisplayName"]), Convert.ToString(dr["PrimaryEntityID"])));
    //            }
    //            return values.ToArray();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return null;
    //}

    //// Created on 30-Oct-2013, By Nitesh Kumar to retrieve the escalation category.
    //[WebMethod]
    //public CascadingDropDownNameValue[] GetAllActiveEscalationCategoryList(string knownCategoryValues, string category)
    //{
    //    try
    //    {
    //        DataTable dtresult = null;
    //        using (clsHelpdeskCategoryMaster objCategory = new clsHelpdeskCategoryMaster())
    //        {
    //            objCategory.PageIndex = 1;
    //            objCategory.PageSize = 500;
    //            dtresult = objCategory.SelectAll(1);
    //        }
    //        if (dtresult != null)
    //        {
    //            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
    //            foreach (DataRow dr in dtresult.Rows)
    //            {
    //                values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["CategoryDesc"]), Convert.ToString(dr["CategoryID"])));
    //            }
    //            return values.ToArray();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return null;
    //}

    //[WebMethod]
    //public CascadingDropDownNameValue[] GetActiveSubCategoriesForCategoryID(string knownCategoryValues, string category, string contextKey)
    //{
    //    try
    //    {
    //        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
    //        Int16 _intCategoryID;
    //        if (!kv.ContainsKey("Category") || !Int16.TryParse(kv["Category"], out _intCategoryID))
    //        {
    //            return null;
    //        }
    //        DataTable dtresult = null;
    //        using (clsHelpdeskSubcategoryMaster objSubcategory = new clsHelpdeskSubcategoryMaster())
    //        {
    //            objSubcategory.PageIndex = 1;
    //            objSubcategory.PageSize = 1000;
    //            objSubcategory.CategoryID = _intCategoryID;
    //            dtresult = objSubcategory.SelectByCategoryId(1);
    //        }

    //        if (dtresult != null)
    //        {
    //            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
    //            foreach (DataRow dr in dtresult.Rows)
    //            {
    //                values.Add(new CascadingDropDownNameValue(Convert.ToString(dr["SubCategoryDesc"]), Convert.ToString(dr["SubCategoryID"])));
    //            }
    //            if (contextKey == "-1")
    //                values.Add(new CascadingDropDownNameValue("Other", "0"));
    //            return values.ToArray();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return null;
    //}

    ///* End By Nitesh Kumar */

    ///*#CC01: add Function For Entity Mapping Interface (end) */
    ///* #CC04 : add Function For Auto Complete Dealer Name  (start) shilpi sharma on 24-Sep-2013 function added*/
    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod]
    //public string[] GetEntityNameList(string prefixText, int count, string contextKey)
    //{
    //    string[] strEntityName = new string[0];
    //    clsDealerMaster objDealer = new clsDealerMaster();
    //    try
    //    {
    //        Hashtable htEntityName;
    //        using (DataTable dtEntityName = objDealer.GetAllDealorsAccordingToname(prefixText, Convert.ToInt32(contextKey)))
    //        {
    //            int intCounter = 0;
    //            if (dtEntityName.Rows.Count > 0)
    //            {
    //                strEntityName = new string[dtEntityName.Rows.Count];
    //                htEntityName = new Hashtable(dtEntityName.Rows.Count);
    //                foreach (DataRow drEntityName in dtEntityName.Rows)
    //                {
    //                    if (Convert.ToString(drEntityName["CompanyName"]) != null)/*#CC05 :added*/
    //                    {
    //                        strEntityName.SetValue(drEntityName["CompanyName"], intCounter);
    //                        htEntityName.Add(drEntityName["CompanyName"].ToString().Trim().ToLower(), drEntityName["EntityID"].ToString());
    //                        intCounter++;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                strEntityName = new string[1];
    //                htEntityName = new Hashtable(1);
    //                strEntityName.SetValue("No Match Found", 0);
    //                htEntityName.Add("No Match Found", "-1");
    //            }
    //            return strEntityName;
    //        }
    //    }
    //    catch (System.Data.SqlClient.SqlException ex)
    //    {
    //        if (ex.Number == 1205)// 1205 = deadlock
    //        {

    //        }
    //        else
    //        {

    //        }
    //        return strEntityName;
    //    }
    //    catch (Exception ex)
    //    {

    //        return strEntityName;
    //    }
    //    finally
    //    {
    //        // Do Nothing.
    //    }
    //}
    ///*#CC04: add Function For Auto Complete (end) */


    ///* #CC02 Add Start */
    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod]
    //public string[] GetPartCodeForSapListWithID(string prefixText, int count, string contextKey)
    //{
    //    string[] strPartCode = new string[0];
    //    try
    //    {
    //        clsProductMaster objPartcode = new clsProductMaster();
    //        Hashtable htPartCode;
    //        DataTable dtPartCode;
    //        objPartcode.ProductSAPCode = prefixText;
    //        using (dtPartCode = objPartcode.SearchPartDeatils())
    //        {
    //            int intCounter = 0;
    //            if (dtPartCode.Rows.Count > 0)
    //            {
    //                strPartCode = new string[dtPartCode.Rows.Count];
    //                htPartCode = new Hashtable(dtPartCode.Rows.Count);
    //                foreach (DataRow drPartCode in dtPartCode.Rows)
    //                {
    //                    //strPartCode.SetValue((drPartCode["SapPartCode"].ToString() + "|" + drPartCode["PartID"]), intCounter);
    //                    strPartCode.SetValue((drPartCode["SapPartCode"].ToString() + "(" + drPartCode["PartName"].ToString() + ")"), intCounter);
    //                    //strPartCode.SetValue(drPartCode["SapPartCode"], intCounter);
    //                    htPartCode.Add(drPartCode["SapPartCode"].ToString().Trim().ToLower(), drPartCode["PartID"].ToString());
    //                    intCounter++;
    //                }
    //            }
    //            else
    //            {
    //                strPartCode = new string[1];
    //                htPartCode = new Hashtable(1);
    //                strPartCode.SetValue("No Match Found", 0);
    //                htPartCode.Add("No Match Found", "-1");
    //            }
    //            return strPartCode;
    //        }
    //    }
    //    catch (System.Data.SqlClient.SqlException ex)
    //    {
    //        if (ex.Number == 1205)// 1205 = deadlock
    //        {

    //        }
    //        else
    //        {

    //        }
    //        return strPartCode;
    //    }
    //    catch (Exception ex)
    //    {
    //        return strPartCode;
    //    }
    //    finally
    //    {

    //    }
    //}
    ///* #CC02 Add End */


    ///* #CC03: Added Start*/
    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod]
    //public string[] GetProductCodeForSapListWithID(string prefixText)
    //{
    //    string[] strProdCode = new string[0];
    //    try
    //    {
    //        clsProductMaster objProductcode = new clsProductMaster();
    //        Hashtable htProdCode;
    //        DataTable dtProdCode;
    //        objProductcode.ProductSAPCode = prefixText;
    //        using (dtProdCode = objProductcode.SelectProductList())
    //        {
    //            int intCounter = 0;
    //            if (dtProdCode.Rows.Count > 0)
    //            {
    //                strProdCode = new string[dtProdCode.Rows.Count];
    //                htProdCode = new Hashtable(dtProdCode.Rows.Count);
    //                foreach (DataRow drProdCode in dtProdCode.Rows)
    //                {
    //                    //strProdCode.SetValue((drProdCode["ProductSapCode"].ToString() + "|" + drProdCode["ProductID"]), intCounter);
    //                    strProdCode.SetValue((drProdCode["ProductSapCode"].ToString() + ConfigurationManager.AppSettings["SAPPartCodeAndIDSaperator"].ToString() + drProdCode["ProductID"]), intCounter);
    //                    htProdCode.Add(drProdCode["ProductSapCode"].ToString().Trim().ToLower(), drProdCode["ProductID"].ToString());
    //                    intCounter++;
    //                }
    //            }
    //            else
    //            {
    //                strProdCode = new string[1];
    //                htProdCode = new Hashtable(1);
    //                strProdCode.SetValue("No Match Found", 0);
    //                htProdCode.Add("No Match Found", "-1");
    //            }
    //            return strProdCode;
    //        }
    //    }
    //    catch (System.Data.SqlClient.SqlException ex)
    //    {
    //        if (ex.Number == 1205)// 1205 = deadlock
    //        {

    //        }
    //        else
    //        {

    //        }
    //        return strProdCode;
    //    }
    //    catch (Exception ex)
    //    {
    //        return strProdCode;
    //    }
    //    finally
    //    {

    //    }
    //}

    ///* #CC03: Added End*/
}
