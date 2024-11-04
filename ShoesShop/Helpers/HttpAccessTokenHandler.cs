using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Contracts.Services;
using ShoesShop.Core.Contracts.Services;
namespace ShoesShop.Helpers;
public class AccessTokenHandler : DelegatingHandler
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IStoreServerOriginService _storeServerOriginService;

    public AccessTokenHandler(IAuthenticationService authenticationService, IStoreServerOriginService storeServerOriginService)
    {
        _authenticationService = authenticationService;
        _storeServerOriginService = storeServerOriginService;
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get the access token from the authentication service
        var accessToken = _authenticationService.GetAccessToken();

        var port = _storeServerOriginService.Port;
        var host = _storeServerOriginService.Host;
        var currentUri = request.RequestUri.ToString();
        var apiV1Index = currentUri.IndexOf("api/v1/");
        var apiV1Length = "api/v1/".Length;
        var apiV1 = currentUri.Substring(apiV1Index + apiV1Length);
        request.RequestUri = new Uri(@$"{host}:{port}/api/v1/{apiV1}");

        if (!string.IsNullOrEmpty(accessToken))
        {
            // Add the access token to the request headers
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        // Pass the request to the base handler
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}