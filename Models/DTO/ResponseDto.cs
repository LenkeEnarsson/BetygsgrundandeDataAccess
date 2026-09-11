namespace Models.DTO
{
    public record ResponsePageDto<T>
    {
        public List<T> PageItems { get; init; }
        public int DbItemsCount { get; init; }
        public int PageNr { get; init; }
        public int PageSize { get; init; }
        public int PageCount => (int)Math.Ceiling((double) DbItemsCount / PageSize);
    }
    public record ResponseItemDto<T>
    {
#if DEBUG
        //Only used in debug mode to show the connection string
        public string ConnectionString { get; init; }
#endif

        public T Item { get; init; }
    }
}
