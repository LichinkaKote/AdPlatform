namespace AdPlatform.API.Services
{
    public class InMemoryAdStorage : IStorage
    {
        private readonly Dictionary<string, HashSet<string>> _locationsDatabase = new();

        public void LoadData(Stream fileStream)
        {
            _locationsDatabase.Clear();

            var tempDatabase = new Dictionary<string, HashSet<string>>();
            var adPlatformDatas = LineParser.GetData(fileStream);

            InitDatabaseLocations(tempDatabase, adPlatformDatas);
            PopulateDatabase(tempDatabase, adPlatformDatas);

            foreach (var entry in tempDatabase)
                _locationsDatabase.Add(entry.Key, entry.Value);
        }

        public IEnumerable<string> FindPlatformsForLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
                return Enumerable.Empty<string>();

            if (_locationsDatabase.TryGetValue(location, out var platforms))
            {
                return platforms.ToList();
            }

            return Enumerable.Empty<string>();
        }

        private void InitDatabaseLocations(Dictionary<string, HashSet<string>> tempDatabase, List<AdPlatformData> adPlatformDatas)
        {
            foreach (var adPlatformData in adPlatformDatas)
            {
                foreach (var location in adPlatformData.PlatformLocations)
                {
                    if (!tempDatabase.ContainsKey(location))
                        tempDatabase.Add(location, new HashSet<string>{ adPlatformData.PlatformName });
                    else
                        throw new Exception($"Платформа: {adPlatformData.PlatformName} попытка добавить уже существующую локацию {location}");
                }
            }
        }
        private void PopulateDatabase(Dictionary<string, HashSet<string>> tempDatabase, List<AdPlatformData> adPlatformDatas)
        {
            foreach (var adPlatformData in adPlatformDatas)
            {
                foreach (var location in adPlatformData.PlatformLocations)
                {
                    foreach (var data in tempDatabase)
                    {
                        if (data.Key.Contains(location) && data.Key.Replace(location, "").StartsWith('/'))
                            data.Value.Add(adPlatformData.PlatformName);
                    }
                }
            }
        }
    }
}
