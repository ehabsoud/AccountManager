using AccountManager.Models;
using Microsoft.JSInterop;

namespace AccountManager.Services;

public class LocalStorageService : IStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveAccounts(List<BankAccount> accounts)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(accounts);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "accounts", json);
    }

    public async Task<List<BankAccount>> LoadAccounts()
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "accounts");
        if (string.IsNullOrWhiteSpace(json))
        {
            Console.WriteLine("No accounts found");
            return new List<BankAccount>();
        }
        return System.Text.Json.JsonSerializer.Deserialize<List<BankAccount>>(json);
    }
}