using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VsscHelper_CoreLibrary
{
    public class Strings
    {
      /*
       * checkEmptyStr(String)
       * Returns true if empty or null, else false
       */
        public static Boolean checkEmptyStr(string checkStr)
        {
            if (checkStr == null || checkStr.Length == 0)
                return true;
            else
                return false;
        }

        /*
        * sanitizeUsername(String1, String2)
        * Remove vha// portion from username if no vha// is found return null
        * String1: Remove From
        * String2: SubString to Remove
        */
        public static string RemoveSubString(string removeFrom, string valueRemoved)
        {

            if (checkEmptyStr(valueRemoved) || checkEmptyStr(removeFrom))
            {
                return null;
            }

            int pos = removeFrom.IndexOf(valueRemoved);
            if (pos >= 0)
            {
                return removeFrom.Remove(pos, valueRemoved.Length);
            }
            return removeFrom;
        }

        public static int? parseNullableInt(string value)
        {
            if (value == null || value.Trim() == string.Empty)
            {
                return null;
            }
            else
            {
                try
                {
                    return int.Parse(value);
                }
                catch
                {
                    return null;
                }
            }

        }
    }
}
