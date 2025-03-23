using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Lagkage.Integration.Tests.AcceptanceTests;

public class HttpFixture : IDisposable
{
    private readonly WebApplicationFactory<Program> _application;
    internal HttpClient client;
    internal JsonSerializerOptions serializerOptions;
    
    public HttpFixture()
    {
        _application = new WebApplicationFactory<Program>();
        client = _application.CreateClient();

        serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true

        };
        serializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public void Dispose()
    {
        _application.Dispose();
        client.Dispose();
    }
}