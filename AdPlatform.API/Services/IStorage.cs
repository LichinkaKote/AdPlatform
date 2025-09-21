namespace AdPlatform.API.Services
{
    public interface IStorage
    {
        void LoadData(Stream fileStream);
        IEnumerable<string> FindPlatformsForLocation(string location);
    }
}
