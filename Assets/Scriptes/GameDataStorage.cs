using System;
using UnityEngine;

public static class GameDataStorage
{
    private const string PROGRESS_KEY = "ProgressLevel";
    private const string GEMCOUNT_KEY = "GemCount";

    public static void SaveProgress(int level)
    {
        SaveInt(PROGRESS_KEY, level);
    }

    public static void SaveGemCount(int gem)
    {
        SaveInt(GEMCOUNT_KEY, gem);
    }

    public static int LoadProgress() => LoadInt(PROGRESS_KEY);

    public static int LoadGemCount() => LoadInt(GEMCOUNT_KEY);

    private static void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    private static int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }
}

