﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using ClientDB.DbModel;
using ClientDB.AppFunctions;

namespace ClientDB.AppFunctions
{
    public class ClientSearch
    {

        public virtual string SearchArgument { get; set; }
        public virtual bool SortStatus { get; set; }
        public virtual string PagingArgument { get; set; }
        public virtual int itemCount { get; set; }
        public virtual string flag {get; set;}
        public virtual int perPage { get; set; }
        public virtual bool OrderBy { get; set; }
        public virtual bool SearchBy { get; set; }
        public virtual int activeClient { get; set; }


    }
    public class ContactSearch
    {
        public virtual string SearchArgument { get; set; }
        public virtual bool SortStatus { get; set; }
        public virtual string PagingArgument { get; set; }
        public virtual int itemCount { get; set; }
        public virtual string flag { get; set; }
        public virtual int perPage { get; set; }
    }
}