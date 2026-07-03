using Microsoft.AspNetCore.WebUtilities;
using ZohoDesk.Authentication;
using ZohoDesk.Constants;

namespace ZohoDesk.Infrastructure;

/// <summary>
/// Фабрика для создания HTTP-запросов.
/// </summary>
public static class HttpRequestFactory
{
    /// <summary>
    /// Создает OAuth-запрос на получение Access Token.
    /// </summary>
    public static HttpRequestMessage CreateRefreshTokenRequest(
        string baseAddress,
        RefreshAccessTokenRequest request)
    {
        var url = QueryHelpers.AddQueryString(
            $"{baseAddress.TrimEnd('/')}{ZohoEndpoints.OAuthToken}",
            request.ToQueryParameters()!);

        return new HttpRequestMessage(
            HttpMethod.Post,
            url);
    }

    /// <summary>
    /// Создает GET-запрос.
    /// </summary>
    public static HttpRequestMessage CreateGet(
        string url)
    {
        return new HttpRequestMessage(
            HttpMethod.Get,
            url);
    }

    /// <summary>
    /// Создает POST-запрос.
    /// </summary>
    public static HttpRequestMessage CreatePost(
        string url,
        HttpContent content)
    {
        return new HttpRequestMessage(
            HttpMethod.Post,
            url)
        {
            Content = content
        };
    }

    /// <summary>
    /// Создает PATCH-запрос.
    /// </summary>
    public static HttpRequestMessage CreatePatch(
        string url,
        HttpContent content)
    {
        return new HttpRequestMessage(
            HttpMethod.Patch,
            url)
        {
            Content = content
        };
    }

    /// <summary>
    /// Создает DELETE-запрос.
    /// </summary>
    public static HttpRequestMessage CreateDelete(
        string url)
    {
        return new HttpRequestMessage(
            HttpMethod.Delete,
            url);
    }
}