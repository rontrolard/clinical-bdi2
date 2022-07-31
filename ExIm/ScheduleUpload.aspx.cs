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
using System.IO;
using System.Xml;


namespace BDI2Web.ExIm
{
    public partial class ScheduleUpload : SecureBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccess(Permission.Import, false);

            if (!IsPostBack)
            {
                txtEmailId.Text = SessionData.Staff.Email;
            }
        }
        #region Overridden Functions
        protected override void SetPageID()
        {
            PageID = 58; //Value from Page Table in db
        }

        #endregion Overridden Functions

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            //BDI2Core.Common.Hash h = new Hash();
            //h.HashFile("C:\\dev\\Tickets\\ImportTestFiles\\BDI2_CompleteExportWith_UDF.xml");
            

            string uploadFilePath = Settings.GetOutputPath();
            if (IsValidForm())
            { 
                ArrayList errorList = new ArrayList();

                foreach (string fileInputID in Request.Files)
                {                    
                    UploadedFile file = UploadedFile.FromHttpPostedFile(Request.Files[fileInputID]);

                    //file.SaveAs(uploadFilePath + file.GetName(),true);
                    //Zip objZip = new Zip();

                    //int count = objZip.FileCount(uploadFilePath + file.GetName());

                    if (file.ContentLength > 0 && file.GetExtension().ToLower() == ".zip")
                    {
                        string fileNameXml;
                        string fileNameZip;
                        string fileName;
                        int studentRecordCount=0, customerID=0;
                        fileNameZip = "MI_" + DateTime.Now.ToFileTime().ToString();// +".zip";//file.GetName();
                        fileNameXml = fileNameZip + ".xml";
                        fileNameZip = fileNameZip + ".zip";

                        //if (File.Exists(uploadFilePath + file.GetName()))
                        //{
                        //    //change the file name by adding number at the end of the file name

                        //}


                        file.SaveAs(uploadFilePath + fileNameZip);

                        //string[] upldFileName = file.GetNameWithoutExtension().Split('_');
                        fileName = file.GetNameWithoutExtension();

                        file = null;

                        Zip objZip = new Zip();

                        //if (!objZip.IsZipFile(uploadFilePath + fileNameZip))
                        //{
                        //    errorList.Add("Not a valid .zip file");
                        //}
                                              

                        if (objZip.FileCount(uploadFilePath + fileNameZip)>1)
                        {
                            errorList.Add("Only 1 .xml file is allowed.");                                                        
                        }
                        else if (!objZip.ExtractXmlFile(uploadFilePath, uploadFilePath + fileNameZip, fileNameXml))
                        {
                            errorList.Add("Invalid .xml file.");
                        }

                        if (errorList.Count > 0)
                        {
                            SetErrorMessageWithArrayList(errorList);
                            return;
                        }                        
                        //string hashCode = objZip.GetFileComment(uploadFilePath + fileNameZip);

                        BDI2Core.Common.Hash h = new Hash();

                        if (!h.ValidateHashFileName(uploadFilePath + fileNameXml, objZip.GetXMLFileName(uploadFilePath + fileNameZip)))
                        {
                            if (!h.ValidateHashFile(uploadFilePath + fileNameXml, objZip.GetFileComment(uploadFilePath + fileNameZip)))
                            {
                                errorList.Add("File was modified and cannot be imported.");
                            }
                        }                        

                        //validate if the same customer upload the file and get the Student Record Count
                        if (File.Exists(uploadFilePath + fileNameXml))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.XmlResolver = null;
                            doc.Load(uploadFilePath + fileNameXml);
                            XmlNodeList EC = doc.GetElementsByTagName("Extract_Criteria");
                            foreach (XmlNode node in EC)
                            {
                                XmlElement ecElement = (XmlElement)node;
                                customerID = Convert.ToInt32(ecElement.GetElementsByTagName("customerID")[0].InnerText);
                                studentRecordCount = Convert.ToInt32(ecElement.GetElementsByTagName("studentRecordCount")[0].InnerText);
                            }
                            if (customerID == Convert.ToInt32(Session["CustomerID"]))
                            {
                                errorList.Add("This export file can only be imported into a different Data Manager Web account");
                            }
                        }

                        if (errorList.Count > 0)
                        {
                            SetErrorMessageWithArrayList(errorList);                                               
                        }                        
                        else
                        {
                            if (File.Exists(uploadFilePath + fileNameXml))
                            {

                                //fileName = "MI_" + upldFileName[upldFileName.Length - 1]; 

                                ScheduledJob sj = new ScheduledJob();
                                sj.ProgramNoteOperator = ProgramNoteOperator.None;
                                sj.JobType = ScheduledJobType.Import;
                                sj.Description = "Import - File Upload";
                                //sj.ProgamNoteID1 = Convert.ToInt32(ddlProgramNote1.SelectedValue);
                                //sj.ProgramNoteID2 = Convert.ToInt32(ddlProgramNote2.SelectedValue);
                                sj.StaffID = SessionData.Staff.StaffID.Value;
                                sj.Status = BDI2Core.Common.ScheduledStatus.NotStarted;
                                //sj.OrganizaitonID = Convert.ToInt32(LabelOrganizationID.Text);
                                //sj.ToDate = ToDate.SelectedDate.Value;
                                //sj.FromDate = FromDate.SelectedDate.Value;
                                sj.Email = txtEmailId.Text;
                                sj.FileName = fileNameXml;
                                //sj.FileName = uploadFilePath + file.GetNameWithoutExtension() + ".xml";
                                //sj.JobCode = rblFile.SelectedValue;
                                //sj.Delimitor = rblFormat.SelectedValue;

                                sj.Save();

                                // Change the file name to new File name MI_JOBXXX_JOBXXX
                                File.Move(uploadFilePath + fileNameXml, uploadFilePath + "MI_JOB" + sj.ScheduledJobID.ToString() + "_" + fileName + ".xml");

                                sj.FileName = "MI_JOB" + sj.ScheduledJobID.ToString() + "_" + fileName + ".xml";
                                sj.Save();

                                ////Check for real time Process.                               
                                //DataSet xmlDS = new DataSet();
                                //xmlDS.ReadXml(uploadFilePath + sj.FileName);
                                //int studentCount = Convert.ToInt32(xmlDS.Tables["Extract_Criteria"].Rows[0]["studentRecordCount"]);
                                if (studentRecordCount < Settings.UploadStudentSize)
                                {
                                    BDI2Core.Common.ScheduledJob.ProcessJob(sj.ScheduledJobID, "");      
                                }
                                //ScheduledJob.SendJobConfirmationEmail(sj);
                                SessionData.StatusMessage = "Import has been scheduled.";
                                Response.Redirect("~/ExIm/ScheduledStatus.aspx");
                            }
                        }                        
                    }
                    else
                    {
                        errorList.Add("Either file name is empty or not a valid .zip file");
                        if (errorList.Count > 0)
                        {
                            SetErrorMessageWithArrayList(errorList);                         
                        }
                    }
                }               
            }            
        }
        #region Security
        protected void HandleStaffSecurity(int orgHierarchyLevelID, int levelTypeID, ref bool canExport)
        {
            canExport = true;
        }

        private bool IsValidForm()
        {

            bool isValid = true;
            ArrayList errorList = new ArrayList();

            if (!txtScheduleUpload.HasFile)
            {
                errorList.Add("Either file name is empty or not a valid .zip file");            
            }
            if (txtEmailId.Text.Trim() == "")
            {
                errorList.Add("E-mail is required.");
            }
            else
            {
                if (!BaseDataObject.IsValidEmail(txtEmailId.Text))
                {
                    errorList.Add("E-mail is not in correct format");
                }
            }
            

            if (errorList.Count > 0)
            {
                SetErrorMessageWithArrayList(errorList);
                isValid = false;
            }

            return isValid;
        }
        //private string getFileName(string fileName)
        //{ 
            
        //}
        #endregion

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    String str;
        //    str = BDI2Core.Common.ScheduledJob.ProcessJob(66, "c:\\temp\\");
        //    str = "  " + str;
        //}
    }
}
