using Microsoft.JSInterop;
using System.Threading.Tasks;

public interface IAlertService
{
    Task ShowAlert(string message);
}

public class AlertService : IAlertService
{
    private readonly IJSRuntime _jsRuntime;

    public AlertService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task ShowAlert(string message)
    {
        await _jsRuntime.InvokeVoidAsync("alert", message);
    }
}
