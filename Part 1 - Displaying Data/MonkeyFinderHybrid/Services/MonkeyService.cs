using System.Net.Http.Json;

namespace MonkeyFinderHybrid.Services;

public class MonkeyService
{
    private List<Monkey> monkeysList = new();

    private readonly HttpClient httpClient;

    public MonkeyService()
    {
        httpClient = new HttpClient();
    }


    public async Task<List<Monkey>> GetMonkeysAsync()
    {
        if (monkeysList.Count > 0)
            return monkeysList;


        var response = await httpClient.GetAsync("https://montemagno.com/monkeys.json/");

        if (response.IsSuccessStatusCode)
        {
            var monkeysResult = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);

            if (monkeysResult != null)
                monkeysList = monkeysResult;
        }

        return monkeysList;
    }
}