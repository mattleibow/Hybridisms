using System.Runtime.CompilerServices;
using Hybridisms.Client.Shared.Services;

namespace Hybridisms.Server.WebApp.Services;

public class DbLabelRecommendationService(DbNotesService db) : ILabelRecommendationService
{
    // Dummy implementation: returns 3 random labels from the database
    public async IAsyncEnumerable<Label> RecommendLabelsAsync(Note note, int count = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var allLabels = new List<Label>();
        await foreach (var label in db.GetLabelsAsync(cancellationToken))
        {
            allLabels.Add(label);
        }

        if (allLabels.Count == 0)
        {
            yield break; // No labels available for recommendation
        }

        // Randomly select 3 labels
        var randomLabels = allLabels.OrderBy(_ => Guid.NewGuid()).Take(count);

        foreach (var label in randomLabels)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return label;
        }
    }
}
