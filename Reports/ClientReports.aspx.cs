using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Collections;
using System.Drawing.Printing;
using System.IO;
using ClientDB.DbModel;
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;


namespace ClientDB.Reports
{
    public partial class ClientReports : System.Web.UI.Page
    {

        public clsSession sess = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    RVClientReport.Visible = false;
                    HeadingDiv.Visible = false;
                    divbirthdate.Visible = false;
                    divPlacement.Visible = false;
                    divContact.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [Serializable]
        public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
        {

            // local variable for network credential.
            private string _UserName;
            private string _PassWord;
            private string _DomainName;

            public CustomReportCredentials(string UserName, string PassWord, string DomainName)
            {
                _UserName = UserName;
                _PassWord = PassWord;
                _DomainName = DomainName;
            }

            public System.Security.Principal.WindowsIdentity ImpersonationUser
            {
                get
                {
                    return null;  // not use ImpersonationUser
                }
            }
            public ICredentials NetworkCredentials
            {
                get
                {
                    // use NetworkCredentials
                    return new NetworkCredential(_UserName, _PassWord, _DomainName);
                }
            }
            public bool GetFormsCredentials(out Cookie authCookie, out string user,
                out string password, out string authority)
            {

                // not use FormsCredentials unless you have implements a custom autentication.
                authCookie = null;
                user = password = authority = null;
                return false;
            }
        }




        protected void btnquarter_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnBirthdate";
                RVClientReport.Visible = false;
                if (ddlQuarter.SelectedItem.Value != "0")
                {
                    tdMsg.InnerHtml = "";
                    RVClientReport.Visible = true;
                    int Schoolid = 0;
                    string schooltype = ConfigurationManager.AppSettings["Server"];
                    if (schooltype == "NE")
                        Schoolid = 1;
                    else
                        Schoolid = 2;
                    RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ClientReportDOB"];
                    RVClientReport.ShowParameterPrompts = false;
                    ReportParameter[] parm = new ReportParameter[2];
                    parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                    parm[1] = new ReportParameter("Quarter", ddlQuarter.SelectedItem.Value);
                    this.RVClientReport.ServerReport.SetParameters(parm);
                    RVClientReport.ServerReport.Refresh();
                }
                else
                {
                    tdMsg.InnerHtml = "<div class='warning_box'>Please select birthdate quarter</div>";
                    ddlQuarter.Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnOldReport_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                FillStudNameIDs();
                FillStudLocationIDs();
                FillStudRaceIDs();
                FillStudStatusIDs();
                hfstatus.Value = "A";
                DropDownCheckBoxesActive.SelectedValue = hfstatus.Value;
                divStatisticalNew.Visible = true;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                hdnMenu.Value = "btnallClient";
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients Info";
                divbirthdate.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                for (int i = 0; i < ChkStatisticalList2.Items.Count; i++)
                {
                    ChkStatisticalList2.Items[i].Selected = true;
                }
                var selected = ChkStatisticalList2.Items.Cast<ListItem>().Where(li => li.Selected).Count();
                if (selected != 0)
                {
                    List<ListItem> selectedItemList = ChkStatisticalList2.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
                    RVClientReport.Visible = true;
                    RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["StatisticalReportNew"];
                    RVClientReport.ShowParameterPrompts = false;
                    ReportParameter[] parm = new ReportParameter[13];
                    parm[0] = new ReportParameter("ParamStudRow", ContainsLoop("Total number of client", selectedItemList));
                    parm[1] = new ReportParameter("ParamStudName", ContainsLoop("Student Name", selectedItemList));
                    parm[2] = new ReportParameter("ParamLocation", ContainsLoop("Location", selectedItemList));
                    parm[3] = new ReportParameter("ParamCity", ContainsLoop("City", selectedItemList));
                    parm[4] = new ReportParameter("ParamState", ContainsLoop("State", selectedItemList));
                    parm[5] = new ReportParameter("ParamLanguage", ContainsLoop("Primary Language", selectedItemList));
                    parm[6] = new ReportParameter("ParamRace", ContainsLoop("Race", selectedItemList));
                    parm[7] = new ReportParameter("ParamPlacement", ContainsLoop("Placement Type", selectedItemList));
                    parm[8] = new ReportParameter("ParamDepartment", ContainsLoop("Department", selectedItemList));
                    parm[9] = new ReportParameter("ParamProgram", ContainsLoop("Program", selectedItemList));
                    parm[10] = new ReportParameter("ParamGender", ContainsLoop("Gender", selectedItemList));
                    parm[11] = new ReportParameter("ParamActive", ContainsLoop("Active", selectedItemList));
                    parm[12] = new ReportParameter("GetActiveID", hfstatus.Value);
                    this.RVClientReport.ServerReport.SetParameters(parm);
                    RVClientReport.ServerReport.Refresh();
                }
                else
                {
                    tdMsg.InnerHtml = "<div class='warning_box'>Please select report items</div>";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //// --== All Client Click ==- START --
            //try
            //{
            //    divchanges.Visible = false;
            //    divStatistical.Visible = false;
            //    divDischarge.Visible = false;
            //    divAdmission.Visible = false;
            //    divbyBirthdate.Visible = false;
            //    divFunder.Visible = false;
            //    divPlacement.Visible = false;
            //    hdnMenu.Value = "btnallClient";
            //    int Schoolid = 0;
            //    string schooltype = ConfigurationManager.AppSettings["Server"];
            //    if (schooltype == "NE")
            //        Schoolid = 1;
            //    else
            //        Schoolid = 2;
            //    RVClientReport.SizeToReportContent = false;
            //    tdMsg.InnerHtml = "";
            //    HeadingDiv.Visible = true;
            //    HeadingDiv.InnerHtml = "All Clients Info";
            //    RVClientReport.Visible = true;
            //    RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            //    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ClientReport"];
            //    RVClientReport.ShowParameterPrompts = false;
            //    ReportParameter[] parm = new ReportParameter[1];
            //    parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
            //    this.RVClientReport.ServerReport.SetParameters(parm);
            //    RVClientReport.ServerReport.Refresh();
            //    divbirthdate.Visible = false;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //// --== All Client Click ==- END --
        }
        protected void btnallClient_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkHighcharts.Checked)
                {
                    btnOldReport_Click(sender, e);
                }
                else
                {
                    divContact.Visible = false;
                    divnodata.Visible = false;
                    FillStudNameIDs();
                    FillStudLocationIDs();
                    FillStudRaceIDs();
                    FillStudStatusIDs();
                    hfstatus.Value = "A";
                    DropDownCheckBoxesActive.SelectedValue = hfstatus.Value;
                    divStatisticalNew.Visible = false;
                    divchanges.Visible = false;
                    divStatistical.Visible = false;
                    divDischarge.Visible = false;
                    divAdmission.Visible = false;
                    divbyBirthdate.Visible = false;
                    divFunder.Visible = false;
                    divPlacement.Visible = false;
                    hdnMenu.Value = "btnallClient";
                    tdMsg.InnerHtml = "";
                    RVClientReport.Visible = false;
                    HeadingDiv.Visible = true;
                    HeadingDiv.InnerHtml = "All Clients Info";
                    divbirthdate.Visible = false;
                    btnResetAllClient.Visible = true;
                    btnShowReport.Visible = true;
                    btnReset.Visible = false;
                    for (int i = 0; i < ChkStatisticalList2.Items.Count; i++)
                    {
                        ChkStatisticalList2.Items[i].Selected = true;
                    }
                    var selected = ChkStatisticalList2.Items.Cast<ListItem>().Where(li => li.Selected).Count();
                    if (selected != 0)
                    {
                        List<ListItem> selectedItemList = ChkStatisticalList2.Items.Cast<ListItem>().Where(li => li.Selected).ToList();


                        //RVClientReport.Visible = true;
                        //RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                        //RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["StatisticalReportNew"];
                        //RVClientReport.ShowParameterPrompts = false;
                        //ReportParameter[] parm = new ReportParameter[13];
                        //parm[0] = new ReportParameter("ParamStudRow", ContainsLoop("Total number of client", selectedItemList));
                        //parm[1] = new ReportParameter("ParamStudName", ContainsLoop("Student Name", selectedItemList));
                        //parm[2] = new ReportParameter("ParamLocation", ContainsLoop("Location", selectedItemList));
                        //parm[3] = new ReportParameter("ParamCity", ContainsLoop("City", selectedItemList));
                        //parm[4] = new ReportParameter("ParamState", ContainsLoop("State", selectedItemList));
                        //parm[5] = new ReportParameter("ParamLanguage", ContainsLoop("Primary Language", selectedItemList));
                        //parm[6] = new ReportParameter("ParamRace", ContainsLoop("Race", selectedItemList));
                        //parm[7] = new ReportParameter("ParamPlacement", ContainsLoop("Placement Type", selectedItemList));
                        //parm[8] = new ReportParameter("ParamDepartment", ContainsLoop("Department", selectedItemList));
                        //parm[9] = new ReportParameter("ParamProgram", ContainsLoop("Program", selectedItemList));
                        //parm[10] = new ReportParameter("ParamGender", ContainsLoop("Gender", selectedItemList));
                        //parm[11] = new ReportParameter("ParamActive", ContainsLoop("Active", selectedItemList));
                        //parm[12] = new ReportParameter("GetActiveID", hfstatus.Value);
                        //this.RVClientReport.ServerReport.SetParameters(parm);
                        //RVClientReport.ServerReport.Refresh();

                        SqlDataAdapter da = new SqlDataAdapter();
                        SqlCommand cmd = null;
                        DataTable dt = new DataTable();
                        DataTable dtFinal = new DataTable();
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                        con.Open();
                        cmd = new SqlCommand("ClientStatisticalGraph", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ParamStudName", ContainsLoop("Student Name", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamGender", ContainsLoop("Gender", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamLanguage", ContainsLoop("Primary Language", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamRace", ContainsLoop("Race", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamLocation", ContainsLoop("Location", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamProgram", ContainsLoop("Program", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamPlacement", ContainsLoop("Placement Type", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamDepartment", ContainsLoop("Department", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamActive", "true");
                        cmd.Parameters.AddWithValue("@ParamCity", ContainsLoop("City", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamState", ContainsLoop("State", selectedItemList));
                        cmd.Parameters.AddWithValue("@ParamStudRow", ContainsLoop("Total number of client", selectedItemList));
                        cmd.Parameters.AddWithValue("@GetActiveID", "A");

                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        dtFinal = GetSelectedColumns(dt);
                        DataTable dtActive = new DataTable();
                        dtActive.Columns.Add("Status");
                        dtActive.Rows.Add("Active");
                        dtFinal = filterDataTable(dtFinal, dtActive);
                        PopulateDropdown(dtFinal);
                        var jsonData = ConvertDataTableToJson(dtFinal);
                        //noOfClients.Text = "Total No. of Clients : " + dtFinal.Rows.Count.ToString();
                        ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServer(" + jsonData + ");", true);
                    }
                    else
                    {
                        tdMsg.InnerHtml = "<div class='warning_box'>Please select report items</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //// --== All Client Click ==- START --
            //try
            //{
            //    divchanges.Visible = false;
            //    divStatistical.Visible = false;
            //    divDischarge.Visible = false;
            //    divAdmission.Visible = false;
            //    divbyBirthdate.Visible = false;
            //    divFunder.Visible = false;
            //    divPlacement.Visible = false;
            //    hdnMenu.Value = "btnallClient";
            //    int Schoolid = 0;
            //    string schooltype = ConfigurationManager.AppSettings["Server"];
            //    if (schooltype == "NE")
            //        Schoolid = 1;
            //    else
            //        Schoolid = 2;
            //    RVClientReport.SizeToReportContent = false;
            //    tdMsg.InnerHtml = "";
            //    HeadingDiv.Visible = true;
            //    HeadingDiv.InnerHtml = "All Clients Info";
            //    RVClientReport.Visible = true;
            //    RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            //    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ClientReport"];
            //    RVClientReport.ShowParameterPrompts = false;
            //    ReportParameter[] parm = new ReportParameter[1];
            //    parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
            //    this.RVClientReport.ServerReport.SetParameters(parm);
            //    RVClientReport.ServerReport.Refresh();
            //    divbirthdate.Visible = false;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //// --== All Client Click ==- END --
        }
        private DataTable cleanDataTable(DataTable dt)
        {
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == "Location")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string value = row[column].ToString();

                        string[] parts = value.Split(new char[] { ':' }, 2);
                        value = parts.Length > 1 ? parts[1].Trim() : value;
                        row[column] = value;
                    }
                }
            }
            return dt;

        }
        private DataTable getAllClientReport(DataTable dataTbl)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = null;
                DataTable dt = new DataTable();
                DataTable dtFinal = new DataTable();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                con.Open();
                cmd = new SqlCommand("ClientStatisticalGraph", con);
                cmd.CommandType = CommandType.StoredProcedure;
                string para = "true";
                cmd.Parameters.AddWithValue("@ParamStudName", para);
                cmd.Parameters.AddWithValue("@ParamGender", para);
                cmd.Parameters.AddWithValue("@ParamLanguage", para);
                cmd.Parameters.AddWithValue("@ParamRace", para);
                cmd.Parameters.AddWithValue("@ParamLocation", para);
                cmd.Parameters.AddWithValue("@ParamProgram", para);
                cmd.Parameters.AddWithValue("@ParamPlacement", para);
                cmd.Parameters.AddWithValue("@ParamDepartment", para);
                cmd.Parameters.AddWithValue("@ParamActive", para);
                cmd.Parameters.AddWithValue("@ParamCity", para);
                cmd.Parameters.AddWithValue("@ParamState", para);
                cmd.Parameters.AddWithValue("@ParamStudRow", para);
                cmd.Parameters.AddWithValue("@GetActiveID", "A,I,D");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dtFinal = GetSelectedColumns(dt);
                if (dataTbl.Rows.Count == 0) // No filter
                {
                    DataTable dtActive = new DataTable();
                    dtActive.Columns.Add("Status");
                    dtActive.Rows.Add("Active");
                    dtFinal = filterDataTable(dtFinal, dtActive);
                    return dtFinal;
                }
                else
                    return filterDataTable(dtFinal, dataTbl); //Filter present
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable filterDataTable(DataTable fullData, DataTable selectedData)
        {
            //Filter the table based on selected data
            DataTable filteredData = fullData.Clone();

            foreach (DataRow fullRow in fullData.Rows)
            {
                bool isMatch = true;

                foreach (DataColumn selectedColumn in selectedData.Columns)
                {
                    if (fullData.Columns.Contains(selectedColumn.ColumnName))
                    {
                        List<string> selectedValues = selectedData.AsEnumerable()
                            .Select(row => row[selectedColumn.ColumnName] != DBNull.Value
                                ? row[selectedColumn.ColumnName].ToString().Trim()
                                : string.Empty)
                            .Where(val => !string.IsNullOrEmpty(val))
                            .ToList();

                        string fullRowValue = fullRow[selectedColumn.ColumnName] != DBNull.Value
                            ? fullRow[selectedColumn.ColumnName].ToString().Trim()
                            : string.Empty;

                        if (selectedColumn.ColumnName == "Location") //Extract individual classes (Day and Residential)
                        {

                            DataTable newDataTable = new DataTable();
                            newDataTable.Columns.Add(selectedColumn.ColumnName, typeof(string));
                            DataRow newRow = newDataTable.NewRow();
                            newRow[selectedColumn.ColumnName] = fullRowValue;
                            newDataTable.Rows.Add(newRow);
                            List<string> roomNamesInRow = ExtractLocation(newDataTable, selectedColumn.ColumnName);

                            if (!selectedValues.Any(selectedRoom => roomNamesInRow.Contains(selectedRoom)))
                            {
                                isMatch = false;
                                break;
                            }
                        }
                        else
                        {
                            if (selectedValues.Count > 0 && !selectedValues.Contains(fullRowValue))
                            {
                                isMatch = false;
                                break;
                            }
                        }
                    }
                }

                if (isMatch)
                {
                    filteredData.ImportRow(fullRow);
                }
            }

            
            if (selectedData.Columns.Contains("Status"))
                return filteredData; //Already filtered based on status
            else
            {
                // Making Active, the default status
                DataTable dt = new DataTable();
                dt.Columns.Add("Status");
                dt.Rows.Add("Active");
                return filterDataTable(filteredData, dt);
            }
        }


        [WebMethod]
        public static string CreateDataTableFromSelectedValues(Dictionary<string, List<string>> selectedValues)
        {
            //Create datatable of filters
            try
            {
                DataTable dt = new DataTable();

                foreach (KeyValuePair<string, List<string>> entry in selectedValues)
                {
                    dt.Columns.Add(entry.Key);
                }

                int maxSelections = 0;
                foreach (KeyValuePair<string, List<string>> entry in selectedValues)
                {
                    if (entry.Value.Count > maxSelections)
                    {
                        maxSelections = entry.Value.Count;
                    }
                }

                for (int i = 0; i < maxSelections; i++)
                {
                    DataRow row = dt.NewRow();

                    foreach (KeyValuePair<string, List<string>> entry in selectedValues)
                    {
                        List<string> selectedTexts = entry.Value;

                        if (i < selectedTexts.Count)
                        {
                            row[entry.Key] = selectedTexts[i];
                        }
                        else
                        {
                            row[entry.Key] = DBNull.Value;
                        }
                    }

                    dt.Rows.Add(row);
                }
                ClientReports clientReportsInstance = new ClientReports();
                DataTable dtFinal = clientReportsInstance.getAllClientReport(dt);
                dtFinal.DefaultView.Sort = dtFinal.Columns[0].ColumnName + " ASC";
                dtFinal = dtFinal.DefaultView.ToTable();
                string jsonData = clientReportsInstance.ConvertDataTableToJson(dtFinal);
                Console.WriteLine(jsonData);
                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void registerScript(string script)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadDataScript", script, true);
        }
        private void PopulateDropdown(DataTable dtFinal)
        {
            //Populate dropdown menu for filtration
            DataTable dt = dtFinal.Copy();
            StringBuilder htmlBuilder = new StringBuilder();
            Literal dropdown = new Literal();

            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == "Student Name" || column.ColumnName == "Status" || column.ColumnName == "Location" || column.ColumnName == "Race")
                {
                    htmlBuilder.Append("<div class='dropdown'>");
                    htmlBuilder.Append("<button class='dropdown-btn'>" + column.ColumnName + " &#9660</button>");
                    htmlBuilder.Append("<div class='dropdown-content'>");

                    HashSet<string> uniqueValues = new HashSet<string>();
                    List<string> sortedValues = new List<string>();
                    if (column.ColumnName == "Location")
                    {
                        sortedValues = ExtractLocation(dt, column.ColumnName);
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string value = row[column].ToString();
                            if (!uniqueValues.Contains(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                uniqueValues.Add(value);
                            }
                        }
                        sortedValues = uniqueValues.ToList();
                    }
                    sortedValues.Sort(); 

                    foreach (string value in sortedValues)
                    {
                        htmlBuilder.Append("<label><input type='checkbox' value='" + value + "' class='filter-checkbox' data-column='" + column.ColumnName + "'> " + value + "</label><br>");
                    }

                    
                    if (column.ColumnName == "Status")
                    {
                        //htmlBuilder.Append("<label><input type='checkbox' value='" + "Inactive" + "' class='filter-checkbox' data-column='" + column.ColumnName + "'> " + "Inactive" + "</label><br>");
                        htmlBuilder.Append("<label><input type='checkbox' value='" + "Discharged" + "' class='filter-checkbox' data-column='" + column.ColumnName + "'> " + "Discharged" + "</label><br>");
                    }

                    htmlBuilder.Append("</div>");
                    htmlBuilder.Append("</div>");
                }
                dropdown.Text = htmlBuilder.ToString();
                dropdown_container.Controls.Add(dropdown);
            }
        }

        

        public DataTable GetSelectedColumns(DataTable originalTable)
        {
            //To return only required columns for the table.
            DataTable newTable = new DataTable();

            string[] selectedColumns = { "StudName", "Gender", "StudLanguage", "RaceName", "City", "StudState", "ClassName", "Program", "Placement_Type", "DepartmentName", "StudStatus"};

            foreach (var columnName in selectedColumns)
            {
                if (originalTable.Columns.Contains(columnName))
                {
                    newTable.Columns.Add(columnName, originalTable.Columns[columnName].DataType);
                }
            }

            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();

                foreach (var columnName in selectedColumns)
                {
                    newRow[columnName] = row[columnName];
                }

                newTable.Rows.Add(newRow);
            }
            
            if (newTable.Columns.Contains("StudName"))
            {
                newTable.Columns["StudName"].ColumnName = "Student Name";
            }
            
            if (newTable.Columns.Contains("StudLanguage"))
            {
                newTable.Columns["StudLanguage"].ColumnName = "Primary Language";
            } 
            
            if (newTable.Columns.Contains("RaceName"))
            {
                newTable.Columns["RaceName"].ColumnName = "Race";
            }
            
            if (newTable.Columns.Contains("StudState"))
            {
                newTable.Columns["StudState"].ColumnName = "State";
            }
            
            if (newTable.Columns.Contains("ClassName"))
            {
                newTable.Columns["ClassName"].ColumnName = "Location";
            }

            if (newTable.Columns.Contains("Placement_Type"))
            {
                newTable.Columns["Placement_Type"].ColumnName = "Placement Type";
            }

            if (newTable.Columns.Contains("DepartmentName"))
            {
                newTable.Columns["DepartmentName"].ColumnName = "Department";
            }

            if (newTable.Columns.Contains("StudStatus"))
            {
                newTable.Columns["StudStatus"].ColumnName = "Status";
            }
            foreach (DataRow dr in newTable.Rows)
            {
                if (dr["Status"] != DBNull.Value)
                {
                    string status = dr["Status"].ToString();

                    if (status == "A")
                        dr["Status"] = "Active";
                    //else if (status == "I")
                    //    dr["Status"] = "Inactive";
                    else if (status == "D")
                        dr["Status"] = "Discharged";
                }
            }

            return newTable;
        }
        public string ConvertDataTableToJson(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var rowsList = new System.Collections.ArrayList();

            foreach (DataRow row in dt.Rows)
            {
                var rowDictionary = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn column in dt.Columns)
                {
                    rowDictionary[column.ColumnName] = row[column];
                }
                rowsList.Add(rowDictionary);
            }

            return jsSerializer.Serialize(rowsList);
        }
        static List<string> ExtractLocation(DataTable dt, string columnName)
        {
            HashSet<string> rooms = new HashSet<string>(); // HashSet to store unique room names
            Regex regex = new Regex(@":\s*([^:,]+(?:,\s*[^:,]+)*)"); // Match everything after ': '

            foreach (DataRow row in dt.Rows)
            {
                if (row[columnName] != DBNull.Value) // Check for null values
                {
                    string rowData = row[columnName].ToString();
                    if (rowData.Length > 0) // Ensure the string is not empty
                    {
                        MatchCollection matches = regex.Matches(rowData);
                        foreach (Match match in matches)
                        {
                            // Extract and split room names
                            string[] roomNames = match.Groups[1].Value.Split(',');
                            foreach (string room in roomNames)
                            {
                                rooms.Add(room.Trim()); // Trim spaces and add to HashSet (eliminates duplicates)
                            }
                        }
                    }
                }
            }
            return new List<string>(rooms); // Convert HashSet to List and return
        }
        protected void btnOldClienContact_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnClienContact";
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "Emergency/Home Contact";
                RVClientReport.Visible = true;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ClientReportEmer"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[1];
                parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClienContact_Click(object sender, EventArgs e)
        {   
            try
            {
                if (!checkHighcharts.Checked)
                {
                    btnOldClienContact_Click(sender, e);
                }
                else
                {
                    
                    divContact.Visible = false;
                    divnodata.Visible = false;
                    divStatisticalNew.Visible = false;
                    divchanges.Visible = false;
                    divStatistical.Visible = false;
                    divDischarge.Visible = false;
                    divAdmission.Visible = false;
                    divbyBirthdate.Visible = false;
                    divFunder.Visible = false;
                    divPlacement.Visible = false;
                    btnShowReport.Visible = false;
                    btnResetAllClient.Visible = false;
                    hdnMenu.Value = "btnClienContact";
                    RVClientReport.Visible = false;
                    HeadingDiv.Visible = true;
                    HeadingDiv.InnerHtml = "Emergency/Home Contact";
                    btnShowReport.Visible = false;
                    btnResetAllClient.Visible = false;


                    string query = "SELECT SP.StudentPersonalId, PLC.EndDate, SP.SchoolId, SP.LastName+','+SP.FirstName AS studentPersonalName " +
        " ,CONVERT(VARCHAR(10), SP.[BirthDate], 101) AS BirthDate	" +
        " ,DATEDIFF(YEAR,SP.BirthDate,GETDATE()) - (CASE WHEN DATEADD(YY,DATEDIFF(YEAR,SP.BirthDate,GETDATE()),SP.BirthDate) >  GETDATE() THEN 1 ELSE 0 END) AS Age" +
        " ,CASE WHEN DATEPART(MM,SP.BirthDate)>= 01 AND DATEPART(MM,SP.BirthDate)<= 03 THEN 1 ELSE " +
        " CASE WHEN DATEPART(MM,SP.BirthDate)>= 04 AND DATEPART(MM,SP.BirthDate)<= 06 THEN 2 ELSE " +
        " CASE WHEN DATEPART(MM,SP.BirthDate)>= 07 AND DATEPART(MM,SP.BirthDate)<= 09 THEN 3 ELSE 4 END END END AS mMonth " +
        " ,CASE WHEN SP.Gender=1 THEN 'Male'	ELSE 'Female'	END Gender " +
        " ,EMERGENCYCONT.LastName+','+EMERGENCYCONT.FirstName AS EmergencyContactName " +
        " ,EMERGENCYCONT.Phone AS EmergencyContactPhone " +
        " ,EMERGENCYCONT.Mobile AS EmergencyContactMobile " +
        " FROM StudentPersonal SP " +
        " INNER JOIN Placement PLC ON PLC.StudentPersonalId = SP.StudentPersonalId " +
        " LEFT JOIN " +
        " (SELECT CP.ContactPersonalId,CP.LastName,CP.FirstName,CP.StudentPersonalId,AL.Phone,AL.Mobile,AL.OtherPhone  FROM " +
        "   [dbo].[ContactPersonal] CP " +
        " INNER JOIN [dbo].[StudentContactRelationship] SCR ON CP.ContactPersonalId=SCR.ContactPersonalId " +
        " INNER JOIN LookUp LP ON LP.LookupId=SCR.RelationshipId " +
        " INNER JOIN [dbo].[StudentAddresRel] SAR ON SAR.ContactPersonalId=CP.ContactPersonalId " +
        " INNER JOIN [dbo].[AddressList] AL ON AL.AddressId=SAR.AddressId " +
        " WHERE LP.LookupName='Emergency Contact' AND SAR.ContactSequence=1 AND CP.Status=1) EMERGENCYCONT ON SP.StudentPersonalId=EMERGENCYCONT.StudentPersonalId " +
        "  WHERE SP.StudentType='Client' and (PLC.EndDate is null or PLC.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
        " 							FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " +
        " 							WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " +
        " 							ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " +
        " 							WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client')" +
        "   and ST.StudentPersonalId not in (SELECT Distinct " +
        "   ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) AND CONVERT(INT,SP.ClientId)>0 " +
        "  UNION " +
        " (SELECT SP.StudentPersonalId,PLC.EndDate, SP.SchoolId,SP.LastName + ',' + SP.FirstName AS studentPersonalName,CONVERT(VARCHAR(10), SP.BirthDate, 101) AS BirthDate, " +
        " DATEDIFF(YEAR,SP.BirthDate,GETDATE()) - (CASE WHEN DATEADD(YY,DATEDIFF(YEAR,SP.BirthDate,GETDATE()),SP.BirthDate) >  GETDATE() THEN 1 ELSE 0 END) AS Age, " +
        " CASE WHEN DATEPART(MM,SP.BirthDate)>= 01 AND DATEPART(MM,SP.BirthDate)<= 03 THEN 1 ELSE  " +
        " CASE WHEN DATEPART(MM,SP.BirthDate)>= 04 AND DATEPART(MM,SP.BirthDate)<= 06 THEN 2 ELSE " +
        " CASE WHEN DATEPART(MM,SP.BirthDate)>= 07 AND DATEPART(MM,SP.BirthDate)<= 09 THEN 3 ELSE 4 END END END AS mMonth,CASE WHEN SP.Gender = '1' THEN 'Male' ELSE 'Female' END AS Gender, " +
        " CP.LastName + ',' + CP.FirstName AS EmergencyContactName,AL.Phone AS EmergencyContactPhone,AL.Mobile EmergencyContactMobile  " +
        " FROM StudentPersonal SP " +
        " INNER JOIN Placement PLC ON PLC.StudentPersonalId = SP.StudentPersonalId " +
        " INNER JOIN  ContactPersonal AS CP ON SP.StudentPersonalId = CP.StudentPersonalId " +
        " INNER JOIN [dbo].[StudentContactRelationship] SCR ON CP.ContactPersonalId=SCR.ContactPersonalId		" +
        " INNER JOIN [dbo].[StudentAddresRel] SAR ON SAR.ContactPersonalId=CP.ContactPersonalId " +
        " INNER JOIN [dbo].[AddressList] AL ON AL.AddressId=SAR.AddressId  " +
        " WHERE (SP.StudentType = 'Client') and (PLC.EndDate is null or PLC.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 " +
        " and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
        " 							FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " +
        " 							WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " +
        " 							ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " +
        " 							WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " +
        "  and ST.StudentPersonalId not in (SELECT Distinct " +
        "  ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) " +
        "  AND CONVERT(INT,SP.ClientId)>0 AND CP.IsEmergency='true' AND CP.Status=1) ";


                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnsEmergency(dt);
                    dt.DefaultView.Sort = dt.Columns["Client Name"].ColumnName + " ASC";
                    dt = dt.DefaultView.ToTable();

                    
                    string jsonData = ConvertDataTableToJson(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServerEmergency(" + jsonData + ");", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSelectedColumnsEmergency(DataTable originalTable)
        {
            //To return only required columns for the table.
            DataTable newTable = new DataTable();

            string[] selectedColumns = {"studentPersonalName", "BirthDate", "Age", "EmergencyContactName", "EmergencyContactPhone"};

            foreach (var columnName in selectedColumns)
            {
                if (originalTable.Columns.Contains(columnName))
                {
                    newTable.Columns.Add(columnName, originalTable.Columns[columnName].DataType);
                }
            }

            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();

                foreach (var columnName in selectedColumns)
                {
                    newRow[columnName] = row[columnName];
                }

                newTable.Rows.Add(newRow);
            }

            if (newTable.Columns.Contains("studentPersonalName"))
            {
                newTable.Columns["studentPersonalName"].ColumnName = "Client Name";
            }

            if (newTable.Columns.Contains("BirthDate"))
            {
                newTable.Columns["BirthDate"].ColumnName = "Birth Date";
            }

            if (newTable.Columns.Contains("EmergencyContactName"))
            {
                newTable.Columns["EmergencyContactName"].ColumnName = "Contact Name";
            }

            if (newTable.Columns.Contains("EmergencyContactPhone"))
            {
                newTable.Columns["EmergencyContactPhone"].ColumnName = "Contact Phone";
            }

            newTable.DefaultView.Sort = newTable.Columns["Client Name"].ColumnName + " ASC";
            newTable = newTable.DefaultView.ToTable();

            for (int i = 0; i < newTable.Rows.Count; i++)
                    {
                        if (newTable.Rows[i]["Contact Name"].ToString() == "" && newTable.Rows[i]["Contact Phone"].ToString() == "")
                        {
                           string clientName = newTable.Rows[i]["Client Name"].ToString();
                           int count = newTable.Select("[Client Name] = '" + clientName.Replace("'", "''") + "'").Length;
                            if (count > 1)
                            {
                                newTable.Rows.RemoveAt(i);
                                --i;
                            }
                        }
                        else if (newTable.Rows[i]["Contact Phone"].ToString() == "")
                        {
                            string contactName = newTable.Rows[i]["Contact Name"].ToString();
                            int count = newTable.Select("[Contact Name] = '" + contactName.Replace("'", "''") + "'").Length;
                            if (count > 1)
                            {
                                newTable.Rows.RemoveAt(i);
                                --i;
                            }
                        }
                    }

            return newTable;
        }
        protected void btnOldPgmRoster_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnPgmRoster";
                RVClientReport.SizeToReportContent = true;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "Program Roster";
                RVClientReport.Visible = true;
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ClientReportRoster"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[1];
                parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnPgmRoster_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkHighcharts.Checked)
                {
                    btnOldPgmRoster_Click(sender, e);
                }
                else
                {
                    divContact.Visible = false;
                    divnodata.Visible = false;
                    divStatisticalNew.Visible = false;
                    divchanges.Visible = false;
                    divStatistical.Visible = false;
                    divDischarge.Visible = false;
                    divAdmission.Visible = false;
                    divbyBirthdate.Visible = false;
                    divFunder.Visible = false;
                    divPlacement.Visible = false;
                    btnShowReport.Visible = false;
                    btnResetAllClient.Visible = false;
                    hdnMenu.Value = "btnPgmRoster";
                    RVClientReport.SizeToReportContent = false;
                    tdMsg.InnerHtml = "";
                    RVClientReport.Visible = false;
                    HeadingDiv.Visible = true;
                    HeadingDiv.InnerHtml = "Program Roster";
                    int Schoolid = 0;
                    divbirthdate.Visible = false;
                    
                    string query = "SELECT 		SP.StudentPersonalId ,SP.SchoolId,SP.LastName+','+SP.FirstName AS studentPersonalName " +
                                   ",CONVERT(VARCHAR(10), SP.[BirthDate], 101) AS BirthDate	" +
                                   ",DATEDIFF(YEAR,SP.BirthDate,GETDATE()) - (CASE WHEN DATEADD(YY,DATEDIFF(YEAR,SP.BirthDate,GETDATE()),SP.BirthDate) >  GETDATE() THEN 1 ELSE 0 END) AS Age " +
                                   ",CASE WHEN DATEPART(MM,SP.BirthDate)>= 01 AND DATEPART(MM,SP.BirthDate)<= 03 THEN 1 ELSE " +
                                   "CASE WHEN DATEPART(MM,SP.BirthDate)>= 04 AND DATEPART(MM,SP.BirthDate)<= 06 THEN 2 ELSE " +
                                   "CASE WHEN DATEPART(MM,SP.BirthDate)>= 07 AND DATEPART(MM,SP.BirthDate)<= 09 THEN 3 ELSE 4 END END END AS mMonth " +
                                   ",CASE WHEN SP.Gender=1 THEN 'Male'	ELSE 'Female'	END Gender " +
                                   ",LP.LookupName AS PlacementType " +
                                   ",CONVERT(VARCHAR(10),PL.StartDate,101) AS StartDate " +
                                   ",CONVERT(VARCHAR(10),PL.EndDate,101) AS EndDate " +
                                   ",(SELECT LookupName FROM LookUp WHERE LookupId=PL.Department) AS Department " +
                                   ",(SELECT LookupName FROM LookUp WHERE LookupId=PL.BehaviorAnalyst) AS BehaviorAnalyst " +
                                   "FROM StudentPersonal SP LEFT JOIN [dbo].[Placement] PL ON SP.StudentPersonalId=PL.StudentPersonalId  " +
                                   "LEFT JOIN LookUp LP ON LP.LookupId=PL.PlacementType		 " +
                                   " WHERE SP.StudentType='Client' and (PL.EndDate is null or PL.EndDate >= cast (GETDATE() as DATE)) and PL.Status=1 and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
                                   "FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " +
                                   "WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " +
                                   "ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " +
                                   "WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " +
                                   "and ST.StudentPersonalId not in (SELECT Distinct " +
                                   "ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) AND PL.Status=1 AND CONVERT(INT,SP.ClientId)>0";
                    
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnsProgramRoster(dt);

                    string jsonData = ConvertDataTableToJson(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServerProgramRoster(" + jsonData + ");", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSelectedColumnsProgramRoster(DataTable originalTable)
        {
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("Client Name", typeof(string));
            dtTemp.Columns.Add("Birth Date", typeof(string));
            dtTemp.Columns.Add("Age", typeof(string));
            dtTemp.Columns.Add("Gender", typeof(string));
            //Day Program
            dtTemp.Columns.Add("Day Program/Start Date", typeof(string));
            dtTemp.Columns.Add("Day Program/End Date", typeof(string));
            dtTemp.Columns.Add("Day Program/Department", typeof(string));
            dtTemp.Columns.Add("Day Program/Behavior Analyst", typeof(string));
            //Residential Program
            dtTemp.Columns.Add("Residential Program/Start Date", typeof(string));
            dtTemp.Columns.Add("Residential Program/End Date", typeof(string));
            dtTemp.Columns.Add("Residential Program/Department", typeof(string));
            dtTemp.Columns.Add("Residential Program/Behavior Analyst", typeof(string));
            //Day
            dtTemp.Columns.Add("Day/Start Date", typeof(string));
            dtTemp.Columns.Add("Day/End Date", typeof(string));
            dtTemp.Columns.Add("Day/Department", typeof(string));
            dtTemp.Columns.Add("Day/Behavior Analyst", typeof(string));
            //Residential
            dtTemp.Columns.Add("Residential/Start Date", typeof(string));
            dtTemp.Columns.Add("Residential/End Date", typeof(string));
            dtTemp.Columns.Add("Residential/Department", typeof(string));
            dtTemp.Columns.Add("Residential/Behavior Analyst", typeof(string));


            Dictionary<string, DataRow> clientData = new Dictionary<string, DataRow>();

            foreach (DataRow dr in originalTable.Rows)
            {
                string clientName = dr["studentPersonalName"].ToString();

                if (!clientData.ContainsKey(clientName))
                {
                    DataRow drTemp = dtTemp.NewRow();
                    drTemp["Client Name"] = clientName;
                    drTemp["Birth Date"] = dr["BirthDate"].ToString();
                    drTemp["Age"] = dr["Age"].ToString();
                    drTemp["Gender"] = dr["Gender"].ToString();
                    clientData[clientName] = drTemp;
                }

                DataRow existingRow = clientData[clientName];

                if (dr["PlacementType"] != null && dr["PlacementType"].ToString() != "")
                {
                    string placementType = dr["PlacementType"].ToString();
                    string startDate = dr["StartDate"].ToString();
                    string endDate = dr["EndDate"].ToString();
                    string department = dr["Department"].ToString();
                    string behaviorAnalyst = dr["BehaviorAnalyst"].ToString();

                    if (placementType == "Day Program")
                    {
                        existingRow["Day Program/Start Date"] = MergeValues(existingRow["Day Program/Start Date"], startDate);
                        existingRow["Day Program/End Date"] = MergeValues(existingRow["Day Program/End Date"], endDate);
                        existingRow["Day Program/Department"] = MergeValues(existingRow["Day Program/Department"], department);
                        existingRow["Day Program/Behavior Analyst"] = MergeValues(existingRow["Day Program/Behavior Analyst"], behaviorAnalyst);
                    }
                    else if (placementType == "Residential Program")
                    {
                        existingRow["Residential Program/Start Date"] = MergeValues(existingRow["Residential Program/Start Date"], startDate);
                        existingRow["Residential Program/End Date"] = MergeValues(existingRow["Residential Program/End Date"], endDate);
                        existingRow["Residential Program/Department"] = MergeValues(existingRow["Residential Program/Department"], department);
                        existingRow["Residential Program/Behavior Analyst"] = MergeValues(existingRow["Residential Program/Behavior Analyst"], behaviorAnalyst);
                    }
                    else if (placementType == "Day")
                    {
                        existingRow["Day/Start Date"] = MergeValues(existingRow["Day/Start Date"], startDate);
                        existingRow["Day/End Date"] = MergeValues(existingRow["Day/End Date"], endDate);
                        existingRow["Day/Department"] = MergeValues(existingRow["Day/Department"], department);
                        existingRow["Day/Behavior Analyst"] = MergeValues(existingRow["Day/Behavior Analyst"], behaviorAnalyst);
                    }
                    else if (placementType == "Residential")
                    {
                        existingRow["Residential/Start Date"] = MergeValues(existingRow["Residential/Start Date"], startDate);
                        existingRow["Residential/End Date"] = MergeValues(existingRow["Residential/End Date"], endDate);
                        existingRow["Residential/Department"] = MergeValues(existingRow["Residential/Department"], department);
                        existingRow["Residential/Behavior Analyst"] = MergeValues(existingRow["Residential/Behavior Analyst"], behaviorAnalyst);
                    }
                }
            }

            foreach (var row in clientData.Values)
            {
                dtTemp.Rows.Add(row);
            }

            dtTemp.DefaultView.Sort = dtTemp.Columns["Client Name"].ColumnName + " ASC";
            dtTemp = dtTemp.DefaultView.ToTable();

            return dtTemp;
        }

        private string MergeValues(object existingValue, string newValue)
        {
            string existing = existingValue != DBNull.Value ? existingValue.ToString() : "";
            return existing == "" ? newValue : existing + ", " + newValue;
        }

        protected void btnVendor_Click(object sender, EventArgs e)
        {
            try
            {

                FillRelationship();
                FillConStudNameIDs();

                HContactStudname.Value = "All";
                HContactstatus.Value = "1";
                HContactRelation.Value = "All";

                CheckBoxListcontact.Items[0].Selected = true;
                CheckBoxListcontact.Items[1].Selected = false;
                
                divbirthdate.Visible = false;
                divContact.Visible = true;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnVendor";
                RVClientReport.SizeToReportContent = true;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "Client/Contact/Vendor";
                RVClientReport.Visible = true;
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ClientReportContact"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[3];
                //parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                parm[0] = new ReportParameter("HContactStudname", HContactStudname.Value.ToString());
                parm[1] = new ReportParameter("HContactRelation", HContactRelation.Value.ToString());
                parm[2] = new ReportParameter("HContactstatus", HContactstatus.Value.ToString());
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShowVendor_Click(object sender, EventArgs e)
        {
            try
            {
                string chkcontact = "";

                foreach (ListItem item in CheckBoxListcontact.Items)
                {
                    if (item.Selected == true)
                    {
                        chkcontact += item.Text;
                    }
                }
                if (chkcontact == "Active")
                {
                    HContactstatus.Value = "1";
                }
                //if (chkcontact == "Inactive")
                //{
                //    HContactstatus.Value = "0";
                //}
                if (chkcontact == "Discharged")
                {
                    HContactstatus.Value = "2";
                }
                //if (chkcontact == "ActiveInactive")
                //{
                //    HContactstatus.Value = "0,1";
                //}
                if (chkcontact == "ActiveDischarged")
                {
                    HContactstatus.Value = "1,2";
                }
                //if (chkcontact == "InactiveDischarged")
                //{
                //    HContactstatus.Value = "0,2";
                //}
                //if (chkcontact == "ActiveInactiveDischarged")
                //{
                //    HContactstatus.Value = "0,1,2";
                //}

                if (DropDownCheckBoxesConStudname.SelectedIndex == -1 && HContactStudname.Value == "")
                {
                    HContactStudname.Value = "All";
                }
                if (DropDownCheckBoxesRelation.SelectedIndex == -1 && HContactRelation.Value == "")
                {
                    HContactRelation.Value = "All";
                }

                divbirthdate.Visible = false;
                divContact.Visible = true;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                hdnMenu.Value = "btnVendor";
                RVClientReport.SizeToReportContent = true;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "Client/Contact/Vendor";
                RVClientReport.Visible = true;
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ClientReportContact"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[3];
                //parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                parm[0] = new ReportParameter("HContactStudname", HContactStudname.Value.ToString());
                parm[1] = new ReportParameter("HContactRelation", HContactRelation.Value.ToString());
                parm[2] = new ReportParameter("HContactstatus", HContactstatus.Value.ToString());
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void btnBirthdate_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;

                hdnMenu.Value = "btnBirthdate";
                RVClientReport.SizeToReportContent = false;
                ddlQuarter.SelectedValue = "0";
                tdMsg.InnerHtml = "";
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients by Birthdate Quarter";
                divbirthdate.Visible = true;
                RVClientReport.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnResRoster_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                divchanges.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnResRoster";
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "Residential Roster Report";
                RVClientReport.Visible = true;
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ClientReportResRoster"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[1];
                parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAllPlacement_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                FillDept(ddlDeptLocDept);
                FillDept(ddlDeptPlctypeDept);
                FillLocation(ddlDeptLocLoc);
                FillLocation(ddlLocLoc);
                FillPlacementType(ddlDeptPlctypePlcType);
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = true;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnAllPlacement";
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients by Placement";
                RVClientReport.Visible = true;
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                string ActiveStartDate = (txtActiveStartDate.Text != "" ? GetDateFromText(txtActiveStartDate.Text) : "");
                string ActiveEndDate = (txtActiveEndDate.Text != "" ? GetDateFromText(txtActiveEndDate.Text) : "");
                string DischrEndDate = (txtDischrEndDate.Text != "" ? GetDateFromText(txtDischrEndDate.Text) : "");
                string DischrStartDate = (txtDischrStartDate.Text != "" ? GetDateFromText(txtDischrStartDate.Text) : "");
                string NewEndDate = (txtNewEndDate.Text != "" ? GetDateFromText(txtNewEndDate.Text) : "");
                string NewStartDate = (txtNewStartDate.Text != "" ? GetDateFromText(txtNewStartDate.Text) : "");
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["PlacementReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[8];
                parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                parm[1] = new ReportParameter("Department", (hdnballet.Value == "" ? "0" : (hdnballet.Value == "Choose Department and Location" ? ddlDeptLocDept.SelectedValue.ToString() : ddlDeptPlctypeDept.SelectedValue.ToString())));
                parm[2] = new ReportParameter("PlacementType", (hdnballet.Value == "" ? "0" : (hdnballet.Value == "Choose Department and Placement Type" ? ddlDeptPlctypePlcType.SelectedValue.ToString() : ddlDeptPlctypePlcType.SelectedValue.ToString())));
                parm[3] = new ReportParameter("Location", (hdnballet.Value == "" ? "0" : (hdnballet.Value == "Choose Department and Location" ? ddlDeptLocLoc.SelectedValue.ToString() : ddlLocLoc.SelectedValue.ToString())));
                parm[4] = new ReportParameter("StartDate", (hdnDateRange.Value == "" ? "1900-01-01" : (hdnDateRange.Value == "Active Placement" ? ActiveStartDate : (hdnDateRange.Value == "Discharged Placement" ? DischrStartDate : NewStartDate))));
                parm[5] = new ReportParameter("EndDate", (hdnDateRange.Value == "" ? GetDateFromToday(Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("dd-MM-yyyy")) : (hdnDateRange.Value == "Active Placement" ? ActiveEndDate : (hdnDateRange.Value == "Discharged Placement" ? DischrEndDate : NewEndDate))));
                parm[6] = new ReportParameter("DateType", (hdnDateRange.Value == "" ? "0" :(hdnDateRange.Value == "Active Placement"?"Active Placement,New Placement":hdnDateRange.Value)));
                parm[7] = new ReportParameter("CategoryType", (hdnballet.Value == "" ? "0" : hdnballet.Value));
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAllFunder_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                FillFundingSource();
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = true;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnAllFunder";
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients by Funder";
                RVClientReport.Visible = true;
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["FunderReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[2];
                parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                parm[1] = new ReportParameter("FundingSource", "0");
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FillDept(DropDownList ddlDept)
        {
            BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
            var DeptList = (from objPA in Objdata.LookUps
                            where objPA.LookupType == "PlacementDepartment"
                            select new
                            {
                                DeptId = objPA.LookupId,
                                DeptName = objPA.LookupName
                            }).ToList();
            DataTable DtDept = new DataTable();
            DtDept.Columns.Add("DeptName", typeof(String));
            DtDept.Columns.Add("DeptId", typeof(String));
            string[] row = new string[2];
            row[0] = "------Select------";
            row[1] = "0";
            DtDept.Rows.Add(row);
            foreach (var Deptsource in DeptList)
            {
                row[0] = Deptsource.DeptName.ToString();
                row[1] = Deptsource.DeptId.ToString();
                DtDept.Rows.Add(row);
            }
            ddlDept.DataSource = null;
            ddlDept.DataBind();
            ddlDept.DataSource = DtDept;
            ddlDept.DataTextField = "DeptName";
            ddlDept.DataValueField = "DeptId";
            ddlDept.DataBind();
        }
        private void FillLocation(DropDownList ddlLoc)
        {
            BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
            var LocList = (from objPA in Objdata.Classes
                           where objPA.ActiveInd == "A"
                           select new
                           {
                               LocId = objPA.ClassId,
                               LocName = objPA.ClassName
                           }).ToList();
            DataTable DtLoc = new DataTable();
            DtLoc.Columns.Add("LocName", typeof(String));
            DtLoc.Columns.Add("LocId", typeof(String));
            string[] row = new string[2];
            row[0] = "------Select------";
            row[1] = "0";
            DtLoc.Rows.Add(row);
            foreach (var Locsource in LocList)
            {
                row[0] = Locsource.LocName.ToString();
                row[1] = Locsource.LocId.ToString();
                DtLoc.Rows.Add(row);
            }
            ddlLoc.DataSource = null;
            ddlLoc.DataBind();
            ddlLoc.DataSource = DtLoc;
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataBind();
        }
        private void FillPlacementType(DropDownList ddlPlcType)
        {
            BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
            var PlcTypeList = (from objPA in Objdata.LookUps
                               where objPA.LookupType == "Placement Type"
                               select new
                               {
                                   PlcId = objPA.LookupId,
                                   PlcName = objPA.LookupName
                               }).ToList();
            DataTable DtPlacement = new DataTable();
            DtPlacement.Columns.Add("PlcName", typeof(String));
            DtPlacement.Columns.Add("PlcId", typeof(String));
            string[] row = new string[2];
            row[0] = "------Select------";
            row[1] = "0";
            DtPlacement.Rows.Add(row);
            foreach (var Plcsource in PlcTypeList)
            {
                row[0] = Plcsource.PlcName.ToString();
                row[1] = Plcsource.PlcId.ToString();
                DtPlacement.Rows.Add(row);
            }
            ddlPlcType.DataSource = null;
            ddlPlcType.DataBind();
            ddlPlcType.DataSource = DtPlacement;
            ddlPlcType.DataTextField = "PlcName";
            ddlPlcType.DataValueField = "PlcId";
            ddlPlcType.DataBind();
        }
        private void FillFundingSource()
        {
            BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
            var Funding = (from objPA in Objdata.StudentPersonalPAs
                           where objPA.FundingSource != null && objPA.FundingSource != ""
                           select new
                           {
                               Fsource = objPA.FundingSource
                           }).OrderBy(x => x.Fsource).Distinct().ToList();
            DataTable DtFunding = new DataTable();
            DtFunding.Columns.Add("FundingSource", typeof(String));
            DtFunding.Columns.Add("FundingSourceId", typeof(String));
            string[] row = new string[2];
            row[0] = "------Select------";
            row[1] = "0";
            DtFunding.Rows.Add(row);
            foreach (var fundsource in Funding)
            {
                row[0] = fundsource.Fsource.ToString();
                row[1] = fundsource.Fsource.ToString();
                DtFunding.Rows.Add(row);
            }
            ddlFundingSource.DataSource = null;
            ddlFundingSource.DataBind();
            ddlFundingSource.DataSource = DtFunding;
            ddlFundingSource.DataTextField = "FundingSource";
            ddlFundingSource.DataValueField = "FundingSourceId";
            ddlFundingSource.DataBind();
        }
        //private void FillMonth()
        //{
        //    object[] dt = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        //    List<string> Monthresult = (from d in dt select d.ToString()).ToList();

        //    DataTable DtMonth = new DataTable();
        //    DtMonth.Columns.Add("MonthName", typeof(String));
        //    DtMonth.Columns.Add("MonthNameId", typeof(String));
        //    string[] row = new string[2];
        //    row[0] = "------Select------";
        //    row[1] = "0";
        //    DtMonth.Rows.Add(row);
        //    foreach (var MnthName in Monthresult)
        //    {
        //        row[0] = MnthName.ToString();
        //        row[1] = MnthName.ToString();
        //        DtMonth.Rows.Add(row);
        //    }
        //    ddlMonth.DataSource = null;
        //    ddlMonth.DataBind();
        //    ddlMonth.DataSource = DtMonth;
        //    ddlMonth.DataTextField = "MonthName";
        //    ddlMonth.DataValueField = "MonthNameId";
        //    ddlMonth.DataBind();
        //}

        protected void btnShowFunder_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divDischarge.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["FunderReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[2];
                parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                parm[1] = new ReportParameter("FundingSource", ddlFundingSource.SelectedValue.ToString());
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAllBirthdate_Click(object sender, EventArgs e)
        {
            try
            {
                //FillMonth();
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = true;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnAllBirthdate";
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients by Birthdate";
                RVClientReport.Visible = true;
                string BithdateStart = (txtBithdateStart.Text != "" ? GetDateFromText(txtBithdateStart.Text) : "");
                string BirthdateEnd = (txtBirthdateEnd.Text != "" ? GetDateFromText(txtBirthdateEnd.Text) : "");
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["BirthdateReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[5];
                parm[0] = new ReportParameter("Month", ddlMonth.SelectedItem.Value.ToString());
                parm[1] = new ReportParameter("AgeFrom", (txtAgeFrom.Text == "" ? "0" : txtAgeFrom.Text));
                parm[2] = new ReportParameter("AgeTo", (txtAgeTo.Text == "" ? "200" : txtAgeTo.Text));
                parm[3] = new ReportParameter("StartDate", (BithdateStart == "" ? "1900-01-01" : BithdateStart));
                parm[4] = new ReportParameter("EndDate", (BirthdateEnd == "" ? GetDateFromToday(Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("dd-MM-yyyy")) : BirthdateEnd));
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAllAdmissionDate_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = false;
                divAdmission.Visible = true;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnAllAdmissionDate";
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients by Admission date";
                RVClientReport.Visible = true;
                string AdmissionFrom = (txtAdmissionFrom.Text != "" ? GetDateFromText(txtAdmissionFrom.Text) : "");
                string AdmissionTo = (txtAdmissionTo.Text != "" ? GetDateFromText(txtAdmissionTo.Text) : "");
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["AdmissionDateReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[3];
                parm[0] = new ReportParameter("AdmStart", (AdmissionFrom == "" ? "1900-01-01" : AdmissionFrom));
                parm[1] = new ReportParameter("AdmEnd", (AdmissionTo == "" ? GetDateFromToday(Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("dd-MM-yyyy")) : AdmissionTo));
                parm[2] = new ReportParameter("NumberOfAdm", (txtNumberOfAdmission.Text == "" ? "10000000" : txtNumberOfAdmission.Text));
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAllDischargedate_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = false;
                divDischarge.Visible = true;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnAllDischargedate";
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients by Discharge date";
                RVClientReport.Visible = true;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DischargeDateReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[1];
                parm[0] = new ReportParameter("Status", "D");//rbtnDischargeStatus.SelectedValue.ToString());
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnStatistical_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                divnodata.Visible = false;
                divStatisticalNew.Visible = false;
                divchanges.Visible = false;
                divStatistical.Visible = true;
                divDischarge.Visible = false;
                divAdmission.Visible = false;
                divbyBirthdate.Visible = false;
                divFunder.Visible = false;
                divPlacement.Visible = false;
                btnShowReport.Visible = false;
                btnResetAllClient.Visible = false;
                hdnMenu.Value = "btnStatistical";
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "Statistical Report";
                divbirthdate.Visible = false;
                var selected = ChkStatisticalList.Items.Cast<ListItem>().Where(li => li.Selected).Count();
                if (selected != 0)
                {
                    List<ListItem> selectedItemList = ChkStatisticalList.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
                    RVClientReport.Visible = true;
                    RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["StatisticalReport"];
                    RVClientReport.ShowParameterPrompts = false;
                    ReportParameter[] parm = new ReportParameter[8];
                    parm[0] = new ReportParameter("Totalnumberofclient", ContainsLoop("Total number of client", selectedItemList));
                    parm[1] = new ReportParameter("Gender", ContainsLoop("Gender", selectedItemList));
                    parm[2] = new ReportParameter("Department", ContainsLoop("Department", selectedItemList));
                    parm[3] = new ReportParameter("PlacementType", ContainsLoop("Placement Type", selectedItemList));
                    parm[4] = new ReportParameter("Program", ContainsLoop("Program", selectedItemList));
                    parm[5] = new ReportParameter("Location", ContainsLoop("Location", selectedItemList));
                    parm[6] = new ReportParameter("Race", ContainsLoop("Race", selectedItemList));
                    parm[7] = new ReportParameter("Maximumclientoccupancy", ContainsLoop("Maximum client occupancy", selectedItemList));
                    this.RVClientReport.ServerReport.SetParameters(parm);
                    RVClientReport.ServerReport.Refresh();
                }
                else
                {
                    tdMsg.InnerHtml = "<div class='warning_box'>Please select report items</div>";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShowBirthdate_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                RVClientReport.Visible = true;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["BirthdateReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[5];
                parm[0] = new ReportParameter("Month", ddlMonth.SelectedItem.Value.ToString());
                parm[1] = new ReportParameter("AgeFrom", (txtAgeFrom.Text == "" ? "0" : txtAgeFrom.Text));
                parm[2] = new ReportParameter("AgeTo", (txtAgeTo.Text == "" ? "200" : txtAgeTo.Text));
                parm[3] = new ReportParameter("StartDate", (txtBithdateStart.Text == "" ? "1900-01-01" : txtBithdateStart.Text));
                parm[4] = new ReportParameter("EndDate", (txtBirthdateEnd.Text == "" ? GetDateFromToday(Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("dd-MM-yyyy")) : txtBirthdateEnd.Text));
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShowAdmissionDate_Click(object sender, EventArgs e)
        {
            try
            {
                RVClientReport.Visible = true;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["AdmissionDateReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[3];
                parm[0] = new ReportParameter("AdmStart", (txtAdmissionFrom.Text == "" ? "1900-01-01" : txtAdmissionFrom.Text));
                parm[1] = new ReportParameter("AdmEnd", (txtAdmissionTo.Text == "" ? GetDateFromToday(Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("dd-MM-yyyy")) : txtAdmissionTo.Text));
                parm[2] = new ReportParameter("NumberOfAdm", (txtNumberOfAdmission.Text == "" ? "10000000" : txtNumberOfAdmission.Text));
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShowDischarge_Click(object sender, EventArgs e)
        {
            try
            {
                RVClientReport.Visible = true;
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["DischargeDateReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[1];
                parm[0] = new ReportParameter("Status", "D");//rbtnDischargeStatus.SelectedValue.ToString());
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShowStatistical_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = ChkStatisticalList.Items.Cast<ListItem>().Where(li => li.Selected).Count();
                if (selected != 0)
                {
                    List<ListItem> selectedItemList = ChkStatisticalList.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
                    RVClientReport.Visible = true;
                    RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["StatisticalReport"];
                    RVClientReport.ShowParameterPrompts = false;
                    ReportParameter[] parm = new ReportParameter[8];
                    parm[0] = new ReportParameter("Totalnumberofclient", ContainsLoop("Total number of client", selectedItemList));
                    parm[1] = new ReportParameter("Gender", ContainsLoop("Gender", selectedItemList));
                    parm[2] = new ReportParameter("Department", ContainsLoop("Department", selectedItemList));
                    parm[3] = new ReportParameter("PlacementType", ContainsLoop("Placement Type", selectedItemList));
                    parm[4] = new ReportParameter("Program", ContainsLoop("Program", selectedItemList));
                    parm[5] = new ReportParameter("Location", ContainsLoop("Location", selectedItemList));
                    parm[6] = new ReportParameter("Race", ContainsLoop("Race", selectedItemList));
                    parm[7] = new ReportParameter("Maximumclientoccupancy", ContainsLoop("Maximum client occupancy", selectedItemList));
                    this.RVClientReport.ServerReport.SetParameters(parm);
                    RVClientReport.ServerReport.Refresh();
                }
                else
                {
                    tdMsg.InnerHtml = "<div class='warning_box'>Please select report items</div>";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string ContainsLoop(string StrChkItem, List<ListItem> selectedItemList)
        {
            for (int i = 0; i < selectedItemList.Count; i++)
            {
                if (selectedItemList[i].ToString() == StrChkItem)
                {
                    return "true";
                }
            }
            return "false";
        }

        protected void btnShowPlacement_Click(object sender, EventArgs e)
        {
            try
            {
                RVClientReport.Visible = true;
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                string ActiveStartDate = (txtActiveStartDate.Text != "" ? GetDateFromText(txtActiveStartDate.Text) : "");
                string ActiveEndDate = (txtActiveEndDate.Text != "" ? GetDateFromText(txtActiveEndDate.Text) : "");
                string DischrEndDate = (txtDischrEndDate.Text != "" ? GetDateFromText(txtDischrEndDate.Text) : "");
                string DischrStartDate = (txtDischrStartDate.Text != "" ? GetDateFromText(txtDischrStartDate.Text) : "");
                string NewEndDate = (txtNewEndDate.Text != "" ? GetDateFromText(txtNewEndDate.Text) : "");
                string NewStartDate = (txtNewStartDate.Text != "" ? GetDateFromText(txtNewStartDate.Text) : "");
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["PlacementReport"];
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[8];
                parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                parm[1] = new ReportParameter("Department", (hdnballet.Value == "" ? "0" : (hdnballet.Value == "Choose Department and Location" ? ddlDeptLocDept.SelectedValue.ToString() : ddlDeptPlctypeDept.SelectedValue.ToString())));
                parm[2] = new ReportParameter("PlacementType", (hdnballet.Value == "" ? "0" : (hdnballet.Value == "Choose Department and Placement Type" ? ddlDeptPlctypePlcType.SelectedValue.ToString() : ddlDeptPlctypePlcType.SelectedValue.ToString())));
                parm[3] = new ReportParameter("Location", (hdnballet.Value == "" ? "0" : (hdnballet.Value == "Choose Department and Location" ? ddlDeptLocLoc.SelectedValue.ToString() : ddlLocLoc.SelectedValue.ToString())));
                parm[4] = new ReportParameter("StartDate", (hdnDateRange.Value == "" ? "1900-01-01" : (hdnDateRange.Value == "Active Placement" ? ActiveStartDate : (hdnDateRange.Value == "Discharged Placement" ? DischrStartDate : NewStartDate))));
                parm[5] = new ReportParameter("EndDate", (hdnDateRange.Value == "" ? GetDateFromToday(Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("dd-MM-yyyy")) : (hdnDateRange.Value == "Active Placement" ? ActiveEndDate : (hdnDateRange.Value == "Discharged Placement" ? DischrEndDate : NewEndDate))));
                parm[6] = new ReportParameter("DateType", (hdnDateRange.Value == "" ? "0" : (hdnDateRange.Value == "Active Placement" ? "Active Placement,New Placement" : hdnDateRange.Value)));
                parm[7] = new ReportParameter("CategoryType", (hdnballet.Value == "" ? "0" : hdnballet.Value));
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetDateFromText(string DateString)
        {
            string[] DateArray = new string[3];
            DateArray = DateString.Split('/');
            return DateArray[2].ToString() + "-" + DateArray[0].ToString() + "-" + DateArray[1].ToString();
        }
        private string GetDateFromToday(string DateString)
        {
            string[] DateArray = new string[3];
            DateArray = DateString.Split('-');
            return DateArray[2].ToString() + "-" + DateArray[1].ToString() + "-" + DateArray[0].ToString();
        }

        protected void btnContactChanges_Click(object sender, EventArgs e)
        {
            divContact.Visible = false;
            divnodata.Visible = false;
            divStatisticalNew.Visible = false;
            divchanges.Visible = true;
            divStatistical.Visible = false;
            divDischarge.Visible = false;
            divAdmission.Visible = false;
            divbyBirthdate.Visible = false;
            divFunder.Visible = false;
            divPlacement.Visible = false;
            btnShowReport.Visible = false;
            btnResetAllClient.Visible = false;
            hdnMenu.Value = "btnContactChanges";
            tdMsg.InnerHtml = "";
            RVClientReport.Visible = false;
            HeadingDiv.Visible = true;
            HeadingDiv.InnerHtml = "Contact Changes";
            divbirthdate.Visible = false;
        }

        protected void btnGuardianChanges_Click(object sender, EventArgs e)
        {
            divContact.Visible = false;
            divnodata.Visible = false;
            divStatisticalNew.Visible = false;
            divchanges.Visible = true;
            divStatistical.Visible = false;
            divDischarge.Visible = false;
            divAdmission.Visible = false;
            divbyBirthdate.Visible = false;
            divFunder.Visible = false;
            divPlacement.Visible = false;
            btnShowReport.Visible = false;
            btnResetAllClient.Visible = false;
            hdnMenu.Value = "btnGuardianChanges";
            tdMsg.InnerHtml = "";
            RVClientReport.Visible = false;
            HeadingDiv.Visible = true;
            HeadingDiv.InnerHtml = "Guardianship Changes";
            divbirthdate.Visible = false;
        }

        protected void btnPlcChange_Click(object sender, EventArgs e)
        {
            divContact.Visible = false;
            divnodata.Visible = false;
            divStatisticalNew.Visible = false;
            divchanges.Visible = true;
            divStatistical.Visible = false;
            divDischarge.Visible = false;
            divAdmission.Visible = false;
            divbyBirthdate.Visible = false;
            divFunder.Visible = false;
            divPlacement.Visible = false;
            btnShowReport.Visible = false;
            btnResetAllClient.Visible = false;
            hdnMenu.Value = "btnPlcChange";
            tdMsg.InnerHtml = "";
            RVClientReport.Visible = false;
            HeadingDiv.Visible = true;
            HeadingDiv.InnerHtml = "Placement Changes";
            divbirthdate.Visible = false;
        }

        protected void btnFundChange_Click(object sender, EventArgs e)
        {
            divContact.Visible = false;
            divnodata.Visible = false;
            divStatisticalNew.Visible = false;
            divchanges.Visible = true;
            divStatistical.Visible = false;
            divDischarge.Visible = false;
            divAdmission.Visible = false;
            divbyBirthdate.Visible = false;
            divFunder.Visible = false;
            divPlacement.Visible = false;
            btnShowReport.Visible = false;
            btnResetAllClient.Visible = false;
            hdnMenu.Value = "btnFundChange";
            tdMsg.InnerHtml = "";
            RVClientReport.Visible = false;
            HeadingDiv.Visible = true;
            HeadingDiv.InnerHtml = "Funding Changes";
            divbirthdate.Visible = false;
        }

        protected void btnChangeResult_Click(object sender, EventArgs e)
        {
            try
            {
                RVClientReport.Visible = true;
                string NewStartDate = GetDateFromText(txtchangeSdate.Text);
                string NewEndDate = GetDateFromText(txtchangeEdate.Text);
                RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                if (hdnMenu.Value == "btnFundChange")
                {
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["FundingChangesReport"];
                }
                else if (hdnMenu.Value == "btnPlcChange")
                {
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["PlacementChangesReport"];
                }
                else if (hdnMenu.Value == "btnGuardianChanges")
                {
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["GuardianshipChangesReport"];
                }
                else if (hdnMenu.Value == "btnContactChanges")
                {
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ContactChangesReport"];
                }
                RVClientReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[2];
                parm[0] = new ReportParameter("StartDate", NewStartDate);
                parm[1] = new ReportParameter("EndDate", NewEndDate);
                this.RVClientReport.ServerReport.SetParameters(parm);
                RVClientReport.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShowStatistical2_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = ChkStatisticalList2.Items.Cast<ListItem>().Where(li => li.Selected).Count();
                if (selected != 0)
                {
                    divnodata.Visible = false;
                    int PmCnt = 0;
                    int SetPmCnt = 0;
                    List<ListItem> selectedItemList = ChkStatisticalList2.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
                    RVClientReport.Visible = true;
                    RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["StatisticalReportNew"];
                    RVClientReport.ShowParameterPrompts = false;

                    bool NameStatus = Convert.ToBoolean(ContainsLoop("Student Name", selectedItemList));
                    bool LocationStatus = Convert.ToBoolean(ContainsLoop("Location", selectedItemList));
                    bool RaceStatus = Convert.ToBoolean(ContainsLoop("Race", selectedItemList));
                    bool ActiveStatus = Convert.ToBoolean(ContainsLoop("Status", selectedItemList));

                    if (NameStatus == false && DropDownCheckBoxesStudname.SelectedIndex >= 0) { DropDownCheckBoxesStudname.ClearSelection(); hfstudname.Value = ""; } //else { if (hfstudname.Value != "") { DropDownCheckBoxesStudname.SelectedValue = hfstudname.Value; } }
                    if (LocationStatus == false && DropDownCheckBoxesLocation.SelectedIndex >= 0) { DropDownCheckBoxesLocation.ClearSelection(); hflocation.Value = ""; } //else { if (hflocation.Value != "") { DropDownCheckBoxesLocation.SelectedValue = hflocation.Value; } }
                    if (RaceStatus == false && DropDownCheckBoxesRaces.SelectedIndex >= 0) { DropDownCheckBoxesRaces.ClearSelection(); hfrace.Value = ""; } //else { if (hfrace.Value != "") { DropDownCheckBoxesRaces.SelectedValue = hfrace.Value; } }
                    if (ActiveStatus == false && DropDownCheckBoxesActive.SelectedIndex >= 0) { DropDownCheckBoxesActive.ClearSelection(); hfstatus.Value = ""; } else { if (hfstatus.Value == "") { hfstatus.Value = "A"; DropDownCheckBoxesActive.SelectedValue = hfstatus.Value; } }

                    if (hfstudname.Value != "" && DropDownCheckBoxesStudname.SelectedIndex >= 0 && NameStatus == true) { PmCnt += 1; }
                    if (hflocation.Value != "" && DropDownCheckBoxesLocation.SelectedIndex >= 0 && LocationStatus == true) { PmCnt += 1; }
                    if (hfrace.Value != "" && DropDownCheckBoxesRaces.SelectedIndex >= 0 && RaceStatus == true) { PmCnt += 1; }
                    if (hfstatus.Value != "" && DropDownCheckBoxesActive.SelectedIndex >= 0 && ActiveStatus == true) { PmCnt += 1; }

                    ReportParameter[] parm = new ReportParameter[12];
                    if (PmCnt > 0)
                    {
                        SetPmCnt = 12 + PmCnt;
                        parm = new ReportParameter[SetPmCnt];
                    }

                    parm[0] = new ReportParameter("ParamStudRow", ContainsLoop("Total number of client", selectedItemList));
                    parm[1] = new ReportParameter("ParamStudName", ContainsLoop("Student Name", selectedItemList));
                    parm[2] = new ReportParameter("ParamLocation", ContainsLoop("Location", selectedItemList));
                    parm[3] = new ReportParameter("ParamCity", ContainsLoop("City", selectedItemList));
                    parm[4] = new ReportParameter("ParamState", ContainsLoop("State", selectedItemList));
                    parm[5] = new ReportParameter("ParamLanguage", ContainsLoop("Primary Language", selectedItemList));
                    parm[6] = new ReportParameter("ParamRace", ContainsLoop("Race", selectedItemList));
                    parm[7] = new ReportParameter("ParamPlacement", ContainsLoop("Placement Type", selectedItemList));
                    parm[8] = new ReportParameter("ParamDepartment", ContainsLoop("Department", selectedItemList));
                    parm[9] = new ReportParameter("ParamProgram", ContainsLoop("Program", selectedItemList));
                    parm[10] = new ReportParameter("ParamGender", ContainsLoop("Gender", selectedItemList));
                    parm[11] = new ReportParameter("ParamActive", ContainsLoop("Status", selectedItemList));                    

                    for (int i = 1; i <= PmCnt; i++)
                    {
                        if (hfstudname.Value != "" && DropDownCheckBoxesStudname.SelectedIndex >= 0 && NameStatus == true) { int Studi = 11 + i; parm[Studi] = new ReportParameter("GetStudID", hfstudname.Value); i++; }
                        if (hflocation.Value != "" && DropDownCheckBoxesLocation.SelectedIndex >= 0 && LocationStatus == true) { int Loci = 11 + i; parm[Loci] = new ReportParameter("GetLocationID", hflocation.Value); i++; }
                        if (hfrace.Value != "" && DropDownCheckBoxesRaces.SelectedIndex >= 0 && RaceStatus == true) { int Raci = 11 + i; parm[Raci] = new ReportParameter("GetRaceID", hfrace.Value); i++; }
                        if (hfstatus.Value != "" && DropDownCheckBoxesActive.SelectedIndex >= 0 && ActiveStatus == true) { int Stati = 11 + i; parm[Stati] = new ReportParameter("GetActiveID", hfstatus.Value); i++; }
                    }

                    this.RVClientReport.ServerReport.SetParameters(parm);
                    RVClientReport.ServerReport.Refresh();

                    //hfstudname.Value = "";
                    //hflocation.Value = "";
                    //hfrace.Value = "";
                    //hfstatus.Value = "";
                }
                else
                {
                    tdMsg.InnerHtml = "<div class='warning_box'>Please select report items</div>";
                    RVClientReport.Visible = false;
                    divnodata.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillRelationship()
        {
            try
            {
                BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
                var ConRelation = (from objPA in Objdata.LookUps
                               where objPA.LookupType == "Relationship"
                               select new
                               {
                                   LookupId = objPA.LookupId,
                                   LookupName = objPA.LookupName,
                               }).Distinct().OrderBy(x => x.LookupName).ToList();
                DataTable Dtrelation = new DataTable();
                Dtrelation.Columns.Add("LookupName", typeof(String));
                Dtrelation.Columns.Add("LookupId", typeof(String));
                string[] row = new string[2];
                foreach (var relation in ConRelation)
                {
                    row[0] = relation.LookupName.ToString();
                    row[1] = relation.LookupId.ToString();
                    Dtrelation.Rows.Add(row);
                }
                DropDownCheckBoxesRelation.DataSource = null;
                DropDownCheckBoxesRelation.DataBind();
                DropDownCheckBoxesRelation.DataSource = Dtrelation;
                DropDownCheckBoxesRelation.DataTextField = "LookupName";
                DropDownCheckBoxesRelation.DataValueField = "LookupId";
                DropDownCheckBoxesRelation.DataBind();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        private void FillConStudNameIDs()
        {
            try
            {
                BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();               

                var ConStudname = (from objPA in Objdata.StudentPersonals
                                   where objPA.StudentPersonalId != null 
                                   && objPA.StudentPersonalId != 0 
                                   && objPA.StudentType == "Client" 
                                   && objPA.ClientId > 0
                               select new
                               {
                                   StudentIDs = objPA.StudentPersonalId,
                                   StudentNames = objPA.LastName + " " + objPA.FirstName,
                                   TestName = objPA.LastName
                               }).Distinct().OrderBy(x => x.TestName).ToList();
                DataTable Dtconstudname = new DataTable();
                Dtconstudname.Columns.Add("StudentName", typeof(String));
                Dtconstudname.Columns.Add("StudentId", typeof(String));
                string[] row = new string[2];
                foreach (var studname in ConStudname)
                {
                    row[0] = studname.StudentNames.ToString();
                    row[1] = studname.StudentIDs.ToString();
                    Dtconstudname.Rows.Add(row);
                }
                DropDownCheckBoxesConStudname.DataSource = null;
                DropDownCheckBoxesConStudname.DataBind();
                DropDownCheckBoxesConStudname.DataSource = Dtconstudname;
                DropDownCheckBoxesConStudname.DataTextField = "StudentName";
                DropDownCheckBoxesConStudname.DataValueField = "StudentId";
                DropDownCheckBoxesConStudname.DataBind();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        
        private void FillStudNameIDs()
        {
            try
            {
                BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
                var Funding = (from objPA in Objdata.StudentPersonals
                               where objPA.StudentPersonalId != null && objPA.StudentPersonalId != 0 && objPA.StudentType == "Client" && objPA.ClientId > 0
                               select new
                               {
                                   StudentIDs = objPA.StudentPersonalId,
                                   StudentNames = objPA.LastName + " " + objPA.FirstName,
                                   TestName = objPA.LastName
                               }).Distinct().OrderBy(x => x.TestName).ToList();
                DataTable Dtstudname = new DataTable();
                Dtstudname.Columns.Add("StudentName", typeof(String));
                Dtstudname.Columns.Add("StudentId", typeof(String));
                string[] row = new string[2];
                foreach (var studname in Funding)
                {
                    row[0] = studname.StudentNames.ToString();
                    row[1] = studname.StudentIDs.ToString();
                    Dtstudname.Rows.Add(row);
                    //hfstudname.Value += studname.StudentIDs.ToString() + ", ";
                }
                DropDownCheckBoxesStudname.DataSource = null;
                DropDownCheckBoxesStudname.DataBind();
                DropDownCheckBoxesStudname.DataSource = Dtstudname;
                DropDownCheckBoxesStudname.DataTextField = "StudentName";
                DropDownCheckBoxesStudname.DataValueField = "StudentId";
                DropDownCheckBoxesStudname.DataBind();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        private void FillStudLocationIDs()
        {
            try
            {
                BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
                var Funding = (from objPA in Objdata.Classes
                               where objPA.ClassId != null && objPA.ClassId != 0 && objPA.ActiveInd == "A"
                               select new
                               {
                                   ClassIDs = objPA.ClassId,
                                   ClassNames = objPA.ClassName
                               }).Distinct().OrderBy(x => x.ClassNames).ToList();
                DataTable Dtclsname = new DataTable();
                Dtclsname.Columns.Add("ClassName", typeof(String));
                Dtclsname.Columns.Add("ClassId", typeof(String));
                string[] row = new string[2];
                foreach (var clsname in Funding)
                {
                    row[0] = clsname.ClassNames.ToString();
                    row[1] = clsname.ClassIDs.ToString();
                    Dtclsname.Rows.Add(row);
                    //hflocation.Value += clsname.ClassIDs.ToString() + ", ";
                }
                DropDownCheckBoxesLocation.DataSource = null;
                DropDownCheckBoxesLocation.DataBind();
                DropDownCheckBoxesLocation.DataSource = Dtclsname;
                DropDownCheckBoxesLocation.DataTextField = "ClassName";
                DropDownCheckBoxesLocation.DataValueField = "ClassId";
                DropDownCheckBoxesLocation.DataBind();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        private void FillStudRaceIDs()
        {
            try
            {
                BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
                var Funding = (from objPA in Objdata.StudentPersonals
                               join lkp in Objdata.LookUps on objPA.RaceId equals lkp.LookupId
                               where objPA.RaceId != null && objPA.RaceId != 0 && lkp.LookupType == "Race"
                               select new
                               {
                                   RacedIDs = objPA.RaceId,
                                   RaceNames = lkp.LookupName
                               }).Distinct().OrderBy(x => x.RaceNames).ToList();
                DataTable Dtracname = new DataTable();
                Dtracname.Columns.Add("RaceName", typeof(String));
                Dtracname.Columns.Add("RaceId", typeof(String));
                string[] row = new string[2];
                foreach (var racname in Funding)
                {
                    row[0] = racname.RaceNames.ToString();
                    row[1] = racname.RacedIDs.ToString();
                    Dtracname.Rows.Add(row);
                    //hfrace.Value += racname.RacedIDs.ToString() + ", ";
                }
                DropDownCheckBoxesRaces.DataSource = null;
                DropDownCheckBoxesRaces.DataBind();
                DropDownCheckBoxesRaces.DataSource = Dtracname;
                DropDownCheckBoxesRaces.DataTextField = "RaceName";
                DropDownCheckBoxesRaces.DataValueField = "RaceId";
                DropDownCheckBoxesRaces.DataBind();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        private void FillStudStatusIDs()
        {
            try
            {
                BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
                DataTable Dtracname = new DataTable();
                Dtracname.Columns.Add("Active", typeof(String));
                Dtracname.Columns.Add("ActiveID", typeof(String));
                string[] row = new string[2];
                row[0] = "Active";
                row[1] = "A";
                Dtracname.Rows.Add(row);
                hfstatus.Value += row[1] + ", ";

                //row[0] = "Inactive";
                //row[1] = "I";
                //Dtracname.Rows.Add(row);
                //hfstatus.Value += row[1] + ", ";

                row[0] = "Discharged";
                row[1] = "D";
                Dtracname.Rows.Add(row);
                hfstatus.Value += row[1] + ", ";
                DropDownCheckBoxesActive.DataSource = null;
                DropDownCheckBoxesActive.DataBind();
                DropDownCheckBoxesActive.DataSource = Dtracname;
                DropDownCheckBoxesActive.DataTextField = "Active";
                DropDownCheckBoxesActive.DataValueField = "ActiveID";
                DropDownCheckBoxesActive.DataBind();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        protected void DropDownCheckBoxesRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string LookupId = "";
            string LookupName = "";
            HContactRelation.Value = "";
            if (DropDownCheckBoxesRelation.SelectedIndex == -1)
            {
                HContactRelation.Value = "All";
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem item in DropDownCheckBoxesRelation.Items)
                {
                    if (item.Selected == true)
                    {
                        LookupId += item.Value + ",";
                        LookupName += item.Text + ";";
                    }
                }
                if (LookupId.Length > 0)
                {
                    LookupName = LookupName.Substring(0, (LookupName.Length - 1));
                    HContactRelation.Value = LookupId;
                }
            }
        }

        protected void DropDownCheckBoxesConStudname_SelectedIndexChanged(object sender, EventArgs e)
        {
            string StudentId = "";
            string Studentname = "";
            HContactStudname.Value = "";
            if (DropDownCheckBoxesConStudname.SelectedIndex == -1)
            {
                HContactStudname.Value = "All";
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem item in DropDownCheckBoxesConStudname.Items)
                {
                    if (item.Selected == true)
                    {
                        StudentId += item.Value + ",";
                        Studentname += item.Text + ";";
                    }
                }
                if (StudentId.Length > 0)
                {
                    StudentId = StudentId.Substring(0, (StudentId.Length - 1));
                    HContactStudname.Value = StudentId;
                }
            }
        }

        protected void DropDownCheckBoxesStudname_SelectedIndexChanged(object sender, EventArgs e)
        {
            string StudentId = "";
            string Studentname = "";
            hfstudname.Value = "";
            foreach (System.Web.UI.WebControls.ListItem item in DropDownCheckBoxesStudname.Items)
            {
                if (item.Selected == true)
                {
                    StudentId += item.Value + ",";
                    Studentname += item.Text + ";";
                }
            }
            if (StudentId.Length > 0)
            {
                StudentId = StudentId.Substring(0, (StudentId.Length - 1));
                hfstudname.Value = StudentId;
            }
        }

        protected void DropDownCheckBoxesLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string LocationId = "";
            string Locationname = "";
            foreach (System.Web.UI.WebControls.ListItem item in DropDownCheckBoxesLocation.Items)
            {
                if (item.Selected == true)
                {
                    LocationId += item.Value + ",";
                    Locationname += item.Text + ";";
                }
                //hflocation.Value = LocationId;
            }
            if (LocationId.Length > 0)
            {
                LocationId = LocationId.Substring(0, (LocationId.Length - 1));
                hflocation.Value = LocationId;
            }
        }

        protected void DropDownCheckBoxesRaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            string RacesId = "";
            string Racesname = "";
            foreach (System.Web.UI.WebControls.ListItem item in DropDownCheckBoxesRaces.Items)
            {
                if (item.Selected == true)
                {
                    RacesId += item.Value + ",";
                    Racesname += item.Text + ";";
                }
                //hfrace.Value = RacesId;
            }
            if (RacesId.Length > 0)
            {
                RacesId = RacesId.Substring(0, (RacesId.Length - 1));
                hfrace.Value = RacesId;
            }
        }

        protected void DropDownCheckBoxesActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ActiveId = "";
            string Activename = "";
            foreach (System.Web.UI.WebControls.ListItem item in DropDownCheckBoxesActive.Items)
            {
                if (item.Selected == true)
                {
                    ActiveId += item.Value + ",";
                    Activename += item.Text + ";";
                }
                //hfstatus.Value = ActiveId;
            }
            if (ActiveId.Length > 0)
            {
                ActiveId = ActiveId.Substring(0, (ActiveId.Length - 1));
                hfstatus.Value = ActiveId;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            hfstudname.Value = "";
            hflocation.Value = "";
            hfrace.Value = "";
            hfstatus.Value = "";
            for (int i = 0; i < ChkStatisticalList2.Items.Count; i++)
            {
                ChkStatisticalList2.Items[i].Selected = true;
            }
            btnallClient_Click(sender, e);
        }
    }
}