public interface IPersistent
{
    public void Subscribe();
    public void Unsubscribe();
    public bool HasJsonSave(string persistentDataPath);
    public void SaveAsJson(string persistentDataPath);
    public void LoadFromJson(string persistentDataPath);
}