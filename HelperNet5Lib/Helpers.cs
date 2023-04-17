using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace VsscHelper_CoreLibrary
{
   public class RuleViolation
  {
    public string ErrorMessage { get; private set; }

    public string PropertyName { get; private set; }

    public RuleViolation(string errorMessage)
    {
      ErrorMessage = errorMessage;
    }

    public RuleViolation(string errorMessage, string propertyName)
    {
      ErrorMessage = errorMessage;
      PropertyName = propertyName;
    }
  }

  public static class ModelStateHelpers
  {
    public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<RuleViolation> errors)
    {
      foreach (RuleViolation issue in errors)
      {
        modelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
      }
    }
  }

  public static class UserExtensions
  {
    public static string GetDomain(this IIdentity identity)
    {
      Match match = Regex.Match(identity.Name, ".*\\\\");
      if (match.Success)
        return match.ToString();
      else
        return string.Empty;
    }

    public static string GetLogin(this IIdentity identity)
    {
      return Regex.Replace(identity.Name, ".*\\\\", "");
    }
  }

  public class PaginatedList<T> : List<T>
  {
    public int PageIndex { get; private set; }

    public int PageSize { get; private set; }

    public int TotalCount { get; private set; }

    public int TotalPages { get; private set; }

    public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize)
    {
      PageIndex = pageIndex;
      PageSize = pageSize <= 0 ? 1 : pageSize;
      TotalCount = source.Count();

      TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

      if (PageIndex < 0) PageIndex = 0;
      if (PageIndex > TotalPages) PageIndex = TotalPages - 1;

      this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
    }

    public bool HasPreviousPage
    {
      get
      {
        return (PageIndex > 0);
      }
    }

    public bool HasNextPage
    {
      get
      {
        return (PageIndex + 1 < TotalPages);
      }
    }
  }

  public class PhoneValidator
  {
    static IDictionary<string, Regex> countryRegex = new Dictionary<string, Regex>()
        {
            //Key, Regex Pattern
            { "USA", new Regex("^(\\d{3})\\D*(\\d{3})\\D*(\\d{4})\\D*(\\d*)$")},
            { "UK", new Regex("(^1300\\d{6}$)|(^1800|1900|1902\\d{6}$)|(^0[2|3|7|8]{1}[0-9]{8}$)|(^13\\d{4}$)|(^04\\d{2,3}\\d{6}$)")},
            { "Netherlands", new Regex("(^\\+[0-9]{2}|^\\+[0-9]{2}\\(0\\)|^\\(\\+[0-9]{2}\\)\\(0\\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\\-\\s]{10}$)")},
        };

    public static bool IsValidNumber(string phoneNumber, string country)
    {
      if (country != null && countryRegex.ContainsKey(country))
        return countryRegex[country].IsMatch(phoneNumber);
      else
        return false;
    }

    //Requires .Net 4.0 Project Target PlatForm
    //public static ValidationResult IsValidNumber(string phoneNumber)
    //{
    //if (IsValidNumber(phoneNumber, "USA"))
    //    return ValidationResult.Success;
    //else
    //    return new ValidationResult("Invalid US phone number");
    //}

    public static IEnumerable<string> Countries
    {
      get
      {
        return countryRegex.Keys;
      }
    }
  }

  public class PhoneParser
  {
    public static string parsePhone(string phoneNumber)
    {
      string parsedPhoneNumber = string.Empty;

      if (string.IsNullOrEmpty(phoneNumber)) return string.Empty;

      //999 d{3}
      //999 d{3}
      //9999 d{4}
      //9999 d*$
      //phone number and extension pattern with optional character delimiters \D*
      Regex phoneRegex = new Regex(@"^(\d{3})\D*(\d{3})\D*(\d{4})\D*(\d*)$");
      Match m = phoneRegex.Match(phoneNumber);
      if (m.Success)
      {
        parsedPhoneNumber = "user: " + m.Groups["user"].Value;
        parsedPhoneNumber += "host: " + m.Groups["host"].Value;
        return parsedPhoneNumber;
      }
      else
        return "Invalid Phone Number";
    }

    public static string stripPuncturation(string s)
    {
      if (String.IsNullOrEmpty(s))
        return String.Empty;

      /*
      var tmpString = from ch in s
        where !Char.IsPunctuation(ch)
        select ch;

      var bytes = UnicodeEncoding.ASCII.GetBytes(s.ToArray());
      var stringResult = UnicodeEncoding.ASCII.GetString(bytes);
      */

      //\\p{P}+ will find all punctuation
      string stringResult = Regex.Replace(s, "\\p{P}+", String.Empty);

      return stringResult;
    }

    public static string formatPhone(string s)
    {
      if (String.IsNullOrEmpty(s))
        return String.Empty;

      string results = string.Empty;
      string formatPattern = @"(\d{3})\D*(\d{3})\D*(\d{4})$";
      results = Regex.Replace(s, formatPattern, "($1) $2-$3");
      return results;
    }
  }

  public class EmailParser
  {
    public static string parseEmail(string emailURI)
    {
      string parsedEmail = string.Empty;

      if (string.IsNullOrEmpty(emailURI)) return string.Empty;

      Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
      Match m = emailRegex.Match(emailURI);
      if (m.Success)
      {
        parsedEmail = "user: " + m.Groups["user"].Value;
        parsedEmail += "host: " + m.Groups["host"].Value;
        return parsedEmail;
      }
      else
        return emailURI;
    }
  }

  public class ActiveDiretoryAgent
  {
    public ActiveDiretoryAgent()
    {
    }

    public UserPrincipal queryUser(System.Security.Principal.IIdentity userIdentity)
    {
      UserPrincipal user;

      using (var context = new PrincipalContext(ContextType.Domain))
      {
        user = UserPrincipal.FindByIdentity(context, userIdentity.Name);
      }
      return user;
    }
  }

  //public class ViewEngine : WebFormViewEngine
  //{
  //    public ViewEngine()
  //    {
  //        PartialViewLocationFormats = PartialViewLocationFormats
  //            .Union(new[]
  //                   {
  //                       "~/Views/{1}/Partial/{0}.ascx",
  //                       "~/Views/Shared/Partial/{0}.ascx",
  //                   }).ToArray();
  //    }
  //}
}