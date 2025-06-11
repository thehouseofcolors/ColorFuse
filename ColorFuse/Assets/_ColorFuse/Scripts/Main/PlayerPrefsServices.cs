using UnityEngine;

public static class PlayerPrefsService
{
    // Seviye Bilgileri
    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt(Constants.CurrentLevelKey, 1);
        set => PlayerPrefs.SetInt(Constants.CurrentLevelKey, value);
    }

    public static int RemainingUndo
    {
        get => PlayerPrefs.GetInt(Constants.RemainingUndoKey, 0);
        set => PlayerPrefs.SetInt(Constants.RemainingUndoKey, value);
    }

    public static int RemainingShuffle
    {
        get => PlayerPrefs.GetInt(Constants.RemainingShuffleKey, 0);
        set => PlayerPrefs.SetInt(Constants.RemainingShuffleKey, value);
    }

    // Ayarlar
    public static bool IsSoundOn
    {
        get => PlayerPrefs.GetInt(Constants.SoundOn, 1) == 1;
        set => PlayerPrefs.SetInt(Constants.SoundOn, value ? 1 : 0);
    }

    public static bool IsMusicOn
    {
        get => PlayerPrefs.GetInt(Constants.MusicOn, 1) == 1;
        set => PlayerPrefs.SetInt(Constants.MusicOn, value ? 1 : 0);
    }

    // Ortak Kullanımlar
    public static void Save() => PlayerPrefs.Save();

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
