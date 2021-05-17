using UnityEngine;

public class JsonSaveLoad : ISaveLoadVisiter
{
    private const string GemBalanceKey = nameof(GemBalanceKey);

    #region GemBalance
    public void Save(GemBalance balance)
    {
        string saveJson = JsonUtility.ToJson(balance);
        PlayerPrefs.SetString(GemBalanceKey, saveJson);
        PlayerPrefs.Save();
    }

    public GemBalance Load(GemBalance balance)
    {
        if (PlayerPrefs.HasKey(GemBalanceKey))
        {
            string saveJson = PlayerPrefs.GetString(GemBalanceKey);
            return JsonUtility.FromJson<GemBalance>(saveJson);
        }

        return balance;
    }
    #endregion
}
