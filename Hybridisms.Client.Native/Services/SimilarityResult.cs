namespace Hybridisms.Client.Native.Services;

public record struct SimilarityResult<T>(T Match, float Similarity)
{
    public static implicit operator (T Match, float Similarity)(SimilarityResult<T> value)
    {
        return (value.Match, value.Similarity);
    }

    public static implicit operator SimilarityResult<T>((T Match, float Similarity) value)
    {
        return new SimilarityResult<T>(value.Match, value.Similarity);
    }
}
