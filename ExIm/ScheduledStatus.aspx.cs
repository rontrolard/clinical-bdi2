using System;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Web.Common_Code;
using Telerik.Web.UI;

namespace BDI2Web.ExIm
{
    public partial class ScheduledStatus : SecureBasePage
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                rgvJobs.DataSource = ScheduledJob.GetScheduledJobsByStaffID(SessionData.Staff.StaffID.Value);
            }
        }

        protected void DeleteJobButton_Click(object sender, EventArgs e)
        {
            ImageButton tmpButton = (ImageButton) sender;
            int scheduledJobID = Convert.ToInt32(tmpButton.CommandArgument);
            ScheduledJob sj = ScheduledJob.GetScheduldedJobByID(scheduledJobID);
            sj.DeleteDate = DateTime.Now;
            sj.Save();
            SessionData.StatusMessage = "Scheduled Job has been deleted.";

            LoadGrid();
        }

        protected void rgvJobs_ItemDataBound(object sender, GridItemEventArgs e)
        {
            GridItem gvr = e.Item;            
            DataRowView drv = (DataRowView)gvr.DataItem;            

            if (gvr.ItemType == GridItemType.Item || gvr.ItemType == GridItemType.AlternatingItem)
            {
                int jobTypeID = Convert.ToInt32(drv["jobtypeID"]);
                int scheduledStatusID = Convert.ToInt32(drv["scheduledstatusID"]);

                ImageButton tmpButton = (ImageButton)gvr.FindControl("DeleteJobButton");
                tmpButton.Attributes.Add("title", "Delete");
                tmpButton.OnClientClick = "if (confirm('Are you sure you want to delete this scheduled job?')) return true; else return false;";

                tmpButton = (ImageButton)gvr.FindControl("ButtonReroster");
                if (jobTypeID == 2 && scheduledStatusID == (int)BDI2Core.Common.ScheduledStatus.Finished)
                {
                    tmpButton.Visible = true;
                }
                else
                {
                    tmpButton.Visible = false;
                }

                tmpButton = (ImageButton)gvr.FindControl("ButtonDownload");
                tmpButton.Attributes.Add("title", "Download");
                if ((jobTypeID == 1 || jobTypeID == 4) && scheduledStatusID == (int)BDI2Core.Common.ScheduledStatus.Finished)
                {
                    tmpButton.Visible = true;
                }
                else
                {
                    tmpButton.Visible = false;
                }

                tmpButton = (ImageButton)gvr.FindControl("ButtonViewFilters");
                tmpButton.Attributes.Add("title", "View Filters");
                tmpButton.OnClientClick = "ShowNewWindow(" + drv["scheduledJobID"].ToString() + ");return false;";
                if (jobTypeID == 3 && Convert.ToInt32(drv["showRosterFilter"]) == 0)
                {
                    tmpButton.Visible = false;
                }

                tmpButton = (ImageButton)gvr.FindControl("ButtonMIRoster");
                if (jobTypeID == 3 && scheduledStatusID == (int)BDI2Core.Common.ScheduledStatus.ReadyToRoster)
                {
                    tmpButton.Attributes.Add("title", "Schedule Roster");
                    tmpButton.Visible = true;
                }
                else
                {
                    tmpButton.Visible = false;
                }


                /*
                string filter;
                Literal tmpLiteral = (Literal)gvr.FindControl("FilterLabel");
                filter = "Org:" + drv["organizationDescription"].ToString();
                filter = filter + "<br>Program Note 1:";

                tmpLiteral.Text = filter;
                 */
            }

        }

        protected void rgvJobs_ItemCommand(object source, GridCommandEventArgs e)
        {
            //Trace.Warn(e.CommandName + " - " + e.CommandArgument.ToString());

            switch (e.CommandName)
            {
                case "REROSTER":
                    Session["PAGE_17_studentID"] = e.CommandArgument.ToString();
                    Response.Redirect("EditChild.aspx");
                    break;
                case "DOWNLOAD":
                    ScheduledJob sj = ScheduledJob.GetScheduldedJobByID(int.Parse(e.CommandArgument.ToString()));
                    ScheduledJob.BrowserDownload(ConfigurationManager.AppSettings["outputpath"] + "\\" + sj.FileName);
                    break;
                case "MIROSTER":
                    Session["scheduledJobID"] = e.CommandArgument.ToString();                 
                    Response.Redirect("~/Student/RosterCriteria.aspx");
                    
                    break;
            }
        }

        protected void rgvJobs_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            LoadGrid();
        }

        protected void rgvJobs_SortCommand(object source, GridSortCommandEventArgs e)
        {
            LoadGrid();
        }

        #endregion Events
        
        #region Private Events

        private void LoadGrid()
        {
            rgvJobs.DataSource = ScheduledJob.GetScheduledJobsByStaffID(SessionData.Staff.StaffID.Value);
            rgvJobs.DataBind();
        }
        #endregion Events

        #region Virtual Methods

        protected override void SetPageID()
        {
            PageID = 56;
        }

        #endregion Virtual Methods
    }
}
