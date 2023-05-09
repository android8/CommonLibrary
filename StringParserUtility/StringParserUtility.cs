using Microsoft.VisualBasic;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

//fork from https://github.com/dotnet/samples/blob/main/windowsforms/formatting-utility/cs

namespace StringParserUtility
{
    public class StringParserUtility
    {
        //private const int DEFAULTSELECTION = 5;
        private static string currentCultureName = string.Empty;
        private static CultureInfo? cultureInfo = null;
        private static string pattern = string.Empty;
        private static string? decimalSeparator;
        private static readonly List<string> cultureNames = new();
        private readonly string amDesignator;
        private readonly string pmDesignator;
        private readonly string aDesignator;
        private readonly string pDesignator;
        //private readonly string[] numberFormats = { "C", "D", "E", "e", "F", "G", "N", "P", "R", "X", "x" };
        //private readonly string[] dateFormats = { "g", "d", "D", "f", "F", "g", "G", "M", "O", "R", "s","t", "T", "u", "U", "Y" };
        // Flags to indicate presence of error information in status bar
        //readonly bool valueInfo;
        //readonly bool formatInfo;

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

            // For regex pattern for date and time components.
            pattern = @$"^\s*\S+\s+\S+\s+\S+(\s+\S+)?(?<!{amDesignator}|{aDesignator}|{pmDesignator}|{pDesignator})\s*$";

        }

        public static string ParseDateString(string dateString)
        {
            // Get name of the current culture.
            cultureInfo = CultureInfo.CreateSpecificCulture(currentCultureName);

            DateTime dat = DateTime.MinValue;
            DateTimeOffset dto = DateTimeOffset.MinValue;
            bool hasOffset = false;

            // Is the date a number expressed in ticks?
            if (Int64.TryParse(dateString, out long ticks))
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
            // Get decimal separator.
            decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;


            // Format a floating point value.
            if (numberString.Contains(decimalSeparator) || numberString.ToUpper(CultureInfo.InvariantCulture).Contains('E'))
            {
                try
                {
                    if (!Double.TryParse(numberString, out double floatToFormat))
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
                if (!BigInteger.TryParse(numberString, out BigInteger bigintToFormat))
                {
                    return $"{numberString} is an invalid number format";
                }
                else
                {
                    // Format an Int64
                    if (BigInteger.Zero >= Int64.MinValue && BigInteger.Zero <= Int64.MaxValue)
                    {
                        intToFormat = (long)BigInteger.Zero;
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
                            return BigInteger.Zero.ToString("g", cultureInfo);
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
            bool hasOffset = false;

            // Is the date a number expressed in ticks?
            if (Int64.TryParse(dateString, out long ticks))
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
            // Get decimal separator.
            decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;


            // Format a floating point value.
            if (numberString.Contains(decimalSeparator) || numberString.ToUpper(CultureInfo.InvariantCulture).Contains('E'))
            {
                try
                {
                    if (!Double.TryParse(numberString, out double floatToFormat))
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
                if (!BigInteger.TryParse(numberString, out BigInteger bigintToFormat))
                {
                    return Int64.MinValue;
                }
                else
                {
                    // Format an Int64
                    if (BigInteger.Zero >= Int64.MinValue && BigInteger.Zero <= Int64.MaxValue)
                    {
                        intToFormat = (long)BigInteger.Zero;
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
                            return (long)BigInteger.Zero;
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
