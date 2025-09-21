namespace AdPlatform.API.Services
{
    public class LocationNode
    {
        public Dictionary<string, LocationNode> Children { get; set; } = new();
        public List<string> AdvertisingPlatforms { get; set; } = new();
        public string FullPath { get; set; } = string.Empty;
    }
}
