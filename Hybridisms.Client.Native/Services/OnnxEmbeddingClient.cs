using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Hybridisms.Client.Native.Services;

// TODO: AI - [B] Embedded ONNX embedding client
/// <summary>
/// OnnxEmbeddingClient is a client for working with ONNX models for embedding.
/// 
/// It provides invisible support for loading models and tokenizers, and allows for
/// getting ranked matches based on cosine similarity.
/// </summary>
public class OnnxEmbeddingClient(IAppFileProvider fileProvider, IOptions<OnnxEmbeddingClient.EmbeddingClientOptions> options, ILogger<OnnxEmbeddingClient>? logger)
    : OnnxModelClient(fileProvider, options, logger)
{
    private BertTokenizer? tokenizer;
    private InferenceSession? session;

    private async Task<(InferenceSession, BertTokenizer)> LoadSessionAsync()
    {
        if (session is not null && tokenizer is not null)
            return (session, tokenizer);

        await EnsureModelExtractedAsync();

        if (session is not null && tokenizer is not null)
            return (session, tokenizer);

        logger?.LogInformation("Loading ONNX model and tokenizer...");

        var vocabPath = Path.Combine(options.Value.ExtractedPath, "vocab.txt");
        tokenizer = BertTokenizer.Create(vocabPath);

        var modelPath = Path.Combine(options.Value.ExtractedPath, "model.onnx");
        session = new InferenceSession(modelPath);

        logger?.LogInformation("ONNX model and tokenizer loaded successfully.");

        return (session, tokenizer);
    }

    public async Task<ICollection<(T Match, float Similarity)>> GetRankedMatchesAsync<T>(string text, IEnumerable<T> options, int count = 3, CancellationToken cancellationToken = default) =>
        await GetRankedMatchesAsync(text, options, x => x?.ToString(), count, cancellationToken);

    public async Task<ICollection<(T Match, float Similarity)>> GetRankedMatchesAsync<T>(string text, IEnumerable<T> options, Func<T, string?> optionTextSelector, int count = 3, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Ranking matches for text: {Text}", text);

        if (string.IsNullOrWhiteSpace(text))
        {
            logger?.LogWarning("Text is null or empty.");
            return [];
        }

        await LoadSessionAsync();

        var topicEmbeddings = new List<(T Option, float[] Embedding)>();
        foreach (var option in options)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var optionText = optionTextSelector(option);
            if (string.IsNullOrWhiteSpace(optionText))
                continue;

            var embedding = GetEmbedding(optionText);
            topicEmbeddings.Add((option, embedding));
        }

        if (topicEmbeddings.Count == 0)
        {
            logger?.LogWarning("No valid options provided for ranking.");
            return [];
        }

        cancellationToken.ThrowIfCancellationRequested();

        var noteEmbedding = GetEmbedding(text);

        var ranked = topicEmbeddings
            .Select(te => (Match: te.Option, Similarity: CosineSimilarity(noteEmbedding, te.Embedding)))
            .OrderByDescending(x => x.Similarity)
            .ToList();

        var reduced = ranked
            .Take(count)
            .ToList();

        logger?.LogInformation("Selected {Count} ranked matches.", reduced.Count);

        return reduced;
    }

    private float[] GetEmbedding(string text)
    {
        if (session is null || tokenizer is null)
            throw new InvalidOperationException("Session or tokenizer is not initialized.");

        var inputIds = tokenizer.EncodeToIds(text);
        var inputIdsTensor = new DenseTensor<long>(inputIds.Select(i => (long)i).ToArray(), [1, inputIds.Count]);

        var typeIds = tokenizer.CreateTokenTypeIdsFromSequences(inputIds);
        var typeIdsTensor = new DenseTensor<long>(typeIds.Take(inputIds.Count).Select(i => (long)i).ToArray(), [1, inputIds.Count]);

        var attentionMask = inputIds.Select(id => id == 0 ? 0L : 1L).ToArray();
        var attentionMaskTensor = new DenseTensor<long>(attentionMask, [1, inputIds.Count]);

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_ids", inputIdsTensor),
            NamedOnnxValue.CreateFromTensor("token_type_ids", typeIdsTensor),
            NamedOnnxValue.CreateFromTensor("attention_mask", attentionMaskTensor)
        };

        using var results = session.Run(inputs);

        return results[0].AsTensor<float>().ToArray();
    }

    private static float CosineSimilarity(float[] a, float[] b)
    {
        var dotProduct = a.Zip(b, (x, y) => x * y).Sum();
        var magnitudeA = (float)MathF.Sqrt(a.Sum(x => x * x));
        var magnitudeB = (float)MathF.Sqrt(b.Sum(x => x * x));
        return dotProduct / (magnitudeA * magnitudeB);
    }

    public class EmbeddingClientOptions : OnnxModelClientOptions
    {
    }
}
