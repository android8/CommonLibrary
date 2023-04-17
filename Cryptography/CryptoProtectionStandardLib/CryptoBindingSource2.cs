using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CryptoProtection
{
  /// <summary>
  /// we define the BindingSource object which represents the binding source for CryptoValueProvider i.e. the encrypted value from the route parameter. This binding source is useful when we want to bind the action method parameter value from the encrypted string as demonstrated in the earlier method
  /// </summary>
  class CryptoBindingSource
  {
    public static readonly BindingSource Crypto = new BindingSource(
     "Crypto",
     "BindingSource_Crypto",
     isGreedy: false,
     isFromRequest: true);
  }
}
