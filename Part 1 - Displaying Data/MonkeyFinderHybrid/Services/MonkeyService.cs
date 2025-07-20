using System.Net.Http.Json;

namespace MonkeyFinderHybrid.Services;

public class MonkeyService
{
    private readonly List<Monkey> monkeysList = new List<Monkey>();

    private readonly HttpClient httpClient;

    public MonkeyService()
    {
        httpClient = new HttpClient(); // Could use HttpClientFactory if needed
    }

    public async Task<List<Monkey>> GetMonkeys()
    {
        if (monkeysList.Count > 0)
            return monkeysList; // Return cached list if already populated

        var response = await httpClient.GetAsync("https://montemagno.com/monkeys.json");

        if (response.IsSuccessStatusCode)
        {
            var monkeyResult = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);

            if (monkeyResult is not null)
            {
                monkeysList.AddRange(monkeyResult);
            }
        }

        return monkeysList;
    }
}
