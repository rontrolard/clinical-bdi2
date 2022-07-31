using System;
using System.Collections;
using System.Web.SessionState;
using BDI2Core.Common;

namespace BDI2Web.Common_Code
{
    public class SessionData
    {
        public SessionData()
        {
        }

        #region Constants

        private const string USER_KEY = "userKey";
        private const string STATUS_MESSAGE_KEY = "statusMessageKey";
        private const string PREVIOUS_PAGE = "previouspage";
        private const string CUSTOMER_ID = "customerID";
        private const string MDSUNLIMITED = "mdsunlimited";
        private const string CSS_SHEET = "cssSheet";
        private const string CHILD_CRITERIA = "childCriteria";
        private const string STAFF_CRITERIA = "staffCriteria";


        #endregion Constants

        #region Variables

        private HttpSessionState _session = null;

        #endregion

        #region Constructor

        public SessionData(HttpSessionState session)
        {
            _session = session;
        }

        #endregion

        #region Properties

        public Staff Staff
        {
            get
            {
                Staff returnSTAFF = _session[USER_KEY] as Staff;
                if (returnSTAFF == null)
                {
                    returnSTAFF = new Staff();
                    _session[USER_KEY] = returnSTAFF;
                }

                return returnSTAFF;
            }
            set
            {
                _session[USER_KEY] = value;
            }
        }
        public SearchChildCriteria childCriteria
        {
            get
            {
                SearchChildCriteria criteria = _session[CHILD_CRITERIA] as SearchChildCriteria;
                if (criteria == null)
                {
                    criteria = new SearchChildCriteria();
                    _session[CHILD_CRITERIA] = criteria;
                }

                return criteria;
            }
            set
            {
                _session[CHILD_CRITERIA] = value;
            }
        }
        public SearchStaffCriteria staffCriteria
        {
            get
            {
                SearchStaffCriteria criteria = _session[STAFF_CRITERIA] as SearchStaffCriteria;
                if (criteria == null)
                {
                    criteria = new SearchStaffCriteria();
                    _session[STAFF_CRITERIA] = criteria;
                }

                return criteria;
            }
            set
            {
                _session[STAFF_CRITERIA] = value;
            }
        }
        public int? CustomerID
        {
            get
            {
                int? returnCustomerID = null;
                if (_session[CUSTOMER_ID] != null)
                {
                    returnCustomerID = Convert.ToInt32(_session[CUSTOMER_ID]);
                }
                return returnCustomerID;
            }
            set
            {
                _session[CUSTOMER_ID] = value;
            }
        }

        public bool MDSUnlimited
        {
            get
            {
                bool mdsUnlimited = false;
                if (_session[MDSUNLIMITED] != null)
                {
                    mdsUnlimited = Convert.ToBoolean(_session[MDSUNLIMITED]);
                }
                return mdsUnlimited;
            }
            set
            {
                _session[MDSUNLIMITED] = value;
            }
        }
        
        public int? PreviousPageID
        {
            get
            {
                int? returnPreviousPage = null;
                if (_session[PREVIOUS_PAGE]!=null)
                {
                    returnPreviousPage = Convert.ToInt32(_session[PREVIOUS_PAGE]);
                }
                return returnPreviousPage;
            }
            set
            {
                _session[PREVIOUS_PAGE] = value;
            }
        }

        public string StatusMessage
        {
            get
            {
                string returnStatusMessage = _session[STATUS_MESSAGE_KEY] as string;
                if (returnStatusMessage == null)
                {
                    returnStatusMessage = string.Empty;
                    _session[STATUS_MESSAGE_KEY] = returnStatusMessage;
                }

                return returnStatusMessage;
            }
            set
            {
                _session[STATUS_MESSAGE_KEY] = value;
            }
        }

        public string CSSSheet
        {
            get
            {
                string returnStatusMessage = _session[CSS_SHEET] as string;
                if (returnStatusMessage == null)
                {
                    returnStatusMessage = "messageText";
                    _session[CSS_SHEET] = returnStatusMessage;
                }

                return returnStatusMessage;
            }
            set
            {
                _session[CSS_SHEET] = value;
            }
        }



        #endregion Properties

    }    
}
