using BlazorApp.Data;
using System.Net.Http;
using System.Text.Json;


namespace BlazorApp.Components.Pages
{
    public partial class Inventory
    {
        private ulong searchID = 0;
        private string? searchName;
        private Product[]? products;
        private void NavigateToRoute()
        {
            NavigationManager.NavigateTo("/add-new-inventory-item");
        }
        protected override async Task OnInitializedAsync()
        {
            GetUserContext();
            await ListProd();
        }

        private async Task ListProd()
        {
            string name;
            if (string.IsNullOrEmpty(searchName))
                name = "all";
            else
                name = searchName;
            products = await httpClient.GetFromJsonAsync<Product[]>($"http://localhost:5038/list_prods/{searchID}/{name}");
        }

        private async Task DeleteProduct(ulong id)
        {
            Console.WriteLine($"http://localhost:5038/del_prod/{id}");
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost:5038/del_prod/{id}");
            await ListProd();
        }

        private void GetUserContext()
        {
            string filename = "C:/C#.NET/InventoryManagementApp/BlazorApp/Data/UserStatus.json";
            string jsonstring = File.ReadAllText(filename);
            user_context = JsonSerializer.Deserialize<UserContext>(jsonstring);
        }
    }
}