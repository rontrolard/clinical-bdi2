using System;
using BDI2Core.Common;
using BDI2Core.Configuration;
using BDI2Core.Data;
using BDI2Web.Common_Code;

namespace BDI2Web.CustomerRegistration
{
    public partial class EULA : SecureBasePage
    {
        #region events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                sEULA.InnerHtml = Lookups.GetAppplicationSettingByKeyName("EULA");
            }
        }
        #endregion

        #region private functions
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            Staff staff = SessionData.Staff;

            int ret = SQLHelper.ExecuteNonQuery(Settings.ConnectionStringName, "SaveCustomerRegistrationDate", staff.CustomerID);

            if (SetupHelper.IsCustomerInitialized(staff.CustomerID))
            {
                if (!string.IsNullOrEmpty(staff.Email))
                    Response.Redirect("~/Welcome/HomePage.aspx");
                else
                    Response.Redirect("~/ProfilePage.aspx");
            }
            else
            {
                Response.Redirect("~/Setup/SelectSetupType.aspx");
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
        #endregion

        #region Overridden Functions
        protected override void SetPageID()
        {
            PageID = 31; //Value from Page Table in db
        }

        #endregion Overridden Functions
    }
}
