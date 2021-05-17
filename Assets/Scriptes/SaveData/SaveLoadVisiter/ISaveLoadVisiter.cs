public interface ISaveLoadVisiter
{
    void Save(GemBalance balance);
    GemBalance Load(GemBalance balance);
}
