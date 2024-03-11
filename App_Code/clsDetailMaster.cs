/*
Rajesh Upadhyay 09-july-2015 #CC01 -Property Added CallProjectType
Vivek kumar 13-jan-2016 #CC02 -Property Added CustomerCode
Vivek kumar 28-apr-2016 #CC03 -Property Added CustomerCategory
12-Aug-2016, Vijay Katiyar, #CC04 - Added CustomerTypeId property
25-Jan-2017, Vijay Katiyar, #CC05, added parameter ModelType and BankName data table in function LogNewCustomerComplaintV3()
 * 08-Feb-2018, Sumit Maurya, #CC06, New property created GSTINNo Done For (Amararaja).
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

  public abstract class clsDetailMaster: IDisposable
    {
        #region Private Class Variables

        private Int64 _intDetailId;
        private Int64 _intJobSheetID;
        private string _strFirstName;
        private string _strMiddleName;
        private string _strLastName;
        private string _strCompanyName;
        private short _shtAge;
        private DateTime? _dtDOB;
        private DateTime? _dtMarriageAnniversary;
        private short _shtCountryId;
        private short _shtStateId;
        private int _intCityId;
        private int? _intLocalityID;
        private string _strCountryName;
        
        private string _strStateName;
        private string _strCityName;
        private string _strLocalityName;
        private string _strStreet;
        private string _strAddress;
        private string _strPincode;
        private string _strMobileNo;
        private string _strAltMobileNo;
        private string _strPhoneNo;
        private string _strAltPhoneNo;
        private string _strEmailID;
        private string _strRoleIDXml;
        private string _strSTDCode;

        private DateTime? _dtLastLoginOn;
        private DateTime? _dtPasswordLastUpdatedOn;
        private DateTime? _dtLastLockedOn;
        private bool _blnIsLockedOut;
        private short _shtFailedPasswordAttemptCount;
        private bool _blnActive;
        private string _strEmployeeID;
        private string _strPassword;
        private string _strPasswordSalt;
        private Int64 _intUserDetailID;
        private string _strLandMark;
        private int _intRoleEntityId;
        private int _intUserRoleId;

        private string _strError;
        private Int32 _intPageIndex;
        private Int32 _intPageSize;
        private Int32 _intTotalRecords;
        private string _strEntityCode;
        private string _strUserLogIn;
        private string _strCallProjectType;//#CC01:added//
        private string _strCustomerCode;//#CC02:added//
        private string _strCustomerCategory;//#CC03:added//
        private Int16 _intCustomerTypeId;/* #CC04 Added*/
        private Int16 _intCustomerEditMode;/* #CC04 Added*/
        private string _strModelType=string.Empty;/* #CC05:Added */
        #endregion

        #region Dispose
		private bool IsDisposed = false;
        
		//Call Dispose to free resources explicitly
		public void Dispose()
		{
    		//Pass true in dispose method to clean managed resources too and say GC to skip finalize 
    		// in next line.
    		Dispose(true);
    		//If dispose is called already then say GC to skip finalize on this instance.
    		GC.SuppressFinalize(this);
		}

        ~clsDetailMaster()
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

        #region Public Properties
        //#CC03:added(start)//
        public string CustomerCategory
        {
            get
            {
                return _strCustomerCategory;
            }
            set
            {
                _strCustomerCategory = value;
            }
        }//#CC03:added(end)//

        //#CC02:added(start)//
        public string CustomerCode
        {
            get
            {
                return _strCustomerCode;
            }
            set
            {
                _strCustomerCode = value;
            }
        }//#CC02:added(end)//

        //#CC01:added(start)//
        public string CallProjectType
        {
            get
            {
                return _strCallProjectType;
            }
            set
            {
                _strCallProjectType = value;
            }
        }//#CC01:added(end)//
        
        public string EntityCode
        {
            get { return _strEntityCode; }
            set { _strEntityCode = value; }
        }

        public Int64 DetailId
        {
            get
            {
                return _intDetailId;
            }
            set
            {
                _intDetailId = value;
            }
        }
        public string UserLogIn
        {
            get
            {
                return _strUserLogIn;
            }
            set
            {
                _strUserLogIn = value;
            }
        }
        public Int64 JobSheetID
        {
            get
            {
                return _intJobSheetID;
            }
            set
            {
                _intJobSheetID = value;
            }
        }
        public string FirstName
        {
            get
            {
                return _strFirstName;
            }
            set
            {
                _strFirstName = value;
            }
        }
        public string RoleEntityXml
        {
            get
            {
                return _strRoleIDXml;
            }
            set
            {
                _strRoleIDXml = value;
            }
        }
        public string MiddleName
        {
            get
            {
                return _strMiddleName;
            }
            set
            {
                _strMiddleName = value;
            }
        }
        public string LastName
        {
            get
            {
                return _strLastName;
            }
            set
            {
                _strLastName = value;
            }
        }
        public string CompanyName
        {
            get
            {
                return _strCompanyName;
            }
            set
            {
                _strCompanyName = value;
            }
        }
        public short Age
        {
            get
            {
                return _shtAge;
            }
            set
            {
                _shtAge = value;
            }
        }
        public DateTime? DOB
        {
            get
            {
                return _dtDOB;
            }
            set
            {
                _dtDOB = value;
            }
        }
        public DateTime? MarriageAnniversary
        {
            get
            {
                return _dtMarriageAnniversary;
            }
            set
            {
                _dtMarriageAnniversary = value;
            }
        }
        public short CountryId
        {
            get { return _shtCountryId; }
            set { _shtCountryId = value; }
        }
        public short StateId
        {
            get
            {
                return _shtStateId;
            }
            set
            {
                _shtStateId = value;
            }
        }
        public int CityId
        {
            get
            {
                return _intCityId;
            }
            set
            {
                _intCityId = value;
            }
        }
        public int UserRoleId
        {
            get
            {
                return _intUserRoleId;
            }
            set
            {
                _intUserRoleId = value;
            }
        }
        public int RoleEntityId
        {
            get
            {
                return _intRoleEntityId;
            }
            set
            {
                _intRoleEntityId = value;
            }
        }
        public int? LocalityID
        {
            get
            {
                return _intLocalityID;
            }
            set
            {
                _intLocalityID = value;
            }
        }
        public string CountryName
        {
            get { return _strCountryName; }
            set { _strCountryName = value; }
        }
        public string StateName
        {
            get
            {
                return _strStateName;
            }
            set
            {
                _strStateName = value;
            }
        }
        public string CityName
        {
            get
            {
                return _strCityName;
            }
            set
            {
                _strCityName = value;
            }
        }
        public string LocalityName
        {
            get
            {
                return _strLocalityName;
            }
            set
            {
                _strLocalityName = value;
            }
        }
        public string Street
        {
            get
            {
                return _strStreet;
            }
            set
            {
                _strStreet = value;
            }
        }
        public string Address
        {
            get
            {
                return _strAddress;
            }
            set
            {
                _strAddress = value;
            }
        }
        public string Pincode
        {
            get
            {
                return _strPincode;
            }
            set
            {
                _strPincode = value;
            }
        }
        public string MobileNo
        {
            get
            {
                return _strMobileNo;
            }
            set
            {
                _strMobileNo = value;
            }
        }
        public string AltMobileNo
        {
            get
            {
                return _strAltMobileNo;
            }
            set
            {
                _strAltMobileNo = value;
            }
        }
        public string PhoneNo
        {
            get
            {
                return _strPhoneNo;
            }
            set
            {
                _strPhoneNo = value;
            }
        }
        public string AltPhoneNo
        {
            get
            {
                return _strAltPhoneNo;
            }
            set
            {
                _strAltPhoneNo = value;
            }
        }
        public string EmailID
        {
            get
            {
                //if ((_strEmailID).ToLower() == "na")
                //    _strEmailID = "";

                return _strEmailID;
            }
            set
            {
                _strEmailID = value;
            }
        }
        public string LandMark
        {
            get
            {
                return _strLandMark;
            }
            set
            {
                _strLandMark = value;
            }
        }
        public string STDCode
        {
            get
            {
                return _strSTDCode;
            }
            set
            {
                _strSTDCode = value;
            }
        }

        public DateTime? LastLoginOn
        {
            get
            {
                return _dtLastLoginOn;
            }
            set
            {
                _dtLastLoginOn = value;
            }
        }
        public DateTime? PasswordLastUpdatedOn
        {
            get
            {
                return _dtPasswordLastUpdatedOn;
            }
            set
            {
                _dtPasswordLastUpdatedOn = value;
            }
        }
        public DateTime? LastLockedOn
        {
            get
            {
                return _dtLastLockedOn;
            }
            set
            {
                _dtLastLockedOn = value;
            }
        }
        public bool IsLockedOut
        {
            get
            {
                return _blnIsLockedOut;
            }
            set
            {
                _blnIsLockedOut = value;
            }
        }
        public short FailedPasswordAttemptCount
        {
            get
            {
                return _shtFailedPasswordAttemptCount;
            }
            set
            {
                _shtFailedPasswordAttemptCount = value;
            }
        }
        public bool Active
        {
            get
            {
                return _blnActive;
            }
            set 
            {
                _blnActive = value;
            }
        }
        public string EmployeeID
        {
            get
            {
                return _strEmployeeID;
            }
            set 
            {
                _strEmployeeID = value;
            }
        }
        public string Password
        {
            get
            {
                return _strPassword;
            }
            set
            {
                _strPassword = value;
            }
        }
        public string PasswordSalt
        {
            get
            {
                return _strPasswordSalt;
            }
            set
            {
                _strPasswordSalt = value;
            }
        }
        public Int64 UserDetailID
        {
            get
            {
                return _intUserDetailID;
            }
            set
            {
                _intUserDetailID = value;
            }
        }
        
        public string Error
        {
            get
            {
                return _strError;
            }
            protected set
            {
                _strError = value;
            }
        }
        public Int32 PageIndex
        {
            protected get
            {
                return _intPageIndex;
            }
            set
            {
                _intPageIndex = value;
            }
        }
        public Int32 PageSize
        {
            protected get
            {
                return _intPageSize;
            }
            set
            {
                _intPageSize = value;
            }
        }
        public Int32 TotalRecords
        {
            get
            {
                return _intTotalRecords;
            }
            protected set
            {
                _intTotalRecords = value;
            }
        }

        /* #CC04 Added start*/

        public Int16 CustomerTypeId
        {
            get
            {
                return _intCustomerTypeId;
            }
            set
            {
                _intCustomerTypeId = value;
            }
        }
        public Int16 CustomerEditMode
        {
            get
            {
                return _intCustomerEditMode;
            }
            set
            {
                _intCustomerEditMode = value;
            }
        }


        /* #CC04 Added end*/

        /* #CC05:Added start */
        public string ModelType
        {
            get
            {
                return _strModelType;
            }
            set
            {
                _strModelType = value;
            }
        }
        public DataTable dtBankName
        {
            get;
            set;
        }
        /* #CC05:Added end */

        /* #CC06 Add Start  */

        public string GSTINNo
        {
            get;
            set;
        }/* #CC06 Add End */

        #endregion

        public abstract Int16 Save();
        public abstract Int16 Update();
        public abstract bool Delete();
        public abstract DataTable SelectAll();
        public abstract DataTable SelectById();
        public abstract void Load();
        public abstract bool ToggleActivation(); 
    }

