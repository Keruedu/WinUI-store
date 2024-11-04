using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Contracts.Services;
using ShoesShop.Core.Contracts.Services;
using System.Net;

namespace ShoesShop.Helpers;
public class AuthenticationResponseHandler : DelegatingHandler
{
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;


    public AuthenticationResponseHandler(INavigationService navigationService, IAuthenticationService authenticationService)
    {
        _navigationService = navigationService;
        _authenticationService = authenticationService;
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if ((request.RequestUri.ToString().Contains("login")))
        {
            return response;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            await _authenticationService.LogoutAsync();
            _navigationService.Refresh();
        }

        return response;
    }
}
