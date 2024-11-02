namespace _Project.Scripts.Persistence
{
    public interface IBind<TData> where TData : ISaveable
    {
        string Id { get; set; }
       
        void Bind(TData data);
    }
}