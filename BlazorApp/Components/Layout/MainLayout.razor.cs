using BlazorApp.Data;
using System.Text.Json;

namespace BlazorApp.Components.Layout
{
    public partial class MainLayout
    {
        protected override void OnInitialized()
        {
            GetUserContext();
        }
        
        private void GetUserContext()
        {
            string filename = "C:/C#.NET/BlazorApp/Data/UserStatus.json";
            string jsonstring = File.ReadAllText(filename);
            UserContext user_context = new UserContext();
            user_context = JsonSerializer.Deserialize<UserContext>(jsonstring);
        }
    }
}
