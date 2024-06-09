using System.Diagnostics;
using System.Globalization;
using MessageBus.Interfaces.Messages;
using Microsoft.Extensions.Logging;
using Valuator.Domain.Regions;
using Valuator.Domain.ValueObjects;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;
using Valuator.Repositories.Interfaces;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.RankCalculator.Consumers;

public class CalculateRankMessageConsumer : IMessageConsumer
{
    public static MessageId MessageId => Messages.CalculateRankRequest;

    private readonly ILogger _logger;

    private readonly IMessagePublisher _messagePublisher;

    private readonly IRegionSearcher _regionSearcher;
    private readonly IShardedRepositoryCreator<ITextRepository> _textRepositoryCreator;
    private readonly IShardedRepositoryCreator<IRankRepository> _rankRepositoryCreator;

    public CalculateRankMessageConsumer(
        ILogger<CalculateRankMessageConsumer> logger,
        IMessagePublisher messagePublisher,
        IShardedRepositoryCreator<ITextRepository> textRepositoryCreator,
        IShardedRepositoryCreator<IRankRepository> rankRepositoryCreator,
        IRegionSearcher regionSearcher )
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
        _textRepositoryCreator = textRepositoryCreator;
        _rankRepositoryCreator = rankRepositoryCreator;
        _regionSearcher = regionSearcher;
    }

    public void Consume( string messageContent )
    {
        _logger.LogInformation( $"Consuming message. Consumer: {nameof( CalculateRankMessageConsumer )}, Message: {messageContent}" );

        var textId = new TextId( messageContent );
        Region region = GetTextRegion( textId );
        _logger.LogInformation( $"LOOKUP: {textId}, {region}" );

        string? text = _textRepositoryCreator.Create( region ).Get( textId );
        if ( text is null )
        {
            return;
        }

        double rank = CalculateRank( text );
        _rankRepositoryCreator
            .Create( region )
            .Add( new RankId( textId ), rank.ToString( CultureInfo.InvariantCulture ) );
        
        _messagePublisher.Publish( Messages.RankCalculatedNotification, new RankCalculatedNotificationDto
        {
             Rank = rank,
             TextId = textId.ToString()
        } );
    }

    private Region GetTextRegion( TextId textId )
    { 
        Region? region = _regionSearcher.Search( textId );
        if ( region is null )
        {
            throw new UnreachableException( $"Region was not found for text. TextId: {textId}" );
        }

        return region;
    }

    private double CalculateRank( string text )
    {
        int notAlphabetSymbolsNumber = text.Count( symbol => !Char.IsLetter( symbol ) );
        double rank = notAlphabetSymbolsNumber / ( double )text.Length;

        return rank;
    }
}