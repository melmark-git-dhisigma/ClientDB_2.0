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

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/canvg/3.0.10/umd.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://unpkg.com/svg2pdf.js/dist/svg2pdf.umd.min.js"></script>
     <script src="../Documents/highcharts/7.1.2/highcharts.js"></script>
   <script src="../Documents/highcharts/7.1.2/modules/accessibility.js"></script>
    <script src="../Documents/highcharts/7.1.2/grouped-categories.js"></script>
     <script src="../Documents/highcharts/7.1.2/modules/exporting.js"></script>
         <script src="../Documents/highcharts/7.1.2/modules/offline-exporting.js"></script>

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




        .hc-x-label { display:inline-block; text-align:center; line-height:1; }
        .hc-x-label .quarter { font-weight:600; font-size:12px; display:block; }
        .hc-x-label .year    { font-size:11px; color:#666; display:block; margin-top:2px; }


        .highcharts-data-labels span,
		.highcharts-data-labels div {
		    pointer-events: none !important;
		}

		.highcharts-tooltip {
			  z-index: 2147483647 !important;  /* very high so tooltip sits above chart elements */
			  pointer-events: none;            /* avoid interfering with mouse events */
		}


    </style>
    <script type="text/javascript">
        var columndata={};

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
            return handleClientClick();
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
            if (elmId == "btnPlacementPlanning") {
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "none");
                $('#chartContainer').css("display", "block");
                $('#chartContainer').load('../Reports/PlacementPlanningChart'); // your new chart endpoint
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
            return handleClientClick();
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
        /*Loader Styling*/
        .loader-overlay {
            position: absolute; /* Scope to parent */
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(255, 255, 255, 0.8);
            display: flex;
            justify-content: center;
            align-items: center;
            opacity: 0;
            pointer-events: none;
            transition: opacity 0.3s ease;
            z-index: 100;
        }

            .loader-overlay.visible {
                opacity: 1;
                pointer-events: auto;
            }

        .loader-text {
            font-size: 1.5rem;
            color: #333;
        }

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
        function showLoader() {
            document.getElementById("loaderOverlay").classList.add("visible");
        }

        function hideLoader() {
            document.getElementById("loaderOverlay").classList.remove("visible");
        }

        function handleClientClick() {
            var checkbox = document.getElementById('<%= checkHighcharts.ClientID %>');
            if (checkbox && checkbox.checked) {
                columndata={};
                showLoader();
            }
            return true;
        }

        function handleClientClick1() {
             document.getElementById("hfAgeFrom").value='';
             document.getElementById("hfAgeTo").value='';
             var filt = document.getElementById("agefilter");
             if (filt) {
                 filt.style.display = "block"; 
             }
             var exp = document.getElementById("exportChartBtn");
             if (exp) {
                 exp.style.display = "block"; 
             }
            var checkbox = document.getElementById('<%= checkHighcharts.ClientID %>');
            if (checkbox && checkbox.checked) {
                columndata={};
                showLoader();
            }
            return true;
        }

        var currentPage = 1;
        var rowsPerPage = 10;
        var fullData = [];
        function loadDataFromServer(data,checkstat) {
            fullData = data;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            tableBody.innerHTML = '';

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                document.getElementById("noOfClients").textContent = "Total No. of Clients : 0";
                var paginationContainer = document.getElementById("paginationControls");
                paginationContainer.innerHTML = '';
                hideLoader();
                return;
            }
            else
                tableHeader.style.removeProperty("display");

            // Get column headers
            var columns = Object.keys(data[0]);
            var headingDiv = document.getElementById("HeadingDiv").textContent.trim();
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
            
            if(checkstat===true)
            {
            createColumnVisibilityCheckboxes(columns);
            }

            // Create pagination controls
            createPaginationControls(data.length, data);

            //Display count of clients
            document.getElementById("noOfClients").textContent = "Total No. of Clients : " + data.length;
            hideLoader();
        }

        function createPaginationControls(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';


            // First button
            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.type = 'button';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    loadDataFromServer(data,false);
                    hideandshowcolumn();
                }
            };
            paginationContainer.appendChild(firstButton);

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.type = 'button';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    loadDataFromServer(data,false); // Re-load the table data for the new page
                    hideandshowcolumn();
                }
            };
            paginationContainer.appendChild(prevButton);

            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + Math.ceil(totalRows / rowsPerPage);
            paginationContainer.appendChild(pageIndicator);

            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.type = 'button';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServer(data,false);
                    hideandshowcolumn();
                }
            };
            paginationContainer.appendChild(nextButton);

            // Last button
            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.type = 'button';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    loadDataFromServer(data, false);
                    hideandshowcolumn();
                }
            };
            paginationContainer.appendChild(lastButton);

            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.type = 'button';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportToExcelWithImages();
                //loadDataFromServer(data);
                document.getElementById("filterDiv").style.display = 'block';
                document.getElementById("buttonContainer").style.display = 'block';
                document.getElementById("btnShowReport").style.display = 'inline-block';
                document.getElementById("btnResetAllClient").style.display = 'inline-block';
            };
            paginationContainer.appendChild(exportButton);

            var exportPdfBtn = document.createElement('button');
            exportPdfBtn.type = 'button';
            exportPdfBtn.textContent = 'Export PDF';
            exportPdfBtn.onclick = function () {
                // call the fit export
                exportPlacementWithSvgLabelsAndPdf();
            };
            paginationContainer.appendChild(exportPdfBtn);
        }
        function hideandshowcolumn()
        {
            var newcol=[];
            for (var key in columndata) {
                if(columndata[key]==false){
                    newcol.push(key);
                }
            }
            newcol.forEach(function(columnName) {
                var table = document.getElementById("table");
                var columnIndex = Array.from(table.rows[0].cells).findIndex(function (cell) {
                    return cell.textContent.replace(" ⬍", "") === columnName;
                });
                var checkbox=columndata[columnName];
                Array.from(table.rows).forEach(function (row) {
                    if (checkbox.checked) {
                        row.cells[columnIndex].style.display = '';
                    } else {
                        row.cells[columnIndex].style.display = 'none';
                    }
                });
            });
        
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
                
                columndata[col] = true;

               
                filterDiv.appendChild(checkboxLabel);
            });
        }


        function toggleColumnVisibility(columnName, checkbox) {
            var table = document.getElementById("table");
            var columnIndex = Array.from(table.rows[0].cells).findIndex(function (cell) {
                return cell.textContent.replace(" ⬍", "") === columnName;
            });
            columndata[columnName] = checkbox.checked;
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
                var paginationContainer = document.getElementById("paginationControls");
                paginationContainer.innerHTML = '';
                hideLoader();
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
            hideLoader();
        }

        function createPaginationControlsEmergency(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';


            // First button
            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.type = 'button';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    loadDataFromServerEmergency(data);
                }
            };
            paginationContainer.appendChild(firstButton);

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.type = 'button';
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

            // Last button
            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.type = 'button';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    loadDataFromServerEmergency(data);
                }
            };
            paginationContainer.appendChild(lastButton);

            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.type = 'button';
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
                var paginationContainer = document.getElementById("paginationControls");
                paginationContainer.innerHTML = '';
                hideLoader();
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
            hideLoader();
        }

        function createPaginationControlsProgramRoster(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';

            // First button
            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    loadDataFromServerProgramRoster(data);
                }
            };
            paginationContainer.appendChild(firstButton);


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

            // Last button
            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    loadDataFromServerProgramRoster(data);
                }
            };
            paginationContainer.appendChild(lastButton);

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
            rowsPerPage = 10;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            tableBody.innerHTML = '';
            var table = document.getElementById("table");
            table.style.tableLayout = "auto";
            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                var paginationContainer = document.getElementById("paginationControls");
                paginationContainer.innerHTML = '';
                hideLoader();
                return;
            }
            else
                tableHeader.style.removeProperty("display");

            // Get column headers
            var columns = Object.keys(data[0]);
            columns = columns.filter(function(item) {
                return item !== "ID";
            });            // Clear any existing headers
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
                var key = pageData[i]["ID"]; // Grouping key
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
                var key = pageData[i]["ID"]; // Grouping key

                for (var index = 0; index < columns.length; index++) {
                    var columnName = columns[index];
                    var td = document.createElement('td');

                    // Merge "Client Name", "Birth Date", and "Age" together
                    if (columnName === "Client Last") {
                        if (!seen[key]) { // Only add merged cell if not already seen
                            seen[key] = true; // Mark as seen
                            //td.textContent = pageData[i]["ID"];
                            //td.setAttribute("rowspan", rowSpanMap[key]); // Set rowspan for correct merging
                            //tr.appendChild(td);

                            //var tdClientlast = document.createElement("td");
                            td.textContent = pageData[i]["Client Last"];
                            td.setAttribute("rowspan", rowSpanMap[key]);
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
                    else if ( columnName !== "Client First" && columnName !== "Date of Birth" && columnName !== "Admission Date" && columnName !== "Program and Active Placement(s)" && columnName !== "Status") {
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
            hideLoader();
        }
        function createPaginationControlsVendor(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';


            // First button
            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.type='button';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    loadDataFromServerVendor(data);
                }
            };
            paginationContainer.appendChild(firstButton);

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.type='button';
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
            nextButton.type='button';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServerVendor(data);
                }
            };
            paginationContainer.appendChild(nextButton);


            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.type='button';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    loadDataFromServerVendor(data);
                }
            };
            paginationContainer.appendChild(lastButton);
            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.type='button';
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
            else 
            rowsPerPage = 10;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            document.getElementById("filterDiv").style.display = "none";
            document.getElementById("buttonContainer").style.display = "none";
            tableBody.innerHTML = '';

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                var paginationContainer = document.getElementById("paginationControls");
                paginationContainer.innerHTML = '';
                hideLoader();
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
            hideLoader();
        }
        function createPaginationControlsQuarter(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';


            // First button
            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.type='button';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    loadDataFromServerQuarter(data);
                }
            };
            paginationContainer.appendChild(firstButton);


            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.type='button';
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
            nextButton.type='button';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServerQuarter(data);
                }
            };
            paginationContainer.appendChild(nextButton);

            // Last button
            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.type='button';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    loadDataFromServerQuarter(data);
                }
            };
            paginationContainer.appendChild(lastButton);
            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.type='button';
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
            rowsPerPage = 5;
            document.getElementById("filterDiv").style.display = "none";
            document.getElementById("buttonContainer").style.display = "none";

            var tableHeader = document.getElementById('tableHeader');
            var tableBody = document.getElementById('tableBody');
            var paginationContainer = document.getElementById("paginationControls");

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                paginationContainer.innerHTML = '';
                hideLoader();
                return;
            } else {
                tableHeader.style.removeProperty("display");
            }

            // Group by Funder
            var grouped = {};
            data.forEach(function (row) {
                if (!grouped[row.Funder]) grouped[row.Funder] = [];
                grouped[row.Funder].push({
                    ClientId: row.ClientId,
                    ClientName: row.ClientName
                });
            });

            var funderNames = Object.keys(grouped);
            var totalRows = funderNames.length;
            var start = (currentPage - 1) * rowsPerPage;
            var end = Math.min(start + rowsPerPage, totalRows);

            tableHeader.innerHTML = "";
            tableBody.innerHTML = "";

            for (var i = start; i < end; i++) {
                var funder = funderNames[i];
                var tableId = "innerTable_" + funder.replace(/\s+/g, "_");

                var outerRow = document.createElement('tr');
                var outerCell = document.createElement('td');
                outerCell.colSpan = 2;

                var innerTable = document.createElement('table');
                innerTable.id = tableId;
                innerTable.style.width = "60%";
                innerTable.style.tableLayout = "fixed";
                innerTable.style.borderCollapse = "collapse";
                innerTable.border = "1";
                innerTable.style.margin = "0 auto 20px auto";

                var funderHeaderRow = document.createElement('tr');
                var funderHeaderCell = document.createElement('th');
                funderHeaderCell.colSpan = 2;
                funderHeaderCell.innerText = funder;
                funderHeaderCell.style.backgroundColor = "#4CAF50";
                funderHeaderCell.style.color = "white";
                funderHeaderRow.appendChild(funderHeaderCell);
                innerTable.appendChild(funderHeaderRow);

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

            createPaginationControlsFunder(funderNames.length, data);
            hideLoader();
        }

        function createPaginationControlsFunder(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");
            paginationContainer.innerHTML = '';


            // First button
            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.type='button';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    loadDataFromServerFunder(data);
                }
            };
            paginationContainer.appendChild(firstButton);

            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.type='button';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    loadDataFromServerFunder(data);
                }
            };
            paginationContainer.appendChild(prevButton);

            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = ' Page ' + currentPage + ' of ' + totalPages + ' ';
            paginationContainer.appendChild(pageIndicator);

            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.type='button';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServerFunder(data);
                }
            };
            paginationContainer.appendChild(nextButton);
            // Last button
            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.type='button';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    loadDataFromServerFunder(data);
                }
            };
            paginationContainer.appendChild(lastButton);
            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.type='button';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportFunderTableToExcel(); // or your export logic
                loadDataFromServerFunder(data);
            };
            paginationContainer.appendChild(exportButton);
        }

        function exportFunderTableToExcel() {
            var workbook = new ExcelJS.Workbook();
            var now = new Date();
            var formattedDateTime = now.toLocaleDateString().replace(/\//g, '-') + " " +
                                    now.toLocaleTimeString().replace(/:/g, '-').replace(/ /g, '');
            var worksheet = workbook.addWorksheet("Funder Export");

            var currentRow = 1;

            // Group fullData by funder
    var grouped = {};
            fullData.forEach(function(row) {
    if (!grouped[row.Funder]) {
        grouped[row.Funder] = [];   
    }
    grouped[row.Funder].push({
        ClientId: row.ClientId,
        ClientName: row.ClientName
    });
    });


        // For each funder, create merged header and client rows
        for (var funder in grouped) {
            var funderClients = grouped[funder];

            // Merged Funder Title Row
            worksheet.mergeCells(`A${currentRow}:B${currentRow}`);
        var funderCell = worksheet.getCell(`A${currentRow}`);
        funderCell.value = funder;
        funderCell.font = { bold: true, color: { argb: 'FFFFFFFF' }, size: 12 };
        funderCell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: '4CAF50' }
        };
        funderCell.alignment = { horizontal: 'center', vertical: 'middle' };
        funderCell.border = {
            top: { style: 'thin' },
            left: { style: 'thin' },
            bottom: { style: 'thin' },
            right: { style: 'thin' }
        };

        currentRow++;

        // Column headers
        worksheet.getCell("A" + currentRow).value = "Client ID";
        worksheet.getCell("B" + currentRow).value = "Client Name";

        ["A", "B"].forEach(function (col) {
            var cell = worksheet.getCell(col + currentRow);
            cell.font = { bold: true };
            cell.alignment = { horizontal: 'center' };
            cell.border = {
                top: { style: 'thin' },
                left: { style: 'thin' },
                bottom: { style: 'thin' },
                right: { style: 'thin' }
            };
        });


        currentRow++;

        // Client rows
        funderClients.forEach(client => {
            worksheet.getCell(`A${currentRow}`).value = client.ClientId;
        worksheet.getCell(`B${currentRow}`).value = client.ClientName;

        ["A", "B"].forEach(col => {
            var cell = worksheet.getCell(`${col}${currentRow}`);
        cell.alignment = { horizontal: 'center' };
        cell.border = {
            top: { style: 'thin' },
            left: { style: 'thin' },
            bottom: { style: 'thin' },
            right: { style: 'thin' }
        };
        });

        currentRow++;
        });

        // Blank row after each funder block
        currentRow++;
        }

        worksheet.columns = [
            { width: 20 },
            { width: 30 }
        ];

        workbook.xlsx.writeBuffer().then(buffer => {
            saveAs(new Blob([buffer]), "Funder_Grouped_" + formattedDateTime + ".xlsx");
        });
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
                headers[i].innerText = headers[i].innerText.replace(" ⬆", "").replace(" ⬇", "").replace(" ⬍", "") + " ⬍";
            }

            // Set arrow on active column
            var baseText = header.innerText.replace(" ⬍", "").replace(" ⬆", "").replace(" ⬇", "");
            header.innerText = baseText + (ascending ? " ⬆" : " ⬇");

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
        //All Placement Table
        function loadDataFromServerPlacement(data) {
            fullData = data;
            rowsPerPage = 15;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            document.getElementById("filterDiv").style.display = "none";
            document.getElementById("buttonContainer").style.display = "none";
            tableBody.innerHTML = '';

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                document.getElementById("paginationControls").innerHTML = '';
                hideLoader();
                return;
            } else {
                tableHeader.style.removeProperty("display");
            }

            var columns = Object.keys(data[0]);
            tableHeader.innerHTML = '';
            var headerRow = document.createElement('tr');
            columns.forEach(function (col, index) {
                var th = document.createElement('th');
                th.textContent = col + " ⬍";
                var headerId = "placement_header_" + index;
                th.id = headerId;
                th.style.cursor = "pointer";
                th.onclick = function () {
                    SortColumns("table", index, headerId, fullData, loadDataFromServerPlacement);
                };
                headerRow.appendChild(th);
            });
            tableHeader.appendChild(headerRow);

            var startIndex = (currentPage - 1) * rowsPerPage;
            var endIndex = startIndex + rowsPerPage;
            var pageData = data.slice(startIndex, endIndex);

            var previousClientId = null;
            var rowspanTracker = {};

            // First pass: calculate rowspans for consecutive Client Ids
            for (var i = 0; i < pageData.length; i++) {
                var currentId = pageData[i]["Client Id"];
                if (currentId === previousClientId) {
                    rowspanTracker[currentId]++;
                } else {
                    rowspanTracker[currentId] = 1;
                    previousClientId = currentId;
                }
            }

            previousClientId = null;

            for (var i = 0; i < pageData.length; i++) {
                var row = pageData[i];
                var currentId = row["Client Id"];
                var isFirstOfGroup = currentId !== previousClientId;
                var currentRowspan = rowspanTracker[currentId];

                var tr = document.createElement('tr');

                columns.forEach(function (col) {
                    if ((col === "Client Id" || col === "Client Name") && !isFirstOfGroup) {
                        return; // Skip merged cell
                    }

                    var td = document.createElement('td');
                    td.textContent = row[col];

                    if ((col === "Client Id" || col === "Client Name") && isFirstOfGroup) {
                        td.rowSpan = currentRowspan;
                    }

                    tr.appendChild(td);
                });

                previousClientId = currentId;
                tableBody.appendChild(tr);
            }

            createPaginationControlsPlacement(data.length, data);
            hideLoader();
        }

        function createPaginationControlsPlacement(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';


            // First button
            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.type='button';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    loadDataFromServerPlacement(data);
                }
            };
            paginationContainer.appendChild(firstButton);

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.type='button';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    loadDataFromServerPlacement(data); // Reload birthdate table for new page
                }
            };
            paginationContainer.appendChild(prevButton);

            // Page indicator
            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + totalPages;
            paginationContainer.appendChild(pageIndicator);

            // Next button
            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.type='button';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    loadDataFromServerPlacement(data);
                }
            };
            paginationContainer.appendChild(nextButton);


            // Last button
            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.type='button';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    loadDataFromServerPlacement(data);
                }
            };
            paginationContainer.appendChild(lastButton);

            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.type='button';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportToExcelPlacement();
                loadDataFromServerPlacement(data);
            };
            paginationContainer.appendChild(exportButton);
        }

        //Birthdate Table
        function LoadDataFromServerBirthdate(data) {
            fullData = data;
            rowsPerPage = 15;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            document.getElementById("filterDiv").style.display = "none";
            document.getElementById("buttonContainer").style.display = "none";
            tableBody.innerHTML = '';

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                document.getElementById("paginationControls").innerHTML = '';
                hideLoader();
                return;
            } else {
                tableHeader.style.removeProperty("display");
            }

            // Get column headers
            var columns = Object.keys(data[0]);

            // Clear and build headers
            tableHeader.innerHTML = '';
            var headerRow = document.createElement('tr');
            columns.forEach(function (col, index) {
                var th = document.createElement('th');
                th.textContent = col + " ⬍"; // Add default sort icon
                var headerId = "birthdate_header_" + index;
                th.id = headerId;

                // Add click event to enable sorting
                th.style.cursor = "pointer";
                th.onclick = function () {
                    SortColumns("table", index, headerId, fullData, LoadDataFromServerBirthdate);
                };

                headerRow.appendChild(th);
            });
            tableHeader.appendChild(headerRow);

            // Pagination logic
            var startIndex = (currentPage - 1) * rowsPerPage;
            var endIndex = startIndex + rowsPerPage;
            var pageData = data.slice(startIndex, endIndex);

            // Populate table rows
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
            createPaginationControlsBirthdate(data.length, data);
            hideLoader();
        }

        function createPaginationControlsBirthdate(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';


            // First button
            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.type='button';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    LoadDataFromServerBirthdate(data);
                }
            };
            paginationContainer.appendChild(firstButton);

            // Previous button
            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.type='button';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    LoadDataFromServerBirthdate(data); // Reload birthdate table for new page
                }
            };
            paginationContainer.appendChild(prevButton);

            // Page indicator
            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + totalPages;
            paginationContainer.appendChild(pageIndicator);

            // Next button
            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.type='button';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    LoadDataFromServerBirthdate(data);
                }
            };
            paginationContainer.appendChild(nextButton);

            // Last button
            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.type='button';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    LoadDataFromServerBirthdate(data);
                }
            };
            paginationContainer.appendChild(lastButton);

            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.type='button';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportToExcelWithImages();
                LoadDataFromServerBirthdate(data);
            };
            paginationContainer.appendChild(exportButton);
        }

        function SortColumns(tableId, columnIndex, headerId, fullDataArray, reloadFunction) {
            var key = tableId + "_" + columnIndex;
            var ascending = true;

            if (tableSortState[key] !== undefined) {
                ascending = !tableSortState[key];
            }
            tableSortState = {}; 
            tableSortState[key] = ascending;

            var header = document.getElementById(headerId);
            var headerRow = header.parentNode;
            var headers = headerRow.getElementsByTagName("th");

            for (var i = 0; i < headers.length; i++) {
                headers[i].innerText = headers[i].innerText.replace(" ⬆", "").replace(" ⬇", "").replace(" ⬍", "") + " ⬍";
            }

            var baseText = header.innerText.replace(" ⬍", "").replace(" ⬆", "").replace(" ⬇", "");
            header.innerText = baseText + (ascending ? " ⬆" : " ⬇");

            var columnKey = Object.keys(fullDataArray[0])[columnIndex];

            fullDataArray.sort(function (a, b) {
                var valA = a[columnKey];
                var valB = b[columnKey];

                var dateA = new Date(valA);
                var dateB = new Date(valB);
                if (!isNaN(dateA) && !isNaN(dateB)) {
                    return ascending ? dateA - dateB : dateB - dateA;
                }

                var numA = parseFloat(valA);
                var numB = parseFloat(valB);
                if (!isNaN(numA) && !isNaN(numB)) {
                    return ascending ? numA - numB : numB - numA;
                }

                return ascending
                    ? String(valA).localeCompare(String(valB))
                    : String(valB).localeCompare(String(valA));
            });

            currentPage = 1;
            reloadFunction(fullDataArray);  
        }

        //Statistical Report
        function LoadDataFromServerStatistical(data,checkstat) {
            fullData = data;
            rowsPerPage = 15;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            document.getElementById("buttonContainer").style.display = "none";
            tableBody.innerHTML = '';

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                document.getElementById("noOfClients").textContent = "Total No. of Clients : 0";
                document.getElementById("paginationControls").innerHTML = '';
                hideLoader();
                return;
            } else {
                tableHeader.style.removeProperty("display");
            }

            // Get column headers
            var columns = Object.keys(data[0]);

            // Clear and build headers
            tableHeader.innerHTML = '';
            var headerRow = document.createElement('tr');
            columns.forEach(function (col, index) {
                var th = document.createElement('th');
                th.textContent = col + " ⬍"; // Add default sort icon
                var headerId = "Statistical_header_" + index;
                th.id = headerId;

                // Add click event to enable sorting
                th.style.cursor = "pointer";
                th.onclick = function () {
                    SortColumns("table", index, headerId, fullData, LoadDataFromServerStatistical);
                };

                headerRow.appendChild(th);
            });
            tableHeader.appendChild(headerRow);

            // Pagination logic
            var startIndex = (currentPage - 1) * rowsPerPage;
            var endIndex = startIndex + rowsPerPage;
            var pageData = data.slice(startIndex, endIndex);

            // Populate table rows
            pageData.forEach(function (row) {
                var tr = document.createElement('tr');
                columns.forEach(function (col) {
                    var td = document.createElement('td');
                    td.textContent = row[col];
                    tr.appendChild(td);
                });
                tableBody.appendChild(tr);
            });
            if(checkstat===true)
            {
            createColumnVisibilityCheckboxes(columns);
            }

            // Create pagination controls
            createPaginationControlsStatistical(data.length, data);

            var totalStudents = data.reduce(function (sum, row) {
                var val = parseInt(row["Total Students"]);
                return sum + (isNaN(val) ? 0 : val);
            }, 0);
            document.getElementById("noOfClients").textContent = "Total number of clients : " + totalStudents;

            hideLoader();
        }

        function createPaginationControlsStatistical(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';

            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.type = 'button';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    LoadDataFromServerStatistical(data,false);
                    hideandshowcolumn();
                }
            };
            paginationContainer.appendChild(firstButton);

            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.type = 'button';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    LoadDataFromServerStatistical(data,false);
                    hideandshowcolumn();

                }
            };
            paginationContainer.appendChild(prevButton);

            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + totalPages;
            paginationContainer.appendChild(pageIndicator);

            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.type = 'button';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    LoadDataFromServerStatistical(data,false);
                    hideandshowcolumn();

                }
            };
            paginationContainer.appendChild(nextButton);

            // Last button
            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.type = 'button';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    LoadDataFromServerStatistical(data,false);
                    hideandshowcolumn();
                }
            };
            paginationContainer.appendChild(lastButton);

            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.textContent = 'Export';
            exportButton.type = 'button';
            exportButton.onclick = function () {
                exportToExcelWithImages();
                //LoadDataFromServerStatistical(data);

            };
            paginationContainer.appendChild(exportButton);
        }


        //Changes Reports
        function LoadDataFromServerChanges(data) {
            fullData = data;
            rowsPerPage = 15;
            var tableBody = document.getElementById("tableBody");
            var tableHeader = document.getElementById("tableHeader");
            document.getElementById("buttonContainer").style.display = "none";
            tableBody.innerHTML = '';

            if (!data || data.length === 0) {
                tableBody.innerHTML = '<tr><td colspan="100%">No data available to display</td></tr>';
                tableHeader.style.display = "none";
                document.getElementById("paginationControls").innerHTML = '';
                hideLoader();
                return;
            } else {
                tableHeader.style.removeProperty("display");
            }

            // Get column headers
            var columns = Object.keys(data[0]);

            // Clear and build headers
            tableHeader.innerHTML = '';
            var headerRow = document.createElement('tr');
            columns.forEach(function (col, index) {
                var th = document.createElement('th');
                th.textContent = col + " ⬍"; // Add default sort icon
                var headerId = "Changes_header_" + index;
                th.id = headerId;

                // Add click event to enable sorting
                th.style.cursor = "pointer";
                th.onclick = function () {
                    SortColumns("table", index, headerId, fullData, LoadDataFromServerChanges);
                };

                headerRow.appendChild(th);
            });
            tableHeader.appendChild(headerRow);

            // Pagination logic
            var startIndex = (currentPage - 1) * rowsPerPage;
            var endIndex = startIndex + rowsPerPage;
            var pageData = data.slice(startIndex, endIndex);

            // Populate table rows
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
            createPaginationControlsChanges(data.length, data);

            hideLoader();
        }

        function createPaginationControlsChanges(totalRows, data) {
            var totalPages = Math.ceil(totalRows / rowsPerPage);
            var paginationContainer = document.getElementById("paginationControls");

            paginationContainer.innerHTML = '';


            // First button
            var firstButton = document.createElement('button');
            firstButton.textContent = 'First';
            firstButton.type='button';
            firstButton.disabled = currentPage === 1;
            firstButton.onclick = function () {
                if (currentPage !== 1) {
                    currentPage = 1;
                    LoadDataFromServerChanges(data);
                }
            };
            paginationContainer.appendChild(firstButton);

            var prevButton = document.createElement('button');
            prevButton.textContent = 'Previous';
            prevButton.type='button';
            prevButton.disabled = currentPage === 1;
            prevButton.onclick = function () {
                if (currentPage > 1) {
                    currentPage--;
                    LoadDataFromServerChanges(data);
                }
            };
            paginationContainer.appendChild(prevButton);

            var pageIndicator = document.createElement('span');
            pageIndicator.textContent = 'Page ' + currentPage + ' of ' + totalPages;
            paginationContainer.appendChild(pageIndicator);

            var nextButton = document.createElement('button');
            nextButton.textContent = 'Next';
            nextButton.type='button';
            nextButton.disabled = currentPage === totalPages;
            nextButton.onclick = function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    LoadDataFromServerChanges(data);
                }
            };
            paginationContainer.appendChild(nextButton);

            // Last button
            var lastButton = document.createElement('button');
            lastButton.textContent = 'Last';
            lastButton.type='button';
            lastButton.disabled = currentPage === totalPages;
            lastButton.onclick = function () {
                if (currentPage !== totalPages) {
                    currentPage = totalPages;
                    LoadDataFromServerChanges(data);
                }
            };
            paginationContainer.appendChild(lastButton);

            var exportButton = document.createElement('button');
            exportButton.id = 'BtnExport';
            exportButton.type='button';
            exportButton.textContent = 'Export';
            exportButton.onclick = function () {
                exportToExcelWithImages();
                //LoadDataFromServerStatistical(data);
            };
            paginationContainer.appendChild(exportButton);
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


            if (headingDiv === "Statistical Report" || headingDiv === "All Clients Info") {
                if((Object.keys(columndata).length)>0)
                {
                    var newcol=[];
                    for (var key in columndata) {
                        if(columndata[key]==true){
                            newcol.push(key);
                        }
                    }
                    columns=newcol;
                }
            }


            worksheet.columns = columns.map(function (col) {
                return { header: col, key: col, width: 25 };
            });
            if (headingDiv === "Statistical Report" || headingDiv === "All Clients Info") {
                var totalStudents;
                if (headingDiv === "Statistical Report" ) {
                    totalStudents = fullData.reduce(function (sum, row) {
                        var val = parseInt(row["Total Students"]);
                        return sum + (isNaN(val) ? 0 : val);
                    }, 0);
                } else if(headingDiv === "All Clients Info") {
                    totalStudents = fullData.length;
                }
                worksheet.insertRow(1, ["Total number of clients : ", totalStudents]);

                // Optional: format the cells
                var totalRow = worksheet.getRow(1);
                totalRow.eachCell(function (cell, colNumber) {
                    cell.font = { bold: true };
                    cell.alignment = { vertical: 'middle', horizontal: 'center' };
                    cell.border = {
                        top: { style: 'thin' },
                        left: { style: 'thin' },
                        bottom: { style: 'thin' },
                        right: { style: 'thin' }
                    };
                });
            }
            var headerRow = worksheet.getRow(headingDiv === "Statistical Report" || headingDiv === "All Clients Info" ? 2 : 1);
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
            var rowOffset = (headingDiv === "Statistical Report" || headingDiv === "All Clients Info" ) ? 2 : 1;

            for (var i = 0; i < fullData.length; i++) {
                var row = fullData[i];
                var excelRow = worksheet.getRow(i + rowOffset + 1);

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
                            tl: { col: j, row: i + rowOffset + 1 },
                            ext: { width: 100, height: 80 }
                        });
                        worksheet.getRow(i + rowOffset + 1).height = 80;
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
            columns = columns.filter(function(item) {
                return item !== "ID" && item !== "Status";
            });
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
                var key = fullData[i]["ID"];
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
                var key = row["ID"];
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

        function exportToExcelPlacement() {
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

            // Add data rows
            for (var i = 0; i < fullData.length; i++) {
                var rowValues = [];
                for (var j = 0; j < columns.length; j++) {
                    rowValues.push(fullData[i][columns[j]]);
                }
                worksheet.addRow(rowValues);
            }

            // Merge adjacent identical cells for Client Id and Client Name
            var currentClientId = null;
            var currentClientName = null;
            var startRow = 2;
            var clientIdCol = columns.indexOf("Client Id") + 1;
            var clientNameCol = columns.indexOf("Client Name") + 1;

            for (var i = 2; i <= worksheet.rowCount; i++) {
                var thisClientId = worksheet.getCell(i, clientIdCol).value;
                var thisClientName = worksheet.getCell(i, clientNameCol).value;

                if (thisClientId !== currentClientId || thisClientName !== currentClientName) {
                    if (i - startRow > 1) {
                        worksheet.mergeCells(startRow, clientIdCol, i - 1, clientIdCol);
                        worksheet.mergeCells(startRow, clientNameCol, i - 1, clientNameCol);

                        var mergedCell1 = worksheet.getCell(startRow, clientIdCol);
                        var mergedCell2 = worksheet.getCell(startRow, clientNameCol);
                        mergedCell1.alignment = mergedCell2.alignment = {
                            vertical: 'middle',
                            horizontal: 'center',
                            wrapText: true
                        };
                    }
                    currentClientId = thisClientId;
                    currentClientName = thisClientName;
                    startRow = i;
                }
            }

            // Merge the last group if needed
            if (worksheet.rowCount + 1 - startRow > 1) {
                worksheet.mergeCells(startRow, clientIdCol, worksheet.rowCount, clientIdCol);
                worksheet.mergeCells(startRow, clientNameCol, worksheet.rowCount, clientNameCol);

                var mergedCell1 = worksheet.getCell(startRow, clientIdCol);
                var mergedCell2 = worksheet.getCell(startRow, clientNameCol);
                mergedCell1.alignment = mergedCell2.alignment = {
                    vertical: 'middle',
                    horizontal: 'center',
                    wrapText: true
                };
            }

            // Apply border and alignment to all cells
            worksheet.eachRow(function (row, rowNumber) {
                row.eachCell(function (cell) {
                    cell.border = {
                        top: { style: 'thin' },
                        left: { style: 'thin' },
                        bottom: { style: 'thin' },
                        right: { style: 'thin' }
                    };
                    if (rowNumber > 1) {
                        cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
                    }
                });
            });

            workbook.xlsx.writeBuffer().then(function (buffer) {
                saveAs(new Blob([buffer]), headingDiv + ".xlsx");
            });
        }

        function escapeHtml(s) {
            return String(s || '')
                .replace(/&/g, '&amp;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;')
                .replace(/"/g, '&quot;')
                .replace(/'/g, '&#39;');
        }

        function renderAggregatedPlacementChart(data) {
            // delegate to the new implementation
            try {
                renderPlacementChart(data || []);
            } catch (e) {
                console.error('renderAggregatedPlacementChart wrapper error', e);
            }
        }

        function sortByYearQuarter(data) {
            return data.sort((a, b) => {
                const yearA = Number(a.Year || a.year);
        const yearB = Number(b.Year || b.year);

            // Normalize quarter value Q1/Q2/Q3/Q4
        const qA = Number((a.Quarter || a.quarter || 'Q0').toString().replace('Q', ''));
        const qB = Number((b.Quarter || b.quarter || 'Q0').toString().replace('Q', ''));

            // First sort by year
            if (yearA !== yearB) return yearA - yearB;

            // Then sort Q1 → Q4
            return qA - qB;
        });
        }

        function getBirthYearFromAge(age) {
            if (!age || isNaN(age)) return null;

            var currentYear = new Date().getFullYear();
            return currentYear - parseInt(age);
        }

        function renderPlacementChart(dataFromServer, opts) {
            var filt = document.getElementById("agefilter");
            if (filt) {
                filt.style.display = "block"; 
            }
            if( dataFromServer.length==0)
            {
                placementChartContainer.innerHTML = '<div style="padding:12px;color:#666">No placement data to display</div>';
                var exportBtn = document.getElementById("exportChartBtn");
                if (exportBtn) {
                    exportBtn.style.display = "none"; // hides the button
                }
            }
            else{
                var exp = document.getElementById("exportChartBtn");
                if (exp) {
                    exp.style.display = "block"; 
                }
                var minYear = null;
            var maxYear = null;

            // Find min and max year from server data
            for (var i = 0; i < dataFromServer.length; i++) {
                var y = parseInt(dataFromServer[i].Year, 10);
                if (minYear === null || y < minYear) minYear = y;
                if (maxYear === null || y > maxYear) maxYear = y;
            }

            // Apply age filtering
            //var ageFrom = ddocument.getElementById("fromage").value; 
            //var ageTo   = document.getElementById("txtAgeToClient").value;   

            var ageFrom = document.getElementById("hfAgeFrom").value;
            var ageTo   = document.getElementById("hfAgeTo").value;
            if (ageFrom !== '') {
                maxYear = getBirthYearFromAge(ageFrom);
            }

            if (ageTo !== '') {
                minYear = getBirthYearFromAge(ageTo);
            }
           
            if (minYear > maxYear) {
                var t = minYear;
                minYear = maxYear;
                maxYear = t;
            }

            var groupedCategories = [];
            var allCategories = [];
            var currentYear = new Date().getFullYear();

            // Build categories (ascending order, ensures visibility for empty years)
            for (var year = minYear; year <= maxYear; year++) {

                var quarters = ["Q1", "Q2", "Q3", "Q4"];
                var age = currentYear - year;
                var yearLabel = year + "<br/>(" + age + " y/o)";

                groupedCategories.push({
                    name: yearLabel,
                    categories: quarters
                });

                quarters.forEach(q => allCategories.push(year + " " + q));
            }

            // Build series
            var seriesMap = {};
            for (var i = 0; i < dataFromServer.length; i++) {
                var yq = dataFromServer[i].Year + " " + dataFromServer[i].Quarter;

                var catIndex = allCategories.indexOf(yq);

                // ❗ IMPORTANT FIX: skip data outside the selected range
                if (catIndex === -1) continue;

                var names = dataFromServer[i].SampleNames.split(';');
                for (var j = 0; j < names.length; j++) {
                    var name = names[j].trim();

                    if (!seriesMap[name]) {
                        let shortName = name.length > 18 ? name.substring(0, 14) : name;
                        seriesMap[name] = {
                            name: name,
                            shortName: shortName,
                            data: new Array(allCategories.length).fill(0)
                        };
                    }

                    seriesMap[name].data[catIndex] = {
                        y: 1,
                        displayName: seriesMap[name].shortName,
                        dataLabels: { enabled: true }
                    };
                }
            }

            var seriesList = Object.values(seriesMap);

            // Compute Y-axis values
            var maxCount = Math.max(...dataFromServer.map(d => parseInt(d.Count, 10)));
            var totalCount = dataFromServer.reduce((sum, d) => sum + parseInt(d.Count, 10), 0);

            var today = new Date();
            var printedOn =
                ("0" + (today.getMonth() + 1)).slice(-2) + "/" +
                ("0" + today.getDate()).slice(-2) + "/" +
                today.getFullYear();


            Highcharts.chart('placementChartContainer', {
                chart: {
                    type: 'column',
                    animation: false,
                    scrollablePlotArea: {
                        minWidth: Math.max(allCategories.length * 20, 800),
                        scrollPositionX: 0
                    },
                    borderColor: '#000',
                    borderWidth: 2,
                    borderRadius: 1,
                    events: {
                        load: function () {
                            var chart = this;
                            var labelText = "Printed On: " + printedOn;

                            var lastX = chart.xAxis[0].max;

                            var label = chart.renderer.text(labelText, 0, 0)
                                .css({
                                    fontSize: '12px',
                                    fontWeight: 'bold',
                                    color: '#000'
                                })
                                .add();

                            chart.printedLabel = label;

                            function positionLabel() {
                                var bbox = label.getBBox();
                                var xPos = chart.xAxis[0].toPixels(lastX, true) - bbox.width - 10;
                                var yPos = chart.plotTop + chart.plotHeight + 80;
                                label.attr({ x: xPos, y: yPos });
                            }

                            positionLabel();
                            Highcharts.addEvent(chart, 'redraw', positionLabel);
                        }
                    }
                },

                credits: { enabled: false },

                title: {
                    text: 'All Current Students by Birthdate',
                    style: { color: '#000', fontWeight: 'bold' }
                },

                subtitle: {
                    text: 'Total Count = ' + totalCount,
                    align: 'left',
                    style: {
                        color: '#000',
                        fontWeight: 'bold',
                        fontSize: '12px'
                    }
                },

                xAxis: {
                    categories: groupedCategories,
                    labels: {
                        rotation: 0,
                        style: {
                            color: '#000',
                            fontWeight: 'bold'
                        }
                    },
                    tickLength: 8,
                    title: {
                        text: 'BirthDate',
                        style: { color: '#000', fontWeight: 'bold' }
                    }
                },

                yAxis: {
                    min: 0,
                    max: Math.max(1, maxCount),
                    allowDecimals: false,
                    labels: {
                        style: {
                            color: '#000',
                            fontWeight: 'bold'
                        }
                    },
                    title: {
                        text: 'Student Count',
                        style: { color: '#000', fontWeight: 'bold' }
                    }
                },

                legend: {
                    enabled: true,
                    layout: 'horizontal',
                    align: 'right',
                    verticalAlign: 'top',
                    y: 25,
                    floating: true,
                    symbolHeight: 14,
                    symbolWidth: 14,
                    symbolRadius: 0,
                    itemStyle: {
                        fontWeight: 'bold',
                        fontSize: '12px',
                        color: '#000'
                    }
                },

                plotOptions: {
                    series: {
                        stacking: 'normal',
                        color: '#C8F7C5',
                        pointWidth: 20,
                        borderColor: '#000',
                        borderWidth: 1,
                        showInLegend: false,
                        dataLabels: {
                            enabled: false,
                            useHTML: false,
                            defer: true,
                            allowOverlap: true,
                            inside: true,
                            rotation: -90,
                            crop: false,
                            overflow: 'allow',
                            formatter: function () {
                                return (this.point.displayName && this.y === 1)
                                    ? this.point.displayName
                                    : '';
                            },
                            style: {
                                color: '#000',
                                fontWeight: 'bold',
                                textOutline: 'none',
                                fontSize: '10px'
                            }
                        }
                    }
                },

                series: [
                    ...seriesList.map(s => ({ ...s, showInLegend: false })),
        {
        name: 'Current Students',
        data: [],
        showInLegend: true,
        color: '#C8F7C5',
        marker: { symbol: 'square' }
        }
        ],

        exporting: {
            enabled: true,
            sourceWidth: Math.max(allCategories.length * 40, 1400),
            sourceHeight: 1200,
            scale: 1,
            fallbackToExportServer: false,
            buttons: {
                contextButton: {
                        menuItems: ['downloadPNG', 'downloadJPEG']
                }
            }
        }
        });
        }
        }



        function exportChartToPDF() {
            // Get Highcharts chart object
            var chart = Highcharts.charts[0];  // If only one chart. If multiple, pass ID.

            if (!chart) {
                alert("Chart not found!");
                return;
            }

            // STEP 1: Get SVG from Highcharts
            var svg = chart.getSVG({
                exporting: {
                    scale: 2    // higher quality output
                }
            });

            // STEP 2: Create Canvas & draw the SVG on it
            var canvas = document.createElement("canvas");
            var ctx = canvas.getContext("2d");

            // Set canvas dimensions large enough
            var svgBlob = new Blob([svg], { type: "image/svg+xml;charset=utf-8" });
            var url = URL.createObjectURL(svgBlob);

            var img = new Image();
            img.onload = function () {

                canvas.width = img.width;
                canvas.height = img.height;

                ctx.drawImage(img, 0, 0);

                // STEP 3: Canvas to PNG
                var pngData = canvas.toDataURL("image/png");

                // STEP 4: Create jsPDF (landscape)
                var { jsPDF } = window.jspdf;
            var pdf = new jsPDF({
                orientation: "landscape",
                unit: "px",
                format: [img.width, img.height]
            });

            // Add PNG image into PDF
            pdf.addImage(pngData, "PNG", 0, 0, img.width, img.height);

            // STEP 5: Save the PDF
            pdf.save("chart.pdf");

            URL.revokeObjectURL(url);
        };

        img.src = url;
        }



        function renderPlacementChart1(dataFromServer, opts) {
            // Full-featured placement chart:
            // - columns show counts
            // - scatter overlay places one label per y-slot (0.5,1.5,2.5...) centered vertically
            // - vertical letters, truncated with ellipsis to fit the slot
            // - chart height grows according to maxCount * nameLinePx
            opts = opts || {};
            var containerId = opts.containerId || 'placementChartContainer';
            var wrapperId = opts.wrapperId || 'placementChartWrapper';
            var desiredBarPx = Number(opts.desiredBarPx || 20);
            var minChartInnerWidth = Number(opts.minChartInnerWidth || 700);
            var extraPadding = Number(opts.extraPadding || 160);
            var baseChartHeight = Number(opts.chartHeight || 420);
            var chartTitle = typeof opts.title === 'string' ? opts.title : 'All Current Students by Birthdate';

            // Tweakables:
            var nameLinePx = Number(opts.nameLinePx || 100); // vertical pixels per Y-slot (default larger for taller chart)
            var maxChartHeight = Number(opts.maxChartHeight || 3000);
            var truncatedFontSize = Number(opts.truncatedFontSize || 10); // fixed font size for names (smaller)

            function escapeHtml(s) {
                return String(s || '')
                    .replace(/&/g, '&amp;')
                    .replace(/</g, '&lt;')
                    .replace(/>/g, '&gt;')
                    .replace(/"/g, '&quot;')
                    .replace(/'/g, '&#39;');
            }
            function ensureEl(id) { return document.getElementById(id); }

            try {
                if (!Array.isArray(dataFromServer)) dataFromServer = [];

                var container = ensureEl(containerId);
                if (!container) { console.error('renderPlacementChart: container not found:', containerId); return; }
                if (typeof Highcharts === 'undefined') { console.error('renderPlacementChart: Highcharts not loaded'); return; }

                var categories = [];
                var columnSeriesData = [];
                var buckets = [];   // for tooltips: { count, names }
                var scatterData = [];
                var maxCount = 0;

                dataFromServer = (dataFromServer || []).slice(); // clone to avoid mutating original array
                //dataFromServer = dataFromServer.sort(function (a, b) {
                //    var yA = Number(a.Year || a.year || a.BirthYear || a.birthYear) || 0;
                //    var yB = Number(b.Year || b.year || b.BirthYear || b.birthYear) || 0;
                //    if (yA !== yB) return yA - yB; // sort by year first

                //    // sort by quarter
                //    var qA = (a.Quarter || a.quarter || '').toString().trim().toUpperCase();
                //    var qB = (b.Quarter || b.quarter || '').toString().trim().toUpperCase();

                //    var order = { "Q1": 1, "Q2": 2, "Q3": 3, "Q4": 4 };

                //    return (order[qA] || 0) - (order[qB] || 0);
                //});
                dataFromServer = sortByYearQuarter(dataFromServer);

                var yearMap = {}; // year => { 'Q1': {count,namesArr}, ... }
                var yearsOrder = []; // keep ascending order
                dataFromServer.forEach(function (r) {
                    var quarter = (r.Quarter || r.quarter || '').toString().trim().toUpperCase();
                    var year = (r.Year || r.year || r.BirthYear || r.birthYear || '').toString().trim();
                    var cat = (r.Category || r.category || '').toString().trim();
                    var count = Number(r.Count || r.count || 0) || 0;
                    var samplesRaw = (r.SampleNames || r.SampleNames || r.Names || r.names || '').toString();

                    if (!year) return; // skip malformed rows

                    if (!yearMap[year]) {
                        yearMap[year] = { 'Q1': null, 'Q2': null, 'Q3': null, 'Q4': null };
                        yearsOrder.push(year);
                    }

                    var qKey = quarter || 'Q1';
                    if (!/^Q[1-4]$/i.test(qKey)) qKey = 'Q1';
                    qKey = qKey.toUpperCase();

                    // build names array
                    var namesArr = [];

                    if (samplesRaw) {
                        var raw = samplesRaw.toString().trim();

                        if (!raw) {
                            namesArr = [];
                        }
                            // 1) semicolon → multiple names
                        else if (raw.indexOf(';') >= 0) {
                            namesArr = raw.split(';').map(function (x) {
                                return x.replace(/<[^>]*>/g, '')     // strip HTML
                                         .replace(/[\r\n]+/g, ' ')  // remove line breaks
                                         .replace(/\s+/g, ' ')      // collapse spaces
                                         .trim();
                            }).filter(Boolean);
                        }
                            // 2) Detect "Last, First" pairs → convert to "First Last"
                        else if (/^([^,]+),\s*([^,]+)$/.test(raw)) {
                            var m = raw.split(',');
                            var last = m[0].trim();
                            var first = m[1].trim();
                            namesArr = [ (first + ' ' + last).replace(/\s+/g, ' ').trim() ];
                        }
                            // 3) Detect multiple "Last, First" records stuck together with commas
                        else {
                            var pairs = raw.match(/[^,;]+,\s*[^,;]+/g); 
                            if (pairs && pairs.length > 0) {
                                namesArr = pairs.map(function (p) {
                                    var s = p.split(',');
                                    return (s[1].trim() + ' ' + s[0].trim()).replace(/\s+/g,' ').trim();
                                });
                            }
                                // 4) Fallback → treat as ONE single name (prevents accidental split)
                            else {
                                namesArr = [
                                    raw.replace(/<[^>]*>/g, '')    // remove HTML
                                       .replace(/[\r\n]+/g, ' ')   // remove breaks
                                       .replace(/\s+/g, ' ')       // collapse spaces
                                       .trim()
                                ];
                            }
                        }
                    }

                    yearMap[year][qKey] = { count: count, names: namesArr };
                });

                // Ensure yearsOrder sorted numerically ascending
                yearsOrder.sort(function(a,b) { return Number(a) - Number(b); });

                // Build full categories and series with empty quarters filled with zeros
                categories = [];
                columnSeriesData = [];
                buckets = [];
                scatterData = [];
                maxCount = 0;

                yearsOrder.forEach(function(yr) {
                    ['Q1','Q2','Q3','Q4'].forEach(function(q) {
                        var bucket = yearMap[yr][q];
                        var age = new Date().getFullYear() - Number(yr);
                        var displayCategory = q + ' ' + yr + ' (' + age + ')';
                        categories.push(displayCategory);

                        var count = (bucket && typeof bucket.count === 'number') ? bucket.count : 0;
                        columnSeriesData.push({ y: count });

                        var namesArr = (bucket && bucket.names) ? bucket.names : [];
                        buckets.push({ count: count, names: namesArr });

                        var catIndex = categories.length - 1;
                        for (var i = 0; i < namesArr.length; i++) {
                            scatterData.push({
                                x: catIndex,
                                y: i + 0.5,
                                name: namesArr[i]
                            });
                        }

                        if (count > maxCount) maxCount = count;
                    });
                });

                if (!categories.length) {
                    container.innerHTML = '<div style="padding:12px;color:#666">No placement data to display</div>';
                    return;
                }

                var yearGroups = [];
                for (var yi = 0; yi < yearsOrder.length; yi++) {
                    var startIndex = yi * 4;          // Q1 of year yi sits at this category index
                    var endIndex = startIndex + 3;    // Q4 index
                    yearGroups.push({ year: yearsOrder[yi], startIndex: startIndex, endIndex: endIndex });
                }


                // wrapper for horizontal scrolling
                var wrapper = document.getElementById('placementChartWrapper');
                if (!wrapper) {
                    wrapper = document.createElement('div');
                    wrapper.id = wrapperId;
                    wrapper.style.width = "100%";
                    wrapper.style.overflowX = "auto";
                    wrapper.style.overflowY = "hidden";
                    wrapper.style.whiteSpace = "nowrap";
                    wrapper.style.boxSizing = "border-box";

                    // Insert wrapper *before* container, then move container inside
                    container.parentNode.insertBefore(wrapper, container);
                    wrapper.appendChild(container);
                } else {
                    wrapper.style.overflowX = "auto";
                    wrapper.style.overflowY = "hidden";
                    wrapper.style.whiteSpace = "nowrap";
                    wrapper.style.boxSizing = "border-box";
                }

                // width sizing
                var idealInnerWidth = Math.max(minChartInnerWidth, categories.length * desiredBarPx + extraPadding);
                container.style.minWidth = idealInnerWidth + 'px';
                container.style.width = idealInnerWidth + 'px';
                container.style.boxSizing = 'border-box';

                // compute chart height to allocate nameLinePx per slot, plus margin for axes/title
                var plotPadding = 140; // room for title/axes (tweakable)
                var computedChartHeight = Math.max(baseChartHeight, Math.min(maxChartHeight, maxCount * nameLinePx + plotPadding));

                var computedPointWidth = Math.max(8, Math.floor(desiredBarPx * 0.9));

                // Helper: truncate name to fit a single vertical slot
                function truncateForSlot(name, fontSizePx, slotPx) {
                    if (!name) return '';
                    // For vertical writing-mode the limiting factor is number of characters that can be stacked
                    // approximate one character vertical height ≈ fontSizePx
                    var maxChars = Math.max(1, Math.floor(slotPx / fontSizePx));
                    if (name.length <= maxChars) return name;
                    if (maxChars <= 1) return name.charAt(0) + '…';
                    return name.substring(0, maxChars - 1) + '…';
                }

                var chartOptions = {
                    chart: {
                        type: 'column',
                        height: computedChartHeight,
                        width: idealInnerWidth,
                        plotBorderWidth: 1,
                        plotBorderColor: '#000',

                        // make room at the bottom for the quarter/year boxes + axis title
                        spacingBottom: 120,     // increase if needed (controls internal spacing)
                        marginBottom: 120,      // gives more stable export results

                        events: {
                            load: function () { try { drawQuarterAndYearBoxes(this); } catch(e){} },
                            redraw: function () { try { drawQuarterAndYearBoxes(this); } catch(e){} }
                        }
                    },
                    title: { text: chartTitle },
                    xAxis: {
                        categories: categories,
                        // move 'y' into title — Highcharts reads axis-title offsets here
                        title: {
                            text: 'Birthdate',
                            y: 70           // pushes the title down relative to the axis line. Tweak (40..100) as needed.
                        },

                        /* hide native labels — you draw Q/Y rows with renderer */
                        labels: {
                            useHTML: false,
                            rotation: 0,
                            y: 0,
                            style: { fontSize: '0px', color: 'transparent' },
                            formatter: function () { return ''; }
                        },
                        tickLength: 8
                    },

                    exporting: {
                        enabled: false,
                        allowHTML: true
                    },

                    yAxis: {
                        min: 0,
                        max: Math.max(1, maxCount),
                        tickInterval: 1,
                        allowDecimals: false,
                        title: { text: 'Student Count' },
                        labels: { formatter: function () { return this.value; } }
                    },

                    tooltip: {
                        useHTML: true,
                        outside: true,         // <- render tooltip outside the chart SVG (prevents clipping)
                        style: { zIndex: '2147483647', pointerEvents: 'none' },
                        shared: false, // ensure we show tooltip for the relevant point only
                        formatter: function () {
                            try {
                                // Determine the category index (works for both column and scatter)
                                var idx = -1;
                                if (this.point && typeof this.point.x === 'number') {
                                    // scatter: x is the category index (may be fractional if you offset; round it)
                                    idx = Math.round(this.point.x);
                                }
                                if (idx === -1 && typeof this.point.index === 'number') {
                                    // fallback: point.index (works for column points)
                                    idx = this.point.index;
                                }
                                // Another fallback: try this.x as a numeric category index
                                if (idx === -1 && typeof this.x === 'number') {
                                    idx = Math.round(this.x);
                                }

                                // now build label safely
                                var label = (idx >= 0 && categories[idx]) ? categories[idx] : (this.x || this.point && this.point.category) || this.x || '';
                                var count = (idx >= 0 && buckets[idx] && typeof buckets[idx].count !== 'undefined') ? buckets[idx].count : (this.y || (this.point && this.point.y) || 0);
                                var namesHtml = '';
                                if (idx >= 0 && buckets[idx] && buckets[idx].names && buckets[idx].names.length) {
                                    namesHtml = '<hr/>' + buckets[idx].names.map(escapeHtml).join('<br/>');
                                } else if (this.point && this.point.name) {
                                    // if we couldn't find bucket, show this point's name (fallback)
                                    namesHtml = '<hr/>' + escapeHtml(String(this.point.name || ''));
                                }

                                return '<b>' + escapeHtml(String(label)) + '</b><br/>Count: ' + escapeHtml(String(count)) + namesHtml;
                            } catch (e) {
                                // fallback safe tooltip
                                return escapeHtml(String(this.x || (this.point && this.point.name) || this.y || ''));
                            }
                        }
                    },

                    plotOptions: {
                        column: {
                            pointWidth: computedPointWidth,
                            pointPadding: 0.02,
                            groupPadding: 0.04,
                            borderWidth: 1,
                            dataLabels: { enabled: false },
                            states: { inactive: { enabled: false } }   // << add this line
                        },
                        scatter: {
                            marker: { enabled: false },
                            clip: false,
                            states: { hover: { enabled: false }, inactive: { enabled: false } },
                            tooltip: { enabled: false },
                            zIndex: 5
                        }
                    },

                    series: [
                      {
                          name: 'Students',
                          data: columnSeriesData,
                          zIndex: 1, 
                          showInLegend: false,
                          // single uniform color
                          color: '#C8F7C5'
                      },
                      {
                          type: 'scatter',
                          name: 'Names',
                          data: scatterData,
                          zIndex: 2,
                          showInLegend: false,
                          marker: { enabled: false },
                          dataLabels: {
                              enabled: true,
                              useHTML: true,
                              crop: false,
                              overflow: 'allow',
                              allowOverlap: true,
                              verticalAlign: 'middle',
                              align: 'center',

                              /* ---- REPLACED FORMATTER: determine underlying bar color and compute contrast color ---- */
                              formatter: function () {
                                  try {
                                      var nm = this.point && this.point.name ? String(this.point.name) : '';
                                      if (!nm) return '';

                                      var chart = this.series.chart;
                                      var catIndex = Math.round(this.point.x);

                                      // Get the bar for this index
                                      var colSeries = chart.series && chart.series[0];
                                      var colPoint = (colSeries && colSeries.data && colSeries.data[catIndex]) ? colSeries.data[catIndex] : null;

                                      // Hide if outside bar height
                                      var slotY = this.point.y;
                                      var barHeight = colPoint && typeof colPoint.y === 'number'
                                          ? colPoint.y
                                          : (colPoint && colPoint.options && colPoint.options.y) || 0;
                                      if (!barHeight || (slotY > barHeight + 0.001)) return '';

                                      if (Math.round(barHeight) <= 1) {
                                          // show the first portion, truncated if needed
                                          var fontPx = Math.max(8, Math.min(12, Math.round(truncatedFontSize)));

                                          // allow longer names (20–30 chars depending on font)
                                          var maxCharsForSingle = Math.max(15, Math.floor(30 * (12 / fontPx))); 
                                          // Example: if fontPx = 10 → ~36 chars allowed

                                          var singleDisplay =
                                              nm.length <= maxCharsForSingle
                                              ? nm
                                              : nm.substring(0, maxCharsForSingle - 1) + '';

                                          var safeName = escapeHtml(singleDisplay);
                                      }

                                      // Exact bar pixel width (rendered) and slot height (from outer scope)
                                      var colPixelWidth =
                                          (colPoint && colPoint.shapeArgs && colPoint.shapeArgs.width) ||
                                          (colSeries && colSeries.pointWidth) ||
                                          computedPointWidth;
                                      colPixelWidth = Math.max(1, Math.round(colPixelWidth));

                                      var slotPx = Math.max(1, Math.round(nameLinePx));

                                      // Keep bar color and border (border slightly darker)
                                      var barColor = colPoint && colPoint.color ? colPoint.color : (colSeries && colSeries.color) ? colSeries.color : '#C8F7C5';
                                      function hexToRgb(h) {
                                          if (!h) return [44,175,254];
                                          if (h.indexOf && h.indexOf('rgb') === 0) {
                                              var nums = h.match(/\d+/g);
                                              return nums ? [parseInt(nums[0]), parseInt(nums[1]), parseInt(nums[2])] : [44,175,254];
                                          }
                                          if (h[0] === '#') {
                                              var hex = h.slice(1);
                                              if (hex.length === 3) hex = hex.split('').map(function(c){return c+c;}).join('');
                                              var int = parseInt(hex,16) || 0;
                                              return [(int>>16)&255, (int>>8)&255, int&255];
                                          }
                                          return [44,175,254];
                                      }
                                      function darkenRgbArr(arr, amt) {
                                          return 'rgb(' + Math.max(0, Math.round(arr[0]*(1-amt))) + ',' + Math.max(0, Math.round(arr[1]*(1-amt))) + ',' + Math.max(0, Math.round(arr[2]*(1-amt))) + ')';
                                      }
                                      var rgb = hexToRgb(barColor);
                                      var borderColor = '#111';//darkenRgbArr(rgb, 0.18);

                                      // Keep text styles exactly as before (do not change color/size/orientation)
                                      // If you earlier used a specific color variable, preserve it here:
                                      var textColor = '#111';
                                      var truncated = truncateLeadingForSlot(nm, truncatedFontSize, nameLinePx, 6);
                                      var safe = escapeHtml(truncated);

                                      // Build HTML:
                                      // Outer box = exact width/height + background + border
                                      // Inner structure uses table/table-cell to vertically center the inner span.
                                      // The inner span preserves writing-mode / rotation / font-size etc. exactly as before.
                                      var html = ''
                                          + '<div style="display:inline-block;'
                                          +    'width:' + colPixelWidth + 'px;'
                                          +    'height:' + slotPx + 'px;'
                                          +    'box-sizing:border-box;'
                                          +    'padding:0; margin:0;'
                                          + 'background-color:' + barColor + ';'
                                          + 'outline:1px solid ' + borderColor + ';'                 // outer border (no offset)
                                          + 'box-shadow: inset 0 0 0 0 ' + borderColor + ';'       // inner border (always visible)
                                          +    'overflow:hidden;">'
                                          // table wrapper to center vertically (no flex)
                                          +    '<div style="display:table; width:100%; height:100%;">'
                                          +        '<div style="display:table-cell; vertical-align:bottom; text-align:center;">'
                                          // keep the original vertical writing mode + rotation and font size intact
                                          +            '<span style="writing-mode:vertical-rl; text-orientation:mixed; transform:rotate(180deg);'
                                          +                         'font-size:' + truncatedFontSize + 'px; color:' + textColor + ';'
                                          +                         'display:inline-block; line-height:1; white-space:nowrap; overflow:hidden; text-overflow:ellipsis;">'
                                          +                safe
                                          +            '</span>'
                                          +        '</div>'
                                          +    '</div>'
                                          + '</div>';

                                      return html;
                                  } catch (e) {
                                      return '';
                                  }
                              },
                              style: { textOutline: 'none' }
                          }
                      }
                    ],
                    credits: { enabled: false }
                };

                // create or update chart instance
                if (window._placementChart && typeof window._placementChart.update === 'function') {
                    try {
                        window._placementChart.update(chartOptions, true, true);
                        if (typeof window._placementChart.setSize === 'function') {
                            window._placementChart.setSize(idealInnerWidth, window._placementChart.chartHeight || computedChartHeight, false);
                        }
                    } catch (e) {
                        try { window._placementChart.destroy(); } catch (er) {}
                        window._placementChart = Highcharts.chart(containerId, chartOptions);
                    }
                } else {
                    window._placementChart = Highcharts.chart(containerId, chartOptions);
                }
                wirePlacementReportUI(window._placementChart);

                addExportAnnotations(window._placementChart, {
                    totalText: null,
                    legendLabel: 'Current Student',
                    legendColor: '#C8F7C5',
                    printedPrefix: 'Printed on:'
                });

                // responsiveness (horizontal scroll)
                var resizeTimeout = null;

                // helper to resolve wrapper safely (falls back to container)
                function resolveWrapper() {
                    var w = null;
                    try {
                        w = document.getElementById(wrapperId);
                    } catch (e) { w = null; }
                    if (!w) {
                        // if wrapper not present, fall back to container element
                        w = container || document.getElementById(containerId) || null;
                    }
                    return w;
                }

                function adjustChartOnResize() {
                    try {
                        var w = resolveWrapper();
                        if (!w) {
                            // nothing to do if we can't find a usable element
                            return;
                        }
                        var visibleWidth = (typeof w.clientWidth === 'number') ? w.clientWidth : (w.offsetWidth || (container && container.clientWidth) || idealInnerWidth);
                        var newWidth = Math.max(idealInnerWidth, visibleWidth);

                        if (window._placementChart && typeof window._placementChart.setSize === 'function') {
                            try {
                                // only call setSize if chart exists
                                window._placementChart.setSize(newWidth, window._placementChart.chartHeight || computedChartHeight, false);
                            } catch (e) {
                                console.warn('adjustChartOnResize: setSize failed', e);
                            }
                        }
                        // keep container sized to the new inner width
                        try { container.style.minWidth = newWidth + 'px'; container.style.width = newWidth + 'px'; } catch (e) {}
                    } catch (err) {
                        console.warn('adjustChartOnResize error', err);
                    }
                }

                function onResize() { clearTimeout(resizeTimeout); resizeTimeout = setTimeout(adjustChartOnResize, 120); }

                // Clean up previous handler safely before adding new one
                try {
                    if (window.__placement_onResizeRef && typeof window.removeEventListener === 'function') {
                        try { window.removeEventListener('resize', window.__placement_onResizeRef); } catch (e) {}
                    }
                } catch(e) {}

                window.__placement_onResizeRef = onResize;
                window.addEventListener('resize', onResize);

                // export a safe cleanup function (destroy chart and remove listener)
                window._placementChartCleanup = function () {
                    try { window.removeEventListener('resize', window.__placement_onResizeRef); } catch (e) {}
                    try { window.__placement_onResizeRef = null; } catch (e) {}
                    try { if (window._placementChart) { window._placementChart.destroy(); window._placementChart = null; } } catch (e) {}
                };

                if (window._placementChart && typeof window._placementChart.reflow === 'function') window._placementChart.reflow();
                adjustChartOnResize();

            } catch (err) {
                console.error('renderPlacementChart error', err && err.stack ? err.stack : err);
                try {
                    container.innerHTML = '<pre style="color:red;white-space:pre-wrap;">Chart error:\\n' + (err && err.stack ? err.stack : String(err)) + '</pre>';
                } catch (e) {}
            }
        }

        // compatibility wrapper for server calls
        window.renderAggregatedPlacementChart = function(data) {
            try { renderPlacementChart(data || []); } catch (e) { console.error('renderAggregatedPlacementChart wrapper error', e); }
        };


        function drawYearSpans(chart, yearGroups) {
            try {
                if (chart._yearGroup && chart._yearGroup.destroy) {
                    chart._yearGroup.destroy();
                    chart._yearGroup = null;
                }
                var g = chart.renderer.g('yearGroups').attr({ zIndex: 120 }).add();
                chart._yearGroup = g;

                var xAxis = chart.xAxis && chart.xAxis[0];
                if (!xAxis) return;

                // plot area geometry (safe fallbacks)
                var plotLeft = chart.plotLeft || 0;
                var plotTop = chart.plotTop || 0;
                var plotWidth = chart.plotWidth || Math.max(0, (chart.chartWidth || 0) - plotLeft);
                var plotHeight = chart.plotHeight || Math.max(0, (chart.chartHeight || 0) - plotTop);

                var totalCategories = (xAxis && xAxis.categories && xAxis.categories.length) ? xAxis.categories.length : 0;
                if (!totalCategories) return;

                // compute fixed category width (pixel) and ensure it matches the plotted area
                var catWidth = plotWidth / totalCategories;

                // visuals
                var padSides = 4; // small horizontal padding inside each group box
                var rectStroke = '#444';
                var rectStrokeWidth = 1;
                var rectFill = 'rgba(255,255,255,0)'; // change if you want fill
                var rectR = 4;

                // compute rectangle vertical position: just below plot area (adjust these numbers if needed)
                var rectY = Math.round(plotTop + plotHeight + 2); // top of box
                var rectHeight = 28; // height of the box that encloses Q labels + year
                var yearLabelYOffset = 6; // vertical centering tweak inside rect

                yearGroups.forEach(function (yg) {
                try {
                        // use equal-width logic: left = plotLeft + startIndex * catWidth
                        var left = plotLeft + (yg.startIndex * catWidth) - padSides;
                        var width = Math.max(8, Math.round((yg.endIndex - yg.startIndex + 1) * catWidth + padSides * 2));

                        // clamp within plot area
                        if (left < plotLeft) left = plotLeft;
                        if (left + width > plotLeft + plotWidth) width = Math.round(Math.max(8, plotLeft + plotWidth - left));

                        // draw the rectangle border
                        chart.renderer.rect(left, rectY, width, rectHeight, rectR)
                            .attr({ fill: rectFill, stroke: rectStroke, 'stroke-width': rectStrokeWidth })
                            .add(g);

                        // draw centered year text inside the rect
                        var centerX = Math.round(left + width / 2);
                        var yearLabelY = rectY + Math.round(rectHeight / 2) + yearLabelYOffset;

                        chart.renderer.text(String(yg.year), centerX, yearLabelY)
                            .attr({ align: 'center' })
                            .css({ color: '#222', fontSize: '12px', fontWeight: '600' })
                            .add(g);

                    } catch (inner) {
                        console.warn('drawYearSpans group failed', inner);
                    }
                    });
                } catch (e) {
                console.warn('drawYearSpans error', e);
                }
            }



        //function wirePlacementReportUI(chart) {
        //    document.getElementById('exportChartBtn').onclick = function () {
        //        console.log("3726");
        //        try {
        //            // find chart (use your existing chart variable)
        //            var foundChart = (typeof chart !== 'undefined' && chart) ? chart : (window.chart || (Highcharts && Highcharts.charts && Highcharts.charts[0]));
        //            if (!foundChart) { alert('Chart not found'); return; }

        //            // compute desired rasterScale: try 300 DPI
        //            // we need the final draw width in px that exporter uses (it computes scale to fit page).
        //            // As an approximation call exporter once with rasterScale=1 to compute drawW, or compute from chart width:
        //            var approxSvgW = foundChart.chartWidth || 800;
        //            // desired DPI:
        //            var desiredDPI = 300;
        //            // compute scale factor relative to 96ppi baseline:
        //            var rasterScale = Math.ceil((desiredDPI / 96) * 1); // e.g. 300/96 = 3.125 -> 4
        //            // more conservative: clamp to 1..4 to avoid extremely large PDFs
        //            rasterScale = Math.min(Math.max(1, rasterScale), 4);

        //            // call high-res exporter (use rasterScale computed)
        //            exportSvgToPdfRasterHighRes(foundChart, 'placement-chart.pdf', {
        //                page: 'A4',
        //                orientation: 'landscape',
        //                marginMm: 10,
        //                rasterScale: rasterScale
        //            });
        //        } catch (e) {
        //            console.error(e);
        //            alert('Export failed — see console');
        //        }
        //    };
        //    var sel = document.getElementById('exportFormatSelect');
        //    // prevent server fallback globally
        //    if (window.Highcharts && Highcharts.setOptions) {
        //        Highcharts.setOptions({ exporting: { fallbackToExportServer: false , url: '' } });
        //    }
        //}

        function addExportAnnotations(chart, opts) {
            opts = opts || {};
            var totalText = (typeof opts.totalText !== 'undefined') ? opts.totalText : null;
            var legendLabel = opts.legendLabel || 'Current Student';
            var legendColor = opts.legendColor || '#C8F7C5';
            var printedPrefix = opts.printedPrefix || 'Printed on:';

            if (!chart) return;
            if (!chart._customAnnotations) chart._customAnnotations = {};

            function clear() {
                var c = chart._customAnnotations;
                Object.keys(c).forEach(function (k) {
                    try { c[k].destroy(); } catch (e) {}
                    delete c[k];
                });
            }

            function build() {
                clear();
                var c = chart._customAnnotations;
                var sb = chart.spacingBox;   // ← THIS IS OUTSIDE THE INNER BORDER

                // compute total if required
                var total = 0;
                    try {
                        // prefer explicit column series (assumed index 0)
                        var colSeries = (chart.series && chart.series[0]) ? chart.series[0] : null;
                        if (colSeries && Array.isArray(colSeries.data)) {
                            total = colSeries.data.reduce(function (acc, p) {
                                var v = 0;
                                if (typeof p.y === 'number') v = p.y;
                                else if (p && p.options && typeof p.options.y === 'number') v = p.options.y;
                                else if (typeof p.options === 'object' && typeof p.options.value === 'number') v = p.options.value;
                                return acc + (isNaN(v) ? 0 : v);
                            }, 0);
                        } else {
                            // fallback: try summing chart.userBuckets (if you exposed buckets) or other known data
                            if (chart.userBuckets && Array.isArray(chart.userBuckets)) {
                                total = chart.userBuckets.reduce(function (s, b) { return s + (b.count || 0); }, 0);
                            } else {
                                // worst-case: iterate all series but ignore scatter (type === 'scatter')
                                (chart.series || []).forEach(function (s) {
                                    if (s.type && s.type.toLowerCase() === 'scatter') return;
                                    (s.data || []).forEach(function (p) {
                                        var v = (typeof p.y === 'number') ? p.y : (p && p.options && typeof p.options.y === 'number' ? p.options.y : 0);
                                        total += isNaN(v) ? 0 : v;
                                    });
                                });
                            }
                        }
                    } catch (eTot) {
                        console.warn('[anno-v2] compute total failed', eTot);
                        total = 0;
                    }
                if (totalText !== null) total = totalText; // still allow forced override
                console.log('[anno-v2] computed total =', total);

                // ---- TOP-LEFT: TOTAL ----
                c.total = chart.renderer.text(
                    "Total = " + total,
                    sb.x + 5,
                    sb.y + 15
                )
                .css({ fontSize: "12px", fontWeight: "600", color: "#000" })
                .add();

                // ---- TOP-RIGHT: LEGEND BOX ----
                var boxW = 150, boxH = 26;
                var rightX = sb.x + sb.width - boxW - 5;    // right aligned in outer border
                var topY = sb.y + 2;

                //c.legendBox = chart.renderer.rect(rightX, topY, boxW, boxH, 4)
                //    .attr({ fill: "rgba(255,255,255,0.95)", stroke: "#000", "stroke-width": 1 })
                //    .add();

                c.legendSample = chart.renderer.rect(rightX + 6, topY + 6, 12, 12)
                    .attr({ fill: legendColor, stroke: "#333" })
                    .add();

                c.legendText = chart.renderer.text(
                    legendLabel,
                    rightX + 6 + 16 + 4,
                    topY + 18
                )
                .css({ fontSize: "11px", color: "#000" })
                .add();

                // ---- BOTTOM-RIGHT: PRINTED ON ----
                var printed = printedPrefix + " " + (new Date()).toLocaleDateString();
                c.printed = chart.renderer.text(
                    printed,
                    sb.x + sb.width - 5,
                    sb.y + sb.height +80
                )
                .css({ fontSize: "10px", color: "#000" })
                .attr({ align: "right" })
                .add();
            }

            build();

            // Rebuild after redraw
            if (!chart._annoHooked) {
                chart._annoHooked = true;
                var orig = chart.redraw;
                chart.redraw = function () {
                    var r = orig.apply(this, arguments);
                    build();
                    return r;
                };
            }
        }

        function computeRasterScaleForDPI(svgDrawWidthPx, desiredDPI) {
            // convert desired DPI to px per inch used in jsPDF (we used 96 px per inch earlier)
            var pxPerInch = 96; // our SVG/page pixel mapping
            // target pixel density = desiredDPI / 96  (i.e. how many screen px per our px)
            // rasterScale = targetPixelDensity
            return Math.max(1, Math.ceil((desiredDPI / pxPerInch) * 1)); // integer scale >=1
        }

        function exportSvgToPdfRasterHighRes(chart, filename, opts) {
            try {
                if (!chart) { alert('Chart instance missing'); return; }
                filename = filename || 'placement-chart.pdf';
                opts = opts || {};
                var page = (opts.page || 'A4').toUpperCase();
                var orientation = (opts.orientation || 'landscape').toLowerCase();
                var marginMm = (typeof opts.marginMm === 'number') ? opts.marginMm : 10;
                var rasterScale = (typeof opts.rasterScale === 'number' && opts.rasterScale > 0) ? opts.rasterScale : (window.devicePixelRatio || 2);

                // annotation options
                var includeAnnotations = (typeof opts.includeAnnotations === 'boolean') ? opts.includeAnnotations : true;
                var removeAfter = (typeof opts.removeAnnotationsAfter === 'boolean') ? opts.removeAnnotationsAfter : false;
                var total = 0;
                    try {
                        // prefer explicit column series (assumed index 0)
                        var colSeries = (chart.series && chart.series[0]) ? chart.series[0] : null;
                        if (colSeries && Array.isArray(colSeries.data)) {
                            total = colSeries.data.reduce(function (acc, p) {
                                var v = 0;
                                if (typeof p.y === 'number') v = p.y;
                                else if (p && p.options && typeof p.options.y === 'number') v = p.options.y;
                                else if (typeof p.options === 'object' && typeof p.options.value === 'number') v = p.options.value;
                                return acc + (isNaN(v) ? 0 : v);
                            }, 0);
                        } else {
                            // fallback: try summing chart.userBuckets (if you exposed buckets) or other known data
                            if (chart.userBuckets && Array.isArray(chart.userBuckets)) {
                                total = chart.userBuckets.reduce(function (s, b) { return s + (b.count || 0); }, 0);
                            } else {
                                // worst-case: iterate all series but ignore scatter (type === 'scatter')
                                (chart.series || []).forEach(function (s) {
                                    if (s.type && s.type.toLowerCase() === 'scatter') return;
                                    (s.data || []).forEach(function (p) {
                                        var v = (typeof p.y === 'number') ? p.y : (p && p.options && typeof p.options.y === 'number' ? p.options.y : 0);
                                        total += isNaN(v) ? 0 : v;
                                    });
                                });
                            }
                        }
                    } catch (eTot) {
                        console.warn('[anno-v2] compute total failed', eTot);
                        total = 0;
                    };

                var annotationOptions = opts.annotationOptions || { totalText: total, legendLabel: 'Current Student', legendColor: '#C8F7C5', printedPrefix: 'Printed on:' };

                // try to create renderer annotations (best-effort, harmless if missing)
                try { if (includeAnnotations && typeof addExportAnnotations_SVG === 'function') { addExportAnnotations_SVG(chart, annotationOptions); if (typeof chart.redraw === 'function') chart.redraw(); } } catch(e){ console.warn('addExportAnnotations_SVG failed', e); }

                // obtain SVG string (fallback if missing we'll still draw)
                var svgStr;
                try { svgStr = chart.getSVG(); } catch (getErr) { console.warn('chart.getSVG failed', getErr); svgStr = null; }

                if (!svgStr) {
                    alert('SVG not available for export, but will attempt raster fallback.');
                    // continue — we'll still try to build an image from chart by serializing current container via svgStr fallback
                    // but in this implementation we abort early only if svg missing and we cannot proceed
                    return;
                }

                // parse svg size
                var tmp = document.createElement('div'); tmp.innerHTML = svgStr;
                var svgEl = tmp.querySelector && tmp.querySelector('svg');
                var svgW = (svgEl && parseFloat(svgEl.getAttribute('width'))) || chart.chartWidth || 800;
                var svgH = (svgEl && parseFloat(svgEl.getAttribute('height'))) || chart.chartHeight || 400;

                // page sizes in mm
                var pageSizes = { 'A4': { w:210, h:297 }, 'LETTER': { w:216, h:279 } };
                var ps = pageSizes[page] || pageSizes['A4'];

                // convert mm to px (approx 96 DPI)
                var pxPerMm = 96 / 25.4;
                var pageWpx = ps.w * pxPerMm;
                var pageHpx = ps.h * pxPerMm;

                // swap for orientation
                var pageWidthPx = (orientation === 'landscape') ? pageHpx : pageWpx;
                var pageHeightPx = (orientation === 'landscape') ? pageWpx : pageHpx;

                var marginPx = marginMm * pxPerMm;
                var availW = pageWidthPx - 2 * marginPx;
                var availH = pageHeightPx - 2 * marginPx;

                // compute scale to fit inside available area (document units are px)
                var scale = Math.min(availW / svgW, availH / svgH, 1.0);

                var drawW = Math.round(svgW * scale);
                var drawH = Math.round(svgH * scale);

                // High res canvas size
                var canvasW = Math.round(drawW * rasterScale);
                var canvasH = Math.round(drawH * rasterScale);

                // create img from svg string
                var img = new Image();
                img.onload = function() {
                    try {
                        // create hi-res canvas and draw the image scaled up
                        var canvas = document.createElement('canvas');
                        canvas.width = canvasW;
                        canvas.height = canvasH;
                        var ctx = canvas.getContext('2d');

                        // drawing the SVG into canvas (stretched to canvas size)
                        try { ctx.imageSmoothingEnabled = true; ctx.imageSmoothingQuality = 'high'; } catch(e){}

                        ctx.clearRect(0,0,canvasW,canvasH);
                        ctx.drawImage(img, 0, 0, canvasW, canvasH);

                        // === NEW: draw annotations directly onto the canvas (so they appear in exported PDF) ===
                        if (includeAnnotations) {
                            try {
                                // compute scale from SVG user units -> canvas pixels
                                var scaleCanvas = canvasW / svgW; // mapping: svg coordinate * scaleCanvas => canvas px

                                // compute spacingBox in svg coords (same approach as injection)
                                var sb = chart.spacingBox;
                                if (!sb || typeof sb.x !== 'number') {
                                    sb = { x: 10, y: 10, width: (chart.chartWidth || svgW) - 20, height: (chart.chartHeight || svgH) - 20 };
                                }

                                // compute text values
                                var total = annotationOptions.totalText;
                                if (typeof total === 'undefined' || total === null) {
                                    total = 0;
                                    (chart.series || []).forEach(function (s) {
                                        (s.data || []).forEach(function (p) {
                                            var v = (typeof p.y === 'number') ? p.y : (p && p.options && typeof p.options.y === 'number' ? p.options.y : 0);
                                            total += (isNaN(v) ? 0 : v);
                                        });
                                    });
                                }
                                var legendLabel = annotationOptions.legendLabel || 'Current Student';
                                var legendColor = annotationOptions.legendColor || '#C8F7C5';
                                var printedPrefix = annotationOptions.printedPrefix || 'Printed on:';
                                var printedText = printedPrefix + ' ' + (new Date()).toLocaleDateString();

                                // coordinates (svg units)
                                var totalX = sb.x + 5, totalY = sb.y + 14;
                                var boxW = 150, boxH = 26, boxPad = 6;
                                var rightX = sb.x + sb.width - boxW - 5;
                                if (rightX < sb.x) rightX = Math.max(sb.x, (chart.chartWidth || svgW) - boxW - 10);
                                var topY = sb.y + 2;
                                var printedX = sb.x + sb.width - 5, printedY = sb.y + sb.height +80;

                                // scale coords to canvas pixels
                                var cTotalX = Math.round(totalX * scaleCanvas);
                                var cTotalY = Math.round(totalY * scaleCanvas);
                                var cRightX = Math.round(rightX * scaleCanvas);
                                var cTopY = Math.round(topY * scaleCanvas);
                                var cBoxW  = Math.round(boxW * scaleCanvas);
                                var cBoxH  = Math.round(boxH * scaleCanvas);
                                var cBoxPad = Math.round(boxPad * scaleCanvas);
                                var cPrintedX = Math.round(printedX * scaleCanvas);
                                var cPrintedY = Math.round(printedY * scaleCanvas);

                                // draw "Total = N" (use fillText)
                                ctx.save();
                                // choose font - scale font size consistent with svg units -> choose 12px * scaleCanvas / rasterScale? Better: compute visually
                                var baseFontPx = Math.round(12 * scaleCanvas);
                                if (baseFontPx < 8) baseFontPx = 8;
                                ctx.font = (baseFontPx|0) + 'px sans-serif';
                                ctx.fillStyle = '#000';
                                ctx.textBaseline = 'alphabetic';
                                // note: y in canvas fillText is baseline; adjust a bit to mimic SVG y placement
                                ctx.fillText('Total = ' + total, cTotalX, cTotalY);
                                ctx.restore();

                                // draw legend box (background rect)
                                ctx.save();
                                ctx.fillStyle = 'rgba(255,255,255,0.95)';
                                //ctx.strokeStyle = '#000';
                                //ctx.lineWidth = Math.max(1, Math.round(1 * scaleCanvas));
                                ctx.fillRect(cRightX, cTopY, cBoxW, cBoxH);
                                //ctx.strokeRect(cRightX, cTopY, cBoxW, cBoxH);

                                // draw color sample
                                var cSampleX = cRightX + cBoxPad;
                                var cSampleY = cTopY + Math.round((cBoxH - Math.round(12*scaleCanvas))/2);
                                ctx.fillStyle = legendColor;
                                ctx.fillRect(cSampleX, cSampleY, Math.round(12*scaleCanvas), Math.round(12*scaleCanvas));
                                ctx.strokeStyle = '#333';
                                ctx.strokeRect(cSampleX, cSampleY, Math.round(12*scaleCanvas), Math.round(12*scaleCanvas));

                                // draw legend text
                                var legendFontPx = Math.max(8, Math.round(11 * scaleCanvas));
                                ctx.font = legendFontPx + 'px sans-serif';
                                ctx.fillStyle = '#000';
                                ctx.textBaseline = 'middle';
                                ctx.fillText(legendLabel, cSampleX + Math.round(12*scaleCanvas) + Math.round(6*scaleCanvas), cTopY + Math.round(cBoxH/2));

                                // draw printed text right aligned
                                var printedFontPx = Math.max(8, Math.round(10 * scaleCanvas));
                                ctx.font = printedFontPx + 'px sans-serif';
                                ctx.fillStyle = '#000';
                                ctx.textBaseline = 'alphabetic';
                                var printedTextWidth = ctx.measureText(printedText).width;
                                ctx.fillText(printedText, cPrintedX - printedTextWidth, cPrintedY);

                                ctx.restore();
                            } catch (drawAnnErr) {
                                console.warn('Drawing annotations onto canvas failed', drawAnnErr);
                            }
                        }

                        // prepare pdf (page size in px)
                        var jsPDFCtor = (window.jspdf && window.jspdf.jsPDF) ? window.jspdf.jsPDF : (window.jsPDF || null);
                        if (!jsPDFCtor) { alert('jsPDF not found. Include jspdf.umd.min.js'); return; }
                        var pdf = new jsPDFCtor({ unit: 'px', format: [pageWidthPx, pageHeightPx], orientation: (pageHeightPx >= pageWidthPx ? 'portrait' : 'landscape') });

                        // compute placement (center) in PDF units (drawW, drawH are in PDF px units)
                        var x = Math.round((pageWidthPx - drawW)/2);
                        var y = Math.round((pageHeightPx - drawH)/2);

                        // Convert hi-res canvas to data URL (PNG)
                        var dataUrl = canvas.toDataURL('image/png', 1.0);

                        // add image to PDF at target draw size (not full hi-res pixel size)
                        pdf.addImage(dataUrl, 'PNG', x, y, drawW, drawH);
                        pdf.save(filename);
                    } catch (err) {
                        console.error('High-res export failed', err);
                        alert('High-res export failed — see console.');
                    } finally {
                        // optionally remove renderer annotations if requested
                        try {
                            if (removeAfter && chart._annotationsGroup) {
                                try { chart._annotationsGroup.destroy(); chart._annotationsGroup = null; if (typeof chart.redraw === 'function') chart.redraw(); } catch(e) {}
                            }
                        } catch(e){}
                    }
                };

                img.onerror = function(e) {
                    console.error('Failed to load SVG into Image for high-res render', e);
                    alert('Failed to render SVG image. See console.');
                };

                // load image (encoded SVG)
                try {
                    var svg64 = btoa(unescape(encodeURIComponent(svgStr)));
                    img.src = 'data:image/svg+xml;base64,' + svg64;
                } catch (eBase64) {
                    img.src = 'data:image/svg+xml;charset=utf-8,' + encodeURIComponent(svgStr);
                }

            } catch (outer) {
                console.error('exportSvgToPdfRasterHighRes outer error', outer);
                alert('Export failed. See console for details.');
            }
        }












        function addExportAnnotations_SVG(chart, opts) {
            opts = opts || {};
            var totalText = (typeof opts.totalText !== 'undefined') ? opts.totalText : null;
            var legendLabel = opts.legendLabel || 'Current Student';
            var legendColor = opts.legendColor || '#C8F7C5';
            var printedPrefix = opts.printedPrefix || 'Printed on:';

            if (!chart || !chart.renderer) return;

            // remove existing group if present
            try {
                if (chart._annotationsGroup && chart._annotationsGroup.destroy) {
                    chart._annotationsGroup.destroy();
                }
            } catch (e) { /* ignore */ }

            // create a group attached to the renderer root (so it will be serialized in getSVG)
            var g = chart.renderer.g('customAnnotations').attr({ zIndex: 1000 }).add();

            // compute total if not provided
            var total = 0;
            if (totalText !== null) {
                total = totalText;
            } else {
                (chart.series || []).forEach(function (s) {
                    (s.data || []).forEach(function (p) {
                        var v = (typeof p.y === 'number') ? p.y : (p && p.options && typeof p.options.y === 'number' ? p.options.y : 0);
                        total += (isNaN(v) ? 0 : v);
                    });
                });
            }

            // Using spacingBox for positions outside inner plot but within outer chart
            var sb = chart.spacingBox || { x: chart.chartWidth*0.05||10, y: chart.chartHeight*0.02||10, width: chart.chartWidth-20, height: chart.chartHeight-20 };

            // TOP-LEFT: Total = N (placed relative to spacingBox)
            var txtTotal = chart.renderer.text('Total = ' + total, sb.x + 5, sb.y + 15)
                .css({ fontSize: '12px', fontWeight: '600', color: '#000' })
                .add(g);

            // TOP-RIGHT: Legend box (rect + color sample + label)
            var boxW = 150, boxH = 26, boxPadding = 6;
            var rightX = sb.x + sb.width - boxW - 5;
            if (rightX < sb.x) rightX = Math.max(sb.x, chart.chartWidth - boxW - 10)+2;
            rightX = rightX + 2;
            var boxTop = sb.y + 2 + 1;

            //var rect = chart.renderer.rect(rightX, sb.y + 2, boxW, boxH, 4)
            //    .attr({ fill: 'rgba(255,255,255,0.95)', stroke: '#000', 'stroke-width': 1 })
            //    .add(g);

            var sw = 12;
            var sample = chart.renderer.rect(rightX + boxPadding-2, sb.y + 1 + (boxH - sw) / 2, sw, sw, 0)
                .attr({ fill: legendColor, stroke: '#333' })
                .add(g);

            var legendTxt = chart.renderer.text(legendLabel, rightX + boxPadding + sw + 6, sb.y + 2 + boxH/2 + 4)
                .css({ fontSize: '11px', color: '#000' })
                .attr({ align: 'left' })
                .add(g);

            // BOTTOM-RIGHT: Printed on
            var printed = printedPrefix + ' ' + (new Date()).toLocaleDateString();
            var printedTxt = chart.renderer.text(
                    printed,
                    sb.x + sb.width - 5,
                    sb.y + sb.height + 80   // was -6 → add 10px downward
                )
                .css({ fontSize: '10px', color: '#000' })
                .attr({ align: 'right' })
                .add(g);

            // Keep the group reference on the chart for cleanup later
            chart._annotationsGroup = g;

            // return group
            return g;
        }



        function ChangeSelectedMenu(menuName) {
            try {
                // remove highlight from all
                $('.menuButton').removeClass('menuSelected');

                // add highlight to the selected one
                $('#' + menuName).addClass('menuSelected');

                // store current menu selection if needed
                var hdn = document.getElementById('<%= hdnMenu.ClientID %>');
                if (hdn) hdn.value = menuName;
            } catch (e) {
                console.log('ChangeSelectedMenu error:', e);
            }
        }





        function createSvgLabelsForExport_v2(chart) {
            var created = { svgEls: [], hiddenLabels: [], group: null };
            try {
                if (!chart) return created;

                // Create a group to hold created labels
                var group = chart.renderer.g('export-labels-group').add();
                created.group = group;

                // small vertical offset to nudge text into bar visually; tweak if needed
                var yOffset = 0; // try 0, 4, -4 depending on alignment

                for (var s = 0; s < chart.series.length; s++) {
                    var series = chart.series[s];
                    // We want to recreate labels only for the series that used HTML labels originally
                    var useHtml = series.options && series.options.dataLabels && series.options.dataLabels.useHTML;
                    if (!useHtml && series.type !== 'scatter') continue;

                    for (var p = 0; p < series.data.length; p++) {
                        var point = series.data[p];
                        if (!point) continue;

                        // Try to find the HTML element for the label (Highcharts puts it on dataLabel.element or .div)
                        var dl = point.dataLabel;
                        var htmlEl = null;
                        if (dl && dl.element && (dl.element.tagName || '').toLowerCase() !== 'svg') {
                            htmlEl = dl.element;
                        } else if (dl && dl.div) {
                            htmlEl = dl.div;
                        }

                        // If there's no HTML label, skip
                        if (!htmlEl) continue;

                        // Hide HTML label so it won't overlap exported SVG
                        try { htmlEl.style.visibility = 'hidden'; created.hiddenLabels.push(htmlEl); } catch (e) {}

                        // compute SVG coordinates for label based on point.plotX/plotY
                        var plotX = (typeof point.plotX === 'number') ? point.plotX : 0;
                        var plotY = (typeof point.plotY === 'number') ? point.plotY : 0;

                        var x = Math.round(chart.plotLeft + plotX);
                        var y = Math.round(chart.plotTop + plotY + yOffset);

                        // label text: prefer point.name -> point.options.name -> category
                        var text = (point.name && String(point.name).trim()) || (point.options && point.options.name) || (point.category || '');
                        if (!text) continue;

                        // Create SVG text element, center it and rotate 90 degrees clockwise so it reads top->bottom
                        // We set text-anchor to middle so x is the center; rotation is around the (x,y)
                        var svgText = chart.renderer.text(text, x, y)
                            .attr({ align: 'center' })
                            .css({
                                fontSize: (series.options.dataLabels && series.options.dataLabels.style && series.options.dataLabels.style.fontSize) || '11px',
                                lineHeight: '1',
                                whiteSpace: 'nowrap',
                                color: (series.options.dataLabels && series.options.dataLabels.style && series.options.dataLabels.style.color) || '#222'
                            })
                            .add(group);

                        // rotate 90 degrees around (x, y) so text orientation matches screen's vertical-rl + rotate(180deg)
                        // If you see upside-down, change 90 to -90.
                        try {
                            svgText.attr({ rotation: 90 });
                            // Some Highcharts versions require explicit transform for origin:
                            // svgText.element.setAttribute('transform', 'rotate(90 ' + x + ' ' + y + ')');
                        } catch (eRot) {
                            try {
                                svgText.element.setAttribute('transform', 'rotate(90 ' + x + ' ' + y + ')');
                            } catch (e2) {}
                        }

                        // center anchor in SVG (text-anchor)
                        try {
                            svgText.element.setAttribute('text-anchor', 'middle');
                        } catch (eAttr) {}

                        created.svgEls.push(svgText);
                    }
                }

                return created;
            } catch (err) {
                console.error('createSvgLabelsForExport_v2 failed', err);
                // cleanup on error
                try {
                    if (created.svgEls) created.svgEls.forEach(function (el) { try { el.destroy(); } catch (e) {} });
                    if (created.hiddenLabels) created.hiddenLabels.forEach(function (el) { try { el.style.visibility = ''; } catch (e) {} });
                    if (created.group) try { created.group.destroy(); } catch (e) {}
                } catch (e) {}
                return created;
            }
        }

        function restoreHtmlLabelsFromExport_v2(created, chart) {
            try {
                if (!created) return;
                if (created.svgEls && created.svgEls.length) {
                    for (var i = 0; i < created.svgEls.length; i++) {
                        try { created.svgEls[i].destroy(); } catch (e) {}
                    }
                }
                if (created.hiddenLabels && created.hiddenLabels.length) {
                    for (var j = 0; j < created.hiddenLabels.length; j++) {
                        try { created.hiddenLabels[j].style.visibility = ''; } catch (e) {}
                    }
                }
                if (created.group) {
                    try { created.group.destroy(); } catch (e) {}
                }
            } catch (e) {
                console.warn('restoreHtmlLabelsFromExport_v2 error', e);
            }
        }

       


        


        function exportSvgToPdfRasterRobust(chart, filename) {
            try {
                if (!chart) { alert('Chart instance missing'); return; }
                filename = filename || 'placement-chart.pdf';

                // get SVG string
                var svgStr = chart.getSVG();
                if (!svgStr) { alert('SVG not available'); return; }

                // parse svg for width/height fallback
                var tmp = document.createElement('div'); tmp.innerHTML = svgStr;
                var svgEl = tmp.querySelector && tmp.querySelector('svg');
                var w = (svgEl && parseFloat(svgEl.getAttribute('width'))) || chart.chartWidth || 800;
                var h = (svgEl && parseFloat(svgEl.getAttribute('height'))) || chart.chartHeight || 400;

                // prepare canvas
                var canvas = document.createElement('canvas');
                canvas.width = Math.ceil(w);
                canvas.height = Math.ceil(h);
                var ctx = canvas.getContext('2d');

                // helper to save canvas as PDF
                function saveCanvasAsPdf() {
                    try {
                        var jsPDFCtor = (window.jspdf && window.jspdf.jsPDF) ? window.jspdf.jsPDF : (window.jsPDF || null);
                        if (!jsPDFCtor) { alert('jsPDF not loaded. Include jspdf.umd.min.js'); return; }
                        var pdf = new jsPDFCtor({ unit: 'px', format: [canvas.width, canvas.height] });
                        var dataUrl = canvas.toDataURL('image/png', 1.0);
                        pdf.addImage(dataUrl, 'PNG', 0, 0, canvas.width, canvas.height);
                        pdf.save(filename);
                    } catch (err) {
                        console.error('saveCanvasAsPdf error', err);
                        alert('PDF save failed — see console.');
                    }
                }

                // Image fallback: draw encoded SVG into an Image and onto canvas
                function drawViaImage() {
                    var img = new Image();
                    img.onload = function () {
                        try { ctx.clearRect(0,0,canvas.width,canvas.height); ctx.drawImage(img,0,0,canvas.width,canvas.height); saveCanvasAsPdf(); }
                        catch(e){ console.error('drawImage error', e); alert('drawImage failed — see console.'); }
                    };
                    img.onerror = function(e){ console.error('Image load error', e); alert('Image fallback failed — see console.'); };
                    img.src = 'data:image/svg+xml;charset=utf-8,' + encodeURIComponent(svgStr);
                }

                // Try various canvg shapes
                try {
                    var CanvgCtor = window.Canvg || window.canvg || (window.canvg && window.canvg.Canvg) || null;
                    if (CanvgCtor) {
                        // Modern API: Canvg.from(ctx, svgStr).render()
                        if (typeof CanvgCtor.from === 'function') {
                            try {
                                var maybe = CanvgCtor.from(ctx, svgStr);
                                // if returns promise-like or object with render()
                                if (maybe && typeof maybe.then === 'function') {
                                    maybe.then(function(inst){
                                        if (inst && typeof inst.render === 'function') {
                                            inst.render().then(saveCanvasAsPdf).catch(function(e){ console.warn('canvg.render promise failed', e); drawViaImage(); });
                                        } else {
                                            // if inst drew directly, try saving
                                            try { saveCanvasAsPdf(); } catch(e2){ drawViaImage(); }
                                        }
                                    }).catch(function(err){ console.warn('Canvg.from promise failed', err); drawViaImage(); });
                                    return;
                                } else if (maybe && typeof maybe.render === 'function') {
                                    var r = maybe.render();
                                    if (r && typeof r.then === 'function') { r.then(saveCanvasAsPdf).catch(function(e){ console.warn('render promise failed', e); drawViaImage(); }); }
                                    else { saveCanvasAsPdf(); }
                                    return;
                                }
                            } catch (eFrom) { console.warn('Canvg.from invocation failed', eFrom); }
                        }

                        // Older API: Canvg(ctx, svgStr)
                        try {
                            if (typeof CanvgCtor === 'function') {
                                var inst = CanvgCtor(ctx, svgStr);
                                if (inst && typeof inst.render === 'function') {
                                    var rr = inst.render();
                                    if (rr && typeof rr.then === 'function') { rr.then(saveCanvasAsPdf).catch(function(e){ console.warn('inst.render failed', e); drawViaImage(); }); }
                                    else { saveCanvasAsPdf(); }
                                    return;
                                }
                            }
                        } catch (eOld) { console.warn('Old Canvg invocation failed', eOld); }

                        // canvg.render(svgStr, ctx) style?
                        try {
                            if (window.canvg && typeof window.canvg.render === 'function') {
                                var maybeP = window.canvg.render(svgStr, ctx);
                                if (maybeP && typeof maybeP.then === 'function') { maybeP.then(saveCanvasAsPdf).catch(function(e){ console.warn('canvg.render promise failed', e); drawViaImage(); }); }
                                else { saveCanvasAsPdf(); }
                                return;
                            }
                        } catch (eRender) { console.warn('canvg.render attempt failed', eRender); }

                        // If canvg present but none matched, fallback
                        console.warn('canvg present but API not matched; falling back to Image method');
                        drawViaImage();
                        return;
                    }
                } catch (errCanvg) {
                    console.warn('canvg detection threw', errCanvg);
                }

                // No canvg -> image fallback
                drawViaImage();

            } catch (outer) {
                console.error('exportSvgToPdfRasterRobust outer error', outer);
                alert('Export failed — see console.');
            }
        }

        // wiring snippet example
        //var btn = document.getElementById('exportChartBtn');
        //if (btn) {
        //    btn.onclick = function () {
        //        console.log("4581");
        //        // ensure 'chart' is your Highcharts instance (replace variable name if different)
        //        exportSvgToPdfRasterRobust(chart, 'placement-chart.pdf');
        //    };
        //}



        function exportPlacementWithSvgLabelsAndPdf(opts) {
            opts = opts || {};
            opts.elementId = opts.elementId || "placementChartContainer";
            opts.filename = opts.filename || "PlacementPlanning.pdf";
            opts.mode = opts.mode || "multi";
            opts.scale = typeof opts.scale === "number" ? opts.scale : 2;

            var chart = window._placementChart; // your chart instance
            var originalUseHTML = null;
            var restored = false;

            try {
                // If we have a Highcharts chart instance, toggle useHTML -> false
                if (chart && chart.options && chart.options.xAxis && chart.options.xAxis[0]) {
                    try {
                        originalUseHTML = chart.options.xAxis[0].labels && chart.options.xAxis[0].labels.useHTML;
                        // Only update if currently using HTML labels
                        if (originalUseHTML) {
                            chart.update({
                                xAxis: [{
                                    labels: {
                                        useHTML: false,
                                        style: {
                                            color: '#000',      // ensure visible contrast in export
                                            fontSize: '10px',
                                            whiteSpace: 'normal'
                                        }
                                    }
                                }]
                            }, false); // do not redraw twice
                            // force redraw now
                            chart.redraw();
                        }
                    } catch (e) {
                        console.warn("Could not switch useHTML -> false for export:", e);
                    }
                }

                // Wait a tick for the chart to actually redraw (small timeout increases reliability)
                setTimeout(function () {
                    try {
                        // Call the html2canvas-based exporter you already have
                        exportChartElementToPdfHtml2Canvas({
                            elementId: opts.elementId,
                            mode: opts.mode,
                            scale: opts.scale,
                            filename: opts.filename
                        });
                    } finally {
                        // Restore original label rendering after a small delay so export finishes
                        setTimeout(function () {
                            try {
                                if (chart && originalUseHTML && chart.options && chart.options.xAxis && chart.options.xAxis[0]) {
                                    chart.update({
                                        xAxis: [{
                                            labels: {
                                                useHTML: true
                                            }
                                        }]
                                    }, false);
                                    chart.redraw();
                                }
                            } catch (er) {
                                console.warn("Failed to restore useHTML after export:", er);
                            }
                        }, 1200);
                    }
                }, 120); // small delay
            } catch (outerErr) {
                console.error("exportPlacementWithSvgLabelsAndPdf error:", outerErr);
                // fallback: call exporter directly
                exportChartElementToPdfHtml2Canvas({
                    elementId: opts.elementId,
                    mode: opts.mode,
                    scale: opts.scale,
                    filename: opts.filename
                });
            }
        }


        function truncateLeadingForSlot(name, fontSizePx, slotPx, extraChars) {
            if (!name) return '';
            try {
                extraChars = Number(extraChars) || 0; // optional extra chars to show beyond strict slot fit
                var maxChars = Math.max(1, Math.floor(slotPx / fontSizePx));
                // if name already fits, return it unchanged (no ellipsis)
                if (name.length <= maxChars) return name;

                // decide how many characters to take when truncating
                var take = Math.min(name.length, maxChars + extraChars);

                // if take covers entire name, just return name (no ellipsis)
                if (take >= name.length) return name;

                // special case: when maxChars is 1, show 1 char + ellipsis
                if (maxChars <= 1) return name.charAt(0) + '…';

                return name.substring(0, take) + '…';
            } catch (e) {
                return name;
            }
        }


        function drawQuarterBoxes(chart) {
            try {
                // remove previous group if any
                if (chart._quarterBoxGroup && chart._quarterBoxGroup.destroy) {
                    chart._quarterBoxGroup.destroy();
                    chart._quarterBoxGroup = null;
                }

                var g = chart.renderer.g('quarterBoxes').attr({ zIndex: 120 }).add();
                chart._quarterBoxGroup = g;

                var xAxis = chart.xAxis && chart.xAxis[0];
                if (!xAxis) return;

                // plot area geometry (safe fallbacks)
                var plotLeft = chart.plotLeft || 0;
                var plotTop = chart.plotTop || 0;
                var plotWidth = chart.plotWidth || Math.max(0, (chart.chartWidth || 0) - plotLeft);
                var plotHeight = chart.plotHeight || Math.max(0, (chart.chartHeight || 0) - plotTop);

                var categories = xAxis.categories || [];
                var totalCategories = categories.length;
                if (!totalCategories) return;

                // ----- CONFIGURABLE VISUALS -----
                var padSides = 4;            // horizontal padding removed from each side when computing cell area (tweak)
                var rectStroke = '#444';
                var rectStrokeWidth = 1;
                var rectFill = 'rgba(255,255,255,0)'; // transparent fill
                var rectR = 3;               // corner radius

                // label area sizes (tweak as needed)
                var qLabelHeight = 18;       // height for the Q label area inside box
                var rectHeight = qLabelHeight + 8; // rectangle height that encloses the Q label
                var rectY = Math.round(plotTop + plotHeight - qLabelHeight - 6); // tweak -6 if needed

                var yearGap = 8;
                var yearFontSize = 11;

                // Small right/left alignment nudge to fix overlap with first boxes
                // Set to 0 to disable. Start with 4 if you previously used that.
                var QY_ALIGNMENT_PX = 4;

                // --- compute integer widths distributed evenly across categories ---
                // Use the plot area reduced by padSides (so boxes don't overflow visual boundaries)
                var effectiveLeft = Math.round(plotLeft + padSides);
                var effectiveWidth = Math.max(0, Math.round(plotWidth - padSides * 2));

                // base width per category (integer), and distribute remainder pixels
                var baseW = Math.floor(effectiveWidth / totalCategories);
                var remainder = effectiveWidth - baseW * totalCategories; // 0..totalCategories-1

                // start x
                var curX = effectiveLeft;

                // track previous year so we only print year once per year group
                var prevYear = null;

                for (var i = 0; i < totalCategories; i++) {
                    try {
                        // give +1 pixel to the first `remainder` cells
                        var cellW = baseW + (i < remainder ? 1 : 0);

                        // if this is the very first cell, apply alignment nudge (shift right)
                        var left = curX;
                        if (i === 0 && QY_ALIGNMENT_PX) {
                            left += QY_ALIGNMENT_PX;
                            // reduce cell width so sum remains correct visually
                            cellW = Math.max(4, cellW - QY_ALIGNMENT_PX);
                        }

                        // If last cell, ensure it extends exactly to the plot right boundary (avoid 1px gap)
                        if (i === totalCategories - 1) {
                            var rightEdge = Math.round(plotLeft + plotWidth - padSides);
                            cellW = Math.max(4, rightEdge - left); // stretch last cell to exact rightEdge
                        }

                        // clamp left/width to plot area
                        if (left < plotLeft) left = plotLeft;
                        if (left + cellW > plotLeft + plotWidth) cellW = Math.round(Math.max(8, plotLeft + plotWidth - left));

                        // draw rect for this quarter
                        chart.renderer.rect(left, rectY, cellW, rectHeight, rectR)
                            .attr({ fill: rectFill, stroke: rectStroke, 'stroke-width': rectStrokeWidth })
                            .add(g);

                        // draw the quarter label (category label) centered in the cell
                        var cat = String(categories[i] || '');
                        var centerX = Math.round(left + cellW / 2);
                        // place quarter label just below the rect (adjust Y as required)
                        var qLabelY = rectY + rectHeight - 4; // slightly inside the rect bottom; tweak if you want below it
                        chart.renderer.text(cat, centerX, qLabelY)
                            .attr({ align: 'center' })
                            .css({ color: '#222', fontSize: '10px' })
                            .add(g);

                        // extract year (first 4-digit group) from category text
                        var yearMatch = cat.match(/\b(19|20)\d{2}\b/);
                        var yearText = yearMatch ? yearMatch[0] : '';

                        // draw year centered under the rect only when the year changes (prevents duplicates)
                        if (yearText && yearText !== prevYear) {
                            var yearY = rectY + rectHeight + yearGap + (yearFontSize / 2);
                            chart.renderer.text(String(yearText), centerX, yearY)
                                .attr({ align: 'center' })
                                .css({ color: '#222', fontSize: yearFontSize + 'px' })
                                .add(g);

                            prevYear = yearText;
                        }

                        // advance current x by original base cell width (use consistent stepping)
                        // Use the original cellW that was assigned before last-cell stretch to keep loop predictable
                        curX += baseW + (i < remainder ? 1 : 0);
                    } catch (inner) {
                        console.warn('drawQuarterBoxes: draw group failed', inner);
                    }
                }

                // defensive: if rounding left a tiny leftover on the right, we already stretched last cell,
                // but ensure group doesn't exceed chart bounds (optional).
            } catch (e) {
                console.warn('drawQuarterBoxes error', e);
            }
        }

        function drawQuarterAndYearBoxes(chart) {
            try {
                // remove previous group
                if (chart._qyBoxGroup && chart._qyBoxGroup.destroy) {
                    chart._qyBoxGroup.destroy();
                    chart._qyBoxGroup = null;
                }
                var group = chart.renderer.g('quarterYearBoxes').attr({ zIndex: 120 }).add();
                chart._qyBoxGroup = group;

                var xAxis = chart.xAxis && chart.xAxis[0];
                if (!xAxis) return;

                var plotLeft = chart.plotLeft || 0;
                var plotTop = chart.plotTop || 0;
                var plotWidth = chart.plotWidth || Math.max(0, (chart.chartWidth || 0) - plotLeft);
                var plotHeight = chart.plotHeight || Math.max(0, (chart.chartHeight || 0) - plotTop);

                var categories = xAxis.categories || [];
                var totalCats = categories.length;
                if (!totalCats) return;

                // equal width per category
                var catW = plotWidth / totalCats;

                // visuals / tuning
                var padSide = 4;              // horizontal extra padding for each quarter box
                var qLabelFont = 12;
                var yearFont = 12;
                var qBoxHeight = 28;          // height for quarter box
                var gapBetweenPlotAndQ = 12;  // **gap between bottom of plot and top of quarter boxes**
                var gapBetweenRows = 8;       // gap between Q-box bottom and year-box top

                var qStroke = '#444';
                var qStrokeW = 1;
                var qFill = 'rgba(255,255,255,0)'; // transparent - change if want background
                var qRadius = 0;

                // POSITION: put quarter boxes *below* the plot area
                var qRectTop = Math.round(plotTop + plotHeight + gapBetweenPlotAndQ); // moved *below* the bars
                var qRectHeight = qBoxHeight;

                // YEAR row below quarter row
                var yearRectHeight = 40;
                var yearRectTop = qRectTop + qRectHeight + gapBetweenRows;

                // Draw quarter boxes (top row): only quarter label centered (no counts)
                for (var idx = 0; idx < totalCats; idx++) {
                    try {
                        var left = Math.round(plotLeft + idx * catW) ;
                        var width = Math.max(8, Math.round(catW + padSide * 2));

                        // clamp to plot area
                        if (left < plotLeft) left = plotLeft;
                        if (left + width > plotLeft + plotWidth) width = Math.round(Math.max(8, plotLeft + plotWidth - left+9));

                        // rectangle for quarter
                        chart.renderer.rect(left, qRectTop-11, width-8, qRectHeight, 0)
                            .attr({ fill: qFill, stroke: qStroke, 'stroke-width': qStrokeW })
                            .add(group);

                        // quarter label centered
                        var cat = String(categories[idx] || '');
                        var q = (cat.match(/\b(Q[1-4])\b/i) || [null, ''])[1];
                        if (!q) q = (cat.split(/\s+/)[0] || '');

                        // center point of the box
                        var centerX = Math.round(left + width / 2);
                        var centerY = Math.round(qRectTop + qBoxHeight / 2)-11;

                        // create the SVG text, then rotate it -90° about the center
                        var txt = chart.renderer.text(escapeHtml(q.toUpperCase()), centerX, centerY)
                            .attr({ align: 'center' })
                            .css({ fontSize: qLabelFont + 'px', fontWeight: '600', color: '#111', lineHeight: '1' })
                            .add(group);

                        // rotate about the center (Highcharts rotation uses the element's transform, set rotation then adjust attributes if needed)
                        try {
                            // rotation pivot: set the transform attribute so rotation occurs around the center point
                            // Use SVG transform to ensure consistent rotation on export
                            if (txt && txt.element) {
                                var el = txt.element;
                                // move text anchor to middle horizontally (dominant-baseline for vertical centering)
                                el.setAttribute('text-anchor', 'middle');
                                // rotate -90 degrees about the center point
                                el.setAttribute('transform', 'rotate(-90 ' + centerX + ' ' + centerY + ')');
                                // optional: adjust y slightly because rotated baseline may shift; tweak offset if needed:
                                // el.setAttribute('transform', 'rotate(-90 ' + centerX + ' ' + (centerY) + ') translate(0,0)');
                            } else {
                                // fallback: use Highcharts attr rotation if element isn't present yet
                                txt.attr({ rotation: -90 });
                            }
                        } catch (e) {
                            // fallback to attr rotation
                            try { txt.attr({ rotation: -90 }); } catch (e2) {}
                        }

                    } catch (inner) { console.warn('quarter draw error', inner); }
                }

                // Bottom row: year boxes only (one per year group). Map years -> contiguous indices
                var yearMap = {};
                for (var i = 0; i < totalCats; i++) {
                    var catText = String(categories[i] || '');
                    var m = catText.match(/\b(19|20)\d{2}\b/);
                    var year = m ? m[0] : 'Unknown';
                    if (!yearMap[year]) yearMap[year] = { indices: [] };
                    yearMap[year].indices.push(i);
                }
                // order years by first index
                var yearsOrdered = Object.keys(yearMap).sort(function(a,b){
                    return Math.min.apply(null, yearMap[a].indices) - Math.min.apply(null, yearMap[b].indices);
                });

                yearsOrdered.forEach(function(yr) {
                    try {
                        var info = yearMap[yr];
                        if (!info || !info.indices || !info.indices.length) return;
                        var firstIndex = info.indices[0];
                        var lastIndex = info.indices[info.indices.length - 1];

                        // equal-width approach
                        var left = Math.round(plotLeft + firstIndex * catW);
                        var right = Math.round(plotLeft + (lastIndex + 1) * catW) + padSide+5;
                        left = Math.max(plotLeft, left);
                        right = Math.min(plotLeft + plotWidth, right);
                        var width = Math.max(8, Math.round(right - left));

                        if (yr === yearsOrdered[yearsOrdered.length - 1]) {
                            width += 9;   // <-- add 9 pixels
                        }

                        // draw year rect
                        chart.renderer.rect(left, yearRectTop-20, width-8, yearRectHeight, qRadius)
                            .attr({ fill: 'rgba(255,255,255,0)', stroke: qStroke, 'stroke-width': qStrokeW })
                            .add(group);

                        // draw centered year text only (no totals)
                        var centerX = Math.round(left + width / 2);
                        var yearY = yearRectTop + Math.round(yearRectHeight / 2) - 20;  // move up a bit
                        chart.renderer.text(String(yr), centerX, yearY)
                            .attr({ align: 'center' })
                            .css({ fontSize: yearFont + 'px', fontWeight: '600', color: '#111' })
                            .add(group);

                        // ----- age label (second line) -----
                        var parsedYear = Number(yr);
                        if (!isNaN(parsedYear) && parsedYear > 0) {
                            var computedAge = new Date().getFullYear() - parsedYear;
                            var ageText = '('+computedAge + ' y/o)';

                            var ageY = yearY + 16;   // ⬅️ 16px below the year text (adjust as needed)

                            chart.renderer.text(ageText, centerX, ageY)
                                .attr({ align: 'center' })
                                .css({ fontSize: (yearFont - 1) + 'px', color: '#333' })
                                .add(group);
                        }

                    } catch (inner2) { console.warn('year draw error', inner2); }
                });

                // ---- ensure chart has enough bottom spacing so boxes are not clipped on redraw/export ----
                // If chart spacing/margins are too small, boxes will be clipped. Suggest increasing spacingBottom.
                try {
                    var requiredBottomSpace = (yearRectTop + yearRectHeight + 12) - (plotTop + plotHeight);
                    // If current spacingBottom smaller, set a larger spacingBottom (non-destructive)
                    if (!chart.options) chart.options = {};
                    if (!chart.options.chart) chart.options.chart = {};
                    var curSpacingBottom = (chart.options.chart.spacingBottom || chart.spacing && chart.spacing[2]) || (chart.options.chart.marginBottom || 0);
                    if (!curSpacingBottom || curSpacingBottom < requiredBottomSpace) {
                        // set both options and call update so export respects it
                        chart.update({ chart: { spacingBottom: Math.max(requiredBottomSpace, 80), marginBottom: Math.max(requiredBottomSpace, 80) } }, false);
                    }
                } catch (s) { /* ignore spacing failures */ }

            } catch (e) {
                console.warn('drawQuarterYearBoxes error', e);
            }
        }


        function toInt(v) {
            if (v === null || v === undefined) return NaN;
            var n = parseInt(v, 10);
            return isNaN(n) ? NaN : n;
        }

        // Called by the Apply Age button or by your server script when loading data
        // jsonData should be a JS array of objects (not a string).
        function filterByAge(jsonData, applyToFunctionName) {
            var fromEl = document.getElementById('<%= txtAgeFrom.ClientID %>');
            var toEl = document.getElementById('<%= txtAgeTo.ClientID %>');
            var from = toInt(fromEl ? fromEl.value.trim() : '');
            var to = toInt(toEl ? toEl.value.trim() : '');

            // if no bounds provided, return original
            var useFrom = !isNaN(from);
            var useTo = !isNaN(to);

            // If neither provided, just return original
            if (!useFrom && !useTo) return jsonData;

            // Filter - expects each row to have an 'Age' property (number/string)
            var filtered = jsonData.filter(function (row) {
                var age = toInt(row.Age);
                if (isNaN(age)) return false; // if Age missing/invalid, exclude (change if desired)
                if (useFrom && age < from) return false;
                if (useTo && age > to) return false;
                return true;
            });

            return filtered;
        }

        // This function is wired to the Apply Age button. It assumes the last-loaded dataset
        // is available in window.__lastLoadedData and that the rendering function is loadDataFromServer.
        function applyAgeFilterClientSide() {
            try {
                // you must ensure your page stores the last JSON payload in window.__lastLoadedData
                var jsonData = window.__lastLoadedData;
                if (!jsonData) {
                    alert('No data loaded to filter.');
                    return;
                }
                var filtered = filterByAge(jsonData);
                // If your rendering function signature is loadDataFromServer(data, true)
                if (typeof loadDataFromServer === 'function') {
                    loadDataFromServer(filtered, true);
                } else if (typeof loadDataFromServerQuarter === 'function') {
                    // if you use loadDataFromServerQuarter for quarter page
                    loadDataFromServerQuarter(filtered);
                } else {
                    console.warn('No known client render function found.');
                }
            } catch (e) {
                console.error(e);
            }
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

                .age-filter { display:flex; align-items:center; gap:8px; font-size:13px; }
  .age-filter .label { min-width:56px; text-align:right; padding-right:6px; color:#222; }
  .age-filter .age-input {
    width:60px;
    padding:6px 8px;
    border-radius:6px;
    border:1px solid #c6d0d6;
    box-sizing:border-box;
    font-size:13px;
    text-align:center;
  }
  .age-filter .apply-btn {
    padding:6px 10px;
    border-radius:6px;
    border: none;
    background:#03507D;
    color:#fff;
    cursor:pointer;
    font-size:13px;
    box-shadow:0 1px 0 rgba(0,0,0,0.1);
  }
  .age-filter .apply-btn[disabled] { opacity:0.6; cursor:not-allowed; }
  .age-filter .small-note { font-size:11px; color:#666; margin-left:6px; }
  .age-filter .error { color:#b00020; font-size:12px; margin-left:8px; }
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
            showLoader();
            document.getElementById("btnShowReport").style.display = 'inline-block';
            document.getElementById("btnResetAllClient").style.display = 'inline-block';
            event.preventDefault();
                var selectedValues = {};

                var checkboxes = document.querySelectorAll(".filter-checkbox:checked");

                for (var i = 0; i < checkboxes.length; i++) {
                    var checkbox = checkboxes[i];
                    var column = checkbox.getAttribute("data-column");

                    //var label = checkbox.closest("label");

                    //var text = label ? label.textContent.trim() : ""; 
                    var value = checkbox.value || "";


                    //text = text.replace(/^\s+|\s+$/g, ""); 

                    if (!selectedValues[column]) {
                        selectedValues[column] = [];
                    }

                    selectedValues[column].push(value);
                }
                if (!selectedValues["Status"])
                {
                    selectedValues["Status"] = [];
                    selectedValues["Status"].push("Active");
                    selectedValues["Status"].push("Discharged");
                }
                $.ajax({
                    type: "POST",
                    url: "../Report/GetFilteredReport",
                    data: JSON.stringify(selectedValues),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        //alert(JSON.stringify(response));
                        hideLoader();
                        //var data = JSON.parse(response);
                        loadDataFromServer(response);
                    },
                    error: function (xhr, status, error) {
                        hideLoader();
                        try {
                            var err = JSON.parse(xhr.responseText);
                            alert("Server error: " + (err.error || "Unknown") + "\n" + (err.stack || ""));
                        } catch (e) {
                            alert("Unexpected error: " + xhr.responseText);
                        }
                    }
                });
        }

        function getSelectedValuesAndSendVendor() {
            showLoader();
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
                            hideLoader();
                            console.error("Error parsing JSON:", e);
                        }
                    } else {
                        hideLoader();
                        console.error("Empty response received.");
                    }
                }
            };

            xhr.send(JSON.stringify({ selectedValues: selectedValues }));
        }

        function applyAgeAndPostback() {
            var errEl = document.getElementById('ageFilterError');
            errEl.style.display = 'none';
            errEl.textContent = '';

            var fromEl = document.getElementById('txtAgeFromClient');
            var toEl = document.getElementById('txtAgeToClient');
            var fromVal = (fromEl && fromEl.value.trim() !== '') ? parseInt(fromEl.value.trim(), 10) : null;
            var toVal = (toEl && toEl.value.trim() !== '') ? parseInt(toEl.value.trim(), 10) : null;

            // Basic client validation
            if (fromVal !== null && (isNaN(fromVal) || fromVal < 0 || fromVal > 150)) {
                errEl.style.display = 'inline';
                errEl.textContent = 'Please enter a valid minimum age (0–150).';
                fromEl.focus();
                return false;
            }
            if (toVal !== null && (isNaN(toVal) || toVal < 0 || toVal > 150)) {
                errEl.style.display = 'inline';
                errEl.textContent = 'Please enter a valid maximum age (0–150).';
                toEl.focus();
                return false;
            }
            if (fromVal !== null && toVal !== null && fromVal > toVal) {
                errEl.style.display = 'inline';
                errEl.textContent = 'Minimum age cannot be greater than maximum age.';
                fromEl.focus();
                return false;
            }

            // All good — copy to server hidden fields
            var hfFrom = document.getElementById('<%= hfAgeFrom.ClientID %>');
            var hfTo = document.getElementById('<%= hfAgeTo.ClientID %>');
            if (hfFrom) hfFrom.value = (fromVal !== null) ? String(fromVal) : '';
            if (hfTo)   hfTo.value   = (toVal !== null)   ? String(toVal)   : '';

            // optional: disable button briefly to avoid double clicks
            var btn = document.getElementById('btnApplyAge');
            if (btn) { btn.disabled = true; setTimeout(function(){ btn.disabled = false; }, 2000); }

            // Trigger the hidden server button postback (calls your existing server handler)
            __doPostBack('<%= btnApplyAgeServer.UniqueID %>', '');
                    return false;
                }
                (function(){
                    var f = document.getElementById('txtAgeFromClient'), t = document.getElementById('txtAgeToClient');
                    function keyHandler(e){ if (!e) e = window.event; if (e.key === 'Enter' || e.keyCode === 13) { applyAgeAndPostback(); return false; } }
                    if (f) f.addEventListener('keydown', keyHandler);
                    if (t) t.addEventListener('keydown', keyHandler);
                })();
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
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
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
                        <asp:Button ID="btnallClient" CssClass="leftMenu" runat="server" Text="All Clients Info" ToolTip="All Clients Info" OnClientClick="return handleClientClick();"    OnClick="btnallClient_Click"></asp:Button>

                        <asp:Button ID="btnClienContact" CssClass="leftMenu" runat="server" Text="Emergency/Home Contact" ToolTip="Emergency/Home Contact" OnClientClick="return handleClientClick();" OnClick="btnClienContact_Click"></asp:Button>

                        <%--                                    <asp:Button ID="btnClientContactRes" CssClass="leftMenu" runat="server" Text="Emergency/Home Contact – Residence Only" ToolTip="Emergency/Home Contact – Residence Only"   ></asp:Button>--%>

                        <asp:Button ID="btnPgmRoster" CssClass="leftMenu" runat="server" Text="Program Roster" ToolTip="Program Roster" OnClientClick="return handleClientClick();" OnClick="btnPgmRoster_Click"></asp:Button>

                        <asp:Button ID="btnVendor" runat="server" CssClass="leftMenu" Text="Client/Contact/Vendor" ToolTip="Client/Contact/Vendor" OnClientClick="return handleClientClick();" OnClick="btnVendor_Click"></asp:Button>

                        <%--<asp:Button ID="btnVenderDischarged" runat="server" CssClass="leftMenu" Text="Client/Contact/Vendor – Discharged" ToolTip="Client/Contact/Vendor – Discharged"   ></asp:Button>--%>

                        <asp:Button ID="btnBirthdate" runat="server" CssClass="leftMenu" Text="All Clients by Birthdate Quarter" ToolTip="All Clients by Birthdate Quarter" OnClick="btnBirthdate_Click"></asp:Button>
                        <asp:Button ID="btnPlacementPlanning" runat="server" CssClass="leftMenu" Text="Placement Planning" ToolTip="Placement Planning Chart" OnClientClick="return handleClientClick1();" OnClick="btnPlacementPlanning_Click"></asp:Button>
                        <asp:Button ID="btnResRoster" runat="server" CssClass="leftMenu" Text=" Residential Roster Report" ToolTip=" Residential Roster Reports" OnClientClick="return handleClientClick();" OnClick="btnResRoster_Click"></asp:Button>
                        <asp:Button ID="btnAllFunder" runat="server" CssClass="leftMenu" Text="All Clients by Funder" ToolTip="All Clients by Funder" OnClientClick="return handleClientClick();" OnClick="btnAllFunder_Click"></asp:Button>
                        <asp:Button ID="btnAllPlacement" runat="server" CssClass="leftMenu" Text="All Clients by Placement" ToolTip="All Clients by placement" OnClientClick="return handleClientClick();" OnClick="btnAllPlacement_Click"></asp:Button>
                        <asp:Button ID="btnAllBirthdate" runat="server" CssClass="leftMenu" Text="All Clients by Birthdate" ToolTip="All Clients by Birthdate" OnClientClick="return handleClientClick();" OnClick="btnAllBirthdate_Click"></asp:Button>
                        <asp:Button ID="btnAllAdmissionDate" runat="server" CssClass="leftMenu" Text="All Clients by Admission date" ToolTip="All Clients by Admission date" OnClientClick="return handleClientClick();" OnClick="btnAllAdmissionDate_Click"></asp:Button>
                        <asp:Button ID="btnAllDischargedate" runat="server" CssClass="leftMenu" Text="All Clients by Discharge date" ToolTip="All Clients by Discharge date" OnClientClick="return handleClientClick();" OnClick="btnAllDischargedate_Click"></asp:Button>
                        <asp:Button ID="btnStatistical" runat="server" CssClass="leftMenu" Text="Statistical Report" ToolTip="Statistical Report" OnClientClick="return handleClientClick();" OnClick="btnStatistical_Click"></asp:Button>
                        <asp:Button ID="btnFundChange" runat="server" CssClass="leftMenu" Text="Funding Changes" ToolTip="Funding Changes" OnClick="btnFundChange_Click" OnClientClick="resetVal();"></asp:Button>
                        <asp:Button ID="btnPlcChange" runat="server" CssClass="leftMenu" Text="Placement Changes" ToolTip="Placement Changes" OnClick="btnPlcChange_Click" OnClientClick="resetVal();"></asp:Button>
                        <asp:Button ID="btnGuardianChanges" runat="server" CssClass="leftMenu" Text="Guardianship Changes" ToolTip="Guardianship Changes" OnClick="btnGuardianChanges_Click" OnClientClick="resetVal();"></asp:Button>
                        <asp:Button ID="btnContactChanges" runat="server" CssClass="leftMenu" Text="Contact Changes" ToolTip="Contact Changes" OnClick="btnContactChanges_Click" OnClientClick="resetVal();"></asp:Button>
                    </div>


                    <div class="middleContainer" style="width: 75%">

                        <div id="content" style="position: relative;">
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
                                                <asp:Button ID="btnquarter" runat="server" Text="Show Report" OnClientClick="return handleClientClick();" OnClick="btnquarter_Click" Width="120px" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:HiddenField ID="hfAgeFrom" runat="server" runat="server" />
                                <asp:HiddenField ID="hfAgeTo" runat="server" />
                                <asp:Button ID="btnApplyAgeServer" runat="server" OnClick="btnPlacementPlanning_Click" Style="display:none;" />
                                <div id="divPlacementPlanning" runat="server">
                                    <div id="placementExportBar" style="margin-bottom:6px; padding:4px 0;">
                                        <div class="age-filter" role="group" aria-label="Age filter" id="agefilter" runat="server" style="display:none;">
                                          <label class="label" for="txtAgeFromClient">Age From</label>

                                          <!-- plain inputs (easy to control) -->
                                          <input id="txtAgeFromClient" name="txtAgeFromClient" class="age-input" 
                                                 type="text" inputmode="numeric" pattern="[0-9]*" maxlength="3"
                                                 placeholder="e.g. 10" title="Enter minimum age (years)"
                                                 oninput="this.value=this.value.replace(/[^0-9]/g,'');" />

                                          <span style="font-weight:600;">—</span>

                                          <label class="label" for="txtAgeToClient" style="min-width:15px; text-align:left;">To</label>

                                          <input id="txtAgeToClient" name="txtAgeToClient" class="age-input"
                                                 type="text" inputmode="numeric" pattern="[0-9]*" maxlength="3"
                                                 placeholder="e.g. 18" title="Enter maximum age (years)"
                                                 oninput="this.value=this.value.replace(/[^0-9]/g,'');" />

                                          <button id="btnApplyAge" type="button" class="apply-btn"
                                                  onclick="return applyAgeAndPostback();">
                                            Apply Age
                                            </button>

                                          <span id="ageFilterError" class="error" aria-live="polite" style="display:none;"></span>
                                          <%--<span class="small-note">years</span>--%>
                                        </div>
                                        <div style="float:right;">
                                           <input id="exportChartBtn"
       type="button"
       value="Export Chart"
       onclick="exportChartToPDF();"
       style="padding:6px 10px; background:#03507D; border:1px solid #82a783; 
              border-radius:4px; cursor:pointer; color:white; font-size:13px; display:none" />
                                        </div>
                                        <div style="clear:both;"></div>
                                    </div>
                                  <div id="placementChartContainer" style="width:100%; height:1000px; overflow: auto;"></div>
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
                                                <asp:Button ID="btnShowFunder" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClientClick ="return handleClientClick();" OnClick="btnShowFunder_Click" />
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
                                                <asp:Button ID="btnShowBirthdate" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClientClick="return handleClientClick();" OnClick="btnShowBirthdate_Click" />
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
                                                <asp:Button ID="btnShowAdmissionDate" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClientClick="return handleClientClick();" OnClick="btnShowAdmissionDate_Click" /></td>
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
                                          <%--  <td>
                                                <asp:Button ID="btnShowDischarge" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClientClick="return handleClientClick();" OnClick="btnShowDischarge_Click" /></td>--%>
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
                                    <asp:Button ID="btnShowReport" CssClass="button-style" runat="server" Visible="false" Text="Show Report" OnClientClick="getSelectedValuesAndSend();return false;" />
                                    <asp:Button ID="btnResetAllClient" CssClass="button-style" runat="server" Visible="false" Text="Reset" OnClientClick="return handleClientClick();" OnClick="btnallClient_Click" />
                                    <asp:Button ID="btnShowReportVendor" CssClass="button-style" runat="server" Visible="false" Text="Show Report" OnClientClick="getSelectedValuesAndSendVendor();" />
                                    <asp:Button ID="btnResetVendor" CssClass="button-style" runat="server" Visible="false" Text="Reset"  OnClientClick="return handleClientClick();" OnClick="btnVendor_Click" />
                                    <%--<asp:Button ID="btnOldReport" CssClass="button-style" runat="server" Visible="false" Text="Old Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnOldReport_Click" />--%>

                                </div>
                                <asp:Label ID="showlab" runat="server" Text="" style=" font-size:14px; color:black;"></asp:Label>
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
                            <div id="loaderOverlay" class="loader-overlay">
                                <div class="loader-text">Loading ...</div>
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

