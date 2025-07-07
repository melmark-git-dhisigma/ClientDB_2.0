using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.Models;
using System.Data.SqlClient;
using System.Data;
using ClientDB.AppFunctions;
using ClientDB.DbModel;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace ClientDB.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetFilteredReport()
        {
            try
            {
                string jsonBody;
                using (var reader = new StreamReader(Request.InputStream))
                {
                    jsonBody = reader.ReadToEnd();
                }
                var selectedValues = JsonConvert
                .DeserializeObject<Dictionary<string, List<string>>>(jsonBody);

                if (selectedValues == null || !selectedValues.Any())
                    return Json(
                        new { error = "selectedValues is null or empty!" },
                        JsonRequestBehavior.AllowGet
                    );

                DataTable dt = new DataTable();
                DataTable dtFinal = new DataTable();

                // Get connection string from web.config/app.config
                string connStr = ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString();

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("ClientStatisticalGraph", con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300;
                    // Add parameters - assumes ContainsLoop is a method returning the expected value
                    cmd.Parameters.AddWithValue("@ParamStudName", ContainsLoop("Student NameFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamGender", ContainsLoop("GenderFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamLanguage", ContainsLoop("Primary LanguageFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamRace", ContainsLoop("RaceFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamLocation", ContainsLoop("LocationFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamProgram", ContainsLoop("ProgramFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamPlacement", ContainsLoop("Placement TypeFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamDepartment", ContainsLoop("DepartmentFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamActive", "true");
                    cmd.Parameters.AddWithValue("@ParamCity", ContainsLoop("CityFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamState", ContainsLoop("StateFlg", selectedValues));
                    cmd.Parameters.AddWithValue("@ParamStudRow", ContainsLoop("Total number of clientFlg", selectedValues));

                    string statusText = GetFilterValueString("Status", selectedValues);

                    //System.Diagnostics.Debug.WriteLine(
                    //    string.Format(
                    //        "[DEBUG] @GetActiveID = '{0}'; selectedValues = {1}",
                    //        statusText,
                    //        "!ReportController!"
                    //    )
                    //);

                    string code;
                    if (statusText == "Active") code = "A";
                    else if (statusText == "Discharged") code = "D";
                    else if (statusText == "Active,Discharged") code = "A,D";
                    else code = "A";

                    cmd.Parameters.AddWithValue("@GetActiveID", code);
                    cmd.Parameters.AddWithValue("@GetStudID", GetFilterValueString("Student Name", selectedValues));
                    cmd.Parameters.AddWithValue("@GetLocationID", GetFilterValueString("Location", selectedValues));
                    cmd.Parameters.AddWithValue("@GetRaceID", GetFilterValueString("Race", selectedValues));

                    con.Open();
                    da.Fill(dt);
                    string dtJson = JsonConvert.SerializeObject(dt);

                    // 2) Write it to Debug (or your preferred logger)
                    //System.Diagnostics.Debug.WriteLine("[DEBUG] dt contents:\n" + dtJson);
                    dtFinal = GetSelectedColumns(dt);
                }
                var result = dtFinal.AsEnumerable()
                .Select(row => dtFinal.Columns
                    .Cast<DataColumn>()
                    .ToDictionary(col => col.ColumnName, col => row[col]))
                .ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
                //return Json(new { message = "Now it works with JSON!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content("Server error: " + ex.Message + "\n" + ex.StackTrace, "text/plain");
            }
        }

        private string GetFilterValueString(string key, Dictionary<string, List<string>> selectedValues)
        {
            if (selectedValues != null)
            {
                List<string> list;
                if (selectedValues.TryGetValue(key, out list)
                    && list != null
                    && list.Count > 0)
                {
                    return string.Join(",", list);
                }
            }
            return null;
        }

        private string ContainsLoop(string key, Dictionary<string, List<string>> selectedItemList)
        {
            // Example: return first value, or empty string if key not present
            if (selectedItemList.ContainsKey(key) && selectedItemList[key].Count > 0)
                return selectedItemList[key][0];
            return null;
        }

        public DataTable GetSelectedColumns(DataTable originalTable)
        {
            //To return only required columns for the table.
            DataTable newTable = new DataTable();

            string[] selectedColumns = { "StudName", "Gender", "StudLanguage", "RaceName", "City", "StudState", "ClassName", "Program", "Placement_Type", "DepartmentName", "StudStatus" };

            foreach (var columnName in selectedColumns)
            {
                if (originalTable.Columns.Contains(columnName))
                {
                    newTable.Columns.Add(
                        columnName,
                        originalTable.Columns[columnName].DataType
                    );
                }
            }

            // 2) Copy each DataRow, but only for columns that exist in originalTable
            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newTable.NewRow();

                foreach (var columnName in selectedColumns)
                {
                    if (originalTable.Columns.Contains(columnName))
                    {
                        // Now safe: originalTable has this columnName
                        newRow[columnName] = row[columnName];
                    }
                    // If originalTable does NOT have columnName, skip it entirely.
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
    }
}
