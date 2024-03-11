using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;
/* Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 19 Apr 2016, Sumit Maurya, #CC01, New propety created "IsLoggedIn" and set value in function "AuthenticateUser" to check user already logged in or not.
 *  31-march-2020,Vijay Kumar Prajapati,#CC02,Added Companyname on this page.
 *  24-Apr-2023, Hema Thapa, #CC03, MySQL connections
 */
namespace DataAccess
{
    public class Authenticates : IDisposable
    {

        private string _Password;

        SqlParameter[] SqlParam;
        MySqlParameter[] SqlParam1; // #CC03 added

        private string _PasswordSalt;
        private bool _blnCheckLogin;
        private Int16 intResult;
        private Int32 intNextPasswordExpiryDays;
        private string _ConfigKey;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        public string
            ConfigKey
        {
            get { return _ConfigKey; }
            set { _ConfigKey = value; }
        }
        public Int16 Result
        {
            get { return intResult; }
            set { intResult = value; }
        }
        public Int32 NextPasswordExpiryDays
        {
            get { return intNextPasswordExpiryDays; }
            set { intNextPasswordExpiryDays = value; }
        }
        public bool CheckLogin
        {
            get { return _blnCheckLogin; }
            set { _blnCheckLogin = value; }
        }
        private string _strImg;
        public string StrImg
        {
            get { return _strImg; }
            set { _strImg = value; }
        }
        private string _strPassword;
        public string StrPassword
        {
            get { return _strPassword; }
            set { _strPassword = value; }
        }
        private string _siteurl;
        public string Siteurl
        {
            get { return _siteurl; }
            set { _siteurl = value; }
        }
        private string _WebSite;
        public string WebSite
        {
            get { return _WebSite; }
            set { _WebSite = value; }
        }
        private string _ErrorMessage;
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }


        private string PasswordSalt
        {
            get { return _PasswordSalt; }
            set { _PasswordSalt = value; }
        }

        public Int32 CompanyId { get; set; }

        /*#CC01 START ADDED*/
        private int _IsLoggedIn = 0;

        public int IsLoggedIn
        {
            get { return _IsLoggedIn; }
            set { _IsLoggedIn = value; }
        }
        /*#CC01 START END*/

        public string DecryptPassword(string cipherText, string saltValue)
        {

            try
            {
                byte[] initVectorBytes = System.Text.Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");
                byte[] saltValueBytes = System.Text.Encoding.ASCII.GetBytes(saltValue);

                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                PasswordDeriveBytes password = new PasswordDeriveBytes("zedaxis", saltValueBytes, "SHA1", 2);

                byte[] keyBytes = password.GetBytes(128 / 8);
                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(cipherTextBytes);

                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                string plainText = System.Text.Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.   
                return plainText;
            }
            catch (Exception ex1)
            {
                return string.Empty;
            }
        }

        public string EncryptPassword(string plainText, string saltValue)
        {
            byte[] initVectorBytes = System.Text.Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");
            // Must be 16 bytes
            byte[] saltValueBytes = System.Text.Encoding.ASCII.GetBytes(saltValue);
            // Encoding Password Salt Value.
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            //Encoding the string to be en
            PasswordDeriveBytes password = new PasswordDeriveBytes("zedaxis", saltValueBytes, "SHA1", 2);
            //Rfc2898DeriveBytes("zedaxis", saltValueBytes, 2)

            byte[] keyBytes = password.GetBytes(128 / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
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


        public string base64Encode(string data)
        {
            byte[] encData_byte = new byte[data.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
            string encodedData = Convert.ToBase64String(encData_byte);

            return encodedData;
        }

        public DataTable GetApplicationConfiguration()
        {
            DataTable _UserDetail = new DataTable();
            try
            {
                /* #CC03 add start */
                MySqlParameter[] objSqlParam = new MySqlParameter[2];
                objSqlParam[0] = new MySqlParameter("@p_ConfigKey", ConfigKey);
                objSqlParam[1] = new MySqlParameter("@p_CompanyId", CompanyId);
                DataTable _dt = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetApplicationConfigMaster", CommandType.StoredProcedure, objSqlParam);
           
                /* #CC03 add end */

                    /* #CC03 comment start */
                    //SqlParameter[] objSqlParam = new SqlParameter[2];
                    //objSqlParam[0] = new SqlParameter("@ConfigKey", ConfigKey);
                    //objSqlParam[0] = new SqlParameter("@CompanyId", CompanyId);
                    //DataTable _dt = DataAccess.Instance.GetTableFromDatabase("prcGetApplicationConfigMaster", CommandType.StoredProcedure, objSqlParam);
                    /* #CC03 comment end */


                    if (_dt.Rows.Count > 0 && _dt != null)
                    {
                        _UserDetail = _dt;
                    }
            }
            catch (Exception ex)
            { throw ex; }

            return _UserDetail;
        }

        public string base64Decode(string data)
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
        public string GenerateSalt(int length)
        {
            string guidResult = System.Guid.NewGuid().ToString();
            guidResult = guidResult.Replace("-", string.Empty);
            if (length <= 0 || length > guidResult.Length)
            {
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
            }
            return guidResult.Substring(0, length);
        }

        public void LoginAttemp(string loginName, short Action)
        {

            try
            {
                /* #CC03 add start */
                SqlParam1 = new MySqlParameter[2];
                SqlParam1[0] = new MySqlParameter("@p_LoginName", loginName);
                SqlParam1[1] = new MySqlParameter("@p_ActionID", Action);
                DataAccess.Instance.DBInsert_MySqlCommand("prcUpdateLoginStatus", SqlParam1);
                /* #CC03 add start */

                /* #CC03 comment start */
                //SqlParam = new SqlParameter[2];
                //SqlParam[0] = new SqlParameter("@LoginName", loginName);
                //SqlParam[1] = new SqlParameter("@ActionID", Action);
                //DataAccess.Instance.DBInsertCommand("prcUpdateLoginStatus", SqlParam);
                /* #CC03 comment end */
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlParam != null)
                {
                    SqlParam = null;
                }
            }
        }
        public short FncCheckValidUser(string UniqueID)
        {
            short IntResult = 0;
            IntResult = 0;
            try
            {

                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@UniqueID", UniqueID);
                SqlParam[1] = new SqlParameter("@Result", "");
                SqlParam[1].Direction = ParameterDirection.Output;


                DataAccess.Instance.DBInsertCommand("PrcCheckValidUser", SqlParam);
                IntResult = Convert.ToInt16(Convert.ToInt16(SqlParam[1].Value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlParam != null)
                {
                    SqlParam = null;
                }
            }
            return IntResult;
        }
        public bool AuthenticateUser(string UserLoginID, string UserPassword, ref DataSet _AuthenticateUserDetail, ref short PasLocked,/*#CC02 Added*/string UserCompanyname, int CallingMode)
        {

            bool IsAuthenticated = false;
            try
            {
                /* #CC03 add start */
                SqlParam1 = new MySqlParameter[5];
                SqlParam1[0] = new MySqlParameter("@p_LoginName", UserLoginID);
                SqlParam1[1] = new MySqlParameter("@p_ResultOut", "");
                SqlParam1[1].Direction = ParameterDirection.Output;
                SqlParam1[2] = new MySqlParameter("@p_ChkforExistinglogin", CheckLogin);
                SqlParam1[3] = new MySqlParameter("@p_UserCompanyname", UserCompanyname);
                SqlParam1[4] = new MySqlParameter("@p_CallingMode", CallingMode); // #CC03 added
                //DataSet _dt = DataAccess.Instance.GetDataSetFromMySqlDatabase("prcAuthenticateUser", CommandType.StoredProcedure, SqlParam1);
                DataSet _dt = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcAuthenticateUser", CommandType.StoredProcedure, SqlParam1);

                PasLocked = Convert.ToInt16(Convert.ToInt16(SqlParam1[1].Value));
                if (PasLocked == 1)
                {
                    if (_dt != null && _dt.Tables[0].Rows.Count > 0)
                    {
                        /* #CC01 Add Start */
                        IsLoggedIn = Convert.ToInt16(Convert.ToInt16(_dt.Tables[0].Rows[0]["isLogin"].ToString()));
                        if (IsLoggedIn == 1)
                        {
                            return IsAuthenticated;
                        }
                        /* #CC01 End Start */
                        string cnfPassword = DecryptPassword(Convert.ToString(_dt.Tables[0].Rows[0]["Password"]), Convert.ToString(_dt.Tables[0].Rows[0]["PasswordSalt"]));

                        if (CompareString(Convert.ToString(_dt.Tables[0].Rows[0]["LoginName"]).Trim(), UserLoginID.Trim(), true) && CompareString(cnfPassword, UserPassword, false))
                        {
                            IsAuthenticated = true;
                            _AuthenticateUserDetail = _dt;
                            LoginAttemp(UserLoginID, 2);
                        }
                        else
                        {
                            LoginAttemp(UserLoginID, 1);
                        }

                    }
                }
                /* #CC03 add end */

                #region SQl query comment
                /* #CC03 comment start */

                //SqlParam = new SqlParameter[4];/*#CC02 Added 4 instead of 3*/
                //SqlParam[0] = new SqlParameter("@LoginName", UserLoginID);
                //SqlParam[1] = new SqlParameter("@ResultOut", "");
                //SqlParam[1].Direction = ParameterDirection.Output;
                //SqlParam[2] = new SqlParameter("@ChkforExistinglogin", CheckLogin);
                //SqlParam[3] = new SqlParameter("@UserCompanyname", UserCompanyname);
                //DataSet _dt = DataAccess.Instance.GetDataSetFromDatabase("prcAuthenticateUser", CommandType.StoredProcedure, SqlParam);



                //PasLocked = Convert.ToInt16(Convert.ToInt16(SqlParam[1].Value));
                //if (PasLocked == 1)
                //{
                //    if (_dt != null && _dt.Tables[0].Rows.Count > 0)
                //    {
                //        /* #CC01 Add Start */
                //        IsLoggedIn = Convert.ToInt16(Convert.ToInt16(_dt.Tables[0].Rows[0]["isLogin"].ToString()));
                //        if (IsLoggedIn == 1)
                //        {
                //            return IsAuthenticated;
                //        }
                //        /* #CC01 End Start */
                //        string cnfPassword = DecryptPassword(Convert.ToString(_dt.Tables[0].Rows[0]["Password"]), Convert.ToString(_dt.Tables[0].Rows[0]["PasswordSalt"]));

                //        if (CompareString(Convert.ToString(_dt.Tables[0].Rows[0]["LoginName"]).Trim(), UserLoginID.Trim(), true) && CompareString(cnfPassword, UserPassword, false))
                //        {
                //            IsAuthenticated = true;
                //            _AuthenticateUserDetail = _dt;
                //            LoginAttemp(UserLoginID, 2);
                //        }
                //        else
                //        {
                //            LoginAttemp(UserLoginID, 1);
                //        }

                //    }
                //}

                /* #CC03 comment end */
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlParam != null)
                {
                    SqlParam = null;
                }
            }
            return IsAuthenticated;
        }

        private bool CompareString(string strA, string strB, bool ignoreCase)
        {
            return (string.Compare(strA, strB, ignoreCase) == 0);
        }
        public Int16 ChangePassword(string UserLoginID, string UserNewEncryptedPassword, string NewPasswordSalt)
        {
            // 0: Success, 1:  Old three password
            try
            {
                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[8];
                objSqlParam[0] = new MySqlParameter("@p_LoginName", UserLoginID);
                objSqlParam[1] = new MySqlParameter("@p_Password", UserNewEncryptedPassword);
                objSqlParam[2] = new MySqlParameter("@p_PasswordSalt", NewPasswordSalt);
                objSqlParam[3] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@p_PasswordExpiryDays", NextPasswordExpiryDays);
                objSqlParam[5] = new MySqlParameter("@p_ErrorMessage", MySqlDbType.VarChar, 200);
                objSqlParam[5].Direction = ParameterDirection.Output;
                objSqlParam[6] = new MySqlParameter("@p_strPassword", StrPassword);
                objSqlParam[7] = new MySqlParameter("@p_CompanyId", CompanyId);
                DataAccess.Instance.DBInsert_MySqlCommand("prcChangeUserPassword", objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[8];
                //objSqlParam[0] = new SqlParameter("@LoginName", UserLoginID);
                //objSqlParam[1] = new SqlParameter("@Password", UserNewEncryptedPassword);
                //objSqlParam[2] = new SqlParameter("@PasswordSalt", NewPasswordSalt);
                //objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@PasswordExpiryDays", NextPasswordExpiryDays);
                //objSqlParam[5] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                //objSqlParam[5].Direction = ParameterDirection.Output;
                //objSqlParam[6] = new SqlParameter("@strPassword", StrPassword);
                //objSqlParam[7] = new SqlParameter("@CompanyId", CompanyId);
                //DataAccess.Instance.DBInsertCommand("prcChangeUserPassword", objSqlParam);
                /* #CC02 comment end */
                ErrorMessage = objSqlParam[5].Value.ToString();
                Result = Convert.ToInt16(objSqlParam[3].Value);

            }
            catch (Exception ex)
            { throw ex; }
            return Result;
        }
        public DataTable RemindPassword(string LoginName)
        {
            DataTable _UserDetail = new DataTable();
            try
            {
                SqlParameter[] objSqlParam = new SqlParameter[2];
                objSqlParam[0] = new SqlParameter("@LoginName", LoginName);
                objSqlParam[1] = new SqlParameter("@ResultOut", "");
                objSqlParam[1].Direction = ParameterDirection.Output;
                DataTable _dt = DataAccess.Instance.GetTableFromDatabase("prcAuthenticateUser", CommandType.StoredProcedure, objSqlParam);
                if (_dt != null && _dt.Rows.Count > 0)
                {

                    _UserDetail = _dt;
                }



            }
            catch (Exception ex)
            { throw ex; }

            return _UserDetail;
        }
        public DataSet RemindUserPasswordLog(string LoginName)
        {
            DataSet _UserDetail = new DataSet();
            try
            {

                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[2];
                objSqlParam[0] = new MySqlParameter("@p_LoginName", LoginName);
                objSqlParam[1] = new MySqlParameter("@p_CompanyId", CompanyId);
                DataSet _ds = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcRemindUserPasswordLog", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[2];
                //objSqlParam[0] = new SqlParameter("@LoginName", LoginName);
                //objSqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
                //DataSet _ds = DataAccess.Instance.GetDataSetFromDatabase("prcRemindUserPasswordLog", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                if (_ds.Tables[0].Rows.Count > 0 && _ds != null)
                {
                    _UserDetail = _ds;
                }
            }
            catch (Exception ex)
            { throw ex; }

            return _UserDetail;
        }

        public string RemindPassword(string UserLoginID, string EmailID, ref DataTable _UserDetail)
        {

            string returnPassword = string.Empty;
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@UserLoginID", UserLoginID);
                SqlParam[1] = new SqlParameter("@EmailID", EmailID);


                DataTable _dt = DataAccess.Instance.GetDataSetFromDatabase("prcRetriveUserPasswordWithDetails", CommandType.StoredProcedure, SqlParam).Tables[0];
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    returnPassword = DecryptPassword(Convert.ToString(_dt.Rows[0]["Password"]), Convert.ToString(_dt.Rows[0]["PasswordSalt"]));

                    _UserDetail = _dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlParam != null)
                {
                    SqlParam = null;
                }
            }
            return returnPassword;
        }


        public bool AuthenticateUserforRegistraion(string UserLoginID)
        {

            bool IsAuthenticateforRegistraion = false;
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@LoginName", UserLoginID);
                DataSet dt = DataAccess.Instance.GetDataSetFromDatabase("prcAuthenticateUserForRegistraion", CommandType.StoredProcedure, SqlParam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    IsAuthenticateforRegistraion = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlParam != null)
                {
                    SqlParam = null;
                }
            }
            return IsAuthenticateforRegistraion;
        }
        public short Saveregisteration(string Firstname, string Lastname, string Displayname, string LoginName, string StrPSalt, string StrPwd, string EmailId, string VUniquno)
        {
            short IntResult = 0;
            IntResult = 0;

            try
            {
                SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@Firstname", Firstname);
                SqlParam[1] = new SqlParameter("@Lastname", Lastname);
                SqlParam[2] = new SqlParameter("@Displayname", Displayname);
                SqlParam[3] = new SqlParameter("@LoginName", LoginName);
                SqlParam[4] = new SqlParameter("@StrPSalt", StrPSalt);
                SqlParam[5] = new SqlParameter("@StrPwd", StrPwd);
                SqlParam[6] = new SqlParameter("@EmailId", EmailId);
                SqlParam[7] = new SqlParameter("@VUniquno", VUniquno);
                SqlParam[8] = new SqlParameter("@Result", "");
                SqlParam[8].Direction = ParameterDirection.Output;


                DataAccess.Instance.DBInsertCommand("PrcSaveregistrationUser", SqlParam);
                if (Convert.ToInt16(SqlParam[8].Value) != 0)
                {
                    IntResult = Convert.ToInt16(Convert.ToInt16(SqlParam[8].Value));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlParam != null)
                {
                    SqlParam = null;
                }
            }
            return IntResult;
        }

        #region dispose
        //Call Dispose to free resources explicitly
        private bool IsDisposed = false;
        public void Dispose()
        {
            //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
            // in next line.
            Dispose(true);
            //If dispose is called already then say GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        ~Authenticates()
        {
            //Pass false as param because no need to free managed resources when you call finalize it
            //  will be done
            //by GC itself as its work of finalize to manage managed resources.
            Dispose(false);
        }

        //Implement dispose to free resources
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                // Released unmanaged Resources
                if (disposedStatus)
                {
                    // Released managed Resources
                }
            }
        }

        #endregion
    }
}
