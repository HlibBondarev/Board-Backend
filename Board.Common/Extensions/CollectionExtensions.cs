namespace Board.Common.Extensions;

public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any();
    }

    public static List<TOut> MapToList<TIn, TOut>(this IEnumerable<TIn> list, Func<TIn, TOut> map)
        => [.. list.Select(map)];
}