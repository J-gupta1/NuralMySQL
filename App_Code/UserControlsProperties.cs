using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BussinessLogic/*ZedAxis.ZedEBS*/
{
  public  class UserControlsProperties: IDisposable
    {
        #region Private Class Variables
        private string _strError;
        private int _intPageIndex;
        private int _intPageSize;
        private int _intTotalRecords;
        private int _intEntityID;
        private int _intUserID;
        private int _IntEntityTypeRoleID;
        private int _intSelectedServiceCentreID;
        private Int16 _intEntityTypeID;
        private Int16 _intGetEntityTypeID;
        private Int16 _intCallTypeID;
        private Int32 _intSelectedServiceCentre;

        // Variable For Search Module in Dash Board

        private string _strjobNo;
        private string _strPartCode;
        private string _strCustomerCode;
        private int _intProductCategoryID;
        private int _intModelId;
        private DateTime? _dtJobSheetCreationFrom;
        private DateTime? _dtJobSheetCreationTo;
        private int _intJobStatusID;
        private int _intWarrantyStatusID;
        private int _intEngineerName;

        private DateTime? _dtFromDate;
        private DateTime? _dtToDate;
        private string _strSelectedServiceCenters;
        private Int16 _intJobCloseStatus;
        private string _strTRCExecutive;
        #endregion

        #region Public Properties

        public int SelectedServiceCentreID
        {
            get
            {
                return _intSelectedServiceCentreID;
            }
            set
            {
                _intSelectedServiceCentreID = value;
            }
        }


        public int WarrantyPeriod
        {
            get;
            set;
        }
        


        public string Error
        {
            get
            {
                return _strError;
            }
            set
            {
                _strError = value;
            }
        }
        public string PartCode
        {
            get
            {
                return _strPartCode;
            }
            set
            {
                _strPartCode = value;
            }
        }
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
        }
        public Int32 PageIndex
        {
            get
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
            get
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
            set
            {
                _intTotalRecords = value;
            }
        }
        public int EntityID
        {
            get { return _intEntityID; }
            set { _intEntityID = value; }
        }
        //public int SelectedServiceCentre   //#CC03 commented
        //{
        //    get { return _intSelectedServiceCentre; }
        //    set { _intSelectedServiceCentre = value; }
        //}
        public int UserID
        {
            get { return _intUserID; }
            set { _intUserID = value; }
        }
        public int EntitytypeRoleID
        {
            get { return _IntEntityTypeRoleID; }
            set { _IntEntityTypeRoleID = value; }
        }
        public Int16 EntitytypeID
        {
            get { return _intEntityTypeID; }
            set { _intEntityTypeID = value; }
        }
        public Int16 GetEntityTypeID
        {
            get { return _intGetEntityTypeID; }
            set { _intGetEntityTypeID = value; }
        }

        public Int16 CallTypeID
        {
            get { return _intCallTypeID; }
            set { _intCallTypeID = value; }
        }

        // Properties For Search Module in Dash Board
        public string JobNo
        {
            get
            {
                return _strjobNo;
            }
            set
            {
                _strjobNo = value;
            }
        }
        public int ProductCategoryID
        {
            get
            {
                return _intProductCategoryID;
            }
            set
            {
                _intProductCategoryID = value;
            }
        }
        public int ModelId
        {
            get
            {
                return _intModelId;
            }
            set
            {
                _intModelId = value;
            }
        }
        public DateTime? JobSheetCreationFrom
        {
            get
            {
                return _dtJobSheetCreationFrom;
            }
            set
            {
                _dtJobSheetCreationFrom = value;
            }
        }
        public DateTime? JobSheetCreationTo
        {
            get
            {
                return _dtJobSheetCreationTo;
            }
            set
            {
                _dtJobSheetCreationTo = value;
            }
        }
        public int JobStatusID
        {
            get
            {
                return _intJobStatusID;
            }
            set
            {
                _intJobStatusID = value;
            }
        }

        public int WarrantyStatusID
        {
            get
            {
                return _intWarrantyStatusID;
            }
            set
            {
                _intWarrantyStatusID = value;
            }
        }
        public int EngineerNameID
        {
            get
            {
                return _intEngineerName;
            }
            set
            {
                _intEngineerName = value;
            }
        }

        public DateTime? FromDate
        {
            get
            {
                return _dtFromDate;
            }
            set
            {
                _dtFromDate = value;
            }
        }
        public DateTime? ToDate
        {
            get
            {
                return _dtToDate;
            }
            set
            {
                _dtToDate = value;
            }
        }
        public string SelectedServiceCenters
        { get { return _strSelectedServiceCenters; } set { _strSelectedServiceCenters = value; } }
        public Int16 JobCloseStatus
        { get { return _intJobCloseStatus; } set { _intJobCloseStatus = value; } }
        public string TRCExecutive
        { get { return _strTRCExecutive; } set { _strTRCExecutive = value; } }
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

        ~UserControlsProperties()
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
