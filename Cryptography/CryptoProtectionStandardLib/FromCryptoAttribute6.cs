using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace CryptoProtection
{
  /// <summary>
  /// FromCryptoAttribute, which can be used to define the optional binding source at parameter level. The rest of the action parameters will be bound with the help of the default Value Provider.
  /// register CryptoParamsProtector and CryptoValueProviderFactory in Startup.ConfigureServices to wire-up altogether.
  /// </summary>
  [AttributeUsage(AttributeTargets.Parameter)]
  public class FromCryptoAttribute : Attribute, IBindingSourceMetadata
  {
    public BindingSource BindingSource => CryptoBindingSource.Crypto;
  }
}
