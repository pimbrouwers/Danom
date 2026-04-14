namespace Danom.MinimalApi.Tests;

using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

internal static class TestExtensions {
    public static async Task<HttpResponse> ToResponse(this IResult httpResult) {
        var services = new ServiceCollection();
        services.AddLogging();
        var responseStream = new MemoryStream();
        var httpContext = new DefaultHttpContext {
            ServiceScopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>(),
            Features =
            {
                [typeof(IHttpResponseBodyFeature)] = new StreamResponseBodyFeature(responseStream),
            },
        };

        await httpResult.ExecuteAsync(httpContext);

        return httpContext.Response;
    }

    public static T? DeserializeBody<T>(this HttpResponse httpResponse) {
        var stream = httpResponse.Body;
        stream.Position = 0;
        return JsonSerializer.Deserialize<T>(stream, JsonSerializerOptions.Web);
    }
}
