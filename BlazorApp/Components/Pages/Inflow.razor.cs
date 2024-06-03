using Microsoft.AspNetCore.Components;
using BlazorApp.Data;
using System.Text.Json;

namespace BlazorApp.Components.Pages
{
    public partial class Inflow
    {
        private string? searchDate, searchName;
        private ulong searchID;
        private Record[]? records;
        private void GetUserContext()
        {
            string filename = "C:/C#.NET/InventoryManagementApp/BlazorApp/Data/UserStatus.json";
            string jsonstring = File.ReadAllText(filename);
            user_context = JsonSerializer.Deserialize<UserContext>(jsonstring);
        }
        private void NavigateToRoute()
        {
            NavigationManager.NavigateTo("/add-today-inventory-item");
        }
        protected override async Task OnInitializedAsync()
        {
            GetUserContext();
            await ListProd();
        }

        private async Task ListProd()
        {
            string ln, ld;
            if (string.IsNullOrEmpty(searchName))
                ln = "all";
            else
                ln = searchName;
            if (string.IsNullOrEmpty(searchDate))
                ld = "all";
            else
                ld = searchDate;
            records = await httpClient.GetFromJsonAsync<Record[]>($"http://localhost:5038/list_records/{searchID}/{ln}/{ld}");
        }
    }
}