using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProtection
{
  /// <summary>
  /// Now that we have created the CryptoValueProvider, we have to instantiate it and plug it in when model binding takes place for our targeted action method. We will create CryptoValueProviderFactory to accomplish this.
  /// </summary>
  public class CryptoValueProviderFactory : IValueProviderFactory
  {
    public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
      var paramsProtector = (CryptoParamsProtector)context.ActionContext.HttpContext
          .RequestServices.GetService(typeof(CryptoParamsProtector));

      context.ValueProviders.Add(
        new CryptoValueProvider(
          CryptoBindingSource.Crypto,
          paramsProtector,
          context.ActionContext.RouteData.Values["q"]?.ToString()
        )
      );

      return Task.CompletedTask;
    }
  }
}
