using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoProtection
{
  /// <summary>
  /// Next, we will create CryptoValueProviderAttribute which will be used to decorate the action method to denote that all the parameters of this method will be bound with the help of CryptoValueProvider.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class CryptoValueProviderAttribute : Attribute, IResourceFilter
  {
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
      context.ValueProviderFactories.Clear();
      context.ValueProviderFactories.Add(new CryptoValueProviderFactory());
    }
  }
}
