public interface ISaveLoadVisiter
{
    void Save(GemBalance balance);
    GemBalance Load(GemBalance balance);

    void Save(SnakeInventory balance);
    SnakeInventory Load(SnakeInventory balance);

    void Save(CurrentLevelData currentLevel);
    CurrentLevelData Load(CurrentLevelData currentLevel);
}
