using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


    public static class Helpers
    {
        public static bool GetBool(this bool? data)
        {
            if(data==null)
            {
                return false;
            }
            else
            {
                return (bool)data;
            }
        }

        public static string EmptyIfNull(this string obj)
        {
            if (obj == null)
                return "";
            else
                return obj;
        }

        private static Dictionary<string, string> _FirstNamePrefixIndex = new Dictionary<string, string>();
        private static Dictionary<string, string> _LastNameSuffix = new Dictionary<string, string>();

        public static string GetPrefixText(string val)
        {
            if (_FirstNamePrefixIndex.Count == 0)
            {
                _LoadFirstNamePrefix();
            }
            if (val!=null && _FirstNamePrefixIndex.ContainsKey(val))
                return _FirstNamePrefixIndex[val];
            else
                return null;
        }

        public static string GetSuffixText(string val)
        {
            if (_LastNameSuffix.Count == 0)
            {
                _LoadLastNameSuffix();
            }
            if (val!=null && _LastNameSuffix.ContainsKey(val))
                return _LastNameSuffix[val];
            else
                return null;
        }


        private static List<SelectListItem> _GetFirstNamePrefix;
        public static List<SelectListItem> GetFirstNamePrefix
        {
            get
            {
                if (_GetFirstNamePrefix == null)
                {
                    _GetFirstNamePrefix = _LoadFirstNamePrefix();
                }
                return _GetFirstNamePrefix;
            }
        }

        private static List<SelectListItem> _LoadFirstNamePrefix()
        {
            List<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Mr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Mrs.", Value = "2" });
            x.Add(new SelectListItem { Text = "Ms.", Value = "3" });
            x.Add(new SelectListItem { Text = "Dr.", Value = "4" });
            x.MakeListIndex(ref _FirstNamePrefixIndex,1);
            return x;
        }

        private static List<SelectListItem> _GetLastNameSuffix;

        public static List<SelectListItem> GetLastNameSuffix
        {
            get
            {
                if(_GetLastNameSuffix==null)
                {
                    _GetLastNameSuffix = _LoadLastNameSuffix();
                }
                return _GetLastNameSuffix;
            }
        }
        private static List<SelectListItem> _LoadLastNameSuffix()
        {
            List<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Jr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Sr.", Value = "2" });
            x.Add(new SelectListItem { Text = "I", Value = "3" });
            x.Add(new SelectListItem { Text = "II", Value = "4" });
            x.Add(new SelectListItem { Text = "III", Value = "5" });
            x.Add(new SelectListItem { Text = "IV", Value = "6" });
            x.Add(new SelectListItem { Text = "V", Value = "7" });
            x.Add(new SelectListItem { Text = "VI", Value = "8" });
            x.MakeListIndex(ref _LastNameSuffix,1);
            return x;
        }

        public static void MakeListIndex(this IEnumerable<SelectListItem> CurObj,ref Dictionary<string, string> index,int StartIndex=0)
        {
            int CurInx = 0;
            foreach (var item in CurObj)
            {
                if (CurInx < StartIndex)
                {
                    CurInx++;
                    continue;
                }
                if(!index.ContainsKey(item.Value))
                index.Add(item.Value, item.Text);
                CurInx++;
            }
        }
    }