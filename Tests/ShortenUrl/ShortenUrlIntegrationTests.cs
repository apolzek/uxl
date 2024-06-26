using System.Net;
using Uxl.Back.ShortenUrl;
using System.Net.Http.Json;

namespace Uxl.Tests.ShortenUrl;

public class ShortenUrlIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_shorten_url()
    {
        // Arrange
        var client = _factory.CreateClient();
        var data = new ShortenUrlIn { Target = _longUrl };

        // Act
        var response = await client.PostAsJsonAsync("/api/urls", data);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Should_not_duplicate_hashes()
    {
        // Arrange
        var client = _factory.CreateClient();
        var data = new ShortenUrlIn { Target = _longUrl };
        var hashes = new HashSet<string>();
        const int count = 1_000;

        // Act
        for (int i = 0; i < count; i++)
        {
            var response = await client.PostAsJsonAsync("/api/urls", data);
            var url = await response.DeserializeTo<UrlOut>();
            hashes.Add(url.Hash);
        }

        // Assert
        hashes.Should().HaveCount(count);
    }
}
