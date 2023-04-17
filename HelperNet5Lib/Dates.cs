using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VsscHelper_CoreLibrary
{
    public class Dates
    {
        /*
         * Input: DATETIME
         * Output:  True if today is monday
         *          False if today isnt monday
         * A helper function used to determine if today is Monday.
         */
        public static Boolean isMonday(DateTime timenow)
        {
            if (timenow.DayOfWeek.ToString().Equals("Monday"))
                return true;

            return false;
        }

        /*
         * Assumption: timenow will be monday, use isMonday() to check...
         * Input: DATETIME
         * Output:  string containting datetime of friday before monday
         * A helper function used to get the date of friday before current day, requires current day be a monday.
         */
        public static DateTime getFridayBeforeWeekend(DateTime timenow)
        {
            //System.Web.HttpContext.Current.Trace.Write(timenow.ToString());
            if (!timenow.DayOfWeek.ToString().Equals("Monday"))
                throw new InvalidProgramException("Recieved non Monday date for getFridayBeforeWeekend in WebHelp:Dates class.");
            return timenow.AddDays(-3);
        }

        /*
         * Assumption: timenow is any day except monday.
         * Input: DATETIME
         * Output:  string containting datetime of yesterday
         * A helper function used to get the date of yesterday...
         */
        public static DateTime getYesterday(DateTime timenow)
        {
            return timenow.AddDays(-1);
        }

    }
}
