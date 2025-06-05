namespace BookStore.Dtos.Common
{
    public class GetPagingRequest
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? SearchText { get; set; }
    }
}
