using System.Globalization;
using MessageBus.Interfaces.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Domain.Countries;
using Valuator.Domain.Regions;
using Valuator.Domain.ValueObjects;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;
using Valuator.Repositories.Interfaces;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IShardedRepositoryCreator<ITextRepository> _textRepositoryCreator;
    private readonly IShardedRepositoryCreator<ISimilarityRepository> _similarityRepositoryCreator;

    public List<Country> Countries = Country.GetAllCountries();
    
    public IndexModel(
        ILogger<IndexModel> logger, 
        IMessagePublisher messagePublisher,
        IShardedRepositoryCreator<ITextRepository> textRepositoryCreator,
        IShardedRepositoryCreator<ISimilarityRepository> similarityRepositoryCreator )
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
        _textRepositoryCreator = textRepositoryCreator;
        _similarityRepositoryCreator = similarityRepositoryCreator;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost( string text, int countryIndex )
    {
        _logger.LogDebug( text );

        // Commit text
        var textId = TextId.New();
        Region region = Countries[countryIndex].ToRegion();
        ITextRepository textRepository = _textRepositoryCreator.Create( region );
        textRepository.Add( textId, text );

        _logger.LogInformation( $"LOOKUP: {textId}, {region}" );
        
        // Process rank
        _messagePublisher.Publish( Messages.CalculateRankRequest, textId.ToString() );
        
        // Process similarity
        int similarity = CalculateSimilarity( textId, text, textRepository );
        _similarityRepositoryCreator
            .Create( region )
            .Add( new SimilarityId( textId ), similarity.ToString( CultureInfo.InvariantCulture ) );
        _messagePublisher.Publish(
            Messages.SimilarityCalculatedNotification, 
            new SimilarityCalculatedNotificationDto
            {
                Similarity = similarity,
                TextId = textId.ToString()
            } );

        return Redirect( $"summary?id={textId.Value}" );
    }

    private int CalculateSimilarity( 
        TextId textToCalculateId,
        string textToCalculate,
        ITextRepository textRepository )
    {
        List<TextId> texts = textRepository.GetAllTexts();

        bool hasSameText = texts.Any( textId =>
        {
            if ( textId.Equals( textToCalculateId ) )
            {
                return false;
            }
            
            string text = textRepository.Get( textId )!;
            return text.Equals( textToCalculate, StringComparison.InvariantCultureIgnoreCase );
        } );

        return hasSameText ? 1 : 0;
    }
}