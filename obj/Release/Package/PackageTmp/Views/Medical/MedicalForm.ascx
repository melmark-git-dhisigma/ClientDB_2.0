<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.MedicalModel>" %>
 <style type="text/css"> 
 .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-year {
        width: 49% !important;
    }
</style>
<script type="text/javascript">
    $("#PhyExmDate").datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
       // maxDate: new Date,


        /* fix buggy IE focus functionality */
        fixFocusIE: true
    });

</script> 

<%using (@Ajax.BeginForm("SavemedicalData", "Medical", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content"}, new { id = "MedicalForm" }))
  { %>
<div style="width: 100%">
    <div id="partialMainArea">
        <% if (TempData["notice"] != null)
           { %>
        <p style="width: 100%; color: red; font-size: 14px; font-weight: bold"><%= Html.Encode(TempData["notice"]) %></p>
        <% } %>

         <table style="width: 100%;">

                    <tr>
                        <td class="auto-style2" colspan="7">
                            <h4>Medical Information</h4>
                        </td>
                    </tr>
            </table>
         <table>
             <tr>
                 <td>
                      <label class="lblSpan">Current Medications</label><span class="nospan-align">*</span><br />
                        <%=Html.TextBoxFor(m => m.CurrentMedications, Model.CurrentMedications, new {  @class="[] newClass",maxlength=500 })%></td>                 
             </tr>
             <tr>
                <td colspan="1">
                    <label class="lblSpan">Last Date Of Physical Exam</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateOfLastPhysicalExam, Model.DateOfLastPhysicalExam, new {  @class="datepicker", ID = "PhyExmDate"  })%></td>
            </tr> 
                <tr>
                    <td colspan="2">
                        <label class="lblSpan">Allergies</label><span class="nospan-align">*</span><br />
                        <%=Html.TextBoxFor(m => m.Allergie, Model.Allergie, new {  @class="[] newClass",maxlength=500 })%></td>
                 </tr>
           </table>
            
         

        <table class="tblStyle" style="width: 100%;">


            <tr>


                <td>
             <%-- <p style="width: 100%; color: red; font-size: 16px; font-weight: bold;text-align:center">Click <a href="#">Here</a> to Login to EMR</p> --%>
                </td>

              
            </tr>
              <tr>
                <td colspan="2">
                    <label class="lblSpan">Seizures</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Seizures, Model.Seizures, new {  @class="[] newClass",maxlength=500 })%></td>
            </tr>
                <tr>
                <td colspan="2">
                    <label class="lblSpan">Diet</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Diet, Model.Diet, new {  @class="[] newClass",maxlength=500 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Other</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Other, Model.Other, new {  @class="[] newClass",maxlength=500 })%></td>
            </tr>


                
                </Table>
        <table style="width: 100%;">
            <tr>
                <td class="auto-style2" colspan="4">
                    <h4>Diagnosis</h4>
                </td>
            </tr>
        </table>
       <table style="width: 100%;">
           <%-- first --%>
           <% if( Model.Diagnosis.Count>0)
            { %>
                <%for (int i = 0; i < Model.Diagnosis.Count && i < 3; i++)
                  { %>
                    <tr>
                         <%if (Model.Diagnosis.Count > i)
                         {
                             if (Model.Diagnosis[i] != null)
                             { %>
                                 <td class="auto-style2" colspan="2"><%=Html.TextBoxFor(m => m.Diagnosis[i].Name, Model.Diagnosis[i].Name, new {  @class=" newClass",maxlength=50 })%></td>
               
                            <% }
                         }
                        else
                        {%>
                            <td class="auto-style2" colspan="2">
                               <input type="text" value="" name="Diagnosis[<%=i %>].Name" /></td>
               
                        <%
                         } %>
                     </tr>
                 <%} %>
           <%} %>
           
            <%else
                        {%>
           <tr>
                            <td class="auto-style2" colspan="2">
                               <input type="text" value="" name="Diagnosis[0].Name" /></td> 
            </tr>    
                        <%
                         } %> 
          
        </table>    
          
           
         <table>
            <tr>

                <td>
                    <% if (ViewBag.permission == "true")
                       {
                           if (Model.ID > 0)
                           {
                               if (Session["PlacementStat"].ToString() != "I")
                               {
                    %>
                    <input id="btnUpdate" type="submit" value="Update" style="margin-left: 5%" />
                    <%}
                           }
                           else
                           {%>
                    <input id="btnSubmit" type="submit" value="Save" style="margin-left: 5%" /><% }
                       }%>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>

    </div>

</div>
<%} %>

