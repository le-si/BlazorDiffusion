﻿using BlazorDiffusion.ServiceModel;
using ServiceStack.Blazor;

namespace BlazorDiffusion;

public class UserState
{
    public CachedLocalStorage LocalStorage { get; }
    public JsonApiClient Client { get; }

    ApiResult<QueryResponse<Creative>> apiHistory = new();

    public HashSet<int> LikedArtifactIds { get; private set; } = new();

    public List<Creative> CreativeHistory => apiHistory.Response?.Results ?? new();

    public Dictionary<int, Artifact> ArtifactsMap { get; } = new();

    public Dictionary<int, Creative> CreativesMap { get; } = new();

    public List<Artifact> LikedArtifacts => LikedArtifactIds.Select(x => ArtifactsMap.TryGetValue(x, out var a) ? a : null)
        .Where(x => x != null).Cast<Artifact>().ToList();

    public UserState(CachedLocalStorage localStorage, JsonApiClient client)
    {
        LocalStorage = localStorage;
        Client = client;
    }

    public async Task LoadAsync(int userId)
    {
        apiHistory = await Client.ApiAsync(new QueryCreatives
        {
            OwnerId = userId,
            Take = 30,
            OrderByDesc = nameof(Creative.Id),
        });
        await LoadLikesAsync(userId);
    }

    public async Task LoadLikesAsync(int userId)
    {
        var apiLikes = await Client.ApiAsync(new QueryArtifactLikes());
        if (apiLikes.Succeeded)
        {
            LikedArtifactIds = apiLikes.Response!.Results.Select(x => x.ArtifactId).ToSet();
        }

        var missingIds = new List<int>();
        foreach (var artifactId in LikedArtifactIds)
        {
            if (GetCachedArtifact(artifactId) == null)
                missingIds.Add(artifactId);
        }
        if (missingIds.Count > 0)
        {
            var api = await Client.ApiAsync(new QueryArtifacts { Ids = missingIds });
            if (api.Response?.Results != null) LoadArtifacts(api.Response.Results);
        }

        NotifyStateChanged();
    }

    public void LoadCreatives(IEnumerable<Creative> creatives) => creatives.Each(LoadCreative);
    public void LoadCreative(Creative creative)
    {
        CreativesMap[creative.Id] = creative;
        foreach (var artifact in creative.Artifacts.OrEmpty())
        {
            ArtifactsMap[artifact.Id] = artifact;
        }
    }

    public void LoadArtifacts(IEnumerable<Artifact> artifacts) => artifacts.Each(LoadArtifact);
    public void LoadArtifact(Artifact artifact) => ArtifactsMap[artifact.Id] = artifact;

    public Artifact? GetCachedArtifact(int? id) => id != null
        ? ArtifactsMap.TryGetValue(id.Value, out var a) ? a : null
        : null;

    public Creative? GetCachedCreative(int? id) => id != null
        ? CreativesMap.TryGetValue(id.Value, out var a) ? a : null
        : null;

    public async Task<Creative?> GetCreativeAsync(int? creativeId)
    {
        var creative = GetCachedCreative(creativeId);
        if (creativeId == null || creative != null)
            return creative;

        var api = await Client.ApiAsync(new QueryCreatives { Id = creativeId });
        if (api.Succeeded && api.Response?.Results != null)
        {
            LoadCreatives(api.Response.Results);
        }
        return GetCachedCreative(creativeId);
    }

    public async Task<Artifact?> GetArtifactAsync(int? artifactId)
    {
        var artifact = GetCachedArtifact(artifactId);
        if (artifactId == null || artifact != null)
            return artifact;

        var api = await Client.ApiAsync(new QueryArtifacts { Id = artifactId });
        if (api.Succeeded && api.Response?.Results != null)
        {
            LoadArtifacts(api.Response.Results);
        }
        return GetCachedArtifact(artifactId);
    }


    public bool HasLiked(Artifact artifact) => LikedArtifactIds.Contains(artifact.Id);

    public async Task LikeArtifactAsync(Artifact artifact)
    {
        ArtifactsMap[artifact.Id] = artifact;
        LikedArtifactIds.Add(artifact.Id);
        var api = await Client.ApiAsync(new CreateArtifactLike
        {
            ArtifactId = artifact.Id,
        });
        if (!api.Succeeded)
        {
            LikedArtifactIds.Remove(artifact.Id);
        }
        NotifyStateChanged();
    }

    public async Task UnlikeArtifactAsync(Artifact artifact)
    {
        ArtifactsMap[artifact.Id] = artifact;
        LikedArtifactIds.Remove(artifact.Id);
        var api = await Client.ApiAsync(new DeleteArtifactLike
        {
            ArtifactId = artifact.Id,
        });
        if (!api.Succeeded)
        {
            LikedArtifactIds.Add(artifact.Id);
        }
        NotifyStateChanged();
    }


    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
}
