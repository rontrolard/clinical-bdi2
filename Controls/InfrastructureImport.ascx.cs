using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BDI2Core.Configuration;
using BDI2Core.Data;

public partial class Controls_InfrastructureImport : System.Web.UI.UserControl
{

#region Variable Declarations

    private String _HeaderCssClass = String.Empty;
    private String _currentFile;
    //private String _connectionStringName = "Connection String";
    private int _batchId;
    //private int _hierColCount = 4;
    private int _orgColCount = 24;
    private int _staffColCount = 36;
    private int _studentColCount = 79;
    private DataSet _dsError;
    private bool _orgFile;
    private bool _staffFile;
    private bool _studentFile;

#endregion

#region Properties

    /// <summary>
    /// Set/Get the CssClass Name for the Headers.
    /// </summary>
    public String HeaderCssClass
    {
        get { return _HeaderCssClass; }
        set { _HeaderCssClass = value; }
    }

    /// <summary>
    /// Set/Get the CssClass Name for the Headers.
    /// </summary>
    public String CurrentFile
    {
        get { return _currentFile; }
        set { _currentFile = value; }
    }

    /// <summary>
    /// Set/Get the BatchNumber.
    /// </summary>
    public int BatchId
    {
        get { return _batchId; }
        set { _batchId = value; }
    }

    /// <summary>
    /// Set/Get the CustomerNumber.
    /// </summary>
    public Int32 CustomerId
    {
        get { return Convert.ToInt32(Session["CustomerID"]); }
    }

    /// <summary>
    /// Set/Get the CustomerNumber.
    /// </summary>
    public DataSet dsError
    {
        get { return _dsError; }
        set { _dsError = value; }
    }

    /// <summary>
    /// Set/Get the userId.
    /// </summary>
    public String UserId
    {
        get { return Session["StaffName"].ToString(); }
    }

#endregion 

#region UI Events

    protected void Page_Load(object sender, EventArgs e)
    {

        //fuStaff.Enabled = false;
        //fuStudent.Enabled = false;
    }

    protected void butCancel_Click(object sender, EventArgs e)
    {
        
    }

    protected void butImport_Click(object sender, EventArgs e)
    {
        tbMessage.CssClass = "defaulTextBoxStyle";
        tbMessage.Visible = false;
        ImportInfrastructure();
    }

#endregion

#region Private Methods

    /// <summary>
    /// Controls the import process .
    /// </summary>
    /// <param name="ddlProductQty"></param>
    private void ImportInfrastructure()
    {
        try
        {   
            //  Make sure there is data to process
            CurrentFile = "None";
            if (!(fuHier.HasFile || fuOrg.HasFile || fuStaff.HasFile || fuStudent.HasFile))
            {
                tbMessage.Text = "You need to select at least one file to upload";
                tbMessage.Visible = true;
                tbMessage.CssClass = "errorMessage";
                return;
            }

            //   get a batch ID for this import
            GetBatchId();

            _orgFile = false;
            _staffFile = false;
            _studentFile = false;

            // upload and validate each file
            ValidateFiles();

            // check validation
            if (GetErrorCount() < 1)
            {
                // if there is no Staff File or the confirmation is accepted the Update can take place
                if (_staffFile == false || Confirmation())
                {
                    // todo  create confirmation screen
                    UpdateBatch();
                    if (GetErrorCount() < 1)
                    {
                        CurrentFile = "Success Message";
                        LogMessage(0, 0, "File Upload Batch " + BatchId.ToString()
                                                      + " -- Completed Successfully "
                                                      + "   On " + DateTime.Now.ToLocalTime());
                    }
                    else
                    {
                        CurrentFile = "Abort Message";
                        LogMessage(0, 0, "File Upload Batch " + BatchId.ToString()
                                                      + " -- Aborted due to a database update error "
                                                      + "   Completed On " + DateTime.Now.ToLocalTime());
                        tbMessage.Text = "Errors Found -- See list below";
                        tbMessage.Visible = true;
                        tbMessage.CssClass = "errorMessage";
                    }
                }
                else
                {
                    CurrentFile = "Abort Message";
                    LogMessage(0, 0, "File Upload Batch " + BatchId.ToString()
                                                  + " -- Aborted due to non acceptance of staff allocations "
                                                  + "   Completed On " + DateTime.Now.ToLocalTime());
                    tbMessage.Text = "Errors Found -- See list below";
                    tbMessage.Visible = true;
                    tbMessage.CssClass = "errorMessage";
                }
            }
            else
            {
                CurrentFile = "Abort Message";
                LogMessage(0, 0, "File Upload Batch " + BatchId.ToString()
                                              + " -- Aborted due to data errors on the input files "
                                              + "   Completed On " + DateTime.Now.ToLocalTime());
                tbMessage.Text = "Errors Found -- See list below";
                tbMessage.Visible = true;
                tbMessage.CssClass = "errorMessage";
            }

            //todo code this routine;
        }
        catch (Exception ex)
        {
            CurrentFile = "Abort Message";
            LogMessage(9, 0, "File could not be uploaded due to -- " + ex.ToString());
            tbMessage.Text = "Errors Found -- See list below";
            tbMessage.Visible = true;
            tbMessage.CssClass = "errorMessage";
            //throw (ex);
        }
        
        //butImport.Enabled = false;
    }

    /// <summary>
    /// call the upload validator passing the procedure name, batch number and CustomerID .
    /// </summary>
    private void CallUploadValidator(string procedure)
    {
        
        SqlParameter[] userParams = new SqlParameter[2];
        userParams[0] = new SqlParameter("@BatchID", BatchId);
        userParams[1] = new SqlParameter("@customerID", CustomerId);
        SQLHelper.ExecuteNonQuery(BDI2Core.Configuration.Settings.ConnectionStringName, procedure, userParams);
    }

    /// <summary>
    /// Displays the confirmation screen for staff access.
    /// </summary>
    private bool Confirmation()
    {
        // todo code this
        return true;
    }

    /// <summary>
    /// Reads the Data File (tab delimited) into a data table.
    /// </summary>
    private DataTable CreateDataTableFromFile(StreamReader sr, int columnCount, string fileExtension)
    {
        char[] delimiter = (fileExtension == ".csv" ? new char[] { ',' } : new char[] { '\t' });
        DataTable dt = new DataTable();
        DataColumn dc;
        DataRow dr;
        // Three interg columns for BatchID, CustomerID and RowID are added to each row
        for (int i = 1; i < 4; i++)
        {
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "c" + i.ToString();
            dc.Unique = false;
            dt.Columns.Add(dc);
        }
        for (int i = 4; i < (columnCount + 4); i++)
        {
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "c" + i.ToString();
            dc.Unique = false;
            dt.Columns.Add(dc);
        }
        string input;
        int RowId = 0;
        while ((input = sr.ReadLine()) != null)
        {
            RowId++;
            string[] s = input.Split(delimiter);
            if (s.Length != columnCount)
            {
                throw new ArgumentException("Input row invalid number of colums, batch " + BatchId.ToString() + " customer " + CustomerId.ToString() +  " row " + RowId.ToString() + "expected " + columnCount.ToString() + " found " + s.Length.ToString() + ".");
            }
            else
            {
                dr = dt.NewRow();
                dr["c1"] = BatchId;
                dr["c2"] = CustomerId;
                dr["c3"] = RowId;
                for (int i = 4; i < (columnCount + 4); i++)
                {
                    dr["c" + i.ToString()] = s[i - 4];
                }
                dt.Rows.Add(dr);
            }
        }
        sr.Close();
        return dt;
    }

    /// <summary>
    /// get a batchId .
    /// </summary>
    private void GetBatchId()
    {
        
        SqlParameter[] userParams = new SqlParameter[2];
        userParams[0] = new SqlParameter("@customerID", CustomerId);
        userParams[1] = new SqlParameter("@userID", UserId);
        BatchId = Convert.ToInt32(SQLHelper.ExecuteScalar(BDI2Core.Configuration.Settings.ConnectionStringName, "exim.eiInsertNewBatch", userParams));
        this.tbBatchID.Text = BatchId.ToString();
    }

    /// <summary>
    /// Check log for errors .
    /// </summary>
    private int GetErrorCount()
    {

        SqlParameter[] userParams = new SqlParameter[2];
        userParams[0] = new SqlParameter("@customerID", CustomerId);
        userParams[1] = new SqlParameter("@BatchID", BatchId);
        return Convert.ToInt32(SQLHelper.ExecuteScalar(BDI2Core.Configuration.Settings.ConnectionStringName, "exim.eiGetErrorCount", userParams));
    }

    /// <summary>
    /// ADD A MESSAGE TO THE LOG FILE .
    /// </summary>
    private void LogMessage(int severity, int row, string Message)
    {

        SqlParameter[] userParams = new SqlParameter[6];
        userParams[0] = new SqlParameter("@BatchID", BatchId);
        userParams[1] = new SqlParameter("@customerID", CustomerId);
        userParams[2] = new SqlParameter("@rowID", row);
        userParams[3] = new SqlParameter("@Severity", severity);
        userParams[4] = new SqlParameter("@TableName", CurrentFile);
        userParams[5] = new SqlParameter("@MessageTxt", (Message.Length > 256 ? Message.Substring(0,255) : Message));
        SQLHelper.ExecuteNonQuery(BDI2Core.Configuration.Settings.ConnectionStringName, "exim.eiInsertUploadLog", userParams);
    }

    /// <summary>
    /// Check log for errors .
    /// </summary>
    private int UpdateBatch()
    {

        SqlParameter[] userParams = new SqlParameter[2];
        userParams[0] = new SqlParameter("@customerID", CustomerId);
        userParams[1] = new SqlParameter("@BatchID", BatchId);
        return Convert.ToInt32(SQLHelper.ExecuteScalar(BDI2Core.Configuration.Settings.ConnectionStringName, "exim.eiImportUpdate", userParams));
    }


    /// <summary>
    /// Upload a data file into the database.
    /// </summary>
    private void UploadDataFile(FileUpload fu, String tableName, int colCount )
    {
        bool fileOK = false;
        String fileExtension = System.IO.Path.GetExtension(fu.FileName).ToLower();
        String[] allowedExtensions = { ".txt", ".csv"};
        for (int i = 0; i < allowedExtensions.Length; i++)
        {
            if (fileExtension == allowedExtensions[i])
            {
                fileOK = true;
            }
        }
        if (fileOK)
        {
            try
            {
                StreamReader sr = new StreamReader(fu.PostedFile.InputStream);
                SqlBulkCopy bulkCopy = new SqlBulkCopy(BDI2Core.Configuration.Settings.GetConnectionString(BDI2Core.Configuration.Settings.ConnectionStringName), SqlBulkCopyOptions.TableLock);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(CreateDataTableFromFile(sr, colCount, fileExtension));
            }
            catch (Exception ex)
            {
                LogMessage(9, 0, "File could not be uploaded due to -- " + ex.ToString());
                LogMessage(9, 0, "File Not Loaded " + System.IO.Path.GetFileName(fu.FileName).ToLower());
            }
        }
        else
        {
            LogMessage(9, 0, "Cannot accept files of this type. " + fileExtension);
            LogMessage(9, 0, "File Not Loaded " + System.IO.Path.GetFileName(fu.FileName).ToLower());
        }
    }

    /// <summary>
    /// uploads the files to the database and calls the validation routines.
    /// </summary>
    private void ValidateFiles()
    {
        
            if (fuOrg.HasFile)
            {
                CurrentFile = "Organization";
                _orgFile = true;
                LogMessage(0, 0, "File Upload Batch " + BatchId.ToString()
                                              + "   File " + fuOrg.PostedFile.FileName
                                              + "   Started On " + DateTime.Now.ToLocalTime());
                UploadDataFile(fuOrg, "exim.UploadOrganization", _orgColCount);
                CallUploadValidator("exim.eiValidateOrganization");
            }
            if (fuStaff.HasFile)
            {
                CurrentFile = "Staff";
                _staffFile = true;
                LogMessage(0, 0, "File Upload Batch " + BatchId.ToString()
                                             + "   File " + fuOrg.PostedFile.FileName
                                             + "   Started On " + DateTime.Now.ToLocalTime());
                UploadDataFile(fuStaff, "exim.UploadStaff", _staffColCount);
                CallUploadValidator("exim.eiValidateStaff");
            }
            if (fuStudent.HasFile)
            {
                CurrentFile = "Student";
                _studentFile = true;
                LogMessage(0, 0, "File Upload Batch " + BatchId.ToString()
                                             + "   File " + fuOrg.PostedFile.FileName
                                             + "   Started On " + DateTime.Now.ToLocalTime());
                UploadDataFile(fuStudent, "exim.UploadStudent", _studentColCount);
                CallUploadValidator("exim.eiValidateStudent");
            }
            // ok now the files are up there
            CurrentFile = "Cross Validation";

            //validate relationships
    }

#endregion

}
