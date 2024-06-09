using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Domain.Regions;
using Valuator.Domain.Shards;
using Valuator.Domain.ValueObjects;
using Valuator.Repositories.Interfaces;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.Pages;

public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    private readonly IShardMap _shardMap;
    private readonly IShardedRepositoryCreator<IRankRepository> _rankRepositoryCreator;
    private readonly IShardedRepositoryCreator<ISimilarityRepository> _similarityRepositoryCreator;

    public SummaryModel( 
        ILogger<SummaryModel> logger,
        IShardedRepositoryCreator<IRankRepository> rankRepositoryCreator,
        IShardedRepositoryCreator<ISimilarityRepository> similarityRepositoryCreator,
        IShardMap shardMap )
    {
        _logger = logger;
        _rankRepositoryCreator = rankRepositoryCreator;
        _similarityRepositoryCreator = similarityRepositoryCreator;
        _shardMap = shardMap;
    }

    public double Rank { get; set; }
    public double Similarity { get; set; }

    public void OnGet( string id )
    {
        _logger.LogDebug( id );

        TextId textId = new TextId( id );
        Region region = GetTextRegion( textId );

        Rank = ParseDouble( _rankRepositoryCreator
            .Create( region )
            .Get( new RankId( textId ) ) );

        Similarity = ParseDouble( _similarityRepositoryCreator
            .Create( region )
            .Get( new SimilarityId( textId ) ) );
    }

    private Region GetTextRegion( TextId textId )
    { 
        Region? region = _shardMap.Search( new ShardId( textId ) );
        if ( region is null )
        {
            throw new UnreachableException( $"Region was not found for text. TextId: {textId}" );
        }

        return region;
    }

    private double ParseDouble( string? value )
    {
        return Double.TryParse( value, CultureInfo.InvariantCulture, out double result )
            ? result
            : 0;
    }
}