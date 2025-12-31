namespace Danom.MinimalApi.IntegrationTests;

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class MinimalApiTests : IClassFixture<WebApplicationFactory<Program>> {
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MinimalApiTests(WebApplicationFactory<Program> factory) {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("/option/some")]
    [InlineData("/typed/option/some")]
    public async Task Get_OptionSome_ReturnsOk(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Theory]
    [InlineData("/option/none")]
    [InlineData("/typed/option/none")]
    public async Task Get_OptionNone_ReturnsNotFound(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Theory]
    [InlineData("/option/none/custom")]
    [InlineData("/typed/option/none/custom")]
    public async Task Get_OptionNone_ReturnsNotFound_Custom(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        var content = await resp.Content.ReadAsStringAsync();
        Assert.Equal("\"Custom Not Found!\"", content); // Because it's returned as JSON string
    }


    [Theory]
    [InlineData("/result/ok")]
    [InlineData("/typed/result/ok")]
    public async Task Get_ResultOk_ReturnsOk(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var content = await resp.Content.ReadAsStringAsync();
        Assert.Equal("\"Success!\"", content); // Because it's returned as JSON string
    }

    [Theory]
    [InlineData("/result/error")]
    [InlineData("/typed/result/error")]
    public async Task Get_ResultError_ReturnsBadRequest(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        var errors = await resp.Content.ReadAsStringAsync();
        Assert.NotNull(errors);
        Assert.Equal("[{\"key\":\"\",\"errors\":[\"An error occurred.\"]}]", errors);
    }

    [Theory]
    [InlineData("/result/error/custom")]
    [InlineData("/typed/result/error/custom")]
    public async Task Get_ResultErrorCustom_ReturnsOkWithCustomMessage(string url) {
        var resp = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var content = await resp.Content.ReadAsStringAsync();
        Assert.Equal("{\"message\":\"There was a problem\",\"error\":\"An error occurred.\"}", content);
    }
}
