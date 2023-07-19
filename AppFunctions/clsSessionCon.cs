using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using ClientDB.AppFunctions;


/// <summary>
/// Summary description for clsSessionCon
/// </summary>
public class clsSessionCon
{
    public clsSessionCon()
    {

    }
    public object getSessionObject()
    {
        clsSession sess = new clsSession();
        if (HttpContext.Current.Session["IsLogin"] != null && HttpContext.Current.Session["perPage"] != null)
        {
            sess.IsLogin = Convert.ToBoolean(HttpContext.Current.Session["IsLogin"]);
            sess.Classid = Convert.ToInt32(HttpContext.Current.Session["Classid"]);
            sess.SchoolId = Convert.ToInt32(HttpContext.Current.Session["SchoolId"]);
            sess.LoginId = Convert.ToInt32(HttpContext.Current.Session["LoginId"]);
            sess.UserName = HttpContext.Current.Session["UserName"].ToString();
            sess.RoleId = Convert.ToInt32(HttpContext.Current.Session["RoleId"]);
            sess.RoleName = HttpContext.Current.Session["RoleName"].ToString();
            sess.YearId = Convert.ToInt32(HttpContext.Current.Session["YearId"]);
            sess.perPage = (ArrayList)HttpContext.Current.Session["perPage"];
            sess.perPageName = (Hashtable)HttpContext.Current.Session["perPageName"];
            sess.perPageBinder = (ArrayList)HttpContext.Current.Session["perPageBinder"];
            if (HttpContext.Current.Session["StudentId"] != null) sess.StudentId = Convert.ToInt16(HttpContext.Current.Session["StudentId"]);
            if (HttpContext.Current.Session["StudentName"] != null) sess.StudentName = HttpContext.Current.Session["StudentName"].ToString();
            if (HttpContext.Current.Session["IEPId"] != null) sess.IEPId = Convert.ToInt32(HttpContext.Current.Session["IEPId"]);
            if (HttpContext.Current.Session["IEPStatus"] != null) sess.IEPStatus = Convert.ToInt32(HttpContext.Current.Session["IEPStatus"]);
            
        }
        return sess;
    }
}