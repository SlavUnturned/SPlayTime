namespace SPlayTime;

public class ConnectionStringBuilder
{
    ConnectionStringBuilder() { UseServer().UseUser(); }
    public static ConnectionStringBuilder Create() => new();
    readonly StringBuilder ConnectionString = new();
    public ConnectionStringBuilder Add(string key, object value) => Add($"{key}={value};");
    public ConnectionStringBuilder Add(string text)
    {
        ConnectionString.Append(text);
        return this;
    }
    public virtual ConnectionStringBuilder UseServer(string server = "localhost") => Add("Server", server);
    public virtual ConnectionStringBuilder UsePort(ushort port = 3306) => 
        Add("Port", port);
    public virtual ConnectionStringBuilder UseDataBase(string name) => 
        Add("Database", name);
    public virtual ConnectionStringBuilder UseUser(string user = "root") => 
        Add("Uid", user);
    public virtual ConnectionStringBuilder UsePassword(string pass) =>
        Add("Pwd", pass);
    public virtual string Build() => ConnectionString.ToString();
    public override sealed string ToString() => Build();
}

public class Config : IRocketPluginConfiguration
{
    public string
        ConnectionString = "Server=127.0.0.1;Port=3306;Database=test;Uid=root;Pwd=pass;",
        TableName = "SomeTable",
        IdColumn = "PlayerId",
        TimeColumn = "PlayTime";
    public void LoadDefaults()
    {
    }
}
