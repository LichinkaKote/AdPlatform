using AdPlatform.API.Services;

namespace AdPlatform.API
{
    public static class LineParser
    {
        public static List<AdPlatformData> GetData(Stream fileStream)
        {
            var result = new List<AdPlatformData>();
            using (StreamReader sr = new StreamReader(fileStream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var data = new AdPlatformData(line);
                    if (data.IsValid)
                        result.Add(new AdPlatformData(line));
                }
            }
            return result;
        }
    }
}
