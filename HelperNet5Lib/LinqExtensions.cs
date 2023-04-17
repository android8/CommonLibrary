using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsscHelper_CoreLibrary
{
  public static class LinqExtensions
  {
    public static T[] ToArray<T>(this System.Collections.Generic.IEnumerable<T> source)
    {
      if (source == null)
        throw new ArgumentNullException("enumerable");

      return source as T[] ?? source.ToArray();
    }
  }
}
