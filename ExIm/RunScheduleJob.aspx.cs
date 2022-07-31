using System;
using System.Configuration;
using System.IO;
using BDI2Core.Common;
using BDI2Core.Configuration;
using BDI2Core.Data;
using BDI2Web.Common_Code;
using Telerik.Web.UI;

namespace BDI2Web.ExIm
{
    public partial class RunScheduleJob : SecureBasePage
    {
        private int pageTimeOut = 90;
        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            pageTimeOut = Server.ScriptTimeout;
            Server.ScriptTimeout = Settings.GetSqlConnectionTimeOut();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            Server.ScriptTimeout = pageTimeOut;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordPanel.Visible = !RunJobPanel.Visible;
        }

        protected void PasswordButton_Click(object sender, EventArgs e)
        {
            RunJobPanel.Visible = RunJobPasswordTextBox.Text.ToUpper() == "BDIJOBS";
            PasswordPanel.Visible = !RunJobPanel.Visible;
        }

        protected void ProcessJobButton_Click(object sender, EventArgs e)
        {
            int scheduleJobID;
            try
            {                
                if (Int32.TryParse(JobIDTextBox.Text, out scheduleJobID))
                {
                    LogIt("Start Job - " + scheduleJobID.ToString());
                    ScheduledJob.ProcessJob(scheduleJobID, Settings.GetOutputPath());
                    SessionData.StatusMessage = "Job ID: " + scheduleJobID.ToString() + " Processed.";
                    LogIt("End Job - " + scheduleJobID.ToString());
                }
                else
                {
                    SetErrorMessage("Job ID is not valid.");
                }
            }
            catch (Exception ex)
            {
                LogIt("Error - " + ex.Message);
                LogIt("Error - " + ex.StackTrace);
                LogIt("End Job");
            }
        }

        protected void ChangeStatusButton_Click(object sender, EventArgs e)
        {
            int scheduleJobID;
            if (Int32.TryParse(EditJobIDTextBox.Text, out scheduleJobID))
            {
                int scheduledStatusID;
                if (Int32.TryParse(ScheduleStatusIDTextBox.Text, out scheduledStatusID))
                {
                    ScheduledJob sj = ScheduledJob.GetScheduldedJobByID(scheduleJobID);
                    sj.Status = (BDI2Core.Common.ScheduledStatus)Enum.ToObject((typeof(BDI2Core.Common.ScheduledStatus)), scheduledStatusID);
                    sj.Save();
                    SessionData.StatusMessage = "Job ID: " + scheduleJobID.ToString() + " Scheduled Status has changed.";
                }
                else
                {
                    SetErrorMessage("Schedule Status must be a number between 1-11.");
                }                
            }
            else
            {
                SetErrorMessage("ScheduleStatus is not valid.");
            }
        }

        protected void GetFileButton_Click(object sender, EventArgs e)
        {
            int scheduleJobID;
            if (Int32.TryParse(GetFileJobIDTextBox.Text, out scheduleJobID))
            {
                ScheduledJob sj = ScheduledJob.GetScheduldedJobByID(int.Parse(GetFileJobIDTextBox.Text));
                ScheduledJob.BrowserDownload(ConfigurationManager.AppSettings["outputpath"] + "\\" + sj.FileName);
            }
            else
            {
                SetErrorMessage("Schedule Job ID is not numeric.");
            }
        }

        protected void ExpiredCustomerButton_Click(object sender, EventArgs e)
        {
            if (!StartDateRDP.IsEmpty && !EndDateRDP.IsEmpty)
            {
                ResultGrid.DataSource = Lookups.GetExpiredCustomers(StartDateRDP.SelectedDate.Value, EndDateRDP.SelectedDate.Value);
                ResultGrid.DataBind();
                
                ResultGrid.ExportSettings.ExportOnlyData = false;
                ResultGrid.ExportSettings.IgnorePaging = true;
                ResultGrid.MasterTableView.ExportToExcel();
            }
        }

        #endregion 

        #region Private Methods

        private static void LogIt(string txt)
        {
            string logFolder = Settings.GetOutputPath();

            StreamWriter sw = new StreamWriter(logFolder + "\\processJobLog.txt", true);
            sw.WriteLine(DateTime.Now.ToString() + " " + txt);
            sw.Close();

        }
        #endregion Private Methods


        #region Overridden Functions
        protected override void SetPageID()
        {
            PageID = 63; //Value from Page Table in db
        }

        #endregion Overridden Functions



    }
}
