using System;
using System.Data;
using System.Web.UI.WebControls;
using BDI2Core.Common;
using BDI2Web.Common_Code;

namespace BDI2Web.Assessment
{
    public partial class DomainTotals : SecureBasePage
    {
        public string strSSTotal = "-";
        public string strDQTotal = "-";
        public string strPRTotal = "-";
        public string strAEColor = "White";
        public int assessmentTypeID = int.MinValue;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int SSTotal = 0;

                StudentAssessment sa = AssessmentCommon.GetSessionStudentAssessment();
                if (sa != null)
                {
                    this.assessmentTypeID = sa.AssessmentTypeID;
                    DataSet ds = sa.GetDomainTotalSet();

                    if (sa.IsAllComplete()) //total field
                    {
                        if (assessmentTypeID == (int)AssessmentType.Full)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                SSTotal += ("" + dr["scaledScore"] == "" ? 0 : int.Parse(dr["scaledScore"].ToString()));
                            }

                            if (SSTotal >= 0) strSSTotal = SSTotal.ToString();
                            if (sa.TotalDQScore >= 0) strDQTotal = sa.TotalDQScore.ToString();
                            if (sa.TotalPRScore >= 0) strPRTotal = sa.PRSign + String.Format("{0:0.0}",sa.TotalPRScore);
                        }
                        else //screener
                        {
                            this.strAEColor = "#fde5a3";
                            LabelAE.Text = "Age Equivalent: " + (sa.GetStudentScreenerAE() < 0 ? "" : sa.GetStudentScreenerAE().ToString());
                            LabelAE.Visible = true;

                            foreach (Object obj in sa.ListStudentDomains)
                            {
                                StudentDomain sd = obj as StudentDomain;
                                if (sd.DomainTypeID == (int)DomainType.Domain)
                                {
                                    int dev1 = (sd.StdDev1Score >= 0 ? sd.StdDev1Score : -1);
                                    int dev2 = (sd.StdDev1Score >= 0 ? sd.StdDev2Score : -1);
                                    int dev3 = (sd.StdDev1Score >= 0 ? sd.StdDev3Score : -1);

                                    if (dev1 >= 0 && dev2 >= 0 && dev3 >= 0)
                                    {
                                        strSSTotal = ((sd.Rawscore - dev1) > 0 ? "Pass" : "Refer");
                                        strDQTotal = ((sd.Rawscore - dev2) > 0 ? "Pass" : "Refer");
                                        strPRTotal = ((sd.Rawscore - dev3) > 0 ? "Pass" : "Refer");
                                    }
                                    else
                                    {
                                        strSSTotal = "";
                                        strDQTotal = "";
                                        strPRTotal = "";
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    RepeaterDomain.DataSource = ds;
                    RepeaterDomain.DataBind();
                }
            }
        }

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 25; //Value from Page Table in db
        }

        #endregion Overridden Functions

        protected void RepeaterDomain_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (this.assessmentTypeID == (int)AssessmentType.Screener)
            {
                //change title
                Label title = (Label)e.Item.FindControl("LabelTitle");
                if (title != null)
                {
                    title.Text = "Screener Domain Total";
                }

                //change sub-title
                Label SS = (Label)e.Item.FindControl("LabelSS");
                if (SS != null)
                {
                    SS.Text = "-1.0";
                    Label DQ = (Label)e.Item.FindControl("LabelDQ");
                    if (DQ != null) DQ.Text = "-1.5";
                    Label PR = (Label)e.Item.FindControl("LabelPR");
                    if (PR != null) PR.Text = "-2.0";
                }
            }

            //summary
            Label total1 = (Label)e.Item.FindControl("LabelTotal1");
            if (total1 != null)
            {
                total1.Text = this.strSSTotal;

                Label total2 = (Label)e.Item.FindControl("LabelTotal2");
                if (total2 != null)
                {
                    total2.Text = this.strDQTotal;
                }

                Label total3 = (Label)e.Item.FindControl("LabelTotal3");
                if (total3 != null)
                {
                    total3.Text = this.strPRTotal;
                }
            }
        }

        protected void ButtonClose_Click(object sender, EventArgs e)
        {
            LiteralScript.Text = "<script>\n" + "CloseWindow();\n" + "</script>";
        }
    }
}
