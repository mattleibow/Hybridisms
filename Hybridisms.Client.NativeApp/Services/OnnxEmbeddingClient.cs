using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace Hybridisms.Client.NativeApp.Services;

public class OnnxEmbeddingClient(IOptions<OnnxEmbeddingClient.EmbeddingClientOptions> options) : OnnxModelClient<OnnxEmbeddingClient.EmbeddingClientOptions>(options)
{
    private BertTokenizer? tokenizer;
    private InferenceSession? embeddingSession;

    private async Task<(InferenceSession, BertTokenizer)> LoadSessionAsync()
    {
        if (embeddingSession is not null && tokenizer is not null)
            return (embeddingSession, tokenizer);

        await EnsureModelExtractedAsync();

        if (embeddingSession is not null && tokenizer is not null)
            return (embeddingSession, tokenizer);

        var vocabPath = Path.Combine(options.Value.ExtractedPath, "vocab.txt");
        tokenizer = BertTokenizer.Create(vocabPath);

        var modelPath = Path.Combine(options.Value.ExtractedPath, "model.onnx");
        embeddingSession = new InferenceSession(modelPath);

        return (embeddingSession, tokenizer);
    }

    public async IAsyncEnumerable<(T Match, float Similarity)> GetRankedMatchesAsync<T>(string text, IEnumerable<T> options, Func<T, string> optionTextSelector, int count = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(text))
            yield break;

        var topicEmbeddings = new List<(T Option, Tensor<float> Embedding)>();
        foreach (var option in options)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var optionText = optionTextSelector(option);
            if (string.IsNullOrWhiteSpace(optionText))
                continue;

            var embedding = await GetEmbeddingAsync(optionText);
            topicEmbeddings.Add((option, embedding));
        }

        if (topicEmbeddings.Count == 0)
            yield break;

        cancellationToken.ThrowIfCancellationRequested();

        var noteEmbedding = await GetEmbeddingAsync(text);

        var ranked = topicEmbeddings
            .Select(te => (Match: te.Option, Similarity: CosineSimilarity(noteEmbedding, te.Embedding)))
            .OrderByDescending(x => x.Similarity)
            .Take(count)
            .ToList();

        foreach (var item in ranked)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return item;
        }
    }

    private async Task<Tensor<float>> GetEmbeddingAsync(string text)
    {
        var (session, tokenizer) = await LoadSessionAsync();

        var inputIds = tokenizer.EncodeToIds(text);
        var inputIdsLong = inputIds.Select(i => (long)i).ToArray();
        var inputIdsTensor = new DenseTensor<long>(inputIdsLong, [1, inputIds.Count]);

        var typeIds = tokenizer.CreateTokenTypeIdsFromSequences(inputIds);
        var typeIdsLong = typeIds.Take(inputIds.Count).Select(i => (long)i).ToArray();
        var typeIdsTensor = new DenseTensor<long>(typeIdsLong, [1, inputIds.Count]);

        var attentionMask = inputIds.Select(id => id == 0 ? 0L : 1L).ToArray();
        var attentionMaskTensor = new DenseTensor<long>(attentionMask, [1, inputIds.Count]);

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_ids", inputIdsTensor),
            NamedOnnxValue.CreateFromTensor("token_type_ids", typeIdsTensor),
            NamedOnnxValue.CreateFromTensor("attention_mask", attentionMaskTensor)
        };

        using var results = session.Run(inputs);

        return results[0].AsTensor<float>();
    }

    private static float CosineSimilarity(Tensor<float> a, Tensor<float> b)
    {
        var dot = a.Zip(b, (x, y) => x * y).Sum();
        var magA = (float)Math.Sqrt(a.Sum(x => x * x));
        var magB = (float)Math.Sqrt(b.Sum(x => x * x));
        return dot / (magA * magB);
    }

    public class EmbeddingClientOptions : OnnxModelClientOptions
    {
    }
}
