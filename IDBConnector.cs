namespace SPlayTime;

public interface IDBConnector : IDisposable
{
    uint GetSeconds(string id);
    bool TryConnect();
}
