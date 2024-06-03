namespace BlazorApp.Data
{
    public class Product_FIN
    {
        public ulong Id { get; set; }
        public string? name { get; set; }
        public string? desc { get; set; }
        public Decimal unitprice { get; set; }
        public int qty { get; set; }
        public Decimal discount { get; set; }
    }

    public class InvoiceRecord
    {
        public ulong Id { get; set; }
        public string? name { get; set; }
        public Decimal uprice { get; set; }
        public int qty { get; set; }
        public Decimal discount { get; set; }
        public Decimal price { get; set; }
    }

    public class InvoiceWrapped
    {
        public ulong custID { get; set; }
        public string? custName { get; set; }
        public string? phone { get; set; }
        public ulong userID { get; set; }
        public string? UserName { get; set; }
        public List<InvoiceRecord> invoice_list { get; set; }
    }
}
