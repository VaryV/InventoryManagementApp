using BlazorApp.Data;
using System.Text.Json;

namespace BlazorApp.Components.Pages
{
    public partial class Home
    {
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
    }
}
