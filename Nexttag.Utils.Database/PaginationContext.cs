namespace Nexttag.Utils.Database
{
    public class PaginationContext
    {
        public PaginationContext(uint? size, uint? page, string query)
        {
            Size = size;
            Page = page;
            Query = query;
        }

        public uint? Size { get; private set; }
        public uint? Page { get; private set; }
        public string Query { get; internal set; }
        public uint Total { get; internal set; }
    }
}