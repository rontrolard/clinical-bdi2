using System;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using BDI2Core.Common;
using BDI2Core.Data;
using BDI2Core.Configuration;

namespace BDI2Web.Assessment
{
    public class AssessmentCommon
    {
        //temp method, move to session data later
        public static void SetSessionStudentAssessment(StudentAssessment sa)
        {
            HttpContext.Current.Session["CurrentStudentAssessment"] = sa;
        }

        public static StudentAssessment GetSessionStudentAssessment()
        {
            StudentAssessment sa = null;

            if (HttpContext.Current.Session["CurrentStudentAssessment"] != null)
            {
                sa = HttpContext.Current.Session["CurrentStudentAssessment"] as StudentAssessment;
            }

            return sa;
        }

        public static void ClearSessionStudentAssessment()
        {
            if (HttpContext.Current.Session["CurrentStudentAssessment"] != null)
            {
                HttpContext.Current.Session["CurrentStudentAssessment"] = null;
            }

            if (HttpContext.Current.Session["PAGE_21_studentAssessmentID"] != null)
            {
                HttpContext.Current.Session["PAGE_21_studentAssessmentID"] = null;
            }
        }
    }
}
