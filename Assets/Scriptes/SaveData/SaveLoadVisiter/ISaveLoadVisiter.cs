public interface ISaveLoadVisiter
{
    void Save(GemBalance balance);
    GemBalance Load(GemBalance balance);

    void Save(SnakeInventory balance);
    SnakeInventory Load(SnakeInventory balance);
}
