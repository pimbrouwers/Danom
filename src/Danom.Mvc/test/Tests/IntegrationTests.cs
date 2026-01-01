namespace Danom.Mvc.Tests;

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class MvcTests : IClassFixture<WebApplicationFactory<Program>> {
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MvcTests(WebApplicationFactory<Program> factory) {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/inherited")]
    public async Task Get_Index_ReturnsOk(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        Assert.Equal("text/html; charset=utf-8", resp.Content.Headers.ContentType?.ToString());
        var content = await resp.Content.ReadAsStringAsync();
        Assert.Contains("<h1>Index</h1>", content);
        Assert.Contains("<h2>/</h2>", content);
    }

    [Theory]
    [InlineData("/option/some")]
    [InlineData("/inherited/option/some")]
    public async Task Get_OptionSome_ReturnsOk(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        Assert.Equal("text/html; charset=utf-8", resp.Content.Headers.ContentType?.ToString());
        var content = await resp.Content.ReadAsStringAsync();
        Assert.Contains("<h1>Hello world</h1>", content);
    }

    [Theory]
    [InlineData("/option/none")]
    [InlineData("/inherited/option/none")]
    public async Task Get_OptionNone_ReturnsNotFound(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Theory]
    [InlineData("/option/none/custom")]
    [InlineData("/inherited/option/none/custom")]
    public async Task Get_OptionNone_ReturnsNotFound_Custom(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        Assert.Equal("text/plain; charset=utf-8", resp.Content.Headers.ContentType?.ToString());
        var content = await resp.Content.ReadAsStringAsync();
        Assert.Equal("Custom Not Found!", content);
    }

    [Theory]
    [InlineData("/result/ok")]
    [InlineData("/inherited/result/ok")]
    public async Task Get_ResultOk_ReturnsOk(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        Assert.Equal("text/html; charset=utf-8", resp.Content.Headers.ContentType?.ToString());
        var content = await resp.Content.ReadAsStringAsync();
        Assert.Contains("<h1>Success!</h1>", content);
    }

    [Theory]
    [InlineData("/result/error")]
    [InlineData("/inherited/result/error")]
    public async Task Get_ResultError_ReturnsBadRequest(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var content = await resp.Content.ReadAsStringAsync();
        Assert.NotNull(content);
        Assert.Contains("An error occurred.", content);
    }

    [Theory]
    [InlineData("/result/error/custom")]
    [InlineData("/inherited/result/error/custom")]
    public async Task Get_ResultErrorCustom_ReturnsBadRequest(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        var content = await resp.Content.ReadAsStringAsync();
        Assert.NotNull(content);
        Assert.Equal("Custom error occurred!", content);
    }
}
