
namespace AdPlatform.API.Services
{
    public class InMemoryTrieAdStorage : IStorage
    {
        private readonly LocationNode _root = new LocationNode { FullPath = "/" };
        public IEnumerable<string> FindPlatformsForLocation(string location)
        {
            var result = new HashSet<string>();

            var normalizedSearchLocation = location.Trim().TrimEnd('/');
            var segments = normalizedSearchLocation.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var currentNode = _root;

            result.UnionWith(_root.AdvertisingPlatforms);

            foreach (var segment in segments)
            {
                if (currentNode.Children.TryGetValue(segment, out var nextNode))
                {
                    currentNode = nextNode;
                    result.UnionWith(currentNode.AdvertisingPlatforms);
                }
                else
                    return new List<string>();
            }

            return result.ToList();
        }

        public void LoadData(Stream fileStream)
        {
            var parsedData = LineParser.GetData(fileStream);

            _root.Children.Clear();
            _root.AdvertisingPlatforms.Clear();
            var addedLocations = new List<string>();
            foreach (var data in parsedData)
            {
                foreach (var location in data.PlatformLocations)
                {
                    if (!addedLocations.Contains(location))
                    {
                        AddPlatformToTrie(location, data.PlatformName);
                        addedLocations.Add(location);
                    }
                }
            }
        }
        private void AddPlatformToTrie(string locationPath, string platformName)
        {
            var segments = locationPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var currentNode = _root;

            foreach (var segment in segments)
            {
                if (!currentNode.Children.ContainsKey(segment))
                {
                    var newFullPath = currentNode.FullPath + (currentNode == _root ? "" : "/") + segment;
                    currentNode.Children.Add(segment, new LocationNode { FullPath = newFullPath });
                }
                currentNode = currentNode.Children[segment];
            }

            if (!currentNode.AdvertisingPlatforms.Contains(platformName))
                currentNode.AdvertisingPlatforms.Add(platformName);
        }
    }
}
