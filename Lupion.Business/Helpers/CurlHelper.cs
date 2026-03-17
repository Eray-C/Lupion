using Microsoft.AspNetCore.Http;
using System.Text;

namespace Lupion.Business.Helpers;
public static class CurlHelper
{
    public static string GenerateCurl(HttpRequest request, string? requestBody)
    {
        var curl = new StringBuilder("curl");

        curl.Append($" -X {request.Method}");

        foreach (var header in request.Headers)
        {
            var safeHeaderValue = header.Value.ToString().Replace("\"", "\\\"");
            curl.Append($" -H \"{header.Key}: {safeHeaderValue}\"");
        }

        if (!string.IsNullOrWhiteSpace(requestBody) && !string.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase))
        {
            var safeBody = requestBody.Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
            curl.Append($" -d \"{safeBody}\"");
        }

        var urlBuilder = new StringBuilder();
        urlBuilder.Append($"{request.Scheme}://{request.Host}{request.Path}");
        if (request.QueryString.HasValue)
        {
            urlBuilder.Append(request.QueryString.Value);
        }

        curl.Append($" \"{urlBuilder}\"");

        return curl.ToString();
    }
}