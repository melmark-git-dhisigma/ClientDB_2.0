using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientDB.Models
{
    public class ClientFunderModel
    {
        public IList<FunderList> FunderList_emp { get; set; }
    }

    public class FunderList
    {
        public virtual string Employer { get; set; }

    }
}