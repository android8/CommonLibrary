using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelperNet5Lib
{
  public static class AsssemblyExtension
  {
    public static IEnumerable<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
    {
      return assembly.GetTypesAssignableFrom(typeof(T));
    }
    public static IEnumerable<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType)
    {
      List<Type> ret = new List<Type>();
      foreach (var type in assembly.DefinedTypes)
      {
        if (compareType.IsAssignableFrom(type) && compareType != type)
        {
          ret.Add(type);
        }
      }
      return ret;
    }
  }
}
