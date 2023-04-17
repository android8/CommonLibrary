namespace EntityLoader.Abstracts
{
   public interface IRepositoryAggregate
   {
      object[] repositories { get; }
      void Save();
   }
}
