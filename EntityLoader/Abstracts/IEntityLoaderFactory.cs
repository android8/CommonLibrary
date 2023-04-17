namespace EntityLoader.Abstracts
{
   public interface IEntityLoaderFactory<T> where T : class
  {
    public IEntityLoader<T> GetEntityLoader();
  }
}
