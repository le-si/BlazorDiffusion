using Microsoft.AspNetCore.Components.Authorization;
using ServiceStack;
using ServiceStack.Blazor;

namespace BlazorDiffusion;

/// <summary>
/// Manages App Authentication State
/// </summary>
public class ServiceStackStateProvider : ServiceStackAuthenticationStateProvider
{
    public ServiceStackStateProvider(JsonApiClient client, ILogger<ServiceStackAuthenticationStateProvider> log)
        : base(client, log) { }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        Log.LogDebug("GetAuthenticationStateAsync()");
        return base.GetAuthenticationStateAsync();
    }

    public override Task<ApiResult<AuthenticateResponse>> LoginAsync(string email, string password)
    {
        Log.LogDebug("LoginAsync({0},password)", email);
        return base.LoginAsync(email, password);
    }

    public override Task<ApiResult<AuthenticateResponse>> LogoutAsync()
    {
        Log.LogDebug("LogoutAsync()");
        return base.LogoutAsync();
    }

    public override Task LogoutIfAuthenticatedAsync()
    {
        Log.LogDebug("LogoutIfAuthenticatedAsync()");
        return base.LogoutIfAuthenticatedAsync();
    }
    
    public override Task<ApiResult<AuthenticateResponse>> SignInAsync(ApiResult<AuthenticateResponse> api)
    {
        Log.LogDebug("SignInAsync(ApiResult<AuthenticateResponse>)");
        return base.SignInAsync(api);
    }

    public override Task<ApiResult<AuthenticateResponse>> SignInAsync(AuthenticateResponse authResponse)
    {
        Log.LogDebug("SignInAsync(AuthenticateResponse)");
        return base.SignInAsync(authResponse);
    }
    public override Task<ApiResult<AuthenticateResponse>> SignInAsync(RegisterResponse registerResponse)
    {
        Log.LogDebug("SignInAsync(RegisterResponse)");
        return base.SignInAsync(registerResponse);
    }

}
