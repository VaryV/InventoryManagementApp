using BlazorApp.Data;
using System.Text.Json;

namespace BlazorApp.Components.Pages
{
    public partial class AddTodayInventoryItem
    {
        private ulong id;
        private int Sqty;
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
            NavigationManager.NavigateTo("/inflow");
        }

        private async Task AddNewItem()
        {
            if (id <= 0 || Sqty <= 0)
            {
                await alert.ShowAlert("Fields cannot be empty.");
            }
            else
            {
                if (id > 0 && Sqty > 0)
                {
                    ProductInflow cust = new ProductInflow();
                    cust.Id = id;
                    cust.qty = Sqty;
                    Console.WriteLine(cust.Id + " " + cust.qty);
                    string res = "";
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync<string>($"http://localhost:5038/inflow/{id}/{Sqty}", res);
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        await alert.ShowAlert("Invalid Product ID.");
                    else
                    {
                        await alert.ShowAlert("Created");
                        NavigateToRoute();
                    }
                }
            }
        }
    }
}