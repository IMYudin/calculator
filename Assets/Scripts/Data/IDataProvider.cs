public interface IDataProvider
{
    void Load();
    void Save();
}

public interface IDataProvider<out T> : IDataProvider
{
    T Data { get; }
}