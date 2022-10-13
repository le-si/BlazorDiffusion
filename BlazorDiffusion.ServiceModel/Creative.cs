﻿using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace BlazorDiffusion.ServiceModel;

public class Creative
{
    [AutoIncrement]
    public int Id { get; set; }

    public string UserPrompt { get; set; }
    public string Prompt { get; set; }

    public string? ImageBasisPath { get; set; }

    public int Images { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Steps { get; set; }

    public int? PrimaryArtifactId { get; set; }

    List<string> ArtistNames { get; set; }
    List<string> ModifierNames { get; set; }

    [Reference]
    public List<CreativeArtist> Artists { get; set; }
    [Reference]
    public List<CreativeModifier> Modifiers { get; set; }

    [Reference]
    [Format("presentFilesPreview")]
    public List<CreativeArtifact> Artifacts { get; set; }
}

public class CreativeArtifact
{
    [AutoIncrement]
    public int Id { get; set; }

    [References(typeof(Creative))]
    public int CreativeId { get; set; }

    public string FileName { get; set; }

    [Format(FormatMethods.Attachment)]
    public string FilePath { get; set; }
    public string ContentType { get; set; }

    [Format(FormatMethods.Bytes)]
    public long ContentLength { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    public ulong Seed { get; set; }
    public string Prompt { get; set; }
    public int? HighResArtifactId { get; set; }
}

public class QueryCreative : QueryDb<Creative>
{
    public int? Id { get; set; }
    public int? CreativeId { get; set; }
}

public class CreateCreative : ICreateDb<Creative>, IReturn<Creative>
{
    public int CreativeId { get; set; }

    [Required]
    public string UserPrompt { get; set; }
    public string Prompt { get; set; }
    public int? ArtistId { get; set; }
    
    [AutoDefault(Value = 4)]
    public int? Images { get; set; }
    
    [AutoDefault(Value = 512)]
    public int? Width { get; set; }
    
    [AutoDefault(Value = 512)]
    public int? Height { get; set; }
    
    [AutoDefault(Value = 50)]
    public int? Steps { get; set; }
    public long? Seed { get; set; }
    List<int> ArtistIds { get; set; }
    List<int> ModifierIds { get; set; }
}

public class DeleteCreative : IDeleteDb<Creative>, IReturnVoid
{
    public int Id { get; set; }
}

public class QueryCreativeArtifacts : QueryDb<CreativeArtifact>
{
    
}


public class QueryArtists : QueryDb<Artist> {}

public class CreateArtist : ICreateDb<Artist>, IReturn<Artist>
{
    public string? FirstName { get; set; }
    [ValidateNotEmpty, Required]
    public string LastName { get; set; }
    public int? YearDied { get; set; }
    public List<string>? Type { get; set; }
}

public class UpdateArtist : IPatchDb<Artist>, IReturn<Artist>
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? YearDied { get; set; }
    public List<string>? Type { get; set; }
}
public class DeleteArtist : IDeleteDb<Artist>, IReturnVoid 
{
    public int Id { get; set; }
}

public class Artist
{
    [AutoIncrement]
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string LastName { get; set; }
    public int? YearDied { get; set; }
    public List<string>? Type { get; set; }
}


public class QueryModifiers : QueryDb<Modifier> { }

public class CreateModifier : ICreateDb<Modifier>, IReturn<Modifier>
{
    [ValidateNotEmpty, Required]
    public string Name { get; set; }
    [ValidateNotEmpty, Required]
    public string Category { get; set; }
    public string? Description { get; set; }
}

public class UpdateModifier : ICreateDb<Modifier>, IReturn<Modifier>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
}

public class DeleteModifier : IDeleteDb<Modifier>, IReturnVoid
{
    public int Id { get; set; }
}


public class Modifier
{
    [AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string? Description { get; set; }
}


public class QueryCreativeArtists : QueryDb<CreativeArtist>
{
    public int? CreativeTaskId { get; set; }
    public int? ModifierId { get; set; }
}

public class CreateCreativeArtist : ICreateDb<CreativeArtist>, IReturn<CreativeArtist>
{
    [ValidateGreaterThan(0)]
    public int? CreativeTaskId { get; set; }
    [ValidateGreaterThan(0)]
    public int? ModifierId { get; set; }
}

public class DeleteCreativeArtist : IDeleteDb<CreativeArtist>, IReturnVoid
{
    public int? Id { get; set; }
    public int[]? Ids { get; set; }
}
public class CreativeArtist
{
    [AutoIncrement]
    public int Id { get; set; }
    [References(typeof(Creative))]
    public int CreativeTaskId { get; set; }
    [References(typeof(Artist))]
    public int ArtistId { get; set; }
}


public class QueryCreativeModifiers : QueryDb<CreativeModifier> 
{
    public int? CreativeTaskId { get; set; }
    public int? ModifierId { get; set; }
}

public class CreateCreativeModifier : ICreateDb<CreativeModifier>, IReturn<CreativeModifier>
{
    [ValidateGreaterThan(0)]
    public int? CreativeTaskId { get; set; }
    [ValidateGreaterThan(0)]
    public int? ModifierId { get; set; }
}

public class DeleteCreativeModifier : IDeleteDb<CreativeModifier>, IReturnVoid
{
    public int? Id { get; set; }
    public int[]? Ids { get; set; }
}

public class CreativeModifier
{
    [AutoIncrement]
    public int Id { get; set; }
    [References(typeof(Creative))]
    public int CreativeTaskId { get; set; }
    [References(typeof(Modifier))]
    public int ModifierId { get; set; }
}
