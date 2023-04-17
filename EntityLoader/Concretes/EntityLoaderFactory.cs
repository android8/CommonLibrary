using EntityLoader.Abstracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityLoader.Concretes
{
   public class EntityLoaderFactory<T> : IEntityLoaderFactory<T> where T : class
   {
      private readonly enumLoaderType _loaderType;

      /// <summary>
      /// CTOR with injection. Concrete class inheriting EntityLoaderFactoryAbstract
      /// </summary>
      /// <param name="repoAggregate"></param>
      public EntityLoaderFactory(enumLoaderType loaderType)
      {
         _loaderType = loaderType;

      }
      public IEntityLoader<T> GetEntityLoader()
      {
         //Reflection
         var loaderTypeName = Enum.GetName(typeof(enumLoaderType), _loaderType);
         string objectToInstantiate =
            $"EntityLoader.Concretes.Entity{loaderTypeName}, EntityLoader";

         var objectType = Type.GetType(objectToInstantiate);

         return (IEntityLoader<T>)Activator.CreateInstance(objectType);

         //EntityLoaderAbstract<T> loader = null;
         //switch (_loaderType)
         //{
         //   case "Eager Loader":
         //      loader = new EntityEagerLoader<T>(_repoAggregate, _fy, _pageNumber);
         //      break;
         //   case "Explicit Loader":
         //      loader = new EntityExplicitLoader<T>(_repoAggregate, _fy, _pageNumber);
         //      break;
         //   case "Lazy Loader":
         //      loader = new EntityLazyLoader<T>(_repoAggregate, _fy, _pageNumber);
         //      break;
         //}
         //return loader;
      }
   }
}
