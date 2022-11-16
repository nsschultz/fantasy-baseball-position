using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

public class HttpClientFixture : IDisposable
{
    private WebApplicationFactory<Program> _application;

    public HttpClientFixture()
    {
        _application = new WebApplicationFactory<Program>();
        Client = _application.CreateClient();
    }

    public HttpClient Client { get; private set; }

    public void Dispose()
    {
        Client.Dispose();
        _application.Dispose();
    }
}