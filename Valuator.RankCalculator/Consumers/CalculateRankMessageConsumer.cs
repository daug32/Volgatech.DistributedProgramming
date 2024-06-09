using System.Diagnostics;
using System.Globalization;
using MessageBus.Interfaces.Messages;
using Microsoft.Extensions.Logging;
using Valuator.Domain.Regions;
using Valuator.Domain.Shards;
using Valuator.Domain.ValueObjects;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;
using Valuator.Repositories.Interfaces;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.RankCalculator.Consumers;

public class CalculateRankMessageConsumer(
    ILogger<CalculateRankMessageConsumer> logger,
    IMessagePublisher messagePublisher,
    IShardedRepositoryCreator<ITextRepository> textRepositoryCreator,
    IShardedRepositoryCreator<IRankRepository> rankRepositoryCreator,
    IShardMap shardMap )
    : IMessageConsumer
{
    public static MessageId MessageId => Messages.CalculateRankRequest;

    public void Consume( string messageContent )
    {
        logger.LogInformation( $"Consuming message. Consumer: {nameof( CalculateRankMessageConsumer )}, Message: {messageContent}" );

        var textId = new TextId( messageContent );
        Region region = GetTextRegion( textId );
        logger.LogInformation( $"LOOKUP: {textId}, {region}" );

        string? text = textRepositoryCreator.Create( region ).Get( textId );
        if ( text is null )
        {
            return;
        }

        double rank = CalculateRank( text );
        rankRepositoryCreator
            .Create( region )
            .Add( new RankId( textId ), rank.ToString( CultureInfo.InvariantCulture ) );
        
        messagePublisher.Publish( Messages.RankCalculatedNotification, new RankCalculatedNotificationDto
        {
             Rank = rank,
             TextId = textId.ToString()
        } );
    }

    private Region GetTextRegion( TextId textId )
    { 
        Region? region = shardMap.Search( new ShardId( textId ) );
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