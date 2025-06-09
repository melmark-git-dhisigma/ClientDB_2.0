<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ClientReports.aspx.cs" Inherits="ClientDB.Reports.ClientReports" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.13.4/css/jquery.dataTables.min.css" />

    <!-- jQuery CDN -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<!-- jQuery UI CDN -->
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
<link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>

<!-- Other JS and CSS -->
<script src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/exceljs/4.3.0/exceljs.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/2.0.5/FileSaver.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf-autotable/3.5.28/jspdf.plugin.autotable.min.js"></script>

<!-- Local Scripts (Ensure the paths are correct) -->
<script src="../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../Documents/JS/jquery.validationEngine.js"></script>
<script src="../Documents/JS/jquery.unobtrusive-ajax.js"></script>

<!-- Local Stylesheets -->
<link href="../Documents/CSS/validationEngine.jquery.css" rel="stylesheet" />
<link href="../Documents/CSS/ReportStyle.css" rel="stylesheet" />


    <style type="text/css">
        .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-year {
            width: 50% !important;
        }

        .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-month {
            width: 50% !important;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {


            $("#ddlDeptLocDept").prop('disabled', true);
            $("#ddlDeptLocLoc").prop('disabled', true);
            $("#ddlDeptPlctypeDept").prop('disabled', true);
            $("#ddlDeptPlctypePlcType").prop('disabled', true);
            $("#ddlLocLoc").prop('disabled', true);
            $("#txtActiveStartDate").prop('disabled', true);
            $("#txtActiveEndDate").prop('disabled', true);
            $("#txtNewStartDate").prop('disabled', true);
            $("#txtNewEndDate").prop('disabled', true);
            $("#txtDischrStartDate").prop('disabled', true);
            $("#txtDischrEndDate").prop('disabled', true);

            var date = new Date();
            date.setDate(date.getDate());
            $('.datepicker').datepicker(
             {
                 dateFormat: "mm/dd/yy",
                 changeMonth: true,
                 changeYear: true,
                 showAnim: "fadeIn",
                 yearRange: 'c-100:c+100',
                 //minDate: date,
                 /* fix buggy IE focus functionality */
                 fixFocusIE: false,
                 constrainInput: false
             });





            $.get("../ClientRegistration/GetTitleReport", function (data) {
                document.title = data;

            });



        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function GetValidate() {
            if ($('#hdnballet').val() == "Choose Department and Location") {
                if ($('#ddlDeptLocDept').prop('selectedIndex') == 0) {
                    alert("Please select Department");
                    return false;
                }
                if ($('#ddlDeptLocLoc').prop('selectedIndex') == 0) {
                    alert("Please select Location");
                    return false;
                }

            }
            else if ($('#hdnballet').val() == "Choose Department and Placement Type") {
                if ($('#ddlDeptPlctypeDept').prop('selectedIndex') == 0) {
                    alert("Please select Department");
                    return false;
                }
                if ($('#ddlDeptPlctypePlcType').prop('selectedIndex') == 0) {
                    alert("Please select Placement Type");
                    return false;
                }
            }
            else if ($('#hdnballet').val() == "Choose Location") {
                if ($('#ddlLocLoc').prop('selectedIndex') == 0) {
                    alert("Please select Location");
                    return false;
                }
            }
            if ($('#hdnDateRange').val() == "Active Placement") {
                if ($('#txtActiveStartDate').val() == "") {
                    alert("Please select Startdate");
                    return false;
                }
                if ($('#txtActiveEndDate').val() == "") {
                    alert("Please select Enddate");
                    return false;
                }

            }
            else if ($('#hdnDateRange').val() == "Discharged Placement") {
                if ($('#txtDischrStartDate').val() == "") {
                    alert("Please select Startdate");
                    return false;
                }
                if ($('#txtDischrEndDate').val() == "") {
                    alert("Please select Enddate");
                    return false;
                }
            }
            else if ($('#hdnDateRange').val() == "New Placement") {
                if ($('#txtNewStartDate').val() == "") {
                    alert("Please select Startdate");
                    return false;
                }
                if ($('#txtNewEndDate').val() == "") {
                    alert("Please select Enddate");
                    return false;
                }
            }
            return true;
        }


        function ChangeSelectedMenu(MenuClass) {
            if ($(".leftMenu")[0]) {
                $('.leftMenu').css('background-position', 'none');
            }
            $('#' + MenuClass).css('background-position', '0 0');
        }


        $('.leftMenu').click(function () {

            var elmId = $(this).attr('id');
            $('.leftMenu').removeClass('current');
            $(this).addClass('current');
            if (elmId == "btnallClient") {
                //$('#content').load('../ClientRegistration/ClientRegistration|Fill');
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
                $('.EditProfile').css("display", "block");
                $('#calender').css("display", "none");


            }
            if (elmId == "btnClienContact") {
                // $('#content').load('../Medical/Medical/');
                //       $('#content').load('/Medical/FillMedicalData/0');
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                $('#calender').css("display", "block");

            }
            if (elmId == "btnPgmRoster") {
                //$('#content').load('../Placement/Placement/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }
            if (elmId == "btnVendor") {
                //$('#content').load('../Contact/ListContactVendor/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                // $('#content').load('/Contact/fillContactDetails/');
            }
            if (elmId == "btnBirthdate") {
                //$('#content').load('../Visitation/Visitation/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }
            if (elmId == "btnResRoster") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnAllPlacement") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnStatistical") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnAllDischargedate") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnAllAdmissionDate") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnAllBirthdate") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnContactChanges") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnGuardianChanges") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnPlcChange") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnFundChange") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }
        });

        function ValidateChanges() {
            if ($('#txtchangeSdate').val() == "") {
                alert("Please select Startdate");
                return false;
            }
            else if ($('#txtchangeEdate').val() == "") {
                alert("Please select Enddate");
                return false;
            }            
                return true;
        }

        function resetVal() {
            $('#txtchangeSdate').val("") ;
            $('#txtchangeEdate').val("") ;
        }

    </script>
    <style>
        #checkHighcharts {
            margin-left:10px;
            height:15px;
            width:15px;
        }
    </style>
    <style>

        /*Table Styling*/

        #table {
        table-layout:fixed;
        width: 100%;
        border: 1px solid black;
        border-collapse: collapse;
        margin-top: 20px;
        background-color: #fff;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    }
        #table tr {
    width:auto;
}
    /* Table Header styling */
    #tableHeader {
        background-color: #4CAF50;
        color: white;
        text-align: left;
        font-weight: bold;
    }

    /* Table Header cells */
    #tableHeader th {
        text-align:center;
        border: 1px solid #ddd;
        padding: 12px 15px;
        cursor: pointer;
        height: 30px;
    }

    /* Table Body styling */
    #tableBody {
        background-color: #fff;
    }

    /* Table rows and cells */
    #tableBody tr:nth-child(even) {
        background-color: #f9f9f9;
    }

        #tableBody tr {
            height: 40px;
            width: auto;
        }
    #tableBody tr:hover {
        background-color: #f1f1f1;
    }
    
    .disable-hover tr:hover {
    background-color: transparent !important;
    }

    #tableBody td {
        text-align:center;
        border: 1px solid #ddd;
        padding: 12px 15px;
        border-bottom: 1px solid #ddd;
    }

    /* Filter Section */
    #filterDiv {
        margin-bottom: 20px;
        padding: 10px;
        background-color: #fff;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
    }

    /* Checkbox container in filter section */
    #filterDiv input[type="checkbox"] {
        margin-right: 10px;
        margin-top: 5px;
        user-select:none;
    }

    #filterDiv label {
        margin-right: 15px;
        font-size: 14px;
        font-weight: normal;
    }

    /* Responsive design for smaller screens */
    @media (max-width: 768px) {
        #table {
            font-size: 14px;
        }

        #tableBody td {
            padding: 10px;
        }

        #tableHeader th {
            padding: 10px;
        }
    }
    </style>
   
    <script>

        var currentPage = 1;
        var rowsPerPage = 10;
        var fullData = [];
        function loadDataFromServer(data) {
            fullData = data;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            tableBody.innerHTML = '';

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                document.getElementById("noOfClients").textContent = "Total No. of Clients : 0";
                return;
            }
            else
                tableHeader.style.removeProperty("display");

            // Get column headers
            var columns = Object.keys(data[0]);

            // Clear any existing headers
            tableHeader.innerHTML = '';
            var headerRow = document.createElement('tr');

            // Create header cells
            columns.forEach(function (col) {
                var th = document.createElement('th');
                th.textContent = col;
                th.setAttribute('onclick', 'sortTable("' + col + '")');
                headerRow.appendChild(th);
            });

            // Append the header row
            tableHeader.appendChild(headerRow);

            // Pagination logic: slice data for the current page
            var startIndex = (currentPage - 1) * rowsPerPage;
            var endIndex = startIndex + rowsPerPage;
            var pageData = data.slice(startIndex, endIndex);

            // Populate table body with rows for the current page
            pageData.forEach(function (row) {
                var tr = document.createElement('tr');
                columns.forEach(function (col) {
                    var td = document.createElement('td');
                    td.textContent = row[col];
                    tr.appendChild(td);
                });
                tableBody.appendChild(tr);
            });

            // Create column visibility checkboxes
            createColumnVisibilityCheckboxes(columns);

            // Create pagination controls
            createPaginationControls(data.length, data);

            //Display count of clients
            document.getElementById("noOfClients").textContent = "Total No. of Clients : " + data.length;
        }

        function createPaginationControls(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    loadDataFromServer(data); // Re-load the table data for the new page
                }
            };
            paginationContainer.appendChild(prevButton);

            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + Math.ceil(totalRows / rowsPerPage);
            paginationContainer.appendChild(pageIndicator);

            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServer(data);
                }
            };
            paginationContainer.appendChild(nextButton);
            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportToExcelWithImages();
                loadDataFromServer(data);loadDataFromServer(data);
                document.getElementById("filterDiv").style.display = 'block';
                document.getElementById("buttonContainer").style.display = 'block';
                document.getElementById("btnShowReport").style.display = 'inline-block';
                document.getElementById("btnResetAllClient").style.display = 'inline-block';
            };
            paginationContainer.appendChild(exportButton);
        }



        var sortDirection = {};

        function sortTable(columnName) {
            if (!fullData || fullData.length === 0) return;

            var ascending = sortDirection[columnName] === "asc";
            sortDirection[columnName] = ascending ? "desc" : "asc";

            fullData.sort(function (a, b) {
                var valueA = a[columnName] ? a[columnName].toString().trim() : "";
                var valueB = b[columnName] ? b[columnName].toString().trim() : "";

                return ascending ? valueA.localeCompare(valueB) : valueB.localeCompare(valueA);
            });
            currentPage = 1;
            loadDataFromServer(fullData);
        }

        function createColumnVisibilityCheckboxes(columns) {
            var filterDiv = document.getElementById("filterDiv");
            filterDiv.innerHTML = '';

            columns.forEach(function (col) {
                var checkboxLabel = document.createElement('label');
                var checkbox = document.createElement('input');
                checkbox.type = 'checkbox';
                checkbox.checked = true;
                checkbox.setAttribute('onclick', 'toggleColumnVisibility("' + col + '", this)');

                checkboxLabel.appendChild(checkbox);
                checkboxLabel.appendChild(document.createTextNode(col));
                filterDiv.appendChild(checkboxLabel);
            });
        }


        function toggleColumnVisibility(columnName, checkbox) {
            var table = document.getElementById("table");
            var columnIndex = Array.from(table.rows[0].cells).findIndex(function (cell) {
                return cell.textContent === columnName;
            });

            Array.from(table.rows).forEach(function (row) {
                if (checkbox.checked) {
                    row.cells[columnIndex].style.display = '';
                } else {
                    row.cells[columnIndex].style.display = 'none';
                }
            });
        }

        function filterTable(columnName, selectedValues) {
            var table = document.getElementById("table");
            var rows = Array.from(table.rows).slice(1);
            var columnIndex = Array.from(table.rows[0].cells).findIndex(function (cell) {
                return cell.textContent === columnName;
            });

            rows.forEach(function (row) {
                var cellValue = row.cells[columnIndex].textContent;
                if (selectedValues.includes('All') || selectedValues.includes(cellValue)) {
                    row.style.display = '';
                } else {
                    row.style.display = 'none';
                }
            });
        }

        //Emergency/Home Contact Table
        function loadDataFromServerEmergency(data) {
            document.getElementById("filterDiv").style.display = "none";
            document.getElementById("buttonContainer").style.display = "none";
            fullData = data;
            rowsPerPage = 30;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            tableBody.innerHTML = '';

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                return;
            } else {
                tableHeader.style.removeProperty("display");
            }

            // Get column headers
            var columns = Object.keys(data[0]);

            // Clear any existing headers
            tableHeader.innerHTML = '';

            // Create first row for main headers
            var mainHeaderRow = document.createElement('tr');
            var subHeaderRow = document.createElement('tr');

            for (var index = 0; index < columns.length; index++) {
                var col = columns[index];
                var th = document.createElement('th');

                //Creating main and sub-columns
                if (columns[index] === "Contact Name") {
                    th.setAttribute("colspan", "2");
                    th.textContent = "Emergency Contact";
                    mainHeaderRow.appendChild(th);

                    var subTh1 = document.createElement('th');
                    subTh1.textContent = columns[index];
                    var subTh2 = document.createElement('th');
                    subTh2.textContent = columns[++index];
                    subHeaderRow.appendChild(subTh1);
                    subHeaderRow.appendChild(subTh2);
                } else { 
                    th.textContent = col;
                    th.setAttribute("rowspan", "2");
                    mainHeaderRow.appendChild(th);
                }
            }

            // Append header rows
            tableHeader.appendChild(mainHeaderRow);
            tableHeader.appendChild(subHeaderRow);
            tableBody.classList.add("disable-hover");

            // Pagination logic: slice data for the current page
            var startIndex = (currentPage - 1) * rowsPerPage;
            var endIndex = startIndex + rowsPerPage;
            var pageData = data.slice(startIndex, endIndex);

            // Calculate row spans for each Client Name
            var rowSpanMap = {};
            for (var i = 0; i < pageData.length; i++) {
                var key = pageData[i]["Client Name"]; // Grouping key
                if (rowSpanMap[key]) {
                    rowSpanMap[key] += 1;
                } else {
                    rowSpanMap[key] = 1;
                }
            }

            // Populate table body with grouped rows
            var seen = {}; // Track seen Client Name values to prevent duplicate cells
            for (var i = 0; i < pageData.length; i++) {
                var tr = document.createElement('tr');
                var key = pageData[i]["Client Name"]; // Grouping key

                for (var index = 0; index < columns.length; index++) {
                    var columnName = columns[index];
                    var td = document.createElement('td');

                    // Merge "Client Name", "Birth Date", and "Age" together
                    if (columnName === "Client Name") {
                        if (!seen[key]) { // Only add merged cell if not already seen
                            seen[key] = true; // Mark as seen
                            td.textContent = pageData[i]["Client Name"];
                            td.setAttribute("rowspan", rowSpanMap[key]); // Set rowspan for correct merging
                            tr.appendChild(td);

                            // Also merge Birth Date and Age into the same row
                            var tdBirthDate = document.createElement("td");
                            tdBirthDate.textContent = pageData[i]["Birth Date"];
                            tdBirthDate.setAttribute("rowspan", rowSpanMap[key]);
                            tr.appendChild(tdBirthDate);

                            var tdAge = document.createElement("td");
                            tdAge.textContent = pageData[i]["Age"];
                            tdAge.setAttribute("rowspan", rowSpanMap[key]);
                            tr.appendChild(tdAge);
                        }
                    }
                    else if (columnName !== "Birth Date" && columnName !== "Age") {
                        // Normal columns without merging
                        td.textContent = pageData[i][columnName];
                        tr.appendChild(td);
                    }
                }

                // Append the row to the table
                tableBody.appendChild(tr);
            }

            createPaginationControlsEmergency(data.length, data);
        }

        function createPaginationControlsEmergency(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    loadDataFromServerEmergency(data); // Re-load the table data for the new page
                }
            };
            paginationContainer.appendChild(prevButton);

            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + Math.ceil(totalRows / rowsPerPage);
            paginationContainer.appendChild(pageIndicator);

            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServerEmergency(data);
                }
            };
            paginationContainer.appendChild(nextButton);
            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportToExcelEmergency();
                loadDataFromServerEmergency(data);
            };
            paginationContainer.appendChild(exportButton);
        }


        //Program Roster Table
        function loadDataFromServerProgramRoster(data) {
            document.getElementById("filterDiv").style.display = "none";
            document.getElementById("buttonContainer").style.display = "none";
            fullData = data;
            rowsPerPage = 30;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            tableBody.innerHTML = '';
            var table = document.getElementById("table");
            table.style.tableLayout = "auto";
            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                return;
            }
            else
                tableHeader.style.removeProperty("display");

            // Get column headers
            var columns = Object.keys(data[0]);

            // Clear any existing headers
            tableHeader.innerHTML = '';

            // Create first row for main headers
            var mainHeaderRow = document.createElement('tr');
            var subHeaderRow = document.createElement('tr');

            for (var index = 0; index < columns.length; index++) {

                var col = columns[index];
                var th = document.createElement('th');
                var parts = col.split("/");

                //Creating main and sub-columns
                if (parts.length>1) { 
                    th.setAttribute("colspan", "4");
                    th.textContent = parts[0];
                    mainHeaderRow.appendChild(th);
                    mainHeaderRow.style.whiteSpace = "nowrap";
                    var subTh1 = document.createElement('th');
                    subTh1.textContent = columns[index].replace(parts[0]+"/","");;

                    var subTh2 = document.createElement('th');
                    subTh2.textContent = columns[++index].replace(parts[0] + "/", "");

                    var subTh3 = document.createElement('th');
                    subTh3.textContent = columns[++index].replace(parts[0] + "/", "");

                    var subTh4 = document.createElement('th');
                    subTh4.textContent = columns[++index].replace(parts[0] + "/", "");

                    subHeaderRow.appendChild(subTh1);
                    subHeaderRow.appendChild(subTh2);
                    subHeaderRow.appendChild(subTh3);
                    subHeaderRow.appendChild(subTh4);
                    subHeaderRow.style.whiteSpace = "nowrap";
                } else {
                    th.textContent = col;
                    th.setAttribute("rowspan", "2");
                    mainHeaderRow.appendChild(th);
                }
            }

            // Append header rows
            tableHeader.appendChild(mainHeaderRow);
            tableHeader.appendChild(subHeaderRow);

            // Pagination logic: slice data for the current page
            var startIndex = (currentPage - 1) * rowsPerPage;
            var endIndex = startIndex + rowsPerPage;
            var pageData = data.slice(startIndex, endIndex);

            // Populate table body with rows for the current page
            pageData.forEach(function (row) {
                var tr = document.createElement('tr');
                columns.forEach(function (col) {
                    var td = document.createElement('td');
                    td.textContent = row[col];
                    tr.appendChild(td);
                });
                tableBody.appendChild(tr);
            });

            // Create pagination controls
            createPaginationControlsProgramRoster(data.length, data);
        }

        function createPaginationControlsProgramRoster(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    loadDataFromServerProgramRoster(data); // Re-load the table data for the new page
                }
            };
            paginationContainer.appendChild(prevButton);

            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + Math.ceil(totalRows / rowsPerPage);
            paginationContainer.appendChild(pageIndicator);

            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServerProgramRoster(data);
                }
            };
            paginationContainer.appendChild(nextButton);
            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportToExcelProgramRoster();
                loadDataFromServerProgramRoster(data);
            };
            paginationContainer.appendChild(exportButton);
        }


        //Client/Contact/Vendor Table
        function loadDataFromServerVendor(data) {
            fullData = data;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            tableBody.innerHTML = '';
            var table = document.getElementById("table");
            table.style.tableLayout = "auto";
            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                return;
            }
            else
                tableHeader.style.removeProperty("display");

            // Get column headers
            var columns = Object.keys(data[0]);

            // Clear any existing headers
            tableHeader.innerHTML = '';
            var headerRow = document.createElement('tr');

            // Create header cells
            columns.forEach(function (col) {
                if (col != "Status") {
                    var th = document.createElement('th');
                    th.textContent = col;
                    headerRow.appendChild(th);
                }
            });

            // Append the header row
            tableHeader.appendChild(headerRow);

            // Pagination logic: slice data for the current page
            var startIndex = (currentPage - 1) * rowsPerPage;
            var endIndex = startIndex + rowsPerPage;
            var pageData = data.slice(startIndex, endIndex);

            //// Populate table body with rows for the current page
            //pageData.forEach(function (row) {
            //    var tr = document.createElement('tr');
            //    columns.forEach(function (col) {
            //        var td = document.createElement('td');
            //        td.textContent = row[col];
            //        tr.appendChild(td);
            //    });
            //    tableBody.appendChild(tr);
            //});

            // Calculate row spans for each Client Name
            var rowSpanMap = {};
            for (var i = 0; i < pageData.length; i++) {
                var key = pageData[i]["Client Last"]; // Grouping key
                if (rowSpanMap[key]) {
                    rowSpanMap[key] += 1;
                } else {
                    rowSpanMap[key] = 1;
                }
            }

            // Populate table body with grouped rows
            var seen = {}; // Track seen Client Name values to prevent duplicate cells
            for (var i = 0; i < pageData.length; i++) {
                var tr = document.createElement('tr');
                var key = pageData[i]["Client Last"]; // Grouping key

                for (var index = 0; index < columns.length; index++) {
                    var columnName = columns[index];
                    var td = document.createElement('td');

                    // Merge "Client Name", "Birth Date", and "Age" together
                    if (columnName === "Client Last") {
                        if (!seen[key]) { // Only add merged cell if not already seen
                            seen[key] = true; // Mark as seen
                            td.textContent = pageData[i]["Client Last"];
                            td.setAttribute("rowspan", rowSpanMap[key]); // Set rowspan for correct merging
                            tr.appendChild(td);

                            var tdClientFirst = document.createElement("td");
                            tdClientFirst.textContent = pageData[i]["Client First"];
                            tdClientFirst.setAttribute("rowspan", rowSpanMap[key]);
                            tr.appendChild(tdClientFirst);

                            // Also merge Birth Date and Age into the same row
                            var tdBirthDate = document.createElement("td");
                            tdBirthDate.textContent = pageData[i]["Date of Birth"];
                            tdBirthDate.setAttribute("rowspan", rowSpanMap[key]);
                            tdBirthDate.style.whiteSpace = "nowrap";
                            tr.appendChild(tdBirthDate);

                            var tdAdmDate = document.createElement("td");
                            tdAdmDate.textContent = pageData[i]["Admission Date"];
                            tdAdmDate.setAttribute("rowspan", rowSpanMap[key]);
                            tdAdmDate.style.whiteSpace = "nowrap";
                            tr.appendChild(tdAdmDate);

                            var tdPrgmPlc = document.createElement("td");
                            tdPrgmPlc.textContent = pageData[i]["Program and Active Placement(s)"];
                            tdPrgmPlc.setAttribute("rowspan", rowSpanMap[key]);
                            tr.appendChild(tdPrgmPlc);
                        }
                    }
                    else if (columnName !== "Client First" && columnName !== "Date of Birth" && columnName !== "Admission Date" && columnName !== "Program and Active Placement(s)" && columnName !== "Status") {
                        // Normal columns without merging
                        td.textContent = pageData[i][columnName];
                        tr.appendChild(td);
                    }
                }

                // Append the row to the table
                tableBody.appendChild(tr);
            }

            // Create pagination controls
            createPaginationControlsVendor(data.length, data);
        }
        function createPaginationControlsVendor(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    loadDataFromServerVendor(data); // Re-load the table data for the new page
                }
            };
            paginationContainer.appendChild(prevButton);

            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + Math.ceil(totalRows / rowsPerPage);
            paginationContainer.appendChild(pageIndicator);

            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServerVendor(data);
                }
            };
            paginationContainer.appendChild(nextButton);
            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportToExcelClientContactVendor();
                loadDataFromServerVendor(data);
            };
            paginationContainer.appendChild(exportButton);
        }

        //Birthdate Quarter Table
        function loadDataFromServerQuarter(data) {
            fullData = data;
            if (HeadingDiv.innerHTML === "Residential Roster Report") {
                rowsPerPage = 15;
            }
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            document.getElementById("filterDiv").style.display = "none";
            document.getElementById("buttonContainer").style.display = "none";
            tableBody.innerHTML = '';

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                return;
            }
            else
                tableHeader.style.removeProperty("display");

            // Get column headers
            var columns = Object.keys(data[0]);

            // Clear any existing headers
            tableHeader.innerHTML = '';
            var headerRow = document.createElement('tr');

            // Create header cells
            columns.forEach(function (col) {
                var th = document.createElement('th');
                th.textContent = col;
                headerRow.appendChild(th);
            });

            // Append the header row
            tableHeader.appendChild(headerRow);

            // Pagination logic: slice data for the current page
            var startIndex = (currentPage - 1) * rowsPerPage;
            var endIndex = startIndex + rowsPerPage;
            var pageData = data.slice(startIndex, endIndex);

            // Populate table body with rows for the current page
            pageData.forEach(function (row) {
                var tr = document.createElement('tr');
                columns.forEach(function (col) {
                    var td = document.createElement('td');

                    if (typeof row[col] === "string" && (col.toLowerCase().includes("image") || row[col].startsWith("/9j/") || row[col].startsWith("R0lGOD") || row[col].startsWith("iVBORw0KGgoAAA"))) {
                        var img = document.createElement('img');

                        // Check for JPEG image format
                        if (row[col].startsWith("/9j/")) {
                            img.src = "data:image/jpeg;base64," + row[col]; // JPEG format
                        }
                            // Check for GIF image format
                        else if (row[col].startsWith("R0lGOD")) {
                            img.src = "data:image/gif;base64," + row[col];  // GIF format
                        }
                            // Check for PNG image format
                        else if (row[col].startsWith("iVBORw0KGgoAAA")) {
                            img.src = "data:image/png;base64," + row[col];  // PNG format
                        }

                        img.style.maxWidth = "100px";
                        img.style.height = "auto";
                        td.appendChild(img);
                    }
                    else {
                        if (col.includes("image")) {
                            var img = document.createElement('img');
                            img.src = "/Images/Client.gif";
                            img.style.maxWidth = "100px";
                            img.style.height = "auto";
                            td.appendChild(img);
                        }
                        else
                            td.textContent = row[col];
                    }

                    tr.appendChild(td);
                });
                tableBody.appendChild(tr);
            });

            // Create pagination controls
            createPaginationControlsQuarter(data.length, data);
        }
        function createPaginationControlsQuarter(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    loadDataFromServerQuarter(data); // Re-load the table data for the new page
                }
            };
            paginationContainer.appendChild(prevButton);

            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + Math.ceil(totalRows / rowsPerPage);
            paginationContainer.appendChild(pageIndicator);

            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServerQuarter(data);
                }
            };
            paginationContainer.appendChild(nextButton);
            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportToExcelWithImages();
                loadDataFromServerQuarter(data);
            };
            paginationContainer.appendChild(exportButton);
        }


        //Funder Table
        var sortState = {}; // Keeps track of sort direction per funder

        function loadDataFromServerFunder(data) {
            fullData = data;
            document.getElementById("filterDiv").style.display = "none";
            document.getElementById("buttonContainer").style.display = "none";

            var grouped = {};
            data.forEach(function (row) {
                if (!grouped[row.Funder]) {
                    grouped[row.Funder] = [];
                }
                grouped[row.Funder].push({
                    ClientId: row.ClientId,
                    ClientName: row.ClientName
                });
            });

            var tableHeader = document.getElementById('tableHeader');
            var tableBody = document.getElementById('tableBody');
            tableHeader.innerHTML = "";
            tableBody.innerHTML = "";

            for (var funder in grouped) {
                var tableId = "innerTable_" + funder.replace(/\s+/g, "_"); // unique ID
                var outerRow = document.createElement('tr');
                var outerCell = document.createElement('td');
                outerCell.colSpan = 2;

                var innerTable = document.createElement('table');
                innerTable.id = tableId;
                innerTable.style.width = "60%";
                innerTable.style.tableLayout = "fixed"; // ensure fixed column width
                innerTable.style.borderCollapse = "collapse"; // cleaner layout
                innerTable.border = "1";
                innerTable.style.marginBottom = "20px";
                innerTable.style.marginLeft = "auto";  // center horizontally
                innerTable.style.marginRight = "auto";

                var funderHeaderRow = document.createElement('tr');
                var funderHeaderCell = document.createElement('th');
                funderHeaderCell.colSpan = 2;
                funderHeaderCell.innerText = funder;
                funderHeaderCell.style.backgroundColor = "#4CAF50";
                funderHeaderCell.style.color = "white";
                funderHeaderRow.appendChild(funderHeaderCell);
                innerTable.appendChild(funderHeaderRow);

                // Column headers with unique IDs
                var columnsRow = document.createElement('tr');

                var idHeader = document.createElement('th');
                idHeader.id = tableId + "_col0";
                idHeader.innerText = "Client ID ⬍";
                idHeader.style.cursor = "pointer";
                idHeader.onclick = (function (tableId, colIndex, headerId) {
                    return function () {
                        handleSort(tableId, colIndex, headerId);
                    };
                })(tableId, 0, idHeader.id);

                var nameHeader = document.createElement('th');
                nameHeader.id = tableId + "_col1";
                nameHeader.innerText = "Client Name ⬍";
                nameHeader.style.cursor = "pointer";
                nameHeader.onclick = (function (tableId, colIndex, headerId) {
                    return function () {
                        handleSort(tableId, colIndex, headerId);
                    };
                })(tableId, 1, nameHeader.id);

                columnsRow.appendChild(idHeader);
                columnsRow.appendChild(nameHeader);
                innerTable.appendChild(columnsRow);

                // Data rows
                grouped[funder].forEach(function (client) {
                    var row = document.createElement('tr');
                    var idCell = document.createElement('td');
                    idCell.innerText = client.ClientId;
                    var nameCell = document.createElement('td');
                    nameCell.innerText = client.ClientName;
                    row.appendChild(idCell);
                    row.appendChild(nameCell);
                    innerTable.appendChild(row);
                });

                outerCell.appendChild(innerTable);
                outerRow.appendChild(outerCell);
                tableBody.appendChild(outerRow);
            }
        }
        var tableSortState = {};
        function handleSort(tableId, columnIndex, headerId) {
            var key = tableId + "_" + columnIndex;
            var ascending = true;

            // Toggle sort direction
            if (tableSortState[key] !== undefined) {
                ascending = !tableSortState[key];
            }
            tableSortState = {}; // Reset state for other columns
            tableSortState[key] = ascending;

            // Reset all header arrows in this table
            var header = document.getElementById(headerId);
            var headerRow = header.parentNode;
            var headers = headerRow.getElementsByTagName("th");

            for (var i = 0; i < headers.length; i++) {
                headers[i].innerText = headers[i].innerText.replace(" ▲", "").replace(" ▼", "").replace(" ⬍", "") + " ⬍";
            }

            // Set arrow on active column
            var baseText = header.innerText.replace(" ⬍", "").replace(" ▲", "").replace(" ▼", "");
            header.innerText = baseText + (ascending ? " ▲" : " ▼");

            // Sort table
            sortHtmlTableByColumn(tableId, columnIndex, ascending);
        }
        function sortHtmlTableByColumn(tableId, columnIndex, asc) {
            var table = document.getElementById(tableId);
            if (!table) return;

            var rows = table.rows;
            var switching = true;

            while (switching) {
                switching = false;
                for (var i = 2; i < rows.length - 1; i++) {
                    var x = rows[i].getElementsByTagName("TD")[columnIndex];
                    var y = rows[i + 1].getElementsByTagName("TD")[columnIndex];

                    if (!x || !y) continue;

                    var xVal = x.innerText.toLowerCase();
                    var yVal = y.innerText.toLowerCase();

                    var shouldSwitch = asc ? xVal > yVal : xVal < yVal;

                    if (shouldSwitch) {
                        rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                        switching = true;
                        break;
                    }
                }
            }
        }

        function toggleSort(funder, column) {
            var state = sortState[funder];
            if (state.column === column) {
                state.asc = !state.asc;
            } else {
                state.column = column;
                state.asc = true;
            }

            fullData.sort(function (a, b) {
                if (a.Funder !== funder) return 0; // only sort within the matching funder

                var valA = a[column];
                var valB = b[column];

                if (typeof valA === 'string') {
                    valA = valA.toLowerCase();
                    valB = valB.toLowerCase();
                }

                if (valA < valB) return state.asc ? -1 : 1;
                if (valA > valB) return state.asc ? 1 : -1;
                return 0;
            });

            loadDataFromServerFunder(fullData);
        }


    </script>
    <script>
        //Export Feature
        
        function exportToExcelWithImages() {
            var workbook = new ExcelJS.Workbook();
            var headingDiv = document.getElementById("HeadingDiv").textContent.trim();

            var now = new Date();
            var formattedDateTime = now.toLocaleDateString().replace(/\//g, '-') + " " +
                                    now.toLocaleTimeString().replace(/:/g, '-').replace(/ /g, '');


            var worksheet = workbook.addWorksheet(formattedDateTime);

            var columns = Object.keys(fullData[0]);

            worksheet.columns = columns.map(function (col) {
                return { header: col, key: col, width: 25 };
            });
            var headerRow = worksheet.getRow(1);
            headerRow.eachCell(function (cell) {
                cell.font = { 
                    bold: true,
                    color: { argb: 'FFFFFFFF' }
                };

                cell.fill = {
                    type: 'pattern',
                    pattern: 'solid',
                    fgColor: { argb: '4CAF50' }
                };

                cell.border = {
                    top: { style: 'thin' },
                    left: { style: 'thin' },
                    bottom: { style: 'thin' },
                    right: { style: 'thin' }
                };
                cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
            });
            var imageCounter = 1;

            for (var i = 0; i < fullData.length; i++) {
                var row = fullData[i];
                var excelRow = worksheet.addRow([]); 

                for (var j = 0; j < columns.length; j++) {
                    var col = columns[j];
                    var value = row[col];

                    var cell = excelRow.getCell(j + 1);

                    // Check for image base64 string
                    if (typeof value === "string" &&
                        (value.startsWith("/9j/") || value.startsWith("iVBOR") || value.startsWith("R0lGOD"))) {

                        var mimeType = "image/jpeg";
                        if (value.startsWith("iVBOR")) mimeType = "image/png";
                        if (value.startsWith("R0lGOD")) mimeType = "image/gif";

                        var imageId = workbook.addImage({
                            base64: "data:" + mimeType + ";base64," + value,
                            extension: mimeType.split('/')[1],
                        });

                        worksheet.addImage(imageId, {
                            tl: { col: j, row: i + 1 },
                            ext: { width: 100, height: 80 }
                        });
                        cell.value = "";
                        worksheet.getRow(i + 2).height = 80;
                    } else {
                        cell.value = value;
                    }

                    cell.border = {
                        top: { style: 'thin' },
                        left: { style: 'thin' },
                        bottom: { style: 'thin' },
                        right: { style: 'thin' }
                    };
                    cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
                }
            }

            workbook.xlsx.writeBuffer().then(function (buffer) {
                saveAs(new Blob([buffer]), headingDiv + ".xlsx");
            });
        }



        function exportToExcelProgramRoster() {
            var workbook = new ExcelJS.Workbook();
            var worksheetName = new Date().toLocaleDateString().replace(/\//g, '-') + " " +
                                new Date().toLocaleTimeString().replace(/:/g, '-').replace(/ /g, '');
            var worksheet = workbook.addWorksheet(worksheetName);

            var headingDiv = document.getElementById("HeadingDiv").textContent.trim();

            var columns = Object.keys(fullData[0]);
            var colIndex = 1;
            var headerRow1 = [];
            var headerRow2 = [];

            columns.forEach(function(colName, index) {
                var colmn = worksheet.getColumn(index + 1);
                console.log(colName.length);
                colmn.width = colName.length + 1;
        });

            while (colIndex <= columns.length) {
                var col = columns[colIndex - 1];
                var parts = col.split("/");

                if (parts.length > 1) {

                    worksheet.mergeCells(1, colIndex, 1, colIndex + 3);
                    worksheet.getCell(1, colIndex).value = parts[0];
                    worksheet.getCell(1, colIndex).alignment = { vertical: 'middle', horizontal: 'center' };
                    worksheet.getCell(1, colIndex).font = { bold: true, color: { argb: 'FFFFFFFF' } };
                    worksheet.getCell(1, colIndex).fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: '4CAF50' } };
                    worksheet.getCell(1, colIndex).border = {
                        top: { style: 'thin', color: { argb: '000000' } },
                        left: { style: 'thin', color: { argb: '000000' } },
                        bottom: { style: 'thin', color: { argb: '000000' } },
                        right: { style: 'thin', color: { argb: '000000' } }
                    };

                    for (var i = 0; i < 4; i++) {
                        var subHeader = columns[colIndex - 1 + i].split("/")[1];
                        worksheet.getCell(2, colIndex + i).value = subHeader;
                        worksheet.getCell(2, colIndex + i).font = { bold: true, color: { argb: 'FFFFFFFF' } };
                        worksheet.getCell(2, colIndex + i).fill = {type: 'pattern', pattern: 'solid', fgColor: { argb: '4CAF50' }};
                        worksheet.getCell(2, colIndex + i).alignment = { vertical: 'middle', horizontal: 'center' };
                        worksheet.getCell(2,colIndex + i).border = {
                            top: { style: 'thin', color: { argb: '000000' } },
                            left: { style: 'thin', color: { argb: '000000' } },
                            bottom: { style: 'thin', color: { argb: '000000' } },
                            right: { style: 'thin', color: { argb: '000000' } }
                        };
                    }
                    colIndex += 4;
                } else {
                    worksheet.mergeCells(1, colIndex, 2, colIndex);
                    worksheet.getCell(1, colIndex).value = col;
                    worksheet.getCell(1, colIndex).alignment = { vertical: 'middle', horizontal: 'center' };
                    worksheet.getCell(1, colIndex).font = { bold: true, color: { argb: 'FFFFFFFF' } };
                    worksheet.getCell(1, colIndex).fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: '4CAF50' } };
                    worksheet.getCell(1, colIndex).border = {
                        top: { style: 'thin', color: { argb: '000000' } },
                        left: { style: 'thin', color: { argb: '000000' } },
                        bottom: { style: 'thin', color: { argb: '000000' } },
                        right: { style: 'thin', color: { argb: '000000' } }
                    };
                    colIndex++;
                }
            }

            fullData.forEach(function (rowData, rowIdx) {
                var row = worksheet.getRow(rowIdx + 3);
                Object.values(rowData).forEach(function (value, colIdx) {
                    var cell = row.getCell(colIdx + 1);

                        cell.value = value;
                        cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };

                    cell.border = {
                        top: { style: 'thin' },
                        left: { style: 'thin' },
                        bottom: { style: 'thin' },
                        right: { style: 'thin' }
                    };
                });
            });

            workbook.xlsx.writeBuffer().then(function (buffer) {
                saveAs(new Blob([buffer]), headingDiv + ".xlsx");
            });
        }

        function exportToExcelEmergency() {
            var workbook = new ExcelJS.Workbook();
            var worksheetName = new Date().toLocaleDateString().replace(/\//g, '-') + " " +
                                new Date().toLocaleTimeString().replace(/:/g, '-').replace(/ /g, '');
            var worksheet = workbook.addWorksheet(worksheetName);

            var headingDiv = document.getElementById("HeadingDiv").textContent.trim();
            var columns = Object.keys(fullData[0]);

            var headerRow1 = worksheet.getRow(1);
            var headerRow2 = worksheet.getRow(2);
            var colIdx = 1;

            columns.forEach(function (colName, index) {
                var colmn = worksheet.getColumn(index + 1);
                console.log(colName.length);
                colmn.width = colName.length + 2;
            });

            while (colIdx <= columns.length) {
                var col = columns[colIdx - 1];

                if (col === "Contact Name") {
                    worksheet.mergeCells(1, colIdx, 1, colIdx + 1);
                    headerRow1.getCell(colIdx).value = "Emergency Contact";
                    headerRow1.getCell(colIdx).alignment = { vertical: "middle", horizontal: "center" };
                    headerRow1.getCell(colIdx).font = { bold: true, color: { argb: 'FFFFFFFF' } };
                    headerRow1.getCell(colIdx).fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: '4CAF50' } };

                    headerRow2.getCell(colIdx).value = columns[colIdx - 1];
                    headerRow2.getCell(colIdx + 1).value = columns[colIdx];
                    headerRow2.getCell(colIdx).font = headerRow2.getCell(colIdx + 1).font = { bold: true, color: { argb: 'FFFFFFFF' } };
                    headerRow2.getCell(colIdx).alignment = headerRow2.getCell(colIdx + 1).alignment = { vertical: "middle", horizontal: "center" };
                    headerRow2.getCell(colIdx).fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: '4CAF50' } };

                    headerRow2.getCell(colIdx + 1).font = headerRow2.getCell(colIdx + 1).font = { bold: true, color: { argb: 'FFFFFFFF' } };
                    headerRow2.getCell(colIdx + 1).alignment = headerRow2.getCell(colIdx + 1).alignment = { vertical: "middle", horizontal: "center" };
                    headerRow2.getCell(colIdx + 1).fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: '4CAF50' } };
                    
                    colIdx += 2;
                } else if (col === "Birth Date" || col === "Age") {
                    worksheet.mergeCells(1, colIdx, 2, colIdx); 
                    var cell = headerRow1.getCell(colIdx);
                    cell.value = col;
                    cell.font = { bold: true, color: { argb: 'FFFFFFFF' } };
                    cell.alignment = { vertical: "middle", horizontal: "center" };
                    cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: '4CAF50' } };  
                    
                    colIdx++;
                } else {
                    worksheet.mergeCells(1, colIdx, 2, colIdx);
                    var cell = headerRow1.getCell(colIdx);
                    cell.value = col;
                    cell.font = { bold: true, color: { argb: 'FFFFFFFF' } };
                    cell.alignment = { vertical: "middle", horizontal: "center" };
                    cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: '4CAF50' } }; 
                    
                    colIdx++;
                }
            }

            var rowSpanMap = {};
            for (var i = 0; i < fullData.length; i++) {
                var key = fullData[i]["Client Name"];
                rowSpanMap[key] = (rowSpanMap[key] || 0) + 1;
            }

            var seenClients = {};
            var dataStartRow = 3;

            for (var i = 0; i < fullData.length; i++) {
                var rowData = fullData[i];
                var key = rowData["Client Name"];
                var row = worksheet.getRow(dataStartRow + i);
                var cellIndex = 1;

                for (var j = 0; j < columns.length; j++) {
                    var colName = columns[j];

                    if (colName === "Client Name") {
                        if (!seenClients[key]) {
                            seenClients[key] = true;

                            worksheet.mergeCells(dataStartRow + i, cellIndex, dataStartRow + i + rowSpanMap[key] - 1, cellIndex);
                            worksheet.getCell(dataStartRow + i, cellIndex).value = rowData["Client Name"];
                            worksheet.getCell(dataStartRow + i, cellIndex).alignment = { vertical: "middle", horizontal: "center" };
                            cellIndex++;

                            worksheet.mergeCells(dataStartRow + i, cellIndex, dataStartRow + i + rowSpanMap[key] - 1, cellIndex);
                            worksheet.getCell(dataStartRow + i, cellIndex).value = rowData["Birth Date"];
                            worksheet.getCell(dataStartRow + i, cellIndex).alignment = { vertical: "middle", horizontal: "center" };
                            cellIndex++;

                            worksheet.mergeCells(dataStartRow + i, cellIndex, dataStartRow + i + rowSpanMap[key] - 1, cellIndex);
                            worksheet.getCell(dataStartRow + i, cellIndex).value = rowData["Age"];
                            worksheet.getCell(dataStartRow + i, cellIndex).alignment = { vertical: "middle", horizontal: "center" };
                            cellIndex++;
                        } else {
                            cellIndex += 3;
                        }
                    } else if (colName === "Birth Date" || colName === "Age") {
                        continue;
                    } else {
                        var value = rowData[colName];
                        var cell = row.getCell(cellIndex);

                        cell.value = value;
                        cell.alignment = { vertical: "middle", horizontal: "center", wrapText: true };
                        
                        cellIndex++;
                    }
                }
            }
            var totalRows = worksheet.rowCount;

            for (var i = 1; i <= totalRows; i++) {
                var row = worksheet.getRow(i);
                row.eachCell({ includeEmpty: true }, function (cell) {
                    cell.border = {
                        top: { style: 'thin', color: { argb: '000000' } },
                        left: { style: 'thin', color: { argb: '000000' } },
                        bottom: { style: 'thin', color: { argb: '000000' } },
                        right: { style: 'thin', color: { argb: '000000' } }
                    };
                });
        }
            workbook.xlsx.writeBuffer().then(function (buffer) {
                saveAs(new Blob([buffer]), headingDiv + ".xlsx");
            });
        }

        function exportToExcelClientContactVendor() {
            var workbook = new ExcelJS.Workbook();
            var headingDiv = document.getElementById("HeadingDiv").textContent.trim();

            var now = new Date();
            var formattedDateTime = now.toLocaleDateString().replace(/\//g, '-') + " " +
                now.toLocaleTimeString().replace(/:/g, '-').replace(/ /g, '');

            var worksheet = workbook.addWorksheet(formattedDateTime);

            if (!fullData || fullData.length === 0) {
                alert("No data to export");
                return;
            }

            var columns = Object.keys(fullData[0]);

            // Set header row
            worksheet.columns = columns.map(function (col) {
                return { header: col, key: col, width: 25 };
            });

            var headerRow = worksheet.getRow(1);
            headerRow.eachCell(function (cell) {
                cell.font = { bold: true, color: { argb: 'FFFFFFFF' } };
                cell.fill = {
                    type: 'pattern',
                    pattern: 'solid',
                    fgColor: { argb: '4CAF50' }
                };
                cell.border = {
                    top: { style: 'thin' },
                    left: { style: 'thin' },
                    bottom: { style: 'thin' },
                    right: { style: 'thin' }
                };
                cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
            });

            // Group rows by Client Last
            var rowSpanMap = {};
            for (var i = 0; i < fullData.length; i++) {
                var key = fullData[i]["Client Last"];
                if (rowSpanMap[key]) {
                    rowSpanMap[key] += 1;
                } else {
                    rowSpanMap[key] = 1;
                }
            }

            var processed = {};
            var currentRowIndex = 2;

            for (var i = 0; i < fullData.length; i++) {
                var row = fullData[i];
                var key = row["Client Last"];
                var excelRow = worksheet.getRow(currentRowIndex);
                var colIndex = 1;

                if (!processed[key]) {
                    processed[key] = true;

                    var rowspan = rowSpanMap[key];
                    var mergeColumns = [
                        "Client Last",
                        "Client First",
                        "Date of Birth",
                        "Admission Date",
                        "Program and Active Placement(s)"
                    ];

                    for (var j = 0; j < mergeColumns.length; j++) {
                        var colName = mergeColumns[j];
                        var colPos = columns.indexOf(colName) + 1;

                        if (rowspan > 1) {
                            worksheet.mergeCells(currentRowIndex, colPos, currentRowIndex + rowspan - 1, colPos);
                        }

                        var cell = worksheet.getCell(currentRowIndex, colPos);
                        cell.value = row[colName];
                        cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
                        cell.border = {
                            top: { style: 'thin' },
                            left: { style: 'thin' },
                            bottom: { style: 'thin' },
                            right: { style: 'thin' }
                        };
                    }
                }

                // Write non-merged columns (excluding the 5 merged + Status)
                for (var k = 0; k < columns.length; k++) {
                    var colName = columns[k];
                    if (
                        colName !== "Client Last" &&
                        colName !== "Client First" &&
                        colName !== "Date of Birth" &&
                        colName !== "Admission Date" &&
                        colName !== "Program and Active Placement(s)" &&
                        colName !== "Status"
                    ) {
                        var cell = excelRow.getCell(k + 1);
                        cell.value = row[colName];
                        cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
                        cell.border = {
                            top: { style: 'thin' },
                            left: { style: 'thin' },
                            bottom: { style: 'thin' },
                            right: { style: 'thin' }
                        };
                    }
                }

                currentRowIndex++;
            }

            workbook.xlsx.writeBuffer().then(function (buffer) {
                saveAs(new Blob([buffer]), headingDiv + ".xlsx");
            });
        }




    </script>
    <style>
        /*Column Dropdown Styling*/


        .dropdown {
            background-color: #4CAF50;
            width:266px;
            position: relative;
            display: inline-block;
        }

        .dropdown-btn {
            background-color: #4CAF50;
            user-select: none;
            color: white;
            padding: 10px;
            font-size: 16px;
            border: none;
            pointer-events: none;
            cursor: default;
        }

        .dropdown-content {
            display: none;
            user-select: none;
            position: absolute;
            background-color: #f9f9f9;
            width: 356px;
            height: auto;
            max-height: 400px;
            overflow-y: auto;
            box-shadow: 0px 8px 16px 0px rgba(0, 0, 0, 0.2);
            z-index: 1;
            padding: 10px;
        }

        .dropdown:hover .dropdown-content {
            display: block;
        }

        .dropdown-content label {
            display: block;
            margin-bottom: 5px;
        }
    </style>
    <style>
        /*Button Styling*/
       .button-style {
    background-color: #03507D;  /* Bold Orange */
    color: white;
    font-weight: bold;
    cursor: pointer;
    text-align: center;
    display: inline-block;
    margin-top:5px;
    margin-bottom:5px;
    margin-right:5px;
    padding-top:5px;
    padding-bottom:5px;
    padding-right:10px;
    border-radius:5px;
}

.button-style:hover {
    background-color: #C74C2C;  
    transform: scale(1.05); 
}

.button-style:focus {
    outline: none;
}
    </style>
    <style>
        /*Pagination Styling*/
        .pagination-container {
            display: inline-block;
            height: 50px;
            text-align: center;
            line-height: 50px;
            background-color: #f8f8f8;
        }

            .pagination-container button {
                padding: 5px 10px;
                font-size: 14px;
                cursor: pointer;
                background-color: #03507D;
                color: white;
                border: none;
                border-radius: 4px;
                display: inline-block;
                margin: 0 5px;
            }

                .pagination-container button:disabled {
                    background-color: #ccc;
                    cursor: not-allowed;
                }
    </style>

    <script>
        $(document).ready(function () {
            $('#<%= btnallClient.ClientID %>').click(function () {
                $('#<%= dropdown_container.ClientID %>').toggle();
            });
        });

        $(document).ready(function () {
            $('#<%= btnVendor.ClientID %>').click(function () {
                $('#<%= dropdown_container.ClientID %>').toggle();
            });
        });


        function getSelectedValuesAndSend() {
            document.getElementById("btnShowReport").style.display = 'inline-block';
            document.getElementById("btnResetAllClient").style.display = 'inline-block';
            event.preventDefault();
                var selectedValues = {};

                var checkboxes = document.querySelectorAll(".filter-checkbox:checked");

                for (var i = 0; i < checkboxes.length; i++) {
                    var checkbox = checkboxes[i];
                    var column = checkbox.getAttribute("data-column");

                    var label = checkbox.closest("label");

                    var text = label ? label.textContent.trim() : ""; 


                    text = text.replace(/^\s+|\s+$/g, ""); 

                    if (!selectedValues[column]) {
                        selectedValues[column] = [];
                    }

                    selectedValues[column].push(text);
                }

                var xhr = new XMLHttpRequest();
                xhr.open("POST", "ClientReports.aspx/CreateDataTableFromSelectedValues", true);
                xhr.setRequestHeader("Content-Type", "application/json");

                xhr.onreadystatechange = function () {
                    if (xhr.readyState === 4 && xhr.status === 200) {
                        var trimmedResponse = xhr.responseText.trim();

                        if (trimmedResponse) {
                            try {
                                var jsonResponse = JSON.parse(trimmedResponse);
                                var data = JSON.parse(jsonResponse.d);
                                currentPage = 1;
                                loadDataFromServer(data);
                            } catch (e) {
                                console.error("Error parsing JSON:", e);
                            }
                        } else {
                            console.error("Empty response received.");
                        }
                    }
                };

                xhr.send(JSON.stringify({ selectedValues: selectedValues }));
        }

        function getSelectedValuesAndSendVendor() {
            document.getElementById("btnShowReportVendor").style.display = 'inline-block';
            document.getElementById("btnResetVendor").style.display = 'inline-block';
            event.preventDefault();
            var selectedValues = {};

            var checkboxes = document.querySelectorAll(".filter-checkbox:checked");

            for (var i = 0; i < checkboxes.length; i++) {
                var checkbox = checkboxes[i];
                var column = checkbox.getAttribute("data-column");
                if (column == "Status")
                    var text = checkbox.value;
                else {
                    var label = checkbox.closest("label");

                    var text = label ? label.textContent.trim() : "";


                    text = text.replace(/^\s+|\s+$/g, "");
                }
                if (!selectedValues[column]) {
                    selectedValues[column] = [];
                }
                selectedValues[column].push(text);
            }

            var xhr = new XMLHttpRequest();
            xhr.open("POST", "ClientReports.aspx/CreateDataTableFromSelectedValuesVendor", true);
            xhr.setRequestHeader("Content-Type", "application/json");

            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var trimmedResponse = xhr.responseText.trim();

                    if (trimmedResponse) {
                        try {
                            var jsonResponse = JSON.parse(trimmedResponse);
                            var data = JSON.parse(jsonResponse.d);
                            currentPage = 1;
                            loadDataFromServerVendor(data);
                        } catch (e) {
                            console.error("Error parsing JSON:", e);
                        }
                    } else {
                        console.error("Empty response received.");
                    }
                }
            };

            xhr.send(JSON.stringify({ selectedValues: selectedValues }));
        }
    </script>

    <style type="text/css">
        .leftMenu:active {
            background-position: 0 0;
        }
    </style>


</head>







<body>
    <form id="FormClientReport" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:HiddenField ID="hdnType" runat="server" />
        </div>
        <div class="mainContainer">

            <div class="topHead">
                <a class="admin" href="#">

                     <% sess = (clsSession)Session["UserSessionClient"]; %>
                    <%=sess.UserName %>



                </a>
                <a class="logout" href="../../../Login.aspx">Logout</a>
                <a class="Report" href="../Reports/ClientReports.aspx">Reports</a>
                <a class="home" href="../Client/Index">Home</a>
            </div>
            <div class="contentPart">
                <div class="imgcorner">
                    <a class="logo" href="#">
                        <img src="../Documents/images/logo.jpg" width="200" height="40" /></a>
                </div>
                <div class="ContentAreaContainer">
                    <div class="leftContainer2" style="width: 23%">

                        <asp:CheckBox ID="checkHighcharts" runat="server" />
                        <asp:Button ID="btnallClient" CssClass="leftMenu" runat="server" Text="All Clients Info" ToolTip="All Clients Info" OnClick="btnallClient_Click"></asp:Button>

                        <asp:Button ID="btnClienContact" CssClass="leftMenu" runat="server" Text="Emergency/Home Contact" ToolTip="Emergency/Home Contact" OnClick="btnClienContact_Click"></asp:Button>

                        <%--                                    <asp:Button ID="btnClientContactRes" CssClass="leftMenu" runat="server" Text="Emergency/Home Contact – Residence Only" ToolTip="Emergency/Home Contact – Residence Only"   ></asp:Button>--%>

                        <asp:Button ID="btnPgmRoster" CssClass="leftMenu" runat="server" Text="Program Roster" ToolTip="Program Roster" OnClick="btnPgmRoster_Click"></asp:Button>

                        <asp:Button ID="btnVendor" runat="server" CssClass="leftMenu" Text="Client/Contact/Vendor" ToolTip="Client/Contact/Vendor" OnClick="btnVendor_Click"></asp:Button>

                        <%--<asp:Button ID="btnVenderDischarged" runat="server" CssClass="leftMenu" Text="Client/Contact/Vendor – Discharged" ToolTip="Client/Contact/Vendor – Discharged"   ></asp:Button>--%>

                        <asp:Button ID="btnBirthdate" runat="server" CssClass="leftMenu" Text="All Clients by Birthdate Quarter" ToolTip="All Clients by Birthdate Quarter" OnClick="btnBirthdate_Click"></asp:Button>

                        <asp:Button ID="btnResRoster" runat="server" CssClass="leftMenu" Text=" Residential Roster Report" ToolTip=" Residential Roster Reports" OnClick="btnResRoster_Click"></asp:Button>
                        <asp:Button ID="btnAllFunder" runat="server" CssClass="leftMenu" Text="All Clients by Funder" ToolTip="All Clients by Funder" OnClick="btnAllFunder_Click"></asp:Button>
                        <asp:Button ID="btnAllPlacement" runat="server" CssClass="leftMenu" Text="All Clients by Placement" ToolTip="All Clients by placement" OnClick="btnAllPlacement_Click"></asp:Button>
                        <asp:Button ID="btnAllBirthdate" runat="server" CssClass="leftMenu" Text="All Clients by Birthdate" ToolTip="All Clients by Birthdate" OnClick="btnAllBirthdate_Click"></asp:Button>
                        <asp:Button ID="btnAllAdmissionDate" runat="server" CssClass="leftMenu" Text="All Clients by Admission date" ToolTip="All Clients by Admission date" OnClick="btnAllAdmissionDate_Click"></asp:Button>
                        <asp:Button ID="btnAllDischargedate" runat="server" CssClass="leftMenu" Text="All Clients by Discharge date" ToolTip="All Clients by Discharge date" OnClick="btnAllDischargedate_Click"></asp:Button>
                        <asp:Button ID="btnStatistical" runat="server" CssClass="leftMenu" Text="Statistical Report" ToolTip="Statistical Report" OnClick="btnStatistical_Click"></asp:Button>
                        <asp:Button ID="btnFundChange" runat="server" CssClass="leftMenu" Text="Funding Changes" ToolTip="Funding Changes" OnClick="btnFundChange_Click" OnClientClick="resetVal();"></asp:Button>
                        <asp:Button ID="btnPlcChange" runat="server" CssClass="leftMenu" Text="Placement Changes" ToolTip="Placement Changes" OnClick="btnPlcChange_Click" OnClientClick="resetVal();"></asp:Button>
                        <asp:Button ID="btnGuardianChanges" runat="server" CssClass="leftMenu" Text="Guardianship Changes" ToolTip="Guardianship Changes" OnClick="btnGuardianChanges_Click" OnClientClick="resetVal();"></asp:Button>
                        <asp:Button ID="btnContactChanges" runat="server" CssClass="leftMenu" Text="Contact Changes" ToolTip="Contact Changes" OnClick="btnContactChanges_Click" OnClientClick="resetVal();"></asp:Button>
                    </div>


                    <div class="middleContainer" style="width: 75%">

                        <div id="content">
                            <div class="headingDivBar" style="width: 100%" id="HeadingDiv" runat="server" visible="false">
                            </div>
                            <div style="float: left; width: 100%" id="tdMsg" runat="server" visible="false">
                            </div>

                                 <%--Code design for contact report--%>                       

                                <div runat="server" id="divContact" visible="false">
                                        <asp:UpdatePanel ID="UpdatePanelContact" runat="server">
                                        <ContentTemplate>
                                <table>                                
                                    <tr>
                                    
                                         <td>
                                             <asp:Label ID="Labelclient" runat="server" ForeColor="Black" Text="Client:"></asp:Label>
                                         </td>
                                         <td>
                                            <asp:DropDownCheckBoxes ID="DropDownCheckBoxesConStudname" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesConStudname_SelectedIndexChanged">
                                            <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="190" DropDownBoxCssClass="ddchkLesson"/><Texts SelectBoxCaption="All"/>
                                            </asp:DropDownCheckBoxes>                                        

                                         </td>

                                         <td>
                                             <asp:Label ID="Labelrelation" runat="server" ForeColor="Black" Text="Relationship:"></asp:Label>
                                         </td>
                                         <td>    
                                            <asp:DropDownCheckBoxes ID="DropDownCheckBoxesRelation" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesRelation_SelectedIndexChanged">
                                            <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="190" DropDownBoxCssClass="ddchkLesson"/><Texts SelectBoxCaption="All"/>
                                            </asp:DropDownCheckBoxes>                                                  
                                         </td>

                                         <td>
                                            <asp:CheckBoxList ID="CheckBoxListcontact" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True">Active</asp:ListItem>
                                            <%--<asp:ListItem>Inactive</asp:ListItem>--%>
                                            <asp:ListItem>Discharged</asp:ListItem>
                                            </asp:CheckBoxList>
                                         </td>
                                        
                                        <td><asp:Button ID="Btncontact" runat="server" Text="Show Report" OnClick="btnShowVendor_Click" Width="120px" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" />
                                        </td>
                                        <td><asp:Button ID="BtncontactReset" runat="server" Text="Reset" OnClick="btnVendor_Click" Width="120px" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" />
                                        </td>
                                    </tr>

                                    <tr>
                                    <td><asp:HiddenField runat="server" ID="HContactRelation"/></td>
                                    <td><asp:HiddenField runat="server" ID="HContactstatus"/></td>
                                    <td><asp:HiddenField runat="server" ID="HContactStudname"/></td>
                                    </tr>

                                    </table>
                                         </ContentTemplate>
                                         </asp:UpdatePanel>                     
                                 </div>
                            <%--Code design end for contact report--%>

                            <div>

                                <div id="divbirthdate" runat="server" visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%">
                                                <asp:Label ID="Label1" runat="server" Text="Birthdate Quarter"></asp:Label>
                                            </td>
                                            <td style="width: 25%">
                                                <asp:DropDownList ID="ddlQuarter" runat="server">
                                                    <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                                    <asp:ListItem Value="1">January - March</asp:ListItem>
                                                    <asp:ListItem Value="2">April - June</asp:ListItem>
                                                    <asp:ListItem Value="3">July - September</asp:ListItem>
                                                    <asp:ListItem Value="4">October - December</asp:ListItem>
                                                </asp:DropDownList>


                                            </td>
                                            <td>
                                                <asp:Button ID="btnquarter" runat="server" Text="Show Report" OnClick="btnquarter_Click" Width="120px" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPlacement" runat="server" visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="chb" value="Choose Department and Location" id="rbtnDeptLoc" />
                                                        Choose Department and Location
       
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Department</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlDeptLocDept" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Location</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlDeptLocLoc" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="chb" value="Choose Department and Placement Type" id="rbtnDeptPlaceType" />
                                                        Choose Department and Placement Type
     
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Department</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlDeptPlctypeDept" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Placement Type</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlDeptPlctypePlcType" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="chb" value="Choose Location" id="rbtnLocation" />
                                                        Choose Location
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Location</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlLocLoc" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="DateRange" value="Active Placement" id="rbtnActivePlc" />
                                                        Active Placement</legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Start Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtActiveStartDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>End Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtActiveEndDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="DateRange" value="Discharged Placement" id="rbtnDischargedPlc" />
                                                        Discharged Placement
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Start Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtDischrStartDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>End Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtDischrEndDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="DateRange" value="New Placement" id="rbtnNewPlacement" />
                                                        New Placement
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Start Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtNewStartDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>End Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtNewEndDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td>

                                                <asp:Button ID="btnShowPlacement" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowPlacement_Click" OnClientClick="return GetValidate();" />

                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div id="divFunder" runat="server" visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%">Funding Source</td>
                                            <td style="width: 20%">
                                                <asp:DropDownList ID="ddlFundingSource" runat="server" Width="220px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 65%">
                                                <asp:Button ID="btnShowFunder" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowFunder_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" visible="false" id="divbyBirthdate">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>Month</td>
                                            <td style="width: 10%">
                                                <asp:DropDownList ID="ddlMonth" runat="server" Width="130px">
                                                    <asp:ListItem Value="0">------Select------</asp:ListItem>
                                                    <asp:ListItem>January</asp:ListItem>
                                                    <asp:ListItem>February</asp:ListItem>
                                                    <asp:ListItem>March</asp:ListItem>
                                                    <asp:ListItem>April</asp:ListItem>
                                                    <asp:ListItem>May</asp:ListItem>
                                                    <asp:ListItem>June</asp:ListItem>
                                                    <asp:ListItem>July</asp:ListItem>
                                                    <asp:ListItem>August</asp:ListItem>
                                                    <asp:ListItem>September</asp:ListItem>
                                                    <asp:ListItem>October</asp:ListItem>
                                                    <asp:ListItem>November</asp:ListItem>
                                                    <asp:ListItem>December</asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td></td>
                                            <td style="width: 8%">Age From</td>
                                            <td style="width: 10%">
                                                <asp:TextBox ID="txtAgeFrom" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                            <td></td>
                                            <td style="width: 4%">To</td>
                                            <td>
                                                <asp:TextBox ID="txtAgeTo" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Start Date</td>
                                            <td>
                                                <asp:TextBox ID="txtBithdateStart" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td>End Date</td>
                                            <td>
                                                <asp:TextBox ID="txtBirthdateEnd" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btnShowBirthdate" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowBirthdate_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div runat="server" visible="false" id="divAdmission">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>Admission Date From</td>
                                            <td>
                                                <asp:TextBox ID="txtAdmissionFrom" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td>To</td>
                                            <td>
                                                <asp:TextBox ID="txtAdmissionTo" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td>Number of Admissions</td>
                                            <td>
                                                <asp:TextBox ID="txtNumberOfAdmission" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btnShowAdmissionDate" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowAdmissionDate_Click" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" visible="false" id="divDischarge">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 20%">
                                                <%--<asp:RadioButtonList ID="rbtnDischargeStatus" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="A">Active</asp:ListItem>
                                                    <asp:ListItem Value="I">Inactive</asp:ListItem>
                                                </asp:RadioButtonList></td>--%>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Button ID="btnShowDischarge" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowDischarge_Click" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" id="divStatistical" visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkStatisticalList" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True">Total number of client</asp:ListItem>
                                                    <asp:ListItem Selected="True">Gender</asp:ListItem>
                                                    <asp:ListItem Selected="True">Department</asp:ListItem>
                                                    <asp:ListItem Selected="True">Placement Type</asp:ListItem>
                                                    <asp:ListItem Selected="True">Program</asp:ListItem>
                                                    <asp:ListItem Selected="True">Location</asp:ListItem>
                                                    <asp:ListItem Selected="True">Race</asp:ListItem>
                                                    <asp:ListItem Selected="True">Maximum client occupancy</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="text-align: right">

                                                <asp:Button ID="btnShowStatistical" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowStatistical_Click" />

                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" id="divStatisticalNew" visible="false">
                                    <div id="tesss">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Student Name:</span><br />
                                                        <asp:DropDownCheckBoxes ID="DropDownCheckBoxesStudname" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesStudname_SelectedIndexChanged">
                                                        <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="190" DropDownBoxCssClass="ddchkLesson"/>
                                                        </asp:DropDownCheckBoxes>
                                                    </td>
                                                    <td>
                                                        <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Location:</span><br /> 
                                                        <asp:DropDownCheckBoxes ID="DropDownCheckBoxesLocation" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesLocation_SelectedIndexChanged">
                                                        <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="190" DropDownBoxCssClass="ddchkLesson"/>
                                                        </asp:DropDownCheckBoxes>
                                                    </td>
                                                    <td>
                                                        <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Races:</span><br /> 
                                                        <asp:DropDownCheckBoxes ID="DropDownCheckBoxesRaces" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesRaces_SelectedIndexChanged">
                                                        <Style SelectBoxWidth="275" DropDownBoxBoxWidth="240" DropDownBoxBoxHeight="90" DropDownBoxCssClass="ddchkLesson"/>
                                                        </asp:DropDownCheckBoxes>
                                                    </td>
                                                    <td>
                                                        <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Status:</span><br /> 
                                                        <asp:DropDownCheckBoxes ID="DropDownCheckBoxesActive" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesActive_SelectedIndexChanged">
                                                        <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="50" DropDownBoxCssClass="ddchkLesson"/>
                                                        <%--<Items>
                                                        <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                                        <asp:ListItem Text="Discharged" Value="D"></asp:ListItem> 
                                                        </Items>--%>
                                                        </asp:DropDownCheckBoxes>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><asp:HiddenField runat="server" ID="hfstudname"/></td>
                                                    <td><asp:HiddenField runat="server" ID="hflocation"/></td>
                                                    <td><asp:HiddenField runat="server" ID="hfrace"/></td>
                                                    <td><asp:HiddenField runat="server" ID="hfstatus"/></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        </asp:UpdatePanel> 
                                    </div>
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                               <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Show Labels:</span><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkStatisticalList2" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True">Total number of client</asp:ListItem>
                                                    <asp:ListItem Selected="True">Student Name</asp:ListItem>
                                                    <asp:ListItem Selected="True">Location</asp:ListItem>
                                                    <asp:ListItem Selected="True">City</asp:ListItem>
                                                    <asp:ListItem Selected="True">State</asp:ListItem>
                                                    <asp:ListItem Selected="True">Primary Language</asp:ListItem>
                                                    <asp:ListItem Selected="True">Race</asp:ListItem>
                                                    <asp:ListItem Selected="True">Placement Type</asp:ListItem>
                                                    <asp:ListItem Selected="True">Department</asp:ListItem>
                                                    <asp:ListItem Selected="True">Program</asp:ListItem>
                                                    <asp:ListItem Selected="True">Gender</asp:ListItem>
                                                    <asp:ListItem Selected="True">Status</asp:ListItem>
                                                    <%--<asp:ListItem Selected="True" style="display:none;">Maximum client occupancy</asp:ListItem>--%>
                                                </asp:CheckBoxList>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Button ID="btnShowStatistical2" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowStatistical2_Click" />
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnReset_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div runat="server" id="divnodata" visible="false">
                                        <p style="text-align:center;vertical-align:central">Please select report items</p>
                                    </div>
                                </div>
                                <div runat="server" id="divchanges" visible="false">
                                    <table style="width:100%">
                                          <tr>
                                            <td>Start Date</td>
                                            <td>
                                                <asp:TextBox ID="txtchangeSdate" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td>End Date</td>
                                            <td>
                                                <asp:TextBox ID="txtchangeEdate" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btnChangeResult" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnChangeResult_Click" OnClientClick="return ValidateChanges();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:HiddenField ID="hdnballet" runat="server" />
                                    <asp:HiddenField ID="hdnDateRange" runat="server" />
                                </div>
                            </div>
                            <div style="width: 100%; overflow-x: auto">
                                <rsweb:ReportViewer ID="RVClientReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="true" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false" SizeToReportContent="true" Width="100%" Visible="false" AsyncRendering="true" Height="1000px">

                                    <ServerReport ReportServerUrl="<%$ appSettings:ReportUrl %>" />

                                </rsweb:ReportViewer>
                                <div id="dropdown_container" runat="server">
                                    </div>
                                <div id="buttonContainer" style="text-align: left; width:270px; height:25px;">
                                    <asp:Button ID="btnShowReport" CssClass="button-style" runat="server" Visible="false" Text="Show Report" OnClientClick="getSelectedValuesAndSend();" />
                                    <asp:Button ID="btnResetAllClient" CssClass="button-style" runat="server" Visible="false" Text="Reset" OnClick="btnallClient_Click" />
                                    <asp:Button ID="btnShowReportVendor" CssClass="button-style" runat="server" Visible="false" Text="Show Report" OnClientClick="getSelectedValuesAndSendVendor();" />
                                    <asp:Button ID="btnResetVendor" CssClass="button-style" runat="server" Visible="false" Text="Reset" OnClick="btnVendor_Click" />
                                    <%--<asp:Button ID="btnOldReport" CssClass="button-style" runat="server" Visible="false" Text="Old Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnOldReport_Click" />--%>

                                </div>
                                <div id="filterDiv"></div>
                                <asp:Label ID="noOfClients" runat="server" Text="" style="font-weight: bold; font-size:18px; color:black;"></asp:Label>
                                <div id="paginationControls" class="pagination-container"></div>
                                <table id="table">
                                    <thead id="tableHeader">
                                        <!-- Column headers will be dynamically added here -->
                                    </thead>
                                    <tbody id="tableBody">
                                        <!-- Data rows will be dynamically added here -->
                                    </tbody>
                                </table>
                                
                            </div>




                    </div>





                    <div class="clear">
                        <asp:HiddenField ID="hdnMenu" runat="server" />
                    </div>
                </div>

                <div class="clear"></div>
            </div>

            <div class="clear"></div>
            <div class="footer">

                <img src="../../Documents/images/smllogo.JPG" width="100" height="23" />
                <div class="copyright">&copy; Copyright 2015, Melmark, Inc. All rights reserved.</div>
            </div>


            <div class="clear"></div>
        </div>

    </form>

    <script type="text/javascript">
        $(document).ready(function () {

            var MenuType = document.getElementById('hdnMenu').value;
            if (MenuType != "") {
                ChangeSelectedMenu(MenuType);
            }
        });
    </script>
</body>
<script type="text/javascript">
    $(document).ready(function () {

        $("#ddlDeptLocDept").prop('disabled', true);
        $("#ddlDeptLocLoc").prop('disabled', true);
        $("#ddlDeptPlctypeDept").prop('disabled', true);
        $("#ddlDeptPlctypePlcType").prop('disabled', true);
        $("#ddlLocLoc").prop('disabled', true);
        $("#txtActiveStartDate").prop('disabled', true);
        $("#txtActiveEndDate").prop('disabled', true);
        $("#txtNewStartDate").prop('disabled', true);
        $("#txtNewEndDate").prop('disabled', true);
        $("#txtDischrStartDate").prop('disabled', true);
        $("#txtDischrEndDate").prop('disabled', true);

        if ($('#hdnballet').val() != "") {
            if ($('#hdnballet').val() == "Choose Department and Location") {
                $('#rbtnDeptLoc').attr('checked', true);
                $("#ddlDeptLocDept").prop('disabled', false);
                $("#ddlDeptLocLoc").prop('disabled', false);
            }
            else if ($('#hdnballet').val() == "Choose Department and Placement Type") {
                $('#rbtnDeptPlaceType').attr('checked', true);
                $("#ddlDeptPlctypeDept").prop('disabled', false);
                $("#ddlDeptPlctypePlcType").prop('disabled', false);
            }
            else if ($('#hdnballet').val() == "Choose Location") {
                $('#rbtnLocation').attr('checked', true);
                $("#ddlLocLoc").prop('disabled', false);
            }

        }
        if ($('#hdnDateRange').val() != "") {
            if ($('#hdnDateRange').val() == "Active Placement") {
                $('#rbtnActivePlc').attr('checked', true);
                $("#txtActiveStartDate").prop('disabled', false);
                $("#txtActiveEndDate").prop('disabled', false);
            }
            else if ($('#hdnDateRange').val() == "Discharged Placement") {
                $('#rbtnDischargedPlc').attr('checked', true);
                $("#txtDischrStartDate").prop('disabled', false);
                $("#txtDischrEndDate").prop('disabled', false);
            }
            else if ($('#hdnDateRange').val() == "New Placement") {
                $('#rbtnNewPlacement').attr('checked', true);
                $("#txtNewStartDate").prop('disabled', false);
                $("#txtNewEndDate").prop('disabled', false);
            }
        }

        var date = new Date();
        date.setDate(date.getDate());
        $('.datepicker').datepicker(
         {
             dateFormat: "mm/dd/yy",
             changeMonth: true,
             changeYear: true,
             showAnim: "fadeIn",
             yearRange: 'c-100:c+100',
             //minDate: date,
             /* fix buggy IE focus functionality */
             fixFocusIE: false,
             constrainInput: false
         });


        $(".chb").click(function () {

            if ($(this).prop('checked') != false) {
                $(".chb").prop('checked', false);
                $(this).prop('checked', true);
            }
            else {
                $('#hdnballet').val("");
            }
            if ($('#rbtnDeptLoc').prop('checked') == true) {
                $('#hdnballet').val($('#rbtnDeptLoc').val());
                $("#ddlDeptLocDept").prop('disabled', false);
                $("#ddlDeptLocLoc").prop('disabled', false);
            }
            else {
                $('#ddlDeptLocDept').prop('selectedIndex', 0);
                $('#ddlDeptLocLoc').prop('selectedIndex', 0);
                $("#ddlDeptLocDept").prop('disabled', true);
                $("#ddlDeptLocLoc").prop('disabled', true);
            }

            if ($('#rbtnDeptPlaceType').prop('checked') == true) {

                $('#hdnballet').val($('#rbtnDeptPlaceType').val());
                $("#ddlDeptPlctypeDept").prop('disabled', false);
                $("#ddlDeptPlctypePlcType").prop('disabled', false);
            }
            else {
                $('#ddlDeptPlctypeDept').prop('selectedIndex', 0);
                $('#ddlDeptPlctypePlcType').prop('selectedIndex', 0);
                $("#ddlDeptPlctypeDept").prop('disabled', true);
                $("#ddlDeptPlctypePlcType").prop('disabled', true);
            }

            if ($('#rbtnLocation').prop('checked') == true) {
                $('#hdnballet').val($('#rbtnLocation').val());
                $("#ddlLocLoc").prop('disabled', false);
            }
            else {
                $('#ddlLocLoc').prop('selectedIndex', 0);
                $("#ddlLocLoc").prop('disabled', true);
            }

        });



        $(".DateRange").click(function () {

            if ($(this).prop('checked') != false) {
                $(".DateRange").prop('checked', false);
                $(this).prop('checked', true);
            }
            else {
                $('#hdnDateRange').val("");
            }

            if ($('#rbtnActivePlc').prop('checked') == true) {
                $('#hdnDateRange').val($('#rbtnActivePlc').val());
                $("#txtActiveStartDate").prop('disabled', false);
                $("#txtActiveEndDate").prop('disabled', false);
            }
            else {
                $("#txtActiveStartDate").prop('disabled', true);
                $("#txtActiveEndDate").prop('disabled', true);
                $("#txtActiveStartDate").val('');
                $("#txtActiveEndDate").val('');
            }

            if ($('#rbtnDischargedPlc').prop('checked') == true) {
                $('#hdnDateRange').val($('#rbtnDischargedPlc').val());
                $("#txtDischrStartDate").prop('disabled', false);
                $("#txtDischrEndDate").prop('disabled', false);
            }
            else {
                $("#txtDischrStartDate").prop('disabled', true);
                $("#txtDischrEndDate").prop('disabled', true);
                $("#txtDischrStartDate").val('');
                $("#txtDischrEndDate").val('');
            }

            if ($('#rbtnNewPlacement').prop('checked') == true) {
                $('#hdnDateRange').val($('#rbtnNewPlacement').val());
                $("#txtNewStartDate").prop('disabled', false);
                $("#txtNewEndDate").prop('disabled', false);
            }
            else {
                $("#txtNewStartDate").prop('disabled', true);
                $("#txtNewEndDate").prop('disabled', true);
                $("#txtNewStartDate").val('');
                $("#txtNewEndDate").val('');
            }


        });


        $.get("../ClientRegistration/GetTitleReport", function (data) {
            document.title = data;

        });



    });
</script>
























</html>

