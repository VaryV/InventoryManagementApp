using BlazorApp.Data;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace BlazorApp.Components.Pages
{
    public partial class Customers
    {
        private string? searchPhone, searchName, searchEmail, searchAddress;
        private ulong searchID = 0;
        private List<Cust>? customers;
        private void NavigateToRoute(string link)
        {
            NavigationManager.NavigateTo("/add-customer");
        }
        protected override async Task OnInitializedAsync()
        {
            GetUserContext();
            await ListCust();
        }

        private async Task ListCust()
        {
            string ln, lp, le, la;
            if (string.IsNullOrEmpty(searchName))
                ln = "all";
            else
                ln = searchName;
            if (string.IsNullOrEmpty(searchPhone))
                lp = "all";
            else
                lp = searchPhone;
            if (string.IsNullOrEmpty(searchEmail))
                le = "all";
            else
                le = searchEmail;
            if (string.IsNullOrEmpty(searchAddress))
                la = "all";
            else
                la = searchAddress;
            customers = await httpClient.GetFromJsonAsync<List<Cust>>($"http://localhost:5038/list_cust/{searchID}/{ln}/{lp}/{le}/{la}");
        }

        private async Task DeleteCustomer(ulong id)
        {
            Console.WriteLine($"http://localhost:5038/del_cust/{id}");
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost:5038/del_cust/{id}");
            await ListCust();
        }
        private void GetUserContext()
        {
            string filename = "C:/C#.NET/InventoryManagementApp/BlazorApp/Data/UserStatus.json";
            string jsonstring = File.ReadAllText(filename);
            user_context = JsonSerializer.Deserialize<UserContext>(jsonstring);
        }
    }
}