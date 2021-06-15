public interface ISavedObject
{
    void Save(ISaveLoadVisiter saveLoadVisiter);
    void Load(ISaveLoadVisiter saveLoadVisiter);
}