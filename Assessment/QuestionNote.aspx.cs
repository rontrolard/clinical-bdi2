using System;
using BDI2Core.Common;
using BDI2Web.Common_Code;
using System.Web.UI.WebControls;
using BDI2Core.Data;
using System.Data.SqlClient;
using System.Data;

namespace BDI2Web.Assessment
{
    public partial class QuestionNote : SecureBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string questionID = Request.QueryString.Get("qid");
                LabelQuestionID.Text = questionID;

                string noteType = Request.QueryString.Get("type");

                if (noteType == "q")
                {
                    TextBoxNote.Height = new Unit(174, UnitType.Pixel);
                    Page.Title = "Assessment Note";
                    if (questionID != null)
                    {
                        StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                        if (sa != null)
                        {
                            StudentDomain sd = sa.GetCurrentDomain();
                            if (sd != null)
                            {
                                StudentQuestion sq = sd.GetQuestion(int.Parse(questionID));
                                if (sq != null)
                                {
                                    TextBoxNote.Text = sq.Notes;
                                    LabelTitle.Text = "Note for Item #" + sq.ItemNumber + ":";
                                }
                            }
                        }
                    }
                }
                else //assessment note
                {
                    PanelProgramNotes.Visible = true;
                    PopulateTagDropList();
                    StudentAssessment sa;
                    if (questionID != null)
                        sa = new StudentAssessment(int.Parse(questionID));
                    else
                        sa = AssessmentCommon.GetSessionStudentAssessment();

                    sa.LoadFromDB();
                    TextBoxNote.TextMode = System.Web.UI.WebControls.TextBoxMode.SingleLine;
                    TextBoxNote.MaxLength = 25;
                    TextBoxNote.Text = sa.ProgramNote;
                    TextBoxNote.Height = new Unit(50, UnitType.Pixel);

                    DropDownProgramNote2.SelectedValue = sa.ProgramNote2;
                    DropDownProgramNote3.SelectedValue = sa.ProgramNote3;
                    DropDownProgramNote4.SelectedValue = sa.ProgramNote4;
                    DropDownProgramNote5.SelectedValue = sa.ProgramNote5;


                    Page.Title = "Program Note";
                    LabelTitle.Text = "Program Note: (Maximum 25 characters)";
                }
            }
        }

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 30; //Value from Page Table in db
        }

        #endregion Overridden Functions

        

        private void PopulateTagDropList()
        {
            DataSet ds = Lookups.GetProgramNote();
            FillListControl(DropDownProgramNote2, ds, "description", "programnoteid", true);
            FillListControl(DropDownProgramNote3, ds, "description", "programnoteid", true);
            FillListControl(DropDownProgramNote4, ds, "description", "programnoteid", true);
            FillListControl(DropDownProgramNote5, ds, "description", "programnoteid", true);
            
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            string questionID = LabelQuestionID.Text;

            string noteType = Request.QueryString.Get("type");
            if (noteType == "q")
            {
                if (questionID != "q")
                {
                    StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                    if (sa != null)
                    {
                        StudentDomain sd = sa.GetCurrentDomain();
                        if (sd != null)
                        {
                            StudentQuestion sq = sd.GetQuestion(int.Parse(questionID));
                            if (sq != null)
                            {
                                sq.Notes = TextBoxNote.Text;
                            }
                        }
                    }
                }
            }
            else //Assessment note
            {
                StudentAssessment sa ;
                if (questionID == null || questionID == "")
                    sa = AssessmentCommon.GetSessionStudentAssessment();
                else
                    sa = new StudentAssessment(int.Parse(questionID));

                int note2 = DropDownProgramNote2.SelectedValue == "" ? Int32.MinValue : int.Parse(DropDownProgramNote2.SelectedValue);
                int note3 = DropDownProgramNote3.SelectedValue == "" ? Int32.MinValue : int.Parse(DropDownProgramNote3.SelectedValue);
                int note4 = DropDownProgramNote4.SelectedValue == "" ? Int32.MinValue : int.Parse(DropDownProgramNote4.SelectedValue);
                int note5 = DropDownProgramNote5.SelectedValue == "" ? Int32.MinValue : int.Parse(DropDownProgramNote5.SelectedValue);

                sa.SaveNote(TextBoxNote.Text,note2,note3, note4,note5);
            }

            LiteralScript.Text = "<script>\n" + "CloseWindow();\n" + "</script>";
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {

        }

    }
}
