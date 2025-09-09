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
using System.Web.Script.Services;


namespace ClientDB.Reports
{
    public class Logger
    {
        public static string strPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string logFilePath = strPath + @"/ErrorLog/log.txt";

        public static void LogError(string message, Exception ex = null)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true)) 
                {
                    writer.WriteLine("DateTime: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteLine("Message: " + message);
                    if (ex != null)
                    {
                        writer.WriteLine("Exception: " + ex.Message);
                        writer.WriteLine("Stack Trace: " + ex.StackTrace);
                    }
                    writer.WriteLine("\n");
                }
            }
            catch
            {
            }
        }
    }
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




        protected void btnOldquarter_Click(object sender, EventArgs e)
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
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
        protected void btnquarter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkHighcharts.Checked)
                {
                    btnOldquarter_Click(sender, e);
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
                    btnShowReportVendor.Visible = false;
                    btnResetVendor.Visible = false;
                    hdnMenu.Value = "btnBirthdate";
                    RVClientReport.Visible = false;
                    if (ddlQuarter.SelectedItem.Value != "0")
                    {
                        tdMsg.InnerHtml = "";
                        int Schoolid = 0;
                        string schooltype = ConfigurationManager.AppSettings["Server"];
                        if (schooltype == "NE")
                            Schoolid = 1;
                        else
                            Schoolid = 2;

                        string quarterQuery = "SELECT        SD.StudentPersonalId, SD.SchoolId, SD.LastName + ',' + SD.FirstName AS studentPersonalName, CASE WHEN [ImageUrl] IS NULL OR " + 
                               " [ImageUrl] = '' THEN CASE WHEN SD.Gender = 1 THEN " +
                               " (SELECT        FormatImg " + 
                               " FROM            [dbo].[DefaultImage] " + 
                               "  WHERE        Sex = 'M') ELSE " + 
                               " (SELECT        FormatImg " + 
                               " FROM            [dbo].[DefaultImage] " + 
                               " WHERE        Sex = 'F') END ELSE [ImageUrl] END AS ImageUrl, CONVERT(VARCHAR(10), SD.BirthDate, 101) AS BirthDate, SD.PlaceOfBirth, SD.CountryOfBirth,  " + 
                               " SD.Height, SD.Weight, " + 
                               " (SELECT        LookupName " + 
                               " FROM            LookUp " + 
                               "  WHERE        (LookupId = PL.PlacementType)) AS PlacementType, " + 
                               " (SELECT        LookupName " + 
                               " FROM            LookUp AS LookUp_3 " + 
                               "  WHERE        (LookupId = PL.Department)) AS Department, " + 
                               " (SELECT        LookupName " + 
                               " FROM            LookUp AS LookUp_2 " + 
                               " WHERE        (LookupId = PL.BehaviorAnalyst)) AS BehaviorAnalyst, " + 
                               " (SELECT        LookupName " + 
                               " FROM            LookUp AS LookUp_1 " + 
                               " WHERE        (LookupId = PL.PrimaryNurse)) AS PrimaryNurse, EC.LastName + ',' + EC.FirstName AS emerContact, EC.Title, EC.Phone, DATEDIFF(YEAR,  " + 
                               " SD.BirthDate, GETDATE()) - (CASE WHEN DATEADD(YY, DATEDIFF(YEAR, SD.BirthDate, GETDATE()), SD.BirthDate) > GETDATE() THEN 1 ELSE 0 END) AS Age,  " + 
                               " CASE WHEN DATEPART(MM, SD.BirthDate) >= 01 AND DATEPART(MM, SD.BirthDate) <= 03 THEN 1 ELSE CASE WHEN DATEPART(MM, SD.BirthDate) >= 04 AND  " + 
                               " DATEPART(MM, SD.BirthDate) <= 06 THEN 2 ELSE CASE WHEN DATEPART(MM, SD.BirthDate) >= 07 AND DATEPART(MM, SD.BirthDate)  " + 
                               " <= 09 THEN 3 ELSE 4 END END END AS mMonth, CASE WHEN SD.Gender = 1 THEN 'Male' ELSE 'Female' END AS Gender " + 
                               " FROM            StudentPersonal AS SD LEFT OUTER JOIN " + 
                               " Placement AS PL ON PL.StudentPersonalId = SD.StudentPersonalId LEFT OUTER JOIN " + 
                               " EmergencyContactSchool AS EC ON EC.StudentPersonalId = SD.StudentPersonalId " + 
                               " INNER JOIN LookUp LKP on LKP.LookupId = PL.Department " + 
                               " WHERE        (SD.StudentType = 'Client ')  and (PL.EndDate is null or PL.EndDate >= cast (GETDATE() as DATE)) and PL.Status=1 AND LKP.LookupType = 'Department' " + 
                               " and SD.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " + 
                               " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " + 
                               " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " + 
                               " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " + 
                               " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " + 
                               " and ST.StudentPersonalId not in (SELECT Distinct " + 
                               " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) AND CONVERT(INT,SD.ClientId)>0"; 
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                        con.Open();
                        SqlCommand cmd = new SqlCommand(quarterQuery, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        dt = GetSelectedColumnQuarter(dt, ddlQuarter.SelectedItem.Value);
                        if (dt.Rows.Count > 0)
                        dt = dt.AsEnumerable().OrderByDescending(row => DateTime.ParseExact(row.Field<string>("Birth Date"), "MM/dd/yyyy", CultureInfo.InvariantCulture)).CopyToDataTable();
                        var jsonData = JsonConvert.SerializeObject(dt);
                        ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServerQuarter(" + jsonData + ");", true);
                        
                    }
                    else
                    {
                        tdMsg.InnerHtml = "<div class='warning_box'>Please select birthdate quarter</div>";
                        ddlQuarter.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
                throw ex;
            }
        }

        public DataTable GetSelectedColumnQuarter(DataTable originalTable, string quarter)
        {
            for (int i = originalTable.Rows.Count - 1; i >= 0; i--)
            {
                if (originalTable.Rows[i]["mMonth"].ToString() != quarter)
                {
                    originalTable.Rows[i].Delete();
                }
            }
            originalTable.AcceptChanges();

            DataTable newTable = new DataTable();
            string[] selectedColumns = { "studentPersonalName", "BirthDate", "Age"};

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
            newTable = newTable
                                .AsEnumerable()
                                .GroupBy(row => string.Join("|", row.ItemArray))
                                .Select(group => group.First())
                                .CopyToDataTable();

            return newTable;
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;

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
                    btnShowReportVendor.Visible = false;
                    btnResetVendor.Visible = false;
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
                        DataTable dtFinalCopy = new DataTable();
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

                        dtFinalCopy = GetSelectedColumns(dt);
                        //DataTable dtActive = new DataTable();
                        //dtActive.Columns.Add("Status");
                        //dtActive.Rows.Add("Active");
                        //dtFinal = filterDataTable(dtFinal, dtActive);
                        string query = "SELECT ClassName AS 'Location',ClassId AS 'Location Id' FROM class WHERE ActiveInd='A' ORDER BY ClassName";
                        SqlCommand cmnd = new SqlCommand(query, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable ClassTbl = new DataTable();
                        sda = new SqlDataAdapter(cmnd);
                        sda.Fill(ClassTbl);
                        PopulateDropdown(dtFinalCopy,ClassTbl);
                        dtFinal = RemoveIdColumns(dtFinalCopy);
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
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
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
                cmd.Parameters.AddWithValue("@GetActiveID", "A,D");
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
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
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
        private void PopulateDropdown(DataTable dtFinal, DataTable ClassTbl)
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
                        sortedValues = ExtractLocationWithIdAsString(ClassTbl, "Location", "Location Id");
                    }
                    else if (column.ColumnName == "Student Name")
                    {
                        sortedValues = ExtractWithIdAsString(dt, "Student Name", "Student Id");
                    }
                    else if (column.ColumnName == "Race")
                    {
                        sortedValues = ExtractWithIdAsString(dt, "Race", "Race Id");
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

                    if (column.ColumnName != "Student Name" && column.ColumnName != "Location")
                    {
                        sortedValues.Sort();
                    }

                    if (column.ColumnName == "Status")
                    {
                        htmlBuilder.Append("<label><input type='checkbox' checked value='" + "Active" + "' class='filter-checkbox' data-column='" + column.ColumnName + "'> " + "Active" + "</label><br>");
                        htmlBuilder.Append("<label><input type='checkbox' value='" + "Discharged" + "' class='filter-checkbox' data-column='" + column.ColumnName + "'> " + "Discharged" + "</label><br>");
                    }
                    else
                    {
                        foreach (string value in sortedValues)
                        {
                            string[] parts = value.Split(new[] { '~' }, 2);
                            string firstPortion = parts.Length > 0 ? parts[0].Trim() : "";
                            string secondPortion = parts.Length > 1 ? parts[1].Trim() : "";
                            htmlBuilder.Append("<label><input type='checkbox' value='" + firstPortion + "' class='filter-checkbox' data-column='" + column.ColumnName + "'> " + secondPortion + "</label><br>");
                        }
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

            string[] selectedColumns = { "StudName", "StudentPersonalId", "Gender", "StudLanguage", "RaceName", "RaceID", "City", "StudState", "ClassName", "LocationID", "Program", "Placement_Type", "DepartmentName", "StudStatus" };

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

            if (newTable.Columns.Contains("StudentPersonalId"))
            {
                newTable.Columns["StudentPersonalId"].ColumnName = "Student Id";
            }
            
            if (newTable.Columns.Contains("StudLanguage"))
            {
                newTable.Columns["StudLanguage"].ColumnName = "Primary Language";
            } 
            
            if (newTable.Columns.Contains("RaceName"))
            {
                newTable.Columns["RaceName"].ColumnName = "Race";
            }
            if (newTable.Columns.Contains("RaceID"))
            {
                newTable.Columns["RaceID"].ColumnName = "Race Id";
            }
            
            if (newTable.Columns.Contains("StudState"))
            {
                newTable.Columns["StudState"].ColumnName = "State";
            }
            
            if (newTable.Columns.Contains("ClassName"))
            {
                newTable.Columns["ClassName"].ColumnName = "Location";
            }
            if (newTable.Columns.Contains("LocationID"))
            {
                newTable.Columns["LocationID"].ColumnName = "Location Id";
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
        public DataTable RemoveIdColumns(DataTable originalTable)
        {
            //To return only required columns for the table.
            DataTable newTable = new DataTable();

            string[] selectedColumns = { "Student Name", "Gender", "Primary Language", "Race", "City", "State", "Location", "Program", "Placement Type", "Department", "Status" };

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
        static List<string> ExtractLocationWithIdAsString(DataTable dt,string locationColumnName,string idColumnName)
        {
            var results = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in dt.Rows)
            {
                if (row.IsNull(locationColumnName) || row.IsNull(idColumnName))
                    continue;

                string rawLocation = row[locationColumnName].ToString().Trim();
                string rawId = row[idColumnName].ToString().Trim();

                if (rawLocation.Length == 0 || rawId.Length == 0)
                    continue;

                // Use simple concatenation instead of interpolation:
                string combined = rawId + "~" + rawLocation;
                // Or, if you prefer string.Format:
                // string combined = string.Format("{0}~{1}", rawId, rawLocation);

                results.Add(combined);
            }

            return results.ToList();
        }
        static List<string> ExtractWithIdAsString(DataTable dt, string valueColumnName, string idColumnName)
        {
            // Use a HashSet to avoid exact duplicate entries (ID:Value)
            var idValueSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in dt.Rows)
            {
                // Skip if either column is NULL or empty
                if (row[valueColumnName] == DBNull.Value || row[idColumnName] == DBNull.Value)
                    continue;

                string rawValue = row[valueColumnName].ToString().Trim();
                string rawId = row[idColumnName].ToString().Trim();

                if (string.IsNullOrEmpty(rawValue) || string.IsNullOrEmpty(rawId))
                    continue;

                // Combine "ID:Value"
                string combined = string.Format("{0}~{1}", rawId, rawValue);
                idValueSet.Add(combined);
            }

            return idValueSet.ToList();
        }
        static List<string> ExtractLocationFilter(DataTable dt, string columnName)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var results = new List<string>();
            var regex = new Regex(@":\s*([^:,]+(?:,\s*[^:,]+)*)");

            foreach (DataRow row in dt.Rows)
            {
                if (row["Location Id"] == DBNull.Value ||
                    row["Location"] == DBNull.Value)
                {
                    continue;
                }

                string idValue = row["Location Id"].ToString().Trim();
                string locationText = row["Location"].ToString();
                if (string.IsNullOrWhiteSpace(locationText))
                    continue;

                var matches = regex.Matches(locationText);
                foreach (Match match in matches)
                {
                    string[] roomNames = match.Groups[1].Value.Split(',');
                    foreach (string rawRoom in roomNames)
                    {
                        string room = rawRoom.Trim();
                        if (room.Length == 0)
                            continue;

                        string uniqueKey = idValue + "||" + room;
                        if (!seen.Contains(uniqueKey))
                        {
                            seen.Add(uniqueKey);
                            // Replace interpolated string with concatenation:
                            results.Add(idValue + ":" + room);
                        }
                    }
                }
            }

            // Sort by the room portion (after the colon):
            results.Sort((a, b) =>
            {
                var roomA = a.Substring(a.IndexOf(':') + 1);
                var roomB = b.Substring(b.IndexOf(':') + 1);
                return string.Compare(roomA, roomB, StringComparison.OrdinalIgnoreCase);
            });

            return results;
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
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
                    btnShowReportVendor.Visible = false;
                    btnResetVendor.Visible = false;
                    hdnMenu.Value = "btnClienContact";
                    RVClientReport.Visible = false;
                    HeadingDiv.Visible = true;
                    HeadingDiv.InnerHtml = "Emergency/Home Contact";
                    btnShowReport.Visible = false;


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
        " INNER JOIN LookUp LKP ON LKP.LookupId = PLC.Department " +
        " LEFT JOIN " +
        " (SELECT CP.ContactPersonalId,CP.LastName,CP.FirstName,CP.StudentPersonalId,AL.Phone,AL.Mobile,AL.OtherPhone  FROM " +
        "   [dbo].[ContactPersonal] CP " +
        " INNER JOIN [dbo].[StudentContactRelationship] SCR ON CP.ContactPersonalId=SCR.ContactPersonalId " +
        " INNER JOIN LookUp LP ON LP.LookupId=SCR.RelationshipId " +
        " INNER JOIN [dbo].[StudentAddresRel] SAR ON SAR.ContactPersonalId=CP.ContactPersonalId " +
        " INNER JOIN [dbo].[AddressList] AL ON AL.AddressId=SAR.AddressId " +
        " WHERE LP.LookupName='Emergency Contact' AND SAR.ContactSequence=1 AND CP.Status=1) EMERGENCYCONT ON SP.StudentPersonalId=EMERGENCYCONT.StudentPersonalId " +
        "  WHERE SP.StudentType='Client' and (PLC.EndDate is null or PLC.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 AND LKP.LookupType = 'Department' and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
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
        " INNER JOIN LookUp LKP ON LKP.LookupId = PLC.Department " +
        " INNER JOIN [dbo].[StudentContactRelationship] SCR ON CP.ContactPersonalId=SCR.ContactPersonalId		" +
        " INNER JOIN [dbo].[StudentAddresRel] SAR ON SAR.ContactPersonalId=CP.ContactPersonalId " +
        " INNER JOIN [dbo].[AddressList] AL ON AL.AddressId=SAR.AddressId  " +
        " WHERE (SP.StudentType = 'Client') and (PLC.EndDate is null or PLC.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 " +
        " and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
        " 							FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " +
        " 							WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " +
        " 							ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " +
        " 							WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 AND LKP.LOOKUPTYPE= 'Department'  and ST.StudentType='Client') " +
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
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
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
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
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
                    btnShowReportVendor.Visible = false;
                    btnResetVendor.Visible = false;
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
                                   "INNER JOIN LookUp LKP ON LKP.LookupId = PL.Department " +
                                   " WHERE SP.StudentType='Client' and (PL.EndDate is null or PL.EndDate >= cast (GETDATE() as DATE)) and PL.Status=1 AND LKP.LookupType ='Department' and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
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

        protected void btnOldVendor_Click(object sender, EventArgs e)
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
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
        protected void btnVendor_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkHighcharts.Checked)
                {
                    btnOldVendor_Click(sender, e);
                }
                else
                {
                    HContactStudname.Value = "All";
                    HContactstatus.Value = "0,1,2";
                    HContactRelation.Value = "All";

                    CheckBoxListcontact.Items[0].Selected = true;
                    CheckBoxListcontact.Items[1].Selected = false;

                    divbirthdate.Visible = false;
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
                    btnShowReportVendor.Visible = true;
                    btnResetVendor.Visible = true;
                    hdnMenu.Value = "btnVendor";
                    RVClientReport.SizeToReportContent = true;
                    tdMsg.InnerHtml = "";
                    RVClientReport.Visible = false;
                    HeadingDiv.Visible = true;
                    HeadingDiv.InnerHtml = "Client/Contact/Vendor";
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = null;
                    DataTable dt = new DataTable();
                    DataTable dtFinal = new DataTable();
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    cmd = new SqlCommand("clientcontactreport", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@HContactStudname", HContactStudname.Value);
                    cmd.Parameters.AddWithValue("@HContactstatus", HContactstatus.Value);
                    cmd.Parameters.AddWithValue("@HContactRelation", HContactRelation.Value);

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dtFinal = GetSelectedColumnsVendor(dt);
                    DataTable dtActive = new DataTable();
                    dtActive.Columns.Add("Status");
                    dtActive.Rows.Add("1");
                    dtFinal = filterDataTableVendor(dtFinal, dtActive);
                    PopulateDropdownVendor(dtFinal);
                    var jsonData = ConvertDataTableToJson(dtFinal);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServerVendor(" + jsonData + ");", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
                Logger.LogError("btnVendor_Click",ex);
            }
        }
        public DataTable GetSelectedColumnsVendor(DataTable originalTable)
        {
            //To return only required columns for the table.
            DataTable newTable = new DataTable();

            string[] selectedColumns = { "CLIENTLAST", "CLIENTFIRST", "DOB", "ADMISSIONDATE", "PROGRAM", "RELATIONSHIP", "CONTACTLAST", "CONTACTFIRST", "TYPE", "STREETNAME", "PHONE", "MOBILE", "ORGANIZATION", "OCCUPATION", "EMAIL", "EMERGENCY", "STATUS" };

            foreach (var columnName in selectedColumns)
            {
                if (originalTable.Columns.Contains(columnName))
                {
                    if (columnName == "DOB" || columnName == "ADMISSIONDATE")
                        newTable.Columns.Add(columnName, typeof(string));
                    else
                        newTable.Columns.Add(columnName, originalTable.Columns[columnName].DataType);
                }
            }

            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();

                foreach (var columnName in selectedColumns)
                {
                    if (columnName == "PROGRAM")
                    {
                        newRow[columnName] = "PROG : " + row[columnName].ToString() + "\nACTIVE : " + row["PLACEMENT"].ToString();
                    }
                    else if (columnName == "STREETNAME")
                    {
                        newRow[columnName] = row[columnName] + "\n" + row["FLOOR"].ToString() + "\n" + row["CITY"];
                    }
                    else if (columnName == "DOB" || columnName == "ADMISSIONDATE")
                    {
                        string input = row[columnName].ToString();
                        input = input.Replace("-", "/");
                        string[] formats = {
                                               "M/d/yyyy h:mm:ss tt",   // e.g. 4/14/1993 12:00:00 AM
                                               "MM/dd/yyyy h:mm:ss tt", // e.g. 04/14/1993 12:00:00 AM
                                               "dd/MM/yyyy HH:mm:ss",   // e.g. 15/05/2004 00:00:00
                                               "d/M/yyyy H:mm:ss"       // e.g. 5/6/2023 1:00:00
                                           };
                        DateTime parsedDate;
                        if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                        {
                            string formattedDate = parsedDate.ToString("MM/dd/yyyy");
                            newRow[columnName] = formattedDate;
                        }
                        else
                        {
                            Logger.LogError("Failed to parse date: " + input);
                        }
                    }
                    else
                        newRow[columnName] = row[columnName];
                }

                newTable.Rows.Add(newRow);
            }

            if (newTable.Columns.Contains("CLIENTLAST"))
            {
                newTable.Columns["CLIENTLAST"].ColumnName = "Client Last";
            }

            if (newTable.Columns.Contains("CLIENTFIRST"))
            {
                newTable.Columns["CLIENTFIRST"].ColumnName = "Client First";
            }

            if (newTable.Columns.Contains("DOB"))
            {
                newTable.Columns["DOB"].ColumnName = "Date of Birth";
            }

            if (newTable.Columns.Contains("ADMISSIONDATE"))
            {
                newTable.Columns["ADMISSIONDATE"].ColumnName = "Admission Date";
            }

            if (newTable.Columns.Contains("PROGRAM"))
            {
                newTable.Columns["PROGRAM"].ColumnName = "Program and Active Placement(s)";
            }

            if (newTable.Columns.Contains("RELATIONSHIP"))
            {
                newTable.Columns["RELATIONSHIP"].ColumnName = "Relationship";
            }

            if (newTable.Columns.Contains("CONTACTLAST"))
            {
                newTable.Columns["CONTACTLAST"].ColumnName = "Last";
            }

            if (newTable.Columns.Contains("CONTACTFIRST"))
            {
                newTable.Columns["CONTACTFIRST"].ColumnName = "First";
            }

            if (newTable.Columns.Contains("TYPE"))
            {
                newTable.Columns["TYPE"].ColumnName = "Type";
            }

            if (newTable.Columns.Contains("STREETNAME"))
            {
                newTable.Columns["STREETNAME"].ColumnName = "Street Address";
            }

            if (newTable.Columns.Contains("PHONE"))
            {
                newTable.Columns["PHONE"].ColumnName = "Phone";
            }

            if (newTable.Columns.Contains("MOBILE"))
            {
                newTable.Columns["MOBILE"].ColumnName = "Mobile";
            }

            if (newTable.Columns.Contains("ORGANIZATION"))
            {
                newTable.Columns["ORGANIZATION"].ColumnName = "Organization";
            }

            if (newTable.Columns.Contains("OCCUPATION"))
            {
                newTable.Columns["OCCUPATION"].ColumnName = "Occupation";
            }

            if (newTable.Columns.Contains("EMAIL"))
            {
                newTable.Columns["EMAIL"].ColumnName = "Email";
            }

            if (newTable.Columns.Contains("EMERGENCY"))
            {
                newTable.Columns["EMERGENCY"].ColumnName = "Emerg.Contact";
            }

            if (newTable.Columns.Contains("STATUS"))
            {
                newTable.Columns["STATUS"].ColumnName = "Status";
            }

            newTable.DefaultView.Sort = newTable.Columns["Client Last"].ColumnName + " ASC";
            newTable = newTable.DefaultView.ToTable();

            return newTable;
        }
        private void PopulateDropdownVendor(DataTable dtFinal)
        {
            //Populate dropdown menu for filtration
            DataTable dt = dtFinal.Copy();
            StringBuilder htmlBuilder = new StringBuilder();
            Literal dropdown = new Literal();

            for(int i=0;i< dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "Client Last" || dt.Columns[i].ColumnName == "Relationship" || dt.Columns[i].ColumnName == "Status")
                {
                    htmlBuilder.Append("<div class='dropdown'>");
                    if(dt.Columns[i].ColumnName == "Client Last")
                        htmlBuilder.Append("<button class='dropdown-btn'>Client &#9660</button>");
                    else if(dt.Columns[i].ColumnName == "Relationship")
                        htmlBuilder.Append("<button class='dropdown-btn'>Relationship &#9660</button>");
                    else if (dt.Columns[i].ColumnName == "Status")
                        htmlBuilder.Append("<button class='dropdown-btn'>Status &#9660</button>");


                    htmlBuilder.Append("<div class='dropdown-content'>");

                    HashSet<string> uniqueValues = new HashSet<string>();
                    List<string> sortedValues = new List<string>();
                        foreach (DataRow row in dt.Rows)
                        {
                            string value = "";
                            if(dt.Columns[i].ColumnName == "Client Last")
                                value = row[dt.Columns[i]].ToString() + " " + row["Client First"].ToString();
                            else
                                value = row[dt.Columns[i]].ToString();

                            if (!uniqueValues.Contains(value) && !string.IsNullOrWhiteSpace(value))
                            {
                                uniqueValues.Add(value);
                            }
                        }
                    sortedValues = uniqueValues.ToList();
                    sortedValues.Sort();

                    foreach (string value in sortedValues)
                    {
                        if (dt.Columns[i].ColumnName == "Client Last")
                            htmlBuilder.Append("<label><input type='checkbox' value='" + value + "' class='filter-checkbox' data-column='Client'> " + value + "</label><br>");
                        else if (dt.Columns[i].ColumnName == "Relationship")
                            htmlBuilder.Append("<label><input type='checkbox' value='" + value + "' class='filter-checkbox' data-column='Relationship'> " + value + "</label><br>");
                    }
                    if (dt.Columns[i].ColumnName == "Status")
                    {
                        htmlBuilder.Append("<label><input type='checkbox' value='1' class='filter-checkbox' data-column='Status'>Active</label><br>");
                        htmlBuilder.Append("<label><input type='checkbox' value='2' class='filter-checkbox' data-column='Status'>Discharged</label><br>");
                    }
                    htmlBuilder.Append("</div>");
                    htmlBuilder.Append("</div>");
                }
                dropdown.Text = htmlBuilder.ToString();
                dropdown_container.Controls.Add(dropdown);
            }
        }
        public static DataTable filterDataTableVendor(DataTable fullData, DataTable selectedData)
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
                dt.Rows.Add("1");
                return filterDataTable(filteredData, dt);
            }
        }
        [WebMethod]
        public static string CreateDataTableFromSelectedValuesVendor(Dictionary<string, List<string>> selectedValues)
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
                if (dt.Columns.Contains("Client"))
                {
                    dt.Columns.Add("Client Last", typeof(string));
                    dt.Columns.Add("Client First", typeof(string));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Client Last"] = dr["Client"].ToString().Split(' ').First();
                        dr["Client First"] = dr["Client"].ToString().Split(' ').Last();
                    }
                    dt.Columns.Remove("Client");
                }
                ClientReports clientReportsInstance = new ClientReports();
                DataTable dtFinal = clientReportsInstance.getVendorReport(dt);
                string jsonData = clientReportsInstance.ConvertDataTableToJson(dtFinal);
                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataTable getVendorReport(DataTable dataTbl)
        {
            try
            {
                string HContactStudname_Value = "All";
                string HContactstatus_Value = "0,1,2";
                string HContactRelation_Value = "All";
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = null;
                DataTable dt = new DataTable();
                DataTable dtFinal = new DataTable();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                con.Open();
                cmd = new SqlCommand("clientcontactreport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HContactStudname", HContactStudname_Value);
                cmd.Parameters.AddWithValue("@HContactstatus", HContactstatus_Value);
                cmd.Parameters.AddWithValue("@HContactRelation", HContactRelation_Value);

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dtFinal = GetSelectedColumnsVendor(dt);
                if (dataTbl.Rows.Count == 0) // No filter
                {
                    DataTable dtActive = new DataTable();
                    dtActive.Columns.Add("Status");
                    dtActive.Rows.Add("1");
                    dtFinal = filterDataTableVendor(dtFinal, dtActive);
                    return dtFinal;
                }
                else
                    return filterDataTableVendor(dtFinal, dataTbl); //Filter present
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;

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

        protected void btnOldResRoster_Click(object sender, EventArgs e)
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
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
        protected void btnResRoster_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkHighcharts.Checked)
                {
                    btnOldResRoster_Click(sender, e);
                }
                else
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
                    btnShowReportVendor.Visible = false;
                    btnResetVendor.Visible = false;
                    hdnMenu.Value = "btnResRoster";
                    RVClientReport.SizeToReportContent = false;
                    tdMsg.InnerHtml = "";
                    RVClientReport.Visible = false;
                    HeadingDiv.Visible = true;
                    HeadingDiv.InnerHtml = "Residential Roster Report";
                    RVClientReport.Visible = false;
                    int Schoolid = 0;
                    string schooltype = ConfigurationManager.AppSettings["Server"];
                    if (schooltype == "NE")
                        Schoolid = 1;
                    else
                        Schoolid = 2;
                    string resRosterQuery = "SELECT  (select ClassName from Class where Classid = PL.Location) as Location " +
                                    " ,SP.StudentPersonalId ,SP.SchoolId ,SP.LastName+','+SP.FirstName AS studentPersonalName " + 
		                            " ,CONVERT(VARCHAR(10), SP.[BirthDate], 101) AS BirthDate	 " + 
		                            " ,DATEDIFF(YEAR,SP.BirthDate,GETDATE()) - (CASE WHEN DATEADD(YY,DATEDIFF(YEAR,SP.BirthDate,GETDATE()),SP.BirthDate) >  GETDATE() THEN 1 ELSE 0 END) AS Age " + 
		                            " ,CASE WHEN DATEPART(MM,SP.BirthDate)>= 01 AND DATEPART(MM,SP.BirthDate)<= 03 THEN 1 ELSE  " + 
		                            " CASE WHEN DATEPART(MM,SP.BirthDate)>= 04 AND DATEPART(MM,SP.BirthDate)<= 06 THEN 2 ELSE " + 
		                            " CASE WHEN DATEPART(MM,SP.BirthDate)>= 07 AND DATEPART(MM,SP.BirthDate)<= 09 THEN 3 ELSE 4 END END END AS mMonth " + 
		                            " ,CASE WHEN SP.Gender=1 THEN 'Male'	ELSE 'Female'	END Gender ,LP.LookupName AS PlacementType " + 
		                            " ,CONVERT(VARCHAR(10),PL.StartDate,101) AS StartDate ,CONVERT(VARCHAR(10),PL.EndDate,101) AS EndDate " + 
		                            " ,(SELECT LookupName FROM LookUp WHERE LookupId=PL.Department) AS Department " + 
		                            " ,(SELECT LookupName FROM LookUp WHERE LookupId=PL.BehaviorAnalyst) AS BehaviorAnalyst " + 
		                            " FROM StudentPersonal SP LEFT JOIN [dbo].[Placement] PL ON SP.StudentPersonalId=PL.StudentPersonalId  " + 
                                    " INNER JOIN LookUp LKP ON LKP.LookupId = PL.Department " +
                                    " INNER JOIN LookUp LP ON LP.LookupId=PL.PlacementType " +
                                    " INNER JOIN (SELECT * FROM Class WHERE ResidenceInd = 1) CL ON	CL.ClassId = PL.Location " +
		                            " WHERE SP.StudentType='Client' and (PL.EndDate is null or PL.EndDate >= cast (GETDATE() as DATE)) and PL.Status=1 AND LKP.LookupType = 'Department' and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " + 
                                    " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " + 
                                    " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " + 
                                    " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " + 
                                    " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " + 
                                    " and ST.StudentPersonalId not in (SELECT Distinct " + 
                                    " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) AND CONVERT(INT,SP.ClientId)>0";

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(resRosterQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnResRoster(dt);
                    dt.DefaultView.Sort = dt.Columns["Location"].ColumnName + " ASC";
                    dt = dt.DefaultView.ToTable();
                    var jsonData = JsonConvert.SerializeObject(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServerQuarter(" + jsonData + ");", true);
                    divbirthdate.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
                throw ex;
            }
        }
        public DataTable GetSelectedColumnResRoster(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns = { "Location", "studentPersonalName", "BirthDate", "Age", "Gender", "StartDate", "EndDate", "Department", "BehaviorAnalyst" };

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

            if (newTable.Columns.Contains("StartDate"))
            {
                newTable.Columns["StartDate"].ColumnName = "Start Date";
            }

            if (newTable.Columns.Contains("EndDate"))
            {
                newTable.Columns["EndDate"].ColumnName = "End Date";
            }

            if (newTable.Columns.Contains("Department"))
            {
                newTable.Columns["Department"].ColumnName = "Program";
            }

            if (newTable.Columns.Contains("BehaviorAnalyst"))
            {
                newTable.Columns["BehaviorAnalyst"].ColumnName = "Behavior Analyst";
            }

            return newTable;
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
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
                if (checkHighcharts.Checked)
                {
                    RVClientReport.Visible = false;
                    string placementQuery = " SELECT *,CASE WHEN PLCStatus='New Admission' OR PLCStatus='Re-Admission' THEN 'New Placement' ELSE CASE WHEN PLCStatus='Respite' OR PLCStatus='Move' OR PLCStatus='Partial Discharge' THEN 'Active Placement' ELSE CASE WHEN PLCStatus='Discharge' THEN 'Discharged Placement' END END " +
                                            " END AS PlacementStatus ,(SELECT STUFF(ISNULL((SELECT ', ' + DATA " +
                                            " FROM  " +
                                            " [Split] (IsDays,',') WHERE DATA<>'0' " +
                                            " FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '')) Days FROM (SELECT SP.SchoolId,PT.StudentPersonalId,SP.ClientId,SP.LastName+','+SP.FirstName AS ClientName,PT.BehaviorAnalyst,PT.Department " +
                                            " ,(SELECT LookupName FROM LookUp WHERE LookupId=PT.Department) AS DepartmentName,CONVERT(VARCHAR(20),PT.EndDate,101) EndDate,CONVERT(DATE,PT.EndDate) EdDate,PT.Location " +
                                            " ,(SELECT ClassName FROM Class WHERE ClassId=PT.Location) AS LocationName ,PT.PlacementDepartment " +
                                            " ,(SELECT LookupName FROM LookUp WHERE LookupId=PT.PlacementDepartment) AS PlacementDepartmentName " +
                                            " ,PT.PlacementReason,(SELECT LookupName FROM LookUp WHERE LookupId=PT.PlacementReason) AS PlacementReasonName  " +
                                            " ,PT.PlacementType,(SELECT LookupName FROM LookUp WHERE LookupId=PT.PlacementType) AS PlacementTypeName " +
                                            " ,PT.Reason,CONVERT(VARCHAR(20),PT.StartDate,101) StartDate,CONVERT(DATE,PT.StartDate) StDate,(SELECT LookupName FROM LookUp WHERE LookupId=PT.PlacementReason) PLCStatus " +
                                            " ,CONVERT(DATE,PT.CreatedOn) CreatedBy,(CASE WHEN PT.IsMonday=1 THEN 'Monday' ELSE '0' END +','+CASE WHEN PT.IsTuesday=1 THEN 'Tuesday' ELSE '0' END +','+ " +
                                            " CASE WHEN PT.IsWednesday=1 THEN 'Wednesday' ELSE '0' END +','+CASE WHEN PT.IsThursday=1 THEN 'Thursday' ELSE '0' END +','+CASE WHEN PT.IsFriday=1 THEN 'Friday' ELSE '0' END +','+ " +
                                            " CASE WHEN PT.IsSaturday=1 THEN 'Saturday' ELSE '0' END +','+CASE WHEN PT.IsSunday=1 THEN 'Sunday' ELSE '0' END ) IsDays " +
                                            " FROM Placement PT LEFT JOIN StudentPersonal SP " +
                                            " ON PT.StudentPersonalId=SP.StudentPersonalId WHERE  " +
                                            " SP.StudentType='Client'    " +
                                            " AND SP.ClientId IS NOT NULL  AND CONVERT(INT,SP.ClientId)>0  " +
                                            " AND PT.Status=1 " +
                                            " AND (CONVERT(DATE,PT.EndDate)>CONVERT(DATE,GETDATE()) OR PT.EndDate IS NULL)  " +
                                            " AND PT.PlacementReason IS NOT NULL) PLACE  ORDER BY ClientId ";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(placementQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnPlacement(dt);
                    dt.DefaultView.Sort = dt.Columns["Client Id"].ColumnName + " ASC";
                    dt = dt.DefaultView.ToTable();
                    var jsonData = JsonConvert.SerializeObject(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServerPlacement(" + jsonData + ");", true);

                }
                else
                {
                    RVClientReport.Visible = true;
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
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
                throw ex;
            }
        }
        public DataTable GetSelectedColumnPlacement(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns = { "ClientId", "ClientName", "PlacementDepartmentName", "PlacementTypeName", "DepartmentName", "LocationName", "PlacementReasonName", "StartDate",  "EndDate", "Days" };

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

            if (newTable.Columns.Contains("ClientId"))
            {
                newTable.Columns["ClientId"].ColumnName = "Client Id";
            }

            if (newTable.Columns.Contains("ClientName"))
            {
                newTable.Columns["ClientName"].ColumnName = "Client Name";
            }

            if (newTable.Columns.Contains("PlacementDepartmentName"))
            {
                newTable.Columns["PlacementDepartmentName"].ColumnName = "Department";
            }

            if (newTable.Columns.Contains("PlacementTypeName"))
            {
                newTable.Columns["PlacementTypeName"].ColumnName = "Placement Type";
            }

            if (newTable.Columns.Contains("PlacementTypeName"))
            {
                newTable.Columns["DepartmentName"].ColumnName = "Program";
            }

            if (newTable.Columns.Contains("LocationName"))
            {
                newTable.Columns["LocationName"].ColumnName = "Location";
            }

            if (newTable.Columns.Contains("PlacementReasonName"))
            {
                newTable.Columns["PlacementReasonName"].ColumnName = "Placement Reason";
            }

            if (newTable.Columns.Contains("StartDate"))
            {
                newTable.Columns["StartDate"].ColumnName = "Start Date";
            }

            if (newTable.Columns.Contains("EndDate"))
            {
                newTable.Columns["EndDate"].ColumnName = "End Date";
            }

            return newTable;
        }
        protected void btnOldAllFunder_Click(object sender, EventArgs e)
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
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

        protected void btnAllFunder_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkHighcharts.Checked)
                {
                    btnOldAllFunder_Click(sender, e);
                }
                else
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
                    btnShowReportVendor.Visible = false;
                    btnResetVendor.Visible = false;
                    hdnMenu.Value = "btnAllFunder";
                    RVClientReport.SizeToReportContent = false;
                    tdMsg.InnerHtml = "";
                    RVClientReport.Visible = false;
                    HeadingDiv.Visible = true;
                    HeadingDiv.InnerHtml = "All Clients by Funder";
                    RVClientReport.Visible = false;
                    int Schoolid = 0;
                    string schooltype = ConfigurationManager.AppSettings["Server"];
                    if (schooltype == "NE")
                        Schoolid = 1;
                    else
                        Schoolid = 2;
                    divbirthdate.Visible = false;
                    string funderQuery = "SELECT SPA.FundingSource,SP.LastName+','+SP.FirstName AS ClientName,SP.ClientId,SP.SchoolId FROM StudentPersonal SP INNER JOIN StudentPersonalPA SPA ON SP.StudentPersonalId=SPA.StudentPersonalId " +
                                         " WHERE SPA.FundingSource IS NOT NULL AND SPA.FundingSource<>'' AND SP.StudentType='Client' AND CONVERT(INT,SP.ClientId)>0  ORDER BY SPA.FundingSource,SP.ClientId";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(funderQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnFunder(dt);
                    dt.DefaultView.Sort = dt.Columns["Funder"].ColumnName + " ASC";
                    dt = dt.DefaultView.ToTable();
                    var jsonData = JsonConvert.SerializeObject(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServerFunder(" + jsonData + ");", true);


                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
                throw ex;
            }
        }
        public DataTable GetSelectedColumnFunder(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns = { "FundingSource", "ClientName", "ClientId"};

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

            if (newTable.Columns.Contains("FundingSource"))
            {
                newTable.Columns["FundingSource"].ColumnName = "Funder";
            }

            if (newTable.Columns.Contains("ClientName"))
            {
                newTable.Columns["ClientName"].ColumnName = "ClientName";
            }

            if (newTable.Columns.Contains("ClientId"))
            {
                newTable.Columns["ClientId"].ColumnName = "ClientId";
            }

            return newTable;
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
                int Schoolid = 0;
                string schooltype = ConfigurationManager.AppSettings["Server"];
                if (schooltype == "NE")
                    Schoolid = 1;
                else
                    Schoolid = 2;
                if (!checkHighcharts.Checked)
                {
                    RVClientReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["FunderReport"];
                    RVClientReport.ShowParameterPrompts = false;
                    ReportParameter[] parm = new ReportParameter[2];
                    parm[0] = new ReportParameter("SchoolID", Schoolid.ToString());
                    parm[1] = new ReportParameter("FundingSource", ddlFundingSource.SelectedValue.ToString());
                    this.RVClientReport.ServerReport.SetParameters(parm);
                    RVClientReport.ServerReport.Refresh();
                }
                else
                {
                    string funderQuery = "SELECT SPA.FundingSource,SP.LastName+','+SP.FirstName AS ClientName,SP.ClientId,SP.SchoolId FROM StudentPersonal SP INNER JOIN StudentPersonalPA SPA ON SP.StudentPersonalId=SPA.StudentPersonalId " +
                                         " WHERE SPA.FundingSource IS NOT NULL AND SPA.FundingSource<>'' AND SP.StudentType='Client' AND SP.PlacementStatus<>'I' AND CONVERT(INT,SP.ClientId)>0  ORDER BY SPA.FundingSource,SP.ClientId";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(funderQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (ddlFundingSource.SelectedValue.ToString() != "0")
                    {
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dt.Rows[i]["FundingSource"].ToString() != ddlFundingSource.SelectedValue.ToString())
                            {
                                dt.Rows[i].Delete();
                            }
                        }
                        dt.AcceptChanges();
                    }
                    dt = GetSelectedColumnFunder(dt);
                    dt.DefaultView.Sort = dt.Columns["Funder"].ColumnName + " ASC";
                    dt = dt.DefaultView.ToTable();
                    var jsonData = JsonConvert.SerializeObject(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServerFunder(" + jsonData + ");", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
                hdnMenu.Value = "btnAllBirthdate";
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients by Birthdate";
                string BithdateStart = (txtBithdateStart.Text != "" ? GetDateFromText(txtBithdateStart.Text) : "");
                string BirthdateEnd = (txtBirthdateEnd.Text != "" ? GetDateFromText(txtBirthdateEnd.Text) : "");
                if (checkHighcharts.Checked)
                {
                    RVClientReport.Visible = false;
                    ddlMonth.SelectedItem.Value = "0";
                    txtAgeTo.Text = txtAgeFrom.Text = txtBithdateStart.Text = txtBirthdateEnd.Text = "";
                    string birthdateQuery = "SELECT distinct ClientId,Lastname,Firstname ,CONVERT(VARCHAR(20),BirthDate,101) AS BirthDate,BirthDate AS BDate " +
                    " ,DATEDIFF(YEAR,BirthDate,GETDATE())-(CASE WHEN DATEADD(YY,DATEDIFF(YEAR,BirthDate,GETDATE()),BirthDate) > GETDATE() THEN 1 " +
                    " ELSE 0 END) AS Age, DATENAME(month,BirthDate) Month FROM StudentPersonal ST  " +
                    " JOIN Placement PLC ON PLC.StudentPersonalId = ST.StudentPersonalId " +
                    " INNER JOIN LookUp LKP ON LKP.LookupId = PLC.Department " +
                    " WHERE StudentType='Client' and (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and " +
                    " LKP.LookupType = 'Department' AND " +
                    " ST.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
                    " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " +
                    " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " +
                    " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " +
                    " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " +
                    " and ST.StudentPersonalId not in (SELECT Distinct " +
                    " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) AND ClientId IS NOT NULL AND ClientId<>'' AND CONVERT(INT,ClientId)>0  ORDER BY Lastname";

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(birthdateQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnsBirthdate(dt);
                    dt.DefaultView.Sort = dt.Columns["Last Name"].ColumnName + " ASC";
                    dt = dt.DefaultView.ToTable();


                    string jsonData = ConvertDataTableToJson(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerBirthdate(" + jsonData + ");", true);
                }
                else
                {
                    RVClientReport.Visible = true;
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
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
                throw ex;
            }
        }
        public DataTable GetSelectedColumnsBirthdate(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns = { "ClientId", "Lastname", "Firstname", "BirthDate", "Age"};

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

            if (newTable.Columns.Contains("ClientId"))
            {
                newTable.Columns["ClientId"].ColumnName = "Client Id";
            }

            if (newTable.Columns.Contains("Lastname"))
            {
                newTable.Columns["Lastname"].ColumnName = "Last Name";
            }

            if (newTable.Columns.Contains("Firstname"))
            {
                newTable.Columns["Firstname"].ColumnName = "First Name";
            }

            if (newTable.Columns.Contains("BirthDate"))
            {
                newTable.Columns["BirthDate"].ColumnName = "Birth Date";
            }

            return newTable;
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
                hdnMenu.Value = "btnAllAdmissionDate";
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients by Admission date";
                if (checkHighcharts.Checked)
                {
                    string admissionQuery = "SELECT distinct ClientId,Lastname,Firstname,CONVERT(VARCHAR(20),AdmissionDate,101) AS AdmDate,AdmissionDate FROM StudentPersonal ST " +
                            " JOIN Placement PLC on PLC.StudentPersonalId = ST.StudentPersonalId " +
                            " WHERE StudentType='Client' and (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1  " +
                            " and ST.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " + 
                            " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " +
                            " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " +
                            " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " +
                            " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " +
                            " and ST.StudentPersonalId not in (SELECT Distinct " +
                            " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) AND CONVERT(INT,ClientId)>0 ORDER BY AdmissionDate DESC ";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(admissionQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnsAdmissionDate(dt);
                    if (dt.Rows.Count > 0)
                    dt = dt.AsEnumerable().OrderByDescending(row => DateTime.ParseExact(row.Field<string>("Admission Date"), "MM/dd/yyyy", CultureInfo.InvariantCulture)).CopyToDataTable();

                    string jsonData = ConvertDataTableToJson(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerBirthdate(" + jsonData + ");", true);
                }
                else
                {
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
                }
                    divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
                throw ex;
            }
        }
        public DataTable GetSelectedColumnsAdmissionDate(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns = { "ClientId", "Lastname", "Firstname", "AdmDate"};

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

            if (newTable.Columns.Contains("ClientId"))
            {
                newTable.Columns["ClientId"].ColumnName = "Client Id";
            }

            if (newTable.Columns.Contains("Lastname"))
            {
                newTable.Columns["Lastname"].ColumnName = "Last Name";
            }

            if (newTable.Columns.Contains("Firstname"))
            {
                newTable.Columns["Firstname"].ColumnName = "First Name";
            }

            if (newTable.Columns.Contains("AdmDate"))
            {
                newTable.Columns["AdmDate"].ColumnName = "Admission Date";
            }

            return newTable;
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
                hdnMenu.Value = "btnAllDischargedate";
                RVClientReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Clients by Discharge date";
                if (checkHighcharts.Checked)
                {
                    string dischargeQuery = "SELECT PA.ClientId,PA.Lastname,PA.Firstname,PA.AdmissionDate,CONVERT(VARCHAR(20),PA.AdmissionDate,101) AS ADate,PA.DischargeDate AS SPDischargeDate " + 
                    " ,PL.EndDate AS PLDischargeDate,CONVERT(VARCHAR(20),PL.EndDate,101) EndDate FROM Placement PL INNER JOIN StudentPersonal PA ON PL.StudentPersonalId=PA.StudentPersonalId INNER JOIN Class CLS ON PL.Location = CLS.ClassId WHERE  " + 
                    " PA.PlacementStatus = 'D' and CLS.ClassCd = 'DSCH' " + 
                    " AND PA.ClientId<>'' AND PL.Status=1 AND CONVERT(INT,PA.ClientId)>0  ORDER BY PL.EndDate ";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(dischargeQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnsDischargeDate(dt);
                    if (dt.Rows.Count > 0)
                        dt = dt.AsEnumerable().OrderBy(row => DateTime.ParseExact(row.Field<string>("Discharge Date"), "MM/dd/yyyy", CultureInfo.InvariantCulture)).CopyToDataTable();

                    string jsonData = ConvertDataTableToJson(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerBirthdate(" + jsonData + ");", true);

                }
                else
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
                divbirthdate.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected DataTable GetSelectedColumnsDischargeDate(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns = { "ClientId", "Lastname", "Firstname", "ADate", "EndDate" };

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

            if (newTable.Columns.Contains("ClientId"))
            {
                newTable.Columns["ClientId"].ColumnName = "Client Id";
            }

            if (newTable.Columns.Contains("Lastname"))
            {
                newTable.Columns["Lastname"].ColumnName = "Last Name";
            }

            if (newTable.Columns.Contains("Firstname"))
            {
                newTable.Columns["Firstname"].ColumnName = "First Name";
            }

            if (newTable.Columns.Contains("ADate"))
            {
                newTable.Columns["ADate"].ColumnName = "Admission Date";
            }

            if (newTable.Columns.Contains("EndDate"))
            {
                newTable.Columns["EndDate"].ColumnName = "Discharge Date";
            }

            return newTable;
 
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
                btnShowReportVendor.Visible = false;
                btnResetVendor.Visible = false;
                hdnMenu.Value = "btnStatistical";
                tdMsg.InnerHtml = "";
                RVClientReport.Visible = false;
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "Statistical Report";
                divbirthdate.Visible = false;
                if (checkHighcharts.Checked)
                {
                    divStatistical.Visible = false;
                    string statisticalQuery = "SELECT Location,(SELECT ClassName FROM Class WHERE ClassId=Location) ClassName,MaxStudents,COUNT( CASE WHEN Gender='Male' " + 
                   " THEN 1 END ) AS Male,COUNT( CASE WHEN Gender='Female' " + 
                   " THEN 1 END ) AS Female,COUNT(StudentPersonalId) AS TotalStudents,Pgm,(SELECT LookupName FROM LookUp WHERE LookupId=Pgm) Program, " + 
				   " PlacementType AS PlacementTypeId,(SELECT LookupName FROM LookUp WHERE LookupId=PlacementType) Placement_Type " + 
				   " ,Departmt,(SELECT LookupName FROM LookUp WHERE LookupId=Departmt) AS DepartmentName,RaceId, " + 
				   " (SELECT LookupName FROM LookUp WHERE LookupId=RaceId) RaceName FROM (SELECT SL.StudentPersonalId,CASE WHEN SL.Gender=1 THEN 'Male' ELSE 'Female' END Gender,PT.Location,(SELECT MaxStudents FROM Class  " +
                   " WHERE ClassId=PT.Location) MaxStudents,PT.Department AS Pgm,PT.PlacementType,PT.PlacementDepartment AS Departmt,SL.RaceId " +
                   " FROM Placement PT INNER JOIN " +
                   " StudentPersonal SL ON PT.StudentPersonalId=SL.StudentPersonalId INNER JOIN LookUp LKP ON LKP.LookupId = PT.Department WHERE PT.Status=1 AND SL.StudentType='Client' and (PT.EndDate is null or PT.EndDate >= cast (GETDATE() as DATE)) and PT.Status=1  " +
                   " and SL.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
                   " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " +
                   " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " +
                   " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " +
                   " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 AND LKP.LookupType = 'Department' and ST.StudentType='Client') " +
                   " and ST.StudentPersonalId not in (SELECT Distinct " +
                   " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) AND CONVERT(INT,SL.ClientId)>0) SLPT  " +
                   " WHERE Location IS NOT NULL " +
                   " GROUP BY Location,MaxStudents,Pgm,PlacementType,Departmt,RaceId  ORDER BY Location ";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(statisticalQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnsStatistical(dt);

                    string jsonData = ConvertDataTableToJson(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerStatistical(" + jsonData + ");", true);

                }
                else
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected DataTable GetSelectedColumnsStatistical(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns = { "ClassName", "MaxStudents", "RaceName", "Placement_Type", "DepartmentName", "Program", "TotalStudents", "Male", "Female"};

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

            if (newTable.Columns.Contains("ClassName"))
            {
                newTable.Columns["ClassName"].ColumnName = "Location";
            }

            if (newTable.Columns.Contains("MaxStudents"))
            {
                newTable.Columns["MaxStudents"].ColumnName = "Maximum Client Occupancy";
            }

            if (newTable.Columns.Contains("DepartmentName"))
            {
                newTable.Columns["DepartmentName"].ColumnName = "Department";
            }

            if (newTable.Columns.Contains("TotalStudents"))
            {
                newTable.Columns["TotalStudents"].ColumnName = "Total Students";
            }

            if (newTable.Columns.Contains("Placement_Type"))
            {
                newTable.Columns["Placement_Type"].ColumnName = "Placement Type";
            }

            if (newTable.Columns.Contains("RaceName"))
            {
                newTable.Columns["RaceName"].ColumnName = "Race";
            }

            if (newTable.Columns.Contains("Male"))
            {
                newTable.Columns["Male"].ColumnName = "Male Count";
            }

            if (newTable.Columns.Contains("Female"))
            {
                newTable.Columns["Female"].ColumnName = "Female Count";
            }

            return newTable;

        }
        protected void btnShowBirthdate_Click(object sender, EventArgs e)
        {
            try
            {
                divContact.Visible = false;
                if (checkHighcharts.Checked)
                {
                    RVClientReport.Visible = false;

                    string birthdateQuery = "SELECT distinct ClientId,Lastname,Firstname ,CONVERT(VARCHAR(20),BirthDate,101) AS BirthDate,BirthDate AS BDate " +
                    " ,DATEDIFF(YEAR,BirthDate,GETDATE())-(CASE WHEN DATEADD(YY,DATEDIFF(YEAR,BirthDate,GETDATE()),BirthDate) > GETDATE() THEN 1 " +
                    " ELSE 0 END) AS Age, DATENAME(month,BirthDate) Month FROM StudentPersonal ST  " +
                    " JOIN Placement PLC ON PLC.StudentPersonalId = ST.StudentPersonalId " +
                    " INNER JOIN LookUp LKP ON LKP.LookupId = PLC.Department " +
                    " WHERE StudentType='Client' and (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and " +
                    " LKP.LookupType = 'Department' AND " +
                    " ST.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
                    " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " +
                    " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " +
                    " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " +
                    " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " +
                    " and ST.StudentPersonalId not in (SELECT Distinct " +
                    " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) AND ClientId IS NOT NULL AND ClientId<>'' AND CONVERT(INT,ClientId)>0  ORDER BY Lastname";

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(birthdateQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    dt = FilterTableBirthdate(dt);
                    dt = GetSelectedColumnsBirthdate(dt);
                    dt.DefaultView.Sort = dt.Columns["Last Name"].ColumnName + " ASC";
                    dt = dt.DefaultView.ToTable();


                    string jsonData = ConvertDataTableToJson(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerBirthdate(" + jsonData + ");", true);
                }
                else
                {
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
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
                throw ex;
            }
        }

        public DataTable FilterTableBirthdate(DataTable originalTable)
        {
            string month = ddlMonth.SelectedItem.Value.ToString();
            int ageFrom = string.IsNullOrWhiteSpace(txtAgeFrom.Text) ? 0 : Convert.ToInt32(txtAgeFrom.Text);
            int ageTo = string.IsNullOrWhiteSpace(txtAgeTo.Text) ? 200 : Convert.ToInt32(txtAgeTo.Text);
            string bDateS = txtBithdateStart.Text == "" ? "01/01/1900" : txtBithdateStart.Text;
            string bDateE = txtBirthdateEnd.Text == "" ? DateTime.Now.ToString("MM/dd/yyyy") : txtBirthdateEnd.Text;
            bDateS = bDateS.Replace("-", "/");
            bDateE = bDateE.Replace("-", "/");

            DateTime bDateStart = DateTime.ParseExact(bDateS,"MM/dd/yyyy",CultureInfo.InvariantCulture);
            DateTime bDateEnd = DateTime.ParseExact(bDateE,"MM/dd/yyyy",CultureInfo.InvariantCulture);


            DataTable filteredTable = originalTable.Clone();

            foreach (DataRow row in originalTable.Rows)
            {
                DateTime birthDate = DateTime.ParseExact(
                string.IsNullOrWhiteSpace(row["BirthDate"].ToString()) ? DateTime.Now.ToString("MM/dd/yyyy") : row["BirthDate"].ToString(),
                "MM/dd/yyyy",
                CultureInfo.InvariantCulture
                );;
                int age;

                if (!int.TryParse(row["Age"] != null ? row["Age"].ToString() : "", out age))
                    continue;

                // Filter by month
                if (!string.IsNullOrWhiteSpace(month) && month != "0")
                {
                    string rowMonth = row["Month"].ToString();
                    if (!rowMonth.Equals(month, StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                // Filter by age range
                if (age < ageFrom || age > ageTo)
                    continue;

                // Filter by birth date range
                if (birthDate < bDateStart || birthDate > bDateEnd)
                    continue;

                filteredTable.ImportRow(row);
            }

            return filteredTable;
        }


        protected void btnShowAdmissionDate_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkHighcharts.Checked)
                {
                    RVClientReport.Visible = false;
                    string admissionQuery = "SELECT distinct ClientId,Lastname,Firstname,CONVERT(VARCHAR(20),AdmissionDate,101) AS AdmDate,AdmissionDate FROM StudentPersonal ST " +
                            " JOIN Placement PLC on PLC.StudentPersonalId = ST.StudentPersonalId " +
                            " WHERE StudentType='Client' and (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1  " +
                            " and ST.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " +
                            " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " +
                            " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " +
                            " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " +
                            " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " +
                            " and ST.StudentPersonalId not in (SELECT Distinct " +
                            " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) AND CONVERT(INT,ClientId)>0 ORDER BY AdmissionDate DESC ";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(admissionQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = FilterTableAdmissionDate(dt);
                    dt = GetSelectedColumnsAdmissionDate(dt);
                    if(dt.Rows.Count>0)
                    dt = dt.AsEnumerable().OrderByDescending(row => DateTime.ParseExact(row.Field<string>("Admission Date"), "MM/dd/yyyy", CultureInfo.InvariantCulture)).CopyToDataTable();

                    string jsonData = ConvertDataTableToJson(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerBirthdate(" + jsonData + ");", true);
                }
                else
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
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoaderScript", "hideLoader()", true);
                throw ex;
            }
        }
        public DataTable FilterTableAdmissionDate(DataTable originalTable)
        {
            int count = Convert.ToInt32(txtNumberOfAdmission.Text == "" ? "-1" : txtNumberOfAdmission.Text);
            string admDateFrom = txtAdmissionFrom.Text == "" ? "01/01/1900" : txtAdmissionFrom.Text;
            string admDateTo = txtAdmissionTo.Text == "" ? DateTime.Now.ToString("MM/dd/yyyy") : txtAdmissionTo.Text;
            admDateFrom = admDateFrom.Replace("-", "/");
            admDateTo = admDateTo.Replace("-", "/");

            DateTime admissionStart = DateTime.ParseExact(admDateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime admissionEnd = DateTime.ParseExact(admDateTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);


            DataTable filteredTable = originalTable.Clone();

            foreach (DataRow row in originalTable.Rows)
            {
                DateTime admissionDate = DateTime.ParseExact(
                string.IsNullOrWhiteSpace(row["AdmDate"].ToString()) ? DateTime.Now.ToString("MM/dd/yyyy") : row["AdmDate"].ToString(),
                "MM/dd/yyyy",
                CultureInfo.InvariantCulture
                ); ;

                if (admissionDate < admissionStart || admissionDate > admissionEnd)
                    continue;

                filteredTable.ImportRow(row);
            }
            if(filteredTable.Rows.Count == 0)
                return originalTable.Clone();
            else if(filteredTable.Rows.Count > 0)
                filteredTable = filteredTable.AsEnumerable().OrderByDescending(row => DateTime.ParseExact(row.Field<string>("AdmDate"), "MM/dd/yyyy", CultureInfo.InvariantCulture)).CopyToDataTable();
            else
                return filteredTable;

            if (count >= 0)
            {
                filteredTable = filteredTable.AsEnumerable().Take(count).CopyToDataTable();
            }



            return filteredTable;
        }
        protected void btnShowDischarge_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkHighcharts.Checked)
                {
                    RVClientReport.Visible = false;
                    string dischargeQuery = "SELECT PA.ClientId,PA.Lastname,PA.Firstname,PA.AdmissionDate,CONVERT(VARCHAR(20),PA.AdmissionDate,101) AS ADate,PA.DischargeDate AS SPDischargeDate " +
                    " ,PL.EndDate AS PLDischargeDate,CONVERT(VARCHAR(20),PL.EndDate,101) EndDate FROM Placement PL INNER JOIN StudentPersonal PA ON PL.StudentPersonalId=PA.StudentPersonalId INNER JOIN Class CLS ON PL.Location = CLS.ClassId WHERE  " +
                    " PA.PlacementStatus = 'D' and CLS.ClassCd = 'DSCH' " +
                    " AND PA.ClientId<>'' AND PL.Status=1 AND CONVERT(INT,PA.ClientId)>0  ORDER BY PL.EndDate ";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(dischargeQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = GetSelectedColumnsDischargeDate(dt);
                    if (dt.Rows.Count > 0)
                        dt = dt.AsEnumerable().OrderBy(row => DateTime.ParseExact(row.Field<string>("Discharge Date"), "MM/dd/yyyy", CultureInfo.InvariantCulture)).CopyToDataTable();

                    string jsonData = ConvertDataTableToJson(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerBirthdate(" + jsonData + ");", true);

                }
                else
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
                if (checkHighcharts.Checked)
                {
                    RVClientReport.Visible = false;

                    string placementQuery = " SELECT *,CASE WHEN PLCStatus='New Admission' OR PLCStatus='Re-Admission' THEN 'New Placement' ELSE CASE WHEN PLCStatus='Respite' OR PLCStatus='Move' OR PLCStatus='Partial Discharge' THEN 'Active Placement' ELSE CASE WHEN PLCStatus='Discharge' THEN 'Discharged Placement' END END " +
                                            " END AS PlacementStatus ,(SELECT STUFF(ISNULL((SELECT ', ' + DATA " +
                                            " FROM  " +
                                            " [Split] (IsDays,',') WHERE DATA<>'0' " +
                                            " FOR XML PATH (''), TYPE).value('.','VARCHAR(max)'), ''), 1, 2, '')) Days FROM (SELECT SP.SchoolId,PT.StudentPersonalId,SP.ClientId,SP.LastName+','+SP.FirstName AS ClientName,PT.BehaviorAnalyst,PT.Department " +
                                            " ,(SELECT LookupName FROM LookUp WHERE LookupId=PT.Department) AS DepartmentName,CONVERT(VARCHAR(20),PT.EndDate,101) EndDate,CONVERT(DATE,PT.EndDate) EdDate,PT.Location " +
                                            " ,(SELECT ClassName FROM Class WHERE ClassId=PT.Location) AS LocationName ,PT.PlacementDepartment " +
                                            " ,(SELECT LookupName FROM LookUp WHERE LookupId=PT.PlacementDepartment) AS PlacementDepartmentName " +
                                            " ,PT.PlacementReason,(SELECT LookupName FROM LookUp WHERE LookupId=PT.PlacementReason) AS PlacementReasonName  " +
                                            " ,PT.PlacementType,(SELECT LookupName FROM LookUp WHERE LookupId=PT.PlacementType) AS PlacementTypeName " +
                                            " ,PT.Reason,CONVERT(VARCHAR(20),PT.StartDate,101) StartDate,CONVERT(DATE,PT.StartDate) StDate,(SELECT LookupName FROM LookUp WHERE LookupId=PT.PlacementReason) PLCStatus " +
                                            " ,CONVERT(DATE,PT.CreatedOn) CreatedBy,(CASE WHEN PT.IsMonday=1 THEN 'Monday' ELSE '0' END +','+CASE WHEN PT.IsTuesday=1 THEN 'Tuesday' ELSE '0' END +','+ " +
                                            " CASE WHEN PT.IsWednesday=1 THEN 'Wednesday' ELSE '0' END +','+CASE WHEN PT.IsThursday=1 THEN 'Thursday' ELSE '0' END +','+CASE WHEN PT.IsFriday=1 THEN 'Friday' ELSE '0' END +','+ " +
                                            " CASE WHEN PT.IsSaturday=1 THEN 'Saturday' ELSE '0' END +','+CASE WHEN PT.IsSunday=1 THEN 'Sunday' ELSE '0' END ) IsDays " +
                                            " FROM Placement PT LEFT JOIN StudentPersonal SP " +
                                            " ON PT.StudentPersonalId=SP.StudentPersonalId WHERE  " +
                                            " SP.StudentType='Client'    " +
                                            " AND SP.ClientId IS NOT NULL  AND CONVERT(INT,SP.ClientId)>0  " +
                                            " AND PT.Status=1 " +
                                            " AND (CONVERT(DATE,PT.EndDate)>CONVERT(DATE,GETDATE()) OR PT.EndDate IS NULL)  " +
                                            " AND PT.PlacementReason IS NOT NULL) PLACE  ORDER BY ClientId ";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand(placementQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt = filterDataTablePlacement(dt);
                    dt = GetSelectedColumnPlacement(dt);
                    dt.DefaultView.Sort = dt.Columns["Client Id"].ColumnName + " ASC";
                    dt = dt.DefaultView.ToTable();
                    var jsonData = JsonConvert.SerializeObject(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "loadDataFromServerPlacement(" + jsonData + ");", true);

                }
                else
                {
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable filterDataTablePlacement(DataTable originalTable)
            {
            string ActiveStartDate = (txtActiveStartDate.Text != "" ? GetDateFromText(txtActiveStartDate.Text) : "");
            string ActiveEndDate = (txtActiveEndDate.Text != "" ? GetDateFromText(txtActiveEndDate.Text) : "");
            string DischrEndDate = (txtDischrEndDate.Text != "" ? GetDateFromText(txtDischrEndDate.Text) : "");
            string DischrStartDate = (txtDischrStartDate.Text != "" ? GetDateFromText(txtDischrStartDate.Text) : "");
            string NewEndDate = (txtNewEndDate.Text != "" ? GetDateFromText(txtNewEndDate.Text) : "");
            string NewStartDate = (txtNewStartDate.Text != "" ? GetDateFromText(txtNewStartDate.Text) : "");

            string department = hdnballet.Value == "" ? "0" : (hdnballet.Value == "Choose Department and Location" ? ddlDeptLocDept.SelectedValue.ToString() : ddlDeptPlctypeDept.SelectedValue.ToString());
            string placementType = hdnballet.Value == "" ? "0" : (hdnballet.Value == "Choose Department and Placement Type" ? ddlDeptPlctypePlcType.SelectedValue.ToString() : ddlDeptPlctypePlcType.SelectedValue.ToString());
            string location = hdnballet.Value == "" ? "0" : (hdnballet.Value == "Choose Department and Location" ? ddlDeptLocLoc.SelectedValue.ToString() : ddlLocLoc.SelectedValue.ToString());
            string startDate = hdnDateRange.Value == "" ? "1900-01-01" : (hdnDateRange.Value == "Active Placement" ? ActiveStartDate : (hdnDateRange.Value == "Discharged Placement" ? DischrStartDate : NewStartDate));
            string endDate = hdnDateRange.Value == "" ? GetDateFromToday(Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("dd-MM-yyyy")) : (hdnDateRange.Value == "Active Placement" ? ActiveEndDate : (hdnDateRange.Value == "Discharged Placement" ? DischrEndDate : NewEndDate));
            string dateType = hdnDateRange.Value == "" ? "0" : (hdnDateRange.Value == "Active Placement" ? "Active Placement,New Placement" : hdnDateRange.Value);
            string categoryType = hdnballet.Value == "" ? "0" : hdnballet.Value;
            startDate = startDate.Replace("-", "/");
            endDate = endDate.Replace("-", "/");

            DateTime DateStart = DateTime.ParseExact(startDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime DateEnd = DateTime.ParseExact(endDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);


            DataTable filteredTable = originalTable.Clone();

            foreach (DataRow row in originalTable.Rows)
            {
                if (categoryType == "Choose Department and Location")
                {
                    if (row["PlacementDepartment"].ToString() != department)
                        continue;
                    if (row["Location"].ToString() != location)
                        continue;
                }
                else if (categoryType == "Choose Department and Placement Type")
                {
                    if (row["PlacementDepartment"].ToString() != department)
                        continue;
                    if (row["PlacementType"].ToString() != placementType)
                        continue;
                }
                else if (categoryType == "Choose Location")
                {
                    if (row["Location"].ToString() != location)
                        continue;
                }

                DateTime stDate = row.Field<DateTime>("StDate");
                DateTime edDate = row.Table.Columns.Contains("EdDate") && row["EdDate"] != DBNull.Value? row.Field<DateTime>("EdDate"): DateTime.MinValue;

                if(dateType == "New Placement")
                {
                    if (row["PlacementStatus"].ToString() != "New Placement")
                        continue;
                }

                DateTime compareField = dateType.Contains("Discharged Placement") ? edDate : stDate;

                if (compareField >= DateStart && compareField <= DateEnd)
                {
                    filteredTable.ImportRow(row);
                }

            }

            return filteredTable;
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
            btnShowReportVendor.Visible = false;
            btnResetVendor.Visible = false;
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
            btnShowReportVendor.Visible = false;
            btnResetVendor.Visible = false;
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
            btnShowReportVendor.Visible = false;
            btnResetVendor.Visible = false;
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
            btnShowReportVendor.Visible = false;
            btnResetVendor.Visible = false;
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
                    if (checkHighcharts.Checked)
                    {
                        RVClientReport.Visible = false;

                        string fundingChngQry = " SELECT *,CASE WHEN PreviousValue IS NULL OR PreviousValue='' THEN 'Add' ELSE 'Update' END AS Status FROM (SELECT SP.ClientId,SP.LastName+','+SP.FirstName AS ClientName,ObjectField, " + 
                                                " CASE WHEN PreviousValue LIKE '--%'+'Select'+'%--' THEN NULL ELSE PreviousValue END AS " + 
                                                " PreviousValue,NewValue,FORMAT(EventDate,'MM/dd/yyyy') EventDate,ObjectType,EventLogId FROM EventLogs EL " + 
                                                " JOIN StudentPersonal SP ON EL.StudentPersonalId=SP.StudentPersonalId " + 
                                                " INNER JOIN Placement PLC ON PLC.StudentPersonalId = SP.StudentPersonalId " + 
                                                " INNER JOIN LookUp LKP ON LKP.LookupId = PLC.Department " + 
                                                " WHERE (PLC.EndDate is null or PLC.EndDate >= cast (GETDATE() as DATE))  " + 
		                                        " and PLC.Status=1 AND LKP.LookupType = 'Department'  and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " + 
          		                                " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " + 
          		                                " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " + 
          		                                " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " + 
          		                                " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " +
                                                " and ST.StudentPersonalId not in (SELECT Distinct " +
                                                " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client'))) FUND WHERE ObjectType='Funder' " +
                                                " AND CONVERT(DATE,EventDate) >= CONVERT(DATE, '" + NewStartDate + "') AND CONVERT(DATE, EventDate) <= CONVERT(DATE, '" + NewEndDate + "') " + 
                                                " ORDER BY EventLogId DESC ";

                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                        con.Open();
                        SqlCommand cmd = new SqlCommand(fundingChngQry, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        dt = GetSelectedColumnFundingChanges(dt);
                        var jsonData = JsonConvert.SerializeObject(dt);
                        ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerChanges(" + jsonData + ");", true);
                    }
                    else
                        RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["FundingChangesReport"];
                }
                else if (hdnMenu.Value == "btnPlcChange")
                {
                    if (checkHighcharts.Checked)
                    {
                        RVClientReport.Visible = false;

                        string placementChngQry = " SELECT SP.ClientId,SP.LastName+','+SP.FirstName AS ClientName,ObjectField, " + 
                                                  " CASE WHEN PreviousValue LIKE '--%'+'Select'+'%--' THEN NULL ELSE PreviousValue END AS " + 
                                                  " PreviousValue,NewValue,FORMAT(EventDate,'MM/dd/yyyy') EventDate FROM EventLogs EL " + 
                                                  " JOIN StudentPersonal SP ON EL.StudentPersonalId=SP.StudentPersonalId WHERE ObjectType='Placement' and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " + 
                                                  " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " + 
                                                  " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " + 
                                                  " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " + 
                                                  " INNER JOIN LookUp LKP ON LKP.LookupId = PLC.Department " + 
                                                  " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 AND LKP.LookupType = 'Department' and ST.StudentType='Client') " + 
                                                  " and ST.StudentPersonalId not in (SELECT Distinct " + 
                                                  " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) " +
                                                  " AND CONVERT(DATE, EventDate) >= CONVERT(DATE, '" + NewStartDate + "') AND CONVERT(DATE, EventDate) <= CONVERT(DATE, '" + NewEndDate + "') ORDER BY EventLogId DESC ";

                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                        con.Open();
                        SqlCommand cmd = new SqlCommand(placementChngQry, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        dt = GetSelectedColumnFundingChanges(dt);
                        var jsonData = JsonConvert.SerializeObject(dt);
                        ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerChanges(" + jsonData + ");", true);
                    }
                    else
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["PlacementChangesReport"];
                }
                else if (hdnMenu.Value == "btnGuardianChanges")
                {
                    if (checkHighcharts.Checked)
                    {
                        RVClientReport.Visible = false;

                        string guardianshipChngQry = " SELECT SP.ClientId,SP.LastName+','+SP.FirstName AS ClientName,ObjectField, " + 
                                                     " CASE WHEN PreviousValue LIKE '--%'+'Select'+'%--' THEN NULL ELSE PreviousValue END AS " + 
                                                     " PreviousValue,NewValue,FORMAT(EventDate,'MM/dd/yyyy') EventDate,CASE WHEN ObjectField='Guardian(Self)' AND NewValue='Unchecked' THEN 'No' " + 
                                                     " ELSE CASE WHEN ObjectField='Guardian(Self)' AND NewValue='Checked' THEN 'Yes' END END Selfguard, " + 
                                                     " CASE WHEN ObjectField='Guardian' AND PreviousValue='Checked' AND NewValue='Unchecked' THEN (SELECT LastName+','+FirstName FROM ContactPersonal " + 
                                                     " WHERE ContactPersonalId=ObjectTypeId) END Oldguard,CASE WHEN ObjectField='Guardian' AND PreviousValue='Unchecked' AND NewValue='Checked' THEN (SELECT LastName+','+FirstName FROM ContactPersonal " + 
                                                     " WHERE ContactPersonalId=ObjectTypeId) END Newguard FROM EventLogs EL " + 
                                                     " JOIN StudentPersonal SP ON EL.StudentPersonalId=SP.StudentPersonalId " + 
                                                     " INNER JOIN Placement PLC ON PLC.StudentPersonalId = SP.StudentPersonalId " + 
                                                     " INNER JOIN LookUp LKP ON LKP.LookupId = PLC.Department " + 
                                                     " WHERE ObjectType='Guardianship' and (PLC.EndDate is null or PLC.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 AND LKP.LookupType = 'Department' and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " + 
                                                     " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " + 
                                                     " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " + 
                                                     " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " + 
                                                     " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " + 
                                                     " and ST.StudentPersonalId not in (SELECT Distinct " + 
                                                     " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) " +
                                                     " AND CONVERT(DATE, EventDate) >= CONVERT(DATE, '" + NewStartDate + "') AND CONVERT(DATE, EventDate) <= CONVERT(DATE, '" + NewEndDate + "') ORDER BY EventLogId DESC ";

                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                        con.Open();
                        SqlCommand cmd = new SqlCommand(guardianshipChngQry, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        dt = GetSelectedColumnGuardianChanges(dt);
                        var jsonData = JsonConvert.SerializeObject(dt);
                        ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerChanges(" + jsonData + ");", true);
                    }
                    else
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["GuardianshipChangesReport"];
                }
                else if (hdnMenu.Value == "btnContactChanges")
                {
                    if (checkHighcharts.Checked)
                    {
                        RVClientReport.Visible = false;

                        string contactChngQry = " SELECT SP.ClientId,SP.LastName+','+SP.FirstName AS ClientName,ObjectField,(SELECT LastName+','+FirstName FROM ContactPersonal WHERE ContactPersonalId=ObjectTypeId) ContactName, " + 
                                                " CASE WHEN PreviousValue LIKE '--%'+'Select'+'%--' THEN NULL ELSE PreviousValue END AS " + 
                                                " PreviousValue,NewValue,FORMAT(EventDate,'MM/dd/yyyy') EventDate FROM EventLogs EL " + 
                                                " JOIN StudentPersonal SP ON EL.StudentPersonalId=SP.StudentPersonalId " + 
                                                " INNER JOIN Placement PLC ON PLC.StudentPersonalId = SP.StudentPersonalId " + 
                                                " INNER JOIN LookUp LKP ON LKP.LookupId = PLC.Department " + 
                                                " WHERE ObjectType='Contact' and (PLC.EndDate is null or PLC.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 AND LKP.LookupType = 'Department' " + 
                                                " and SP.StudentPersonalId not in (SELECT Distinct ST.StudentPersonalId " + 
                                                " FROM StudentPersonal ST join ContactPersonal cp on cp.StudentPersonalId=ST.StudentPersonalId " + 
                                                " WHERE ST.StudentType='Client' and sT.ClientId>0 and ST.StudentPersonalId not in (SELECT Distinct " + 
                                                " ST.StudentPersonalId FROM StudentPersonal ST join Placement PLC on PLC.StudentPersonalId=ST.StudentPersonalId " + 
                                                " WHERE (PLC.EndDate is null or plc.EndDate >= cast (GETDATE() as DATE)) and PLC.Status=1 and ST.StudentType='Client') " + 
                                                " and ST.StudentPersonalId not in (SELECT Distinct " +
                                                " ST.StudentPersonalId FROM StudentPersonal ST WHERE ST.PlacementStatus='D' and ST.StudentType='Client')) " +
                                                " AND CONVERT(DATE, EventDate) >= CONVERT(DATE, '" + NewStartDate + "') AND CONVERT(DATE, EventDate) <= CONVERT(DATE, '" + NewEndDate + "') ORDER BY EventLogId DESC";

                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
                        con.Open();
                        SqlCommand cmd = new SqlCommand(contactChngQry, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        dt = GetSelectedColumnContactChanges(dt);
                        var jsonData = JsonConvert.SerializeObject(dt);
                        ClientScript.RegisterStartupScript(this.GetType(), "LoadData", "LoadDataFromServerChanges(" + jsonData + ");", true);
                    }
                    else
                    RVClientReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ContactChangesReport"];
                }
                if (!checkHighcharts.Checked)
                {
                    RVClientReport.ShowParameterPrompts = false;
                    ReportParameter[] parm = new ReportParameter[2];
                    parm[0] = new ReportParameter("StartDate", NewStartDate);
                    parm[1] = new ReportParameter("EndDate", NewEndDate);
                    this.RVClientReport.ServerReport.SetParameters(parm);
                    RVClientReport.ServerReport.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetSelectedColumnFundingChanges(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns;
            if(hdnMenu.Value == "btnPlcChange")
                selectedColumns = new string[] { "ClientId", "ClientName", "ObjectField", "PreviousValue", "NewValue", "EventDate"};
            else
                selectedColumns = new string[] { "ClientId", "ClientName", "ObjectField", "Status", "PreviousValue", "NewValue", "EventDate" };


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

            if (newTable.Columns.Contains("ClientId"))
            {
                newTable.Columns["ClientId"].ColumnName = "Client ID";
            }

            if (newTable.Columns.Contains("ClientName"))
            {
                newTable.Columns["ClientName"].ColumnName = "Client Name";
            }

            if (newTable.Columns.Contains("ObjectField"))
            {
                newTable.Columns["ObjectField"].ColumnName = "Change Made to";
            }
            if (hdnMenu.Value == "btnFundChange")
            {
                if (newTable.Columns.Contains("Status"))
                {
                    newTable.Columns["Status"].ColumnName = "Update/Add";
                }
            }

            if (newTable.Columns.Contains("PreviousValue"))
            {
                newTable.Columns["PreviousValue"].ColumnName = "Old Value";
            }

            if (newTable.Columns.Contains("NewValue"))
            {
                newTable.Columns["NewValue"].ColumnName = "New Value";
            }

            if (newTable.Columns.Contains("EventDate"))
            {
                newTable.Columns["EventDate"].ColumnName = "Date of Change";
            }

            return newTable;
        }

        public DataTable GetSelectedColumnGuardianChanges(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns = new string[] { "ClientId", "ClientName", "Selfguard", "Oldguard", "Newguard", "EventDate" };


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

            if (newTable.Columns.Contains("ClientId"))
            {
                newTable.Columns["ClientId"].ColumnName = "Client ID";
            }

            if (newTable.Columns.Contains("ClientName"))
            {
                newTable.Columns["ClientName"].ColumnName = "Client Name";
            }

            if (newTable.Columns.Contains("Selfguard"))
            {
                newTable.Columns["Selfguard"].ColumnName = "Self Guardian";
            }

            if (newTable.Columns.Contains("Oldguard"))
            {
                newTable.Columns["Oldguard"].ColumnName = "Old Guardian";
            }
            
            if (newTable.Columns.Contains("Newguard"))
            {
                newTable.Columns["Newguard"].ColumnName = "New Guardian";
            }

            if (newTable.Columns.Contains("EventDate"))
            {
                newTable.Columns["EventDate"].ColumnName = "Date of Change";
            }

            return newTable;
        }

        public DataTable GetSelectedColumnContactChanges(DataTable originalTable)
        {
            DataTable newTable = new DataTable();
            string[] selectedColumns = new string[] { "ClientId", "ClientName", "ContactName", "ObjectField", "PreviousValue", "NewValue", "EventDate" };


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

            if (newTable.Columns.Contains("ClientId"))
            {
                newTable.Columns["ClientId"].ColumnName = "Client ID";
            }

            if (newTable.Columns.Contains("ClientName"))
            {
                newTable.Columns["ClientName"].ColumnName = "Client Name";
            }

            if (newTable.Columns.Contains("ContactName"))
            {
                newTable.Columns["ContactName"].ColumnName = "Contact Name";
            }

            if (newTable.Columns.Contains("ObjectField"))
            {
                newTable.Columns["ObjectField"].ColumnName = "Change Made to";
            }

            if (newTable.Columns.Contains("PreviousValue"))
            {
                newTable.Columns["PreviousValue"].ColumnName = "Old Value";
            }

            if (newTable.Columns.Contains("NewValue"))
            {
                newTable.Columns["NewValue"].ColumnName = "New Value";
            }

            if (newTable.Columns.Contains("EventDate"))
            {
                newTable.Columns["EventDate"].ColumnName = "Date of Change";
            }

            return newTable;
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