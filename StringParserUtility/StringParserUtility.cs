using Microsoft.VisualBasic;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace StringParserUtility
{
    public class StringParserUtility
    {
        private static string currentCultureName = string.Empty;
        private static CultureInfo cultureInfo = null;
        private static string pattern = string.Empty;
        private static List<string> cultureNames = new();
        private static string decimalSeparator;
        private string amDesignator, pmDesignator, aDesignator, pDesignator;

        // Flags to indicate presence of error information in status bar
        bool valueInfo;
        bool formatInfo;

        private string[] numberFormats = { "C", "D", "E", "e", "F", "G", "N", "P", "R", "X", "x" };
        private const int DEFAULTSELECTION = 5;
        private string[] dateFormats = { "g", "d", "D", "f", "F", "g", "G", "M", "O", "R", "s",
                                       "t", "T", "u", "U", "Y" };
        public StringParserUtility()
        {
            // Populate cultureNames list
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            // Define a string list so that we can sort and modify the names.

            foreach (var culture in cultures)
                cultureNames.Add(culture.Name);

            cultureNames.Sort();

            // Make the current culture the selected culture.
            for (int ctr = 0; ctr < cultureNames.Count; ctr++)
            {
                if (cultureNames[ctr] == CultureInfo.CurrentCulture.Name)
                {
                    currentCultureName = cultureNames[ctr];
                    break;
                }
            }

            // Get am, pm designators.
            amDesignator = DateTimeFormatInfo.CurrentInfo.AMDesignator;
            if (amDesignator.Length >= 1)
                aDesignator = amDesignator.Substring(0, 1);
            else
                aDesignator = String.Empty;

            pmDesignator = DateTimeFormatInfo.CurrentInfo.PMDesignator;
            if (pmDesignator.Length >= 1)
                pDesignator = pmDesignator.Substring(0, 1);
            else
                pDesignator = String.Empty;
        }

        public static string ParseDateString(string dateString)
        {
            // Get name of the current culture.
            cultureInfo = CultureInfo.CreateSpecificCulture(currentCultureName);

            DateTime dat = DateTime.MinValue;
            DateTimeOffset dto = DateTimeOffset.MinValue;
            long ticks;
            bool hasOffset = false;

            // Is the date a number expressed in ticks?
            if (Int64.TryParse(dateString, out ticks))
            {
                dat = new DateTime(ticks);
            }
            else
            {
                // Does the date have three components (date, time offset), or fewer than 3?
                if (Regex.IsMatch(dateString, pattern, RegexOptions.IgnoreCase))
                {
                    if (DateTimeOffset.TryParse(dateString, out dto))
                    {
                        hasOffset = true;
                    }
                    else
                    {
                        return $"{dateString} is an invalid Date Time Offset (DTO)";
                    }
                }
                else
                {
                    // The string is to be interpeted as a DateTime, not a DateTimeOffset.
                    if (DateTime.TryParse(dateString, out dat))
                    {
                        hasOffset = false;
                    }
                    else
                    {
                        return $"{dateString} is an invalid Date Time (DT)"; ;
                    }
                }
            }
            // Format date value.
            return (hasOffset ? dto : dat).ToString("g", cultureInfo);

        }
        public static string ParseNumberString(string numberString)
        {
            // Handle formatting of a number.
            long intToFormat;
            BigInteger bigintToFormat = BigInteger.Zero;
            double floatToFormat;
            // Get decimal separator.
            decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;


            // Format a floating point value.
            if (numberString.Contains(decimalSeparator) || numberString.ToUpper(CultureInfo.InvariantCulture).Contains("E"))
            {
                try
                {
                    if (!Double.TryParse(numberString, out floatToFormat))
                        return $"{numberString} is an invalid float number format";
                    else
                        return floatToFormat.ToString(numberString, cultureInfo);
                }
                catch (FormatException)
                {
                    return $"{numberString} is an invalid number format";
                }
            }
            else
            {
                // Handle formatting an integer.
                //
                // Determine whether value is out of range of an Int64
                if (!BigInteger.TryParse(numberString, out bigintToFormat))
                {
                    return $"{numberString} is an invalid number format";
                }
                else
                {
                    // Format an Int64
                    if (bigintToFormat >= Int64.MinValue && bigintToFormat <= Int64.MaxValue)
                    {
                        intToFormat = (long)bigintToFormat;
                        try
                        {
                            return intToFormat.ToString("g", cultureInfo);
                        }
                        catch (FormatException)
                        {
                            return $"{numberString} is an invalid number format";
                        }
                    }
                    else
                    {
                        // Format a BigInteger
                        try
                        {
                            return bigintToFormat.ToString("g", cultureInfo);
                        }
                        catch (FormatException)
                        {
                            return $"{numberString} is an invalid number format";
                        }
                    }
                }
            }
        }

        public static DateTime ParseStringToDateTime(string dateString)
        {
            // Get name of the current culture.
            cultureInfo = CultureInfo.CreateSpecificCulture(currentCultureName);

            DateTime thisDate = DateTime.MinValue;
            DateTimeOffset theDateTimeOffset = DateTimeOffset.MinValue;
            long ticks;
            bool hasOffset = false;

            // Is the date a number expressed in ticks?
            if (Int64.TryParse(dateString, out ticks))
            {
                thisDate = new DateTime(ticks);
            }
            else
            {
                // Does the date have three components (date, time offset), or fewer than 3?
                if (Regex.IsMatch(dateString, pattern, RegexOptions.IgnoreCase))
                {
                    if (DateTimeOffset.TryParse(dateString, out theDateTimeOffset))
                    {
                        hasOffset = true;
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }
                }
                else
                {
                    // The string is to be interpeted as a DateTime, not a DateTimeOffset.
                    if (DateTime.TryParse(dateString, out thisDate))
                    {
                        hasOffset = false;
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }
                }
            }
            // Format date value.
            return hasOffset ? new DateTime(theDateTimeOffset.Ticks) : thisDate;

        }
        public static Int64 ParseStringToNumber(string numberString)
        {
            // Handle formatting of a number.
            long intToFormat;
            BigInteger bigintToFormat = BigInteger.Zero;
            double floatToFormat;
            // Get decimal separator.
            decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;


            // Format a floating point value.
            if (numberString.Contains(decimalSeparator) || numberString.ToUpper(CultureInfo.InvariantCulture).Contains("E"))
            {
                try
                {
                    if (!Double.TryParse(numberString, out floatToFormat))
                        return Int64.MinValue;
                    else
                        return (long)floatToFormat;
                }
                catch (FormatException)
                {
                    return Int64.MinValue;
                }
            }
            else
            {
                // Handle formatting an integer.
                //
                // Determine whether value is out of range of an Int64
                if (!BigInteger.TryParse(numberString, out bigintToFormat))
                {
                    return Int64.MinValue;
                }
                else
                {
                    // Format an Int64
                    if (bigintToFormat >= Int64.MinValue && bigintToFormat <= Int64.MaxValue)
                    {
                        intToFormat = (long)bigintToFormat;
                        try
                        {
                            return intToFormat;
                        }
                        catch (FormatException)
                        {
                            return Int64.MinValue;
                        }
                    }
                    else
                    {
                        // Format a BigInteger
                        try
                        {
                            return (long)bigintToFormat;
                        }
                        catch (FormatException)
                        {
                            return Int64.MinValue;
                        }
                    }
                }
            }
        }

    }
}
