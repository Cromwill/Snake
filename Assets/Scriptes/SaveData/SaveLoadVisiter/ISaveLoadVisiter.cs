public interface ISaveLoadVisiter
{
    void Save(GemBalance balance);
    GemBalance Load(GemBalance balance);

    void Save(SnakeInventory inventory);
    SnakeInventory Load(SnakeInventory inventory);

    void Save(CurrentLevelData currentLevel);
    CurrentLevelData Load(CurrentLevelData currentLevel);

    void Save(HatCollection hatCollection);
    HatCollection Load(HatCollection hatCollection);

    void Save(SettingData setting);
    SettingData Load(SettingData setting);

    void Save(SerializableDateTime time);
    SerializableDateTime Load(SerializableDateTime time);
}
