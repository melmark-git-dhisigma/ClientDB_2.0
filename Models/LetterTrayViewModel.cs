using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientDB.Models
{
    public class LetterTrayViewModel
    {
        public string LetterItem { get; set; }
        public string LetterName { get; set; }
        public IList<LetterTrayViewModel> LetterList { get; set; }


        public LetterTrayViewModel()
        {
            LetterList = new List<LetterTrayViewModel>();
        }

    }

}