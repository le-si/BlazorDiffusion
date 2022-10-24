using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceStack;
using ServiceStack.Blazor;

namespace BlazorDiffusion;

/// <summary>
/// For Pages and Components that make use of ServiceStack functionality, e.g. Client
/// </summary>
public abstract class AppComponentBase : ServiceStack.Blazor.BlazorComponentBase, IHasJsonApiClient
{
}

/// <summary>
/// For Pages and Components requiring Authentication
/// </summary>
public abstract class AppAuthComponentBase : AuthBlazorComponentBase
{
    public bool IsModerator => IsAuthenticated && User.HasRole(AppRoles.Moderator);
}

public enum AppPage
{
    Search,
    Create,
    Likes,
}