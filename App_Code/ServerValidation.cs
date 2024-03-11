#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Manish chitransh
* Role : Software Engineer
* Module : Luminous
* FileName : Validate.cs
* Description : Component Class Validate (For server side validation of fields)
* ====================================================================================================
*/
#endregion
using System;
using System.IO;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.Configuration;
using System.Collections;
using System.Web.Security;
using System.Configuration;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Net;
using Microsoft.VisualBasic;

/// <summary>
/// Summary description for Validate
/// </summary>
namespace BussinessLogic
{
    public class ServerValidation
    {

        public ServerValidation()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public const string isAlphaNumericPatern = @"^[a-zA-Z0-9]*$";
        public static string isMobileNoPatern1 = @"^[0-9]{11}$";
        public static string isMobileNoPatern2 = @"^[0-9]{10}$";
        public static string isPinNoPatern = @"^[0-9]{6}$";
        public static string isValidPatern = @"^\d+$";
        private static Regex _isDecimal = new Regex(@"^(\d|,)*\.?\d*$");
        private static Regex _isNumber = new Regex(@"^[0-9]+$");

        /* Return 0:Valid, 1:mandatory, 2:invalid */

        public static Int16 IsValidEmail(string vemail, string strChkExpression, bool IsMandatory)
        {
            if (IsMandatory)
            {
                if (vemail.Trim().Length <= 0)
                { return 1; }
            }
            if (vemail.Trim().Length <= 0)
            { return 0; }
            else
            { if (System.Text.RegularExpressions.Regex.IsMatch(vemail, "\\w+([-+.]\\w+)*@" + strChkExpression)) { return 0; } else { return 2; } }
        }
        public static Int16 IsValidEmail(string vemail, bool IsMandatory)
        {
            try
            {
                string strRegexPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                           @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                if (IsMandatory)
                {
                    if (vemail.Trim().Length <= 0)
                    { return 1; }
                }

                if (vemail.Trim().Length <= 0)
                { return 0; }
                else
                { if (System.Text.RegularExpressions.Regex.IsMatch(vemail, strRegexPattern)) { return 0; } else { return 2; } }

            }
            catch { return 2; }
        }
        public static  bool IsDate(object theValue)
        {
            try
            { Convert.ToDateTime(theValue); return true; }
            catch
            { return false; }
        }
        public static Int16 IsDate(object theValue, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(theValue).Trim().Length <= 0)
                    { return 1; }
                }
                if (Convert.ToString(theValue).Trim().Length > 0)
                {
                    Convert.ToDateTime(theValue); return 0;
                }
                else
                { return 0; }
            }
            catch
            { return 2; }
        }
        protected bool IsDate(string strDateToValidate, bool getCurrDateIFNotValid, out string strGetValidDate)
        {
            strGetValidDate = string.Empty; bool returnValue = false;
            try
            { strGetValidDate = Convert.ToDateTime(strDateToValidate).ToShortDateString(); returnValue = true; }
            catch
            {
                if (getCurrDateIFNotValid)
                { strGetValidDate = DateTime.Now.ToShortDateString(); returnValue = true; }
                else { returnValue = false; }
            }
            return returnValue;
        }
        public static Int16 IsValidDateOfBirth(object theValue, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(theValue).Trim().Length <= 0)
                    { return 1; }
                }

                if (Convert.ToString(theValue).Trim().Length <= 0)
                { return 0; }
                else
                {
                    Convert.ToDateTime(theValue);
                    if (Convert.ToDateTime(theValue).AddYears(4).Year > System.DateTime.Now.Year)
                    { return 2; }
                    else
                    { return 0; }
                }
            }
            catch
            { return 2; }
        }

        public static bool IsInteger(object objTmp)
        {
            bool flag = false;
            try
            {
                int xyz = Convert.ToInt32(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public static Int16 IsInteger(object objTmp, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(objTmp).Trim().Length <= 0)
                    { return 1; }

                }
                if (Convert.ToString(objTmp).Trim().Length <= 0)
                { return 0; }
                else
                {
                    int xyz = Convert.ToInt32(objTmp); return 0;
                }

            }
            catch
            {
                return 2;
            }
        }
        public static bool IsInteger(string theValue)
        {
            Match m = _isNumber.Match(theValue);
            return m.Success;
        }

        public static bool IsDecimal(object objTmp)
        {
            bool flag = false;
            try
            {
                decimal xyz = Convert.ToDecimal(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public static Int16 IsDecimal(object objTmp, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(objTmp).Trim().Length <= 0)
                    { return 1; }

                }
                if (Convert.ToString(objTmp).Trim().Length <= 0)
                { return 0; }
                else
                {
                    decimal xyz = Convert.ToDecimal(objTmp); return 0;
                }
            }
            catch
            {
                return 2;
            }

        }
        public static bool IsDecimal(string theValue)
        {
            Match D = _isDecimal.Match(theValue);
            return D.Success;
        }


        public static bool IsDouble(object objTmp)
        {
            bool flag = false;
            try
            {
                double xyz = Convert.ToDouble(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public static Int16 IsDouble(object objTmp, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(objTmp).Trim().Length <= 0)
                    { return 1; }

                }
                if (Convert.ToString(objTmp).Trim().Length <= 0)
                { return 0; }
                else
                {
                    double xyz = Convert.ToDouble(objTmp); return 0;
                }
            }
            catch
            {
                return 2;
            }

        }

        public static bool IsBigInt(object objTmp)
        {
            bool flag = false;
            try
            {
                long xyz = Convert.ToInt64(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public static bool IsSmallint(object objTmp)
        {
            bool flag = false;
            try
            {
                int xyz = Convert.ToInt16(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public static Int16 IsSmallint(object objTmp, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(objTmp).Trim().Length <= 0)
                    { return 1; }

                }
                if (Convert.ToString(objTmp).Trim().Length <= 0)
                { return 0; }
                else
                {
                    int xyz = Convert.ToInt16(objTmp); return 0;
                }

            }
            catch
            {
                return 2;
            }
        }
        public static Int16 IsBigInt(object objTmp, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(objTmp).Trim().Length <= 0)
                    { return 1; }

                }
                if (Convert.ToString(objTmp).Trim().Length <= 0)
                { return 0; }
                else
                {
                    long xyz = Convert.ToInt64(objTmp); return 0;
                }
            }
            catch
            {
                return 2;
            }

        }

        public static Int16 IsAlphaNumeric(string strToCheck, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(strToCheck).Trim().Length <= 0)
                    { return 1; }

                }
                if (Convert.ToString(strToCheck).Trim().Length <= 0)
                { return 0; }
                else
                {
                    if (Regex.IsMatch(strToCheck, isAlphaNumericPatern))
                    { return 0; }
                    else { return 2; }
                }
            }
            catch { return 2; }
        }
        public static Int16 IsMobileNo(string strToCheck, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(strToCheck).Trim().Length <= 0)
                    { return 1; }

                }
                if (Convert.ToString(strToCheck).Trim().Length <= 0)
                { return 0; }
                else
                {
                    if ((Regex.IsMatch(strToCheck, isMobileNoPatern1)) || (Regex.IsMatch(strToCheck, isMobileNoPatern2)))
                    { return 0; }
                    else { return 2; }
                }
            }
            catch { return 2; }
        }
        public static Int16 IsPinCode(string strToCheck, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(strToCheck).Trim().Length <= 0)
                    { return 1; }

                }
                if (Convert.ToString(strToCheck).Trim().Length <= 0)
                { return 0; }
                else
                {
                    if (Regex.IsMatch(strToCheck, isPinNoPatern))
                    { return 0; }
                    else { return 2; }
                }
            }
            catch { return 2; }
        }
        public static Int16 IsValidContactCode(string strToCheck, bool IsMandatory)
        {
            try
            {
                if (IsMandatory)
                {
                    if (Convert.ToString(strToCheck).Trim().Length <= 0)
                    { return 1; }

                }
                if (Convert.ToString(strToCheck).Trim().Length <= 0)
                { return 0; }
                else
                {
                    if (Regex.IsMatch(strToCheck, isValidPatern))
                    { return 0; }
                    else { return 2; }
                }
            }
            catch { return 2; }
        }

    }
}