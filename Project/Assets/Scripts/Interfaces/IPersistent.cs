public interface IPersistent
{
    public void Subscribe();
    public void Unsubscribe();
    public void LoadFromJson(string persistentDataPath);
    public void SaveAsJson(string persistentDataPath);
}