using MySql.Data.MySqlClient;
using Dapper;

namespace SPlayTime;

public class MySqlDBConnector : IDBConnector
{
    readonly Config Config;
    MySqlConnection Connection;
    DateTime LastConnectionUpdate = DateTime.MinValue;
    public MySqlDBConnector(Config config)
    {
        Config = config;
    }
    public void TryUpdateConnection()
    {
        if ((DateTime.Now - LastConnectionUpdate).TotalSeconds < 60)
            return;
        Connection?.Dispose();
        LastConnectionUpdate = DateTime.Now;
        Connection = new MySqlConnection(Config.ConnectionString);
    }

    public uint GetSeconds(string id)
    {
        TryUpdateConnection();
        return Connection.QueryFirstOrDefault<uint>($"SELECT `{Config.TimeColumn}` FROM `{Config.TableName}` WHERE `{Config.IdColumn}` = '{id}'");
    }

    public void Dispose() => Connection?.Dispose();

    public bool TryConnect()
    {
        try
        {
            GetSeconds("0");
            return true;
        }
        catch (Exception ex)
        {
            try
            {
                Logger.Log(ex);
            }
            catch { Console.WriteLine(ex); }
        }
        return false;
    }
}
