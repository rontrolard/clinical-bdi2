using System;
using System.Data;
using System.Web.UI.WebControls;
using BDI2Core.Configuration;
using BDI2Core.Data;
using BDI2Web.Common_Code;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace BDI2Web.ErrorLog
{
    public partial class ErrorList : SecureBasePage
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void ResetSearchButton_Click(object sender, EventArgs e)
        {
            ErrorMessageTextBox.Text = "";
            BeginDateTextBox.Text = "";
            EndDateTextBox.Text = "";
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                BindErrorListData();
            }
        }

        protected void gvErrorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Displays the Details of a particular error.
            GridView gv = (GridView)sender;
            SqlDatabase sdb = new SqlDatabase(Settings.GetConnectionString("Error Handling Connection String"));
            DataSet ds = sdb.ExecuteDataSet(CommandType.Text, "select * from log where logid = " + gv.SelectedValue.ToString());
            dvErrorItem.DataSource = ds;
            dvErrorItem.DataBind();
        }

        protected void gvErrorList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                gvErrorList.PageIndex = e.NewPageIndex;
                BindErrorListData();
            }
        }

        #endregion Events

        #region Private Methods

        private void BindErrorListData()
        {
            DataSet ds = SQLHelper.ExecuteDataSet(Settings.ErrorLogConnectionStringName, CommandType.Text, GetSearchSQL());
            gvErrorList.DataSource = ds;
            gvErrorList.DataBind();
            dvErrorItem.DataSource = null;
            dvErrorItem.DataBind();

            //No records found.
            NoRecordsFound.Visible = ds.Tables[0].Rows.Count == 0;
        }

        private string GetSearchSQL()
        {
            string sSQL = "select logid, timestamp, message, substring(formattedmessage, 0, 120) as formattedmessage from log ";

            if (BeginDateTextBox.Text.Trim().Length > 0 && EndDateTextBox.Text.Trim().Length > 0 && ErrorMessageTextBox.Text.Trim().Length > 0)
            {
                sSQL = "select logid, timestamp, message, substring(formattedmessage, 0, 120) as formattedmessage  from log where message like '%" + ErrorMessageTextBox.Text.Trim() + "%' and timestamp between '" + BeginDateTextBox.Text + "' and '" + EndDateTextBox.Text + "'";
            }
            else if (ErrorMessageTextBox.Text.Trim().Length > 0)
            {
                sSQL = "select logid, timestamp, message, substring(formattedmessage, 0, 120) as formattedmessage  from log where message like '%" + ErrorMessageTextBox.Text.Trim() + "%'";
            }
            else if (BeginDateTextBox.Text.Trim().Length > 0 && EndDateTextBox.Text.Trim().Length > 0)
            {
                sSQL = "select logid, timestamp, message, substring(formattedmessage, 0, 120) as formattedmessage  from log where timestamp between '" + BeginDateTextBox.Text.Trim() + "' and '" + EndDateTextBox.Text + "'";
            }

            sSQL = sSQL + " order by timestamp desc";

            return sSQL;
        }
        #endregion Private Methods

        #region Overridden Functions

        protected override void SetPageID()
        {
            PageID = 0; //Value from Page Table in db
        }

        #endregion Overridden Functions
    }
}
