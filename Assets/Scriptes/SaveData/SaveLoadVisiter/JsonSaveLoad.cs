using UnityEngine;

public class JsonSaveLoad : ISaveLoadVisiter
{
    private const string GemBalanceKey = nameof(GemBalanceKey);
    private const string SnakeInventoryKey = nameof(SnakeInventoryKey);
    private const string CurrentLevelData = nameof(CurrentLevelData);

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

    #region SnakeInventory
    public void Save(SnakeInventory snakeInventory)
    {
        string saveJson = JsonUtility.ToJson(snakeInventory);
        PlayerPrefs.SetString(SnakeInventoryKey, saveJson);
        PlayerPrefs.Save();
    }

    public SnakeInventory Load(SnakeInventory snakeInventory)
    {
        if (PlayerPrefs.HasKey(SnakeInventoryKey))
        {
            string saveJson = PlayerPrefs.GetString(SnakeInventoryKey);
            return JsonUtility.FromJson<SnakeInventory>(saveJson);
        }

        return snakeInventory;
    }
    #endregion

    #region CurrentLevelData
    public void Save(CurrentLevelData currentLevel)
    {
        string saveJson = JsonUtility.ToJson(currentLevel);
        PlayerPrefs.SetString(CurrentLevelData, saveJson);
        PlayerPrefs.Save();
    }

    public CurrentLevelData Load(CurrentLevelData currentLevel)
    {
        if (PlayerPrefs.HasKey(CurrentLevelData))
        {
            string saveJson = PlayerPrefs.GetString(CurrentLevelData);
            return JsonUtility.FromJson<CurrentLevelData>(saveJson);
        }

        return currentLevel;
    }
    #endregion
}
