namespace AdPlatform.API.Services
{
    public class AdPlatformData
    {
        public string PlatformName { get; private set; } = string.Empty;
        public string[] PlatformLocations { get; private set; } = Array.Empty<string>();
        public bool IsValid { get; private set; } = true;

        public AdPlatformData(string data) 
        {
            var parts = data.Split(':', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length != 2 || parts[1].Contains(' '))
            {
                IsValid = false;
                return;
            }

            PlatformName = parts[0].Trim();
            var locations = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            PlatformLocations = new string[locations.Length];
            for (int i = 0; i < locations.Length; i++)
                PlatformLocations[i] = NormalizeLocation(locations[i]);
        }
        private string NormalizeLocation(string location)
        {
            if (!location.StartsWith('/'))
            {
                location = '/' + location;
            }

            location = location.TrimEnd('/');

            if (string.IsNullOrEmpty(location))
            {
                location = "/";
            }
            return location;
        }
    }
}
