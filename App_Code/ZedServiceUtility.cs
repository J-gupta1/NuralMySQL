using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Web.Security;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Globalization;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Net;
using System.Data;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;


namespace ZedService.Utility
{
    /// <summary>
    /// Summary description for ZedServiceUtility
    /// </summary>
    public class ZedServiceUtil
    {
        public ZedServiceUtil()
        {
        }

        #region PASSWORD Methods

        public static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }
        public static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd =
             FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
            return hashedPwd;
        }
        public static string GeneratePassword()
        {
            string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
            string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            string PASSWORD_CHARS_NUMERIC = "23456789";
            string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

            char[][] charGroups = new char[][]{
            PASSWORD_CHARS_LCASE.ToCharArray(),
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
            PASSWORD_CHARS_SPECIAL.ToCharArray()};

            int[] charsLeftInGroup = new int[charGroups.Length];
            for (int i = 0; i < charsLeftInGroup.Length; i++)
            {
                charsLeftInGroup[i] = charGroups[i].Length;
            }

            int[] leftGroupsOrder = new int[charGroups.Length];
            for (int i = 0; i < leftGroupsOrder.Length; i++)
            {
                leftGroupsOrder[i] = i;
            }

            byte[] randomBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            int seed = (randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3];

            Random random = new Random(seed);
            char[] password = new char[8];
            int nextCharIdx; int nextGroupIdx; int nextLeftGroupsOrderIdx; int lastCharIdx;
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            for (int i = 0; i < password.Length; i++)
            {
                if (lastLeftGroupsOrderIdx == 0) { nextLeftGroupsOrderIdx = 0; }
                else { nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx); }
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                if (lastCharIdx == 0)
                {
                    nextCharIdx = 0;
                }
                else
                {
                    nextCharIdx = random.Next(0, lastCharIdx + 1);
                }

                password[i] = charGroups[nextGroupIdx][nextCharIdx];
                if (lastCharIdx == 0)
                {
                    charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                }
                else
                {
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx]--;
                }

                if (lastLeftGroupsOrderIdx == 0)
                {
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                }
                else
                {
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    lastLeftGroupsOrderIdx--;
                }
            }
            return new string(password);
        }
        public static string GenerateSalt(int length)
        {
            string guidResult = System.Guid.NewGuid().ToString();
            guidResult = guidResult.Replace("-", string.Empty);
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

            return guidResult.Substring(0, length);
        }
        public static string EncryptPassword(string plainText, string saltValue)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");// Must be 16 bytes
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);// Encoding Password Salt Value.

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);//Encoding the string to be en
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            "zedaxis",
                                                            saltValueBytes,
                                                            "SHA1",
                                                            2);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(128 / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }
        public static string DecryptPassword(string cipherText, string saltValue)
        {
            try
            {
                // Convert strings defining encryption key characteristics into byte
                // arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                // First, we must create a password, from which the key will be 
                // derived. This password will be generated from the specified 
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                                "zedaxis",
                                                                saltValueBytes,
                                                                "SHA1",
                                                                2);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(128 / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                                 keyBytes,
                                                                 initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                              decryptor,
                                                              CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                           0,
                                                           plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. 
                // Let us assume that the original plaintext string was UTF8-encoded.
                string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                           0,
                                                           decryptedByteCount);

                // Return decrypted string.   
                return plainText;
            }
            catch
            {
                return "";
            }
        }
        protected string CreateSaltedPasswordHash(string password)
        {
            // Generate random salt string
            RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[16];
            csp.GetNonZeroBytes(saltBytes);
            string saltString = Convert.ToBase64String(saltBytes);
            // Append the salt string to the password
            string saltedPassword = password + saltString;
            // Hash the salted password
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile
            (saltedPassword, "SHA1");
            // Append the salt to the hash
            string saltedHash = hash + saltString;
            return saltedHash;
        }
        protected bool ValidatePassword(string password, string saltedHash)
        {
            // Extract hash and salt string
            string saltString = saltedHash.Substring(saltedHash.Length - 24);
            string hash1 = saltedHash.Substring(0, saltedHash.Length - 24);
            // Append the salt string to the password
            string saltedPassword = password + saltString;
            // Hash the salted password
            string hash2 = FormsAuthentication.HashPasswordForStoringInConfigFile
            (saltedPassword, "SHA1");
            // Compare the hashes
            return (hash1.CompareTo(hash2) == 0);
        }

        #endregion

        #region ENCODER/DECODER

        public string Base64Decode(string data)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(data);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        public string Base64Encode(string data)
        {
            byte[] encData_byte = new byte[data.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }

        #endregion

        #region COMMENTED

        //public static DateTime GetBusinessDaysAfter(DateTime dt, int days)
        //{
        //    for (int i = 1; i < days; i++)
        //    {
        //        dt = dt.AddDays(1.0);
        //        if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
        //            days++;
        //    }

        //    return dt;
        //}
        //public string strDateFormat(string sDate)
        //{
        //    string[] str = sDate.Split(new char[] { '/' });
        //    try
        //    {
        //        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        //        return str[0].ToString() + "-" + dtfi.GetAbbreviatedMonthName(Convert.ToInt32(str[1])); //GetMonthName(Convert.ToInt16(str[1]));
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}
        //public string GetMonthName(short MonthNo)
        //{
        //    string sMonthName = string.Empty;
        //    switch (MonthNo)
        //    {
        //        case 1: sMonthName = "Jan"; break;
        //        case 2: sMonthName = "Feb"; break;
        //        case 3: sMonthName = "Mar"; break;
        //        case 4: sMonthName = "Apr"; break;
        //        case 5: sMonthName = "May"; break;
        //        case 6: sMonthName = "Jun"; break;
        //        case 7: sMonthName = "Jul"; break;
        //        case 8: sMonthName = "Aug"; break;
        //        case 9: sMonthName = "Sep"; break;
        //        case 10: sMonthName = "Oct"; break;
        //        case 11: sMonthName = "Nov"; break;
        //        case 12: sMonthName = "Dec"; break;
        //    }
        //    return sMonthName;
        //} 

        #endregion

        #region Miscellaneous

        public static void GridFormatBinding(GridView grdToFormat, string rowToColor)
        {
            foreach (GridViewRow grow in grdToFormat.Rows)
            {
                if (grow.RowType == DataControlRowType.DataRow)
                {
                    HtmlTableRow grdTblRow = (HtmlTableRow)grow.FindControl(rowToColor);
                    if (grdTblRow != null)
                    {
                        grdTblRow.Attributes.Add("onMouseOver", "changeColor('" + grdTblRow.ClientID + "');");
                        grdTblRow.Attributes.Add("onMouseOut", "resetColor('" + grdTblRow.ClientID + "');");
                    }
                    else
                    {
                        grow.Attributes.Add("onMouseOver", "changeColor('" + grow.ClientID + "');");
                        grow.Attributes.Add("onMouseOut", "resetColor('" + grow.ClientID + "');");
                    }
                }
            }
        }
        public static bool ContainsWord(String word, String phrase)
        {
            bool _vReturn = false;
            for (int i = 0; i < phrase.Split(' ').Length; i++)
            {
                if (phrase.Split(' ')[i].ToString() == word)
                {
                    _vReturn = true;
                }
                else
                {
                    _vReturn = false;
                }
            }
            return _vReturn;
        }

        #endregion

        #region SMS Integration

        public void IsValidSMS(string SMSContent, ArrayList arrAbusiveWords, out bool AbusiveContent, out string arrContainAbsiveWords)
        {
            AbusiveContent = false;
            arrContainAbsiveWords = string.Empty;
            bool _AbusiveContent = AbusiveContent;
            for (int i = 0; i < arrAbusiveWords.Count; i++)
            {
                //if (SMSContent.ToLower().Contains(Convert.ToString(arrAbusiveWords[i]).ToLower()))
                if (ContainsWord(Convert.ToString(arrAbusiveWords[i]).ToLower(), SMSContent.ToLower()))
                {
                    arrContainAbsiveWords += Convert.ToString(arrAbusiveWords[i]) + ",";
                }
            }
            if (arrContainAbsiveWords.Length > 0) { AbusiveContent = true; }

        }
        public bool SendSMS(string PrimaryMobileNo, string SMSReminderText)
        {
            bool returnValue = false;
            string strSMSURL = string.Empty;

            if (PrimaryMobileNo.Trim().Length > 0 && SMSReminderText.Trim().Length > 0)
            {
                string sendSMS = Convert.ToString(ConfigurationManager.AppSettings["sendSMS"]).Trim();
                if (sendSMS.Equals("1"))
                {
                    string smsURL = Convert.ToString(ConfigurationManager.AppSettings["smsURL"]).Trim();
                    string smsUser = Convert.ToString(ConfigurationManager.AppSettings["smsUser"]).Trim();
                    string smsPassword = Convert.ToString(ConfigurationManager.AppSettings["smsPassword"]).Trim();
                    string smsSID = Convert.ToString(ConfigurationManager.AppSettings["smsSID"]).Trim();
                    string smsText = HttpContext.Current.Server.UrlEncode(SMSReminderText.Trim());
                    string sCustomer = string.Empty, sAddress = string.Empty, sPhone = string.Empty, sSeries = string.Empty, SleadType = string.Empty;


                    //strSMSURL = smsURL + "?user=" + smsUser + "&pwd=" + smsPassword +
                    //"&from=" + smsSID + "&to=" + PrimaryMobileNo + "&msg=" + smsText;  

                    strSMSURL = smsURL + "?MSISDN=" + PrimaryMobileNo + "&TEXT=" + smsText;



                    string _includeProxy = Convert.ToString(ConfigurationManager.AppSettings["IncludeProxy"]).Trim();
                    string proxyHost = Convert.ToString(ConfigurationManager.AppSettings["proxyHost"]).Trim();
                    int proxyPort = 80;
                    try { proxyPort = Convert.ToInt32(ConfigurationManager.AppSettings["proxyPort"].Trim()); }
                    catch { }

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strSMSURL);
                    if (_includeProxy.Equals("1"))
                    {
                        WebProxy prxy = new WebProxy(proxyHost, proxyPort);
                        req.Proxy = prxy; req.Method = "GET";
                    }

                    StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
                    string strSMSResponse = stIn.ReadToEnd();

                    returnValue = true;
                }
            }
            return returnValue;

        }

        #endregion
        public static string siteurl = ConfigurationManager.AppSettings["siteurl"].ToString();
        public static string ConvertDateFormat(DateTime dtValue)
        {
            string result = string.Empty;
            if (dtValue != null)
            {
                result = string.Concat(dtValue.Date, "/", dtValue.Month, "/", dtValue.Year);
            }
            return result;
        }

        public static void DeleteFiles(String Dirpath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Dirpath);
            FileInfo[] fileInfo = dirInfo.GetFiles("*.xlsx*", SearchOption.AllDirectories);

            for (int i = 0; i < fileInfo.Length; i++)
            {
                TimeSpan ts = DateTime.Now - Convert.ToDateTime(fileInfo[i].LastWriteTime);
                if (ts.Hours > 2)
                {
                    try
                    {
                        fileInfo[i].Delete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public static void ExportToExecl(DataSet objDs, string FilenameToExport)
        {
            string[] SheetName = null;
            OpenXMLExcel xl = new OpenXMLExcel();
            string ExportFileLocation = HttpContext.Current.Server.MapPath("~") + "/Excel/Download/";
            string TempLocation = HttpContext.Current.Server.MapPath("~") + "/Excel/Templates/";
            string strExcelFileName = FilenameToExport + System.DateTime.Now.Ticks + ".xlsx";
            //DeleteFiles(ExportFileLocation);
            xl.ExportDataTableV2(objDs, ExportFileLocation, TempLocation + "BlankExportTemplate.xlsx", strExcelFileName, true, 1, SheetName);
            #region Not In Use
            //Newly Commented By Sushil
            /*xl.ExportDataTable(objDs, ExportFileLocation, TempLocation + "uploaddownloadTemp.xlsx", strExcelFileName, true, 1);
            
            string path = ExportFileLocation + strExcelFileName;
            HttpContext.Current.Response.Redirect(siteurl + "frmdownload.aspx?path=" + path + "&fname=" + FilenameToExport + System.DateTime.Now.Ticks.ToString(), false);*/
            //Newly Commented By Sushil
            
            //HttpContext.Current.Response.Redirect(siteurl + "/frmdownload.aspx?path=" + path + "&fname=" + FilenameToExport, false);

            //HttpContext.Current.Server.Execute(HttpContext.Current.Request.ApplicationPath + "/frmdownload.aspx?path=" + path);
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Charset = "";
            //string type = "application/vnd.ms-excel";
            //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + FilenameToExport + ".xlsx");

            //HttpContext.Current.Response.ContentType = type;
            //HttpContext.Current.Response.WriteFile(path);
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            #endregion
        }
        //Newly aaded by- 11-Apr-2013, Sushil Kumar Singh
        public static void ExportToExecl(DataSet objDs, string FilenameToExport,int TotalSheets, string[] SheetName)
        {
            if (objDs.Tables.Count != TotalSheets || objDs.Tables.Count != SheetName.Length || TotalSheets != SheetName.Length)
            {
                return;
            }
            OpenXMLExcel xl = new OpenXMLExcel();
            string ExportFileLocation = HttpContext.Current.Server.MapPath("~") + "/Excel/Download/";
            string TempLocation = HttpContext.Current.Server.MapPath("~") + "/Excel/Templates/";
            string strExcelFileName = FilenameToExport + System.DateTime.Now.Ticks + ".xlsx";
            xl.ExportDataTableV2(objDs, ExportFileLocation, TempLocation + "BlankExportTemplate.xlsx", strExcelFileName, true, TotalSheets, SheetName);
        }
        
        public static string SerializeToJsonString(Dictionary<string, object> value)
        {
            string strResult = string.Empty;

            if (value == null || value.Count == 0) return strResult;

            using (MemoryStream stream1 = new MemoryStream())
            {
                DataContractJsonSerializer serialize = new DataContractJsonSerializer(typeof(Dictionary<string, object>));
                serialize.WriteObject(stream1, value);
                stream1.Position = 0;
                StreamReader sr = new StreamReader(stream1);
                strResult = sr.ReadToEnd();
                stream1.Close();
            }

            return strResult;
        }
        public static Dictionary<string, object> DeserializeFromJsonString(string value)
        {
            Dictionary<string, object> data = null;

            if (value == string.Empty) return data;

            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(value)))
            {
                DataContractJsonSerializer deserialize = new DataContractJsonSerializer(typeof(Dictionary<string, object>));
                data = deserialize.ReadObject(ms) as Dictionary<string, object>;
                ms.Close();
            }

            return data;
        }

        public static string GetFormattedTime(object longTime)
        {
            string strformattedTime = string.Empty;
            try
            {
                if (longTime != null && longTime != DBNull.Value)
                {
                    strformattedTime = (new DateTime(((TimeSpan)longTime).Ticks).ToString("hh:mm tt"));
                }
            }
            catch { }
            return strformattedTime;
        }

        public static string GetUploadFilePath(DateTime DateCreated, string FileName)
        {
            StringBuilder sbReturnString = new StringBuilder(string.Empty);
            try
            {
                if (DateCreated == null || DateCreated <= DateTime.MinValue)
                    return sbReturnString.ToString();

                sbReturnString.Append("../DownloadUpload/WarrantyCard/");
                sbReturnString.Append(String.Format("{0:yyyy}", DateCreated));
                sbReturnString.Append(string.Concat("/", String.Format("{0:MM}", DateCreated)));
                sbReturnString.Append(string.Concat("/", String.Format("{0:dd}", DateCreated)));

                if (!string.IsNullOrEmpty(FileName))
                    sbReturnString.Append(string.Concat("/", FileName));

                return sbReturnString.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string uploadFile(System.Web.HttpPostedFile txtFile)
        {
            try
            {
                string strFileName = null;
                string strFileNamePath = null;
                string strFileFolder = null;
                string strExtension = null;

                strFileFolder = System.Web.HttpContext.Current.Server.MapPath("~") + "/uploaddownload/UploadPersistent/POPScan/";
                strFileName = System.IO.Path.GetFileName(txtFile.FileName);
                strExtension = System.IO.Path.GetExtension(strFileName);
                DateTime myDate = DateTime.Now;
                string myTime = ((((DateTime.Now.Year.ToString() + "_") + DateTime.Now.Month.ToString() + "_") + DateTime.Now.Day.ToString() + "_") + DateTime.Now.Hour.ToString() + "_") + DateTime.Now.Second.ToString();
                strFileNamePath = strFileFolder + myTime + strFileName.Replace(" ", "");
                txtFile.SaveAs(strFileNamePath);
                return myTime + strFileName.Replace(" ", "");
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string GetUploadFilePathV2(DateTime DateCreated, string FileName)
        {
            StringBuilder sbReturnString = new StringBuilder(string.Empty);
            try
            {
                if (DateCreated == null || DateCreated <= DateTime.MinValue)
                    return sbReturnString.ToString();

                sbReturnString.Append("../Excel/Upload/SAPIntegration/UploadedPdfInvoice/");
                sbReturnString.Append(String.Format("{0:yyyy}", DateCreated));
                sbReturnString.Append(string.Concat("/", String.Format("{0:MM}", DateCreated)));
                sbReturnString.Append(string.Concat("/", String.Format("{0:dd}", DateCreated)));

                if (!string.IsNullOrEmpty(FileName))
                    sbReturnString.Append(string.Concat("/", FileName));

                return sbReturnString.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
