namespace BlazorApp.Data
{
    public class Product
    {
        public ulong Id { get; set; }
        public string? name { get; set; }
        public string? desc { get; set; }
        public Decimal unitprice { get; set; }
        public int qty { get; set; }
        public Decimal discount { get; set; }
    }
}
