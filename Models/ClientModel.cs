using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using ClientDB.AppFunctions;
using ClientDB.DbModel;
using System.Data.Objects.SqlClient;
namespace ClientDB.Models
{
    public class ClientModel
    {
        public virtual int Id { get; set; }
        public virtual int schoolId { get; set; }
        public virtual string Name { get; set; }
        public virtual string DateOfBirth { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Age { get; set; }
        public virtual string ImageUrl { get; set; }
        public bool sortOrder { get; set; }
        public static clsSession sess = null;
        public static BiWeeklyRCPNewEntities RPCobj;
        public static IList<ClientModel> fillCLients(ClientSearch objClientSearch, clsSession sess)
        {
            RPCobj = new BiWeeklyRCPNewEntities();
            IList<ClientModel> retunmodel = new List<ClientModel>();
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            string pageArg = objClientSearch.PagingArgument;
            const int pagesize = 16;
            string[] datasplit = pageArg.Split('*');
            string way = datasplit[1];
            int dec = 0;
            string flag = "";
            int page = Convert.ToInt32(datasplit[0]);
            string dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagessLocation"].ToString();
            Database objData = new Database();
            IList<StudentPersonal> result = new List<StudentPersonal>();
            IList<StudentPersonal> result2 = new List<StudentPersonal>();
            IList<StudentPersonal> result4 = new List<StudentPersonal>();     
            IList<StudentPersonal> result5 = new List<StudentPersonal>();
            int tempId = 0;
            
            
            

            if (way == "n")
            {
                page++;
                objClientSearch.PagingArgument = page.ToString();
                result = RPCobj.StudentPersonals.OrderBy(objStudentpersonal => objStudentpersonal.StudentPersonalId).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                    && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                if (objClientSearch.SortStatus == false)
                {
                    if (objClientSearch.SearchArgument == null)
                    {
                        result = RPCobj.StudentPersonals.Where(objStudentpersonal => objClientSearch.SearchArgument == null ||
                            objStudentpersonal.StudentType == "Client" &&
                            objStudentpersonal.SchoolId == sess.SchoolId &&
                            (objStudentpersonal.LastName.StartsWith(objClientSearch.SearchArgument) ||
                            objStudentpersonal.FirstName.StartsWith(objClientSearch.SearchArgument))).OrderBy(x => x.LastName).ToList();
                    }
                    //mc
                    else
                    {
                        if (objClientSearch.SearchBy == true)
                        {
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.StudentType == "Client" &&
                                objStudentpersonal.SchoolId == sess.SchoolId &&
                                (objStudentpersonal.LastName.StartsWith(objClientSearch.SearchArgument) ||
                                objStudentpersonal.FirstName.StartsWith(objClientSearch.SearchArgument))).OrderBy(x => x.LastName).ToList();
                        }
                        else if (objClientSearch.SearchBy == false)
                        {
                            try
                            {
                                tempId = Int32.Parse(objClientSearch.SearchArgument);
                            }
                            catch
                            {
                            }
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.StudentType == "Client" &&
                                objStudentpersonal.SchoolId == sess.SchoolId &&
                                objStudentpersonal.ClientId == tempId).OrderBy(x => x.ClientId).ToList();
                        }


                    }
                }
                else
                {

                    if (objClientSearch.SearchArgument.Contains("$"))
                    {
                        string[] datasplit2 = objClientSearch.SearchArgument.Split('$');
                        string searchArg = datasplit2[0];
                        //string sortArg = datasplit2[1];
                        if (objClientSearch.SearchBy == true)
                        {
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.SchoolId == sess.SchoolId && (objStudentpersonal.LastName.StartsWith(searchArg) || objStudentpersonal.FirstName.StartsWith(searchArg))).OrderBy(x => x.LastName).ToList();
                            //result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.LastName.StartsWith(searchArg) || objStudentpersonal.FirstName.StartsWith(searchArg)).ToList();
                        }
                        else if (objClientSearch.SearchBy == false)
                        {
                            try
                            {
                                tempId = Int32.Parse(searchArg);
                            }
                            catch
                            {
                            }
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.SchoolId == sess.SchoolId && objStudentpersonal.ClientId == tempId).OrderBy(x => x.ClientId).ToList();
                        }
                    }


                }
                result2 = (from studpersonal in RPCobj.StudentPersonals
                          // join cp in RPCobj.ContactPersonals on studpersonal.StudentPersonalId equals cp.StudentPersonalId
                           where studpersonal.SchoolId == sess.SchoolId && studpersonal.StudentType == "Client" && studpersonal.ClientId > 0
                           select studpersonal).Distinct().ToList();
                result4 = (from studpersonal in RPCobj.StudentPersonals
                           join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                           where studpersonal.SchoolId == sess.SchoolId && (plc.EndDate == null || plc.EndDate >= DateTime.Today) && plc.Status == 1 && studpersonal.StudentType == "Client"
                           select studpersonal).Distinct().ToList();
                result5 = (from studpersonal in RPCobj.StudentPersonals
                           join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                           where studpersonal.SchoolId == sess.SchoolId && studpersonal.PlacementStatus == "D" && studpersonal.StudentType == "Client"
                           select studpersonal).Distinct().ToList();
                if (result4.Count > 0)
                {
                    for (int i = 0; i < result4.Count; i++)
                    {
                        result2.Remove(result4[i]);
                    }
                }
                if (objClientSearch.activeClient == 1)
                {
                    if (result2.Count > 0)
                    {
                        for (int i = 0; i < result2.Count; i++)
                        {
                            result.Remove(result2[i]);
                        }
                    }
                    if (result5.Count > 0)
                    {
                        for (int i = 0; i < result5.Count; i++)
                        {
                            result.Remove(result5[i]);
                        }
                    }
                }
                else if (objClientSearch.activeClient == 2)
                {
                    result4.Clear();
                    foreach (var item in result)
                        result4.Add(item);
                    if (result2.Count > 0)
                    {
                        for (int i = 0; i < result2.Count; i++)
                        {
                            result4.Remove(result2[i]);
                        }
                    }
                    if (result4.Count > 0)
                    {
                        for (int i = 0; i < result4.Count; i++)
                        {
                            result.Remove(result4[i]);
                        }
                    }
                    //if (result5.Count > 0)
                    //{
                    //    for (int i = 0; i < result5.Count; i++)
                    //    {
                    //        result.Add(result5[i]);
                    //    }
                    //}
                    if (result5.Count > 0)
                    {
                        for (int i = 0; i < result5.Count; i++)
                        {
                            result.Remove(result5[i]);
                        }
                    }
                }
                else if (objClientSearch.activeClient == 4)
                {
                    //result4.Clear();
                    //foreach (var item in result)
                    //    result4.Add(item);
                    //if (result2.Count > 0)
                    //{
                    //    for (int i = 0; i < result2.Count; i++)
                    //    {
                    //        result4.Remove(result2[i]);
                    //    }
                    //}
                    //if (result4.Count > 0)
                    //{
                    //    for (int i = 0; i < result4.Count; i++)
                    //    {
                    //        result.Remove(result4[i]);
                    //    }
                    //}
                    result4.Clear();
                    if (result5.Count > 0)
                    {
                        for (int i = 0; i < result5.Count; i++)
                        {
                            if (result.Contains(result5[i]))
                                result4.Add(result5[i]);
                        }
                        result = result4;
                    }
                }
                result = clientSearchList(objClientSearch, result);

                if (result.Count == pagesize)
                {
                    dec = 1;
                    flag = "<>";
                }
                else
                {
                    flag = "<";
                }
            }
            else if (way == "p")
            {
                page--;
                objClientSearch.PagingArgument = page.ToString();
                result = RPCobj.StudentPersonals.OrderBy(objStudentpersonal => objStudentpersonal.StudentPersonalId).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                    && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                if (objClientSearch.SortStatus == false)
                {
                    if (objClientSearch.SearchArgument == null)
                    {
                        result = RPCobj.StudentPersonals.Where(p => objClientSearch.SearchArgument == null || p.StudentType == "Client"
                            && p.SchoolId == sess.SchoolId && (p.LastName.StartsWith(objClientSearch.SearchArgument) || p.FirstName.StartsWith(objClientSearch.SearchArgument))).OrderBy(x => x.LastName).ToList();
                    }
                    else
                    {
                        if (objClientSearch.SearchBy == true)
                        {
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.StudentType == "Client" &&
                                objStudentpersonal.SchoolId == sess.SchoolId &&
                                (objStudentpersonal.LastName.StartsWith(objClientSearch.SearchArgument) ||
                                objStudentpersonal.FirstName.StartsWith(objClientSearch.SearchArgument))).OrderBy(x => x.LastName).ToList();
                        }
                        else if (objClientSearch.SearchBy == false)
                        {
                            try
                            {
                                tempId = Int32.Parse(objClientSearch.SearchArgument);
                            }
                            catch
                            {
                            }
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.StudentType == "Client" &&
                                objStudentpersonal.SchoolId == sess.SchoolId &&
                                objStudentpersonal.ClientId == tempId).OrderBy(x => x.ClientId).ToList();
                        }
                    }
                }
                else
                {

                    if (objClientSearch.SearchArgument.Contains("$"))
                    {
                        string[] datasplit2 = objClientSearch.SearchArgument.Split('$');
                        string searchArg = datasplit2[0];
                        //string sortArg = datasplit2[1];
                        if (objClientSearch.SearchBy == true)
                        {
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.SchoolId == sess.SchoolId && (objStudentpersonal.LastName.StartsWith(searchArg) || objStudentpersonal.FirstName.StartsWith(searchArg))).OrderBy(x => x.LastName).ToList();
                            //result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.LastName.StartsWith(searchArg) || objStudentpersonal.FirstName.StartsWith(searchArg)).ToList();
                        }
                        else if (objClientSearch.SearchBy == false)
                        {
                            try
                            {
                                tempId = Int32.Parse(searchArg);
                            }
                            catch
                            {
                            }
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.SchoolId == sess.SchoolId && objStudentpersonal.ClientId == tempId).OrderBy(x => x.ClientId).ToList();
                        }
                    }

                }
                result2 = (from studpersonal in RPCobj.StudentPersonals
                          // join cp in RPCobj.ContactPersonals on studpersonal.StudentPersonalId equals cp.StudentPersonalId
                           where studpersonal.SchoolId == sess.SchoolId && studpersonal.StudentType == "Client" && studpersonal.ClientId > 0
                           select studpersonal).Distinct().ToList();
                result4 = (from studpersonal in RPCobj.StudentPersonals
                           join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                           where studpersonal.SchoolId == sess.SchoolId && (plc.EndDate == null || plc.EndDate >= DateTime.Today) && plc.Status == 1 && studpersonal.StudentType == "Client"
                           select studpersonal).Distinct().ToList();
                result5 = (from studpersonal in RPCobj.StudentPersonals
                           join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                           where studpersonal.SchoolId == sess.SchoolId && studpersonal.PlacementStatus == "D" && studpersonal.StudentType == "Client"
                           select studpersonal).Distinct().ToList();
                if (result4.Count > 0)
                {
                    for (int i = 0; i < result4.Count; i++)
                    {
                        result2.Remove(result4[i]);
                    }
                }
                if (objClientSearch.activeClient == 1)
                {
                    if (result2.Count > 0)
                    {
                        for (int i = 0; i < result2.Count; i++)
                        {
                            result.Remove(result2[i]);
                        }
                    }
                    if (result5.Count > 0)
                    {
                        for (int i = 0; i < result5.Count; i++)
                        {
                            result.Remove(result5[i]);
                        }
                    }
                }
                else if (objClientSearch.activeClient == 2)
                {
                    result4.Clear();
                    foreach (var item in result)
                        result4.Add(item);
                    if (result2.Count > 0)
                    {
                        for (int i = 0; i < result2.Count; i++)
                        {
                            result4.Remove(result2[i]);
                        }
                    }
                    if (result4.Count > 0)
                    {
                        for (int i = 0; i < result4.Count; i++)
                        {
                            result.Remove(result4[i]);
                        }
                    }
                    //if (result5.Count > 0)
                    //{
                    //    for (int i = 0; i < result5.Count; i++)
                    //    {
                    //        result.Add(result5[i]);
                    //    }
                    //}
                    if (result5.Count > 0)
                    {
                        for (int i = 0; i < result5.Count; i++)
                        {
                            result.Remove(result5[i]);
                        }
                    }
                }
                else if (objClientSearch.activeClient == 4)
                {
                    //result4.Clear();
                    //foreach (var item in result)
                    //    result4.Add(item);
                    //if (result2.Count > 0)
                    //{
                    //    for (int i = 0; i < result2.Count; i++)
                    //    {
                    //        result4.Remove(result2[i]);
                    //    }
                    //}
                    //if (result4.Count > 0)
                    //{
                    //    for (int i = 0; i < result4.Count; i++)
                    //    {
                    //        result.Remove(result4[i]);
                    //    }
                    //}
                    result4.Clear();
                    if (result5.Count > 0)
                    {
                        for (int i = 0; i < result5.Count; i++)
                        {
                            if(result.Contains(result5[i]))
                            result4.Add(result5[i]);
                        }
                        result = result4;
                    }
                }
                result = clientSearchList(objClientSearch, result);

                flag = ">";

                if (result.Count == pagesize)
                {

                    dec = 1;
                    flag = "<>";
                    if (page == 0)
                    {
                        flag = ">";
                    }
                }
                else
                {

                    flag = ">";
                }
            }
            else
            {
                objClientSearch.PagingArgument = page.ToString();
                try
                {

                    result = RPCobj.StudentPersonals.OrderBy(objStudentpersonal => objStudentpersonal.StudentPersonalId).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                        && objStudentpersonal.SchoolId == sess.SchoolId).OrderBy(x => x.LastName).ToList();
                }
                catch
                {

                }
                if (objClientSearch.SortStatus == false)
                {
                    if (objClientSearch.SearchArgument != "*")
                    {
                        if (objClientSearch.SearchBy == true)
                        {
                            result = RPCobj.StudentPersonals.Where(p => objClientSearch.SearchArgument == null || p.StudentType == "Client"
                                && p.SchoolId == sess.SchoolId && (p.LastName.StartsWith(objClientSearch.SearchArgument) || p.FirstName.StartsWith(objClientSearch.SearchArgument))).OrderBy(x => x.LastName).ToList();
                        }
                        else if (objClientSearch.SearchBy == false)
                        {
                            try
                            {
                                tempId = Int32.Parse(objClientSearch.SearchArgument);
                            }
                            catch
                            {
                            }
                            result = RPCobj.StudentPersonals.Where(p => objClientSearch.SearchArgument == null || p.StudentType == "Client"
                                && p.SchoolId == sess.SchoolId && p.ClientId == tempId).OrderBy(x => x.ClientId).ToList();
                        }

                        result2 = (from studpersonal in RPCobj.StudentPersonals
                                   //join cp in RPCobj.ContactPersonals on studpersonal.StudentPersonalId equals cp.StudentPersonalId
                                   where studpersonal.SchoolId == sess.SchoolId && studpersonal.StudentType == "Client" && studpersonal.ClientId > 0
                                   select studpersonal).Distinct().ToList();
                        result4 = (from studpersonal in RPCobj.StudentPersonals
                                   join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                                   where studpersonal.SchoolId == sess.SchoolId && (plc.EndDate == null || plc.EndDate >= DateTime.Today) && plc.Status == 1 && studpersonal.StudentType == "Client"
                                   select studpersonal).Distinct().ToList();
                        result5 = (from studpersonal in RPCobj.StudentPersonals
                                   join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                                   where studpersonal.SchoolId == sess.SchoolId && studpersonal.PlacementStatus == "D" && studpersonal.StudentType == "Client"
                                   select studpersonal).Distinct().ToList();
                        if (result4.Count > 0)
                        {
                            for (int i = 0; i < result4.Count; i++)
                            {
                                result2.Remove(result4[i]);

                            }
                        }
                        if (objClientSearch.activeClient == 1)
                        {    
                            if (result2.Count > 0)
                            {
                                for (int i = 0; i < result2.Count; i++)
                                {
                                    result.Remove(result2[i]);

                                }
                            }
                            if (result5.Count > 0)
                            {
                                for (int i = 0; i < result5.Count; i++)
                                {
                                    result.Remove(result5[i]);
                                }
                            }

                        }
                        else if (objClientSearch.activeClient == 2)
                        {   
                            result4.Clear();
                            foreach (var item in result)
                                result4.Add(item);

                            if (result2.Count > 0)
                            {
                                for (int i = 0; i < result2.Count; i++)
                                {
                                    result4.Remove(result2[i]);

                                }
                            }
                            if (result4.Count > 0)
                            {
                                for (int i = 0; i < result4.Count; i++)
                                {
                                    result.Remove(result4[i]);

                                }
                            }
                            //if (result5.Count > 0)
                            //{
                            //    for (int i = 0; i < result5.Count; i++)
                            //    {
                            //        result.Add(result5[i]);
                            //    }
                            //}
                            if (result5.Count > 0)
                            {
                                for (int i = 0; i < result5.Count; i++)
                                {
                                    result.Remove(result5[i]);
                                }
                            }

                        }
                        else if (objClientSearch.activeClient == 4)
                        {
                            //result4.Clear();
                            //foreach (var item in result)
                            //    result4.Add(item);
                            //if (result2.Count > 0)
                            //{
                            //    for (int i = 0; i < result2.Count; i++)
                            //    {
                            //        result4.Remove(result2[i]);
                            //    }
                            //}
                            //if (result4.Count > 0)
                            //{
                            //    for (int i = 0; i < result4.Count; i++)
                            //    {
                            //        result.Remove(result4[i]);
                            //    }
                            //}
                            result4.Clear();
                            if (result5.Count > 0)
                            {
                                for (int i = 0; i < result5.Count; i++)
                                {
                                    if (result.Contains(result5[i]))
                                        result4.Add(result5[i]);
                                }
                                result = result4;
                            }

                        }
                    }
                    else
                    {
                        //if (objClientSearch.SearchBy == true)
                        //{
                        //    result = RPCobj.StudentPersonals.Where(p => objClientSearch.SearchArgument == null || p.StudentType == "Client"
                        //        && p.SchoolId == sess.SchoolId && (p.LastName.StartsWith(objClientSearch.SearchArgument) || p.FirstName.StartsWith(objClientSearch.SearchArgument))).OrderBy(x => x.LastName).ToList();
                        //}
                        //else 
                        if (objClientSearch.SearchBy == false)
                        {
                            try
                            {
                                tempId = Int32.Parse(objClientSearch.SearchArgument);
                            }
                            catch
                            {
                            }
                            result = RPCobj.StudentPersonals.Where(p => objClientSearch.SearchArgument == null || p.StudentType == "Client"
                                && p.SchoolId == sess.SchoolId && p.ClientId == tempId).OrderBy(x => x.ClientId).ToList();
                        }

                        result2 = (from studpersonal in RPCobj.StudentPersonals
                                  // join cp in RPCobj.ContactPersonals on studpersonal.StudentPersonalId equals cp.StudentPersonalId
                                   where studpersonal.SchoolId == sess.SchoolId && studpersonal.StudentType == "Client" && studpersonal.ClientId > 0
                                   select studpersonal).Distinct().ToList();
                        result4 = (from studpersonal in RPCobj.StudentPersonals
                                   join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                                   where studpersonal.SchoolId == sess.SchoolId && (plc.EndDate == null || plc.EndDate >= DateTime.Today) && plc.Status == 1 && studpersonal.StudentType == "Client"
                                   select studpersonal).Distinct().ToList();
                        result5 = (from studpersonal in RPCobj.StudentPersonals
                                   join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                                   where studpersonal.SchoolId == sess.SchoolId && studpersonal.PlacementStatus == "D" && studpersonal.StudentType == "Client"
                                   select studpersonal).Distinct().ToList();
                        if (result4.Count > 0)
                        {
                            for (int i = 0; i < result4.Count; i++)
                            {
                                result2.Remove(result4[i]);

                            }
                        }
                        if (objClientSearch.activeClient == 1)
                        {
                            if (result2.Count > 0)
                            {
                                for (int i = 0; i < result2.Count; i++)
                                {
                                    result.Remove(result2[i]);

                                }
                            }
                            if (result5.Count > 0)
                            {
                                for (int i = 0; i < result5.Count; i++)
                                {
                                    result.Remove(result5[i]);
                                }
                            }
                        }
                        else if (objClientSearch.activeClient == 2)
                        {
                            result4.Clear();
                            foreach (var item in result)
                                result4.Add(item);

                            if (result2.Count > 0)
                            {
                                for (int i = 0; i < result2.Count; i++)
                                {
                                    result4.Remove(result2[i]);

                                }
                            }
                            if (result4.Count > 0)
                            {
                                for (int i = 0; i < result4.Count; i++)
                                {
                                    result.Remove(result4[i]);

                                }
                            }
                            //if (result5.Count > 0)
                            //{
                            //    for (int i = 0; i < result5.Count; i++)
                            //    {
                            //        result.Add(result5[i]);
                            //    }
                            //}
                            if (result5.Count > 0)
                            {
                                for (int i = 0; i < result5.Count; i++)
                                {
                                    result.Remove(result5[i]);
                                }
                            }

                        }
                        else if (objClientSearch.activeClient == 4)
                        {
                            //result4.Clear();
                            //foreach (var item in result)
                            //    result4.Add(item);
                            //if (result2.Count > 0)
                            //{
                            //    for (int i = 0; i < result2.Count; i++)
                            //    {
                            //        result4.Remove(result2[i]);
                            //    }
                            //}
                            //if (result4.Count > 0)
                            //{
                            //    for (int i = 0; i < result4.Count; i++)
                            //    {
                            //        result.Remove(result4[i]);
                            //    }
                            //}
                            result4.Clear();
                            if (result5.Count > 0)
                            {
                                for (int i = 0; i < result5.Count; i++)
                                {
                                    if (result.Contains(result5[i]))
                                        result4.Add(result5[i]);
                                }
                                result = result4;
                            }

                        }
                    }
                }
                else
                {
                    //
                    if (objClientSearch.SearchArgument.Contains("$"))
                    {
                        string[] datasplit2 = objClientSearch.SearchArgument.Split('$');
                        string searchArg = datasplit2[0];
                        //string sortArg = datasplit2[1];

                        if (objClientSearch.SearchBy == true)
                        {
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.SchoolId == sess.SchoolId && (objStudentpersonal.LastName.StartsWith(searchArg) || objStudentpersonal.FirstName.StartsWith(searchArg))).OrderBy(x => x.LastName).ToList();
                            //result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.LastName.StartsWith(searchArg) || objStudentpersonal.FirstName.StartsWith(searchArg)).ToList();
                        }
                        else if (objClientSearch.SearchBy == false)
                        {
                            try
                            {
                                tempId = Int32.Parse(searchArg);
                            }
                            catch
                            {
                            }
                            result = RPCobj.StudentPersonals.Where(objStudentpersonal => objStudentpersonal.SchoolId == sess.SchoolId && objStudentpersonal.ClientId == tempId).OrderBy(x => x.ClientId).ToList();
                        }
                    }
                    result2 = (from studpersonal in RPCobj.StudentPersonals
                              // join cp in RPCobj.ContactPersonals on studpersonal.StudentPersonalId equals cp.StudentPersonalId
                               where studpersonal.SchoolId == sess.SchoolId && studpersonal.StudentType == "Client" && studpersonal.ClientId > 0
                               select studpersonal).Distinct().ToList();
                    result4 = (from studpersonal in RPCobj.StudentPersonals
                               join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                               where studpersonal.SchoolId == sess.SchoolId && (plc.EndDate == null || plc.EndDate >= DateTime.Today) && plc.Status == 1 && studpersonal.StudentType == "Client"
                               select studpersonal).Distinct().ToList();
                    result5 = (from studpersonal in RPCobj.StudentPersonals
                               join plc in RPCobj.Placements on studpersonal.StudentPersonalId equals plc.StudentPersonalId
                               where studpersonal.SchoolId == sess.SchoolId && studpersonal.PlacementStatus == "D" && studpersonal.StudentType == "Client"
                               select studpersonal).Distinct().ToList();
                    if (result4.Count > 0)
                    {
                        for (int i = 0; i < result4.Count; i++)
                        {
                            result2.Remove(result4[i]);

                        }
                    }
                    if (objClientSearch.activeClient == 1)
                    {
                        if (result2.Count > 0)
                        {
                            for (int i = 0; i < result2.Count; i++)
                            {
                                result.Remove(result2[i]);

                            }
                        }
                        if (result5.Count > 0)
                        {
                            for (int i = 0; i < result5.Count; i++)
                            {
                                result.Remove(result5[i]);
                            }
                        }

                    }
                    else if (objClientSearch.activeClient == 2)
                    {
                        result4.Clear();
                        foreach (var item in result)
                            result4.Add(item);

                        if (result2.Count > 0)
                        {
                            for (int i = 0; i < result2.Count; i++)
                            {
                                result4.Remove(result2[i]);

                            }
                        }
                        if (result4.Count > 0)
                        {
                            for (int i = 0; i < result4.Count; i++)
                            {
                                result.Remove(result4[i]);

                            }
                        }
                        //if (result5.Count > 0)
                        //{
                        //    for (int i = 0; i < result5.Count; i++)
                        //    {
                        //        result.Add(result5[i]);
                        //    }
                        //}
                        if (result5.Count > 0)
                        {
                            for (int i = 0; i < result5.Count; i++)
                            {
                                result.Remove(result5[i]);
                            }
                        }

                    }
                    else if (objClientSearch.activeClient == 4)
                    {
                        //result4.Clear();
                        //foreach (var item in result)
                        //    result4.Add(item);
                        //if (result2.Count > 0)
                        //{
                        //    for (int i = 0; i < result2.Count; i++)
                        //    {
                        //        result4.Remove(result2[i]);
                        //    }
                        //}
                        //if (result4.Count > 0)
                        //{
                        //    for (int i = 0; i < result4.Count; i++)
                        //    {
                        //        result.Remove(result4[i]);
                        //    }
                        //}
                        result4.Clear();
                        foreach (var item in result)
                            result4.Add(item);

                        if (result5.Count > 0)
                        {
                            for (int i = 0; i < result5.Count; i++)
                            {
                                result4.Remove(result5[i]);

                            }
                        }
                        if (result4.Count > 0)
                        {
                            for (int i = 0; i < result4.Count; i++)
                            {
                                result.Remove(result4[i]);

                            }
                        }

                        //if (result5.Count > 0)
                        //{
                        //    for (int i = 0; i < result5.Count; i++)
                        //    {
                        //        if (result.Contains(result5[i]))
                        //            result4.Add(result5[i]);
                        //    }
                        //    result = result4;
                        //}

                    }
                    result = clientSearchList(objClientSearch, result);

                }
                result = result.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                if (result.Count == pagesize)
                {
                    dec = 1;
                    flag = ">";
                }

            }
            objClientSearch.flag = flag;
            objClientSearch.perPage = page;
            string gender = "";
            string suffix = "";
            if (result != null && result.Count > 0)
            {

                for (int i = 0; i < result.Count - dec; i++)
                {
                    if (result[i].Gender == "1")
                    {
                        gender = "Male";
                    }
                    else if (result[i].Gender == "2")
                    {
                        gender = "Female";
                    }
                    objClientSearch.itemCount = 1;


                    switch (result[i].Suffix)
                    {
                        case "1": suffix = "Jr.";
                            break;
                        case "2": suffix = "Sr.";
                            break;
                        case "3": suffix = "I";
                            break;
                        case "4": suffix = "II";
                            break;
                        case "5": suffix = "III";
                            break;
                        case "6": suffix = "IV";
                            break;
                        case "7": suffix = "V";
                            break;
                        case "8": suffix = "VI";
                            break;
                        default: suffix = "";
                            break;
                    }

                    //if (result[i].Suffix == "1")
                    //{
                    //    suffix = "Jr.";
                    //}
                    //else if (result[i].Suffix == "2")   //List Clients Suffix Name
                    //{
                    //    suffix = "Sr.";
                    //}
                    //else
                    //    suffix = "";
                    ClientModel model = new ClientModel
                    {
                        Id = result[i].StudentPersonalId,
                        Name = result[i].LastName + " " + suffix + " , " + result[i].FirstName,
                        DateOfBirth = ConvertDate(result[i].BirthDate),
                        Gender = gender,
                        ImageUrl = result[i].ImageUrl,
                        //Age = (DateTime.Now - result[i].BirthDate).ToString(),
                    };
                    //int tempDatetime;
                    decimal tempDatetime;
                    TimeSpan Diff;
                    try
                    {
                        //tempDatetime = DateTime.Now.Year - ((DateTime)result[i].BirthDate).Year;
                        Diff = DateTime.Now - ((DateTime)result[i].BirthDate);
                        tempDatetime = (int)((decimal)Diff.TotalDays / 365);
                        if (tempDatetime < 0) tempDatetime = tempDatetime * -1;
                        //model.Age = ((int)Diff.TotalDays / 365) + " Years and " + (int)((Diff.TotalDays % 365) / 30) + " Months";

                        DateTime birthday = ((DateTime)result[i].BirthDate);

                        DateTime today = DateTime.Today;
                        int age = today.Year - birthday.Year;
                        if (birthday > today.AddYears(-age)) age--;
                        model.Age = age.ToString();

                        model.Age = calulateAge(ConvertDate(result[i].BirthDate));


                    }
                    catch
                    {
                        tempDatetime = 0;
                        model.Age = "1 Year";
                        //model.Age = tempDatetime.ToString();
                    }
                    // model.Age = tempDatetime.ToString();//(Convert.ToInt32(tempDatetime.TotalDays / 360)).ToString();
                    if (model.Age == "")
                    {
                        // Diff = DateTime.Now - ((DateTime)result[i].BirthDate);
                        //model.Age = (int)((Diff.TotalDays % 365) / 30) + " Months";
                        model.Age = "1 Year";
                    }
                    retunmodel.Add(model);

                }
            }


            return retunmodel;
        }

        public static string calulateAge(string DateOfBirth)
        {

            DateTime Cday = DateTime.Now;
            DateTime Bday = DateTime.ParseExact(DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            int Years, Months, Days;


            if ((Cday.Year - Bday.Year) > 0 ||
(((Cday.Year - Bday.Year) == 0) && ((Bday.Month < Cday.Month) ||
  ((Bday.Month == Cday.Month) && (Bday.Day <= Cday.Day)))))
            {
                int DaysInBdayMonth = DateTime.DaysInMonth(Bday.Year, Bday.Month);
                int DaysRemain = Cday.Day + (DaysInBdayMonth - Bday.Day);

                if (Cday.Month > Bday.Month)
                {
                    Years = Cday.Year - Bday.Year;
                    Months = Cday.Month - (Bday.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
                else if (Cday.Month == Bday.Month)
                {
                    if (Cday.Day >= Bday.Day)
                    {
                        Years = Cday.Year - Bday.Year;
                        Months = 0;
                        Days = Cday.Day - Bday.Day;
                    }
                    else
                    {
                        Years = (Cday.Year - 1) - Bday.Year;
                        Months = 11;
                        Days = DateTime.DaysInMonth(Bday.Year, Bday.Month) - (Bday.Day - Cday.Day);
                    }
                }
                else
                {
                    Years = (Cday.Year - 1) - Bday.Year;
                    Months = Cday.Month + (11 - Bday.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
            }
            else
            {
                throw new ArgumentException("Birthday date must be earlier than current date");
            }

            return Years + " Years " + Months + " Months";// +Days + " Days";
        }

        private static IList<StudentPersonal> clientSearchList(ClientSearch objClientSearch, IList<StudentPersonal> result)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            IList<StudentPersonal> results = new List<StudentPersonal>();

            //
            if (objClientSearch.SearchArgument.Contains("$"))
            {
                string[] datasplit2 = objClientSearch.SearchArgument.Split('$');
                objClientSearch.SearchArgument = datasplit2[1];
            }

            if (objClientSearch.OrderBy == true)
            {
                switch (objClientSearch.SearchArgument)
                {
                    case "Age":
                        results = result.OrderBy(p => p.BirthDate).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "Date of Birth":
                        results = result.OrderBy(p => p.BirthDate).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "AdmissionDate":
                        results = result.OrderBy(p => p.AdmissionDate).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "ClientID":
                        results = result.OrderBy(p => p.ClientId).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "FirstName":
                        results = result.OrderBy(p => p.FirstName).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "LastName":
                        results = result.OrderBy(p => p.LastName).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    default:
                        results = result.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;

                }
            }
            else if (objClientSearch.OrderBy == false)
            {

                switch (objClientSearch.SearchArgument)
                {
                    case "Age":
                        results = result.OrderByDescending(p => p.BirthDate).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "Date of Birth":
                        results = result.OrderByDescending(p => p.BirthDate).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "AdmissionDate":
                        results = result.OrderByDescending(p => p.AdmissionDate).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "ClientID":
                        results = result.OrderByDescending(p => p.ClientId).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "FirstName":
                        results = result.OrderByDescending(p => p.FirstName).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    case "LastName":
                        results = result.OrderByDescending(p => p.LastName).Where(objStudentpersonal => objStudentpersonal.StudentType == "Client"
                            && objStudentpersonal.SchoolId == sess.SchoolId).ToList();
                        results = results.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;
                    default:
                        results = result.Skip(Convert.ToInt32(objClientSearch.PagingArgument) * (16 - 1)).Take(16).ToList();
                        break;

                }
            }
            return results;
        }

        private static string ConvertDate(DateTime? nullable)
        {
            string result = "";
            DateTime temp;
            try
            {
                temp = (DateTime)nullable;
                result = temp.ToString("MM/dd/yyyy").Replace('-', '/');
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public static string ConvertDate(DateTime dateString)
        {
            string result = "";
            DateTime temp = (DateTime)dateString;
            result = temp.ToString("MM/dd/yyyy").Replace('-', '/');
            return result;
        }



    }


}