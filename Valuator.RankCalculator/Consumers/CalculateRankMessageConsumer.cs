using System.Globalization;
using MessageBus.Interfaces.Messages;
using Microsoft.Extensions.Logging;
using Valuator.Caches.ValueObjects;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;
using Valuator.Repositories.Interfaces;

namespace Valuator.RankCalculator.Consumers;

public class CalculateRankMessageConsumer : IMessageConsumer
{
    private readonly ILogger _logger;
    private readonly ITextRepository _textRepository;
    private readonly IRankRepository _rankRepository;
    private readonly IMessagePublisher _messagePublisher;

    public static MessageId MessageId => Messages.CalculateRankRequest;

    public CalculateRankMessageConsumer(
        ILogger<CalculateRankMessageConsumer> logger,
        IMessagePublisher messagePublisher,
        ITextRepository textRepository,
        IRankRepository rankRepository )
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
        _textRepository = textRepository;
        _rankRepository = rankRepository;
    }

    public void Consume( string messageContent )
    {
        _logger.LogInformation( $"Consuming message. Consumer: {nameof( CalculateRankMessageConsumer )}, Message: {messageContent}" );

        var textId = new TextId( messageContent );

        string? text = _textRepository.Get( textId );
        if ( text is null )
        {
            return;
        }

        double rank = CalculateRank( text );
        _rankRepository.Add(
            new RankId( textId ),
            rank.ToString( CultureInfo.InvariantCulture ) );
        
        _messagePublisher.Publish( Messages.RankCalculatedNotification, new RankCalculatedNotificationDto
        {
             Rank = rank,
             TextId = textId.ToString()
        } );
    }

    private double CalculateRank( string text )
    {
        int notAlphabetSymbolsNumber = text.Count( symbol => !Char.IsLetter( symbol ) );
        double rank = notAlphabetSymbolsNumber / ( double )text.Length;

        return rank;
    }
}