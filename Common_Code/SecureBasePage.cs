using System;

namespace BDI2Web.Common_Code
{
    public class SecureBasePage : BasePage
    {
        #region Virtual Functions

        protected override void OnLoad(EventArgs e)
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
                            Session["SessionExpired"] = "true";
                            SessionData.StatusMessage = "Your session has expired, re-login to the application.";
                            Response.Redirect("~/default.aspx");
                        }
                    }
                }
            }

            if (SessionData.Staff.StaffID == Int32.MinValue)
            {
                Session["SessionExpired"] = true;
                SessionData.StatusMessage = "Re-login to the application to use that feature.";
                Response.Redirect("~/default.aspx");
            }

            base.OnLoad(e);
        }

        #endregion Virtual Functions
    }
}
