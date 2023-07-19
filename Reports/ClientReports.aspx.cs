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
using System.Collections;
using System.Drawing.Printing;
using System.IO;
using ClientDB.DbModel;


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

        protected void btnallClient_Click(object sender, EventArgs e)
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

        protected void btnClienContact_Click(object sender, EventArgs e)
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

        protected void btnPgmRoster_Click(object sender, EventArgs e)
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
                if (chkcontact == "Inactive")
                {
                    HContactstatus.Value = "0";
                }
                if (chkcontact == "Discharged")
                {
                    HContactstatus.Value = "2";
                }
                if (chkcontact == "ActiveInactive")
                {
                    HContactstatus.Value = "0,1";
                }
                if (chkcontact == "ActiveDischarged")
                {
                    HContactstatus.Value = "1,2";
                }
                if (chkcontact == "InactiveDischarged")
                {
                    HContactstatus.Value = "0,2";
                }
                if (chkcontact == "ActiveInactiveDischarged")
                {
                    HContactstatus.Value = "0,1,2";
                }

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

                row[0] = "Inactive";
                row[1] = "I";
                Dtracname.Rows.Add(row);
                hfstatus.Value += row[1] + ", ";

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