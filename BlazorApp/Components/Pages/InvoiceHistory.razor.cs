using Microsoft.AspNetCore.Components;
using BlazorApp.Data;
namespace BlazorApp.Components.Pages
{
    public partial class InvoiceHistory
    {
        [Parameter]
        public string? custID { get; set; }
        public Cust customer = new Cust();
        public List<Cust>? temp;
        public List<InvoiceRet>? invoices;
        Dictionary<InvoiceKey, List<ProductInvoice>>? invoice_dict;
        private void NavigateToRoute()
        {
            NavigationManager.NavigateTo("/customers");
        }
        protected override async Task OnInitializedAsync()
        {
            temp = await httpClient.GetFromJsonAsync<List<Cust>>($"http://localhost:5038/list_cust/{custID}/all/all/all/all");
            if (temp != null)
            {
                if (temp.Count() > 0)
                {
                    customer = temp[0];
                }
            }
            invoices = await httpClient.GetFromJsonAsync<List<InvoiceRet>>($"http://localhost:5038/get_invoices/{custID}/");
            invoice_dict = new Dictionary<InvoiceKey, List<ProductInvoice>>();
            foreach(InvoiceRet ret in invoices)
            {
                Console.WriteLine(ret);
                InvoiceKey temp = new InvoiceKey();
                temp.InvoiceNo = ret.InvoiceNo; temp.UserId = ret.UserId; temp.UserName = ret.UserName; temp.Date = ret.Date; temp.Time = ret.Time;
                ProductInvoice prod = new ProductInvoice();
                prod.Id = ret.Id; prod.name = ret.name; prod.qty = ret.qty; prod.discount = ret.discount; prod.price = ret.price;
                if (invoice_dict.ContainsKey(temp)) {
                    invoice_dict[temp].Add(prod);
                }
                else
                {
                    invoice_dict[temp] = new List<ProductInvoice>();
                    invoice_dict[temp].Add(prod);
                }
            }
        }
    }
}