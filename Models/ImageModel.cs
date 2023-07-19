using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientDB.Models
{
    public class ImageModel
    {
        public virtual string StudentId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Suffix { get; set; }
        public virtual string ImageUrl { get; set; }

        public string PhotoDate { get; set; }
    }
}