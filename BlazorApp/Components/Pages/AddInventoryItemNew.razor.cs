using Microsoft.AspNetCore.Components;
using BlazorApp.Data;
using System.Text.Json;

namespace BlazorApp.Components.Pages
{
    public partial class AddInventoryItemNew
    {
        private string? name;
        private string? desc;
        private Decimal UnitPrice;
        private int Sqty;
        private Decimal disc;
        protected override void OnInitialized()
        {
            GetUserContext();
        }
        private void GetUserContext()
        {
            string filename = "C:/C#.NET/InventoryManagementApp/BlazorApp/Data/UserStatus.json";
            string jsonstring = File.ReadAllText(filename);
            user_context = JsonSerializer.Deserialize<UserContext>(jsonstring);
        }
        private void NavigateToRoute()
        {
            NavigationManager.NavigateTo("/inventory");
        }

        private async Task AddNewItem()
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(desc) || UnitPrice <= 0 || Sqty <= 0 || disc.Equals(null))
            {
                await alert.ShowAlert("Fields cannot be empty");
            }
            else
            {
                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(desc) || UnitPrice > 0 || Sqty > 0 || !disc.Equals(null))
                {
                    Product cust = new Product();
                    cust.Id = 1;
                    cust.name = name;
                    cust.desc = desc;
                    cust.unitprice = UnitPrice;
                    cust.qty = Sqty;
                    cust.discount = disc;
                    Console.WriteLine(cust.name + cust.qty);
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync("http://localhost:5038/add_prod", cust);
                    Console.WriteLine(response);
                    await alert.ShowAlert($"{response.StatusCode}");
                    NavigateToRoute();
                }
            }
        }
    }
}