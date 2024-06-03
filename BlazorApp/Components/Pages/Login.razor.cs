using BlazorApp.Data;
using System.Reflection.Metadata;
using System.Text.Json;

namespace BlazorApp.Components.Pages
{
    public partial class Login
    {
        private string? UserName, Password;

        protected override void OnInitialized()
        {
            SetUserContext("i");
            Console.WriteLine($"Start {user_context.UserName} : {user_context.IsAdmin}");
        }

        private void SetUserContext(string state)
        {
            string filename = "C:/C#.NET/InventoryManagementApp/BlazorApp/Data/UserStatus.json";
            string jsonstring = JsonSerializer.Serialize(user_context);
            if (state.Equals("i")) {
                user_context = new UserContext();
                jsonstring = JsonSerializer.Serialize(user_context);
            }
            File.WriteAllText(filename, jsonstring);
        }
        private async void login()
        {
            if (user_context != null && !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync($"http://localhost:5038/login", new {usn = UserName, pwd = Password});
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await alert.ShowAlert($"{response.StatusCode}");
                }
                else
                {
                    var acuser = await response.Content.ReadFromJsonAsync<UserContext>();
                    user_context = new UserContext(acuser.UserName, acuser.IsAdmin);
                    SetUserContext("f");
                    Console.WriteLine($"Final {user_context.UserName} : {user_context.IsAdmin}");
                    NavigationManager.NavigateTo("/");
                }
            }
        }
    }
}