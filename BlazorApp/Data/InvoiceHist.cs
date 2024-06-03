namespace BlazorApp.Data
{
    public class ProductInvoice
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public int qty { get; set; }
        public Decimal uprice { get; set; }
        public Decimal discount { get; set; }
        public Decimal price { get; set; }
    }
    public class InvoiceKey : IEquatable<InvoiceKey>
    {
        public int InvoiceNo { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }

        public bool Equals(InvoiceKey other)
        {
            return other != null &&
                   InvoiceNo == other.InvoiceNo &&
                   UserId == other.UserId &&
                   UserName == other.UserName &&
                   Date == other.Date;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InvoiceNo, UserId, UserName, Date, Time);
        }
    }

    public class InvoiceRet
    {
        public int InvoiceNo { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public int Id { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public Decimal unitprice { get; set; }
        public Decimal discount { get; set; }
        public Decimal price { get; set; }
    }
}
