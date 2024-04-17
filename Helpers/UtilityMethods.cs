namespace ContactHub.Helpers
{
    public class UtilityMethods
    {
        public static IEnumerable<T> Paginate <T>(List<T> source, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 10 ? 10 : pageSize;

            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
