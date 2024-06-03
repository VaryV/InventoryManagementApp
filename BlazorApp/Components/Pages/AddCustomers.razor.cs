using BlazorApp.Data;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace BlazorApp.Components.Pages
{
    public partial class AddCustomers
    {
        private string? name;
        private string? phone;
        private string? mail;
        private string? address;
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
            NavigationManager.NavigateTo("/customers");
        }

        private async Task AddCust()
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(address))
            {
                await alert.ShowAlert($"Fields cannot be empty.");
            }
            else
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(address))
                {
                    Cust cust = new Cust();
                    cust.Id = 1;
                    cust.name = name;
                    cust.phone = phone;
                    cust.mail = mail;
                    cust.address = address;
                    Console.WriteLine(cust.name + cust.phone);
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync("http://localhost:5038/add_cust", cust);
                    Console.WriteLine(response);
                    await alert.ShowAlert($"{response.StatusCode}");
                    NavigateToRoute();
                }
            }
        }
    }
}