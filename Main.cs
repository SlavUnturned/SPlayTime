using MySql.Data.MySqlClient;

namespace SPlayTime;

public class Main : RocketPlugin<Config>
{
    public static Main Instance { get; private set; }
    public static IDBConnector DBConnector { get; set; }
    protected override void Load()
    {
        Instance = this;
        DBConnector = new MySqlDBConnector(conf);
        if(!DBConnector.TryConnect())
        {
            UnloadPlugin();
            return;
        }
    }

    protected override void Unload()
    {
        DBConnector?.Dispose();
    }

    #region Translations
    public override TranslationList DefaultTranslations => DefaultTranslationList;

    public new string Translate(string key, params object[] args) => base.Translate(key.Trim(TranslationKeyTrimCharacters), args);
    #endregion
}
