using BlazorApp.Data;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace BlazorApp.Components.Pages
{
    public partial class GenerateInvoice
    {
        private void GetUserContext()
        {
            string filename = "C:/C#.NET/InventoryManagementApp/BlazorApp/Data/UserStatus.json";
            string jsonstring = File.ReadAllText(filename);
            user_context = JsonSerializer.Deserialize<UserContext>(jsonstring);
        }
        private bool editmode = true;
        private Cust curr_customer = new Cust();
        private Decimal TotalPrice = 0;
        private ulong searchID = 0, selectedProductId, selectedCustID = 1;
        private List<ulong> pid_list = new List<ulong>();
        private List<string> cname_list = new List<string>();
        private string? searchName = "all";
        private Product_FIN[]? products;
        private Cust[]? customers;
        private List<InvoiceRecord> invoice_list = new List<InvoiceRecord>();
        private void NavigateToRoute()
        {
            NavigationManager.NavigateTo("/payment");
        }
        protected override async Task OnInitializedAsync()
        {
            GetUserContext();
            invoice_list.Clear();
            editmode = true;
            await ListProd();
            await ListCust();
        }

        private async Task ListCust()
        {
            cname_list.Clear();
            customers = await httpClient.GetFromJsonAsync<Cust[]>($"http://localhost:5038/list_cust/0/all/all/all/all");
            if (string.IsNullOrEmpty(curr_customer.name))
            {
                Console.WriteLine("Selecting customer 1 by defualt");
            }
            curr_customer = customers[0];
            Console.WriteLine($"Current Customer Name : {curr_customer.name}");
            if (customers != null)
            {
                foreach (Cust cust in customers)
                {
                    if (cust.name != null)
                    {
                        cname_list.Add(cust.name);
                    }
                }
            }
        }

        private void DecreaseQuantity(InvoiceRecord record)
        {
            if (record.qty > 1)
            {
                record.qty--;
                // Recalculate the price or any other related logic
                record.price = record.price - (record.uprice - (record.discount / 100 * record.uprice));
                TotalPrice = TotalPrice - (record.uprice - (record.discount / 100 * record.uprice));
            }
            else
            {
                pid_list.Add(record.Id);
                pid_list.Sort();
                TotalPrice = TotalPrice - (record.uprice - (record.discount / 100 * record.uprice));
                invoice_list.Remove(record);
            }
        }
        private async void Save_Invoice()
        {
            InvoiceWrapped wi = new InvoiceWrapped();
            wi.custID = curr_customer.Id;
            wi.custName = curr_customer.name;
            wi.phone = curr_customer.phone;
            Console.WriteLine($"IN");
            wi.userID = await httpClient.GetFromJsonAsync<ulong>($"http://localhost:5038/get_user/{user_context.UserName}");
            wi.UserName = user_context.UserName;
            Console.WriteLine($"{wi.userID} : {wi.UserName}");
            wi.invoice_list = new List<InvoiceRecord>();
            Console.WriteLine($"{wi.custID} {wi.custName}, {wi.phone}");
            Console.WriteLine($"{wi.userID} {wi.UserName}");
            foreach (InvoiceRecord rec in invoice_list)
            {
                Console.WriteLine($"{rec.Id} {rec.name} {rec.uprice} {rec.qty} {rec.discount} {rec.price}");
                wi.invoice_list.Add(rec);
            }

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("http://localhost:5038/save_invoice", wi);
            Console.WriteLine(response);
            NavigateToRoute();
        }

        private void EditSet_Invoice()
        {
            editmode = true;
            // Logic to generate the invoice
            // This could involve sending data to a backend server, processing it, and then potentially displaying the generated invoice to the user.
        }

        private void Generate_Invoice()
        {
            if (!string.IsNullOrEmpty(curr_customer.name) && invoice_list.Count() > 0)
            {
                editmode = false;
            }// Logic to generate the invoice
             // This could involve sending data to a backend server, processing it, and then potentially displaying the generated invoice to the user.
        }
        private void IncreaseQuantity(InvoiceRecord record)
        {
            record.qty++;
            // Recalculate the price or any other related logic
            record.price = record.price + record.uprice - (record.discount / 100 * record.uprice);
            TotalPrice = TotalPrice + (record.uprice - (record.discount / 100 * record.uprice));
        }

        private void HandleSelectionCust(ChangeEventArgs e)
        {
            string name = (string)e.Value;
            foreach (Cust cust in customers)
            {
                if (cust.name.Equals(name))
                {
                    curr_customer = cust;
                    Console.WriteLine($"{curr_customer.Id} : {curr_customer.name} {curr_customer.phone} {curr_customer.mail} {curr_customer.address}");
                }
            }
        }
        private void HandleSelection(ChangeEventArgs e)
        {
            selectedProductId = Convert.ToUInt64(e.Value);
            if (products != null)
            {
                foreach (Product_FIN prod in products)
                {
                    if (prod.Id == selectedProductId)
                    {
                        InvoiceRecord record = new InvoiceRecord();
                        record.Id = prod.Id;
                        record.name = prod.name;
                        record.uprice = prod.unitprice;
                        record.qty = 1;
                        record.discount = prod.discount;
                        record.price = prod.unitprice - (prod.discount / 100 * prod.unitprice);
                        invoice_list.Add(record);
                        TotalPrice = TotalPrice + record.price;
                        pid_list.Remove(selectedProductId);
                    }
                }
            }
        }
        private async Task ListProd()
        {
            pid_list.Clear();
            products = await httpClient.GetFromJsonAsync<Product_FIN[]>($"http://localhost:5038/list_prods/{searchID}/{searchName}");
            if (products != null)
            {
                foreach (Product_FIN prod in products)
                {
                    pid_list.Add(prod.Id);
                    Console.WriteLine($"{prod.Id} : {prod.name} {prod.unitprice} {prod.qty} {prod.discount}");
                }
            }
        }
    }
}