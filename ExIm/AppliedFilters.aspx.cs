using System;
using BDI2Core.Common;
using BDI2Web.Common_Code;
using System.Data;
namespace BDI2Web.ExIm
{
    public partial class AppliedFilters : SecureBasePage
    {
        protected string fileOptionsMsg = "Delimiter:";
        protected string fromDateMsg = "From Date:";
        protected string toDateMsg = "To Date:";
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string scheduledJobID = Request.QueryString.Get("sjid");

                if(scheduledJobID.Trim().Length > 0)
                {
                    ScheduledJob sj = ScheduledJob.GetScheduldedJobByID(Convert.ToInt32(scheduledJobID));
                    JobIDLabel.Text = sj.ScheduledJobID.ToString();
                    DescriptionLabel.Text = sj.Description;

                    //Added the If condition: Ticket# BDI-82 
                    if (sj.ProgramNoteOperator == ProgramNoteOperator.None)
                        ProgramNoteOperatorLabel.Text = string.Empty;                    
                    else
                        ProgramNoteOperatorLabel.Text = sj.ProgramNoteOperator.ToString();

                    //ProgramNoteOperatorLabel.Text = sProgramNoteOperatorLabelText.ToString();
                    ToDateLabel.Text = sj.ToDate.ToShortDateString();
                    FromDateLabel.Text = sj.FromDate.ToShortDateString();
                    ProgramNote1Label.Text = sj.ProgramNote1Description == "" ? "&nbsp;" : sj.ProgramNote1Description;
                    ProgramNote2Label.Text = sj.ProgramNote2Description == "" ? "&nbsp;" : sj.ProgramNote2Description;
                    if (sj.JobType == ScheduledJobType.Import)
                    {                          
                        fileOptionsMsg = "File options:";
                        fromDateMsg = "Import Beginning Date:";
                        toDateMsg = "Import Ending Date:";                        
                        ScheduleRoster sr = new ScheduleRoster();
                        sr.ScheduledJobID = Convert.ToInt32(scheduledJobID);
                        sr.CustomerID = SessionData.CustomerID.Value;
                        DataSet ds = sr.GetScheduledRosterInfoImportProcess();
                        string strFileOptions;
                        strFileOptions = (sj.UpdtDemographics) ? "Update Demographics" : "";
                        strFileOptions = (sj.UpdtAssessments) ? (strFileOptions.Length > 0) ? strFileOptions + ", Update Assessments" : "Update Assessments" : strFileOptions + "";
                        strFileOptions = (sj.UpdtProgramNotes) ? (strFileOptions.Length > 0) ? strFileOptions + ", Update Program Notes" : "Update Program Notes" : strFileOptions + "";
                        strFileOptions = (sj.UpdtUDF) ? (strFileOptions.Length > 0) ? strFileOptions + " and Update User Defined Values" : "Update User Defined Values" : strFileOptions + "";
                        DelimitorLabel.Text = strFileOptions;
                        organizationRow.Visible = false;
                        //string htmlEmail;
                        //htmlEmail = sr.GetScheduledRosterHTMLInfoImportProcess();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            RosterInfoPanel.Visible = true;
                            ResultsGridView.DataSource = ds.Tables[0].DefaultView;
                            ResultsGridView.DataBind();
                        }
                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            ChildIdPanel.Visible = true;
                            ChildIdGridView.DataSource = ds.Tables[1].DefaultView;
                            ChildIdGridView.DataBind();
                        }
                    }
                    else if (sj.JobType == ScheduledJobType.GroupReport)
                    {
                        pnlNotesFilter.Visible = false;
                        RosterInfoPanel.Visible = false;
                        ChildIdPanel.Visible = false;
                        FileOptionRow.Visible = false;
                        GroupReportPanel.Visible = true;
                        organizationRow.Visible = true;

                        //Screener
                        if(sj.ReportId == 19)
                        {
                            string[] dateRange = sj.AssessementDateRange.Split('|');
                            FromDateLabel.Text = dateRange[0];
                            ToDateLabel.Text = dateRange[1];
                            StdDevLabel.Text = sj.DomainDeviationList;

                            FromDateRow.Visible = true;
                            ToDateRow.Visible = true;
                            StdDevRow.Visible = true;
                            ScoreTypeRow.Visible = false;
                            FromDate1Row.Visible = false;
                            ToDate1Row.Visible = false;
                            FromDate2Row.Visible = false;
                            ToDate2Row.Visible = false;
                            FromDate3Row.Visible = false;
                            ToDate3Row.Visible = false;

                        }
                        //Head Start
                        else if (sj.ReportId == 22)
                        {
                            string[] dateRanges = sj.AssessementDateRange.Split(',');
                            string[] aDateRange = dateRanges[0].Split('|');

                            FromDate1Label.Text = aDateRange[0];
                            ToDate1Label.Text = aDateRange[1];

                            if (dateRanges.Length >1)
                            {
                                aDateRange = dateRanges[1].Split('|');
                                FromDate2Label.Text = aDateRange[0];
                                ToDate2Label.Text = aDateRange[1];
                            }
                            if(dateRanges.Length >2)
                            {
                                aDateRange = dateRanges[2].Split('|');
                                FromDate3Label.Text = aDateRange[0];
                                ToDate3Label.Text = aDateRange[1];
                            }

                            FromDateRow.Visible = false;
                            ToDateRow.Visible = false;
                            StdDevRow.Visible = false;
                            ScoreTypeRow.Visible = false;
                            FromDate1Row.Visible = true;
                            ToDate1Row.Visible = true;
                            FromDate2Row.Visible = true;
                            ToDate2Row.Visible = true;
                            FromDate3Row.Visible = true;
                            ToDate3Row.Visible = true;
                            
                        }
                        //Complete Roster
                        else if (sj.ReportId == 23)
                        {
                            string[] dateRange = sj.AssessementDateRange.Split('|');
                            FromDateLabel.Text = dateRange[0];
                            ToDateLabel.Text = dateRange[1];
                            
                            //TODO: Populate Score Types
                            string[] scoreTypes = sj.ScoreList.Split(',');
                            Report rpt = new Report(23);
                            DataSet ds = rpt.GetScoreTypes();
                            
                            foreach(string st in scoreTypes)
                            {
                                ScoreTypeLabel.Text = ScoreTypeLabel.Text + ds.Tables[0].Select("reportscoreTypeID = " + st)[0]["description"].ToString() + "<br/>";                                
                            }

                            FromDateRow.Visible = true;
                            ToDateRow.Visible = true;
                            StdDevRow.Visible = false;
                            ScoreTypeRow.Visible = true;
                            FromDate1Row.Visible = false;
                            ToDate1Row.Visible = false;
                            FromDate2Row.Visible = false;
                            ToDate2Row.Visible = false;
                            FromDate3Row.Visible = false;
                            ToDate3Row.Visible = false;
                        }

                    }
                    else
                    {
                        organizationRow.Visible = true;
                        fileOptionsMsg = "Delimiter:";
                        if (sj.Delimitor.ToLower() == "xml")
                            DelimitorLabel.Text = "XML";
                        else
                            DelimitorLabel.Text = sj.Delimitor == "," ? "Comma" : "Tab";
                    }
                    OrganizationRecord org = new OrganizationRecord(sj.OrganizaitonID);
                    org.LoadFromDB();
                    OrganizationLabel.Text = org.Description;

                    if (sj.JobCode.ToUpper() == "ASSESSMENTDETAIL" || sj.JobCode.ToUpper() == "ASSESSMENTDOMAINS" || sj.JobType == ScheduledJobType.Import || sj.JobType == ScheduledJobType.GroupReport)
                        pnlNotesFilter.Visible = false;
                    else
                        pnlNotesFilter.Visible = true;


                }
            }
        }

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 57; //Value from Page Table in db
        }

        #endregion Overridden Functions
    }
}
