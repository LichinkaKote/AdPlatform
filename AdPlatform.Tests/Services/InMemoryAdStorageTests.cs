using AdPlatform.API.Services;
using System.Text;

namespace AdPlatform.Tests.Services
{
    public class InMemoryAdStorageTests
    {
        private readonly InMemoryAdStorage _storage;

        public InMemoryAdStorageTests()
        {
            _storage = new InMemoryAdStorage();
        }

        [Fact]
        public void LoadData_And_FindPlatformsForLocation_IntegrationTest()
        {
            var testData = "Platform1:/ru/svrd/revda\nPlatform2:/ru/svrd";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));

            _storage.LoadData(stream);

            var resultForRevda = _storage.FindPlatformsForLocation("/ru/svrd/revda");
            var resultForSvrd = _storage.FindPlatformsForLocation("/ru/svrd");
            var resultForRu = _storage.FindPlatformsForLocation("/ru");

            Assert.Contains("Platform1", resultForRevda);
            Assert.Contains("Platform2", resultForRevda);

            Assert.Contains("Platform2", resultForSvrd);
            Assert.DoesNotContain("Platform1", resultForSvrd);

            Assert.Empty(resultForRu);
        }

        [Fact]
        public void FindPlatformsForLocation_EmptyLocation_ReturnsEmptyList()
        {
            var result = _storage.FindPlatformsForLocation("");

            Assert.Empty(result);
        }
    }
}
