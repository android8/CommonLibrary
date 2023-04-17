using System.DirectoryServices.AccountManagement;
using Microsoft.AspNetCore.Http;

namespace VsscHelper_CoreLibrary
{
  public class LDAP_Helper
  {
    public static string NetworkName { get; private set; }
    public static string LastName { get; private set; }
    public static string FirstName { get; private set; }
    public static string MiddleName { get; private set; }
    public static string Email { get; private set; }
    public static string VoicePhone { get; private set; }
    public static UserPrincipal CompleteActiveDirectoryEntry {get; private set;}

    public static bool checkUser(string t_username, string u_pwd)
    {
      //bool is_good = false;
      //string LDAP = "LDAP://<LDAP IP GOES HERE>/OU=User Accounts,DC=<network>,DC=<more network>,DC=<more network info>";
      //string username = @"<same as network>\" + t_username;
      //DirectoryEntry de = new DirectoryEntry(LDAP, username, u_pwd);

      //try
      //{
      //    // Test binding to see if user creds are good
      //    Object obj = de.NativeObject;
      //    is_good = true;
      //}
      //catch (Exception ex)
      //{
      //    Console.WriteLine(ex.Message);
      //    is_good = false;
      //}
      //return is_good;

      return false;
    }

    public LDAP_Helper() 
    {

    }
    
    public LDAP_Helper(IHttpContextAccessor httpContextAccessor, string domain, string userAccount)
    {
      //string LDAPServiceAccount = System.Configuration.ConfigurationManager.AppSettings["LDAPUser"].ToString();
      //string LDAPServiceAccountPwd = System.Configuration.ConfigurationManager.AppSettings["LDAPPassword"].ToString();
      using(var context = new System.DirectoryServices.AccountManagement.PrincipalContext(
          System.DirectoryServices.AccountManagement.ContextType.Domain, domain)) //, LDAPServiceAccount, LDAPServiceAccountPwd))
      {
        //query specified userAccount or the current login account
        if(string.IsNullOrEmpty(userAccount))
          userAccount = httpContextAccessor.HttpContext.User.Identity.Name;

        var principal = System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(context, userAccount);

        CompleteActiveDirectoryEntry = principal;
        NetworkName = principal.SamAccountName;
        LastName = principal.Surname;
        FirstName = principal.GivenName;
        MiddleName = principal.MiddleName;
        Email = principal.EmailAddress;
        VoicePhone = principal.VoiceTelephoneNumber;
      }
    }
  }
}
