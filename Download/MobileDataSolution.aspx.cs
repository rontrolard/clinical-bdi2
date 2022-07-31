using System;
using System.IO;
using System.Web;
using BDI2Core.Common;
using BDI2Web.Common_Code;
using System.Data;

namespace BDI2Web.Download
{
    public partial class MobileDataSolution :SecureBasePage
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccess(Permission.Download, false);

            string currentFileLocation = Server.MapPath("MDSInstallInstructions.pdf");
            FileInfo fi = new FileInfo(currentFileLocation);

            if (fi.Exists)
            {
                MDSInstallLabel.Text = "MDS install instructions were updated on " + fi.LastWriteTime;
            }

        }
        protected void DownloadButton_Click(object sender, EventArgs e)
        {
            string filename;
            DataSet ds = BDI2Core.Data.Lookups.CurrentAppVersionByAppID(7);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                filename = "~" + ds.Tables[0].Rows[0]["DownloadUrl"].ToString();
                string sFilePathName = HttpContext.Current.Server.MapPath(filename);
                BrowserDownload(sFilePathName);
                AuditTrail.PublishAuditTrail(SessionData.Staff.StaffID.Value,"MDS Eula Accepted",AuditActions.DownloadMDS);
            }
            else
            {
                SetErrorMessage("Contact Riverside Customer Service. URL not set up in Customer Service application.");
            }
        }

        #endregion Events

        #region Private Methods

        public void BrowserDownload(string fullfilename)
        {

            System.IO.Stream iStream = null;
            // Buffer to read 10K bytes in chunk:
            byte[] buffer = new Byte[10000];
            // Length of the file:
            int length;
            // Total bytes to read:
            long dataToRead;
            // Identify the file name.
            string filename = System.IO.Path.GetFileName(fullfilename);
            
            try
            {
                
                // Open the file.                
                iStream = new System.IO.FileStream(fullfilename, System.IO.FileMode.Open,
                      System.IO.FileAccess.Read, System.IO.FileShare.Read);

                // Total bytes to read:
                dataToRead = iStream.Length;

                //HttpContext.Current.Response.BufferOutput=false; 
                //HttpContext.Current.Response.Buffer=false; 
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("content-length", dataToRead.ToString());
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                HttpContext.Current.Response.Flush();

                // Read the bytes.
                while (dataToRead > 0)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = iStream.Read(buffer, 0, 10000);
                        // Write the data to the current output stream.
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        // Flush the data to the HTML output.
                        HttpContext.Current.Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
                
            }

            catch (Exception ex)
            {
                // Trap the error, if any.
                BDI2Core.Common.ErrorLog.PublishError(ex);
                HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }
                HttpContext.Current.Response.Close();
            }
        }

        #endregion Private Methods

        #region Overriden Funcitons

        protected override void SetPageID()
        {
            PageID = 66; //Value from Page Table in db
        }

        #endregion Overridden Functions


    }
}
