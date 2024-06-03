using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components.Pages
{
    public partial class Payment
    {
        private void NavigateToRoute()
        {
            // Use InvokeAsync to ensure the navigation happens on the correct context
            NavigationManager.NavigateTo("/generate_invoice");
        }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(3000);
            NavigateToRoute();
        }
    }
}
