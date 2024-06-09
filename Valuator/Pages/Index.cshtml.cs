using System.Globalization;
using MessageBus.Interfaces.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Domain.ValueObjects;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;
using Valuator.Repositories.Interfaces;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ITextRepository _textRepository;
    private readonly ISimilarityRepository _similarityRepository;

    public IndexModel( ILogger<IndexModel> logger, IMessagePublisher messagePublisher, ITextRepository textRepository,
        ISimilarityRepository similarityRepository )
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
        _textRepository = textRepository;
        _similarityRepository = similarityRepository;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost( string text )
    {
        _logger.LogDebug( text );

        var textId = TextId.New();
        _textRepository.Add( textId, text );
        
        _messagePublisher.Publish( Messages.CalculateRankRequest, textId.ToString() );

        int similarity = CalculateSimilarity( textId, text );
        _similarityRepository.Add(  new SimilarityId( textId ), similarity.ToString( CultureInfo.InvariantCulture ) );
        _messagePublisher.Publish( Messages.SimilarityCalculatedNotification, new SimilarityCalculatedNotificationDto
        {
            Similarity = similarity,
            TextId = textId.ToString()
        } );

        return Redirect( $"summary?id={textId.Value}" );
    }

    private int CalculateSimilarity( TextId textToCalculateId, string textToCalculate )
    {
        List<TextId> texts = _textRepository.GetAllTexts();

        bool hasSameText = texts.Any( textId =>
        {
            if ( textId.Equals( textToCalculateId ) )
            {
                return false;
            }
            
            string text = _textRepository.Get( textId )!;
            return text.Equals( textToCalculate, StringComparison.InvariantCultureIgnoreCase );
        } );

        return hasSameText ? 1 : 0;
    }
}