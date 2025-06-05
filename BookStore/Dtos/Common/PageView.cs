namespace BookStore.Dtos.Common
{
    public class PageView <T>
    {
        public List<T> Items { get; set; }
        public int TotalRecord {  get; set; }

    }
}
