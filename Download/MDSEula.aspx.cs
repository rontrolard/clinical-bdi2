using System;
using BDI2Core.Data;
using BDI2Web.Common_Code;

namespace BDI2Web.Download
{
    public partial class MDSEula : SecureBasePage
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                sEULA.InnerHtml = Lookups.GetAppplicationSettingByKeyName("MDSEULA");
            }
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Download/MobileDataSolution.aspx");
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Welcome/HomePage.aspx");
        }

        #endregion Events

        #region Overriden Funcitons

        protected override void SetPageID()
        {
            PageID = 69; //Value from Page Table in db
        }

        #endregion Overridden Functions
    }
}
