namespace TemplateMAUI.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<TSource> Invoke<TSource>(this IEnumerable<TSource> source, Action<TSource> predicate)
        {
            var enumerable = source as IList<TSource> ?? source.ToList();
            foreach (var item in enumerable)
            {
                predicate.Invoke(item);
            }

            return enumerable;
        }
    }
}
