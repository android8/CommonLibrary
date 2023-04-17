namespace EntityLoader.Abstracts
{
   public abstract class EntityLoaderFactoryAbstract<T> where T : class
  {
    public abstract EntityLoaderAbstract<T> GetEntityLoader();
  }
}
