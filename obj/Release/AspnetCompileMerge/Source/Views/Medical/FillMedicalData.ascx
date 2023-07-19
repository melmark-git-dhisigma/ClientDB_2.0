<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.MedicalModel>" %>
<link href="../../Documents/CSS/General.css" rel="stylesheet" />
<script src="../../Documents/JS/jquery-ui-1.11.2.js"></script>
<script src="../../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../../Documents/JS/jquery.validationEngine.js"></script>
<script type="text/javascript">
    // $('#imagePanel').load('/Contact/ImageUploadPanel');

    $(document).ready(function () {             
       // $("#calender").css({ left: -18 });
    });

   

</script>
<style type="text/css">
    #partialMainArea table td {
        border: 1px solid lightgrey;
        border-collapse: collapse;
        border-spacing: 0;
    }

    #partialMainArea table {
        border-collapse: collapse;
        border-spacing: 0;
    }
</style>
<div style="width: 100%">
    <div id="partialMainArea">
        <table style="width: 100%; border: medium none; margin-bottom: 0px;" class="gridStyle">
            <tr>
                <td colspan="5" style="border: none">
                    <input class="" type="button" value="Edit Details" id="Medi-<%=Model.ID%>" onclick="EditMediData(this.id);" style="float: right" /></td>
            </tr>
            <tr>
                <td rowspan="2" style="background-color: #333333; color: #FFFFFF; width: 1%"><strong><%=Model.Physician %>&nbsp;
                    Physician</strong></td>
                <td style="width: 1%;">Full Name</td>
                <td><%=Model.LastNames %>&nbsp;&nbsp;<%=Model.FirstName %></td>
                <td>Office Phone</td>
                <td><%=Model.OfficePhone %></td>
            </tr>
            <tr>
                <%--<%=Model.Addressline1 %>,&nbsp;<%=Model.Street %>,&nbsp;--%>
                <td>Address</td>
                <td colspan="3"><%=Model.City %>,&nbsp;<%=Model.State %>,&nbsp;<%=Model.Country %></td>
            </tr>
        </table>
        <table class="gridStyle" style="margin: 0 !Important">
            <tr style="height: 1px;">
                <td colspan="2" style="border: none !Important; border-collapse: collapse;"></td>
            </tr>
            <tr>

                <td style="width:1%;">Date Of Last Physical Exam</td>
                <td><%=Model.DateOfLastPhysicalExam %></td>
            </tr>
            <tr>
                <td style="width: 1%;">Medical Conditions / Diagnosis</td>
                <td><%=Model.MedicalConditionsDiagnosis %></td>
            </tr>
            <tr>
                <td style="width: 1%;">Allergies</td>
                <td><%=Model.Allergies %></td>
            </tr>
            <tr>
                <td style="width: 1%;">Current Medications</td>
                <td><%=Model.CurrentMedications %></td>
            </tr>
            <tr>
                <td style="width: 1%;">Self Preservation Ability</td>
                <td><%=Model.SelfPreservationAbility %></td>
            </tr>
            <tr>
                <td style="width: 1%;">Significant Behavior Characteristics</td>
                <td><%=Model.SignificantBehaviorCharacteristics %></td>
            </tr>
            <tr style="background-color: white;">
                <td colspan="2"></td>

            </tr>
            <tr>
                <td colspan="2"><b>Relevent Capabilities,
                    Limitations And Preferences</b></td>

            </tr>
            <tr>
                <td style="width: 1%;">Capabilities</td>
                <td><%=Model.Capabilities %></td>
            </tr>
            <tr>
                <td style="width: 1%;">Limitations </td>
                <td><%=Model.Limitations %></td>
            </tr>
            <tr>
                <td style="width: 1%;">Preferences</td>
                <td><%=Model.Preferances %></td>
            </tr>

        </table>
    </div>
    <%--<div id="imagePanel" style="border: 1px solid #CCCCCC; float: left; margin-left: 5px; margin-right: 20px; margin-top: 20px; width: 18%;">
    </div>--%>
</div>


