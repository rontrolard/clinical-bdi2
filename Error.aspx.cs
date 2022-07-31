using System;
using BDI2Core.Common;
using BDI2Web.Common_Code;
using System.Web.Security;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace BDI2Web
{
    public partial class Error : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString[0] != null)
            {
                string errorCode = Hash.Decrypt(Request.QueryString[0]);
                
                if (errorCode.Equals("0"))
                    errorCode = "Page Not Found";
                else
                    errorCode = "Error Code " + errorCode;

                Label1.Text = "The BDI2 application has encountered an error (" + errorCode +
                              "). Please contact your support representative.";
            }
        }
    

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 0; //Value from Page Table in db
        }

        #endregion Overridden Functions
    }
}
