using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PCC_FIT.Helpers.Factory
{
  /// <summary>
  /// In ASP.NET, we can access cookies using httpcontext.current but in ASP.NET Core, there is no htttpcontext.current. 
  /// In ASP.NET Core, everything is decoupled and modular. Httpcontext is accessible from Request object and 
  /// the IHttpContextAccessor interface which is under "Microsoft.AspNetCore.Http" namespace and this is available 
  /// anywhere in the application.
  /// This factory method allows setter injection by instantiating and return the required class that would have to be CTOR injected
  /// 
  /// Reference:
  /// https://www.c-sharpcorner.com/article/asp-net-core-working-with-cookie/
  /// https://stackoverflow.com/questions/6138924/constructor-dependency-injection-in-base-class
  /// </summary>
  public static class HttpContextAccessorFactory
  {
    private static IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();


    public static string GetCookies(string key)
    {
      return _httpContextAccessor.HttpContext.Request.Cookies[key];
    }

    public static void SetCookies(string key, string value)
    {
      _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
      CookieOptions options = new CookieOptions()
      { 
        //Expires = DateTime.Now.AddDays(1),
        IsEssential = true, 
        Secure = true
      };
      _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
    }
  }
}
