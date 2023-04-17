﻿/*
 *  
 * This class will have the properties corresponding to the information of the AD User.
 * 1. This class has read only properties for fetching First Name, Last Name, City, Login Name etc.
 * 2. Constructor of the class is taking one parameter of type DirectoryEntry class.
 * 3. In Constructor all the information about ADUser is getting fetched using static class ADProperties.
 * 4. There are two static functions inside this class. GetUser and GetProperty
 * 5. Get Property is returning a string which holds property of AD User.
 * 6. GetUser static function is returning an ADUser. 
 * 
 */
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace VsscHelper_CoreLibrary
{
   public class ActiveDirectoryHelper
   {
      private DirectoryEntry _directoryEntry = null;

      private DirectoryEntry SearchRoot
      {
         get
         {
            if (_directoryEntry == null)
            {
               _directoryEntry = new DirectoryEntry(LDAPPath, LDAPUser, LDAPPassword, AuthenticationTypes.Secure);

            }
            return _directoryEntry;
         }
      }

      private String LDAPPath
      {
         get
         {
            //return ;
            System.DirectoryServices.DirectoryEntry DirectoryEntryTemp = new DirectoryEntry(ConfigurationManager.AppSettings["LDAPPath"]);
            String nameStr = "LDAP://" + DirectoryEntryTemp.Properties["dnsHostName"].Value.ToString();
            return nameStr;
         }
      }

      private String LDAPUser
      {
         get
         {
            return ConfigurationManager.AppSettings["LDAPUser"];
         }
      }

      private String LDAPPassword
      {
         get
         {
            return ConfigurationManager.AppSettings["LDAPPassword"];
         }
      }

      private String LDAPDomain
      {
         get
         {
            return ConfigurationManager.AppSettings["LDAPDomain"];
         }
      }

      internal ADUserDetail GetUserByFullName(String userName)
      {
         try
         {
            _directoryEntry = null;
            DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
            directorySearch.Filter = "(&(objectClass=user)(cn=" + userName + "))";
            SearchResult results = directorySearch.FindOne();

            if (results != null)
            {
               DirectoryEntry user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
               return ADUserDetail.GetUser(user);
            }
            else
            {
               return null;
            }
         }
         catch (Exception ex)
         {
            return null;
         }
      }

      public ADUserDetail GetUserByLoginName(String userName)
      {


         try
         {
            _directoryEntry = null;
            DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
            directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))";


            SearchResult results = directorySearch.FindOne();

            if (results != null)
            {

               DirectoryEntry user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
               return ADUserDetail.GetUser(user);
            }

            return null;
         }
         catch (Exception ex)
         {
            //System.Web.HttpContext.Current.Trace.Write("GetUserByLoginName(): " + ex.Message.ToString());
            return null;
         }
      }


      /// <summary>
      /// This function will take a DL or Group name and return list of users
      /// </summary>
      /// <param name="groupName"></param>
      /// <returns></returns>
      public List<ADUserDetail> GetUserFromGroup(String groupName)
      {
         List<ADUserDetail> userlist = new List<ADUserDetail>();
         try
         {
            _directoryEntry = null;
            DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
            directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + groupName + "))";
            SearchResult results = directorySearch.FindOne();
            if (results != null)
            {

               DirectoryEntry deGroup = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
               System.DirectoryServices.PropertyCollection pColl = deGroup.Properties;
               int count = pColl["member"].Count;


               for (int i = 0; i < count; i++)
               {
                  string respath = results.Path;
                  string[] pathnavigate = respath.Split("CN".ToCharArray());
                  respath = pathnavigate[0];
                  string objpath = pColl["member"][i].ToString();
                  string path = respath + objpath;


                  DirectoryEntry user = new DirectoryEntry(path, LDAPUser, LDAPPassword);
                  ADUserDetail userobj = ADUserDetail.GetUser(user);
                  userlist.Add(userobj);
                  user.Close();
               }
            }
            return userlist;
         }
         catch (Exception ex)
         {
            return userlist;
         }

      }

      #region Get user with First Name

      public List<ADUserDetail> GetUsersByFirstName(string fName)
      {

         //UserProfile user;
         List<ADUserDetail> userlist = new List<ADUserDetail>();
         string filter = "";

         _directoryEntry = null;
         DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
         directorySearch.Asynchronous = true;
         directorySearch.CacheResults = true;
         filter = string.Format("(givenName={0}*", fName);
         //  filter = "(&(objectClass=user)(objectCategory=person)(givenName="+fName+ "*))";


         directorySearch.Filter = filter;

         SearchResultCollection userCollection = directorySearch.FindAll();
         foreach (SearchResult users in userCollection)
         {
            DirectoryEntry userEntry = new DirectoryEntry(users.Path, LDAPUser, LDAPPassword);
            ADUserDetail userInfo = ADUserDetail.GetUser(userEntry);

            userlist.Add(userInfo);

         }

         directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + fName + "*))";
         SearchResultCollection results = directorySearch.FindAll();
         if (results != null)
         {

            foreach (SearchResult r in results)
            {
               DirectoryEntry deGroup = new DirectoryEntry(r.Path, LDAPUser, LDAPPassword);

               ADUserDetail agroup = ADUserDetail.GetUser(deGroup);
               userlist.Add(agroup);
            }

         }
         return userlist;
      }

      #endregion

      #region AddUserToGroup
      public bool AddUserToGroup(string userlogin, string groupName)
      {
         try
         {
            _directoryEntry = null;
            ADManager admanager = new ADManager(LDAPDomain, LDAPUser, LDAPPassword);
            admanager.AddUserToGroup(userlogin, groupName);
            return true;
         }
         catch (Exception ex)
         {
            return false;
         }
      }
      #endregion


      #region RemoveUserToGroup
      public bool RemoveUserToGroup(string userlogin, string groupName)
      {
         try
         {
            _directoryEntry = null;
            ADManager admanager = new ADManager("xxx", LDAPUser, LDAPPassword);
            admanager.RemoveUserFromGroup(userlogin, groupName);
            return true;
         }
         catch (Exception ex)
         {
            return false;
         }
      }
      #endregion
   }
   public class ADManager
   {
      PrincipalContext context;

      public ADManager()
      {
         context = new PrincipalContext(ContextType.Machine, "xxx", "xxx", "xxx");
      }

      public ADManager(string domain, string container)
      {
         context = new PrincipalContext(ContextType.Domain, domain, container);
      }

      public ADManager(string domain, string username, string password)
      {
         context = new PrincipalContext(ContextType.Domain, username, password);
      }

      public bool AddUserToGroup(string userName, string groupName)
      {
         bool done = false;
         GroupPrincipal group = GroupPrincipal.FindByIdentity(context, groupName);
         if (group == null)
         {
            group = new GroupPrincipal(context, groupName);
         }
         UserPrincipal user = UserPrincipal.FindByIdentity(context, userName);

         if (user != null & group != null)
         {
            group.Members.Add(user);
            group.Save();
            done = (user.IsMemberOf(group));
         }

         return done;

      }


      public bool RemoveUserFromGroup(string userName, string groupName)
      {

         bool done = false;
         UserPrincipal user = UserPrincipal.FindByIdentity(context, userName);
         GroupPrincipal group = GroupPrincipal.FindByIdentity(context, groupName);

         if (user != null & group != null)
         {
            group.Members.Remove(user);
            group.Save();
            done = !(user.IsMemberOf(group));

         }

         return done;

      }

   }

   public static class ADProperties
   {

      public const String OBJECTCLASS = "objectClass";

      public const String CONTAINERNAME = "cn";

      public const String LASTNAME = "sn";

      public const String COUNTRYNOTATION = "c";

      public const String CITY = "l";

      public const String STATE = "st";

      public const String TITLE = "title";

      public const String POSTALCODE = "postalCode";

      public const String PHYSICALDELIVERYOFFICENAME = "physicalDeliveryOfficeName";

      public const String FIRSTNAME = "givenName";

      public const String MIDDLENAME = "initials";

      public const String DISTINGUISHEDNAME = "distinguishedName";

      public const String INSTANCETYPE = "instanceType";

      public const String WHENCREATED = "whenCreated";

      public const String WHENCHANGED = "whenChanged";

      public const String DISPLAYNAME = "displayName";

      public const String USNCREATED = "uSNCreated";

      public const String MEMBEROF = "memberOf";

      public const String USNCHANGED = "uSNChanged";

      public const String COUNTRY = "co";

      public const String DEPARTMENT = "department";

      public const String COMPANY = "company";

      public const String PROXYADDRESSES = "proxyAddresses";

      public const String STREETADDRESS = "streetAddress";

      public const String DIRECTREPORTS = "directReports";

      public const String NAME = "name";

      public const String OBJECTGUID = "objectGUID";

      public const String USERACCOUNTCONTROL = "userAccountControl";

      public const String BADPWDCOUNT = "badPwdCount";

      public const String CODEPAGE = "codePage";

      public const String COUNTRYCODE = "countryCode";

      public const String BADPASSWORDTIME = "badPasswordTime";

      public const String LASTLOGOFF = "lastLogoff";

      public const String LASTLOGON = "lastLogon";

      public const String PWDLASTSET = "pwdLastSet";

      public const String PRIMARYGROUPID = "primaryGroupID";

      public const String OBJECTSID = "objectSid";

      public const String ADMINCOUNT = "adminCount";

      public const String ACCOUNTEXPIRES = "accountExpires";

      public const String LOGONCOUNT = "logonCount";

      public const String LOGINNAME = "sAMAccountName";

      public const String SAMACCOUNTTYPE = "sAMAccountType";

      public const String SHOWINADDRESSBOOK = "showInAddressBook";

      public const String LEGACYEXCHANGEDN = "legacyExchangeDN";

      public const String USERPRINCIPALNAME = "userPrincipalName";

      public const String EXTENSION = "ipPhone";

      public const String SERVICEPRINCIPALNAME = "servicePrincipalName";

      public const String OBJECTCATEGORY = "objectCategory";

      public const String DSCOREPROPAGATIONDATA = "dSCorePropagationData";

      public const String LASTLOGONTIMESTAMP = "lastLogonTimestamp";

      public const String EMAILADDRESS = "mail";

      public const String MANAGER = "manager";

      public const String MOBILE = "mobile";

      public const String PAGER = "pager";

      public const String FAX = "facsimileTelephoneNumber";

      public const String HOMEPHONE = "homePhone";

      public const String MSEXCHUSERACCOUNTCONTROL = "msExchUserAccountControl";

      public const String MDBUSEDEFAULTS = "mDBUseDefaults";

      public const String MSEXCHMAILBOXSECURITYDESCRIPTOR = "msExchMailboxSecurityDescriptor";

      public const String HOMEMDB = "homeMDB";

      public const String MSEXCHPOLICIESINCLUDED = "msExchPoliciesIncluded";

      public const String HOMEMTA = "homeMTA";

      public const String MSEXCHRECIPIENTTYPEDETAILS = "msExchRecipientTypeDetails";

      public const String MAILNICKNAME = "mailNickname";

      public const String MSEXCHHOMESERVERNAME = "msExchHomeServerName";

      public const String MSEXCHVERSION = "msExchVersion";

      public const String MSEXCHRECIPIENTDISPLAYTYPE = "msExchRecipientDisplayType";

      public const String MSEXCHMAILBOXGUID = "msExchMailboxGuid";

      public const String NTSECURITYDESCRIPTOR = "nTSecurityDescriptor";

   }

   public class ADUserDetail
   {
      private String _firstName;
      private String _middleName;
      private String _lastName;
      private String _loginName;
      private String _loginNameWithDomain;
      private String _streetAddress;
      private String _city;
      private String _state;
      private String _postalCode;
      private String _country;
      private String _homePhone;
      private String _extension;
      private String _mobile;
      private String _fax;
      private String _emailAddress;
      private String _title;
      private String _company;
      private String _manager;
      private String _managerName;
      private String _department;

      public String Department
      {
         get { return _department; }
      }

      public String FirstName
      {
         get { return _firstName; }
      }

      public String MiddleName
      {
         get { return _middleName; }
      }

      public String LastName
      {
         get { return _lastName; }
      }

      public String LoginName
      {
         get { return _loginName; }
      }

      public String LoginNameWithDomain
      {
         get { return _loginNameWithDomain; }
      }

      public String StreetAddress
      {
         get { return _streetAddress; }
      }

      public String City
      {
         get { return _city; }
      }

      public String State
      {
         get { return _state; }
      }

      public String PostalCode
      {
         get { return _postalCode; }
      }

      public String Country
      {
         get { return _country; }
      }

      public String HomePhone
      {
         get { return _homePhone; }
      }

      public String Extension
      {
         get { return _extension; }
      }

      public String Mobile
      {
         get { return _mobile; }
      }

      public String Fax
      {
         get { return _fax; }
      }

      public String EmailAddress
      {
         get { return _emailAddress; }
      }

      public String Title
      {
         get { return _title; }
      }

      public String Company
      {
         get { return _company; }
      }

      public ADUserDetail Manager
      {
         get
         {
            if (!String.IsNullOrEmpty(_managerName))
            {
               ActiveDirectoryHelper ad = new ActiveDirectoryHelper();
               return ad.GetUserByFullName(_managerName);
            }
            return null;
         }
      }

      public String ManagerName
      {
         get { return _managerName; }
      }

      private ADUserDetail(DirectoryEntry directoryUser)
      {
         String domainAddress;
         String domainName;

         _firstName = GetProperty(directoryUser, ADProperties.FIRSTNAME);

         _middleName = GetProperty(directoryUser, ADProperties.MIDDLENAME);

         _lastName = GetProperty(directoryUser, ADProperties.LASTNAME);

         _loginName = GetProperty(directoryUser, ADProperties.LOGINNAME);

         String userPrincipalName = GetProperty(directoryUser, ADProperties.USERPRINCIPALNAME);

         if (!string.IsNullOrEmpty(userPrincipalName))
         {
            domainAddress = userPrincipalName.Split('@')[1];
         }
         else
         {
            domainAddress = String.Empty;
         }

         if (!string.IsNullOrEmpty(domainAddress))
         {
            domainName = domainAddress.Split('.').First();
         }
         else
         {
            domainName = String.Empty;
         }

         _loginNameWithDomain = String.Format(@"{0}\{1}", domainName, _loginName);

         _streetAddress = GetProperty(directoryUser, ADProperties.STREETADDRESS);

         _city = GetProperty(directoryUser, ADProperties.CITY);

         _state = GetProperty(directoryUser, ADProperties.STATE);

         _postalCode = GetProperty(directoryUser, ADProperties.POSTALCODE);

         _country = GetProperty(directoryUser, ADProperties.COUNTRY);

         _company = GetProperty(directoryUser, ADProperties.COMPANY);

         _department = GetProperty(directoryUser, ADProperties.DEPARTMENT);

         _homePhone = GetProperty(directoryUser, ADProperties.HOMEPHONE);

         _extension = GetProperty(directoryUser, ADProperties.EXTENSION);

         _mobile = GetProperty(directoryUser, ADProperties.MOBILE);

         _fax = GetProperty(directoryUser, ADProperties.FAX);

         _emailAddress = GetProperty(directoryUser, ADProperties.EMAILADDRESS);

         _title = GetProperty(directoryUser, ADProperties.TITLE);

         _manager = GetProperty(directoryUser, ADProperties.MANAGER);

         if (!String.IsNullOrEmpty(_manager))
         {
            String[] managerArray = _manager.Split(',');
            _managerName = managerArray[0].Replace("CN=", "");
         }
      }

      private static String GetProperty(DirectoryEntry userDetail, String propertyName)
      {

         if (userDetail.Properties.Contains(propertyName))
         {
            return userDetail.Properties[propertyName][0].ToString();
         }
         else
         {
            return string.Empty;
         }

      }

      public static ADUserDetail GetUser(DirectoryEntry directoryUser)
      {
         return new ADUserDetail(directoryUser);
      }

   }
}
