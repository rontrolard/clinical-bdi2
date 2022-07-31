using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BDI2Core.Common;
using BDI2Web.Assessment;
using Telerik.Web.UI;
using Image = System.Web.UI.WebControls.Image;

namespace BDI2Web.Common_Code
{
    public class BasePage : Page
    {
        #region Protected Variables

        protected SessionData _sessionData;

        #endregion Protected Variables

        #region Private Variables

        private int _pageID = Int32.MinValue;

        #endregion Private Variables

        #region Private Methods

        private void IsSessionTimedOut()
        {
            if (!(Request.Path.ToUpper().EndsWith("DEFAULT.ASPX")))
            {
                if (Context.Session != null)
                {
                    if (Session.IsNewSession)
                    {
                        // If it says it is a new session, but an existing cookie exists, then it must 
                        // have timed out (can't use the cookie collection because even on first 
                        // request it already contains the cookie (request and response
                        // seem to share the collection)
                        string szCookieHeader = Request.Headers["Cookie"];
                        if ((null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                        {
                            SessionData.StatusMessage = "Your session has expired, re-login to the application.";
                            Response.Redirect("~/default.aspx");
                        }
                    }
                }
            }
        }

        private void DisplayStatusMessage()
        {
            if (Master != null)
            {
                Control div = Master.FindControl("brdiv");
                if (div != null)
                    div.Visible = false;
                if (SessionData.StatusMessage.Trim() != "")
                {
                    Label lbl = (Label)Master.FindControl("MPStatusMessageLabel");
                    if (lbl != null)
                    {
                        lbl.Text = SessionData.StatusMessage + "<br>";
                        SessionData.StatusMessage = "";
                        if (Convert.ToInt32(Session["PAGE_75_PageID"]) == 75)
                        {
                            SessionData.CSSSheet = "profilemessageText";
                            lbl.CssClass = SessionData.CSSSheet;
                            Session["PAGE_75_PageID"] = null;
                            if (div != null)
                                div.Visible = true;
                        }
                        else
                        {
                            lbl.CssClass = SessionData.CSSSheet;
                        }
                    }
                }
            }
            SessionData.CSSSheet = "messageText";
        }

        private static void SetTopLevelMenu(RadMenu radM, string RadMenuItemText)
        {
            RadMenuItem rmi;
            rmi = radM.Items.FindItemByText(RadMenuItemText);
            if (rmi != null)
            {
                rmi.BackColor = Color.Black;
            }
        }

        private static void ShowMenu(RadMenu radM, string RadMenuParentValue, string RadMenuItemValue, bool showItem)
        {
            RadMenuItem rmi;
            if (RadMenuParentValue == null)
            {
                rmi = radM.Items.FindItemByValue(RadMenuItemValue);
            }
            else
            {
                rmi = radM.Items.FindItemByValue(RadMenuParentValue);
                if (rmi != null)
                {
                    rmi = rmi.Items.FindItemByValue(RadMenuItemValue);
                }
            }
            if (rmi != null)
            {
                rmi.Visible = showItem;
            }
        }
        #endregion Private Methods

        #region Overriden Functions

        protected override void OnLoad(EventArgs e)
        {
            IsSessionTimedOut();
            SetPageID();
            if (!IsPostBack)
            {
                SetPageStyles();
                if (PageID == 16 || PageID == 56)
                {
                    HtmlLink cssLink = new HtmlLink();
                    cssLink.ID = "RadGridPagerCss";
                    cssLink.Href = "RadControls/Grid/Skins/Default/Grid.Telerik.css";
                    cssLink.Attributes.Add("rel", "stylesheet");
                    cssLink.Attributes.Add("type", "text/css");

                    // Add the HtmlLink to the Head section of the page.
                    Page.Header.Controls.Add(cssLink);
                }
            }

            //Non-IE browsers may have issue with postback from ajax components
            //AFTER a page is refreshed. 
            //See http://erikmich.spaces.live.com/Blog/cns!EECE29F3E771C821!150.entry
            if (Request.Browser.MSDomVersion.Major == 0) // Non IE Browser?)
                Response.Cache.SetNoStore(); // No client side cashing for non IE browsers

            base.OnLoad(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            DisplayStatusMessage();
        }

        protected override void OnError(EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (SessionData.Staff != null)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                if (SessionData.Staff.StaffID != null)
                    dict.Add("StaffID", SessionData.Staff.StaffID.Value);
                //Add more items from SessionData object here.
                dict.Add("PageID", PageID);
                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                if (sa != null)
                {
                    dict.Add("StudentAssessmentID", sa.StudentAssessmentID);
                }
                BDI2Core.Common.ErrorLog.PublishError(ex, dict);
            }
            else
            {
                BDI2Core.Common.ErrorLog.PublishError(ex);
            }

            base.OnError(e);
        }

        #endregion Overridden Functions

        #region Properties

        public SessionData SessionData
        {
            get
            {
                if (_sessionData == null)
                {
                    _sessionData = new SessionData(Session);
                }
                return _sessionData;
            }
        }

        public int PageID
        {
            get
            {
                return _pageID;
            }
            set
            {
                _pageID = value;
            }
        }

        #endregion Properties

        #region Public Methods

        public void ResetMenus()
        {
            SetPageStyles();
        }

        public bool CheckAccess(Permission permission, bool fromMenu)
        {
            bool allow = false;

            switch (permission)
            {
                case Permission.Export:
                    allow = SessionData.Staff.Privileges.Contains(Privileges.ImportExport);
                    break;

                case Permission.Import:
                    allow = SessionData.Staff.Privileges.Contains(Privileges.ImportExport) &&
                            SessionData.Staff.LevelType == LevelType.TopLevel;
                    break;

                case Permission.ManageChild:
                    allow = SessionData.Staff.Privileges.Contains(Privileges.ManageChild);
                    break;

                case Permission.ManageERFMDS:
                    allow = SessionData.Staff.Privileges.Contains(Privileges.ManageERFMDS)
                            || SessionData.Staff.Privileges.Contains(Privileges.ManageStaff);
                    break;

                case Permission.ManageStaff:
                    allow = SessionData.Staff.LevelType != LevelType.BottomLevel;
                    break;

                case Permission.Download:
                    allow = SessionData.MDSUnlimited || SessionData.Staff.MDSActivationStatus;
                    break;

                case Permission.TopLevel:
                    allow = SessionData.Staff.LevelType == LevelType.TopLevel;
                    break;
            }

            if (!fromMenu && !allow)
            {
                string errorCode = Hash.Encrypt("0");
                Response.Redirect("~/Error.aspx?code=" + errorCode, true);
            }

            return allow;
        }

        #endregion Public Methods

        #region Virtual Methods

        protected virtual void SetPageID()
        {
            throw new NotImplementedException("You must implement SetPageID if you are deriving from BasePage");
        }

        #endregion Virtual Methods

        #region Protected Methods

        protected void SetPageStyles()
        {
            if (Master != null)
            {
                BDIPage bdiPage = BDIPage.GetPageByPageID(PageID);
                RadMenu radM = Master.FindControl("RadMenu1") as RadMenu;

                if (radM != null)
                {
                    ShowMenu(radM, null, "MenuExim", (SessionData.Staff.Privileges.Contains(Privileges.ImportExport) || (SessionData.Staff.Privileges.Contains(Privileges.ViewReports))));
                    ShowMenu(radM, null, "MenuDownload", CheckAccess(Permission.Download, true));
                    ShowMenu(radM, "RadMenuHiearchy", "MenuMemberTypeManagement", CheckAccess(Permission.TopLevel, true));
                    ShowMenu(radM, "RadMenuHiearchy", "MenuUDFManagement", CheckAccess(Permission.TopLevel, true));
                    ShowMenu(radM, "RadMenuHiearchy", "MenuERFManagement", CheckAccess(Permission.ManageERFMDS, true));
                    ShowMenu(radM, "RadMenuHiearchy", "MenuMDSLicenseManagement", CheckAccess(Permission.ManageERFMDS, true));
                    ShowMenu(radM, "MenuExim", "rmExport", CheckAccess(Permission.Export, true));
                    ShowMenu(radM, "MenuExim", "RadMenuImport", CheckAccess(Permission.Import, true));
                    ShowMenu(radM, "MenuExim", "rmMigrationImport", CheckAccess(Permission.Import, true));
                    ShowMenu(radM, "MenuChild", "AddChild", CheckAccess(Permission.ManageChild, true));
                    ShowMenu(radM, "MenuChild", "RerosterChild", CheckAccess(Permission.ManageChild, true));

                    var canManageMDS = CheckAccess(Permission.ManageERFMDS, true);
                    var isBottomLevel = CheckAccess(Permission.ManageERFMDS, true);

                    ShowMenu(radM, null, "MenuStaff", CheckAccess(Permission.ManageStaff, true) || CheckAccess(Permission.ManageERFMDS, true));
                    ShowMenu(radM, "MenuStaff", "SearchStaff", CheckAccess(Permission.ManageStaff, true));
                    ShowMenu(radM, "MenuStaff", "AddStaff", CheckAccess(Permission.ManageStaff, true));
                    ShowMenu(radM, "MenuStaff", "ViewStaffProfileReport", CheckAccess(Permission.ManageStaff, true));
                    ShowMenu(radM, "MenuStaff", "StaffMDS", CheckAccess(Permission.ManageERFMDS, true));

                    radM.Skin = bdiPage.MenuCSS;

                    switch (bdiPage.MenuCSS)
                    {
                        case "CssHierarchy":
                            SetTopLevelMenu(radM, "Hierarchy Organization");
                            break;
                        case "CssStaff":
                            SetTopLevelMenu(radM, "Staff Administration");
                            break;
                        case "CssChild":
                            SetTopLevelMenu(radM, "Child Administration");
                            break;
                        case "CssReport":
                            SetTopLevelMenu(radM, "Reports");
                            break;
                        case "CssExim":
                            SetTopLevelMenu(radM, "Import/Export");
                            break;
                    }

                }

                Image bannerImage = Master.FindControl("BannerImage") as Image;
                if (bannerImage != null)
                {
                    bannerImage.ImageUrl = bdiPage.BannerImage;
                }
            }
        }

        protected void ClearStatusMessageLabel()
        {
            Label lbl = (Label)Master.FindControl("MPStatusMessageLabel");
            if (lbl != null)
            {
                lbl.Text = "";
                lbl.CssClass = "messageText";
            }
        }

        protected void SetErrorMessage(string ErrorMessage)
        {
            SessionData.StatusMessage = ErrorMessage;
            SessionData.CSSSheet = "errorMessage";
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "ReturnToTop", "document.getElementById('MainContent').scrollTop = 0;", true);
        }

        protected void SetErrorMessageWithArrayList(ArrayList al)
        {
            string s = "";

            foreach (string err in al)
            {
                s = s + err + "<br>";
            }
            SessionData.StatusMessage = s;
            SessionData.CSSSheet = "errorMessage";
        }

        protected static void FillListControl(ListControl listControl, IListSource list, string dataTextField, string DataValueField, bool insertBlankValue)
        {
            listControl.DataTextField = dataTextField;
            listControl.DataValueField = DataValueField;
            internalFillListControl(listControl, list, insertBlankValue);
        }

        protected static void FillListControl(ListControl listControl, IList list, bool insertBlankValue)
        {
            internalFillListControl(listControl, list, insertBlankValue);
        }

        protected static void internalFillListControl(ListControl listControl, object list, bool insertBlankValue)
        {
            listControl.DataSource = list;
            listControl.DataBind();

            if (insertBlankValue)
            {
                listControl.Items.Insert(0, "");
            }
        }

        #region sorting methods
        protected static void SortByValue(ListControl combo)
        {
            SortCombo(combo, new ComboValueComparer());
        }

        protected static void SortByText(ListControl combo)
        {
            SortCombo(combo, new ComboTextComparer());
        }
        #endregion
        #endregion Protected Methods

        #region protected Methods - for internal use
        private static void SortCombo(ListControl combo, IComparer comparer)
        {
            int i;
            if (combo.Items.Count <= 1)
                return;
            ArrayList arrItems = new ArrayList();
            for (i = 0; i < combo.Items.Count; i++)
            {
                ListItem item = combo.Items[i];
                arrItems.Add(item);
            }
            arrItems.Sort(comparer);
            combo.Items.Clear();
            for (i = 0; i < arrItems.Count; i++)
            {
                combo.Items.Add((ListItem)arrItems[i]);
            }
        }

        #region Combo Comparers
        /// <summary>
        /// compare list items by their value
        /// </summary>
        private class ComboValueComparer : IComparer
        {
            public enum SortOrder
            {
                Ascending = 1,
                Descending = -1
            }

            private int _modifier;

            public ComboValueComparer()
            {
                _modifier = (int)SortOrder.Ascending;
            }

            public ComboValueComparer(SortOrder order)
            {
                _modifier = (int)order;
            }

            //sort by value
            public int Compare(Object o1, Object o2)
            {
                ListItem cb1 = (ListItem)o1;
                ListItem cb2 = (ListItem)o2;
                return cb1.Value.CompareTo(cb2.Value) * _modifier;
            }
        } //end class ComboValueComparer

        /// <summary>
        /// compare list items by their text.
        /// </summary>
        private class ComboTextComparer : IComparer
        {
            public enum SortOrder
            {
                Ascending = 1,
                Descending = -1
            }

            private int _modifier;

            public ComboTextComparer()
            {
                _modifier = (int)SortOrder.Ascending;
            }

            public ComboTextComparer(SortOrder order)
            {
                _modifier = (int)order;
            }

            //sort by value
            public int Compare(Object o1, Object o2)
            {
                ListItem cb1 = (ListItem)o1;
                ListItem cb2 = (ListItem)o2;
                return cb1.Text.CompareTo(cb2.Text) * _modifier;

            }

        } //end class ComboTextComparer

        #endregion

        #endregion

    }
}
