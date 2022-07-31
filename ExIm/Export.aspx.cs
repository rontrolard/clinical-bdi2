using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BDI2Core.Common;
using BDI2Core.Configuration;
using BDI2Core.Data;
using BDI2Web.Common_Code;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

namespace BDI2Web.ExIm
{
    public partial class Export : SecureBasePage
    {
        #region Variable Declarations

        private const string _crlf = "\r\n";
        private const string _buttonScheduled = "Schedule Export";
        private const string _buttonExport = "Export File";        

        #endregion

        #region Private Functions

        private void LoadProgramNotesDL()
        {
            DataSet ds = Lookups.GetProgramNote();
            FillListControl(ddlProgramNote1, ds, "description", "programnoteid", false);
            ddlProgramNote1.Items.Insert(0, new ListItem("", Int32.MinValue.ToString()));
            FillListControl(ddlProgramNote2, ds, "description", "programnoteid", false);
            ddlProgramNote2.Items.Insert(0, new ListItem("", Int32.MinValue.ToString()));

            ddlProgramNoteJoin.Items.Add(new ListItem("", " "));
            ddlProgramNoteJoin.Items.Add(new ListItem("And", "and"));
            ddlProgramNoteJoin.Items.Add(new ListItem("Or", "or"));

            ddlProgramNote2.Enabled = false;

        }
        private void ReLoadProgramNotesDL(DropDownList list)
        {
            DataSet ds = Lookups.GetProgramNote();
            FillListControl(list, ds, "description", "programnoteid", false);
            list.Items.Insert(0, new ListItem("", Int32.MinValue.ToString()));            
        }
        private void buildExportFile()
        {
            string stFileName;
            if (rblFormat.SelectedValue == "BDI3")
            {
                stFileName = rblFile.SelectedValue + rblFormat.SelectedValue + ".csv";
            }
            else
            {
                stFileName = rblFile.SelectedValue + (rblFormat.SelectedValue == "," ? ".csv" : ".txt");
            }
            string strContentType = "application/octet-stream";

            // set up the output file name
            Response.Clear();

            Response.AddHeader("Content-Disposition", "attachment; filename=" + stFileName);
            // tell browser its a file not html
            Response.ContentType = strContentType;
            // now extract and write the file
            if (rblFile.SelectedValue == "Organizations" || rblFile.SelectedValue == "Staff" || rblFile.SelectedValue == "Students")
            {
                if (!ExecuteOrganizationBasedExtract())
                {
                    SetErrorMessage("No Data found.");
                    return;
                }
            }
            else
            {
                if (!ExecuteDateBasedExtract(stFileName))
                {
                    SetErrorMessage("No Data found.");
                    return;
                }
            }
            // end the process
            if(Response.IsClientConnected)
            {
                Response.Flush();
            }
            Response.End();

        }

        private bool ExecuteDateBasedExtract(string fileName)
        {
            string strExtractField;
            bool isValid;
            string stProcedure = "exim.eiExport" + rblFile.SelectedValue;
            Int32 icust = Convert.ToInt32(Session["CustomerID"]);

            try
            {
                bool hasNextRecord = false;
                StringBuilder sbLine = new StringBuilder();

                String[] userParams = new String[5];
                if (rblFile.SelectedValue == "StudentAssessmentSum")
                {
                    userParams = new String[8];
                }

                userParams[0] = icust.ToString();
                userParams[1] = FromDate.SelectedDate.Value.ToShortDateString();
                userParams[2] = ToDate.SelectedDate.Value.ToShortDateString();
                userParams[3] = LabelOrganizationID.Text;
                userParams[4] = SessionData.Staff.StaffID.ToString();

                if (rblFile.SelectedValue == "StudentAssessmentSum")
                {
                    userParams[5] = ddlProgramNote1.SelectedIndex != 0 ? ddlProgramNote1.SelectedValue : "-1";
                    userParams[6] = ddlProgramNote2.SelectedIndex != 0 ? ddlProgramNote2.SelectedValue : "-1";
                    userParams[7] = ddlProgramNote2.SelectedIndex != 0 && ddlProgramNoteJoin.SelectedIndex != 0 ? ddlProgramNote1.SelectedValue : "-1";
                }

                SqlDataReader myReader = SQLHelper.ExecuteReaderWithTimeout(Settings.ConnectionStringName, stProcedure, Settings.GetSqlConnectionTimeOut(), userParams);

                if (myReader.HasRows)
                {
                    isValid = true;
                    hasNextRecord = myReader.Read();
                    while (hasNextRecord)
                    {
                        // loop through the returned colums adding them to the output string
                        for (int i = 0; i < myReader.FieldCount; i++)
                        {
                            strExtractField = myReader[i].ToString().Trim();
                            if (!myReader.GetName(i).Equals("RowNumber"))
                            {
                                if (rblFormat.SelectedValue == "," && strExtractField.Contains(","))
                                    strExtractField = @"""" + strExtractField + @"""";
                                sbLine.Append(strExtractField);
                                //Not to Add Comma or Tab after the last column.
                                if (i != myReader.FieldCount - 1)
                                    sbLine.Append(rblFormat.SelectedValue == "," ? "," : "\t");
                            }
                        }
                        sbLine.Append(_crlf);

                        if (!Response.IsClientConnected)
                        {
                            isValid = false;
                            break;
                        }
                        //write line
                        Response.Write(sbLine);
                        sbLine.Remove(0, sbLine.Length);

                        hasNextRecord = myReader.Read();
                    }
                    myReader.Close();
                    
                }
                else
                {
                    myReader.Close();
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                //_msg = ex.Message.ToString();
                //todo: possible remove
                throw new Exception(ex.Message);
            }

            return isValid;
        }

        private bool ExecuteOrganizationBasedExtract()
        {
            //			string Value;
            string strExtractField;
            bool isValid;
            string stProcedure;
            if (rblFormat.SelectedValue == "BDI3")
            {
                stProcedure = "exim.eiExport" + rblFile.SelectedValue + rblFormat.SelectedValue;
            }
            else
            {
                stProcedure = "exim.eiExport" + rblFile.SelectedValue;
            }
            Int32 icust = Convert.ToInt32(Session["CustomerID"]);

            try
            {
                bool hasNextRecord;
                StringBuilder sbLine = new StringBuilder();

                //SqlParameter[] userParams = new SqlParameter[2];
                //userParams[0] = new SqlParameter("@CustomerID", icust);
                //userParams[1] = new SqlParameter("@Organization", Convert.ToInt32(LabelOrgSelected.Text));

                String[] userParams = new String[3];
                userParams[0] = icust.ToString();
                userParams[1] = LabelOrganizationID.Text;
                userParams[2] = SessionData.Staff.StaffID.ToString();

                SqlDataReader myReader = SQLHelper.ExecuteReaderWithTimeout(Settings.ConnectionStringName, stProcedure, Settings.GetSqlConnectionTimeOut(), userParams);

                if (myReader.HasRows)
                {
                    isValid = true;
                    hasNextRecord = myReader.Read();
                    while (hasNextRecord)
                    {
                        // loop through the returned colums adding them to the output string
                        for (int i = 0; i < myReader.FieldCount; i++)
                        {
                            strExtractField = myReader[i].ToString().Trim();
                            if (!myReader.GetName(i).Equals("RowNumber"))
                            {
                                sbLine.Append(strExtractField);
                                //Not to Add Comma or Tab after the last column.
                                if (i != myReader.FieldCount - 2)
                                    sbLine.Append((rblFormat.SelectedValue == "," || rblFormat.SelectedValue == "BDI3") ? "," : "\t");
                            }
                        }
                        sbLine.Append(_crlf);

                        if (!Response.IsClientConnected)
                        {
                            isValid = false;
                            break;
                        }
                        //write line
                        Response.Write(sbLine);
                        sbLine.Remove(0, sbLine.Length);

                        hasNextRecord = myReader.Read();
                    }
                    myReader.Close();
                    
                }
                else
                {
                    myReader.Close();
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                //_msg = ex.Message.ToString();
                //todo: possible remove
                throw new Exception(ex.Message);
            }

            return isValid;
        }

        #endregion Private Functions

        #region UI Events

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccess(Permission.Export, false);

            FromDate.MaxDate = DateTime.Today;
            ToDate.MaxDate = DateTime.Today;

            if (!Page.IsPostBack)
            {
                //Build File type list
                if (SessionData.Staff.LevelType.Equals(LevelType.TopLevel))
                {
                    rblFile.Items.Add("Organizations");
                    rblFile.Items.Add("Staff");
                }
                rblFile.Items.Add("Students");
                rblFile.Items.Add(new ListItem("Assessment Summary", "AssessmentSummary"));
                rblFile.Items.Add(new ListItem("Assessment Domains", "AssessmentDomains"));
                rblFile.Items.Add(new ListItem("Assessment Details", "AssessmentDetail"));
                rblFile.Items.Add(new ListItem("Assessment Observations", "AssessmentObservation"));
                rblFile.Items.Add(new ListItem("Student and Assessment Summary", "StudentAssessmentSum"));
                rblFile.Items.Add(new ListItem("Migration Export", "MigrationExport"));
                
                rblFile.Items[0].Selected = true;
                rblFormat.Items[0].Selected = true;
                rblFormat.Items[2].Enabled = false;
                rblFormat.Items[3].Enabled = false;
                OrgTreePanel.Visible = true;
                butExport.Enabled = true;

                LoadProgramNotesDL();

                //support staff under multiple parents now
                DataSet ds = OrganizationHelper.GetOrganizationTreeForStaff(SessionData.Staff.StaffID.Value, true);

                t1.DataSource = ds;
                t1.DataBind();
                t1.CollapseAllNodes();
                //t2.DataSource = ds;
                //t2.DataBind();
                //t2.CollapseAllNodes();
                //selection
                if (Session["Export_SelectionIndex"] != null)
                {
                    int selectedOrgID = (int)Session["Export_SelectionIndex"];
                    ArrayList selectionTree = new ArrayList();
                    OrganizationHelper.BuildSelectionTree(ref selectionTree, selectedOrgID);

                    if (selectionTree.Count > 1)
                    {
                        RadTreeNode currentNode = t1.Nodes[0];
                        currentNode.Expanded = true;

                        while (currentNode.Nodes.Count > 0)
                        {
                            bool bFound = false;
                            foreach (RadTreeNode node in currentNode.Nodes)
                            {
                                if (selectionTree.Contains(node.Value))
                                {
                                    bFound = true;
                                    node.Expanded = true;
                                    currentNode = node;
                                    break;
                                }
                            }
                            if (!bFound)
                            {
                                break;
                            }

                            if (currentNode.Value == selectedOrgID.ToString())
                            {
                                break;
                            }
                        }
                    }
                }
            }
            ClearStatusMessageLabel();
        }

        protected void RunScheduleJobButton_Click(object sender, EventArgs e)
        {
            int scheduleJobID;
            
            if (Int32.TryParse(ScheduleJobIDTextBox.Text, out scheduleJobID))
            {
                ScheduledJob.ProcessJob(scheduleJobID, Settings.GetOutputPath());
            }
            
        }

        protected void butExport_Click(object sender, EventArgs e)
        {
            if (rblFile.SelectedValue != "MigrationExport")
            {
                if (IsValidForm())
                {
                    if (rblFile.SelectedValue == "AssessmentDetail" || rblFile.SelectedValue == "AssessmentDomains" || rblFile.SelectedValue == "StudentAssessmentSum")
                    {
                        StringBuilder sb = new StringBuilder();                        
                        ScheduledJob sj = new ScheduledJob();
                        sj.ProgramNoteOperator = ProgramNoteOperator.None;
                        if (ddlProgramNoteJoin.SelectedItem.Value.ToLower() == "and")
                            sj.ProgramNoteOperator = ProgramNoteOperator.And;
                        else if (ddlProgramNoteJoin.SelectedItem.Value.ToLower() == "or")
                            sj.ProgramNoteOperator = ProgramNoteOperator.Or;

                        if (rblFile.SelectedValue == "AssessmentDetail")
                        {
                            sj.JobType = ScheduledJobType.Export;
                            sj.Description = "Export - Assessment Details";
                        }
                        else if (rblFile.SelectedValue == "StudentAssessmentSum")
                        {
                            sj.JobType = ScheduledJobType.Export;
                            sj.Description = "Export - Student and Assessment Summary";
                            sj.ProgamNoteID1 = Convert.ToInt32(ddlProgramNote1.SelectedValue);
                            sj.ProgramNoteID2 = Convert.ToInt32(ddlProgramNote2.SelectedValue);
                        }
                        else if (rblFile.SelectedValue == "AssessmentDomains")
                        {
                            sj.JobType = ScheduledJobType.Export;
                            sj.Description = "Export - Assessment Domains";
                        }
                        sj.StaffID = SessionData.Staff.StaffID.Value;
                        sj.Status = BDI2Core.Common.ScheduledStatus.NotStarted;
                        sj.OrganizaitonID = Convert.ToInt32(LabelOrganizationID.Text);
                        sj.ToDate = ToDate.SelectedDate.Value;
                        sj.FromDate = FromDate.SelectedDate.Value;
                        sj.Email = EmailTextBox.Text;
                        sj.JobCode = rblFile.SelectedValue;
                        sj.Delimitor = rblFormat.SelectedValue == "," ? "," : "\t";

                        sj.Save();

                        sb.Append("<table>");
                        sb.Append("<tr><td align='right'>Job ID:</td><td alighn='left'>");
                        sb.Append(sj.ScheduledJobID.ToString());
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td align='right'>Description:</td><td alighn='left'>");
                        sb.Append("Export - ");
                        sb.Append(rblFile.SelectedItem.Text);
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td align='right'>Organization Name:</td><td alighn='left'>");
                        OrganizationRecord orgRec = new OrganizationRecord(sj.OrganizaitonID);
                        orgRec.LoadFromDB();
                        sb.Append(orgRec.Description);
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td align='right'>From Date:</td><td alighn='left'>");
                        sb.Append(sj.FromDate.ToString("MM/dd/yyyy"));
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td align='right'>To Date:</td><td alighn='left'>");
                        sb.Append(sj.ToDate.ToString("MM/dd/yyyy"));
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td align='right'>Delimiter:</td><td alighn='left'>");
                        sb.Append(rblFormat.SelectedItem.Text);
                        sb.Append("</td></tr>");

                        if (rblFile.SelectedValue == "StudentAssessmentSum")
                        {
                            sb.Append("<tr><td align='right'>Program Note Criteria 1:</td><td alighn='left'>");
                            sb.Append(ddlProgramNote1.SelectedItem.Text);
                            sb.Append("</td></tr>");

                            sb.Append("<tr><td align='right'>And/Or Operator:</td><td alighn='left'>");
                            sb.Append(ddlProgramNoteJoin.SelectedItem.Text);
                            sb.Append("</td></tr>");

                            sb.Append("<tr><td align='right'>Program Note Criteria 2:</td><td alighn='left'>");
                            sb.Append(ddlProgramNote2.SelectedItem.Text);
                            sb.Append("</td></tr>");
                        }
                        sb.Append("</table>");

                        ScheduledJob.SendJobConfirmationEmail(sj, sb.ToString());

                        SessionData.StatusMessage = "Export has been scheduled.";
                        Response.Redirect("ScheduledStatus.aspx");
                    }


                    butExport.Enabled = false;
                    buildExportFile();
                    OrgTreePanel.Visible = false;
                    DatesPanel.Visible = false;
                    EmailPanel.Visible = false;
                    //lblInstruction.Visible = true;
                }
            }

            //MigrationExport Extract
            else if (rblFile.SelectedValue == "MigrationExport")
            {
                if (IsValidForm())
                {
                    ScheduledJob sj = new ScheduledJob();
                    StringBuilder sb = new StringBuilder();                    

                    sj.JobType = ScheduledJobType.Export;
                    sj.Description = "Export - Migration Export";
                    sj.ProgamNoteID1 = Convert.ToInt32(ddlProgramNote1.SelectedValue);
                    sj.ProgramNoteID2 = Convert.ToInt32(ddlProgramNote2.SelectedValue);
                    sj.StaffID = SessionData.Staff.StaffID.Value;
                    sj.Status = BDI2Core.Common.ScheduledStatus.NotStarted;
                    sj.OrganizaitonID = Convert.ToInt32(LabelOrganizationID.Text);
                    sj.ToDate = ToDate.SelectedDate.Value;
                    sj.FromDate = FromDate.SelectedDate.Value;
                    sj.Email = EmailTextBox.Text;
                    sj.JobCode = rblFile.SelectedValue;
                    sj.Delimitor = rblFormat.SelectedValue;

                    sj.ProgramNoteOperator = ProgramNoteOperator.None;
                    if (ddlProgramNoteJoin.SelectedItem.Value.ToLower() == "and")
                        sj.ProgramNoteOperator = ProgramNoteOperator.And;
                    else if (ddlProgramNoteJoin.SelectedItem.Value.ToLower() == "or")
                        sj.ProgramNoteOperator = ProgramNoteOperator.Or;                    

                    sj.Save();

                    sb.Append("<table>");
                    sb.Append("<tr><td align='right'>Job ID:</td><td alighn='left'>");
                    sb.Append(sj.ScheduledJobID.ToString());
                    sb.Append("</td></tr>");

                    sb.Append("<tr><td align='right'>Description:</td><td alighn='left'>");
                    sb.Append("Export - ");
                    sb.Append(rblFile.SelectedItem.Text);
                    sb.Append("</td></tr>");

                    sb.Append("<tr><td align='right'>Organization Name:</td><td alighn='left'>");
                    OrganizationRecord orgRec = new OrganizationRecord(sj.OrganizaitonID);
                    orgRec.LoadFromDB();
                    sb.Append(orgRec.Description);                    
                    sb.Append("</td></tr>");
                    
                    sb.Append("<tr><td align='right'>From Date:</td><td alighn='left'>");
                    sb.Append(sj.FromDate.ToString("MM/dd/yyyy"));
                    sb.Append("</td></tr>");

                    sb.Append("<tr><td align='right'>To Date:</td><td alighn='left'>");
                    sb.Append(sj.ToDate.ToString("MM/dd/yyyy"));
                    sb.Append("</td></tr>");

                    sb.Append("<tr><td align='right'>Program Note Criteria 1:</td><td alighn='left'>");
                    sb.Append(ddlProgramNote1.SelectedItem.Text);
                    sb.Append("</td></tr>");

                    sb.Append("<tr><td align='right'>And/Or Operator:</td><td alighn='left'>");
                    sb.Append(ddlProgramNoteJoin.SelectedItem.Text);
                    sb.Append("</td></tr>");

                    sb.Append("<tr><td align='right'>Program Note Criteria 2:</td><td alighn='left'>");
                    sb.Append(ddlProgramNote2.SelectedItem.Text);
                    sb.Append("</td></tr>");

                    sb.Append("</table>");

                    ScheduledJob.SendJobConfirmationEmail(sj, sb.ToString());
                    SessionData.StatusMessage = "Export has been scheduled.";
                    Response.Redirect("ScheduledStatus.aspx");
                }
            }
        }

        protected void rblFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblFormat.Items[0].Enabled = true;
            rblFormat.Items[1].Enabled = true;
            rblFormat.Items[2].Enabled = false;
            rblFormat.Items[3].Enabled = false;
            rblFormat.Items[0].Selected = true;
            rblFormat.Items[1].Selected = false;
            rblFormat.Items[2].Selected = false;
            rblFormat.Items[3].Selected = false;

            if (rblFile.SelectedValue == "Organizations" || rblFile.SelectedValue == "Staff" || rblFile.SelectedValue == "Students")
            {
                //lblInstruction.Text = "Select organization from tree and press the Export File button";
                //lblInstruction.Visible = true;
                OrgTreePanel.Visible = true;
                DatesPanel.Visible = false;
                EmailPanel.Visible = false;
                butExport.Text = _buttonExport;
                ProgramNotesPanel.Visible = false;
                if (rblFile.SelectedValue == "Staff" || rblFile.SelectedValue == "Students")
                {
                    rblFormat.Items[3].Enabled = true;
                }
            }
            else if (rblFile.SelectedValue == "AssessmentDetail" || rblFile.SelectedValue == "AssessmentDomains")
            {
                //lblInstruction.Text = "Select the beginning and ending dates for the export and press the Export File button";
                //lblInstruction.Visible = true;
                //OrgTreePanel.Visible = false;
                DatesPanel.Visible = true;
                EmailPanel.Visible = true;
                butExport.Text = _buttonScheduled;
                EmailTextBox.Text = SessionData.Staff.Email;
                ProgramNotesPanel.Visible = false;
            }
            else if (rblFile.SelectedValue == "StudentAssessmentSum")
            {
                //lblInstruction.Text = "Select the beginning dates, ending dates and program notes for the export and press the Export File button";
                //lblInstruction.Visible = true;
                //OrgTreePanel.Visible = false;
                DatesPanel.Visible = true;
                EmailPanel.Visible = true;
                butExport.Text = _buttonScheduled;
                EmailTextBox.Text = SessionData.Staff.Email;
                ProgramNotesPanel.Visible = true;
            }
            else if (rblFile.SelectedValue == "MigrationExport")
            {
                //lblInstruction.Text = "Select the beginning and ending dates for the export and press the Export File button";
                //lblInstruction.Visible = true;
                //OrgTreePanel.Visible = false;
                DatesPanel.Visible = true;
                EmailPanel.Visible = true;
                butExport.Text = _buttonScheduled;
                EmailTextBox.Text = SessionData.Staff.Email;
                ProgramNotesPanel.Visible = true;

                rblFormat.Items[0].Enabled = false;
                rblFormat.Items[1].Enabled = false;
                rblFormat.Items[2].Enabled = true;
                rblFormat.Items[0].Selected = false;
                rblFormat.Items[1].Selected = false;
                rblFormat.Items[2].Selected = true;
            }
            else
            {
                //lblInstruction.Text = "Select the beginning and ending dates for the export and press the Export File button";
                //lblInstruction.Visible = true;
                //OrgTreePanel.Visible = false;
                DatesPanel.Visible = true;
                EmailPanel.Visible = false;
                butExport.Text = _buttonExport;
                ProgramNotesPanel.Visible = false;
            }
        }

        protected void rtvOrganization_NodeClick(object o, RadTreeNodeEventArgs e)
        {
            bool canExport = false;
            bool isBottomLevel = false;
            int organizationID = int.Parse(e.Node.Value);
            LabelOrgSelected.Text = e.Node.Text;
            Session["Export_SelectionIndex"] = organizationID;

            LabelOrganizationID.Text = organizationID.ToString();
            OrganizationRecord orgRec = new OrganizationRecord(organizationID);
            orgRec.LoadFromDB();

            HierarchyRecord hr = new HierarchyRecord(orgRec.HierarchyLevelID);
            hr.LoadFromDB();
            if (hr.LevelTypeID == LevelType.BottomLevel)
            {
                isBottomLevel = true;
            }

            LabelHierarchyLevelID.Text = orgRec.HierarchyLevelID.ToString();
            Int32 icust = Convert.ToInt32(Session["CustomerID"]);
            DataSet ds = OrganizationHelper.GetChildCount(icust, SessionData.Staff.StaffID.Value, organizationID, isBottomLevel);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LabelOrgSelected.Text = e.Node.Text + " [" + ds.Tables[0].Rows[0].ItemArray[0].ToString() + "]";
            }


            HandleStaffSecurity(orgRec.HierarchyLevelID, orgRec.LevelTypeID, ref canExport);

            //if (canExport)
            //{
            //    butExport.Enabled = true;
            //}
        }

        protected void ddlProgramNote1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProgramNote1.SelectedIndex == 0)
            {
                ddlProgramNoteJoin.SelectedIndex = 0;
                ddlProgramNote2.SelectedIndex = 0;
                ddlProgramNote2.Enabled = false;
            }
            else
            {
                string selectedValue = "";
                selectedValue = ddlProgramNote2.Items[ddlProgramNote2.SelectedIndex].Value;
                ReLoadProgramNotesDL(ddlProgramNote2);
                ddlProgramNote2.Items.Remove(ddlProgramNote1.Items[ddlProgramNote1.SelectedIndex]);
                ddlProgramNote2.Items.FindByValue(selectedValue).Selected = true;
            }
            if (ddlProgramNoteJoin.SelectedIndex > 0)
            {
                ddlProgramNote2.Enabled = true;
            }
        }

        protected void ddlProgramNote2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProgramNote2.SelectedIndex != 0)
            {
                string selectedValue = "";
                selectedValue = ddlProgramNote1.Items[ddlProgramNote1.SelectedIndex].Value;
                ReLoadProgramNotesDL(ddlProgramNote1);
                ddlProgramNote1.Items.Remove(ddlProgramNote2.Items[ddlProgramNote2.SelectedIndex]);
                ddlProgramNote1.Items.FindByValue(selectedValue).Selected = true;
            }            
        }
        protected void ddlProgramNoteJoin_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlProgramNoteJoin.SelectedIndex == 0 || ddlProgramNote1.SelectedIndex == 0)
            {
                ddlProgramNote2.SelectedIndex = 0;
                ddlProgramNote2.Enabled = false;
            }
            else
            {
                ddlProgramNote2.SelectedIndex = 0;
                ddlProgramNote2.Enabled = true;
            }

        }

        #endregion

        #region Security
        protected void HandleStaffSecurity(int orgHierarchyLevelID, int levelTypeID, ref bool canExport)
        {
            canExport = true;
        }

        private bool IsValidForm()
        {

            bool isValid = true;
            ArrayList errorList = new ArrayList();
            if (OrgTreePanel.Visible == true)
            {
                if (LabelOrganizationID.Text.Length < 1)
                {
                    errorList.Add("Organization is required");
                }
            }
            if (DatesPanel.Visible == true)
            {
                //if (LabelOrganizationID.Text.Length < 1)
                //{
                //    errorList.Add("Organization is required for Scheduled Export.");
                //}
                if (FromDate.SelectedDate == null)
                {
                    errorList.Add("Beginning Date cannot be blank or be greater than today.");
                }
                if (ToDate.SelectedDate == null)
                {
                    errorList.Add("Ending Date cannot be blank or be greater than today.");
                }
                if ((ToDate.SelectedDate != null) && (FromDate.SelectedDate != null))
                {
                    if (FromDate.SelectedDate > ToDate.SelectedDate)
                    {
                        errorList.Add("'Ending Date' must be greater or equal to 'Beginning Date'.");
                    }
                }

            }

            if (EmailPanel.Visible)
            {
                if (EmailTextBox.Text.Trim() == "")
                {
                    errorList.Add("E-mail is required");
                }
                else
                {
                    if (!BaseDataObject.IsValidEmail(EmailTextBox.Text))
                    {
                        errorList.Add("E-mail is not in correct format");
                    }
                }
            }

            if (errorList.Count > 0)
            {
                SetErrorMessageWithArrayList(errorList);
                isValid = false;
            }

            return isValid;
        }

        #endregion

        #region Overridden Functions
        protected override void SetPageID()
        {
            PageID = 49; //Value from Page Table in db
        }

        #endregion Overridden Functions


        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    btnCompleteExport_Click(sender, e);
        //}

        //protected void btnCompleteExport_Click(object sender, EventArgs e)
        //{
        //    if (IsValidForm())
        //    {
        //        string conStr, fileName = string.Empty;
        //        int custID, staffID, organizationID = Int32.MinValue;
        //        DateTime startDate, endDate = DateTime.MinValue;
        //        int programNote1, programNote2 = Int32.MinValue;
        //        string joinValue = string.Empty;

        //        conStr = @"Data Source=10.88.22.122;Initial Catalog=Copperhead_Prod;Persist Security Info=True;User ID=sa;Password=6618lkp";
        //        fileName = @"c:\BDI2_CompleteExport";
        //        custID = Convert.ToInt32(Session["CustomerID"]);
        //        staffID = SessionData.Staff.StaffID.Value;
        //        organizationID = Convert.ToInt32(LabelOrganizationID.Text);
        //        startDate = FromDate.SelectedDate.Value;
        //        endDate = ToDate.SelectedDate.Value;
        //        programNote1 = -1; //Convert.ToInt32(ddlProgramNote1.SelectedValue);
        //        programNote2 = -1; //Convert.ToInt32(ddlProgramNote2.SelectedValue);
        //        joinValue = "";  //ddlProgramNoteJoin.SelectedItem.Value;

        //        Report objReport = new Report();
        //        DataSet ds = objReport.GetCompleteExportDataSet(conStr, custID, staffID, organizationID, startDate, endDate, programNote1, programNote2, joinValue);

        //        ds.WriteXml(fileName + ".XML", XmlWriteMode.IgnoreSchema);
        //        ds.WriteXmlSchema(fileName + "_Schema.XML");

        //        Hash h = new Hash();
        //        h.HashFile(fileName + ".XML");

        //        ClientScript.RegisterClientScriptBlock(GetType(), "Javascript",
        //                                                "<script>alert('Complete Export Successfully Done!')</script>");
        //    }
        //}

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    //btnCompleteExport_Click(sender, e);
        //    //BDI2Core.Common.ScheduledJob.ProcessJob(Convert.ToInt32(TextBox1.Text) , "c:\\temp\\");  
        //}
    }
}